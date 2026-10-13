using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using NexusERP.Web.Models;

namespace NexusERP.Web.Account
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterHyperLink.NavigateUrl = "Register";
            // Enable this once you have account confirmation enabled for password reset functionality
            //ForgotPasswordHyperLink.NavigateUrl = "Forgot";
            OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];
            var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            if (!String.IsNullOrEmpty(returnUrl))
            {
                RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            }
        }

        protected void LogIn(object sender, EventArgs e)
        {
            ErrorMessage.Visible = false;
            FailureText.Text = string.Empty;

            if (!IsValid)
            {
                return;
            }

            var signinManager = Context.GetOwinContext()
                .GetUserManager<ApplicationSignInManager>();

            var result = signinManager.PasswordSignIn(
                Email.Text.Trim(),
                Password.Text,
                RememberMe.Checked,
                shouldLockout: true);

            switch (result)
            {
                case SignInStatus.Success:
                    IdentityHelper.RedirectToReturnUrl(
                        Request.QueryString["ReturnUrl"],
                        Response);
                    break;

                case SignInStatus.LockedOut:
                    FailureText.Text =
                        "تم قفل الحساب مؤقتًا بسبب تكرار محاولات الدخول. "
                        + "حاولي مرة أخرى بعد 5 دقائق.";

                    ErrorMessage.Visible = true;
                    break;

                case SignInStatus.RequiresVerification:
                    Response.Redirect(
                        "~/Account/TwoFactorAuthenticationSignIn"
                        + "?ReturnUrl="
                        + HttpUtility.UrlEncode(
                            Request.QueryString["ReturnUrl"])
                        + "&RememberMe="
                        + RememberMe.Checked);
                    break;

                case SignInStatus.Failure:
                default:
                    FailureText.Text =
                        "البريد الإلكتروني أو كلمة المرور غير صحيحة.";

                    ErrorMessage.Visible = true;
                    break;
            }
        }
    }
}