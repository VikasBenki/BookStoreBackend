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
                    cmd.Parameters.AddWithValue("@EmailId", userReg.EmailId);
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
                                var userId = Convert.ToInt32(reader["UserId"] == DBNull.Value ? default : reader["UserId"]);
                                var password = Convert.ToString(reader["Password"] == DBNull.Value ? default : reader["Password"]);


                                loginResponse.FullName = Convert.ToString(reader["Full_Name"] == DBNull.Value ? default : reader["Full_Name"]);
                                loginResponse.EmailId = Convert.ToString(reader["Email_Id"] == DBNull.Value ? default : reader["Email_Id"]);
                                loginResponse.MobileNumber = Convert.ToInt64(reader["Mobile_Number"] == DBNull.Value ? default : reader["Mobile_Number"]);


                                var decryptedPassword = DecryptedPassword(password);
                                if (decryptedPassword == userLogin.Password)
                                {
                                    loginResponse.Token = GenerateSecurityToken(userLogin.EmailId, userId);

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

        public string GenerateSecurityToken(string emailID, int userId)
        {
            var SecurityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("THIS_IS_MY_KEY_TO_GENERATE_TOKEN"));
            var credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
               new Claim(ClaimTypes.Role, "User"),
                new Claim(ClaimTypes.Email, emailID),
                new Claim("UserId", userId.ToString())
            };
            var token = new JwtSecurityToken(
                this.configuration["Jwt:Issuer"],
                this.configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
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
                            var userId = Convert.ToInt32(rdr["UserId"] == DBNull.Value ? default : rdr["UserId"]);

                            string token = GenerateSecurityToken(forgotPassword.EmailId, userId);
                            new MailService().SendMessage(token);
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
