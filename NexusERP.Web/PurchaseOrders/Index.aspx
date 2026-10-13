<%@ Page Title="<%$ Resources:PurchaseOrders, Title %>"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Index.aspx.cs"
    Inherits="NexusERP.Web.PurchaseOrders.Index" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .orders-heading { display:flex; justify-content:space-between; align-items:center; flex-wrap:wrap; gap:12px; margin-bottom:24px; }
        .orders-heading h2 { margin:0; color:#14243c; font-size:24px; }
        .orders-page .card { border-color:#e3e9f2; border-radius:12px; overflow:hidden; }
        .orders-page .card-header { background:#f7f9fd; padding:16px 20px; }
        .orders-page .card-header h5 { color:#25324a; font-size:17px; }
        .orders-page .card-body { padding:20px; }
        .orders-table-wrap { overflow-x:auto; }
        .orders-table { min-width:850px; margin-bottom:0; }
        .orders-table th { background:#eef3fa; white-space:nowrap; text-align:inherit; }
        .orders-table td { vertical-align:middle; }
        .orders-row-actions { display:flex; flex-wrap:wrap; gap:8px; }
    </style>

    <div class="orders-page">
        <div class="orders-heading">
            <h2><asp:Literal runat="server" Text="<%$ Resources:PurchaseOrders, Title %>" /></h2>
            <asp:HyperLink runat="server" NavigateUrl="~/PurchaseOrders/Create.aspx"
                CssClass="btn btn-primary" Text="<%$ Resources:PurchaseOrders, Create %>" />
        </div>

        <asp:Label ID="lblMessage" runat="server" Visible="false" role="alert" />

        <div class="card shadow-sm">
            <div class="card-header"><h5 class="mb-0"><asp:Literal runat="server" Text="<%$ Resources:PurchaseOrders, List %>" /></h5></div>
            <div class="card-body">
                <div class="orders-table-wrap">
                    <asp:GridView ID="gvPurchaseOrders" runat="server"
                        AutoGenerateColumns="False"
                        CssClass="table table-striped table-bordered orders-table"
                        EmptyDataText="<%$ Resources:PurchaseOrders, Empty %>"
                        OnRowCommand="gvPurchaseOrders_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="OrderNumber" HeaderText="<%$ Resources:PurchaseOrders, OrderNumber %>" />
                            <asp:BoundField DataField="OrderDate" HeaderText="<%$ Resources:PurchaseOrders, OrderDate %>" DataFormatString="{0:yyyy-MM-dd}" />
                            <asp:BoundField DataField="SupplierName" HeaderText="<%$ Resources:PurchaseOrders, Supplier %>" />
                            <asp:BoundField DataField="StatusName" HeaderText="<%$ Resources:PurchaseOrders, Status %>" />
                            <asp:BoundField DataField="ItemsCount" HeaderText="<%$ Resources:PurchaseOrders, ItemsCount %>" />
                            <asp:BoundField DataField="GrandTotal" HeaderText="<%$ Resources:PurchaseOrders, GrandTotal %>" DataFormatString="{0:N2}" />
                            <asp:TemplateField HeaderText="<%$ Resources:PurchaseOrders, Actions %>">
                                <ItemTemplate>
                                    <div class="orders-row-actions">
                                        <asp:HyperLink runat="server"
                                            NavigateUrl='<%# "Details.aspx?id=" + Eval("Id") %>'
                                            CssClass="btn btn-sm btn-info"
                                            Text="<%$ Resources:PurchaseOrders, Details %>" />

                                        <asp:LinkButton ID="btnDelete" runat="server"
                                            Text="<%$ Resources:PurchaseOrders, Delete %>"
                                            CssClass="btn btn-sm btn-danger"
                                            CausesValidation="false"
                                            CommandName="DeleteOrder"
                                            CommandArgument='<%# Eval("Id") %>'
                                            Visible='<%# Convert.ToBoolean(Eval("CanDelete")) %>'
                                            OnClientClick='<%# DeleteConfirmationScript() %>' />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
