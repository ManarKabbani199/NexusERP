<%@ Page Title="تفاصيل الموظف"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Details.aspx.cs"
    Inherits="NexusERP.Web.Employees.Details" %>

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
            ID="pnlDetails"
            runat="server"
            Visible="false">s

            <div class="card shadow-sm">

                <div class="card-header
                            d-flex justify-content-between
                            align-items-center">

                    <h3 class="mb-0">تفاصيل الموظف</h3>

                    <asp:Label
                        ID="lblEmploymentStatus"
                        runat="server"
                        CssClass="badge bg-primary" />

                </div>

                <div class="card-body">

                    <h5 class="mb-3">البيانات الشخصية</h5>

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
                            <strong>الاسم بالعربية:</strong>
                            <div>
                                <asp:Literal
                                    ID="litFullNameAr"
                                    runat="server" />
                            </div>
                        </div>

                        <div class="col-md-4 mb-3">
                            <strong>الاسم بالإنجليزية:</strong>
                            <div>
                                <asp:Literal
                                    ID="litFullNameEn"
                                    runat="server" />
                            </div>
                        </div>

                        <div class="col-md-4 mb-3">
                            <strong>الرقم المدني:</strong>
                            <div>
                                <asp:Literal
                                    ID="litCivilId"
                                    runat="server" />
                            </div>
                        </div>

                        <div class="col-md-4 mb-3">
                            <strong>تاريخ الميلاد:</strong>
                            <div>
                                <asp:Literal
                                    ID="litDateOfBirth"
                                    runat="server" />
                            </div>
                        </div>

                        <div class="col-md-4 mb-3">
                            <strong>الجنس:</strong>
                            <div>
                                <asp:Literal
                                    ID="litGender"
                                    runat="server" />
                            </div>
                        </div>

                        <div class="col-md-4 mb-3">
                            <strong>الجنسية:</strong>
                            <div>
                                <asp:Literal
                                    ID="litNationality"
                                    runat="server" />
                            </div>
                        </div>

                        <div class="col-md-4 mb-3">
                            <strong>الهاتف:</strong>
                            <div>
                                <asp:Literal
                                    ID="litPhone"
                                    runat="server" />
                            </div>
                        </div>

                        <div class="col-md-4 mb-3">
                            <strong>البريد الإلكتروني:</strong>
                            <div>
                                <asp:Literal
                                    ID="litEmail"
                                    runat="server" />
                            </div>
                        </div>

                    </div>

                    <hr />

                    <h5 class="mb-3">الهيكل التنظيمي</h5>

                    <div class="row">

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

                    <h5 class="mb-3">بيانات التوظيف</h5>

                    <div class="row">

                        <div class="col-md-4 mb-3">
                            <strong>تاريخ التعيين:</strong>
                            <div>
                                <asp:Literal
                                    ID="litHireDate"
                                    runat="server" />
                            </div>
                        </div>

                        <div class="col-md-4 mb-3">
                            <strong>تاريخ إنهاء الخدمة:</strong>
                            <div>
                                <asp:Literal
                                    ID="litTerminationDate"
                                    runat="server" />
                            </div>
                        </div>

                        <div class="col-md-4 mb-3">
                            <strong>نوع التوظيف:</strong>
                            <div>
                                <asp:Literal
                                    ID="litEmploymentType"
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
                            <strong>الراتب الأساسي:</strong>
                            <div>
                                <asp:Literal
                                    ID="litBasicSalary"
                                    runat="server" />
                                د.ك
                            </div>
                        </div>

                    </div>

                    <hr />

                    <div class="d-flex gap-2">

                        <asp:HyperLink
                            ID="lnkEdit"
                            runat="server"
                            CssClass="btn btn-warning"
                            Text="تعديل الموظف" />

                        <a href="Index.aspx"
                           class="btn btn-secondary">
                            العودة إلى القائمة
                        </a>

                    </div>

                </div>

            </div>

        </asp:Panel>

    </div>


</asp:Content>