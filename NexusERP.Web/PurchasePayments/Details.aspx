<%@ Page Title="<%$ Resources:PurchasePaymentDetails, Title %>"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Details.aspx.cs"
    Inherits="NexusERP.Web.PurchasePayments.Details" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .payment-details-heading { display:flex; justify-content:space-between; align-items:center; flex-wrap:wrap; gap:12px; margin-bottom:24px; }
        .payment-details-heading h2 { margin:0; color:#14243c; font-size:24px; }
        .payment-details-page .card { border-color:#e3e9f2; border-radius:12px; overflow:hidden; }
        .payment-details-page .card-header { background:#f7f9fd; padding:16px 20px; }
        .payment-details-page .card-header h5 { color:#25324a; font-size:17px; }
        .payment-details-page .card-body { padding:24px; }
        .payment-details-page .row strong { display:block; color:#52647d; margin-bottom:8px; }
        .payment-details-page .row { overflow-wrap:anywhere; }
        .payment-details-page .payment-notes { margin-bottom:24px; overflow-wrap:anywhere; }
        .payment-details-page .payment-notes strong { display:block; margin-bottom:8px; }
        .payment-details-page .cancellation-data div { margin-top:8px; overflow-wrap:anywhere; }
        .payment-details-page .cancel-form { border:1px solid #efc7cd; border-radius:10px; padding:20px; margin-top:20px; }
        .payment-details-page .cancel-actions { display:flex; gap:10px; flex-wrap:wrap; }
    </style>

    <div class="payment-details-page">
        <div class="payment-details-heading">
            <h2><asp:Literal runat="server" Text="<%$ Resources:PurchasePaymentDetails, Title %>" /></h2>
            <asp:HyperLink runat="server" NavigateUrl="~/PurchasePayments/Index.aspx" Text="<%$ Resources:PurchasePaymentDetails, BackToList %>" CssClass="btn btn-secondary" />
        </div>

        <asp:Label ID="lblMessage" runat="server" Visible="false" role="alert" />
        <asp:Panel ID="pnlDetails" runat="server" Visible="false">
            <div class="card shadow-sm">
                <div class="card-header"><h5 class="mb-0"><asp:Literal runat="server" Text="<%$ Resources:PurchasePaymentDetails, PaymentInformation %>" /></h5></div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-3 mb-3"><strong><asp:Literal runat="server" Text="<%$ Resources:PurchasePaymentDetails, PaymentNumber %>" /></strong><div><asp:Label ID="lblPaymentNumber" runat="server" /></div></div>
                        <div class="col-md-3 mb-3"><strong><asp:Literal runat="server" Text="<%$ Resources:PurchasePaymentDetails, PaymentDate %>" /></strong><div><asp:Label ID="lblPaymentDate" runat="server" /></div></div>
                        <div class="col-md-3 mb-3"><strong><asp:Literal runat="server" Text="<%$ Resources:PurchasePaymentDetails, Amount %>" /></strong><div><asp:Label ID="lblAmount" runat="server" CssClass="fw-bold text-success" /></div></div>
                        <div class="col-md-3 mb-3"><strong><asp:Literal runat="server" Text="<%$ Resources:PurchasePaymentDetails, Status %>" /></strong><div><asp:Label ID="lblStatus" runat="server" CssClass="badge bg-success" /></div></div>
                    </div>
                    <div class="row">
                        <div class="col-md-4 mb-3"><strong><asp:Literal runat="server" Text="<%$ Resources:PurchasePaymentDetails, PurchaseInvoice %>" /></strong><div><asp:HyperLink ID="lnkInvoice" runat="server" /></div></div>
                        <div class="col-md-4 mb-3"><strong><asp:Literal runat="server" Text="<%$ Resources:PurchasePaymentDetails, Supplier %>" /></strong><div><asp:Label ID="lblSupplier" runat="server" /></div></div>
                        <div class="col-md-4 mb-3"><strong><asp:Literal runat="server" Text="<%$ Resources:PurchasePaymentDetails, PaymentMethod %>" /></strong><div><asp:Label ID="lblPaymentMethod" runat="server" /></div></div>
                    </div>
                    <div class="row">
                        <div class="col-md-6 mb-3"><strong><asp:Literal runat="server" Text="<%$ Resources:PurchasePaymentDetails, ReferenceNumber %>" /></strong><div><asp:Label ID="lblReferenceNumber" runat="server" /></div></div>
                        <div class="col-md-6 mb-3"><strong><asp:Literal runat="server" Text="<%$ Resources:PurchasePaymentDetails, CreatedBy %>" /></strong><div><asp:Label ID="lblCreatedBy" runat="server" /></div></div>
                    </div>
                    <div class="payment-notes"><strong><asp:Literal runat="server" Text="<%$ Resources:PurchasePaymentDetails, Notes %>" /></strong><div><asp:Label ID="lblNotes" runat="server" /></div></div>

                    <asp:Panel ID="pnlCancellationDetails" runat="server" Visible="false" CssClass="alert alert-danger cancellation-data">
                        <h6><asp:Literal runat="server" Text="<%$ Resources:PurchasePaymentDetails, CancellationInformation %>" /></h6>
                        <div><strong><asp:Literal runat="server" Text="<%$ Resources:PurchasePaymentDetails, CancelledAt %>" /></strong> <asp:Label ID="lblCancelledAt" runat="server" /></div>
                        <div><strong><asp:Literal runat="server" Text="<%$ Resources:PurchasePaymentDetails, CancelledBy %>" /></strong> <asp:Label ID="lblCancelledBy" runat="server" /></div>
                        <div><strong><asp:Literal runat="server" Text="<%$ Resources:PurchasePaymentDetails, CancellationReasonLabel %>" /></strong> <asp:Label ID="lblCancellationReason" runat="server" /></div>
                    </asp:Panel>

                    <asp:Button ID="btnShowCancel" runat="server" Text="<%$ Resources:PurchasePaymentDetails, CancelPayment %>" CssClass="btn btn-danger" Visible="false" CausesValidation="false" OnClick="btnShowCancel_Click" />

                    <asp:Panel ID="pnlCancelForm" runat="server" Visible="false" CssClass="cancel-form">
                        <h6 class="text-danger mb-3"><asp:Literal runat="server" Text="<%$ Resources:PurchasePaymentDetails, CancelPayment %>" /></h6>
                        <div class="mb-3">
                            <asp:Label runat="server" AssociatedControlID="txtCancellationReason" CssClass="form-label" Text="<%$ Resources:PurchasePaymentDetails, CancellationReason %>" />
                            <asp:TextBox ID="txtCancellationReason" runat="server" TextMode="MultiLine" Rows="3" MaxLength="500" CssClass="form-control" />
                            <asp:RequiredFieldValidator ID="requiredCancellationReason" runat="server" ControlToValidate="txtCancellationReason" ValidationGroup="CancelPayment" ErrorMessage="<%$ Resources:PurchasePaymentDetails, ReasonRequired %>" CssClass="text-danger" Display="Dynamic" />
                        </div>
                        <div class="cancel-actions">
                            <asp:Button ID="btnConfirmCancel" runat="server" Text="<%$ Resources:PurchasePaymentDetails, ConfirmCancellation %>" CssClass="btn btn-danger" ValidationGroup="CancelPayment" OnClick="btnConfirmCancel_Click" />
                            <asp:Button ID="btnCloseCancel" runat="server" Text="<%$ Resources:PurchasePaymentDetails, Back %>" CssClass="btn btn-secondary" CausesValidation="false" OnClick="btnCloseCancel_Click" />
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
