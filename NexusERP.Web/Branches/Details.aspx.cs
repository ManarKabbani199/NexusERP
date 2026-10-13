using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;

namespace NexusERP.Web.Branches
{
    public partial class Details : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IBranchService _branchService;

        protected void Page_Init(object sender, EventArgs e)
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);

            _branchService =
                new BranchService(_unitOfWork);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadBranch();
            }
        }

        private void LoadBranch()
        {
            int branchId;

            if (!int.TryParse(
                    Request.QueryString["id"],
                    out branchId))
            {
                ShowError("رقم الفرع غير صحيح.");
                return;
            }

            var branch =
                _branchService.GetById(branchId);

            if (branch == null)
            {
                ShowError("الفرع غير موجود.");
                return;
            }

            lblCompany.Text =
                branch.Company != null
                    ? branch.Company.NameAr
                    : "غير محددة";

            lblCode.Text = branch.Code;
            lblNameAr.Text = branch.NameAr;
            lblNameEn.Text = branch.NameEn;

            lblAddressAr.Text =
                string.IsNullOrWhiteSpace(branch.AddressAr)
                    ? "غير محدد"
                    : branch.AddressAr;

            lblAddressEn.Text =
                string.IsNullOrWhiteSpace(branch.AddressEn)
                    ? "غير محدد"
                    : branch.AddressEn;

            lblPhone.Text =
                string.IsNullOrWhiteSpace(branch.Phone)
                    ? "غير محدد"
                    : branch.Phone;

            lblStatus.Text =
                branch.IsActive
                    ? "نشط"
                    : "غير نشط";

            lnkEdit.NavigateUrl =
                "Edit.aspx?id=" + branch.Id;
        }

        private void ShowError(string message)
        {
            lnkEdit.Visible = false;

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