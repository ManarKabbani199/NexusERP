using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;

namespace NexusERP.Web.WarehouseStocks
{
    public partial class Index : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IWarehouseStockService _warehouseStockService;

        protected void Page_Init(object sender, EventArgs e)
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);

            _warehouseStockService =
                new WarehouseStockService(_unitOfWork);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadWarehouseStocks();
            }
        }

        private void LoadWarehouseStocks()
        {
            gvWarehouseStocks.DataSource =
                _warehouseStockService.GetAll();

            gvWarehouseStocks.DataBind();
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