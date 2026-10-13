using System;
using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;

namespace NexusERP.Web.Companies
{
    public partial class Edit : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private ICompanyService _companyService;

        protected void Page_Init(object sender, EventArgs e)
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);
            _companyService = new CompanyService(_unitOfWork);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCompany();
            }
        }

        private void LoadCompany()
        {
            int companyId;

            if (!int.TryParse(
                    Request.QueryString["id"],
                    out companyId))
            {
                ShowError("رقم الشركة غير صحيح.");
                btnUpdate.Enabled = false;
                return;
            }

            try
            {
                var company =
                    _companyService.GetById(companyId);

                if (company == null)
                {
                    ShowError("الشركة المطلوبة غير موجودة.");
                    btnUpdate.Enabled = false;
                    return;
                }

                txtCode.Text = company.Code;
                txtNameAr.Text = company.NameAr;
                txtNameEn.Text = company.NameEn;

                txtCommercialRegistrationNumber.Text =
                    company.CommercialRegistrationNumber;

                txtTaxNumber.Text = company.TaxNumber;
                txtAddressAr.Text = company.AddressAr;
                txtAddressEn.Text = company.AddressEn;
                txtPhone.Text = company.Phone;
                txtEmail.Text = company.Email;
                txtWebsite.Text = company.Website;

                txtBaseCurrencyCode.Text =
                    company.BaseCurrencyCode;

                if (ddlDefaultLanguage.Items.FindByValue(
                        company.DefaultLanguage) != null)
                {
                    ddlDefaultLanguage.SelectedValue =
                        company.DefaultLanguage;
                }
            }
            catch (Exception)
            {
                ShowError("حدث خطأ أثناء تحميل بيانات الشركة.");
                btnUpdate.Enabled = false;
            }
        }

        protected void btnUpdate_Click(
            object sender,
            EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            int companyId;

            if (!int.TryParse(
                    Request.QueryString["id"],
                    out companyId))
            {
                ShowError("رقم الشركة غير صحيح.");
                return;
            }

            try
            {
                var company =
                    _companyService.GetById(companyId);

                if (company == null)
                {
                    ShowError("الشركة المطلوبة غير موجودة.");
                    return;
                }

                company.Code =
                    txtCode.Text.Trim();

                company.NameAr =
                    txtNameAr.Text.Trim();

                company.NameEn =
                    txtNameEn.Text.Trim();

                company.CommercialRegistrationNumber =
                    EmptyToNull(
                        txtCommercialRegistrationNumber.Text);

                company.TaxNumber =
                    EmptyToNull(txtTaxNumber.Text);

                company.AddressAr =
                    EmptyToNull(txtAddressAr.Text);

                company.AddressEn =
                    EmptyToNull(txtAddressEn.Text);

                company.Phone =
                    EmptyToNull(txtPhone.Text);

                company.Email =
                    EmptyToNull(txtEmail.Text);

                company.Website =
                    EmptyToNull(txtWebsite.Text);

                company.BaseCurrencyCode =
                    txtBaseCurrencyCode.Text
                        .Trim()
                        .ToUpper();

                company.DefaultLanguage =
                    ddlDefaultLanguage.SelectedValue;

                bool updated =
                    _companyService.Update(company);

                if (!updated)
                {
                    ShowError("لم يتم تحديث بيانات الشركة.");
                    return;
                }

                Response.Redirect("Index.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception)
            {
                ShowError(
                    "حدث خطأ أثناء تحديث بيانات الشركة.");
            }
        }

        private string EmptyToNull(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? null
                : value.Trim();
        }

        private void ShowError(string message)
        {
            lblMessage.Text =
                Server.HtmlEncode(message);

            lblMessage.Visible = true;
        }

        protected void Page_Unload(
            object sender,
            EventArgs e)
        {
            _unitOfWork?.Dispose();
        }
    }
}