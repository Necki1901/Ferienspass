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
    public partial class Admin_Project_View_Add : System.Web.UI.Page
    {
        CsharpDB db;

        protected void Page_Load(object sender, EventArgs e)
        {

            // PRIVILLEGE CHECK
            int ug = CsharpDB.GetUserGroup(Session["usergroup"]);
            if (ug != 0)
            {
                Response.Redirect("NotPermittedPage.html");
            }

            db = new CsharpDB();
            lbl_Message.Text = string.Empty;
            if (!Page.IsPostBack)
            {
                ViewState["project"] = Request.QueryString["project"];
                Fill_Gridview();
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

        private void Fill_Gridview()
        {
            DataTable dt = db.Query($"SELECT * FROM user");

            if (dt.Rows.Count == 0)
                lbl_Message.Text = "Keine Anmeldungen vorhanden";
            DataView dv = new DataView(dt);

            gv_add_child.DataSource = dv;
            gv_add_child.DataBind();
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }

        protected void gv_add_child_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_children");
                string user_id = ((Label)e.Row.FindControl("lbl_user_id")).Text;

                string project = Convert.ToString(ViewState["project"]);
                DataTable dt = db.Query($"select * from child WHERE UID = {user_id} AND CID NOT IN(Select CID from participation WHERE PID <> {project})");

                List <Child> children = new List<Child>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    children.Add(new Child(Convert.ToInt32(dt.Rows[i]["CID"]),dt.Rows[i]["GN"].ToString(), dt.Rows[i]["SN"].ToString(), Convert.ToDateTime(dt.Rows[i]["BD"])));
                }

                for(int j = 0; j< children.Count; j++)
                {
                    ListItem item = new ListItem($"{children[j].Surname} {children[j].Givenname}", children[j].ChildID.ToString());
                    if (j == 0)
                        item.Selected = true;
                    ddl.Items.Add(item);

                }

              
                DropDownList ddl_paid = (DropDownList)e.Row.FindControl("ddl_paid");

                ddl_paid.Items.Add(new ListItem("Nein", "0"));
                ddl_paid.Items.Add(new ListItem("Ja", "1"));

            }
        }

        protected void gv_add_child_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "save":
                    GridViewRow gvr = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                    int rowIndex = gvr.RowIndex;

                    string project_id = Convert.ToString(ViewState["project"]);
                    string user_id = ((Label)gv_add_child.Rows[rowIndex].FindControl("lbl_user_id")).Text;
                 
                    DropDownList ddl_child = ((DropDownList)gv_add_child.Rows[rowIndex].FindControl("ddl_children"));

                    string child_id = string.Empty;
                    if(ddl_child.SelectedItem == null)
                    {
                        lbl_Message.Text = "Einstellungen konnten nicht übernommen werden";
                        break;

                    }
                    else
                        child_id = ddl_child.SelectedItem.Value;


                    DropDownList ddl_paid = ((DropDownList)gv_add_child.Rows[rowIndex].FindControl("ddl_paid"));
                    string paid = ddl_paid.SelectedItem.Value;

                    db.Query($"INSERT INTO participation (CID, PID, DATE, paid) VALUES({child_id}, '{project_id}', '{DateTime.Now.ToString("yyyy/MM/dd")}', {paid})");
                    lbl_Message.Text = $"Kind erfolgreich für das Projekt angemeldet";
                    break;
            }
        }
    }
}