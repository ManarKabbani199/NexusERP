using System;
using System.Linq;
using System.Web.UI.WebControls;
using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using NexusERP.Domain.Enums;

namespace NexusERP.Web.PurchaseOrders
{
    public partial class Index : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IPurchaseOrderService _purchaseOrderService;

        private bool CanManagePurchaseOrders
        {
            get
            {
                return Request.IsAuthenticated &&
                    (User.IsInRole("Admin") || User.IsInRole("PurchasingManager"));
            }
        }

        protected string Translate(string key)
        {
            return Convert.ToString(GetGlobalResourceObject("PurchaseOrders", key));
        }

        protected string DeleteConfirmationScript()
        {
            return "return confirm(" +
                System.Web.HttpUtility.JavaScriptStringEncode(
                    Translate("ConfirmDelete"), true) + ");";
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);
            _purchaseOrderService = new PurchaseOrderService(_unitOfWork);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPurchaseOrders();

                if (Request.QueryString["created"] == "true")
                {
                    ShowSuccess(Translate("CreatedSuccess"));
                }
            }
        }

        private void LoadPurchaseOrders()
        {
            bool canManage = CanManagePurchaseOrders;

            var purchaseOrders = _purchaseOrderService
                .GetAll()
                .Select(order => new
                {
                    Id = order.Id,
                    OrderNumber = order.OrderNumber,
                    OrderDate = order.OrderDate,

                    CanDelete = canManage &&
                        order.Status == PurchaseOrderStatus.Draft,

                    StatusName = GetStatusName(order.Status),

                    SupplierName = order.Supplier != null
                        ? order.Supplier.NameAr
                        : Translate("Unspecified"),

                    ItemsCount = order.Items.Count(item => !item.IsDeleted),

                    GrandTotal = order.Items
                        .Where(item => !item.IsDeleted)
                        .Sum(item => (decimal?)(item.Quantity * item.UnitPrice)) ?? 0
                })
                .ToList();

            gvPurchaseOrders.DataSource = purchaseOrders;
            gvPurchaseOrders.DataBind();
        }

        protected void gvPurchaseOrders_RowCommand(
            object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "DeleteOrder")
            {
                return;
            }

            if (!CanManagePurchaseOrders)
            {
                ShowError(Translate("NotAuthorized"));
                return;
            }

            int purchaseOrderId;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out purchaseOrderId)
                || purchaseOrderId <= 0)
            {
                ShowError(Translate("InvalidId"));
                return;
            }

            try
            {
                var order = _unitOfWork.PurchaseOrders.GetById(purchaseOrderId);

                if (order == null || order.IsDeleted)
                {
                    LoadPurchaseOrders();
                    ShowError(Translate("NotFound"));
                    return;
                }

                if (order.Status != PurchaseOrderStatus.Draft)
                {
                    LoadPurchaseOrders();
                    ShowError(Translate("DraftOnly"));
                    return;
                }

                _purchaseOrderService.Delete(purchaseOrderId);
                LoadPurchaseOrders();
                ShowSuccess(Translate("DeletedSuccess"));
            }
            catch (Exception exception)
            {
                ShowError(exception.Message);
            }
        }

        private void ShowSuccess(string message)
        {
            lblMessage.Text = Server.HtmlEncode(message);
            lblMessage.CssClass = "alert alert-success d-block";
            lblMessage.Visible = true;
        }

        private void ShowError(string message)
        {
            lblMessage.Text = Server.HtmlEncode(message);
            lblMessage.CssClass = "alert alert-danger d-block";
            lblMessage.Visible = true;
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (_unitOfWork != null)
            {
                _unitOfWork.Dispose();
                _unitOfWork = null;
                _context = null;
            }
            else if (_context != null)
            {
                _context.Dispose();
                _context = null;
            }
        }

        private string GetStatusName(PurchaseOrderStatus status)
        {
            switch (status)
            {
                case PurchaseOrderStatus.Draft:
                    return Translate("Draft");
                case PurchaseOrderStatus.Approved:
                    return Translate("Approved");
                case PurchaseOrderStatus.PartiallyReceived:
                    return Translate("PartiallyReceived");
                case PurchaseOrderStatus.Received:
                    return Translate("Received");
                case PurchaseOrderStatus.Cancelled:
                    return Translate("Cancelled");
                default:
                    return Translate("Unknown");
            }
        }
    }
}
