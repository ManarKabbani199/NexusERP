using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;
using System.Web.UI.WebControls;

namespace NexusERP.Web.Branches
{
    public partial class Index : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IBranchService _branchService;

        protected void Page_Init(object sender, EventArgs e)
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);
            _branchService = new BranchService(_unitOfWork);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadBranches();
            }
        }

        private void LoadBranches()
        {
            gvBranches.DataSource =
                _branchService.GetAll();

            gvBranches.DataBind();
        }

        protected void btnDelete_Click(
            object sender,
            EventArgs e)
        {
            var deleteButton = sender as LinkButton;

            if (deleteButton == null)
            {
                return;
            }

            int branchId;

            if (!int.TryParse(
                    deleteButton.CommandArgument,
                    out branchId))
            {
                ShowMessage(
                    "رقم الفرع غير صحيح.",
                    false);

                return;
            }

            try
            {
                bool deleted =
                    _branchService.Delete(branchId);

                if (!deleted)
                {
                    ShowMessage(
                        "الفرع غير موجود أو لم يتم حذفه.",
                        false);

                    return;
                }

                LoadBranches();

                ShowMessage(
                    "تم حذف الفرع بنجاح.",
                    true);
            }
            catch (Exception)
            {
                ShowMessage(
                    "لا يمكن حذف الفرع؛ قد يكون مرتبطًا بأقسام أو موظفين.",
                    false);
            }
        }

        private void ShowMessage(
            string message,
            bool success)
        {
            string safeMessage =
                message.Replace("'", "\\'");

            string script =
                "alert('" + safeMessage + "');";

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