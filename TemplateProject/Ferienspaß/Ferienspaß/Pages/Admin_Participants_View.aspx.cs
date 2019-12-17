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
    public partial class Admin_Participants_View : System.Web.UI.Page
    {

        CsharpDB db;
        protected void Page_Load(object sender, EventArgs e)
        {
            db = new CsharpDB();
            if (!Page.IsPostBack)
            {
                Fill_ddl_Years();
                Fill_ddl_Projects();
                Fill_Gv_Participants();
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

        private void Fill_ddl_Years()
        {
            DataTable dt = db.Query("SELECT date FROM project");
            List<string> years = new List<string>();

            for(int i = 0; i < dt.Rows.Count; i++)
            {
                DateTime date = (DateTime)dt.Rows[i]["date"];
                string yearonly = date.Year.ToString();
                if (years.Contains(yearonly) == false)
                    years.Add(yearonly);
            }

            for(int i = 0; i< years.Count; i++)
            {
                ListItem item = new ListItem(years[i], years[i]);
                if (item.Value == DateTime.Now.Year.ToString())
                    item.Selected = true;
                ddl_Years.Items.Add(item);
            }
        }

        private void Fill_ddl_Projects()
        {
            DataTable dt = db.Query($"SELECT name, pid FROM project where date LIKE '{ddl_Years.SelectedValue}%'");

            ddl_Projects.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string name = (string)dt.Rows[i]["NAME"];
                int id = (int)dt.Rows[i]["PID"];
                ListItem item = new ListItem(name, id.ToString());
                ddl_Projects.Items.Add(item);
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }

        protected void ddl_Projects_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fill_Gv_Participants();
        }

        private void Fill_Gv_Participants()
        {
            DataTable dt = db.Query($"SELECT child.GN, child.SN, child.BD, participation.CID, user.PHONE " +
                $"FROM child " +
                $"INNER JOIN participation " +
                $"ON child.CID = participation.CID " +
                $"INNER JOIN user " +
                $"ON user.UID = child.UID " +
                $"WHERE participation.PID = {ddl_Projects.SelectedValue}");

            if (dt.Rows.Count == 0)
                lbl_Message.Text = "Keine Anmeldungen vorhanden";
            DataView dv = new DataView(dt);

            gv_Participants.DataSource = dv;
            gv_Participants.DataBind();
        }


        protected void ddl_Years_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fill_ddl_Projects();
            Fill_Gv_Participants();
        }
    }
}