<%@ Page Title="حذف الصنف"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Delete.aspx.cs"
    Inherits="NexusERP.Web.Items.Delete" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <div class="card border-danger shadow-sm">

            <div class="card-header bg-danger text-white">
                <h3 class="mb-0">حذف الصنف</h3>
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

                    <div class="alert alert-warning">
                        هل أنت متأكد من حذف الصنف التالي؟
                        لا يمكن التراجع عن عملية الحذف.
                    </div>

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

                    </dl>

                    <div class="d-flex gap-2">

                        <asp:Button
                            ID="btnDelete"
                            runat="server"
                            Text="تأكيد الحذف"
                            CssClass="btn btn-danger"
                            OnClick="btnDelete_Click"
                            CausesValidation="false" />

                        <asp:HyperLink
                            ID="lnkCancel"
                            runat="server"
                            NavigateUrl="~/Items/Index.aspx"
                            CssClass="btn btn-secondary">
                            إلغاء
                        </asp:HyperLink>

                    </div>

                </asp:Panel>

            </div>
        </div>
    </div>

</asp:Content>