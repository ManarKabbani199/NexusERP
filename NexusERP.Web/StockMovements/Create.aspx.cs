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

namespace NexusERP.Web.StockMovements
{
    public partial class Create : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IStockMovementService _stockMovementService;

        protected void Page_Init(object sender, EventArgs e)
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);

            _stockMovementService =
                new StockMovementService(_unitOfWork);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMovementTypes();
                BindWarehouses();
                BindItems();

                txtMovementDate.Text =
                    DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
            }
        }

        private void BindMovementTypes()
        {
            ddlMovementType.Items.Clear();

            ddlMovementType.Items.Add(
                new ListItem("-- اختر نوع الحركة --", ""));

            ddlMovementType.Items.Add(
                new ListItem(
                    "رصيد افتتاحي",
                    ((int)StockMovementType.OpeningBalance)
                    .ToString()));

            ddlMovementType.Items.Add(
                new ListItem(
                    "استلام",
                    ((int)StockMovementType.Receipt)
                    .ToString()));

            ddlMovementType.Items.Add(
                new ListItem(
                    "صرف",
                    ((int)StockMovementType.Issue)
                    .ToString()));

            ddlMovementType.Items.Add(
                new ListItem(
                    "تحويل",
                    ((int)StockMovementType.Transfer)
                    .ToString()));

            ddlMovementType.Items.Add(
                new ListItem(
                    "تسوية جرد",
                    ((int)StockMovementType.StockAdjustment)
                    .ToString()));
        }

        private void BindWarehouses()
        {
            var warehouses = _unitOfWork.Warehouses
                .GetAll()
                .Where(warehouse =>
                    warehouse.IsActive &&
                    !warehouse.IsDeleted)
                .OrderBy(warehouse => warehouse.NameAr)
                .ToList();

            ddlWarehouse.DataSource = warehouses;
            ddlWarehouse.DataTextField = "NameAr";
            ddlWarehouse.DataValueField = "Id";
            ddlWarehouse.DataBind();

            ddlWarehouse.Items.Insert(
                0,
                new ListItem("-- اختر المستودع --", ""));

            ddlDestinationWarehouse.DataSource = warehouses;
            ddlDestinationWarehouse.DataTextField = "NameAr";
            ddlDestinationWarehouse.DataValueField = "Id";
            ddlDestinationWarehouse.DataBind();

            ddlDestinationWarehouse.Items.Insert(
                0,
                new ListItem("-- اختر مستودع الوجهة --", ""));
        }

        private void BindItems()
        {
            var items = _unitOfWork.Items
                .GetAll()
                .Where(item =>
                    item.IsActive &&
                    !item.IsDeleted)
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            try
            {
                decimal quantity;

                if (!TryParseQuantity(
                        txtQuantity.Text.Trim(),
                        out quantity))
                {
                    ShowError("قيمة الكمية غير صحيحة.");
                    return;
                }

                DateTime movementDate;

                if (!DateTime.TryParse(
                        txtMovementDate.Text,
                        out movementDate))
                {
                    ShowError("تاريخ الحركة غير صحيح.");
                    return;
                }

                StockMovementType movementType =
                    (StockMovementType)int.Parse(
                        ddlMovementType.SelectedValue);

                int? destinationWarehouseId = null;

                if (movementType == StockMovementType.Transfer)
                {
                    int parsedDestinationId;

                    if (!int.TryParse(
                            ddlDestinationWarehouse.SelectedValue,
                            out parsedDestinationId))
                    {
                        ShowError(
                            "يجب اختيار مستودع الوجهة عند التحويل.");

                        return;
                    }

                    destinationWarehouseId =
                        parsedDestinationId;
                }

                StockMovement movement = new StockMovement
                {
                    MovementType = movementType,

                    WarehouseId = int.Parse(
                        ddlWarehouse.SelectedValue),

                    ItemId = int.Parse(
                        ddlItem.SelectedValue),

                    Quantity = quantity,
                    MovementDate = movementDate,

                    DestinationWarehouseId =
                        destinationWarehouseId,

                    ReferenceNumber =
                        txtReferenceNumber.Text.Trim(),

                    Notes = txtNotes.Text.Trim(),

                    CreatedBy = User.Identity.IsAuthenticated
                        ? User.Identity.Name
                        : null
                };

                _stockMovementService.CreateMovement(movement);

                Response.Redirect("Index.aspx");
            }
            catch (InvalidOperationException ex)
            {
                ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                ShowError(
                    "حدث خطأ أثناء حفظ حركة المخزون: " +
                    ex.Message);
            }
        }

        private bool TryParseQuantity(
            string value,
            out decimal quantity)
        {
            if (decimal.TryParse(
                    value,
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out quantity))
            {
                return true;
            }

            return decimal.TryParse(
                value,
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out quantity);
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
            }
        }
    }
}