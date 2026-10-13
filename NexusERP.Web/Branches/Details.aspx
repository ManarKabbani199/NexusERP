<%@ Page Title="تفاصيل الفرع"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Details.aspx.cs"
    Inherits="NexusERP.Web.Branches.Details" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <div class="card shadow-sm">

            <div class="card-header">
                <h3 class="mb-0">تفاصيل الفرع</h3>
            </div>

            <div class="card-body">

                <dl class="row">

                    <dt class="col-sm-3">الشركة</dt>
                    <dd class="col-sm-9">
                        <asp:Label
                            ID="lblCompany"
                            runat="server" />
                    </dd>

                    <dt class="col-sm-3">كود الفرع</dt>
                    <dd class="col-sm-9">
                        <asp:Label
                            ID="lblCode"
                            runat="server" />
                    </dd>

                    <dt class="col-sm-3">الاسم بالعربية</dt>
                    <dd class="col-sm-9">
                        <asp:Label
                            ID="lblNameAr"
                            runat="server" />
                    </dd>

                    <dt class="col-sm-3">الاسم بالإنجليزية</dt>
                    <dd class="col-sm-9">
                        <asp:Label
                            ID="lblNameEn"
                            runat="server" />
                    </dd>

                    <dt class="col-sm-3">العنوان بالعربية</dt>
                    <dd class="col-sm-9">
                        <asp:Label
                            ID="lblAddressAr"
                            runat="server" />
                    </dd>

                    <dt class="col-sm-3">العنوان بالإنجليزية</dt>
                    <dd class="col-sm-9">
                        <asp:Label
                            ID="lblAddressEn"
                            runat="server" />
                    </dd>

                    <dt class="col-sm-3">الهاتف</dt>
                    <dd class="col-sm-9">
                        <asp:Label
                            ID="lblPhone"
                            runat="server" />
                    </dd>

                    <dt class="col-sm-3">الحالة</dt>
                    <dd class="col-sm-9">
                        <asp:Label
                            ID="lblStatus"
                            runat="server" />
                    </dd>

                </dl>

                <asp:HyperLink
                    ID="lnkEdit"
                    runat="server"
                    Text="تعديل"
                    CssClass="btn btn-warning" />

                <a href="Index.aspx"
                   class="btn btn-secondary">
                    العودة إلى الفروع
                </a>

            </div>

        </div>

    </div>

</asp:Content>