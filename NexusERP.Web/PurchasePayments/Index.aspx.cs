using System;
using System.Linq;
using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Enums;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;

namespace NexusERP.Web.PurchasePayments
{
    public partial class Index : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IPurchasePaymentService _purchasePaymentService;

        protected void Page_Init(object sender, EventArgs e)
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);
            _purchasePaymentService =
                new PurchasePaymentService(_unitOfWork);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPayments();
            }
        }

        protected string Translate(string key)
        {
            return Convert.ToString(
                GetGlobalResourceObject("PurchasePayments", key));
        }

        private void LoadPayments()
        {
            pnlMessage.Visible = false;

            try
            {
                var payments = _purchasePaymentService
                    .GetAll()
                    .Select(payment => new
                    {
                        payment.Id,
                        payment.PaymentNumber,
                        payment.PaymentDate,
                        payment.Amount,
                        payment.IsCancelled,

                        InvoiceNumber =
                            payment.PurchaseInvoice != null
                                ? payment.PurchaseInvoice.InvoiceNumber
                                : "-",

                        SupplierName =
                            payment.PurchaseInvoice != null &&
                            payment.PurchaseInvoice.PurchaseOrder != null &&
                            payment.PurchaseInvoice.PurchaseOrder.Supplier != null
                                ? payment.PurchaseInvoice.PurchaseOrder.Supplier.NameAr
                                : "-",

                        PaymentMethodName =
                            GetPaymentMethodName(payment.PaymentMethod)
                    })
                    .ToList();

                gvPurchasePayments.DataSource = payments;
                gvPurchasePayments.DataBind();
            }
            catch (Exception exception)
            {
                ShowError(Translate("LoadError") + exception.Message);
            }
        }

        private string GetPaymentMethodName(PaymentMethod paymentMethod)
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
