using BussinessLayer.Interfaces;
using DatabaseLayer.Models;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BussinessLayer.Services
{
    public class AdminBL : IAdminBL
    {
        IAdminRL adminRL;
        public AdminBL(IAdminRL adminRL)
        {
            this.adminRL = adminRL;
        }
        public AdminLoginResponse AdminLogin(AdminLogin adminLogin)
        {
            try
            {
                return this.adminRL.AdminLogin(adminLogin);
            }
            catch (Exception EX)
            {

                throw EX;
            }
        }
    }
}
