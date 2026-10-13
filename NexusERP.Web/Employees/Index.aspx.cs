using NexusERP.Application.Interfaces;
using NexusERP.Application.Services;
using NexusERP.Domain.Entities;
using NexusERP.Infrastructure.Data;
using NexusERP.Infrastructure.Repositories;
using System;

namespace NexusERP.Web.Employees
{
    public partial class Index : System.Web.UI.Page
    {
        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IEmployeeService _employeeService;

        protected void Page_Init(object sender, EventArgs e)
        {
            _context = new ApplicationDbContext();

            _unitOfWork = new UnitOfWork(_context);

            _employeeService =
                new EmployeeService(_unitOfWork);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEmployees();
                ShowStatusMessage();
            }
        }

        private void LoadEmployees()
        {
            gvEmployees.DataSource =
                _employeeService.GetAll();

            gvEmployees.DataBind();
        }

        protected string GetManagerName(object managerObject)
        {
            var manager = managerObject as Employee;

            if (manager == null)
            {
                return "—";
            }

            return manager.FirstNameAr + " " +
                   manager.LastNameAr;
        }

        private void ShowStatusMessage()
        {
            string message =
                Request.QueryString["message"];

            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            pnlMessage.Visible = true;
            pnlMessage.CssClass =
                "alert alert-success";

            switch (message)
            {
                case "created":
                    litMessage.Text =
                        "تمت إضافة الموظف بنجاح.";
                    break;

                case "updated":
                    litMessage.Text =
                        "تم تعديل بيانات الموظف بنجاح.";
                    break;

                case "deleted":
                    litMessage.Text =
                        "تم حذف الموظف بنجاح.";
                    break;

                default:
                    pnlMessage.Visible = false;
                    break;
            }
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