using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Ferienspaß.Pages
{
    public partial class Admin_Child_Administration : System.Web.UI.Page
    {
        CsharpDB db;

        protected void Page_Load(object sender, EventArgs e)
        {
            db = new CsharpDB();
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["id"] == null) Response.Redirect("Admin_User_View.aspx");
                ViewState["UID"] = Convert.ToInt32(Request.QueryString["id"]);
                //ViewState["UID"] = Convert.ToInt32(1);   //only for testing 
                ViewState["isAdding"] = false;
                Fill_gv_Children();
            }
            try
            {

                HtmlGenericControl c = (HtmlGenericControl)Master.FindControl("menu_mydata");
                if (c != null) c.Attributes.Add("class", "active");

                lbl_loggedInUser.Text = db.GetUserName(User.Identity.Name);
            }
            catch (Exception ex)
            {

            }
            lblMessage.Text = string.Empty;
        }

        private void Fill_gv_Children()
        {
            DataTable dt;
            DataView dv;

            //dt = db.Query($"SELECT * FROM child where UID = {ViewState["UID"]}");
            dt = db.Query($"SELECT * FROM child where UID = {ViewState["UID"]}");
            dv = new DataView(dt);

            gv_Children.DataSource = dv;
            gv_Children.DataBind();
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }

        protected void gv_Children_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "add":
                    DataTable dt;
                    dt = db.Query($"Select * from child where UID = {ViewState["UID"]} limit 1");
                    dt.Clear();
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                    ViewState["isAdding"] = true;

                    DataView dv = new DataView(dt);
                    gv_Children.DataSource = dt;
                    gv_Children.EditIndex = 0;
                    gv_Children.DataBind();

                    break;
            }
        }

        protected void gv_Children_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gv_Children.EditIndex = e.NewEditIndex;
            Fill_gv_Children();
        }

        protected void gv_Children_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gv_Children.Rows[e.RowIndex];     //Reihe erfassen

            string newSurname = ((TextBox)gv_Children.Rows[e.RowIndex].FindControl("txtSurname")).Text;
            string newGivenname = ((TextBox)gv_Children.Rows[e.RowIndex].FindControl("txtGivenname")).Text;
            string newBirthdayString = ((TextBox)gv_Children.Rows[e.RowIndex].FindControl("txtBirthday")).Text;

            if(ValidateInput(newSurname, newGivenname, newBirthdayString) == true)
            {
                DateTime newBirthday = Convert.ToDateTime(newBirthdayString);

                if (Convert.ToBoolean(ViewState["isAdding"]) == true)
                {
                    db.Query($"INSERT INTO child(GN, SN, BD, UID) VALUES('{newGivenname}', '{newSurname}', '{newBirthday.ToString("yyyy/MM/dd")}', {ViewState["UID"]})");
                }
                else
                {
                    //Werte erfassen
                    string childID = ((Label)gv_Children.Rows[e.RowIndex].FindControl("lblChildID")).Text;

                    db.Query($"UPDATE child SET GN = '{newGivenname}', SN = '{newSurname}', BD = '{newBirthday.ToString("yyyy/MM/dd")}'" +
                        $"WHERE CID = {childID}");
                }
            }

            ViewState["isAdding"] = false;
            gv_Children.EditIndex = -1;
            gv_Children.SelectedIndex = -1;
            Fill_gv_Children();
        }

        private bool ValidateInput(string newSurname, string newGivenname, string newBirthday)
        {
            if(string.IsNullOrEmpty(newSurname) || newSurname.Length > 50)
            {
                lblMessage.Text = "Eingabefehler-Vorname: (höchstens 50 Zeichen)";
                return false;
            }   
            if(string.IsNullOrEmpty(newGivenname) || newGivenname.Length > 50)
            {
                lblMessage.Text = "Eingabefehler-Nachname: (höchstens 50 Zeichen)";
                return false;
            }

            if (string.IsNullOrEmpty(newBirthday) || DateTime.TryParse(newBirthday, out var x) == false)
            {
                lblMessage.Text = "Eingabefehler-Geburtsdatum: (Format(dd/MM/yyyy))";
                return false;
            }
            return true;
        }

        protected void gv_Children_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gv_Children.EditIndex = -1;
            gv_Children.SelectedIndex = -1;
            Fill_gv_Children();
        }

        protected void gv_Children_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = gv_Children.Rows[e.RowIndex];

            string childrenID = ((Label)row.FindControl("lblChildID")).Text;    
            db.Query($"delete from child where CID = {childrenID}");    

            Fill_gv_Children();
        }
    }
}