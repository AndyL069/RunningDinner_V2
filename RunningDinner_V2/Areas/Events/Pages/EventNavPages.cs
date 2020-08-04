using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace RunningDinner.Areas.Events.Views
{
    public static class EventNavPages
    {
        public static string ActivePageKey => "ActivePage";

        public static string Index => "Index";

        public static string MyRoute => "MyRoute";

        public static string EventMap => "EventMap";

        public static string Participants => "Participants";

        public static string Teams => "Teams";

        public static string SendEmail => "SendEmail";

        public static string Routes => "Routes";

        public static string KitchenMap => "KitchenMap";

        public static string Settings => "Settings";

        public static string EventMapNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string MyRouteNavClass(ViewContext viewContext) => PageNavClass(viewContext, MyRoute);

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string ParticipantsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Participants);

        public static string TeamsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Teams);

        public static string SendEmailNavClass(ViewContext viewContext) => PageNavClass(viewContext, SendEmail);

        public static string RoutesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Routes);

        public static string KitchenMapNavClass(ViewContext viewContext) => PageNavClass(viewContext, KitchenMap);

        public static string SettingsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Settings);

        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext?.ViewData["ActivePage"] as string;
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }

        public static void AddActivePage(this ViewDataDictionary viewData, string activePage) => viewData[ActivePageKey] = activePage;
    }
}
