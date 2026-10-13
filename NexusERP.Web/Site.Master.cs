using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;

namespace NexusERP.Web
{
    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }


        protected bool IsArabic
        {
            get
            {
                return System.Threading.Thread
                    .CurrentThread.CurrentUICulture
                    .TwoLetterISOLanguageName == "ar";
            }
        }

        protected string InterfaceLanguage
        {
            get { return IsArabic ? "ar" : "en"; }
        }

        protected string InterfaceDirection
        {
            get { return IsArabic ? "rtl" : "ltr"; }
        }

        protected string Translate(string key)
        {
            return Convert.ToString(
                GetGlobalResourceObject("Site", key));
        }

        protected void ChangeLanguage_Click(
            object sender,
            EventArgs e)
        {
            var button =
                sender as System.Web.UI.WebControls.LinkButton;

            if (button == null)
            {
                return;
            }

            string language = button.CommandArgument;

            if (language != "ar" && language != "en")
            {
                return;
            }

            var cookie = new HttpCookie(
                "NexusERP.Language",
                language)
            {
                HttpOnly = true,
                Secure = Request.IsSecureConnection,
                Path = "/",
                Expires = DateTime.UtcNow.AddYears(1)
            };

            Response.Cookies.Set(cookie);

            // إعادة تحميل الصفحة الحالية لتطبيق اللغة الجديدة.
            Response.Redirect(Request.RawUrl, false);

            Context.ApplicationInstance.CompleteRequest();
        }
    }

}