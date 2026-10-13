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
    public partial class Edit : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IItemService _itemService;
        private IItemCategoryService _itemCategoryService;

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
            _itemCategoryService =
                new ItemCategoryService(_unitOfWork);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadItemCategories();
                LoadItem();
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

        private void LoadItem()
        {
            int id;

            if (!int.TryParse(Request.QueryString["id"], out id))
            {
                RedirectToIndex();
                return;
            }

            Item item = _itemService.GetById(id);

            if (item == null)
            {
                RedirectToIndex();
                return;
            }

            ItemId = item.Id;

            txtCode.Text = item.Code;
            txtNameAr.Text = item.NameAr;
            txtNameEn.Text = item.NameEn;
            txtDescriptionAr.Text = item.DescriptionAr;
            txtDescriptionEn.Text = item.DescriptionEn;

            txtMinimumStockLevel.Text =
                item.MinimumStockLevel.ToString(
                    "0.##",
                    CultureInfo.InvariantCulture);

            chkIsActive.Checked = item.IsActive;

            ListItem categoryItem =
                ddlItemCategory.Items.FindByValue(
                    item.ItemCategoryId.ToString());

            if (categoryItem != null)
            {
                ddlItemCategory.SelectedValue =
                    item.ItemCategoryId.ToString();
            }

            ListItem unitItem =
                ddlUnitOfMeasure.Items.FindByValue(
                    item.UnitOfMeasure);

            if (unitItem != null)
            {
                ddlUnitOfMeasure.SelectedValue =
                    item.UnitOfMeasure;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid || ItemId == 0)
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
                Item item = _itemService.GetById(ItemId);

                if (item == null)
                {
                    ShowError("الصنف المطلوب غير موجود.");
                    return;
                }

                item.Code = txtCode.Text.Trim();
                item.NameAr = txtNameAr.Text.Trim();
                item.NameEn = txtNameEn.Text.Trim();
                item.ItemCategoryId = itemCategoryId;
                item.UnitOfMeasure =
                    ddlUnitOfMeasure.SelectedValue;
                item.MinimumStockLevel = minimumStockLevel;
                item.IsActive = chkIsActive.Checked;

                item.DescriptionAr =
                    string.IsNullOrWhiteSpace(txtDescriptionAr.Text)
                        ? null
                        : txtDescriptionAr.Text.Trim();

                item.DescriptionEn =
                    string.IsNullOrWhiteSpace(txtDescriptionEn.Text)
                        ? null
                        : txtDescriptionEn.Text.Trim();

                _itemService.Update(item);
                RedirectToIndex();
            }
            catch (Exception)
            {
                ShowError(
                    "حدث خطأ أثناء تعديل الصنف. يرجى المحاولة مرة أخرى.");
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