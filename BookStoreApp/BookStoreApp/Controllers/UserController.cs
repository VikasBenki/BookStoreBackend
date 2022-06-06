using BussinessLayer.Interfaces;
using DatabaseLayer.UserModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace BookStoreApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserBL userBL;
        public UserController(IUserBL userBL)
        {
            this.userBL = userBL;
        }
        [HttpPost("Register")]
        public ActionResult Registeration(UserReg userReg)
        {
            try
            {
                var result= this.userBL.Registration(userReg);
                if (result != null)
                {
                    return this.Ok(new { success = true, message = $"Register Successful ", Response= result });

                }
                return this.BadRequest(new { success = false, message = $"Register Failed" });

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost("Login")]
        public ActionResult UserLogin(UserLogin userLogin)
        {
            try
            {
                var result = this.userBL.UserLogin(userLogin);
                if (result != null)
                {
                    return this.Ok(new { success = true, message = $"login Successfull ", Response = result });

                }
                return this.BadRequest(new { success = false, message = $"Login failed" });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost("ForgetPassword")]
        public ActionResult ForgotPassword(ForgotPassword forgotPassword)
        {
            try
            {
                var result = this.userBL.ForgotPassword(forgotPassword);
                if (result != false)
                {
                    return this.Ok(new { success = true, message = $"mail sent successfull ", Response = result });

                }
                return this.BadRequest(new { success = false, message = $"enter valid mail" });
            }
            catch (Exception Ex)
            {

                throw Ex;
            }  
        }
        [Authorize]
        [HttpPatch("ChangePassword")]
        public ActionResult ResetPassword(ResetPassword resetPassword)
        {
            try
            {
               
                var EmailId = User.FindFirst("EmailId").Value;
                var res = userBL.ResetPassword(resetPassword, EmailId);

                if (res.ToLower().Contains("success"))
                {
                    return Ok(new { success = true, message = res, });
                }
                else
                {
                    return BadRequest(new { success = true, message = res, });
                }
            }
            catch (System.Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

    }
}
