using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Entities;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;

namespace NexusERP.Web.PurchaseOrders
{
    public partial class Create : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IPurchaseOrderService _purchaseOrderService;

        private DataTable OrderItemsTable
        {
            get
            {
                if (ViewState["OrderItemsTable"] == null)
                {
                    ViewState["OrderItemsTable"] =
                        CreateOrderItemsTable();
                }

                return (DataTable)ViewState["OrderItemsTable"];
            }

            set
            {
                ViewState["OrderItemsTable"] = value;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);

            _purchaseOrderService =
                new PurchaseOrderService(_unitOfWork);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtOrderDate.Text =
                    DateTime.Today.ToString("yyyy-MM-dd");

                LoadSuppliers();
                LoadItems();
                BindOrderItems();
            }
        }

        private void LoadSuppliers()
        {
            var suppliers = _unitOfWork.Suppliers
                .GetAll()
                .Where(supplier =>
                    !supplier.IsDeleted &&
                    supplier.IsActive)
                .OrderBy(supplier => supplier.NameAr)
                .ToList();

            ddlSupplier.DataSource = suppliers;
            ddlSupplier.DataTextField = "NameAr";
            ddlSupplier.DataValueField = "Id";
            ddlSupplier.DataBind();

            ddlSupplier.Items.Insert(
                0,
                new ListItem("-- اختر المورد --", ""));
        }

        private void LoadItems()
        {
            var items = _unitOfWork.Items
                .GetAll()
                .Where(item =>
                    !item.IsDeleted &&
                    item.IsActive)
                .OrderBy(item => item.NameAr)
                .ToList();

            ddlItem.DataSource = items;
            ddlItem.DataTextField = "NameAr";
            ddlItem.DataValueField = "Id";
            ddlItem.DataBind();

            ddlItem.Items.Insert(
                0,
                new ListItem("-- اختر الصنف --", ""));
        }

        protected void btnAddItem_Click(
            object sender,
            EventArgs e)
        {
            HideMessage();

            int itemId;
            decimal quantity;
            decimal unitPrice;

            if (!int.TryParse(
                ddlItem.SelectedValue,
                out itemId))
            {
                ShowError("يجب اختيار الصنف.");
                return;
            }

            if (!TryParseDecimal(
                txtQuantity.Text,
                out quantity) ||
                quantity <= 0)
            {
                ShowError(
                    "الكمية يجب أن تكون رقمًا أكبر من صفر.");
                return;
            }

            if (!TryParseDecimal(
                txtUnitPrice.Text,
                out unitPrice) ||
                unitPrice < 0)
            {
                ShowError(
                    "سعر الوحدة يجب أن يكون صفرًا أو أكبر.");
                return;
            }

            DataTable table = OrderItemsTable;

            bool itemAlreadyAdded = table
                .AsEnumerable()
                .Any(row =>
                    row.Field<int>("ItemId") == itemId);

            if (itemAlreadyAdded)
            {
                ShowError(
                    "تمت إضافة هذا الصنف مسبقًا.");
                return;
            }

            DataRow newRow = table.NewRow();

            newRow["ItemId"] = itemId;
            newRow["ItemName"] =
                ddlItem.SelectedItem.Text;
            newRow["Quantity"] = quantity;
            newRow["UnitPrice"] = unitPrice;
            newRow["LineTotal"] =
                quantity * unitPrice;

            table.Rows.Add(newRow);

            OrderItemsTable = table;

            BindOrderItems();
            ClearItemInputs();

            ShowSuccess(
                "تمت إضافة الصنف إلى أمر الشراء.");
        }

        protected void gvOrderItems_RowCommand(
            object sender,
            GridViewCommandEventArgs e)
        {
            if (e.CommandName != "RemoveItem")
            {
                return;
            }

            int itemId;

            if (!int.TryParse(
                e.CommandArgument.ToString(),
                out itemId))
            {
                return;
            }

            DataTable table = OrderItemsTable;

            DataRow rowToDelete = table
                .AsEnumerable()
                .FirstOrDefault(row =>
                    row.Field<int>("ItemId") == itemId);

            if (rowToDelete != null)
            {
                table.Rows.Remove(rowToDelete);
            }

            OrderItemsTable = table;

            BindOrderItems();

            ShowSuccess(
                "تمت إزالة الصنف من أمر الشراء.");
        }

        protected void btnSave_Click(
            object sender,
            EventArgs e)
        {
            HideMessage();

            if (!Page.IsValid)
            {
                return;
            }

            int supplierId;
            DateTime orderDate;

            if (!int.TryParse(
                ddlSupplier.SelectedValue,
                out supplierId))
            {
                ShowError("يجب اختيار المورد.");
                return;
            }

            if (!DateTime.TryParseExact(
                txtOrderDate.Text,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out orderDate))
            {
                ShowError("تاريخ أمر الشراء غير صحيح.");
                return;
            }

            if (OrderItemsTable.Rows.Count == 0)
            {
                ShowError(
                    "يجب إضافة صنف واحد على الأقل.");
                return;
            }

            try
            {
                if (!Request.IsAuthenticated ||
    !(User.IsInRole("Admin") ||
      User.IsInRole("PurchasingManager")))
                {
                    ShowError("ليس لديك صلاحية إنشاء أمر شراء.");
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

                var purchaseOrder =
                    new PurchaseOrder
                    {
                        CreatedBy = createdBy,

                        OrderNumber =
                            txtOrderNumber.Text.Trim(),

                        OrderDate = orderDate,

                        SupplierId = supplierId,

                        Notes = string.IsNullOrWhiteSpace(
                            txtNotes.Text)
                            ? null
                            : txtNotes.Text.Trim()
                    };

                foreach (DataRow row
                    in OrderItemsTable.Rows)
                {
                    purchaseOrder.Items.Add(
                        new PurchaseOrderItem
                        {
                            ItemId =
                                Convert.ToInt32(
                                    row["ItemId"]),

                            Quantity =
                                Convert.ToDecimal(
                                    row["Quantity"]),

                            UnitPrice =
                                Convert.ToDecimal(
                                    row["UnitPrice"]),

                                CreatedBy = createdBy,
                        });
                }

                _purchaseOrderService.Create(
                    purchaseOrder);

                Response.Redirect(
                    "Index.aspx?created=true");
            }
            catch (Exception exception)
            {
                ShowError(exception.Message);
            }
        }

        private DataTable CreateOrderItemsTable()
        {
            var table = new DataTable();

            table.Columns.Add(
                "ItemId",
                typeof(int));

            table.Columns.Add(
                "ItemName",
                typeof(string));

            table.Columns.Add(
                "Quantity",
                typeof(decimal));

            table.Columns.Add(
                "UnitPrice",
                typeof(decimal));

            table.Columns.Add(
                "LineTotal",
                typeof(decimal));

            return table;
        }

        private void BindOrderItems()
        {
            gvOrderItems.DataSource = OrderItemsTable;
            gvOrderItems.DataBind();

            decimal grandTotal = OrderItemsTable
                .AsEnumerable()
                .Sum(row =>
                    row.Field<decimal>("LineTotal"));

            lblGrandTotal.Text =
                grandTotal.ToString("N2");
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

        private void ClearItemInputs()
        {
            ddlItem.SelectedIndex = 0;
            txtQuantity.Text = "";
            txtUnitPrice.Text = "";
        }

        private void ShowSuccess(string message)
        {
            lblMessage.Text = message;
            lblMessage.CssClass =
                "alert alert-success d-block";
            lblMessage.Visible = true;
        }

        private void ShowError(string message)
        {
            lblMessage.Text = message;
            lblMessage.CssClass =
                "alert alert-danger d-block";
            lblMessage.Visible = true;
        }

        private void HideMessage()
        {
            lblMessage.Visible = false;
            lblMessage.Text = "";
        }

     
    }
}