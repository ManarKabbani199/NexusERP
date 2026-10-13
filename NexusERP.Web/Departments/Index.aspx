<%@ Page Title="إدارة الأقسام"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Index.aspx.cs"
    Inherits="NexusERP.Web.Departments.Index" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <div class="d-flex justify-content-between
                    align-items-center mb-4">

            <h2>إدارة الأقسام</h2>

            <a href="Create.aspx"
               class="btn btn-primary">
                إضافة قسم
            </a>

        </div>

        <asp:GridView ID="gvDepartments"
            runat="server"
            AutoGenerateColumns="False"
            CssClass="table table-bordered table-striped"
            EmptyDataText="لا توجد أقسام مسجلة">

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

                <asp:TemplateField HeaderText="القسم الأب">
                    <ItemTemplate>
                        <%#
                            Eval("ParentDepartment") == null
                                ? "قسم رئيسي"
                                : Eval("ParentDepartment.NameAr")
                        %>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:CheckBoxField
                    DataField="IsActive"
                    HeaderText="نشط" />

                <asp:TemplateField HeaderText="الإجراءات">
                    <ItemTemplate>

                        <asp:HyperLink
                            ID="lnkDetails"
                            runat="server"
                            Text="التفاصيل"
                            CssClass="btn btn-info btn-sm text-white"
                            NavigateUrl='<%#
                                "Details.aspx?id=" + Eval("Id")
                            %>' />

                        <asp:HyperLink
                            ID="lnkEdit"
                            runat="server"
                            Text="تعديل"
                            CssClass="btn btn-warning btn-sm"
                            NavigateUrl='<%#
                                "Edit.aspx?id=" + Eval("Id")
                            %>' />

                        <asp:LinkButton
                            ID="btnDelete"
                            runat="server"
                            Text="حذف"
                            CssClass="btn btn-danger btn-sm"
                            CommandArgument='<%# Eval("Id") %>'
                            CausesValidation="false"
                            OnClick="btnDelete_Click"
                            OnClientClick="return confirm('هل أنت متأكد من حذف هذا القسم؟');">
                        </asp:LinkButton>

                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>

        </asp:GridView>

    </div>

</asp:Content>