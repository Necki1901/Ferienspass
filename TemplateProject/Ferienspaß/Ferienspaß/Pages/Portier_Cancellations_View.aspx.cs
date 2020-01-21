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
        static bool isFiltered = false;

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
            string sql = "select cancellations.cancel_id, project.NAME, CONCAT(child.GN,' ',child.SN) AS childName, cancellations.date, project.price, user.phone, CONCAT(user.gn,' ',user.sn) AS userName from(((cancellations inner join project on cancellations.pid = project.pid) inner join child on cancellations.cid = child.cid) inner join user on child.UID = user.uid)";
            bool filterInUse = false;
                    
            if ((txtProjectName.Text != "" || txtStornoDate.Text != "" || txtParentName.Text != "" || txtChildName.Text != "") && isFiltered)
            {
                sql += " HAVING ";
                if (txtProjectName.Text != "")
                {
                    filterInUse = true;
                    sql += $"project.NAME LIKE '{txtProjectName.Text}%'";
                }
                if (txtStornoDate.Text != "")
                {
                    if (filterInUse) sql += "AND "; else filterInUse = true;
                    sql += $"cancellations.date = '{txtStornoDate.Text}'";
                }
                if (txtParentName.Text != "")
                {
                    if (filterInUse) sql += "AND "; else filterInUse = true;
                    sql += $"userName LIKE '%{txtParentName.Text}%'";
                }
                if (txtChildName.Text != "")
                {
                    if (filterInUse) sql += "AND ";
                    sql += $"childName LIKE '%{txtChildName.Text}%'";
                }
            }
            DataTable dt = db.Query(sql);
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
            switch (e.CommandName)
            {
                case "completed":
                    GridViewRow gvr = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                    int rowIndex = gvr.RowIndex;

                    string id = ((Label)gv_cancellations.Rows[rowIndex].FindControl("lblID")).Text;
                    db.Query($"DELETE FROM cancellations WHERE cancel_id={id}");
                    FillGvCancellations();
                    break;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            isFiltered = true;
            FillGvCancellations();
        }
    }
}