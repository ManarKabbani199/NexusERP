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
    public partial class Edit : System.Web.UI.Page
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
                LoadBranch();
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

        private void LoadBranch()
        {
            int branchId;

            if (!TryGetBranchId(out branchId))
            {
                ShowError("رقم الفرع غير صحيح.");
                DisableForm();
                return;
            }

            var branch =
                _branchService.GetById(branchId);

            if (branch == null)
            {
                ShowError("الفرع غير موجود.");
                DisableForm();
                return;
            }

            txtCode.Text = branch.Code;
            txtNameAr.Text = branch.NameAr;
            txtNameEn.Text = branch.NameEn;
            txtAddressAr.Text = branch.AddressAr;
            txtAddressEn.Text = branch.AddressEn;
            txtPhone.Text = branch.Phone;
            chkIsActive.Checked = branch.IsActive;

            var companyItem =
                ddlCompany.Items.FindByValue(
                    branch.CompanyId.ToString());

            if (companyItem != null)
            {
                ddlCompany.ClearSelection();
                companyItem.Selected = true;
            }
        }

        protected void btnSave_Click(
            object sender,
            EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            int branchId;

            if (!TryGetBranchId(out branchId))
            {
                ShowError("رقم الفرع غير صحيح.");
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
                    Id = branchId,
                    Code = txtCode.Text.Trim(),
                    NameAr = txtNameAr.Text.Trim(),
                    NameEn = txtNameEn.Text.Trim(),
                    AddressAr = txtAddressAr.Text.Trim(),
                    AddressEn = txtAddressEn.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    CompanyId = companyId,
                    IsActive = chkIsActive.Checked
                };

                bool updated =
                    _branchService.Update(branch);

                if (!updated)
                {
                    ShowError("الفرع غير موجود.");
                    return;
                }

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

        private bool TryGetBranchId(
            out int branchId)
        {
            return int.TryParse(
                Request.QueryString["id"],
                out branchId);
        }

        private void DisableForm()
        {
            btnSave.Enabled = false;
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