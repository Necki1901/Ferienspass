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
    public partial class Admin_Project_View_Add : System.Web.UI.Page
    {
        CsharpDB db;

        protected void Page_Load(object sender, EventArgs e)
        {
            db = new CsharpDB();
            lbl_Message.Text = string.Empty;
            if (!Page.IsPostBack)
            {
                Fill_Gridview();
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

        private void Fill_Gridview()
        {
            DataTable dt = db.Query($"SELECT UID, GN, SN from user");

            if (dt.Rows.Count == 0)
                lbl_Message.Text = "Keine Anmeldungen vorhanden";
            DataView dv = new DataView(dt);

            gv_add_child.DataSource = dv;
            gv_add_child.DataBind();
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }
    }
}