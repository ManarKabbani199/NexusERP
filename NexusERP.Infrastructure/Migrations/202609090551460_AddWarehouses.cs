namespace NexusERP.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWarehouses : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Warehouses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 20),
                        NameAr = c.String(nullable: false, maxLength: 150),
                        NameEn = c.String(nullable: false, maxLength: 150),
                        AddressAr = c.String(maxLength: 250),
                        AddressEn = c.String(maxLength: 250),
                        Phone = c.String(maxLength: 30),
                        IsActive = c.Boolean(nullable: false),
                        BranchId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedAt = c.DateTime(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branches", t => t.BranchId)
                .Index(t => t.BranchId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Warehouses", "BranchId", "dbo.Branches");
            DropIndex("dbo.Warehouses", new[] { "BranchId" });
            DropTable("dbo.Warehouses");
        }
    }
}
