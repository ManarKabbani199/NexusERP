using System;
using System.Globalization;
using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Entities;
using NexusERP.Domain.Enums;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;

namespace NexusERP.Web.PurchasePayments
{
    public partial class Create : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IPurchasePaymentService _purchasePaymentService;
        private decimal _remainingAmount;

        private bool CanCreatePayments
        {
            get
            {
                return Request.IsAuthenticated &&
                    (User.IsInRole("Admin") || User.IsInRole("Accountant"));
            }
        }

        private string Translate(string key)
        {
            return Convert.ToString(GetGlobalResourceObject(
                "PurchasePaymentCreate", key));
        }

        private bool TryGetInvoiceId(out int invoiceId)
        {
            return int.TryParse(Request.QueryString["invoiceId"], out invoiceId)
                && invoiceId > 0;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);
            _purchasePaymentService = new PurchasePaymentService(_unitOfWork);

            btnSave.OnClientClick = "return confirm(" +
                System.Web.HttpUtility.JavaScriptStringEncode(
                    Translate("ConfirmSave"), true) + ");";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CanCreatePayments)
            {
                pnlForm.Visible = false;
                ShowError(Translate("NotAuthorized"));
                return;
            }

            if (!IsPostBack)
            {
                txtPaymentDate.Text = DateTime.Today.ToString(
                    "yyyy-MM-dd", CultureInfo.InvariantCulture);

                LoadInvoice(true);
            }
        }

        private bool LoadInvoice(bool initializeAmount)
        {
            pnlForm.Visible = false;

            int invoiceId;
            if (!TryGetInvoiceId(out invoiceId))
            {
                ShowError(Translate("InvalidInvoiceId"));
                return false;
            }

            PurchaseInvoice invoice = _unitOfWork.PurchaseInvoices.GetById(invoiceId);
            if (invoice == null || invoice.IsDeleted)
            {
                ShowError(Translate("InvoiceNotFound"));
                return false;
            }

            lnkBack.NavigateUrl = "~/PurchaseInvoices/Details.aspx?id=" + invoice.Id;
            lnkCancel.NavigateUrl = lnkBack.NavigateUrl;

            if (invoice.Status == PurchaseInvoiceStatus.Draft)
            {
                ShowError(Translate("PostFirst"));
                return false;
            }
            if (invoice.Status == PurchaseInvoiceStatus.Cancelled)
            {
                ShowError(Translate("CancelledInvoice"));
                return false;
            }
            if (invoice.Status == PurchaseInvoiceStatus.Paid)
            {
                ShowError(Translate("FullyPaid"));
                return false;
            }
            if (invoice.Status != PurchaseInvoiceStatus.Posted)
            {
                ShowError(Translate("InvalidStatus"));
                return false;
            }

            decimal invoiceTotal = _purchasePaymentService.GetInvoiceTotal(invoice.Id);
            decimal paidAmount = _purchasePaymentService.GetPaidAmount(invoice.Id);
            _remainingAmount = _purchasePaymentService.GetRemainingAmount(invoice.Id);

            if (_remainingAmount <= 0)
            {
                ShowError(Translate("FullyPaid"));
                return false;
            }

            lblInvoiceNumber.Text = Server.HtmlEncode(invoice.InvoiceNumber);
            lblSupplier.Text = invoice.PurchaseOrder != null &&
                invoice.PurchaseOrder.Supplier != null
                    ? Server.HtmlEncode(invoice.PurchaseOrder.Supplier.NameAr)
                    : "-";
            lblInvoiceTotal.Text = invoiceTotal.ToString("N2");
            lblPaidAmount.Text = paidAmount.ToString("N2");
            lblRemainingAmount.Text = _remainingAmount.ToString("N2");

            // HTML number fields use a dot as the decimal separator.
            if (initializeAmount && string.IsNullOrWhiteSpace(txtAmount.Text))
            {
                txtAmount.Text = _remainingAmount.ToString(
                    "0.00", CultureInfo.InvariantCulture);
            }

            pnlForm.Visible = true;
            return true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
            if (!CanCreatePayments)
            {
                pnlForm.Visible = false;
                ShowError(Translate("NotAuthorized"));
                return;
            }

            // Refresh eligibility and balance without overwriting entered values.
            if (!LoadInvoice(false))
            {
                return;
            }

            Page.Validate("PaymentForm");
            if (!Page.IsValid)
            {
                return;
            }

            int invoiceId;
            if (!TryGetInvoiceId(out invoiceId))
            {
                ShowError(Translate("InvalidInvoiceId"));
                return;
            }

            string paymentNumber = txtPaymentNumber.Text.Trim();
            if (string.IsNullOrWhiteSpace(paymentNumber))
            {
                ShowError(Translate("PaymentNumberRequired"));
                return;
            }
            if (paymentNumber.Length > 50)
            {
                ShowError(Translate("PaymentNumberTooLong"));
                return;
            }

            decimal amount;
            if (!TryParseAmount(txtAmount.Text.Trim(), out amount))
            {
                ShowError(Translate("InvalidAmount"));
                return;
            }
            if (amount <= 0)
            {
                ShowError(Translate("PositiveAmount"));
                return;
            }
            if (amount > _remainingAmount)
            {
                ShowError(Translate("ExceedsRemaining"));
                return;
            }

            DateTime paymentDate;
            if (!DateTime.TryParseExact(txtPaymentDate.Text.Trim(),
                "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out paymentDate))
            {
                ShowError(Translate("InvalidDate"));
                return;
            }

            int paymentMethodValue;
            if (!int.TryParse(ddlPaymentMethod.SelectedValue, out paymentMethodValue)
                || !Enum.IsDefined(typeof(PaymentMethod), paymentMethodValue))
            {
                ShowError(Translate("InvalidMethod"));
                return;
            }

            string createdBy = User.Identity.Name;
            if (string.IsNullOrWhiteSpace(createdBy))
            {
                ShowError(Translate("UserNotFound"));
                return;
            }
            createdBy = createdBy.Trim();
            if (createdBy.Length > 256)
            {
                ShowError(Translate("UserNameTooLong"));
                return;
            }

            string referenceNumber = txtReferenceNumber.Text.Trim();
            string notes = txtNotes.Text.Trim();
            if (referenceNumber.Length > 100)
            {
                ShowError(Translate("ReferenceTooLong"));
                return;
            }
            if (notes.Length > 500)
            {
                ShowError(Translate("NotesTooLong"));
                return;
            }

            PurchasePayment payment = new PurchasePayment
            {
                PaymentNumber = paymentNumber,
                PaymentDate = paymentDate,
                PurchaseInvoiceId = invoiceId,
                Amount = amount,
                PaymentMethod = (PaymentMethod)paymentMethodValue,
                ReferenceNumber = string.IsNullOrWhiteSpace(referenceNumber)
                    ? null : referenceNumber,
                Notes = string.IsNullOrWhiteSpace(notes) ? null : notes,
                CreatedBy = createdBy
            };

            try
            {
                int paymentId = _purchasePaymentService.Create(payment);
                Response.Redirect("~/PurchasePayments/Details.aspx?id=" +
                    paymentId + "&created=1", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception exception)
            {
                bool canStillPay = LoadInvoice(false);
                if (canStillPay)
                {
                    ShowError(exception.Message);
                }
            }
        }

        private bool TryParseAmount(string text, out decimal value)
        {
            NumberStyles styles = NumberStyles.AllowDecimalPoint |
                NumberStyles.AllowLeadingSign;
            return decimal.TryParse(text, styles, CultureInfo.InvariantCulture, out value)
                || decimal.TryParse(text, styles, CultureInfo.CurrentCulture, out value);
        }

        private void ShowError(string message)
        {
            lblMessage.Text = Server.HtmlEncode(message);
            lblMessage.CssClass = "alert alert-danger d-block";
            lblMessage.Visible = true;
        }

        protected void Page_Unload(object sender, EventArgs e)
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
