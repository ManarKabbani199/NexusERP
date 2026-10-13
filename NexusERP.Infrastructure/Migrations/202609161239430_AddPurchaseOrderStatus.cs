namespace NexusERP.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPurchaseOrderStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseOrders", "Status", c => c.Int(nullable: false,
            defaultValue: 1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PurchaseOrders", "Status");
        }
    }
}
