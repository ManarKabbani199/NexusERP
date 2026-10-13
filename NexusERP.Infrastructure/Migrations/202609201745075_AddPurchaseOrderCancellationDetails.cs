namespace NexusERP.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPurchaseOrderCancellationDetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseOrders", "CancelledAt", c => c.DateTime());
            AddColumn("dbo.PurchaseOrders", "CancelledBy", c => c.String(maxLength: 100));
            AddColumn("dbo.PurchaseOrders", "CancellationReason", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PurchaseOrders", "CancellationReason");
            DropColumn("dbo.PurchaseOrders", "CancelledBy");
            DropColumn("dbo.PurchaseOrders", "CancelledAt");
        }
    }
}
