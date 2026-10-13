using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Entities;
using NexusERP.Domain.Enums;
using NexusERP.Domain.Enums.NexusERP.Domain.Enums;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;
using System.Linq;

namespace NexusERP.Web.PurchaseReturns
{
    public partial class Details : System.Web.UI.Page
    {
        private ApplicationDbContext _context;

        private IUnitOfWork _unitOfWork;

        private IStockMovementService
            _stockMovementService;

        private IPurchaseReturnService
            _purchaseReturnService;

        protected void Page_Init(
            object sender,
            EventArgs e)
        {
            _context =
                new ApplicationDbContext();

            _unitOfWork =
                new UnitOfWork(_context);

            _stockMovementService =
                new StockMovementService(
                    _unitOfWork);

            _purchaseReturnService =
                new PurchaseReturnService(
                    _unitOfWork,
                    _stockMovementService);
        }

        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPurchaseReturn();
            }
        }

        private void LoadPurchaseReturn()
        {
            int purchaseReturnId;

            if (!TryGetPurchaseReturnId(
                out purchaseReturnId))
            {
                ShowError(
                    "رقم مرتجع المشتريات غير صحيح.");

                pnlDetails.Visible = false;
                return;
            }

            PurchaseReturn purchaseReturn =
                _purchaseReturnService.GetById(
                    purchaseReturnId);

            if (purchaseReturn == null)
            {
                ShowError(
                    "مرتجع المشتريات غير موجود.");

                pnlDetails.Visible = false;
                return;
            }

            pnlDetails.Visible = true;

            lblReturnNumber.Text =
                Server.HtmlEncode(
                    purchaseReturn.ReturnNumber);

            lblReturnDate.Text =
                purchaseReturn.ReturnDate.ToString(
                    "yyyy-MM-dd");

            lblPostedAt.Text =
                purchaseReturn.PostedAt.HasValue
                    ? purchaseReturn.PostedAt.Value
                        .ToString("yyyy-MM-dd HH:mm")
                    : "-";

            lblReceiptNumber.Text =
                purchaseReturn.PurchaseReceipt != null
                    ? Server.HtmlEncode(
                        purchaseReturn.PurchaseReceipt
                            .ReceiptNumber)
                    : "-";

            lblPurchaseOrder.Text =
                purchaseReturn.PurchaseReceipt != null &&
                purchaseReturn.PurchaseReceipt
                    .PurchaseOrder != null
                    ? Server.HtmlEncode(
                        purchaseReturn.PurchaseReceipt
                            .PurchaseOrder.OrderNumber)
                    : "-";

            lblSupplier.Text =
                purchaseReturn.PurchaseReceipt != null &&
                purchaseReturn.PurchaseReceipt
                    .PurchaseOrder != null &&
                purchaseReturn.PurchaseReceipt
                    .PurchaseOrder.Supplier != null
                    ? Server.HtmlEncode(
                        purchaseReturn.PurchaseReceipt
                            .PurchaseOrder.Supplier.NameAr)
                    : "-";

            lblWarehouse.Text =
                purchaseReturn.Warehouse != null
                    ? Server.HtmlEncode(
                        purchaseReturn.Warehouse.NameAr)
                    : "-";

            lblNotes.Text =
                string.IsNullOrWhiteSpace(
                    purchaseReturn.Notes)
                    ? "لا توجد ملاحظات"
                    : Server.HtmlEncode(
                        purchaseReturn.Notes);

            SetStatus(purchaseReturn.Status);

            LoadCancellationDetails(
                purchaseReturn);

            LoadReturnItems(
                purchaseReturn);

            bool canManageReturn =
    Request.IsAuthenticated &&
    (User.IsInRole("Admin") ||
     User.IsInRole("PurchasingManager") ||
     User.IsInRole("WarehouseManager"));

            btnPost.Visible =
                canManageReturn &&
                purchaseReturn.Status == PurchaseReturnStatus.Draft;

            btnShowCancel.Visible =
                canManageReturn &&
                (purchaseReturn.Status == PurchaseReturnStatus.Draft ||
                 purchaseReturn.Status == PurchaseReturnStatus.Posted);

            if (!btnShowCancel.Visible)
            {
                pnlCancelForm.Visible = false;
            }

            if (purchaseReturn.Status ==
                PurchaseReturnStatus.Cancelled)
            {
                pnlCancelForm.Visible = false;
            }
        }

        private void SetStatus(
            PurchaseReturnStatus status)
        {
            switch (status)
            {
                case PurchaseReturnStatus.Draft:

                    lblStatus.Text = "مسودة";
                    lblStatus.CssClass =
                        "badge bg-secondary";

                    break;

                case PurchaseReturnStatus.Posted:

                    lblStatus.Text = "مُرحّل";
                    lblStatus.CssClass =
                        "badge bg-success";

                    break;

                case PurchaseReturnStatus.Cancelled:

                    lblStatus.Text = "ملغي";
                    lblStatus.CssClass =
                        "badge bg-danger";

                    break;

                default:

                    lblStatus.Text = "غير معروف";
                    lblStatus.CssClass =
                        "badge bg-dark";

                    break;
            }
        }

        private void LoadCancellationDetails(
            PurchaseReturn purchaseReturn)
        {
            bool isCancelled =
                purchaseReturn.Status ==
                    PurchaseReturnStatus.Cancelled;

            pnlCancellationDetails.Visible =
                isCancelled;

            if (!isCancelled)
            {
                return;
            }

            lblCancelledAt.Text =
                purchaseReturn.CancelledAt.HasValue
                    ? purchaseReturn.CancelledAt.Value
                        .ToString("yyyy-MM-dd HH:mm")
                    : "-";

            lblCancelledBy.Text =
                string.IsNullOrWhiteSpace(
                    purchaseReturn.CancelledBy)
                    ? "-"
                    : Server.HtmlEncode(
                        purchaseReturn.CancelledBy);

            lblCancellationReason.Text =
                string.IsNullOrWhiteSpace(
                    purchaseReturn.CancellationReason)
                    ? "-"
                    : Server.HtmlEncode(
                        purchaseReturn
                            .CancellationReason);
        }

        private void LoadReturnItems(
            PurchaseReturn purchaseReturn)
        {
            var returnItems =
                purchaseReturn.Items
                    .Where(returnItem =>
                        !returnItem.IsDeleted)
                    .Select(returnItem => new
                    {
                        ItemCode =
                            returnItem.PurchaseReceiptItem != null &&
                            returnItem.PurchaseReceiptItem
                                .PurchaseOrderItem != null &&
                            returnItem.PurchaseReceiptItem
                                .PurchaseOrderItem.Item != null
                                ? returnItem.PurchaseReceiptItem
                                    .PurchaseOrderItem.Item.Code
                                : "-",

                        ItemName =
                            returnItem.PurchaseReceiptItem != null &&
                            returnItem.PurchaseReceiptItem
                                .PurchaseOrderItem != null &&
                            returnItem.PurchaseReceiptItem
                                .PurchaseOrderItem.Item != null
                                ? returnItem.PurchaseReceiptItem
                                    .PurchaseOrderItem.Item.NameAr
                                : "صنف غير موجود",

                        ReceivedQuantity =
                            returnItem.PurchaseReceiptItem != null
                                ? returnItem.PurchaseReceiptItem
                                    .ReceivedQuantity
                                : 0,

                        returnItem.ReturnedQuantity,

                        returnItem.UnitPrice,

                        LineTotal =
                            returnItem.ReturnedQuantity *
                            returnItem.UnitPrice
                    })
                    .ToList();

            gvReturnItems.DataSource =
                returnItems;

            gvReturnItems.DataBind();

            decimal grandTotal =
                returnItems.Sum(item =>
                    item.LineTotal);

            lblGrandTotal.Text =
                grandTotal.ToString("N2");
        }

        protected void btnPost_Click(
            object sender,
            EventArgs e)
        {
            int purchaseReturnId;

            if (!TryGetPurchaseReturnId(
                out purchaseReturnId))
            {
                ShowError(
                    "رقم مرتجع المشتريات غير صحيح.");

                return;
            }

            try
            {
                _purchaseReturnService.Post(
                    purchaseReturnId,
                    GetCurrentUserName());

                LoadPurchaseReturn();

                ShowSuccess(
                    "تم ترحيل مرتجع المشتريات بنجاح، " +
                    "وتم إنقاص المخزون.");
            }
            catch (Exception exception)
            {
                LoadPurchaseReturn();

                ShowError(
                    exception.Message);
            }
        }

        protected void btnShowCancel_Click(
    object sender,
    EventArgs e)
        {
            try
            {
                GetCurrentUserName();

                lblMessage.Visible = false;
                txtCancellationReason.Text = string.Empty;
                pnlCancelForm.Visible = true;
            }
            catch (Exception exception)
            {
                pnlCancelForm.Visible = false;
                ShowError(exception.Message);
            }
        }
        protected void btnCloseCancel_Click(
            object sender,
            EventArgs e)
        {
            txtCancellationReason.Text =
                string.Empty;

            pnlCancelForm.Visible = false;
        }

        protected void btnConfirmCancel_Click(
            object sender,
            EventArgs e)
        {
            Page.Validate(
                "CancelPurchaseReturn");

            if (!Page.IsValid)
            {
                pnlCancelForm.Visible = true;
                return;
            }

            int purchaseReturnId;

            if (!TryGetPurchaseReturnId(
                out purchaseReturnId))
            {
                ShowError(
                    "رقم مرتجع المشتريات غير صحيح.");

                pnlCancelForm.Visible = true;
                return;
            }

            string cancellationReason =
                txtCancellationReason.Text.Trim();

            if (string.IsNullOrWhiteSpace(
                cancellationReason))
            {
                ShowError(
                    "سبب إلغاء المرتجع مطلوب.");

                pnlCancelForm.Visible = true;
                return;
            }

            try
            {
                _purchaseReturnService.Cancel(
                    purchaseReturnId,
                    cancellationReason,
                    GetCurrentUserName());

                txtCancellationReason.Text =
                    string.Empty;

                pnlCancelForm.Visible = false;

                LoadPurchaseReturn();

                ShowSuccess(
                    "تم إلغاء مرتجع المشتريات بنجاح. " +
                    "إذا كان مُرحّلًا فقد تمت إعادة " +
                    "الكميات إلى المخزون.");
            }
            catch (Exception exception)
            {
                LoadPurchaseReturn();

                pnlCancelForm.Visible = true;

                ShowError(
                    exception.Message);
            }
        }

        private bool TryGetPurchaseReturnId(
            out int purchaseReturnId)
        {
            return int.TryParse(
                Request.QueryString["id"],
                out purchaseReturnId);
        }

        private string GetCurrentUserName()
        {
            if (!Request.IsAuthenticated ||
                !(User.IsInRole("Admin") ||
                  User.IsInRole("PurchasingManager") ||
                  User.IsInRole("WarehouseManager")))
            {
                throw new System.Web.HttpException(
                    403,
                    "ليس لديك صلاحية إدارة مرتجعات المشتريات.");
            }

            string userName = User.Identity.Name;

            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new InvalidOperationException(
                    "تعذر تحديد المستخدم الحالي.");
            }

            userName = userName.Trim();

            if (userName.Length > 256)
            {
                throw new InvalidOperationException(
                    "اسم المستخدم لا يمكن أن يتجاوز 256 حرفًا.");
            }

            return userName;
        }

        private void ShowSuccess(
            string message)
        {
            lblMessage.Text =
                Server.HtmlEncode(message);

            lblMessage.CssClass =
                "alert alert-success d-block";

            lblMessage.Visible = true;
        }

        private void ShowError(
            string message)
        {
            lblMessage.Text =
                Server.HtmlEncode(message);

            lblMessage.CssClass =
                "alert alert-danger d-block";

            lblMessage.Visible = true;
        }

        protected void Page_Unload(
            object sender,
            EventArgs e)
        {
            if (_unitOfWork != null)
            {
                _unitOfWork.Dispose();
                _unitOfWork = null;
                _context = null;
            }
            else if (_context != null)
            {
                _context.Dispose();
                _context = null;
            }
        }
    }
}