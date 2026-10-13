namespace NexusERP.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPurchaseReceiptCancellationDetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseReceipts", "CancelledBy", c => c.String(maxLength: 100));
            AddColumn("dbo.PurchaseReceipts", "CancellationReason", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PurchaseReceipts", "CancellationReason");
            DropColumn("dbo.PurchaseReceipts", "CancelledBy");
        }
    }
}
