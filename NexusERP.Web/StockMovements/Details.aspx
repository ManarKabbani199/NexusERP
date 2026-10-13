<%@ Page Title="تفاصيل حركة المخزون"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Details.aspx.cs"
    Inherits="NexusERP.Web.StockMovements.Details" %>

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
            ID="pnlDetails"
            runat="server">

            <div class="card shadow-sm">

                <div class="card-header">
                    <h3 class="mb-0">
                        تفاصيل حركة المخزون
                    </h3>
                </div>

                <div class="card-body">

                    <div class="row">

                        <div class="col-md-6 mb-3">
                            <strong>رقم الحركة:</strong>
                            <asp:Label
                                ID="lblId"
                                runat="server" />
                        </div>

                        <div class="col-md-6 mb-3">
                            <strong>نوع الحركة:</strong>
                            <asp:Label
                                ID="lblMovementType"
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

                        <div class="col-md-6 mb-3">
                            <strong>الكمية:</strong>
                            <asp:Label
                                ID="lblQuantity"
                                runat="server" />
                        </div>

                        <div class="col-md-6 mb-3">
                            <strong>تاريخ الحركة:</strong>
                            <asp:Label
                                ID="lblMovementDate"
                                runat="server" />
                        </div>

                        <asp:Panel
                            ID="pnlDestinationWarehouse"
                            runat="server"
                            CssClass="col-md-6 mb-3"
                            Visible="false">

                            <strong>مستودع الوجهة:</strong>

                            <asp:Label
                                ID="lblDestinationWarehouse"
                                runat="server" />

                        </asp:Panel>

                        <div class="col-md-6 mb-3">
                            <strong>الرقم المرجعي:</strong>
                            <asp:Label
                                ID="lblReferenceNumber"
                                runat="server" />
                        </div>

                        <div class="col-12 mb-3">
                            <strong>الملاحظات:</strong>

                            <div class="border rounded p-3 mt-2">

                                <asp:Label
                                    ID="lblNotes"
                                    runat="server" />

                            </div>
                        </div>

                        <div class="col-md-6 mb-3">
                            <strong>أُنشئت بواسطة:</strong>
                            <asp:Label
                                ID="lblCreatedBy"
                                runat="server" />
                        </div>

                        <div class="col-md-6 mb-3">
                            <strong>تاريخ الإنشاء:</strong>
                            <asp:Label
                                ID="lblCreatedAt"
                                runat="server" />
                        </div>

                        <asp:Panel
    ID="pnlPurchaseReturn"
    runat="server"
    Visible="false"
    CssClass="col-md-4 mb-3">

    <strong>مرتجع المشتريات</strong>

    <div>
        <asp:HyperLink
            ID="lnkPurchaseReturn"
            runat="server"
            CssClass="btn btn-link p-0" />
    </div>

</asp:Panel>

                        <asp:Panel
    ID="pnlCancelled"
    runat="server"
    Visible="false"
    CssClass="alert alert-danger">

    <h5>هذه الحركة ملغاة</h5>

    <div class="mb-2">
        <strong>سبب الإلغاء:</strong>

        <asp:Label
            ID="lblCancellationReason"
            runat="server" />
    </div>

    <div class="mb-2">
        <strong>أُلغيَت بواسطة:</strong>

        <asp:Label
            ID="lblCancelledBy"
            runat="server" />
    </div>

    <div>
        <strong>تاريخ الإلغاء:</strong>

        <asp:Label
            ID="lblCancelledAt"
            runat="server" />
    </div>

</asp:Panel>

<asp:Panel
    ID="pnlReversal"
    runat="server"
    Visible="false"
    CssClass="alert alert-secondary">

    هذه حركة عكسية للحركة الأصلية رقم:

    <asp:HyperLink
        ID="lnkOriginalMovement"
        runat="server"
        CssClass="alert-link">
    </asp:HyperLink>

</asp:Panel>

                    </div>

                    <a href="Index.aspx"
                       class="btn btn-secondary">
                        رجوع إلى حركات المخزون
                    </a>

                </div>

            </div>

        </asp:Panel>

    </div>

</asp:Content>