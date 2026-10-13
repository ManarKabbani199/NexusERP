<%@ Page Title="تفاصيل استلام المشتريات"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Details.aspx.cs"
    Inherits="NexusERP.Web.PurchaseReceipts.Details" %>

<asp:Content
    ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <div class="d-flex justify-content-between align-items-center mb-3">

            <h2 class="mb-0">
                تفاصيل استلام المشتريات
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

        <asp:Panel
            ID="pnlDetails"
            runat="server"
            Visible="false">

            <div class="card shadow-sm mb-4">

                <div class="card-header">
                    <h5 class="mb-0">
                        بيانات مستند الاستلام
                    </h5>
                </div>

                <div class="card-body">

                    <div class="row">

                        <div class="col-md-3 mb-3">
                            <strong>رقم الاستلام</strong>

                            <div>
                                <asp:Label
                                    ID="lblReceiptNumber"
                                    runat="server" />
                            </div>
                        </div>

                        <div class="col-md-3 mb-3">
                            <strong>تاريخ الاستلام</strong>

                            <div>
                                <asp:Label
                                    ID="lblReceiptDate"
                                    runat="server" />
                            </div>
                        </div>

                        <div class="col-md-3 mb-3">
                            <strong>الحالة</strong>

                            <div>
                                <asp:Label
                                    ID="lblStatus"
                                    runat="server"
                                    CssClass="badge bg-secondary" />
                            </div>
                        </div>

                        <div class="col-md-3 mb-3">
                            <strong>تاريخ الترحيل</strong>

                            <div>
                                <asp:Label
                                    ID="lblPostedAt"
                                    runat="server" />
                            </div>
                        </div>

                        <div class="col-md-3">
    <strong>تم الترحيل بواسطة</strong>
    <div>
        <asp:Label
            ID="lblPostedBy"
            runat="server" />
    </div>
</div>

                    </div>

                    <div class="row">

                        <div class="col-md-4 mb-3">
                            <strong>أمر الشراء</strong>

                            <div>
                                <asp:Label
                                    ID="lblPurchaseOrder"
                                    runat="server" />
                            </div>
                        </div>

                        <div class="col-md-4 mb-3">
                            <strong>المورد</strong>

                            <div>
                                <asp:Label
                                    ID="lblSupplier"
                                    runat="server" />
                            </div>
                        </div>

                        <div class="col-md-4 mb-3">
                            <strong>مستودع الاستلام</strong>

                            <div>
                                <asp:Label
                                    ID="lblWarehouse"
                                    runat="server" />
                            </div>
                        </div>

                    </div>

                    <div class="mb-3">
                        <strong>الملاحظات</strong>

                        <div>
                            <asp:Label
                                ID="lblNotes"
                                runat="server" />
                        </div>
                    </div>

                    <%-- بيانات الإلغاء --%>

                    <asp:Panel
                        ID="pnlCancellationDetails"
                        runat="server"
                        Visible="false"
                        CssClass="alert alert-danger mt-3">

                        <h6 class="mb-3">
                            بيانات الإلغاء
                        </h6>

                        <div class="mb-2">
                            <strong>تاريخ الإلغاء:</strong>

                            <asp:Label
                                ID="lblCancelledAt"
                                runat="server" />
                        </div>

                        <div class="mb-2">
                            <strong>تم الإلغاء بواسطة:</strong>

                            <asp:Label
                                ID="lblCancelledBy"
                                runat="server" />
                        </div>

                        <div>
                            <strong>سبب الإلغاء:</strong>

                            <asp:Label
                                ID="lblCancellationReason"
                                runat="server" />
                        </div>

                    </asp:Panel>

                    <%-- أزرار العمليات --%>

                    <div class="d-flex gap-2 mt-3">

                        <asp:Button
                            ID="btnPost"
                            runat="server"
                            Text="ترحيل الاستلام"
                            CssClass="btn btn-success"
                            Visible="false"
                            CausesValidation="false"
                            OnClick="btnPost_Click"
                            OnClientClick="return confirm('هل تريد ترحيل الاستلام؟ سيتم زيادة رصيد المستودع.');" />

                        <asp:Button
                            ID="btnShowCancel"
                            runat="server"
                            Text="إلغاء مستند الاستلام"
                            CssClass="btn btn-danger"
                            Visible="false"
                            CausesValidation="false"
                            OnClick="btnShowCancel_Click" />

                        <asp:HyperLink
    ID="lnkCreatePurchaseReturn"
    runat="server"
    Text="إنشاء مرتجع مشتريات"
    CssClass="btn btn-warning"
    Visible="false" />

                    </div>

                    <%-- نموذج إدخال سبب الإلغاء --%>

                    <asp:Panel
                        ID="pnlCancelForm"
                        runat="server"
                        Visible="false"
                        CssClass="border rounded p-3 mt-3">

                        <h6 class="mb-3">
                            إلغاء مستند الاستلام
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
                                placeholder="أدخلي سبب إلغاء مستند الاستلام" />

                            <asp:RequiredFieldValidator
                                ID="requiredCancellationReason"
                                runat="server"
                                ControlToValidate="txtCancellationReason"
                                ValidationGroup="CancelReceipt"
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
                                ValidationGroup="CancelReceipt"
                                OnClick="btnConfirmCancel_Click"
                                OnClientClick="return confirm('سيتم إنشاء حركات عكسية وتحديث المخزون. هل تريد المتابعة؟');" />

                            <asp:Button
                                ID="btnCloseCancel"
                                runat="server"
                                Text="تراجع"
                                CssClass="btn btn-secondary"
                                CausesValidation="false"
                                OnClick="btnCloseCancel_Click" />

                        </div>

                    </asp:Panel>

                </div>
            </div>

            <%-- أصناف مستند الاستلام --%>

            <div class="card shadow-sm">

                <div class="card-header">
                    <h5 class="mb-0">
                        الأصناف المستلمة
                    </h5>
                </div>

                <div class="card-body">

                    <asp:GridView
                        ID="gvReceiptItems"
                        runat="server"
                        AutoGenerateColumns="False"
                        CssClass="table table-striped table-bordered"
                        EmptyDataText="لا توجد أصناف في مستند الاستلام.">

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
                                HeaderText="المستلم في هذا المستند"
                                DataFormatString="{0:N2}" />

                            <asp:BoundField
    DataField="ReturnedQuantity"
    HeaderText="الكمية المرتجعة"
    DataFormatString="{0:N2}" />

<asp:BoundField
    DataField="NetReceivedQuantity"
    HeaderText="صافي المستلم"
    DataFormatString="{0:N2}" />

                            <asp:BoundField
                                DataField="UnitPrice"
                                HeaderText="سعر الوحدة"
                                DataFormatString="{0:N2}" />

                            <asp:BoundField
                                DataField="LineTotal"
                                HeaderText="إجمالي المستلم"
                                DataFormatString="{0:N2}" />

                        </Columns>

                    </asp:GridView>

                    <div class="row justify-content-end mt-3">

    <div class="col-md-5">

        <div class="d-flex justify-content-between mb-2">
            <strong>إجمالي قيمة الاستلام:</strong>

            <asp:Label
                ID="lblGrandTotal"
                runat="server"
                Text="0.00" />
        </div>

        <div class="d-flex justify-content-between mb-2 text-danger">
            <strong>إجمالي قيمة المرتجع:</strong>

            <asp:Label
                ID="lblReturnedTotal"
                runat="server"
                Text="0.00" />
        </div>

        <hr />

        <div class="d-flex justify-content-between text-success">
            <strong>صافي قيمة الاستلام:</strong>

            <asp:Label
                ID="lblNetTotal"
                runat="server"
                Text="0.00" />
        </div>

    </div>

</div>

                </div>
            </div>

            <div class="card shadow-sm mt-4">

    <div class="card-header">
        <h5 class="mb-0">
            سجل مرتجعات مستند الاستلام
        </h5>
    </div>

    <div class="card-body">

        <div class="table-responsive">

            <asp:GridView
                ID="gvPurchaseReturns"
                runat="server"
                AutoGenerateColumns="False"
                CssClass="table table-striped table-bordered"
                EmptyDataText="لا توجد مرتجعات مرتبطة بمستند الاستلام.">

                <Columns>

                    <asp:BoundField
                        DataField="ReturnNumber"
                        HeaderText="رقم المرتجع" />

                    <asp:BoundField
                        DataField="ReturnDate"
                        HeaderText="تاريخ المرتجع"
                        DataFormatString="{0:yyyy-MM-dd}" />

                    <asp:BoundField
                        DataField="ItemsCount"
                        HeaderText="عدد الأصناف" />

                    <asp:BoundField
                        DataField="TotalQuantity"
                        HeaderText="إجمالي الكمية"
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

        </asp:Panel>

    </div>

</asp:Content>