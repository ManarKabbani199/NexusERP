using System;
using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;

namespace NexusERP.Web.Companies
{
    public partial class Details : System.Web.UI.Page
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

                lblCode.Text =
                    DisplayValue(company.Code);

                lblNameAr.Text =
                    DisplayValue(company.NameAr);

                lblNameEn.Text =
                    DisplayValue(company.NameEn);

                lblCommercialRegistrationNumber.Text =
                    DisplayValue(
                        company.CommercialRegistrationNumber);

                lblTaxNumber.Text =
                    DisplayValue(company.TaxNumber);

                lblAddressAr.Text =
                    DisplayValue(company.AddressAr);

                lblAddressEn.Text =
                    DisplayValue(company.AddressEn);

                lblPhone.Text =
                    DisplayValue(company.Phone);

                lblEmail.Text =
                    DisplayValue(company.Email);

                lblBaseCurrencyCode.Text =
                    DisplayValue(company.BaseCurrencyCode);

                if (company.DefaultLanguage == "ar")
                {
                    lblDefaultLanguage.Text = "العربية";
                }
                else if (company.DefaultLanguage == "en")
                {
                    lblDefaultLanguage.Text = "الإنجليزية";
                }
                else
                {
                    lblDefaultLanguage.Text =
                        DisplayValue(company.DefaultLanguage);
                }

                ConfigureWebsite(company.Website);

                lnkEdit.NavigateUrl =
                    "Edit.aspx?id=" + company.Id;

                pnlCompany.Visible = true;
                lblMessage.Visible = false;
            }
            catch (Exception)
            {
                ShowError(
                    "حدث خطأ أثناء تحميل بيانات الشركة.");
            }
        }

        private string DisplayValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "—";
            }

            return Server.HtmlEncode(value);
        }

        private void ConfigureWebsite(string website)
        {
            lnkWebsite.Visible = false;

            if (string.IsNullOrWhiteSpace(website))
            {
                return;
            }

            Uri websiteUri;

            bool isValid = Uri.TryCreate(
                website,
                UriKind.Absolute,
                out websiteUri);

            if (!isValid)
            {
                return;
            }

            bool isHttp =
                websiteUri.Scheme == Uri.UriSchemeHttp;

            bool isHttps =
                websiteUri.Scheme == Uri.UriSchemeHttps;

            if (!isHttp && !isHttps)
            {
                return;
            }

            lnkWebsite.Text =
                Server.HtmlEncode(website);

            lnkWebsite.NavigateUrl =
                websiteUri.AbsoluteUri;

            lnkWebsite.Visible = true;
        }

        private void ShowError(string message)
        {
            pnlCompany.Visible = false;

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