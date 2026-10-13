using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Entities;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;
using System.Globalization;

namespace NexusERP.Web.WarehouseStocks
{
    public partial class Details : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IWarehouseStockService _warehouseStockService;

        protected void Page_Init(object sender, EventArgs e)
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);

            _warehouseStockService =
                new WarehouseStockService(_unitOfWork);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadWarehouseStock();
            }
        }

        private void LoadWarehouseStock()
        {
            int id;

            if (!int.TryParse(Request.QueryString["id"], out id))
            {
                ShowError("رقم رصيد المستودع غير صحيح.");
                return;
            }

            WarehouseStock stock =
                _warehouseStockService.GetById(id);

            if (stock == null)
            {
                ShowError("رصيد المستودع المطلوب غير موجود.");
                return;
            }

            lblWarehouse.Text =
                stock.Warehouse == null
                    ? "-"
                    : stock.Warehouse.Code + " - " +
                      stock.Warehouse.NameAr;

            lblBranch.Text =
                stock.Warehouse == null ||
                stock.Warehouse.Branch == null
                    ? "-"
                    : stock.Warehouse.Branch.NameAr;

            lblItemCode.Text =
                stock.Item == null
                    ? "-"
                    : stock.Item.Code;

            lblItemName.Text =
                stock.Item == null
                    ? "-"
                    : stock.Item.NameAr;

            lblItemCategory.Text =
                stock.Item == null ||
                stock.Item.ItemCategory == null
                    ? "-"
                    : stock.Item.ItemCategory.NameAr;

            lblUnitOfMeasure.Text =
                stock.Item == null
                    ? "-"
                    : GetUnitOfMeasureName(
                        stock.Item.UnitOfMeasure);

            lblQuantity.Text =
                stock.Quantity.ToString(
                    "N2",
                    CultureInfo.CurrentCulture);

            decimal minimumStockLevel =
                stock.Item == null
                    ? 0
                    : stock.Item.MinimumStockLevel;

            lblMinimumStockLevel.Text =
                minimumStockLevel.ToString(
                    "N2",
                    CultureInfo.CurrentCulture);

            if (stock.Quantity <= minimumStockLevel)
            {
                lblStockStatus.Text = "مخزون منخفض";
                lblStockStatus.CssClass = "badge bg-danger";
            }
            else
            {
                lblStockStatus.Text = "متوفر";
                lblStockStatus.CssClass = "badge bg-success";
            }

            lnkEdit.NavigateUrl =
                "~/WarehouseStocks/Edit.aspx?id=" + stock.Id;

            lnkDelete.NavigateUrl =
                "~/WarehouseStocks/Delete.aspx?id=" + stock.Id;
        }

        private string GetUnitOfMeasureName(string unitOfMeasure)
        {
            switch (unitOfMeasure)
            {
                case "Piece":
                    return "قطعة";

                case "Box":
                    return "صندوق";

                case "Carton":
                    return "كرتون";

                case "Kilogram":
                    return "كيلوغرام";

                case "Liter":
                    return "لتر";

                default:
                    return string.IsNullOrWhiteSpace(unitOfMeasure)
                        ? "-"
                        : unitOfMeasure;
            }
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            pnlError.Visible = true;
            pnlDetails.Visible = false;
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