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

    public partial class Admin_Participants_Print : System.Web.UI.Page
    {
        CsharpDB db;
        protected void Page_Load(object sender, EventArgs e)
        {

            // PRIVILLEGE CHECK
            int ug = CsharpDB.GetUserGroup(Session["usergroup"]);
            if (ug != 0)
            {
                Response.Redirect("NotPermittedPage.html");
            }


            db = new CsharpDB();
            if (!Page.IsPostBack)
            {
                ViewState["project"] = Request.QueryString["project"];
                Fill_gv_Participants();
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

        private void Fill_gv_Participants()
        {
            DataTable dt = db.Query($"SELECT child.GN, child.SN, child.BD, participation.paid, participation.CID, user.PHONE " +
              $"FROM child " +
              $"INNER JOIN participation " +
              $"ON child.CID = participation.CID " +
              $"INNER JOIN user " +
              $"ON user.UID = child.UID " +
              $"WHERE participation.PID = {ViewState["project"]}");

            if (dt.Rows.Count == 0)
                lbl_Message.Text = "Keine Anmeldungen vorhanden";
            DataView dv = new DataView(dt);

            Set_heading();

            gv_participants.DataSource = dv;
            gv_participants.DataBind();
        }

        private void Set_heading()
        {
            DataTable dt = db.Query($"SELECT name, description FROM project WHERE pid = {ViewState["project"]}");

            if(dt.Rows.Count != 0)
            {
                lbl_project.Text = dt.Rows[0]["name"].ToString();
                lbl_description.Text = dt.Rows[0]["description"].ToString();

            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }
    }
}