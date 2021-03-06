using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseLayer.Models
{
    public class FeedbackResponse
    {
        public int FeedbackId { get; set; }
        public float Rating { get; set; }
        public string Comment { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
    }
}
