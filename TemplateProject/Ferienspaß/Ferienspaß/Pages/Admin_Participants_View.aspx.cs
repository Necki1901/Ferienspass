﻿using System;
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
        static bool isSorted = false;
        static string sortExpression = null;

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
            lbl_projectname.Text = ddl_Projects.SelectedItem.Text;

        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }

        protected void ddl_Projects_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fill_Gv_Participants();
            lbl_projectname.Text = ddl_Projects.SelectedItem.Text;
        }

        private void Fill_Gv_Participants()
        {
            if (!isSorted)
            {
                DataTable dt = db.Query($"SELECT child.GN, child.SN, child.BD, participation.paid, participation.CID, user.PHONE " +
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
            else gv_Participants_Sort();
        }


        protected void ddl_Years_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fill_ddl_Projects();
            Fill_Gv_Participants();
        }

        protected void gv_Participants_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = gv_Participants.Rows[e.RowIndex];

            string childID = ((Label)row.FindControl("lblChildID")).Text;
            db.Query($"delete from participation where CID = {childID}");

            lbl_Message.Text = "Datensatz gelöscht";
            Fill_Gv_Participants();
        }

        protected void gv_Participants_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gv_Participants.EditIndex = e.NewEditIndex;
            Fill_Gv_Participants();
        }

        protected void gv_Participants_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gv_Participants.Rows[e.RowIndex];     //Reihe erfassen

            string paid = ((DropDownList)row.FindControl("ddl_Paid")).SelectedValue;
            string id = ((Label)(row.FindControl("lblChildID"))).Text;

            db.Query($"UPDATE participation SET paid = {paid} WHERE CID = {id}");


            lbl_Message.Text = "Bearbeitung abgeschlossen";
            ((Button)gv_Participants.HeaderRow.FindControl("btn_add_participation")).Enabled = true;
            gv_Participants.EditIndex = -1;
            gv_Participants.SelectedIndex = -1;
            Fill_Gv_Participants();
        }

        public static string ChangePaidType(int paid)
        {
            if (paid == 1)
                return "Ja";
            return "Nein";
        }

        protected void gv_Participants_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow && gv_Participants.EditIndex == e.Row.RowIndex)
            {
                Control ctrl = e.Row.FindControl("ddl_Paid");
                string id = ((Label)e.Row.FindControl("lblChildID")).Text;

                DropDownList ddl = ctrl as DropDownList;
                DataTable dt = db.Query($"SELECT paid FROM participation WHERE CID = {id}");

                int paid = (int)dt.Rows[0]["paid"];

                ListItem item_no = new ListItem("Nein", "0");
                ListItem item_yes = new ListItem("Ja", "1");

                if (paid == 0)
                    item_no.Selected = true;
                else
                    item_yes.Selected = true;

                ddl.Items.Add(item_no);
                ddl.Items.Add(item_yes);

                DataRowView dr = e.Row.DataItem as DataRowView;
            }
        }

        protected void gv_Participants_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "add":
                    string project = ddl_Projects.SelectedValue;

                    Response.Redirect($"Admin_Participants_Add.aspx?project={project}");
                    break;
            }
        }

        protected void gv_Participants_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gv_Participants.EditIndex = -1;
            gv_Participants.SelectedIndex = -1;
            Fill_Gv_Participants();
        }

        protected void print_Click(object sender, EventArgs e)
        {
            Response.Redirect($"Admin_Participants_Print.aspx?project={ddl_Projects.SelectedItem.Value}");
        }

        protected void gv_Participants_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sql = $"SELECT child.GN, child.SN, child.BD, participation.paid, participation.CID, user.PHONE FROM child INNER JOIN participation " +
                $"ON child.CID = participation.CID INNER JOIN user ON user.UID = child.UID WHERE participation.PID = {ddl_Projects.SelectedValue}";
            DataTable dt = db.Query(sql);
            if (dt != null)
            {
                ViewState["sortCounter"] = Convert.ToInt32(ViewState["sortCounter"]) + 1;
                DataView dv = new DataView(dt);
                if (Convert.ToInt32(ViewState["sortCounter"]) % 2 == 0)
                {
                    e.SortDirection = SortDirection.Ascending;
                }
                else
                {
                    e.SortDirection = SortDirection.Descending;
                }
                sortExpression = ConvertSortDirectionToSql(e.SortDirection);
                dv.Sort = e.SortExpression + " " + sortExpression;
                sortExpression = " ORDER BY " + e.SortExpression + " " + sortExpression;
                gv_Participants.DataSource = dv;
                gv_Participants.DataBind();
                isSorted = true;
            }
        }

        protected void gv_Participants_Sort()
        {
            string sql = $"SELECT child.GN, child.SN, child.BD, participation.paid, participation.CID, user.PHONE FROM child INNER JOIN participation " +
                $"ON child.CID = participation.CID INNER JOIN user ON user.UID = child.UID WHERE participation.PID = {ddl_Projects.SelectedValue}";
            sql += sortExpression;

            DataTable dt = db.Query(sql);
            DataView dv = new DataView(dt);
            gv_Participants.DataSource = dv;
            gv_Participants.DataBind();
        }

        private string ConvertSortDirectionToSql(SortDirection sortDirection)
        {
            string newSortDirection = String.Empty;

            switch (sortDirection)
            {
                case SortDirection.Ascending:
                    newSortDirection = "ASC";
                    break;

                case SortDirection.Descending:
                    newSortDirection = "DESC";
                    break;
            }

            return newSortDirection;
        }

        protected void gv_Participants_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_Participants.PageIndex = e.NewPageIndex;
            Fill_Gv_Participants();
        }
    }
    

   
}