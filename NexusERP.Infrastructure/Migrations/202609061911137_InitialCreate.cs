namespace NexusERP.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 20),
                        NameAr = c.String(nullable: false, maxLength: 150),
                        NameEn = c.String(nullable: false, maxLength: 150),
                        AddressAr = c.String(maxLength: 250),
                        AddressEn = c.String(maxLength: 250),
                        Phone = c.String(maxLength: 30),
                        CompanyId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedAt = c.DateTime(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 20),
                        NameAr = c.String(nullable: false, maxLength: 150),
                        NameEn = c.String(nullable: false, maxLength: 150),
                        CommercialRegistrationNumber = c.String(maxLength: 50),
                        TaxNumber = c.String(maxLength: 50),
                        AddressAr = c.String(maxLength: 250),
                        AddressEn = c.String(maxLength: 250),
                        Phone = c.String(maxLength: 30),
                        Email = c.String(maxLength: 150),
                        Website = c.String(maxLength: 200),
                        LogoPath = c.String(maxLength: 250),
                        BaseCurrencyCode = c.String(nullable: false, maxLength: 3),
                        DefaultLanguage = c.String(nullable: false, maxLength: 10),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedAt = c.DateTime(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 20),
                        NameAr = c.String(nullable: false, maxLength: 150),
                        NameEn = c.String(nullable: false, maxLength: 150),
                        DescriptionAr = c.String(maxLength: 500),
                        DescriptionEn = c.String(maxLength: 500),
                        BranchId = c.Int(nullable: false),
                        ParentDepartmentId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedAt = c.DateTime(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branches", t => t.BranchId)
                .ForeignKey("dbo.Departments", t => t.ParentDepartmentId)
                .Index(t => t.BranchId)
                .Index(t => t.ParentDepartmentId);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeNumber = c.String(nullable: false, maxLength: 30),
                        FirstNameAr = c.String(nullable: false, maxLength: 100),
                        LastNameAr = c.String(nullable: false, maxLength: 100),
                        FirstNameEn = c.String(nullable: false, maxLength: 100),
                        LastNameEn = c.String(nullable: false, maxLength: 100),
                        CivilId = c.String(maxLength: 20),
                        DateOfBirth = c.DateTime(),
                        Gender = c.Int(nullable: false),
                        Nationality = c.String(maxLength: 100),
                        Phone = c.String(maxLength: 30),
                        Email = c.String(maxLength: 150),
                        HireDate = c.DateTime(nullable: false),
                        TerminationDate = c.DateTime(),
                        EmploymentType = c.Int(nullable: false),
                        EmploymentStatus = c.Int(nullable: false),
                        BasicSalary = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BranchId = c.Int(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                        PositionId = c.Int(nullable: false),
                        ManagerEmployeeId = c.Int(),
                        IdentityUserId = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedAt = c.DateTime(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(),
                        Manager_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branches", t => t.BranchId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId)
                .ForeignKey("dbo.Employees", t => t.Manager_Id)
                .ForeignKey("dbo.Positions", t => t.PositionId)
                .Index(t => t.BranchId)
                .Index(t => t.DepartmentId)
                .Index(t => t.PositionId)
                .Index(t => t.Manager_Id);
            
            CreateTable(
                "dbo.Positions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 20),
                        TitleAr = c.String(nullable: false, maxLength: 150),
                        TitleEn = c.String(nullable: false, maxLength: 150),
                        DescriptionAr = c.String(maxLength: 1000),
                        DescriptionEn = c.String(maxLength: 1000),
                        JobLevel = c.String(maxLength: 50),
                        IsManagerial = c.Boolean(nullable: false),
                        ApprovedHeadCount = c.Int(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedAt = c.DateTime(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentId)
                .Index(t => t.DepartmentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "PositionId", "dbo.Positions");
            DropForeignKey("dbo.Positions", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Employees", "Manager_Id", "dbo.Employees");
            DropForeignKey("dbo.Employees", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Employees", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.Departments", "ParentDepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Departments", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.Branches", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Positions", new[] { "DepartmentId" });
            DropIndex("dbo.Employees", new[] { "Manager_Id" });
            DropIndex("dbo.Employees", new[] { "PositionId" });
            DropIndex("dbo.Employees", new[] { "DepartmentId" });
            DropIndex("dbo.Employees", new[] { "BranchId" });
            DropIndex("dbo.Departments", new[] { "ParentDepartmentId" });
            DropIndex("dbo.Departments", new[] { "BranchId" });
            DropIndex("dbo.Branches", new[] { "CompanyId" });
            DropTable("dbo.Positions");
            DropTable("dbo.Employees");
            DropTable("dbo.Departments");
            DropTable("dbo.Companies");
            DropTable("dbo.Branches");
        }
    }
}
