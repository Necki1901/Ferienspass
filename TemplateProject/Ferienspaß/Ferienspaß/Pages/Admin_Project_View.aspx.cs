﻿using System;
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
        static int idForUpdating;
        protected void Page_Load(object sender, EventArgs e)
        {            
            lblInfo.Text = "";
            
            Fill_gvAdminProjects();
            if(!Page.IsPostBack)
            {
                isAdding = false;
                Fillddl();
                Fill_ddlGuide3();
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
            string sql = "SELECT project.PID, project.DATE, project.START, project.END, project.NAME, project.DESCRIPTION, project.PLACE, project.NUMBER, project.CAPACITY, projectguide.GID, projectguide.GN, projectguide.SN  FROM project INNER JOIN projectguide ON project.GID = projectguide.GID";
            bool filter = false;

            if ((txtEventName.Text != "" || datepicker.Text != "" || ddlGuide3.SelectedValue != "Alle") && isFiltered == true)
            {
                sql += " Having ";

                if (txtEventName.Text != "")
                {
                    sql += $"project.Name LIKE '{txtEventName.Text}%'";
                    filter = true;
                }
                if (datepicker.Text != "")
                {
                    if (filter) sql += " AND "; else filter = true;
                    sql += $"project.DATE='{datepicker.Text}'";
                }
                if (ddlGuide3.SelectedValue != "Alle")
                {
                    if (filter) sql += " AND ";
                    sql += $"projectguide.GID = {ddlGuide3.SelectedValue}";
                }
            }
            DataTable dt = db.Query(sql);
            DataView dv = new DataView(dt);
            gvAdminProjects.DataSource = dv;
            gvAdminProjects.DataBind();
        }

        public void Fill_ddlGuide3()
        {
            CsharpDB db = new CsharpDB();
            List<int> usergroups = new List<int>();
            DataTable dt = GetAllGuides();
            ddlGuide3.Items.Add(new ListItem("Alle"));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddlGuide3.Items.Add(new ListItem(dt.Rows[i]["SN"].ToString(), dt.Rows[i]["GID"].ToString()));
            }
            ddlGuide.SelectedValue = "Alle";
        }

        protected void btnEdit_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void gvAdminProjects_RowEditing(object sender, GridViewEditEventArgs e)
        {
            pnlBlockBg.Visible = true;
            pnlUpdate.Visible = true;
            idForUpdating = e.NewEditIndex;
            FillControlsWithValues();         
        }

        private void FillControlsWithValues()
        {
            int id;
            id = Convert.ToInt32(((Label)gvAdminProjects.Rows[idForUpdating].FindControl("lblItemTemplateProjectID")).Text);
            DataTable dt = db.Query("SELECT * FROM project WHERE PID=?", id);
            txtCapacity2.Text = dt.Rows[0]["CAPACITY"].ToString();
            txtName2.Text = dt.Rows[0]["NAME"].ToString();
            txtStart2.Text = dt.Rows[0]["START"].ToString();
            txtEnd2.Text = dt.Rows[0]["END"].ToString();
            txtDesc2.Text = dt.Rows[0]["DESCRIPTION"].ToString();
            txtPlace2.Text = dt.Rows[0]["PLACE"].ToString();
            txtNumber2.Text = dt.Rows[0]["NUMBER"].ToString();
            txtDate2.Text = Convert.ToDateTime(dt.Rows[0]["DATE"]).ToString("yyyy/MM/dd");                       
            ddlGuide2.SelectedValue = dt.Rows[0]["GID"].ToString();//Garantiert, dass der richtige wert ausgewählt ist, aus den werten die zu beginn in die DDl geschrieben wurden (FillDDL)

        }

        private void Fillddl()
        {
            DataTable dt2 = GetAllGuides();
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                ListItem li = new ListItem(dt2.Rows[i]["SN"].ToString(), dt2.Rows[i]["GID"].ToString());
                ddlGuide2.Items.Add(li);
                ddlGuide.Items.Add(li);
            }

            ddlGuide2.DataValueField = "GID";
            ddlGuide2.DataTextField = "SN";
            ddlGuide.DataValueField = "GID";
            ddlGuide.DataTextField = "SN";

        }
      
        private DataTable GetAllGuides()
        {
            return db.Query("SELECT SN, GID FROM projectguide");
        }
           
        private string ChangeDateFormat()
        {
            string oldDate = txtDate.Text;
            string newDate = oldDate.Replace(".", "-");
            return newDate;

        }

        private string ChangeDateFormat2()
        {
            string oldDate = txtDate2.Text;
            string newDate = oldDate.Replace(".", "-");
            return newDate;

        }

        private bool ValidateData()
        {
            string errorDescription = "";
            bool valid = true;
            bool checkifinteger = true;

            //Proof null-values
            if (txtDate.Text == "" || txtStart.Text == "" || txtEnd.Text == "" || txtCapacity.Text == "" || txtName.Text == "" || txtDesc.Text == "" || txtPlace.Text == "") { valid = false; errorDescription += "Einer oder mehrere der Werte sind leer!  "; }
            else
            {               
                 txtDate.Text = ChangeDateFormat();

                if (!(txtDate.Text.Length == 10) || !(txtStart.Text.Length == 8) || !(txtEnd.Text.Length == 8)) { valid = false; errorDescription += "DATUM-Format oder ZEIT-Format (START oder END) ist ungültig!  "; }
                else
                {
                    //Proof Date
                    for (int i = 0; i < txtDate.Text.Length; i++)
                    {
                        if (i != 4 && i != 7)
                        {
                            if (!(int.TryParse(txtDate.Text[i].ToString(), out int n)))
                            {
                                checkifinteger = false;
                            }
                        }
                    }



                    if (!(txtDate.Text[4] == '-' && txtDate.Text[7] == '-' && checkifinteger == true)) { valid = false; errorDescription += "DATE-Format ist ungültig!  "; }

                    //Proof Time
                    bool checkifintegerStartTime = true;
                    bool checkifintegerEndTime = true;

                    for (int i = 0; i < txtStart.Text.Length; i++)
                    {
                        if (i != 2 && i != 5)
                        {
                            if (!(int.TryParse(txtStart.Text[i].ToString(), out int n)))
                            {
                                checkifintegerStartTime = false;
                            }
                        }
                    }

                    for (int i = 0; i < txtEnd.Text.Length; i++)
                    {
                        if (i != 2 && i != 5)
                        {
                            if (!(int.TryParse(txtEnd.Text[i].ToString(), out int n)))
                            {
                                checkifintegerEndTime = false;
                            }
                        }
                    }

                    if (!(txtStart.Text[2] == ':' && txtStart.Text[5] == ':' && checkifintegerStartTime == true)) { valid = false; errorDescription += "START-Zeit-Format ist ungültig!  "; }
                    if (!(txtEnd.Text.ToString()[2] == ':' && txtEnd.Text[5] == ':' && checkifintegerEndTime == true)) { valid = false; errorDescription += "END-Zeit-Format ist ungültig!  "; }

                    //Proof Capacity

                    if (!int.TryParse((txtCapacity.Text), out int a)) { valid = false; errorDescription += "KAPAZITÄT-Format ist ungültig!  "; }

                    //Proof lengths of other VARCHAR attributes

                    if (!(txtName.Text.Length <= 50)) { valid = false; errorDescription += "NAME ist zu lang!  "; }
                    if (!(txtDesc.Text.Length <= 140)) { valid = false; errorDescription += "BESCHREIBUNG ist zu lang!!  "; }
                    if (!(txtPlace.Text.Length <= 50)) { valid = false; errorDescription += "ORT ist zu lang!  "; }
                    if (!(txtNumber.Text.Length <= 5)) { valid = false; errorDescription += "HAUSNUMMER ist zu lang!  "; }

                }
            }
            lblInfo.Text = errorDescription;//auf Label in Panel ändern!
            return valid;
        }
        private bool ValidateData2()
        {
            string errorDescription = "";
            bool valid = true;
            bool checkifinteger = true;

            //Proof null-values
            if (txtDate2.Text == "" || txtStart2.Text == "" || txtEnd2.Text == "" || txtCapacity2.Text == "" || txtName2.Text == "" || txtDesc2.Text == "" || txtPlace2.Text == "") { valid = false; errorDescription += "Einer oder mehrere der Werte sind leer!  "; }
            else
            {
                txtDate2.Text = ChangeDateFormat2();

                if (!(txtDate2.Text.Length == 10) || !(txtStart2.Text.Length == 8) || !(txtEnd2.Text.Length == 8)) { valid = false; errorDescription += "DATUM-Format oder ZEIT-Format (START oder END) ist ungültig!  "; }
                else
                {
                    //Proof Date
                    for (int i = 0; i < txtDate2.Text.Length; i++)
                    {
                        if (i != 4 && i != 7)
                        {
                            if (!(int.TryParse(txtDate2.Text[i].ToString(), out int n)))
                            {
                                checkifinteger = false;
                            }
                        }
                    }


                    if (!(txtDate2.Text[4] == '-' && txtDate2.Text[7] == '-' && checkifinteger == true)) { valid = false; errorDescription += "DATE-Format ist ungültig!  "; }

                    //Proof Time
                    bool checkifintegerStartTime = true;
                    bool checkifintegerEndTime = true;

                    for (int i = 0; i < txtStart2.Text.Length; i++)
                    {
                        if (i != 2 && i != 5)
                        {
                            if (!(int.TryParse(txtStart2.Text[i].ToString(), out int n)))
                            {
                                checkifintegerStartTime = false;
                            }
                        }
                    }

                    for (int i = 0; i < txtEnd2.Text.Length; i++)
                    {
                        if (i != 2 && i != 5)
                        {
                            if (!(int.TryParse(txtEnd2.Text[i].ToString(), out int n)))
                            {
                                checkifintegerEndTime = false;
                            }
                        }
                    }

                    if (!(txtStart2.Text[2] == ':' && txtStart2.Text[5] == ':' && checkifintegerStartTime == true)) { valid = false; errorDescription += "START-Zeit-Format ist ungültig!  "; }
                    if (!(txtEnd2.Text.ToString()[2] == ':' && txtEnd2.Text[5] == ':' && checkifintegerEndTime == true)) { valid = false; errorDescription += "END-Zeit-Format ist ungültig!  "; }

                    //Proof Capacity

                    if (!int.TryParse((txtCapacity2.Text), out int a)) { valid = false; errorDescription += "KAPAZITÄT-Format ist ungültig!  "; }

                    //Proof lengths of other VARCHAR attributes

                    if (!(txtName2.Text.Length <= 50)) { valid = false; errorDescription += "NAME ist zu lang!  "; }
                    if (!(txtDesc2.Text.Length <= 140)) { valid = false; errorDescription += "BESCHREIBUNG ist zu lang!!  "; }
                    if (!(txtPlace2.Text.Length <= 50)) { valid = false; errorDescription += "ORT ist zu lang!  "; }
                    if (!(txtNumber2.Text.Length <= 5)) { valid = false; errorDescription += "HAUSNUMMER ist zu lang!  "; }

                }
            }
            lblInfo.Text = errorDescription;//auf Label in Panel ändern!
            return valid;
        }
     

        protected void gvAdminProjects_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {

        }

        protected void gvAdminProjects_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = gvAdminProjects.Rows[e.RowIndex];
            string projectID = ((Label)row.FindControl("lblItemTemplateProjectID")).Text;
            db.Query($"delete from project where PID = {projectID}");

            Fill_gvAdminProjects();
            lblInfo.Text += "Datensatz wurde gelöscht!";
        }

        protected void gvAdminProjects_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Add")
            {
                ViewState["isAdding"] = true;
                pnlBlockBg.Visible = true;
                pnlInsert.Visible = true;
                //DataTable dt =  GetAllGuides();
                //for(int i=0; i<dt.Rows.Count;i++)
                //{
                //    ListItem li = new ListItem(dt.Rows[i]["SN"].ToString(), dt.Rows[i]["GID"].ToString());
                //    ddlGuide.Items.Add(li);
                //}

                //ddlGuide.DataValueField = "GID";
                //ddlGuide.DataTextField = "SN";              
            }
        }

        private string GetTextForDdl(int svalue)//unused
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
            Fill_gvAdminProjects();
        }

        protected void gvAdminProjects_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAdminProjects.PageIndex = e.NewPageIndex;
            Fill_gvAdminProjects();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            pnlBlockBg.Visible = false;
            pnlInsert.Visible = false;
        }

        protected void btnAdd_Click1(object sender, EventArgs e)//Insert from popup panel
        {
            if (Convert.ToBoolean(ViewState["isAdding"]) == true)
            {
                string selectedname = ddlGuide.SelectedValue;
                bool valid = ValidateData();
                if (valid == true)
                {
                    if (db.ExecuteNonQuery("INSERT INTO project (NAME, DESCRIPTION, DATE, START, END, PLACE, NUMBER, CAPACITY, GID) Values(?,?,?,?,?,?,?,?,?)", txtName.Text, txtDesc.Text, Convert.ToDateTime(txtDate.Text), Convert.ToDateTime(txtStart.Text).ToString("HH:mm:ss"), Convert.ToDateTime(txtEnd.Text).ToString("HH:mm:ss"), txtPlace.Text, txtNumber.Text, Convert.ToInt32(txtCapacity.Text), Convert.ToInt32(selectedname)) > 0)//Keine Newvalues mehr sondern Bootstrap pop up
                    {
                        lblInfo.Text = $"<span class='success'> Datensatz hinzugefügt! </span>";
                    }
                    else
                    {
                        lblInfo.Text = $"<span class='error'> Nichts passiert! </span>";
                    }
                    Fill_gvAdminProjects();
                    ViewState["isAdding"] = false;
                    pnlBlockBg.Visible = false;
                    pnlInsert.Visible = false;
                }
            }

        }

        protected void btnUpdate_Click(object sender, EventArgs e)//Update from Popup Panel
        {
            if (Convert.ToBoolean(ViewState["isAdding"]) == false)
            {
                string selectedname = ddlGuide2.SelectedValue;
                bool valid = ValidateData2();
                int id;

                id = Convert.ToInt32(((Label)gvAdminProjects.Rows[idForUpdating].FindControl("lblItemTemplateProjectID")).Text);

                if (valid == true)
                {
                    if (db.ExecuteNonQuery("Update project SET NAME=?, DESCRIPTION=?, DATE=?, START=?, END=?, PLACE=?, NUMBER=?, CAPACITY=?, GID=? WHERE PID=?", txtName2.Text, txtDesc2.Text, Convert.ToDateTime(txtDate2.Text).ToShortDateString(), Convert.ToDateTime(txtStart2.Text).ToString("HH:mm:ss"), Convert.ToDateTime(txtEnd2.Text).ToString("HH:mm:ss"), txtPlace2.Text, txtNumber2.Text, Convert.ToInt32(txtCapacity2.Text), Convert.ToInt32(selectedname), id) > 0)//Keine Newvalues mehr sondern Bootstrap pop up
                    {
                        //lblInfo2.Text = $"<span class='success'> Datensatz geändert! </span>";
                    }
                    else
                    {
                        //lblInfo2.Text = $"<span class='error'> Nichts passiert! </span>";
                    }

                    gvAdminProjects.EditIndex = -1;
                    Fill_gvAdminProjects();
                    pnlBlockBg.Visible = false;
                    pnlUpdate.Visible = false;
                    gvAdminProjects.DataBind();
                    
                }
            }
        }

        protected void btnBack2_Click(object sender, EventArgs e)
        {
            pnlBlockBg.Visible = false;
            pnlUpdate.Visible = false;
            gvAdminProjects.EditIndex = -1;
            Fill_gvAdminProjects();
            gvAdminProjects.DataBind();
        }

        protected void gvAdminProjects_Sorting(object sender, GridViewSortEventArgs e)
        {
            //gvAdminProjects.
        }
    }
}