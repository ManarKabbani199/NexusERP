<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="NexusERP.Web.Suppliers.Details" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-4">
    <h2 class="mb-4">تفاصيل المورد</h2>

    <asp:Label ID="lblError" runat="server"
        CssClass="text-danger d-block mb-3" />

    <asp:Panel ID="pnlDetails" runat="server" Visible="false">
        <table class="table table-bordered">
            <tr><th>الرمز</th><td><asp:Label ID="lblCode" runat="server" /></td></tr>
            <tr><th>الاسم بالعربية</th><td><asp:Label ID="lblNameAr" runat="server" /></td></tr>
            <tr><th>الاسم بالإنجليزية</th><td><asp:Label ID="lblNameEn" runat="server" /></td></tr>
            <tr><th>جهة الاتصال</th><td><asp:Label ID="lblContactPerson" runat="server" /></td></tr>
            <tr><th>الهاتف</th><td><asp:Label ID="lblPhone" runat="server" /></td></tr>
            <tr><th>البريد الإلكتروني</th><td><asp:Label ID="lblEmail" runat="server" /></td></tr>
            <tr><th>الرقم الضريبي</th><td><asp:Label ID="lblTaxNumber" runat="server" /></td></tr>
            <tr><th>العنوان بالعربية</th><td><asp:Label ID="lblAddressAr" runat="server" /></td></tr>
            <tr><th>العنوان بالإنجليزية</th><td><asp:Label ID="lblAddressEn" runat="server" /></td></tr>
            <tr><th>مدة السداد</th><td><asp:Label ID="lblPaymentTermsDays" runat="server" /></td></tr>
        </table>
    </asp:Panel>

    <a href="Index.aspx" class="btn btn-secondary">العودة للموردين</a>
</div>
</asp:Content>
