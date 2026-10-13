namespace NexusERP.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSuppliers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 20),
                        NameAr = c.String(nullable: false, maxLength: 150),
                        NameEn = c.String(nullable: false, maxLength: 150),
                        ContactPerson = c.String(maxLength: 150),
                        Phone = c.String(maxLength: 30),
                        Email = c.String(maxLength: 150),
                        TaxNumber = c.String(maxLength: 50),
                        AddressAr = c.String(maxLength: 300),
                        AddressEn = c.String(maxLength: 300),
                        PaymentTermsDays = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedAt = c.DateTime(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Suppliers");
        }
    }
}
