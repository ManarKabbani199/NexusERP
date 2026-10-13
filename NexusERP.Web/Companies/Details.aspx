<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="NexusERP.Web.Companies.Details" %>
<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4" dir="rtl">

        <div class="card shadow-sm">

            <div class="card-header bg-primary text-white">
                <h3 class="mb-0">تفاصيل الشركة</h3>
            </div>

            <div class="card-body">

                <asp:Panel ID="pnlCompany"
                    runat="server"
                    Visible="false">

                    <div class="table-responsive">

                        <table class="table table-bordered table-striped">

                            <tbody>

                                <tr>
                                    <th style="width: 30%;">رمز الشركة</th>
                                    <td>
                                        <asp:Label ID="lblCode" runat="server" />
                                    </td>
                                </tr>

                                <tr>
                                    <th>اسم الشركة بالعربية</th>
                                    <td>
                                        <asp:Label ID="lblNameAr" runat="server" />
                                    </td>
                                </tr>

                                <tr>
                                    <th>اسم الشركة بالإنجليزية</th>
                                    <td>
                                        <asp:Label ID="lblNameEn" runat="server" />
                                    </td>
                                </tr>

                                <tr>
                                    <th>رقم السجل التجاري</th>
                                    <td>
                                        <asp:Label ID="lblCommercialRegistrationNumber"
                                            runat="server" />
                                    </td>
                                </tr>

                                <tr>
                                    <th>الرقم الضريبي</th>
                                    <td>
                                        <asp:Label ID="lblTaxNumber" runat="server" />
                                    </td>
                                </tr>

                                <tr>
                                    <th>رقم الهاتف</th>
                                    <td>
                                        <asp:Label ID="lblPhone" runat="server" />
                                    </td>
                                </tr>

                                <tr>
                                    <th>البريد الإلكتروني</th>
                                    <td>
                                        <asp:Label ID="lblEmail" runat="server" />
                                    </td>
                                </tr>

                                <tr>
                                    <th>الموقع الإلكتروني</th>
                                    <td>
                                        <asp:HyperLink ID="lnkWebsite"
                                            runat="server"
                                            Target="_blank" />
                                    </td>
                                </tr>

                                <tr>
                                    <th>العنوان بالعربية</th>
                                    <td>
                                        <asp:Label ID="lblAddressAr" runat="server" />
                                    </td>
                                </tr>

                                <tr>
                                    <th>العنوان بالإنجليزية</th>
                                    <td>
                                        <asp:Label ID="lblAddressEn" runat="server" />
                                    </td>
                                </tr>

                                <tr>
                                    <th>العملة الأساسية</th>
                                    <td>
                                        <asp:Label ID="lblBaseCurrencyCode"
                                            runat="server" />
                                    </td>
                                </tr>

                                <tr>
                                    <th>اللغة الافتراضية</th>
                                    <td>
                                        <asp:Label ID="lblDefaultLanguage"
                                            runat="server" />
                                    </td>
                                </tr>

                            </tbody>

                        </table>

                    </div>

                    <div class="mt-3">

                        <asp:HyperLink ID="lnkEdit"
                            runat="server"
                            CssClass="btn btn-primary"
                            Text="تعديل" />

                        <a href="Index.aspx"
                            class="btn btn-secondary">
                            العودة إلى القائمة
                        </a>

                    </div>

                </asp:Panel>

                <asp:Label ID="lblMessage"
                    runat="server"
                    Visible="false"
                    CssClass="alert alert-danger d-block" />

            </div>

        </div>

    </div>

</asp:Content>

