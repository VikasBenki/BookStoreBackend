using DatabaseLayer.UserModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BussinessLayer.Interfaces
{
    public interface IBookBL
    {
        BookModel AddBook(AddBook addBook);
        BookModel UpdateBook(BookModel updateBook);
        string DeleteBook(int bookId);
        List<BookModel> GetAllBooks();
        BookModel GetBookById(int bookId);
    }
}
