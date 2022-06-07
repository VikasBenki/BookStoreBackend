using DatabaseLayer.UserModel;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryLayer.Services
{
    public class BookRL : IBookRL
    {
        private readonly IConfiguration configuration;
        public BookRL(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public BookModel AddBook(AddBook addBook)
        {
            using (SqlConnection conect = new SqlConnection(configuration["ConnectionStrings:BookStore"]))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_Add_Book", conect);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Book_Name", addBook.BookName);
                    cmd.Parameters.AddWithValue("@Author_Name", addBook.AuthorName);
                    cmd.Parameters.AddWithValue("@Book_Image", addBook.BookImage);
                    cmd.Parameters.AddWithValue("@Book_Detail", addBook.BookDetail);
                    cmd.Parameters.AddWithValue("@Discount_Price", addBook.DiscountPrice);
                    cmd.Parameters.AddWithValue("@Actual_Price", addBook.ActualPrice);
                    cmd.Parameters.AddWithValue("@Quantity", addBook.Quantity);
                    cmd.Parameters.AddWithValue("@Rating", addBook.Rating);
                    cmd.Parameters.AddWithValue("@RatingCount", addBook.RatingCount);
                    cmd.Parameters.Add("@BookId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    conect.Open();
                    var result = cmd.ExecuteNonQuery();
                    int bookId = Convert.ToInt32(cmd.Parameters["@BookId"].Value.ToString());
                    conect.Close();
                    if (result != 0)
                    {
                        BookModel bookModel = new BookModel
                        {
                            BookId = bookId,
                            BookName = addBook.BookName,
                            AuthorName = addBook.AuthorName,
                            BookImage = addBook.BookImage,
                            BookDetail = addBook.BookDetail,
                            DiscountPrice = addBook.DiscountPrice,
                            ActualPrice = addBook.ActualPrice,
                            Quantity = addBook.Quantity,
                            Rating = addBook.Rating,
                            RatingCount = addBook.RatingCount
                        };
                        return bookModel;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

        public BookModel UpdateBook(BookModel updateBook)
        {
            using (SqlConnection conect = new SqlConnection(configuration["ConnectionStrings:BookStore"]))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_Update_Book", conect);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@BookId", updateBook.BookId);
                    cmd.Parameters.AddWithValue("@Book_Name", updateBook.BookName);
                    cmd.Parameters.AddWithValue("@Author_Name", updateBook.AuthorName);
                    cmd.Parameters.AddWithValue("@Book_Image", updateBook.BookImage);
                    cmd.Parameters.AddWithValue("@Book_Detail", updateBook.BookDetail);
                    cmd.Parameters.AddWithValue("@Discount_Price", updateBook.DiscountPrice);
                    cmd.Parameters.AddWithValue("@Actual_Price", updateBook.ActualPrice);
                    cmd.Parameters.AddWithValue("@Quantity", updateBook.Quantity);
                    cmd.Parameters.AddWithValue("@Rating", updateBook.Rating);
                    cmd.Parameters.AddWithValue("@RatingCount", updateBook.RatingCount);
                    conect.Open();
                    var result = cmd.ExecuteNonQuery();
                    conect.Close();

                    if (result != 0)
                    {
                        return updateBook;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

        }

        // ------------ Delete method--------------

        public string DeleteBook(int bookId)
        {
            using (SqlConnection conect = new SqlConnection(configuration["ConnectionStrings:BookStore"]))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_Delete_Book", conect);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@BookId", bookId);
                    conect.Open();
                    var result = cmd.ExecuteNonQuery();
                    conect.Close();

                    if (result == 0)
                    {
                        return "failed to delte the book";
                    }
                    else
                    {
                        return "Book deleted successfuly";
                    }

                }
                catch (Exception EX)
                {

                    throw EX;
                }
            }

        }
        public List<BookModel> GetAllBooks()
        {
            using (SqlConnection conect = new SqlConnection(configuration["ConnectionStrings:BookStore"]))
            {
                try
                {
                    List<BookModel> bookModel = new List<BookModel>();
                    SqlCommand cmd = new SqlCommand("SP_GetAll_Books", conect)
                    {
                        CommandType = CommandType.StoredProcedure

                    };

                    conect.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            BookModel book = new BookModel();
                            BookModel temp;
                            temp = ReadData(book, reader);
                            bookModel.Add(temp);

                        }
                        conect.Close();
                        return bookModel;
                    }
                    else
                    {
                        conect.Close();
                        return null;
                    }

                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
        }
        public BookModel ReadData(BookModel bookModel, SqlDataReader reader)
        {
            bookModel.BookId = Convert.ToInt32(reader["BookId"] == DBNull.Value ? default : reader["BookId"]);
            bookModel.BookName = Convert.ToString(reader["Book_Name"] == DBNull.Value ? default : reader["Book_Name"]);
            bookModel.AuthorName = Convert.ToString(reader["Author_Name"] == DBNull.Value ? default : reader["Author_Name"]);
            bookModel.BookImage = Convert.ToString(reader["Book_Image"] == DBNull.Value ? default : reader["Book_Image"]);
            bookModel.BookDetail = Convert.ToString(reader["Book_Detail"] == DBNull.Value ? default : reader["Book_Detail"]);
            bookModel.DiscountPrice = Convert.ToDouble(reader["Discount_Price"] == DBNull.Value ? default : reader["Discount_Price"]);
            bookModel.ActualPrice = Convert.ToDouble(reader["Actual_Price"] == DBNull.Value ? default : reader["Actual_Price"]);
            bookModel.Quantity = Convert.ToInt32(reader["Quantity"] == DBNull.Value ? default : reader["Quantity"]);
            bookModel.Rating = Convert.ToDouble(reader["Rating"] == DBNull.Value ? default : reader["Rating"]);
            bookModel.RatingCount = Convert.ToInt32(reader["RatingCount"] == DBNull.Value ? default : reader["RatingCount"]);

            return bookModel;
        }

        //---  Get Book By BookId-----

        public BookModel GetBookById(int bookId)
        {
            using (SqlConnection conect = new SqlConnection(configuration["ConnectionStrings:BookStore"]))
            {
                try
                {
                    BookModel bookModel = new BookModel();
                    SqlCommand cmd = new SqlCommand("SP_GetBook_ById", conect);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@BookId", bookId);

                    conect.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();


                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            bookModel = ReadData(bookModel, rdr);
                        }
                        conect.Close();
                        return bookModel;
                    }
                    else
                    {
                        conect.Close();
                        return null;
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }
    }
}
