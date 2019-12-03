using Ferienspaß.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
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

        private int GetNumberOfParticipants(int projectID, int capacity)
        {
            DataTable dt;
            string command = $"select count(CID) from participation where pid = {projectID}";
            dt = db.Query(command);

            string row = dt.Rows[0].ItemArray[0].ToString();
            int participants = Convert.ToInt32(row);

            int open_participants = capacity - participants;

            return open_participants;
        }

        protected void gv_UserView_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
    }
}