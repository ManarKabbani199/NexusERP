using System.Data.Entity;
using NexusERP.Domain.Entities;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;


namespace NexusERP.Infrastructure.Data
{
     public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        : base("name=NexusERPConnection")
        {
        }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Branch> Branches { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Warehouse> Warehouses { get; set; }

        public DbSet<ItemCategory> ItemCategories { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<WarehouseStock> WarehouseStocks { get; set; }

        public DbSet<StockMovement> StockMovements { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }

        public DbSet<PurchaseReceipt> PurchaseReceipts
        {
            get;
            set;
        }

        public DbSet<PurchaseReceiptItem> PurchaseReceiptItems
        {
            get;
            set;
        }

        public DbSet<PurchaseInvoice> PurchaseInvoices
        { 
            get; 
            set; 
        }

        public DbSet<PurchaseInvoiceItem> PurchaseInvoiceItems
        { 
            get; 
            set; 
        }

        public DbSet<PurchaseReturn> PurchaseReturns
        {
            get;
            set;
        }

        public DbSet<PurchaseReturnItem> PurchaseReturnItems
        {
            get;
            set;
        }

        public DbSet<PurchasePayment> PurchasePayments
        {
            get;
            set;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Warehouse>()
                .HasRequired(warehouse => warehouse.Branch)
                .WithMany(branch => branch.Warehouses)
                .HasForeignKey(warehouse => warehouse.BranchId)
                .WillCascadeOnDelete(false);
            //من اجل عمليه الحذف
            modelBuilder.Entity<Item>()
                .HasRequired(item => item.ItemCategory)
                .WithMany(category => category.Items)
                .HasForeignKey(item => item.ItemCategoryId)
                .WillCascadeOnDelete(false);

            //من اجل الفهرسة منع اضافه لمستودع الرئيسي + طابعة مرتين في قاعدة البيانات
            modelBuilder.Entity<WarehouseStock>()
    .HasRequired(stock => stock.Warehouse)
    .WithMany(warehouse => warehouse.WarehouseStocks)
    .HasForeignKey(stock => stock.WarehouseId)
    .WillCascadeOnDelete(false);

            //قمنا بأضافه الفهرسمة من اجل 
            //الفهرس الفريد يعني أن النظام لن يسمح بوجود سجلين مثل:
            //المستودع الرئيسي + طابعة
            //  المستودع الرئيسي + طابعة

            modelBuilder.Entity<WarehouseStock>()
                .HasRequired(stock => stock.Item)
                .WithMany(item => item.WarehouseStocks)
                .HasForeignKey(stock => stock.ItemId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<WarehouseStock>()
                .Property(stock => stock.Quantity)
                .HasPrecision(18, 2);

            modelBuilder.Entity<WarehouseStock>()
    .Property(stock => stock.WarehouseId)
    .HasColumnAnnotation(
        IndexAnnotation.AnnotationName,
        new IndexAnnotation(
            new IndexAttribute(
                "IX_WarehouseStock_Warehouse_Item",
                1)
            {
                IsUnique = true
            }));

            modelBuilder.Entity<WarehouseStock>()
                .Property(stock => stock.ItemId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute(
                            "IX_WarehouseStock_Warehouse_Item",
                            2)
                        {
                            IsUnique = true
                        }));


            modelBuilder.Entity<StockMovement>()
    .HasRequired(movement => movement.Warehouse)
    .WithMany()
    .HasForeignKey(movement => movement.WarehouseId)
    .WillCascadeOnDelete(false);

            modelBuilder.Entity<StockMovement>()
                .HasOptional(movement => movement.DestinationWarehouse)
                .WithMany()
                .HasForeignKey(movement => movement.DestinationWarehouseId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<StockMovement>()
                .HasRequired(movement => movement.Item)
                .WithMany()
                .HasForeignKey(movement => movement.ItemId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<StockMovement>()
                .Property(movement => movement.Quantity)
                .HasPrecision(18, 2);


            //الحركة العكسيه لمستودع
            modelBuilder.Entity<StockMovement>()
    .HasOptional(movement => movement.OriginalMovement)
    .WithMany()
    .HasForeignKey(movement => movement.OriginalMovementId)
    .WillCascadeOnDelete(false);

            //من اجل Orders طلبيات
            modelBuilder.Entity<PurchaseOrder>()
    .HasRequired(order => order.Supplier)
    .WithMany()
    .HasForeignKey(order => order.SupplierId)
    .WillCascadeOnDelete(false);

            modelBuilder.Entity<PurchaseOrderItem>()
                .HasRequired(orderItem => orderItem.PurchaseOrder)
                .WithMany(order => order.Items)
                .HasForeignKey(orderItem => orderItem.PurchaseOrderId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PurchaseOrderItem>()
                .HasRequired(orderItem => orderItem.Item)
                .WithMany()
                .HasForeignKey(orderItem => orderItem.ItemId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PurchaseOrderItem>()
                .Property(orderItem => orderItem.Quantity)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PurchaseOrderItem>()
                .Property(orderItem => orderItem.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PurchaseReceipt>()
    .HasRequired(receipt =>
        receipt.PurchaseOrder)
    .WithMany()
    .HasForeignKey(receipt =>
        receipt.PurchaseOrderId)
    .WillCascadeOnDelete(false);

            modelBuilder.Entity<PurchaseReceipt>()
                .HasRequired(receipt =>
                    receipt.Warehouse)
                .WithMany()
                .HasForeignKey(receipt =>
                    receipt.WarehouseId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PurchaseReceiptItem>()
                .HasRequired(receiptItem =>
                    receiptItem.PurchaseReceipt)
                .WithMany(receipt =>
                    receipt.Items)
                .HasForeignKey(receiptItem =>
                    receiptItem.PurchaseReceiptId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PurchaseReceiptItem>()
                .HasRequired(receiptItem =>
                    receiptItem.PurchaseOrderItem)
                .WithMany()
                .HasForeignKey(receiptItem =>
                    receiptItem.PurchaseOrderItemId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PurchaseReceiptItem>()
                .Property(receiptItem =>
                    receiptItem.ReceivedQuantity)
                .HasPrecision(18, 2);

            modelBuilder.Entity<StockMovement>()
    .HasOptional(movement =>
        movement.PurchaseReceipt)
    .WithMany()
    .HasForeignKey(movement =>
        movement.PurchaseReceiptId)
    .WillCascadeOnDelete(false);

            modelBuilder.Entity<PurchaseInvoice>()
    .HasRequired(invoice =>
        invoice.Supplier)
    .WithMany()
    .HasForeignKey(invoice =>
        invoice.SupplierId)
    .WillCascadeOnDelete(false);

            modelBuilder.Entity<PurchaseInvoice>()
                .HasRequired(invoice =>
                    invoice.PurchaseOrder)
                .WithMany()
                .HasForeignKey(invoice =>
                    invoice.PurchaseOrderId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PurchaseInvoiceItem>()
                .HasRequired(invoiceItem =>
                    invoiceItem.PurchaseInvoice)
                .WithMany(invoice =>
                    invoice.Items)
                .HasForeignKey(invoiceItem =>
                    invoiceItem.PurchaseInvoiceId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PurchaseInvoiceItem>()
                .HasRequired(invoiceItem =>
                    invoiceItem.PurchaseOrderItem)
                .WithMany()
                .HasForeignKey(invoiceItem =>
                    invoiceItem.PurchaseOrderItemId)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<PurchaseInvoiceItem>()
    .Property(invoiceItem =>
        invoiceItem.Quantity)
    .HasPrecision(18, 2);

            modelBuilder.Entity<PurchaseInvoiceItem>()
                .Property(invoiceItem =>
                    invoiceItem.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PurchaseInvoiceItem>()
                .Property(invoiceItem =>
                    invoiceItem.TaxRate)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PurchaseReturn>()
    .Property(purchaseReturn =>
        purchaseReturn.ReturnNumber)
    .IsRequired()
    .HasMaxLength(30);

            modelBuilder.Entity<PurchaseReturn>()
                .Property(purchaseReturn =>
                    purchaseReturn.PostedBy)
                .HasMaxLength(100);

            modelBuilder.Entity<PurchaseReturn>()
                .Property(purchaseReturn =>
                    purchaseReturn.CancelledBy)
                .HasMaxLength(100);

            modelBuilder.Entity<PurchaseReturn>()
                .Property(purchaseReturn =>
                    purchaseReturn.CancellationReason)
                .HasMaxLength(500);

            modelBuilder.Entity<PurchaseReturn>()
                .Property(purchaseReturn =>
                    purchaseReturn.Notes)
                .HasMaxLength(500);

            //علاقة المرتجع بمستند الاستلام
            modelBuilder.Entity<PurchaseReturn>()
    .HasRequired(purchaseReturn =>
        purchaseReturn.PurchaseReceipt)
    .WithMany()
    .HasForeignKey(purchaseReturn =>
        purchaseReturn.PurchaseReceiptId)
    .WillCascadeOnDelete(false);


            //علاقة المرتجع بالمستودع
            modelBuilder.Entity<PurchaseReturn>()
    .HasRequired(purchaseReturn =>
        purchaseReturn.Warehouse)
    .WithMany()
    .HasForeignKey(purchaseReturn =>
        purchaseReturn.WarehouseId)
    .WillCascadeOnDelete(false);

            //علاقة بنود المرتجع برأس المرتجع
            modelBuilder.Entity<PurchaseReturnItem>()
    .HasRequired(returnItem =>
        returnItem.PurchaseReturn)
    .WithMany(purchaseReturn =>
        purchaseReturn.Items)
    .HasForeignKey(returnItem =>
        returnItem.PurchaseReturnId)
    .WillCascadeOnDelete(false);

            //علاقة بند المرتجع ببند الاستلام
            modelBuilder.Entity<PurchaseReturnItem>()
    .HasRequired(returnItem =>
        returnItem.PurchaseReceiptItem)
    .WithMany()
    .HasForeignKey(returnItem =>
        returnItem.PurchaseReceiptItemId)
    .WillCascadeOnDelete(false);

            modelBuilder.Entity<PurchaseReturnItem>()
    .Property(returnItem =>
        returnItem.ReturnedQuantity)
    .HasPrecision(18, 2);

            modelBuilder.Entity<PurchaseReturnItem>()
                .Property(returnItem =>
                    returnItem.UnitPrice)
                .HasPrecision(18, 2);



            modelBuilder.Entity<StockMovement>()
    .HasOptional(movement =>
        movement.PurchaseReturn)
    .WithMany()
    .HasForeignKey(movement =>
        movement.PurchaseReturnId)
    .WillCascadeOnDelete(false);


            modelBuilder.Entity<PurchasePayment>()
    .HasRequired(payment =>
        payment.PurchaseInvoice)
    .WithMany(invoice =>
        invoice.Payments)
    .HasForeignKey(payment =>
        payment.PurchaseInvoiceId)
    .WillCascadeOnDelete(false);


            modelBuilder.Entity<PurchasePayment>()
    .Property(payment =>
        payment.PaymentNumber)
    .IsRequired()
    .HasMaxLength(50);

            modelBuilder.Entity<PurchasePayment>()
                .Property(payment =>
                    payment.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PurchasePayment>()
                .Property(payment =>
                    payment.ReferenceNumber)
                .HasMaxLength(100);

            modelBuilder.Entity<PurchasePayment>()
                .Property(payment =>
                    payment.Notes)
                .HasMaxLength(500);

            modelBuilder.Entity<PurchasePayment>()
                .Property(payment =>
                    payment.CancelledBy)
                .HasMaxLength(100);

            modelBuilder.Entity<PurchasePayment>()
                .Property(payment =>
                    payment.CancellationReason)
                .HasMaxLength(500);

            // الموظف يمكن أن يوجد دون حساب تسجيل دخول.
            // عند ربطه بحساب، يجب أن يكون الحساب موجودًا.
         




        }
    }
}