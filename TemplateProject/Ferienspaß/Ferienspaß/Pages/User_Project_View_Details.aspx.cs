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
                    Response.Redirect("User_Project_View.aspx");
                ViewState["PID"] = Convert.ToInt32(Request.QueryString["id"]);
                DisplayDiscountMessage();
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

        private void DisplayDiscountMessage()
        {
            int participationAmount = GetParticipationAmountForUser();
            string discountMessage = string.Empty;

            if(Convert.ToInt32(participationAmount) < 3)
            {
                discountMessage = $"Sie haben {participationAmount} Anmeldungen <br>" +
                    $"Ab der dritten Anmeldung wird Ihnen ein Rabatt von {GetDiscountAmount()} angerechnet";
            }
            else
            {
                discountMessage = $"Sie haben {participationAmount} Anmeldungen <br>" +
                    $"Für jede Weitere Anmeldung wird Ihnen ein Rabatt in der Höhe von {GetDiscountAmount()}% angerechnet";
            }

            lit_msg.Text = CreateMSGString(discountMessage, "warning");
           
        }

        private string GetDiscountAmount()
        {
            return (string)db.Query($"SELECT MyValue FROM settings WHERE MyKey = 'GLOBAL_DISCOUNT'").Rows[0]["MyValue"];
        }

        private int GetParticipationAmountForUser()
        {
            DataTable dt = db.Query("SELECT COUNT(*) uid FROM participation " +
                "INNER JOIN child ON participation.CID = child.CID WHERE uid = ?", User.Identity.Name);
            return Convert.ToInt32(dt.Rows[0]["uid"]);
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
            ViewState["rc"] = remaining_capacity;

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
                lit_msg.Text = CreateMSGString("Das Projekt ist leider ausgebucht<br>" +
                    "Um über eventuelle Verfügbarkeit informiert zu werden, können sie sich in die Warteschlange eintragen", "warning");
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
                        lit_msg.Text = CreateMSGString("Für dieses Projekt kann man sich nicht mehr anmelden", "warning");
                        break;
                    default:
                        lit_msg.Text = CreateMSGString("Für dieses Projekt kann man sich nur noch persönlich im Gemeindeamt anmelden", "warning");
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
                            lit_msg.Text = CreateMSGString("Sie wurden zur Warteschlange hinzugefügt", "success");
                        }
                        else
                        {
                            lit_msg.Text = CreateMSGString("Sie haben sich bereits in die Warteschlange eingetragen", "danger");
                        }
                    }
                    catch(Exception ex)
                    {
                        lit_msg.Text = CreateMSGString(ex.Message, "danger");
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
                lit_msg.Text = CreateMSGString("Sie können keine Kinder mehr auswählen", "danger");

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

            if(childrenIDs.Count > Convert.ToInt32(ViewState["rc"]))
            {
                lit_msg.Text = CreateMSGString($"Es können nicht {childrenIDs.Count} Kinder hinzugefügt werden, da das Projekt voll ist", "danger");
            }
            else
            {
                foreach(int id in childrenIDs)
                {
                    db.Query($"INSERT INTO participation (CID, PID, Date) VALUES({id}, {ViewState["PID"]}, '{DateTime.Now.ToString("yyyy/MM/dd")}')");
                }

                foreach (GridViewRow row in gv_Children.Rows)
                {
                    ((CheckBox)row.FindControl("chkUseChildren")).Checked = false;
                }


                SetVisibility_Children(false);

                string discountMessage = GetDiscountMessageWhenChildAdded();
                lit_msg.Text = CreateMSGString($"Kinder wurden hinzugefügt<br>{discountMessage}", "success");
                
                Send_mail();

                void Send_mail()
                {
                    int amountChildren = childrenIDs.Count;
                    string body = Get_body_for_mail(childrenIDs);

                    string pr = GeneratePaymentReference(amountChildren);
                    body = body + $"Bitte geben Sie folgende Zahlungsreferenz bei Ihrer Überweisung an: {pr}";
                    //get data from the user
                    DataTable dt_user = db.Query($"SELECT email, sn, gn FROM user WHERE uid = {User.Identity.Name}");
                    string fullname = dt_user.Rows[0]["gn"].ToString() + dt_user.Rows[0]["sn"].ToString();

                    //db.SendMail(dt_user.Rows[0]["email"].ToString(), fullname, "Projekt-Anmeldung", body);
                    db.SendHTMLEmail((string)dt_user.Rows[0]["email"], fullname, db.GetPortalOption("MAIL_PROJECT_REGISTER_SUBJECT"), body, false, "", "", "", db.GetPortalOption("MAIL_GRUSSFORMEL"), db.GetPortalOption("MAIL_HINWEIS"));


                }
            }



            Fill_gv_UserView_Details();
        }

        private string GetDiscountMessageWhenChildAdded()
        {

            int participationAmount = GetParticipationAmountForUser();
            string discountMessage = string.Empty;
            string discountAmount = GetDiscountAmount() + "%";

            if (participationAmount >= 3)
            {
                discountMessage = $"Ihnen wurde ein Rabatt von {discountAmount} für diese Anmeldung angerechnet";
            }
            else
            {
                int participationsLeftForDiscount = 3 - participationAmount;    //3 is the requirement for getting a discount
                discountMessage = $" Bei {participationsLeftForDiscount} weiteren Anmeldung/en wird Ihnen ein Rabatt von {discountAmount} angerechnet";
            }

            return discountMessage;
        }

        private string Get_body_for_mail(List<int> childrenIDs)
        {
            DataTable dt = db.Query($"SELECT * FROM project WHERE pid = {ViewState["PID"]}");
            string body = string.Empty;

            //project name and description
            body += $"Anmeldungen zum Projekt {dt.Rows[0]["name"].ToString()}\n";
            body += $"Beschreibung: {dt.Rows[0]["description"].ToString()}\n\n";

            int cnt = 1;
            //print children (1. xxx xxx)
            foreach(int cid in childrenIDs)
            {
                DataTable dt_children = db.Query($"SELECT sn, gn FROM child WHERE cid = {cid}");
                body += $"{cnt}. {dt_children.Rows[0]["gn"]} {dt_children.Rows[0]["sn"]}\n";
                cnt++;
            }
            return body;
        }

       

        private string GeneratePaymentReference(int amountchildren)
        {
            DateTime dt = DateTime.Now;
            string dts = dt.ToString().Replace(".","");
            string dts2 = dts.Replace(":", "");
            string dts3 = dts2.Replace(" ", "");
            string dts4 = dts3 + amountchildren.ToString();
            return dts4;
        }
        private string CreateMSGString(string msg, string type)
        {
            return "<div class=\"alert alert-" + type + " mt-3 mb-1\" role=\"alert\">" + msg + "</div>";
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