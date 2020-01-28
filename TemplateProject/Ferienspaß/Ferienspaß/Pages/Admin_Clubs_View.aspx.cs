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
            dv.Sort = "SN ASC";
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

        }

        protected void btnBack2_Click(object sender, EventArgs e)
        {

        }

        protected void btnAdd_Click1(object sender, EventArgs e)//Hinzufügen Button auf Insert Panel
        {

        }

        protected void btnUpdate_Click(object sender, EventArgs e)//Update Button auf Update PAnel
        {

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
    }
}