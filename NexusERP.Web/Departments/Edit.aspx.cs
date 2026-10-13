using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Entities;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace NexusERP.Web.Departments
{
    public partial class Edit : System.Web.UI.Page
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
                LoadDepartment();
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

        private void LoadParentDepartments(
            int? branchId,
            int departmentId)
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
                    department.Id != departmentId &&
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

        private void LoadDepartment()
        {
            int departmentId;

            if (!TryGetDepartmentId(out departmentId))
            {
                ShowError("رقم القسم غير صحيح.");
                DisableForm();
                return;
            }

            var department =
                _departmentService.GetById(departmentId);

            if (department == null)
            {
                ShowError("القسم غير موجود.");
                DisableForm();
                return;
            }

            txtCode.Text = department.Code;
            txtNameAr.Text = department.NameAr;
            txtNameEn.Text = department.NameEn;

            txtDescriptionAr.Text =
                department.DescriptionAr;

            txtDescriptionEn.Text =
                department.DescriptionEn;

            chkIsActive.Checked =
                department.IsActive;

            var branchItem =
                ddlBranch.Items.FindByValue(
                    department.BranchId.ToString());

            if (branchItem != null)
            {
                ddlBranch.ClearSelection();
                branchItem.Selected = true;
            }

            LoadParentDepartments(
                department.BranchId,
                department.Id);

            if (department.ParentDepartmentId.HasValue)
            {
                var parentItem =
                    ddlParentDepartment.Items.FindByValue(
                        department.ParentDepartmentId
                            .Value
                            .ToString());

                if (parentItem != null)
                {
                    ddlParentDepartment.ClearSelection();
                    parentItem.Selected = true;
                }
            }
        }

        protected void ddlBranch_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            int departmentId;

            if (!TryGetDepartmentId(out departmentId))
            {
                ShowError("رقم القسم غير صحيح.");
                DisableForm();
                return;
            }

            int branchId;

            if (int.TryParse(
                    ddlBranch.SelectedValue,
                    out branchId))
            {
                LoadParentDepartments(
                    branchId,
                    departmentId);
            }
            else
            {
                LoadParentDepartments(
                    null,
                    departmentId);
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

            int departmentId;

            if (!TryGetDepartmentId(out departmentId))
            {
                ShowError("رقم القسم غير صحيح.");
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

                if (parsedParentId == departmentId)
                {
                    ShowError(
                        "لا يمكن أن يكون القسم أبًا لنفسه.");

                    return;
                }

                var parentDepartment =
                    _departmentService.GetById(
                        parsedParentId);

                if (parentDepartment == null)
                {
                    ShowError("القسم الأب غير موجود.");
                    return;
                }

                if (parentDepartment.BranchId != branchId)
                {
                    ShowError(
                        "يجب أن يتبع القسم الأب للفرع نفسه.");

                    return;
                }

                if (CreatesHierarchyCycle(
                        departmentId,
                        parentDepartment))
                {
                    ShowError(
                        "لا يمكن اختيار قسم فرعي تابع لهذا القسم كقسم أب.");

                    return;
                }

                parentDepartmentId = parsedParentId;
            }

            try
            {
                var department = new Department
                {
                    Id = departmentId,
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

                bool updated =
                    _departmentService.Update(department);

                if (!updated)
                {
                    ShowError("القسم غير موجود.");
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

        private bool CreatesHierarchyCycle(
            int departmentId,
            Department parentDepartment)
        {
            var visitedIds = new HashSet<int>();
            var currentDepartment = parentDepartment;

            while (currentDepartment != null)
            {
                if (currentDepartment.Id == departmentId)
                {
                    return true;
                }

                if (!visitedIds.Add(currentDepartment.Id))
                {
                    return true;
                }

                if (!currentDepartment
                        .ParentDepartmentId
                        .HasValue)
                {
                    break;
                }

                currentDepartment =
                    _departmentService.GetById(
                        currentDepartment
                            .ParentDepartmentId
                            .Value);
            }

            return false;
        }

        private bool TryGetDepartmentId(
            out int departmentId)
        {
            return int.TryParse(
                Request.QueryString["id"],
                out departmentId);
        }

        private void DisableForm()
        {
            btnSave.Enabled = false;
            ddlBranch.Enabled = false;
            ddlParentDepartment.Enabled = false;
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