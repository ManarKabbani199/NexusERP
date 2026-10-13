using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Entities;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace NexusERP.Web.Departments
{
    public partial class Create : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IDepartmentService _departmentService;
        private IBranchService _branchService;

        protected void Page_Init(object sender, EventArgs e)
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);

            _departmentService =
                new DepartmentService(_unitOfWork);

            _branchService =
                new BranchService(_unitOfWork);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadBranches();
                LoadParentDepartments(null);
            }
        }

        private void LoadBranches()
        {
            var branches = _branchService
                .GetAll()
                .Where(branch => branch.IsActive)
                .ToList();

            ddlBranch.DataSource = branches;
            ddlBranch.DataTextField = "NameAr";
            ddlBranch.DataValueField = "Id";
            ddlBranch.DataBind();

            ddlBranch.Items.Insert(
                0,
                new ListItem(
                    "-- اختر الفرع --",
                    ""));
        }

        private void LoadParentDepartments(int? branchId)
        {
            ddlParentDepartment.Items.Clear();

            if (!branchId.HasValue)
            {
                ddlParentDepartment.Items.Add(
                    new ListItem(
                        "-- قسم رئيسي --",
                        ""));

                return;
            }

            var departments = _departmentService
                .GetAll()
                .Where(department =>
                    department.BranchId == branchId.Value &&
                    department.IsActive)
                .ToList();

            ddlParentDepartment.DataSource = departments;
            ddlParentDepartment.DataTextField = "NameAr";
            ddlParentDepartment.DataValueField = "Id";
            ddlParentDepartment.DataBind();

            ddlParentDepartment.Items.Insert(
                0,
                new ListItem(
                    "-- قسم رئيسي --",
                    ""));
        }

        protected void ddlBranch_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            int branchId;

            if (int.TryParse(
                    ddlBranch.SelectedValue,
                    out branchId))
            {
                LoadParentDepartments(branchId);
            }
            else
            {
                LoadParentDepartments(null);
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

            if (!int.TryParse(
                    ddlBranch.SelectedValue,
                    out branchId))
            {
                ShowError("يجب اختيار فرع صحيح.");
                return;
            }

            var branch =
                _branchService.GetById(branchId);

            if (branch == null || !branch.IsActive)
            {
                ShowError(
                    "الفرع المختار غير موجود أو غير نشط.");

                return;
            }

            int? parentDepartmentId = null;

            if (!string.IsNullOrWhiteSpace(
                    ddlParentDepartment.SelectedValue))
            {
                int parsedParentId;

                if (!int.TryParse(
                        ddlParentDepartment.SelectedValue,
                        out parsedParentId))
                {
                    ShowError("القسم الأب غير صحيح.");
                    return;
                }

                var parentDepartment =
                    _departmentService.GetById(
                        parsedParentId);

                if (parentDepartment == null)
                {
                    ShowError(
                        "القسم الأب غير موجود.");

                    return;
                }

                if (parentDepartment.BranchId != branchId)
                {
                    ShowError(
                        "يجب أن يتبع القسم الأب للفرع نفسه.");

                    return;
                }

                parentDepartmentId = parsedParentId;
            }

            try
            {
                var department = new Department
                {
                    Code = txtCode.Text.Trim(),
                    NameAr = txtNameAr.Text.Trim(),
                    NameEn = txtNameEn.Text.Trim(),

                    DescriptionAr =
                        txtDescriptionAr.Text.Trim(),

                    DescriptionEn =
                        txtDescriptionEn.Text.Trim(),

                    BranchId = branchId,

                    ParentDepartmentId =
                        parentDepartmentId,

                    IsActive = chkIsActive.Checked
                };

                _departmentService.Create(department);

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