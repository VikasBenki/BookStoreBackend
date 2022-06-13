using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseLayer.Models
{
    public class AddFeedback
    {
        public float Rating { get; set; }
        public string Comment { get; set; }
        public int BookId { get; set; }
    }
}
