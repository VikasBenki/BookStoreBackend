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

    public class BookController : ControllerBase
    {
       private readonly IBookBL bookBL;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        private string KeyName = "VikasBenki";
        public BookController(IBookBL bookBL, IDistributedCache distributedCache, IMemoryCache memoryCache)
        {
            this.bookBL = bookBL;
            this.distributedCache = distributedCache;
            this.memoryCache = memoryCache;
        }
        [Authorize(Roles =Roles.Admin)]
        [HttpPost("Add")]
        public ActionResult AddBook(AddBook addBook)
        {
            try
            {
                var result = bookBL.AddBook(addBook);
                if (result != null)
                {
                    return Created(" ", new { success = true, message = " Book added successfully", Response = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "failed to Add the book" });
                }

            }
            catch (Exception ex)
            {

                return NotFound(new { success = false, message = ex.Message });
            }
        }

        //---------------Update Book--------
        [Authorize(Roles = Roles.Admin)]
        [HttpPut("UpdateBook")]
        public ActionResult UpdateBook(BookModel updateBook)
        {
            try
            {
                var result = bookBL.UpdateBook(updateBook);
                if (result != null)
                {
                    return this.Ok(new { success = true, message = " Book updated successfully", response = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Book Updation Failed" });
                }
            }
            catch (Exception ex)
            {

                return NotFound(new { success = false, message = ex.Message });
            }

        }

        //-------------------------- Delete Book ----------------
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("Delete")]
        public ActionResult DeleteBook(int bookId)
        {
            try
            {
                var result = bookBL.DeleteBook(bookId);
                if (result != null)
                {
                    return this.Ok(new { success = true, message = "Book Deleted successfully", Response = result });
                }
                else
                {
                    return this.BadRequest(new { success = false, message = " Failed to delete the Book" });
                }
            }
            catch (Exception Ex)
            {

                return NotFound(new { success = false, message = Ex.Message });
            }
        }
        //-----------------------------Get all books Method -------------------------------
        [Authorize(Roles = Roles.User)]
        [HttpGet("GetAllBooks")]
        public ActionResult GetAllBooks()
        {
            try
            {
                var result = this.bookBL.GetAllBooks();
                if (result != null)
                {
                    return this.Ok(new { success = true, message = "got all books successfuly", Response = result });
                }
                else
                {
                    return this.BadRequest(new { success = false, message = "Failed to get all Books" });
                }
            }
            catch (Exception EX)
            {

                return NotFound(new { success = false, message = EX.Message });
            }
        }

        //----------- Get Book by BookId ---------------

        [HttpGet("GetBookById")]
        public ActionResult GetBookById(int bookId)
        {
            try
            {
                var result = bookBL.GetBookById(bookId);
                if (result != null)
                {
                    return this.Ok(new { success = true, message = "Book got successfully ", Response = result });
                }
                else
                {
                    return this.BadRequest(new { success = false, message = " Failed to get Book" });
                }
            }
            catch (Exception Ex)
            {

                return NotFound(new { success = false, message = Ex.Message });
            }
        }

        [HttpGet("GetallBooksByRedis")]
        public async Task<ActionResult> GetallBooksRedisCache()
        {
            try
            {
                string serializeBookList;
                var BookList = new List<BookModel>();
                var redisBookList = await distributedCache.GetAsync(KeyName);
                if (redisBookList != null)
                {
                    serializeBookList = Encoding.UTF8.GetString(redisBookList);
                    BookList = JsonConvert.DeserializeObject<List<BookModel>>(serializeBookList);
              }
                else
                {
                    BookList = this.bookBL.GetAllBooks().ToList();
                    serializeBookList = JsonConvert.SerializeObject(BookList);
                    redisBookList = Encoding.UTF8.GetBytes(serializeBookList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    await distributedCache.SetAsync(KeyName, redisBookList, options);
                }
                return this.Ok(new { success = true, message = "Get AllBooks successful!!!", data = BookList });
            }
            catch (Exception Ex)
            {

                return NotFound(new { success = false, message = Ex.Message });
            }
        }
    }
}
