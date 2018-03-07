using System.Web.Http;

namespace Messenger.Webhook.DotNet.Sample
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Enable this in case of issues (for troubleshooting purpose)
            //config.EnableSystemDiagnosticsTracing();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}