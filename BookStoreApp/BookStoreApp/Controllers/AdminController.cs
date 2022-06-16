using BussinessLayer.Interfaces;
using DatabaseLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BookStoreApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminBL adminBL;
        public AdminController( IAdminBL adminBL )
        {
            this.adminBL = adminBL;
        }

        [HttpPost("Login")]
        public ActionResult AdminLogin(AdminLogin adminLogin)
        {
            try
            {
                
                var res = adminBL.AdminLogin(adminLogin);
                if (res != null)
                {
                    return Ok(new { success = true, message = "Login done sucessfully", data = res });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Faild to Login" });
                }
            }
            catch (Exception Ex)
            {

                return NotFound(new { Success = false, message = Ex.Message }) ;
            }
        }
    }
}
