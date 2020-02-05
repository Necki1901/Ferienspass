using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Ferienspaß
{
    public partial class Admin_User_View : System.Web.UI.Page
    {
        CsharpDB db = new CsharpDB();
        static bool isFiltered = false;
        static int idForUpdating;
        int sortCounter = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            lblInfo.Text = "";
            lblInfo2.Text = "";
            lblInfoBottom.Text = "";
            Fill_gvAdminUsers();

            if (!Page.IsPostBack)
            {
                FillDdl();
                Fill_ddlUserGroup3();
            }

            lblInfo.Text = "";
            lblInfo2.Text = "";
            lblInfoBottom.Text = "";
            Fill_gvAdminUsers();
            
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

        private void FillDdl()
        {
            DataTable dt = GetAllUserGroups();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ListItem li = new ListItem(dt.Rows[i]["DESCRIPTION"].ToString(), dt.Rows[i]["UGID"].ToString());
                ddlUserGroup.Items.Add(li);
                ddlUserGroup2.Items.Add(li);
            }
            //kann man eigentlich entfernen, da es beim einfügen so übernommen wird
            ddlUserGroup2.DataValueField = "UGID";
            ddlUserGroup2.DataTextField = "DESCRIPTION";
            ddlUserGroup.DataValueField = "UGID";
            ddlUserGroup.DataTextField = "DESCRIPTION";
            //

            ddlLocked.Items.Add(new ListItem("Ja","1"));
            ddlLocked.Items.Add(new ListItem("Nein", "0"));
            ddlLocked2.Items.Add(new ListItem("Ja", "1"));
            ddlLocked2.Items.Add(new ListItem("Nein", "0"));
            ddlEmailConfirmed.Items.Add(new ListItem("Ja", "1"));
            ddlEmailConfirmed.Items.Add(new ListItem("Nein", "0"));
            ddlEmailConfirmed2.Items.Add(new ListItem("Ja", "1"));
            ddlEmailConfirmed2.Items.Add(new ListItem("Nein", "0"));
        }

        private void Fill_gvAdminUsers()
        {
            string sql = "SELECT user.UID, user.GN, user.SN, user.PHONE, user.EMAIL, user.LOCKED, user.EmailConfirmed, usergroup.UGID, usergroup.DESCRIPTION FROM user INNER JOIN usergroup ON user.UGID = usergroup.UGID";
            
            //überprüfen ob es einen vorhandenen Filter gibt
            if (isFiltered && (txtName.Text != "" || txtSurname3.Text != "" || ddlUserGroup3.SelectedValue != "Alle" || cbxConditionConfirmed.Checked || cbxConditionLocked.Checked))
            {
                bool filter = false;
                sql += " HAVING ";

                //einzelne Filter werden hinzugefügt
                if (txtName.Text != "")
                {
                    sql += $"GN LIKE '{txtName.Text}%'";
                    filter = true;
                }
                if (txtSurname3.Text != "")
                {
                    if (filter) sql += $" AND "; else filter = true;
                    sql += $"SN LIKE '{txtSurname3.Text}%'";
                }
                if (ddlUserGroup3.SelectedValue != "Alle")
                {
                    if (filter) sql += $" AND "; else filter = true;
                    sql += $"UGID = {ddlUserGroup3.SelectedValue}";
                }
                if (cbxConditionConfirmed.Checked)
                {
                    if (filter) sql += $" AND "; else filter = true;
                    sql += $"EmailConfirmed = 1";
                }
                if (cbxConditionLocked.Checked)
                {
                    if (filter) sql += $" AND ";
                    sql += $"locked = 1";
                } 
            }
            DataTable dt = db.Query(sql);
            DataView dv = new DataView(dt);
            dv.Sort = "SN ASC";
            gvAdminUsers.DataSource = dv;
            gvAdminUsers.DataBind();
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }

        protected void gvAdminUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Add")
            {
                pnlBlockBg.Visible = true;
                pnlInsert.Visible = true;
            }

            if (e.CommandName == "Children")
            {
                GridViewRow gvr1 = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
                string userID = ((Label)gvr1.FindControl("lblItemTemplateUserID")).Text;
                Response.Redirect(String.Format("Admin_Child_Administration.aspx?id={0}", userID));
            }
        }

        protected void gvAdminUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            pnlBlockBg.Visible = true;
            pnlUpdate.Visible = true;
            idForUpdating = e.NewEditIndex;
            FillControlsWithValues();
        }

        private void FillControlsWithValues()
        {
            int id;
            id = Convert.ToInt32(((Label)gvAdminUsers.Rows[idForUpdating].FindControl("lblItemTemplateUserID")).Text);
            DataTable dt = db.Query("SELECT * FROM user WHERE UID=?", id);
            txtGivenName2.Text = dt.Rows[0]["GN"].ToString();
            txtSurName2.Text = dt.Rows[0]["SN"].ToString();
            txtPhone2.Text = dt.Rows[0]["PHONE"].ToString();
            txtEMail2.Text = dt.Rows[0]["EMAIL"].ToString();
            ddlUserGroup2.SelectedValue = dt.Rows[0]["UGID"].ToString();
            ddlLocked2.SelectedValue = dt.Rows[0]["LOCKED"].ToString();
            ddlEmailConfirmed2.SelectedValue = dt.Rows[0]["EmailConfirmed"].ToString();
        }

        public string PwdResetHash(string firstname, string email, string uId)
        {
            string toEncode = firstname + email + uId + DateTime.Now;
            return Crypt.GenerateSHA256String(toEncode);
        }
        private bool ValidateData()
        {
            string errorDescription = "";
            bool valid = true;

            if (txtGivenName.Text == "" || txtSurName.Text == "" || txtPhone.Text == "" || txtEMail.Text == "" || ddlLocked.SelectedValue == null || ddlEmailConfirmed.SelectedValue == null || ddlUserGroup.SelectedValue == null) { valid = false; errorDescription += "Einer oder mehrere der Werte sind leer!  "; }
            else
            {
                //proof string values
                if (txtGivenName.Text.Length > 50 || txtSurName.Text.Length > 50 || txtPhone.Text.Length > 20 || txtEMail.Text.Length > 70) { valid = false; errorDescription += "NAME, TELEFON oder MAIL-Format ist ungültig!  "; }

                //proof email

                if (!(txtEMail.Text.Contains("@"))) { valid = false; errorDescription += "MAIL-Format ist ungültig!  "; }

                int amount = Convert.ToInt32(db.ExecuteScalar($"SELECT COUNT(*) FROM user WHERE EMAIL='{txtEMail.Text}'"));
                if (amount > 0) { valid = false; errorDescription += "MAIL bereits vorhanden!"; }

            }
            lblInfo.Text = errorDescription;
            lblInfo2.Text = errorDescription;
            return valid;
        }

        private bool ValidateData2()
        {
            string errorDescription = "";
            bool valid = true;

            if (txtGivenName2.Text == "" || txtSurName2.Text == "" || txtPhone2.Text == "" || txtEMail2.Text == "" || ddlLocked2.SelectedValue == null || ddlEmailConfirmed2.SelectedValue == null || ddlUserGroup2.SelectedValue == null) { valid = false; errorDescription += "Einer oder mehrere der Werte sind leer!  "; }
            else
            {
                //proof string values
                if (txtGivenName2.Text.Length > 50 || txtSurName2.Text.Length > 50 || txtPhone2.Text.Length > 20 || txtEMail2.Text.Length > 70) { valid = false; errorDescription += "NAME, TELEFON oder MAIL-Format ist ungültig!  "; }

                //proof email
                if (!(txtEMail2.Text.Contains("@"))) { valid = false; errorDescription += "MAIL-Format ist ungültig!  "; }

                int amount = Convert.ToInt32(db.ExecuteScalar($"SELECT COUNT(*) FROM user WHERE EMAIL='{txtEMail2.Text}'"));
                if (amount > 0) { valid = false; errorDescription += "MAIL bereits vorhanden!"; }

            }
            lblInfo.Text = errorDescription;
            lblInfo2.Text = errorDescription;
            return valid;
        }

        private DataTable GetAllUserGroups()
        {
            return db.Query("SELECT UGID, DESCRIPTION FROM usergroup");
        }

        protected void gvAdminUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = gvAdminUsers.Rows[e.RowIndex];
            string userID = ((Label)row.FindControl("lblItemTemplateUserID")).Text;
            db.Query($"delete from user where UID = {userID}");
            Fill_gvAdminUsers();
            lblInfoBottom.Text += "Datensatz wurde gelöscht!";
        }

        protected void btnChildren_Click(object sender, ImageClickEventArgs e)
        {
            //Response.Redirect("asfd.aspx?id=sd")
        }

        protected void gvAdminUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAdminUsers.PageIndex = e.NewPageIndex;
            Fill_gvAdminUsers();
        }

        protected void btnBack_Click(object sender, EventArgs e)//Klicken Des Zurück Buttons am Insert Panel
        {
            pnlBlockBg.Visible = false;
            pnlInsert.Visible = false;
        }

        protected void btnAdd_Click1(object sender, EventArgs e)//Klicken des Add buttons am Insert panel
        {
            bool valid = ValidateData();
            if (valid == true)
            {
                if (db.ExecuteNonQuery("INSERT INTO user (GN, SN, PHONE, EMAIL, LOCKED, EmailConfirmed, UGID) Values(?,?,?,?,?,?,?)", txtGivenName.Text, txtSurName.Text, txtSurName.Text, txtEMail.Text, ddlLocked.SelectedValue, ddlEmailConfirmed.SelectedValue, ddlUserGroup.SelectedValue) > 0)//Keine Newvalues mehr sondern Bootstrap pop up
                {
                    lblInfoBottom.Text = $"<span class='success'> Datensatz hinzugefügt! </span>";
                }
                else
                {
                    lblInfoBottom.Text = $"<span class='error'> Nichts passiert! </span>";
                }
                Fill_gvAdminUsers();
                pnlBlockBg.Visible = false;
                pnlInsert.Visible = false;
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)//Klicken des Update Buttons am update panel
        {
            int id = Convert.ToInt32(((Label)gvAdminUsers.Rows[idForUpdating].FindControl("lblItemTemplateUserID")).Text);
            bool valid = ValidateData2();
            if (valid == true)
            {
                if (db.ExecuteNonQuery("UPDATE user SET GN = ?, SN = ?, PHONE = ?, EMAIL = ?, UGID = ?, LOCKED = ?, EmailConfirmed = ? WHERE UID = ?", txtGivenName2.Text, txtSurName2.Text, txtPhone2.Text, txtEMail2.Text, ddlUserGroup2.SelectedValue, ddlLocked2.SelectedValue, ddlEmailConfirmed2.SelectedValue, id) > 0)
                {
                    lblInfoBottom.Text = $"<span class='success'> Datensatz aktualisiert! </span>";
                }
                else
                {
                    lblInfoBottom.Text = $"<span class='error'> Nichts passiert! </span>";
                }
                pnlBlockBg.Visible = false;
                pnlUpdate.Visible = false;
                gvAdminUsers.EditIndex = -1;
                Fill_gvAdminUsers();
            }
        }

        protected void btnBack2_Click(object sender, EventArgs e)//Klicken Des Zurück Buttons am Update Panel
        {
            pnlBlockBg.Visible = false;
            pnlUpdate.Visible = false;
            gvAdminUsers.EditIndex = -1;
            Fill_gvAdminUsers();
            gvAdminUsers.DataBind();
        }

        private void Fill_ddlUserGroup3()
        {
            CsharpDB db = new CsharpDB();
            List<int> usergroups = new List<int>();
            DataTable dt = GetAllUserGroups();
            ddlUserGroup3.Items.Add("Alle");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddlUserGroup3.Items.Add(new ListItem(dt.Rows[i]["DESCRIPTION"].ToString(), dt.Rows[i]["UGID"].ToString()));
            }
            ddlUserGroup3.SelectedValue = "Alle";
        }


        protected void btnSearch_Click(object sender, EventArgs e) 
        {
            isFiltered = true;
            Fill_gvAdminUsers();
        }

        protected void gvAdminUsers_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sql = "SELECT user.UID, user.GN, user.SN, user.PHONE, user.EMAIL, user.LOCKED, user.EmailConfirmed, usergroup.UGID, usergroup.DESCRIPTION FROM user INNER JOIN usergroup ON user.UGID = usergroup.UGID";      
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
                gvAdminUsers.DataSource = dv;
                gvAdminUsers.DataBind();
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