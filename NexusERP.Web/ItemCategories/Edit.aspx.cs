using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Entities;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;
using System.Web.UI.WebControls;

namespace NexusERP.Web.ItemCategories
{
    public partial class Edit : System.Web.UI.Page
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
                Response.Redirect(
                    "~/ItemCategories/Index.aspx",
                    false);

                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            ItemCategory itemCategory =
                _itemCategoryService.GetById(id);

            if (itemCategory == null)
            {
                Response.Redirect(
                    "~/ItemCategories/Index.aspx",
                    false);

                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            ItemCategoryId = itemCategory.Id;

            txtCode.Text = itemCategory.Code;
            txtNameAr.Text = itemCategory.NameAr;
            txtNameEn.Text = itemCategory.NameEn;
            txtDescriptionAr.Text = itemCategory.DescriptionAr;
            txtDescriptionEn.Text = itemCategory.DescriptionEn;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid || ItemCategoryId == 0)
            {
                return;
            }

            try
            {
                ItemCategory itemCategory =
                    _itemCategoryService.GetById(ItemCategoryId);

                if (itemCategory == null)
                {
                    ShowError("تصنيف الصنف المطلوب غير موجود.");
                    return;
                }

                itemCategory.Code = txtCode.Text.Trim();
                itemCategory.NameAr = txtNameAr.Text.Trim();
                itemCategory.NameEn = txtNameEn.Text.Trim();

                itemCategory.DescriptionAr =
                    string.IsNullOrWhiteSpace(txtDescriptionAr.Text)
                        ? null
                        : txtDescriptionAr.Text.Trim();

                itemCategory.DescriptionEn =
                    string.IsNullOrWhiteSpace(txtDescriptionEn.Text)
                        ? null
                        : txtDescriptionEn.Text.Trim();

                _itemCategoryService.Update(itemCategory);

                Response.Redirect(
                    "~/ItemCategories/Index.aspx",
                    false);

                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception)
            {
                ShowError(
                    "حدث خطأ أثناء تعديل تصنيف الصنف. يرجى المحاولة مرة أخرى.");
            }
        }

        private void ShowError(string message)
        {
            CustomValidator validator = new CustomValidator
            {
                IsValid = false,
                ErrorMessage = message,
                ValidationGroup = "ItemCategoryForm"
            };

            Page.Validators.Add(validator);
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