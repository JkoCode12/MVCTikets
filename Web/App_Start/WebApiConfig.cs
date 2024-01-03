using System.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;


namespace MVCTickets
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            // OJO
            // No olvidar el formato de retorno
            // http://localhost:1099/API/Method?type=xml  o type=json


            // Adding formatter for Json   
            // /api/Controller/method/parameter/type=json
            config.Formatters.JsonFormatter.MediaTypeMappings.Add(
                                     new QueryStringMapping("type", "json",
                new MediaTypeHeaderValue("application/json")));

            // Adding formatter for XML  
            // /api/Controller/method/parameter/type=xml
            config.Formatters.XmlFormatter.MediaTypeMappings.Add(new
                                   QueryStringMapping("type", "xml",
                new MediaTypeHeaderValue("application/xml")));

        }
    }
}
