using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;

namespace NexusERP.Web.Positions
{
    public partial class Details : System.Web.UI.Page
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
                LoadPosition();
            }
        }

        private void LoadPosition()
        {
            int positionId;

            if (!int.TryParse(
                    Request.QueryString["id"],
                    out positionId))
            {
                ShowError("رقم الوظيفة غير صحيح.");
                return;
            }

            var position =
                _positionService.GetById(positionId);

            if (position == null)
            {
                ShowError("الوظيفة المطلوبة غير موجودة.");
                return;
            }

            lblCode.Text =
                Server.HtmlEncode(position.Code);

            lblTitleAr.Text =
                Server.HtmlEncode(position.TitleAr);

            lblTitleEn.Text =
                Server.HtmlEncode(position.TitleEn);

            lblDepartment.Text =
                position.Department != null
                    ? Server.HtmlEncode(
                        position.Department.NameAr)
                    : "غير محدد";

            lblJobLevel.Text =
                string.IsNullOrWhiteSpace(position.JobLevel)
                    ? "غير محدد"
                    : Server.HtmlEncode(position.JobLevel);

            lblApprovedHeadCount.Text =
                position.ApprovedHeadCount.ToString();

            lblIsManagerial.Text =
                position.IsManagerial
                    ? "نعم"
                    : "لا";

            lblDescriptionAr.Text =
                string.IsNullOrWhiteSpace(
                    position.DescriptionAr)
                    ? "لا يوجد وصف"
                    : Server.HtmlEncode(
                        position.DescriptionAr);

            lblDescriptionEn.Text =
                string.IsNullOrWhiteSpace(
                    position.DescriptionEn)
                    ? "لا يوجد وصف"
                    : Server.HtmlEncode(
                        position.DescriptionEn);

            lnkEdit.NavigateUrl =
                "Edit.aspx?id=" + position.Id;

            pnlDetails.Visible = true;
            pnlError.Visible = false;
        }

        private void ShowError(string message)
        {
            lblError.Text =
                Server.HtmlEncode(message);

            pnlError.Visible = true;
            pnlDetails.Visible = false;
        }

        protected void Page_Unload(
            object sender,
            EventArgs e)
        {
            _unitOfWork?.Dispose();
        }
    }
}