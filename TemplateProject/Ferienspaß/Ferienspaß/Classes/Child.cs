using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Ferienspaß.Classes
{
    public class Child
    {
        public int? ChildID { get; set; } 
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

        public Child(int _childID, string _givenname, string _surname, DateTime _birthday)
        {
            ChildID = _childID;
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

        public string DeleteChild()
        {
            CsharpDB db = new CsharpDB();
            try
            {
                if(Convert.ToInt32(db.ExecuteScalar("SELECT Count(*) FROM child WHERE GN=? AND SN=? AND BD=?", Givenname, Surname, Birthday)) > 0)
                {
                    DataRow row = db.Query("SELECT CID FROM child WHERE GN=? AND SN=? AND BD=?", Givenname, Surname, Birthday).Rows[0];
                    ChildID = Convert.ToInt32(row["CID"]);
                    db.ExecuteNonQuery("DELETE FROM child WHERE CID=?",ChildID);
                    return ToString();
                }
                else
                {
                    return "Doesn't exist";
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public Child UpdateChild(string _givenname, string _surname, DateTime _birthday)
        {
            CsharpDB db = new CsharpDB();
            try
            {
                DataRow row = db.Query("SELECT CID FROM child WHERE GN=? AND SN=? AND BD=?", Givenname, Surname, Birthday).Rows[0];
                ChildID = Convert.ToInt32(row["CID"]);
                db.ExecuteNonQuery("UPDATE child SET GN=?, SN=?, BD=? WHERE CID=?", _givenname, _surname, _birthday.Date, ChildID);
                Givenname = _givenname;
                Surname = _surname;
                Birthday = _birthday;
                row = db.Query("SELECT GN, SN, BD, UID FROM child WHERE CID=?", ChildID).Rows[0];
                return new Child(row["GN"].ToString(), row["SN"].ToString(), (DateTime)row["BD"]);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }



    }
}