using DatabaseLayer.UserModel;
using Experimental.System.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Xamarin.Essentials;

namespace RepositoryLayer.Services
{
    public class UserRL : IUserRL
    {
        private readonly IConfiguration configuration;
        public UserRL(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public UserReg Registration(UserReg userReg)
        {
            using (SqlConnection conect = new SqlConnection(configuration["ConnectionStrings:BookStore"]))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_User_Registration", conect);
                    cmd.CommandType = CommandType.StoredProcedure;
                    var encryptedPassword = EncryptPassword(userReg.Password);

                    cmd.Parameters.AddWithValue("@FullName", userReg.FullName);
                    cmd.Parameters.AddWithValue("EmailId", userReg.EmailId);
                    cmd.Parameters.AddWithValue("@Password", encryptedPassword);
                    cmd.Parameters.AddWithValue("@MobileNumber", userReg.MobileNumber);

                    conect.Open();
                    var res = cmd.ExecuteNonQuery();
                    conect.Close();
                    if (res != 0)
                    {
                        return userReg;
                    }
                    else
                    {
                        return null;
                    }


                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }

        }
        public static string EncryptPassword(string password)
        {
            try
            {
                if (string.IsNullOrEmpty(password))
                {
                    return null;
                }
                else
                {
                    byte[] b = Encoding.ASCII.GetBytes(password);
                    string encrypted = Convert.ToBase64String(b);
                    return encrypted;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string DecryptedPassword(string encryptedPassword)
        {
            byte[] b;
            string decrypted;
            try
            {
                if (string.IsNullOrEmpty(encryptedPassword))
                {
                    return null;
                }
                else
                {
                    b = Convert.FromBase64String(encryptedPassword);
                    decrypted = Encoding.ASCII.GetString(b);
                    return decrypted;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public LoginResponse UserLogin(UserLogin userLogin)
        {

            using (SqlConnection conect = new SqlConnection(configuration["ConnectionStrings:BookStore"]))
            {

                try
                {
                    if (string.IsNullOrEmpty(userLogin.EmailId) || string.IsNullOrEmpty(userLogin.Password))
                        return null;
                    else
                    {

                        SqlCommand cmd = new SqlCommand("SP_User_Login", conect);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@EmailId", userLogin.EmailId);
                        //cmd.Parameters.AddWithValue("@Password", userLogin.Password);
                        conect.Open();

                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            LoginResponse loginResponse = new LoginResponse();

                            while (reader.Read())
                            {
                                var userId = Convert.ToInt64(reader["UserId"] == DBNull.Value ? default : reader["UserId"]);
                                var password = Convert.ToString(reader["Password"] == DBNull.Value ? default : reader["Password"]);


                                loginResponse.FullName = Convert.ToString(reader["Full_Name"] == DBNull.Value ? default : reader["Full_Name"]);
                                loginResponse.EmailId = Convert.ToString(reader["Email_Id"] == DBNull.Value ? default : reader["Email_Id"]);
                                loginResponse.MobileNumber = Convert.ToInt64(reader["Mobile_Number"] == DBNull.Value ? default : reader["Mobile_Number"]);


                                var decryptedPassword = DecryptedPassword(password);
                                if (decryptedPassword == userLogin.Password)
                                {
                                    loginResponse.Token = GetJWTToken(userLogin.EmailId, userId);

                                    return loginResponse;
                                }
                                else
                                {
                                    return null;
                                }
                            }
                        }
                        else
                        {
                            return null;

                        }
                        conect.Close();
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
            return default;

        }

        public string GetJWTToken(string EmailId, long UserId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes("THIS_IS_MY_KEY_TO_GENERATE_TOKEN");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("EmailId", EmailId),
                    new Claim("UserId",UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),

                SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public bool ForgotPassword(ForgotPassword forgotPassword)
        {
            using (SqlConnection con = new SqlConnection(configuration["ConnectionStrings:BookStore"]))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_User_ForgotPassword", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@EmailId", forgotPassword.EmailId);

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            var userId = Convert.ToInt64(rdr["UserId"] == DBNull.Value ? default : rdr["UserId"]);

                            // Addd message Queue
                            MessageQueue queue;
                            if (MessageQueue.Exists(@".\Private$\BookQueue"))
                            {
                                queue = new MessageQueue(@".\Private$\BookQueue");
                            }
                            else
                            {
                                queue = MessageQueue.Create(@".\Private$\BookQueue");
                            }
                            Message myMessage = new Message();
                            myMessage.Formatter = new BinaryMessageFormatter();
                            myMessage.Body = GetJWTToken(forgotPassword.EmailId, userId);
                            queue.Send(myMessage);
                            Message msg = queue.Receive();
                            msg.Formatter = new BinaryMessageFormatter();
                            MailService.SendMail(forgotPassword.EmailId, myMessage.Body.ToString());
                            queue.ReceiveCompleted += new ReceiveCompletedEventHandler(msmqQueue_ReceiveCompleted);
                            queue.BeginReceive();
                            queue.Close();
                            return true;
                        }
                    }
                    else
                        return false;
                    con.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return default;

        }


        private void msmqQueue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            try
            {
                MessageQueue queue = (MessageQueue)sender;
                Message msg = queue.EndReceive(e.AsyncResult);
                MailService.SendMail(e.Message.ToString(), GenerateToken(e.Message.ToString()));
                queue.BeginReceive();
            }
            catch (MessageQueueException ex)
            {
                if (ex.MessageQueueErrorCode ==
                    MessageQueueErrorCode.AccessDenied)
                {
                    Console.WriteLine("Access is denied. " +
                        "Queue might be a system queue.");
                }
                // Handle other sources of MessageQueueException.
            }

        }

        //GENERATE TOKEN WITH EMAIL
        public string GenerateToken(string email)
        {
            if (email == null)
            {
                return null;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes("THIS_IS_MY_KEY_TO_GENERATE_TOKEN");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Email",email)
                }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public string ResetPassword(ResetPassword resetPassword, string EmailId)
        {
            try
            {
                using (SqlConnection conect = new SqlConnection(configuration["ConnectionStrings:BookStore"]))
                {
                    if (resetPassword.Password == resetPassword.ConfirmPassword)
                    {
                        var encryptPassword = EncryptPassword(resetPassword.Password);

                        SqlCommand cmd = new SqlCommand("SP_User_ResetPassword", conect);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@EmailId", EmailId);
                        cmd.Parameters.AddWithValue("@Password", EncryptPassword(resetPassword.Password));
                        conect.Open();
                        var result = cmd.ExecuteNonQuery();
                        conect.Close();
                        if (result > 0)
                        {
                            return "Congratulations! Your password has been changed successfully";
                        }
                        else
                            return "Failed to reset your password";
                    }
                    else
                        return "Make Sure your Passwords Match";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
