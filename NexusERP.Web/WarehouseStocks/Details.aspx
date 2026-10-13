<%@ Page Title="تفاصيل رصيد المستودع"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Details.aspx.cs"
    Inherits="NexusERP.Web.WarehouseStocks.Details" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <div class="card shadow-sm">

            <div class="card-header">
                <h3 class="mb-0">تفاصيل رصيد المستودع</h3>
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

                        <dt class="col-sm-3">المستودع</dt>
                        <dd class="col-sm-9">
                            <asp:Label
                                ID="lblWarehouse"
                                runat="server" />
                        </dd>

                        <dt class="col-sm-3">الفرع</dt>
                        <dd class="col-sm-9">
                            <asp:Label
                                ID="lblBranch"
                                runat="server" />
                        </dd>

                        <dt class="col-sm-3">رمز الصنف</dt>
                        <dd class="col-sm-9">
                            <asp:Label
                                ID="lblItemCode"
                                runat="server" />
                        </dd>

                        <dt class="col-sm-3">اسم الصنف</dt>
                        <dd class="col-sm-9">
                            <asp:Label
                                ID="lblItemName"
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

                        <dt class="col-sm-3">الكمية الحالية</dt>
                        <dd class="col-sm-9">
                            <asp:Label
                                ID="lblQuantity"
                                runat="server" />
                        </dd>

                        <dt class="col-sm-3">الحد الأدنى للمخزون</dt>
                        <dd class="col-sm-9">
                            <asp:Label
                                ID="lblMinimumStockLevel"
                                runat="server" />
                        </dd>

                        <dt class="col-sm-3">حالة المخزون</dt>
                        <dd class="col-sm-9">
                            <asp:Label
                                ID="lblStockStatus"
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
                            CssClass="btn btn-danger">
                            حذف
                        </asp:HyperLink>

                        <asp:HyperLink
                            ID="lnkBack"
                            runat="server"
                            NavigateUrl="~/WarehouseStocks/Index.aspx"
                            CssClass="btn btn-secondary">
                            العودة إلى القائمة
                        </asp:HyperLink>

                    </div>

                </asp:Panel>

            </div>
        </div>
    </div>

</asp:Content>