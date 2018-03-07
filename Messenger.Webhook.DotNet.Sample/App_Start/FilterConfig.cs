using System.Web;
using System.Web.Mvc;

namespace Messenger.Webhook.DotNet.Sample
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
