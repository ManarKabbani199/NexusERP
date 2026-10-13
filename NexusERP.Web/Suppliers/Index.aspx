<%@ Page Title="الموردون"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Index.aspx.cs"
    Inherits="NexusERP.Web.Suppliers.Index" %>

<%@ Register
    Assembly="DevExpress.Web.v26.1, Version=26.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web"
    TagPrefix="dx" %>

<asp:Content
    ID="Content1"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <div class="d-flex justify-content-between align-items-center mb-4">

            <h2 class="mb-0">الموردون</h2>

            <a href="Create.aspx" class="btn btn-primary">
                + إضافة مورد
            </a>

        </div>

        <div class="mb-3">
    <asp:Button
        ID="btnReset"
        runat="server"
        Text="عرض الكل"
        CssClass="btn btn-secondary"
        CausesValidation="false"
        OnClick="btnReset_Click" />
</div>

        <dx:ASPxGridView
            ID="gvSuppliers"
            runat="server"
            AutoGenerateColumns="false"
            KeyFieldName="Id"
            Width="100%"
            RightToLeft="True"
            Theme="Office365"
            Font-Names="Tahoma"
            Font-Size="10pt">

            <Settings
                ShowFilterRow="true"
                ShowGroupPanel="true" />

            <SettingsBehavior
                AllowSort="true"
                AllowGroup="true" />

            <SettingsPager PageSize="10" />

            <SettingsSearchPanel
                Visible="true"
                ShowApplyButton="true"
                ShowClearButton="true"
                AllowTextInputTimer="false"
                ColumnNames="Code;NameAr;ContactPerson;Phone;Email;StatusName" />

            <SettingsText
                EmptyDataRow="لا يوجد موردون مطابقون."
                GroupPanel="اسحبي عنوان عمود هنا لتجميع الموردين حسبه."
                SearchPanelEditorNullText="ابحثي في الموردين..."
                CommandApplySearchPanelFilter="بحث"
                CommandClearSearchPanelFilter="مسح" />

            <Columns>

                <dx:GridViewDataTextColumn
                    FieldName="Code"
                    Caption="الرمز"
                    VisibleIndex="0" />

                <dx:GridViewDataTextColumn
                    FieldName="NameAr"
                    Caption="اسم المورد"
                    VisibleIndex="1" />

                <dx:GridViewDataTextColumn
                    FieldName="ContactPerson"
                    Caption="جهة الاتصال"
                    VisibleIndex="2" />

                <dx:GridViewDataTextColumn
                    FieldName="Phone"
                    Caption="الهاتف"
                    VisibleIndex="3" />

                <dx:GridViewDataTextColumn
                    FieldName="Email"
                    Caption="البريد الإلكتروني"
                    VisibleIndex="4" />

                <dx:GridViewDataSpinEditColumn
                    FieldName="PaymentTermsDays"
                    Caption="مدة السداد بالأيام"
                    VisibleIndex="5">

                    <PropertiesSpinEdit
                        DisplayFormatString="N0" />

                </dx:GridViewDataSpinEditColumn>

                <dx:GridViewDataTextColumn
                    FieldName="StatusName"
                    Caption="الحالة"
                    VisibleIndex="6">

                    <DataItemTemplate>

                        <asp:Label
                            ID="lblStatus"
                            runat="server"
                            Text='<%# Eval("StatusName") %>'
                            CssClass='<%# Convert.ToBoolean(Eval("IsActive"))
                                ? "badge bg-success"
                                : "badge bg-secondary" %>' />

                    </DataItemTemplate>

                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn
                    Caption="الإجراءات"
                    VisibleIndex="7">

                    <Settings
                        AllowSort="False"
                        AllowGroup="False"
                        AllowAutoFilter="False" />

                    <DataItemTemplate>

                        <asp:HyperLink
                            ID="lnkDetails"
                            runat="server"
                            Text="التفاصيل"
                            CssClass="btn btn-sm btn-info"
                            NavigateUrl='<%# "Details.aspx?id=" + Eval("Id") %>' />

                        <asp:HyperLink
                            ID="lnkEdit"
                            runat="server"
                            Text="تعديل"
                            CssClass="btn btn-sm btn-primary"
                            NavigateUrl='<%# "Edit.aspx?id=" + Eval("Id") %>' />

                        <asp:HyperLink
                            ID="lnkDeactivate"
                            runat="server"
                            Text='<%# Convert.ToBoolean(Eval("IsActive"))
                                ? "تعطيل"
                                : "إعادة تفعيل" %>'
                            CssClass='<%# Convert.ToBoolean(Eval("IsActive"))
                                ? "btn btn-sm btn-danger"
                                : "btn btn-sm btn-success" %>'
                            NavigateUrl='<%#
                                (Convert.ToBoolean(Eval("IsActive"))
                                    ? "Deactivate.aspx?id="
                                    : "Activate.aspx?id=") + Eval("Id") %>' />

                    </DataItemTemplate>

                </dx:GridViewDataTextColumn>

            </Columns>

        </dx:ASPxGridView>

    </div>

</asp:Content>