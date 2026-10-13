using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Entities;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;

namespace NexusERP.Web.ItemCategories
{
    public partial class Details : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IItemCategoryService _itemCategoryService;

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
                ShowError("رقم تصنيف الصنف غير صحيح.");
                lnkEdit.Visible = false;
                return;
            }

            ItemCategory itemCategory =
                _itemCategoryService.GetById(id);

            if (itemCategory == null)
            {
                ShowError("تصنيف الصنف المطلوب غير موجود.");
                lnkEdit.Visible = false;
                return;
            }

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

            lnkEdit.NavigateUrl =
                "~/ItemCategories/Edit.aspx?id=" + itemCategory.Id;
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            pnlError.Visible = true;
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