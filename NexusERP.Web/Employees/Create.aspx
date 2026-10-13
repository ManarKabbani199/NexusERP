<%@ Page Title="إضافة موظف"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Create.aspx.cs"
    Inherits="NexusERP.Web.Employees.Create" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <div class="card shadow-sm">

            <div class="card-header">
                <h3 class="mb-0">إضافة موظف جديد</h3>
            </div>

            <div class="card-body">

                <asp:ValidationSummary
                    ID="validationSummary"
                    runat="server"
                    ValidationGroup="EmployeeForm"
                    CssClass="alert alert-danger"
                    HeaderText="يرجى تصحيح الأخطاء التالية:" />

                <asp:Panel
                    ID="pnlError"
                    runat="server"
                    Visible="false"
                    CssClass="alert alert-danger">

                    <asp:Literal
                        ID="litError"
                        runat="server" />

                </asp:Panel>

                <div class="row">

                    <%-- الرقم الوظيفي --%>
                    <div class="col-md-4 mb-3">

                        <label class="form-label">
                            الرقم الوظيفي
                        </label>

                        <asp:TextBox
                            ID="txtEmployeeNumber"
                            runat="server"
                            CssClass="form-control"
                            MaxLength="30" />

                        <asp:RequiredFieldValidator
                            ID="rfvEmployeeNumber"
                            runat="server"
                            ControlToValidate="txtEmployeeNumber"
                            ValidationGroup="EmployeeForm"
                            CssClass="text-danger"
                            ErrorMessage="الرقم الوظيفي مطلوب."
                            Display="Dynamic" />

                    </div>

                    <%-- الاسم العربي الأول --%>
                    <div class="col-md-4 mb-3">

                        <label class="form-label">
                            الاسم الأول بالعربية
                        </label>

                        <asp:TextBox
                            ID="txtFirstNameAr"
                            runat="server"
                            CssClass="form-control"
                            MaxLength="100" />

                        <asp:RequiredFieldValidator
                            ID="rfvFirstNameAr"
                            runat="server"
                            ControlToValidate="txtFirstNameAr"
                            ValidationGroup="EmployeeForm"
                            CssClass="text-danger"
                            ErrorMessage="الاسم الأول بالعربية مطلوب."
                            Display="Dynamic" />

                    </div>

                    <%-- اسم العائلة العربي --%>
                    <div class="col-md-4 mb-3">

                        <label class="form-label">
                            اسم العائلة بالعربية
                        </label>

                        <asp:TextBox
                            ID="txtLastNameAr"
                            runat="server"
                            CssClass="form-control"
                            MaxLength="100" />

                        <asp:RequiredFieldValidator
                            ID="rfvLastNameAr"
                            runat="server"
                            ControlToValidate="txtLastNameAr"
                            ValidationGroup="EmployeeForm"
                            CssClass="text-danger"
                            ErrorMessage="اسم العائلة بالعربية مطلوب."
                            Display="Dynamic" />

                    </div>

                    <%-- الاسم الإنجليزي الأول --%>
                    <div class="col-md-4 mb-3">

                        <label class="form-label">
                            الاسم الأول بالإنجليزية
                        </label>

                        <asp:TextBox
                            ID="txtFirstNameEn"
                            runat="server"
                            CssClass="form-control"
                            MaxLength="100" />

                        <asp:RequiredFieldValidator
                            ID="rfvFirstNameEn"
                            runat="server"
                            ControlToValidate="txtFirstNameEn"
                            ValidationGroup="EmployeeForm"
                            CssClass="text-danger"
                            ErrorMessage="الاسم الأول بالإنجليزية مطلوب."
                            Display="Dynamic" />

                    </div>

                    <%-- اسم العائلة الإنجليزي --%>
                    <div class="col-md-4 mb-3">

                        <label class="form-label">
                            اسم العائلة بالإنجليزية
                        </label>

                        <asp:TextBox
                            ID="txtLastNameEn"
                            runat="server"
                            CssClass="form-control"
                            MaxLength="100" />

                        <asp:RequiredFieldValidator
                            ID="rfvLastNameEn"
                            runat="server"
                            ControlToValidate="txtLastNameEn"
                            ValidationGroup="EmployeeForm"
                            CssClass="text-danger"
                            ErrorMessage="اسم العائلة بالإنجليزية مطلوب."
                            Display="Dynamic" />

                    </div>

                    <%-- الرقم المدني --%>
                    <div class="col-md-4 mb-3">

                        <label class="form-label">
                            الرقم المدني
                        </label>

                        <asp:TextBox
                            ID="txtCivilId"
                            runat="server"
                            CssClass="form-control"
                            MaxLength="20" />

                    </div>

                    <%-- تاريخ الميلاد --%>
                    <div class="col-md-4 mb-3">

                        <label class="form-label">
                            تاريخ الميلاد
                        </label>

                        <asp:TextBox
                            ID="txtDateOfBirth"
                            runat="server"
                            TextMode="Date"
                            CssClass="form-control" />

                    </div>

                    <%-- الجنس --%>
                    <div class="col-md-4 mb-3">

                        <label class="form-label">
                            الجنس
                        </label>

                        <asp:DropDownList
                            ID="ddlGender"
                            runat="server"
                            CssClass="form-select" />

                    </div>

                    <%-- الجنسية --%>
                    <div class="col-md-4 mb-3">

                        <label class="form-label">
                            الجنسية
                        </label>

                        <asp:TextBox
                            ID="txtNationality"
                            runat="server"
                            CssClass="form-control"
                            MaxLength="100" />

                    </div>

                    <%-- الهاتف --%>
                    <div class="col-md-4 mb-3">

                        <label class="form-label">
                            الهاتف
                        </label>

                        <asp:TextBox
                            ID="txtPhone"
                            runat="server"
                            CssClass="form-control"
                            MaxLength="30" />

                    </div>

                    <%-- البريد الإلكتروني --%>
                    <div class="col-md-4 mb-3">

                        <label class="form-label">
                            البريد الإلكتروني
                        </label>

                        <asp:TextBox
                            ID="txtEmail"
                            runat="server"
                            TextMode="Email"
                            CssClass="form-control"
                            MaxLength="150" />

                        <asp:RegularExpressionValidator
                            ID="revEmail"
                            runat="server"
                            ControlToValidate="txtEmail"
                            ValidationGroup="EmployeeForm"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ErrorMessage="صيغة البريد الإلكتروني غير صحيحة."
                            ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" />

                    </div>

                    <%-- تاريخ التعيين --%>
                    <div class="col-md-4 mb-3">

                        <label class="form-label">
                            تاريخ التعيين
                        </label>

                        <asp:TextBox
                            ID="txtHireDate"
                            runat="server"
                            TextMode="Date"
                            CssClass="form-control" />

                        <asp:RequiredFieldValidator
                            ID="rfvHireDate"
                            runat="server"
                            ControlToValidate="txtHireDate"
                            ValidationGroup="EmployeeForm"
                            CssClass="text-danger"
                            ErrorMessage="تاريخ التعيين مطلوب."
                            Display="Dynamic" />

                    </div>

                    <%-- نوع التوظيف --%>
                    <div class="col-md-4 mb-3">

                        <label class="form-label">
                            نوع التوظيف
                        </label>

                        <asp:DropDownList
                            ID="ddlEmploymentType"
                            runat="server"
                            CssClass="form-select" />

                    </div>

                    <%-- حالة الموظف --%>
                    <div class="col-md-4 mb-3">

                        <label class="form-label">
                            حالة الموظف
                        </label>

                        <asp:DropDownList
                            ID="ddlEmploymentStatus"
                            runat="server"
                            CssClass="form-select" />

                    </div>

                    <%-- الراتب الأساسي --%>
                    <div class="col-md-4 mb-3">

                        <label class="form-label">
                            الراتب الأساسي
                        </label>

                        <asp:TextBox
                            ID="txtBasicSalary"
                            runat="server"
                            TextMode="Number"
                            step="0.001"
                            min="0"
                            CssClass="form-control" />

                        <asp:RequiredFieldValidator
                            ID="rfvBasicSalary"
                            runat="server"
                            ControlToValidate="txtBasicSalary"
                            ValidationGroup="EmployeeForm"
                            CssClass="text-danger"
                            ErrorMessage="الراتب الأساسي مطلوب."
                            Display="Dynamic" />

                    </div>

                    <%-- الشركة --%>
                    <div class="col-md-4 mb-3">

                        <label class="form-label">
                            الشركة
                        </label>

                        <asp:DropDownList
                            ID="ddlCompany"
                            runat="server"
                            AutoPostBack="true"
                            OnSelectedIndexChanged=
                                "ddlCompany_SelectedIndexChanged"
                            CssClass="form-select" />

                        <asp:RequiredFieldValidator
                            ID="rfvCompany"
                            runat="server"
                            ControlToValidate="ddlCompany"
                            InitialValue=""
                            ValidationGroup="EmployeeForm"
                            CssClass="text-danger"
                            ErrorMessage="يرجى اختيار الشركة."
                            Display="Dynamic" />

                    </div>

                    <%-- الفرع --%>
                    <div class="col-md-4 mb-3">

                        <label class="form-label">
                            الفرع
                        </label>

                        <asp:DropDownList
                            ID="ddlBranch"
                            runat="server"
                            AutoPostBack="true"
                            OnSelectedIndexChanged=
                                "ddlBranch_SelectedIndexChanged"
                            CssClass="form-select" />

                        <asp:RequiredFieldValidator
                            ID="rfvBranch"
                            runat="server"
                            ControlToValidate="ddlBranch"
                            InitialValue=""
                            ValidationGroup="EmployeeForm"
                            CssClass="text-danger"
                            ErrorMessage="يرجى اختيار الفرع."
                            Display="Dynamic" />

                    </div>

                    <%-- القسم --%>
                    <div class="col-md-4 mb-3">

                        <label class="form-label">
                            القسم
                        </label>

                        <asp:DropDownList
                            ID="ddlDepartment"
                            runat="server"
                            AutoPostBack="true"
                            OnSelectedIndexChanged=
                                "ddlDepartment_SelectedIndexChanged"
                            CssClass="form-select" />

                        <asp:RequiredFieldValidator
                            ID="rfvDepartment"
                            runat="server"
                            ControlToValidate="ddlDepartment"
                            InitialValue=""
                            ValidationGroup="EmployeeForm"
                            CssClass="text-danger"
                            ErrorMessage="يرجى اختيار القسم."
                            Display="Dynamic" />

                    </div>

                    <%-- الوظيفة --%>
                    <div class="col-md-4 mb-3">

                        <label class="form-label">
                            الوظيفة
                        </label>

                        <asp:DropDownList
                            ID="ddlPosition"
                            runat="server"
                            CssClass="form-select" />

                        <asp:RequiredFieldValidator
                            ID="rfvPosition"
                            runat="server"
                            ControlToValidate="ddlPosition"
                            InitialValue=""
                            ValidationGroup="EmployeeForm"
                            CssClass="text-danger"
                            ErrorMessage="يرجى اختيار الوظيفة."
                            Display="Dynamic" />

                    </div>

                    <%-- المدير المباشر --%>
                    <div class="col-md-4 mb-3">

                        <label class="form-label">
                            المدير المباشر
                        </label>

                        <asp:DropDownList
                            ID="ddlManager"
                            runat="server"
                            CssClass="form-select" />

                        <small class="text-muted">
                            يمكن تركه فارغًا إذا لم يكن للموظف مدير مباشر.
                        </small>

                    </div>

                </div>

                <hr />

                <div class="d-flex gap-2">

                    <asp:Button
                        ID="btnSave"
                        runat="server"
                        Text="حفظ الموظف"
                        CssClass="btn btn-primary"
                        ValidationGroup="EmployeeForm"
                        OnClick="btnSave_Click" />

                    <a href="Index.aspx"
                       class="btn btn-secondary">
                        إلغاء
                    </a>

                </div>

            </div>

        </div>

    </div>

</asp:Content>