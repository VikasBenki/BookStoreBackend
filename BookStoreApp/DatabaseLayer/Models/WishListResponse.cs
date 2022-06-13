using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseLayer.UserModel
{
    public class WishListResponse
    {
        public int WishListId { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public string BookName { get; set; }
        public string AuthorName { get; set; }
        public string BookImage { get; set; }
        public double DiscountPrice { get; set; }
        public double ActualPrice { get; set; }
    }
}
