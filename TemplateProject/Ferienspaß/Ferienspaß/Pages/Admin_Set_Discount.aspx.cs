using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ferienspaß.Pages
{
    public partial class Admin_Set_Discount : System.Web.UI.Page
    {
        CsharpDB db = new CsharpDB();
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = "";
        }

        protected void btnCommit_Click(object sender, EventArgs e)
        {
            DataTable dt = db.Query("SELECT MyValue from settings WHERE MyKey = 'USER_LOGIN_FROM'");        
            DateTime from = Convert.ToDateTime(dt.Rows[0]["MyValue"]);
            DataTable dt2 = db.Query("SELECT MyValue from settings WHERE MyKey = 'USER_LOGIN_TILL'");
            DateTime till = Convert.ToDateTime(dt2.Rows[0]["MyValue"]);
            if (DateTime.Today>=from && DateTime.Today<=till)
            {
                if (int.TryParse(txtDiscount.Text, out int a))
                {

                    db.ExecuteNonQuery($"UPDATE settings Set MyValue ={Convert.ToInt32(txtDiscount.Text)} WHERE MyKey = 'GLOBAL_DISCOUNT'");
                    DataTable dt3 = db.Query("SELECT project.pid FROM project where project.PID not in (SELECT participation.pid from project inner join participation on participation.PID = project.pid)");
                    List<int> ids = new List<int>();
                    DataTable dt4 = db.Query("SELECT MyValue from settings WHERE MyKey = 'GLOBAL_DISCOUNT'");
                    int discountInPercent = Convert.ToInt32(dt4.Rows[0]["MyValue"]);
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        //ids.Add(Convert.ToInt32(dt3.Rows[i]["pid"]));
                        db.ExecuteNonQuery($"UPDATE project SET DISCOUNT = {discountInPercent} WHERE pid = {Convert.ToInt32(dt3.Rows[i]["pid"])}");

                    }
                    lblMessage.Text = "Rabatt für Projekte ohne Anmeldungen wurde auf gewünschten Wert geändert!";
                }
                else
                {
                    lblMessage.Text = "Geben Sie einen korrekten Wert ein!";
                }
            }
            else
            {
                lblMessage.Text = "Sie sind zur Zeit nicht dazu berechtigt, einen Rabatt festzulegen!";
            }
           
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Admin_Project_View.aspx");
        }
    }
}