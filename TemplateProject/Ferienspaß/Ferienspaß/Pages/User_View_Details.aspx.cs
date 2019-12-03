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
    public partial class User_View_Details : System.Web.UI.Page
    {

        private int projectid;
        CsharpDB db = new CsharpDB();

        private int ProjectID
        {
            get
            {
                projectid = Convert.ToInt32(Request.QueryString["id"]);
                return projectid;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                Fill_gv_UserView_Details(ProjectID);
            }


        }

        private void Fill_gv_UserView_Details(int projectID)
        {
            DataTable dt;
            DataView dv;
            RemainingCapacity rc = new RemainingCapacity();

            dt = db.Query($"SELECT * FROM project WHERE PID={projectid}");
            dt = rc.GetDataTableWithRemainingCapacities(dt);




            dv = new DataView(dt);



            gv_User_View_Details.DataSource = dv;
            gv_User_View_Details.DataBind();


            int remaining_capacity = (int)dt.Rows[0]["remainingCapacity"];
            SetRegisterOptions(remaining_capacity);
            SetQueueOptions(remaining_capacity, dt.Rows[0]);


        }





        public void SetRegisterOptions(int remaining_capacity)
        {
            ImageButton btnRegister = (ImageButton)gv_User_View_Details.Rows[0].FindControl("btnRegister");
            ImageButton btnQueue = (ImageButton)gv_User_View_Details.Rows[0].FindControl("btnQueue");


            if (remaining_capacity == 0)
            {
                btnQueue.Visible = true;
                btnRegister.Visible = false;
                lblMessage.Text = "Das Projekt ist leider ausgebucht." +
                    "Um über eventuelle Verfügbarkeit informiert zu werden, können sie sich in die Warteschlange eintragen";
            }
        }

        public void SetQueueOptions(int remaining_capacity, DataRow datarow)
        {
            ImageButton btnRegister = (ImageButton)gv_User_View_Details.Rows[0].FindControl("btnRegister");

            DateTime time = (DateTime)datarow["Date"];
            TimeSpan time_left = time - DateTime.Today;

            if (time_left < new TimeSpan(8, 0, 0, 0))
            {
                btnRegister.Visible = false;
                switch (remaining_capacity)
                {
                    case 0:
                        lblMessage.Text = "Für dieses Projekt kann man sich nicht mehr anmelden";
                        break;
                    default:
                        lblMessage.Text = "Für dieses Projekt kann man sich nur noch persönlich im Gemeindeamt anmelden";
                        break;

                }
            }
        }

    
    }
}