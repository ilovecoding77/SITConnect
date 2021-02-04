using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;


namespace SITConnect
{
    public partial class Login : System.Web.UI.Page
    {
        
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SitDB"].ConnectionString;
        static int attemptcount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        public class MyObject
        {
            public string success { get; set; }
            public List<string>ErrorMessage { get; set; }
        }
        public bool ValidateCaptcha()
        {
            bool result = true;
            string captchaResponse = Request.Form["g-recaptcha-response"];
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=6LfoeUEaAAAAAPiwjSdB4lbkZftRtKJRWV5lRgv3 &response=" + captchaResponse);
            try
            {
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();

                        JavaScriptSerializer js = new JavaScriptSerializer();
                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);
                        result = Convert.ToBoolean(jsonObject.success);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }
        protected string getDBHash(string userid)
        {
            try
            {
                string h = null;
                SqlConnection connection = new SqlConnection(MYDBConnectionString);
                string sql = string.Format("select PasswordHash FROM Account WHERE Email=@USERID");
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@USERID", userid);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            if (reader["PasswordHash"] != null)
                            {
                                if (reader["PasswordHash"] != DBNull.Value)
                                {
                                    h = reader["PasswordHash"].ToString();
                                }
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                finally { connection.Close(); }
                return h;
            }
            catch
            {
                string h = null;
                lblpw.Text = "Enter Valid Input";
                return h;
            }

                
            
            
        }
        protected string getStatus(string userid)
        {
            try
            {
                string s = null;
                SqlConnection connection = new SqlConnection(MYDBConnectionString);
                string sql = string.Format("select STATUS FROM ACCOUNT WHERE Email=@USERID");
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@USERID", userid);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["STATUS"] != null)
                            {
                                if (reader["STATUS"] != DBNull.Value)
                                {
                                    s = reader["STATUS"].ToString();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                finally { connection.Close(); }
                return s;
            }
            catch
            {
                string s = null;
                return s;
            }
        }
        protected string getDBSalt(string userid)
        {
            try
            {
                string s = null;
                SqlConnection connection = new SqlConnection(MYDBConnectionString);
                string sql = string.Format("select PASSWORDSALT FROM ACCOUNT WHERE Email=@USERID");
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@USERID", userid);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["PASSWORDSALT"] != null)
                            {
                                if (reader["PASSWORDSALT"] != DBNull.Value)
                                {
                                    s = reader["PASSWORDSALT"].ToString();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                finally { connection.Close(); }
                return s;
            }
            catch
            {
                string s = null;
                lblpw.Text = "Enter Valid Input";
                return s;
            }

        }
        private void setLockStatus(String userid)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            connection.Open();
            SqlCommand cmd = new SqlCommand();
            String sql = "UPDATE ACCOUNT SET status=@status WHERE Email=@Userid";
            cmd.Parameters.AddWithValue("@Userid", userid);
            cmd.Parameters.AddWithValue("@status", "locked");
            cmd.CommandText = sql;
            cmd.Connection = connection;
            cmd.ExecuteNonQuery();
            connection.Close();
            

        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            //if (ValidateCaptcha())
            //{
                tb_pwd.Text = HttpUtility.HtmlEncode(tb_pwd.Text);
                tb_userid.Text = HttpUtility.HtmlEncode(tb_userid.Text);

                string pwd = tb_pwd.Text.ToString().Trim();
                string userid = tb_userid.Text.ToString().Trim();                
                SHA512Managed hashing = new SHA512Managed();
                string dbHash = getDBHash(userid);
                string dbSalt = getDBSalt(userid);
                string dbstatus = getStatus(userid);
            try
                {
                    if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                    {
                        string pwdWithSalt = pwd + dbSalt;
                        byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                        string userHash = Convert.ToBase64String(hashWithSalt);
                        if (dbstatus == "Open") { 
                            if (userHash.Equals(dbHash))
                            {
                                Session["LoggedIn"] = userid;
                                string guid = Guid.NewGuid().ToString();
                                Session["AuthToken"] = guid;

                                Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                                Response.Redirect("Success.aspx", false);
                            }
                            else
                            {
                                attemptcount = attemptcount + 1;
                                lblpw.Text = "wrong password or username";
                            }
                        }
                        else {
                        lblpw.Text = "Your Account Locked Already";
                        }
                    }
                    if (attemptcount == 3)
                    {
                        lblpw.Text = "Your Account Has Been Locked Due to Three Invalid Attempts - Contact Administrator";                       
                        attemptcount = 0;
                        setLockStatus(userid);
                    }
            }
                catch (Exception ex)
                {
                    lblpw.Text = "Enter valid input";
                    throw new Exception(ex.ToString());
                }

                finally { }
            //}
            
        }

    }
}