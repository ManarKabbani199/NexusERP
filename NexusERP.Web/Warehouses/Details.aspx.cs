using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;

namespace NexusERP.Web.Warehouses
{
    public partial class Details : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IWarehouseService _warehouseService;

        protected void Page_Init(object sender, EventArgs e)
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);
            _warehouseService = new WarehouseService(_unitOfWork);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadWarehouse();
            }
        }

        private void LoadWarehouse()
        {
            int warehouseId;

            if (!int.TryParse(Request.QueryString["id"], out warehouseId))
            {
                RedirectToIndex();
                return;
            }

            var warehouse = _warehouseService.GetById(warehouseId);

            if (warehouse == null)
            {
                RedirectToIndex();
                return;
            }

            lblCode.Text = warehouse.Code;
            lblNameAr.Text = warehouse.NameAr;
            lblNameEn.Text = warehouse.NameEn;

            lblBranch.Text = warehouse.Branch != null
                ? warehouse.Branch.NameAr
                : "غير محدد";

            lblAddressAr.Text = string.IsNullOrWhiteSpace(warehouse.AddressAr)
                ? "غير محدد"
                : warehouse.AddressAr;

            lblAddressEn.Text = string.IsNullOrWhiteSpace(warehouse.AddressEn)
                ? "غير محدد"
                : warehouse.AddressEn;

            lblPhone.Text = string.IsNullOrWhiteSpace(warehouse.Phone)
                ? "غير محدد"
                : warehouse.Phone;

            lblStatus.Text = warehouse.IsActive
                ? "نشط"
                : "غير نشط";

            lblStatus.CssClass = warehouse.IsActive
                ? "badge bg-success"
                : "badge bg-secondary";

            linkEdit.NavigateUrl =
                $"Edit.aspx?id={warehouse.Id}";

            linkDelete.NavigateUrl =
                $"Delete.aspx?id={warehouse.Id}";
        }

        private void RedirectToIndex()
        {
            Response.Redirect("Index.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (_unitOfWork != null)
            {
                _unitOfWork.Dispose();
            }
        }
    }
}