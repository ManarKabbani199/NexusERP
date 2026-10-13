using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Entities;
using NexusERP.Domain.Enums.NexusERP.Domain.Enums;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;
using System.Linq;

namespace NexusERP.Web.PurchaseReceipts
{
    public partial class Details : System.Web.UI.Page
    {
        private ApplicationDbContext _context;

        private IUnitOfWork _unitOfWork;

        private IStockMovementService
            _stockMovementService;

        private IPurchaseReceiptService
            _purchaseReceiptService;

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

            _purchaseReceiptService =
                new PurchaseReceiptService(
                    _unitOfWork,
                    _stockMovementService);
        }

        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadReceipt();
            }
        }

        private void LoadReceipt()
        {
            int receiptId;

            if (!int.TryParse(
                Request.QueryString["id"],
                out receiptId))
            {
                ShowError(
                    "رقم مستند الاستلام غير صحيح.");

                pnlDetails.Visible = false;

                return;
            }

            PurchaseReceipt receipt =
                _purchaseReceiptService.GetById(
                    receiptId);

            if (receipt == null)
            {
                ShowError(
                    "مستند الاستلام غير موجود.");

                pnlDetails.Visible = false;

                return;
            }

            pnlDetails.Visible = true;

            lblReceiptNumber.Text =
                Server.HtmlEncode(
                    receipt.ReceiptNumber);

            lblReceiptDate.Text =
                receipt.ReceiptDate.ToString(
                    "yyyy-MM-dd");

            SetReceiptStatus(receipt);

            LoadCancellationDetails(receipt);

            lblPostedAt.Text =
                receipt.PostedAt.HasValue
                    ? receipt.PostedAt.Value.ToString(
                        "yyyy-MM-dd HH:mm")
                    : "-";
            lblPostedBy.Text =
    string.IsNullOrWhiteSpace(receipt.PostedBy)
        ? "-"
        : Server.HtmlEncode(receipt.PostedBy);

            lblPurchaseOrder.Text =
                receipt.PurchaseOrder != null
                    ? Server.HtmlEncode(
                        receipt.PurchaseOrder.OrderNumber)
                    : "-";

            lblSupplier.Text =
                receipt.PurchaseOrder != null &&
                receipt.PurchaseOrder.Supplier != null
                    ? Server.HtmlEncode(
                        receipt.PurchaseOrder
                            .Supplier.NameAr)
                    : "-";

            lblWarehouse.Text =
                receipt.Warehouse != null
                    ? Server.HtmlEncode(
                        receipt.Warehouse.NameAr)
                    : "-";

            lblNotes.Text =
                string.IsNullOrWhiteSpace(
                    receipt.Notes)
                    ? "لا توجد ملاحظات"
                    : Server.HtmlEncode(
                        receipt.Notes);

            bool canManageReceipt =
Request.IsAuthenticated &&
(User.IsInRole("Admin") ||
User.IsInRole("WarehouseManager"));

            btnPost.Visible =
                canManageReceipt &&
                !receipt.IsPosted &&
                !receipt.IsCancelled;

            btnShowCancel.Visible =
                canManageReceipt &&
                !receipt.IsCancelled;

            bool canCreateReturn =
    receipt.IsPosted &&
    !receipt.IsCancelled;

            lnkCreatePurchaseReturn.Visible =
                canCreateReturn;

            if (canCreateReturn)
            {
                lnkCreatePurchaseReturn.NavigateUrl =
                    "~/PurchaseReturns/Create.aspx" +
                    "?purchaseReceiptId=" +
                    receipt.Id;
            }

            pnlCancelForm.Visible = false;

            LoadReceiptItems(receipt);

            LoadPurchaseReturns(
    receipt.Id);
        }

        private void SetReceiptStatus(
            PurchaseReceipt receipt)
        {
            if (receipt.IsCancelled)
            {
                lblStatus.Text = "ملغي";

                lblStatus.CssClass =
                    "badge bg-danger";
            }
            else if (receipt.IsPosted)
            {
                lblStatus.Text = "مُرحّل";

                lblStatus.CssClass =
                    "badge bg-success";
            }
            else
            {
                lblStatus.Text = "مسودة";

                lblStatus.CssClass =
                    "badge bg-secondary";
            }
        }

        private void LoadCancellationDetails(
            PurchaseReceipt receipt)
        {
            pnlCancellationDetails.Visible =
                receipt.IsCancelled;

            if (!receipt.IsCancelled)
            {
                lblCancelledAt.Text = string.Empty;
                lblCancelledBy.Text = string.Empty;
                lblCancellationReason.Text = string.Empty;

                return;
            }

            lblCancelledAt.Text =
                receipt.CancelledAt.HasValue
                    ? receipt.CancelledAt.Value.ToString(
                        "yyyy-MM-dd HH:mm")
                    : "-";

            lblCancelledBy.Text =
                string.IsNullOrWhiteSpace(
                    receipt.CancelledBy)
                    ? "-"
                    : Server.HtmlEncode(
                        receipt.CancelledBy);

            lblCancellationReason.Text =
                string.IsNullOrWhiteSpace(
                    receipt.CancellationReason)
                    ? "-"
                    : Server.HtmlEncode(
                        receipt.CancellationReason);
        }

        
        private void LoadReceiptItems(
    PurchaseReceipt receipt)
        {
            var receiptItems =
                receipt.Items
                    .Where(item =>
                        !item.IsDeleted)
                    .Select(item =>
                    {
                        decimal returnedQuantity =
                            _unitOfWork.PurchaseReturnItems
                                .GetAll()
                                .Where(returnItem =>
                                    returnItem.PurchaseReceiptItemId ==
                                        item.Id &&
                                    !returnItem.IsDeleted &&
                                    returnItem.PurchaseReturn != null &&
                                    !returnItem.PurchaseReturn.IsDeleted &&
                                    returnItem.PurchaseReturn.Status ==
                                        PurchaseReturnStatus.Posted)
                                .Select(returnItem =>
                                    (decimal?)
                                        returnItem.ReturnedQuantity)
                                .Sum() ?? 0;

                        decimal netReceivedQuantity =
                            item.ReceivedQuantity -
                            returnedQuantity;

                        return new
                        {
                            ItemCode =
                                item.PurchaseOrderItem != null &&
                                item.PurchaseOrderItem.Item != null
                                    ? item.PurchaseOrderItem
                                        .Item.Code
                                    : "-",

                            ItemName =
                                item.PurchaseOrderItem != null &&
                                item.PurchaseOrderItem.Item != null
                                    ? item.PurchaseOrderItem
                                        .Item.NameAr
                                    : "صنف غير موجود",

                            OrderedQuantity =
                                item.PurchaseOrderItem != null
                                    ? item.PurchaseOrderItem
                                        .Quantity
                                    : 0,

                            ReceivedQuantity =
                                item.ReceivedQuantity,

                            ReturnedQuantity =
                                returnedQuantity,

                            NetReceivedQuantity =
                                netReceivedQuantity,

                            UnitPrice =
                                item.PurchaseOrderItem != null
                                    ? item.PurchaseOrderItem
                                        .UnitPrice
                                    : 0,

                            LineTotal =
                                item.PurchaseOrderItem != null
                                    ? item.ReceivedQuantity *
                                      item.PurchaseOrderItem
                                          .UnitPrice
                                    : 0,

                            NetLineTotal =
                                item.PurchaseOrderItem != null
                                    ? netReceivedQuantity *
                                      item.PurchaseOrderItem
                                          .UnitPrice
                                    : 0
                        };
                    })
                    .ToList();

            gvReceiptItems.DataSource =
                receiptItems;

            gvReceiptItems.DataBind();

            decimal grandTotal =
                receiptItems.Sum(item =>
                    item.LineTotal);

            decimal returnedTotal =
                receiptItems.Sum(item =>
                    item.ReturnedQuantity *
                    item.UnitPrice);

            decimal netTotal =
                receiptItems.Sum(item =>
                    item.NetLineTotal);

            lblGrandTotal.Text =
                grandTotal.ToString("N2");

            lblReturnedTotal.Text =
                returnedTotal.ToString("N2");

            lblNetTotal.Text =
                netTotal.ToString("N2");
        }
        

        protected void btnPost_Click(
            object sender,
            EventArgs e)
        {
            int receiptId;

            if (!int.TryParse(
                Request.QueryString["id"],
                out receiptId))
            {
                ShowError(
                    "رقم مستند الاستلام غير صحيح.");

                return;
            }

            try
            {
                string postedBy = GetAuthorizedUserName();

                _purchaseReceiptService.Post(
                    receiptId,
                    postedBy);

            }
            catch (Exception exception)
            {
                LoadReceipt();

                ShowError(exception.Message);
            }
        }

        protected void btnShowCancel_Click(
            object sender,
            EventArgs e)
        {
            pnlCancelForm.Visible = true;

            lblMessage.Visible = false;
        }

        protected void btnCloseCancel_Click(
            object sender,
            EventArgs e)
        {
            pnlCancelForm.Visible = false;

            txtCancellationReason.Text =
                string.Empty;
        }

        protected void btnConfirmCancel_Click(
            object sender,
            EventArgs e)
        {
            if (!Page.IsValid)
            {
                pnlCancelForm.Visible = true;

                return;
            }

            int receiptId;

            if (!int.TryParse(
                Request.QueryString["id"],
                out receiptId))
            {
                ShowError(
                    "رقم مستند الاستلام غير صحيح.");

                pnlCancelForm.Visible = true;

                return;
            }

            string cancellationReason =
                txtCancellationReason.Text.Trim();

            if (string.IsNullOrWhiteSpace(
                cancellationReason))
            {
                ShowError(
                    "سبب الإلغاء مطلوب.");

                pnlCancelForm.Visible = true;

                return;
            }
            try
            {
                string cancelledBy = GetAuthorizedUserName();

                _purchaseReceiptService.Cancel(
                    receiptId,
                    cancellationReason,
                    cancelledBy);
            }

            catch (Exception exception)
            {
                LoadReceipt();

                pnlCancelForm.Visible = true;

                ShowError(exception.Message);
            }
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

        private void LoadPurchaseReturns(
    int receiptId)
        {
            var purchaseReturns =
                _unitOfWork.PurchaseReturns
                    .GetAll()
                    .Where(purchaseReturn =>
                        purchaseReturn.PurchaseReceiptId ==
                            receiptId &&
                        !purchaseReturn.IsDeleted)
                    .OrderByDescending(purchaseReturn =>
                        purchaseReturn.ReturnDate)
                    .Select(purchaseReturn => new
                    {
                        purchaseReturn.Id,

                        purchaseReturn.ReturnNumber,

                        purchaseReturn.ReturnDate,

                        ItemsCount =
                            purchaseReturn.Items.Count(item =>
                                !item.IsDeleted),

                        TotalQuantity =
                            purchaseReturn.Items
                                .Where(item =>
                                    !item.IsDeleted)
                                .Select(item =>
                                    (decimal?)
                                        item.ReturnedQuantity)
                                .Sum() ?? 0,

                        TotalValue =
                            purchaseReturn.Items
                                .Where(item =>
                                    !item.IsDeleted)
                                .Select(item =>
                                    (decimal?)
                                    (item.ReturnedQuantity *
                                     item.UnitPrice))
                                .Sum() ?? 0,

                        StatusName =
                            purchaseReturn.Status ==
                                PurchaseReturnStatus.Draft
                                    ? "مسودة"
                                    : purchaseReturn.Status ==
                                        PurchaseReturnStatus.Posted
                                            ? "مُرحّل"
                                            : purchaseReturn.Status ==
                                                PurchaseReturnStatus.Cancelled
                                                    ? "ملغي"
                                                    : "غير معروف"
                    })
                    .ToList();

            gvPurchaseReturns.DataSource =
                purchaseReturns;

            gvPurchaseReturns.DataBind();
        }


        private string GetAuthorizedUserName()
        {
            if (User == null ||
                User.Identity == null ||
                !User.Identity.IsAuthenticated)
            {
                throw new System.Web.HttpException(
                    401,
                    "يجب تسجيل الدخول لتنفيذ العملية.");
            }

            if (!User.IsInRole("Admin") &&
                !User.IsInRole("WarehouseManager"))
            {
                throw new System.Web.HttpException(
                    403,
                    "ليس لديك صلاحية تنفيذ العملية.");
            }

            string userName = User.Identity.Name;

            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new InvalidOperationException(
                    "تعذر تحديد المستخدم الحالي.");
            }

            return userName.Trim();
        }
    }
}