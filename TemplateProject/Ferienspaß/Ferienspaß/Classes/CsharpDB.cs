using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Ferienspaß
{
    public class CsharpDB
    {

        public enum UserTyp { Administrator,Firma };

        private OdbcConnection connection = null;
        private string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["CsharpDB"].ConnectionString;
            }
        }


        /// <summary>
        /// Connection to database
        /// </summary>
        public OdbcConnection Connection
        {
            get
            {
                if (connection == null) connection = new OdbcConnection(ConnectionString);
                return connection;
            }
        }

        public OdbcCommand CreateCommand(string sql,params object[] parametervalues)
        {
            OdbcCommand cmd = new OdbcCommand(sql, Connection);
            foreach(object parametervalue in parametervalues)
            {
                cmd.Parameters.AddWithValue("", parametervalue);
            }
            return cmd;
        }
        /// <summary>
        /// Execute reader perform a callback on each readed record
        /// </summary>
        /// <param name="action">callback method action(OdbcDataReader reader)</param>
        /// <param name="sql"></param>
        /// <param name="parametervalues"></param>
        public void ExecuteReader(Action<OdbcDataReader> action,string sql, params object[] parametervalues)
        {
            OdbcCommand cmd = CreateCommand(sql,parametervalues);
            Open();
            OdbcDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                action(reader);
            }
            reader.Close();
            Close();
        }

        public void Open()
        {
            Connection.Open();
        }

        public void Close()
        {
            Connection.Close();
        }

        internal DataTable Query(string sql,params object[] parametervalues)
        {
            try {
                DataTable dt = new DataTable();
                OdbcDataAdapter da = new OdbcDataAdapter(CreateCommand(sql, parametervalues));
                da.Fill(dt);
                return dt;
            }catch(Exception ex) {
                return new DataTable();
            }
            
        }

        public object ExecuteScalar(string sql, params object[] parametervalues)
        {
            OdbcCommand cmd = CreateCommand(sql, parametervalues);
            Open();
            object ret = cmd.ExecuteScalar();
            Close();
            return ret;
        }

        public object ExecuteScalar(string sql)
        {
            OdbcCommand cmd = CreateCommand(sql);
            Open();
            object ret = cmd.ExecuteScalar();
            Close();
            return ret;
        }

        internal int ExecuteNonQuery(string sql, params object[] parametervalues)
        {
            OdbcCommand cmd = CreateCommand(sql, parametervalues);
            Open();
            int ret = cmd.ExecuteNonQuery();
            Close();
            return ret;
        }


        internal string GetUserName(object userid) {
            DataTable loggedInUser = Query("SELECT GN,SN FROM user WHERE UID=?", userid.ToString());
            return loggedInUser.Rows[0].ItemArray[0].ToString() + " " + loggedInUser.Rows[0].ItemArray[1].ToString();
        }

        internal string GetPortalOption(string optionname) {
            DataTable option = Query("SELECT MyValue FROM settings WHERE MyKey=?", optionname);
            if (option.Rows[0].ItemArray[0].ToString().Length < 1) return null;
            else return option.Rows[0].ItemArray[0].ToString();
        }


        public bool SaveLogoInDB(string userid,Byte[] logo) {
            if (ExecuteNonQuery("UPDATE portalusers SET Logo=? WHERE userid=?", logo, userid) > 0) return true;
            else return false;

        }

        public static bool CheckPasswordRequirements(string pwd) {
            if (pwd.Length < 8) return false;
            else if (!pwd.Any(c => char.IsDigit(c))) return false;
            else if (!pwd.Any(c => char.IsUpper(c))) return false;
            return true;

        }

        public bool SendMail(string to_mail, string to_name, string subject, string body) {

            try {
                string sendemail = GetPortalOption("MAIL_SEND_ADDRESS");
                var credentials = new NetworkCredential(sendemail, GetPortalOption("MAIL_SEND_PASSWORD"));
                
                var mail = new MailMessage() {
                    From = new MailAddress(sendemail),
                    Subject = subject,
                    Body = body
                };

                mail.To.Add(new MailAddress(to_mail));

                var client = new SmtpClient() {
                    Port = Convert.ToInt32(GetPortalOption("MAIL_PORT")),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = GetPortalOption("MAIL_HOST"),
                    EnableSsl = true,
                    Credentials = credentials
                };

        
                client.Send(mail);
                return true;
            } catch (Exception) {
                return false;
            }
        }

        public bool LogUserInByUserID(string userid,string pwd) {



            return true;
        }

    }
}