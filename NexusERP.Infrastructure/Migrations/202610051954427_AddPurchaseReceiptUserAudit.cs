namespace NexusERP.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPurchaseReceiptUserAudit : DbMigration
    {
        public override void Up()
        {
            Sql(@"
DECLARE @checkSql nvarchar(max) = N'';

SELECT @checkSql = @checkSql +
    N'IF EXISTS (
        SELECT 1 FROM '
    + QUOTENAME(s.name) + N'.' + QUOTENAME(t.name)
    + N' WHERE DATALENGTH(' + QUOTENAME(c.name) + N') > 512
    ) THROW 50001,
      N''توجد قيمة في CreatedBy أو UpdatedBy تتجاوز 256 حرفًا.'',
      1;'
FROM sys.columns AS c
INNER JOIN sys.tables AS t ON t.object_id = c.object_id
INNER JOIN sys.schemas AS s ON s.schema_id = t.schema_id
WHERE s.name = N'dbo'
  AND c.name IN (N'CreatedBy', N'UpdatedBy')
  AND c.system_type_id = 231;

EXEC sys.sp_executesql @checkSql;
");
            AddColumn("dbo.PurchaseReceipts", "PostedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.Branches", "CreatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.Branches", "UpdatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.Companies", "CreatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.Companies", "UpdatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.Departments", "CreatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.Departments", "UpdatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.Employees", "CreatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.Employees", "UpdatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.Positions", "CreatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.Positions", "UpdatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.Warehouses", "CreatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.Warehouses", "UpdatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.WarehouseStocks", "CreatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.WarehouseStocks", "UpdatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.Items", "CreatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.Items", "UpdatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.ItemCategories", "CreatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.ItemCategories", "UpdatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.PurchaseInvoiceItems", "CreatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.PurchaseInvoiceItems", "UpdatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.PurchaseInvoices", "CreatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.PurchaseInvoices", "UpdatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.PurchasePayments", "CreatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.PurchasePayments", "UpdatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.PurchaseOrders", "CreatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.PurchaseOrders", "UpdatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.PurchaseOrderItems", "CreatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.PurchaseOrderItems", "UpdatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.Suppliers", "CreatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.Suppliers", "UpdatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.PurchaseReceiptItems", "CreatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.PurchaseReceiptItems", "UpdatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.PurchaseReceipts", "CancelledBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.PurchaseReceipts", "CreatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.PurchaseReceipts", "UpdatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.PurchaseReturnItems", "CreatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.PurchaseReturnItems", "UpdatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.PurchaseReturns", "CreatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.PurchaseReturns", "UpdatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.StockMovements", "CancelledBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.StockMovements", "CreatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.StockMovements", "UpdatedBy", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StockMovements", "UpdatedBy", c => c.String());
            AlterColumn("dbo.StockMovements", "CreatedBy", c => c.String());
            AlterColumn("dbo.StockMovements", "CancelledBy", c => c.String(maxLength: 100));
            AlterColumn("dbo.PurchaseReturns", "UpdatedBy", c => c.String());
            AlterColumn("dbo.PurchaseReturns", "CreatedBy", c => c.String());
            AlterColumn("dbo.PurchaseReturnItems", "UpdatedBy", c => c.String());
            AlterColumn("dbo.PurchaseReturnItems", "CreatedBy", c => c.String());
            AlterColumn("dbo.PurchaseReceipts", "UpdatedBy", c => c.String());
            AlterColumn("dbo.PurchaseReceipts", "CreatedBy", c => c.String());
            AlterColumn("dbo.PurchaseReceipts", "CancelledBy", c => c.String(maxLength: 100));
            AlterColumn("dbo.PurchaseReceiptItems", "UpdatedBy", c => c.String());
            AlterColumn("dbo.PurchaseReceiptItems", "CreatedBy", c => c.String());
            AlterColumn("dbo.Suppliers", "UpdatedBy", c => c.String());
            AlterColumn("dbo.Suppliers", "CreatedBy", c => c.String());
            AlterColumn("dbo.PurchaseOrderItems", "UpdatedBy", c => c.String());
            AlterColumn("dbo.PurchaseOrderItems", "CreatedBy", c => c.String());
            AlterColumn("dbo.PurchaseOrders", "UpdatedBy", c => c.String());
            AlterColumn("dbo.PurchaseOrders", "CreatedBy", c => c.String());
            AlterColumn("dbo.PurchasePayments", "UpdatedBy", c => c.String());
            AlterColumn("dbo.PurchasePayments", "CreatedBy", c => c.String());
            AlterColumn("dbo.PurchaseInvoices", "UpdatedBy", c => c.String());
            AlterColumn("dbo.PurchaseInvoices", "CreatedBy", c => c.String());
            AlterColumn("dbo.PurchaseInvoiceItems", "UpdatedBy", c => c.String());
            AlterColumn("dbo.PurchaseInvoiceItems", "CreatedBy", c => c.String());
            AlterColumn("dbo.ItemCategories", "UpdatedBy", c => c.String());
            AlterColumn("dbo.ItemCategories", "CreatedBy", c => c.String());
            AlterColumn("dbo.Items", "UpdatedBy", c => c.String());
            AlterColumn("dbo.Items", "CreatedBy", c => c.String());
            AlterColumn("dbo.WarehouseStocks", "UpdatedBy", c => c.String());
            AlterColumn("dbo.WarehouseStocks", "CreatedBy", c => c.String());
            AlterColumn("dbo.Warehouses", "UpdatedBy", c => c.String());
            AlterColumn("dbo.Warehouses", "CreatedBy", c => c.String());
            AlterColumn("dbo.Positions", "UpdatedBy", c => c.String());
            AlterColumn("dbo.Positions", "CreatedBy", c => c.String());
            AlterColumn("dbo.Employees", "UpdatedBy", c => c.String());
            AlterColumn("dbo.Employees", "CreatedBy", c => c.String());
            AlterColumn("dbo.Departments", "UpdatedBy", c => c.String());
            AlterColumn("dbo.Departments", "CreatedBy", c => c.String());
            AlterColumn("dbo.Companies", "UpdatedBy", c => c.String());
            AlterColumn("dbo.Companies", "CreatedBy", c => c.String());
            AlterColumn("dbo.Branches", "UpdatedBy", c => c.String());
            AlterColumn("dbo.Branches", "CreatedBy", c => c.String());
            DropColumn("dbo.PurchaseReceipts", "PostedBy");
        }
    }
}
