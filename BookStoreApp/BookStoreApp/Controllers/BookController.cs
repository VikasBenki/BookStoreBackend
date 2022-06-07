using BussinessLayer.Interfaces;
using DatabaseLayer.UserModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BookStoreApp.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class BookController : ControllerBase
    {
        private readonly IBookBL bookBL;
        public BookController(IBookBL bookBL)
        {
            this.bookBL = bookBL;
        }   

        [HttpPost("Add")]
        public ActionResult AddBook(AddBook addBook)
        {
            try
            {
                var result= bookBL.AddBook(addBook);
                if(result!=null)
                {
                    return Created(" ", new { success = true , message = " Book added successfully", Response = result});
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
        [HttpPut("UpdateBook")]
        public ActionResult UpdateBook(BookModel updateBook)
        {
            try
            {
                var result = bookBL.UpdateBook(updateBook);
                if(result != null)
                {
                    return this.Ok(new { success= true , message = " Book updated successfully", response = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Book Updation Failed" });
                }
            }
            catch (Exception ex)
            {

                return NotFound(new { success=false, message= ex.Message});
            }
            
        }

        //-------------------------- Delete Book ----------------
        [HttpDelete("Delete")]
        public ActionResult DeleteBook(int bookId)
        {
            try
            {
                var result = bookBL.DeleteBook(bookId);
                if(result != null)
                {
                    return this.Ok(new {success = true , message = "Book Deleted successfully", Response=result});
                }
                else
                {
                    return this.BadRequest(new { success = false, message = " Failed to delete the Book" }); 
                }
            }
            catch (Exception Ex)
            {

                return NotFound(new {success =false, message = Ex.Message});
            }
        }
        //-----------------------------Get all books Method -------------------------------

        [HttpGet("GetAllBooks")]
        public ActionResult GetAllBooks()
        {
            try
            {
                var result = this.bookBL.GetAllBooks();
                if(result != null )
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

              return NotFound(new {success=false, message=EX.Message});
            }
        }

        //----------- Get Book by BookId ---------------
        [HttpGet("GetBookById")]
        public ActionResult GetBookById(int bookId)
        {
            try
            {
                var result = bookBL.GetBookById(bookId);
                if (result !=null)
                {
                    return this.Ok(new { success = true, message = "Book got successfully " , Response =result});
                }
                else
                {
                    return this.BadRequest(new { success = false, message = " Failed to get Book" });
                }
            }
            catch (Exception Ex)
            {

                return NotFound(new {success=false, message=Ex.Message});
            }
        }
    }
}
