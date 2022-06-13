using BussinessLayer.Interfaces;
using DatabaseLayer.Models;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BussinessLayer.Services
{
    public class OrderBL : IOrderBL
    {
        private readonly IOrderRL orderRL;
        public OrderBL(IOrderRL orderRL)
        {
            this.orderRL = orderRL;
        }
        public AddOrder AddOrder(AddOrder addOrder, int userId)
        {
            try
            {
                return orderRL.AddOrder(addOrder, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<OrdersResponse> GetAllOrders(int userId)
        {
            try
            {
                return orderRL.GetAllOrders(userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

