<%@ Page Title="تفاصيل الصنف"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Details.aspx.cs"
    Inherits="NexusERP.Web.Items.Details" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <div class="card shadow-sm">

            <div class="card-header">
                <h3 class="mb-0">تفاصيل الصنف</h3>
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

                <asp:Panel
                    ID="pnlDetails"
                    runat="server">

                    <dl class="row">

                        <dt class="col-sm-3">رمز الصنف</dt>
                        <dd class="col-sm-9">
                            <asp:Label
                                ID="lblCode"
                                runat="server" />
                        </dd>

                        <dt class="col-sm-3">الاسم العربي</dt>
                        <dd class="col-sm-9">
                            <asp:Label
                                ID="lblNameAr"
                                runat="server" />
                        </dd>

                        <dt class="col-sm-3">الاسم الإنجليزي</dt>
                        <dd class="col-sm-9">
                            <asp:Label
                                ID="lblNameEn"
                                runat="server" />
                        </dd>

                        <dt class="col-sm-3">تصنيف الصنف</dt>
                        <dd class="col-sm-9">
                            <asp:Label
                                ID="lblItemCategory"
                                runat="server" />
                        </dd>

                        <dt class="col-sm-3">وحدة القياس</dt>
                        <dd class="col-sm-9">
                            <asp:Label
                                ID="lblUnitOfMeasure"
                                runat="server" />
                        </dd>

                        <dt class="col-sm-3">الحد الأدنى للمخزون</dt>
                        <dd class="col-sm-9">
                            <asp:Label
                                ID="lblMinimumStockLevel"
                                runat="server" />
                        </dd>

                        <dt class="col-sm-3">الحالة</dt>
                        <dd class="col-sm-9">
                            <asp:Label
                                ID="lblIsActive"
                                runat="server" />
                        </dd>

                        <dt class="col-sm-3">الوصف العربي</dt>
                        <dd class="col-sm-9">
                            <asp:Label
                                ID="lblDescriptionAr"
                                runat="server" />
                        </dd>

                        <dt class="col-sm-3">الوصف الإنجليزي</dt>
                        <dd class="col-sm-9">
                            <asp:Label
                                ID="lblDescriptionEn"
                                runat="server" />
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
                            ID="lnkDelete"
                            runat="server"
                            OnClick="btnDelete_Click"
                            CssClass="btn btn-danger">
                            حذف
                        </asp:HyperLink>

                        <asp:HyperLink
                            ID="lnkBack"
                            runat="server"
                            NavigateUrl="~/Items/Index.aspx"
                            CssClass="btn btn-secondary">
                            العودة إلى القائمة
                        </asp:HyperLink>

                    </div>

                </asp:Panel>

            </div>
        </div>
    </div>

</asp:Content>