namespace NexusERP.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPurchaseReturnToStockMovement : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StockMovements", "PurchaseReturnId", c => c.Int());
            CreateIndex("dbo.StockMovements", "PurchaseReturnId");
            AddForeignKey("dbo.StockMovements", "PurchaseReturnId", "dbo.PurchaseReturns", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StockMovements", "PurchaseReturnId", "dbo.PurchaseReturns");
            DropIndex("dbo.StockMovements", new[] { "PurchaseReturnId" });
            DropColumn("dbo.StockMovements", "PurchaseReturnId");
        }
    }
}
