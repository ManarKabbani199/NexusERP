<%@ Page Title="تفاصيل القسم"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Details.aspx.cs"
    Inherits="NexusERP.Web.Departments.Details" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <div class="card shadow-sm">

            <div class="card-header">
                <h3 class="mb-0">تفاصيل القسم</h3>
            </div>

            <div class="card-body">

                <dl class="row">

                    <dt class="col-sm-3">الفرع</dt>
                    <dd class="col-sm-9">
                        <asp:Label
                            ID="lblBranch"
                            runat="server" />
                    </dd>

                    <dt class="col-sm-3">القسم الأب</dt>
                    <dd class="col-sm-9">
                        <asp:Label
                            ID="lblParentDepartment"
                            runat="server" />
                    </dd>

                    <dt class="col-sm-3">كود القسم</dt>
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

                    <dt class="col-sm-3">الوصف بالعربية</dt>
                    <dd class="col-sm-9">
                        <asp:Label
                            ID="lblDescriptionAr"
                            runat="server" />
                    </dd>

                    <dt class="col-sm-3">الوصف بالإنجليزية</dt>
                    <dd class="col-sm-9">
                        <asp:Label
                            ID="lblDescriptionEn"
                            runat="server" />
                    </dd>

                    <dt class="col-sm-3">
                        عدد الأقسام الفرعية
                    </dt>
                    <dd class="col-sm-9">
                        <asp:Label
                            ID="lblChildDepartmentsCount"
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
                    العودة إلى الأقسام
                </a>

            </div>

        </div>

    </div>

</asp:Content>