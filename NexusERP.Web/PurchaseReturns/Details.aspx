<%@ Page Title="تفاصيل مرتجع المشتريات"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Details.aspx.cs"
    Inherits="NexusERP.Web.PurchaseReturns.Details" %>

<asp:Content
    ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <div class="d-flex justify-content-between
                    align-items-center mb-3">

            <h2 class="mb-0">
                تفاصيل مرتجع المشتريات
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
                        بيانات مرتجع المشتريات
                    </h5>
                </div>

                <div class="card-body">

                    <div class="row">

                        <div class="col-md-3 mb-3">
                            <strong>رقم المرتجع</strong>

                            <div>
                                <asp:Label
                                    ID="lblReturnNumber"
                                    runat="server" />
                            </div>
                        </div>

                        <div class="col-md-3 mb-3">
                            <strong>تاريخ المرتجع</strong>

                            <div>
                                <asp:Label
                                    ID="lblReturnDate"
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

                    </div>

                    <div class="row">

                        <div class="col-md-3 mb-3">
                            <strong>مستند الاستلام</strong>

                            <div>
                                <asp:Label
                                    ID="lblReceiptNumber"
                                    runat="server" />
                            </div>
                        </div>

                        <div class="col-md-3 mb-3">
                            <strong>أمر الشراء</strong>

                            <div>
                                <asp:Label
                                    ID="lblPurchaseOrder"
                                    runat="server" />
                            </div>
                        </div>

                        <div class="col-md-3 mb-3">
                            <strong>المورد</strong>

                            <div>
                                <asp:Label
                                    ID="lblSupplier"
                                    runat="server" />
                            </div>
                        </div>

                        <div class="col-md-3 mb-3">
                            <strong>المستودع</strong>

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

                    <asp:Panel
                        ID="pnlCancellationDetails"
                        runat="server"
                        Visible="false"
                        CssClass="alert alert-danger mt-3">

                        <h6 class="mb-3">
                            بيانات إلغاء المرتجع
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

                    <div class="d-flex flex-wrap gap-2 mt-3">

                        <asp:Button
                            ID="btnPost"
                            runat="server"
                            Text="ترحيل المرتجع"
                            CssClass="btn btn-success"
                            Visible="false"
                            CausesValidation="false"
                            OnClick="btnPost_Click"
                            OnClientClick="return confirm(
                                'هل تريد ترحيل المرتجع؟ سيتم إنقاص المخزون.');" />

                        <asp:Button
                            ID="btnShowCancel"
                            runat="server"
                            Text="إلغاء المرتجع"
                            CssClass="btn btn-danger"
                            Visible="false"
                            CausesValidation="false"
                            OnClick="btnShowCancel_Click" />

                    </div>

                    <asp:Panel
                        ID="pnlCancelForm"
                        runat="server"
                        Visible="false"
                        CssClass="border border-danger rounded p-3 mt-3">

                        <h6 class="text-danger mb-3">
                            إلغاء مرتجع المشتريات
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
                                placeholder="أدخل سبب إلغاء المرتجع" />

                            <asp:RequiredFieldValidator
                                ID="requiredCancellationReason"
                                runat="server"
                                ControlToValidate="txtCancellationReason"
                                ValidationGroup="CancelPurchaseReturn"
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
                                ValidationGroup="CancelPurchaseReturn"
                                OnClick="btnConfirmCancel_Click"
                                OnClientClick="return confirm(
                                    'هل أنت متأكد من إلغاء المرتجع؟');" />

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

            <div class="card shadow-sm">

                <div class="card-header">
                    <h5 class="mb-0">
                        الأصناف المرتجعة
                    </h5>
                </div>

                <div class="card-body">

                    <div class="table-responsive">

                        <asp:GridView
                            ID="gvReturnItems"
                            runat="server"
                            AutoGenerateColumns="False"
                            CssClass="table table-striped table-bordered"
                            EmptyDataText="لا توجد أصناف في المرتجع.">

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
                                    DataField="ReturnedQuantity"
                                    HeaderText="الكمية المرتجعة"
                                    DataFormatString="{0:N2}" />

                                <asp:BoundField
                                    DataField="UnitPrice"
                                    HeaderText="سعر الوحدة"
                                    DataFormatString="{0:N2}" />

                                <asp:BoundField
                                    DataField="LineTotal"
                                    HeaderText="إجمالي المرتجع"
                                    DataFormatString="{0:N2}" />

                            </Columns>

                        </asp:GridView>

                    </div>

                    <div class="text-end mt-3">

                        <strong>
                            إجمالي قيمة المرتجع:
                        </strong>

                        <asp:Label
                            ID="lblGrandTotal"
                            runat="server"
                            Text="0.00" />

                    </div>

                </div>
            </div>

        </asp:Panel>

    </div>

</asp:Content>