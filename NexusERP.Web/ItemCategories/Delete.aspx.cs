using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Entities;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;

namespace NexusERP.Web.ItemCategories
{
    public partial class Delete : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IItemCategoryService _itemCategoryService;

        private int ItemCategoryId
        {
            get
            {
                return ViewState["ItemCategoryId"] == null
                    ? 0
                    : (int)ViewState["ItemCategoryId"];
            }

            set
            {
                ViewState["ItemCategoryId"] = value;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);
            _itemCategoryService =
                new ItemCategoryService(_unitOfWork);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadItemCategory();
            }
        }

        private void LoadItemCategory()
        {
            int id;

            if (!int.TryParse(Request.QueryString["id"], out id))
            {
                RedirectToIndex();
                return;
            }

            ItemCategory itemCategory =
                _itemCategoryService.GetById(id);

            if (itemCategory == null)
            {
                RedirectToIndex();
                return;
            }

            ItemCategoryId = itemCategory.Id;

            lblCode.Text = itemCategory.Code;
            lblNameAr.Text = itemCategory.NameAr;
            lblNameEn.Text = itemCategory.NameEn;

            lblDescriptionAr.Text =
                string.IsNullOrWhiteSpace(itemCategory.DescriptionAr)
                    ? "-"
                    : itemCategory.DescriptionAr;

            lblDescriptionEn.Text =
                string.IsNullOrWhiteSpace(itemCategory.DescriptionEn)
                    ? "-"
                    : itemCategory.DescriptionEn;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (ItemCategoryId == 0)
            {
                ShowError("تعذر تحديد تصنيف الصنف المطلوب حذفه.");
                return;
            }

            try
            {
                _itemCategoryService.Delete(ItemCategoryId);
                RedirectToIndex();
            }
            catch (Exception)
            {
                ShowError(
                    "حدث خطأ أثناء حذف تصنيف الصنف. قد يكون مرتبطًا بأصناف أخرى.");
            }
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            pnlError.Visible = true;
        }

        private void RedirectToIndex()
        {
            Response.Redirect(
                "~/ItemCategories/Index.aspx",
                false);

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