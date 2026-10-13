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
    public partial class Edit : System.Web.UI.Page
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
                int employeeId;

                if (!int.TryParse(
                    Request.QueryString["id"],
                    out employeeId))
                {
                    Response.Redirect("Index.aspx");
                    return;
                }

                EmployeeId = employeeId;

                BindEnums();
                BindCompanies();
                LoadEmployee();
            }
        }

        private void BindEnums()
        {
            BindEnum<Gender>(ddlGender);

            BindEnum<EmploymentType>(
                ddlEmploymentType);

            BindEnum<EmploymentStatus>(
                ddlEmploymentStatus);
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
                        position.DepartmentId ==
                        departmentId)
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
                        employee.BranchId == branchId &&
                        employee.Id != EmployeeId)
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

        private void LoadEmployee()
        {
            Employee employee =
                _employeeService.GetById(EmployeeId);

            if (employee == null)
            {
                Response.Redirect("Index.aspx");
                return;
            }

            Branch branch =
                _unitOfWork.Branches.GetById(
                    employee.BranchId);

            if (branch == null)
            {
                ShowError(
                    "الفرع المرتبط بالموظف غير موجود.");
                btnSave.Enabled = false;
                return;
            }

            BindBranches(branch.CompanyId);
            BindDepartments(employee.BranchId);
            BindPositions(employee.DepartmentId);
            BindManagers(employee.BranchId);

            SelectValue(
                ddlCompany,
                branch.CompanyId.ToString());

            SelectValue(
                ddlBranch,
                employee.BranchId.ToString());

            SelectValue(
                ddlDepartment,
                employee.DepartmentId.ToString());

            SelectValue(
                ddlPosition,
                employee.PositionId.ToString());

            if (employee.ManagerEmployeeId.HasValue)
            {
                SelectValue(
                    ddlManager,
                    employee.ManagerEmployeeId
                        .Value.ToString());
            }

            SelectValue(
                ddlGender,
                Convert.ToInt32(employee.Gender)
                    .ToString());

            SelectValue(
                ddlEmploymentType,
                Convert.ToInt32(
                    employee.EmploymentType)
                    .ToString());

            SelectValue(
                ddlEmploymentStatus,
                Convert.ToInt32(
                    employee.EmploymentStatus)
                    .ToString());

            txtEmployeeNumber.Text =
                employee.EmployeeNumber;

            txtFirstNameAr.Text =
                employee.FirstNameAr;

            txtLastNameAr.Text =
                employee.LastNameAr;

            txtFirstNameEn.Text =
                employee.FirstNameEn;

            txtLastNameEn.Text =
                employee.LastNameEn;

            txtCivilId.Text =
                employee.CivilId;

            txtDateOfBirth.Text =
                employee.DateOfBirth.HasValue
                    ? employee.DateOfBirth.Value
                        .ToString("yyyy-MM-dd")
                    : string.Empty;

            txtNationality.Text =
                employee.Nationality;

            txtPhone.Text =
                employee.Phone;

            txtEmail.Text =
                employee.Email;

            txtHireDate.Text =
                employee.HireDate.ToString("yyyy-MM-dd");

            txtTerminationDate.Text =
                employee.TerminationDate.HasValue
                    ? employee.TerminationDate.Value
                        .ToString("yyyy-MM-dd")
                    : string.Empty;

            txtBasicSalary.Text =
                employee.BasicSalary.ToString("0.000");
        }

        private void SelectValue(
            DropDownList dropDownList,
            string value)
        {
            ListItem item =
                dropDownList.Items.FindByValue(value);

            if (item != null)
            {
                dropDownList.ClearSelection();
                item.Selected = true;
            }
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

        protected void btnSave_Click(
            object sender,
            EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            Employee existingEmployee =
                _employeeService.GetById(EmployeeId);

            if (existingEmployee == null)
            {
                ShowError("الموظف غير موجود.");
                return;
            }

            DateTime hireDate;
            DateTime? dateOfBirth = null;
            DateTime? terminationDate = null;
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

            if (!TryParseOptionalDate(
                txtDateOfBirth.Text,
                out dateOfBirth))
            {
                ShowError("تاريخ الميلاد غير صحيح.");
                return;
            }

            if (!TryParseOptionalDate(
                txtTerminationDate.Text,
                out terminationDate))
            {
                ShowError(
                    "تاريخ إنهاء الخدمة غير صحيح.");
                return;
            }

            if (terminationDate.HasValue &&
                terminationDate.Value.Date <
                hireDate.Date)
            {
                ShowError(
                    "تاريخ إنهاء الخدمة لا يمكن أن يسبق تاريخ التعيين.");
                return;
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
                        employee.Id != EmployeeId &&
                        employee.EmployeeNumber ==
                        employeeNumber);

            if (employeeNumberExists)
            {
                ShowError(
                    "الرقم الوظيفي مستخدم لموظف آخر.");
                return;
            }

            string civilId =
                txtCivilId.Text.Trim();

            if (!string.IsNullOrWhiteSpace(civilId))
            {
                bool civilIdExists =
                    _unitOfWork.Employees
                        .GetAll()
                        .Any(employee =>
                            employee.Id != EmployeeId &&
                            employee.CivilId == civilId);

                if (civilIdExists)
                {
                    ShowError(
                        "الرقم المدني مستخدم لموظف آخر.");
                    return;
                }
            }

            int? managerEmployeeId = null;
            int selectedManagerId;

            if (int.TryParse(
                ddlManager.SelectedValue,
                out selectedManagerId))
            {
                if (selectedManagerId == EmployeeId)
                {
                    ShowError(
                        "لا يمكن أن يكون الموظف مديرًا لنفسه.");
                    return;
                }

                managerEmployeeId = selectedManagerId;
            }

            try
            {
                var employee = new Employee
                {
                    Id = EmployeeId,

                    EmployeeNumber = employeeNumber,

                    FirstNameAr =
                        txtFirstNameAr.Text.Trim(),

                    LastNameAr =
                        txtLastNameAr.Text.Trim(),

                    FirstNameEn =
                        txtFirstNameEn.Text.Trim(),

                    LastNameEn =
                        txtLastNameEn.Text.Trim(),

                    CivilId =
                        string.IsNullOrWhiteSpace(civilId)
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

                    TerminationDate = terminationDate,

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

                    IdentityUserId =
                        existingEmployee.IdentityUserId
                };

                bool updated =
                    _employeeService.Update(employee);

                if (!updated)
                {
                    ShowError(
                        "تعذر العثور على الموظف.");
                    return;
                }

                Response.Redirect(
                    "Index.aspx?message=updated");
            }
            catch (Exception)
            {
                ShowError(
                    "حدث خطأ أثناء تعديل بيانات الموظف.");
            }
        }

        private bool TryParseOptionalDate(
            string text,
            out DateTime? value)
        {
            value = null;

            if (string.IsNullOrWhiteSpace(text))
            {
                return true;
            }

            DateTime parsedValue;

            if (!DateTime.TryParse(
                text,
                out parsedValue))
            {
                return false;
            }

            value = parsedValue;
            return true;
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