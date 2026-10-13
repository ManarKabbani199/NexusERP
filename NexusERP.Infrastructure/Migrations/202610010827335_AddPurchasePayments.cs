namespace NexusERP.Infrastructure.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddPurchasePayments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PurchasePayments",
                c => new
                {
                    Id = c.Int(
                        nullable: false,
                        identity: true),

                    PaymentNumber =
                        c.String(
                            nullable: false,
                            maxLength: 50),

                    PaymentDate =
                        c.DateTime(nullable: false),

                    PurchaseInvoiceId =
                        c.Int(nullable: false),

                    Amount =
                        c.Decimal(
                            nullable: false,
                            precision: 18,
                            scale: 2),

                    PaymentMethod =
                        c.Int(nullable: false),

                    ReferenceNumber =
                        c.String(maxLength: 100),

                    Notes =
                        c.String(maxLength: 500),

                    IsCancelled =
                        c.Boolean(nullable: false),

                    CancelledAt =
                        c.DateTime(),

                    CancelledBy =
                        c.String(maxLength: 100),

                    CancellationReason =
                        c.String(maxLength: 500),

                    IsActive =
                        c.Boolean(nullable: false),

                    IsDeleted =
                        c.Boolean(nullable: false),

                    CreatedAt =
                        c.DateTime(nullable: false),

                    CreatedBy =
                        c.String(),

                    UpdatedAt =
                        c.DateTime(),

                    UpdatedBy =
                        c.String(),

                    RowVersion =
                        c.Binary()
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey(
                    "dbo.PurchaseInvoices",
                    t => t.PurchaseInvoiceId)
                .Index(t => t.PurchaseInvoiceId);

            CreateIndex(
                "dbo.PurchasePayments",
                "PaymentNumber",
                unique: true,
                name:
                    "IX_PurchasePayment_PaymentNumber");
        }

        public override void Down()
        {
            DropForeignKey(
                "dbo.PurchasePayments",
                "PurchaseInvoiceId",
                "dbo.PurchaseInvoices");

            DropIndex(
                "dbo.PurchasePayments",
                "IX_PurchasePayment_PaymentNumber");

            DropIndex(
                "dbo.PurchasePayments",
                new[] { "PurchaseInvoiceId" });

            DropTable(
                "dbo.PurchasePayments");
        }
    }
}