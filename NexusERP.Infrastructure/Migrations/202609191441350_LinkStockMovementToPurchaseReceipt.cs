namespace NexusERP.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LinkStockMovementToPurchaseReceipt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StockMovements", "PurchaseReceiptId", c => c.Int());
            CreateIndex("dbo.StockMovements", "PurchaseReceiptId");
            AddForeignKey("dbo.StockMovements", "PurchaseReceiptId", "dbo.PurchaseReceipts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StockMovements", "PurchaseReceiptId", "dbo.PurchaseReceipts");
            DropIndex("dbo.StockMovements", new[] { "PurchaseReceiptId" });
            DropColumn("dbo.StockMovements", "PurchaseReceiptId");
        }
    }
}
