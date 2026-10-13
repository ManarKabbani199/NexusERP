using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;
using System.Web.UI.WebControls;

namespace NexusERP.Web.Positions
{
    public partial class Index : System.Web.UI.Page
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
                LoadPositions();
            }
        }

        private void LoadPositions()
        {
            gvPositions.DataSource =
                _positionService.GetAll();

            gvPositions.DataBind();
        }

        protected void btnDelete_Click(
            object sender,
            EventArgs e)
        {
            var deleteButton =
                sender as LinkButton;

            if (deleteButton == null)
            {
                return;
            }

            int positionId;

            if (!int.TryParse(
                    deleteButton.CommandArgument,
                    out positionId))
            {
                ShowMessage(
                    "رقم الوظيفة غير صحيح.",
                    false);

                return;
            }

            try
            {
                bool deleted =
                    _positionService.Delete(positionId);

                if (!deleted)
                {
                    ShowMessage(
                        "الوظيفة غير موجودة أو لم يتم حذفها.",
                        false);

                    return;
                }

                LoadPositions();

                ShowMessage(
                    "تم حذف الوظيفة بنجاح.",
                    true);
            }
            catch (Exception)
            {
                ShowMessage(
                    "لا يمكن حذف الوظيفة؛ قد تكون مرتبطة بموظفين.",
                    false);
            }
        }

        private void ShowMessage(
            string message,
            bool success)
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