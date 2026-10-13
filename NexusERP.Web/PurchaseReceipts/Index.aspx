<%@ Page Title="استلامات المشتريات"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Index.aspx.cs"
    Inherits="NexusERP.Web.PurchaseReceipts.Index" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <div class="d-flex justify-content-between
                    align-items-center mb-3">

            <h2 class="mb-0">
                استلامات المشتريات
            </h2>

            <a href="Create.aspx"
               class="btn btn-primary">
                إنشاء استلام مشتريات
            </a>

        </div>

        <asp:Label
            ID="lblMessage"
            runat="server"
            Visible="false" />

        <div class="card shadow-sm">
            <div class="card-body">

                <asp:GridView
                    ID="gvPurchaseReceipts"
                    runat="server"
                    AutoGenerateColumns="False"
                    CssClass="table table-striped table-bordered"
                    EmptyDataText="لا توجد مستندات استلام حتى الآن."
                    OnRowCommand=
                        "gvPurchaseReceipts_RowCommand">

                    <Columns>

                        <asp:BoundField
                            DataField="ReceiptNumber"
                            HeaderText="رقم الاستلام" />

                        <asp:BoundField
                            DataField="ReceiptDate"
                            HeaderText="تاريخ الاستلام"
                            DataFormatString="{0:yyyy-MM-dd}" />

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
                            DataField="StatusName"
                            HeaderText="الحالة" />

                        <asp:TemplateField HeaderText="الإجراءات">
                            <ItemTemplate>

                                <a href='<%# "Details.aspx?id=" +
                                    Eval("Id") %>'
                                   class="btn btn-sm btn-info">
                                    التفاصيل
                                </a>

                                <asp:LinkButton
                                    ID="btnPost"
                                    runat="server"
                                    Text="ترحيل"
                                    CssClass="btn btn-sm btn-success"
                                    CausesValidation="false"
                                    CommandName="PostReceipt"
                                    CommandArgument='<%# Eval("Id") %>'
                                    Visible='<%#
                                        !(bool)Eval("IsPosted")
                                        &&
                                        !(bool)Eval("IsCancelled")
                                    %>'
                                    OnClientClick="return confirm(
                                        'هل تريد ترحيل الاستلام؟ ' +
                                        'سيتم زيادة رصيد المستودع، ' +
                                        'ولا يمكن ترحيله مرة أخرى.');" />

                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>

            </div>
        </div>

    </div>

</asp:Content>