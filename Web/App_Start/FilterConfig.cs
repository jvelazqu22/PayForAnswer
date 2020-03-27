using System.Web;
using System.Web.Mvc;
using Utilities;

namespace Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new RequireSecureConnectionFilter());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
