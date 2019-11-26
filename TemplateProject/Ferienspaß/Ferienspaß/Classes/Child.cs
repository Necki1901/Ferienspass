using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Ferienspaß.Classes
{
    public class Child
    {
        public string Givenname { get; set; }
        public string Surname { get; set; }

        public DateTime Birthday { get; set; }

        //public int? ParentID { get; set; }


        public Child() { }

        public Child(string _givenname, string _surname, DateTime _birthday)
        {
            Givenname = _givenname;
            Surname = _surname;
            Birthday = _birthday;
        }

        public override string ToString()
        {
            return $"{Givenname} {Surname} {Birthday.Date}";
        }


        public Child CreateChild()
        {
            CsharpDB db = new CsharpDB();
            try
            {
                db.ExecuteNonQuery("INSERT INTO child(GN, SN, BD) VALUES(?,?,?)", Givenname, Surname, Birthday.Date);
                DataRow row = db.Query("SELECT GN, SN, BD, UID FROM child WHERE GN=? AND SN=?", Givenname, Surname).Rows[0];
                return new Child(row["GN"].ToString(), row["SN"].ToString(), (DateTime)row["BD"]);
            }
            catch(Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }



    }
}