using Ferienspaß.Classes;
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
    public partial class Portier_Project_View : System.Web.UI.Page
    {

        CsharpDB db;
        protected void Page_Load(object sender, EventArgs e)
        {
            db = new CsharpDB();
            if (!Page.IsPostBack)
            {
                Fill_gv_UserView();
            }
            try
            {
                db = new CsharpDB();

                HtmlGenericControl c = (HtmlGenericControl)Master.FindControl("menu_mydata");
                if (c != null) c.Attributes.Add("class", "active");

                lbl_loggedInUser.Text = db.GetUserName(User.Identity.Name);
            }
            catch (Exception) { }
        }

        private void Fill_gv_UserView()
        {
            DataTable dt;
            DataView dv;
            RemainingCapacity rc = new RemainingCapacity();

            dt = db.Query($"SELECT * FROM project");
            dt = rc.GetDataTableWithRemainingCapacities(dt);
            dv = new DataView(dt);


            gv_UserView.DataSource = dv;
            gv_UserView.DataBind();

        }



        protected void gv_UserView_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            switch (e.CommandName)
            {
                case "details":

                    GridViewRow gvr = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                    string projectID = ((Label)gvr.FindControl("lblProjectID")).Text;
                    Response.Redirect($"Portier_Project_View_Details.aspx?id={projectID}");
                    break;


            }
        }
        protected void gv_UserView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_UserView.PageIndex = e.NewPageIndex;
            Fill_gv_UserView();
        }
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }
    }
}