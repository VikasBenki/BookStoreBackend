using BussinessLayer.Interfaces;
using DatabaseLayer.UserModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartBL cartBL;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        private string KeyName = "VikasBenki";
        public CartController(ICartBL cartBL, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            this.cartBL = cartBL;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
        }
        [Authorize(Roles = Roles.User)]
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
        [Authorize(Roles = Roles.User)]
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
        [Authorize(Roles = Roles.User)]
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

        [Authorize(Roles = Roles.User)]
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

        [Authorize(Roles = Roles.User)]
        [HttpGet("GetallBooksinCartByRedis")]
        public async Task<ActionResult> GetallBooksInCartRedisCache(int userId)
        {
            try
            {
                string serializeCartList;
                var CartList = new List<CartResponse>();
                var redisCartList = await distributedCache.GetAsync(KeyName);
                if (redisCartList != null)
                {
                    serializeCartList = Encoding.UTF8.GetString(redisCartList);
                    CartList = JsonConvert.DeserializeObject<List<CartResponse>>(serializeCartList);
                }
                else
                {
                    CartList = this.cartBL.GetAllCart(userId).ToList();
                    serializeCartList = JsonConvert.SerializeObject(CartList);
                    redisCartList = Encoding.UTF8.GetBytes(serializeCartList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    await distributedCache.SetAsync(KeyName, redisCartList, options);
                }
                return this.Ok(new { success = true, message = "Get AllBooks from Cart successful!!!", data = redisCartList });
            }
            catch (Exception Ex)
            {

                return NotFound(new { success = false, message = Ex.Message });
            }
        }
    }
}
