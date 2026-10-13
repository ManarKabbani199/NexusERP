<%@ Page Title="إدارة المستودعات"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Index.aspx.cs"
    Inherits="NexusERP.Web.Warehouses.Index" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4" dir="rtl">

        <div class="d-flex justify-content-between align-items-center mb-3">
            <h2 class="mb-0">إدارة المستودعات</h2>

            <a href="Create.aspx" class="btn btn-primary">
                إضافة مستودع جديد
            </a>
        </div>

        <div class="card shadow-sm">
            <div class="card-body">

                <div class="table-responsive">
                    <asp:GridView
                        ID="gvWarehouses"
                        runat="server"
                        AutoGenerateColumns="False"
                        CssClass="table table-bordered table-striped table-hover align-middle"
                        EmptyDataText="لا توجد مستودعات مسجلة حاليًا."
                        GridLines="None">

                        <Columns>

                            <asp:BoundField
                                DataField="Code"
                                HeaderText="الكود" />

                            <asp:BoundField
                                DataField="NameAr"
                                HeaderText="الاسم بالعربية" />

                            <asp:BoundField
                                DataField="NameEn"
                                HeaderText="الاسم بالإنجليزية" />

                            <asp:TemplateField HeaderText="الفرع">
                                <ItemTemplate>
                                    <%# Eval("Branch.NameAr") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField
                                DataField="Phone"
                                HeaderText="الهاتف" />

                            <asp:TemplateField HeaderText="الحالة">
                                <ItemTemplate>
                                    <asp:CheckBox
                                        ID="chkIsActive"
                                        runat="server"
                                        Checked='<%# Eval("IsActive") %>'
                                        Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="الإجراءات">
                                <ItemTemplate>

                                    <a class="btn btn-sm btn-info"
                                       href='<%# Eval("Id", "Details.aspx?id={0}") %>'>
                                        التفاصيل
                                    </a>

                                    <a class="btn btn-sm btn-warning"
                                       href='<%# Eval("Id", "Edit.aspx?id={0}") %>'>
                                        تعديل
                                    </a>

                                    <a class="btn btn-sm btn-danger"
                                       href='<%# Eval("Id", "Delete.aspx?id={0}") %>'>
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