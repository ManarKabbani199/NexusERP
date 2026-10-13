<%@ Page Title="مرتجعات المشتريات"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Index.aspx.cs"
    Inherits="NexusERP.Web.PurchaseReturns.Index" %>

<asp:Content
    ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <div class="d-flex justify-content-between
                    align-items-center mb-3">

            <h2 class="mb-0">
                مرتجعات المشتريات
            </h2>

            <a href="Create.aspx"
               class="btn btn-primary">
                إنشاء مرتجع مشتريات
            </a>

        </div>

        <asp:Label
            ID="lblMessage"
            runat="server"
            Visible="false" />

        <div class="card shadow-sm">

            <div class="card-header">
                <h5 class="mb-0">
                    سجل مرتجعات المشتريات
                </h5>
            </div>

            <div class="card-body">

                <div class="table-responsive">

                    <asp:GridView
                        ID="gvPurchaseReturns"
                        runat="server"
                        AutoGenerateColumns="False"
                        CssClass="table table-striped table-bordered"
                        EmptyDataText="لا توجد مرتجعات مشتريات حتى الآن.">

                        <Columns>

                            <asp:BoundField
                                DataField="ReturnNumber"
                                HeaderText="رقم المرتجع" />

                            <asp:BoundField
                                DataField="ReturnDate"
                                HeaderText="تاريخ المرتجع"
                                DataFormatString="{0:yyyy-MM-dd}" />

                            <asp:BoundField
                                DataField="ReceiptNumber"
                                HeaderText="مستند الاستلام" />

                            <asp:BoundField
                                DataField="PurchaseOrderNumber"
                                HeaderText="أمر الشراء" />

                            <asp:BoundField
                                DataField="SupplierName"
                                HeaderText="المورد" />

                            <asp:BoundField
                                DataField="WarehouseName"
                                HeaderText="المستودع" />

                            <asp:BoundField
                                DataField="ItemsCount"
                                HeaderText="عدد الأصناف" />

                            <asp:BoundField
                                DataField="TotalQuantity"
                                HeaderText="إجمالي الكمية"
                                DataFormatString="{0:N2}" />

                            <asp:BoundField
                                DataField="StatusName"
                                HeaderText="الحالة" />

                            <asp:TemplateField
                                HeaderText="الإجراءات">

                                <ItemTemplate>

                                    <a href='<%#
                                        "Details.aspx?id=" +
                                        Eval("Id") %>'
                                       class="btn btn-info btn-sm">

                                        التفاصيل

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