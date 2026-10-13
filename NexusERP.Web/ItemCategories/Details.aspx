<%@ Page Title="تفاصيل تصنيف الصنف"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Details.aspx.cs"
    Inherits="NexusERP.Web.ItemCategories.Details" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <div class="card shadow-sm">

            <div class="card-header">
                <h3 class="mb-0">تفاصيل تصنيف الصنف</h3>
            </div>

            <div class="card-body">

                <asp:Panel
                    ID="pnlError"
                    runat="server"
                    Visible="false"
                    CssClass="alert alert-danger">

                    <asp:Label
                        ID="lblError"
                        runat="server" />

                </asp:Panel>

                <dl class="row">

                    <dt class="col-sm-3">رمز التصنيف</dt>
                    <dd class="col-sm-9">
                        <asp:Label ID="lblCode" runat="server" />
                    </dd>

                    <dt class="col-sm-3">الاسم العربي</dt>
                    <dd class="col-sm-9">
                        <asp:Label ID="lblNameAr" runat="server" />
                    </dd>

                    <dt class="col-sm-3">الاسم الإنجليزي</dt>
                    <dd class="col-sm-9">
                        <asp:Label ID="lblNameEn" runat="server" />
                    </dd>

                    <dt class="col-sm-3">الوصف العربي</dt>
                    <dd class="col-sm-9">
                        <asp:Label ID="lblDescriptionAr" runat="server" />
                    </dd>

                    <dt class="col-sm-3">الوصف الإنجليزي</dt>
                    <dd class="col-sm-9">
                        <asp:Label ID="lblDescriptionEn" runat="server" />
                    </dd>

                </dl>

                <div class="d-flex gap-2">

                    <asp:HyperLink
                        ID="lnkEdit"
                        runat="server"
                        CssClass="btn btn-warning">
                        تعديل
                    </asp:HyperLink>

                    <asp:HyperLink
                        ID="lnkBack"
                        runat="server"
                        NavigateUrl="~/ItemCategories/Index.aspx"
                        CssClass="btn btn-secondary">
                        العودة إلى القائمة
                    </asp:HyperLink>

                </div>

            </div>
        </div>
    </div>

</asp:Content>