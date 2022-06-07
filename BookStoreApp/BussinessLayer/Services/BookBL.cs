using BussinessLayer.Interfaces;
using DatabaseLayer.UserModel;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BussinessLayer.Services
{
    public class BookBL: IBookBL
    {
        private readonly IBookRL BookRL;

        public BookBL(IBookRL bookRL)
        {
            this.BookRL = bookRL;
        }

        public BookModel AddBook(AddBook addBook)
        {
            try
            {
                return this.BookRL.AddBook(addBook);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //-----------------Update book-------

        public BookModel UpdateBook(BookModel updateBook)
        {
            try
            {
                return this.BookRL.UpdateBook(updateBook);
            }
            catch (Exception ex )
            {

                throw ex;
            }
            
        }

        //-------------Delete the Book--------
        public string DeleteBook(int bookId)
        {
            try
            {
                return this.BookRL.DeleteBook(bookId);
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
        }

        //-------------Get all books ------
        public List<BookModel> GetAllBooks()
        {

            try
            {
                return this.BookRL.GetAllBooks();
            }
            catch (Exception Ex)
            {

                throw Ex;
            }       
        }

        //------------- get Book by book Id ----------------

        public BookModel GetBookById(int bookId)
        {
            try
            {
                return this.BookRL.GetBookById(bookId);
            }
            catch (Exception EX)
            {

                throw EX;
            }
        }
    }
}
