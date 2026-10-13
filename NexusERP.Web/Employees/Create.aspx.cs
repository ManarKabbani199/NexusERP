using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Entities;
using NexusERP.Domain.Enums;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace NexusERP.Web.Employees
{
    public partial class Create : System.Web.UI.Page
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
                BindEnums();
                BindCompanies();
                ResetBranches();
                ResetDepartments();
                ResetPositions();
                ResetManagers();

                txtHireDate.Text =
                    DateTime.Today.ToString("yyyy-MM-dd");
            }
        }

        private void BindEnums()
        {
            BindEnum<Gender>(ddlGender);
            BindEnum<EmploymentType>(ddlEmploymentType);
            BindEnum<EmploymentStatus>(ddlEmploymentStatus);
        }

        private void BindEnum<TEnum>(
            DropDownList dropDownList)
            where TEnum : struct
        {
            dropDownList.DataSource =
                Enum.GetValues(typeof(TEnum))
                    .Cast<TEnum>()
                    .Select(value => new
                    {
                        Text = value.ToString(),
                        Value = Convert.ToInt32(value)
                    })
                    .ToList();

            dropDownList.DataTextField = "Text";
            dropDownList.DataValueField = "Value";
            dropDownList.DataBind();
        }

        private void BindCompanies()
        {
            ddlCompany.DataSource =
                _unitOfWork.Companies
                    .GetAll()
                    .OrderBy(company => company.NameAr)
                    .ToList();

            ddlCompany.DataTextField = "NameAr";
            ddlCompany.DataValueField = "Id";
            ddlCompany.DataBind();

            ddlCompany.Items.Insert(
                0,
                new ListItem("-- اختر الشركة --", ""));
        }

        private void BindBranches(int companyId)
        {
            ddlBranch.DataSource =
                _unitOfWork.Branches
                    .GetAll()
                    .Where(branch =>
                        branch.CompanyId == companyId)
                    .OrderBy(branch => branch.NameAr)
                    .ToList();

            ddlBranch.DataTextField = "NameAr";
            ddlBranch.DataValueField = "Id";
            ddlBranch.DataBind();

            ddlBranch.Items.Insert(
                0,
                new ListItem("-- اختر الفرع --", ""));
        }

        private void BindDepartments(int branchId)
        {
            ddlDepartment.DataSource =
                _unitOfWork.Departments
                    .GetAll()
                    .Where(department =>
                        department.BranchId == branchId)
                    .OrderBy(department =>
                        department.NameAr)
                    .ToList();

            ddlDepartment.DataTextField = "NameAr";
            ddlDepartment.DataValueField = "Id";
            ddlDepartment.DataBind();

            ddlDepartment.Items.Insert(
                0,
                new ListItem("-- اختر القسم --", ""));
        }

        private void BindPositions(int departmentId)
        {
            ddlPosition.DataSource =
                _unitOfWork.Positions
                    .GetAll()
                    .Where(position =>
                        position.DepartmentId == departmentId)
                    .OrderBy(position =>
                        position.TitleAr)
                    .ToList();

            ddlPosition.DataTextField = "TitleAr";
            ddlPosition.DataValueField = "Id";
            ddlPosition.DataBind();

            ddlPosition.Items.Insert(
                0,
                new ListItem("-- اختر الوظيفة --", ""));
        }

        private void BindManagers(int branchId)
        {
            var managers =
                _unitOfWork.Employees
                    .GetAll()
                    .Where(employee =>
                        employee.BranchId == branchId)
                    .OrderBy(employee =>
                        employee.FirstNameAr)
                    .ThenBy(employee =>
                        employee.LastNameAr)
                    .Select(employee => new
                    {
                        employee.Id,

                        FullName =
                            employee.FirstNameAr + " " +
                            employee.LastNameAr
                    })
                    .ToList();

            ddlManager.DataSource = managers;
            ddlManager.DataTextField = "FullName";
            ddlManager.DataValueField = "Id";
            ddlManager.DataBind();

            ddlManager.Items.Insert(
                0,
                new ListItem(
                    "-- بدون مدير مباشر --",
                    ""));
        }

        private void ResetBranches()
        {
            ddlBranch.Items.Clear();

            ddlBranch.Items.Add(
                new ListItem(
                    "-- اختر الشركة أولًا --",
                    ""));
        }

        private void ResetDepartments()
        {
            ddlDepartment.Items.Clear();

            ddlDepartment.Items.Add(
                new ListItem(
                    "-- اختر الفرع أولًا --",
                    ""));
        }

        private void ResetPositions()
        {
            ddlPosition.Items.Clear();

            ddlPosition.Items.Add(
                new ListItem(
                    "-- اختر القسم أولًا --",
                    ""));
        }

        private void ResetManagers()
        {
            ddlManager.Items.Clear();

            ddlManager.Items.Add(
                new ListItem(
                    "-- اختر الفرع أولًا --",
                    ""));
        }

        protected void ddlCompany_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            ResetDepartments();
            ResetPositions();
            ResetManagers();

            int companyId;

            if (int.TryParse(
                ddlCompany.SelectedValue,
                out companyId))
            {
                BindBranches(companyId);
            }
            else
            {
                ResetBranches();
            }
        }

        protected void ddlBranch_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            ResetPositions();

            int branchId;

            if (int.TryParse(
                ddlBranch.SelectedValue,
                out branchId))
            {
                BindDepartments(branchId);
                BindManagers(branchId);
            }
            else
            {
                ResetDepartments();
                ResetManagers();
            }
        }

        protected void ddlDepartment_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            int departmentId;

            if (int.TryParse(
                ddlDepartment.SelectedValue,
                out departmentId))
            {
                BindPositions(departmentId);
            }
            else
            {
                ResetPositions();
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

            DateTime hireDate;
            DateTime? dateOfBirth = null;
            decimal basicSalary;

            int branchId;
            int departmentId;
            int positionId;

            if (!DateTime.TryParse(
                txtHireDate.Text,
                out hireDate))
            {
                ShowError("تاريخ التعيين غير صحيح.");
                return;
            }

            if (!string.IsNullOrWhiteSpace(
                    txtDateOfBirth.Text))
            {
                DateTime parsedDateOfBirth;

                if (!DateTime.TryParse(
                    txtDateOfBirth.Text,
                    out parsedDateOfBirth))
                {
                    ShowError("تاريخ الميلاد غير صحيح.");
                    return;
                }

                dateOfBirth = parsedDateOfBirth;
            }

            if (!decimal.TryParse(
                txtBasicSalary.Text,
                out basicSalary) ||
                basicSalary < 0)
            {
                ShowError("الراتب الأساسي غير صحيح.");
                return;
            }

            if (!int.TryParse(
                    ddlBranch.SelectedValue,
                    out branchId) ||
                !int.TryParse(
                    ddlDepartment.SelectedValue,
                    out departmentId) ||
                !int.TryParse(
                    ddlPosition.SelectedValue,
                    out positionId))
            {
                ShowError(
                    "يرجى اختيار الفرع والقسم والوظيفة.");
                return;
            }

            string employeeNumber =
                txtEmployeeNumber.Text.Trim();

            bool employeeNumberExists =
                _unitOfWork.Employees
                    .GetAll()
                    .Any(employee =>
                        employee.EmployeeNumber ==
                        employeeNumber);

            if (employeeNumberExists)
            {
                ShowError(
                    "الرقم الوظيفي مستخدم مسبقًا.");
                return;
            }

            string civilId = txtCivilId.Text.Trim();

            if (!string.IsNullOrWhiteSpace(civilId))
            {
                bool civilIdExists =
                    _unitOfWork.Employees
                        .GetAll()
                        .Any(employee =>
                            employee.CivilId == civilId);

                if (civilIdExists)
                {
                    ShowError(
                        "الرقم المدني مستخدم مسبقًا.");
                    return;
                }
            }

            int? managerEmployeeId = null;
            int selectedManagerId;

            if (int.TryParse(
                ddlManager.SelectedValue,
                out selectedManagerId))
            {
                managerEmployeeId = selectedManagerId;
            }

            try
            {
                var employee = new Employee
                {
                    EmployeeNumber = employeeNumber,

                    FirstNameAr =
                        txtFirstNameAr.Text.Trim(),

                    LastNameAr =
                        txtLastNameAr.Text.Trim(),

                    FirstNameEn =
                        txtFirstNameEn.Text.Trim(),

                    LastNameEn =
                        txtLastNameEn.Text.Trim(),

                    CivilId = string.IsNullOrWhiteSpace(civilId)
                        ? null
                        : civilId,

                    DateOfBirth = dateOfBirth,

                    Gender = (Gender)int.Parse(
                        ddlGender.SelectedValue),

                    Nationality =
                        txtNationality.Text.Trim(),

                    Phone = txtPhone.Text.Trim(),

                    Email = txtEmail.Text.Trim(),

                    HireDate = hireDate,

                    TerminationDate = null,

                    EmploymentType =
                        (EmploymentType)int.Parse(
                            ddlEmploymentType.SelectedValue),

                    EmploymentStatus =
                        (EmploymentStatus)int.Parse(
                            ddlEmploymentStatus.SelectedValue),

                    BasicSalary = basicSalary,

                    BranchId = branchId,
                    DepartmentId = departmentId,
                    PositionId = positionId,

                    ManagerEmployeeId =
                        managerEmployeeId,

                    IdentityUserId = null
                };

                _employeeService.Create(employee);

                Response.Redirect(
                    "Index.aspx?message=created");
            }
            catch (Exception)
            {
                ShowError(
                    "حدث خطأ أثناء حفظ الموظف.");
            }
        }

        private void ShowError(string message)
        {
            pnlError.Visible = true;
            litError.Text = Server.HtmlEncode(message);
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