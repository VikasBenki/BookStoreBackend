using DatabaseLayer.UserModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRL
    {
        UserReg Registration(UserReg userReg);
        LoginResponse UserLogin(UserLogin userLogin);

        bool ForgotPassword(ForgotPassword forgotPassword);
        string ResetPassword(ResetPassword resetPassword, string EmailId);
    }
}
