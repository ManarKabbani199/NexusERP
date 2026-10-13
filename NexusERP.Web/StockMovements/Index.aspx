<%@ Page Title="حركات المخزون"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Index.aspx.cs"
    Inherits="NexusERP.Web.StockMovements.Index" %>

<%@ Register assembly="DevExpress.Web.v26.1, Version=26.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">
    <asp:UpdatePanel
    ID="upStockMovements"
    runat="server"
    UpdateMode="Conditional">

    <ContentTemplate>
        <asp:UpdateProgress
    ID="progressStockMovements"
    runat="server"
    AssociatedUpdatePanelID="upStockMovements"
    DisplayAfter="200">

    <ProgressTemplate>

        <div class="alert alert-info"
             role="status"
             aria-live="polite"
             dir="rtl">

            جارٍ تحميل البيانات، يرجى الانتظار…

        </div>

    </ProgressTemplate>

</asp:UpdateProgress>
    <div class="container py-4">

        <div class="d-flex justify-content-between
                    align-items-center mb-4">

            <h3 class="mb-0">حركات المخزون</h3>

            <a href="Create.aspx"
               class="btn btn-primary">
                إضافة حركة جديدة
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

        <div dir="rtl" style="margin-bottom: 15px;">
    <asp:HyperLink
        ID="lnkMovementReport"
        runat="server"
        NavigateUrl="~/StockMovements/MovementReport.aspx"
        Text="عرض تقرير حركات المخزون" />
</div>

        <div class="card shadow-sm">
            <div class="card shadow-sm mb-3" dir="rtl">
    <div class="card-body">

        <div class="row g-3">

            <div class="col-md-4">
                <asp:Label
                    runat="server"
                    AssociatedControlID="ddlMovementType"
                    Text="نوع الحركة"
                    CssClass="form-label" />

                <asp:DropDownList
                    ID="ddlMovementType"
                    runat="server"
                    CssClass="form-select" />
            </div>

            <div class="col-md-4">
                <asp:Label
                    runat="server"
                    AssociatedControlID="txtReferenceNumber"
                    Text="الرقم المرجعي"
                    CssClass="form-label" />

                <asp:TextBox
                    ID="txtReferenceNumber"
                    runat="server"
                    MaxLength="100"
                    CssClass="form-control" />
            </div>

            <div class="col-md-4 d-flex align-items-end gap-2">
                <asp:Button
                    ID="btnSearch"
                    runat="server"
                    Text="بحث"
                    CssClass="btn btn-primary"
                    CausesValidation="false"
                    OnClick="btnSearch_Click" />

                <asp:Button
                    ID="btnReset"
                    runat="server"
                    Text="عرض الكل"
                    CssClass="btn btn-secondary"
                    CausesValidation="false"
                    OnClick="btnReset_Click" />
            </div>

        </div>

    </div>
</div>

  <dx:ASPxGridView
    ID="gvStockMovements"
    runat="server"
    AutoGenerateColumns="false"
    KeyFieldName="Id"
    Width="100%"
    RightToLeft="True"
    EnableCallBacks="false"
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

    <SettingsText
    EmptyDataRow="لا توجد حركات مخزون مطابقة."
    GroupPanel="اسحبي عنوان عمود هنا لتجميع الحركات حسبه."
    SearchPanelEditorNullText="ابحثي في الحركات..."
    CommandApplySearchPanelFilter="بحث"
    CommandClearSearchPanelFilter="مسح" />

       <SettingsSearchPanel
    Visible="true"
    ShowApplyButton="true"
    ShowClearButton="true"
    AllowTextInputTimer="false"
    ColumnNames="MovementTypeName;WarehouseName;ItemName;ReferenceNumber;DestinationName" />

    <Columns>

        <dx:GridViewDataTextColumn
            FieldName="Id"
            Caption="#"
            VisibleIndex="0" />

        <dx:GridViewDataTextColumn
            FieldName="MovementTypeName"
            Caption="نوع الحركة"
            VisibleIndex="1" />

        <dx:GridViewDataTextColumn
            FieldName="WarehouseName"
            Caption="المستودع"
            VisibleIndex="2" />

        <dx:GridViewDataTextColumn
            FieldName="ItemName"
            Caption="الصنف"
            VisibleIndex="3" />

        <dx:GridViewDataSpinEditColumn
            FieldName="Quantity"
            Caption="الكمية"
            VisibleIndex="4">

            <PropertiesSpinEdit
                DisplayFormatString="N2" />

        </dx:GridViewDataSpinEditColumn>

        <dx:GridViewDataDateColumn
            FieldName="MovementDate"
            Caption="التاريخ"
            VisibleIndex="5">

            <PropertiesDateEdit
                DisplayFormatString="yyyy/MM/dd HH:mm" />

        </dx:GridViewDataDateColumn>

        <dx:GridViewDataTextColumn
            FieldName="ReferenceNumber"
            Caption="الرقم المرجعي"
            VisibleIndex="6" />

        <dx:GridViewDataTextColumn
            FieldName="DestinationName"
            Caption="الوجهة"
            VisibleIndex="7" />

        <dx:GridViewDataTextColumn
            Caption="الإجراءات"
            VisibleIndex="8">

            <Settings
                AllowSort="False"
                AllowGroup="False"
                AllowAutoFilter="False" />

            <DataItemTemplate>

                <a class="btn btn-sm btn-info"
                   href='<%# "Details.aspx?id=" + Eval("Id") %>'>
                    التفاصيل
                </a>

                <asp:HyperLink
                    ID="lnkCancel"
                    runat="server"
                    CssClass="btn btn-sm btn-danger"
                    Text="إلغاء"
                    NavigateUrl='<%# "Cancel.aspx?id=" + Eval("Id") %>'
                    Visible='<%# CanCancelMovement(
                        Eval("IsCancelled"),
                        Eval("OriginalMovementId"),
                        Eval("MovementType")) %>' />

                <asp:Label
                    ID="lblCancelled"
                    runat="server"
                    CssClass="badge bg-danger"
                    Text="ملغاة"
                    Visible='<%# Convert.ToBoolean(
                        Eval("IsCancelled")) %>' />

                <asp:Label
                    ID="lblReversal"
                    runat="server"
                    CssClass="badge bg-secondary"
                    Text="حركة عكسية"
                    Visible='<%# Eval("OriginalMovementId") != null
                        && Eval("OriginalMovementId") != DBNull.Value %>' />

            </DataItemTemplate>

        </dx:GridViewDataTextColumn>

    </Columns>

       <TotalSummary>

    <dx:ASPxSummaryItem
        FieldName="Id"
        SummaryType="Count"
        ShowInColumn="MovementTypeName"
        DisplayFormat="عدد الحركات: {0:N0}" />

</TotalSummary>

</dx:ASPxGridView>

        </div>

    </div>
            </ContentTemplate>

</asp:UpdatePanel>

 <script type="text/javascript">
     function initializeMovementSearch() {
         var $referenceInput =
             jQuery("#<%= txtReferenceNumber.ClientID %>");

        $referenceInput
            .off("keydown.movementSearch")
            .on("keydown.movementSearch", function (event) {
                if (event.key === "Enter") {
                    event.preventDefault();

                    var searchButton =
                        document.getElementById(
                            "<%= btnSearch.ClientID %>");

                    if (searchButton && !searchButton.disabled) {
                        searchButton.click();
                    }
                }
            });
     }

     function setMovementLoading(isLoading) {
         var $searchButton =
             jQuery("#<%= btnSearch.ClientID %>");

        var $resetButton =
            jQuery("#<%= btnReset.ClientID %>");

        $searchButton.prop("disabled", isLoading);
        $resetButton.prop("disabled", isLoading);

        $searchButton.val(
            isLoading ? "جارٍ البحث…" : "بحث");

        jQuery("#<%= gvStockMovements.ClientID %>")
             .attr("aria-busy", isLoading ? "true" : "false");
     }

     Sys.Application.add_load(function () {
         initializeMovementSearch();
     });

     var movementRequestManager =
         Sys.WebForms.PageRequestManager.getInstance();

     movementRequestManager.add_beginRequest(
         function (sender, args) {
             setMovementLoading(true);
         });

     movementRequestManager.add_endRequest(
         function (sender, args) {
             setMovementLoading(false);
         });
</script>

</asp:Content>