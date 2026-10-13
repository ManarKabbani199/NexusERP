<%@ Page Title="إضافة تصنيف صنف"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Create.aspx.cs"
    Inherits="NexusERP.Web.ItemCategories.Create" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <div class="card shadow-sm">

            <div class="card-header">
                <h3 class="mb-0">إضافة تصنيف صنف جديد</h3>
            </div>

            <div class="card-body">

                <asp:ValidationSummary
                    ID="validationSummary"
                    runat="server"
                    ValidationGroup="ItemCategoryForm"
                    CssClass="alert alert-danger"
                    HeaderText="يرجى تصحيح الأخطاء التالية:" />

                <div class="mb-3">
                    <label class="form-label">رمز التصنيف</label>

                    <asp:TextBox
                        ID="txtCode"
                        runat="server"
                        CssClass="form-control"
                        MaxLength="20" />

                    <asp:RequiredFieldValidator
                        ID="requiredCode"
                        runat="server"
                        ControlToValidate="txtCode"
                        ValidationGroup="ItemCategoryForm"
                        ErrorMessage="رمز التصنيف مطلوب."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label class="form-label">الاسم بالعربي</label>

                    <asp:TextBox
                        ID="txtNameAr"
                        runat="server"
                        CssClass="form-control"
                        MaxLength="150" />

                    <asp:RequiredFieldValidator
                        ID="requiredNameAr"
                        runat="server"
                        ControlToValidate="txtNameAr"
                        ValidationGroup="ItemCategoryForm"
                        ErrorMessage="الاسم العربي مطلوب."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label class="form-label">الاسم الإنجليزي</label>

                    <asp:TextBox
                        ID="txtNameEn"
                        runat="server"
                        CssClass="form-control"
                        MaxLength="150" />

                    <asp:RequiredFieldValidator
                        ID="requiredNameEn"
                        runat="server"
                        ControlToValidate="txtNameEn"
                        ValidationGroup="ItemCategoryForm"
                        ErrorMessage="الاسم الإنجليزي مطلوب."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label class="form-label">الوصف العربي</label>

                    <asp:TextBox
                        ID="txtDescriptionAr"
                        runat="server"
                        CssClass="form-control"
                        TextMode="MultiLine"
                        Rows="3"
                        MaxLength="500" />
                </div>

                <div class="mb-3">
                    <label class="form-label">الوصف الإنجليزي</label>

                    <asp:TextBox
                        ID="txtDescriptionEn"
                        runat="server"
                        CssClass="form-control"
                        TextMode="MultiLine"
                        Rows="3"
                        MaxLength="500" />
                </div>

                <div class="d-flex gap-2">

                    <asp:Button
                        ID="btnSave"
                        runat="server"
                        Text="حفظ"
                        CssClass="btn btn-primary"
                        ValidationGroup="ItemCategoryForm" 
                         OnClick="btnSave_Click"/>

                    <asp:HyperLink
                        ID="lnkCancel"
                        runat="server"
                        NavigateUrl="~/ItemCategories/Index.aspx"
                        CssClass="btn btn-secondary">
                        إلغاء
                    </asp:HyperLink>

                </div>

            </div>
        </div>
    </div>

</asp:Content>