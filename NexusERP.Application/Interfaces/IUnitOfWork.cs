using System;
using NexusERP.Domain.Entities;

namespace NexusERP.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Company> Companies { get; }

        IGenericRepository<Branch> Branches { get; }

        IGenericRepository<Department> Departments { get; }

        IGenericRepository<Position> Positions { get; }

        IGenericRepository<Employee> Employees { get; }

        IGenericRepository<Warehouse> Warehouses { get; }

        IGenericRepository<PurchasePayment> PurchasePayments
        {
            get;
        }

        IGenericRepository<PurchaseReturn> PurchaseReturns
        {
            get;
        }

        IGenericRepository<PurchaseReturnItem> PurchaseReturnItems
        {
            get;
        }

        IGenericRepository<ItemCategory> ItemCategories { get; }

        IGenericRepository<Item> Items { get; }

        IGenericRepository<WarehouseStock> WarehouseStocks { get; }

        IGenericRepository<StockMovement> StockMovements { get; }

        IGenericRepository<Supplier> Suppliers { get; }

        IGenericRepository<PurchaseOrder> PurchaseOrders { get; }
        IGenericRepository<PurchaseOrderItem> PurchaseOrderItems { get; }

        IGenericRepository<PurchaseReceipt>
       
        PurchaseReceipts
        { get; }

        IGenericRepository<PurchaseReceiptItem>
        PurchaseReceiptItems
        { get; }

        IGenericRepository<PurchaseInvoice>
    PurchaseInvoices
        { get; }

        IGenericRepository<PurchaseInvoiceItem>
            PurchaseInvoiceItems
        { get; }

        int SaveChanges();
    }
}