using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Enums;
using NexusERP.Domain.Enums.NexusERP.Domain.Enums;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;
using System.Linq;

namespace NexusERP.Web.PurchaseReturns
{
    public partial class Index : System.Web.UI.Page
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
                LoadPurchaseReturns();
            }
        }

        private void LoadPurchaseReturns()
        {
            var purchaseReturns =
                _purchaseReturnService
                    .GetAll()
                    .Select(purchaseReturn => new
                    {
                        purchaseReturn.Id,

                        purchaseReturn.ReturnNumber,

                        purchaseReturn.ReturnDate,

                        ReceiptNumber =
                            purchaseReturn.PurchaseReceipt != null
                                ? purchaseReturn
                                    .PurchaseReceipt
                                    .ReceiptNumber
                                : "-",

                        PurchaseOrderNumber =
                            purchaseReturn.PurchaseReceipt != null &&
                            purchaseReturn.PurchaseReceipt
                                .PurchaseOrder != null
                                ? purchaseReturn
                                    .PurchaseReceipt
                                    .PurchaseOrder
                                    .OrderNumber
                                : "-",

                        SupplierName =
                            purchaseReturn.PurchaseReceipt != null &&
                            purchaseReturn.PurchaseReceipt
                                .PurchaseOrder != null &&
                            purchaseReturn.PurchaseReceipt
                                .PurchaseOrder.Supplier != null
                                ? purchaseReturn
                                    .PurchaseReceipt
                                    .PurchaseOrder.Supplier
                                    .NameAr
                                : "-",

                        WarehouseName =
                            purchaseReturn.Warehouse != null
                                ? purchaseReturn
                                    .Warehouse.NameAr
                                : "-",

                        ItemsCount =
                            purchaseReturn.Items.Count(item =>
                                !item.IsDeleted),

                        TotalQuantity =
                            purchaseReturn.Items
                                .Where(item =>
                                    !item.IsDeleted)
                                .Select(item =>
                                    (decimal?)
                                        item.ReturnedQuantity)
                                .Sum() ?? 0,

                        StatusName =
                            purchaseReturn.Status ==
                                PurchaseReturnStatus.Draft
                                    ? "مسودة"
                                    : purchaseReturn.Status ==
                                        PurchaseReturnStatus.Posted
                                            ? "مُرحّل"
                                            : purchaseReturn.Status ==
                                                PurchaseReturnStatus.Cancelled
                                                    ? "ملغي"
                                                    : "غير معروف"
                    })
                    .ToList();

            gvPurchaseReturns.DataSource =
                purchaseReturns;

            gvPurchaseReturns.DataBind();
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