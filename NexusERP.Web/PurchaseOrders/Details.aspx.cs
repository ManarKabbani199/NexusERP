using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Enums;
using NexusERP.Domain.Enums.NexusERP.Domain.Enums;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;
using System.Linq;

namespace NexusERP.Web.PurchaseOrders
{
    public partial class Details : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IPurchaseOrderService _purchaseOrderService;
        private IPurchaseInvoiceService _purchaseInvoiceService;

        protected void Page_Init(object sender, EventArgs e)
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);

            _purchaseOrderService =
                new PurchaseOrderService(_unitOfWork);

            _purchaseInvoiceService =
    new PurchaseInvoiceService(
        _unitOfWork);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPurchaseOrder();
            }
        }

        private void LoadPurchaseOrder()
        {

     

            int purchaseOrderId;

            if (!int.TryParse(
                Request.QueryString["id"],
                out purchaseOrderId))
            {
                ShowError("رقم أمر الشراء غير صحيح.");
                return;
            }
            var purchaseOrder =
    _purchaseOrderService.GetById(
        purchaseOrderId);

            if (purchaseOrder == null)
            {
                ShowError(
                    "أمر الشراء غير موجود.");

                return;
            }

            lblApprovedBy.Text =
    string.IsNullOrWhiteSpace(purchaseOrder.ApprovedBy)
        ? "-"
        : Server.HtmlEncode(purchaseOrder.ApprovedBy);

lblApprovedAt.Text =
    purchaseOrder.ApprovedAt.HasValue
        ? purchaseOrder.ApprovedAt.Value.ToString(
            "yyyy-MM-dd HH:mm")
        : "-";

            bool canManageOrder =
     Request.IsAuthenticated &&
     (User.IsInRole("Admin") ||
      User.IsInRole("PurchasingManager"));

            btnCancel.Visible =
                canManageOrder &&
                (purchaseOrder.Status == PurchaseOrderStatus.Draft ||
                 purchaseOrder.Status == PurchaseOrderStatus.Approved);

            pnlCancelForm.Visible = false;

            pnlDetails.Visible = true;

            if (purchaseOrder == null)
            {
                ShowError("أمر الشراء غير موجود.");
                return;
            }

            pnlDetails.Visible = true;

            lblOrderNumber.Text =
                Server.HtmlEncode(
                    purchaseOrder.OrderNumber);

            lblOrderDate.Text =
                purchaseOrder.OrderDate
                    .ToString("yyyy-MM-dd");

            lblNotes.Text =
                string.IsNullOrWhiteSpace(
                    purchaseOrder.Notes)
                    ? "لا توجد ملاحظات"
                    : Server.HtmlEncode(
                        purchaseOrder.Notes);

            if (purchaseOrder.Supplier != null)
            {
                lblSupplierName.Text =
                    Server.HtmlEncode(
                        purchaseOrder.Supplier.NameAr);

                lblContactPerson.Text =
                    string.IsNullOrWhiteSpace(
                        purchaseOrder.Supplier.ContactPerson)
                        ? "-"
                        : Server.HtmlEncode(
                            purchaseOrder.Supplier.ContactPerson);

                lblSupplierPhone.Text =
                    string.IsNullOrWhiteSpace(
                        purchaseOrder.Supplier.Phone)
                        ? "-"
                        : Server.HtmlEncode(
                            purchaseOrder.Supplier.Phone);

                lblSupplierEmail.Text =
                    string.IsNullOrWhiteSpace(
                        purchaseOrder.Supplier.Email)
                        ? "-"
                        : Server.HtmlEncode(
                            purchaseOrder.Supplier.Email);
            }
            else
            {
                lblSupplierName.Text = "غير محدد";
                lblContactPerson.Text = "-";
                lblSupplierPhone.Text = "-";
                lblSupplierEmail.Text = "-";
            }

            lblStatus.Text =
    GetStatusName(purchaseOrder.Status);
            switch (purchaseOrder.Status)
            {
                case PurchaseOrderStatus.Draft:
                    lblStatus.CssClass =
                        "badge bg-secondary";
                    break;

                case PurchaseOrderStatus.Approved:
                    lblStatus.CssClass =
                        "badge bg-primary";
                    break;

                case PurchaseOrderStatus.PartiallyReceived:
                    lblStatus.CssClass =
                        "badge bg-warning text-dark";
                    break;

                case PurchaseOrderStatus.Received:
                    lblStatus.CssClass =
                        "badge bg-success";
                    break;

                case PurchaseOrderStatus.Cancelled:
                    lblStatus.CssClass =
                        "badge bg-danger";
                    break;
            }

            pnlCancellationDetails.Visible =
    purchaseOrder.Status ==
    PurchaseOrderStatus.Cancelled;

            if (purchaseOrder.Status ==
                PurchaseOrderStatus.Cancelled)
            {
                lblCancelledAt.Text =
                    purchaseOrder.CancelledAt.HasValue
                        ? purchaseOrder.CancelledAt.Value.ToString(
                            "yyyy-MM-dd HH:mm")
                        : "-";

                lblCancelledBy.Text =
                    string.IsNullOrWhiteSpace(
                        purchaseOrder.CancelledBy)
                        ? "-"
                        : Server.HtmlEncode(
                            purchaseOrder.CancelledBy);

                lblCancellationReason.Text =
                    string.IsNullOrWhiteSpace(
                        purchaseOrder.CancellationReason)
                        ? "-"
                        : Server.HtmlEncode(
                            purchaseOrder.CancellationReason);
            }

            btnApprove.Visible =
    canManageOrder &&
    purchaseOrder.Status == PurchaseOrderStatus.Draft;

            bool canReceive =
    purchaseOrder.Status ==
        PurchaseOrderStatus.Approved
    ||
    purchaseOrder.Status ==
        PurchaseOrderStatus.PartiallyReceived;

            lnkCreateReceipt.Visible = canReceive;

            if (canReceive)
            {
                lnkCreateReceipt.NavigateUrl =
                    "~/PurchaseReceipts/Create.aspx" +
                    "?purchaseOrderId=" +
                    purchaseOrder.Id;
            }

            var orderItems =
     purchaseOrder.Items
         .Where(orderItem =>
             !orderItem.IsDeleted)
         .Select(orderItem =>
         {
             // إجمالي الكمية المستلمة والمرحلة.
             decimal receivedQuantity =
                 _unitOfWork.PurchaseReceiptItems
                     .GetAll()
                     .Where(receiptItem =>
                         !receiptItem.IsDeleted &&
                         receiptItem.PurchaseOrderItemId ==
                             orderItem.Id &&
                         receiptItem.PurchaseReceipt != null &&
                         receiptItem.PurchaseReceipt.IsPosted &&
                         !receiptItem.PurchaseReceipt
                             .IsCancelled &&
                         !receiptItem.PurchaseReceipt
                             .IsDeleted)
                     .Select(receiptItem =>
                         receiptItem.ReceivedQuantity)
                     .DefaultIfEmpty(0)
                     .Sum();

             // إجمالي الكمية المرتجعة والمرحّلة.
             decimal returnedQuantity =
                 _unitOfWork.PurchaseReturnItems
                     .GetAll()
                     .Where(returnItem =>
                         !returnItem.IsDeleted &&
                         returnItem.PurchaseReceiptItem != null &&
                         returnItem.PurchaseReceiptItem
                             .PurchaseOrderItemId ==
                                 orderItem.Id &&
                         returnItem.PurchaseReturn != null &&
                         !returnItem.PurchaseReturn.IsDeleted &&
                         returnItem.PurchaseReturn.Status ==
                             PurchaseReturnStatus.Posted)
                     .Select(returnItem =>
                         returnItem.ReturnedQuantity)
                     .DefaultIfEmpty(0)
                     .Sum();

             // الكمية المتبقية للاستلام.
             decimal remainingQuantity =
                 orderItem.Quantity -
                 receivedQuantity;

             if (remainingQuantity < 0)
             {
                 remainingQuantity = 0;
             }

             // إجمالي الكمية الموجودة في فواتير
             // غير ملغاة.
             decimal invoicedQuantity =
                 _unitOfWork.PurchaseInvoiceItems
                     .GetAll()
                     .Where(invoiceItem =>
                         !invoiceItem.IsDeleted &&
                         invoiceItem.PurchaseOrderItemId ==
                             orderItem.Id &&
                         invoiceItem.PurchaseInvoice != null &&
                         !invoiceItem.PurchaseInvoice
                             .IsDeleted &&
                         invoiceItem.PurchaseInvoice.Status !=
                             PurchaseInvoiceStatus.Cancelled)
                     .Select(invoiceItem =>
                         invoiceItem.Quantity)
                     .DefaultIfEmpty(0)
                     .Sum();

             // لا يمكن فوترته إلا بعد استلامه.
             decimal availableToInvoice = receivedQuantity - returnedQuantity - invoicedQuantity;

             if (availableToInvoice < 0)
             {
                 availableToInvoice = 0;
             }

             if (availableToInvoice < 0)
             {
                 availableToInvoice = 0;
             }

             return new
             {
                 ItemCode =
                     orderItem.Item != null
                         ? orderItem.Item.Code
                         : "-",

                 ItemName =
                     orderItem.Item != null
                         ? orderItem.Item.NameAr
                         : "صنف غير موجود",

                 Quantity =
                     orderItem.Quantity,

                 ReceivedQuantity =
                     receivedQuantity,

                 RemainingQuantity =
                     remainingQuantity,

                 InvoicedQuantity =
                     invoicedQuantity,

                 AvailableToInvoice =
                     availableToInvoice,

                 ReturnedQuantity =
        returnedQuantity,

                 UnitPrice =
                     orderItem.UnitPrice,

                 LineTotal =
                     orderItem.Quantity *
                     orderItem.UnitPrice
             };
         })
         .ToList();

            gvOrderItems.DataSource =
                orderItems;

            gvOrderItems.DataBind();

            bool canCreateInvoice =
    purchaseOrder.Status !=
        PurchaseOrderStatus.Cancelled &&
    orderItems.Any(item =>
        item.AvailableToInvoice > 0);

            lnkCreateInvoice.Visible =
                canCreateInvoice;

            if (canCreateInvoice)
            {
                lnkCreateInvoice.NavigateUrl =
                    "~/PurchaseInvoices/Create.aspx" +
                    "?purchaseOrderId=" +
                    purchaseOrder.Id;
            }
            else
            {
                lnkCreateInvoice.NavigateUrl =
                    string.Empty;
            }




            decimal grandTotal =
                orderItems.Sum(item =>
                    item.LineTotal);

            lblGrandTotal.Text =
                grandTotal.ToString("N2");

            var purchaseReceipts =
    _unitOfWork.PurchaseReceipts
        .GetAll()
        .Where(receipt =>
            !receipt.IsDeleted &&
            receipt.PurchaseOrderId ==
                purchaseOrder.Id)
        .OrderByDescending(receipt =>
            receipt.ReceiptDate)
        .ThenByDescending(receipt =>
            receipt.Id)
        .Select(receipt => new
        {
            receipt.Id,

            receipt.ReceiptNumber,

            receipt.ReceiptDate,

            WarehouseName =
                receipt.Warehouse != null
                    ? receipt.Warehouse.NameAr
                    : "غير محدد",

            TotalQuantity =
                receipt.Items
                    .Where(item =>
                        !item.IsDeleted)
                    .Select(item =>
                        item.ReceivedQuantity)
                    .DefaultIfEmpty(0)
                    .Sum(),

            StatusName =
                receipt.IsCancelled
                    ? "ملغي"
                    : receipt.IsPosted
                        ? "مُرحّل"
                        : "مسودة"
        })
        .ToList();

            gvPurchaseReceipts.DataSource =
                purchaseReceipts;

            gvPurchaseReceipts.DataBind();

            var purchaseInvoices =
    _unitOfWork.PurchaseInvoices
        .GetAll()
        .Where(invoice =>
            !invoice.IsDeleted &&
            invoice.PurchaseOrderId ==
                purchaseOrder.Id)
        .OrderByDescending(invoice =>
            invoice.InvoiceDate)
        .ThenByDescending(invoice =>
            invoice.Id)
        .Select(invoice => new
        {
            invoice.Id,

            invoice.InvoiceNumber,

            invoice.InvoiceDate,

            invoice.DueDate,

            TotalQuantity =
                invoice.Items
                    .Where(item =>
                        !item.IsDeleted)
                    .Select(item =>
                        item.Quantity)
                    .DefaultIfEmpty(0)
                    .Sum(),

            GrandTotal =
                invoice.Items
                    .Where(item =>
                        !item.IsDeleted)
                    .Select(item =>
                        (item.Quantity *
                         item.UnitPrice) +
                        (
                            item.Quantity *
                            item.UnitPrice *
                            item.TaxRate / 100
                        ))
                    .DefaultIfEmpty(0)
                    .Sum(),

            StatusName =
                invoice.Status ==
                    PurchaseInvoiceStatus.Draft
                        ? "مسودة"
                : invoice.Status ==
                    PurchaseInvoiceStatus.Posted
                        ? "مُرحّلة"
                : invoice.Status ==
                    PurchaseInvoiceStatus.Paid
                        ? "مدفوعة"
                : invoice.Status ==
                    PurchaseInvoiceStatus.Cancelled
                        ? "ملغاة"
                : "غير معروفة"
        })
        .ToList();

            gvPurchaseInvoices.DataSource =
                purchaseInvoices;

            gvPurchaseInvoices.DataBind();

            LoadPurchaseReturns(
                purchaseOrder.Id);
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

        protected void btnApprove_Click(
    object sender,
    EventArgs e)
        {
            int purchaseOrderId;

            if (!int.TryParse(
                Request.QueryString["id"],
                out purchaseOrderId))
            {
                ShowError("رقم أمر الشراء غير صحيح.");
                return;
            }

            try
            {
                if (!Request.IsAuthenticated ||
    !(User.IsInRole("Admin") ||
      User.IsInRole("PurchasingManager")))
                {
                    ShowError("ليس لديك صلاحية اعتماد أمر شراء.");
                    return;
                }

                _purchaseOrderService.Approve(purchaseOrderId, User.Identity.Name);

                LoadPurchaseOrder();

                lblMessage.Text =
                    "تم اعتماد أمر الشراء بنجاح. " +
                    "لم تتم زيادة المخزون بعد.";

                lblMessage.CssClass =
                    "alert alert-success d-block";

                lblMessage.Visible = true;
            }
            catch (Exception exception)
            {
                ShowError(exception.Message);
            }
        }

        private string GetStatusName(
            PurchaseOrderStatus status)
        {
            switch (status)
            {
                case PurchaseOrderStatus.Draft:
                    return "مسودة";

                case PurchaseOrderStatus.Approved:
                    return "معتمد";

                case PurchaseOrderStatus.PartiallyReceived:
                    return "مستلم جزئيًا";

                case PurchaseOrderStatus.Received:
                    return "مستلم بالكامل";

                case PurchaseOrderStatus.Cancelled:
                    return "ملغي";

                default:
                    return "غير معروف";
            }
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

            int purchaseOrderId;

            if (!int.TryParse(
                Request.QueryString["id"],
                out purchaseOrderId))
            {
                ShowError(
                    "رقم أمر الشراء غير صحيح.");

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

            if (!Request.IsAuthenticated ||
    !(User.IsInRole("Admin") ||
      User.IsInRole("PurchasingManager")))
            {
                pnlCancelForm.Visible = false;
                ShowError("ليس لديك صلاحية إلغاء أمر شراء.");
                return;
            }

            string cancelledBy = User.Identity.Name;

            try
            {
                _purchaseOrderService.Cancel(
                    purchaseOrderId,
                    cancellationReason,
                    cancelledBy);

                txtCancellationReason.Text =
                    string.Empty;

                LoadPurchaseOrder();

                ShowSuccess(
                    "تم إلغاء أمر الشراء بنجاح.");
            }
            catch (Exception exception)
            {
                LoadPurchaseOrder();

                pnlCancelForm.Visible = true;

                ShowError(exception.Message);
            }
        }

        protected void btnCancel_Click(
    object sender,
    EventArgs e)
        {
            if (!Request.IsAuthenticated ||
    !(User.IsInRole("Admin") ||
      User.IsInRole("PurchasingManager")))
            {
                ShowError("ليس لديك صلاحية إلغاء أمر شراء.");
                return;
            }
            pnlCancelForm.Visible = true;

            lblMessage.Visible = false;
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

        private void LoadPurchaseReturns(
    int purchaseOrderId)
        {
            var purchaseReturns =
                _unitOfWork.PurchaseReturns
                    .GetAll()
                    .Where(purchaseReturn =>
                        !purchaseReturn.IsDeleted &&
                        purchaseReturn.PurchaseReceipt != null &&
                        purchaseReturn.PurchaseReceipt
                            .PurchaseOrderId ==
                                purchaseOrderId)
                    .OrderByDescending(purchaseReturn =>
                        purchaseReturn.ReturnDate)
                    .Select(purchaseReturn => new
                    {
                        purchaseReturn.Id,

                        purchaseReturn.ReturnNumber,

                        purchaseReturn.ReturnDate,

                        ReceiptNumber =
                            purchaseReturn.PurchaseReceipt != null
                                ? purchaseReturn.PurchaseReceipt
                                    .ReceiptNumber
                                : "-",

                        WarehouseName =
                            purchaseReturn.Warehouse != null
                                ? purchaseReturn.Warehouse.NameAr
                                : "-",

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
    }
}