using System;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Entities;
using NexusERP.Domain.Enums;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;

namespace NexusERP.Web.PurchaseReceipts
{
    public partial class Create : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IStockMovementService _stockMovementService;
        private IPurchaseReceiptService _purchaseReceiptService;

        protected void Page_Init(
            object sender,
            EventArgs e)
        {
            _context = new ApplicationDbContext();

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
                txtReceiptDate.Text =
                    DateTime.Today.ToString(
                        "yyyy-MM-dd");

                LoadPurchaseOrders();
                LoadWarehouses();

                int purchaseOrderId;

                if (int.TryParse(
                    Request.QueryString[
                        "purchaseOrderId"],
                    out purchaseOrderId))
                {
                    ListItem orderItem =
                        ddlPurchaseOrder.Items
                            .FindByValue(
                                purchaseOrderId
                                    .ToString());

                    if (orderItem != null)
                    {
                        ddlPurchaseOrder
                            .SelectedValue =
                            purchaseOrderId
                                .ToString();

                        LoadReceiptItems(
                            purchaseOrderId);
                    }
                }
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
                                .Approved ||
                         order.Status ==
                            PurchaseOrderStatus
                                .PartiallyReceived))
                    .OrderByDescending(order =>
                        order.OrderDate)
                    .Select(order => new
                    {
                        order.Id,

                        DisplayName =
                            order.OrderNumber +
                            " - " +
                            (order.Supplier != null
                                ? order.Supplier.NameAr
                                : "مورد غير محدد")
                    })
                    .ToList();

            ddlPurchaseOrder.DataSource =
                purchaseOrders;

            ddlPurchaseOrder.DataValueField =
                "Id";

            ddlPurchaseOrder.DataTextField =
                "DisplayName";

            ddlPurchaseOrder.DataBind();

            ddlPurchaseOrder.Items.Insert(
                0,
                new ListItem(
                    "-- اختر أمر الشراء --",
                    ""));
        }

        private void LoadWarehouses()
        {
            var warehouses =
                _unitOfWork.Warehouses
                    .GetAll()
                    .Where(warehouse =>
                        !warehouse.IsDeleted &&
                        warehouse.IsActive)
                    .OrderBy(warehouse =>
                        warehouse.NameAr)
                    .ToList();

            ddlWarehouse.DataSource =
                warehouses;

            ddlWarehouse.DataValueField =
                "Id";

            ddlWarehouse.DataTextField =
                "NameAr";

            ddlWarehouse.DataBind();

            ddlWarehouse.Items.Insert(
                0,
                new ListItem(
                    "-- اختر المستودع --",
                    ""));
        }

        protected void
            ddlPurchaseOrder_SelectedIndexChanged(
                object sender,
                EventArgs e)
        {
            HideMessage();

            int purchaseOrderId;

            if (!int.TryParse(
                ddlPurchaseOrder.SelectedValue,
                out purchaseOrderId))
            {
                gvReceiptItems.DataSource = null;
                gvReceiptItems.DataBind();
                return;
            }

            LoadReceiptItems(purchaseOrderId);
        }

        private void LoadReceiptItems(
            int purchaseOrderId)
        {
            PurchaseOrder purchaseOrder =
                _unitOfWork.PurchaseOrders
                    .GetById(purchaseOrderId);

            if (purchaseOrder == null ||
                purchaseOrder.IsDeleted)
            {
                gvReceiptItems.DataSource = null;
                gvReceiptItems.DataBind();

                ShowError(
                    "أمر الشراء غير موجود.");

                return;
            }

            var receiptItems =
                purchaseOrder.Items
                    .Where(orderItem =>
                        !orderItem.IsDeleted)
                    .Select(orderItem =>
                    {
                        decimal receivedQuantity =
                            _purchaseReceiptService
                                .GetReceivedQuantity(
                                    orderItem.Id);

                        decimal remainingQuantity =
                            orderItem.Quantity -
                            receivedQuantity;

                        return new
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
                                    : "صنف غير موجود",

                            OrderedQuantity =
                                orderItem.Quantity,

                            ReceivedQuantity =
                                receivedQuantity,

                            RemainingQuantity =
                                remainingQuantity < 0
                                    ? 0
                                    : remainingQuantity
                        };
                    })
                    .Where(item =>
                        item.RemainingQuantity > 0)
                    .ToList();

            gvReceiptItems.DataSource =
                receiptItems;

            gvReceiptItems.DataBind();

            if (!receiptItems.Any())
            {
                ShowError(
                    "تم استلام جميع كميات " +
                    "أمر الشراء.");
            }
        }

        protected void btnSaveDraft_Click(
            object sender,
            EventArgs e)
        {
            HideMessage();

            if (!Page.IsValid)
            {
                return;
            }

            int purchaseOrderId;
            int warehouseId;
            DateTime receiptDate;

            if (!int.TryParse(
                ddlPurchaseOrder.SelectedValue,
                out purchaseOrderId))
            {
                ShowError(
                    "يجب اختيار أمر الشراء.");

                return;
            }

            if (!int.TryParse(
                ddlWarehouse.SelectedValue,
                out warehouseId))
            {
                ShowError(
                    "يجب اختيار مستودع الاستلام.");

                return;
            }

            if (!DateTime.TryParseExact(
                txtReceiptDate.Text,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out receiptDate))
            {
                ShowError(
                    "تاريخ الاستلام غير صحيح.");

                return;
            }

            var purchaseReceipt =
                new PurchaseReceipt
                {
                    ReceiptNumber =
                        txtReceiptNumber.Text.Trim(),

                    ReceiptDate = receiptDate,

                    PurchaseOrderId =
                        purchaseOrderId,

                    WarehouseId = warehouseId,

                    Notes =
                        string.IsNullOrWhiteSpace(
                            txtNotes.Text)
                            ? null
                            : txtNotes.Text.Trim()
                };

            foreach (GridViewRow row
                in gvReceiptItems.Rows)
            {
                if (row.RowType !=
                    DataControlRowType.DataRow)
                {
                    continue;
                }

                TextBox quantityTextBox =
                    row.FindControl(
                        "txtReceivedQuantity")
                    as TextBox;

                if (quantityTextBox == null ||
                    string.IsNullOrWhiteSpace(
                        quantityTextBox.Text))
                {
                    continue;
                }

                decimal receivedQuantity;

                if (!TryParseDecimal(
                    quantityTextBox.Text,
                    out receivedQuantity))
                {
                    ShowError(
                        "إحدى الكميات المدخلة " +
                        "غير صحيحة.");

                    return;
                }

                if (receivedQuantity < 0)
                {
                    ShowError(
                        "الكمية المستلمة لا يمكن " +
                        "أن تكون سالبة.");

                    return;
                }

                if (receivedQuantity == 0)
                {
                    continue;
                }

                int purchaseOrderItemId =
                    Convert.ToInt32(
                        gvReceiptItems.DataKeys[
                            row.RowIndex].Value);

                purchaseReceipt.Items.Add(
                    new PurchaseReceiptItem
                    {
                        PurchaseOrderItemId =
                            purchaseOrderItemId,

                        ReceivedQuantity =
                            receivedQuantity
                    });
            }

            if (!purchaseReceipt.Items.Any())
            {
                ShowError(
                    "يجب إدخال كمية مستلمة " +
                    "لصنف واحد على الأقل.");

                return;
            }

            try
            {
                if (!Request.IsAuthenticated ||
    !(User.IsInRole("Admin") ||
      User.IsInRole("WarehouseManager")))
                {
                    ShowError("ليس لديك صلاحية إنشاء مستند استلام.");
                    return;
                }

                string createdBy = User.Identity.Name;

                if (string.IsNullOrWhiteSpace(createdBy))
                {
                    ShowError("تعذر تحديد المستخدم الحالي.");
                    return;
                }

                createdBy = createdBy.Trim();

                if (createdBy.Length > 256)
                {
                    ShowError("اسم المستخدم لا يمكن أن يتجاوز 256 حرفًا.");
                    return;
                }

                purchaseReceipt.CreatedBy = createdBy;

                foreach (PurchaseReceiptItem item in purchaseReceipt.Items)
                {
                    item.CreatedBy = createdBy;
                }

                _purchaseReceiptService
                    .CreateDraft(
                        purchaseReceipt);

                Response.Redirect(
                    "Index.aspx?created=true");
            }
            catch (Exception exception)
            {
                ShowError(exception.Message);

                // إعادة عرض الأصناف لأن DataBind
                // لا يعمل تلقائيًا بعد الخطأ.
                LoadReceiptItems(
                    purchaseOrderId);
            }
        }

        private bool TryParseDecimal(
            string value,
            out decimal result)
        {
            return decimal.TryParse(
                       value,
                       NumberStyles.Number,
                       CultureInfo.CurrentCulture,
                       out result)
                   ||
                   decimal.TryParse(
                       value,
                       NumberStyles.Number,
                       CultureInfo.InvariantCulture,
                       out result);
        }

        private void ShowError(
            string message)
        {
            lblMessage.Text = message;

            lblMessage.CssClass =
                "alert alert-danger d-block";

            lblMessage.Visible = true;
        }

        private void HideMessage()
        {
            lblMessage.Text = "";
            lblMessage.Visible = false;
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