<%@ Page Title="<%$ Resources:PurchaseInvoiceCreate, Title %>"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Create.aspx.cs"
    Inherits="NexusERP.Web.PurchaseInvoices.Create" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .invoice-create-heading { display:flex; justify-content:space-between; align-items:center; flex-wrap:wrap; gap:12px; margin-bottom:24px; }
        .invoice-create-heading h2 { margin:0; color:#14243c; font-size:24px; }
        .invoice-create-page .card { border-color:#e3e9f2; border-radius:12px; overflow:hidden; }
        .invoice-create-page .card-header { background:#f7f9fd; padding:16px 20px; }
        .invoice-create-page .card-header h5 { color:#25324a; font-size:17px; }
        .invoice-create-page .card-body { padding:20px; }
        .invoice-create-page .form-label { display:block; font-weight:600; color:#334866; }
        .invoice-create-table-wrap { overflow-x:auto; }
        .invoice-create-table { min-width:1000px; }
        .invoice-create-table th { background:#eef3fa; white-space:nowrap; }
        .invoice-create-table td { vertical-align:middle; }
        .invoice-create-table input { min-width:100px; }
        .invoice-create-page input[type=number],
        .invoice-create-page input[type=date] { direction:ltr; }
        .invoice-create-actions { display:flex; gap:12px; flex-wrap:wrap; }
    </style>

    <div class="invoice-create-page">
        <div class="invoice-create-heading">
            <h2><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceCreate, Title %>" /></h2>
            <asp:HyperLink runat="server" NavigateUrl="~/PurchaseInvoices/Index.aspx" CssClass="btn btn-secondary" Text="<%$ Resources:PurchaseInvoiceCreate, BackToList %>" />
        </div>

        <asp:Label ID="lblMessage" runat="server" Visible="false" />
        <asp:ValidationSummary ID="validationSummary" runat="server" ValidationGroup="InvoiceForm" CssClass="alert alert-danger" HeaderText="<%$ Resources:PurchaseInvoiceCreate, ValidationHeader %>" />

        <div class="card shadow-sm mb-4">
            <div class="card-header"><h5 class="mb-0"><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceCreate, InvoiceInformation %>" /></h5></div>
            <div class="card-body">
                <div class="row">
                    <div class="col-md-4 mb-3">
                        <asp:Label runat="server" AssociatedControlID="txtInvoiceNumber" CssClass="form-label" Text="<%$ Resources:PurchaseInvoiceCreate, InvoiceNumber %>" />
                        <asp:TextBox ID="txtInvoiceNumber" runat="server" MaxLength="30" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="requiredInvoiceNumber" runat="server" ControlToValidate="txtInvoiceNumber" ValidationGroup="InvoiceForm" ErrorMessage="<%$ Resources:PurchaseInvoiceCreate, InvoiceNumberRequired %>" CssClass="text-danger" Display="Dynamic" />
                    </div>
                    <div class="col-md-4 mb-3">
                        <asp:Label runat="server" AssociatedControlID="txtInvoiceDate" CssClass="form-label" Text="<%$ Resources:PurchaseInvoiceCreate, InvoiceDate %>" />
                        <asp:TextBox ID="txtInvoiceDate" runat="server" TextMode="Date" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="requiredInvoiceDate" runat="server" ControlToValidate="txtInvoiceDate" ValidationGroup="InvoiceForm" ErrorMessage="<%$ Resources:PurchaseInvoiceCreate, InvoiceDateRequired %>" CssClass="text-danger" Display="Dynamic" />
                    </div>
                    <div class="col-md-4 mb-3">
                        <asp:Label runat="server" AssociatedControlID="txtDueDate" CssClass="form-label" Text="<%$ Resources:PurchaseInvoiceCreate, DueDate %>" />
                        <asp:TextBox ID="txtDueDate" runat="server" TextMode="Date" CssClass="form-control" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4 mb-3">
                        <asp:Label runat="server" AssociatedControlID="ddlPurchaseOrder" CssClass="form-label" Text="<%$ Resources:PurchaseInvoiceCreate, PurchaseOrder %>" />
                        <asp:DropDownList ID="ddlPurchaseOrder" runat="server" CssClass="form-select" AutoPostBack="true" CausesValidation="false" OnSelectedIndexChanged="ddlPurchaseOrder_SelectedIndexChanged" />
                        <asp:RequiredFieldValidator ID="requiredPurchaseOrder" runat="server" ControlToValidate="ddlPurchaseOrder" InitialValue="" ValidationGroup="InvoiceForm" ErrorMessage="<%$ Resources:PurchaseInvoiceCreate, PurchaseOrderRequired %>" CssClass="text-danger" Display="Dynamic" />
                    </div>
                    <div class="col-md-4 mb-3">
                        <span class="form-label"><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceCreate, Supplier %>" /></span>
                        <div class="form-control bg-light"><asp:Label ID="lblSupplier" runat="server" Text="-" /></div>
                    </div>
                    <div class="col-md-4 mb-3">
                        <asp:Label runat="server" AssociatedControlID="txtDiscountAmount" CssClass="form-label" Text="<%$ Resources:PurchaseInvoiceCreate, DiscountAmount %>" />
                        <asp:TextBox ID="txtDiscountAmount" runat="server" Text="0" TextMode="Number" step="0.01" min="0" CssClass="form-control" />
                    </div>
                </div>
                <div class="mb-3">
                    <asp:Label runat="server" AssociatedControlID="txtNotes" CssClass="form-label" Text="<%$ Resources:PurchaseInvoiceCreate, Notes %>" />
                    <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" Rows="3" MaxLength="500" CssClass="form-control" />
                </div>
            </div>
        </div>

        <asp:Panel ID="pnlItems" runat="server" Visible="false" CssClass="card shadow-sm mb-4">
            <div class="card-header"><h5 class="mb-0"><asp:Literal runat="server" Text="<%$ Resources:PurchaseInvoiceCreate, InvoiceItems %>" /></h5></div>
            <div class="card-body">
                <div class="invoice-create-table-wrap">
                    <asp:GridView ID="gvInvoiceItems" runat="server" AutoGenerateColumns="False" DataKeyNames="PurchaseOrderItemId" CssClass="table table-bordered table-striped invoice-create-table" EmptyDataText="<%$ Resources:PurchaseInvoiceCreate, NoAvailableItems %>">
                        <Columns>
                            <asp:BoundField DataField="ItemCode" HeaderText="<%$ Resources:PurchaseInvoiceCreate, ItemCode %>" />
                            <asp:BoundField DataField="ItemName" HeaderText="<%$ Resources:PurchaseInvoiceCreate, ItemName %>" />
                            <asp:BoundField DataField="ReceivedQuantity" HeaderText="<%$ Resources:PurchaseInvoiceCreate, Received %>" DataFormatString="{0:N2}" />
                            <asp:BoundField DataField="InvoicedQuantity" HeaderText="<%$ Resources:PurchaseInvoiceCreate, PreviouslyInvoiced %>" DataFormatString="{0:N2}" />
                            <asp:BoundField DataField="AvailableQuantity" HeaderText="<%$ Resources:PurchaseInvoiceCreate, Available %>" DataFormatString="{0:N2}" />
                            <asp:TemplateField HeaderText="<%$ Resources:PurchaseInvoiceCreate, InvoiceQuantity %>">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtQuantity" runat="server" Text='<%# NumberInputValue(Eval("AvailableQuantity")) %>' CssClass="form-control" TextMode="Number" step="0.01" min="0" aria-label="<%$ Resources:PurchaseInvoiceCreate, InvoiceQuantity %>" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:PurchaseInvoiceCreate, UnitPrice %>">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtUnitPrice" runat="server" Text='<%# NumberInputValue(Eval("UnitPrice")) %>' CssClass="form-control" TextMode="Number" step="0.01" min="0" aria-label="<%$ Resources:PurchaseInvoiceCreate, UnitPrice %>" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:PurchaseInvoiceCreate, TaxRate %>">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtTaxRate" runat="server" Text="0.00" CssClass="form-control" TextMode="Number" step="0.01" min="0" max="100" aria-label="<%$ Resources:PurchaseInvoiceCreate, TaxRate %>" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:Panel>

        <div class="invoice-create-actions">
            <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:PurchaseInvoiceCreate, SaveDraft %>" CssClass="btn btn-primary" ValidationGroup="InvoiceForm" OnClick="btnSave_Click" />
        </div>
    </div>
</asp:Content>
