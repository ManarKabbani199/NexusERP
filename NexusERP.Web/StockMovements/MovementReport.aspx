<%@ Page Title="تقرير حركات المخزون"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="MovementReport.aspx.cs"
    Inherits="NexusERP.Web.StockMovements.MovementReport" %>

<%@ Register Assembly="CrystalDecisions.Web"
    Namespace="CrystalDecisions.Web"
    TagPrefix="CR" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <h2 dir="rtl">تقرير حركات المخزون</h2>

    <div dir="rtl" style="margin-bottom: 15px;">
        <asp:Label runat="server"
            AssociatedControlID="txtFromDate"
            Text="من تاريخ:" />

        <asp:TextBox ID="txtFromDate"
            runat="server"
            TextMode="Date" />

        <asp:Label runat="server"
            AssociatedControlID="txtToDate"
            Text="إلى تاريخ:" />

        <asp:TextBox ID="txtToDate"
            runat="server"
            TextMode="Date" />
    </div>

    <div dir="rtl" style="margin-bottom: 15px;">
        <asp:Label runat="server"
            AssociatedControlID="ddlWarehouses"
            Text="المستودع:" />

        <asp:DropDownList ID="ddlWarehouses"
            runat="server" />

        <asp:Label runat="server"
            AssociatedControlID="ddlItems"
            Text="الصنف:" />

        <asp:DropDownList ID="ddlItems"
            runat="server" />

        <asp:Label runat="server"
            AssociatedControlID="ddlMovementTypes"
            Text="نوع الحركة:" />

        <asp:DropDownList ID="ddlMovementTypes"
            runat="server" />

        <asp:Button ID="btnShowReport"
            runat="server"
            Text="عرض التقرير"
            CausesValidation="false" />
    </div>

    <div dir="rtl">
        <asp:Label ID="lblMessage"
            runat="server"
            Visible="false" />
    </div>

    <CR:CrystalReportViewer ID="crvStockMovements"
        runat="server"
        AutoDataBind="false"
        ToolPanelView="None"
        HasToggleGroupTreeButton="false"
        HasToggleParameterPanelButton="false"
        EnableParameterPrompt="false" />

</asp:Content>