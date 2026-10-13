<%@ Page Title="تصنيفات الأصناف"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Index.aspx.cs"
    Inherits="NexusERP.Web.ItemCategories.Index" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <div class="d-flex justify-content-between align-items-center mb-3">
            <h2 class="mb-0">تصنيفات الأصناف</h2>

            <a href="Create.aspx" class="btn btn-primary">
                إضافة تصنيف جديد جديد
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

        <div class="card shadow-sm">
            <div class="card-body">

                <div class="table-responsive">

                    <asp:GridView
                        ID="gvItemCategories"
                        runat="server"
                        AutoGenerateColumns="false"
                        DataKeyNames="Id"
                        CssClass="table table-bordered table-hover align-middle"
                        EmptyDataText="لا توجد تصنيفات أصناف مسجلة حاليًا.">

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

                            <asp:BoundField
                                DataField="DescriptionAr"
                                HeaderText="الوصف بالعربية" />

                            <asp:BoundField
                                DataField="DescriptionEn"
                                HeaderText="الوصف بالإنجليزية" />

                            <asp:TemplateField HeaderText="الإجراءات">
                                <ItemTemplate>
                                     <a class="btn btn-sm btn-info"
    href='<%# Eval("Id", "Details.aspx?id={0}") %>'>
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