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
    public partial class _MyData : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HtmlGenericControl c = (HtmlGenericControl)Master.FindControl("menu_mydata");
            if (c != null) c.Attributes.Add("class", "active");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }
    }
}