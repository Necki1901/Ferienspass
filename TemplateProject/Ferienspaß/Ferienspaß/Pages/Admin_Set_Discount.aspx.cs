using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ferienspaß.Pages
{
    public partial class Admin_Set_Discount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // PRIVILLEGE CHECK
            int ug = CsharpDB.GetUserGroup(Session["usergroup"]);
            if (ug != 0)
            {
                Response.Redirect("NotPermittedPage.html");
            }
        }
    }
}