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

        private int SessionLoginAttempt {
            get { return Convert.ToInt32(Session["loginAttemptCount"]); }
            set { Session["loginAttemptCount"] = value; }
        }

        private bool PwdVisible {
            get { return Convert.ToBoolean(Session["pwdVisible"]); }
            set { Session["pwdVisible"] = value; }
        }

        private string LoginUser {
            get {
                return Session["userId"].ToString(); 
            }
            set { Session["userId"] = value; }
        }



        public string ConfirmationHash(string firstname, string email, string uId) {
            string toEncode = DateTime.Now + firstname + email + uId ;
            return Crypt.GenerateSHA256String(toEncode);
        }


        protected void Page_Load(object sender, EventArgs e) {
            db = new CsharpDB();
            try {
                MaxLoginAttempts = Convert.ToInt32(db.GetPortalOption("MAX_LOGIN_ATTEMPTS"));
            } catch (Exception ex) { lit_msg.Text = CreateMSGString("Ein interner Fehler ist aufgetreten! " + Environment.NewLine + ex, 0); }

            if (Session["registrationMSG"] != null) {
                lit_msg.Text = CreateSuccessMSGString(Session["registrationMSG"].ToString());
                Session["registrationMSG"] = null;
            }


            if (!Page.IsPostBack) {
                Session["loginAttemptCount"] = 0;
                LoginUser = "";


                if (Session["confirmed"] == null) {
                    //  Email Bestätigen LINK
                    string hash = Request.QueryString["hash"];
                    if (!String.IsNullOrEmpty(hash)) {

                        if (db.ExecuteNonQuery("UPDATE user SET EmailConfirmed=1,EmailConfirmationHash=? WHERE EmailConfirmationHash=?;", "", hash) > 0) {
                            Session["confirmed"] = true;
                            lit_msg.Text = CreateSuccessMSGString("Erfolgreich bestätigt! <br/>Sie können sich jetzt anmelden.");
                        } else lit_msg.Text = CreateMSGString("Der Vorgang ist gescheitert! Wenden Sie sich an einen Administrator.", -1);
                    }
                }


                if (Request["sendEmailConfirmation"] != null) {

                    if(Session["sent"] != null) {
                        lit_msg.Text = CreateMSGString("Email wurde bereits gesendet!", -1);
                    } else {
                        try {

                            DataRow user = (DataRow)Session["user"];
                            string createhash = ConfirmationHash(user["GN"].ToString(), user["EMAIL"].ToString(), user["UID"].ToString());

                            if (db.ExecuteNonQuery("UPDATE user SET EmailConfirmationHash=? WHERE UID=?;", createhash, user["UID"].ToString()) > 0) {
                                //Erfolg
                                CsharpDB db = new CsharpDB();
                                string url = "http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + "/Login.aspx?hash=" + createhash;
                                bool sendHtmlEmail = db.SendHTMLEmail(user["EMAIL"].ToString(), user["GN"].ToString() + " " + user["SN"].ToString(), db.GetPortalOption("MAIL_EMAIL_CONFIRMATION_SUBJECT"), db.GetPortalOption("MAIL_EMAIL_CONFIRMATION_BODY"), true, db.GetPortalOption("MAIL_EMAIL_CONFIRMATION_BTN_TEXT"), url, "", db.GetPortalOption("MAIL_GRUSSFORMEL"), db.GetPortalOption("MAIL_HINWEIS"));
                                lit_msg.Text = CreateSuccessMSGString("Bestätigungsmail wurde gesendet!");
                                Session["sent"] = true;
                            } else {
                                //Gescheitert
                                lit_msg.Text = CreateMSGString("Der Vorgang ist gescheitert!", -1);
                            }
                        } catch (Exception ex) {
                            lit_msg.Text = CreateMSGString("Der Vorgang ist gescheitert! Wenden Sie sich an einen Administrator.", -1);
                        }
                    }
                }
            }


           



            //FormsAuthentication.RedirectFromLoginPage("1", false);


            if (SessionLoginAttempt+1>=MaxLoginAttempts) {
                btn_login.Enabled = false;
                grp_user.Visible = false;
                btn_login.Visible = false;
                grp_pwd.Visible = false;

                lit_msg.Text = CreateMSGString("Anzahl der Anmeldeversuche wurde überschritten!<br/> <strong>Ihr Benutzer wird aus Sicherheitsgründen gesperrt!</strong><br/>Wenden Sie sich an einen Administrator!",-1);
                LockAccount(LoginUser);
            } 


            //grp_pwd.Visible = PwdVisible;
            
        }

        protected void Login_Click(object sender, EventArgs e) {
            //FormsAuthentication.RedirectFromLoginPage("2", false);
            //Response.Redirect("projects");

            if (SessionLoginAttempt+1 >= MaxLoginAttempts) return;

            if(String.IsNullOrEmpty(tbx_pass.Text) && !String.IsNullOrEmpty(tbx_user.Text)) {
                DataTable u = db.Query("SELECT UID FROM user WHERE EMAIL LIKE ? LIMIT 1", tbx_user.Text);
                if (u.Rows.Count == 0) LoginUser = "-1";
                else {
                    LoginUser = u.Rows[0].ItemArray[0].ToString();
                }

                PwdVisible = true;
                grp_pwd.Visible = true;
                tbx_user.CssClass = "form-control border-success";
                tbx_user.Enabled = false;
                span_iconMail.Attributes["class"] = "input-group-text border-success";

                return;

            }else if (!String.IsNullOrEmpty(LoginUser) && !String.IsNullOrEmpty(tbx_pass.Text)) {
                try {
                    db = new CsharpDB();
                    DataTable user = db.Query("SELECT * FROM user WHERE UID = ?;", LoginUser);


                    if (user.Rows.Count == 0) {
                        SessionLoginAttempt++;
                        lit_msg.Text = CreateMSGString("Benutzer und/oder Passwort ungültig!", (MaxLoginAttempts - SessionLoginAttempt));
                        throw new ApplicationException("Benutzer nicht vorhanden!");
                    }

                    if (user.Rows[0]["LOCKED"] != DBNull.Value) {
                        if(Convert.ToInt32(user.Rows[0]["LOCKED"])==1) {
                            lit_msg.Text = CreateMSGString("<strong>Benutzer gesperrt!<br/></strong> Wenden Sie sich an einen Administrator!", -1);
                            throw new ApplicationException("Benutzer gesperrt!");
                        }
                    }


                    int usertype = Convert.ToInt32(user.Rows[0]["UGID"]);
                    
                    string salt = user.Rows[0]["SALT"].ToString();
                    string hashpw = Crypt.GenerateSHA512String(tbx_pass.Text + salt.ToString());

                    string dbPass = user.Rows[0]["ENCODEDPASS"].ToString();
                    if (String.Compare(hashpw, dbPass) == 0) {

                        if (Convert.ToInt32(user.Rows[0]["EmailConfirmed"]) == 0) {
                            lit_msg.Text = CreateMSGString("<strong>E-Mail Adresse noch nicht bestätigt!</strong><br><a href='?sendEmailConfirmation=1'>Bestätigunsemail nochmals anfordern</a>", -1);
                            Session["user"] = user.Rows[0];
                            return;
                        }


                        if(!CsharpDB.IsUserAllowedToLogin()) {
                            DateTime[] dates = CsharpDB.GetPortalLoginDates();
                            lit_msg.Text = CreateMSGString("<strong>Ferienspaß nicht verfügbar!</strong><br>Ferienspaß ist wieder verfügbar von:<br> "+dates[0].ToLocalTime()+" bis "+dates[1].ToLocalTime(),-1);
                            return;
                        }


                        SessionLoginAttempt = 0;

                        switch (usertype) {

                            case 0:
                                //MANAGEMENT
                                Session["usergroup"] = 0;
                                FormsAuthentication.RedirectFromLoginPage(user.Rows[0]["UID"].ToString(), false);
                                Response.Redirect("Pages/Admin_Project_View.aspx");
                                break;

                            case 1:
                                //INVOLVED USER
                                Session["usergroup"] = 1;
                                FormsAuthentication.RedirectFromLoginPage(user.Rows[0]["UID"].ToString(), false);
                                Response.Redirect("Pages/Admin_Project_View.aspx");
                                break;

                            case 3:
                                //PORTIER
                                Session["usergroup"] = 3;
                                FormsAuthentication.RedirectFromLoginPage(user.Rows[0]["UID"].ToString(), false);
                                Response.Redirect("Pages/Portier_Cancellations_View.aspx");
                                break;


                            case 4:
                                //SECRETARY
                                Session["usergroup"] = 4;
                                FormsAuthentication.RedirectFromLoginPage(user.Rows[0]["UID"].ToString(), false);
                                Response.Redirect("Pages/Portier_Cancellations_View.aspx");
                                break;

                            default:
                                // NORMALER  BENUTZER
                                Session["usergroup"] = 2;
                                FormsAuthentication.RedirectFromLoginPage(user.Rows[0]["UID"].ToString(), false);
                                Response.Redirect("Pages/User_Project_View.aspx");
                                break;
                        }

                        Session["usertype"] = usertype;

                    } else {
                        if (SessionLoginAttempt + 1 >= MaxLoginAttempts) {
                            LockAccount(LoginUser);
                            lit_msg.Text = CreateMSGString("<strong>Erlaubte Anmeldeversuche überschritten! Benutzer gesperrt!<br/></strong> Wenden Sie sich an einen Administrator!", -1);
                        } else {
                            SessionLoginAttempt++;
                            lit_msg.Text = CreateMSGString("Benutzer und/oder Passwort ungültig!", (MaxLoginAttempts - SessionLoginAttempt));
                            throw new ApplicationException("Passwort stimmt nicht überien");
                        }
                    }

                } catch (Exception ex) {
                    Console.WriteLine(ex);
                };

            } else {
                lit_msg.Text = CreateMSGString("Alle Felder müssen ausgefüllt werden!", -1);
            }
        }

        private string CreateSuccessMSGString(string msg) {
            return "<div class=\"alert alert-success mt-3 mb-1\" role=\"alert\">" + msg + "</div>";
        }
        private string CreateMSGString(string msg, int loginAttemptleft) {
            if (loginAttemptleft != -1) return "<div class=\"alert alert-danger mt-3 mb-1\" role=\"alert\">" + msg + "<br/><strong>" + loginAttemptleft + " Anmeldeversuche Verbleibend</strong></div>";
            return "<div class=\"alert alert-danger mt-3 mb-1\" role=\"alert\">" + msg + "</div>";
        }

        private void LockAccount(string userid) {
            if(userid!="-1") db.ExecuteNonQuery("UPDATE user SET LOCKED=? WHERE UID=?;", 1, userid);
        }

        protected void btn_pwdforget_Click(object sender, EventArgs e) {
            Response.Redirect("forgotPassword.aspx");
        }
    }
}