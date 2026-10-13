using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Entities;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;
using System.Linq;

namespace NexusERP.Web.Employees
{
    public partial class Delete : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IEmployeeService _employeeService;

        private int EmployeeId
        {
            get
            {
                object value = ViewState["EmployeeId"];

                return value == null
                    ? 0
                    : (int)value;
            }
            set
            {
                ViewState["EmployeeId"] = value;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);

            _employeeService =
                new EmployeeService(_unitOfWork);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEmployee();
            }
        }

        private void LoadEmployee()
        {
            int employeeId;

            if (!int.TryParse(
                Request.QueryString["id"],
                out employeeId))
            {
                ShowError("رقم الموظف غير صحيح.");
                return;
            }

            EmployeeId = employeeId;

            Employee employee =
                _employeeService.GetById(employeeId);

            if (employee == null)
            {
                ShowError("الموظف غير موجود.");
                return;
            }

            Branch branch =
                _unitOfWork.Branches.GetById(
                    employee.BranchId);

            Department department =
                _unitOfWork.Departments.GetById(
                    employee.DepartmentId);

            Position position =
                _unitOfWork.Positions.GetById(
                    employee.PositionId);

            Company company = null;

            if (branch != null)
            {
                company =
                    _unitOfWork.Companies.GetById(
                        branch.CompanyId);
            }

            Employee manager = null;

            if (employee.ManagerEmployeeId.HasValue)
            {
                manager =
                    _unitOfWork.Employees.GetById(
                        employee.ManagerEmployeeId.Value);
            }

            pnlDelete.Visible = true;

            litEmployeeNumber.Text =
                Display(employee.EmployeeNumber);

            litEmployeeName.Text =
                Display(
                    employee.FirstNameAr + " " +
                    employee.LastNameAr);

            litEmploymentStatus.Text =
                Display(
                    employee.EmploymentStatus.ToString());

            litCompany.Text =
                company == null
                    ? "—"
                    : Display(company.NameAr);

            litBranch.Text =
                branch == null
                    ? "—"
                    : Display(branch.NameAr);

            litDepartment.Text =
                department == null
                    ? "—"
                    : Display(department.NameAr);

            litPosition.Text =
                position == null
                    ? "—"
                    : Display(position.TitleAr);

            litManager.Text =
                manager == null
                    ? "—"
                    : Display(
                        manager.FirstNameAr + " " +
                        manager.LastNameAr);
        }

        protected void btnDelete_Click(
            object sender,
            EventArgs e)
        {
            Employee employee =
                _employeeService.GetById(EmployeeId);

            if (employee == null)
            {
                ShowError("الموظف غير موجود.");
                return;
            }

            bool hasDirectReports =
                _unitOfWork.Employees
                    .GetAll()
                    .Any(item =>
                        item.ManagerEmployeeId ==
                        EmployeeId);

            if (hasDirectReports)
            {
                ShowError(
                    "لا يمكن حذف هذا الموظف لأنه مدير مباشر لموظفين آخرين. " +
                    "يرجى تغيير المدير المباشر لهؤلاء الموظفين أولًا.");

                return;
            }

            try
            {
                bool deleted =
                    _employeeService.Delete(EmployeeId);

                if (!deleted)
                {
                    ShowError(
                        "تعذر العثور على الموظف.");
                    return;
                }

                Response.Redirect(
                    "Index.aspx?message=deleted");
            }
            catch (Exception)
            {
                ShowError(
                    "تعذر حذف الموظف لوجود بيانات أخرى مرتبطة به.");
            }
        }

        private string Display(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "—";
            }

            return Server.HtmlEncode(value);
        }

        private void ShowError(string message)
        {
            pnlDelete.Visible = false;
            pnlError.Visible = true;

            litError.Text =
                Server.HtmlEncode(message);
        }

        protected void Page_Unload(
            object sender,
            EventArgs e)
        {
            if (_unitOfWork != null)
            {
                _unitOfWork.Dispose();
            }
        }
    }
}