using DatabaseLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface IOrderRL
    {
        AddOrder AddOrder(AddOrder addOrder, int userId);
        List<OrdersResponse> GetAllOrders(int userId);
    }
}

