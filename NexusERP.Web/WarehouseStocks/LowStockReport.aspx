<%@ Page Title="تقرير الأصناف منخفضة المخزون"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="LowStockReport.aspx.cs"
    Inherits="NexusERP.Web.WarehouseStocks.LowStockReport" %>

<%@ Register Assembly="CrystalDecisions.Web"
    Namespace="CrystalDecisions.Web"
    TagPrefix="CR" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <h2>تقرير الأصناف منخفضة المخزون</h2>

    <div dir="rtl" style="margin-bottom: 15px;">
        <asp:Label
            ID="lblWarehouse"
            runat="server"
            AssociatedControlID="ddlWarehouses"
            Text="المستودع:" />

        <asp:DropDownList
            ID="ddlWarehouses"
            runat="server" />

        <asp:Button
            ID="btnShowReport"
            runat="server"
            Text="عرض التقرير"
            CausesValidation="false" />
    </div>

    <asp:Label
        ID="lblMessage"
        runat="server"
        Visible="false"
        Text="لا توجد أصناف منخفضة المخزون في المستودعات المحددة." />

    <CR:CrystalReportViewer
        ID="crvLowStock"
        runat="server"
        AutoDataBind="false"
        ToolPanelView="None"
        HasToggleGroupTreeButton="false"
        HasToggleParameterPanelButton="false"
        EnableDatabaseLogonPrompt="false"
        EnableParameterPrompt="false" />

</asp:Content>