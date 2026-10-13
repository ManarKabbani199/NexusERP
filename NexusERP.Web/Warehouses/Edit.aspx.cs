using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;
using System.Web;

namespace NexusERP.Web.Warehouses
{
    public partial class Edit : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IWarehouseService _warehouseService;
        private IBranchService _branchService;

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
            _branchService = new BranchService(_unitOfWork);
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

                LoadBranches();
                LoadWarehouse();
            }
        }

        private void LoadBranches()
        {
            ddlBranch.DataSource = _branchService.GetAll();
            ddlBranch.DataTextField = "NameAr";
            ddlBranch.DataValueField = "Id";
            ddlBranch.DataBind();

            ddlBranch.Items.Insert(
                0,
                new System.Web.UI.WebControls.ListItem(
                    "-- اختر الفرع --",
                    ""));
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

            txtCode.Text = warehouse.Code;
            txtNameAr.Text = warehouse.NameAr;
            txtNameEn.Text = warehouse.NameEn;
            txtAddressAr.Text = warehouse.AddressAr;
            txtAddressEn.Text = warehouse.AddressEn;
            txtPhone.Text = warehouse.Phone;
            chkIsActive.Checked = warehouse.IsActive;

            ddlBranch.SelectedValue =
                warehouse.BranchId.ToString();

            linkCancel.NavigateUrl =
                $"Details.aspx?id={warehouse.Id}";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid || WarehouseId <= 0)
            {
                return;
            }

            try
            {
                var warehouse =
                    _warehouseService.GetById(WarehouseId);

                if (warehouse == null)
                {
                    RedirectToIndex();
                    return;
                }

                warehouse.BranchId =
                    Convert.ToInt32(ddlBranch.SelectedValue);

                warehouse.Code = txtCode.Text.Trim();
                warehouse.NameAr = txtNameAr.Text.Trim();
                warehouse.NameEn = txtNameEn.Text.Trim();
                warehouse.AddressAr = txtAddressAr.Text.Trim();
                warehouse.AddressEn = txtAddressEn.Text.Trim();
                warehouse.Phone = txtPhone.Text.Trim();
                warehouse.IsActive = chkIsActive.Checked;

                _warehouseService.Update(warehouse);

                Response.Redirect(
                    $"Details.aspx?id={warehouse.Id}",
                    false);

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
                $"alert('حدث خطأ أثناء تعديل المستودع: {safeMessage}');";

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