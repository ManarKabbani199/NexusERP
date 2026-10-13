using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Entities;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;
using System.Globalization;

namespace NexusERP.Web.Items
{
    public partial class Delete : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IItemService _itemService;

        private int ItemId
        {
            get
            {
                return ViewState["ItemId"] == null
                    ? 0
                    : (int)ViewState["ItemId"];
            }

            set
            {
                ViewState["ItemId"] = value;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);
            _itemService = new ItemService(_unitOfWork);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadItem();
            }
        }

        private void LoadItem()
        {
            int id;

            if (!int.TryParse(Request.QueryString["id"], out id))
            {
                ShowError("رقم الصنف غير صحيح.");
                return;
            }

            Item item = _itemService.GetById(id);

            if (item == null)
            {
                ShowError("الصنف المطلوب غير موجود.");
                return;
            }

            ItemId = item.Id;

            lblCode.Text = item.Code;
            lblNameAr.Text = item.NameAr;
            lblNameEn.Text = item.NameEn;

            lblItemCategory.Text =
                item.ItemCategory == null
                    ? "-"
                    : item.ItemCategory.NameAr;

            lblUnitOfMeasure.Text =
                GetUnitOfMeasureName(item.UnitOfMeasure);

            lblMinimumStockLevel.Text =
                item.MinimumStockLevel.ToString(
                    "N2",
                    CultureInfo.CurrentCulture);

            if (item.IsActive)
            {
                lblIsActive.Text = "نشط";
                lblIsActive.CssClass = "badge bg-success";
            }
            else
            {
                lblIsActive.Text = "غير نشط";
                lblIsActive.CssClass = "badge bg-secondary";
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (ItemId == 0)
            {
                ShowError("تعذر تحديد الصنف المطلوب حذفه.");
                return;
            }

            try
            {
                _itemService.Delete(ItemId);
                RedirectToIndex();
            }
            catch (Exception)
            {
                ShowError(
                    "تعذر حذف الصنف. قد يكون مرتبطًا بحركات مخزون.");
            }
        }

        private string GetUnitOfMeasureName(string unitOfMeasure)
        {
            switch (unitOfMeasure)
            {
                case "Piece":
                    return "قطعة";

                case "Box":
                    return "صندوق";

                case "Carton":
                    return "كرتون";

                case "Kilogram":
                    return "كيلوغرام";

                case "Liter":
                    return "لتر";

                default:
                    return string.IsNullOrWhiteSpace(unitOfMeasure)
                        ? "-"
                        : unitOfMeasure;
            }
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            pnlError.Visible = true;
            pnlDetails.Visible = false;
        }

        private void RedirectToIndex()
        {
            Response.Redirect("~/Items/Index.aspx", false);
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