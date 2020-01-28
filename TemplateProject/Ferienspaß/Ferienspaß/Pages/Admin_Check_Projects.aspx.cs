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

        protected void gvProjects_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "two_weeks":

                    DataTable dt = db.Query("SELECT user.EMAIL, Concat(user.GN,'',user.SN) as username, Concat(child.GN,' ',child.SN) as childname " +
                        "FROM project INNER JOIN participation " +
                        "ON participation.PID = project.PID " +
                        "INNER JOIN child ON participation.CID = child.cid " +
                        "INNER JOIN user ON user.UID = child.UID " +
                        "WHERE project.PID = 27");


                    //klasse mailinfo deklarieren
                    //alle infos in eine liste schreiben
                    //Für jedes item eine mail schicken 


                    for(int i = 0; i < dt.Rows.Count; i++)
                    {
                        var mailInfo = new { mail = dt.Rows[i]["email"].ToString(), username = dt.Rows[i]["username"].ToString(), childname = dt.Rows[i]["childname"].ToString()};
                    }


                    break;
            }
        }
    }
}