using DatabaseLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BussinessLayer.Interfaces
{
    public interface IFeedbackBL
    {
        public AddFeedback AddFeedback(AddFeedback addAddress, int userId);
        public List<FeedbackResponse> GetAllFeedbacks(int bookId);
    }
}
