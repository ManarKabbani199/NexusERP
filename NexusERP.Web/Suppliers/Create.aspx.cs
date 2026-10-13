using System;
using System.Linq;
using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Entities;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;

namespace NexusERP.Web.Suppliers
{
    public partial class Create : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private ISupplierService _supplierService;

        protected void Page_Init(object sender, EventArgs e)
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);
            _supplierService = new SupplierService(_unitOfWork);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            int paymentTermsDays;
            if (!int.TryParse(txtPaymentTermsDays.Text, out paymentTermsDays)
                || paymentTermsDays < 0
                || paymentTermsDays > 3650)
            {
                lblError.Text = "مدة السداد يجب أن تكون بين 0 و3650 يومًا.";
                return;
            }

            string code = txtCode.Text.Trim();

            if (_supplierService.GetAll().Any(s => s.Code == code))
            {
                lblError.Text = "رمز المورد مستخدم بالفعل.";
                return;
            }

            var supplier = new Supplier
            {
                Code = code,
                NameAr = txtNameAr.Text.Trim(),
                NameEn = txtNameEn.Text.Trim(),
                ContactPerson = txtContactPerson.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                TaxNumber = txtTaxNumber.Text.Trim(),
                AddressAr = txtAddressAr.Text.Trim(),
                AddressEn = txtAddressEn.Text.Trim(),
                PaymentTermsDays = paymentTermsDays,
                IsActive = true
            };

            try
            {
                _supplierService.Add(supplier);
                Response.Redirect("~/Suppliers/Index.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception)
            {
                lblError.Text = "تعذّر حفظ المورد. تحققي من البيانات وحاولي مجددًا.";
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (_unitOfWork != null)
                _unitOfWork.Dispose();
            else if (_context != null)
                _context.Dispose();
        }
    }
}