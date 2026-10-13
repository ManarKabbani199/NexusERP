using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Entities;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace NexusERP.Web.Branches
{
    public partial class Create : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IBranchService _branchService;
        private ICompanyService _companyService;

        protected void Page_Init(object sender, EventArgs e)
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);

            _branchService =
                new BranchService(_unitOfWork);

            _companyService =
                new CompanyService(_unitOfWork);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCompanies();
            }
        }

        private void LoadCompanies()
        {
            var companies = _companyService
                .GetAll()
                .Where(company => company.IsActive)
                .ToList();

            ddlCompany.DataSource = companies;
            ddlCompany.DataTextField = "NameAr";
            ddlCompany.DataValueField = "Id";
            ddlCompany.DataBind();

            ddlCompany.Items.Insert(
                0,
                new ListItem(
                    "-- اختر الشركة --",
                    ""));
        }

        protected void btnSave_Click(
            object sender,
            EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            int companyId;

            if (!int.TryParse(
                    ddlCompany.SelectedValue,
                    out companyId))
            {
                ShowError("يجب اختيار شركة صحيحة.");
                return;
            }

            var company =
                _companyService.GetById(companyId);

            if (company == null)
            {
                ShowError("الشركة المختارة غير موجودة.");
                return;
            }

            try
            {
                var branch = new Branch
                {
                    Code = txtCode.Text.Trim(),
                    NameAr = txtNameAr.Text.Trim(),
                    NameEn = txtNameEn.Text.Trim(),
                    AddressAr = txtAddressAr.Text.Trim(),
                    AddressEn = txtAddressEn.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    CompanyId = companyId,
                    IsActive = chkIsActive.Checked
                };

                _branchService.Create(branch);

                Response.Redirect(
                    "Index.aspx",
                    false);

                Context.ApplicationInstance
                    .CompleteRequest();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void ShowError(string message)
        {
            string safeMessage =
                message.Replace("'", "\\'");

            string script =
                "alert('حدث خطأ: " +
                safeMessage +
                "');";

            ClientScript.RegisterStartupScript(
                GetType(),
                Guid.NewGuid().ToString(),
                script,
                true);
        }

        protected void Page_Unload(
            object sender,
            EventArgs e)
        {
            _unitOfWork?.Dispose();
        }
    }
}