using System;
using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Entities;
using NexusERP.Domain.Enums;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;

namespace NexusERP.Web.PurchasePayments
{
    public partial class Details :
        System.Web.UI.Page
    {
        private ApplicationDbContext _context;

        private IUnitOfWork _unitOfWork;

        private IPurchasePaymentService
            _purchasePaymentService;

        private string Translate(string key)
        {
            return Convert.ToString(GetGlobalResourceObject(
                "PurchasePaymentDetails", key));
        }

        private bool CanCancelPayments
        {
            get
            {
                return Request.IsAuthenticated &&
                    (User.IsInRole("Admin") || User.IsInRole("Accountant"));
            }
        }

        private bool TryGetPaymentId(out int paymentId)
        {
            return int.TryParse(Request.QueryString["id"], out paymentId)
                && paymentId > 0;
        }

        protected void Page_Init(
            object sender,
            EventArgs e)
        {
            btnConfirmCancel.OnClientClick = "return confirm(" +
                System.Web.HttpUtility.JavaScriptStringEncode(
                    Translate("ConfirmCancel"), true) + ");";

            _context =
                new ApplicationDbContext();

            _unitOfWork =
                new UnitOfWork(_context);

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
                LoadPayment();

                if (pnlDetails.Visible && Request.QueryString["created"] ==
                    "1")
                {
                    ShowSuccess(
                        Translate("CreatedSuccess"));
                }

                if (pnlDetails.Visible && pnlCancellationDetails.Visible &&
                    Request.QueryString["cancelled"] ==
                    "1")
                {
                    ShowSuccess(
                        Translate("CancelledSuccess"));
                }
            }
        }

        private void LoadPayment()
        {
            int paymentId;

            if (!TryGetPaymentId(out paymentId))
            {
                ShowError(
                    Translate("InvalidId"));

                return;
            }

            PurchasePayment payment =
                _purchasePaymentService.GetById(
                    paymentId);

            if (payment == null || payment.IsDeleted)
            {
                ShowError(
                    Translate("NotFound"));

                return;
            }

            pnlDetails.Visible = true;
            pnlCancelForm.Visible = false;

            lblPaymentNumber.Text =
                Server.HtmlEncode(
                    payment.PaymentNumber);

            lblPaymentDate.Text =
                payment.PaymentDate.ToString(
                    "yyyy-MM-dd");

            lblAmount.Text =
                payment.Amount.ToString("N2");

            lblPaymentMethod.Text =
                GetPaymentMethodName(
                    payment.PaymentMethod);

            lblReferenceNumber.Text =
                string.IsNullOrWhiteSpace(
                    payment.ReferenceNumber)
                    ? "-"
                    : Server.HtmlEncode(
                        payment.ReferenceNumber);

            lblNotes.Text =
                string.IsNullOrWhiteSpace(
                    payment.Notes)
                    ? Translate("NoNotes")
                    : Server.HtmlEncode(
                        payment.Notes);

            lblCreatedBy.Text =
                string.IsNullOrWhiteSpace(
                    payment.CreatedBy)
                    ? "-"
                    : Server.HtmlEncode(
                        payment.CreatedBy);

            if (payment.PurchaseInvoice != null)
            {
                lnkInvoice.Text =
                    Server.HtmlEncode(
                        payment.PurchaseInvoice
                            .InvoiceNumber);

                lnkInvoice.NavigateUrl =
                    "~/PurchaseInvoices/Details.aspx?id=" +
                    payment.PurchaseInvoiceId;

                if (payment.PurchaseInvoice
                        .PurchaseOrder != null &&
                    payment.PurchaseInvoice
                        .PurchaseOrder.Supplier != null)
                {
                    lblSupplier.Text =
                        Server.HtmlEncode(
                            payment.PurchaseInvoice
                                .PurchaseOrder.Supplier
                                .NameAr);
                }
                else
                {
                    lblSupplier.Text = "-";
                }
            }
            else
            {
                lnkInvoice.Text =
                    payment.PurchaseInvoiceId.ToString();

                lnkInvoice.NavigateUrl =
                    "~/PurchaseInvoices/Details.aspx?id=" +
                    payment.PurchaseInvoiceId;

                lblSupplier.Text = "-";
            }

            if (payment.IsCancelled)
            {
                lblStatus.Text = Translate("Cancelled");

                lblStatus.CssClass =
                    "badge bg-danger";

                btnShowCancel.Visible = false;

                pnlCancellationDetails.Visible =
                    true;

                lblCancelledAt.Text =
                    payment.CancelledAt.HasValue
                        ? payment.CancelledAt.Value
                            .ToString(
                                "yyyy-MM-dd HH:mm")
                        : "-";

                lblCancelledBy.Text =
                    string.IsNullOrWhiteSpace(
                        payment.CancelledBy)
                        ? "-"
                        : Server.HtmlEncode(
                            payment.CancelledBy);

                lblCancellationReason.Text =
                    string.IsNullOrWhiteSpace(
                        payment.CancellationReason)
                        ? "-"
                        : Server.HtmlEncode(
                            payment.CancellationReason);
            }
            else
            {
                lblStatus.Text = Translate("Active");

                lblStatus.CssClass =
                    "badge bg-success";

                btnShowCancel.Visible = CanCancelPayments;

                pnlCancellationDetails.Visible =
                    false;
            }
        }

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

        protected void btnShowCancel_Click(
    object sender,
    EventArgs e)
        {
            try
            {
                GetAuthorizedUserName();

                LoadPayment();

                if (!pnlDetails.Visible)
                {
                    return;
                }

                if (pnlCancellationDetails.Visible)
                {
                    ShowErrorMessage(Translate("AlreadyCancelled"));
                    return;
                }

                lblMessage.Visible = false;
                txtCancellationReason.Text = string.Empty;
                pnlCancelForm.Visible = true;
            }
            catch (Exception exception)
            {
                pnlCancelForm.Visible = false;
                ShowErrorMessage(exception.Message);
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
            Page.Validate("CancelPayment");

            if (!Page.IsValid)
            {
                pnlCancelForm.Visible = true;

                return;
            }

            int paymentId;

            if (!TryGetPaymentId(out paymentId))
            {
                ShowError(
                    Translate("InvalidId"));

                return;
            }

            string cancellationReason =
                txtCancellationReason.Text.Trim();

            if (string.IsNullOrWhiteSpace(
                cancellationReason))
            {
                ShowErrorMessage(
                    Translate("ReasonRequired"));

                pnlCancelForm.Visible = true;

                return;
            }

            if (cancellationReason.Length > 500)
            {
                ShowErrorMessage(Translate("ReasonTooLong"));
                pnlCancelForm.Visible = true;
                return;
            }

            try
            {
                string cancelledBy = GetAuthorizedUserName();

                _purchasePaymentService.Cancel(
                    paymentId,
                    cancellationReason,
                    cancelledBy);

                Response.Redirect(
                    "Details.aspx?id=" +
                    paymentId +
                    "&cancelled=1",
                    false);

                Context.ApplicationInstance.CompleteRequest();
            }

            catch (Exception exception)
            {
                LoadPayment();

                pnlCancelForm.Visible = pnlDetails.Visible &&
                    CanCancelPayments && !pnlCancellationDetails.Visible;

                ShowErrorMessage(
                    exception.Message);
            }
        }

        private void ShowError(string message)
        {
            pnlDetails.Visible = false;

            ShowErrorMessage(message);
        }

        private void ShowErrorMessage(
            string message)
        {
            lblMessage.Text =
                Server.HtmlEncode(message);

            lblMessage.CssClass =
                "alert alert-danger d-block";

            lblMessage.Visible = true;
        }

        private void ShowSuccess(string message)
        {
            lblMessage.Text =
                Server.HtmlEncode(message);

            lblMessage.CssClass =
                "alert alert-success d-block";

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

        private string GetAuthorizedUserName()
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

            if (userName.Length > 256)
            {
                throw new InvalidOperationException(
                    Translate("UserNameTooLong"));
            }

            return userName;
        }
    }
}