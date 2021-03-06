using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseLayer.UserModel
{
    public class CartResponse
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int BookQuantity { get; set; }
        public string BookName { get; set; }
        public string BookImage { get; set; }
        public string AuthorName { get; set; }
        public double DiscountPrice { get; set; }
        public double ActualPrice { get; set; }
    }
}
