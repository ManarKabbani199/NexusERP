using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Entities;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;
using System.Web;

namespace NexusERP.Web.Warehouses
{
    public partial class Create : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IWarehouseService _warehouseService;
        private IBranchService _branchService;

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
                LoadBranches();
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            try
            {
                var warehouse = new Warehouse
                {
                    BranchId = Convert.ToInt32(ddlBranch.SelectedValue),
                    Code = txtCode.Text.Trim(),
                    NameAr = txtNameAr.Text.Trim(),
                    NameEn = txtNameEn.Text.Trim(),
                    AddressAr = txtAddressAr.Text.Trim(),
                    AddressEn = txtAddressEn.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    IsActive = chkIsActive.Checked
                };

                _warehouseService.Create(warehouse);

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
                $"alert('حدث خطأ أثناء حفظ المستودع: {safeMessage}');";

            ClientScript.RegisterStartupScript(
                GetType(),
                Guid.NewGuid().ToString(),
                script,
                true);
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