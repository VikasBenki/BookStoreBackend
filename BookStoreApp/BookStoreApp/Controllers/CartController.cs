using BussinessLayer.Interfaces;
using DatabaseLayer.UserModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BookStoreApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartBL cartBL;
        public CartController(ICartBL cartBL)
        {
            this.cartBL = cartBL;
        }

        [HttpPost("AddToCart")]
        public ActionResult AddToCart(AddCart addCart, int userId)
        {
            try
            {
                var result = this.cartBL.AddtoCart(addCart, userId);
                if(result != null)
                {
                    return this.Ok(new { success = true, message = "Added to cart Successfully", Response = result });
                }
                else
                {
                    return this.BadRequest(new { success = false, message = " faile to add to cart" });
                }
            }
            catch (Exception Ex)
            {

                return NotFound(new { success = false, message = Ex.Message });
            }
        }

        // ----------------- Remove from cart ------
        [HttpDelete("Delete")]
        public ActionResult RemoveFromCart(int cartId)
        {
            try
            {
                var result = this.cartBL.RemoveFromCart(cartId);
                if(result != null)
                {
                    return this.Ok(new { succeess = true, message = " Book removed from the cart", Response = result });
                }
                else
                {
                    return this.BadRequest(new { success = false, message = " failed to remove the book from cart" });
                }
            }
            catch (Exception Ex)
            {

                return NotFound(new {success = false, message = Ex.Message});
            }
        }

        [HttpGet("GetAllFromCart")]
        public ActionResult GetAllFromCart(int userId)
        {
            try
            {
                var result = this.cartBL.GetAllCart(userId);
                if(result == null)
                {
                    return this.BadRequest(new { success = false, message = " failed to get from Cart" });
                }
                else
                    return this.Ok(new {success = true, message = " Got all Books from cart", Response = result});
            }
            catch (Exception ex)
            {

                return NotFound(new {success =false, message = ex.Message});
            }
        }

        [HttpPut("Update")]
        public ActionResult UpdateQtyInCart(int cartId, int bookQty)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var result = this.cartBL.UpdateQtyInCart(cartId, bookQty,userId);
                if(result.ToLower().Contains("success"))
                {
                    return Ok(new { Success = true, message = "Updation of Qty is successful " , response =result});
                }
                else
                {
                    return BadRequest(new {success = false, message =" failed to update Quantity"});
                }
            }
            catch (Exception ex)
            {

                return NotFound(new {success = false, message = ex.Message});
            }
        }
    }
}
