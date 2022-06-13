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
    public class WishListRL : IWishListRL
    {
        private readonly IConfiguration configuration;
        public WishListRL(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // ------ Method For Add To WishList -----------

        public string AddToWishList(int bookId, int userId)
        {
            using(SqlConnection conect = new SqlConnection(configuration["ConnectionStrings:BookStore"]))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_AddTo_WishList", conect);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@BookId", bookId);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    conect.Open();
                    var result = cmd.ExecuteNonQuery();
                    conect.Close();

                    if(result != 0)
                    {
                        return "Book Added to WishList successfully";
                    }
                    else
                    {
                        return "Failed to add to the wishList";
                    }
                }
                catch (Exception Ex)
                {

                    throw Ex;
                }
            }
        }

        public string RemoveFromWishList(int wishListId, int userId)
        {
            using (SqlConnection con = new SqlConnection(configuration["ConnectionStrings:BookStore"]))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_Remove_FromWishList", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@WishListId", wishListId);

                    con.Open();
                    var result = cmd.ExecuteNonQuery();
                    con.Close();

                    if (result != 0)
                    {
                        return "Item Removed from WishList Successfully";
                    }
                    else
                    {
                        return "Failed to Remove item from WishList";
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

        public List<WishListResponse> GetAllWishList(int userId)
        {
            using (SqlConnection con = new SqlConnection(configuration["ConnectionStrings:BookStore"]))
            {
                try
                {
                    List<WishListResponse> wishListResponse = new List<WishListResponse>();
                    SqlCommand cmd = new SqlCommand("SP_GetAll_FromWishList", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserId", userId);

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            WishListResponse wishList = new WishListResponse();
                            WishListResponse temp;
                            temp = ReadData(wishList, rdr);
                            wishListResponse.Add(temp);
                        }
                        con.Close();
                        return wishListResponse;
                    }
                    else
                    {
                        con.Close();
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

        public WishListResponse ReadData(WishListResponse wishList, SqlDataReader rdr)
        {
            wishList.BookId = Convert.ToInt32(rdr["BookId"] == DBNull.Value ? default : rdr["BookId"]);
            wishList.UserId = Convert.ToInt32(rdr["UserId"] == DBNull.Value ? default : rdr["UserId"]);
            wishList.WishListId = Convert.ToInt32(rdr["WishListId"] == DBNull.Value ? default : rdr["WishListId"]);
            wishList.BookName = Convert.ToString(rdr["Book_Name"] == DBNull.Value ? default : rdr["Book_Name"]);
            wishList.AuthorName = Convert.ToString(rdr["Author_Name"] == DBNull.Value ? default : rdr["Author_Name"]);
            wishList.BookImage = Convert.ToString(rdr["Book_Image"] == DBNull.Value ? default : rdr["Book_Image"]);
            wishList.DiscountPrice = Convert.ToDouble(rdr["Discount_Price"] == DBNull.Value ? default : rdr["Discount_Price"]);
            wishList.ActualPrice = Convert.ToDouble(rdr["Actual_Price"] == DBNull.Value ? default : rdr["Actual_Price"]);

            return wishList;
        }

    }
}
