using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;
using System.Web.UI.WebControls;

namespace NexusERP.Web.Departments
{
    public partial class Index : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IDepartmentService _departmentService;

        protected void Page_Init(object sender, EventArgs e)
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);

            _departmentService =
                new DepartmentService(_unitOfWork);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDepartments();
            }
        }

        private void LoadDepartments()
        {
            gvDepartments.DataSource =
                _departmentService.GetAll();

            gvDepartments.DataBind();
        }

        protected void btnDelete_Click(
            object sender,
            EventArgs e)
        {
            var deleteButton = sender as LinkButton;

            if (deleteButton == null)
            {
                return;
            }

            int departmentId;

            if (!int.TryParse(
                    deleteButton.CommandArgument,
                    out departmentId))
            {
                ShowMessage(
                    "رقم القسم غير صحيح.",
                    false);

                return;
            }

            try
            {
                bool deleted =
                    _departmentService.Delete(departmentId);

                if (!deleted)
                {
                    ShowMessage(
                        "القسم غير موجود أو لم يتم حذفه.",
                        false);

                    return;
                }

                LoadDepartments();

                ShowMessage(
                    "تم حذف القسم بنجاح.",
                    true);
            }
            catch (Exception)
            {
                ShowMessage(
                    "لا يمكن حذف القسم؛ قد يكون مرتبطًا بأقسام فرعية أو وظائف أو موظفين.",
                    false);
            }
        }

        private void ShowMessage(
            string message,
            bool success)
        {
            string safeMessage =
                message.Replace("'", "\\'");

            string script =
                "alert('" + safeMessage + "');";

            ClientScript.RegisterStartupScript(
                GetType(),
                Guid.NewGuid().ToString(),
                script,
                true);
        }

        protected void Page_Unload(
            object sender,
            EventArgs e)
        {
            _unitOfWork?.Dispose();
        }
    }
}