<%@ Page Title="تقرير أرصدة المستودعات"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="StockReport.aspx.cs"
    Inherits="NexusERP.Web.WarehouseStocks.StockReport" %>

<%@ Register Assembly="CrystalDecisions.Web"
    Namespace="CrystalDecisions.Web"
    TagPrefix="CR" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <h2>تقرير أرصدة المستودعات</h2>

    <div dir="rtl" style="margin-bottom: 15px;">
    <asp:Label
        ID="lblWarehouse"
        runat="server"
        AssociatedControlID="ddlWarehouses"
        Text="المستودع:" />

    <asp:DropDownList
        ID="ddlWarehouses"
        runat="server" />

        <asp:CheckBox
    ID="chkHideZeroStock"
    runat="server"
    Text="إخفاء الأرصدة الصفرية" />

    <asp:Button
        ID="btnShowReport"
        runat="server"
        Text="عرض التقرير"
        CausesValidation="false" />
</div>

    <CR:CrystalReportViewer
        ID="crvWarehouseStock"
        runat="server"
        AutoDataBind="false"
        ToolPanelView="None"
        HasToggleGroupTreeButton="false"
        HasToggleParameterPanelButton="false"
        EnableDatabaseLogonPrompt="false"
        EnableParameterPrompt="false" />

</asp:Content>