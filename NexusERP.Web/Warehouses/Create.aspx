<%@ Page Title="إضافة مستودع"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Create.aspx.cs"
    Inherits="NexusERP.Web.Warehouses.Create" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4" dir="rtl">

        <div class="card shadow-sm">

            <div class="card-header">
                <h3 class="mb-0">إضافة مستودع جديد</h3>
            </div>

            <div class="card-body">

                <asp:ValidationSummary
                    ID="validationSummary"
                    runat="server"
                    ValidationGroup="WarehouseForm"
                    CssClass="alert alert-danger"
                    HeaderText="يرجى تصحيح الأخطاء التالية:" />

                <div class="mb-3">
                    <label class="form-label">الفرع</label>

                    <asp:DropDownList
                        ID="ddlBranch"
                        runat="server"
                        CssClass="form-select">
                    </asp:DropDownList>

                    <asp:RequiredFieldValidator
                        ID="requiredBranch"
                        runat="server"
                        ControlToValidate="ddlBranch"
                        InitialValue=""
                        ValidationGroup="WarehouseForm"
                        ErrorMessage="يرجى اختيار الفرع."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label class="form-label">كود المستودع</label>

                    <asp:TextBox
                        ID="txtCode"
                        runat="server"
                        CssClass="form-control"
                        MaxLength="20" />

                    <asp:RequiredFieldValidator
                        ID="requiredCode"
                        runat="server"
                        ControlToValidate="txtCode"
                        ValidationGroup="WarehouseForm"
                        ErrorMessage="كود المستودع مطلوب."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label class="form-label">اسم المستودع بالعربية</label>

                    <asp:TextBox
                        ID="txtNameAr"
                        runat="server"
                        CssClass="form-control"
                        MaxLength="150" />

                    <asp:RequiredFieldValidator
                        ID="requiredNameAr"
                        runat="server"
                        ControlToValidate="txtNameAr"
                        ValidationGroup="WarehouseForm"
                        ErrorMessage="اسم المستودع بالعربية مطلوب."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label class="form-label">اسم المستودع بالإنجليزية</label>

                    <asp:TextBox
                        ID="txtNameEn"
                        runat="server"
                        CssClass="form-control"
                        MaxLength="150" />

                    <asp:RequiredFieldValidator
                        ID="requiredNameEn"
                        runat="server"
                        ControlToValidate="txtNameEn"
                        ValidationGroup="WarehouseForm"
                        ErrorMessage="اسم المستودع بالإنجليزية مطلوب."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label class="form-label">العنوان بالعربية</label>

                    <asp:TextBox
                        ID="txtAddressAr"
                        runat="server"
                        CssClass="form-control"
                        MaxLength="250"
                        TextMode="MultiLine"
                        Rows="3" />
                </div>

                <div class="mb-3">
                    <label class="form-label">العنوان بالإنجليزية</label>

                    <asp:TextBox
                        ID="txtAddressEn"
                        runat="server"
                        CssClass="form-control"
                        MaxLength="250"
                        TextMode="MultiLine"
                        Rows="3" />
                </div>

                <div class="mb-3">
                    <label class="form-label">رقم الهاتف</label>

                    <asp:TextBox
                        ID="txtPhone"
                        runat="server"
                        CssClass="form-control"
                        MaxLength="30" />
                </div>

                <div class="form-check mb-3">
                    <asp:CheckBox
                        ID="chkIsActive"
                        runat="server"
                        Checked="true"
                        CssClass="form-check-input" />

                    <label class="form-check-label" for="<%= chkIsActive.ClientID %>">
                        المستودع نشط
                    </label>
                </div>

                <asp:Button
                    ID="btnSave"
                    runat="server"
                    Text="حفظ"
                    CssClass="btn btn-primary"
                    ValidationGroup="WarehouseForm"
                    OnClick="btnSave_Click" />

                <a href="Index.aspx" class="btn btn-secondary">
                    إلغاء
                </a>

            </div>
        </div>

    </div>

</asp:Content>