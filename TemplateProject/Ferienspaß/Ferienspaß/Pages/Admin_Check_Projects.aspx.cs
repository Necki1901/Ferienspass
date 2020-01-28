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
     
        protected void btnTwoWeeks_Click(object sender, EventArgs e)
        {
            List<int> userIDs = GetUIDsWithinTwoWeeks();

            foreach (var id in userIDs)
            {
                SendMailToUser(id);
            }
        }

        private void SendMailToUser(int id)
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
            List<int> ids = new List<int>();

            //get all participation user IDs
            DataTable dt = db.Query("SELECT user.UID " +
                "FROM project INNER JOIN participation " +
                "ON participation.PID = project.PID " +
                "INNER JOIN child ON participation.CID = child.cid " +
                "INNER JOIN user ON user.UID = child.UID " +
                "WHERE project.DATE > NOW() AND project.DATE < NOW() + INTERVAL 14 DAY");


            //only add the items, which are not yet in the list
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                int item = (int)dt.Rows[i]["uid"];
                if (ids.Contains(item) == false)
                    ids.Add(item);
            }

            return ids;
        }
    }
}