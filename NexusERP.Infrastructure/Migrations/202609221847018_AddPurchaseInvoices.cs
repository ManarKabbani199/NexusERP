namespace NexusERP.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPurchaseInvoices : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PurchaseInvoiceItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PurchaseInvoiceId = c.Int(nullable: false),
                        PurchaseOrderItemId = c.Int(nullable: false),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TaxRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedAt = c.DateTime(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PurchaseInvoices", t => t.PurchaseInvoiceId)
                .ForeignKey("dbo.PurchaseOrderItems", t => t.PurchaseOrderItemId)
                .Index(t => t.PurchaseInvoiceId)
                .Index(t => t.PurchaseOrderItemId);
            
            CreateTable(
                "dbo.PurchaseInvoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InvoiceNumber = c.String(nullable: false, maxLength: 30),
                        InvoiceDate = c.DateTime(nullable: false),
                        DueDate = c.DateTime(),
                        SupplierId = c.Int(nullable: false),
                        PurchaseOrderId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Notes = c.String(maxLength: 500),
                        PostedAt = c.DateTime(),
                        PostedBy = c.String(maxLength: 100),
                        CancelledAt = c.DateTime(),
                        CancelledBy = c.String(maxLength: 100),
                        CancellationReason = c.String(maxLength: 500),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedAt = c.DateTime(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PurchaseOrders", t => t.PurchaseOrderId)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId)
                .Index(t => t.SupplierId)
                .Index(t => t.PurchaseOrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchaseInvoiceItems", "PurchaseOrderItemId", "dbo.PurchaseOrderItems");
            DropForeignKey("dbo.PurchaseInvoiceItems", "PurchaseInvoiceId", "dbo.PurchaseInvoices");
            DropForeignKey("dbo.PurchaseInvoices", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.PurchaseInvoices", "PurchaseOrderId", "dbo.PurchaseOrders");
            DropIndex("dbo.PurchaseInvoices", new[] { "PurchaseOrderId" });
            DropIndex("dbo.PurchaseInvoices", new[] { "SupplierId" });
            DropIndex("dbo.PurchaseInvoiceItems", new[] { "PurchaseOrderItemId" });
            DropIndex("dbo.PurchaseInvoiceItems", new[] { "PurchaseInvoiceId" });
            DropTable("dbo.PurchaseInvoices");
            DropTable("dbo.PurchaseInvoiceItems");
        }
    }
}
