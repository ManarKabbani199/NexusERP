using CrystalDecisions.CrystalReports.Engine;
using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace NexusERP.Web.WarehouseStocks
{
    public partial class StockReport : System.Web.UI.Page
    {
        private IUnitOfWork _unitOfWork;
        private IWarehouseStockService _stockService;
        private ReportDocument _report;

        protected void Page_Init(object sender, EventArgs e)
        {
            var context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(context);

            _stockService =
                new WarehouseStockService(_unitOfWork);

            // تعبئة القائمة مبكرًا لاستقبال الاختيار عند إعادة الطلب.
            var warehouseService =
                new WarehouseService(_unitOfWork);

            ddlWarehouses.DataSource = warehouseService
                .GetAll()
                .OrderBy(warehouse => warehouse.NameAr)
                .ToList();

            ddlWarehouses.DataTextField = "NameAr";
            ddlWarehouses.DataValueField = "Id";
            ddlWarehouses.DataBind();

            ddlWarehouses.Items.Insert(
                0,
                new ListItem("جميع المستودعات", "0"));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // هنا تكون قيمة المستودع المختار قد تم تحميلها.
            LoadReport();
        }

        private void LoadReport()
        {
            int warehouseId;


            if (!int.TryParse(
                ddlWarehouses.SelectedValue,
                out warehouseId))
            {
                throw new InvalidOperationException(
                    "اختيار المستودع غير صحيح.");
            }

            var stocks = _stockService.GetAll();

            if (chkHideZeroStock.Checked)
            {
                stocks = stocks.Where(stock => stock.Quantity > 0);
            }

            if (warehouseId != 0)
            {
                stocks = stocks.Where(
                    stock => stock.WarehouseId == warehouseId);
            }

            var table = new DataTable("WarehouseStock");

            table.Columns.Add("WarehouseName", typeof(string));
            table.Columns.Add("ItemCode", typeof(string));
            table.Columns.Add("ItemName", typeof(string));
            table.Columns.Add("Quantity", typeof(decimal));

            foreach (var stock in stocks)
            {
                var warehouse = stock.Warehouse
                    ?? _unitOfWork.Warehouses.GetById(
                        stock.WarehouseId);

                var item = stock.Item
                    ?? _unitOfWork.Items.GetById(stock.ItemId);

                if (warehouse == null || item == null)
                {
                    throw new InvalidOperationException(
                        "تعذر العثور على المستودع أو الصنف المرتبط برصيد المخزون.");
                }

                table.Rows.Add(
                    warehouse.NameAr,
                    item.Code,
                    item.NameAr,
                    stock.Quantity);
            }

            _report =
                new NexusERP.Reporting.WarehouseStockReport();

            _report.SetDataSource(table);
            crvWarehouseStock.ReportSource = _report;
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            try
            {
                if (_report != null)
                {
                    _report.Close();
                    _report.Dispose();
                }
            }
            finally
            {
                if (_unitOfWork != null)
                {
                    _unitOfWork.Dispose();
                }
            }
        }
    }
}