<%@ Page Title="" Language="C#" MasterPageFile="~/MapCallSite.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="CurrentInventory.aspx.cs" Inherits="MapCall.Reports.Contractors.CurrentInventory" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls" TagPrefix="mapcall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.DropDowns" TagPrefix="mapcall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.SearchFields" TagPrefix="search" %>
<%@ Register Src="~/Controls/DetailsViewDataPageTemplate.ascx" TagPrefix="mapcall"
    TagName="DetailsViewDataPageTemplate" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>
    
<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
    Current Contractor Inventory
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">
    
<mapcall:DetailsViewDataPageTemplate ID="template" runat="server" 
    DataElementTableName="ContractorInventories" 
    DataElementPrimaryFieldName="OperatingCenterID"
    IsReadOnlyPage="true"
    Label="Employee Position Control Report">
        <SearchBox runat="server">
            <Fields>
                <search:DropDownSearchField 
                    Label="Contractor" 
                    DataFieldName="ContractorID"
                    TableName="Contractors"
                    TextFieldName="Name"
                    ValueFieldName="ContractorID" />
                <search:TemplatedSearchField 
                    Label="Operating Center"
                    DataFieldName="OperatingCenterID"
                    BindingControlID="searchOpCntr"
                    BindingDataType="Int32"
                    BindingPropertyName="SelectedValue">
                    <Template>
                        <mapcall:OperatingCenterDropDownList ID="searchOpCntr" runat="server" />
                    </Template>    
                </search:TemplatedSearchField>
                <search:TemplatedSearchField DataFieldName="StockLocationID"
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
            </Fields>
        </SearchBox>

    <ResultsGridView>
        <Columns>
            <mapcall:BoundField DataField="OpCntr"/>
            <mapcall:BoundField DataField="Contractor" />
            <mapcall:BoundField DataField="Warehouse"/>
            <mapcall:BoundField DataField="PartNumber"/>
            <mapcall:BoundField DataField="PartDescription"/>
            <mapcall:BoundField DataField="Quantity"/>
        </Columns>
    </ResultsGridView>
    <ResultsDataSource ConnectionString='<%$ ConnectionStrings:MCProd %>' 
        SelectCommand="
			SELECT * FROM
			(SELECT 
	            OperatingCenterID,
				OpCntr,
	            ContractorID,
	            Contractor, 
	            StockLocationID, 
	            Warehouse,
	            MaterialID,
	            PartNumber, 
	            PartDescription, 
	            SUM(Quantity) as [Quantity]
            FROM 
	            (
	            SELECT
		            CI.OperatingCenterID, 
		            OC.OperatingCenterCode + ' - ' + OC.OperatingCenterName as [OpCntr],
		            C.ContractorID,
		            C.Name as [Contractor],
		            CI.StockLocationID,
		            SL.Description as [Warehouse], 
		            M.MaterialID, 
		            M.PartNumber,
		            cast(M.Description as varchar) as [PartDescription],
		            Quantity
	            FROM
		            ContractorInventories CI
	            JOIN
		            ContractorInventoriesMaterials CIM
	            ON 
		            CI.ContractorInventoriesID = CIM.ContractorInventoriesID
	            JOIN
		            Contractors C
	            ON
		            C.ContractorID = CI.ContractorID
	            JOIN
		            StockLocations SL
	            ON
		            SL.StockLocationID = CI.StockLocationID
	            JOIN
		            Materials M
	            ON
		            M.MaterialID = CIM.MaterialID	
		        JOIN
					OperatingCenters OC
				ON
					OC.OperatingCenterID = CI.OperatingCenterID
				AND
					M.IsActive = 1
	            UNION ALL
	            SELECT
		            WO.OperatingCenterID,
		            OC.OperatingCenterCode + ' - ' + OC.OperatingCenterName as [OpCntr],
		            C.ContractorID,
		            C.Name as [Contractor],
		            MU.StockLocationID,
		            SL.Description as [Warehouse],
		            M.MaterialID, 
		            M.PartNumber,
		            cast(M.Description as varchar) as [PartDescription],
		            -MU.Quantity
	            FROM
		            MaterialsUsed MU
	            JOIN
		            WorkOrders WO
	            ON
		            WO.WorkOrderID = MU.WorkOrderID
	            JOIN
		            Contractors C
	            ON
		            C.ContractorID = wo.AssignedContractorID
	            JOIN
		            StockLocations SL
	            ON
		            SL.StockLocationID = MU.StockLocationID
	            JOIN
		            Materials M
	            ON
		            M.MaterialID = MU.MaterialID
		        JOIN
					OperatingCenters OC
				ON
					OC.OperatingCenterID = WO.OperatingCenterID
				AND
					M.IsActive = 1
	            ) 
            AS 
            [Results]

            GROUP BY
	            OperatingCenterID,
	            OpCntr,
	            ContractorID,
	            Contractor, 
	            StockLocationID, 
	            Warehouse,
	            MaterialID,
	            PartNumber,
	            PartDescription
	        ) AS [SERIOUSLY]" />

    </mapcall:DetailsViewDataPageTemplate>

</asp:Content>
