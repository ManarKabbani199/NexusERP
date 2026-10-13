<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="NexusERP.Web.Suppliers.Edit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-4">
    <h2 class="mb-4">تعديل المورد</h2>

    <asp:Label ID="lblError" runat="server"
        CssClass="text-danger d-block mb-3" />

    <asp:ValidationSummary ID="validationSummary"
        runat="server"
        ValidationGroup="SupplierForm"
        CssClass="alert alert-danger" />

    <asp:Panel ID="pnlForm" runat="server" Visible="false">
        <div class="mb-3">
            <label class="form-label">رمز المورد</label>
            <asp:TextBox ID="txtCode" runat="server"
                CssClass="form-control" MaxLength="20" />
            <asp:RequiredFieldValidator runat="server"
                ControlToValidate="txtCode"
                ValidationGroup="SupplierForm"
                ErrorMessage="رمز المورد مطلوب."
                Display="None" />
        </div>

        <div class="mb-3">
            <label class="form-label">الاسم بالعربية</label>
            <asp:TextBox ID="txtNameAr" runat="server"
                CssClass="form-control" MaxLength="150" />
            <asp:RequiredFieldValidator runat="server"
                ControlToValidate="txtNameAr"
                ValidationGroup="SupplierForm"
                ErrorMessage="الاسم بالعربية مطلوب."
                Display="None" />
        </div>

        <div class="mb-3">
            <label class="form-label">الاسم بالإنجليزية</label>
            <asp:TextBox ID="txtNameEn" runat="server"
                CssClass="form-control" MaxLength="150" />
            <asp:RequiredFieldValidator runat="server"
                ControlToValidate="txtNameEn"
                ValidationGroup="SupplierForm"
                ErrorMessage="الاسم بالإنجليزية مطلوب."
                Display="None" />
        </div>

        <div class="mb-3">
            <label class="form-label">جهة الاتصال</label>
            <asp:TextBox ID="txtContactPerson" runat="server"
                CssClass="form-control" MaxLength="150" />
        </div>

        <div class="mb-3">
            <label class="form-label">الهاتف</label>
            <asp:TextBox ID="txtPhone" runat="server"
                CssClass="form-control" MaxLength="30" />
        </div>

        <div class="mb-3">
            <label class="form-label">البريد الإلكتروني</label>
            <asp:TextBox ID="txtEmail" runat="server"
                CssClass="form-control" TextMode="Email" MaxLength="150" />
        </div>

        <div class="mb-3">
            <label class="form-label">الرقم الضريبي</label>
            <asp:TextBox ID="txtTaxNumber" runat="server"
                CssClass="form-control" MaxLength="50" />
        </div>

        <div class="mb-3">
            <label class="form-label">العنوان بالعربية</label>
            <asp:TextBox ID="txtAddressAr" runat="server"
                CssClass="form-control" MaxLength="300" />
        </div>

        <div class="mb-3">
            <label class="form-label">العنوان بالإنجليزية</label>
            <asp:TextBox ID="txtAddressEn" runat="server"
                CssClass="form-control" MaxLength="300" />
        </div>

        <div class="mb-3">
            <label class="form-label">مدة السداد بالأيام</label>
            <asp:TextBox ID="txtPaymentTermsDays" runat="server"
                CssClass="form-control" TextMode="Number" />
            <asp:RequiredFieldValidator runat="server"
                ControlToValidate="txtPaymentTermsDays"
                ValidationGroup="SupplierForm"
                ErrorMessage="مدة السداد مطلوبة."
                Display="None" />
            <asp:RangeValidator runat="server"
                ControlToValidate="txtPaymentTermsDays"
                ValidationGroup="SupplierForm"
                Type="Integer"
                MinimumValue="0"
                MaximumValue="3650"
                ErrorMessage="مدة السداد يجب أن تكون بين 0 و3650 يومًا."
                Display="None" />
        </div>

        <asp:Button ID="btnSave" runat="server"
            Text="حفظ التعديلات"
            CssClass="btn btn-primary"
            ValidationGroup="SupplierForm"
            OnClick="btnSave_Click" />
    </asp:Panel>

         <asp:HyperLink
     ID="linkCancel"
     runat="server"
             href="Index.aspx"
     CssClass="btn btn-secondary">
     إلغاء
 </asp:HyperLink>

  
</div>
</asp:Content>
