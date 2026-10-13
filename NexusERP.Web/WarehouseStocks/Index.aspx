<%@ Page Title="أرصدة المستودعات"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Index.aspx.cs"
    Inherits="NexusERP.Web.WarehouseStocks.Index" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <div class="d-flex justify-content-between align-items-center mb-3">
            <h2 class="mb-0">أرصدة المستودعات</h2>

            <a href="Create.aspx" class="btn btn-primary">
                إضافة رصيد صنف
            </a>
        </div>

        <asp:Panel
            ID="pnlMessage"
            runat="server"
            Visible="false"
            CssClass="alert">

            <asp:Label
                ID="lblMessage"
                runat="server" />

        </asp:Panel>

        <div style="margin-bottom: 15px;">
    <asp:HyperLink
        ID="lnkStockReport"
        runat="server"
        NavigateUrl="~/WarehouseStocks/StockReport.aspx"
        Text="عرض تقرير أرصدة المستودعات" />
</div>

        <asp:HyperLink
    ID="lnkLowStockReport"
    runat="server"
    NavigateUrl="~/WarehouseStocks/LowStockReport.aspx"
    Text="تقرير الأصناف منخفضة المخزون"
    style="margin-right: 15px;" />


        <div class="card shadow-sm">
            <div class="card-body">

                <div class="table-responsive">

                    <asp:GridView
                        ID="gvWarehouseStocks"
                        runat="server"
                        AutoGenerateColumns="false"
                        DataKeyNames="Id"
                        CssClass="table table-bordered table-hover align-middle"
                        EmptyDataText="لا توجد أرصدة مسجلة حاليًا.">

                        <Columns>

                            <asp:BoundField
                                DataField="Warehouse.NameAr"
                                HeaderText="المستودع" />

                            <asp:BoundField
                                DataField="Item.Code"
                                HeaderText="رمز الصنف" />

                            <asp:BoundField
                                DataField="Item.NameAr"
                                HeaderText="الصنف" />

                            <asp:BoundField
                                DataField="Item.ItemCategory.NameAr"
                                HeaderText="التصنيف" />

                            <asp:BoundField
                                DataField="Quantity"
                                HeaderText="الكمية الحالية"
                                DataFormatString="{0:N2}" />

                            <asp:BoundField
                                DataField="Item.UnitOfMeasure"
                                HeaderText="وحدة القياس" />

                            <asp:BoundField
                                DataField="Item.MinimumStockLevel"
                                HeaderText="الحد الأدنى"
                                DataFormatString="{0:N2}" />

                            <asp:TemplateField HeaderText="حالة المخزون">
                                <ItemTemplate>

                                    <span class='<%#
                                        Convert.ToDecimal(Eval("Quantity"))
                                            <= Convert.ToDecimal(
                                                Eval("Item.MinimumStockLevel"))
                                            ? "badge bg-danger"
                                            : "badge bg-success" %>'>

                                        <%#
                                            Convert.ToDecimal(Eval("Quantity"))
                                                <= Convert.ToDecimal(
                                                    Eval("Item.MinimumStockLevel"))
                                                ? "مخزون منخفض"
                                                : "متوفر" %>

                                    </span>

                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="الإجراءات">
                                <ItemTemplate>

                                    <a class="btn btn-sm btn-info"
                                       href='<%# "Details.aspx?id=" + Eval("Id") %>'>
                                        التفاصيل
                                    </a>

                                    <a class="btn btn-sm btn-warning"
                                       href='<%# "Edit.aspx?id=" + Eval("Id") %>'>
                                        تعديل
                                    </a>

                                    <a class="btn btn-sm btn-danger"
                                       href='<%# "Delete.aspx?id=" + Eval("Id") %>'>
                                        حذف
                                    </a>

                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>

                    </asp:GridView>

                </div>

            </div>
        </div>

    </div>

</asp:Content>