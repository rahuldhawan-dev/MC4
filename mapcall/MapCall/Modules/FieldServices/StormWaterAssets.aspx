<%@ Page Title="Storm Water Assets" Theme="bender" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="StormWaterAssets.aspx.cs" Inherits="MapCall.Modules.FieldServices.StormWaterAssets" EnableEventValidation="false" %>
<%@ Register Src="~/Controls/ddlMcProdOperatingCenter.ascx" TagName="ddlMcProdOperatingCenter" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>
<%@ Register assembly="MMSINC" namespace="MMSINC" tagPrefix="mmsinc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Storm Water Assets
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel ID="pnlSearch" runat="server">
        <center>
            <table style="width:650px;border:1px solid black;">
                <tr>
                    <td class="leftcol">Operating Center : </td>
                    <td class="rightcol">
                        <mmsi:ddlMcProdOperatingCenter runat="server" id="ddlOpCntr" BaseRole="Water Non Potable_Storm Water" Required="true" />
                    </td>
                </tr>
                
                <tr>
                    <td class="leftcol">Town : </td>
                    <td class="rightcol">
                        <asp:DropDownList runat="server" ID="ddlTown" />
                        <atk:CascadingDropDown runat="server" ID="cddTowns" 
                            TargetControlID="ddlTown" ParentControlID="ddlOpCntr$ddlOpCntr" 
                            Category="Town" 
                            EmptyText="None Found" EmptyValue=""
                            PromptText="--Select Here--" PromptValue="" 
                            LoadingText="[Loading Towns...]"
                            ServicePath="~/Modules/Data/DropDowns.asmx" 
                            ServiceMethod="GetTownsByOperatingCenter"
                            SelectedValue='<%# Bind("TownID") %>' 
                        />
                    </td>
                </tr>
                
                <tr>
                    <td class="leftcol">Street :</td>
                    <td class="rightcol">
                        <asp:DropDownList runat="server" ID="ddlStreet" />
                        <atk:CascadingDropDown runat="server" ID="cddStreets"   
                            TargetControlID="ddlStreet"
                            ParentControlID="ddlTown"
                            Category="Street"
                            EmptyText="None Found" EmptyValue=""
                            PromptText="--Select Here--" PromptValue=""
                            LoadingText="[Loading Streets...]"
                            ServicePath="~/Modules/Data/DropDowns.asmx" 
                            ServiceMethod="GetStreetsByTown"
                            SelectedValue='<%# Bind("StreetID") %>' 
                        />
                    </td>
                </tr>
                
                <mmsi:DataField runat="server" ID="dfAssetNumber" DataType="String" HeaderText="Asset Number : " DataFieldName="AssetNumber" />
                <mmsi:DataField runat="server" ID="dfAssetStatus" DataType="DropDownList" HeaderText="Status : " 
                    DataFieldName="AssetStatusID"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    SelectCommand="SELECT DISTINCT AssetStatusID AS [Val], Description AS [Txt] FROM [AssetStatuses] ORDER BY [Description]"
            />
                
                <tr>
                    <td></td>
                    <td>
                        <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                        <asp:Button runat="server" ID="btnReset" Text="Reset" OnClick="btnReset_Click" CausesValidation="false" />
                        <asp:Button runat="server" ID="btnAdd" Text="Add" CausesValidation="False" OnClick="btnAdd_Click" />
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
    <asp:Panel ID="pnlResults" runat="server" Visible="false">
        <asp:HiddenField runat="server" ID="hidFilter" />
        <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Export" />
        <asp:Button runat="server" ID="btnMap" Visible="false" PostBackUrl="~/Modules/Maps/RealTimeOperations.aspx" Text="Map" />
        <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
        <asp:Label runat="server" ID="lblRecordCount" />
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1"
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" 
            DataKeyNames="StormWaterAssetID" 
            AutoGenerateColumns="false"
            AllowSorting="true"
        >
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
                <asp:BoundField HeaderText="Operating Center" DataField="OperatingCenterCode" SortExpression="OperatingCenterCode" />
                <asp:BoundField HeaderText="Town" DataField="Town" SortExpression="Town" />
                <asp:BoundField HeaderText="Asset Type" DataField="AssetType" SortExpression="AssetType" />
                <asp:BoundField HeaderText="Asset #" DataField="AssetNumber" SortExpression="Assetumber" />
                <asp:BoundField HeaderText="Street" DataField="StreetName" SortExpression="StreetName" />
                <asp:BoundField HeaderText="Intersecting Street" DataField="IntersectingStreetName" SortExpression="IntersectingStreetName" />
                <asp:BoundField HeaderText="Status" DataField="StatusText" SortExpression="StatusText" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:MCProd %>" SelectCommand="
SELECT 
    O.OperatingCenterCode,
    T.Town,
    S.StreetName,
	swat.Description as AssetType,
	#as.Description as StatusText,
    CS.FullStName as IntersectingStreetName,
    sm.*
FROM 
    [StormWaterAssets] sm
LEFT JOIN 
	[StormWaterAssetTypes] swat
ON
	sm.StormWaterAssetTypeID = swat.StormWaterAssetTypeID 
LEFT JOIN 
	[AssetStatuses] as #as
ON
	sm.AssetStatusID = #as.AssetSTatusID
LEFT JOIN 
    [OperatingCenters] O
ON
    O.OperatingCenterID = sm.OperatingCenterID
LEFT JOIN 
    [Towns] T
ON
    T.TownID = sm.TownID
LEFT JOIN 
    [Streets] S
ON
    S.StreetID = sm.StreetID
LEFT JOIN 
    [Streets] CS
ON
    CS.StreetID = sm.IntersectingStreetID   
	            " 
	    />
    </asp:Panel>
    <asp:Panel ID="pnlDetail" runat="server" Visible="false">
        <asp:DetailsView runat="server" ID="dvStormWaterAsset" AutoGenerateRows="False" 
            DataKeyNames="StormWaterAssetID" DataSourceID="dsStormWaterAsset"
            OnItemInserting="DetailView_ItemInserting"
            OnItemInserted="DetailView_ItemInserted"
            OnItemUpdating="DetailView_ItemUpdating"
            OnDataBound="DetailView_DataBound"
            Width="100%"
            >
            <FieldHeaderStyle Width="50%" />
            <Fields>
                <asp:BoundField DataField="StormWaterAssetID" HeaderText="StormWaterAssetID" 
                    InsertVisible="False" ReadOnly="True" SortExpression="StormWaterAssetID" />
                <asp:TemplateField HeaderText="Operating Center :">
                    <ItemTemplate><asp:Label runat="server" ID="lblOpCntr" Text='<%#Eval("OperatingCenterCode") %>' /></ItemTemplate>
                    <InsertItemTemplate>
                        <mmsi:ddlMcProdOperatingCenter runat="server" id="ddlOpCntr" SelectedValue='<%#Bind("OperatingCenterID") %>' 
                            BaseRole="_Water Non Potable_Storm Water_Add" Required="true" />
                    </InsertItemTemplate>
                    <EditItemTemplate>
                        <mmsi:ddlMcProdOperatingCenter runat="server" id="ddlOpCntr" SelectedValue='<%#Bind("OperatingCenterID") %>' 
                            BaseRole="_Water Non Potable_Storm Water_Edit" Required="true" />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Town :">
                    <ItemTemplate><%#Eval("Town") %></ItemTemplate>
                    <EditItemTemplate>
                        <%--This will be a cascading drop-down filtered by opcode --%>
                        <asp:DropDownList runat="server" ID="ddlTown" />
                        <asp:RequiredFieldValidator runat="server" ID="rfvTown" ControlToValidate="ddlTown" Text="*required" />
                            
                        <atk:CascadingDropDown runat="server" ID="cddTowns" 
                            TargetControlID="ddlTown" ParentControlID="ddlOpCntr$ddlOpCntr" 
                            Category="Town" 
                            EmptyText="None Found" EmptyValue=""
                            PromptText="--Select Here--" PromptValue="" 
                            LoadingText="[Loading Towns...]"
                            ServicePath="~/Modules/Data/DropDowns.asmx" 
                            ServiceMethod="GetTownsByOperatingCenter"
                            SelectedValue='<%# Bind("TownID") %>' 
                        />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Asset Type :">
                    <ItemTemplate><%#Eval("AssetType")%></ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList runat="server" ID="ddlStormWaterAssetType"
                            DataSourceID="dsStormWaterAssetTye"
                            DataTextField="Description" DataValueField="StormWaterAssetTypeID"
                            SelectedValue='<%#Bind("StormWaterAssetTypeID") %>'
                            AppendDataBoundItems="true"
                        >
                            <asp:ListItem Text="--Select Here--" Value=""/>
                        </asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="dsStormWaterAssetTye" 
                            ConnectionString="<%$ ConnectionStrings:MCProd %>"
                            SelectCommand="Select StormWaterAssetTypeID, Description from StormWaterAssetTypes Order by 2"/>
                            
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="AssetNumber" HeaderText="Asset # :" 
                    SortExpression="AssetNumber" />
                <asp:TemplateField HeaderText="Street : ">
                    <ItemTemplate><%#Eval("FullStName") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList runat="server" ID="ddlStreet" />
                        <atk:CascadingDropDown runat="server" ID="cddStreets"   
                            TargetControlID="ddlStreet"
                            ParentControlID="ddlTown"
                            Category="Street"
                            EmptyText="None Found" EmptyValue=""
                            PromptText="--Select Here--" PromptValue=""
                            LoadingText="[Loading Streets...]"
                            ServicePath="~/Modules/Data/DropDowns.asmx" 
                            ServiceMethod="GetStreetsByTown"
                            SelectedValue='<%# Bind("StreetID") %>' 
                        />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Intersecting Street :">
                    <ItemTemplate><%#Eval("FullStName2")%></ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList runat="server" ID="ddlIntersectingStreetID" />
                        <atk:CascadingDropDown runat="server" ID="cddIntersectingStreets"   
                            TargetControlID="ddlIntersectingStreetID"
                            ParentControlID="ddlTown"
                            Category="IntersectingStreet"
                            EmptyText="None Found" EmptyValue=""
                            PromptText="--Select Here--" PromptValue=""
                            LoadingText="[Loading Streets...]"
                            ServicePath="~/Modules/Data/DropDowns.asmx" 
                            ServiceMethod="GetStreetsByTown"
                            SelectedValue='<%# Bind("IntersectingStreetID") %>' 
                        />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="TaskNumber" HeaderText="Task # :" 
                    SortExpression="TaskNumber" />
                <mmsinc:BoundField DataField="DateInstalled" HeaderText="Date Installed :" 
                    SqlDataType="DateTime" SortExpression="DateInstalled" />
                <mmsinc:BoundField DataField="DateRetired" HeaderText="Date Retired :" 
                    SqlDataType="DateTime" SortExpression="DateRetired" />
                <asp:BoundField DataField="MapPage" HeaderText="Map Page :" 
                    SortExpression="MapPage" />
                <mmsinc:LatLonPickerField DataField="CoordinateID" />
                <asp:TemplateField HeaderText="Status :">
                    <ItemTemplate><%#Eval("StatusText") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList runat="server" ID="ddlStatus" DataSourceID="dsStatus"
                            DataTextField="Description" DataValueField="AssetStatusID"
                            SelectedValue='<%#Bind("AssetStatusID") %>'
                            AppendDataBoundItems="true"
                        >
                            <asp:ListItem Text="--Select Here--" Value=""/>
                        </asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="dsStatus" 
                            ConnectionString="<%$ ConnectionStrings:MCProd %>"
                            SelectCommand="Select AssetStatusID, Description from AssetStatuses order by 2"/>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Created By :" DataField="CreatedBy" InsertVisible="false" ReadOnly="true" SortExpression="CreatedBy" />
                <asp:BoundField HeaderText="Created On :" DataField="CreatedOn" InsertVisible="false" ReadOnly="true" SortExpression="CreatedOn" />
                <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                        <asp:LinkButton ID="btnUpdate" runat="server" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton>
                        <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:LinkButton ID="btnInsert" runat="server" CausesValidation="True" CommandName="Insert" Text="Insert"></asp:LinkButton>
                        <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="false" CommandName="Cancel" Text="Cancel" OnClick="btnCancelInsert_Click"></asp:LinkButton>
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="False" Visible="false" CommandName="Edit" Text="Edit"></asp:LinkButton>
                        <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False" Visible="false" CommandName="Delete" Text="Delete" OnClientClick="return confirm('Are you sure?');"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Fields>
        </asp:DetailsView>
        <asp:SqlDataSource ID="dsStormWaterAsset" runat="server" 
            ConnectionString="<%$ ConnectionStrings:MCProd %>" 
            OnInserted="SqlDataSource1_Inserted"
            DeleteCommand="DELETE FROM [StormWaterAssets] WHERE [StormWaterAssetID] = @StormWaterAssetID" 
            InsertCommand="INSERT INTO [StormWaterAssets] ([OperatingCenterID], [TownID], [AssetNumber], [StreetID], [IntersectingStreetID], [TaskNumber], [DateInstalled], [DateRetired], [MapPage], [CoordinateID], [AssetStatusID], CreatedBy, StormWaterAssetTypeID) VALUES (@OperatingCenterID, @TownID, @AssetNumber, @StreetID, @IntersectingStreetID, @TaskNumber, @DateInstalled, @DateRetired, @MapPage, @CoordinateID, @AssetStatusID, @CreatedBy, @StormWaterAssetTypeID);SELECT @StormWaterAssetID = (Select @@IDENTITY)" 
            SelectCommand="
SELECT 
    sm.*,
    O.OperatingCenterCode,
    T.Town,
    S.FullStName,
    CS.FullStName as FullStName2,
	swat.Description as AssetType,
	#as.Description as StatusText
FROM 
    [StormWaterAssets] sm
                LEFT JOIN [StormWaterAssetTypes] swat  ON sm.StormWaterAssetTypeID = swat.StormWaterAssetTypeID 
                LEFT JOIN [AssetStatuses] as #as       ON sm.AssetStatusID = #as.AssetSTatusID
                LEFT JOIN [OperatingCenters] O         ON O.OperatingCenterID = sm.OperatingCenterID
                LEFT JOIN [Towns] T                    ON T.TownID = sm.TownID
                LEFT JOIN [Streets] S                  ON S.StreetID = sm.StreetID
                LEFT JOIN [Streets] CS                 ON CS.StreetID = sm.IntersectingStreetID                    
                WHERE
                    StormWaterAssetID=@StormWaterAssetID
            " 
            UpdateCommand="UPDATE [StormWaterAssets] SET [OperatingCenterID] = @OperatingCenterID, [TownID] = @TownID, [AssetNumber] = @AssetNumber, [StreetID] = @StreetID, [IntersectingStreetID] = @IntersectingStreetID, [TaskNumber] = @TaskNumber, [DateInstalled] = @DateInstalled, [DateRetired] = @DateRetired, [MapPage] = @MapPage, [CoordinateID] = @CoordinateID, [AssetStatusID] = @AssetStatusID, [StormWaterAssetTypeID] = @StormWaterAssetTypeID WHERE [StormWaterAssetID] = @StormWaterAssetID">
            <SelectParameters>
                <asp:Parameter Name="StormWaterAssetID" Type="Int32" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="StormWaterAssetID" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="OperatingCenterID" Type="Int32" />
                <asp:Parameter Name="TownID" Type="Int32" />
                <asp:Parameter Name="AssetNumber" Type="String" />
                <asp:Parameter Name="StreetID" Type="Int32" />
                <asp:Parameter Name="IntersectingStreetID" Type="Int32" />
                <asp:Parameter Name="TaskNumber" Type="String" />
                <asp:Parameter Name="DateInstalled" Type="DateTime" />
                <asp:Parameter Name="DateRetired" Type="DateTime" />
                <asp:Parameter Name="MapPage" Type="String" />
                <asp:Parameter Name="CoordinateID" Type="Int32" />
                <asp:Parameter Name="AssetStatusID" Type="Int32" />
                <asp:Parameter Name="StormWaterAssetID" Type="Int32" />
                <asp:Parameter Name="StormWaterAssetTypeID" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="StormWaterAssetID" Type="Int32" Direction="Output" />
                <asp:Parameter Name="OperatingCenterID" Type="Int32" />
                <asp:Parameter Name="TownID" Type="Int32" />
                <asp:Parameter Name="AssetNumber" Type="String" />
                <asp:Parameter Name="StreetID" Type="Int32" />
                <asp:Parameter Name="IntersectingStreetID" Type="Int32" />
                <asp:Parameter Name="TaskNumber" Type="String" />
                <asp:Parameter Name="DateInstalled" Type="DateTime" />
                <asp:Parameter Name="DateRetired" Type="DateTime" />
                <asp:Parameter Name="MapPage" Type="String" />
                <asp:Parameter Name="CoordinateID" Type="Int32" />
                <asp:Parameter Name="AssetStatusID" Type="Int32" />
                <asp:Parameter Name="CreatedBy" Type="String" />
                <asp:Parameter Name="StormWaterAssetTypeID" Type="Int32" />
            </InsertParameters>
        </asp:SqlDataSource>
        <mmsi:Notes ID="Notes1" runat="server" DataTypeID="96" />
        <mmsi:Documents ID="Documents1" runat="server" DataTypeID="96" />
        <center>
            <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
    </asp:Panel>
    
</asp:Content>
