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
            DataTable dt = db.Query($"SELECT participation.CID, participation.pid, gn, sn, bd, name, project.DATE " +
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

        protected void gv_participations_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "cancel_participation":
                    //get the rowindex
                    GridViewRow gvr = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                    int rowIndex = gvr.RowIndex;


                    //get the datetime of the project
                    DateTime project_date = Convert.ToDateTime(((Label)gv_participations.Rows[rowIndex].FindControl("lbl_project_date")).Text);

                    //you can't cancel if the project is within 1 week
                    TimeSpan time_left = project_date - DateTime.Today;

                    if (time_left < new TimeSpan(8, 0, 0, 0))
                    {
                        lit_msg.Text = CreateMSGString("Diese Anmeldung kann nicht mehr storniert werden!", "warning");
                        break;
                    }
                    else
                    {
                        Cancel_participation(rowIndex);

                        lit_msg.Text = CreateMSGString("Anmeldung erfolgreich storniert", "info");
                        break;
                    }
            }

        }
        private void Cancel_participation(int rowIndex)
        {
            int cid = Convert.ToInt32(((Label)gv_participations.Rows[rowIndex].FindControl("lbl_cid")).Text);
            int pid = Convert.ToInt32(((Label)gv_participations.Rows[rowIndex].FindControl("lbl_pid")).Text);

            db.Query($"DELETE FROM participation WHERE pid = {pid} AND cid = {cid}");
        }

        private string CreateMSGString(string msg, string type)
        {
            return "<div class=\"alert alert-" + type + " mt-3 mb-1\" role=\"alert\">" + msg + "</div>";
        }
    }
}