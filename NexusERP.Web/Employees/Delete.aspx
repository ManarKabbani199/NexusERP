<%@ Page Title="حذف الموظف"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Delete.aspx.cs"
    Inherits="NexusERP.Web.Employees.Delete" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <asp:Panel
            ID="pnlError"
            runat="server"
            Visible="false"
            CssClass="alert alert-danger">

            <asp:Literal
                ID="litError"
                runat="server" />

        </asp:Panel>

        <asp:Panel
            ID="pnlDelete"
            runat="server"
            Visible="false">

            <div class="card shadow-sm border-danger">

                <div class="card-header bg-danger text-white">
                    <h3 class="mb-0">حذف الموظف</h3>
                </div>

                <div class="card-body">

                    <div class="alert alert-warning">

                        <strong>تحذير:</strong>

                        هل أنت متأكد من حذف هذا الموظف؟
                        لا يمكن التراجع عن عملية الحذف بعد تنفيذها.

                    </div>

                    <div class="row">

                        <div class="col-md-4 mb-3">

                            <strong>الرقم الوظيفي:</strong>

                            <div>
                                <asp:Literal
                                    ID="litEmployeeNumber"
                                    runat="server" />
                            </div>

                        </div>

                        <div class="col-md-4 mb-3">

                            <strong>اسم الموظف:</strong>

                            <div>
                                <asp:Literal
                                    ID="litEmployeeName"
                                    runat="server" />
                            </div>

                        </div>

                        <div class="col-md-4 mb-3">

                            <strong>الحالة الوظيفية:</strong>

                            <div>
                                <asp:Literal
                                    ID="litEmploymentStatus"
                                    runat="server" />
                            </div>

                        </div>

                        <div class="col-md-4 mb-3">

                            <strong>الشركة:</strong>

                            <div>
                                <asp:Literal
                                    ID="litCompany"
                                    runat="server" />
                            </div>

                        </div>

                        <div class="col-md-4 mb-3">

                            <strong>الفرع:</strong>

                            <div>
                                <asp:Literal
                                    ID="litBranch"
                                    runat="server" />
                            </div>

                        </div>

                        <div class="col-md-4 mb-3">

                            <strong>القسم:</strong>

                            <div>
                                <asp:Literal
                                    ID="litDepartment"
                                    runat="server" />
                            </div>

                        </div>

                        <div class="col-md-4 mb-3">

                            <strong>الوظيفة:</strong>

                            <div>
                                <asp:Literal
                                    ID="litPosition"
                                    runat="server" />
                            </div>

                        </div>

                        <div class="col-md-4 mb-3">

                            <strong>المدير المباشر:</strong>

                            <div>
                                <asp:Literal
                                    ID="litManager"
                                    runat="server" />
                            </div>

                        </div>

                    </div>

                    <hr />

                    <div class="d-flex gap-2">

                        <asp:Button
                            ID="btnDelete"
                            runat="server"
                            Text="تأكيد الحذف"
                            CssClass="btn btn-danger"
                            OnClick="btnDelete_Click"
                            OnClientClick="return confirm('هل أنت متأكد من حذف الموظف؟');" />

                        <a href="Index.aspx"
                           class="btn btn-secondary">
                            إلغاء
                        </a>

                    </div>

                </div>

            </div>

        </asp:Panel>

    </div>

</asp:Content>