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
                            DataTable dt1 = db.Query("SELECT UID FROM user WHERE EMAIL=?;", tbx_email.Text);
                            if (dt1.Rows.Count>0) {
                                RegisteredUID = dt1.Rows[0]["UID"].ToString();

                                FormsAuthentication.RedirectFromLoginPage(RegisteredUID,false);
                                db.SendMail(tbx_email.Text, $"{tbx_firstname.Text} {tbx_lastname.Text}", "Registrierung Ferienspaß", "Vielen Dank für Ihre Registrierung bei Ferienspaß");

                                Response.Redirect("RegistrationChildren.aspx?fromRegistration=1");
                                lit_msg.Text = CreateMSGString("Sie können zu Ihrem Konto jetzt Ihre Kinder anlegen!", "info");
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