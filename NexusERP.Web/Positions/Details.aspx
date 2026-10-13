<%@ Page Title="تفاصيل الوظيفة"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Details.aspx.cs"
    Inherits="NexusERP.Web.Positions.Details" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <asp:Panel
            ID="pnlError"
            runat="server"
            CssClass="alert alert-danger"
            Visible="false">

            <asp:Label
                ID="lblError"
                runat="server">
            </asp:Label>

        </asp:Panel>

        <asp:Panel
            ID="pnlDetails"
            runat="server"
            CssClass="card shadow-sm"
            Visible="false">

            <div class="card-header">
                <h3 class="mb-0">تفاصيل الوظيفة</h3>
            </div>

            <div class="card-body">

                <div class="row mb-3">
                    <div class="col-md-4 fw-bold">
                        كود الوظيفة
                    </div>

                    <div class="col-md-8">
                        <asp:Label
                            ID="lblCode"
                            runat="server" />
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-md-4 fw-bold">
                        المسمى بالعربية
                    </div>

                    <div class="col-md-8">
                        <asp:Label
                            ID="lblTitleAr"
                            runat="server" />
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-md-4 fw-bold">
                        المسمى بالإنجليزية
                    </div>

                    <div class="col-md-8">
                        <asp:Label
                            ID="lblTitleEn"
                            runat="server" />
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-md-4 fw-bold">
                        القسم
                    </div>

                    <div class="col-md-8">
                        <asp:Label
                            ID="lblDepartment"
                            runat="server" />
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-md-4 fw-bold">
                        المستوى الوظيفي
                    </div>

                    <div class="col-md-8">
                        <asp:Label
                            ID="lblJobLevel"
                            runat="server" />
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-md-4 fw-bold">
                        العدد المعتمد
                    </div>

                    <div class="col-md-8">
                        <asp:Label
                            ID="lblApprovedHeadCount"
                            runat="server" />
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-md-4 fw-bold">
                        وظيفة إدارية
                    </div>

                    <div class="col-md-8">
                        <asp:Label
                            ID="lblIsManagerial"
                            runat="server" />
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-md-4 fw-bold">
                        الوصف بالعربية
                    </div>

                    <div class="col-md-8">
                        <asp:Label
                            ID="lblDescriptionAr"
                            runat="server" />
                    </div>
                </div>

                <div class="row mb-4">
                    <div class="col-md-4 fw-bold">
                        الوصف بالإنجليزية
                    </div>

                    <div class="col-md-8">
                        <asp:Label
                            ID="lblDescriptionEn"
                            runat="server" />
                    </div>
                </div>

                <asp:HyperLink
                    ID="lnkEdit"
                    runat="server"
                    Text="تعديل"
                    CssClass="btn btn-warning" />

                <a href="Index.aspx"
                   class="btn btn-secondary">
                    رجوع
                </a>

            </div>

        </asp:Panel>

    </div>

</asp:Content>