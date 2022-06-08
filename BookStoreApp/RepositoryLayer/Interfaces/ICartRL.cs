using DatabaseLayer.UserModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface ICartRL
    {
        AddCart AddtoCart(AddCart addCart, int userId);
        string RemoveFromCart(int cartId);
        List<CartResponse> GetAllCart(int userId);
        string UpdateQtyInCart(int cartId, int bookQty, int userId);
    }
}
