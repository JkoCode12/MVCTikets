using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;

namespace MVCTickets.Web
{
    static class Extension
    {
        public static string GetFullErrorMessage(this ModelStateDictionary modelState)
        {
            var messages = new List<string>();

            foreach (var entry in modelState)
            {
                foreach (var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }
            return String.Join(" ", messages);
        }

        public static DateTime LastDayOfMonth(this DateTime inDate)
        {
            var daysInMonth = DateTime.DaysInMonth(inDate.Year, inDate.Month);
            return new DateTime(inDate.Year, inDate.Month, daysInMonth);
        }
    }
}