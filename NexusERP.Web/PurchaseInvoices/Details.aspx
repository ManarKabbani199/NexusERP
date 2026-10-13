<%@ Page Title="<%$ Resources:PurchaseInvoiceDetails, DetailsTitle %>"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Details.aspx.cs"
    Inherits="NexusERP.Web.PurchaseInvoices.Details" %>

<asp:Content
    ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">


    <style>
        .invoice-details-heading { display:flex; align-items:center; justify-content:space-between; flex-wrap:wrap; gap:12px; margin-bottom:24px; }
        .invoice-details-heading h2 { margin:0; color:#14243c; font-size:24px; }
        .invoice-details-page .card { border-color:#e3e9f2; border-radius:12px; overflow:hidden; }
        .invoice-details-page .card-header { background:#f7f9fd; padding:16px 20px; }
        .invoice-details-page .card-header h5 { color:#25324a; font-size:17px; }
        .invoice-details-page .card-body { padding:20px; }
        .invoice-details-page .row strong { display:inline-block; margin-bottom:6px; }
        .invoice-details-table-wrap { overflow-x:auto; }
        .invoice-details-table { min-width:620px; }
        .invoice-details-table th { background:#eef3fa; white-space:nowrap; }
        .invoice-details-table td { vertical-align:middle; }
    </style>

    <div class="invoice-details-page">

        <div class="invoice-details-heading">

            <h2 class="mb-0"><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceDetails, DetailsTitle %>" /></h2>

            <a href="Index.aspx"
               class="btn btn-secondary"><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceDetails, BackToList %>" /></a>

        </div>

        <asp:Label
            ID="lblMessage"
            runat="server"
            Visible="false" />

        <asp:Panel
            ID="pnlDetails"
            runat="server"
            Visible="false">

            <!-- بيانات الفاتورة -->
            <div class="card shadow-sm mb-4">

                <div class="card-header">
                    <h5 class="mb-0"><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceDetails, InvoiceInformation %>" /></h5>
                </div>

                <div class="card-body">

                    <div class="row">

                        <div class="col-md-3 mb-3">
                            <strong><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceDetails, InvoiceNumber %>" /></strong>

                            <div>
                                <asp:Label
                                    ID="lblInvoiceNumber"
                                    runat="server" />
                            </div>
                        </div>

                        <div class="col-md-3 mb-3">
                            <strong><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceDetails, InvoiceDate %>" /></strong>

                            <div>
                                <asp:Label
                                    ID="lblInvoiceDate"
                                    runat="server" />
                            </div>
                        </div>

                        <div class="col-md-3 mb-3">
                            <strong><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceDetails, DueDate %>" /></strong>

                            <div>
                                <asp:Label
                                    ID="lblDueDate"
                                    runat="server" />
                            </div>
                        </div>

                        <div class="col-md-3 mb-3">
                            <strong><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceDetails, Status %>" /></strong>

                            <div>
                                <asp:Label
                                    ID="lblStatus"
                                    runat="server"
                                    CssClass="badge bg-secondary" />
                            </div>
                        </div>

                    </div>

                    <div class="row">

                        <div class="col-md-4 mb-3">
                            <strong><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceDetails, PurchaseOrder %>" /></strong>

                            <div>
                                <asp:Label
                                    ID="lblPurchaseOrder"
                                    runat="server" />
                            </div>
                        </div>

                        <div class="col-md-4 mb-3">
                            <strong><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceDetails, Supplier %>" /></strong>

                            <div>
                                <asp:Label
                                    ID="lblSupplier"
                                    runat="server" />
                            </div>
                        </div>

                        <div class="col-md-4 mb-3">
                            <strong><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceDetails, PostedAt %>" /></strong>

                            <div>
                                <asp:Label
                                    ID="lblPostedAt"
                                    runat="server" />
                            </div>
                        </div>

                    </div>

                    <div class="row">

                        <div class="col-md-4 mb-3">
                            <strong><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceDetails, PaidAmount %>" /></strong>

                            <div>
                                <asp:Label
                                    ID="lblPaidAmount"
                                    runat="server"
                                    Text="0.00" />
                            </div>
                        </div>

                        <div class="col-md-4 mb-3">
                            <strong><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceDetails, RemainingAmount %>" /></strong>

                            <div>
                                <asp:Label
                                    ID="lblRemainingAmount"
                                    runat="server"
                                    Text="0.00" />
                            </div>
                        </div>

                    </div>

                    <div class="mb-3">
                        <strong><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceDetails, Notes %>" /></strong>

                        <div>
                            <asp:Label
                                ID="lblNotes"
                                runat="server" />
                        </div>
                    </div>

                    <!-- بيانات الإلغاء -->
                    <asp:Panel
                        ID="pnlCancellationDetails"
                        runat="server"
                        Visible="false"
                        CssClass="alert alert-danger mt-3 mb-0">

                        <h6 class="mb-3"><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceDetails, CancellationInformation %>" /></h6>

                        <div class="mb-2">
                            <strong><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceDetails, CancelledAt %>" /></strong>

                            <asp:Label
                                ID="lblCancelledAt"
                                runat="server" />
                        </div>

                        <div class="mb-2">
                            <strong><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceDetails, CancelledBy %>" /></strong>

                            <asp:Label
                                ID="lblCancelledBy"
                                runat="server" />
                        </div>

                        <div>
                            <strong><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceDetails, CancellationReasonLabel %>" /></strong>

                            <asp:Label
                                ID="lblCancellationReason"
                                runat="server" />
                        </div>

                    </asp:Panel>

                </div>
            </div>

            <!-- الأصناف -->
            <div class="card shadow-sm mb-4">

                <div class="card-header">
                    <h5 class="mb-0"><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceDetails, InvoiceItems %>" /></h5>
                </div>


                <div class="card-body">

                    <!-- أزرار الإجراءات -->
                    <div class="d-flex flex-wrap gap-2 mb-4">

                        <asp:Button
                            ID="btnPost"
                            runat="server"
                            Text="<%$ Resources:PurchaseInvoiceDetails, PostInvoice %>"
                            CssClass="btn btn-success"
                            Visible="false"
                            CausesValidation="false"
                            OnClick="btnPost_Click" />

                        <asp:Button
                            ID="btnMarkAsPaid"
                            runat="server"
                            Text="<%$ Resources:PurchaseInvoiceDetails, MarkAsPaid %>"
                            CssClass="btn btn-primary"
                            Visible="false"
                            CausesValidation="false"
                            OnClick="btnMarkAsPaid_Click" />

                        <asp:Button
                            ID="btnShowCancelInvoice"
                            runat="server"
                            Text="<%$ Resources:PurchaseInvoiceDetails, CancelInvoice %>"
                            CssClass="btn btn-danger"
                            Visible="false"
                            CausesValidation="false"
                            OnClick="btnShowCancelInvoice_Click" />

                        <asp:HyperLink
        ID="lnkCreatePayment"
        runat="server"
        Text="<%$ Resources:PurchaseInvoiceDetails, CreatePayment %>"
        CssClass="btn btn-success"
        Visible="false" />

                    </div>

                    <!-- نموذج الإلغاء الوحيد -->
                    <asp:Panel
                        ID="pnlCancelInvoiceForm"
                        runat="server"
                        Visible="false"
                        CssClass="border border-danger rounded p-3 mb-4">

                        <h6 class="text-danger mb-3"><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceDetails, CancelInvoice %>" /></h6>

                        <div class="mb-3">

                            <label class="form-label"
                                   for="<%= txtInvoiceCancellationReason.ClientID %>"><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceDetails, CancellationReason %>" /></label>

                            <asp:TextBox
                                ID="txtInvoiceCancellationReason"
                                runat="server"
                                TextMode="MultiLine"
                                Rows="3"
                                MaxLength="500"
                                CssClass="form-control"
                                placeholder="<%$ Resources:PurchaseInvoiceDetails, ReasonPlaceholder %>" />

                            <asp:RequiredFieldValidator
                                ID="requiredInvoiceCancellationReason"
                                runat="server"
                                ControlToValidate="txtInvoiceCancellationReason"
                                ValidationGroup="CancelPurchaseInvoice"
                                ErrorMessage="<%$ Resources:PurchaseInvoiceDetails, ReasonRequired %>"
                                CssClass="text-danger"
                                Display="Dynamic" />

                        </div>

                        <div class="d-flex flex-wrap gap-2">

                            <asp:Button
                                ID="btnConfirmCancelInvoice"
                                runat="server"
                                Text="<%$ Resources:PurchaseInvoiceDetails, ConfirmCancellation %>"
                                CssClass="btn btn-danger"
                                ValidationGroup="CancelPurchaseInvoice"
                                OnClick="btnConfirmCancelInvoice_Click" />

                            <asp:Button
                                ID="btnCloseCancelInvoice"
                                runat="server"
                                Text="<%$ Resources:PurchaseInvoiceDetails, Back %>"
                                CssClass="btn btn-secondary"
                                CausesValidation="false"
                                OnClick="btnCloseCancelInvoice_Click" />

                        </div>

                    </asp:Panel>

                    <div class="invoice-details-table-wrap">
<asp:GridView
                        ID="gvInvoiceItems"
                        runat="server"
                        AutoGenerateColumns="False"
                        CssClass="table table-striped table-bordered invoice-details-table"
                        EmptyDataText="<%$ Resources:PurchaseInvoiceDetails, NoItems %>">

                        <Columns>

                            <asp:BoundField
                                DataField="ItemCode"
                                HeaderText="<%$ Resources:PurchaseInvoiceDetails, ItemCode %>" />

                            <asp:BoundField
                                DataField="ItemName"
                                HeaderText="<%$ Resources:PurchaseInvoiceDetails, ItemName %>" />

                            <asp:BoundField
                                DataField="Quantity"
                                HeaderText="<%$ Resources:PurchaseInvoiceDetails, Quantity %>"
                                DataFormatString="{0:N2}" />

                            <asp:BoundField
                                DataField="UnitPrice"
                                HeaderText="<%$ Resources:PurchaseInvoiceDetails, UnitPrice %>"
                                DataFormatString="{0:N2}" />

                            <asp:BoundField
                                DataField="LineTotal"
                                HeaderText="<%$ Resources:PurchaseInvoiceDetails, LineTotal %>"
                                DataFormatString="{0:N2}" />

                        </Columns>

                    </asp:GridView>
</div>

                    <div class="row justify-content-end mt-3">

                        <div class="col-md-5">

                            <div class="d-flex justify-content-between mb-2">
                                <strong><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceDetails, SubtotalLabel %>" /></strong>

                                <asp:Label
                                    ID="lblSubTotal"
                                    runat="server"
                                    Text="0.00" />
                            </div>

                            <div class="d-flex justify-content-between mb-2">
                                <strong><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceDetails, DiscountLabel %>" /></strong>

                                <asp:Label
                                    ID="lblDiscountAmount"
                                    runat="server"
                                    Text="0.00" />
                            </div>

                            <div class="d-flex justify-content-between mb-2">
                                <strong><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceDetails, TaxLabel %>" /></strong>

                                <asp:Label
                                    ID="lblTaxAmount"
                                    runat="server"
                                    Text="0.00" />
                            </div>

                            <hr />

                            <div class="d-flex justify-content-between">
                                <strong><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceDetails, InvoiceTotalLabel %>" /></strong>

                                <strong>
                                    <asp:Label
                                        ID="lblGrandTotal"
                                        runat="server"
                                        Text="0.00" />
                                </strong>
                            </div>

                        </div>
                    </div>

                </div>
            </div>

                <div class="card shadow-sm mt-4">

    <div class="card-header">
        <h5 class="mb-0"><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceDetails, PaymentHistory %>" /></h5>
    </div>

    <div class="card-body">

        <div class="invoice-details-table-wrap">
<asp:GridView
            ID="gvPurchasePayments"
            runat="server"
            AutoGenerateColumns="False"
            CssClass="table table-striped table-bordered invoice-details-table"
            EmptyDataText="<%$ Resources:PurchaseInvoiceDetails, NoPayments %>">

            <Columns>

                <asp:BoundField
                    DataField="PaymentNumber"
                    HeaderText="<%$ Resources:PurchaseInvoiceDetails, PaymentNumber %>" />

                <asp:BoundField
                    DataField="PaymentDate"
                    HeaderText="<%$ Resources:PurchaseInvoiceDetails, PaymentDate %>"
                    DataFormatString="{0:yyyy-MM-dd}" />

                <asp:BoundField
                    DataField="Amount"
                    HeaderText="<%$ Resources:PurchaseInvoiceDetails, Amount %>"
                    DataFormatString="{0:N2}" />

                <asp:BoundField
                    DataField="PaymentMethodName"
                    HeaderText="<%$ Resources:PurchaseInvoiceDetails, PaymentMethod %>" />

                <asp:TemplateField
                    HeaderText="<%$ Resources:PurchaseInvoiceDetails, Status %>">

                    <ItemTemplate>

                        <span class='<%#
                            Convert.ToBoolean(
                                Eval("IsCancelled"))
                                ? "badge bg-danger"
                                : "badge bg-success" %>'>

                            <%#
                                Convert.ToBoolean(
                                    Eval("IsCancelled"))
                                    ? Translate("Cancelled")
                                    : Translate("Active") %>

                        </span>

                    </ItemTemplate>

                </asp:TemplateField>

                <asp:TemplateField
                    HeaderText="<%$ Resources:PurchaseInvoiceDetails, Actions %>">

                    <ItemTemplate>

                        <a class="btn btn-info btn-sm"
                           href='<%#
                               "../PurchasePayments/Details.aspx?id=" +
                               Eval("Id") %>'><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceDetails, Details %>" /></a>

                    </ItemTemplate>

                </asp:TemplateField>

            </Columns>

        </asp:GridView>
</div>

        <div class="row mt-3">

            <div class="col-md-4">
                <strong><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceDetails, InvoiceTotalLabel %>" /></strong>

                <asp:Label
                    ID="lblPaymentInvoiceTotal"
                    runat="server"
                    Text="0.00" />
            </div>

            <div class="col-md-4">
                <strong><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceDetails, TotalPaidLabel %>" /></strong>

                <asp:Label
                    ID="lblPaymentPaidAmount"
                    runat="server"
                    Text="0.00"
                    CssClass="text-success fw-bold" />
            </div>

            <div class="col-md-4">
                <strong><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceDetails, RemainingAmountLabel %>" /></strong>

                <asp:Label
                    ID="lblPaymentRemainingAmount"
                    runat="server"
                    Text="0.00"
                    CssClass="text-danger fw-bold" />
            </div>

        </div>

    </div>
</div>


        </asp:Panel>

    </div>

</asp:Content>