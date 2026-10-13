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

namespace NexusERP.Web.PurchasePayments
{
    public partial class PaymentReport : System.Web.UI.Page
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
            crvPurchasePayments.Visible = false;

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
            int paymentMethod;
            int status;

            if (!int.TryParse(
                    ddlSuppliers.SelectedValue,
                    out supplierId) ||
                !int.TryParse(
                    ddlPaymentMethods.SelectedValue,
                    out paymentMethod) ||
                !int.TryParse(
                    ddlStatuses.SelectedValue,
                    out status) ||
                supplierId < 0 ||
                paymentMethod < 0 || paymentMethod > 4 ||
                status < 0 || status > 2)
            {
                ShowMessage("اختيارات التصفية غير صحيحة.");
                return;
            }

            DateTime endExclusive = toDate.Date.AddDays(1);

            // الخدمة تستبعد الدفعات المحذوفة،
            // وتحتفظ بالدفعات الملغاة لعرض سجلها.
            var payments = _paymentService.GetAll()
                .Where(p =>
                    p.PaymentDate >= fromDate.Date &&
                    p.PaymentDate < endExclusive);

            if (paymentMethod != 0)
            {
                payments = payments.Where(p =>
                    (int)p.PaymentMethod == paymentMethod);
            }

            if (status == 1)
            {
                payments = payments.Where(p => !p.IsCancelled);
            }
            else if (status == 2)
            {
                payments = payments.Where(p => p.IsCancelled);
            }

            var table = CreateReportTable();

            foreach (var payment in payments
                .OrderBy(p => p.PaymentDate)
                .ThenBy(p => p.Id)
                .ToList())
            {
                var invoice = payment.PurchaseInvoice ??
                    _unitOfWork.PurchaseInvoices.GetById(
                        payment.PurchaseInvoiceId);

                if (invoice == null)
                {
                    throw new InvalidOperationException(
                        "تعذر العثور على فاتورة الدفعة " +
                        payment.PaymentNumber);
                }

                if (supplierId != 0 &&
                    invoice.SupplierId != supplierId)
                {
                    continue;
                }

                var supplier = invoice.Supplier ??
                    _unitOfWork.Suppliers.GetById(
                        invoice.SupplierId);

                if (supplier == null)
                {
                    throw new InvalidOperationException(
                        "تعذر العثور على مورد الدفعة " +
                        payment.PaymentNumber);
                }

                DataRow row = table.NewRow();

                row["PaymentNumber"] = payment.PaymentNumber;
                row["PaymentDate"] = payment.PaymentDate;
                row["InvoiceNumber"] = invoice.InvoiceNumber;
                row["SupplierName"] = supplier.NameAr;
                row["Amount"] = payment.Amount;

                row["PaymentMethodName"] =
                    GetPaymentMethodName(payment.PaymentMethod);

                row["ReferenceNumber"] =
                    payment.ReferenceNumber ?? "";

                row["StatusName"] =
                    payment.IsCancelled ? "ملغاة" : "فعالة";

                row["Notes"] = payment.Notes ?? "";

                row["CancelledAt"] = payment.CancelledAt.HasValue
                    ? (object)payment.CancelledAt.Value
                    : DBNull.Value;

                row["CancelledBy"] = payment.CancelledBy ?? "";

                row["CancellationReason"] =
                    payment.CancellationReason ?? "";

                table.Rows.Add(row);
            }

            if (table.Rows.Count == 0)
            {
                ShowMessage(
                    "لا توجد دفعات مطابقة للاختيارات المحددة.");
                return;
            }

            _report =
                new NexusERP.Reporting.PurchasePaymentReport();

            _report.SetDataSource(table);

            crvPurchasePayments.ReportSource = _report;
            crvPurchasePayments.Visible = true;
        }

        private DataTable CreateReportTable()
        {
            var table = new DataTable("PurchasePayments");

            table.Columns.Add("PaymentNumber", typeof(string));
            table.Columns.Add("PaymentDate", typeof(DateTime));
            table.Columns.Add("InvoiceNumber", typeof(string));
            table.Columns.Add("SupplierName", typeof(string));
            table.Columns.Add("Amount", typeof(decimal));
            table.Columns.Add("PaymentMethodName", typeof(string));
            table.Columns.Add("ReferenceNumber", typeof(string));
            table.Columns.Add("StatusName", typeof(string));
            table.Columns.Add("Notes", typeof(string));
            table.Columns.Add("CancelledAt", typeof(DateTime));
            table.Columns.Add("CancelledBy", typeof(string));
            table.Columns.Add("CancellationReason", typeof(string));

            return table;
        }

        private string GetPaymentMethodName(PaymentMethod method)
        {
            switch (method)
            {
                case PaymentMethod.Cash:
                    return "نقدًا";

                case PaymentMethod.BankTransfer:
                    return "تحويل بنكي";

                case PaymentMethod.Cheque:
                    return "شيك";

                case PaymentMethod.CreditCard:
                    return "بطاقة ائتمان";

                default:
                    return method.ToString();
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