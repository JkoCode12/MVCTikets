using System.Web;
using System.Web.Mvc;

namespace MVCTickets
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            // Comentelo si desea capturar los errores por ud mismo.
            filters.Add(new HandleErrorAttribute());
        }
    }
}
