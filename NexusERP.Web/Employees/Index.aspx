<%@ Page Title="إدارة الموظفين"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Index.aspx.cs"
    Inherits="NexusERP.Web.Employees.Index" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <div class="d-flex justify-content-between
                    align-items-center mb-3">

            <h2 class="mb-0">إدارة الموظفين</h2>

            <a href="Create.aspx"
               class="btn btn-primary">
                إضافة موظف جديد
            </a>

        </div>

        <asp:Panel
            ID="pnlMessage"
            runat="server"
            Visible="false"
            CssClass="alert">

            <asp:Literal
                ID="litMessage"
                runat="server" />

        </asp:Panel>

        <div class="card shadow-sm">

            <div class="card-header">
                <strong>قائمة الموظفين</strong>
            </div>

            <div class="card-body">

                <div class="table-responsive">

                    <asp:GridView
                        ID="gvEmployees"
                        runat="server"
                        AutoGenerateColumns="false"
                        CssClass="table table-striped
                                  table-bordered table-hover"
                        EmptyDataText="لا يوجد موظفون حاليًا"
                        GridLines="None">

                        <Columns>

                            <asp:BoundField
                                DataField="EmployeeNumber"
                                HeaderText="الرقم الوظيفي" />

                            <asp:TemplateField HeaderText="اسم الموظف">
                                <ItemTemplate>
                                    <%# Eval("FirstNameAr") %>
                                    <%# Eval("LastNameAr") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="الشركة">
                                <ItemTemplate>
                                    <%# Eval("Branch.Company.NameAr") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="الفرع">
                                <ItemTemplate>
                                    <%# Eval("Branch.NameAr") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="القسم">
                                <ItemTemplate>
                                    <%# Eval("Department.NameAr") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="الوظيفة">
                                <ItemTemplate>
                                    <%# Eval("Position.TitleAr") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="المدير المباشر">
                                <ItemTemplate>
                                    <%# GetManagerName(Eval("Manager")) %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField
                                DataField="EmploymentStatus"
                                HeaderText="الحالة الوظيفية" />

                            <asp:TemplateField HeaderText="الإجراءات">
                                <ItemTemplate>

                                    <a class="btn btn-sm btn-info"
                                       href='<%# Eval(
                                           "Id",
                                           "Details.aspx?id={0}") %>'>
                                        التفاصيل
                                    </a>

                                    <a class="btn btn-sm btn-warning"
                                       href='<%# Eval(
                                           "Id",
                                           "Edit.aspx?id={0}") %>'>
                                        تعديل
                                    </a>

                                    <a class="btn btn-sm btn-danger"
                                       href='<%# Eval(
                                           "Id",
                                           "Delete.aspx?id={0}") %>'>
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