using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace PetsProject.Utils
{
    public class SendMail
    {
       public static void sendMail(String mailSendTo, String content, String status)
        {
            if(status=="RECEIVED ORDER")
            {
                sendGoogleMail(mailSendTo, "PETS SHOP", "Hello,<br/> We received your order! Please keep the phone so we can update the order status in the near future.<br/>Have a good day! ^^<br/><br/>"+content);
            } else if (status == "ON THE WAY")
            {
                sendGoogleMail(mailSendTo, "PETS SHOP - Updating your order", "Hello,<br/>Your order is in transit, it won't be long before it reaches your location. Please prepare your amount (if you choose to pay on delivery) before we arrive.<br/>Thank you, have a nice day! ^^<br/><br/>" + content);
            } else if (status == "DELIVERED")
            {
                sendGoogleMail(mailSendTo, "PETS SHOP - Updating your order", "Hello,<br/>Your order has been delivered successfully. Thank you for choosing QSQ Shop products.<br/>Have a nice day! ^^<br/><br/>" + content);
            }
            else if (status == "CANCELLED")
            {
                sendGoogleMail(mailSendTo, "PETS SHOP - Updating your order", "Hello,<br/>Your order has been canceled successfully. We hope you will come back to our Shop in the near future.<br/>Have a nice day! ^^<br/><br/>" + content);
            }
        }
        public static void sendGoogleMail(String mailSendTo, String subject, String content)
        {
            MailMessage mail = new MailMessage();

            mail.From = new System.Net.Mail.MailAddress("gallmount1899@gmail.com");
            SmtpClient smtp = new SmtpClient();
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(mail.From.Address, "nhs191118");
            smtp.Host = "smtp.gmail.com";

            //recipient
            mail.To.Add(new MailAddress(mailSendTo));

            mail.IsBodyHtml = true;

            mail.Subject = subject;
            mail.Body = content;
            smtp.Send(mail);
        }
    }
}