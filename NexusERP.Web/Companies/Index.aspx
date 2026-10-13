<%@ Page Title="إدارة الشركات"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Index.aspx.cs"
    Inherits="NexusERP.Web.Companies.Index" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <div class="d-flex justify-content-between
                    align-items-center mb-4">

            <h2>إدارة الشركات</h2>

            <a href="Create.aspx"
               class="btn btn-primary">
                إضافة شركة
            </a>

        </div>

        <asp:GridView ID="gvCompanies"
            runat="server"
            AutoGenerateColumns="False"
            CssClass="table table-bordered table-striped"
            EmptyDataText="لا توجد شركات مسجلة">

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
                    DataField="Phone"
                    HeaderText="الهاتف" />

                <asp:CheckBoxField
                    DataField="IsActive"
                    HeaderText="نشطة" />
                
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
                
        <asp:LinkButton ID="btnDelete"
            runat="server"
            Text="حذف"
            CssClass="btn btn-danger btn-sm"
            CommandArgument='<%# Eval("Id") %>'
            CausesValidation="false"
            OnClick="btnDelete_Click"
            OnClientClick="return confirm('هل أنت متأكد من حذف هذه الشركة؟');">
        </asp:LinkButton>


            </ItemTemplate>
        </asp:TemplateField>

            </Columns>

        </asp:GridView>

    </div>

</asp:Content>