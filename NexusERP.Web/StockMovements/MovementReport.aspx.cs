using NexusERP.Application.Interfaces;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using NexusERP.Domain.Enums;
using System.Data;

namespace NexusERP.Web.StockMovements
{
    public partial class MovementReport : System.Web.UI.Page
    {
        private ReportDocument _report;
        private IUnitOfWork _unitOfWork;

        protected void Page_Init(object sender, EventArgs e)
        {
            var context = new ApplicationDbContext();

            _unitOfWork = new UnitOfWork(context);

            LoadFilterLists();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // الفترة الافتراضية: من أول الشهر إلى اليوم.
                txtFromDate.Text =
                    new DateTime(
                        DateTime.Today.Year,
                        DateTime.Today.Month,
                        1)
                    .ToString(
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture);

                txtToDate.Text =
                    DateTime.Today.ToString(
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture);
            }
            LoadReport();
        }

        private void LoadFilterLists()
        {
            // قائمة المستودعات.
            ddlWarehouses.DataSource =
                _unitOfWork.Warehouses
                    .GetAll()
                    .OrderBy(warehouse => warehouse.NameAr)
                    .ToList();

            ddlWarehouses.DataTextField = "NameAr";
            ddlWarehouses.DataValueField = "Id";
            ddlWarehouses.DataBind();

            ddlWarehouses.Items.Insert(
                0,
                new ListItem("جميع المستودعات", "0"));

            // قائمة الأصناف.
            ddlItems.DataSource =
                _unitOfWork.Items
                    .GetAll()
                    .OrderBy(item => item.NameAr)
                    .ToList();

            ddlItems.DataTextField = "NameAr";
            ddlItems.DataValueField = "Id";
            ddlItems.DataBind();

            ddlItems.Items.Insert(
                0,
                new ListItem("جميع الأصناف", "0"));

            // القيم مطابقة لتعريف StockMovementType.
            ddlMovementTypes.Items.Clear();

            ddlMovementTypes.Items.Add(
                new ListItem("جميع الحركات", "0"));

            ddlMovementTypes.Items.Add(
                new ListItem("رصيد افتتاحي", "1"));

            ddlMovementTypes.Items.Add(
                new ListItem("استلام", "2"));

            ddlMovementTypes.Items.Add(
                new ListItem("صرف", "3"));

            ddlMovementTypes.Items.Add(
                new ListItem("تحويل بين المستودعات", "4"));

            ddlMovementTypes.Items.Add(
                new ListItem("تسوية جرد", "5"));
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
                {
                    _unitOfWork.Dispose();
                }
            }
        }
        private void LoadReport()
        {
            lblMessage.Visible = false;
            crvStockMovements.Visible = false;

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

            if (!validFrom || !validTo || fromDate > toDate)
            {
                lblMessage.Text =
                    "اختاري تاريخين صحيحين، بحيث يكون تاريخ البداية قبل النهاية أو مساويًا لها.";

                lblMessage.Visible = true;
                return;
            }

            int warehouseId;
            int itemId;
            int movementType;

            if (!int.TryParse(ddlWarehouses.SelectedValue, out warehouseId) ||
                !int.TryParse(ddlItems.SelectedValue, out itemId) ||
                !int.TryParse(ddlMovementTypes.SelectedValue, out movementType))
            {
                lblMessage.Text = "اختيارات التصفية غير صحيحة.";
                lblMessage.Visible = true;
                return;
            }

            // يشمل جميع أوقات يوم النهاية.
            DateTime endExclusive = toDate.Date.AddDays(1);

            var movements = _unitOfWork.StockMovements
                .GetAll()
                .Where(m =>
                    m.MovementDate >= fromDate.Date &&
                    m.MovementDate < endExclusive);

            if (warehouseId != 0)
            {
                // التحويل يظهر للمستودع المصدر والوجهة.
                movements = movements.Where(m =>
                    m.WarehouseId == warehouseId ||
                    m.DestinationWarehouseId == warehouseId);
            }

            if (itemId != 0)
            {
                movements = movements.Where(m => m.ItemId == itemId);
            }

            if (movementType != 0)
            {
                movements = movements.Where(m =>
                    (int)m.MovementType == movementType);
            }

            var table = new DataTable("StockMovements");

            table.Columns.Add("MovementDate", typeof(DateTime));
            table.Columns.Add("MovementTypeName", typeof(string));
            table.Columns.Add("WarehouseName", typeof(string));
            table.Columns.Add("DestinationWarehouseName", typeof(string));
            table.Columns.Add("ItemCode", typeof(string));
            table.Columns.Add("ItemName", typeof(string));
            table.Columns.Add("Quantity", typeof(decimal));
            table.Columns.Add("ReferenceNumber", typeof(string));
            table.Columns.Add("StatusName", typeof(string));
            table.Columns.Add("OriginalMovementId", typeof(int));

            foreach (var movement in movements
                .OrderBy(m => m.MovementDate)
                .ThenBy(m => m.Id))
            {
                var warehouse = movement.Warehouse ??
                    _unitOfWork.Warehouses.GetById(movement.WarehouseId);

                var item = movement.Item ??
                    _unitOfWork.Items.GetById(movement.ItemId);

                if (warehouse == null || item == null)
                {
                    throw new InvalidOperationException(
                        "تعذر العثور على المستودع أو الصنف للحركة رقم " +
                        movement.Id);
                }

                string destinationName = "";

                if (movement.DestinationWarehouseId.HasValue)
                {
                    var destination = movement.DestinationWarehouse ??
                        _unitOfWork.Warehouses.GetById(
                            movement.DestinationWarehouseId.Value);

                    if (destination == null)
                    {
                        throw new InvalidOperationException(
                            "تعذر العثور على مستودع الوجهة للحركة رقم " +
                            movement.Id);
                    }

                    destinationName = destination.NameAr;
                }

                string statusName = movement.IsCancelled
                    ? "ملغاة"
                    : movement.OriginalMovementId.HasValue
                        ? "حركة عكسية"
                        : "فعالة";

                table.Rows.Add(
                    movement.MovementDate,
                    GetMovementTypeName(movement.MovementType),
                    warehouse.NameAr,
                    destinationName,
                    item.Code,
                    item.NameAr,
                    movement.Quantity,
                    movement.ReferenceNumber ?? "",
                    statusName,
                    movement.OriginalMovementId.HasValue
                        ? (object)movement.OriginalMovementId.Value
                        : DBNull.Value);
            }

            if (table.Rows.Count == 0)
            {
                lblMessage.Text =
                    "لا توجد حركات مخزون مطابقة للاختيارات المحددة.";

                lblMessage.Visible = true;
                return;
            }

            _report = new NexusERP.Reporting.StockMovementReport();
            _report.SetDataSource(table);

            crvStockMovements.ReportSource = _report;
            crvStockMovements.Visible = true;
        }

        private string GetMovementTypeName(StockMovementType type)
        {
            switch (type)
            {
                case StockMovementType.OpeningBalance:
                    return "رصيد افتتاحي";

                case StockMovementType.Receipt:
                    return "استلام";

                case StockMovementType.Issue:
                    return "صرف";

                case StockMovementType.Transfer:
                    return "تحويل بين المستودعات";

                case StockMovementType.StockAdjustment:
                    return "تسوية جرد";

                default:
                    return type.ToString();
            }
        }
    }
}