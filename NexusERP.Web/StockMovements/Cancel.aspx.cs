using System;
using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Entities;
using NexusERP.Domain.Enums;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;

namespace NexusERP.Web.StockMovements
{
    public partial class Cancel : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IStockMovementService _stockMovementService;

        protected void Page_Init(object sender, EventArgs e)
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);

            _stockMovementService =
                new StockMovementService(_unitOfWork);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMovement();
            }
        }

        private void LoadMovement()
        {
            int movementId;

            if (!TryGetMovementId(out movementId))
            {
                pnlCancelForm.Visible = false;
                ShowError("رقم حركة المخزون غير صحيح.");
                return;
            }

            try
            {
                StockMovement movement =
                    _stockMovementService.GetById(movementId);

                if (movement == null || movement.IsDeleted)
                {
                    pnlCancelForm.Visible = false;
                    ShowError("حركة المخزون غير موجودة.");
                    return;
                }

                if (movement.IsCancelled)
                {
                    pnlCancelForm.Visible = false;

                    ShowError(
                        "تم إلغاء هذه الحركة مسبقًا.");

                    return;
                }

                if (movement.OriginalMovementId.HasValue)
                {
                    pnlCancelForm.Visible = false;

                    ShowError(
                        "لا يمكن إلغاء حركة عكسية.");

                    return;
                }

                if (movement.MovementType ==
                        StockMovementType.OpeningBalance ||
                    movement.MovementType ==
                        StockMovementType.StockAdjustment)
                {
                    pnlCancelForm.Visible = false;

                    ShowError(
                        "لا يمكن إلغاء الرصيد الافتتاحي " +
                        "أو تسوية الجرد. استخدمي تسوية جرد جديدة.");

                    return;
                }

                lblMovementId.Text = movement.Id.ToString();

                lblMovementType.Text =
                    GetMovementTypeName(
                        movement.MovementType);

                lblQuantity.Text =
                    movement.Quantity.ToString("N2");

                lblWarehouse.Text =
                    movement.Warehouse != null
                        ? movement.Warehouse.NameAr
                        : "-";

                lblItem.Text =
                    movement.Item != null
                        ? movement.Item.NameAr
                        : "-";
            }
            catch (Exception ex)
            {
                pnlCancelForm.Visible = false;

                ShowError(
                    "حدث خطأ أثناء تحميل الحركة: " +
                    ex.Message);
            }
        }

        protected void btnConfirmCancel_Click(
            object sender,
            EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            int movementId;

            if (!TryGetMovementId(out movementId))
            {
                ShowError("رقم حركة المخزون غير صحيح.");
                return;
            }

            try
            {
                string cancelledBy =
                    User.Identity.IsAuthenticated
                        ? User.Identity.Name
                        : null;

                _stockMovementService.CancelMovement(
                    movementId,
                    txtCancellationReason.Text.Trim(),
                    cancelledBy);

                Response.Redirect(
                    "Index.aspx?cancelled=1");
            }
            catch (InvalidOperationException ex)
            {
                ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                ShowError(
                    "حدث خطأ أثناء إلغاء الحركة: " +
                    ex.Message);
            }
        }

        private bool TryGetMovementId(out int movementId)
        {
            return int.TryParse(
                Request.QueryString["id"],
                out movementId);
        }

        private string GetMovementTypeName(
            StockMovementType movementType)
        {
            switch (movementType)
            {
                case StockMovementType.OpeningBalance:
                    return "رصيد افتتاحي";

                case StockMovementType.Receipt:
                    return "استلام";

                case StockMovementType.Issue:
                    return "صرف";

                case StockMovementType.Transfer:
                    return "تحويل";

                case StockMovementType.StockAdjustment:
                    return "تسوية جرد";

                default:
                    return "غير معروف";
            }
        }

        private void ShowError(string message)
        {
            pnlMessage.Visible = true;
            pnlMessage.CssClass = "alert alert-danger";
            litMessage.Text = Server.HtmlEncode(message);
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (_unitOfWork != null)
            {
                _unitOfWork.Dispose();
            }
        }

        protected bool CanCancelMovement(
    object isCancelledValue,
    object originalMovementIdValue,
    object movementTypeValue)
        {
            bool isCancelled =
                Convert.ToBoolean(isCancelledValue);

            bool isReversal =
                originalMovementIdValue != null &&
                originalMovementIdValue != DBNull.Value;

            StockMovementType movementType =
                (StockMovementType)movementTypeValue;

            bool unsupportedType =
                movementType ==
                    StockMovementType.OpeningBalance ||
                movementType ==
                    StockMovementType.StockAdjustment;

            return !isCancelled &&
                   !isReversal &&
                   !unsupportedType;
        }
    }
}