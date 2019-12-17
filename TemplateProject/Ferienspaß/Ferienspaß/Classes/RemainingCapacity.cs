using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Ferienspaß.Classes
{
    public class RemainingCapacity
    {

        public DataTable GetDataTableWithRemainingCapacities(DataTable dt)
        {
            //Add new column
            dt.Columns.Add("remainingCapacity", typeof(int));
            

            foreach (DataRow row in dt.Rows)    
            {
                int projectID = Convert.ToInt32(row["PID"]);
                int capacity = Convert.ToInt32(row["capacity"]);
                int remainingCapacity = GetRemainingCapacity(projectID, capacity);

                row["remainingCapacity"] = remainingCapacity;
            }

            return dt;
        }


        public int GetRemainingCapacity(int projectID, int capacity)
        {
            DataTable dt;
            CsharpDB db = new CsharpDB();

            string command = $"select count(CID) from participation where pid = {projectID}";
            dt = db.Query(command);

            string row = dt.Rows[0].ItemArray[0].ToString();
            int participants = Convert.ToInt32(row);

            int remainintCapacity = capacity - participants;

            return remainintCapacity;
        }







    }
}