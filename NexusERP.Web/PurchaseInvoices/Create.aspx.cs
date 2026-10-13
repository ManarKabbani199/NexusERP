using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Entities;
using NexusERP.Domain.Enums;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace NexusERP.Web.PurchaseInvoices
{
    public partial class Create : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;

        private IPurchaseInvoiceService
            _purchaseInvoiceService;

        private string Translate(string key)
        {
            return Convert.ToString(GetGlobalResourceObject(
                "PurchaseInvoiceCreate", key));
        }

        protected string NumberInputValue(object value)
        {
            return Convert.ToDecimal(value, CultureInfo.InvariantCulture)
                .ToString("0.00", CultureInfo.InvariantCulture);
        }

        protected void Page_Init(
            object sender,
            EventArgs e)
        {
            _context =
                new ApplicationDbContext();

            _unitOfWork =
                new UnitOfWork(_context);

            _purchaseInvoiceService =
                new PurchaseInvoiceService(
                    _unitOfWork);
        }

        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (!IsPostBack)
            {
                txtInvoiceDate.Text =
                    DateTime.Today.ToString(
                        "yyyy-MM-dd", CultureInfo.InvariantCulture);

                LoadPurchaseOrders();
                SelectPurchaseOrderFromQueryString();
            }
        }

        private void LoadPurchaseOrders()
        {
            var purchaseOrders =
                _unitOfWork.PurchaseOrders
                    .GetAll()
                    .Where(order =>
                        !order.IsDeleted &&
                        (order.Status ==
                            PurchaseOrderStatus
                                .PartiallyReceived ||
                         order.Status ==
                            PurchaseOrderStatus
                                .Received))
                    .OrderByDescending(order =>
                        order.OrderDate)
                    .Select(order => new
                    {
                        order.Id,

                        DisplayText =
                            order.OrderNumber +
                            " - " +
                            order.Supplier.NameAr
                    })
                    .ToList();

            ddlPurchaseOrder.DataSource =
                purchaseOrders;

            ddlPurchaseOrder.DataTextField =
                "DisplayText";

            ddlPurchaseOrder.DataValueField =
                "Id";

            ddlPurchaseOrder.DataBind();

            ddlPurchaseOrder.Items.Insert(
                0,
                new ListItem(
                    Translate("SelectPurchaseOrder"),
                    ""));
        }

        private void SelectPurchaseOrderFromQueryString()
        {
            int purchaseOrderId;

            if (!int.TryParse(
                Request.QueryString["purchaseOrderId"],
                out purchaseOrderId))
            {
                return;
            }

            ListItem selectedItem =
                ddlPurchaseOrder.Items.FindByValue(
                    purchaseOrderId.ToString());

            if (selectedItem == null)
            {
                return;
            }

            ddlPurchaseOrder.ClearSelection();

            selectedItem.Selected = true;

            LoadPurchaseOrderItems(
                purchaseOrderId);
        }

        protected void ddlPurchaseOrder_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            int purchaseOrderId;

            if (!int.TryParse(
                ddlPurchaseOrder.SelectedValue,
                out purchaseOrderId))
            {
                lblSupplier.Text = "-";
                pnlItems.Visible = false;

                gvInvoiceItems.DataSource = null;
                gvInvoiceItems.DataBind();

                return;
            }

            LoadPurchaseOrderItems(
                purchaseOrderId);
        }

        private void LoadPurchaseOrderItems(
            int purchaseOrderId)
        {
            PurchaseOrder purchaseOrder =
                _unitOfWork.PurchaseOrders.GetById(
                    purchaseOrderId);

            if (purchaseOrder == null ||
                purchaseOrder.IsDeleted)
            {
                ShowError(
                    Translate("PurchaseOrderNotFound"));

                pnlItems.Visible = false;

                return;
            }

            lblSupplier.Text =
                purchaseOrder.Supplier != null
                    ? Server.HtmlEncode(
                        purchaseOrder.Supplier.NameAr)
                    : "-";

            var invoiceItems =
                purchaseOrder.Items
                    .Where(orderItem =>
                        !orderItem.IsDeleted)
                    .Select(orderItem => new
                    {
                        PurchaseOrderItemId =
                            orderItem.Id,

                        ItemCode =
                            orderItem.Item != null
                                ? orderItem.Item.Code
                                : "-",

                        ItemName =
                            orderItem.Item != null
                                ? orderItem.Item.NameAr
                                : Translate("ItemNotFound"),

                        ReceivedQuantity =
                            _purchaseInvoiceService
                                .GetReceivedQuantity(
                                    orderItem.Id),

                        InvoicedQuantity =
                            _purchaseInvoiceService
                                .GetInvoicedQuantity(
                                    orderItem.Id),

                        AvailableQuantity =
                            _purchaseInvoiceService
                                .GetAvailableQuantity(
                                    orderItem.Id),

                        UnitPrice =
                            orderItem.UnitPrice
                    })
                    .Where(item =>
                        item.AvailableQuantity > 0)
                    .ToList();

            gvInvoiceItems.DataSource =
                invoiceItems;

            gvInvoiceItems.DataBind();

            pnlItems.Visible = true;
        }

        protected void btnSave_Click(
     object sender,
     EventArgs e)
        {
            if (!Request.IsAuthenticated ||
    !(User.IsInRole("Admin") ||
      User.IsInRole("Accountant")))
            {
                throw new HttpException(
                    403,
                    Translate("NotAuthorized"));
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

            Page.Validate("InvoiceForm");

            if (!Page.IsValid)
            {
                return;
            }

            /*
             * التحقق من رقم الفاتورة.
             */
            string invoiceNumber =
                txtInvoiceNumber.Text.Trim();

            if (string.IsNullOrWhiteSpace(
                invoiceNumber))
            {
                ShowError(
                    Translate("InvoiceNumberMissing"));

                return;
            }

            /*
             * التحقق من أمر الشراء.
             */
            int purchaseOrderId;

            if (!int.TryParse(
                ddlPurchaseOrder.SelectedValue,
                out purchaseOrderId))
            {
                ShowError(
                    Translate("PurchaseOrderRequired"));

                return;
            }

            PurchaseOrder purchaseOrder =
                _unitOfWork.PurchaseOrders.GetById(
                    purchaseOrderId);

            if (purchaseOrder == null ||
                purchaseOrder.IsDeleted)
            {
                ShowError(
                    Translate("PurchaseOrderNotFound"));

                return;
            }

            if (purchaseOrder.SupplierId <= 0)
            {
                ShowError(
                    Translate("InvalidSupplier"));

                return;
            }

            /*
             * التحقق من تاريخ الفاتورة.
             */
            DateTime invoiceDate;

            if (!DateTime.TryParseExact(
                txtInvoiceDate.Text.Trim(),
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out invoiceDate))
            {
                ShowError(
                    Translate("InvalidInvoiceDate"));

                return;
            }

            /*
             * التحقق من تاريخ الاستحقاق.
             */
            DateTime? dueDate = null;

            if (!string.IsNullOrWhiteSpace(
                txtDueDate.Text))
            {
                DateTime parsedDueDate;

                if (!DateTime.TryParseExact(
                    txtDueDate.Text.Trim(),
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out parsedDueDate))
                {
                    ShowError(
                        Translate("InvalidDueDate"));

                    return;
                }

                if (parsedDueDate.Date <
                    invoiceDate.Date)
                {
                    ShowError(
                        Translate("DueDateBeforeInvoice"));

                    return;
                }

                dueDate = parsedDueDate;
            }

            /*
             * قراءة مبلغ الخصم.
             */
            decimal discountAmount = 0;

            if (!string.IsNullOrWhiteSpace(
                txtDiscountAmount.Text))
            {
                if (!TryParseDecimal(
                    txtDiscountAmount.Text.Trim(),
                    out discountAmount))
                {
                    ShowError(
                        Translate("InvalidDiscount"));

                    return;
                }
            }

            if (discountAmount < 0)
            {
                ShowError(
                    Translate("NegativeDiscount"));

                return;
            }

            /*
             * إنشاء الفاتورة.
             */
            PurchaseInvoice invoice =
                new PurchaseInvoice
                {
                    CreatedBy = createdBy,

                    InvoiceNumber =
                        invoiceNumber,

                    InvoiceDate =
                        invoiceDate,

                    DueDate =
                        dueDate,

                    PurchaseOrderId =
                        purchaseOrder.Id,

                    SupplierId =
                        purchaseOrder.SupplierId,

                    DiscountAmount =
                        discountAmount,

                    Notes =
                        string.IsNullOrWhiteSpace(
                            txtNotes.Text)
                            ? null
                            : txtNotes.Text.Trim()
                };

            /*
             * قراءة بنود الفاتورة.
             */
            foreach (GridViewRow row
                in gvInvoiceItems.Rows)
            {
                int purchaseOrderItemId =
                    Convert.ToInt32(
                        gvInvoiceItems.DataKeys[
                            row.RowIndex].Value);

                TextBox quantityTextBox =
                    row.FindControl(
                        "txtQuantity") as TextBox;

                TextBox unitPriceTextBox =
                    row.FindControl(
                        "txtUnitPrice") as TextBox;

                TextBox taxRateTextBox =
                    row.FindControl(
                        "txtTaxRate") as TextBox;

                decimal quantity;
                decimal unitPrice;
                decimal taxRate;

                /*
                 * إذا لم تُدخل كمية، نتجاوز هذا الصنف.
                 */
                if (quantityTextBox == null ||
                    !TryParseDecimal(
                        quantityTextBox.Text.Trim(),
                        out quantity))
                {
                    quantity = 0;
                }

                if (quantity <= 0)
                {
                    continue;
                }

                /*
                 * التحقق من سعر الوحدة.
                 */
                if (unitPriceTextBox == null ||
                    !TryParseDecimal(
                        unitPriceTextBox.Text.Trim(),
                        out unitPrice))
                {
                    ShowError(
                        Translate("InvalidUnitPrice"));

                    return;
                }

                if (unitPrice < 0)
                {
                    ShowError(
                        Translate("NegativeUnitPrice"));

                    return;
                }

                /*
                 * التحقق من نسبة الضريبة.
                 */
                if (taxRateTextBox == null ||
                    !TryParseDecimal(
                        taxRateTextBox.Text.Trim(),
                        out taxRate))
                {
                    ShowError(
                        Translate("InvalidTax"));

                    return;
                }

                if (taxRate < 0 ||
                    taxRate > 100)
                {
                    ShowError(
                        Translate("TaxOutOfRange"));

                    return;
                }

                /*
                 * إضافة البند إلى الفاتورة.
                 */
                invoice.Items.Add(
                    new PurchaseInvoiceItem
                    {
                        PurchaseOrderItemId =
                            purchaseOrderItemId,

                        Quantity =
                            quantity,

                        UnitPrice =
                            unitPrice,

                        TaxRate =
                            taxRate
                    });
            }

            /*
             * يجب وجود بند واحد على الأقل.
             */
            if (!invoice.Items.Any())
            {
                ShowError(
                    Translate("OneItemRequired"));

                return;
            }

            /*
             * التحقق من أن الخصم لا يتجاوز
             * إجمالي الفاتورة قبل الضريبة.
             */
            decimal subtotal =
                invoice.Items.Sum(item =>
                    item.Quantity *
                    item.UnitPrice);

            if (discountAmount > subtotal)
            {
                ShowError(
                    Translate("DiscountExceedsSubtotal"));

                return;
            }

            try
            {
                _purchaseInvoiceService.CreateDraft(
                    invoice);

                Response.Redirect(
                    "Details.aspx?id=" +
                    invoice.Id,
                    false);

                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception exception)
            {
                ShowError(
                    exception.Message);
            }
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



        private bool TryParseDecimal(
            string text,
            out decimal value)
        {
            NumberStyles styles =
                NumberStyles.AllowDecimalPoint |
                NumberStyles.AllowLeadingSign;

            return decimal.TryParse(
                       text,
                       styles,
                       CultureInfo.InvariantCulture,
                       out value)
                   ||
                   decimal.TryParse(
                       text,
                       styles,
                       CultureInfo.CurrentCulture,
                       out value);
        }
    }
}