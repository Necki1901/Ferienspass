using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data;

namespace Ferienspaß.Pages
{
    public partial class Admin_Cancellations_View : System.Web.UI.Page
    {
        CsharpDB db;

        protected void Page_Load(object sender, EventArgs e)
        {
            db = new CsharpDB();
            if (!Page.IsPostBack)
            {
                FillGvCancellations();
            }
            try
            {

                HtmlGenericControl c = (HtmlGenericControl)Master.FindControl("menu_mydata");
                if (c != null) c.Attributes.Add("class", "active");

                lbl_loggedInUser.Text = db.GetUserName(User.Identity.Name);
            }
            catch (Exception ex)
            {

            }


        }

        private void FillGvCancellations()
        {
            DataTable dt = db.Query($"select project.pid, project.NAME, child.cid, child.GN, child.SN, cancellations.date, project.price, user.phone, user.gn as 'gnUser', user.sn as 'snUser' " +
                $"from(((cancellations inner join project on cancellations.pid = project.pid) " +
                $"inner join child on cancellations.cid = child.cid)" +
                $"inner join user on child.UID = user.uid)");

            DataView dv = new DataView(dt);

            gv_cancellations.DataSource = dv;
            gv_cancellations.DataBind();
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }

        protected void gv_cancellations_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
    }
}