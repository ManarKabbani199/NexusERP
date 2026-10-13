using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;

namespace NexusERP.Web.Departments
{
    public partial class Details : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IDepartmentService _departmentService;

        protected void Page_Init(object sender, EventArgs e)
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);

            _departmentService =
                new DepartmentService(_unitOfWork);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDepartment();
            }
        }

        private void LoadDepartment()
        {
            int departmentId;

            if (!int.TryParse(
                    Request.QueryString["id"],
                    out departmentId))
            {
                ShowError("رقم القسم غير صحيح.");
                return;
            }

            var department =
                _departmentService.GetById(departmentId);

            if (department == null)
            {
                ShowError("القسم غير موجود.");
                return;
            }

            lblBranch.Text =
                department.Branch != null
                    ? department.Branch.NameAr
                    : "غير محدد";

            lblParentDepartment.Text =
                department.ParentDepartment != null
                    ? department.ParentDepartment.NameAr
                    : "قسم رئيسي";

            lblCode.Text = department.Code;
            lblNameAr.Text = department.NameAr;
            lblNameEn.Text = department.NameEn;

            lblDescriptionAr.Text =
                string.IsNullOrWhiteSpace(
                    department.DescriptionAr)
                    ? "غير محدد"
                    : department.DescriptionAr;

            lblDescriptionEn.Text =
                string.IsNullOrWhiteSpace(
                    department.DescriptionEn)
                    ? "غير محدد"
                    : department.DescriptionEn;

            lblChildDepartmentsCount.Text =
                department.ChildDepartments != null
                    ? department.ChildDepartments.Count.ToString()
                    : "0";

            lblStatus.Text =
                department.IsActive
                    ? "نشط"
                    : "غير نشط";

            lnkEdit.NavigateUrl =
                "Edit.aspx?id=" + department.Id;
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