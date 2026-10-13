using NexusERP.Application.Interfaces;
using NexusERP.Domain.Entities;
using NexusERP.Infrastructure.Data;

namespace NexusERP.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private bool _disposed;

        private IGenericRepository<Supplier> _suppliers;

        private IGenericRepository<PurchaseOrder> _purchaseOrders;

        private IGenericRepository<PurchaseOrderItem> _purchaseOrderItems;

        private IGenericRepository<PurchaseReceipt>
        _purchaseReceipts;

        private IGenericRepository<PurchaseReceiptItem>
            _purchaseReceiptItems;

        private IGenericRepository<PurchasePayment>
    _purchasePayments;

        public IGenericRepository<PurchaseInvoice>
    PurchaseInvoices
        { get; private set; }

        public IGenericRepository<PurchaseInvoiceItem>
            PurchaseInvoiceItems
        { get; private set; }


        private IGenericRepository<PurchaseReturn>
    _purchaseReturns;

        private IGenericRepository<PurchaseReturnItem>
            _purchaseReturnItems;

        public IGenericRepository<PurchaseReturn>
    PurchaseReturns
        {
            get
            {
                if (_purchaseReturns == null)
                {
                    _purchaseReturns =
                        new GenericRepository<PurchaseReturn>(
                            _context);
                }

                return _purchaseReturns;
            }
        }

        public IGenericRepository<PurchaseReturnItem>
            PurchaseReturnItems
        {
            get
            {
                if (_purchaseReturnItems == null)
                {
                    _purchaseReturnItems =
                        new GenericRepository<PurchaseReturnItem>(
                            _context);
                }

                return _purchaseReturnItems;
            }
        }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Companies =
                new GenericRepository<Company>(_context);

            Branches =
                new GenericRepository<Branch>(_context);

            Departments =
                new GenericRepository<Department>(_context);

            Positions =
                new GenericRepository<Position>(_context);

            Employees =
                new GenericRepository<Employee>(_context);

            Warehouses =
                new GenericRepository<Warehouse>(_context);

            ItemCategories =
                new GenericRepository<ItemCategory>(_context);

            Items =
                 new GenericRepository<Item>(_context);

            WarehouseStocks = 
                new GenericRepository<WarehouseStock>(_context);

            StockMovements =
                new GenericRepository<StockMovement>(_context);

            PurchaseInvoices =
    new GenericRepository<PurchaseInvoice>(
        context);

            PurchaseInvoiceItems =
                new GenericRepository<PurchaseInvoiceItem>(
                    context);

        }
        public IGenericRepository<PurchaseOrder> PurchaseOrders
             {
            get
            {
                return _purchaseOrders ?? (_purchaseOrders =
                    new GenericRepository<PurchaseOrder>(_context));
            }
          }

        public IGenericRepository<PurchaseOrderItem> PurchaseOrderItems
        {
            get
            {
                return _purchaseOrderItems ?? (_purchaseOrderItems =
                    new GenericRepository<PurchaseOrderItem>(_context));
            }
        }


        public IGenericRepository<StockMovement> StockMovements
        {
            get;
        }

        public IGenericRepository<Company> Companies
        {
            get;
        }

        public IGenericRepository<Branch> Branches
        {
            get;
        }

        public IGenericRepository<Department> Departments
        {
            get;
        }

        public IGenericRepository<Position> Positions
        {
            get;
        }

        public IGenericRepository<Employee> Employees
        {
            get;
        }

        public IGenericRepository<Warehouse> Warehouses
        {
            get;
        }

        public IGenericRepository<ItemCategory> ItemCategories
        {
            get;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public IGenericRepository<Item> Items
        {
            get;
        }

        public IGenericRepository<WarehouseStock> WarehouseStocks
        {
            get;
        }


        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _context.Dispose();
            _disposed = true;
        }

        public IGenericRepository<Supplier> Suppliers
        {
            get
            {
                if (_suppliers == null)
                {
                    _suppliers = new GenericRepository<Supplier>(_context);
                }

                return _suppliers;
            }
        }

        public IGenericRepository<PurchaseReceipt>
    PurchaseReceipts
        {
            get
            {
                return _purchaseReceipts
                    ?? (_purchaseReceipts =
                        new GenericRepository<PurchaseReceipt>(
                            _context));
            }
        }

        public IGenericRepository<PurchaseReceiptItem>
            PurchaseReceiptItems
        {
            get
            {
                return _purchaseReceiptItems
                    ?? (_purchaseReceiptItems =
                        new GenericRepository<PurchaseReceiptItem>(
                            _context));
            }
        }


        public IGenericRepository<PurchasePayment>
    PurchasePayments
        {
            get
            {
                if (_purchasePayments == null)
                {
                    _purchasePayments =
                        new GenericRepository<PurchasePayment>(
                            _context);
                }

                return _purchasePayments;
            }
        }
    }
}