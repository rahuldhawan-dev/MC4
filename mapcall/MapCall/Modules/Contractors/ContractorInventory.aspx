<%@ Page Title="Contractor Inventory" Language="C#" Theme="bender" MasterPageFile="~/MapCallSite.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="ContractorInventory.aspx.cs" Inherits="MapCall.Modules.Contractors.ContractorInventory" %>
<%@ Register TagPrefix="mapcall" TagName="DetailsViewDataPageTemplate" Src="~/Controls/DetailsViewDataPageTemplate.ascx" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls" TagPrefix="mapcall" %>
<%@ Register TagPrefix="search" Namespace="MapCall.Controls.SearchFields" Assembly="MapCall" %>
<%@ Register TagPrefix="mapcall" Namespace="MapCall.Controls.DropDowns" Assembly="MapCall" %>
<%@ Register Src="~/Modules/Contractors/ContractorInventoriesMaterials.ascx" TagPrefix="contractors" TagName="ContractorInventoriesMaterials" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>
<%@ Register assembly="MMSINC" namespace="MMSINC" tagPrefix="mmsinc" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
    Contractor Inventory
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">
    
<mapcall:DetailsViewDataPageTemplate runat="server" ID="template"
    DataElementTableName="ContractorInventories" DataTypeId="149"
    DataElementPrimaryFieldName="ContractorInventoriesID"
    Label="Contractor Inventory Record">
       
    <SearchBox runat="server">
        <Fields>
            <search:DropDownSearchField 
                Label="Contractor" 
                DataFieldName="c.ContractorID"
                TableName="Contractors"
                TextFieldName="Name"
                ValueFieldName="ContractorID" />
            <search:TemplatedSearchField 
                Label="Operating Center"
                FilterMode="Manual">
                <Template>
                    <mapcall:OperatingCenterDropDownList ID="searchOpCntr" runat="server" />
                </Template>
            </search:TemplatedSearchField>
            <search:TemplatedSearchField DataFieldName="ci.StockLocationID"
                Label="Warehouse"
                BindingControlID="searchWarehouse" 
                BindingDataType="Int32"
                BindingPropertyName="SelectedValue">
                <Template>
                    <asp:DropDownList runat="server" ID="searchWarehouse" />
                    <atk:CascadingDropDown runat="server" ID="CascadingDropDown1"
                        TargetControlID="searchWarehouse" ParentControlID="searchOpCntr"
                        Category="Warehouse"
                        EmptyText="None Found" EmptyValue=""
                        PromptText="--Select Warehouse--" PromptValue=""
                        LoadingText="[Loading Warehouses...]"
                        ServicePath="~/Modules/Data/DropDowns.asmx"
                        ServiceMethod="GetStockLocationsByOperatingCenter"/>

                </Template>
            </search:TemplatedSearchField>
            <search:DateTimeSearchField DataFieldName="ci.DateRecorded" />
            <search:TextSearchField DataFieldName="ci.DocID"/>

        </Fields>
    </SearchBox>
    
    <ResultsGridView>
        <Columns>
            <mapcall:BoundField DataField="DateRecorded" />
            <mapcall:BoundField DataField="ContractorText" HeaderText="Contractor" />
            <mapcall:BoundField DataField="OpCntr" />
            <mapcall:BoundField DataField="StockLocationText" HeaderText="Warehouse" />
            <mapcall:BoundField DataField="DocID" />
            <mapcall:BoundField DataField="Notes" />
            <mapcall:BoundField DataField="MaterialText" HeaderText="Material" />
            <mapcall:BoundField DataField="Quantity" />
        </Columns>
    </ResultsGridView>
    
    <ResultsDataSource
        SelectCommand="
            SELECT
	            *,
                oc.OperatingCenterCode + ' - ' + oc.OperatingCenterName as OpCntr,
                c.Name as [ContractorText],
                sl.Description as [StockLocationText],
                m.Description as [MaterialText]
            FROM
	            ContractorInventories ci
            LEFT JOIN
                Contractors c
            ON
                c.ContractorID = ci.ContractorID
            JOIN 
                OperatingCenters oc
            ON
                oc.OperatingCenterID = ci.OperatingCenterID
            LEFT JOIN
	            ContractorInventoriesMaterials cim
            ON
	            cim.ContractorInventoriesID = ci.ContractorInventoriesID
            LEFT JOIN
	            Materials m
            ON
	            m.MaterialID = cim.MaterialID
            LEFT JOIN
	            StockLocations sl
            ON
	            sl.StockLocationID = ci.StockLocationID"/>

    <DetailsView AutoGenerateRows="False" DataKeyNames="ContractorInventoriesID,OperatingCenterID">
        <Fields>
            <mapcall:BoundField DataField="ContractorInventoriesID" HeaderText="ContractorInventoriesID"
                 ReadOnly="True" InsertVisible="False"/>
            <asp:TemplateField HeaderText="Operating Center">
                <ItemTemplate><%#Eval("OpCntr")%></ItemTemplate>
                <EditItemTemplate>
                    <mapcall:OperatingCenterDropDownList ID="ddlOpCntr" runat="server" SelectedValue='<%#Bind("OperatingCenterID") %>' />
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Contractor">
                <ItemTemplate><%#Eval("ContractorText") %></ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList runat="server" ID="ddlContractors" /> 
                    <atk:CascadingDropDown runat="server" ID="cddlContractor"
                        TargetControlID="ddlContractors" ParentControlID="ddlOpCntr"
                        Category="Contractor" SelectedValue='<%#Bind("ContractorID") %>'
                        EmptyText="None Found" EmptyValue=""
                        PromptText="--Select a Contractor--"
                        LoadingText="[Loading Contractors...]"
                        ServicePath="~/Modules/Data/DropDowns.asmx"
                        ServiceMethod="GetContractorsByOperatingCenter"/>   
                    <asp:RequiredFieldValidator ControlToValidate="ddlContractors" InitialValue="" Text="Required" runat="server" Display="Dynamic" />
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Warehouse">
                <ItemTemplate><%#Eval("StockLocationText") %></ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList runat="server" ID="ddlStockLocations" />
                    <atk:CascadingDropDown runat="server" ID="CascadingDropDown1"
                        TargetControlID="ddlStockLocations" ParentControlID="ddlOpCntr"
                        Category="Warehouse" SelectedValue='<%#Bind("StockLocationID") %>'
                        EmptyText="None Found" EmptyValue=""
                        PromptText="--Select a Warehouse--" PromptValue=""
                        LoadingText="[Loading Warehouses...]"
                        ServicePath="~/Modules/Data/DropDowns.asmx"
                        ServiceMethod="GetStockLocationsByOperatingCenter"/>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="ddlStockLocations" InitialValue="" Text="Required" runat="server" Display="Dynamic" />
                </EditItemTemplate>
            </asp:TemplateField>
            

            <mmsinc:BoundField DataField="DateRecorded" HeaderText="Date Recorded" SqlDataType="DateTime" 
                DataFormatString="{0:M/dd/yyyy}" Required="true" />
            <mapcall:BoundField DataField="DocID" HeaderText="DocID" MaxLength="15" Required="True" />
            <asp:CheckBoxField DataField="Returned" HeaderText="Is the material being returned:"/>
            <mmsinc:BoundField DataField="Notes" HeaderText="Notes" SqlDataType="Text" />
        </Fields>
    </DetailsView>
    
    <DetailsDataSource
        DeleteCommand="
            DELETE FROM [ContractorInventoriesMaterials] WHERE [ContractorInventoriesID] = @ContractorInventoriesID;
            DELETE FROM [ContractorInventories] WHERE [ContractorInventoriesID] = @ContractorInventoriesID"
        
        InsertCommand="
            INSERT INTO [ContractorInventories] ([ContractorID],[OperatingCenterID],[DateRecorded],[StockLocationID],[DocID],[Returned],[Notes]) 
            VALUES(@ContractorID, @OperatingCenterID, @DateRecorded, @StockLocationID, @DocID, @Returned, @Notes); 
            SELECT @ContractorInventoriesID = (SELECT @@IDENTITY)"

        UpdateCommand="UPDATE [ContractorInventories] SET ContractorID = @ContractorID, OperatingCenterID = @OperatingCenterID, DateRecorded=@DateRecorded, @StockLocationID = StockLocationID, DocID = @DocID, Returned=@Returned, Notes=@Notes WHERE ContractorInventoriesID = @ContractorInventoriesID"
        SelectCommand="
            SELECT 
                ci.*,
                oc.OperatingCenterCode + ' - ' + oc.OperatingCenterName as OpCntr,
                c.Name as [ContractorText],
                sl.Description as [StockLocationText]
            FROM 
	            ContractorInventories ci
            LEFT JOIN
                Contractors c
            ON
                c.ContractorID = ci.ContractorID
            JOIN 
                OperatingCenters oc
            ON
                oc.OperatingCenterID = ci.OperatingCenterID
            LEFT JOIN
	            StockLocations sl
            ON
	            sl.StockLocationID = ci.StockLocationID
            WHERE
                ci.ContractorInventoriesID = @ContractorInventoriesID">
        <SelectParameters>
            <asp:Parameter Name="ContractorInventoriesID" Type="Int32" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="ContractorInventoriesID" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="ContractorInventoriesID" Type="Int32" />
            <asp:Parameter Name="OperatingCenterID" Type="Int32"/>
            <asp:Parameter Name="ContractorID" Type="Int32"/>
            <asp:Parameter Name="DateRecorded" Type="DateTime"/>
            <asp:Parameter Name="StockLocationID" Type="Int32"/>
            <asp:Parameter Name="DocID" Type="String"/>
            <asp:Parameter Name="Returned" Type="Boolean" />
            <asp:Parameter Name="Notes" Type="String"/>
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="ContractorInventoriesID" Type="Int32" Direction="Output" />
            <asp:Parameter Name="OperatingCenterID" Type="Int32"/>
            <asp:Parameter Name="ContractorID" Type="Int32"/>
            <asp:Parameter Name="DateRecorded" Type="DateTime"/>
            <asp:Parameter Name="StockLocationID" Type="Int32"/>
            <asp:Parameter Name="DocID" Type="String"/>
            <asp:Parameter Name="Returned" Type="Boolean" />
            <asp:Parameter Name="Notes" Type="String"/>
        </InsertParameters>
    </DetailsDataSource>
    
    <Tabs>
        <mapcall:Tab runat="server" ID="tabMaterials" Label="Materials">
            <contractors:ContractorInventoriesMaterials runat="server" ID="materials"
                CanAdd="<%#Permissions.CreateAccess.IsAllowed %>" 
                CanDelete="<%#Permissions.DeleteAccess.IsAllowed %>"
                CanEdit="<%#Permissions.EditAccess.IsAllowed %>"
                Returned="True"
                EnableViewState="True"
                FilterExpression="WHERE c.ContractorInventoriesID = @ContractorInventoriesID">
                <FilterParameters>
                    <asp:ControlParameter Name="ContractorInventoriesID" DbType="Int32" ControlID="detailView" PropertyName="SelectedValue" />
                </FilterParameters>
            </contractors:ContractorInventoriesMaterials>
        </mapcall:Tab>
    </Tabs>

</mapcall:DetailsViewDataPageTemplate>
    
</asp:Content>
