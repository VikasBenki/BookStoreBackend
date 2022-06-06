using BussinessLayer.Interfaces;
using DatabaseLayer.UserModel;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BussinessLayer.Services
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL userRL;
        public UserBL(IUserRL userRL)
        {
            this.userRL = userRL;
        }

        public UserReg Registration(UserReg userReg)
        {
            try
            {
                 return this.userRL.Registration(userReg);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public LoginResponse UserLogin(UserLogin userLogin)
        {
            try
            {
                return this.userRL.UserLogin(userLogin);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool ForgotPassword(ForgotPassword forgotPassword)
        {
            try
            {
              return this.userRL.ForgotPassword(forgotPassword);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string ResetPassword(ResetPassword resetPassword, string EmailId)
        {
            try
            {
                return userRL.ResetPassword(resetPassword, EmailId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
