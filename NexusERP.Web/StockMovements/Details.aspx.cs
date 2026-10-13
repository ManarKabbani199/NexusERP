using System;
using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Entities;
using NexusERP.Domain.Enums;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;

namespace NexusERP.Web.StockMovements
{
    public partial class Details : System.Web.UI.Page
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
                LoadMovementDetails();
            }
        }

        private void LoadMovementDetails()
        {
            int movementId;

            if (!int.TryParse(
                    Request.QueryString["id"],
                    out movementId))
            {
                ShowError("رقم حركة المخزون غير صحيح.");
                return;
            }

            try
            {
                StockMovement movement =
                    _stockMovementService.GetById(
                        movementId);

                /*
                 * يجب التحقق أولًا قبل استخدام movement.
                 */
                if (movement == null ||
                    movement.IsDeleted)
                {
                    ShowError(
                        "حركة المخزون غير موجودة.");

                    return;
                }

                pnlDetails.Visible = true;

                /*
                 * عرض رابط مرتجع المشتريات
                 * إذا كانت الحركة مرتبطة بمرتجع.
                 */
                if (movement.PurchaseReturnId.HasValue)
                {
                    PurchaseReturn purchaseReturn =
                        _unitOfWork.PurchaseReturns.GetById(
                            movement.PurchaseReturnId.Value);

                    pnlPurchaseReturn.Visible = true;

                    lnkPurchaseReturn.Text =
                        purchaseReturn != null
                            ? Server.HtmlEncode(
                                purchaseReturn.ReturnNumber)
                            : movement.PurchaseReturnId.Value
                                .ToString();

                    lnkPurchaseReturn.NavigateUrl =
                        "~/PurchaseReturns/Details.aspx?id=" +
                        movement.PurchaseReturnId.Value;
                }
                else
                {
                    pnlPurchaseReturn.Visible = false;
                }

                // البيانات الأساسية
                lblId.Text =
                    movement.Id.ToString();

                lblMovementType.Text =
                    GetMovementTypeName(
                        movement.MovementType);

                lblWarehouse.Text =
    movement.Warehouse != null
        ? Server.HtmlEncode(
            movement.Warehouse.NameAr)
        : "-";

                lblItem.Text =
                    movement.Item != null
                        ? Server.HtmlEncode(
                            movement.Item.NameAr)
                        : "-";

                lblQuantity.Text =
                    movement.Quantity.ToString("N2");

                lblMovementDate.Text =
                    movement.MovementDate.ToString(
                        "yyyy/MM/dd HH:mm");

                lblReferenceNumber.Text =
                    string.IsNullOrWhiteSpace(
                        movement.ReferenceNumber)
                        ? "-"
                        : Server.HtmlEncode(
                            movement.ReferenceNumber);

                lblNotes.Text =
                    string.IsNullOrWhiteSpace(
                        movement.Notes)
                        ? "-"
                        : Server.HtmlEncode(
                            movement.Notes);

                lblCreatedBy.Text =
                    string.IsNullOrWhiteSpace(
                        movement.CreatedBy)
                        ? "-"
                        : Server.HtmlEncode(
                            movement.CreatedBy);

                lblCreatedAt.Text =
                    movement.CreatedAt.ToString(
                        "yyyy/MM/dd HH:mm");

                if (movement.MovementType ==
    StockMovementType.Transfer)
                {
                    pnlDestinationWarehouse.Visible =
                        true;

                    lblDestinationWarehouse.Text =
                        movement.DestinationWarehouse != null
                            ? Server.HtmlEncode(
                                movement.DestinationWarehouse
                                    .NameAr)
                            : "-";
                }
                else
                {
                    pnlDestinationWarehouse.Visible =
                        false;
                }

                if (movement.IsCancelled)
                {
                    pnlCancelled.Visible = true;

                    lblCancellationReason.Text =
                        string.IsNullOrWhiteSpace(
                            movement.CancellationReason)
                            ? "-"
                            : Server.HtmlEncode(
                                movement.CancellationReason);

                    lblCancelledBy.Text =
                        string.IsNullOrWhiteSpace(
                            movement.CancelledBy)
                            ? "-"
                            : Server.HtmlEncode(
                                movement.CancelledBy);

                    lblCancelledAt.Text =
                        movement.CancelledAt.HasValue
                            ? movement.CancelledAt.Value
                                .ToString("yyyy/MM/dd HH:mm")
                            : "-";
                }
                else
                {
                    pnlCancelled.Visible = false;
                }

                if (movement.OriginalMovementId.HasValue)
                {
                    pnlReversal.Visible = true;

                    lnkOriginalMovement.Text =
                        movement.OriginalMovementId.Value
                            .ToString();

                    lnkOriginalMovement.NavigateUrl =
                        "Details.aspx?id=" +
                        movement.OriginalMovementId.Value;
                }
                else
                {
                    pnlReversal.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ShowError(
                    "حدث خطأ أثناء تحميل تفاصيل الحركة: " +
                    ex.Message);
            }
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
            pnlDetails.Visible = false;
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
    }
}