using System;
using Experimental.System.Messaging;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace DatabaseLayer.UserModel
{
    public class MailService
    {
        MessageQueue messageQueue = new MessageQueue();
        public void SendMessage(string token)
        {
            messageQueue.Path = @".\Private$\Token";
            try
            {
                if (!MessageQueue.Exists(messageQueue.Path))
                {
                    MessageQueue.Create(messageQueue.Path);
                }
                messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
                messageQueue.ReceiveCompleted += MessageQueue_ReceiveCompleted;
                messageQueue.Send(token);
                messageQueue.BeginReceive();
                messageQueue.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void MessageQueue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            try
            {
                var msg = messageQueue.EndReceive(e.AsyncResult);
                string token = msg.Body.ToString();
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential("luckybenki@gmail.com", "Vikki123#")
                };
                mailMessage.From = new MailAddress("luckybenki@gmail.com");
                mailMessage.To.Add(new MailAddress("luckybenki@gmail.com"));

                //mailMessage.Body = "token to reset your password: \n\n "+ token;

                mailMessage.Body = $"<!DOCTYPE html>" +
                                   $"<html>" +
                                   $"<html lang=\"en\">" +
                                    $"<head>" +
                                    $"<meta charset=\"UTF - 8\">" +
                                    $"</head>" +
                                    $"<body>" +
                                    $"<h2> Dear Book Store User, </h2>\n" +
                                    $"<h3> Please click on the below link to reset password</h3>" +
                                    $"<a href='http://localhost:4200/reset-password/{token}'> ClickHere </a>\n " +
                                    //$"<h3> Token to reset your password is:</h3>" +
                                    //$"<p> {token} </p>\n " +
                                    $"<h3 style = \"color: #EA4335\"> \nThe link is valid for 24 hour </h3>" +
                                    $"</body>" +
                                   $"</html>";


                mailMessage.Subject = "BookStore Password Reset Link";
                mailMessage.IsBodyHtml = true;
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
