using BusinessRules;
using Domain.Exceptions;
using Domain.Models;
using Domain.Models.Entities;
using Domain.Models.Helper;
using log4net;
using PayPal;
using Repository.Interfaces;
using Repository.Queue;
using Repository.SQL;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class PayPalController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //TODO: add more security
        [Authorize]
        public ActionResult PostToPayPal2(string item, string amount, long paymentId)
        {
            PayPalModel paypal = new PayPalModel();
            paypal.cmd = "_xclick";
            paypal.business = ConfigurationManager.AppSettings["BusinessAccountKey"];
            ViewBag.actionURl = ConfigurationManager.AppSettings["PayPalSubmitUrl"];
            paypal.cancel_return = ConfigurationManager.AppSettings["CancelURL"];
            paypal.@return = ConfigurationManager.AppSettings["ReturnURL"];
            paypal.notify_url = ConfigurationManager.AppSettings["NotifyURL"];
            paypal.current_code = ConfigurationManager.AppSettings["CurrencyCode"];
            paypal.item_name = item;
            paypal.amount = amount;
            paypal.custom = paymentId.ToString();
            return View(paypal);
        }

        [Authorize]
        public ActionResult PostToPayPal(string item, string amount, long paymentId)
        {
            string actionURl = ConfigurationManager.AppSettings["PayPalSubmitUrl"];
            PayPalModel paypal = new PayPalModel();
            paypal.cmd = "_xclick";
            paypal.business = ConfigurationManager.AppSettings["BusinessAccountKey"];
            paypal.cancel_return = ConfigurationManager.AppSettings["CancelURL"];
            paypal.@return = ConfigurationManager.AppSettings["ReturnURL"];
            paypal.notify_url = ConfigurationManager.AppSettings["NotifyURL"];
            paypal.current_code = ConfigurationManager.AppSettings["CurrencyCode"];
            paypal.item_name = item;
            paypal.amount = amount;
            paypal.custom = paymentId.ToString();

            string parameters = "cmd=" + paypal.cmd +
                "&business=" + paypal.business +
                "&no_shipping=" + paypal.no_shipping +
                "&return=" + paypal.@return +
                "&cancel_return=" + paypal.cancel_return +
                "&notify_url=" + paypal.notify_url +
                "&current_code=" + paypal.current_code +
                "&item_name=" + paypal.item_name +
                "&amount=" + paypal.amount +
                "&custom=" + paypal.custom;

            return Redirect(actionURl+"?"+parameters);
        }

        // This is the action method where users get re-directed after they pay
        // using the paypal page.
        public ActionResult RedirectFromPayPal()
        {
            try
            {
                string authToken, txToken, query, strResponse;

                // Used parts from https://www.paypaltech.com/PDTGen/
                // Visit above URL to auto-generate PDT script

                authToken = WebConfigurationManager.AppSettings["PDTToken"];

                //read in txn token from querystring
                txToken = Request.QueryString.Get("tx");

                query = string.Format("cmd=_notify-synch&tx={0}&at={1}", txToken, authToken);

                // Create the request back
                string url = WebConfigurationManager.AppSettings["PayPalSubmitUrl"];
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                // Set values for the request back
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = query.Length;

                // Write the request back IPN strings
                StreamWriter stOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
                stOut.Write(query);
                stOut.Close();

                // Do the request to PayPal and get the response
                StreamReader stIn = new StreamReader(req.GetResponse().GetResponseStream());
                strResponse = stIn.ReadToEnd();
                stIn.Close();

                RedirectFromPayPalViewModel redirectFromPayPalModel;
                var queueRepository = new QueueRepository();
                using (IPaymentRepository paymentRepository = new PaymentRepository())
                    redirectFromPayPalModel = new PayPalBR().ProcessRedirectFromPayPal(strResponse, paymentRepository, new EmailBR(), queueRepository);

                return View(redirectFromPayPalModel);
            }
            catch (PayPalPaymentStatusNotCompleteException ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return View(new Question()); // The view will know that String.IsNullOrWhiteSpace(Model.Title) and display appropriate message.
            }
        }

        public ActionResult CancelFromPayPal()
        {
            return View();
        }

        ///TODO: This was for testing purposes only. In can be deleted.
        private void ParseUsingIPNMessage(byte[] parameters)
        {

            IPNMessage ipn = new IPNMessage(parameters);
            bool isIpnValidated = ipn.Validate();
            string transactionType = ipn.TransactionType;
            NameValueCollection map = ipn.IpnMap;
            string results = "transaction type: " + transactionType + "\n";

            foreach (string key in map.AllKeys)
                results += "\n" + key + " : " + map.Get(key) + "\n";
        }

        // This is the action method that catches all the Instant Payment Notifications (IPN)
        // sent from PayPal
        public void NotifyFromPayPal()
        {
            string postUrl = ConfigurationManager.AppSettings["PayPalSubmitUrl"];
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(postUrl);

            //Set values for the request back
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            byte[] param = Request.BinaryRead(System.Web.HttpContext.Current.Request.ContentLength);

            string strRequest = Encoding.ASCII.GetString(param);
            string ipnPost = strRequest;
            strRequest += "&cmd=_notify-validate";
            req.ContentLength = strRequest.Length;

            //for proxy
            //WebProxy proxy = new WebProxy(new Uri("http://url:port#"));
            //req.Proxy = proxy;

            //Send the request to PayPal and get the response
            StreamWriter streamOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
            streamOut.Write(strRequest);
            streamOut.Close();

            StreamReader streamIn = new StreamReader(req.GetResponse().GetResponseStream());
            string strResponseStatus = streamIn.ReadToEnd();
            streamIn.Close();

            IPaymentRepository paymentRepository = new PaymentRepository();
            IAnswerRepository answerRepository = new AnswerRepository();
            var queueRepository = new QueueRepository();
            new PayPalBR().ProcessNotifyFromPayPal(strRequest, strResponseStatus, paymentRepository, answerRepository, new EmailBR(), queueRepository);
            paymentRepository.Dispose();
            answerRepository.Dispose();
        }
    }
}
