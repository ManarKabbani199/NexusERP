using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Entities;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;
using System.Linq;

namespace NexusERP.Web.Positions
{
    public partial class Create : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IPositionService _positionService;

        protected void Page_Init(
            object sender,
            EventArgs e)
        {
            _context =
                new ApplicationDbContext();

            _unitOfWork =
                new UnitOfWork(_context);

            _positionService =
                new PositionService(_unitOfWork);
        }

        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDepartments();
            }
        }

        private void LoadDepartments()
        {
            var departments =
                _unitOfWork.Departments
                    .GetAll()
                    .OrderBy(d => d.NameAr)
                    .ToList();

            ddlDepartment.DataSource =
                departments;

            ddlDepartment.DataTextField =
                "NameAr";

            ddlDepartment.DataValueField =
                "Id";

            ddlDepartment.DataBind();

            ddlDepartment.Items.Insert(
                0,
                new System.Web.UI.WebControls.ListItem(
                    "-- اختر القسم --",
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

            int departmentId;
            int approvedHeadCount;

            if (!int.TryParse(
                    ddlDepartment.SelectedValue,
                    out departmentId))
            {
                ShowError("يرجى اختيار قسم صحيح.");
                return;
            }

            if (!int.TryParse(
                    txtApprovedHeadCount.Text,
                    out approvedHeadCount))
            {
                ShowError(
                    "يرجى إدخال عدد معتمد صحيح.");

                return;
            }

            try
            {
                var position =
                    new Position
                    {
                        Code =
                            txtCode.Text.Trim(),

                        TitleAr =
                            txtTitleAr.Text.Trim(),

                        TitleEn =
                            txtTitleEn.Text.Trim(),

                        DescriptionAr =
                            txtDescriptionAr.Text.Trim(),

                        DescriptionEn =
                            txtDescriptionEn.Text.Trim(),

                        JobLevel =
                            txtJobLevel.Text.Trim(),

                        IsManagerial =
                            chkIsManagerial.Checked,

                        ApprovedHeadCount =
                            approvedHeadCount,

                        DepartmentId =
                            departmentId,

                        IsActive = true
                    };

                _positionService.Create(position);

                Response.Redirect(
                    "Index.aspx",
                    false);

                Context.ApplicationInstance
                    .CompleteRequest();
            }
            catch (Exception)
            {
                ShowError(
                    "حدث خطأ أثناء حفظ الوظيفة. " +
                    "تأكدي من صحة البيانات وعدم تكرار الكود.");
            }
        }

        private void ShowError(string message)
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