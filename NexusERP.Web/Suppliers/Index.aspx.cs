using System;
using System.Linq;
using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;

namespace NexusERP.Web.Suppliers
{
    public partial class Index : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private ISupplierService _supplierService;

        protected void Page_Init(object sender, EventArgs e)
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);

            _supplierService =
                new SupplierService(_unitOfWork);

            LoadSuppliers();
        }

        private void LoadSuppliers()
        {
            var suppliers =
                _supplierService.GetAll().ToList();

            var rows = suppliers.Select(supplier => new
            {
                supplier.Id,
                supplier.Code,
                supplier.NameAr,
                supplier.ContactPerson,
                supplier.Phone,
                supplier.Email,
                supplier.PaymentTermsDays,
                supplier.IsActive,

                StatusName = supplier.IsActive
                    ? "نشط"
                    : "معطّل"
            }).ToList();

            gvSuppliers.DataSource = rows;
            gvSuppliers.DataBind();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            // مسح تصفية الأعمدة والبحث العام.
            gvSuppliers.FilterExpression = string.Empty;
            gvSuppliers.SearchPanelFilter = string.Empty;

            // إزالة الفرز والتجميع.
            gvSuppliers.ClearSort();

            // العودة إلى الصفحة الأولى.
            gvSuppliers.PageIndex = 0;

            LoadSuppliers();
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (_unitOfWork != null)
            {
                _unitOfWork.Dispose();
            }
            else if (_context != null)
            {
                _context.Dispose();
            }
        }
    }
}