using DatabaseLayer.UserModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface IBookRL
    {
         BookModel AddBook(AddBook addBook);
        BookModel UpdateBook(BookModel updateBook);
        string DeleteBook(int bookId);

        List<BookModel> GetAllBooks();
        BookModel GetBookById(int bookId);
    }
}
