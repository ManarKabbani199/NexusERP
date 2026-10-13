<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Activate.aspx.cs" Inherits="NexusERP.Web.Suppliers.Activate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-4">
    <h2 class="mb-4">إعادة تفعيل مورد</h2>

    <asp:Label ID="lblMessage" runat="server"
        CssClass="text-danger d-block mb-3" />

    <asp:Panel ID="pnlConfirm" runat="server" Visible="false">
        <p>
            هل تريدين إعادة تفعيل المورد
            <strong><asp:Label ID="lblSupplierName" runat="server" /></strong>؟
        </p>

        <asp:Button ID="btnActivate" runat="server"
            Text="تأكيد إعادة التفعيل"
            CssClass="btn btn-success"
            CausesValidation="false"
            OnClick="btnActivate_Click" />
    </asp:Panel>

    <a href="Index.aspx" class="btn btn-secondary">إلغاء</a>
</div>
</asp:Content>
