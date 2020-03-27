using BusinessRules.Interfaces;
using Domain.App_GlobalResources;
using Domain.Constants;
using Domain.Models.Account;
using Domain.Models.Entities;
using Domain.Models.Entities.Identity;
using Domain.Models.Helper;
using log4net;
using Repository.Interfaces;
using Repository.SQL;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace BusinessRules
{
    public class EmailBR : IEmailBR
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private EmailModel _emailModel;
        public EmailBR(){}

        public EmailBR(EmailModel emailModel)
        {
            _emailModel = emailModel;
        }

        public void Send(EmailModel emailModel)
        {
            _emailModel = emailModel;
            if (Debugger.IsAttached)
                SendGMail();
            else
                SendEmail();
        }

        private void Send()
        {
            if (Debugger.IsAttached)
                SendGMail();
            else
                SendEmail();
        }

        public void SendEmail()
        {
            // Create the email object first, then add the properties.
            var myMessage = new SendGridMessage();

            if (_emailModel.NotifyAllCampaignManagers)
                _emailModel.ListOfEmailAddresesToNotify.ForEach(e => myMessage.AddTo(e));
            else
                myMessage.AddTo(_emailModel.To);
            
            myMessage.From = new MailAddress(_emailModel.From, _emailModel.SenderName);
            myMessage.Subject = _emailModel.Subject;
            myMessage.Text = _emailModel.Body;
            myMessage.Html = _emailModel.Body;

            // Create network credentials to access your SendGrid account.
            var username = ConfigurationManager.AppSettings["EmailServerUn"];
            var pwd = ConfigurationManager.AppSettings["EmailServerPwd"];

            // TODO: This is for debugging purposes only
            //string emailParams = "AddTo = " + _emailModel.To + ", From = " + _emailModel.From  + ", SenderName = " + _emailModel.SenderName 
            //    + ", Subject = " + _emailModel.Subject + ", Html = " + _emailModel.Body + ", username = " + username + ", pswd = " + pswd;
            //log.Info(emailParams);

            var credentials = new NetworkCredential(username, pwd);

            // Create a Web transport for sending email.
            var transportWeb = new SendGrid.Web(credentials);

            // Send the email.
            if (transportWeb != null)
                transportWeb.Deliver(myMessage);
        }


        private void SendGMail()
        {
            //SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["EmailServer"], 25);
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            MailMessage message = new MailMessage();
            MailAddress from = new MailAddress(_emailModel.From, _emailModel.SenderName);
            message.From = from;

            if (_emailModel.NotifyAllCampaignManagers)
                _emailModel.ListOfEmailAddresesToNotify.ForEach(e => message.To.Add(new MailAddress(e)));
            else
                message.To.Add(new MailAddress(_emailModel.To));

            message.Body = _emailModel.Body;
            message.IsBodyHtml = true;
            message.Subject = _emailModel.Subject;
            NetworkCredential myCreds = new NetworkCredential(ConfigurationManager.AppSettings["GEmailServerUn"], ConfigurationManager.AppSettings["GEmailServerPwd"], "");
            client.Credentials = myCreds;
            client.Send(message);
        } // --- end of Send_download_link --

        public void SendEmail(string to, string subject, string body)
        {
            _emailModel = new EmailModel();
            _emailModel.From = Emails.SUPPORT_EMAIL_ADDRESS;
            //_emailModel.From = CommonResources.FromAutoConfirmEmailAddress;
            _emailModel.SenderName = CommonResources.PayForAnswer;
            _emailModel.To = to;
            _emailModel.Subject = subject;
            _emailModel.Body = body;

            Send();
        }

        //public void SendChangeEmailNotification(ApplicationUser user, string callbackUrl)
        //{
        //    string confirmationLink = String.Format("<a href=\"{0}\">{1} </a>", callbackUrl, CommonResources.lnkTxtConfirmEmailChange);
        //    _emailModel = new EmailModel();
        //    _emailModel.From = CommonResources.FromAutoConfirmEmailAddress;
        //    _emailModel.SenderName = CommonResources.PayForAnswer;
        //    _emailModel.To = user.NewEmail;
        //    _emailModel.Subject = CommonResources.EmailSubjectConfirmEmailChng;
        //    _emailModel.Body = String.Format(CommonResources.EmailBodyConfirmEmailChng,
        //        new object[] { user.UserName, user.NewEmail, confirmationLink, callbackUrl });

        //    Send();
        //}

        public void SendNewQuestionToAllUsersWithRelatedSubject(Question questionModel, ICollection<Subject> Subjects, int paymentType)
        {
            List<string> listOfEmailAddresesToNotify = new List<string>();
            string subjectList = " ";

            foreach (var subject in Subjects)
                subjectList += subject.SubjectName + ", ";

            using (ISubjectRepository subjectRepository = new SubjectRepository(new PfaDb()))
            {
                listOfEmailAddresesToNotify = GetListOfOptInEmailAddressThatHaveNewQuestionSubjects(questionModel, Subjects, subjectRepository);
            }

            var confirmationUrl = Urls.QUESTION_URL + questionModel.Id.ToString();
            var confirmationLink = String.Format(Html.LINK_PLACE_HOLDER, confirmationUrl, questionModel.Title);
            var unsubscribeUrl = Urls.E_NOTIFICATIONS_URL;
            var unsubscribeLink = string.Format(Html.LINK_PLACE_HOLDER, unsubscribeUrl, CommonResources.Unsubscribe);

            _emailModel = new EmailModel();
            _emailModel.From = Emails.SUPPORT_EMAIL_ADDRESS;
            _emailModel.SenderName = CommonResources.PayForAnswer;

            var param = new object[] { confirmationLink, subjectList, questionModel.Amount, unsubscribeLink };
            if( paymentType == QuestionPaymentDetailType.FirstPayment)
            {
                _emailModel.Subject = string.Format(CommonResources.EmailSubjectNewQuestion, questionModel.Amount);
                _emailModel.Body = string.Format(CommonResources.EmailBodyNewQuestion, param);
            }
            else
            {
                _emailModel.Subject = string.Format(CommonResources.EmailSubjectQuestionAmtIncrease, questionModel.Amount);
                _emailModel.Body = string.Format(CommonResources.EmailBodyQuestionAmtIncrease, param);
            }

            foreach (var emailAddress in listOfEmailAddresesToNotify)
            {
                _emailModel.To = emailAddress;
                Send();
            }
        }

        public void NotifyAllCampaignManagers(List<ApplicationUser> campaignManagers)
        {
            List<string> listOfEmailAddresesToNotify = new List<string>();
            campaignManagers.ForEach(cm => listOfEmailAddresesToNotify.Add( cm.Email ));
            listOfEmailAddresesToNotify = listOfEmailAddresesToNotify.Distinct().ToList();

            var confirmationUrl = Urls.CAMPAIGNS_URL;
            var confirmationLink = string.Format(Html.LINK_PLACE_HOLDER, confirmationUrl, CommonResources.SeeListWaitingForCampaigns);

            _emailModel = new EmailModel();
            _emailModel.From = Emails.SUPPORT_EMAIL_ADDRESS;
            _emailModel.SenderName = CommonResources.PayForAnswer;
            _emailModel.Subject = CommonResources.NewMarketingCampaignNeededSubject;
            _emailModel.Body = string.Format(CommonResources.NewMarketingCampaignNeededBody, confirmationLink);
            _emailModel.NotifyAllCampaignManagers = true;

            listOfEmailAddresesToNotify.ForEach(e => _emailModel.ListOfEmailAddresesToNotify.Add(e));
            Send();
        }

        public List<string> GetListOfOptInEmailAddressThatHaveNewQuestionSubjects(Question questionModel, ICollection<Subject> Subjects, 
                                                                            ISubjectRepository subjectRepository)
        {
            List<string> listOfEmailAddresesToNotify = new List<string>();
            foreach (var subject in Subjects)
            {
                IEnumerable<ApplicationUser> Users = subjectRepository.GetUsersBySubject(subject.Id);
                foreach (var user in Users)
                {
                    if (user.Id != questionModel.UserId && user.Notifications.NewQuestionRelatedToMySubjects)
                        listOfEmailAddresesToNotify.Add(user.Email);
                }
            }

            return listOfEmailAddresesToNotify.Distinct().ToList();
        }
        public List<string> GetListOfEmailsForCommentNotifications(Question questionModel, string posterUserName)
        {
            List<string> emails = new List<string>();
            posterUserName = posterUserName.ToLower();

            var userName = questionModel.User.UserName.ToLower();
            if (userName != posterUserName && questionModel.User.Notifications.NewComment)
                emails.Add(questionModel.User.Email);

            foreach(var answer in questionModel.Answers)
            {
                userName = answer.User.UserName.ToLower();
                if (userName != posterUserName && answer.User.Notifications.NewComment)
                    emails.Add(answer.User.Email);
            }
            return emails.Distinct().ToList();
        }

        public void CommentNotifications(Guid questionId, IQuestionRepository questionRepository, string posterUserName, int commentType, string comment)
        {
            Question questionModel = questionRepository.GetQuestionByID(questionId);
            List<string> listOfEmailAddresesToNotify = GetListOfEmailsForCommentNotifications(questionModel, posterUserName);

            var confirmationUrl = Urls.QUESTION_URL + questionModel.Id.ToString();
            var confirmationLink = String.Format(Html.LINK_PLACE_HOLDER, confirmationUrl, questionModel.Title);
            var unsubscribeUrl = Urls.E_NOTIFICATIONS_URL;
            var unsubscribeLink = string.Format(Html.LINK_PLACE_HOLDER, unsubscribeUrl, CommonResources.Unsubscribe);

            _emailModel = new EmailModel();
            _emailModel.From = Emails.SUPPORT_EMAIL_ADDRESS;
            _emailModel.SenderName = CommonResources.PayForAnswer;
            _emailModel.Subject = commentType == CommentType.QuestionComment
                ? string.Format(CommonResources.EmailNewQuestionCommentSubject, questionModel.Amount)
                : string.Format(CommonResources.EmailNewAnswerCommentSubject, questionModel.Amount);

            var param = new object[] { questionModel.Amount, confirmationLink, comment, unsubscribeLink };
            _emailModel.Body = commentType == CommentType.QuestionComment
                ? string.Format(CommonResources.EmailBodyNewQuestionComment, param)
                : string.Format(CommonResources.EmailBodyNewAnswerComment, param);


            foreach (var emailAddress in listOfEmailAddresesToNotify)
            {
                _emailModel.To = emailAddress;
                Send();
            }
            questionRepository.Dispose();
        }

    }
}
