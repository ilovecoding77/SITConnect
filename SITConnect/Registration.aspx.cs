using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;

namespace SITConnect
{
    public partial class Registration : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SitDB"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void createAccount()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@FirstName,@LastName,@CreditCard,@Email,@PasswordHash,@DOB,@PasswordSalt,@Status)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@FirstName", tb_firstname.Text.Trim());
                            cmd.Parameters.AddWithValue("@LastName", tb_lastname.Text.Trim());
                            cmd.Parameters.AddWithValue("@CreditCard", encryptData(tb_creditNo.Text.Trim()));
                            cmd.Parameters.AddWithValue("@Email", tb_email.Text.Trim());
                            cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                            cmd.Parameters.AddWithValue("@DOB", Convert.ToDateTime(tb_dob.Text.Trim()));
                            cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                            cmd.Parameters.AddWithValue("@Status","Open");
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            lbl_registration.Text = "Success";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            int scores = checkPassword(tb_pwd.Text);
            string status = "";
            if (!Regex.IsMatch(tb_email.Text.Trim(), @"^\w+([-+.']\w+)@\w+([-.]\w+).\w+([-.]\w+)*$"))
            {
                lbl_emailcheck.Text = "Invalid Email Format";
            }

            switch (scores)
            {
                case 1:
                    status = "Very Weak, Make Password Stronger";
                    break;
                case 2:
                    status = "Weak, Make Password Stronger";
                    break;
                case 3:
                    status = "Medium, Make Password Stronger";
                    break;
                case 4:
                    status = "Strong";
                    break;
                case 5:
                    status = "Excellent";
                    break;
                default:
                    break;
            }
            lbl_pwdchecker.Text = "Status : " + status;
            if (scores < 4)
            {
                lbl_pwdchecker.ForeColor = Color.Red;
                lbl_registration.Text = "Failed to register";
                return;
            }
            else
            {
                tb_cfpwd.Text = HttpUtility.HtmlEncode(tb_cfpwd.Text);
                tb_creditNo.Text = HttpUtility.HtmlEncode(tb_creditNo.Text);
                tb_email.Text = HttpUtility.HtmlEncode(tb_email.Text);
                tb_dob.Text = HttpUtility.HtmlEncode(tb_dob.Text);
                tb_firstname.Text = HttpUtility.HtmlEncode(tb_firstname.Text);
                tb_lastname.Text = HttpUtility.HtmlEncode(tb_lastname.Text);
                tb_pwd.Text = HttpUtility.HtmlEncode(tb_pwd.Text);

                SqlConnection connection = new SqlConnection(MYDBConnectionString);
                string sql = string.Format("select EMAIL FROM ACCOUNT where Email=@Email");
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@Email", tb_email.Text.Trim());
                connection.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    lbl_emailcheck.Text = "Email Taken";
                    connection.Close();
                    lbl_registration.Text = "Failed to register";
                }
                else
                {
                    connection.Close();
                    lbl_emailcheck.Text = "";
                    // string pwd = get value from your Textbox
                    string pwd = tb_pwd.Text.ToString().Trim(); ;
                    //Generate random "salt"
                    RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                    byte[] saltByte = new byte[8];
                    //Fills array of bytes with a cryptographically strong sequence of random values.
                    rng.GetBytes(saltByte);
                    salt = Convert.ToBase64String(saltByte);
                    SHA512Managed hashing = new SHA512Managed();
                    string pwdWithSalt = pwd + salt;
                    byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
                    byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                    finalHash = Convert.ToBase64String(hashWithSalt);
                    RijndaelManaged cipher = new RijndaelManaged();
                    cipher.GenerateKey();
                    Key = cipher.Key;
                    IV = cipher.IV;

                    createAccount();
                }
                
            }
            
            
        }
        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                //ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0,
               plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return cipherText;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            
        }
        private int checkPassword(string password)
        {
            int score = 0;
            if (password.Length < 8)
            {
                return 1;
            }
            else
            {
                score = 1;
            }
            if (Regex.IsMatch(password, "[a-z]"))
            {
                score++;
            }
            if (Regex.IsMatch(password, "[A-Z]"))
            {
                score++;
            }
            if (Regex.IsMatch(password, "[0-9]"))
            {
                score++;
            }
            if (Regex.IsMatch(password, "[^A-Za-z0-9]"))
            {
                score++;
            }
            return score;
        }
    }
}