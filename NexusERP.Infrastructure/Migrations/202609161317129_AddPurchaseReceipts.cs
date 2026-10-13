namespace NexusERP.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPurchaseReceipts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PurchaseReceipts",
                c => new
                {
                    Id = c.Int(
                        nullable: false,
                        identity: true),

                    ReceiptNumber = c.String(
                        nullable: false,
                        maxLength: 30),

                    ReceiptDate =
                        c.DateTime(nullable: false),

                    PurchaseOrderId =
                        c.Int(nullable: false),

                    WarehouseId =
                        c.Int(nullable: false),

                    IsPosted =
                        c.Boolean(nullable: false),

                    PostedAt = c.DateTime(),

                    IsCancelled =
                        c.Boolean(nullable: false),

                    CancelledAt = c.DateTime(),

                    Notes = c.String(maxLength: 500),

                    IsActive =
                        c.Boolean(nullable: false),

                    IsDeleted =
                        c.Boolean(nullable: false),

                    CreatedAt =
                        c.DateTime(nullable: false),

                    CreatedBy = c.String(),

                    UpdatedAt = c.DateTime(),

                    UpdatedBy = c.String(),

                    RowVersion = c.Binary(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey(
                    "dbo.PurchaseOrders",
                    t => t.PurchaseOrderId)
                .ForeignKey(
                    "dbo.Warehouses",
                    t => t.WarehouseId)
                .Index(t => t.PurchaseOrderId)
                .Index(t => t.WarehouseId);

            CreateTable(
                "dbo.PurchaseReceiptItems",
                c => new
                {
                    Id = c.Int(
                        nullable: false,
                        identity: true),

                    PurchaseReceiptId =
                        c.Int(nullable: false),

                    PurchaseOrderItemId =
                        c.Int(nullable: false),

                    ReceivedQuantity =
                        c.Decimal(
                            nullable: false,
                            precision: 18,
                            scale: 2),

                    IsActive =
                        c.Boolean(nullable: false),

                    IsDeleted =
                        c.Boolean(nullable: false),

                    CreatedAt =
                        c.DateTime(nullable: false),

                    CreatedBy = c.String(),

                    UpdatedAt = c.DateTime(),

                    UpdatedBy = c.String(),

                    RowVersion = c.Binary(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey(
                    "dbo.PurchaseOrderItems",
                    t => t.PurchaseOrderItemId)
                .ForeignKey(
                    "dbo.PurchaseReceipts",
                    t => t.PurchaseReceiptId)
                .Index(t => t.PurchaseReceiptId)
                .Index(t => t.PurchaseOrderItemId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.PurchaseReceiptItems", "PurchaseReceiptId", "dbo.PurchaseReceipts");
            DropForeignKey("dbo.PurchaseReceipts", "WarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.PurchaseReceipts", "PurchaseOrderId", "dbo.PurchaseOrders");
            DropForeignKey("dbo.PurchaseReceiptItems", "PurchaseOrderItemId", "dbo.PurchaseOrderItems");
            DropIndex("dbo.PurchaseReceipts", new[] { "WarehouseId" });
            DropIndex("dbo.PurchaseReceipts", new[] { "PurchaseOrderId" });
            DropIndex("dbo.PurchaseReceiptItems", new[] { "PurchaseOrderItemId" });
            DropIndex("dbo.PurchaseReceiptItems", new[] { "PurchaseReceiptId" });
            DropTable("dbo.PurchaseReceipts");
            DropTable("dbo.PurchaseReceiptItems");
        }
    }
}
