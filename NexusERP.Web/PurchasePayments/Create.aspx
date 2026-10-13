<%@ Page Title="<%$ Resources:PurchasePaymentCreate, Title %>"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Create.aspx.cs"
    Inherits="NexusERP.Web.PurchasePayments.Create" %>

<asp:Content
    ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <style>
        .payment-create-heading { display:flex; justify-content:space-between; align-items:center; flex-wrap:wrap; gap:12px; margin-bottom:24px; }
        .payment-create-heading h2 { margin:0; color:#14243c; font-size:24px; }
        .payment-create-page .card { border-color:#e3e9f2; border-radius:12px; overflow:hidden; }
        .payment-create-page .card-header { background:#f7f9fd; padding:16px 20px; }
        .payment-create-page .card-header h5 { color:#25324a; font-size:17px; }
        .payment-create-page .card-body { padding:24px; }
        .payment-create-page .form-label { font-weight:600; color:#334866; }
        .payment-create-page .row strong { display:block; margin-bottom:8px; color:#52647d; }
        .payment-create-page .row { overflow-wrap:anywhere; }
        .payment-create-page input[type=number], .payment-create-page input[type=date] { direction:ltr; }
    </style>

    <div class="payment-create-page">

        <div class="payment-create-heading">

            <h2 class="mb-0"><asp:Literal runat="server" Text="<%$ Resources:PurchasePaymentCreate, Title %>" /></h2>

            <asp:HyperLink
                ID="lnkBack"
                runat="server"
                Text="<%$ Resources:PurchasePaymentCreate, BackToInvoice %>"
                CssClass="btn btn-secondary" />

        </div>

        <asp:Label
            ID="lblMessage"
            runat="server"
            Visible="false" />

        <asp:Panel
            ID="pnlForm"
            runat="server"
            Visible="false">

            <div class="card shadow-sm mb-4">

                <div class="card-header">
                    <h5 class="mb-0"><asp:Literal runat="server" Text="<%$ Resources:PurchasePaymentCreate, InvoiceInformation %>" /></h5>
                </div>

                <div class="card-body">

                    <div class="row">

                        <div class="col-md-3 mb-3">
                            <strong><asp:Literal runat="server" Text="<%$ Resources:PurchasePaymentCreate, InvoiceNumber %>" /></strong>
                            <div>
                                <asp:Label
                                    ID="lblInvoiceNumber"
                                    runat="server" />
                            </div>
                        </div>

                        <div class="col-md-3 mb-3">
                            <strong><asp:Literal runat="server" Text="<%$ Resources:PurchasePaymentCreate, Supplier %>" /></strong>
                            <div>
                                <asp:Label
                                    ID="lblSupplier"
                                    runat="server" />
                            </div>
                        </div>

                        <div class="col-md-2 mb-3">
                            <strong><asp:Literal runat="server" Text="<%$ Resources:PurchasePaymentCreate, InvoiceTotal %>" /></strong>
                            <div>
                                <asp:Label
                                    ID="lblInvoiceTotal"
                                    runat="server" />
                            </div>
                        </div>

                        <div class="col-md-2 mb-3">
                            <strong><asp:Literal runat="server" Text="<%$ Resources:PurchasePaymentCreate, PaidAmount %>" /></strong>
                            <div>
                                <asp:Label
                                    ID="lblPaidAmount"
                                    runat="server" />
                            </div>
                        </div>

                        <div class="col-md-2 mb-3">
                            <strong><asp:Literal runat="server" Text="<%$ Resources:PurchasePaymentCreate, RemainingAmount %>" /></strong>
                            <div>
                                <asp:Label
                                    ID="lblRemainingAmount"
                                    runat="server"
                                    CssClass="fw-bold text-danger" />
                            </div>
                        </div>

                    </div>

                </div>
            </div>

            <div class="card shadow-sm">

                <div class="card-header">
                    <h5 class="mb-0"><asp:Literal runat="server" Text="<%$ Resources:PurchasePaymentCreate, PaymentInformation %>" /></h5>
                </div>

                <div class="card-body">

                    <asp:ValidationSummary
                        ID="validationSummary"
                        runat="server"
                        ValidationGroup="PaymentForm"
                        CssClass="alert alert-danger"
                        HeaderText="<%$ Resources:PurchasePaymentCreate, ValidationHeader %>" />

                    <div class="row">

                        <div class="col-md-4 mb-3">

                            <asp:Label runat="server" AssociatedControlID="txtPaymentNumber" CssClass="form-label" Text="<%$ Resources:PurchasePaymentCreate, PaymentNumber %>" />

                            <asp:TextBox
                                ID="txtPaymentNumber"
                                runat="server"
                                MaxLength="50"
                                CssClass="form-control" />

                            <asp:RequiredFieldValidator
                                ID="requiredPaymentNumber"
                                runat="server"
                                ControlToValidate="txtPaymentNumber"
                                ValidationGroup="PaymentForm"
                                ErrorMessage="<%$ Resources:PurchasePaymentCreate, PaymentNumberRequired %>"
                                CssClass="text-danger"
                                Display="Dynamic" />

                        </div>

                        <div class="col-md-4 mb-3">

                            <asp:Label runat="server" AssociatedControlID="txtPaymentDate" CssClass="form-label" Text="<%$ Resources:PurchasePaymentCreate, PaymentDate %>" />

                            <asp:TextBox
                                ID="txtPaymentDate"
                                runat="server"
                                TextMode="Date"
                                CssClass="form-control" />

                            <asp:RequiredFieldValidator
                                ID="requiredPaymentDate"
                                runat="server"
                                ControlToValidate="txtPaymentDate"
                                ValidationGroup="PaymentForm"
                                ErrorMessage="<%$ Resources:PurchasePaymentCreate, PaymentDateRequired %>"
                                CssClass="text-danger"
                                Display="Dynamic" />

                        </div>

                        <div class="col-md-4 mb-3">

                            <asp:Label runat="server" AssociatedControlID="ddlPaymentMethod" CssClass="form-label" Text="<%$ Resources:PurchasePaymentCreate, PaymentMethod %>" />

                            <asp:DropDownList
                                ID="ddlPaymentMethod"
                                runat="server"
                                CssClass="form-select">

                                <asp:ListItem
                                    Text="<%$ Resources:PurchasePaymentCreate, SelectMethod %>"
                                    Value="" />

                                <asp:ListItem
                                    Text="<%$ Resources:PurchasePaymentCreate, Cash %>"
                                    Value="1" />

                                <asp:ListItem
                                    Text="<%$ Resources:PurchasePaymentCreate, BankTransfer %>"
                                    Value="2" />

                                <asp:ListItem
                                    Text="<%$ Resources:PurchasePaymentCreate, Cheque %>"
                                    Value="3" />

                                <asp:ListItem
                                    Text="<%$ Resources:PurchasePaymentCreate, CreditCard %>"
                                    Value="4" />

                            </asp:DropDownList>

                            <asp:RequiredFieldValidator
                                ID="requiredPaymentMethod"
                                runat="server"
                                ControlToValidate="ddlPaymentMethod"
                                InitialValue=""
                                ValidationGroup="PaymentForm"
                                ErrorMessage="<%$ Resources:PurchasePaymentCreate, PaymentMethodRequired %>"
                                CssClass="text-danger"
                                Display="Dynamic" />

                        </div>

                    </div>

                    <div class="row">

                        <div class="col-md-6 mb-3">

                            <asp:Label runat="server" AssociatedControlID="txtAmount" CssClass="form-label" Text="<%$ Resources:PurchasePaymentCreate, Amount %>" />

                            <asp:TextBox
                                ID="txtAmount"
                                runat="server"
                                TextMode="Number"
                                step="0.01"
                                min="0.01"
                                CssClass="form-control" />

                            <asp:RequiredFieldValidator
                                ID="requiredAmount"
                                runat="server"
                                ControlToValidate="txtAmount"
                                ValidationGroup="PaymentForm"
                                ErrorMessage="<%$ Resources:PurchasePaymentCreate, AmountRequired %>"
                                CssClass="text-danger"
                                Display="Dynamic" />

                        </div>

                        <div class="col-md-6 mb-3">

                            <asp:Label runat="server" AssociatedControlID="txtReferenceNumber" CssClass="form-label" Text="<%$ Resources:PurchasePaymentCreate, ReferenceNumber %>" />

                            <asp:TextBox
                                ID="txtReferenceNumber"
                                runat="server"
                                MaxLength="100"
                                CssClass="form-control" />

                        </div>

                    </div>

                    <div class="mb-3">

                        <asp:Label runat="server" AssociatedControlID="txtNotes" CssClass="form-label" Text="<%$ Resources:PurchasePaymentCreate, Notes %>" />

                        <asp:TextBox
                            ID="txtNotes"
                            runat="server"
                            TextMode="MultiLine"
                            Rows="3"
                            MaxLength="500"
                            CssClass="form-control" />

                    </div>

                    <asp:Button
                        ID="btnSave"
                        runat="server"
                        Text="<%$ Resources:PurchasePaymentCreate, Save %>"
                        CssClass="btn btn-success"
                        ValidationGroup="PaymentForm"
                        OnClick="btnSave_Click" />

                    <asp:HyperLink
                        ID="lnkCancel"
                        runat="server"
                        Text="<%$ Resources:PurchasePaymentCreate, Cancel %>"
                        CssClass="btn btn-secondary" />

                </div>
            </div>

        </asp:Panel>

    </div>

</asp:Content>