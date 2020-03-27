using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Domain.Constants;
using PayForAnswer.App_Start;
using log4net;
using System.Web;
using System.Net;
using System.Globalization;
using System.Threading;
using Repository.Tables;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using Repository.Blob;
using System.IO;
using System;
//[assembly: log4net.Config.XmlConfigurator(Watch = true)]
//[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]

namespace PayForAnswer
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Application_Start()
        {
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("es");
            log4net.Config.XmlConfigurator.Configure();
            log4net.GlobalContext.Properties["ip"] = "1.1.1.1";
            //log4net.GlobalContext.Properties["ip"] = Dns.GetHostAddresses(Dns.GetHostName())[0].ToString();
            //log4net.GlobalContext.Properties["ip"] = HttpContext.Current.Request.UserHostAddress;

            //log.Debug("Debug logging");
            log.Info("The PayForAnswer web application has started.");
            //log.Warn("Warn logging");
            //log.Error("Error logging");
            //log.Fatal("Fatal logging");
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            Initialize.MembershipDbConnection();
            InitializeObjects.AutoMapObjects();
            CreateTablesQueuesBlobContainers();
            //Needed if using the PfaInitializer class.
            //Database.SetInitializer<PfaDb>(new PfaInitializer());
            //using (var ctx = new PfaDb())
            //{
            //    ctx.Database.Initialize(true);
            //}
            //WebSecurity.InitializeDatabaseConnection("PfaDbSqlExpress", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            //BootstrapSupport.BootstrapBundleConfig.RegisterBundles(System.Web.Optimization.BundleTable.Bundles);
            //BootstrapMvcSample.ExampleLayoutsRouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        private static void CreateTablesQueuesBlobContainers()
        {
            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            // If this is running in an Azure Web Site (not a Cloud Service) use the Web.config file:
            //    var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);
            //CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            //CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //CloudTable subjectEntityTable = tableClient.GetTableReference(SubjectEntityValues.SUBJECT_ENTITIES_TABLE_NAME);
            //subjectEntityTable.CreateIfNotExists();

            //CloudBlobContainer questionContainer = blobClient.GetContainerReference(StorageValues.QUESTION_CONTAINER);
            //questionContainer.CreateIfNotExists();

            //var questionBlobRepository = new BlobRepository(StorageValues.COMMENTS_CONTAINER);
            //string comments = questionBlobRepository.GetQuestionComments("0000-11111-2222");
            //comments = "@#$%^&*(";
            //questionBlobRepository.AddUpdateQuestionComments("0000-11111-2223", comments);
        }

    }
}