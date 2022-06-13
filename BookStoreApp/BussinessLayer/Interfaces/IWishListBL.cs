using DatabaseLayer.UserModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BussinessLayer.Interfaces
{
    public interface IWishListBL
    {
        string AddToWishList(int bookId, int userId);
        string RemoveFromWishList(int wishListId, int userId);
        List<WishListResponse> GetAllWishList(int userId);
    }
}
