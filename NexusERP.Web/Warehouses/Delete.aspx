<%@ Page Title="حذف المستودع"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Delete.aspx.cs"
    Inherits="NexusERP.Web.Warehouses.Delete" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4" dir="rtl">

        <div class="card border-danger shadow-sm">

            <div class="card-header bg-danger text-white">
                <h3 class="mb-0">حذف المستودع</h3>
            </div>

            <div class="card-body">

                <div class="alert alert-warning">
                    هل أنت متأكد من حذف هذا المستودع؟ لا يمكن التراجع عن هذه العملية.
                </div>

                <div class="row mb-3">
                    <div class="col-md-4 fw-bold">كود المستودع:</div>
                    <div class="col-md-8">
                        <asp:Label
                            ID="lblCode"
                            runat="server" />
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-md-4 fw-bold">اسم المستودع:</div>
                    <div class="col-md-8">
                        <asp:Label
                            ID="lblNameAr"
                            runat="server" />
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-md-4 fw-bold">الاسم بالإنجليزية:</div>
                    <div class="col-md-8">
                        <asp:Label
                            ID="lblNameEn"
                            runat="server" />
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-md-4 fw-bold">الفرع:</div>
                    <div class="col-md-8">
                        <asp:Label
                            ID="lblBranch"
                            runat="server" />
                    </div>
                </div>

                <hr />

                <asp:Button
                    ID="btnDelete"
                    runat="server"
                    Text="تأكيد الحذف"
                    CssClass="btn btn-danger"
                    OnClick="btnDelete_Click"
                    OnClientClick="return confirm('هل أنت متأكد من حذف المستودع؟');" />

                <asp:HyperLink
                    ID="linkCancel"
                    runat="server"
                    CssClass="btn btn-secondary">
                    إلغاء
                </asp:HyperLink>

            </div>
        </div>

    </div>

</asp:Content>