using Microsoft.AspNet.Identity;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            return configSendGridasync(message);
            //return Task.FromResult(0);
        }

        private Task configSendGridasync(IdentityMessage message)
        {
            var myMessage = new SendGridMessage();
            myMessage.AddTo(message.Destination);
            myMessage.From = new System.Net.Mail.MailAddress(
                                "krtax@hotmail.com", "K&R Tax Accounting Services");
            myMessage.Subject = message.Subject;
            myMessage.Text = message.Body;
            myMessage.Html = message.Body;

            var credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailServerUn"], ConfigurationManager.AppSettings["EmailServerPwd"]);

            // Create a Web transport for sending email.
            var transportWeb = new SendGrid.Web(credentials);

            // Send the email.
            if (transportWeb != null)
                return transportWeb.DeliverAsync(myMessage);
            else
                return Task.FromResult(0);
        }
    }
}
