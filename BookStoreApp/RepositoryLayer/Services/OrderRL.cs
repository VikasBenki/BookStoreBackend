using DatabaseLayer.Models;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryLayer.Services
{
    public class OrderRL: IOrderRL
    {
        private readonly IConfiguration configuration;
        public OrderRL(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public AddOrder AddOrder(AddOrder addOrder, int userId)
        {
            using (SqlConnection con = new SqlConnection(configuration["ConnectionStrings:BookStore"]))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_Add_Orders", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@BookId", addOrder.BookId);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@AddressId", addOrder.AddressId);

                    con.Open();
                    var result = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();

                    if (result != 2 && result != 3 && result != 4)
                    {
                        return addOrder;
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

        public List<OrdersResponse> GetAllOrders(int userId)
        {
            using (SqlConnection con = new SqlConnection(configuration["ConnectionStrings:BookStore"]))
            {
                try
                {
                    List<OrdersResponse> ordersResponse = new List<OrdersResponse>();
                    SqlCommand cmd = new SqlCommand("SP_GetAll_Orders", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserId", userId);

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            OrdersResponse order = new OrdersResponse();
                            OrdersResponse temp;
                            temp = ReadData(order, rdr);
                            ordersResponse.Add(temp);
                        }
                        con.Close();
                        return ordersResponse;
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

        public OrdersResponse ReadData(OrdersResponse order, SqlDataReader rdr)
        {
            order.OrderId = Convert.ToInt32(rdr["OrderId"] == DBNull.Value ? default : rdr["OrderId"]);
            order.AddressId = Convert.ToInt32(rdr["AddressId"] == DBNull.Value ? default : rdr["AddressId"]);
            order.BookId = Convert.ToInt32(rdr["BookId"] == DBNull.Value ? default : rdr["BookId"]);
            order.UserId = Convert.ToInt32(rdr["UserId"] == DBNull.Value ? default : rdr["UserId"]);
            order.BooksQty = Convert.ToInt32(rdr["Books_Qty"] == DBNull.Value ? default : rdr["Books_Qty"]);
            order.OrderDateTime = Convert.ToDateTime(rdr["Order_Date"] == DBNull.Value ? default : rdr["Order_Date"]);
            order.OrderDate = order.OrderDateTime.ToString("dd-MM-yyyy");
            order.OrderPrice = Convert.ToInt32(rdr["Order_Price"] == DBNull.Value ? default : rdr["Order_Price"]);
            order.ActualPrice = Convert.ToInt32(rdr["Actual_Price"] == DBNull.Value ? default : rdr["Actual_Price"]);
            order.BookName = Convert.ToString(rdr["Book_Name"] == DBNull.Value ? default : rdr["Book_Name"]);
            order.BookImage = Convert.ToString(rdr["Book_Image"] == DBNull.Value ? default : rdr["Book_Image"]);
            order.AuthorName = Convert.ToString(rdr["Author_Name"] == DBNull.Value ? default : rdr["Author_Name"]);

            return order;
        }
    }
}
