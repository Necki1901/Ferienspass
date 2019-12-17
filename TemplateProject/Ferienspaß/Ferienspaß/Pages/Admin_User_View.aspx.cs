using System;
using System.Collections.Generic;
using System.Data;
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
        bool isAdding;
        protected void Page_Load(object sender, EventArgs e)
        {
            lblInfo.Text = "";
            Fill_gvAdminUsers();
            if (!Page.IsPostBack)
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

        private void Fill_gvAdminUsers()
        {
            DataTable dt = db.Query("SELECT user.UID, user.GN, user.SN, user.PHONE, user.EMAIL, user.LOCKED, user.EmailConfirmed, usergroup.UGID, usergroup.DESCRIPTION FROM user INNER JOIN usergroup ON user.UGID = usergroup.UGID");
            DataView dv = new DataView(dt);
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
            GridViewRow gvr1 = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
            if (e.CommandName == "Add")
            {
                DataTable dt = db.Query("SELECT user.UID, user.GN, user.SN, user.PHONE, user.EMAIL, user.LOCKED, user.EmailConfirmed, usergroup.UGID, usergroup.DESCRIPTION FROM user INNER JOIN usergroup ON user.UGID = usergroup.UGID ");
                dt.Clear();
                DataRow dr = dt.NewRow();
                //dr["PID"] = -1;
                ViewState["isAdding"] = true;
                dt.Rows.Add(dr);
                gvAdminUsers.DataSource = dt;
                gvAdminUsers.EditIndex = 0;
                gvAdminUsers.DataBind();
                GridViewRow gvr = gvAdminUsers.Rows[gvAdminUsers.EditIndex];
                ImageButton ib = gvr.FindControl("btnDelete") as ImageButton;
                ib.Enabled = false;
                ViewState["btnDeleteActive"] = ib.Enabled;
            }
            if (e.CommandName == "Children")
            {
                string userID = ((Label)gvr1.FindControl("lblItemTemplateUserID")).Text;
                Response.Redirect(String.Format("Admin_Child_Administration.aspx?id={0}", userID));
            }
        }

        protected void gvAdminUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewRow gvr = gvAdminUsers.Rows[e.NewEditIndex];          
            gvAdminUsers.EditIndex = e.NewEditIndex;            
            Fill_gvAdminUsers();           
        }

        protected void gvAdminUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow gvr = gvAdminUsers.Rows[e.RowIndex];
            DropDownList dropDownList = (DropDownList)gvr.FindControl("ddlEditItemTemplateUserGroup");
            string selectedGroup = dropDownList.SelectedValue;           
            if (Convert.ToBoolean(ViewState["isAdding"]) == false)
            {
                bool valid = ValidateData(e);
                if (valid == true)
                {
                    if (db.ExecuteNonQuery("UPDATE user SET GN = ?, SN = ?, PHONE = ?, EMAIL = ?, UGID = ?, LOCKED = ?, EmailConfirmed = ? WHERE UID = ?", e.NewValues["GN"], e.NewValues["SN"], e.NewValues["PHONE"], e.NewValues["EMAIL"], Convert.ToInt32(selectedGroup), e.NewValues["LOCKED"], e.NewValues["EmailConfirmed"], e.Keys[0]) > 0)
                    {
                        lblInfo.Text = $"<span class='success'> Datensatz aktualisiert! </span>";
                    }
                    else
                    {
                        lblInfo.Text = $"<span class='error'> Nichts passiert! </span>";
                    }
                    gvAdminUsers.EditIndex = -1;
                    Fill_gvAdminUsers();
                }
            }
            else
            {
                bool valid = ValidateData(e);
                if (valid == true)
                {
                    if (db.ExecuteNonQuery("INSERT INTO user (GN, SN, PHONE, EMAIL, LOCKED, EmailConfirmed, UGID) Values(?,?,?,?,?,?,?)", e.NewValues["GN"], e.NewValues["SN"], e.NewValues["PHONE"], e.NewValues["EMAIL"], e.NewValues["LOCKED"], e.NewValues["EmailConfirmed"],Convert.ToInt32(selectedGroup)) > 0)
                    {
                        lblInfo.Text = $"<span class='success'> Datensatz hinzugefügt! </span>";
                    }
                    else
                    {
                        lblInfo.Text = $"<span class='error'> Nichts passiert! </span>";
                    }
                    gvAdminUsers.EditIndex = -1;
                    Fill_gvAdminUsers();
                    DataTable u = db.Query("SELECT UID,GN,EMAIL,SN FROM user WHERE EMAIL LIKE ? LIMIT 1", e.NewValues["EMAIL"]);
                    string resetHash = PwdResetHash(u.Rows[0]["GN"].ToString(), u.Rows[0]["EMAIL"].ToString(), u.Rows[0]["UID"].ToString());
                    bool sentEmail = db.SendMail(u.Rows[0]["EMAIL"].ToString(), u.Rows[0]["GN"].ToString() + " " + u.Rows[0]["SN"].ToString(), "Passwort zurücksetzen", "http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + "/PasswordReset.aspx?hash=" + resetHash);

                }
            }
            ViewState["isAdding"] = false;
            GridViewRow gvr2 = gvAdminUsers.Rows[e.RowIndex];//so auch bei Add-Button
            ImageButton ib = gvr2.FindControl("btnDelete") as ImageButton;
            ViewState["btnDeleteActive"] = true;
            ib.Enabled = Convert.ToBoolean(ViewState["btnDeleteActive"]);

        }

        public string PwdResetHash(string firstname, string email, string uId)
        {
            string toEncode = firstname + email + uId + DateTime.Now;
            return Crypt.GenerateSHA256String(toEncode);
        }
        private bool ValidateData(GridViewUpdateEventArgs e)
        {
            string errorDescription = "";
            bool valid = true;
           
            if (e.NewValues["GN"] == null || e.NewValues["SN"] == null || e.NewValues["PHONE"] == null || e.NewValues["EMAIL"] == null || e.NewValues["LOCKED"] == null || e.NewValues["EmailConfirmed"] == null) { valid = false; errorDescription += "Einer oder mehrere der Werte sind null!  "; }
            else
            {
                //proof integer values
                if ((!int.TryParse((e.NewValues["LOCKED"].ToString()), out int a)) || (e.NewValues["LOCKED"].ToString()!="1" && e.NewValues["LOCKED"].ToString() != "0" )) { valid = false; errorDescription += "ZUSTAND-Format ist ungültig!  "; }
                if ((!int.TryParse((e.NewValues["EmailConfirmed"].ToString()), out int b))|| (e.NewValues["EmailConfirmed"].ToString() != "1" && e.NewValues["EmailConfirmed"].ToString() != "0")) { valid = false; errorDescription += "MAIL-ZUSTAND-Format ist ungültig!  "; }

                //proof string values
                if(e.NewValues["GN"].ToString().Length>50 || e.NewValues["SN"].ToString().Length > 50 || e.NewValues["PHONE"].ToString().Length > 20 || e.NewValues["EMAIL"].ToString().Length > 70) { valid = false; errorDescription += "NAME, TELEFON oder MAIL-Format ist ungültig!  "; }

                //proof email

                if (!(e.NewValues["EMAIL"].ToString().Contains("@"))){ valid = false; errorDescription += "MAIL-Format ist ungültig!  "; }

                       
            }
            lblInfo.Text = errorDescription;
            return valid;
        }

        protected void gvAdminUsers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvAdminUsers.EditIndex = -1;
            Fill_gvAdminUsers();
            lblInfo.Text = "";
            GridViewRow gvr2 = gvAdminUsers.Rows[e.RowIndex];
            ImageButton ib = gvr2.FindControl("btnDelete") as ImageButton;
            ViewState["btnDeleteActive"] = true;
            ib.Enabled = Convert.ToBoolean(ViewState["btnDeleteActive"]);
        }

        protected void gvAdminUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && gvAdminUsers.EditIndex == e.Row.RowIndex)
            {

                Control ctrl = e.Row.FindControl("ddlEditItemTemplateUserGroup");
                DropDownList ddl = ctrl as DropDownList;
                DataTable dt = GetAllUserGroups();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //ddl.Items.Add(Convert.ToString(dt.Rows[i].ItemArray[0]));
                    ddl.Items.Add(new ListItem(dt.Rows[i]["DESCRIPTION"].ToString(), dt.Rows[i]["UGID"].ToString()));
                }
                DataRowView dr = e.Row.DataItem as DataRowView;
                ddl.SelectedValue = dr["UGID"].ToString();
                ddl.SelectedItem.Text = dr["DESCRIPTION"].ToString();
            }
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
            lblInfo.Text += "Datensatz wurde gelöscht!";
        }

        protected void btnChildren_Click(object sender, ImageClickEventArgs e)
        {
            //Response.Redirect("asfd.aspx?id=sd")
        }
    }
}