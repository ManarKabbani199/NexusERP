<%@ Page Title="تفاصيل أمر الشراء"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Details.aspx.cs"
    Inherits="NexusERP.Web.PurchaseOrders.Details" %>

<asp:Content
    ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <!-- عنوان الصفحة -->
        <div class="d-flex justify-content-between
                    align-items-center mb-3">

            <h2 class="mb-0">
                تفاصيل أمر الشراء
            </h2>

            <a href="Index.aspx"
               class="btn btn-secondary">
                العودة إلى القائمة
            </a>

        </div>

        <!-- رسائل النجاح والخطأ -->
        <asp:Label
            ID="lblMessage"
            runat="server"
            Visible="false" />

        <asp:Panel
            ID="pnlDetails"
            runat="server"
            Visible="false">

            <!-- بيانات أمر الشراء -->
            <div class="card shadow-sm mb-4">

                <div class="card-header">
                    <h5 class="mb-0">
                        بيانات أمر الشراء
                    </h5>
                </div>

                <div class="row mt-3">
    <div class="col-md-6">
        <strong>تم الاعتماد بواسطة</strong>
        <div>
            <asp:Label ID="lblApprovedBy" runat="server" />
        </div>
    </div>

    <div class="col-md-6">
        <strong>تاريخ الاعتماد</strong>
        <div>
            <asp:Label ID="lblApprovedAt" runat="server" />
        </div>
    </div>
</div>

                <div class="card-body">

                    <div class="row">

                        <div class="col-md-4 mb-3">

                            <strong>
                                حالة أمر الشراء
                            </strong>

                            <div>
                                <asp:Label
                                    ID="lblStatus"
                                    runat="server"
                                    CssClass="badge bg-secondary" />
                            </div>

                        </div>

                        <div class="col-md-4 mb-3">

                            <strong>
                                رقم أمر الشراء
                            </strong>

                            <div>
                                <asp:Label
                                    ID="lblOrderNumber"
                                    runat="server" />
                            </div>

                        </div>

                        <div class="col-md-4 mb-3">

                            <strong>
                                تاريخ أمر الشراء
                            </strong>

                            <div>
                                <asp:Label
                                    ID="lblOrderDate"
                                    runat="server" />
                            </div>

                        </div>

                    </div>

                    <div class="row">

                        <div class="col-md-4 mb-3">

                            <strong>
                                المورد
                            </strong>

                            <div>
                                <asp:Label
                                    ID="lblSupplierName"
                                    runat="server" />
                            </div>

                        </div>

                        <div class="col-md-4 mb-3">

                            <strong>
                                جهة الاتصال
                            </strong>

                            <div>
                                <asp:Label
                                    ID="lblContactPerson"
                                    runat="server" />
                            </div>

                        </div>

                        <div class="col-md-4 mb-3">

                            <strong>
                                هاتف المورد
                            </strong>

                            <div>
                                <asp:Label
                                    ID="lblSupplierPhone"
                                    runat="server" />
                            </div>

                        </div>

                    </div>

                    <div class="row">

                        <div class="col-md-6 mb-3">

                            <strong>
                                البريد الإلكتروني
                            </strong>

                            <div>
                                <asp:Label
                                    ID="lblSupplierEmail"
                                    runat="server" />
                            </div>

                        </div>

                        <div class="col-md-6 mb-3">

                            <strong>
                                الملاحظات
                            </strong>

                            <div>
                                <asp:Label
                                    ID="lblNotes"
                                    runat="server" />
                            </div>

                        </div>

                    </div>

                    <!-- بيانات إلغاء أمر الشراء -->
                    <asp:Panel
                        ID="pnlCancellationDetails"
                        runat="server"
                        Visible="false"
                        CssClass="alert alert-danger mt-3">

                        <h6 class="mb-3">
                            بيانات إلغاء أمر الشراء
                        </h6>

                        <div class="mb-2">

                            <strong>
                                تاريخ الإلغاء:
                            </strong>

                            <asp:Label
                                ID="lblCancelledAt"
                                runat="server" />

                        </div>

                        <div class="mb-2">

                            <strong>
                                تم الإلغاء بواسطة:
                            </strong>

                            <asp:Label
                                ID="lblCancelledBy"
                                runat="server" />

                        </div>

                        <div>

                            <strong>
                                سبب الإلغاء:
                            </strong>

                            <asp:Label
                                ID="lblCancellationReason"
                                runat="server" />

                        </div>

                    </asp:Panel>

                </div>

            </div>

            <!-- أصناف أمر الشراء -->
            <div class="card shadow-sm mb-4">

                <div class="card-header">

                    <h5 class="mb-0">
                        أصناف أمر الشراء
                    </h5>

                </div>

                <div class="card-body">

                    <asp:GridView
                        ID="gvOrderItems"
                        runat="server"
                        AutoGenerateColumns="False"
                        CssClass="table table-striped table-bordered"
                        EmptyDataText="لا توجد أصناف في أمر الشراء.">

                        <Columns>

                            <asp:BoundField
                                DataField="ItemCode"
                                HeaderText="رمز الصنف" />

                            <asp:BoundField
                                DataField="ItemName"
                                HeaderText="اسم الصنف" />

                            <asp:BoundField
                                DataField="Quantity"
                                HeaderText="الكمية المطلوبة"
                                DataFormatString="{0:N2}" />

                            <asp:BoundField
                                DataField="ReceivedQuantity"
                                HeaderText="إجمالي المستلم"
                                DataFormatString="{0:N2}" />

                            <asp:BoundField
                                DataField="RemainingQuantity"
                                HeaderText="الكمية المتبقية"
                                DataFormatString="{0:N2}" />

                            <asp:BoundField
    DataField="InvoicedQuantity"
    HeaderText="الكمية المفوترة"
    DataFormatString="{0:N2}" />

<asp:BoundField
    DataField="AvailableToInvoice"
    HeaderText="المتاح للفوترة"
    DataFormatString="{0:N2}" />

                            <asp:BoundField
                                DataField="UnitPrice"
                                HeaderText="سعر الوحدة"
                                DataFormatString="{0:N2}" />

                            <asp:BoundField
                                DataField="LineTotal"
                                HeaderText="الإجمالي"
                                DataFormatString="{0:N2}" />

                        </Columns>

                    </asp:GridView>

                    <div class="text-end mt-3">

                        <h5>
                            الإجمالي الكلي:

                            <asp:Label
                                ID="lblGrandTotal"
                                runat="server"
                                Text="0.00" />
                        </h5>

                    </div>

                </div>

            </div>

            <!-- سجل استلامات أمر الشراء -->
            <div class="card shadow-sm mb-4">

                <div class="card-header">

                    <h5 class="mb-0">
                        سجل استلامات أمر الشراء
                    </h5>

                </div>

                <div class="card-body">

                    <asp:GridView
                        ID="gvPurchaseReceipts"
                        runat="server"
                        AutoGenerateColumns="False"
                        CssClass="table table-striped table-bordered"
                        EmptyDataText="لا توجد مستندات استلام لأمر الشراء.">

                        <Columns>

                            <asp:BoundField
                                DataField="ReceiptNumber"
                                HeaderText="رقم الاستلام" />

                            <asp:BoundField
                                DataField="ReceiptDate"
                                HeaderText="تاريخ الاستلام"
                                DataFormatString="{0:yyyy-MM-dd}" />

                            <asp:BoundField
                                DataField="WarehouseName"
                                HeaderText="المستودع" />

                            <asp:BoundField
                                DataField="TotalQuantity"
                                HeaderText="الكمية المستلمة"
                                DataFormatString="{0:N2}" />

                            <asp:BoundField
                                DataField="StatusName"
                                HeaderText="الحالة" />

                            <asp:HyperLinkField
                                HeaderText="الإجراءات"
                                Text="التفاصيل"
                                DataNavigateUrlFields="Id"
                                DataNavigateUrlFormatString=
                                    "~/PurchaseReceipts/Details.aspx?id={0}"
                                ControlStyle-CssClass=
                                    "btn btn-info btn-sm" />

                        </Columns>

                    </asp:GridView>

                </div>

            </div>

            <div class="card shadow-sm mt-4">

    <div class="card-header">
        <h5 class="mb-0">
            سجل مرتجعات أمر الشراء
        </h5>
    </div>

    <div class="card-body">

        <div class="table-responsive">

            <asp:GridView
                ID="gvPurchaseReturns"
                runat="server"
                AutoGenerateColumns="False"
                CssClass="table table-striped table-bordered"
                EmptyDataText="لا توجد مرتجعات مرتبطة بأمر الشراء.">

                <Columns>

                    <asp:BoundField
                        DataField="ReturnNumber"
                        HeaderText="رقم المرتجع" />

                    <asp:BoundField
                        DataField="ReturnDate"
                        HeaderText="تاريخ المرتجع"
                        DataFormatString="{0:yyyy-MM-dd}" />

                    <asp:BoundField
                        DataField="ReceiptNumber"
                        HeaderText="مستند الاستلام" />

                    <asp:BoundField
                        DataField="WarehouseName"
                        HeaderText="المستودع" />

                    <asp:BoundField
                        DataField="TotalQuantity"
                        HeaderText="الكمية المرتجعة"
                        DataFormatString="{0:N2}" />

                    <asp:BoundField
                        DataField="TotalValue"
                        HeaderText="قيمة المرتجع"
                        DataFormatString="{0:N2}" />

                    <asp:BoundField
                        DataField="StatusName"
                        HeaderText="الحالة" />

                    <asp:TemplateField
                        HeaderText="الإجراءات">

                        <ItemTemplate>

                            <a href='<%#
                                "~/PurchaseReturns/Details.aspx?id=" +
                                Eval("Id") %>'
                               runat="server"
                               class="btn btn-info btn-sm">

                                التفاصيل

                            </a>

                        </ItemTemplate>

                    </asp:TemplateField>

                </Columns>

            </asp:GridView>

        </div>

    </div>

</div>

            <!-- سجل فواتير أمر الشراء -->
<div class="card shadow-sm mb-4">

    <div class="card-header">

        <h5 class="mb-0">
            سجل فواتير أمر الشراء
        </h5>

    </div>

    <div class="card-body">

        <asp:GridView
            ID="gvPurchaseInvoices"
            runat="server"
            AutoGenerateColumns="False"
            CssClass="table table-striped table-bordered"
            EmptyDataText="لا توجد فواتير مرتبطة بأمر الشراء.">

            <Columns>

                <asp:BoundField
                    DataField="InvoiceNumber"
                    HeaderText="رقم الفاتورة" />

                <asp:BoundField
                    DataField="InvoiceDate"
                    HeaderText="تاريخ الفاتورة"
                    DataFormatString="{0:yyyy-MM-dd}" />

                <asp:BoundField
                    DataField="DueDate"
                    HeaderText="تاريخ الاستحقاق"
                    DataFormatString="{0:yyyy-MM-dd}" />

                <asp:BoundField
                    DataField="TotalQuantity"
                    HeaderText="الكمية المفوترة"
                    DataFormatString="{0:N2}" />

                <asp:BoundField
                    DataField="GrandTotal"
                    HeaderText="إجمالي الفاتورة"
                    DataFormatString="{0:N2}" />

                <asp:BoundField
                    DataField="StatusName"
                    HeaderText="الحالة" />

                <asp:HyperLinkField
                    HeaderText="الإجراءات"
                    Text="التفاصيل"
                    DataNavigateUrlFields="Id"
                    DataNavigateUrlFormatString=
                        "~/PurchaseInvoices/Details.aspx?id={0}"
                    ControlStyle-CssClass=
                        "btn btn-info btn-sm" />

            </Columns>

        </asp:GridView>

    </div>

</div>

            <!-- أزرار الإجراءات -->
            <div class="d-flex flex-wrap gap-2 mt-3">

                <asp:Button
                    ID="btnApprove"
                    runat="server"
                    Text="اعتماد أمر الشراء"
                    CssClass="btn btn-success"
                    Visible="false"
                    CausesValidation="false"
                    OnClick="btnApprove_Click"
                    OnClientClick=
                        "return confirm('هل تريد اعتماد أمر الشراء؟');" />

                <asp:HyperLink
                    ID="lnkCreateReceipt"
                    runat="server"
                    Text="إنشاء استلام"
                    CssClass="btn btn-primary"
                    Visible="false" />

                <asp:HyperLink
                    ID="lnkCreateInvoice"
                    runat="server"
                    Text="إنشاء فاتورة مشتريات"
                    CssClass="btn btn-warning"
                    Visible="false" />

                <asp:Button
                    ID="btnCancel"
                    runat="server"
                    Text="إلغاء أمر الشراء"
                    CssClass="btn btn-danger"
                    Visible="false"
                    CausesValidation="false"
                    OnClick="btnCancel_Click" />

            </div>

            <!-- نموذج إلغاء أمر الشراء -->
            <asp:Panel
                ID="pnlCancelForm"
                runat="server"
                Visible="false"
                CssClass="border rounded p-3 mt-3">

                <h6 class="mb-3">
                    إلغاء أمر الشراء
                </h6>

                <div class="mb-3">

                    <label class="form-label">
                        سبب الإلغاء
                    </label>

                    <asp:TextBox
                        ID="txtCancellationReason"
                        runat="server"
                        TextMode="MultiLine"
                        Rows="3"
                        MaxLength="500"
                        CssClass="form-control"
                        placeholder="أدخلي سبب إلغاء أمر الشراء" />

                    <asp:RequiredFieldValidator
                        ID="requiredCancellationReason"
                        runat="server"
                        ControlToValidate="txtCancellationReason"
                        ValidationGroup="CancelPurchaseOrder"
                        ErrorMessage="سبب الإلغاء مطلوب."
                        CssClass="text-danger"
                        Display="Dynamic" />

                </div>

                <div class="d-flex gap-2">

                    <asp:Button
                        ID="btnConfirmCancel"
                        runat="server"
                        Text="تأكيد الإلغاء"
                        CssClass="btn btn-danger"
                        ValidationGroup="CancelPurchaseOrder"
                        OnClick="btnConfirmCancel_Click"
                        OnClientClick=
                            "return confirm('هل أنت متأكد من إلغاء أمر الشراء؟');" />

                    <asp:Button
                        ID="btnCloseCancel"
                        runat="server"
                        Text="تراجع"
                        CssClass="btn btn-secondary"
                        CausesValidation="false"
                        OnClick="btnCloseCancel_Click" />

                </div>

            </asp:Panel>

        </asp:Panel>

    </div>

</asp:Content>