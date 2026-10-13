<%@ Page Title="إضافة حركة مخزون"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Create.aspx.cs"
    Inherits="NexusERP.Web.StockMovements.Create" %>

<asp:Content
    ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container py-4">

        <div class="card shadow-sm">

            <div class="card-header">
                <h3 class="mb-0">إضافة حركة مخزون</h3>
            </div>

            <div class="card-body">

                <asp:ValidationSummary
                    ID="validationSummary"
                    runat="server"
                    ValidationGroup="MovementForm"
                    CssClass="alert alert-danger"
                    HeaderText="يرجى تصحيح الأخطاء التالية:" />

                <asp:Panel
                    ID="pnlMessage"
                    runat="server"
                    Visible="false"
                    CssClass="alert alert-danger">

                    <asp:Literal
                        ID="litMessage"
                        runat="server" />

                </asp:Panel>

                <div class="row">

                    <%-- نوع الحركة --%>
                    <div class="col-md-6 mb-3">

                        <label class="form-label">
                            نوع الحركة
                        </label>

                        <asp:DropDownList
                            ID="ddlMovementType"
                            runat="server"
                            CssClass="form-select"
                            onchange="toggleDestinationWarehouse();">
                        </asp:DropDownList>

                        <asp:RequiredFieldValidator
                            ID="rfvMovementType"
                            runat="server"
                            ControlToValidate="ddlMovementType"
                            InitialValue=""
                            ValidationGroup="MovementForm"
                            CssClass="text-danger"
                            ErrorMessage="يجب اختيار نوع الحركة."
                            Display="Dynamic" />

                    </div>

                    <%-- المستودع الأساسي --%>
                    <div class="col-md-6 mb-3">

                        <label class="form-label">
                            المستودع
                        </label>

                        <asp:DropDownList
                            ID="ddlWarehouse"
                            runat="server"
                            CssClass="form-select">
                        </asp:DropDownList>

                        <asp:RequiredFieldValidator
                            ID="rfvWarehouse"
                            runat="server"
                            ControlToValidate="ddlWarehouse"
                            InitialValue=""
                            ValidationGroup="MovementForm"
                            CssClass="text-danger"
                            ErrorMessage="يجب اختيار المستودع."
                            Display="Dynamic" />

                    </div>

                    <%-- الصنف --%>
                    <div class="col-md-6 mb-3">

                        <label class="form-label">
                            الصنف
                        </label>

                        <asp:DropDownList
                            ID="ddlItem"
                            runat="server"
                            CssClass="form-select">
                        </asp:DropDownList>

                        <asp:RequiredFieldValidator
                            ID="rfvItem"
                            runat="server"
                            ControlToValidate="ddlItem"
                            InitialValue=""
                            ValidationGroup="MovementForm"
                            CssClass="text-danger"
                            ErrorMessage="يجب اختيار الصنف."
                            Display="Dynamic" />

                    </div>

                    <%-- الكمية --%>
                    <div class="col-md-6 mb-3">

                        <label class="form-label">
                            الكمية
                        </label>

                        <asp:TextBox
                            ID="txtQuantity"
                            runat="server"
                            CssClass="form-control"
                            TextMode="Number"
                            min="0.01"
                            step="0.01">
                        </asp:TextBox>

                        <asp:RequiredFieldValidator
                            ID="rfvQuantity"
                            runat="server"
                            ControlToValidate="txtQuantity"
                            ValidationGroup="MovementForm"
                            CssClass="text-danger"
                            ErrorMessage="يجب إدخال الكمية."
                            Display="Dynamic" />

                    </div>

                    <%-- تاريخ الحركة --%>
                    <div class="col-md-6 mb-3">

                        <label class="form-label">
                            تاريخ الحركة
                        </label>

                        <asp:TextBox
                            ID="txtMovementDate"
                            runat="server"
                            CssClass="form-control"
                            TextMode="DateTimeLocal">
                        </asp:TextBox>

                        <asp:RequiredFieldValidator
                            ID="rfvMovementDate"
                            runat="server"
                            ControlToValidate="txtMovementDate"
                            ValidationGroup="MovementForm"
                            CssClass="text-danger"
                            ErrorMessage="يجب إدخال تاريخ الحركة."
                            Display="Dynamic" />

                    </div>

                    <%-- مستودع الوجهة --%>
                    <div
                        id="destinationWarehouseContainer"
                        class="col-md-6 mb-3"
                        style="display:none;">

                        <label class="form-label">
                            مستودع الوجهة
                        </label>

                        <asp:DropDownList
                            ID="ddlDestinationWarehouse"
                            runat="server"
                            CssClass="form-select">
                        </asp:DropDownList>

                        <small class="text-muted">
                            هذا الحقل مطلوب فقط عند التحويل.
                        </small>

                    </div>

                    <%-- الرقم المرجعي --%>
                    <div class="col-md-6 mb-3">

                        <label class="form-label">
                            الرقم المرجعي
                        </label>

                        <asp:TextBox
                            ID="txtReferenceNumber"
                            runat="server"
                            CssClass="form-control"
                            MaxLength="50">
                        </asp:TextBox>

                    </div>

                    <%-- الملاحظات --%>
                    <div class="col-12 mb-3">

                        <label class="form-label">
                            الملاحظات
                        </label>

                        <asp:TextBox
                            ID="txtNotes"
                            runat="server"
                            CssClass="form-control"
                            TextMode="MultiLine"
                            Rows="4"
                            MaxLength="500">
                        </asp:TextBox>

                    </div>

                </div>

                <asp:Button
                    ID="btnSave"
                    runat="server"
                    Text="حفظ الحركة"
                    CssClass="btn btn-primary"
                    ValidationGroup="MovementForm"
                    OnClick="btnSave_Click" />

                <a href="Index.aspx"
                   class="btn btn-secondary">
                    رجوع
                </a>

            </div>

        </div>

    </div>

    <script type="text/javascript">

        function toggleDestinationWarehouse() {

            var movementType =
                document.getElementById(
                    '<%= ddlMovementType.ClientID %>');

            var destinationContainer =
                document.getElementById(
                    'destinationWarehouseContainer');

            if (!movementType || !destinationContainer) {
                return;
            }

            // القيمة 4 تعني Transfer
            if (movementType.value === '4') {
                destinationContainer.style.display = 'block';
            }
            else {
                destinationContainer.style.display = 'none';
            }
        }

        document.addEventListener(
            'DOMContentLoaded',
            toggleDestinationWarehouse);

    </script>

</asp:Content>