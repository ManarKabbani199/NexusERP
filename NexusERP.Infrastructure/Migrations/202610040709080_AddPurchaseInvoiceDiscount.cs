namespace NexusERP.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPurchaseInvoiceDiscount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseInvoices", "DiscountAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PurchaseInvoices", "DiscountAmount");
        }
    }
}
