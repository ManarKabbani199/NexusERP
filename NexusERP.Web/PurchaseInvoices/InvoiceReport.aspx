<%@ Page Title="تقرير فواتير المشتريات"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="InvoiceReport.aspx.cs"
    Inherits="NexusERP.Web.PurchaseInvoices.InvoiceReport" %>

<%@ Register Assembly="CrystalDecisions.Web"
    Namespace="CrystalDecisions.Web"
    TagPrefix="CR" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <h2 dir="rtl">تقرير فواتير المشتريات</h2>

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
            AssociatedControlID="ddlSuppliers"
            Text="المورد:" />

        <asp:DropDownList ID="ddlSuppliers"
            runat="server" />

        <asp:Label runat="server"
            AssociatedControlID="ddlStatuses"
            Text="حالة الفاتورة:" />

        <asp:DropDownList ID="ddlStatuses" runat="server">
            <asp:ListItem Text="جميع الحالات" Value="0" />
            <asp:ListItem Text="مسودة" Value="1" />
            <asp:ListItem Text="مُرحّلة" Value="2" />
            <asp:ListItem Text="مدفوعة" Value="3" />
            <asp:ListItem Text="ملغاة" Value="4" />
        </asp:DropDownList>

        <asp:Button ID="btnShowReport"
            runat="server"
            Text="عرض التقرير"
            CausesValidation="false" />
    </div>

    <div dir="rtl" style="margin-bottom: 10px;">
        <asp:Label ID="lblMessage"
            runat="server"
            Visible="false" />
    </div>

    <CR:CrystalReportViewer ID="crvPurchaseInvoices"
        runat="server"
        AutoDataBind="false"
        ToolPanelView="None"
        HasToggleGroupTreeButton="false"
        HasToggleParameterPanelButton="false"
        EnableParameterPrompt="false" />

</asp:Content>