using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseLayer.UserModel
{
    public class BookModel
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string AuthorName { get; set; }
        public string BookImage { get; set; }
        public string BookDetail { get; set; }
        public double DiscountPrice { get; set; }
        public double ActualPrice { get; set; }
        public int Quantity { get; set; }
        public double Rating { get; set; }
        public int RatingCount { get; set; }
    }
}
