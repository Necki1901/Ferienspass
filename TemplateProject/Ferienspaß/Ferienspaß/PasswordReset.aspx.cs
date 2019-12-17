
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ferienspaß {
    public partial class PasswordReset : Page {

       
        private CsharpDB db;

        private string PwdResetUserID { get; set; }

        protected void Page_Load(object sender, EventArgs e) {
            db = new CsharpDB();
           
            if (!Page.IsPostBack) {

            }


            string hash = Request.QueryString["hash"];
            if (!String.IsNullOrEmpty(hash)) {


                DataTable dt = db.Query("SELECT OnUID,ExpireAt FROM pwdrecovery WHERE RecoveryHash=? ORDER BY `ExpireAt` DESC LIMIT 1;", hash);
                if(dt.Rows.Count>0) {

                    DateTime expire = DateTime.Now;
                    try {
                        expire = DateTime.Parse(dt.Rows[0]["ExpireAt"].ToString());
                    }catch(Exception ex) {
                        // WENN DATENFORMAT NICHT KORREKT
                    }
                     

                    if (expire>DateTime.Now) {
                        PwdResetUserID = dt.Rows[0]["OnUID"].ToString();
                        grp_user.Visible = false;
                        grp_pwd1.Visible = true;
                        grp_pwd2.Visible = true;
                    } else db.ExecuteNonQuery("DELETE FROM pwdrecovery WHERE RecoveryHash=?", hash);
                }

                
            }
        }

        public string PwdResetHash(string firstname, string email, string uId) {
            string toEncode = firstname + email + uId + DateTime.Now;
            return Crypt.GenerateSHA256String(toEncode);
        }



        protected void Reset_Click(object sender, EventArgs e) {


            if (!String.IsNullOrEmpty(tbx_user.Text) && String.IsNullOrEmpty(PwdResetUserID)) {

                DataTable u = db.Query("SELECT UID,GN,EMAIL,SN FROM user WHERE EMAIL LIKE ? LIMIT 1", tbx_user.Text);
                if (u.Rows.Count > 0) {

                    string resetHash = PwdResetHash(u.Rows[0]["GN"].ToString(), u.Rows[0]["EMAIL"].ToString(), u.Rows[0]["UID"].ToString());
                    if (db.ExecuteNonQuery("INSERT INTO pwdrecovery SET OnUID=?,RecoveryHash=?,ExpireAt=?;",u.Rows[0]["UID"].ToString(),resetHash,DateTime.Now.AddMinutes(5)) > 0) {
                        //Erfolg
                        CsharpDB db = new CsharpDB();
                        bool sentEmail = db.SendMail(u.Rows[0]["EMAIL"].ToString(), u.Rows[0]["GN"].ToString() + " " + u.Rows[0]["SN"].ToString(), "Passwort zurücksetzen", "http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + "/PasswordReset.aspx?hash=" + resetHash);
                        grp_user.Visible = false;
                        btn_login.Visible = false;
                        lit_msg.Text = CreateMSGString("Eine E-Mail mit dem Link zum erstellen eines neuen Passworts wurde an Ihre hinterlegte E-Mail Adresse gesendet! <strong>Achtung, dieser Link ist nur 5 Minuten gültig!</strong>", "success");
                    } else {
                        //Gescheitert
                        lit_msg.Text = CreateMSGString("Der Vorgang ist gescheitert!", "warning");
                    }

                } else {
                    // Kein Benutzer mit dieser Email
                    grp_user.Visible = false;
                    btn_login.Visible = false;
                    lit_msg.Text = CreateMSGString("Eine E-Mail mit dem Link zum erstellen eines neuen Passworts wurde an Ihre hinterlegte E-Mail Adresse gesendet! <strong>Achtung, dieser Link ist nur 5 Minuten gültig!</strong>", "success");
                }


            } else if (!String.IsNullOrEmpty(PwdResetUserID)) {

                if(!String.IsNullOrEmpty(tbx_pwd1.Text) && !String.IsNullOrEmpty(tbx_pwd2.Text)) {

                    if(String.Compare(tbx_pwd1.Text,tbx_pwd2.Text)==0) {


                        if(CsharpDB.CheckPasswordRequirements(tbx_pwd1.Text)) {
                            string salt = Crypt.CreateSalt();
                            string hashedPWD = Crypt.GenerateSHA512String(tbx_pwd1.Text + salt);

                            if (db.ExecuteNonQuery("UPDATE user SET ENCODEDPASS=?,SALT=? WHERE UID=?;", hashedPWD, salt, PwdResetUserID) > 0) {

                                grp_pwd1.Visible = false;
                                grp_pwd2.Visible = false;

                                btn_login.Visible = false;

                                db.ExecuteNonQuery("DELETE FROM pwdrecovery WHERE OnUID=?", PwdResetUserID);

                                DataTable u = db.Query("SELECT GN,EMAIL,SN FROM user WHERE UID LIKE ? LIMIT 1", PwdResetUserID);
                                if (u.Rows.Count > 0) {
                                    bool sentEmail = db.SendMail(u.Rows[0]["EMAIL"].ToString(), u.Rows[0]["GN"].ToString() + " " + u.Rows[0]["SN"].ToString(), "Passwort wurde geändert!", "Ihr Passwort wurde soeben geändert!");
                                }

                                lit_msg.Text = CreateMSGString("Das Passwort wurde erfolgreich geändert!", "success");

                            } else {
                                //Gescheitert
                                lit_msg.Text = CreateMSGString("Der Vorgang ist gescheitert!", "warning");
                            }
                        } else {
                            lit_msg.Text = CreateMSGString("Passwort entspricht nicht den obigen Anforderungen!", "warning");
                        }
                        
                    } else {
                        //Passwörter stimmen nicht übere8in
                        lit_msg.Text = CreateMSGString("Passwörter stimmen nicht überein!", "warning");
                    }
                } else {
                    lit_msg.Text = CreateMSGString("Alle Felder müssen ausgefüllt werden!", "danger");
                }
            } else {
                lit_msg.Text = CreateMSGString("Alle Felder müssen ausgefüllt werden!", "danger");
            }
        }


        private string CreateMSGString(string msg, string type) {
            return "<div class=\"alert alert-"+type+" mt-3 mb-1\" role=\"alert\">" + msg + "</div>";
        }
    }
}