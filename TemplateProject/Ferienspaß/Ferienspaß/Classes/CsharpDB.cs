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

        /// <summary>
        /// Checks if current Date is inbetween Settings From and Till login
        /// </summary>
        /// <returns>bool</returns>
        public static bool IsUserAllowedToLogin() {
            DateTime[] dates = GetPortalLoginDates();
            return (DateTime.Now <= dates[1] && DateTime.Now >= dates[0]);
        }


        public static DateTime[] GetPortalLoginDates() {
            try {
                CsharpDB db = new CsharpDB();
                string from = db.GetPortalOption("USER_LOGIN_FROM");
                string till = db.GetPortalOption("USER_LOGIN_TILL");
                DateTime fromDT = DateTime.Parse(from);
                DateTime tillDT = DateTime.Parse(till);

                return new DateTime[2] {fromDT,tillDT };

            } catch (Exception ex) { return new DateTime[2] { DateTime.MinValue, DateTime.MaxValue }; }
        }

        public static bool CheckPasswordRequirements(string pwd) {
            if (pwd.Length < 1) return false;
            //else if (!pwd.Any(c => char.IsDigit(c))) return false;
            //else if (!pwd.Any(c => char.IsUpper(c))) return false;
            return true;

        }

        // OBSOLETE


       /* public bool SendMail(string to_mail, string to_name, string subject, string body) {

            try {
                string sendemail = GetPortalOption("MAIL_SEND_ADDRESS");
                var credentials = new NetworkCredential(sendemail, GetPortalOption("MAIL_SEND_PASSWORD"));
                
                var mail = new MailMessage() {

                    From = new MailAddress(sendemail),
                    Subject = subject,
                    Body = body
                };

                mail.To.Add(new MailAddress(to_mail, to_name));

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
        }*/




        public bool SendHTMLEmail(string to_mail, string to_name, string subject, string mainText, bool needButton, string btnText, string url, string hinweis, string schlussformel, string kleingedruckt) {
            try {
                string sendemail = GetPortalOption("MAIL_SEND_ADDRESS");
                var credentials = new NetworkCredential(sendemail, GetPortalOption("MAIL_SEND_PASSWORD"));

                var mail = new MailMessage() {
                    From = new MailAddress(sendemail),
                    Subject = subject,
                    IsBodyHtml = true,
                    Body = GetHTMLEmailContent(subject,to_name, needButton, mainText, url, btnText, hinweis, schlussformel, kleingedruckt)
                };

                mail.To.Add(new MailAddress(to_mail,to_name));

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
            } catch (Exception ex) {
                return false;
            }
        }

        internal void SendMail(string v1, string v2, string v3, string body)
        {
            throw new NotImplementedException();
        }

        public static string GetHTMLEmailContent(string action,string name, bool needButton, string mainText, string url, string btnText, string hinweis, string schlussformel, string kleingedruckt) {

            CsharpDB db = new CsharpDB();
            string hinweisField = @"<strong>ACHTUNG:</strong>" + hinweis;
            if (hinweis.Length <= 0) hinweisField = "";

            string button = @"<tr>
			<td align=""left"" bgcolor=""#ffffff"">
			  <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
			<tr>
			  <td align=""center"" bgcolor=""#ffffff"" style= ""padding: 12px;"">
					<table border=""0"" cellpadding=""0"" cellspacing=""0"">
				   <tr>
					 <td align=""center"" bgcolor=""#1a82e2"" style= ""border-radius: 6px;"">
						  <a href=" + url + @" target=""_blank"" style=""display: inline-block; padding: 16px 36px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; color: #ffffff; text-decoration: none; border-radius: 6px;"">" + btnText + @"</a>
                        </td>
					  </tr>
					</table>
				  </td>
				</tr>
			  </table>
			</td>
		  </tr>
		  <tr>
			<td align=""left"" bgcolor=""#ffffff"" style= ""padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;"">
			  <p style=""margin: 0; "">Sollte der Knopf nicht funktionieren, verwenden Sie den folgenden Link:</p>
			  <p style=""margin: 0; ""><a href=" + url + @" target=""_blank"">" + url + @"</a></p>
			</td>
		  </tr>";

            if (!needButton) {
                button = "";
            }


            return @"<!DOCTYPE html>
<html>
<head>

  <meta charset=""utf-8"">
  <meta http-equiv=""x-ua-compatible"" content=""ie=edge"">
  <title>" + action + @"</title>
  <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
  <style type=""text/css"">

  @media screen {
				@font-face {
					font-family: 'Source Sans Pro';
					font-style: normal;
					font-weight: 400;
				src: local('Source Sans Pro Regular'), local('SourceSansPro-Regular'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/ODelI1aHBYDBqgeIAH2zlBM0YzuT7MdOe03otPbuUS0.woff) format('woff');

	}
				@font-face {
					font-family: 'Source Sans Pro';
					font-style: normal;
					font-weight: 700;
				src: local('Source Sans Pro Bold'), local('SourceSansPro-Bold'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/toadOcfmlt9b38dHJxOBGFkQc6VGVFSmCnC_l7QZG60.woff) format('woff');

	}
			}


			body,
  table,
  td,
  a {
				-ms-text-size-adjust: 100%; /* 1 */
				-webkit-text-size-adjust: 100%; /* 2 */
			}
			table,
  td {
				mso-table-rspace: 0pt;
				mso-table-lspace: 0pt;
			}
			img {
				-ms-interpolation-mode: bicubic;
			}

			a[x-apple-data-detectors] {
				font-family: inherit !important;
				font-size: inherit !important;
				font-weight: inherit !important;
				line-height: inherit !important;
			color: inherit !important;
				text-decoration: none !important;
			}

			div[style*=""margin: 16px 0;""] {
			margin: 0 !important;
			}
			body {
			width: 100%!important;
			height: 100%!important;
			padding: 0 !important;
			margin: 0 !important;
			}

			table {
				border-collapse: collapse !important;
			}
			a {
			color: #1a82e2;
  }
			img {
			height: auto;
				line-height: 100%;
				text-decoration: none;
			border: 0;
			outline: none;
			}
  </style>

</head>
<body style=""background-color: #e9ecef;"">


  <div class=""preheader"" style=""display: none; max-width: 0; max-height: 0; overflow: hidden; font-size: 1px; line-height: 1px; color: #fff; opacity: 0;"">
    JobPortal HTL Vöcklabruck, " + action + @"
  </div>


  <table border = ""0"" cellpadding= ""0"" cellspacing= ""0"" width= ""100%"">


	<tr>

	  <td align= ""center"" bgcolor= ""#e9ecef"">

		<table border= ""0"" cellpadding= ""0"" cellspacing= ""0"" width= ""100%"" style= ""max-width: 600px;"">

		  <tr>

			<td align= ""center"" valign= ""top"" style= ""padding: 36px 24px;"">

				<img src= ""https://www.htlvb.at/wp-content/themes/HTL%20Voecklabruck2/Bilder/htlvb.png"" alt= ""Logo"" border= ""0"" width= ""200"" style= ""display: block; width: 200px; max-width: 200px; min-width: 48px;"">

			  </a>

			</td>

		  </tr>

		</table>

	  </td>

	</tr>


	<tr>

	  <td align= ""center"" bgcolor= ""#e9ecef"">

		<table border= ""0"" cellpadding= ""0"" cellspacing= ""0"" width= ""100%"" style= ""max-width: 600px;"">

		  <tr>

			<td align= ""left"" bgcolor= ""#ffffff"" style= ""padding: 36px 24px 0; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; border-top: 3px solid #d4dadf;"">

			  <h1 style= ""margin: 0; font-size: 32px; font-weight: 700; letter-spacing: -1px; line-height: 48px;"">" + action + @"</h1>
            </td>
          </tr>
        </table>
      </td>
    </tr>
 
    <tr>
      <td align = ""center"" bgcolor= ""#e9ecef"">

		<table border= ""0"" cellpadding= ""0"" cellspacing= ""0"" width= ""100%"" style= ""max-width: 600px;"">

		  <tr>

			<td align= ""left"" bgcolor= ""#ffffff"" style= ""padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;"">
              <p style= ""margin: 0; margin-bottom:20px;""><b>Sehr geehrte*r " + name + @"!</b></p>
			  <p style= ""margin: 0;"">" + mainText + @"</p>
              <p style = ""margin: 0;"">" + hinweisField + @"</p>
            </td>
          </tr>

          " + button + @"

          <tr>
            <td align = ""left"" bgcolor= ""#ffffff"" style= ""padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px; border-bottom: 3px solid #d4dadf"">

			  <p style= ""margin: 0;"">" + schlussformel + @"</p>
            </td>
          </tr>
        </table>
      </td>
    </tr>
    <tr>
      <td align = ""center"" bgcolor= ""#e9ecef"" style= ""padding: 24px;"">

		<table border= ""0"" cellpadding= ""0"" cellspacing= ""0"" width= ""100%"" style= ""max-width: 600px;"">


		  <tr>

			<td align= ""center"" bgcolor= ""#e9ecef"" style= ""padding: 12px 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 14px; line-height: 20px; color: #666;"">

			  <p style= ""margin: 0;"">" + kleingedruckt + @"</p>
            </td>
          </tr>
          <tr>
              <td align = ""center"" bgcolor= ""#e9ecef"" style= ""padding: 12px 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 14px; line-height: 20px; color: #666;"">

				  <p style= ""margin: 0;"">Dieses E-Mail wurde automatisch versandt, bitte nicht antworten!</p>
                  <p style = ""margin: 0;"">" + db.GetPortalOption("MAIL_ANSCHRIFT_ABSENDER") + @"</p>
                  <p style = ""margin: 0;"">Ferienspaß © 2020 </p>
              </td>
          </tr>
        </table>
      
      </td>
    </tr>
  </table>
</body>
</html>";




        }



        public bool LogUserInByUserID(string userid,string pwd) {



            return true;
        }


    }
}