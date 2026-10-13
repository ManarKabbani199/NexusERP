using System;
using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Entities;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;

namespace NexusERP.Web.Companies
{
    public partial class Create : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private ICompanyService _companyService;

        protected void Page_Init(object sender, EventArgs e)
        {
            // إنشاء الاتصال بقاعدة البيانات
            _context = new ApplicationDbContext();

            // إنشاء وحدة العمل
            _unitOfWork = new UnitOfWork(_context);

            // إنشاء خدمة الشركات
            _companyService = new CompanyService(_unitOfWork);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // عدم تنفيذ الحفظ إذا كانت الحقول المطلوبة غير صحيحة
            if (!Page.IsValid)
            {
                return;
            }

            try
            {
                var company = new Company
                {
                    Code = txtCode.Text.Trim(),

                    NameAr = txtNameAr.Text.Trim(),

                    NameEn = txtNameEn.Text.Trim(),

                    CommercialRegistrationNumber =
                        EmptyToNull(txtCommercialRegistrationNumber.Text),

                    TaxNumber =
                        EmptyToNull(txtTaxNumber.Text),

                    AddressAr =
                        EmptyToNull(txtAddressAr.Text),

                    AddressEn =
                        EmptyToNull(txtAddressEn.Text),

                    Phone =
                        EmptyToNull(txtPhone.Text),

                    Email =
                        EmptyToNull(txtEmail.Text),

                    Website =
                        EmptyToNull(txtWebsite.Text),

                    BaseCurrencyCode =
                        txtBaseCurrencyCode.Text.Trim().ToUpper(),

                    DefaultLanguage =
                        ddlDefaultLanguage.SelectedValue
                };

                // إرسال الشركة إلى طبقة الخدمات لحفظها
                _companyService.Create(company);

                // الانتقال إلى صفحة قائمة الشركات
                Response.Redirect("Index.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception)
            {
                lblMessage.Text =
                    "حدث خطأ أثناء حفظ الشركة. تأكدي من البيانات وحاولي مرة أخرى.";

                lblMessage.Visible = true;
            }
        }

        private string EmptyToNull(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return value.Trim();
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            // تحرير موارد قاعدة البيانات بعد انتهاء الطلب
            _unitOfWork?.Dispose();
        }
    }
}