using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Entities;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;
using System.Globalization;

namespace NexusERP.Web.WarehouseStocks
{
    public partial class Delete : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IWarehouseStockService _warehouseStockService;

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

            WarehouseStockId = stock.Id;

            lblWarehouse.Text =
                stock.Warehouse == null
                    ? "-"
                    : stock.Warehouse.Code + " - " +
                      stock.Warehouse.NameAr;

            lblItemCode.Text =
                stock.Item == null
                    ? "-"
                    : stock.Item.Code;

            lblItemName.Text =
                stock.Item == null
                    ? "-"
                    : stock.Item.NameAr;

            lblUnitOfMeasure.Text =
                stock.Item == null
                    ? "-"
                    : GetUnitOfMeasureName(
                        stock.Item.UnitOfMeasure);

            lblQuantity.Text =
                stock.Quantity.ToString(
                    "N2",
                    CultureInfo.CurrentCulture);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (WarehouseStockId == 0)
            {
                ShowError(
                    "تعذر تحديد رصيد المستودع المطلوب حذفه.");
                return;
            }

            try
            {
                _warehouseStockService.Delete(
                    WarehouseStockId);

                RedirectToIndex();
            }
            catch (Exception)
            {
                ShowError(
                    "تعذر حذف الرصيد. قد يكون مرتبطًا بحركات مخزون.");
            }
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