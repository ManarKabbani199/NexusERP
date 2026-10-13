namespace NexusERP.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStockMovements : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StockMovements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MovementType = c.Int(nullable: false),
                        WarehouseId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MovementDate = c.DateTime(nullable: false),
                        ReferenceNumber = c.String(maxLength: 50),
                        Notes = c.String(maxLength: 500),
                        DestinationWarehouseId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedAt = c.DateTime(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Warehouses", t => t.DestinationWarehouseId)
                .ForeignKey("dbo.Items", t => t.ItemId)
                .ForeignKey("dbo.Warehouses", t => t.WarehouseId)
                .Index(t => t.WarehouseId)
                .Index(t => t.ItemId)
                .Index(t => t.DestinationWarehouseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StockMovements", "WarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.StockMovements", "ItemId", "dbo.Items");
            DropForeignKey("dbo.StockMovements", "DestinationWarehouseId", "dbo.Warehouses");
            DropIndex("dbo.StockMovements", new[] { "DestinationWarehouseId" });
            DropIndex("dbo.StockMovements", new[] { "ItemId" });
            DropIndex("dbo.StockMovements", new[] { "WarehouseId" });
            DropTable("dbo.StockMovements");
        }
    }
}
