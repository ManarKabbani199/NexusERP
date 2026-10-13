<%@ Page Title="تعديل الصنف"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Edit.aspx.cs"
    Inherits="NexusERP.Web.Items.Edit" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <div class="card shadow-sm">

            <div class="card-header">
                <h3 class="mb-0">تعديل بيانات الصنف</h3>
            </div>

            <div class="card-body">

                <asp:ValidationSummary
                    ID="validationSummary"
                    runat="server"
                    ValidationGroup="ItemForm"
                    CssClass="alert alert-danger"
                    HeaderText="يرجى تصحيح الأخطاء التالية:" />

                <div class="mb-3">
                    <label class="form-label">تصنيف الصنف</label>

                    <asp:DropDownList
                        ID="ddlItemCategory"
                        runat="server"
                        CssClass="form-select" />

                    <asp:RequiredFieldValidator
                        ID="requiredItemCategory"
                        runat="server"
                        ControlToValidate="ddlItemCategory"
                        InitialValue=""
                        ValidationGroup="ItemForm"
                        ErrorMessage="يرجى اختيار تصنيف الصنف."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label class="form-label">رمز الصنف</label>

                    <asp:TextBox
                        ID="txtCode"
                        runat="server"
                        CssClass="form-control"
                        MaxLength="30" />

                    <asp:RequiredFieldValidator
                        ID="requiredCode"
                        runat="server"
                        ControlToValidate="txtCode"
                        ValidationGroup="ItemForm"
                        ErrorMessage="رمز الصنف مطلوب."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label class="form-label">الاسم العربي</label>

                    <asp:TextBox
                        ID="txtNameAr"
                        runat="server"
                        CssClass="form-control"
                        MaxLength="150" />

                    <asp:RequiredFieldValidator
                        ID="requiredNameAr"
                        runat="server"
                        ControlToValidate="txtNameAr"
                        ValidationGroup="ItemForm"
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
                        ValidationGroup="ItemForm"
                        ErrorMessage="الاسم الإنجليزي مطلوب."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label class="form-label">وحدة القياس</label>

                    <asp:DropDownList
                        ID="ddlUnitOfMeasure"
                        runat="server"
                        CssClass="form-select">

                        <asp:ListItem
                            Text="-- اختر وحدة القياس --"
                            Value="" />

                        <asp:ListItem Text="قطعة" Value="Piece" />
                        <asp:ListItem Text="صندوق" Value="Box" />
                        <asp:ListItem Text="كرتون" Value="Carton" />
                        <asp:ListItem Text="كيلوغرام" Value="Kilogram" />
                        <asp:ListItem Text="لتر" Value="Liter" />

                    </asp:DropDownList>

                    <asp:RequiredFieldValidator
                        ID="requiredUnitOfMeasure"
                        runat="server"
                        ControlToValidate="ddlUnitOfMeasure"
                        InitialValue=""
                        ValidationGroup="ItemForm"
                        ErrorMessage="وحدة القياس مطلوبة."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label class="form-label">الحد الأدنى للمخزون</label>

                    <asp:TextBox
                        ID="txtMinimumStockLevel"
                        runat="server"
                        CssClass="form-control"
                        TextMode="Number"
                        step="0.01"
                        min="0" />

                    <asp:RequiredFieldValidator
                        ID="requiredMinimumStockLevel"
                        runat="server"
                        ControlToValidate="txtMinimumStockLevel"
                        ValidationGroup="ItemForm"
                        ErrorMessage="الحد الأدنى للمخزون مطلوب."
                        CssClass="text-danger"
                        Display="Dynamic" />

                    <asp:CompareValidator
                        ID="compareMinimumStockLevel"
                        runat="server"
                        ControlToValidate="txtMinimumStockLevel"
                        Operator="GreaterThanEqual"
                        ValueToCompare="0"
                        Type="Double"
                        ValidationGroup="ItemForm"
                        ErrorMessage="يجب أن يكون الحد الأدنى صفرًا أو أكبر."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="mb-3">

                    <asp:CheckBox
                        ID="chkIsActive"
                        runat="server"
                        Text=" الصنف نشط" />

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
                        Text="حفظ التعديلات"
                         OnClick="btnSave_Click"
                        CssClass="btn btn-primary"
                        ValidationGroup="ItemForm" />

                    <asp:HyperLink
                        ID="lnkCancel"
                        runat="server"
                        NavigateUrl="~/Items/Index.aspx"
                        CssClass="btn btn-secondary">
                        إلغاء
                    </asp:HyperLink>

                </div>

            </div>
        </div>
    </div>

</asp:Content>