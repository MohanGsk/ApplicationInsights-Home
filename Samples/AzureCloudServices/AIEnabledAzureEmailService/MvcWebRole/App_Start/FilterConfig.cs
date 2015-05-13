using MvcWebRole.Telemetry;
using System.Web;
using System.Web.Mvc;

namespace MvcWebRole
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //Default replace with AI override to report unhandled exceptions to AI
            filters.Add(new AiHandleErrorAttribute());
        }
    }
}
