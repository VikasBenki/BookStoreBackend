using DatabaseLayer.UserModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface IWishListRL
    {
        string AddToWishList(int bookId, int userId);
        string RemoveFromWishList(int wishListId, int userId);
        List<WishListResponse> GetAllWishList(int userId);
    }
}
