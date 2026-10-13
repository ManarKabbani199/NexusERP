namespace NexusERP.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPurchaseOrderApprovalAudit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseOrders", "ApprovedBy", c => c.String(maxLength: 256));
            AddColumn("dbo.PurchaseOrders", "ApprovedAt", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PurchaseOrders", "ApprovedAt");
            DropColumn("dbo.PurchaseOrders", "ApprovedBy");
        }
    }
}
