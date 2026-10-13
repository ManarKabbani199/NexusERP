<%@ Page Title="إنشاء استلام مشتريات"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Create.aspx.cs"
    Inherits="NexusERP.Web.PurchaseReceipts.Create" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <div class="card shadow-sm">

            <div class="card-header">
                <h3 class="mb-0">
                    إنشاء استلام مشتريات
                </h3>
            </div>

            <div class="card-body">

                <asp:Label
                    ID="lblMessage"
                    runat="server"
                    Visible="false" />

                <asp:ValidationSummary
                    ID="validationSummary"
                    runat="server"
                    ValidationGroup="ReceiptForm"
                    CssClass="alert alert-danger"
                    HeaderText="يرجى تصحيح الأخطاء التالية:" />

                <div class="row">

                    <div class="col-md-3 mb-3">
                        <label class="form-label">
                            رقم مستند الاستلام
                        </label>

                        <asp:TextBox
                            ID="txtReceiptNumber"
                            runat="server"
                            CssClass="form-control"
                            MaxLength="30" />

                        <asp:RequiredFieldValidator
                            ID="requiredReceiptNumber"
                            runat="server"
                            ControlToValidate="txtReceiptNumber"
                            ValidationGroup="ReceiptForm"
                            ErrorMessage="رقم مستند الاستلام مطلوب."
                            CssClass="text-danger"
                            Display="Dynamic" />
                    </div>

                    <div class="col-md-3 mb-3">
                        <label class="form-label">
                            تاريخ الاستلام
                        </label>

                        <asp:TextBox
                            ID="txtReceiptDate"
                            runat="server"
                            TextMode="Date"
                            CssClass="form-control" />

                        <asp:RequiredFieldValidator
                            ID="requiredReceiptDate"
                            runat="server"
                            ControlToValidate="txtReceiptDate"
                            ValidationGroup="ReceiptForm"
                            ErrorMessage="تاريخ الاستلام مطلوب."
                            CssClass="text-danger"
                            Display="Dynamic" />
                    </div>

                    <div class="col-md-3 mb-3">
                        <label class="form-label">
                            أمر الشراء
                        </label>

                        <asp:DropDownList
                            ID="ddlPurchaseOrder"
                            runat="server"
                            CssClass="form-select"
                            AutoPostBack="true"
                            OnSelectedIndexChanged=
                                "ddlPurchaseOrder_SelectedIndexChanged" />

                        <asp:RequiredFieldValidator
                            ID="requiredPurchaseOrder"
                            runat="server"
                            ControlToValidate="ddlPurchaseOrder"
                            InitialValue=""
                            ValidationGroup="ReceiptForm"
                            ErrorMessage="يجب اختيار أمر الشراء."
                            CssClass="text-danger"
                            Display="Dynamic" />
                    </div>

                    <div class="col-md-3 mb-3">
                        <label class="form-label">
                            مستودع الاستلام
                        </label>

                        <asp:DropDownList
                            ID="ddlWarehouse"
                            runat="server"
                            CssClass="form-select" />

                        <asp:RequiredFieldValidator
                            ID="requiredWarehouse"
                            runat="server"
                            ControlToValidate="ddlWarehouse"
                            InitialValue=""
                            ValidationGroup="ReceiptForm"
                            ErrorMessage="يجب اختيار المستودع."
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

                <h5 class="mb-3">
                    كميات الأصناف المستلمة
                </h5>

                <asp:GridView
                    ID="gvReceiptItems"
                    runat="server"
                    AutoGenerateColumns="False"
                    DataKeyNames="PurchaseOrderItemId"
                    CssClass="table table-striped table-bordered"
                    EmptyDataText="اختاري أمر شراء معتمدًا لعرض الأصناف.">

                    <Columns>

                        <asp:BoundField
                            DataField="ItemCode"
                            HeaderText="رمز الصنف" />

                        <asp:BoundField
                            DataField="ItemName"
                            HeaderText="اسم الصنف" />

                        <asp:BoundField
                            DataField="OrderedQuantity"
                            HeaderText="الكمية المطلوبة"
                            DataFormatString="{0:N2}" />

                        <asp:BoundField
                            DataField="ReceivedQuantity"
                            HeaderText="المستلم سابقًا"
                            DataFormatString="{0:N2}" />

                        <asp:BoundField
                            DataField="RemainingQuantity"
                            HeaderText="الكمية المتبقية"
                            DataFormatString="{0:N2}" />

                        <asp:TemplateField
                            HeaderText="الكمية المستلمة الآن">

                            <ItemTemplate>
                                <asp:TextBox
                                    ID="txtReceivedQuantity"
                                    runat="server"
                                    TextMode="Number"
                                    step="0.01"
                                    min="0"
                                    CssClass="form-control"
                                    placeholder="0.00" />
                            </ItemTemplate>

                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>

                <div class="d-flex gap-2 mt-3">

                    <asp:Button
                        ID="btnSaveDraft"
                        runat="server"
                        Text="حفظ كمسودة"
                        CssClass="btn btn-primary"
                        ValidationGroup="ReceiptForm"
                        OnClick="btnSaveDraft_Click" />

                    <a href="Index.aspx"
                       class="btn btn-secondary">
                        إلغاء
                    </a>

                </div>

            </div>
        </div>
    </div>

</asp:Content>