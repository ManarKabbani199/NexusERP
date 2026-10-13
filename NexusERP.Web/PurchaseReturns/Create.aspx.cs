using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Entities;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;

namespace NexusERP.Web.PurchaseReturns
{
    public partial class Create : System.Web.UI.Page
    {
        private ApplicationDbContext _context;

        private IUnitOfWork _unitOfWork;

        private IStockMovementService
            _stockMovementService;

        private IPurchaseReturnService
            _purchaseReturnService;

        protected void Page_Init(
            object sender,
            EventArgs e)
        {
            _context =
                new ApplicationDbContext();

            _unitOfWork =
                new UnitOfWork(_context);

            _stockMovementService =
                new StockMovementService(
                    _unitOfWork);

            _purchaseReturnService =
                new PurchaseReturnService(
                    _unitOfWork,
                    _stockMovementService);
        }

        protected void Page_Load(
    object sender,
    EventArgs e)
        {
            if (!IsPostBack)
            {
                txtReturnDate.Text =
                    DateTime.Today.ToString(
                        "yyyy-MM-dd");

                LoadPurchaseReceipts();

                SelectReceiptFromQueryString();
            }
        }

        private void LoadPurchaseReceipts()
        {
            var receipts =
                _unitOfWork.PurchaseReceipts
                    .GetAll()
                    .Where(receipt =>
                        !receipt.IsDeleted &&
                        receipt.IsPosted &&
                        !receipt.IsCancelled)
                    .OrderByDescending(receipt =>
                        receipt.ReceiptDate)
                    .Select(receipt => new
                    {
                        receipt.Id,

                        DisplayName =
                            receipt.ReceiptNumber +
                            " - " +
                            (receipt.PurchaseOrder != null &&
                             receipt.PurchaseOrder.Supplier != null
                                ? receipt.PurchaseOrder
                                    .Supplier.NameAr
                                : "مورد غير محدد")
                    })
                    .ToList();

            ddlPurchaseReceipt.DataSource =
                receipts;

            ddlPurchaseReceipt.DataValueField =
                "Id";

            ddlPurchaseReceipt.DataTextField =
                "DisplayName";

            ddlPurchaseReceipt.DataBind();

            ddlPurchaseReceipt.Items.Insert(
                0,
                new ListItem(
                    "-- اختر مستند الاستلام --",
                    ""));
        }

        protected void
            ddlPurchaseReceipt_SelectedIndexChanged(
                object sender,
                EventArgs e)
        {
            lblMessage.Visible = false;

            int receiptId;

            if (!int.TryParse(
                ddlPurchaseReceipt.SelectedValue,
                out receiptId))
            {
                ClearReceiptInformation();
                return;
            }

            LoadReceiptInformation(receiptId);
            LoadReceiptItems(receiptId);
        }

        private void LoadReceiptInformation(
            int receiptId)
        {
            PurchaseReceipt receipt =
                _unitOfWork.PurchaseReceipts
                    .GetById(receiptId);

            if (receipt == null ||
                receipt.IsDeleted)
            {
                ClearReceiptInformation();

                ShowError(
                    "مستند الاستلام غير موجود.");

                return;
            }

            txtWarehouse.Text =
                receipt.Warehouse != null
                    ? receipt.Warehouse.NameAr
                    : "-";

            txtSupplier.Text =
                receipt.PurchaseOrder != null &&
                receipt.PurchaseOrder.Supplier != null
                    ? receipt.PurchaseOrder
                        .Supplier.NameAr
                    : "-";
        }

        private void LoadReceiptItems(
            int receiptId)
        {
            PurchaseReceipt receipt =
                _unitOfWork.PurchaseReceipts
                    .GetById(receiptId);

            if (receipt == null ||
                receipt.IsDeleted)
            {
                gvReturnItems.DataSource = null;
                gvReturnItems.DataBind();

                return;
            }

            var items =
                receipt.Items
                    .Where(receiptItem =>
                        !receiptItem.IsDeleted)
                    .Select(receiptItem =>
                    {
                        decimal remainingQuantity =
                            _purchaseReturnService
                                .GetRemainingReturnableQuantity(
                                    receiptItem.Id);

                        decimal previouslyReturned =
                            receiptItem.ReceivedQuantity -
                            remainingQuantity;

                        PurchaseOrderItem orderItem =
                            receiptItem.PurchaseOrderItem;

                        return new
                        {
                            PurchaseReceiptItemId =
                                receiptItem.Id,

                            ItemCode =
                                orderItem != null &&
                                orderItem.Item != null
                                    ? orderItem.Item.Code
                                    : "-",

                            ItemName =
                                orderItem != null &&
                                orderItem.Item != null
                                    ? orderItem.Item.NameAr
                                    : "صنف غير موجود",

                            ReceivedQuantity =
                                receiptItem.ReceivedQuantity,

                            PreviouslyReturned =
                                previouslyReturned,

                            RemainingQuantity =
                                remainingQuantity,

                            UnitPrice =
                                orderItem != null
                                    ? orderItem.UnitPrice
                                    : 0
                        };
                    })
                    .Where(item =>
                        item.RemainingQuantity > 0)
                    .ToList();

            gvReturnItems.DataSource =
                items;

            gvReturnItems.DataBind();
        }

        protected void btnSave_Click(
            object sender,
            EventArgs e)
        {
            string createdBy;

            try
            {
                createdBy = GetCurrentUserName();
            }
            catch (Exception exception)
            {
                ShowError(exception.Message);
                return;
            }

            if (!Page.IsValid)
            {
                return;
            }

            int receiptId;

            if (!int.TryParse(
                ddlPurchaseReceipt.SelectedValue,
                out receiptId))
            {
                ShowError(
                    "يجب اختيار مستند الاستلام.");

                return;
            }

            DateTime returnDate;

            if (!DateTime.TryParse(
                txtReturnDate.Text,
                out returnDate))
            {
                ShowError(
                    "تاريخ المرتجع غير صحيح.");

                return;
            }

            PurchaseReceipt receipt =
                _unitOfWork.PurchaseReceipts
                    .GetById(receiptId);

            if (receipt == null ||
                receipt.IsDeleted)
            {
                ShowError(
                    "مستند الاستلام غير موجود.");

                return;
            }

            List<PurchaseReturnItem> returnItems =
                new List<PurchaseReturnItem>();

            foreach (GridViewRow row
                in gvReturnItems.Rows)
            {
                if (row.RowType !=
                    DataControlRowType.DataRow)
                {
                    continue;
                }

                int receiptItemId =
                    Convert.ToInt32(
                        gvReturnItems.DataKeys[
                            row.RowIndex].Value);

                TextBox quantityTextBox =
                    row.FindControl(
                        "txtReturnedQuantity")
                    as TextBox;

                HiddenField unitPriceField =
                    row.FindControl(
                        "hiddenUnitPrice")
                    as HiddenField;

                HiddenField remainingField =
                    row.FindControl(
                        "hiddenRemainingQuantity")
                    as HiddenField;

                decimal returnedQuantity;

                if (quantityTextBox == null ||
                    !TryParseDecimal(
                        quantityTextBox.Text,
                        out returnedQuantity))
                {
                    returnedQuantity = 0;
                }

                if (returnedQuantity <= 0)
                {
                    continue;
                }

                decimal unitPrice;

                if (unitPriceField == null ||
                    !TryParseDecimal(
                        unitPriceField.Value,
                        out unitPrice))
                {
                    unitPrice = 0;
                }

                decimal remainingQuantity;

                if (remainingField == null ||
                    !TryParseDecimal(
                        remainingField.Value,
                        out remainingQuantity))
                {
                    remainingQuantity = 0;
                }

                if (returnedQuantity >
                    remainingQuantity)
                {
                    ShowError(
                        "إحدى الكميات المرتجعة أكبر " +
                        "من الكمية المتاحة للإرجاع.");

                    return;
                }

                returnItems.Add(
                    new PurchaseReturnItem
                    {
                        PurchaseReceiptItemId =
                            receiptItemId,

                        ReturnedQuantity =
                            returnedQuantity,

                        UnitPrice =
                            unitPrice
                    });
            }

            if (!returnItems.Any())
            {
                ShowError(
                    "يجب إدخال كمية مرتجعة " +
                    "لصنف واحد على الأقل.");

                return;
            }

            PurchaseReturn purchaseReturn =
                new PurchaseReturn
                {
                    ReturnNumber =
                        txtReturnNumber.Text.Trim(),

                    ReturnDate =
                        returnDate,

                    PurchaseReceiptId =
                        receipt.Id,

                    WarehouseId =
                        receipt.WarehouseId,

                    Notes =
                        string.IsNullOrWhiteSpace(
                            txtNotes.Text)
                            ? null
                            : txtNotes.Text.Trim(),

                    CreatedBy = createdBy,

                    Items =
                        returnItems
                };

            try
            {
                _purchaseReturnService.Create(
                    purchaseReturn);

                Response.Redirect(
                    "Details.aspx?id=" +
                    purchaseReturn.Id,
                    false);

                Context.ApplicationInstance
                    .CompleteRequest();
            }
            catch (Exception exception)
            {
                ShowError(
                    exception.Message);
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

        private string GetCurrentUserName()
        {
            if (!Request.IsAuthenticated ||
                !(User.IsInRole("Admin") ||
                  User.IsInRole("PurchasingManager") ||
                  User.IsInRole("WarehouseManager")))
            {
                throw new System.Web.HttpException(
                    403,
                    "ليس لديك صلاحية إدارة مرتجعات المشتريات.");
            }

            string userName = User.Identity.Name;

            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new InvalidOperationException(
                    "تعذر تحديد المستخدم الحالي.");
            }

            userName = userName.Trim();

            if (userName.Length > 256)
            {
                throw new InvalidOperationException(
                    "اسم المستخدم لا يمكن أن يتجاوز 256 حرفًا.");
            }

            return userName;
        }

        private void ClearReceiptInformation()
        {
            txtSupplier.Text = string.Empty;
            txtWarehouse.Text = string.Empty;

            gvReturnItems.DataSource = null;
            gvReturnItems.DataBind();
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

        private void SelectReceiptFromQueryString()
        {
            int receiptId;

            if (!int.TryParse(
                Request.QueryString["purchaseReceiptId"],
                out receiptId))
            {
                return;
            }

            PurchaseReceipt receipt =
                _unitOfWork.PurchaseReceipts
                    .GetById(receiptId);

            if (receipt == null ||
                receipt.IsDeleted)
            {
                ShowError(
                    "مستند الاستلام غير موجود.");

                return;
            }

            if (!receipt.IsPosted)
            {
                ShowError(
                    "يجب ترحيل مستند الاستلام قبل " +
                    "إنشاء مرتجع مشتريات.");

                return;
            }

            if (receipt.IsCancelled)
            {
                ShowError(
                    "لا يمكن إنشاء مرتجع من مستند " +
                    "استلام ملغي.");

                return;
            }

            ListItem selectedItem =
                ddlPurchaseReceipt.Items.FindByValue(
                    receiptId.ToString());

            /*
             * إذا لم يكن المستند موجودًا في القائمة،
             * نضيفه بعد التحقق من صلاحيته.
             */
            if (selectedItem == null)
            {
                string supplierName =
                    receipt.PurchaseOrder != null &&
                    receipt.PurchaseOrder.Supplier != null
                        ? receipt.PurchaseOrder
                            .Supplier.NameAr
                        : "مورد غير محدد";

                selectedItem =
                    new ListItem(
                        receipt.ReceiptNumber +
                        " - " +
                        supplierName,
                        receipt.Id.ToString());

                ddlPurchaseReceipt.Items.Add(
                    selectedItem);
            }

            ddlPurchaseReceipt.ClearSelection();

            selectedItem.Selected = true;

            LoadReceiptInformation(
                receiptId);

            LoadReceiptItems(
                receiptId);
        }
    }
}