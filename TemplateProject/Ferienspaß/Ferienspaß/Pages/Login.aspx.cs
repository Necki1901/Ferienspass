using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ferienspaß {
    public partial class Login : Page {

        private int MaxLoginAttempts { get; set; }
        private CsharpDB db;

        protected void Page_Load(object sender, EventArgs e) {
            db = new CsharpDB();
            try {
                MaxLoginAttempts = Convert.ToInt32(db.GetPortalOption("MAX_LOGIN_ATTEMPTS"));
            } catch (Exception ex) { lit_msg.Text = "Ein interner Fehler ist aufgetreten! " + Environment.NewLine + ex; }

        }

        protected void Login_Click(object sender, EventArgs e) {
            //FormsAuthentication.RedirectFromLoginPage("2", false);
            //Response.Redirect("projects");


            if (!String.IsNullOrEmpty(tbx_user.Text) && !String.IsNullOrEmpty(tbx_pass.Text)) {
                try {
                    db = new CsharpDB();
                    DataTable user = db.Query("SELECT salt,EncodedPassword,userid,NumInvalidLogins,locked,usertyp FROM portalusers WHERE username LIKE ? OR  email LIKE ? LIMIT 1;", tbx_user.Text, tbx_user.Text);

                    int logincnt;
                    if (user.Rows[0]["NumInvalidLogins"] is DBNull) {
                        logincnt = 0;
                    } else {
                        logincnt = Convert.ToInt32(user.Rows[0]["NumInvalidLogins"]);
                    }

                    int locked;
                    if (user.Rows[0]["locked"] is DBNull) {
                        locked = 0;
                    } else {
                        locked = Convert.ToInt32(user.Rows[0]["locked"]);
                    }

                    int usertype = Convert.ToInt32(user.Rows[0]["usertyp"]);

                    if (locked != 1) {
                        if (logincnt >= MaxLoginAttempts) {
                            LockAccount(user.Rows[0]["userid"].ToString());
                            lit_msg.Text = CreateMSGString("Anzahl der Anmeldeversuche wurde überschritten! Wenden Sie sich an einen Administrator!", -1);
                            throw new ApplicationException("Anzahl der Anmeldeversuche wurde überschritten! Wenden Sie sich an einen Administrator!");
                        } else {
                            if (user.Rows.Count > 0) {
                                string salt = user.Rows[0]["salt"].ToString();
                                string hashpw = Crypt.GenerateSHA512String(tbx_pass.Text + salt.ToString());

                                string dbPass = user.Rows[0]["EncodedPassword"].ToString();
                                if (String.Compare(hashpw, dbPass) == 0) {

                                    SetLoginCnt(user.Rows[0]["userid"].ToString(), 0, true);

                                    switch (usertype) {
                                        case 1:
                                            FormsAuthentication.RedirectFromLoginPage(user.Rows[0]["userid"].ToString(), false);
                                            Response.Redirect("firmenregister.aspx");
                                            break;
                                        default:

                                            FormsAuthentication.RedirectFromLoginPage(user.Rows[0]["userid"].ToString(), false);
                                            Response.Redirect("firmenDaten.aspx");
                                            break;
                                    }

                                    Session["usertype"] = usertype;



                                } else {
                                    if (logincnt + 1 >= MaxLoginAttempts) {
                                        LockAccount(user.Rows[0]["userid"].ToString());
                                        lit_msg.Text = CreateMSGString("<strong>Benutzer gesperrt!</strong> Wenden Sie sich an einen Administrator!", -1);
                                    } else {
                                        SetLoginCnt(user.Rows[0]["userid"].ToString(), logincnt + 1, false);
                                        lit_msg.Text = CreateMSGString("Benutzer und/oder Passwort ungültig!", (MaxLoginAttempts - logincnt - 1));
                                        throw new ApplicationException("Passwort stimmt nicht überien");
                                    }
                                }
                            } else lit_msg.Text = CreateMSGString("Benutzer und/oder Passwort ungültig!", -1); throw new ApplicationException("Kein Benutzer gefunden");
                        }
                    } else lit_msg.Text = CreateMSGString("<strong>Benutzer gesperrt!</strong> Wenden Sie sich an einen Administrator!", -1); throw new ApplicationException("Benutzer gesperrt!");

                } catch (Exception ex) { Console.WriteLine(ex); };
            } else lit_msg.Text = CreateMSGString("Alle Felder müssen ausgefüllt werden!", -1);
        }

        private void SetLoginCnt(string userid, int cnt, bool successfulLogin) {
            if (!successfulLogin) db.ExecuteNonQuery("UPDATE portalusers SET NumInvalidLogins=? WHERE userId=?;", cnt, userid);
            else db.ExecuteNonQuery("UPDATE portalusers SET NumInvalidLogins=?,PasswordSent=? WHERE userId=?;", cnt, 0, userid);
        }

        private string CreateMSGString(string msg, int loginAttemptleft) {
            if (loginAttemptleft != -1) return "<div class=\"alert alert-danger mt-3 mb-1\" role=\"alert\">" + msg + "<br/><strong>" + loginAttemptleft + " Anmeldeversuche Verbleibend</strong></div>";
            return "<div class=\"alert alert-danger mt-3 mb-1\" role=\"alert\">" + msg + "</div>";
        }

        private void LockAccount(string userid) {
            db.ExecuteNonQuery("UPDATE portalusers SET locked=? WHERE userId=?;", 1, userid);
        }

        protected void btn_pwdforget_Click(object sender, EventArgs e) {
            Response.Redirect("forgotPassword.aspx");
        }
    }
}