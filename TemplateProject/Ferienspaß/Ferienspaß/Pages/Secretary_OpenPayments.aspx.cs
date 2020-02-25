using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Ferienspaß.Pages
{
    public partial class Secretary_OpenPayments : System.Web.UI.Page
    {
        private CsharpDB db;

        protected void Page_Load(object sender, EventArgs e)
        {

            // PRIVILLEGE CHECK
            int ug = CsharpDB.GetUserGroup(Session["usergroup"]);
            if (ug != 0 && ug != 4)
            {
                Response.Redirect("NotPermittedPage.html");
            }

            db = new CsharpDB();

            if (!Page.IsPostBack)
            {
                ViewState["project"] = Request.QueryString["project"];
                Fill_Gridview();
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

        private void Fill_Gridview()
        {
            DataTable dt = db.Query(
                "SELECT participation.PID, participation.CID, CONCAT(child.GN,' ',child.SN) as childname, CONCAT(user.GN, ' ', user.SN) as username, " +
                "user.EMAIL, user.PHONE, project.NAME, project.PRICE, project.PAYMENT_DEADLINE " +
                "from(((participation " +
                "INNER JOIN child on participation.CID = child.CID) " +
                "INNER JOIN user on child.UID = user.UID) " +
                "INNER JOIN project on participation.PID = project.PID) " +
                "WHERE paid = 0 AND project.price > 0");

            DataView dv = new DataView(dt);

            gvOpenPayments.DataSource = dv;
            gvOpenPayments.DataBind();
        }

        protected void gvOpenPayments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "setParticipationPaid":
                    GridViewRow gvr = (GridViewRow)(((Button)e.CommandSource).NamingContainer);     //get the current gridviewrow
                    int rowIndex = gvr.RowIndex;

                    string pid = ((Label)gvOpenPayments.Rows[rowIndex].FindControl("lblPID")).Text;
                    string cid = ((Label)gvOpenPayments.Rows[rowIndex].FindControl("lblCID")).Text;

                    if (db.ExecuteNonQuery($"UPDATE participation SET paid = 1 WHERE CID = {cid} AND PID = {pid}") == 1)
                    {
                        lit_msg.Text = CreateMSGString("Anmeldung wurde als bezahlt markiert", "success");
                    }
                    else
                        lit_msg.Text = CreateMSGString("Ein Fehler ist aufgetreten", "danger");

                    Fill_Gridview();

                    break;
            }
        }

        private string CreateMSGString(string msg, string type)
        {
            return "<div class=\"alert alert-" + type + " mt-3 mb-1\" role=\"alert\">" + msg + "</div>";
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();

        }
    }
}