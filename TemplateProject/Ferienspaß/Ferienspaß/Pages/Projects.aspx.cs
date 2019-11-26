using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Ferienspaß
{
    public partial class _Projects : Page
    {

        public CsharpDB db;

        protected void Page_Load(object sender, EventArgs e)
        {
            db = new CsharpDB();

            HtmlGenericControl c = (HtmlGenericControl)Master.FindControl("menu_projects");
            if(c!=null) c.Attributes.Add("class", "active");

            lbl_loggedInUser.Text = db.GetUserName(User.Identity.Name);
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }
    }
}