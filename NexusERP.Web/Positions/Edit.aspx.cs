using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Entities;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;
using System.Linq;

namespace NexusERP.Web.Positions
{
    public partial class Edit : System.Web.UI.Page
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
                LoadPosition();
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

        private void LoadPosition()
        {
            int positionId;

            if (!TryGetPositionId(out positionId))
            {
                ShowError("رقم الوظيفة غير صحيح.");
                btnUpdate.Enabled = false;
                return;
            }

            var position =
                _positionService.GetById(positionId);

            if (position == null)
            {
                ShowError("الوظيفة المطلوبة غير موجودة.");
                btnUpdate.Enabled = false;
                return;
            }

            txtCode.Text =
                position.Code;

            txtTitleAr.Text =
                position.TitleAr;

            txtTitleEn.Text =
                position.TitleEn;

            txtDescriptionAr.Text =
                position.DescriptionAr;

            txtDescriptionEn.Text =
                position.DescriptionEn;

            txtJobLevel.Text =
                position.JobLevel;

            txtApprovedHeadCount.Text =
                position.ApprovedHeadCount.ToString();

            chkIsManagerial.Checked =
                position.IsManagerial;

            var departmentItem =
                ddlDepartment.Items.FindByValue(
                    position.DepartmentId.ToString());

            if (departmentItem != null)
            {
                ddlDepartment.SelectedValue =
                    position.DepartmentId.ToString();
            }
        }

        protected void btnUpdate_Click(
            object sender,
            EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            int positionId;
            int departmentId;
            int approvedHeadCount;

            if (!TryGetPositionId(out positionId))
            {
                ShowError("رقم الوظيفة غير صحيح.");
                return;
            }

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
                        Id = positionId,

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
                            departmentId
                    };

                bool updated =
                    _positionService.Update(position);

                if (!updated)
                {
                    ShowError(
                        "الوظيفة غير موجودة أو لم يتم تعديلها.");

                    return;
                }

                Response.Redirect(
                    "Details.aspx?id=" + positionId,
                    false);

                Context.ApplicationInstance
                    .CompleteRequest();
            }
            catch (Exception)
            {
                ShowError(
                    "حدث خطأ أثناء تعديل الوظيفة. " +
                    "تأكدي من صحة البيانات وعدم تكرار الكود.");
            }
        }

        private bool TryGetPositionId(
            out int positionId)
        {
            return int.TryParse(
                Request.QueryString["id"],
                out positionId);
        }

        private void ShowError(string message)
        {
            lblError.Text =
                Server.HtmlEncode(message);

            pnlError.Visible = true;
        }

        protected void Page_Unload(
            object sender,
            EventArgs e)
        {
            _unitOfWork?.Dispose();
        }
    }
}