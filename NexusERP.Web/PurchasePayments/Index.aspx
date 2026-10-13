<%@ Page Title="<%$ Resources:PurchasePayments, Title %>"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Index.aspx.cs"
    Inherits="NexusERP.Web.PurchasePayments.Index" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .payments-heading { display:flex; align-items:center; justify-content:space-between; flex-wrap:wrap; gap:12px; margin-bottom:24px; }
        .payments-heading h2 { margin:0; color:#14243c; font-size:24px; }
        .payments-page .card { border-color:#e3e9f2; border-radius:12px; overflow:hidden; }
        .payments-page .card-header { background:#f7f9fd; padding:16px 20px; }
        .payments-page .card-header h5 { color:#25324a; font-size:17px; }
        .payments-page .card-body { padding:20px; }
        .payments-table-wrap { overflow-x:auto; }
        .payments-table { min-width:850px; margin-bottom:0; }
        .payments-table th { background:#eef3fa; white-space:nowrap; text-align:inherit; }
        .payments-table td { vertical-align:middle; }
        .payments-table .badge { white-space:nowrap; }
    </style>

    <div class="payments-page">
        <div class="payments-heading">
            <h2><asp:Literal runat="server" Text="<%$ Resources:PurchasePayments, Title %>" /></h2>
            <asp:HyperLink ID="lnkPaymentReport" runat="server"
                NavigateUrl="~/PurchasePayments/PaymentReport.aspx"
                CssClass="btn btn-outline-primary"
                Text="<%$ Resources:PurchasePayments, Report %>" />
        </div>

        <asp:Panel ID="pnlMessage" runat="server" Visible="false" role="alert">
            <asp:Literal ID="litMessage" runat="server" />
        </asp:Panel>

        <div class="card shadow-sm">
            <div class="card-header"><h5 class="mb-0"><asp:Literal runat="server" Text="<%$ Resources:PurchasePayments, List %>" /></h5></div>
            <div class="card-body">
                <div class="payments-table-wrap">
                    <asp:GridView ID="gvPurchasePayments" runat="server"
                        AutoGenerateColumns="False"
                        CssClass="table table-striped table-bordered payments-table"
                        EmptyDataText="<%$ Resources:PurchasePayments, Empty %>">
                        <Columns>
                            <asp:BoundField DataField="PaymentNumber" HeaderText="<%$ Resources:PurchasePayments, PaymentNumber %>" />
                            <asp:BoundField DataField="PaymentDate" HeaderText="<%$ Resources:PurchasePayments, PaymentDate %>" DataFormatString="{0:yyyy-MM-dd}" />
                            <asp:BoundField DataField="InvoiceNumber" HeaderText="<%$ Resources:PurchasePayments, InvoiceNumber %>" />
                            <asp:BoundField DataField="SupplierName" HeaderText="<%$ Resources:PurchasePayments, Supplier %>" />
                            <asp:BoundField DataField="Amount" HeaderText="<%$ Resources:PurchasePayments, Amount %>" DataFormatString="{0:N2}" />
                            <asp:BoundField DataField="PaymentMethodName" HeaderText="<%$ Resources:PurchasePayments, PaymentMethod %>" />
                            <asp:TemplateField HeaderText="<%$ Resources:PurchasePayments, Status %>">
                                <ItemTemplate>
                                    <asp:Label runat="server"
                                        CssClass='<%# Convert.ToBoolean(Eval("IsCancelled")) ? "badge bg-danger" : "badge bg-success" %>'
                                        Text='<%# Convert.ToBoolean(Eval("IsCancelled")) ? Translate("Cancelled") : Translate("Active") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="<%$ Resources:PurchasePayments, Actions %>">
                                <ItemTemplate>
                                    <asp:HyperLink runat="server"
                                        CssClass="btn btn-info btn-sm"
                                        NavigateUrl='<%# "Details.aspx?id=" + Eval("Id") %>'
                                        Text="<%$ Resources:PurchasePayments, Details %>" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
