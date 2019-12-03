using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
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

            if(String.IsNullOrEmpty(RegisteredUID)) {
                div_userdata.Visible = true;
                div_children.Visible = true;
                btn_register.Text = "Weiter...";
            } else {
                div_userdata.Visible = false;
                div_children.Visible = true;
                btn_register.Text = "Registration abschließen";
                Fill_gvChildren(RegisteredUID);
            }
        }




        protected void gvChildren_RowEditing(object sender, GridViewEditEventArgs e) {
            gvChildren.EditIndex = e.NewEditIndex;
            Fill_gvChildren(RegisteredUID);
        }

        protected void gvChildren_RowUpdating(object sender, GridViewUpdateEventArgs e) {
            CsharpDB db = new CsharpDB();

            if(!String.IsNullOrEmpty(RegisteredUID)) {
                TextBox tbx_birth = (TextBox)gvChildren.Rows[e.RowIndex].FindControl("tbx_cBirth");

                if (Convert.ToInt32(e.Keys[0]) == -1) {
                    if (db.ExecuteNonQuery("INSERT INTO child (SN,GN,BD,UID) VALUES(?,?,?,?);", e.NewValues["SN"], e.NewValues["GN"], DateTime.Parse(e.NewValues["BD"].ToString()), RegisteredUID) > 0) {
                        lit_msg.Text = CreateMSGString("Kind erfolgreich angelegt!", "success");
                    } else {
                        lit_msg.Text = CreateMSGString("Fehler beim Anlegen", "warning");
                    }
                }

                if (db.ExecuteNonQuery("UPDATE child SET SN=?,GN=?,BD=? WHERE CID=?", e.NewValues["SN"], e.NewValues["GN"], DateTime.Parse(e.NewValues["BD"].ToString()), (int)e.Keys[0]) > 0) {
                    lit_msg.Text = CreateMSGString("Kind erfolgreich geändert!", "success");
                } else {
                    lit_msg.Text = CreateMSGString("Fehler beim Ändern", "warning");
                }

                gvChildren.EditIndex = -1;
                Fill_gvChildren(RegisteredUID);
            } else {
                Console.WriteLine("Fuck");
            }

            
        }

        protected void gvChildren_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e) {
            gvChildren.EditIndex = -1;
            Fill_gvChildren(RegisteredUID);
        }

        protected void gvChildren_RowDeleting(object sender, GridViewDeleteEventArgs e) {

        }

        protected void gvChildren_SelectedIndexChanging(object sender, GridViewSelectEventArgs e) {

        }




        private void Fill_gvChildren(string uid) {

            if(!String.IsNullOrEmpty(uid)) {
                CsharpDB db = new CsharpDB();
                DataTable dtChildren = db.Query("SELECT * FROM child WHERE UID=?;", uid);
                gvChildren.DataSource = null;
                DataView dvChildren = new DataView(dtChildren);
                gvChildren.DataSource = dvChildren;
                gvChildren.DataBind();
            }
           
        }

        protected void btn_login_Click(object sender, EventArgs e) {

        }

        protected void btn_register_Click(object sender, EventArgs e) {
            if(String.IsNullOrEmpty(RegisteredUID)) {
                //Allg Daten

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

                                div_userdata.Visible = false;
                                div_pwd.Visible = false;
                                div_children.Visible = true;
                                btn_register.Text = "Registrierung abschließen";

                                DataTable dt1 = db.Query("SELECT UID FROM user WHERE EMAIL=?;", tbx_email.Text);
                                if (dt1.Rows.Count>0) {
                                    RegisteredUID = dt1.Rows[0]["UID"].ToString();
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

            } else {
                //Children
                Response.Redirect("MyRegistrations.aspx");
            }
        }


        private string CreateMSGString(string msg,string type) {
           return "<div class=\"alert alert-"+type+" mt-3 mb-1\" role=\"alert\">" + msg + "</div>";
        }

        protected void btn_addChild_Click(object sender, EventArgs e) {
            CsharpDB db = new CsharpDB();
            DataTable dt = db.Query("SELECT * FROM child LIMIT 1");
            dt.Clear();
            DataRow newRow = dt.NewRow();
            newRow["CID"] = -1;
            newRow["BD"] = DateTime.Now;

            dt.Rows.Add(newRow);
            gvChildren.DataSource = dt;
            gvChildren.EditIndex = 0;
            gvChildren.DataBind();


            btn_addChild.Text = "Weiteres Kind anlegen";
        }
    }
}