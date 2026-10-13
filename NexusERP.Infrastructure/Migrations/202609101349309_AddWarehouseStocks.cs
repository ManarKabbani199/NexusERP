namespace NexusERP.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWarehouseStocks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WarehouseStocks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WarehouseId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedAt = c.DateTime(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.ItemId)
                .ForeignKey("dbo.Warehouses", t => t.WarehouseId)
                .Index(t => new { t.WarehouseId, t.ItemId }, unique: true, name: "IX_WarehouseStock_Warehouse_Item");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WarehouseStocks", "WarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.WarehouseStocks", "ItemId", "dbo.Items");
            DropIndex("dbo.WarehouseStocks", "IX_WarehouseStock_Warehouse_Item");
            DropTable("dbo.WarehouseStocks");
        }
    }
}
