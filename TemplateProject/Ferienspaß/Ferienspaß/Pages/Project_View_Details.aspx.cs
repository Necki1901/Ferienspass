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
    public partial class User_View_Details : System.Web.UI.Page
    {

        private int projectid;
        CsharpDB db = new CsharpDB();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if(Request.QueryString["id"] == null)
                    Response.Redirect("Project_View.aspx");
                ViewState["PID"] = Convert.ToInt32(Request.QueryString["id"]);
                Fill_gv_UserView_Details();
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

        private void Fill_gv_UserView_Details()
        {
            DataTable dt;
            DataView dv;
            RemainingCapacity rc = new RemainingCapacity();

            dt = db.Query($"SELECT * FROM project WHERE PID={ViewState["PID"]}");
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
            Button btnRegister = (Button)gv_User_View_Details.Rows[0].FindControl("btnRegister");
            Button btnQueue = (Button)gv_User_View_Details.Rows[0].FindControl("btnQueue");


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
            Button btnRegister = (Button)gv_User_View_Details.Rows[0].FindControl("btnRegister");
            Button btnQueue = (Button)gv_User_View_Details.Rows[0].FindControl("btnQueue");


            DateTime time = (DateTime)datarow["Date"];
            TimeSpan time_left = time - DateTime.Today;

            if (time_left < new TimeSpan(8, 0, 0, 0))
            {
               
                btnRegister.Visible = false;
                btnQueue.Visible = false;
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

        protected void gv_User_View_Details_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "register":
                    SetVisibility_Children(true);
                    Fill_gv_Children();
                    break;
                case "queue":
                    try
                    {

                        DataTable dt = db.Query($"SELECT * FROM queue WHERE UID = {User.Identity.Name}");
                        if(dt.Rows.Count == 0)
                        {
                            db.Query($"INSERT INTO queue (UID, PID) VALUES({User.Identity.Name}, {ViewState["PID"]})");
                            lblMessage.Text = "Sie wurden zur Warteschlange hinzugefügt";
                        }
                        else
                        {
                            lblMessage.Text = "Sie haben sich bereits in die Warteschlange eingetragen";
                        }
                    }
                    catch(Exception ex)
                    {
                        lblMessage.Text = ex.Message;
                    }
                    break;
                
            }
        }
        private void Fill_gv_Children()
        {
            DataTable dt;
            DataView dv;
            RemainingCapacity rc = new RemainingCapacity();

            dt = db.Query($"select * from child where child.cid not in(select participation.cid from participation where participation.PID = {ViewState["PID"]}) AND  child.UID = {User.Identity.Name}");
            if (dt.Rows.Count == 0)
            {
                lblChildrenMessage.Text = "Sie können keine Kinder mehr auswählen";
                SetVisibility_Children(false);
            }
            else
            {
                dv = new DataView(dt);
                gv_Children.DataSource = dv;
                gv_Children.DataBind();
            }
            

        }

        protected void btnAddChildren_Click(object sender, EventArgs e)
        {
            List<int> childrenIDs = new List<int>();

            foreach (GridViewRow row in gv_Children.Rows)
            {
                if (((CheckBox)row.FindControl("chkUseChildren")).Checked)
                {
                    int childID = Convert.ToInt32(((Label)row.FindControl("lblChildID")).Text);
                    childrenIDs.Add(childID);
                }
            }

            foreach(int id in childrenIDs)
            {
                db.Query($"INSERT INTO participation (CID, PID, Date) VALUES({id}, {ViewState["PID"]}, '{DateTime.Now.ToString("dd/MM/yyyy")}')");
            }

            foreach (GridViewRow row in gv_Children.Rows)
            {
                ((CheckBox)row.FindControl("chkUseChildren")).Checked = false;
            }


            SetVisibility_Children(false);
            lblChildrenMessage.Text = "Kinder wurden hinzugefügt";
        }

        private void SetVisibility_Children(bool visibility)
        {
            gv_Children.Visible = visibility;
            btnAddChildren.Visible = visibility;
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }
    }
}