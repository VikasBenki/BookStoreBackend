using DatabaseLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BussinessLayer.Interfaces
{
    public interface IAdminBL
    {
        public AdminLoginResponse AdminLogin(AdminLogin adminLogin);
    }
}
