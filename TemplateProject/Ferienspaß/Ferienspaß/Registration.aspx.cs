using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ferienspaß {
    public partial class Registration : System.Web.UI.Page {

        private string RegisteredUID {
            get {
                if (Session["regUID"] == null) return "";
                else return Session["regUID"].ToString();
            }
            set {
                Session["regUID"] = value;
            } 
        }

        protected void Page_Load(object sender, EventArgs e) {
             btn_register.Text = "Registration abschließen";
            
        }

       


        protected void btn_login_Click(object sender, EventArgs e) {

        }



        public string ConfirmationHash(string firstname, string email, string uId) {
            string toEncode = DateTime.Now + firstname + email + uId;
            return Crypt.GenerateSHA256String(toEncode);
        }

        protected void btn_register_Click(object sender, EventArgs e) {

            if (!String.IsNullOrEmpty(tbx_firstname.Text) && !String.IsNullOrEmpty(tbx_lastname.Text) && !String.IsNullOrEmpty(tbx_email.Text) && !String.IsNullOrEmpty(tbx_phone.Text)) {
                    
                CsharpDB db = new CsharpDB();
                DataTable dt = db.Query("SELECT UID FROM user WHERE EMAIL=?;", tbx_email.Text);
                if (dt.Rows.Count>0) {
                    lit_msg.Text = CreateMSGString("Diese E-Mail Adresse ist bereits in Verwendung!", "warning");
                    return;
                }

                if (String.Compare(tbx_pwd1.Text, tbx_pwd2.Text) == 0) {


                    if (CsharpDB.CheckPasswordRequirements(tbx_pwd1.Text)) {
                        string salt = Crypt.CreateSalt();
                        string hashedPWD = Crypt.GenerateSHA512String(tbx_pwd1.Text + salt);

                           

                        if (db.ExecuteNonQuery("INSERT INTO user (GN,SN,PHONE,EMAIL,ENCODEDPASS,SALT,UGID,LOCKED) VALUES (?,?,?,?,?,?,?,?);",tbx_firstname.Text,tbx_lastname.Text,tbx_phone.Text,tbx_email.Text, hashedPWD, salt,2,0) > 0) {
                            DataTable dt1 = db.Query("SELECT * FROM user WHERE EMAIL=?;", tbx_email.Text);
                            if (dt1.Rows.Count>0) {
                                RegisteredUID = dt1.Rows[0]["UID"].ToString();

                                FormsAuthentication.RedirectFromLoginPage(RegisteredUID,false);
                                //db.SendMail(tbx_email.Text, $"{tbx_firstname.Text} {tbx_lastname.Text}", "Registrierung Ferienspaß", "Vielen Dank für Ihre Registrierung bei Ferienspaß");

                                try {

                                    DataRow user = dt1.Rows[0];
                                    string createhash = ConfirmationHash(user["GN"].ToString(), user["EMAIL"].ToString(), user["UID"].ToString());

                                    if (db.ExecuteNonQuery("UPDATE user SET EmailConfirmationHash=? WHERE UID=?;", createhash, user["UID"].ToString()) > 0) {
                                        //Erfolg
                                        string url = "http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + "/Login.aspx?hash=" + createhash;
                                        bool sendHtmlEmail = db.SendHTMLEmail(user["EMAIL"].ToString(), user["GN"].ToString() + " " + user["SN"].ToString(), db.GetPortalOption("MAIL_EMAIL_CONFIRMATION_SUBJECT"), db.GetPortalOption("MAIL_EMAIL_CONFIRMATION_BODY"), true, db.GetPortalOption("MAIL_EMAIL_CONFIRMATION_BTN_TEXT"), url, "", db.GetPortalOption("MAIL_GRUSSFORMEL"), db.GetPortalOption("MAIL_HINWEIS"));
                                        lit_msg.Text = CreateMSGString("Bestätigungsmail wurde gesendet!","success");
                                    } else {
                                        //Gescheitert
                                        lit_msg.Text = CreateMSGString("Der Vorgang ist gescheitert!","danger");
                                    }
                                } catch (Exception ex) {
                                    lit_msg.Text = CreateMSGString("Der Vorgang ist gescheitert! Wenden Sie sich an einen Administrator.","danger");
                                } finally {
                                    Response.Redirect("RegistrationChildren.aspx?fromRegistration=1");
                                    lit_msg.Text = CreateMSGString("Sie können zu Ihrem Konto jetzt Ihre Kinder anlegen!", "info");
                                }
                                
                            } else {
                                lit_msg.Text = CreateMSGString("Der Vorgang ist gescheitert!", "warning");
                            }


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
                lit_msg.Text = CreateMSGString("Alle Felder müssen ausgefüllt werden!","danger");
            }
        }


        private string CreateMSGString(string msg,string type) {
           return "<div class=\"alert alert-"+type+" mt-3 mb-1\" role=\"alert\">" + msg + "</div>";
        }

    }
}