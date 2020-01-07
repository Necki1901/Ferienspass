using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Ferienspaß.Pages
{
    public partial class Admin : System.Web.UI.Page
    {
        CsharpDB db = new CsharpDB();
        bool isAdding;
        static bool isFiltered = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            lblInfo.Text = "";
            Fill_gvAdminProjects(isFiltered);
            if(!Page.IsPostBack)
            {
                isAdding = false;
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

        private void Fill_gvAdminProjects(bool filter)
        {
            DataTable dt;
            DataView dv;
            string sql;
            bool changed = false;
            if ((txtEventName.Text != string.Empty || datepicker.Text != string.Empty || txtOrganizerName.Text != string.Empty || txtOrganizerName.Text != string.Empty) && filter == true)
            {
                sql = "SELECT project.PID, project.DATE, project.START, project.END, project.NAME, project.DESCRIPTION, project.PLACE, project.NUMBER, project.CAPACITY, projectguide.GID, projectguide.GN, projectguide.SN  FROM project INNER JOIN projectguide ON project.GID = projectguide.GID WHERE ";

                if (txtEventName.Text != string.Empty)
                {
                    sql += $"project.Name LIKE '%{txtEventName.Text}%'";
                    changed = true;
                }
                if (datepicker.Text != string.Empty)
                {
                    if (changed) sql += " AND ";
                    sql += $"project.DATE='{datepicker.Text}'";
                }
                if (txtOrganizerName.Text != string.Empty)
                {
                    if (changed) sql += " AND ";
                    sql += $"projectguide.SN LIKE '%{txtOrganizerName.Text}%'";
                }
            }
            else
            {
                sql = "SELECT project.PID, project.DATE, project.START, project.END, project.NAME, project.DESCRIPTION, project.PLACE, project.NUMBER, project.CAPACITY, projectguide.GID, projectguide.GN, projectguide.SN  FROM project INNER JOIN projectguide ON project.GID = projectguide.GID";
            }
            dt = db.Query(sql);
            dv = new DataView(dt);

            gvAdminProjects.DataSource = dv;
            gvAdminProjects.DataBind();
        }

        protected void btnEdit_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void gvAdminProjects_RowEditing(object sender, GridViewEditEventArgs e)
        {            
            gvAdminProjects.EditIndex = e.NewEditIndex;          
            Fill_gvAdminProjects(isFiltered);          
        }

        //protected void gvAdminProjects_RowDataBound(object sender, GridViewRowEventArgs e)
        //{

        //}

        private DataTable GetAllGuides()
        {
            return db.Query("SELECT SN, GID FROM projectguide");
        }

        protected void gvAdminProjects_RowDataBound1(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && gvAdminProjects.EditIndex == e.Row.RowIndex)
            {
                Control ctrl = e.Row.FindControl("ddlEditItemTemplateProjectGuide");

                DropDownList ddl = ctrl as DropDownList;
                DataTable dt = GetAllGuides();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //ddl.Items.Add(Convert.ToString(dt.Rows[i].ItemArray[0]));
                    ddl.Items.Add(new ListItem(dt.Rows[i]["SN"].ToString(), dt.Rows[i]["GID"].ToString()));
                }
                DataRowView dr = e.Row.DataItem as DataRowView;

                ddl.SelectedValue = dr["GID"].ToString();
                ddl.SelectedItem.Text = dr["SN"].ToString();
            }
        }

        protected void gvAdminProjects_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow gvr = gvAdminProjects.Rows[e.RowIndex];
            DropDownList dropDownList = (DropDownList)gvr.FindControl("ddlEditItemTemplateProjectGuide");
            string selectedname = dropDownList.SelectedValue;

            if (Convert.ToBoolean(ViewState["isAdding"]) == false)
            {
                bool valid = ValidateData(e);
                if (valid == true)
                {
                    if (db.ExecuteNonQuery("UPDATE project SET NAME = ?, DESCRIPTION = ?, DATE = ?, START = ?, END = ?, PLACE = ?, NUMBER = ?, CAPACITY = ?, GID = ? WHERE PID = ?", e.NewValues["NAME"], e.NewValues["DESCRIPTION"], Convert.ToDateTime(e.NewValues["DATE"]), Convert.ToDateTime(e.NewValues["START"]).ToString("HH:mm:ss"), Convert.ToDateTime(e.NewValues["END"]).ToString("HH:mm:ss"), e.NewValues["PLACE"], e.NewValues["NUMBER"], Convert.ToInt32(e.NewValues["CAPACITY"]), Convert.ToInt32(selectedname), e.Keys[0]) > 0)
                    {
                        lblInfo.Text = $"<span class='success'> Datensatz aktualisiert! </span>";
                    }
                    else
                    {
                        lblInfo.Text = $"<span class='error'> Nichts passiert! </span>";
                    }
                    gvAdminProjects.EditIndex = -1;
                    Fill_gvAdminProjects(isFiltered);
                }

            }
            else
            {
                bool valid = ValidateData(e);
                if (valid == true)
                {
                    if (db.ExecuteNonQuery("INSERT INTO project (NAME, DESCRIPTION, DATE, START, END, PLACE, NUMBER, CAPACITY, GID) Values(?,?,?,?,?,?,?,?,?)", e.NewValues["NAME"], e.NewValues["DESCRIPTION"], Convert.ToDateTime(e.NewValues["DATE"]).ToShortDateString(), Convert.ToDateTime(e.NewValues["START"]).ToString("HH:mm:ss"), Convert.ToDateTime(e.NewValues["END"]).ToString("HH:mm:ss"), e.NewValues["PLACE"], Convert.ToInt32(e.NewValues["NUMBER"]), Convert.ToInt32(e.NewValues["CAPACITY"]), Convert.ToInt32(selectedname)) > 0)
                    {
                        lblInfo.Text = $"<span class='success'> Datensatz hinzugefügt! </span>";
                    }
                    else
                    {
                        lblInfo.Text = $"<span class='error'> Nichts passiert! </span>";
                    }
                    gvAdminProjects.EditIndex = -1;
                    Fill_gvAdminProjects(isFiltered);
                    ViewState["isAdding"] = false;
                }
            }
        }

        private string ChangeDateFormat(GridViewUpdateEventArgs e)
        {
            string oldDate = e.NewValues["DATE"].ToString();
            string newDate = oldDate.Replace(".", "-");
            return newDate;

        }

        private bool ValidateData(GridViewUpdateEventArgs e)
        {
            string errorDescription = "";
            bool valid = true;
            bool checkifinteger = true;

            //Proof null-values
            if (e.NewValues["DATE"] == null || e.NewValues["START"] == null || e.NewValues["END"] == null || e.NewValues["CAPACITY"] == null || e.NewValues["NAME"] == null || e.NewValues["DESCRIPTION"] == null || e.NewValues["PLACE"] == null || e.NewValues["NUMBER"] == null) { valid = false; errorDescription += "Einer oder mehrere der Werte sind null!  "; }
            else
            {
                e.NewValues["DATE"] = ChangeDateFormat(e);

                if (!(e.NewValues["DATE"].ToString().Length == 10) || !(e.NewValues["START"].ToString().Length == 8) || !(e.NewValues["END"].ToString().Length == 8)) { valid = false; errorDescription += "DATUM-Format oder ZEIT-Format (START oder END) ist ungültig!  "; }
                else
                {
                    //Proof Date
                    for (int i = 0; i < e.NewValues["DATE"].ToString().Length; i++)
                    {
                        if (i != 4 && i != 7)
                        {
                            if (!(int.TryParse(e.NewValues["DATE"].ToString()[i].ToString(), out int n)))
                            {
                                checkifinteger = false;
                            }
                        }
                    }


                    if (!(e.NewValues["DATE"].ToString()[4] == '-' && e.NewValues["DATE"].ToString()[7] == '-' && checkifinteger == true)) { valid = false; errorDescription += "DATE-Format ist ungültig!  "; }

                    //Proof Time
                    bool checkifintegerStartTime = true;
                    bool checkifintegerEndTime = true;

                    for (int i = 0; i < e.NewValues["START"].ToString().Length; i++)
                    {
                        if (i != 2 && i != 5)
                        {
                            if (!(int.TryParse(e.NewValues["START"].ToString()[i].ToString(), out int n)))
                            {
                                checkifintegerStartTime = false;
                            }
                        }
                    }

                    for (int i = 0; i < e.NewValues["END"].ToString().Length; i++)
                    {
                        if (i != 2 && i != 5)
                        {
                            if (!(int.TryParse(e.NewValues["END"].ToString()[i].ToString(), out int n)))
                            {
                                checkifintegerEndTime = false;
                            }
                        }
                    }

                    if (!(e.NewValues["START"].ToString()[2] == ':' && e.NewValues["START"].ToString()[5] == ':' && checkifintegerStartTime == true)) { valid = false; errorDescription += "START-Zeit-Format ist ungültig!  "; }
                    if (!(e.NewValues["END"].ToString()[2] == ':' && e.NewValues["END"].ToString()[5] == ':' && checkifintegerEndTime == true)) { valid = false; errorDescription += "END-Zeit-Format ist ungültig!  "; }

                    //Proof Capacity

                    if (!int.TryParse((e.NewValues["CAPACITY"].ToString()), out int a)) { valid = false; errorDescription += "KAPAZITÄT-Format ist ungültig!  "; }

                    //Proof lengths of other VARCHAR attributes

                    if (!(e.NewValues["NAME"].ToString().Length <= 50)) { valid = false; errorDescription += "NAME ist zu lang!  "; }
                    if (!(e.NewValues["DESCRIPTION"].ToString().Length <= 140)) { valid = false; errorDescription += "BESCHREIBUNG ist zu lang!!  "; }
                    if (!(e.NewValues["PLACE"].ToString().Length <= 50)) { valid = false; errorDescription += "ORT ist zu lang!  "; }
                    if (!(e.NewValues["NUMBER"].ToString().Length <= 5)) { valid = false; errorDescription += "HAUSNUMMER ist zu lang!  "; }

                }
            }
            lblInfo.Text = errorDescription;
            return valid;
        }

        protected void gvAdminProjects_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvAdminProjects.EditIndex = -1;
            Fill_gvAdminProjects(isFiltered);
            lblInfo.Text = "";            
        }

        protected void gvAdminProjects_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {

        }

        protected void gvAdminProjects_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = gvAdminProjects.Rows[e.RowIndex];
            string projectID = ((Label)row.FindControl("lblItemTemplateProjectID")).Text;
            db.Query($"delete from project where PID = {projectID}");

            Fill_gvAdminProjects(isFiltered);
            lblInfo.Text += "Datensatz wurde gelöscht!";
        }

        protected void gvAdminProjects_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Add")
            {

                DataTable dt = db.Query("SELECT project.PID, project.DATE, project.START, project.END, project.NAME, project.DESCRIPTION, project.PLACE, project.NUMBER, project.CAPACITY, projectguide.GID, projectguide.GN, projectguide.SN  FROM project INNER JOIN projectguide ON project.GID = projectguide.GID ");
                dt.Clear();
                DataRow dr = dt.NewRow();
                ViewState["isAdding"] = true;
                dt.Rows.Add(dr);
                gvAdminProjects.DataSource = dt;
                gvAdminProjects.EditIndex = 0;
                gvAdminProjects.DataBind();
                GridViewRow gvr = gvAdminProjects.Rows[gvAdminProjects.EditIndex];
                DropDownList ddl = gvr.FindControl("ddlEditItemTemplateProjectGuide") as DropDownList;
                ddl.SelectedItem.Text = GetTextForDdl(Convert.ToInt32(ddl.SelectedValue));
                ImageButton ib = gvr.FindControl("btnDelete") as ImageButton;
                ib.Visible = false;
            }
        }

        private string GetTextForDdl(int svalue)
        {
            DataTable dt = db.Query("SELECT SN FROM projectguide WHERE GID = ?", svalue);
            return dt.Rows[0]["SN"].ToString();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {

        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            isFiltered = true;
            Fill_gvAdminProjects(isFiltered);
        }

        protected void gvAdminProjects_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAdminProjects.PageIndex = e.NewPageIndex;
            Fill_gvAdminProjects(isFiltered);
        }
    }
}