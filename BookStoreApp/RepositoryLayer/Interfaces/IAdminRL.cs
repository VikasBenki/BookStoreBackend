using DatabaseLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface IAdminRL
    {
        public AdminLoginResponse AdminLogin(AdminLogin adminLogin);
    }
}
