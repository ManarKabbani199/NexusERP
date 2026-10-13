using CrystalDecisions.CrystalReports.Engine;
using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Enums;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;

namespace NexusERP.Web.PurchaseInvoices
{
    public partial class InvoiceReport : System.Web.UI.Page
    {
        private IUnitOfWork _unitOfWork;
        private PurchasePaymentService _paymentService;
        private ReportDocument _report;

        protected void Page_Init(object sender, EventArgs e)
        {
            var context = new ApplicationDbContext();

            _unitOfWork = new UnitOfWork(context);
            _paymentService =
                new PurchasePaymentService(_unitOfWork);

            LoadSuppliers();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFromDate.Text =
                    new DateTime(
                        DateTime.Today.Year,
                        DateTime.Today.Month,
                        1)
                    .ToString(
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture);

                txtToDate.Text = DateTime.Today.ToString(
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture);
            }

            LoadReport();
        }

        private void LoadSuppliers()
        {
            ddlSuppliers.DataSource =
                _unitOfWork.Suppliers
                    .GetAll()
                    .Where(s => !s.IsDeleted)
                    .OrderBy(s => s.NameAr)
                    .ToList();

            ddlSuppliers.DataTextField = "NameAr";
            ddlSuppliers.DataValueField = "Id";
            ddlSuppliers.DataBind();

            ddlSuppliers.Items.Insert(
                0,
                new ListItem("جميع الموردين", "0"));
        }

        private void LoadReport()
        {
            lblMessage.Visible = false;
            crvPurchaseInvoices.Visible = false;

            DateTime fromDate;
            DateTime toDate;

            bool validFrom = DateTime.TryParseExact(
                txtFromDate.Text,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out fromDate);

            bool validTo = DateTime.TryParseExact(
                txtToDate.Text,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out toDate);

            if (!validFrom || !validTo ||
                fromDate > toDate ||
                toDate.Date == DateTime.MaxValue.Date)
            {
                ShowMessage("اختاري فترة زمنية صحيحة.");
                return;
            }

            int supplierId;
            int status;

            if (!int.TryParse(
                    ddlSuppliers.SelectedValue,
                    out supplierId) ||
                !int.TryParse(
                    ddlStatuses.SelectedValue,
                    out status) ||
                supplierId < 0 ||
                status < 0 || status > 4)
            {
                ShowMessage("اختيارات التصفية غير صحيحة.");
                return;
            }

            DateTime endExclusive = toDate.Date.AddDays(1);

            var invoices = _unitOfWork.PurchaseInvoices
                .GetAll()
                .Where(invoice =>
                    !invoice.IsDeleted &&
                    invoice.InvoiceDate >= fromDate.Date &&
                    invoice.InvoiceDate < endExclusive);

            if (supplierId != 0)
            {
                invoices = invoices.Where(invoice =>
                    invoice.SupplierId == supplierId);
            }

            if (status != 0)
            {
                invoices = invoices.Where(invoice =>
                    (int)invoice.Status == status);
            }

            DataTable table = CreateReportTable();

            foreach (var invoice in invoices
                .OrderBy(i => i.InvoiceDate)
                .ThenBy(i => i.Id)
                .ToList())
            {
                var supplier = invoice.Supplier ??
                    _unitOfWork.Suppliers.GetById(
                        invoice.SupplierId);

                if (supplier == null)
                {
                    throw new InvalidOperationException(
                        "تعذر العثور على مورد الفاتورة " +
                        invoice.InvoiceNumber);
                }

                var activeItems = invoice.Items
                    .Where(item => !item.IsDeleted)
                    .ToList();

                decimal subtotal = activeItems.Sum(item =>
                    item.Quantity * item.UnitPrice);

                decimal discount = invoice.DiscountAmount;

                if (discount < 0)
                    discount = 0;

                if (discount > subtotal)
                    discount = subtotal;

                // مطابق لحساب الضريبة في صفحة التفاصيل.
                decimal tax = activeItems.Sum(item =>
                    item.Quantity *
                    item.UnitPrice *
                    item.TaxRate / 100m);

                decimal total = subtotal - discount + tax;

                decimal paid =
                    _paymentService.GetPaidAmount(invoice.Id);

                decimal remaining = Math.Max(0m, total - paid);

                DataRow row = table.NewRow();

                row["InvoiceNumber"] = invoice.InvoiceNumber;
                row["InvoiceDate"] = invoice.InvoiceDate;
                row["DueDate"] = invoice.DueDate.HasValue
                    ? (object)invoice.DueDate.Value
                    : DBNull.Value;

                row["SupplierName"] = supplier.NameAr;
                row["StatusName"] = GetStatusName(invoice.Status);
                row["SubTotal"] = subtotal;
                row["DiscountAmount"] = discount;
                row["TaxAmount"] = tax;
                row["TotalAmount"] = total;
                row["PaidAmount"] = paid;
                row["RemainingAmount"] = remaining;

                row["PostedAt"] = invoice.PostedAt.HasValue
                    ? (object)invoice.PostedAt.Value
                    : DBNull.Value;

                row["PostedBy"] = invoice.PostedBy ?? "";

                row["CancelledAt"] = invoice.CancelledAt.HasValue
                    ? (object)invoice.CancelledAt.Value
                    : DBNull.Value;

                row["CancelledBy"] = invoice.CancelledBy ?? "";
                row["CancellationReason"] =
                    invoice.CancellationReason ?? "";

                row["Notes"] = invoice.Notes ?? "";

                table.Rows.Add(row);
            }

            if (table.Rows.Count == 0)
            {
                ShowMessage(
                    "لا توجد فواتير مطابقة للاختيارات المحددة.");
                return;
            }

            _report =
                new NexusERP.Reporting.PurchaseInvoiceReport();

            _report.SetDataSource(table);

            crvPurchaseInvoices.ReportSource = _report;
            crvPurchaseInvoices.Visible = true;
        }

        private DataTable CreateReportTable()
        {
            var table = new DataTable("PurchaseInvoices");

            table.Columns.Add("InvoiceNumber", typeof(string));
            table.Columns.Add("InvoiceDate", typeof(DateTime));
            table.Columns.Add("DueDate", typeof(DateTime));
            table.Columns.Add("SupplierName", typeof(string));
            table.Columns.Add("StatusName", typeof(string));
            table.Columns.Add("SubTotal", typeof(decimal));
            table.Columns.Add("DiscountAmount", typeof(decimal));
            table.Columns.Add("TaxAmount", typeof(decimal));
            table.Columns.Add("TotalAmount", typeof(decimal));
            table.Columns.Add("PaidAmount", typeof(decimal));
            table.Columns.Add("RemainingAmount", typeof(decimal));
            table.Columns.Add("PostedAt", typeof(DateTime));
            table.Columns.Add("PostedBy", typeof(string));
            table.Columns.Add("CancelledAt", typeof(DateTime));
            table.Columns.Add("CancelledBy", typeof(string));
            table.Columns.Add("CancellationReason", typeof(string));
            table.Columns.Add("Notes", typeof(string));

            return table;
        }

        private string GetStatusName(PurchaseInvoiceStatus status)
        {
            switch (status)
            {
                case PurchaseInvoiceStatus.Draft:
                    return "مسودة";

                case PurchaseInvoiceStatus.Posted:
                    return "مُرحّلة";

                case PurchaseInvoiceStatus.Paid:
                    return "مدفوعة";

                case PurchaseInvoiceStatus.Cancelled:
                    return "ملغاة";

                default:
                    return status.ToString();
            }
        }

        private void ShowMessage(string message)
        {
            lblMessage.Text = message;
            lblMessage.Visible = true;
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            try
            {
                if (_report != null)
                {
                    try
                    {
                        _report.Close();
                    }
                    finally
                    {
                        _report.Dispose();
                    }
                }
            }
            finally
            {
                if (_unitOfWork != null)
                    _unitOfWork.Dispose();
            }
        }
    }
}