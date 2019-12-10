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
        protected void Page_Load(object sender, EventArgs e)
        {
            Fill_gvAdminUsers();
            if (!Page.IsPostBack)
            {
              //  isAdding = false;
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
            DataTable dt = db.Query("SELECT user.UID, user.GN, user.SN, user.PHONE, user.EMAIL, usergroup.UGID FROM user INNER JOIN usergroup ON user.UGID = usergroup.UGID");
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
    }
}