using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using NexusERP.Web.Models;

using ErpDbContext =
    NexusERP.Infrastructure.Data.ApplicationDbContext;

namespace NexusERP.Web.Account
{
    public partial class Register : Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                Response.Redirect(
                    "~/Account/Login?ReturnUrl="
                    + HttpUtility.UrlEncode(Request.RawUrl),
                    true);

                return;
            }

            if (!User.IsInRole("Admin"))
            {
                throw new HttpException(
                    403,
                    "ليس لديك صلاحية إنشاء حسابات المستخدمين.");
            }

            base.OnPreInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEmployees();
            }
        }

        private void LoadEmployees()
        {
            ddlEmployee.Items.Clear();

            ddlEmployee.Items.Add(
                new ListItem("— دون ربط بموظف —", ""));

            using (var context = new ErpDbContext())
            {
                var employees = context.Employees
                    .Where(employee =>
                        !employee.IsDeleted
                        && (employee.IdentityUserId == null
                            || employee.IdentityUserId == ""))
                    .OrderBy(employee => employee.EmployeeNumber)
                    .Select(employee => new
                    {
                        employee.Id,
                        employee.EmployeeNumber,
                        employee.FirstNameAr,
                        employee.LastNameAr
                    })
                    .ToList();

                foreach (var employee in employees)
                {
                    ddlEmployee.Items.Add(
                        new ListItem(
                            employee.EmployeeNumber
                            + " - "
                            + employee.FirstNameAr
                            + " "
                            + employee.LastNameAr,
                            employee.Id.ToString()));
                }
            }
        }

        protected void CreateUser_Click(object sender, EventArgs e)
        {
            ErrorMessage.Text = string.Empty;

            if (!IsValid)
            {
                return;
            }

            string role = ddlRole.SelectedValue;

            string[] allowedRoles =
            {
                "Employee",
                "PurchasingManager",
                "WarehouseManager",
                "Accountant"
            };

            if (!allowedRoles.Contains(role))
            {
                ErrorMessage.Text =
                    "الصلاحية المختارة غير صحيحة.";

                return;
            }

            int? employeeId = null;

            if (!string.IsNullOrEmpty(ddlEmployee.SelectedValue))
            {
                int parsedId;

                if (!int.TryParse(
                        ddlEmployee.SelectedValue,
                        out parsedId)
                    || parsedId <= 0)
                {
                    ErrorMessage.Text =
                        "الموظف المختار غير صحيح.";

                    return;
                }

                employeeId = parsedId;
            }

            var owinContext = Context.GetOwinContext();

            var manager = owinContext
                .GetUserManager<ApplicationUserManager>();

            // نفس سياق Identity المستخدم بواسطة UserManager.
            var identityContext = owinContext
                .Get<ApplicationDbContext>();

            string email = Email.Text.Trim();

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email
            };

            try
            {
                using (var transaction =
                    identityContext.Database.BeginTransaction())
                {
                    var createResult =
                        manager.Create(user, Password.Text);

                    if (!createResult.Succeeded)
                    {
                        transaction.Rollback();

                        ErrorMessage.Text = HttpUtility.HtmlEncode(
                            string.Join("؛ ", createResult.Errors));

                        return;
                    }

                    var roleResult =
                        manager.AddToRole(user.Id, role);

                    if (!roleResult.Succeeded)
                    {
                        transaction.Rollback();

                        ErrorMessage.Text = HttpUtility.HtmlEncode(
                            string.Join("؛ ", roleResult.Errors));

                        return;
                    }

                    if (employeeId.HasValue)
                    {
                        // تحديث مشروط يمنع استبدال ربط موجود،
                        // حتى إذا أنشأ مدير آخر حسابًا للموظف
                        // بعد تحميل القائمة.
                        int affectedRows =
                            identityContext.Database.ExecuteSqlCommand(
                                @"UPDATE dbo.Employees
                                  SET IdentityUserId = @p0
                                  WHERE Id = @p1
                                    AND IsDeleted = 0
                                    AND (
                                        IdentityUserId IS NULL
                                        OR IdentityUserId = N''
                                    )",
                                user.Id,
                                employeeId.Value);

                        if (affectedRows != 1)
                        {
                            transaction.Rollback();

                            ErrorMessage.Text =
                                "الموظف غير متاح أو مرتبط بحساب آخر. "
                                + "لم يتم إنشاء الحساب.";

                            LoadEmployees();
                            return;
                        }
                    }

                    transaction.Commit();
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Trace.TraceError(
                    "Account creation failed: {0}",
                    exception);

                ErrorMessage.Text =
                    "تعذر إتمام إنشاء الحساب. "
                    + "راجعي سجل الأخطاء ثم حاولي مجددًا.";

                return;
            }

            Response.Redirect(
                "~/Account/Register?created=1",
                true);
        }
    }
}