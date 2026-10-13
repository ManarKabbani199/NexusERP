<%@ Page Title="تعديل القسم"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Edit.aspx.cs"
    Inherits="NexusERP.Web.Departments.Edit" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <div class="card shadow-sm">

            <div class="card-header">
                <h3 class="mb-0">تعديل بيانات القسم</h3>
            </div>

            <div class="card-body">

                <asp:ValidationSummary
                    ID="validationSummary"
                    runat="server"
                    ValidationGroup="DepartmentForm"
                    CssClass="alert alert-danger"
                    HeaderText="يرجى تصحيح الأخطاء التالية:" />

                <div class="mb-3">
                    <label class="form-label">الفرع</label>

                    <asp:DropDownList
                        ID="ddlBranch"
                        runat="server"
                        CssClass="form-select"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                    </asp:DropDownList>

                    <asp:RequiredFieldValidator
                        runat="server"
                        ControlToValidate="ddlBranch"
                        InitialValue=""
                        ValidationGroup="DepartmentForm"
                        ErrorMessage="يجب اختيار الفرع."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label class="form-label">القسم الأب</label>

                    <asp:DropDownList
                        ID="ddlParentDepartment"
                        runat="server"
                        CssClass="form-select">
                    </asp:DropDownList>

                    <small class="text-muted">
                        اتركيه فارغًا إذا كان القسم رئيسيًا.
                    </small>
                </div>

                <div class="mb-3">
                    <label class="form-label">كود القسم</label>

                    <asp:TextBox
                        ID="txtCode"
                        runat="server"
                        CssClass="form-control"
                        MaxLength="20">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        runat="server"
                        ControlToValidate="txtCode"
                        ValidationGroup="DepartmentForm"
                        ErrorMessage="كود القسم مطلوب."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label class="form-label">
                        الاسم بالعربية
                    </label>

                    <asp:TextBox
                        ID="txtNameAr"
                        runat="server"
                        CssClass="form-control"
                        MaxLength="150">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        runat="server"
                        ControlToValidate="txtNameAr"
                        ValidationGroup="DepartmentForm"
                        ErrorMessage="اسم القسم بالعربية مطلوب."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label class="form-label">
                        الاسم بالإنجليزية
                    </label>

                    <asp:TextBox
                        ID="txtNameEn"
                        runat="server"
                        CssClass="form-control"
                        MaxLength="150">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        runat="server"
                        ControlToValidate="txtNameEn"
                        ValidationGroup="DepartmentForm"
                        ErrorMessage="اسم القسم بالإنجليزية مطلوب."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label class="form-label">
                        الوصف بالعربية
                    </label>

                    <asp:TextBox
                        ID="txtDescriptionAr"
                        runat="server"
                        CssClass="form-control"
                        TextMode="MultiLine"
                        Rows="3"
                        MaxLength="500">
                    </asp:TextBox>
                </div>

                <div class="mb-3">
                    <label class="form-label">
                        الوصف بالإنجليزية
                    </label>

                    <asp:TextBox
                        ID="txtDescriptionEn"
                        runat="server"
                        CssClass="form-control"
                        TextMode="MultiLine"
                        Rows="3"
                        MaxLength="500">
                    </asp:TextBox>
                </div>

                <div class="form-check mb-3">

                    <asp:CheckBox
                        ID="chkIsActive"
                        runat="server"
                        CssClass="form-check-input" />

                    <label class="form-check-label"
                           for="<%= chkIsActive.ClientID %>">
                        القسم نشط
                    </label>

                </div>

                <asp:Button
                    ID="btnSave"
                    runat="server"
                    Text="حفظ التعديلات"
                    CssClass="btn btn-primary"
                    ValidationGroup="DepartmentForm"
                    OnClick="btnSave_Click" />

                <a href="Index.aspx"
                   class="btn btn-secondary">
                    إلغاء
                </a>

            </div>

        </div>

    </div>

</asp:Content>