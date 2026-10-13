using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Entities;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;
using System.Web.UI.WebControls;

namespace NexusERP.Web.ItemCategories
{
    public partial class Create : System.Web.UI.Page
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
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            try
            {
                ItemCategory itemCategory = new ItemCategory
                {
                    Code = txtCode.Text.Trim(),
                    NameAr = txtNameAr.Text.Trim(),
                    NameEn = txtNameEn.Text.Trim(),
                    DescriptionAr =
                        string.IsNullOrWhiteSpace(txtDescriptionAr.Text)
                            ? null
                            : txtDescriptionAr.Text.Trim(),
                    DescriptionEn =
                        string.IsNullOrWhiteSpace(txtDescriptionEn.Text)
                            ? null
                            : txtDescriptionEn.Text.Trim()
                };

                _itemCategoryService.Create(itemCategory);

                Response.Redirect(
                    "~/ItemCategories/Index.aspx",
                    false);

                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception)
            {
                ShowError(
                    "حدث خطأ أثناء حفظ تصنيف الصنف. يرجى المحاولة مرة أخرى.");
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