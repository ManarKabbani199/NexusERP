<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Deactivate.aspx.cs" Inherits="NexusERP.Web.Suppliers.Deactivate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-4">
    <h2 class="mb-4">تعطيل مورد</h2>

    <asp:Label ID="lblMessage" runat="server"
        CssClass="text-danger d-block mb-3" />

    <asp:Panel ID="pnlConfirm" runat="server" Visible="false">
        <p>
            هل تريدين تعطيل المورد
            <strong><asp:Label ID="lblSupplierName" runat="server" /></strong>؟
        </p>
        <p class="text-muted">سيبقى سجل المورد محفوظًا، لكنه لن يكون نشطًا.</p>

        <asp:Button ID="btnDeactivate" runat="server"
            Text="تأكيد التعطيل"
            CssClass="btn btn-danger"
            CausesValidation="false"
            OnClick="btnDeactivate_Click" />
    </asp:Panel>

    <a href="Index.aspx" class="btn btn-secondary">إلغاء</a>
</div>
</asp:Content>
