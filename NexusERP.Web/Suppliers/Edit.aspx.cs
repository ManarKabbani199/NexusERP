using System;
using System.Linq;
using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;

namespace NexusERP.Web.Suppliers
{
    public partial class Edit : System.Web.UI.Page
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            int id;
            if (!int.TryParse(Request.QueryString["id"], out id) || id <= 0)
            {
                lblError.Text = "رقم المورد غير صحيح.";
                return;
            }

            var supplier = _supplierService.GetById(id);
            if (supplier == null)
            {
                lblError.Text = "المورد غير موجود.";
                return;
            }

            txtCode.Text = supplier.Code;
            txtNameAr.Text = supplier.NameAr;
            txtNameEn.Text = supplier.NameEn;
            txtContactPerson.Text = supplier.ContactPerson;
            txtPhone.Text = supplier.Phone;
            txtEmail.Text = supplier.Email;
            txtTaxNumber.Text = supplier.TaxNumber;
            txtAddressAr.Text = supplier.AddressAr;
            txtAddressEn.Text = supplier.AddressEn;
            txtPaymentTermsDays.Text = supplier.PaymentTermsDays.ToString();

            pnlForm.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            int id;
            int paymentTermsDays;

            if (!int.TryParse(Request.QueryString["id"], out id) || id <= 0)
            {
                lblError.Text = "رقم المورد غير صحيح.";
                return;
            }

            if (!int.TryParse(txtPaymentTermsDays.Text, out paymentTermsDays)
                || paymentTermsDays < 0
                || paymentTermsDays > 3650)
            {
                lblError.Text = "مدة السداد يجب أن تكون بين 0 و3650 يومًا.";
                return;
            }

            var supplier = _supplierService.GetById(id);
            if (supplier == null)
            {
                lblError.Text = "المورد غير موجود.";
                return;
            }

            string code = txtCode.Text.Trim();

            if (_supplierService.GetAll()
                .Any(s => s.Id != id
                    && string.Equals(s.Code, code,
                        StringComparison.OrdinalIgnoreCase)))
            {
                lblError.Text = "رمز المورد مستخدم لمورد آخر.";
                return;
            }

            supplier.Code = code;
            supplier.NameAr = txtNameAr.Text.Trim();
            supplier.NameEn = txtNameEn.Text.Trim();
            supplier.ContactPerson = txtContactPerson.Text.Trim();
            supplier.Phone = txtPhone.Text.Trim();
            supplier.Email = txtEmail.Text.Trim();
            supplier.TaxNumber = txtTaxNumber.Text.Trim();
            supplier.AddressAr = txtAddressAr.Text.Trim();
            supplier.AddressEn = txtAddressEn.Text.Trim();
            supplier.PaymentTermsDays = paymentTermsDays;

            try
            {
                _supplierService.Update(supplier);
                Response.Redirect("~/Suppliers/Index.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception)
            {
                lblError.Text = "تعذّر حفظ التعديلات. تحققي من البيانات وحاولي مجددًا.";
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