using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ferienspaß.Pages
{
    public partial class Admin_Clubs_View : System.Web.UI.Page
    {
        CsharpDB db = new CsharpDB();
        bool isAdding;
        //static bool isFiltered = false;
        static int idForUpdating;
        int sortCounter = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            lblInfo.Text = "";
            lblInfo2.Text = "";
            lblInfoBottom.Text = "";

            Fill_gvAdminClubs();
            if (!Page.IsPostBack)
            {
                isAdding = false;
                Fillddl();
               
            }

        }

        private void Fillddl()
        {
            DataTable dt = GetAllSpokesPersons();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ListItem li = new ListItem(dt.Rows[i]["SN"].ToString(), dt.Rows[i]["UID"].ToString());
                ddlSpokesPerson.Items.Add(li);
                ddlSpokesPerson2.Items.Add(li);
            }
            ddlSpokesPerson2.DataValueField = "GID";
            ddlSpokesPerson2.DataTextField = "SN";
            ddlSpokesPerson.DataValueField = "GID";
            ddlSpokesPerson.DataTextField = "SN";
        }

        private DataTable GetAllSpokesPersons()
        {
            return db.Query("SELECT SN, UID FROM user WHERE UGID=1");
        }

        private void Fill_gvAdminClubs()
        {
            string sql = "SELECT organisation.ORGID, organisation.NAME, organisation.STREET, organisation.NUMBER, organisation.DESCRIPTION, user.UID, user.GN, user.SN FROM organisation INNER JOIN user ON organisation.SPOKESPERSON = user.UID";
            DataTable dt = db.Query(sql);
            DataView dv = new DataView(dt);
            dv.Sort = "ORGID ASC";
            gvAdminClubs.DataSource = dv;
            gvAdminClubs.DataBind();
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            pnlBlockBg.Visible = false;
            pnlInsert.Visible = false;
        }

        protected void btnBack2_Click(object sender, EventArgs e)
        {
            pnlBlockBg.Visible = false;
            pnlUpdate.Visible = false;
            gvAdminClubs.EditIndex = -1;
            Fill_gvAdminClubs();
            gvAdminClubs.DataBind();
        }

        protected void btnAdd_Click1(object sender, EventArgs e)//Hinzufügen Button auf Insert Panel
        {
            if (Convert.ToBoolean(ViewState["isAdding"]) == true)
            {
                string selectedname = ddlSpokesPerson.SelectedValue;
               bool valid = ValidateData();
                if (valid == true)
                {
                    if (db.ExecuteNonQuery("INSERT INTO organisation (NAME, DESCRIPTION, STREET, NUMBER, SPOKESPERSON) Values(?,?,?,?,?)", txtOrganisationName.Text, txtDescription.Text, txtOrganisationStreet.Text, txtOrganisationStreetNumber.Text, Convert.ToInt32(selectedname)) > 0)//Keine Newvalues mehr sondern Bootstrap pop up
                    {
                        lblInfoBottom.Text = $"<span class='success'> Datensatz hinzugefügt! </span>";
                    }
                    else
                    {
                        lblInfoBottom.Text = $"<span class='error'> Nichts passiert! </span>";
                    }
                    Fill_gvAdminClubs();
                    ViewState["isAdding"] = false;
                    pnlBlockBg.Visible = false;
                    pnlInsert.Visible = false;
                }
            }
        }

        private bool ValidateData()
        {
            string errorDescription = "";
            bool valid = true;

            if (txtDescription.Text == "" || txtOrganisationName.Text == "" || txtOrganisationStreet.Text == "" || txtOrganisationStreetNumber.Text == "" || ddlSpokesPerson.SelectedValue == null) { valid = false; errorDescription += "Einer oder mehrere der Werte sind leer!  "; }
            else
            {
                //proof string values
                if (txtOrganisationName.Text.Length > 50 || txtDescription.Text.Length > 120 || txtOrganisationStreet.Text.Length > 50 || txtOrganisationStreetNumber.Text.Length > 5) { valid = false; errorDescription += "Vereinsnamen-, Beschreibungs- oder Anschrifts-Format ist ungültig!  "; }               
            }
            lblInfo.Text = errorDescription;
            lblInfo2.Text = errorDescription;
            return valid;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)//Update Button auf Update PAnel
        {
            if (Convert.ToBoolean(ViewState["isAdding"]) == false)
            {
                string selectedname = ddlSpokesPerson2.SelectedValue;
               bool valid = ValidateData2();
                int id;

                id = Convert.ToInt32(((Label)gvAdminClubs.Rows[idForUpdating].FindControl("lblItemTemplateClubID")).Text);

                if (valid == true)
                {
                    if (db.ExecuteNonQuery("Update organisation SET NAME=?, DESCRIPTION=?, STREET=?, NUMBER=?, SPOKESPERSON=? WHERE ORGID=?", txtOrganisationName2.Text, txtDescription2.Text, txtOrganisationStreet2.Text, txtOrganisationStreetNumber2.Text, Convert.ToInt32(selectedname), id) > 0)//Keine Newvalues mehr sondern Bootstrap pop up
                    {
                        lblInfoBottom.Text = $"<span class='success'> Datensatz geändert! </span>";
                    }
                    else
                    {
                        lblInfoBottom.Text = $"<span class='error'> Nichts passiert! </span>";
                    }

                    gvAdminClubs.EditIndex = -1;
                    Fill_gvAdminClubs();
                    pnlBlockBg.Visible = false;
                    pnlUpdate.Visible = false;
                    gvAdminClubs.DataBind();

                }
            }
        }

        private bool ValidateData2()
        {
            string errorDescription = "";
            bool valid = true;

            if (txtDescription2.Text == "" || txtOrganisationName2.Text == "" || txtOrganisationStreet2.Text == "" || txtOrganisationStreetNumber2.Text == "" || ddlSpokesPerson2.SelectedValue == null) { valid = false; errorDescription += "Einer oder mehrere der Werte sind leer!  "; }
            else
            {
                //proof string values
                if (txtOrganisationName2.Text.Length > 50 || txtDescription2.Text.Length > 120 || txtOrganisationStreet2.Text.Length > 50 || txtOrganisationStreetNumber2.Text.Length > 5) { valid = false; errorDescription += "Vereinsnamen-, Beschreibungs- oder Anschrifts-Format ist ungültig!  "; }
            }
            lblInfo.Text = errorDescription;
            lblInfo2.Text = errorDescription;
            return valid;
        }

        protected void gvAdminClubs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Add")
            {
                ViewState["isAdding"] = true;
                pnlBlockBg.Visible = true;
                pnlInsert.Visible = true;
            }
        }

        protected void gvAdminClubs_RowEditing(object sender, GridViewEditEventArgs e)
        {
            pnlBlockBg.Visible = true;
            pnlUpdate.Visible = true;
            idForUpdating = e.NewEditIndex;
            FillControlsWithValues();
        }

        private void FillControlsWithValues()
        {
            int id;
            id = Convert.ToInt32(((Label)gvAdminClubs.Rows[idForUpdating].FindControl("lblItemTemplateClubID")).Text);
            DataTable dt = db.Query("SELECT * FROM organisation WHERE ORGID=?", id);
            txtOrganisationName2.Text = dt.Rows[0]["NAME"].ToString();
            txtOrganisationStreet2.Text = dt.Rows[0]["STREET"].ToString();
            txtOrganisationStreetNumber2.Text = dt.Rows[0]["NUMBER"].ToString();
            txtDescription2.Text = (dt.Rows[0]["DESCRIPTION"]).ToString();           
            ddlSpokesPerson2.SelectedValue = dt.Rows[0]["SPOKESPERSON"].ToString();//Garantiert, dass der richtige wert ausgewählt ist, aus den werten die zu beginn in die DDl geschrieben wurden (FillDDL)
        }

        protected void gvAdminClubs_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = gvAdminClubs.Rows[e.RowIndex];
            string clubID = ((Label)row.FindControl("lblItemTemplateClubID")).Text;
            db.Query($"delete from organisation where ORGID = {clubID}");
            Fill_gvAdminClubs();
            lblInfoBottom.Text += "Datensatz wurde gelöscht!";
        }

        protected void gvAdminClubs_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sql = "SELECT organisation.ORGID, organisation.NAME, organisation.STREET, organisation.NUMBER, organisation.DESCRIPTION, user.UID, user.GN, user.SN FROM organisation INNER JOIN user ON organisation.SPOKESPERSON = user.UID";
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
                dv.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);
                gvAdminClubs.DataSource = dv;
                gvAdminClubs.DataBind();
            }
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
    }
}