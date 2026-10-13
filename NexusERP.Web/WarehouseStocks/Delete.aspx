<%@ Page Title="حذف رصيد المستودع"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Delete.aspx.cs"
    Inherits="NexusERP.Web.WarehouseStocks.Delete" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <div class="card border-danger shadow-sm">

            <div class="card-header bg-danger text-white">
                <h3 class="mb-0">حذف رصيد المستودع</h3>
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
                        هل أنت متأكد من حذف رصيد الصنف التالي من المستودع؟
                    </div>

                    <dl class="row">

                        <dt class="col-sm-3">المستودع</dt>
                        <dd class="col-sm-9">
                            <asp:Label
                                ID="lblWarehouse"
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

                    </dl>

                    <div class="alert alert-danger">
                        حذف سجل الرصيد سيزيل ارتباط الصنف بالمستودع.
                        لا تحذفيه إذا كان مستخدمًا في حركات مخزون.
                    </div>

                    <div class="d-flex gap-2">

                        <asp:Button
                            ID="btnDelete"
                            runat="server"
                            Text="تأكيد الحذف"
                            CssClass="btn btn-danger"
                            OnClick="btnDelete_Click"
                            OnClientClick="return confirm('هل أنت متأكد من حذف الرصيد ؟');" 
                            CausesValidation="false" />

                        <asp:HyperLink
                            ID="lnkCancel"
                            runat="server"
                            NavigateUrl="~/WarehouseStocks/Index.aspx"
                            CssClass="btn btn-secondary">
                            إلغاء
                        </asp:HyperLink>

                    </div>

                </asp:Panel>

            </div>
        </div>
    </div>

</asp:Content>