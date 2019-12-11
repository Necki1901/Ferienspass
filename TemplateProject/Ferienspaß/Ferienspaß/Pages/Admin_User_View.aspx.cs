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
            DataTable dt = db.Query("SELECT user.UID, user.GN, user.SN, user.PHONE, user.EMAIL, user.LOCKED, user.EmailConfirmed, usergroup.UGID FROM user INNER JOIN usergroup ON user.UGID = usergroup.UGID");
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
            //if (e.CommandName == "Add")
            //{
               
            //}
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
                //bool valid = ValidateData(e);
                //if (valid == true)
                //{
                //    if (db.ExecuteNonQuery("INSERT INTO project (NAME, DESCRIPTION, DATE, START, END, PLACE, NUMBER, CAPACITY, GID) Values(?,?,?,?,?,?,?,?,?)", e.NewValues["NAME"], e.NewValues["DESCRIPTION"], Convert.ToDateTime(e.NewValues["DATE"]).ToShortDateString(), Convert.ToDateTime(e.NewValues["START"]).ToString("HH:mm:ss"), Convert.ToDateTime(e.NewValues["END"]).ToString("HH:mm:ss"), e.NewValues["PLACE"], Convert.ToInt32(e.NewValues["NUMBER"]), Convert.ToInt32(e.NewValues["CAPACITY"]), Convert.ToInt32(selectedname)) > 0)
                //    {
                //        lblInfo.Text = $"<span class='success'> Datensatz hinzugefügt! </span>";
                //    }
                //    else
                //    {
                //        lblInfo.Text = $"<span class='error'> Nichts passiert! </span>";
                //    }
                //    gvAdminUsers.EditIndex = -1;
                //    Fill_gvAdminUsers();

                //}
            }
            ViewState["isAdding"] = false;

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
        }
    }
}