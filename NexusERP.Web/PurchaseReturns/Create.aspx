<%@ Page Title="إنشاء مرتجع مشتريات"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Create.aspx.cs"
    Inherits="NexusERP.Web.PurchaseReturns.Create" %>

<asp:Content
    ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <div class="d-flex justify-content-between
                    align-items-center mb-3">

            <h2 class="mb-0">
                إنشاء مرتجع مشتريات
            </h2>

            <a href="Index.aspx"
               class="btn btn-secondary">
                العودة إلى القائمة
            </a>

        </div>

        <asp:Label
            ID="lblMessage"
            runat="server"
            Visible="false" />

        <div class="card shadow-sm">

            <div class="card-header">
                <h5 class="mb-0">
                    بيانات مرتجع المشتريات
                </h5>
            </div>

            <div class="card-body">

                <asp:ValidationSummary
                    ID="validationSummary"
                    runat="server"
                    ValidationGroup="PurchaseReturn"
                    CssClass="alert alert-danger"
                    HeaderText="يرجى تصحيح الأخطاء التالية:" />

                <div class="row">

                    <div class="col-md-4 mb-3">

                        <label class="form-label">
                            رقم المرتجع
                        </label>

                        <asp:TextBox
                            ID="txtReturnNumber"
                            runat="server"
                            MaxLength="30"
                            CssClass="form-control" />

                        <asp:RequiredFieldValidator
                            ID="requiredReturnNumber"
                            runat="server"
                            ControlToValidate="txtReturnNumber"
                            ValidationGroup="PurchaseReturn"
                            ErrorMessage="رقم المرتجع مطلوب."
                            CssClass="text-danger"
                            Display="Dynamic" />

                    </div>

                    <div class="col-md-4 mb-3">

                        <label class="form-label">
                            تاريخ المرتجع
                        </label>

                        <asp:TextBox
                            ID="txtReturnDate"
                            runat="server"
                            TextMode="Date"
                            CssClass="form-control" />

                        <asp:RequiredFieldValidator
                            ID="requiredReturnDate"
                            runat="server"
                            ControlToValidate="txtReturnDate"
                            ValidationGroup="PurchaseReturn"
                            ErrorMessage="تاريخ المرتجع مطلوب."
                            CssClass="text-danger"
                            Display="Dynamic" />

                    </div>

                    <div class="col-md-4 mb-3">

                        <label class="form-label">
                            مستند الاستلام
                        </label>

                        <asp:DropDownList
                            ID="ddlPurchaseReceipt"
                            runat="server"
                            CssClass="form-select"
                            AutoPostBack="true"
                            OnSelectedIndexChanged=
                                "ddlPurchaseReceipt_SelectedIndexChanged" />

                        <asp:RequiredFieldValidator
                            ID="requiredPurchaseReceipt"
                            runat="server"
                            ControlToValidate="ddlPurchaseReceipt"
                            InitialValue=""
                            ValidationGroup="PurchaseReturn"
                            ErrorMessage="يجب اختيار مستند الاستلام."
                            CssClass="text-danger"
                            Display="Dynamic" />

                    </div>

                </div>

                <div class="row">

                    <div class="col-md-6 mb-3">

                        <label class="form-label">
                            المورد
                        </label>

                        <asp:TextBox
                            ID="txtSupplier"
                            runat="server"
                            ReadOnly="true"
                            CssClass="form-control" />

                    </div>

                    <div class="col-md-6 mb-3">

                        <label class="form-label">
                            المستودع
                        </label>

                        <asp:TextBox
                            ID="txtWarehouse"
                            runat="server"
                            ReadOnly="true"
                            CssClass="form-control" />

                    </div>

                </div>

                <div class="mb-3">

                    <label class="form-label">
                        ملاحظات
                    </label>

                    <asp:TextBox
                        ID="txtNotes"
                        runat="server"
                        TextMode="MultiLine"
                        Rows="3"
                        MaxLength="500"
                        CssClass="form-control" />

                </div>

                <hr />

                <h5 class="mb-3">
                    الأصناف المتاحة للإرجاع
                </h5>

                <div class="table-responsive">

                    <asp:GridView
                        ID="gvReturnItems"
                        runat="server"
                        AutoGenerateColumns="False"
                        CssClass="table table-striped table-bordered"
                        EmptyDataText="اختر مستند استلام لعرض الأصناف."
                        DataKeyNames="PurchaseReceiptItemId">

                        <Columns>

                            <asp:BoundField
                                DataField="ItemCode"
                                HeaderText="رمز الصنف" />

                            <asp:BoundField
                                DataField="ItemName"
                                HeaderText="اسم الصنف" />

                            <asp:BoundField
                                DataField="ReceivedQuantity"
                                HeaderText="الكمية المستلمة"
                                DataFormatString="{0:N2}" />

                            <asp:BoundField
                                DataField="PreviouslyReturned"
                                HeaderText="المرتجع سابقًا"
                                DataFormatString="{0:N2}" />

                            <asp:BoundField
                                DataField="RemainingQuantity"
                                HeaderText="المتاح للإرجاع"
                                DataFormatString="{0:N2}" />

                            <asp:BoundField
                                DataField="UnitPrice"
                                HeaderText="سعر الوحدة"
                                DataFormatString="{0:N2}" />

                            <asp:TemplateField
                                HeaderText="الكمية المرتجعة الآن">

                                <ItemTemplate>

                                    <asp:HiddenField
                                        ID="hiddenUnitPrice"
                                        runat="server"
                                        Value='<%# Eval("UnitPrice") %>' />

                                    <asp:HiddenField
                                        ID="hiddenRemainingQuantity"
                                        runat="server"
                                        Value='<%# Eval("RemainingQuantity") %>' />

                                    <asp:TextBox
                                        ID="txtReturnedQuantity"
                                        runat="server"
                                        Text="0"
                                        TextMode="Number"
                                        CssClass="form-control"
                                        step="0.01"
                                        min="0" />

                                </ItemTemplate>

                            </asp:TemplateField>

                        </Columns>

                    </asp:GridView>

                </div>

                <div class="d-flex gap-2 mt-3">

                    <asp:Button
                        ID="btnSave"
                        runat="server"
                        Text="حفظ كمسودة"
                        CssClass="btn btn-primary"
                        ValidationGroup="PurchaseReturn"
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