using Ferienspaß.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ferienspaß.Pages
{
    public partial class User_View : System.Web.UI.Page
    {

        CsharpDB db = new CsharpDB();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Fill_gv_UserView();
            }
        }


        private void Fill_gv_UserView()
        {
            DataTable dt;
            DataView dv;
            RemainingCapacity rc = new RemainingCapacity();

            dt = db.Query($"SELECT * FROM project");
            dt = rc.GetDataTableWithRemainingCapacities(dt);
            dv = new DataView(dt);


            gv_UserView.DataSource = dv;
            gv_UserView.DataBind();

        }



        protected void gv_UserView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
            //Code used -> https://stackoverflow.com/questions/6503339/get-row-index-on-asp-net-rowcommand-event/6503483

            switch (e.CommandName)
            {
                case "details":

                    string projectID = ((Label)gvr.FindControl("lblProjectID")).Text;
                    Response.Redirect($"User_View_Details.aspx?id={projectID}");
                    break;


            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            DataTable dt;
            DataView dv;
            string sql;
            bool condition = false;

            if (cbTooManyParticipants.Checked == true && cbNoParticipants.Checked == true)
            {
                sql = "SELECT *, (project.CAPACITY - COUNT( participation.PID)) AS 'participants' FROM project JOIN participation ON project.PID = participation.PID GROUP BY project.PID";
            }
            else if (cbTooManyParticipants.Checked == true)
            {
                condition = true;
                sql = "SELECT *, (project.CAPACITY - COUNT( participation.PID)) AS 'participants' FROM project LEFT JOIN participation ON project.PID = participation.PID GROUP BY project.PID HAVING participants>0";
            }
            else if (cbNoParticipants.Checked == true)
            {
                condition = true;
                sql = "SELECT *, (project.CAPACITY - COUNT( participation.PID)) AS 'participants' FROM project LEFT JOIN participation ON project.PID = participation.PID GROUP BY project.PID HAVING participants<project.CAPACITY";
            }
            else
            {
                sql = "SELECT *, (project.CAPACITY - COUNT( participation.PID)) AS 'participants' FROM project LEFT JOIN participation ON project.PID = participation.PID GROUP BY project.PID";
            }
            
            
            if(condition == true)
            {
                if (txtSuchen.Text != string.Empty)
                {
                    sql += $" AND Name Like '%{txtSuchen.Text.ToLower()}%'";
                }

                if (datepicker.Text != string.Empty && txtSuchen.Text == string.Empty)
                {
                    sql += $" AND project.date={datepicker.Text}";
                }
            }
            else
            {
                if (txtSuchen.Text != string.Empty)
                {
                    sql += $" Having Name Like '%{txtSuchen.Text.ToLower()}%'";
                    if (datepicker.Text != string.Empty)
                    {
                        sql += $" AND project.date='{datepicker.Text}'";
                    }
                }

                if (datepicker.Text != string.Empty && txtSuchen.Text == string.Empty)
                {
                    sql += $" Having project.date='{datepicker.Text}'";
                }
            }



            dt = db.Query(sql);

            dv = new DataView(dt);


            gv_UserView.DataSource = dv;
            gv_UserView.DataBind();
        }
    }
}