namespace NexusERP.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPurchaseReturns : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PurchaseReturnItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PurchaseReturnId = c.Int(nullable: false),
                        PurchaseReceiptItemId = c.Int(nullable: false),
                        ReturnedQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedAt = c.DateTime(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PurchaseReceiptItems", t => t.PurchaseReceiptItemId)
                .ForeignKey("dbo.PurchaseReturns", t => t.PurchaseReturnId)
                .Index(t => t.PurchaseReturnId)
                .Index(t => t.PurchaseReceiptItemId);
            
            CreateTable(
                "dbo.PurchaseReturns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReturnNumber = c.String(nullable: false, maxLength: 30),
                        ReturnDate = c.DateTime(nullable: false),
                        PurchaseReceiptId = c.Int(nullable: false),
                        WarehouseId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        PostedAt = c.DateTime(),
                        PostedBy = c.String(maxLength: 100),
                        CancelledAt = c.DateTime(),
                        CancelledBy = c.String(maxLength: 100),
                        CancellationReason = c.String(maxLength: 500),
                        Notes = c.String(maxLength: 500),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedAt = c.DateTime(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PurchaseReceipts", t => t.PurchaseReceiptId)
                .ForeignKey("dbo.Warehouses", t => t.WarehouseId)
                .Index(t => t.PurchaseReceiptId)
                .Index(t => t.WarehouseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchaseReturnItems", "PurchaseReturnId", "dbo.PurchaseReturns");
            DropForeignKey("dbo.PurchaseReturns", "WarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.PurchaseReturns", "PurchaseReceiptId", "dbo.PurchaseReceipts");
            DropForeignKey("dbo.PurchaseReturnItems", "PurchaseReceiptItemId", "dbo.PurchaseReceiptItems");
            DropIndex("dbo.PurchaseReturns", new[] { "WarehouseId" });
            DropIndex("dbo.PurchaseReturns", new[] { "PurchaseReceiptId" });
            DropIndex("dbo.PurchaseReturnItems", new[] { "PurchaseReceiptItemId" });
            DropIndex("dbo.PurchaseReturnItems", new[] { "PurchaseReturnId" });
            DropTable("dbo.PurchaseReturns");
            DropTable("dbo.PurchaseReturnItems");
        }
    }
}
