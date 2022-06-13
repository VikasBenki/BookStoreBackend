using DatabaseLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BussinessLayer.Interfaces
{
    public interface IOrderBL
    {
        List<OrdersResponse> GetAllOrders(int userId);
        AddOrder AddOrder(AddOrder addOrder, int userId);
    }
}
