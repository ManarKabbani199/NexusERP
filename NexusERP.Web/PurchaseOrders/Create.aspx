<%@ Page Title="إنشاء أمر شراء"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Create.aspx.cs"
    Inherits="NexusERP.Web.PurchaseOrders.Create" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <div class="card shadow-sm">
            <div class="card-header">
                <h3 class="mb-0">إنشاء أمر شراء</h3>
            </div>

            <div class="card-body">

                <asp:Label
                    ID="lblMessage"
                    runat="server"
                    Visible="false" />

                <asp:ValidationSummary
                    ID="validationSummary"
                    runat="server"
                    ValidationGroup="PurchaseOrderForm"
                    CssClass="alert alert-danger"
                    HeaderText="يرجى تصحيح الأخطاء التالية:" />

                <div class="row">

                    <div class="col-md-4 mb-3">
                        <label class="form-label">
                            رقم أمر الشراء
                        </label>

                        <asp:TextBox
                            ID="txtOrderNumber"
                            runat="server"
                            CssClass="form-control"
                            MaxLength="30" />

                        <asp:RequiredFieldValidator
                            ID="requiredOrderNumber"
                            runat="server"
                            ControlToValidate="txtOrderNumber"
                            ValidationGroup="PurchaseOrderForm"
                            ErrorMessage="رقم أمر الشراء مطلوب."
                            CssClass="text-danger"
                            Display="Dynamic" />
                    </div>

                    <div class="col-md-4 mb-3">
                        <label class="form-label">
                            تاريخ أمر الشراء
                        </label>

                        <asp:TextBox
                            ID="txtOrderDate"
                            runat="server"
                            TextMode="Date"
                            CssClass="form-control" />

                        <asp:RequiredFieldValidator
                            ID="requiredOrderDate"
                            runat="server"
                            ControlToValidate="txtOrderDate"
                            ValidationGroup="PurchaseOrderForm"
                            ErrorMessage="تاريخ أمر الشراء مطلوب."
                            CssClass="text-danger"
                            Display="Dynamic" />
                    </div>

                    <div class="col-md-4 mb-3">
                        <label class="form-label">المورد</label>

                        <asp:DropDownList
                            ID="ddlSupplier"
                            runat="server"
                            CssClass="form-select" />

                        <asp:RequiredFieldValidator
                            ID="requiredSupplier"
                            runat="server"
                            ControlToValidate="ddlSupplier"
                            InitialValue=""
                            ValidationGroup="PurchaseOrderForm"
                            ErrorMessage="يجب اختيار المورد."
                            CssClass="text-danger"
                            Display="Dynamic" />
                    </div>

                </div>

                <div class="mb-3">
                    <label class="form-label">ملاحظات</label>

                    <asp:TextBox
                        ID="txtNotes"
                        runat="server"
                        TextMode="MultiLine"
                        Rows="3"
                        MaxLength="500"
                        CssClass="form-control" />
                </div>

                <hr />

                <h5 class="mb-3">إضافة أصناف أمر الشراء</h5>

                <div class="row align-items-end">

                    <div class="col-md-4 mb-3">
                        <label class="form-label">الصنف</label>

                        <asp:DropDownList
                            ID="ddlItem"
                            runat="server"
                            CssClass="form-select" />
                    </div>

                    <div class="col-md-2 mb-3">
                        <label class="form-label">الكمية</label>

                        <asp:TextBox
                            ID="txtQuantity"
                            runat="server"
                            TextMode="Number"
                            step="0.01"
                            CssClass="form-control" />
                    </div>

                    <div class="col-md-2 mb-3">
                        <label class="form-label">سعر الوحدة</label>

                        <asp:TextBox
                            ID="txtUnitPrice"
                            runat="server"
                            TextMode="Number"
                            step="0.01"
                            CssClass="form-control" />
                    </div>

                    <div class="col-md-2 mb-3">
                        <asp:Button
                            ID="btnAddItem"
                            runat="server"
                            Text="إضافة الصنف"
                            CssClass="btn btn-outline-primary w-100"
                            CausesValidation="false"
                            OnClick="btnAddItem_Click" />
                    </div>

                </div>

                <asp:GridView
                    ID="gvOrderItems"
                    runat="server"
                    AutoGenerateColumns="False"
                    CssClass="table table-striped table-bordered"
                    EmptyDataText="لم تتم إضافة أصناف إلى أمر الشراء."
                    OnRowCommand="gvOrderItems_RowCommand">

                    <Columns>

                        <asp:BoundField
                            DataField="ItemName"
                            HeaderText="الصنف" />

                        <asp:BoundField
                            DataField="Quantity"
                            HeaderText="الكمية"
                            DataFormatString="{0:N2}" />

                        <asp:BoundField
                            DataField="UnitPrice"
                            HeaderText="سعر الوحدة"
                            DataFormatString="{0:N2}" />

                        <asp:BoundField
                            DataField="LineTotal"
                            HeaderText="الإجمالي"
                            DataFormatString="{0:N2}" />

                        <asp:TemplateField HeaderText="الإجراء">
                            <ItemTemplate>
                                <asp:LinkButton
                                    ID="btnRemoveItem"
                                    runat="server"
                                    Text="إزالة"
                                    CssClass="btn btn-sm btn-danger"
                                    CausesValidation="false"
                                    CommandName="RemoveItem"
                                    CommandArgument='<%# Eval("ItemId") %>'
                                    OnClientClick="return confirm('هل تريد إزالة الصنف؟');" />
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>

                <div class="text-end mb-3">
                    <strong>الإجمالي الكلي: </strong>

                    <asp:Label
                        ID="lblGrandTotal"
                        runat="server"
                        Text="0.00" />
                </div>

                <div class="d-flex gap-2">

                    <asp:Button
                        ID="btnSave"
                        runat="server"
                        Text="حفظ أمر الشراء"
                        CssClass="btn btn-primary"
                        ValidationGroup="PurchaseOrderForm"
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