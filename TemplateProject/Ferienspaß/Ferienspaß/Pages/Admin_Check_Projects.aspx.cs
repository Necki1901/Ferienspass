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
            List<int> userIDs = GetUIDsWithinTwoWeeks();

            if(userIDs.Count == 0)
                lit_msg.Text = CreateMSGString("Es gibt keine Teilnehmer", "warning");
            else
            {
                foreach (var id in userIDs)
                {
                    SendReminderMailToUser(id);
                }
                lit_msg.Text = CreateMSGString("Emails wurden versandt", "success");
            }
        }

        private void SendReminderMailToUser(int id)
        {
            //get the needed data for the specific id
            DataTable dt = db.Query($"SELECT project.PID, project.date, project.name, user.EMAIL, Concat(user.GN, ' ', user.SN) as username, Concat(child.GN, ' ', child.SN) as childname " +
                  $"FROM project INNER JOIN participation " +
                  $"ON participation.PID = project.PID " +
                  $"INNER JOIN child ON participation.CID = child.cid " +
                  $"INNER JOIN user ON user.UID = child.UID " +
                  $"WHERE user.uid = {id} " +
                  $"AND project.DATE > NOW() AND project.DATE < NOW() + INTERVAL 14 DAY");


            string body = $"Erinnerungsmail für das Projekt {dt.Rows[0]["name"]}\n";
            body += $"Dieses Projekt findet am {Convert.ToDateTime(dt.Rows[0]["date"]).ToString("dd/MM/yyyy")} statt\n";
            body += "Sie Haben folgende Kinder zu dem Projekt angemeldet:\n";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                body += $"{dt.Rows[i]["childname"]}\n";
            }

            //sendMail
            db.SendMail((string)dt.Rows[0]["email"], (string)dt.Rows[0]["username"], "Erinnerung für dein Projekt", body);

        }

        private List<int> GetUIDsWithinTwoWeeks()
        {

            //get all participation user IDs
            DataTable dt = db.Query("SELECT user.UID " +
                "FROM project INNER JOIN participation " +
                "ON participation.PID = project.PID " +
                "INNER JOIN child ON participation.CID = child.cid " +
                "INNER JOIN user ON user.UID = child.UID " +
                "WHERE project.DATE > NOW() AND project.DATE < NOW() + INTERVAL 7 DAY");

            List<int> ids = new List<int>();

            //only add the items, which are not yet in the list
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                int item = (int)dt.Rows[i]["uid"];
                if (ids.Contains(item) == false)
                    ids.Add(item);
            }

            return ids;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            List<int> userIDs = GetUIDsWithinOneWeekNotPaid();

            if(userIDs.Count == 0)
                lit_msg.Text = CreateMSGString("Es gibt keine nicht bezahlten Anmeldungen", "warning");
            else
            {
                foreach (var id in userIDs)
                {
                    //command to get the data
                    DataTable dt = db.Query("SELECT participation.cid, project.name, project.DATE,  participation.paid, " +
                        "user.EMAIL, Concat(user.GN, ' ', user.SN) as username, Concat(child.GN, ' ', child.SN) as childname " +
                        "FROM project INNER JOIN participation " +
                        "ON participation.PID = project.PID " +
                        "INNER JOIN child ON participation.CID = child.cid " +
                        "INNER JOIN user ON user.UID = child.UID " +
                        "WHERE project.DATE > NOW() AND project.DATE < NOW() + INTERVAL 7 DAY " +
                        $"AND user.uid = {id} AND paid = 0");

                    //deletes the participation from the database
                    DeleteParticipations(dt);

                    //send mail
                    SendDeletedMailToUser(dt);
                }
                lit_msg.Text = CreateMSGString("Anmeldungen wurden gelöscht", "success");
            }
        }

        private void DeleteParticipations(DataTable dt)
        {
            //delete every participation for child, which is not paid
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                db.Query($"DELETE FROM participation WHERE cid = {dt.Rows[i]["cid"]}");
            }
        }

        private void SendDeletedMailToUser(DataTable dt)
        {
            string body = $"Ihre Anmeldung für das Projekt {dt.Rows[0]["name"]} am {Convert.ToDateTime(dt.Rows[0]["date"]).ToString("dd/MM/yyyy")} wurde gelöscht, " +
                $"da Sie nicht rechtzeitig bezahlt haben!\n";
            body += "folgende Anmeldungen sind betroffen:\n";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                body += $"{dt.Rows[i]["childname"]}\n";
            }

            db.SendMail((string)dt.Rows[0]["email"], (string)dt.Rows[0]["username"], "Löschung Ihrer Anmeldung", body);
        }

        private List<int> GetUIDsWithinOneWeekNotPaid()
        {
            DataTable dt = db.Query("SELECT user.UID " +
                  "FROM project INNER JOIN participation " +
                  "ON participation.PID = project.PID " +
                  "INNER JOIN child ON participation.CID = child.cid " +
                  "INNER JOIN user ON user.UID = child.UID " +
                  "WHERE project.DATE > NOW() AND project.DATE < NOW() + INTERVAL 7 DAY " +
                  "AND paid = 0");

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