using System;
using System.Web;
using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;

namespace NexusERP.Web.Suppliers
{
    public partial class Details : System.Web.UI.Page
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

            lblCode.Text = HttpUtility.HtmlEncode(supplier.Code);
            lblNameAr.Text = HttpUtility.HtmlEncode(supplier.NameAr);
            lblNameEn.Text = HttpUtility.HtmlEncode(supplier.NameEn);
            lblContactPerson.Text = HttpUtility.HtmlEncode(supplier.ContactPerson);
            lblPhone.Text = HttpUtility.HtmlEncode(supplier.Phone);
            lblEmail.Text = HttpUtility.HtmlEncode(supplier.Email);
            lblTaxNumber.Text = HttpUtility.HtmlEncode(supplier.TaxNumber);
            lblAddressAr.Text = HttpUtility.HtmlEncode(supplier.AddressAr);
            lblAddressEn.Text = HttpUtility.HtmlEncode(supplier.AddressEn);
            lblPaymentTermsDays.Text = supplier.PaymentTermsDays + " يوم";
            pnlDetails.Visible = true;
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