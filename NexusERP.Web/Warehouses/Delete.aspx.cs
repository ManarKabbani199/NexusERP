using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;
using System.Web;

namespace NexusERP.Web.Warehouses
{
    public partial class Delete : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IWarehouseService _warehouseService;

        private int WarehouseId
        {
            get
            {
                return ViewState["WarehouseId"] == null
                    ? 0
                    : Convert.ToInt32(ViewState["WarehouseId"]);
            }
            set
            {
                ViewState["WarehouseId"] = value;
            }
        }

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
                int warehouseId;

                if (!int.TryParse(
                    Request.QueryString["id"],
                    out warehouseId))
                {
                    RedirectToIndex();
                    return;
                }

                WarehouseId = warehouseId;
                LoadWarehouse();
            }
        }

        private void LoadWarehouse()
        {
            var warehouse =
                _warehouseService.GetById(WarehouseId);

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

            linkCancel.NavigateUrl =
                $"Details.aspx?id={warehouse.Id}";
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (WarehouseId <= 0)
            {
                RedirectToIndex();
                return;
            }

            try
            {
                _warehouseService.Delete(WarehouseId);

                Response.Redirect("Index.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception exception)
            {
                ShowError(exception.Message);
            }
        }


        private void ShowError(string message)
        {
            string safeMessage =
                HttpUtility.JavaScriptStringEncode(message);

            string script =
                $"alert('تعذر حذف المستودع: {safeMessage}');";

            ClientScript.RegisterStartupScript(
                GetType(),
                Guid.NewGuid().ToString(),
                script,
                true);
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