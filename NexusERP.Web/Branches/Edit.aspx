<%@ Page Title="تعديل الفرع"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Edit.aspx.cs"
    Inherits="NexusERP.Web.Branches.Edit" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <div class="card shadow-sm">

            <div class="card-header">
                <h3 class="mb-0">تعديل بيانات الفرع</h3>
            </div>

            <div class="card-body">

                <asp:ValidationSummary
                    ID="validationSummary"
                    runat="server"
                    ValidationGroup="BranchForm"
                    CssClass="alert alert-danger"
                    HeaderText="يرجى تصحيح الأخطاء التالية:" />

                <div class="mb-3">
                    <label class="form-label">الشركة</label>

                    <asp:DropDownList
                        ID="ddlCompany"
                        runat="server"
                        CssClass="form-select">
                    </asp:DropDownList>

                    <asp:RequiredFieldValidator
                        runat="server"
                        ControlToValidate="ddlCompany"
                        InitialValue=""
                        ValidationGroup="BranchForm"
                        ErrorMessage="يجب اختيار الشركة."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label class="form-label">كود الفرع</label>

                    <asp:TextBox
                        ID="txtCode"
                        runat="server"
                        CssClass="form-control"
                        MaxLength="20">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        runat="server"
                        ControlToValidate="txtCode"
                        ValidationGroup="BranchForm"
                        ErrorMessage="كود الفرع مطلوب."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label class="form-label">الاسم بالعربية</label>

                    <asp:TextBox
                        ID="txtNameAr"
                        runat="server"
                        CssClass="form-control"
                        MaxLength="150">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        runat="server"
                        ControlToValidate="txtNameAr"
                        ValidationGroup="BranchForm"
                        ErrorMessage="الاسم بالعربية مطلوب."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label class="form-label">الاسم بالإنجليزية</label>

                    <asp:TextBox
                        ID="txtNameEn"
                        runat="server"
                        CssClass="form-control"
                        MaxLength="150">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        runat="server"
                        ControlToValidate="txtNameEn"
                        ValidationGroup="BranchForm"
                        ErrorMessage="الاسم بالإنجليزية مطلوب."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label class="form-label">العنوان بالعربية</label>

                    <asp:TextBox
                        ID="txtAddressAr"
                        runat="server"
                        CssClass="form-control"
                        MaxLength="250">
                    </asp:TextBox>
                </div>

                <div class="mb-3">
                    <label class="form-label">العنوان بالإنجليزية</label>

                    <asp:TextBox
                        ID="txtAddressEn"
                        runat="server"
                        CssClass="form-control"
                        MaxLength="250">
                    </asp:TextBox>
                </div>

                <div class="mb-3">
                    <label class="form-label">الهاتف</label>

                    <asp:TextBox
                        ID="txtPhone"
                        runat="server"
                        CssClass="form-control"
                        MaxLength="30">
                    </asp:TextBox>
                </div>

                <div class="form-check mb-3">

                    <asp:CheckBox
                        ID="chkIsActive"
                        runat="server"
                        CssClass="form-check-input" />

                    <label class="form-check-label"
                           for="<%= chkIsActive.ClientID %>">
                        الفرع نشط
                    </label>

                </div>

                <asp:Button
                    ID="btnSave"
                    runat="server"
                    Text="حفظ التعديلات"
                    CssClass="btn btn-primary"
                    ValidationGroup="BranchForm"
                    OnClick="btnSave_Click" />

                <a href="Index.aspx"
                   class="btn btn-secondary">
                    إلغاء
                </a>

            </div>

        </div>

    </div>

</asp:Content>