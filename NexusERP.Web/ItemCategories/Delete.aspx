<%@ Page Title="حذف تصنيف الصنف"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Delete.aspx.cs"
    Inherits="NexusERP.Web.ItemCategories.Delete" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <div class="card border-danger shadow-sm">

            <div class="card-header bg-danger text-white">
                <h3 class="mb-0">حذف تصنيف الصنف</h3>
            </div>

            <div class="card-body">

                <asp:Panel
                    ID="pnlError"
                    runat="server"
                    Visible="false"
                    CssClass="alert alert-danger">

                    <asp:Label
                        ID="lblError"
                        runat="server" />

                </asp:Panel>

                <div class="alert alert-warning">
                    هل أنت متأكد من حذف تصنيف الصنف التالي؟
                    لا يمكن التراجع عن عملية الحذف.
                </div>

                <dl class="row">

                    <dt class="col-sm-3">رمز التصنيف</dt>
                    <dd class="col-sm-9">
                        <asp:Label ID="lblCode" runat="server" />
                    </dd>

                    <dt class="col-sm-3">الاسم العربي</dt>
                    <dd class="col-sm-9">
                        <asp:Label ID="lblNameAr" runat="server" />
                    </dd>

                    <dt class="col-sm-3">الاسم الإنجليزي</dt>
                    <dd class="col-sm-9">
                        <asp:Label ID="lblNameEn" runat="server" />
                    </dd>

                    <dt class="col-sm-3">الوصف العربي</dt>
                    <dd class="col-sm-9">
                        <asp:Label ID="lblDescriptionAr" runat="server" />
                    </dd>

                    <dt class="col-sm-3">الوصف الإنجليزي</dt>
                    <dd class="col-sm-9">
                        <asp:Label ID="lblDescriptionEn" runat="server" />
                    </dd>

                </dl>

                <div class="d-flex gap-2">

                    <asp:Button
                        ID="btnDelete"
                        runat="server"
                        Text="تأكيد الحذف"
                          OnClick="btnDelete_Click"
   OnClientClick =
      "return confirm('هل أنت متأكد من حذف الموظف؟');" />
                     

                    <asp:HyperLink
                        ID="lnkCancel"
                        runat="server"
                        NavigateUrl="~/ItemCategories/Index.aspx"
                        CssClass="btn btn-secondary">
                        إلغاء
                    </asp:HyperLink>

                </div>

            </div>
        </div>
    </div>

</asp:Content>