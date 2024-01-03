<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContractorInventoriesMaterials.ascx.cs" Inherits="MapCall.Modules.Contractors.ContractorInventoriesMaterials" %>
<%@ Register TagPrefix="mmsi" Namespace="MMSINC.Controls" Assembly="MMSINC.Core.WebForms" %>
<%@ Register Src="~/Controls/MaterialPicker.ascx" TagPrefix="mapcall" TagName="MaterialPicker" %>

<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <mmsi:MvpGridView ID="gvResults" runat="server" OnRowDeleting="gvResults_RowDeleting"
            EmptyDataText="No material is associated with this inventory record. Use the control below to add them."
            EnableViewState="True"
            DataKeyNames="ContractorInventoriesMaterialsID,ContractorInventoriesID" AutoGenerateColumns="False">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="btnDelete" 
                            CommandName="Delete" Visible="<%#CanDelete%>"
                            CausesValidation="False" 
                            Text="Delete"  EnableViewState="False"
                            CommandArgument="ContractorInventoriesMaterialsID"
                            OnClientClick="return confirm('Are you sure you want to remove this material?');" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PartNumber" HeaderText="Part Number" />
                <asp:BoundField DataField="Size" HeaderText="Size" />
                <asp:BoundField DataField="Description" HeaderText="Material Description" />
                <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
            </Columns>
        </mmsi:MvpGridView>

        <asp:SqlDataSource runat="server" ID="dsResults"
            ConnectionString="<%$ ConnectionStrings:MCProd %>" CancelSelectOnNullParameter="true" 
            DeleteCommand="DELETE FROM [ContractorInventoriesMaterials] where ContractorInventoriesMaterialsID = @ContractorInventoriesMaterialsID"
            InsertCommand="
                --hack much?
                declare @returned bit
                select @returned = (select returned from ContractorInventories where ContractorInventoriesID = @ContractorInventoriesID)
                if (@returned = 1) select @quantity = -abs(@quantity)

                INSERT INTO [ContractorInventoriesMaterials] 
                Values(@ContractorInventoriesID, @MaterialID, @Quantity)"
            SelectCommand="
                SELECT 
	                M.PartNumber,
                    M.Size, 
                    M.Description,
                    abs(cim.Quantity) as Quantity,
                    cim.ContractorInventoriesMaterialsID,
                    cim.ContractorInventoriesID 
                FROM 
	                [ContractorInventoriesMaterials] cim
                JOIN	
	                Materials M
                ON
	                m.MaterialID = cim.MaterialID
                WHERE 
                    ContractorInventoriesID = @ContractorInventoriesID
                AND
                    m.IsActive = 'true'
            ">
            <DeleteParameters>
                <asp:Parameter Name="ContractorInventoriesMaterialsID" Type="Int32"/>
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="MaterialID" DbType="Int32" />
                <asp:Parameter Name="Quantity" DbType="Int32" />
            </InsertParameters>
        </asp:SqlDataSource>
        <style type="text/css">
            fieldset { margin-top: 20px;width: 400px;}
            fieldset div { padding: 3px;}
        </style>
        <fieldset>
            <legend><h3>Add New Material</h3></legend>
            
            <div>
                <mapcall:MaterialPicker runat="server" ID="materialPicker" EnableViewState="true"
                    OperatingCenterID="<%#OperatingCenterID%>" />
            </div>
            
            <div>
                Quantity: <br/>
                <asp:TextBox runat="server" ID="txtQuantity" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtQuantity" Text="Required" Display="Dynamic"/>
                <asp:RegularExpressionValidator ValidationExpression="\d+" 
                    ControlToValidate="txtQuantity"
                    Text="Please enter a number" runat="server"/>           
            </div>
            <div>
                <asp:Button runat="server" ID="btnAdd" Text="Add" OnClick="btnAdd_Click" Visible="<%#CanAdd%>" />
            </div>
        </fieldset>
    
    </ContentTemplate>
</asp:UpdatePanel>