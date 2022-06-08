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
    public class CartRL : ICartRL
    {
        private readonly IConfiguration configuration;

        public CartRL(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public AddCart AddtoCart(AddCart addCart, int userId)
        {
            using(SqlConnection conect=new SqlConnection(configuration["ConnectionStrings:BookStore"]))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_Add_To_Cart", conect);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@BookId", addCart.BookId);
                    cmd.Parameters.AddWithValue("@BookQuantity", addCart.BookQuantity);

                    conect.Open();
                    var result=cmd.ExecuteNonQuery();
                    conect.Close();
                    if(result != 0)
                    {
                        return addCart;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception Ex)
                {

                    throw Ex;
                }
            }
        }

        //-------- Remove from cart ----------

        public string RemoveFromCart(int cartId)
        {
            using (SqlConnection con = new SqlConnection(configuration["ConnectionStrings:BookStore"]))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_RemoveFrom_Cart", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CartId", cartId);

                    con.Open();
                    var result = cmd.ExecuteNonQuery();
                    con.Close();

                    if (result != 0)
                    {
                        return "Item Removed from cart Successfully";
                    }
                    else
                    {
                        return "Failed to Remove item from cart";
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

        // ------------------- Get all from cart ----------

        public List<CartResponse> GetAllCart(int userId)
        {
            using (SqlConnection con = new SqlConnection(configuration["ConnectionStrings:BookStore"]))
            {
                try
                {
                    List<CartResponse> cartResponses = new List<CartResponse>();
                    SqlCommand cmd = new SqlCommand("SP_GetAll_Cart", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserId", userId);

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            CartResponse cart = new CartResponse();
                            CartResponse temp;
                            temp = ReadData(cart, rdr);
                            cartResponses.Add(temp);
                        }
                        con.Close();
                        return cartResponses;
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

        public CartResponse ReadData(CartResponse cart, SqlDataReader rdr)
        {
            cart.BookId = Convert.ToInt32(rdr["BookId"] == DBNull.Value ? default : rdr["BookId"]);
            cart.UserId = Convert.ToInt32(rdr["UserId"] == DBNull.Value ? default : rdr["UserId"]);
            cart.CartId = Convert.ToInt32(rdr["CartId"] == DBNull.Value ? default : rdr["CartId"]);
            cart.BookName = Convert.ToString(rdr["Book_Name"] == DBNull.Value ? default : rdr["Book_Name"]);
            cart.AuthorName = Convert.ToString(rdr["Author_Name"] == DBNull.Value ? default : rdr["Author_Name"]);
            cart.BookImage = Convert.ToString(rdr["Book_Image"] == DBNull.Value ? default : rdr["Book_Image"]);
            cart.DiscountPrice = Convert.ToDouble(rdr["Discount_Price"] == DBNull.Value ? default : rdr["Discount_Price"]);
            cart.ActualPrice = Convert.ToDouble(rdr["Actual_Price"] == DBNull.Value ? default : rdr["Actual_Price"]);
            cart.BookQuantity = Convert.ToInt32(rdr["Book_Quantity"] == DBNull.Value ? default : rdr["Book_Quantity"]);

            return cart;
        }


        //---------for Update ------------------
        public string UpdateQtyInCart(int cartId, int bookQty, int userId)
        {
            using (SqlConnection con = new SqlConnection(configuration["ConnectionStrings:BookStore"]))
            {
                try
                {
                    CartResponse cartResponses = new CartResponse();
                    SqlCommand cmd = new SqlCommand("SP_UpdateQty_InCart", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CartId", cartId);
                    cmd.Parameters.AddWithValue("@BookQuantity", bookQty);

                    con.Open();
                    var result = cmd.ExecuteNonQuery();
                    con.Close();

                    if (result != 0)
                    {
                        return "Quantity updated in Cart successfully";
                    }
                    else
                    {
                        return "Failed to Update Quantity in Cart";
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
