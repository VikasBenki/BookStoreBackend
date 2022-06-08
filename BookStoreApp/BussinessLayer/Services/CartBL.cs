using BussinessLayer.Interfaces;
using DatabaseLayer.UserModel;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BussinessLayer.Services
{
    public class CartBL : ICartBL
    {
        private readonly ICartRL cartRl;

        public CartBL(ICartRL cartRl)
        {
            this.cartRl = cartRl;
        }

        public AddCart AddtoCart(AddCart addCart, int userId)
        {
            try
            {
                return this.cartRl.AddtoCart(addCart, userId);
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
        }


        public string RemoveFromCart(int cartId)
        {
            try
            {
                return this.cartRl.RemoveFromCart(cartId);
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
        }

        public List<CartResponse> GetAllCart(int userId)
        {
            try
            {
                return this.cartRl.GetAllCart(userId);
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
        }

        public string UpdateQtyInCart(int cartId, int bookQty, int userId)
        {
            try
            {
                return this.cartRl.UpdateQtyInCart(cartId, bookQty, userId);
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
        }
    }
}
