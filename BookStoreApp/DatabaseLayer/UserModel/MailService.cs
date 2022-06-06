using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace DatabaseLayer.UserModel
{
    public class MailService
    {
        public static void SendMail(string email, string token)
        {
            using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = true;
                client.Credentials = new NetworkCredential("luckybenki@gmail.com", "Vikki123#");

                MailMessage msgObj = new MailMessage();
                msgObj.To.Add(email);
                msgObj.From = new MailAddress("luckybenki@gmail.com");
                msgObj.Subject = "Password Reset Link";
                msgObj.Body = $"<!DOCTYPE html>" +
                                   $"<html>" +
                                   $"<html lang=\"en\">" +
                                    $"<head>" +
                                    $"<meta charset=\"UTF - 8\">" +
                                    $"</head>" +
                                    $"<body>" +
                                    $"<h2> Dear Book Store User, </h2>\n" +
                                    $"<h3> Token to reset your password is:</h3>" +
                                    $"<p> {token} </p>\n " +
                                    $"<h3 style = \"color: #EA4335\"> \nThe link is valid for 3 hour </h3>" +
                                    $"</body>" +
                                   $"</html>";
                client.Send(msgObj);


            }
            
        }

        
    }
}
