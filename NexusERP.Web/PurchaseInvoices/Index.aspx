<%@ Page
    Title="<%$ Resources:PurchaseInvoices, Title %>"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Index.aspx.cs"
    Inherits="NexusERP.Web.PurchaseInvoices.Index" %>

<%@ Register
    Assembly="DevExpress.Web.v26.1, Version=26.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web"
    TagPrefix="dx" %>

<asp:Content
    ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <style>
        .pi-heading {
            display: flex;
            justify-content: space-between;
            align-items: center;
            flex-wrap: wrap;
            gap: 12px;
            margin-bottom: 24px;
        }

        .pi-heading h2 {
            margin: 0;
            color: #14243c;
            font-size: 24px;
        }

        .pi-filter-actions {
            display: flex;
            flex-wrap: wrap;
            gap: 8px;
        }

        .pi-report {
            margin: 18px 0;
        }

        .pi-grid-container {
            overflow-x: auto;
        }
    </style>

    <div class="pi-page">

        <div class="pi-heading">
            <h2>
                <asp:Literal
                    runat="server"
                    Text="<%$ Resources:PurchaseInvoices, Title %>" />
            </h2>

            <asp:HyperLink
                runat="server"
                NavigateUrl="~/PurchaseInvoices/Create.aspx"
                CssClass="btn btn-primary"
                Text="<%$ Resources:PurchaseInvoices, Create %>" />
        </div>

        <asp:Label
            ID="lblMessage"
            runat="server"
            Visible="false" />

        <div class="card shadow-sm mb-4">
            <div class="card-header">
                <h5 class="mb-0">
                    <asp:Literal
                        runat="server"
                        Text="<%$ Resources:PurchaseInvoices, Filters %>" />
                </h5>
            </div>

            <div class="card-body">
                <div class="row align-items-end">

                    <div class="col-md-5 mb-3">
                        <asp:Label
                            runat="server"
                            AssociatedControlID="txtSearch"
                            CssClass="form-label"
                            Text="<%$ Resources:PurchaseInvoices, Search %>" />

                        <asp:TextBox
                            ID="txtSearch"
                            runat="server"
                            CssClass="form-control"
                            placeholder="<%$ Resources:PurchaseInvoices, SearchPlaceholder %>" />
                    </div>

                    <div class="col-md-4 mb-3">
                        <asp:Label
                            runat="server"
                            AssociatedControlID="ddlStatus"
                            CssClass="form-label"
                            Text="<%$ Resources:PurchaseInvoices, InvoiceStatus %>" />

                        <asp:DropDownList
                            ID="ddlStatus"
                            runat="server"
                            CssClass="form-select">

                            <asp:ListItem
                                Text="<%$ Resources:PurchaseInvoices, AllStatuses %>"
                                Value="" />

                            <asp:ListItem
                                Text="<%$ Resources:PurchaseInvoices, Draft %>"
                                Value="1" />

                            <asp:ListItem
                                Text="<%$ Resources:PurchaseInvoices, Posted %>"
                                Value="2" />

                            <asp:ListItem
                                Text="<%$ Resources:PurchaseInvoices, Paid %>"
                                Value="3" />

                            <asp:ListItem
                                Text="<%$ Resources:PurchaseInvoices, Cancelled %>"
                                Value="4" />
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-3 mb-3">
                        <div class="pi-filter-actions">
                            <asp:Button
                                ID="btnSearch"
                                runat="server"
                                Text="<%$ Resources:PurchaseInvoices, Search %>"
                                CssClass="btn btn-primary"
                                CausesValidation="false"
                                OnClick="btnSearch_Click" />

                            <asp:Button
                                ID="btnReset"
                                runat="server"
                                Text="<%$ Resources:PurchaseInvoices, Reset %>"
                                CssClass="btn btn-secondary"
                                CausesValidation="false"
                                OnClick="btnReset_Click" />
                        </div>
                    </div>

                </div>
            </div>
        </div>

        <div class="pi-report">
            <asp:HyperLink
                ID="lnkInvoiceReport"
                runat="server"
                NavigateUrl="~/PurchaseInvoices/InvoiceReport.aspx"
                CssClass="btn btn-outline-primary"
                Text="<%$ Resources:PurchaseInvoices, Report %>" />
        </div>

        <div class="card shadow-sm">
            <div class="card-header">
                <h5 class="mb-0">
                    <asp:Literal
                        runat="server"
                        Text="<%$ Resources:PurchaseInvoices, List %>" />
                </h5>
            </div>

            <div class="card-body">
                <div class="pi-grid-container">

                    <dx:ASPxGridView
                        ID="gvPurchaseInvoices"
                        runat="server"
                        AutoGenerateColumns="false"
                        KeyFieldName="Id"
                        Width="100%"
                        Theme="Office365"
                        Font-Names="Tahoma"
                        Font-Size="10pt">

                        <Settings
                            ShowFilterRow="true"
                            ShowGroupPanel="true" />

                        <SettingsBehavior
                            AllowSort="true"
                            AllowGroup="true" />

                        <SettingsPager PageSize="10" />

                        <SettingsSearchPanel
                            Visible="true"
                            ShowApplyButton="true"
                            ShowClearButton="true"
                            AllowTextInputTimer="false"
                            ColumnNames="InvoiceNumber;SupplierName;PurchaseOrderNumber;StatusName" />

                        <SettingsText
                            EmptyDataRow="<%$ Resources:PurchaseInvoices, Empty %>"
                            GroupPanel="<%$ Resources:PurchaseInvoices, GroupHint %>"
                            SearchPanelEditorNullText="<%$ Resources:PurchaseInvoices, GridSearchPlaceholder %>"
                            CommandApplySearchPanelFilter="<%$ Resources:PurchaseInvoices, Search %>"
                            CommandClearSearchPanelFilter="<%$ Resources:PurchaseInvoices, Clear %>" />

                        <Columns>

                            <dx:GridViewDataTextColumn
                                FieldName="InvoiceNumber"
                                Caption="<%$ Resources:PurchaseInvoices, InvoiceNumber %>"
                                VisibleIndex="0" />

                            <dx:GridViewDataDateColumn
                                FieldName="InvoiceDate"
                                Caption="<%$ Resources:PurchaseInvoices, InvoiceDate %>"
                                VisibleIndex="1">

                                <PropertiesDateEdit
                                    DisplayFormatString="yyyy-MM-dd" />
                            </dx:GridViewDataDateColumn>

                            <dx:GridViewDataDateColumn
                                FieldName="DueDate"
                                Caption="<%$ Resources:PurchaseInvoices, DueDate %>"
                                VisibleIndex="2">

                                <PropertiesDateEdit
                                    DisplayFormatString="yyyy-MM-dd" />
                            </dx:GridViewDataDateColumn>

                            <dx:GridViewDataTextColumn
                                FieldName="SupplierName"
                                Caption="<%$ Resources:PurchaseInvoices, Supplier %>"
                                VisibleIndex="3" />

                            <dx:GridViewDataTextColumn
                                FieldName="PurchaseOrderNumber"
                                Caption="<%$ Resources:PurchaseInvoices, PurchaseOrder %>"
                                VisibleIndex="4" />

                            <dx:GridViewDataTextColumn
                                FieldName="StatusName"
                                Caption="<%$ Resources:PurchaseInvoices, Status %>"
                                VisibleIndex="5">

                                <DataItemTemplate>
                                    <asp:Label
                                        ID="lblRowStatus"
                                        runat="server"
                                        Text='<%# Eval("StatusName") %>'
                                        CssClass='<%# Eval("StatusCssClass") %>' />
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>

                            <dx:GridViewDataSpinEditColumn
                                FieldName="GrandTotal"
                                Caption="<%$ Resources:PurchaseInvoices, GrandTotal %>"
                                VisibleIndex="6">

                                <PropertiesSpinEdit
                                    DisplayFormatString="N2" />
                            </dx:GridViewDataSpinEditColumn>

                            <dx:GridViewDataTextColumn
                                Caption="<%$ Resources:PurchaseInvoices, Actions %>"
                                VisibleIndex="7">

                                <Settings
                                    AllowSort="False"
                                    AllowGroup="False"
                                    AllowAutoFilter="False" />

                                <DataItemTemplate>
                                    <asp:HyperLink
                                        ID="lnkDetails"
                                        runat="server"
                                        Text="<%$ Resources:PurchaseInvoices, Details %>"
                                        CssClass="btn btn-sm btn-info"
                                        NavigateUrl='<%# "Details.aspx?id=" + Eval("Id") %>' />
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>

                        </Columns>
                    </dx:ASPxGridView>

                </div>
            </div>
        </div>

    </div>
</asp:Content>