<%@ Page Title="تعديل رصيد المستودع"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Edit.aspx.cs"
    Inherits="NexusERP.Web.WarehouseStocks.Edit" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <div class="card shadow-sm">

            <div class="card-header">
                <h3 class="mb-0">تعديل رصيد المستودع</h3>
            </div>

            <div class="card-body">

                <asp:ValidationSummary
                    ID="validationSummary"
                    runat="server"
                    ValidationGroup="WarehouseStockForm"
                    CssClass="alert alert-danger"
                    HeaderText="يرجى تصحيح الأخطاء التالية:" />

                <div class="mb-3">
                    <label class="form-label">المستودع</label>

                    <asp:DropDownList
                        ID="ddlWarehouse"
                        runat="server"
                        CssClass="form-select" />

                    <asp:RequiredFieldValidator
                        ID="requiredWarehouse"
                        runat="server"
                        ControlToValidate="ddlWarehouse"
                        InitialValue=""
                        ValidationGroup="WarehouseStockForm"
                        ErrorMessage="يرجى اختيار المستودع."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label class="form-label">الصنف</label>

                    <asp:DropDownList
                        ID="ddlItem"
                        runat="server"
                        CssClass="form-select" />

                    <asp:RequiredFieldValidator
                        ID="requiredItem"
                        runat="server"
                        ControlToValidate="ddlItem"
                        InitialValue=""
                        ValidationGroup="WarehouseStockForm"
                        ErrorMessage="يرجى اختيار الصنف."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label class="form-label">الكمية الحالية</label>

                    <asp:TextBox
                        ID="txtQuantity"
                        runat="server"
                        CssClass="form-control"
                        TextMode="Number"
                        step="0.01"
                        min="0" />

                    <asp:RequiredFieldValidator
                        ID="requiredQuantity"
                        runat="server"
                        ControlToValidate="txtQuantity"
                        ValidationGroup="WarehouseStockForm"
                        ErrorMessage="الكمية مطلوبة."
                        CssClass="text-danger"
                        Display="Dynamic" />

                    <asp:CompareValidator
                        ID="compareQuantity"
                        runat="server"
                        ControlToValidate="txtQuantity"
                        Operator="GreaterThanEqual"
                        ValueToCompare="0"
                        Type="Double"
                        ValidationGroup="WarehouseStockForm"
                        ErrorMessage="الكمية لا يمكن أن تكون سالبة."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="alert alert-warning">
                    تعديل الكمية مباشرة مسموح مؤقتًا خلال مرحلة إدخال
                    الأرصدة الافتتاحية. لاحقًا ستتغير الكمية من خلال
                    حركات الاستلام والصرف والتحويل والجرد.
                </div>

                <div class="d-flex gap-2">

                    <asp:Button
                        ID="btnSave"
                        runat="server"
                        Text="حفظ التعديلات"
                        OnClick="btnSave_Click"
                        CssClass="btn btn-primary"
                        ValidationGroup="WarehouseStockForm" />

                    <asp:HyperLink
                        ID="lnkCancel"
                        runat="server"
                        NavigateUrl="~/WarehouseStocks/Index.aspx"
                        CssClass="btn btn-secondary">
                        إلغاء
                    </asp:HyperLink>

                </div>

            </div>
        </div>
    </div>

</asp:Content>