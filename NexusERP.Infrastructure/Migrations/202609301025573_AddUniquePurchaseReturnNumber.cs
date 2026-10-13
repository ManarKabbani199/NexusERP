namespace NexusERP.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUniquePurchaseReturnNumber : DbMigration
    {
        public override void Up()
        {
            CreateIndex(
                "dbo.PurchaseReturns",
                "ReturnNumber",
                unique: true,
                name: "IX_PurchaseReturn_ReturnNumber");
        }
        public override void Down()
        {
            DropIndex(
                "dbo.PurchaseReturns",
                "IX_PurchaseReturn_ReturnNumber");
        }
    }
}
