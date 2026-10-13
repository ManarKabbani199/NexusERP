<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="NexusERP.Web.Companies.Edit" %>
<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4" dir="rtl">

        <div class="card shadow-sm">

            <div class="card-header bg-primary text-white">
                <h3 class="mb-0">عديل بيانات الشركة</h3>
            </div>

            <div class="card-body">

                <asp:ValidationSummary
                    ID="vsCompany"
                    runat="server"
                    ValidationGroup="Company"
                    CssClass="alert alert-danger"
                    HeaderText="يرجى تصحيح الأخطاء التالية:" />

                <div class="row">

                    <!-- رمز الشركة -->
                    <div class="col-md-6 mb-3">
                        <asp:Label runat="server"
                            AssociatedControlID="txtCode"
                            Text="رمز الشركة" />

                        <asp:TextBox ID="txtCode"
                            runat="server"
                            CssClass="form-control"
                            MaxLength="20" />

                        <asp:RequiredFieldValidator
                            runat="server"
                            ControlToValidate="txtCode"
                            ValidationGroup="Company"
                            ErrorMessage="رمز الشركة مطلوب"
                            CssClass="text-danger"
                            Display="Dynamic" />
                    </div>

                    <!-- العملة الأساسية -->
                    <div class="col-md-6 mb-3">
                        <asp:Label runat="server"
                            AssociatedControlID="txtBaseCurrencyCode"
                            Text="رمز العملة الأساسية" />

                        <asp:TextBox ID="txtBaseCurrencyCode"
                            runat="server"
                            CssClass="form-control"
                            MaxLength="3"
                            Text="KWD" />

                        <asp:RequiredFieldValidator
                            runat="server"
                            ControlToValidate="txtBaseCurrencyCode"
                            ValidationGroup="Company"
                            ErrorMessage="رمز العملة مطلوب"
                            CssClass="text-danger"
                            Display="Dynamic" />
                    </div>

                    <!-- الاسم العربي -->
                    <div class="col-md-6 mb-3">
                        <asp:Label runat="server"
                            AssociatedControlID="txtNameAr"
                            Text="اسم الشركة بالعربية" />

                        <asp:TextBox ID="txtNameAr"
                            runat="server"
                            CssClass="form-control"
                            MaxLength="150" />

                        <asp:RequiredFieldValidator
                            runat="server"
                            ControlToValidate="txtNameAr"
                            ValidationGroup="Company"
                            ErrorMessage="اسم الشركة بالعربية مطلوب"
                            CssClass="text-danger"
                            Display="Dynamic" />
                    </div>

                    <!-- الاسم الإنجليزي -->
                    <div class="col-md-6 mb-3">
                        <asp:Label runat="server"
                            AssociatedControlID="txtNameEn"
                            Text="اسم الشركة بالإنجليزية" />

                        <asp:TextBox ID="txtNameEn"
                            runat="server"
                            CssClass="form-control"
                            MaxLength="150" />

                        <asp:RequiredFieldValidator
                            runat="server"
                            ControlToValidate="txtNameEn"
                            ValidationGroup="Company"
                            ErrorMessage="اسم الشركة بالإنجليزية مطلوب"
                            CssClass="text-danger"
                            Display="Dynamic" />
                    </div>

                    <!-- السجل التجاري -->
                    <div class="col-md-6 mb-3">
                        <asp:Label runat="server"
                            AssociatedControlID="txtCommercialRegistrationNumber"
                            Text="رقم السجل التجاري" />

                        <asp:TextBox ID="txtCommercialRegistrationNumber"
                            runat="server"
                            CssClass="form-control"
                            MaxLength="50" />
                    </div>

                    <!-- الرقم الضريبي -->
                    <div class="col-md-6 mb-3">
                        <asp:Label runat="server"
                            AssociatedControlID="txtTaxNumber"
                            Text="الرقم الضريبي" />

                        <asp:TextBox ID="txtTaxNumber"
                            runat="server"
                            CssClass="form-control"
                            MaxLength="50" />
                    </div>

                    <!-- الهاتف -->
                    <div class="col-md-6 mb-3">
                        <asp:Label runat="server"
                            AssociatedControlID="txtPhone"
                            Text="رقم الهاتف" />

                        <asp:TextBox ID="txtPhone"
                            runat="server"
                            CssClass="form-control"
                            MaxLength="30" />
                    </div>

                    <!-- البريد الإلكتروني -->
                    <div class="col-md-6 mb-3">
                        <asp:Label runat="server"
                            AssociatedControlID="txtEmail"
                            Text="البريد الإلكتروني" />

                        <asp:TextBox ID="txtEmail"
                            runat="server"
                            TextMode="Email"
                            CssClass="form-control"
                            MaxLength="150" />

                        <asp:RegularExpressionValidator
                            runat="server"
                            ControlToValidate="txtEmail"
                            ValidationGroup="Company"
                            ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                            ErrorMessage="صيغة البريد الإلكتروني غير صحيحة"
                            CssClass="text-danger"
                            Display="Dynamic" />
                    </div>

                    <!-- الموقع الإلكتروني -->
                    <div class="col-md-6 mb-3">
                        <asp:Label runat="server"
                            AssociatedControlID="txtWebsite"
                            Text="الموقع الإلكتروني" />

                        <asp:TextBox ID="txtWebsite"
                            runat="server"
                            CssClass="form-control"
                            MaxLength="200"
                            placeholder="https://example.com" />
                    </div>

                    <!-- اللغة الافتراضية -->
                    <div class="col-md-6 mb-3">
                        <asp:Label runat="server"
                            AssociatedControlID="ddlDefaultLanguage"
                            Text="اللغة الافتراضية" />

                        <asp:DropDownList ID="ddlDefaultLanguage"
                            runat="server"
                            CssClass="form-control">

                            <asp:ListItem Text="العربية" Value="ar" />
                            <asp:ListItem Text="English" Value="en" />

                        </asp:DropDownList>
                    </div>

                    <!-- العنوان العربي -->
                    <div class="col-md-6 mb-3">
                        <asp:Label runat="server"
                            AssociatedControlID="txtAddressAr"
                            Text="العنوان بالعربية" />

                        <asp:TextBox ID="txtAddressAr"
                            runat="server"
                            TextMode="MultiLine"
                            Rows="3"
                            CssClass="form-control"
                            MaxLength="250" />
                    </div>

                    <!-- العنوان الإنجليزي -->
                    <div class="col-md-6 mb-3">
                        <asp:Label runat="server"
                            AssociatedControlID="txtAddressEn"
                            Text="العنوان بالإنجليزية" />

                        <asp:TextBox ID="txtAddressEn"
                            runat="server"
                            TextMode="MultiLine"
                            Rows="3"
                            CssClass="form-control"
                            MaxLength="250" />
                    </div>

                </div>

                <asp:Label ID="lblMessage"
                    runat="server"
                    Visible="false"
                    CssClass="alert alert-danger d-block" />

                <div class="mt-3">

                    <asp:Button ID="btnUpdate"
                        runat="server"
                        Text="حفظ التعديلات"
                        CssClass="btn btn-primary"
                        ValidationGroup="Company"
                        OnClick="btnUpdate_Click" />

                    <a href="Index.aspx" class="btn btn-secondary">
                        إلغاء
                    </a>

                </div>

            </div>
        </div>
    </div>

</asp:Content>