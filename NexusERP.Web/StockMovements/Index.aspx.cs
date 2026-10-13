using System;
using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Enums;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using NexusERP.Domain.Entities;
using System.Linq;
using System.Web.UI.WebControls;

namespace NexusERP.Web.StockMovements
{
    public partial class Index : System.Web.UI.Page
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
                LoadMovementTypes();

                if (Request.QueryString["cancelled"] == "1")
                {
                    ShowSuccess(
                        "تم إلغاء الحركة وإنشاء الحركة العكسية بنجاح.");
                }
            }

            // تجهيز بيانات الجدول قبل معالجة أحداثه.
            LoadStockMovements();
        }



        protected void btnSearch_Click(object sender, EventArgs e)
        {
            pnlMessage.Visible = false;
            gvStockMovements.PageIndex = 0;

            LoadStockMovements();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            pnlMessage.Visible = false;

            // مسح فلاتر الصفحة.
            ddlMovementType.SelectedIndex = 0;
            txtReferenceNumber.Text = string.Empty;

            // مسح فلاتر جدول DevExpress.
            gvStockMovements.FilterExpression = string.Empty;
            gvStockMovements.SearchPanelFilter = string.Empty;

            // إلغاء الفرز والتجميع.
            gvStockMovements.ClearSort();

            // العودة إلى الصفحة الأولى وإعادة تحميل البيانات.
            gvStockMovements.PageIndex = 0;
            LoadStockMovements();
        }

        private void LoadMovementTypes()
        {
            ddlMovementType.Items.Clear();

            ddlMovementType.Items.Add(
                new ListItem("جميع الأنواع", ""));

            foreach (StockMovementType movementType in
                Enum.GetValues(typeof(StockMovementType)))
            {
                ddlMovementType.Items.Add(
                    new ListItem(
                        GetMovementTypeName(movementType),
                        ((int)movementType).ToString()));
            }
        }


        private void LoadStockMovements()
        {
            try
            {
                // تحويل النتائج إلى قائمة قبل تطبيق التصفية.
                var movements =
                    _stockMovementService.GetAll().ToList();

                int movementTypeValue;

                if (int.TryParse(
                    ddlMovementType.SelectedValue,
                    out movementTypeValue))
                {
                    StockMovementType selectedType =
                        (StockMovementType)movementTypeValue;

                    movements = movements
                        .Where(movement =>
                            movement.MovementType == selectedType)
                        .ToList();
                }

                string referenceNumber =
                    txtReferenceNumber.Text.Trim();

                if (!string.IsNullOrWhiteSpace(referenceNumber))
                {
                    movements = movements
                        .Where(movement =>
                            !string.IsNullOrEmpty(
                                movement.ReferenceNumber) &&
                            movement.ReferenceNumber.IndexOf(
                                referenceNumber,
                                StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToList();
                }

                var rows = movements.Select(movement => new
                {
                    movement.Id,
                    movement.MovementType,
                    movement.IsCancelled,
                    movement.OriginalMovementId,

                    MovementTypeName =
        GetMovementTypeName(movement.MovementType),

                    WarehouseName = movement.Warehouse != null
        ? movement.Warehouse.NameAr
        : "-",

                    ItemName = movement.Item != null
        ? movement.Item.NameAr
        : "-",

                    movement.Quantity,
                    movement.MovementDate,
                    movement.ReferenceNumber,

                    DestinationName = Server.HtmlDecode(
        GetDestinationName(
            movement.DestinationWarehouse,
            movement.MovementType,
            movement.PurchaseReturnId))
                }).ToList();

                gvStockMovements.DataSource = rows;
                gvStockMovements.DataBind();
            }
            catch (Exception ex)
            {
                ShowError(
                    "حدث خطأ أثناء تحميل حركات المخزون: " +
                    ex.Message);
            }
        }

  
        protected string GetMovementTypeName(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            StockMovementType movementType =
                (StockMovementType)value;

            switch (movementType)
            {
                case StockMovementType.OpeningBalance:
                    return "رصيد افتتاحي";

                case StockMovementType.Receipt:
                    return "استلام";

                case StockMovementType.Issue:
                    return "صرف";

                case StockMovementType.Transfer:
                    return "تحويل";

                case StockMovementType.StockAdjustment:
                    return "تسوية جرد";

                default:
                    return "غير معروف";
            }
        }

        protected string GetDestinationName(
    object destinationWarehouseValue,
    object movementTypeValue,
    object purchaseReturnIdValue)
        {
            Warehouse destinationWarehouse =
                destinationWarehouseValue as Warehouse;

            if (destinationWarehouse != null)
            {
                return Server.HtmlEncode(
                    destinationWarehouse.NameAr);
            }

            if (movementTypeValue == null ||
                movementTypeValue == DBNull.Value)
            {
                return "-";
            }

            StockMovementType movementType =
                (StockMovementType)movementTypeValue;

            bool isPurchaseReturn =
                purchaseReturnIdValue != null &&
                purchaseReturnIdValue != DBNull.Value;

            if (isPurchaseReturn)
            {
                if (movementType == StockMovementType.Issue)
                {
                    return "إرجاع إلى المورد";
                }

                if (movementType == StockMovementType.Receipt)
                {
                    return "إلغاء مرتجع — إعادة إلى المستودع";
                }
            }

            if (movementType == StockMovementType.Transfer)
            {
                return "غير محدد";
            }

            return "-";
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

        protected bool CanCancelMovement(
         object isCancelledValue,
         object originalMovementIdValue,
         object movementTypeValue)
        {
            bool isCancelled =
                Convert.ToBoolean(isCancelledValue);

            bool isReversal =
                originalMovementIdValue != null &&
                originalMovementIdValue != DBNull.Value;

            StockMovementType movementType =
                (StockMovementType)movementTypeValue;

            bool unsupportedType =
                movementType ==
                    StockMovementType.OpeningBalance ||
                movementType ==
                    StockMovementType.StockAdjustment;

            return !isCancelled &&
                   !isReversal &&
                   !unsupportedType;
        }

        private void ShowSuccess(string message)
        {
            pnlMessage.Visible = true;
            pnlMessage.CssClass = "alert alert-success";
            litMessage.Text = Server.HtmlEncode(message);
        }
    }
}