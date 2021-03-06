﻿using System;
using System.Data;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace Ferienspaß {
    public partial class RegistrationChildren : System.Web.UI.Page {

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
            if(!IsPostBack) {
                Fill_gvChildren();
            }
            
            if(Request.QueryString["fromRegistration"]=="1") lit_msg.Text = CreateMSGString("Erfolgreich registriert! Sie können Ihre Kinder nun anlgegen.", "info");
        }




        protected void gvChildren_RowEditing(object sender, GridViewEditEventArgs e) {
            gvChildren.EditIndex = e.NewEditIndex;
            Fill_gvChildren();
        }

        protected void gvChildren_RowUpdating(object sender, GridViewUpdateEventArgs e) {
            CsharpDB db = new CsharpDB();

            try {
                TextBox tbx_birth = (TextBox)gvChildren.Rows[e.RowIndex].FindControl("tbx_cBirth");
                DateTime bd = DateTime.Parse(e.NewValues["BD"].ToString());

                if(bd.Year<=(DateTime.Now.Year-100)) {
                    lit_msg.Text = CreateMSGString("Geburtsjahr ist nicht gültig! Ihre Eingabe: "+bd.Year, "warning");
                    return;
                }


                if (!String.IsNullOrEmpty(e.NewValues["SN"].ToString()) && !String.IsNullOrEmpty(e.NewValues["GN"].ToString())) {
                    if (Convert.ToInt32(e.Keys[0]) == -1) {
                        if (db.ExecuteNonQuery("INSERT INTO child (SN,GN,BD,UID) VALUES(?,?,?,?);", e.NewValues["SN"], e.NewValues["GN"], DateTime.Parse(tbx_birth.Text), Convert.ToInt32(User.Identity.Name)) > 0) {
                            lit_msg.Text = CreateMSGString("Kind erfolgreich angelegt!", "success");
                        } else {
                            lit_msg.Text = CreateMSGString("Fehler beim Anlegen", "warning");
                        }
                    } else if (db.ExecuteNonQuery("UPDATE child SET SN=?,GN=?,BD=? WHERE CID=?", e.NewValues["SN"], e.NewValues["GN"], DateTime.Parse(tbx_birth.Text), (int)e.Keys[0]) > 0) {
                        lit_msg.Text = CreateMSGString("Kind erfolgreich geändert!", "success");
                    } else {
                        lit_msg.Text = CreateMSGString("Fehler beim Ändern", "warning");
                    }

                    gvChildren.EditIndex = -1;
                    Fill_gvChildren();
                } else {
                    lit_msg.Text = CreateMSGString("Alle Felder müssen ausgefüllt werden!", "warning");
                }


            } catch(Exception ex) {
                lit_msg.Text = CreateMSGString("Ein Fehler ist aufgetreten! Überprüfen Sie Ihre Eingaben!", "danger");
            }

            

            
        }

        protected void gvChildren_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e) {
            gvChildren.EditIndex = -1;
            Fill_gvChildren();
        }

        protected void gvChildren_RowDeleting(object sender, GridViewDeleteEventArgs e) {

            CsharpDB db = new CsharpDB();
            if (db.ExecuteNonQuery("DELETE FROM child WHERE CID=?;",Convert.ToInt32(e.Keys[0])) > 0) {
                lit_msg.Text = CreateMSGString("Kind erfolgreich entfernt!", "success");
            } else {
                lit_msg.Text = CreateMSGString("Fehler beim Entfernen eines Kindes!", "warning");
            }
            Fill_gvChildren();
        }

        protected void gvChildren_SelectedIndexChanging(object sender, GridViewSelectEventArgs e) {

        }




        private void Fill_gvChildren() {
            int parentID;
            if(int.TryParse(User.Identity.Name,out parentID)) {
                CsharpDB db = new CsharpDB();
                DataTable dtChildren = db.Query("SELECT * FROM child WHERE UID=?;", parentID);
                gvChildren.DataSource = null;
                DataView dvChildren = new DataView(dtChildren);
                gvChildren.DataSource = dvChildren;
                gvChildren.DataBind();
            } else {
                lit_msg.Text = CreateMSGString("Fehler beim Laden der Daten!", "danger");
            }
           
        }

        protected void btn_login_Click(object sender, EventArgs e) {

        }

        protected void btn_register_Click(object sender, EventArgs e) {
            

        }


        private string CreateMSGString(string msg,string type) {
           return "<div class=\"alert alert-"+type+" mt-3 mb-1\" role=\"alert\">" + msg + "</div>";
        }

        protected void btn_addChild_Click(object sender, EventArgs e) {
            CsharpDB db = new CsharpDB();
            DataTable dt = db.Query("SELECT * FROM child LIMIT 1");
            

            DataRow newRow = dt.NewRow();
            gvChildren.DataSource = null;
            dt.Clear();
            newRow["CID"] = -1;
            newRow["BD"] = DateTime.Now;
            dt.Rows.Add(newRow);
            
            gvChildren.DataSource = dt;
            gvChildren.EditIndex = 0;
            gvChildren.DataBind();


            btn_addChild.Text = "Weiteres Kind anlegen";
        }

        protected void gvChildren_RowUpdated(object sender, GridViewUpdatedEventArgs e) {
            
        }

        protected void btn_delete_Click(object sender, EventArgs e) {

        }

        protected void btn_back_Click(object sender, EventArgs e) {
            Session["registrationMSG"] = "Bestätigen Sie ihre Email-Adresse um fortzufahren!<br>Prüfen Sie Ihr EMail-Postfach.";
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
            Response.Redirect("Pages/Login.aspx");
        }
    }
}