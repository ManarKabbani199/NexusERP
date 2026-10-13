using System;
using System.Globalization;
using System.Linq;
using DevExpress.Utils;
using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Enums;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;

namespace NexusERP.Web.PurchaseInvoices
{
    public partial class Index : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IPurchaseInvoiceService _purchaseInvoiceService;

        private bool IsArabic
        {
            get
            {
                return CultureInfo.CurrentUICulture
                    .TwoLetterISOLanguageName == "ar";
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);

            _purchaseInvoiceService =
                new PurchaseInvoiceService(_unitOfWork);

            // اتجاه الجدول بحسب لغة الواجهة.
            gvPurchaseInvoices.RightToLeft =
                IsArabic
                    ? DefaultBoolean.True
                    : DefaultBoolean.False;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // ربط البيانات في كل طلب، بما فيه callbacks.
            LoadInvoices();
        }

        private string Translate(string key)
        {
            return Convert.ToString(
                GetGlobalResourceObject(
                    "PurchaseInvoices",
                    key));
        }

        private void LoadInvoices()
        {
            string searchText = txtSearch.Text.Trim();

            int statusValue;

            bool hasStatusFilter = int.TryParse(
                ddlStatus.SelectedValue,
                out statusValue);

            var invoices = _purchaseInvoiceService
                .GetAll()
                .Where(invoice => !invoice.IsDeleted)
                .ToList();

            if (hasStatusFilter)
            {
                PurchaseInvoiceStatus selectedStatus =
                    (PurchaseInvoiceStatus)statusValue;

                invoices = invoices
                    .Where(invoice =>
                        invoice.Status == selectedStatus)
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                invoices = invoices
                    .Where(invoice =>
                        ContainsSearchText(
                            invoice.InvoiceNumber,
                            searchText) ||
                        ContainsSearchText(
                            invoice.Supplier != null
                                ? invoice.Supplier.NameAr
                                : null,
                            searchText) ||
                        ContainsSearchText(
                            invoice.PurchaseOrder != null
                                ? invoice.PurchaseOrder.OrderNumber
                                : null,
                            searchText))
                    .ToList();
            }

            var rows = invoices
                .OrderByDescending(invoice =>
                    invoice.InvoiceDate)
                .Select(invoice =>
                {
                    var activeItems = invoice.Items
                        .Where(item => !item.IsDeleted)
                        .ToList();

                    decimal subtotal = activeItems.Sum(item =>
                        item.Quantity * item.UnitPrice);

                    decimal taxAmount = activeItems.Sum(item =>
                        item.Quantity *
                        item.UnitPrice *
                        item.TaxRate / 100m);

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

                    return new
                    {
                        invoice.Id,
                        invoice.InvoiceNumber,
                        invoice.InvoiceDate,
                        invoice.DueDate,

                        PurchaseOrderNumber =
                            invoice.PurchaseOrder != null
                                ? invoice.PurchaseOrder.OrderNumber
                                : "-",

                        SupplierName =
                            invoice.Supplier != null
                                ? invoice.Supplier.NameAr
                                : "-",

                        GrandTotal =
                            subtotal - discountAmount + taxAmount,

                        StatusName =
                            GetStatusName(invoice.Status),

                        StatusCssClass =
                            GetStatusCssClass(invoice.Status)
                    };
                })
                .ToList();

            gvPurchaseInvoices.DataSource = rows;
            gvPurchaseInvoices.DataBind();
        }

        private bool ContainsSearchText(
            string value,
            string searchText)
        {
            return !string.IsNullOrEmpty(value) &&
                   value.IndexOf(
                       searchText,
                       StringComparison.OrdinalIgnoreCase) >= 0;
        }

        protected void btnSearch_Click(
            object sender,
            EventArgs e)
        {
            gvPurchaseInvoices.PageIndex = 0;

            LoadInvoices();
        }

        protected void btnReset_Click(
            object sender,
            EventArgs e)
        {
            txtSearch.Text = string.Empty;
            ddlStatus.SelectedValue = string.Empty;

            gvPurchaseInvoices.FilterExpression = string.Empty;
            gvPurchaseInvoices.SearchPanelFilter = string.Empty;

            gvPurchaseInvoices.ClearSort();
            gvPurchaseInvoices.PageIndex = 0;

            LoadInvoices();
        }

        private string GetStatusName(
            PurchaseInvoiceStatus status)
        {
            switch (status)
            {
                case PurchaseInvoiceStatus.Draft:
                    return Translate("Draft");

                case PurchaseInvoiceStatus.Posted:
                    return Translate("Posted");

                case PurchaseInvoiceStatus.Paid:
                    return Translate("Paid");

                case PurchaseInvoiceStatus.Cancelled:
                    return Translate("Cancelled");

                default:
                    return Translate("Unknown");
            }
        }

        private string GetStatusCssClass(
            PurchaseInvoiceStatus status)
        {
            switch (status)
            {
                case PurchaseInvoiceStatus.Draft:
                    return "badge bg-secondary";

                case PurchaseInvoiceStatus.Posted:
                    return "badge bg-primary";

                case PurchaseInvoiceStatus.Paid:
                    return "badge bg-success";

                case PurchaseInvoiceStatus.Cancelled:
                    return "badge bg-danger";

                default:
                    return "badge bg-dark";
            }
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