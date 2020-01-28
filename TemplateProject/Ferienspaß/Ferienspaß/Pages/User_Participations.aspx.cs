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
                        //get the ids from the gridview
                        int pid = Convert.ToInt32(((Label)gv_participations.Rows[rowIndex].FindControl("lbl_pid")).Text);
                        int cid = Convert.ToInt32(((Label)gv_participations.Rows[rowIndex].FindControl("lbl_cid")).Text);

                        Send_cancel_mail(pid, cid);
                        Cancel_participation(pid, cid);
                        Write_cancellation__in_db(pid, cid);

                        lit_msg.Text = CreateMSGString("Ihre Anmeldung wurde storniert", "info");
                        break;
                    }
            }

        }

        private void Cancel_participation(int pid, int cid)
        {
            db.Query($"DELETE FROM participation WHERE pid = {pid} AND cid = {cid}");
            Fill_gv_participations();
        }

        private void Write_cancellation__in_db(int pid, int cid)
        {
            db.Query($"INSERT INTO cancellations (cid, pid, date) VALUES ({cid}, {pid}, '{DateTime.Now.ToString("yyyy/MM/dd")}')");
        }

        private void Send_cancel_mail(int pid, int cid)
        {
            DataTable dt = db.Query($"SELECT gn, sn, email FROM user WHERE uid = {User.Identity.Name}");

            string fullname = $"{dt.Rows[0]["gn"]} {dt.Rows[0]["sn"]}";

            db.SendHTMLEmail((string)dt.Rows[0]["email"], fullname, db.GetPortalOption("MAIL_PROJECT_STORNIERUNG_SUBJECT"), Get_body_for_cancel(), false, "", "", "", db.GetPortalOption("MAIL_GRUSSFORMEL"), db.GetPortalOption("MAIL_HINWEIS"));

           // db.SendMail((string)dt.Rows[0]["email"], fullname, "Stornierung Ferienspaß", Get_body_for_cancel());

            string Get_body_for_cancel()
            {
                DataTable dt_project = db.Query($"SELECT name FROM project WHERE pid = {pid}");
                DataTable dt_child = db.Query($"SELECT gn, sn FROM child WHERE cid = {cid}");

                string fullname_child = $"{dt_child.Rows[0]["gn"]} {dt_child.Rows[0]["sn"]}";

                if (Convert.ToInt32(db.Query($"SELECT paid FROM participation WHERE pid = {pid} AND cid = {cid}").Rows[0][0]) == 1) 
                {
                    return $"Stornierung Projekt {dt_project.Rows[0]["name"]}\n" +
                    $"Die Stornierung der Anmeldung von {fullname_child} ist erfolgt.\n\n" +
                    "Den bereits überwiesenen Betrag können Sie sich im Gemeindegebäude vom Portier zurückerstatten lassen.";
                }
                else
                {
                    return $"Stornierung Projekt {dt_project.Rows[0]["name"]}\n" +
                        $"Die Stornierung der Anmeldung von {fullname_child} ist erfolgt";
                }
            }
        }

        private string CreateMSGString(string msg, string type)
        {
            return "<div class=\"alert alert-" + type + " mt-3 mb-1\" role=\"alert\">" + msg + "</div>";
        }
    }
}