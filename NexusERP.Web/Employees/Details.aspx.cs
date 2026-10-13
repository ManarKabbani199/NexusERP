using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Entities;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;

namespace NexusERP.Web.Employees
{
    public partial class Details : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IEmployeeService _employeeService;

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

            pnlDetails.Visible = true;

            litEmployeeNumber.Text =
                Display(employee.EmployeeNumber);

            litFullNameAr.Text =
                Display(
                    employee.FirstNameAr + " " +
                    employee.LastNameAr);

            litFullNameEn.Text =
                Display(
                    employee.FirstNameEn + " " +
                    employee.LastNameEn);

            litCivilId.Text =
                Display(employee.CivilId);

            litDateOfBirth.Text =
                employee.DateOfBirth.HasValue
                    ? employee.DateOfBirth.Value
                        .ToString("yyyy/MM/dd")
                    : "—";

            litGender.Text =
                Display(employee.Gender.ToString());

            litNationality.Text =
                Display(employee.Nationality);

            litPhone.Text =
                Display(employee.Phone);

            litEmail.Text =
                Display(employee.Email);

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

            litHireDate.Text =
                employee.HireDate.ToString("yyyy/MM/dd");

            litTerminationDate.Text =
                employee.TerminationDate.HasValue
                    ? employee.TerminationDate.Value
                        .ToString("yyyy/MM/dd")
                    : "—";

            litEmploymentType.Text =
                Display(
                    employee.EmploymentType.ToString());

            litEmploymentStatus.Text =
                Display(
                    employee.EmploymentStatus.ToString());

            litBasicSalary.Text =
                employee.BasicSalary.ToString("N3");

            lblEmploymentStatus.Text =
                employee.EmploymentStatus.ToString();

            lnkEdit.NavigateUrl =
                "Edit.aspx?id=" + employee.Id;
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
            pnlDetails.Visible = false;
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