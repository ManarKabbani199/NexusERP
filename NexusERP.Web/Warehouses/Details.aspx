<%@ Page Title="تفاصيل المستودع"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Details.aspx.cs"
    Inherits="NexusERP.Web.Warehouses.Details" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4" dir="rtl">

        <div class="card shadow-sm">

            <div class="card-header">
                <h3 class="mb-0">تفاصيل المستودع</h3>
            </div>

            <div class="card-body">

                <div class="row mb-3">
                    <div class="col-md-4 fw-bold">الكود:</div>
                    <div class="col-md-8">
                        <asp:Label ID="lblCode" runat="server" />
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-md-4 fw-bold">الاسم بالعربية:</div>
                    <div class="col-md-8">
                        <asp:Label ID="lblNameAr" runat="server" />
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-md-4 fw-bold">الاسم بالإنجليزية:</div>
                    <div class="col-md-8">
                        <asp:Label ID="lblNameEn" runat="server" />
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-md-4 fw-bold">الفرع:</div>
                    <div class="col-md-8">
                        <asp:Label ID="lblBranch" runat="server" />
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-md-4 fw-bold">العنوان بالعربية:</div>
                    <div class="col-md-8">
                        <asp:Label ID="lblAddressAr" runat="server" />
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-md-4 fw-bold">العنوان بالإنجليزية:</div>
                    <div class="col-md-8">
                        <asp:Label ID="lblAddressEn" runat="server" />
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-md-4 fw-bold">رقم الهاتف:</div>
                    <div class="col-md-8">
                        <asp:Label ID="lblPhone" runat="server" />
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-md-4 fw-bold">الحالة:</div>
                    <div class="col-md-8">
                        <asp:Label ID="lblStatus" runat="server" />
                    </div>
                </div>

                <hr />

                <asp:HyperLink
                    ID="linkEdit"
                    runat="server"
                    CssClass="btn btn-warning">
                    تعديل
                </asp:HyperLink>

                <asp:HyperLink
                    ID="linkDelete"
                    runat="server"
                    CssClass="btn btn-danger">
                    حذف
                </asp:HyperLink>

                <a href="Index.aspx" class="btn btn-secondary">
                    العودة إلى القائمة
                </a>

            </div>
        </div>

    </div>

</asp:Content>