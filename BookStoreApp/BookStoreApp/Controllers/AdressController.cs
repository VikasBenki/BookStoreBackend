using BussinessLayer.Interfaces;
using DatabaseLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BookStoreApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdressController : ControllerBase
    {
        private readonly IAdressBL addressBL;
        public AdressController(IAdressBL addressBL)
        {
            this.addressBL = addressBL;
        }
        [Authorize(Roles = Roles.User)]
        [HttpPost("Add")]
        public ActionResult AddAddress(AddAddress addAddress)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var res = addressBL.AddAddress(addAddress, userId);
                if (res != null)
                {
                    return Ok(new { success = true, message = "Address Added sucessfully", data = res });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Faild to Add Address" });
                }
            }
            catch (System.Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }
        [Authorize(Roles = Roles.User)]
        [HttpPut("Update")]
        public IActionResult UpdateAddress(AddressModel updateAddress)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var res = addressBL.UpdateAddress(updateAddress, userId);
                if (res != null)
                {
                    return Ok(new { success = true, message = "Address Updated sucessfully", data = res });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Faild to Update Address" });
                }
            }
            catch (System.Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }
        [Authorize(Roles = Roles.User)]
        [HttpDelete("Delete")]
        public IActionResult DeleteAddress(int addressId)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var res = addressBL.DeleteAddress(addressId, userId);
                if (res.ToLower().Contains("success"))
                {
                    return Ok(new { success = true, message = "Address Deleted sucessfully" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Faild to Delete Address" });
                }
            }
            catch (System.Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [Authorize(Roles = Roles.User)]
        [HttpGet("Get")]
        public IActionResult GetAddressById(int typeId, int addressId)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var res = addressBL.GetAddressById(typeId, addressId, userId);
                if (res != null)
                {
                    return Ok(new { success = true, message = "Get Address sucessfully", data = res });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Faild to Get Address" });
                }
            }
            catch (System.Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [Authorize(Roles = Roles.User)]
        [HttpGet("GetAll")]
        public IActionResult GetAllAddresses()
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var res = addressBL.GetAllAddresses(userId);
                if (res != null)
                {
                    return Ok(new { success = true, message = "Get All Address sucessfully", data = res });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Failed to Get All Addresses" });
                }
            }
            catch (System.Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
