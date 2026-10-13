using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace NexusERP.Web
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AcquireRequestState(
    object sender,
    EventArgs e)
        {
            // العربية هي لغة الواجهة الافتراضية.
            string language = "ar";

            var languageCookie =
                Request.Cookies["NexusERP.Language"];

            // قبول اللغتين المدعومتين فقط.
            if (languageCookie != null)
            {
                if (languageCookie.Value == "en")
                {
                    language = "en";
                }
                else if (languageCookie.Value == "ar")
                {
                    language = "ar";
                }
            }

            // تحديد لغة النصوص التي تُقرأ من Resources.
            System.Threading.Thread.CurrentThread.CurrentUICulture =
                System.Globalization.CultureInfo.GetCultureInfo(
                    language);
        }
    }
}