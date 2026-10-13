using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NexusERP.Web.Models;

namespace NexusERP.Web.Security
{
    public static class IdentitySeeder
    {
        public static void SeedRoles()
        {
            using (var context = new ApplicationDbContext())
            using (var roleManager =
                new RoleManager<IdentityRole>(
                    new RoleStore<IdentityRole>(context)))
            {
                string[] roles =
                {
                    "Admin",
                    //إدارة المشتريات.
                    "PurchasingManager",
                    //إدارة المخزون
                    "WarehouseManager",
                    //الفواتير والدفعات
                    "Accountant",
                    "Employee"
                };

                foreach (string roleName in roles)
                {
                    if (roleManager.RoleExists(roleName))
                    {
                        continue;
                    }

                    IdentityResult result =
                        roleManager.Create(
                            new IdentityRole(roleName));

                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException(
                            "تعذر إنشاء الدور "
                            + roleName
                            + ": "
                            + string.Join("; ", result.Errors));
                    }
                }
            }
        }
    }
}