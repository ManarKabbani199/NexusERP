# NexusERP

An ERP portfolio project built with ASP.NET Web Forms and C# to manage organizational structure, inventory, purchasing, supplier invoices, and payments.

## Project Status

The core modules are implemented. UI improvements, Arabic/English localization, and role-based access controls are being refined.

## Features

### Organization Management
- Companies, branches, departments, and positions.
- Employee records and reporting relationships.

### Inventory Management
- Warehouses, item categories, and items.
- Stock balances per warehouse.
- Opening balances, receipts, issues, transfers, and adjustments.
- Stock validation to prevent negative balances.
- Movement cancellation through reversal transactions.

### Purchasing
- Supplier management.
- Purchase orders with approval and cancellation workflows.
- Partial and full purchase receipts.
- Receipt posting and cancellation with stock updates.

### Invoices and Payments
- Purchase invoices with discounts and taxes.
- Invoice posting and cancellation validation.
- Supplier payments and outstanding balance validation.
- Payment cancellation.

### Authentication and Roles
- ASP.NET Identity authentication.
- Roles: Admin, PurchasingManager, WarehouseManager, Accountant, and Employee.

### Reports and User Interface
- Crystal Reports for warehouse stock, stock movements, and supplier payments.
- PDF report export.
- DevExpress grids on selected pages.
- Search, filtering, and pagination.
- AJAX partial updates using UpdatePanel.
- JavaScript and jQuery interactions.

## Technology Stack

- C# and .NET Framework 4.8
- ASP.NET Web Forms and Master Pages
- Entity Framework 6 and Code First Migrations
- SQL Server
- ASP.NET Identity and OWIN
- Crystal Reports
- DevExpress ASP.NET
- HTML, CSS, JavaScript, jQuery, and AJAX

## Solution Structure

| Project | Responsibility |
| --- | --- |
| NexusERP.Domain | Entities, enums, and domain models |
| NexusERP.Application | Service interfaces and business logic |
| NexusERP.Infrastructure | EF6 context, repositories, Unit of Work, and migrations |
| NexusERP.Web | Web Forms pages, authentication, and UI |
| NexusERP.Reporting | Crystal Reports assets |
| NexusERP.Tests | Test project |

## Business Rules

- Stock movements validate available quantities.
- Transfers update both source and destination warehouses.
- Posting a purchase receipt updates stock and purchase order status.
- Receipt cancellation validates related transactions before reversing stock.
- Invoice cancellation is blocked when active payments exist.
- Payments cannot exceed the remaining invoice balance.

## Prerequisites

- Windows with Visual Studio 2022.
- ASP.NET and web development workload.
- .NET Framework 4.8 Developer Pack.
- SQL Server or SQL Server Express.
- Compatible DevExpress ASP.NET components.
- Compatible SAP Crystal Reports developer tools and runtime.

DevExpress and Crystal Reports are third-party dependencies. Obtain the required components and licenses from their vendors. Their installers are not included in this repository.

## Local Setup

1. Clone the repository and open `NexusERP.sln`.
2. Restore NuGet packages.
3. Configure the SQL Server connection strings in `NexusERP.Web/Web.config` for your local environment.
4. Open Package Manager Console and apply the Entity Framework migrations.

For the application database:

```powershell
Update-Database -ProjectName NexusERP.Infrastructure -StartupProjectName NexusERP.Web
```

If the Identity migrations are maintained separately in the Web project, also run:

```powershell
Update-Database -ProjectName NexusERP.Web -StartupProjectName NexusERP.Web
```

If EF detects multiple migration configurations, specify the appropriate configuration using `-ConfigurationTypeName`.

5. Set `NexusERP.Web` as the startup project.
6. Build the solution and run it using IIS Express.
7. Configure local users and roles according to the application's initialization logic.

## Security

- Do not commit real database passwords, publishing credentials, or production secrets.
- Use sample data when demonstrating the application.
- Review server-side authorization before production deployment.

## Screenshots

Screenshots will be added to demonstrate the main modules and reports.

## Purpose

This project demonstrates ERP workflows, layered application design, inventory validation, purchasing processes, and reporting with ASP.NET Web Forms.