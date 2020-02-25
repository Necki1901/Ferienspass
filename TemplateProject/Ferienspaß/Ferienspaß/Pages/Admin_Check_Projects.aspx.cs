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
    public partial class Admin_Check_Projects : System.Web.UI.Page
    {

        CsharpDB db;
        protected void Page_Load(object sender, EventArgs e)
        {

            // PRIVILLEGE CHECK
            int ug = CsharpDB.GetUserGroup(Session["usergroup"]);
            if(ug!=0) {
                Response.Redirect("NotPermittedPage.html");
            }

            db = new CsharpDB();
            if (!Page.IsPostBack)
            {
                Fill_gvAdminProjects();
            }

            try
            {
                db = new CsharpDB();

                HtmlGenericControl c = (HtmlGenericControl)Master.FindControl("menu_mydata");
                if (c != null) c.Attributes.Add("class", "active");

                lbl_loggedInUser.Text = db.GetUserName(User.Identity.Name);
            }
            catch (Exception ex)
            {

            }
        }



        private void Fill_gvAdminProjects()
        {
            DataTable dt = db.Query("SELECT * FROM project");
            DataView  dv = new DataView(dt);
            gvProjects.DataSource = dv;
            gvProjects.DataBind();
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }

        private string CreateMSGString(string msg, string type)
        {
            return "<div class=\"alert alert-" + type + " mt-3 mb-1\" role=\"alert\">" + msg + "</div>";
        }

        protected void btnTwoWeeks_Click(object sender, EventArgs e)
        {
            List<int> projectIDs = GetProjectIDsWithinPeriod(14);

            if (projectIDs.Count == 0)
                lit_msg.Text = CreateMSGString("Es gibt keine Teilnehmer", "warning");
            else
            {
                foreach (var pid in projectIDs)
                {
                    List<int> userIDs = GetUIDsWithinTwoWeeks(pid);

                    foreach (var id in userIDs)
                    {
                        if (CheckIfIDExists(pid, id) == true)
                        {
                            //get all participation user IDs
                            DataTable dt = db.Query("SELECT participation.cid, project.name, project.DATE, participation.paid, " +
                            "user.EMAIL, Concat(user.GN, ' ', user.SN) as username, Concat(child.GN, ' ', child.SN) as childname " +
                                "FROM project INNER JOIN participation " +
                                "ON participation.PID = project.PID " +
                                "INNER JOIN child ON participation.CID = child.cid " +
                                "INNER JOIN user ON user.UID = child.UID " +
                                "WHERE project.DATE > NOW() AND project.DATE < NOW() + INTERVAL 14 DAY " +
                                $"AND project.pid = {pid}");

                                SendReminderMailToUser(dt);
                        }
                    }
                }
                lit_msg.Text = CreateMSGString("Emails wurden versandt", "success");

            }

        }

        private List<int> GetUIDsWithinTwoWeeks(int pid)
        {
            DataTable dt = db.Query("SELECT user.UID " +
                "FROM project INNER JOIN participation " +
                "ON participation.PID = project.PID " +
                "INNER JOIN child ON participation.CID = child.cid " +
                "INNER JOIN user ON user.UID = child.UID " +
                "WHERE project.DATE > NOW() AND project.DATE < NOW() + INTERVAL 14 DAY " +
                $"AND project.pid = {pid}");

            List<int> ids = new List<int>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int item = (int)dt.Rows[i]["uid"];
                if (ids.Contains(item) == false)
                    ids.Add(item);
            }

          
            return ids;
        }

        private bool CheckIfIDExists(int pid, int uid)
        {
            DataTable dt = db.Query("SELECT participation.cid " +
                               "FROM project INNER JOIN participation " +
                               "ON participation.PID = project.PID " +
                               "INNER JOIN child ON participation.CID = child.cid " +
                               "INNER JOIN user ON user.UID = child.UID " +
                               "WHERE project.DATE > NOW() AND project.DATE < NOW() + INTERVAL 14 DAY " +
                               $"AND project.pid = {pid} AND user.uid = {uid}");

            return dt.Rows.Count != 0;
        }

        private void SendReminderMailToUser(DataTable dt)
        {

            string body = $"Erinnerungsmail für das Projekt {dt.Rows[0]["name"]}<br>";
            body += $"Dieses Projekt findet am {Convert.ToDateTime(dt.Rows[0]["date"]).ToString("dd/MM/yyyy")} statt<br>";
            body += "Sie Haben folgende Kinder zu dem Projekt angemeldet:<br>";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                body += $"{dt.Rows[i]["childname"]}<br>";
            }

            //sendMail
            db.SendHTMLEmail((string)dt.Rows[0]["email"], (string)dt.Rows[0]["username"], db.GetPortalOption("MAIL_REMINDER_SUBJECT"), body, false, "", "", "", db.GetPortalOption("MAIL_GRUSSFORMEL"), db.GetPortalOption("MAIL_HINWEIS"));

        }


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            List<int> projectIDs = GetProjectIDsWithinPeriod(7);

            if(projectIDs.Count == 0)
                lit_msg.Text = CreateMSGString("Es gibt keine nicht bezahlten Anmeldungen", "warning");

            else
            {
                foreach (var pid in projectIDs)
                {
                    List<int> userIDs = GetUIDsWithinOneWeekNotPaid(pid);

                    foreach (var id in userIDs)
                    {
                        //command to get the data
                        DataTable dt = db.Query("SELECT participation.cid, project.name, project.DATE, project.pid , participation.paid, " +
                            "user.EMAIL, Concat(user.GN, ' ', user.SN) as username, Concat(child.GN, ' ', child.SN) as childname " +
                            "FROM project INNER JOIN participation " +
                            "ON participation.PID = project.PID " +
                            "INNER JOIN child ON participation.CID = child.cid " +
                            "INNER JOIN user ON user.UID = child.UID " +
                            "WHERE project.DATE > NOW() AND project.DATE < NOW() + INTERVAL 7 DAY " +
                            $"AND user.uid = {id} AND paid = 0 AND project.pid = {pid}");

                        //deletes the participation from the database
                        DeleteParticipations(dt);

                        //send mail
                        SendDeletedMailToUser(dt);
                    }
                    lit_msg.Text = CreateMSGString("Anmeldungen wurden gelöscht", "success");
                }
            }

        }

        private List<int> GetProjectIDsWithinPeriod(int days)
        {
            DataTable dt = db.Query("SELECT project.pid " +
                       "FROM project INNER JOIN participation " +
                       "ON participation.PID = project.PID " +
                       "INNER JOIN child ON participation.CID = child.cid " +
                       "INNER JOIN user ON user.UID = child.UID " +
                       $"WHERE project.DATE > NOW() AND project.DATE < NOW() + INTERVAL {days} DAY");

            List<int> projectIDs = new List<int>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int item = (int)dt.Rows[i]["pid"];
                if (projectIDs.Contains(item) == false)
                    projectIDs.Add(item);

            }

            return projectIDs;

        }

        private void DeleteParticipations(DataTable dt)
        {
            //delete every participation for child, which is not paid
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                db.Query($"DELETE FROM participation WHERE cid = {dt.Rows[i]["cid"]} AND pid = {dt.Rows[i]["pid"]}");
            }
        }

        private void SendDeletedMailToUser(DataTable dt)
        {
            string body = $"Ihre Anmeldung für das Projekt {dt.Rows[0]["name"]} am {Convert.ToDateTime(dt.Rows[0]["date"]).ToString("dd/MM/yyyy")} wurde gelöscht, " +
                $"da Sie nicht rechtzeitig bezahlt haben!<br>";
            body += "folgende Anmeldungen sind betroffen:<br>";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                body += $"{dt.Rows[i]["childname"]}<br>";
            }

            //sendMail
            db.SendHTMLEmail((string)dt.Rows[0]["email"], (string)dt.Rows[0]["username"], db.GetPortalOption("MAIL_REGISTRATION_DELETED_SUBJECT"), body, false, "", "", "", db.GetPortalOption("MAIL_GRUSSFORMEL"), db.GetPortalOption("MAIL_HINWEIS"));

        }

        private List<int> GetUIDsWithinOneWeekNotPaid(int pid)
        {
            DataTable dt = db.Query("SELECT user.UID " +
                  "FROM project INNER JOIN participation " +
                  "ON participation.PID = project.PID " +
                  "INNER JOIN child ON participation.CID = child.cid " +
                  "INNER JOIN user ON user.UID = child.UID " +
                  "WHERE project.DATE > NOW() AND project.DATE < NOW() + INTERVAL 7 DAY " +
                  $"AND paid = 0 AND project.pid = {pid}");

            List<int> ids = new List<int>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int item = (int)dt.Rows[i]["uid"];
                if (ids.Contains(item) == false)
                    ids.Add(item);
            }

            return ids;
        }


    }
}