<%@ Page Title="إضافة وظيفة"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Create.aspx.cs"
    Inherits="NexusERP.Web.Positions.Create" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <div class="card shadow-sm">

            <div class="card-header">
                <h3 class="mb-0">إضافة وظيفة جديدة</h3>
            </div>

            <div class="card-body">

                <asp:ValidationSummary
                    ID="validationSummary"
                    runat="server"
                    ValidationGroup="PositionForm"
                    CssClass="alert alert-danger"
                    HeaderText="يرجى تصحيح الأخطاء التالية:" />

                <div class="mb-3">
                    <label class="form-label">القسم</label>

                    <asp:DropDownList
                        ID="ddlDepartment"
                        runat="server"
                        CssClass="form-select">
                    </asp:DropDownList>

                    <asp:RequiredFieldValidator
                        ID="rfvDepartment"
                        runat="server"
                        ControlToValidate="ddlDepartment"
                        InitialValue=""
                        ValidationGroup="PositionForm"
                        ErrorMessage="يرجى اختيار القسم."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label class="form-label">كود الوظيفة</label>

                    <asp:TextBox
                        ID="txtCode"
                        runat="server"
                        CssClass="form-control"
                        MaxLength="20">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="rfvCode"
                        runat="server"
                        ControlToValidate="txtCode"
                        ValidationGroup="PositionForm"
                        ErrorMessage="كود الوظيفة مطلوب."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label class="form-label">
                        المسمى الوظيفي بالعربية
                    </label>

                    <asp:TextBox
                        ID="txtTitleAr"
                        runat="server"
                        CssClass="form-control"
                        MaxLength="150">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="rfvTitleAr"
                        runat="server"
                        ControlToValidate="txtTitleAr"
                        ValidationGroup="PositionForm"
                        ErrorMessage="المسمى الوظيفي بالعربية مطلوب."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label class="form-label">
                        المسمى الوظيفي بالإنجليزية
                    </label>

                    <asp:TextBox
                        ID="txtTitleEn"
                        runat="server"
                        CssClass="form-control"
                        MaxLength="150">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="rfvTitleEn"
                        runat="server"
                        ControlToValidate="txtTitleEn"
                        ValidationGroup="PositionForm"
                        ErrorMessage="المسمى الوظيفي بالإنجليزية مطلوب."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label class="form-label">الوصف بالعربية</label>

                    <asp:TextBox
                        ID="txtDescriptionAr"
                        runat="server"
                        CssClass="form-control"
                        TextMode="MultiLine"
                        Rows="3"
                        MaxLength="1000">
                    </asp:TextBox>
                </div>

                <div class="mb-3">
                    <label class="form-label">الوصف بالإنجليزية</label>

                    <asp:TextBox
                        ID="txtDescriptionEn"
                        runat="server"
                        CssClass="form-control"
                        TextMode="MultiLine"
                        Rows="3"
                        MaxLength="1000">
                    </asp:TextBox>
                </div>

                <div class="mb-3">
                    <label class="form-label">المستوى الوظيفي</label>

                    <asp:TextBox
                        ID="txtJobLevel"
                        runat="server"
                        CssClass="form-control"
                        MaxLength="50">
                    </asp:TextBox>
                </div>

                <div class="mb-3">
                    <label class="form-label">العدد المعتمد</label>

                    <asp:TextBox
                        ID="txtApprovedHeadCount"
                        runat="server"
                        CssClass="form-control"
                        TextMode="Number"
                        Text="0">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="rfvApprovedHeadCount"
                        runat="server"
                        ControlToValidate="txtApprovedHeadCount"
                        ValidationGroup="PositionForm"
                        ErrorMessage="العدد المعتمد مطلوب."
                        CssClass="text-danger"
                        Display="Dynamic" />

                    <asp:RangeValidator
                        ID="rvApprovedHeadCount"
                        runat="server"
                        ControlToValidate="txtApprovedHeadCount"
                        Type="Integer"
                        MinimumValue="0"
                        MaximumValue="100000"
                        ValidationGroup="PositionForm"
                        ErrorMessage="العدد المعتمد يجب أن يكون صفرًا أو أكبر."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="form-check mb-3">
                    <asp:CheckBox
                        ID="chkIsManagerial"
                        runat="server"
                        CssClass="form-check-input" />

                    <label class="form-check-label"
                           for="<%= chkIsManagerial.ClientID %>">
                        وظيفة إدارية
                    </label>
                </div>

                <asp:Button
                    ID="btnSave"
                    runat="server"
                    Text="حفظ"
                    CssClass="btn btn-success"
                    ValidationGroup="PositionForm"
                    OnClick="btnSave_Click" />

                <a href="Index.aspx"
                   class="btn btn-secondary">
                    رجوع
                </a>

            </div>

        </div>

    </div>

</asp:Content>