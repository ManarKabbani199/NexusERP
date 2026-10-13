using System;
using System.Linq;
using System.Web.UI.WebControls;
using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;

namespace NexusERP.Web.PurchaseReceipts
{
    public partial class Index : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IStockMovementService _stockMovementService;
        private IPurchaseReceiptService _purchaseReceiptService;

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
                LoadPurchaseReceipts();

                if (Request.QueryString["created"] ==
                    "true")
                {
                    ShowSuccess(
                        "تم حفظ مستند الاستلام " +
                        "كمسودة بنجاح. " +
                        "لم يتغير المخزون بعد.");
                }
            }
        }

        private void LoadPurchaseReceipts()
        {
            var purchaseReceipts =
                _purchaseReceiptService
                    .GetAll()
                    .Select(receipt => new
                    {
                        receipt.Id,
                        receipt.ReceiptNumber,
                        receipt.ReceiptDate,
                        receipt.IsPosted,
                        receipt.IsCancelled,

                        PurchaseOrderNumber =
                            receipt.PurchaseOrder != null
                                ? receipt.PurchaseOrder
                                    .OrderNumber
                                : "-",

                        SupplierName =
                            receipt.PurchaseOrder != null &&
                            receipt.PurchaseOrder.Supplier !=
                                null
                                ? receipt.PurchaseOrder
                                    .Supplier.NameAr
                                : "-",

                        WarehouseName =
                            receipt.Warehouse != null
                                ? receipt.Warehouse.NameAr
                                : "-",

                        ItemsCount =
                            receipt.Items.Count(item =>
                                !item.IsDeleted),

                        StatusName =
                            receipt.IsCancelled
                                ? "ملغي"
                                : receipt.IsPosted
                                    ? "مُرحّل"
                                    : "مسودة"
                    })
                    .ToList();

            gvPurchaseReceipts.DataSource =
                purchaseReceipts;

            gvPurchaseReceipts.DataBind();
        }

        protected void
            gvPurchaseReceipts_RowCommand(
                object sender,
                GridViewCommandEventArgs e)
        {
            if (e.CommandName != "PostReceipt")
            {
                return;
            }

            int receiptId;

            if (!int.TryParse(
                e.CommandArgument.ToString(),
                out receiptId))
            {
                ShowError(
                    "رقم مستند الاستلام غير صحيح.");

                return;
            }

            try
            {
                string postedBy = GetAuthorizedUserName();

                _purchaseReceiptService.Post(
                    receiptId,
                    postedBy);

                LoadPurchaseReceipts();

                ShowSuccess(
                    "تم ترحيل الاستلام بنجاح، " +
                    "وتمت زيادة رصيد المستودع.");
            }
            catch (Exception exception)
            {
                LoadPurchaseReceipts();
                ShowError(exception.Message);
            }
        }

        private string GetAuthorizedUserName()
        {
            if (User == null ||
                User.Identity == null ||
                !User.Identity.IsAuthenticated)
            {
                throw new System.Web.HttpException(
                    401,
                    "يجب تسجيل الدخول لتنفيذ العملية.");
            }

            if (!User.IsInRole("Admin") &&
                !User.IsInRole("WarehouseManager"))
            {
                throw new System.Web.HttpException(
                    403,
                    "ليس لديك صلاحية تنفيذ العملية.");
            }

            string userName = User.Identity.Name;

            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new InvalidOperationException(
                    "تعذر تحديد المستخدم الحالي.");
            }

            return userName.Trim();
        }

        private void ShowSuccess(
            string message)
        {
            lblMessage.Text = message;

            lblMessage.CssClass =
                "alert alert-success d-block";

            lblMessage.Visible = true;
        }

        private void ShowError(
            string message)
        {
            lblMessage.Text = message;

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
    }
}