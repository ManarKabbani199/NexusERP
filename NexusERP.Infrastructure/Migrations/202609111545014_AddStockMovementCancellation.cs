namespace NexusERP.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStockMovementCancellation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StockMovements", "IsCancelled", c => c.Boolean(nullable: false));
            AddColumn("dbo.StockMovements", "CancelledAt", c => c.DateTime());
            AddColumn("dbo.StockMovements", "CancelledBy", c => c.String(maxLength: 100));
            AddColumn("dbo.StockMovements", "CancellationReason", c => c.String(maxLength: 500));
            AddColumn("dbo.StockMovements", "OriginalMovementId", c => c.Int());
            CreateIndex("dbo.StockMovements", "OriginalMovementId");
            AddForeignKey("dbo.StockMovements", "OriginalMovementId", "dbo.StockMovements", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StockMovements", "OriginalMovementId", "dbo.StockMovements");
            DropIndex("dbo.StockMovements", new[] { "OriginalMovementId" });
            DropColumn("dbo.StockMovements", "OriginalMovementId");
            DropColumn("dbo.StockMovements", "CancellationReason");
            DropColumn("dbo.StockMovements", "CancelledBy");
            DropColumn("dbo.StockMovements", "CancelledAt");
            DropColumn("dbo.StockMovements", "IsCancelled");
        }
    }
}
