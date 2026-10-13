<%@ Page Title="إلغاء حركة المخزون"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Cancel.aspx.cs"
    Inherits="NexusERP.Web.StockMovements.Cancel" %>

<asp:Content
    ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <asp:Panel
            ID="pnlMessage"
            runat="server"
            Visible="false"
            CssClass="alert alert-danger">

            <asp:Literal
                ID="litMessage"
                runat="server" />

        </asp:Panel>

        <asp:Panel
            ID="pnlCancelForm"
            runat="server">

            <div class="card shadow-sm border-danger">

                <div class="card-header bg-danger text-white">
                    <h3 class="mb-0">
                        إلغاء حركة المخزون
                    </h3>
                </div>

                <div class="card-body">

                    <div class="alert alert-warning">
                        لن يتم حذف الحركة. سيقوم النظام بإنشاء
                        حركة عكسية لتصحيح رصيد المخزون.
                    </div>

                    <div class="row mb-4">

                        <div class="col-md-4 mb-3">
                            <strong>رقم الحركة:</strong>

                            <asp:Label
                                ID="lblMovementId"
                                runat="server" />
                        </div>

                        <div class="col-md-4 mb-3">
                            <strong>نوع الحركة:</strong>

                            <asp:Label
                                ID="lblMovementType"
                                runat="server" />
                        </div>

                        <div class="col-md-4 mb-3">
                            <strong>الكمية:</strong>

                            <asp:Label
                                ID="lblQuantity"
                                runat="server" />
                        </div>

                        <div class="col-md-6 mb-3">
                            <strong>المستودع:</strong>

                            <asp:Label
                                ID="lblWarehouse"
                                runat="server" />
                        </div>

                        <div class="col-md-6 mb-3">
                            <strong>الصنف:</strong>

                            <asp:Label
                                ID="lblItem"
                                runat="server" />
                        </div>

                    </div>

                    <div class="mb-3">

                        <label class="form-label">
                            سبب الإلغاء
                        </label>

                        <asp:TextBox
                            ID="txtCancellationReason"
                            runat="server"
                            CssClass="form-control"
                            TextMode="MultiLine"
                            Rows="4"
                            MaxLength="500">
                        </asp:TextBox>

                        <asp:RequiredFieldValidator
                            ID="rfvCancellationReason"
                            runat="server"
                            ControlToValidate="txtCancellationReason"
                            ValidationGroup="CancelForm"
                            CssClass="text-danger"
                            ErrorMessage="يجب إدخال سبب الإلغاء."
                            Display="Dynamic" />

                    </div>

                    <asp:Button
                        ID="btnConfirmCancel"
                        runat="server"
                        Text="تأكيد إلغاء الحركة"
                        CssClass="btn btn-danger"
                        ValidationGroup="CancelForm"
                        OnClick="btnConfirmCancel_Click"
                        OnClientClick="return confirm(
                            'هل أنت متأكد من إلغاء الحركة وإنشاء حركة عكسية؟');" />

                    <a href="Index.aspx"
                       class="btn btn-secondary">
                        تراجع
                    </a>

                </div>

            </div>

        </asp:Panel>

    </div>

</asp:Content>