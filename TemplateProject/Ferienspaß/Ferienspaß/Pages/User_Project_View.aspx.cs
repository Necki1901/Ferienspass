using Ferienspaß.Classes;
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
    public partial class Projectview : System.Web.UI.Page
    {
        CsharpDB db = new CsharpDB();
        static bool isFiltered = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Fill_gv_UserView();
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


        private void Fill_gv_UserView()
        {
            DataTable dt;
            DataView dv;
            string sql;
            bool filter = false;
            if (isFiltered)
            {

                if (cbTooManyParticipants.Checked == true && cbNoParticipants.Checked == true)
                {
                    sql = "SELECT *, (project.CAPACITY - COUNT( participation.PID)) AS 'remainingCapacity' FROM project JOIN participation ON project.PID = participation.PID GROUP BY project.PID";
                }
                else if (cbTooManyParticipants.Checked == true)
                {
                    filter = true;
                    sql = "SELECT *, (project.CAPACITY - COUNT( participation.PID)) AS 'remainingCapacity' FROM project LEFT JOIN participation ON project.PID = participation.PID GROUP BY project.PID HAVING remainingCapacity>0";
                }
                else if (cbNoParticipants.Checked == true)
                {
                    filter = true;
                    sql = "SELECT *, (project.CAPACITY - COUNT( participation.PID)) AS 'remainingCapacity' FROM project LEFT JOIN participation ON project.PID = participation.PID GROUP BY project.PID HAVING remainingCapacity<project.CAPACITY";
                }
                else
                {
                    sql = "SELECT *, (project.CAPACITY - COUNT( participation.PID)) AS 'remainingCapacity' FROM project LEFT JOIN participation ON project.PID = participation.PID GROUP BY project.PID";
                }

                if (txtSuchen.Text != string.Empty)
                {
                    if (filter) sql += " And "; else sql += " HAVING "; filter = true;
                    sql += $"Name Like '%{txtSuchen.Text.ToLower()}%'";
                }

                if (datepicker.Text != string.Empty)
                {
                    if (filter) sql += " And "; else sql += " HAVING ";
                    sql += $"project.date='{datepicker.Text}'";
                }
            }
            else
            {
                sql = "SELECT *, (project.CAPACITY - COUNT( participation.PID)) AS 'remainingCapacity' FROM project LEFT JOIN participation ON project.PID = participation.PID GROUP BY project.PID";
            }

            dt = db.Query(sql);
            dv = new DataView(dt);

            gv_UserView.DataSource = dv;
            gv_UserView.DataBind();

        }

       

        protected void gv_UserView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "details":

                    GridViewRow gvr = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                    string projectID = ((Label)gvr.FindControl("lblProjectID")).Text;
                    Response.Redirect($"User_Project_View_Details.aspx?id={projectID}");
                    break;
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            isFiltered = true;
            Fill_gv_UserView();
        }

        protected void gv_UserView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_UserView.PageIndex = e.NewPageIndex;
            Fill_gv_UserView();
        }

    }
}