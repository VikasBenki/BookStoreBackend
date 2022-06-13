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
    public class AdressRL: IAdressRL
    {
        private readonly IConfiguration configuration;
        public AdressRL(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public AddAddress AddAddress(AddAddress addAddress, int userId)
        {
            using (SqlConnection con = new SqlConnection(configuration["ConnectionStrings:BookStore"]))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_Add_Address", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Address", addAddress.Address);
                    cmd.Parameters.AddWithValue("@City", addAddress.City);
                    cmd.Parameters.AddWithValue("@State", addAddress.State);
                    cmd.Parameters.AddWithValue("@AdTypeId", addAddress.AdTypeId);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    con.Open();
                    var result = cmd.ExecuteNonQuery();
                    con.Close();

                    if (result != 0)
                    {
                        return addAddress;
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

        public AddressModel UpdateAddress(AddressModel addressModel, int userId)
        {
            using (SqlConnection con = new SqlConnection(configuration["ConnectionStrings:BookStore"]))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_Update_Address", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@AddressId", addressModel.AddressId);
                    cmd.Parameters.AddWithValue("@Address", addressModel.Address);
                    cmd.Parameters.AddWithValue("@City", addressModel.City);
                    cmd.Parameters.AddWithValue("@State", addressModel.State);
                    cmd.Parameters.AddWithValue("@AdTypeId", addressModel.AdTypeId);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    con.Open();
                    var result = cmd.ExecuteNonQuery();
                    con.Close();

                    if (result != 0)
                    {
                        return addressModel;
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


        public string DeleteAddress(int addressId, int userId)
        {
            using (SqlConnection con = new SqlConnection(configuration["ConnectionStrings:BookStore"]))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_Delete_Address", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@AddressId", addressId);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    con.Open();
                    var result = cmd.ExecuteNonQuery();
                    con.Close();

                    if (result != 0)
                    {
                        return "Address Deleted Successfully";
                    }
                    else
                    {
                        return "Failed to Delete Address";
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }
        public AddressModel GetAddressById(int typeId, int addressId, int userId)
        {
            using (SqlConnection con = new SqlConnection(configuration["ConnectionStrings:BookStore"]))
            {
                try
                {
                    AddressModel addressResponse = new AddressModel();
                    SqlCommand cmd = new SqlCommand("spGetAddress", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@TypeId", typeId);
                    cmd.Parameters.AddWithValue("@AddressId", addressId);

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            AddressModel address = new AddressModel();
                            addressResponse = ReadData(address, rdr);
                        }
                        con.Close();
                        return addressResponse;
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

        public AddressModel ReadData(AddressModel address, SqlDataReader rdr)
        {
            address.Address = Convert.ToString(rdr["Address"] == DBNull.Value ? default : rdr["Address"]);
            address.City = Convert.ToString(rdr["City"] == DBNull.Value ? default : rdr["City"]);
            address.State = Convert.ToString(rdr["State"] == DBNull.Value ? default : rdr["State"]);
            address.AdTypeId = Convert.ToInt32(rdr["AdTypeId"] == DBNull.Value ? default : rdr["AdTypeId"]);
            address.AddressId = Convert.ToInt32(rdr["AddressId"] == DBNull.Value ? default : rdr["AddressId"]);

            return address;
        }

        public List<AddressModel> GetAllAddresses(int userId)
        {
            using (SqlConnection con = new SqlConnection(configuration["ConnectionStrings:BookStore"]))
            {
                try
                {
                    List<AddressModel> addressResponse = new List<AddressModel>();
                    SqlCommand cmd = new SqlCommand("SP_Get_AllAddress", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserId", userId);

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            AddressModel address = new AddressModel();
                            AddressModel temp;
                            temp = ReadData(address, rdr);
                            addressResponse.Add(temp);
                        }
                        con.Close();
                        return addressResponse;
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
    }
}
