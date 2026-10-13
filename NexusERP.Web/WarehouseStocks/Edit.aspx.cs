using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Entities;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;

namespace NexusERP.Web.WarehouseStocks
{
    public partial class Edit : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;

        private IWarehouseStockService _warehouseStockService;
        private IWarehouseService _warehouseService;
        private IItemService _itemService;

        private int WarehouseStockId
        {
            get
            {
                return ViewState["WarehouseStockId"] == null
                    ? 0
                    : (int)ViewState["WarehouseStockId"];
            }

            set
            {
                ViewState["WarehouseStockId"] = value;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);

            _warehouseStockService =
                new WarehouseStockService(_unitOfWork);

            _warehouseService =
                new WarehouseService(_unitOfWork);

            _itemService =
                new ItemService(_unitOfWork);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadWarehouses();
                LoadItems();
                LoadWarehouseStock();
            }
        }

        private void LoadWarehouses()
        {
            var warehouses = _warehouseService
                .GetAll()
                .Where(warehouse => !warehouse.IsDeleted)
                .OrderBy(warehouse => warehouse.NameAr)
                .Select(warehouse => new
                {
                    warehouse.Id,

                    DisplayName =
                        warehouse.Code + " - " +
                        warehouse.NameAr
                })
                .ToList();

            ddlWarehouse.DataSource = warehouses;
            ddlWarehouse.DataTextField = "DisplayName";
            ddlWarehouse.DataValueField = "Id";
            ddlWarehouse.DataBind();

            ddlWarehouse.Items.Insert(
                0,
                new ListItem("-- اختر المستودع --", ""));
        }

        private void LoadItems()
        {
            var items = _itemService
                .GetAll()
                .Where(item => !item.IsDeleted)
                .OrderBy(item => item.NameAr)
                .Select(item => new
                {
                    item.Id,

                    DisplayName =
                        item.Code + " - " +
                        item.NameAr
                })
                .ToList();

            ddlItem.DataSource = items;
            ddlItem.DataTextField = "DisplayName";
            ddlItem.DataValueField = "Id";
            ddlItem.DataBind();

            ddlItem.Items.Insert(
                0,
                new ListItem("-- اختر الصنف --", ""));
        }

        private void LoadWarehouseStock()
        {
            int id;

            if (!int.TryParse(Request.QueryString["id"], out id))
            {
                RedirectToIndex();
                return;
            }

            WarehouseStock stock =
                _warehouseStockService.GetById(id);

            if (stock == null)
            {
                RedirectToIndex();
                return;
            }

            WarehouseStockId = stock.Id;

            ddlWarehouse.SelectedValue =
                stock.WarehouseId.ToString();

            ddlItem.SelectedValue =
                stock.ItemId.ToString();

            txtQuantity.Text =
                stock.Quantity.ToString(
                    "0.##",
                    CultureInfo.InvariantCulture);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid || WarehouseStockId == 0)
            {
                return;
            }

            int warehouseId;
            int itemId;
            decimal quantity;

            if (!int.TryParse(
                ddlWarehouse.SelectedValue,
                out warehouseId))
            {
                ShowError("يرجى اختيار مستودع صحيح.");
                return;
            }

            if (!int.TryParse(
                ddlItem.SelectedValue,
                out itemId))
            {
                ShowError("يرجى اختيار صنف صحيح.");
                return;
            }

            if (!TryParseDecimal(txtQuantity.Text, out quantity))
            {
                ShowError("قيمة الكمية غير صحيحة.");
                return;
            }

            if (quantity < 0)
            {
                ShowError("الكمية لا يمكن أن تكون سالبة.");
                return;
            }

            try
            {
                WarehouseStock stock =
                    _warehouseStockService.GetById(
                        WarehouseStockId);

                if (stock == null)
                {
                    ShowError("رصيد المستودع المطلوب غير موجود.");
                    return;
                }

                stock.WarehouseId = warehouseId;
                stock.ItemId = itemId;
                stock.Quantity = quantity;
                stock.UpdatedAt = DateTime.Now;

                _warehouseStockService.Update(stock);

                RedirectToIndex();
            }
            catch (InvalidOperationException ex)
            {
                ShowError(ex.Message);
            }
            catch (Exception)
            {
                ShowError(
                    "حدث خطأ أثناء تعديل رصيد المستودع.");
            }
        }

        private bool TryParseDecimal(
            string value,
            out decimal result)
        {
            if (decimal.TryParse(
                value,
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out result))
            {
                return true;
            }

            return decimal.TryParse(
                value,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out result);
        }

        private void ShowError(string message)
        {
            CustomValidator validator = new CustomValidator
            {
                IsValid = false,
                ErrorMessage = message,
                ValidationGroup = "WarehouseStockForm"
            };

            Page.Validators.Add(validator);
        }

        private void RedirectToIndex()
        {
            Response.Redirect(
                "~/WarehouseStocks/Index.aspx",
                false);

            Context.ApplicationInstance.CompleteRequest();
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (_unitOfWork != null)
            {
                _unitOfWork.Dispose();
            }
        }
    }
}