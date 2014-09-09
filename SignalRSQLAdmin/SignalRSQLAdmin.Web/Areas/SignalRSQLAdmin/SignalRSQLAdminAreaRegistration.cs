using System.Web.Mvc;

namespace SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin
{
    public class SignalRSQLAdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SignalRSQLAdmin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SignalRSQLAdmin_default",
                "SignalRSQLAdmin/{controller}/{action}/{id}",
                new { controller = "Main", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}