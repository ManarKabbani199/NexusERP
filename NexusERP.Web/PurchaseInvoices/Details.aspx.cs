using System;
using System.Linq;
using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Entities;
using NexusERP.Domain.Enums;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;

namespace NexusERP.Web.PurchaseInvoices
{
    public partial class Details : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IPurchaseInvoiceService _purchaseInvoiceService;
        private IPurchasePaymentService _purchasePaymentService;


        protected string Translate(string key)
        {
            return Convert.ToString(GetGlobalResourceObject(
                "PurchaseInvoiceDetails", key));
        }

        private bool CanManageInvoice
        {
            get
            {
                return Request.IsAuthenticated &&
                    (User.IsInRole("Admin") || User.IsInRole("Accountant"));
            }
        }

        private string ConfirmationScript(string key)
        {
            return "return confirm(" +
                System.Web.HttpUtility.JavaScriptStringEncode(
                    Translate(key), true) + ");";
        }

        protected void Page_Init(
            object sender,
            EventArgs e)
        {
            btnPost.OnClientClick = ConfirmationScript("ConfirmPost");
            btnMarkAsPaid.OnClientClick = ConfirmationScript("ConfirmPaid");
            btnConfirmCancelInvoice.OnClientClick = ConfirmationScript("ConfirmCancel");

            _context =
                new ApplicationDbContext();

            _unitOfWork =
                new UnitOfWork(_context);

            _purchaseInvoiceService =
                new PurchaseInvoiceService(
                    _unitOfWork);

            _purchasePaymentService =
                new PurchasePaymentService(
                 _unitOfWork);
        }

        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadInvoice();
            }
        }

        private void LoadInvoice()
        {
            pnlDetails.Visible = false;

            int invoiceId;

            if (!TryGetInvoiceId(out invoiceId))
            {
                ShowError(
                    Translate("InvalidInvoiceId"));

                return;
            }

            PurchaseInvoice invoice =
                _purchaseInvoiceService.GetById(
                    invoiceId);

            if (invoice == null ||
                invoice.IsDeleted)
            {
                ShowError(
                    Translate("InvoiceNotFound"));

                return;
            }

            /*
             * بنود الفاتورة الفعالة.
             */
            var activeItems =
                invoice.Items
                    .Where(item =>
                        !item.IsDeleted)
                    .ToList();

            /*
             * المجموع قبل الخصم والضريبة.
             */
            decimal subtotal =
                activeItems.Sum(item =>
                    item.Quantity *
                    item.UnitPrice);

            decimal discountAmount =
                invoice.DiscountAmount;

            if (discountAmount < 0)
            {
                discountAmount = 0;
            }

            if (discountAmount > subtotal)
            {
                discountAmount = subtotal;
            }

            decimal amountAfterDiscount =
                subtotal - discountAmount;

            /*
             * مجموع ضريبة بنود الفاتورة.
             */
            decimal taxAmount =
                activeItems.Sum(item =>
                    item.Quantity *
                    item.UnitPrice *
                    item.TaxRate / 100m);

            decimal invoiceTotal =
                amountAfterDiscount +
                taxAmount;

            /*
             * مجموع الدفعات الفعالة فقط.
             */
            decimal paidAmount =
                _purchasePaymentService
                    .GetPaidAmount(invoice.Id);

            decimal remainingAmount =
                invoiceTotal - paidAmount;

            if (remainingAmount < 0)
            {
                remainingAmount = 0;
            }

            /*
             * فحص وجود أي دفعة فعالة مرتبطة بالفاتورة.
             */
            bool hasActivePayments =
                _unitOfWork.PurchasePayments
                    .GetAll()
                    .Any(payment =>
                        payment.PurchaseInvoiceId ==
                            invoice.Id &&
                        !payment.IsDeleted &&
                        !payment.IsCancelled);

            /*
             * إظهار تسجيل دفعة عندما تكون الفاتورة
             * مرحلة وما زال عليها مبلغ متبقٍ.
             */
            bool canCreatePayment =
                CanManageInvoice &&
                invoice.Status ==
                    PurchaseInvoiceStatus.Posted &&
                remainingAmount > 0;

            lnkCreatePayment.Visible =
                canCreatePayment;

            if (canCreatePayment)
            {
                lnkCreatePayment.NavigateUrl =
                    "~/PurchasePayments/Create.aspx" +
                    "?invoiceId=" +
                    invoice.Id;
            }
            else
            {
                lnkCreatePayment.NavigateUrl =
                    string.Empty;
            }

            pnlDetails.Visible = true;

            lblInvoiceNumber.Text =
                Server.HtmlEncode(
                    invoice.InvoiceNumber);

            lblInvoiceDate.Text =
                invoice.InvoiceDate.ToString(
                    "yyyy-MM-dd");

            lblDueDate.Text =
                invoice.DueDate.HasValue
                    ? invoice.DueDate.Value.ToString(
                        "yyyy-MM-dd")
                    : "-";

            lblPurchaseOrder.Text =
                invoice.PurchaseOrder != null
                    ? Server.HtmlEncode(
                        invoice.PurchaseOrder
                            .OrderNumber)
                    : "-";

            lblSupplier.Text =
                invoice.Supplier != null
                    ? Server.HtmlEncode(invoice.Supplier.NameAr)
                    : "-";

            lblPostedAt.Text =
                invoice.PostedAt.HasValue
                    ? invoice.PostedAt.Value.ToString(
                        "yyyy-MM-dd HH:mm")
                    : "-";

            lblNotes.Text =
                string.IsNullOrWhiteSpace(
                    invoice.Notes)
                    ? Translate("NoNotes")
                    : Server.HtmlEncode(
                        invoice.Notes);

            /*
             * تحميل جدول البنود أولًا.
             */
            LoadInvoiceItems(invoice);

            /*
             * عرض الإجماليات بعد تحميل الجدول.
             */
            lblSubTotal.Text =
                subtotal.ToString("N2");

            lblDiscountAmount.Text =
                discountAmount.ToString("N2");

            lblTaxAmount.Text =
                taxAmount.ToString("N2");

            lblGrandTotal.Text =
                invoiceTotal.ToString("N2");

            lblPaidAmount.Text =
                paidAmount.ToString("N2");

            lblRemainingAmount.Text =
                remainingAmount.ToString("N2");

            SetInvoiceStatus(
                invoice.Status);

            LoadCancellationDetails(
                invoice);

            /*
             * يمكن ترحيل الفاتورة المسودة فقط.
             */
            bool canManageInvoice =
    Request.IsAuthenticated &&
    (User.IsInRole("Admin") ||
     User.IsInRole("Accountant"));

            btnPost.Visible =
                canManageInvoice &&
                invoice.Status == PurchaseInvoiceStatus.Draft;

            /*
             * الدفع يتم من شاشة دفعات الموردين.
             */
            btnMarkAsPaid.Visible = false;

            /*
             * إظهار زر الإلغاء فقط عندما:
             * - الفاتورة مسودة أو مرحلة.
             * - لا تحتوي على دفعات فعالة.
             */
            bool canCancelInvoice =
                CanManageInvoice &&
                !hasActivePayments &&
                (
                    invoice.Status ==
                        PurchaseInvoiceStatus.Draft
                    ||
                    invoice.Status ==
                        PurchaseInvoiceStatus.Posted
                );

            btnShowCancelInvoice.Visible =
                canCancelInvoice;

            /*
             * إغلاق نموذج الإلغاء إذا أصبحت
             * عملية الإلغاء غير مسموحة.
             */
            if (!canCancelInvoice)
            {
                pnlCancelInvoiceForm.Visible =
                    false;
            }

            /*
             * تحميل سجل دفعات الفاتورة.
             */
            LoadPayments(
                invoice.Id);
        }


        private void LoadPayments(
 int purchaseInvoiceId)
        {
            var payments =
                _purchasePaymentService
                    .GetByInvoiceId(
                        purchaseInvoiceId)
                    .Select(payment => new
                    {
                        payment.Id,

                        payment.PaymentNumber,

                        payment.PaymentDate,

                        payment.Amount,

                        payment.IsCancelled,

                        PaymentMethodName =
                            GetPaymentMethodName(
                                payment.PaymentMethod)
                    })
                    .ToList();

            gvPurchasePayments.DataSource =
                payments;

            gvPurchasePayments.DataBind();

            decimal invoiceTotal =
                _purchasePaymentService
                    .GetInvoiceTotal(
                        purchaseInvoiceId);

            decimal paidAmount =
                _purchasePaymentService
                    .GetPaidAmount(
                        purchaseInvoiceId);

            decimal remainingAmount =
                _purchasePaymentService
                    .GetRemainingAmount(
                        purchaseInvoiceId);

            lblPaymentInvoiceTotal.Text =
                invoiceTotal.ToString("N2");

            lblPaymentPaidAmount.Text =
                paidAmount.ToString("N2");

            lblPaymentRemainingAmount.Text =
                remainingAmount.ToString("N2");
        }



        //طريقة الدفع

        private string GetPaymentMethodName(
    PaymentMethod paymentMethod)
        {
            switch (paymentMethod)
            {
                case PaymentMethod.Cash:
                    return Translate("Cash");

                case PaymentMethod.BankTransfer:
                    return Translate("BankTransfer");

                case PaymentMethod.Cheque:
                    return Translate("Cheque");

                case PaymentMethod.CreditCard:
                    return Translate("CreditCard");

                default:
                    return Translate("Unknown");
            }
        }


        private void SetInvoiceStatus(
            PurchaseInvoiceStatus status)
        {
            switch (status)
            {
                case PurchaseInvoiceStatus.Draft:

                    lblStatus.Text = Translate("Draft");
                    lblStatus.CssClass =
                        "badge bg-secondary";

                    break;

                case PurchaseInvoiceStatus.Posted:

                    lblStatus.Text = Translate("Posted");
                    lblStatus.CssClass =
                        "badge bg-success";

                    break;

                case PurchaseInvoiceStatus.Paid:

                    lblStatus.Text = Translate("Paid");
                    lblStatus.CssClass =
                        "badge bg-primary";

                    break;

                case PurchaseInvoiceStatus.Cancelled:

                    lblStatus.Text = Translate("Cancelled");
                    lblStatus.CssClass =
                        "badge bg-danger";

                    break;

                default:

                    lblStatus.Text = Translate("Unknown");
                    lblStatus.CssClass =
                        "badge bg-dark";

                    break;
            }
        }

        private void LoadCancellationDetails(
            PurchaseInvoice invoice)
        {
            bool isCancelled =
                invoice.Status ==
                    PurchaseInvoiceStatus.Cancelled;

            pnlCancellationDetails.Visible =
                isCancelled;

            if (!isCancelled)
            {
                lblCancelledAt.Text = string.Empty;
                lblCancelledBy.Text = string.Empty;
                lblCancellationReason.Text =
                    string.Empty;

                return;
            }

            lblCancelledAt.Text =
                invoice.CancelledAt.HasValue
                    ? invoice.CancelledAt.Value
                        .ToString("yyyy-MM-dd HH:mm")
                    : "-";

            lblCancelledBy.Text =
                string.IsNullOrWhiteSpace(
                    invoice.CancelledBy)
                    ? "-"
                    : Server.HtmlEncode(
                        invoice.CancelledBy);

            lblCancellationReason.Text =
                string.IsNullOrWhiteSpace(
                    invoice.CancellationReason)
                    ? "-"
                    : Server.HtmlEncode(
                        invoice.CancellationReason);
        }

        private void LoadInvoiceItems(
    PurchaseInvoice invoice)
        {
            var invoiceItems =
                invoice.Items
                    .Where(item =>
                        !item.IsDeleted)
                    .Select(item => new
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
                                : Translate("ItemNotFound"),

                        Quantity =
                            item.Quantity,

                        UnitPrice =
                            item.UnitPrice,

                        LineTotal =
                            item.Quantity *
                            item.UnitPrice
                    })
                    .ToList();

            gvInvoiceItems.DataSource =
                invoiceItems;

            gvInvoiceItems.DataBind();

        }

        protected void btnPost_Click(
            object sender,
            EventArgs e)
        {
            if (!CanManageInvoice)
            {
                ShowError(Translate("NotAuthorized"));
                return;
            }

            int invoiceId;

            if (!TryGetInvoiceId(out invoiceId))
            {
                ShowError(
                    Translate("InvalidInvoiceId"));

                return;
            }

            try
            {
                _purchaseInvoiceService.Post(
    invoiceId,
    GetCurrentUserName());

                LoadInvoice();

                ShowSuccess(
                    Translate("PostSuccess"));
            }
            catch (Exception exception)
            {
                LoadInvoice();

                ShowError(
                    exception.Message);
            }
        }

        protected void btnMarkAsPaid_Click(
            object sender,
            EventArgs e)
        {
            if (!CanManageInvoice)
            {
                ShowError(Translate("NotAuthorized"));
                return;
            }

            int invoiceId;

            if (!TryGetInvoiceId(out invoiceId))
            {
                ShowError(
                    Translate("InvalidInvoiceId"));

                return;
            }

            try
            {
                _purchaseInvoiceService.MarkAsPaid(
                    invoiceId);

                LoadInvoice();

                ShowSuccess(
                    Translate("PaidSuccess"));
            }
            catch (Exception exception)
            {
                LoadInvoice();

                ShowError(
                    exception.Message);
            }
        }

        protected void btnShowCancelInvoice_Click(
            object sender,
            EventArgs e)
        {
            if (!CanManageInvoice)
            {
                ShowError(Translate("NotAuthorized"));
                return;
            }

            int invoiceId;

            if (!TryGetInvoiceId(out invoiceId))
            {
                ShowError(
                    Translate("InvalidInvoiceId"));

                return;
            }

            PurchaseInvoice invoice =
                _purchaseInvoiceService.GetById(
                    invoiceId);

            if (invoice == null ||
                invoice.IsDeleted)
            {
                ShowError(
                    Translate("InvoiceNotFound"));

                return;
            }

            if (invoice.Status ==
                PurchaseInvoiceStatus.Paid)
            {
                ShowError(
                    Translate("CannotCancelPaid"));

                pnlCancelInvoiceForm.Visible =
                    false;

                return;
            }

            if (invoice.Status ==
                PurchaseInvoiceStatus.Cancelled)
            {
                ShowError(
                    Translate("AlreadyCancelled"));

                pnlCancelInvoiceForm.Visible =
                    false;

                return;
            }

            if (invoice.Status !=
                    PurchaseInvoiceStatus.Draft
                &&
                invoice.Status !=
                    PurchaseInvoiceStatus.Posted)
            {
                ShowError(
                    Translate("CannotCancelStatus"));

                pnlCancelInvoiceForm.Visible =
                    false;

                return;
            }

            bool hasActivePayments = _unitOfWork.PurchasePayments.GetAll()
                .Any(payment => payment.PurchaseInvoiceId == invoiceId &&
                    !payment.IsDeleted && !payment.IsCancelled);

            if (hasActivePayments)
            {
                pnlCancelInvoiceForm.Visible = false;
                ShowError(Translate("ActivePaymentsPreventCancel"));
                return;
            }

            lblMessage.Visible = false;

            txtInvoiceCancellationReason.Text =
                string.Empty;

            pnlCancelInvoiceForm.Visible = true;
        }

        protected void btnCloseCancelInvoice_Click(
            object sender,
            EventArgs e)
        {
            txtInvoiceCancellationReason.Text =
                string.Empty;

            pnlCancelInvoiceForm.Visible = false;

            lblMessage.Visible = false;
        }

        protected void btnConfirmCancelInvoice_Click(
            object sender,
            EventArgs e)
        {
            if (!CanManageInvoice)
            {
                ShowError(Translate("NotAuthorized"));
                return;
            }

            Page.Validate(
                "CancelPurchaseInvoice");

            if (!Page.IsValid)
            {
                pnlCancelInvoiceForm.Visible =
                    true;

                return;
            }

            int invoiceId;

            if (!TryGetInvoiceId(out invoiceId))
            {
                ShowError(
                    Translate("InvalidInvoiceId"));

                pnlCancelInvoiceForm.Visible =
                    true;

                return;
            }

            string cancellationReason =
                txtInvoiceCancellationReason.Text
                    .Trim();

            if (string.IsNullOrWhiteSpace(
                cancellationReason))
            {
                ShowError(
                    Translate("ReasonRequired"));

                pnlCancelInvoiceForm.Visible =
                    true;

                return;
            }

            if (cancellationReason.Length > 500)
            {
                ShowError(
                    Translate("ReasonTooLong"));

                pnlCancelInvoiceForm.Visible =
                    true;

                return;
            }



            try
            {
                string cancelledBy = GetCurrentUserName();

                _purchaseInvoiceService.Cancel(
                    invoiceId,
                    cancellationReason,
                    cancelledBy);

                txtInvoiceCancellationReason.Text =
                    string.Empty;

                pnlCancelInvoiceForm.Visible = false;

                LoadInvoice();

                ShowSuccess(
                    Translate("CancelSuccess"));
            }

            catch (Exception exception)
            {
                LoadInvoice();

                PurchaseInvoice invoice =
                    _purchaseInvoiceService.GetById(
                        invoiceId);

                pnlCancelInvoiceForm.Visible =
                    invoice != null &&
                    (invoice.Status ==
                        PurchaseInvoiceStatus.Draft
                     ||
                     invoice.Status ==
                        PurchaseInvoiceStatus.Posted);

                ShowError(
                    exception.Message);
            }
        }

        private bool TryGetInvoiceId(
            out int invoiceId)
        {
            return int.TryParse(
                Request.QueryString["id"],
                out invoiceId) && invoiceId > 0;
        }

        private string GetCurrentUserName()
        {
            if (!Request.IsAuthenticated ||
                !(User.IsInRole("Admin") ||
                  User.IsInRole("Accountant")))
            {
                throw new System.Web.HttpException(
                    403,
                    Translate("NotAuthorized"));
            }

            string userName = User.Identity.Name;

            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new InvalidOperationException(
                    Translate("UserNotFound"));
            }

            userName = userName.Trim();

            if (userName.Length > 100)
            {
                throw new InvalidOperationException(
                    Translate("UserNameTooLong"));
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