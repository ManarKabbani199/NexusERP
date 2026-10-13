using System;
using System.Web;
using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;

namespace NexusERP.Web.Suppliers
{
    public partial class Activate : System.Web.UI.Page
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
            if (!TryGetSupplierId(out id))
            {
                lblMessage.Text = "رقم المورد غير صحيح.";
                return;
            }

            var supplier = _supplierService.GetById(id);
            if (supplier == null)
            {
                lblMessage.Text = "المورد غير موجود.";
                return;
            }

            if (supplier.IsActive)
            {
                lblMessage.Text = "هذا المورد نشط بالفعل.";
                return;
            }

            lblSupplierName.Text = HttpUtility.HtmlEncode(supplier.NameAr);
            pnlConfirm.Visible = true;
        }

        protected void btnActivate_Click(object sender, EventArgs e)
        {
            int id;
            if (!TryGetSupplierId(out id))
            {
                lblMessage.Text = "رقم المورد غير صحيح.";
                pnlConfirm.Visible = false;
                return;
            }

            var supplier = _supplierService.GetById(id);
            if (supplier == null || supplier.IsActive)
            {
                lblMessage.Text = "المورد غير موجود أو نشط بالفعل.";
                pnlConfirm.Visible = false;
                return;
            }

            try
            {
                supplier.IsActive = true;
                _supplierService.Update(supplier);

                Response.Redirect("~/Suppliers/Index.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception)
            {
                lblMessage.Text = "تعذّرت إعادة تفعيل المورد. حاولي مجددًا.";
            }
        }

        private bool TryGetSupplierId(out int id)
        {
            return int.TryParse(Request.QueryString["id"], out id) && id > 0;
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