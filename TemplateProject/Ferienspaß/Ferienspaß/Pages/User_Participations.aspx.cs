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
    public partial class User_Participations : System.Web.UI.Page
    {
        CsharpDB db;
        protected void Page_Load(object sender, EventArgs e)
        {
            db = new CsharpDB();
            if (!Page.IsPostBack)
            {
                Fill_gv_participations();
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

        private void Fill_gv_participations()
        {
            DataTable dt = db.Query($"SELECT gn, sn, bd, name, project.DATE " +
                $"FROM participation " +
                $"LEFT JOIN child ON child.CID = participation.cid " +
                $"LEFT JOIN project " +
                $"ON participation.PID = project.PID " +
                $"WHERE child.UID = {User.Identity.Name}");

            if (dt.Rows.Count == 0)
                lbl_Message.Text = "Keine Daten verfügbar";

            DataView dv = new DataView(dt);

            gv_participations.DataSource = dv;
            gv_participations.DataBind();
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }
    }
}