﻿using Ferienspaß.Classes;
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
    public partial class User_View : System.Web.UI.Page
    {

        CsharpDB db = new CsharpDB();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Fill_gv_UserView();
            }


            try
            {
                db = new CsharpDB();

                HtmlGenericControl c = (HtmlGenericControl)Master.FindControl("menu_mydata");
                if (c != null) c.Attributes.Add("class", "active");

                lbl_loggedInUser.Text = db.GetUserName(User.Identity.Name);
            }
            catch(Exception ex)
            {

            }

        }


        private void Fill_gv_UserView()
        {
            DataTable dt;
            DataView dv;
            RemainingCapacity rc = new RemainingCapacity();

            dt = db.Query($"SELECT * FROM project");
            dt = rc.GetDataTableWithRemainingCapacities(dt);
            dv = new DataView(dt);

           
            gv_UserView.DataSource = dv;
            gv_UserView.DataBind();

        }

       

        protected void gv_UserView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
            //Code used -> https://stackoverflow.com/questions/6503339/get-row-index-on-asp-net-rowcommand-event/6503483

            switch (e.CommandName)
            {
                case "details":

                    string projectID = ((Label)gvr.FindControl("lblProjectID")).Text;
                    Response.Redirect($"User_View_Details.aspx?id={projectID}");
                    break;


            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }
    }
}