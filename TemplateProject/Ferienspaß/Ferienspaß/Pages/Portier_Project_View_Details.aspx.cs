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
    public partial class Portier_Project_View_Details : System.Web.UI.Page
    {
        private int projectid;
        CsharpDB db = new CsharpDB();

        protected void Page_Load(object sender, EventArgs e)
        {
            // PRIVILLEGE CHECK
            int ug = CsharpDB.GetUserGroup(Session["usergroup"]);
            if (ug != 0 && ug != 3)
            {
                Response.Redirect("NotPermittedPage.html");
            }

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["id"] == null)
                    Response.Redirect("Portier_Project_View.aspx");
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

            int remaining_capacity = (int)dt.Rows[0]["remainingCapacity"];
            ViewState["rc"] = remaining_capacity;

            gv_User_View_Details.DataSource = dv;
            gv_User_View_Details.DataBind();

        }


        protected void gv_User_View_Details_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "register":
                    SetVisibility_Children(true);
                    Fill_gv_Children();
                    break;
            }
        }
        private void Fill_gv_Children()
        {
            DataTable dt;
            DataView dv;

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

            if (childrenIDs.Count > Convert.ToInt32(ViewState["rc"]))
            {
                lblChildrenMessage.Text = $"Es können nicht {childrenIDs.Count} Kinder hinzugefügt werden,\n da das Projekt voll ist";
            }
            else
            {
                foreach (int id in childrenIDs)
                {
                    db.Query($"INSERT INTO participation (CID, PID, Date) VALUES({id}, {ViewState["PID"]}, '{DateTime.Now.ToString("dd/MM/yyyy")}')");
                }

                foreach (GridViewRow row in gv_Children.Rows)
                {
                    ((CheckBox)row.FindControl("chkUseChildren")).Checked = false;
                }


                SetVisibility_Children(false);
                lblChildrenMessage.Text = "Kinder wurden hinzugefügt";

                string body = Get_body_for_mail(childrenIDs);
                Send_mail(body);
            }
            Fill_gv_UserView_Details();

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
            foreach (int cid in childrenIDs)
            {
                DataTable dt_children = db.Query($"SELECT sn, gn FROM child WHERE cid = {cid}");
                body += $"{cnt}. {dt_children.Rows[0]["gn"]} {dt_children.Rows[0]["sn"]}\n";
                cnt++;
            }

            return body;
        }

        private void Send_mail(string body)
        {
            //get data from the user
            DataTable dt_user = db.Query($"SELECT email, sn, gn FROM user WHERE uid = {User.Identity.Name}");
            string fullname = dt_user.Rows[0]["gn"].ToString() + dt_user.Rows[0]["sn"].ToString();

            //db.SendMail(dt_user.Rows[0]["email"].ToString(), fullname, "Projekt-Anmeldung", body);
            db.SendHTMLEmail((string)dt_user.Rows[0]["email"], fullname, db.GetPortalOption("MAIL_PROJECT_REGISTER_SUBJECT"), body, false, "", "", "", db.GetPortalOption("MAIL_GRUSSFORMEL"), db.GetPortalOption("MAIL_HINWEIS"));

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
