using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Entities;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;

namespace NexusERP.Web.Items
{
    public partial class Create : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IItemService _itemService;
        private IItemCategoryService _itemCategoryService;

        protected void Page_Init(object sender, EventArgs e)
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);

            _itemService = new ItemService(_unitOfWork);
            _itemCategoryService =
                new ItemCategoryService(_unitOfWork);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadItemCategories();
            }
        }

        private void LoadItemCategories()
        {
            var categories = _itemCategoryService
                .GetAll()
                .OrderBy(category => category.NameAr)
                .ToList();

            ddlItemCategory.DataSource = categories;
            ddlItemCategory.DataTextField = "NameAr";
            ddlItemCategory.DataValueField = "Id";
            ddlItemCategory.DataBind();

            ddlItemCategory.Items.Insert(
                0,
                new ListItem("-- اختر تصنيف الصنف --", ""));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            int itemCategoryId;

            if (!int.TryParse(
                ddlItemCategory.SelectedValue,
                out itemCategoryId))
            {
                ShowError("يرجى اختيار تصنيف صحيح.");
                return;
            }

            decimal minimumStockLevel;

            if (!TryParseDecimal(
                txtMinimumStockLevel.Text,
                out minimumStockLevel))
            {
                ShowError("قيمة الحد الأدنى للمخزون غير صحيحة.");
                return;
            }

            if (minimumStockLevel < 0)
            {
                ShowError("الحد الأدنى للمخزون لا يمكن أن يكون سالبًا.");
                return;
            }

            try
            {
                Item item = new Item
                {
                    Code = txtCode.Text.Trim(),
                    NameAr = txtNameAr.Text.Trim(),
                    NameEn = txtNameEn.Text.Trim(),
                    UnitOfMeasure =
                        ddlUnitOfMeasure.SelectedValue,
                    MinimumStockLevel = minimumStockLevel,
                    IsActive = chkIsActive.Checked,
                    ItemCategoryId = itemCategoryId,

                    DescriptionAr =
                        string.IsNullOrWhiteSpace(txtDescriptionAr.Text)
                            ? null
                            : txtDescriptionAr.Text.Trim(),

                    DescriptionEn =
                        string.IsNullOrWhiteSpace(txtDescriptionEn.Text)
                            ? null
                            : txtDescriptionEn.Text.Trim()
                };

                _itemService.Create(item);

                Response.Redirect(
                    "~/Items/Index.aspx",
                    false);

                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception)
            {
                ShowError(
                    "حدث خطأ أثناء حفظ الصنف. يرجى المحاولة مرة أخرى.");
            }
        }

        private bool TryParseDecimal(
            string value,
            out decimal result)
        {
            if (decimal.TryParse(
                value,
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out result))
            {
                return true;
            }

            return decimal.TryParse(
                value,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out result);
        }

        private void ShowError(string message)
        {
            CustomValidator validator = new CustomValidator
            {
                IsValid = false,
                ErrorMessage = message,
                ValidationGroup = "ItemForm"
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