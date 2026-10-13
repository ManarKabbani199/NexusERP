using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;
using System.Web.UI.WebControls;

namespace NexusERP.Web.Companies
{
    public partial class Index : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private ICompanyService _companyService;

        protected void Page_Init(object sender, EventArgs e)
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);
            _companyService = new CompanyService(_unitOfWork);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCompanies();
            }
        }

        private void LoadCompanies()
        {
            gvCompanies.DataSource =
                _companyService.GetAll();

            gvCompanies.DataBind();
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            _unitOfWork?.Dispose();
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

            int companyId;

            if (!int.TryParse(
                    deleteButton.CommandArgument,
                    out companyId))
            {
                ShowMessage(
                    "رقم الشركة غير صحيح.",
                    false);

                return;
            }

            try
            {
                bool deleted =
                    _companyService.Delete(companyId);

                if (!deleted)
                {
                    ShowMessage(
                        "الشركة غير موجودة أو لم يتم حذفها.",
                        false);

                    return;
                }

                // إعادة تحميل الجدول بعد الحذف
                LoadCompanies();

                ShowMessage(
                    "تم حذف الشركة بنجاح.",
                    true);
            }
            catch (Exception)
            {
                ShowMessage(
                    "لا يمكن حذف الشركة؛ قد تكون مرتبطة بفروع أو بيانات أخرى.",
                    false);
            }
        }

        private void ShowMessage(
            string message,
            bool success)
        {
            string script;

            if (success)
            {
                script =
                    "alert('تم حذف الشركة بنجاح.');";
            }
            else
            {
                script =
                    "alert('حدث خطأ: " +
                    message.Replace("'", "\\'") +
                    "');";
            }

            ClientScript.RegisterStartupScript(
                GetType(),
                Guid.NewGuid().ToString(),
                script,
                true);
        }
    }
}
