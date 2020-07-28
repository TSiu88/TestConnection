<%@ Page Title="Connection Example" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ConnectionExample.aspx.cs" Inherits="DataAccess.ConnectionExample" %>
<%--Created web page to show results from connection to DB--%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Database Connection Example</h2>
    <asp:Literal ID="ltConnectionMessage" runat="server"></asp:Literal>
    <h3>DB items with pickqty > 20: </h3>
    <div class="row">
        <ul>
            <asp:Literal ID="ltOutput" runat="server"></asp:Literal>
        </ul>
    </div>
    <h3> Grid View Example </h3>
    <asp:Literal ID="ltError" runat="server"></asp:Literal>
    <%--Auto generate columns false = choose which columns to display, not show all columns--%>
    <%--Have auto created methods for deleting, editing, updating, and canceling gridview edit--%>
    <asp:GridView ID="gvData" runat="server" AutoGenerateColumns="false" OnRowDeleting="gvData_RowDeleting" OnRowEditing="gvData_RowEditing" OnRowUpdating="gvData_RowUpdating" OnRowCancelingEdit="gvData_RowCancelingEdit" >
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <%--This hidden field so not show up in table but can refer to row to use methods on a specific row's data--%>
                    <asp:HiddenField ID="hdnItemId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "itemlookupcode") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <%--BoundField autogenerates controls for field depending on if editing or not, use for simple text fields--%>
            <%--Use Template Field for more complex fields--%>
            <asp:BoundField DataField="itemDescription" HeaderText="Item Name" />
            <asp:BoundField DataField="storecode" HeaderText="Store Code" />
            <asp:BoundField DataField="pickQty" HeaderText="Pick Qty" />
            <%--These command fields automatically add edit and delete buttons in row--%>
            <asp:CommandField ShowEditButton="true" />
            <asp:CommandField ShowDeleteButton="true" />
        </Columns>
    </asp:GridView>
    <div class="row data-table">
        <%--Button to add a new row to table--%>
        <asp:Button ID="btnAddRow" runat="server" Text="Add New Row" OnClick="btnAddRow_Click" />
    </div>
</asp:Content>
