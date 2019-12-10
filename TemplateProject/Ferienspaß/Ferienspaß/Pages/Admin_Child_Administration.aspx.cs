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
                //if (Request.QueryString["id"] == null)
                //    Response.Redirect("Admin_User_View.aspx");
                //ViewState["UID"] = Convert.ToInt32(Request.QueryString["id"]);
                ViewState["UID"] = Convert.ToInt32(1);   //only for testing 
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

                    DataView dv = new DataView(dt);
                    gv_Children.DataSource = dt;
                    gv_Children.DataBind();

                    gv_Children.EditIndex = 0;
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

            //Werte erfassen
            string childID = ((Label)gv_Children.Rows[e.RowIndex].FindControl("lblChildID")).Text;
            string newSurname = ((TextBox)gv_Children.Rows[e.RowIndex].FindControl("txtSurname")).Text;
            string newGivenname = ((TextBox)gv_Children.Rows[e.RowIndex].FindControl("txtGivenname")).Text;
            DateTime newBirthday = Convert.ToDateTime(((TextBox)gv_Children.Rows[e.RowIndex].FindControl("txtBirthday")).Text);

            db.Query($"UPDATE child SET GN = '{newGivenname}', SN = '{newSurname}', BD = '{newBirthday.ToString("yyyy/MM/dd")}'" +
                $"WHERE CID = {childID}");

            gv_Children.EditIndex = -1;
            gv_Children.SelectedIndex = -1;
            Fill_gv_Children();
        }

        protected void gv_Children_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gv_Children.EditIndex = -1;
            gv_Children.SelectedIndex = -1;
            Fill_gv_Children();
        }
    }
}