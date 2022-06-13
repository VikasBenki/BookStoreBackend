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
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderBL ordersBL;
        public OrderController(IOrderBL ordersBL)
        {
            this.ordersBL = ordersBL;
        }

        [HttpPost("Add")]
        public IActionResult AddOrder(AddOrder addOrder)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var res = ordersBL.AddOrder(addOrder, userId);
                if (res != null)
                {
                    return Ok(new { success = true, message = "Ordered sucessfully", data = res });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Faild to Order" });
                }
            }
            catch (System.Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("GetAll")]
        public IActionResult GetAllOrders()
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var res = ordersBL.GetAllOrders(userId);
                if (res != null)
                {
                    return Ok(new { success = true, message = "Orders Retrieved sucessfully", data = res });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Faild to Retrieve Orders" });
                }
            }
            catch (System.Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

    }
}
