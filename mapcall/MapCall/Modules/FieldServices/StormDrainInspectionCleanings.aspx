<%@ Page Title="Storm Drain Inspections and Cleanings" Language="C#" Theme="bender"  MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="StormDrainInspectionCleanings.aspx.cs" Inherits="MapCall.Modules.FieldServices.StormDrainInspectionCleanings" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls" TagPrefix="mapcall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.DropDowns" TagPrefix="mapcall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.SearchFields" TagPrefix="search" %>
<%@ Register Assembly="MMSINC.Core.WebForms" Namespace="MMSINC.Controls" TagPrefix="mmsi" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>
<%@ Register Src="~/Controls/DetailsViewDataPageTemplate.ascx" TagPrefix="mapcall" TagName="DetailsViewDataPageTemplate" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
Storm Drain Inspections and Cleanings
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">
<mapcall:DetailsViewDataPageTemplate ID="template" runat="server" 
    DefaultPageMode="Home"
    DataElementTableName="swac.StormWaterAssetCleanings"
    DataElementPrimaryFieldName="StormWaterAssetCleaningID"
    DataTypeId="145"
    Label="Storm Drain Cleaning">
    <SearchBox>
        <Fields>
            <search:DropDownSearchField 
                Label="Asset Type"
                DataFieldName="asset.StormWaterAssetTypeID"
                TableName="StormWaterAssetTypes"
                TextFieldName="Description"
                ValueFieldName="StormWaterAssetTypeID" />
             <search:TemplatedSearchField 
                Label="Asset Number"
                DataFieldName="asset.StormWaterAssetID"
                BindingControlID="searchStormWaterAssetSearch"
                BindingDataType="Int32"
                BindingPropertyName="SelectedValue">
                <Template>
                    <mmsi:MvpDropDownList runat="server" ID="searchStormWaterAssetSearch" />
                    <atk:CascadingDropDown runat="server" ID="cddStormWaterAssetsSearch" 
                        TargetControlID="searchStormWaterAssetSearch" 
                        ParentControlID="searchStormWaterAssetTypeID" 
                        Category="StormWaterAsset" 
                        EmptyText="No Assets Found" EmptyValue=""
                        PromptText="-- Select Asset Number --" PromptValue="" 
                        LoadingText="[Loading Assets...]"
                        ServicePath="~/Modules/Data/StormWater/StormWaterAssets.asmx" 
                        ServiceMethod="GetStormWaterAssetsByStormWaterAssetType" 
                        />
                </Template>
             </search:TemplatedSearchField>
             <search:DateTimeSearchField DataFieldName="InspectionDate" />
             <search:BooleanSearchField DataFieldName="NeedsVacuuming" SearchType="DropDownList" />
             <search:DateTimeSearchField DataFieldName="DateVacuumed" />
             <search:BooleanSearchField DataFieldName="IsTurtleMarkerPresent" SearchType="DropDownList" />
             <search:TextSearchField DataFieldName="MainCleaningFootageNumber" Label="Footage of Main Cleaned" />
             <search:DateTimeSearchField DataFieldName="DateMainCleaned" />
        </Fields>
    </SearchBox>

    <ResultsGridView>
        <Columns>
            <mapcall:BoundField DataField="AssetNumber" />
            <mapcall:BoundField DataField="AssetTypeDescription" HeaderText="Asset Type" />
            <mapcall:BoundField DataField="OperatingCenterText" HeaderText="Operating Center" />
            <mapcall:BoundField DataField="CityText" HeaderText="City" />
            <mapcall:BoundField DataField="StreetText" HeaderText="Street" />
            <mapcall:BoundField DataField="IntersectingStreetText" HeaderText="Interesecting Street" />
            <mapcall:BoundField DataField="InspectionDate" DataType="Date" />
            <mapcall:BoundField DataField="NeedsVacuuming" DataType="Bit" />
            <mapcall:BoundField DataField="DateVacuumed" DataType="Date" />
            <mapcall:BoundField DataField="IsTurtleMarkerPresent" HeaderText="Is Turtle Marker Present?" DataType="Bit"  />
            <mapcall:BoundField DataField="MainCleaningFootageNumber" HeaderText="Footage of Main Cleaned" />
            <mapcall:BoundField DataField="DateMainCleaned" DataType="Date" />
            <mapcall:BoundField DataField="CreatedBy" />
            <mapcall:BoundField DataField="CreatedOn" DataType="Date" />
        </Columns>
    </ResultsGridView>

    <ResultsDataSource SelectCommand="SELECT 
                            swac.*,
                            asset.AssetNumber,
                            atype.Description as AssetTypeDescription,
                            atype.StormWaterAssetTypeID,
                            njtown.State as StateText,
                            njtown.Town as CityText,
                            njstreet.FullStName as StreetText,
                            njstreetintersect.FullStName as IntersectingStreetText,
                            ops.OperatingCenterCode as OperatingCenterText
                       FROM 
                            [StormWaterAssetCleanings] swac 
                       LEFT JOIN [StormWaterAssets] asset       ON asset.StormWaterAssetID = swac.StormWaterAssetID
                       LEFT JOIN [StormWaterAssetTypes] atype   ON atype.StormWaterAssetTypeID = asset.StormWaterAssetTypeID
                       LEFT JOIN Towns njtown                   ON njtown.TownID = asset.TownID
                       LEFT JOIN Streets njstreet               ON njstreet.StreetID = asset.StreetID
                       LEFT JOIN Streets njstreetintersect      ON njstreetintersect.StreetID = asset.IntersectingStreetID
                       LEFT JOIN [OperatingCenters] ops         ON ops.OperatingCenterID = asset.OperatingCenterID">
        
    </ResultsDataSource>
    <DetailsView>
        <Fields>
        <%-- Why is this commented out? Because as of this writing(4/4/2011) every storm water asset 
             was in Avalon. This extra filtering would just waste everyone's time. However, the functionality
             works, so it just needs to be put back in, and the web service methods updated.   --%>
           <asp:TemplateField HeaderText="Operating Center">
                <ItemTemplate>
                    <%# Eval("OperatingCenterText") %>
                </ItemTemplate>
                <EditItemTemplate>
                     <mapcall:DataSourceDropDownList ID="ddlOpCenter" runat="server"
                        TableName="OperatingCenters"
                        TextFieldName="OperatingCenterCode"
                        ValueFieldName="OperatingCenterID"
                        SelectedValue='<%# Bind("OperatingCenterID") %>' />
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Town">
                <ItemTemplate>
                   <%# Eval("CityText")%>
                </ItemTemplate>
                <EditItemTemplate>
                   <mmsi:MvpDropDownList ID="ddlCity" runat="server" />
                   <atk:CascadingDropDown runat="server" ID="cddCity" 
                        TargetControlID="ddlCity" 
                        ParentControlID="ddlOpCenter" 
                        Category="Town" 
                        EmptyText="No Cities Found" EmptyValue=""
                        PromptText="-- Select City --" PromptValue="" 
                        LoadingText="[Loading Cities...]"
                        ServicePath="~/Modules/Data/DropDowns.asmx" 
                        ServiceMethod="GetTownsByOperatingCenter" SelectedValue='<%# Bind("CityID") %>'
                        />
                </EditItemTemplate>
            </asp:TemplateField>
              <asp:TemplateField HeaderText="Street">
                <ItemTemplate>
                    <%# Eval("StreetText")%>
                </ItemTemplate>
                <EditItemTemplate>
                    <mmsi:MvpDropDownList ID="ddlStreet" runat="server" />
                    <atk:CascadingDropDown runat="server" ID="cddStreet" 
                        TargetControlID="ddlStreet" 
                        ParentControlID="ddlCity" 
                        Category="Street" 
                        EmptyText="No Streets Found" EmptyValue=""
                        PromptText="-- Select Street --" PromptValue="" 
                        LoadingText="[Loading Streets...]"
                        ServicePath="~/Modules/Data/DropDowns.asmx" 
                        ServiceMethod="GetStreetsByTown" SelectedValue='<%# Bind("StreetID") %>'
                        />
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Intersecting Street">
                <ItemTemplate>
                    <%# Eval("IntersectingStreetText")%>
                </ItemTemplate>
                <EditItemTemplate>
                    <mmsi:MvpDropDownList ID="ddlInterStreet" runat="server" />
                    <atk:CascadingDropDown runat="server" ID="cddInterStreet" 
                        TargetControlID="ddlInterStreet" 
                        ParentControlID="ddlStreet" 
                        Category="IntersectingStreet" 
                        EmptyText="No Streets Found" EmptyValue=""
                        PromptText="-- Select Intersecting Street --" PromptValue="" 
                        LoadingText="[Loading Streets...]"
                        ServicePath="~/Modules/Data/DropDowns.asmx" 
                        ServiceMethod="GetStreetsByNameWithNoStreetOption" SelectedValue='<%# Bind("IntersectingStreetID") %>'
                        />
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Asset Type">
                <ItemTemplate>
                    <%# Eval("AssetTypeDescription") %>
                </ItemTemplate>
                <EditItemTemplate>
                   <mmsi:MvpDropDownList ID="ddlAssetTypeDescription" runat="server" />
                   <atk:CascadingDropDown runat="server" ID="cddStormWaterAssetTypes" 
                        TargetControlID="ddlAssetTypeDescription" 
                        ParentControlID="ddlInterStreet" 
                        Category="StormWaterAssetType" 
                        EmptyText="No Asset Types Found" EmptyValue=""
                        PromptText="-- Select Asset Type --" PromptValue="" 
                        LoadingText="[Loading Asset Types...]"
                        ServicePath="~/Modules/Data/StormWater/StormWaterAssets.asmx" 
                        ServiceMethod="GetStormWaterAssetTypes" SelectedValue='<%# Bind("StormWaterAssetTypeID") %>'
                        />
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Asset Number">
                <ItemTemplate>
                    <%# Eval("AssetNumber") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <mmsi:MvpDropDownList runat="server" ID="ddlStormWaterAssets" />
                    <atk:CascadingDropDown runat="server" ID="cddStormWaterAssets" 
                        TargetControlID="ddlStormWaterAssets" 
                        ParentControlID="ddlAssetTypeDescription" 
                        Category="StormWaterAsset" 
                        EmptyText="No Assets Found" EmptyValue=""
                        PromptText="-- Select Asset Number --" PromptValue="" 
                        LoadingText="[Loading Assets...]"
                        ServicePath="~/Modules/Data/StormWater/StormWaterAssets.asmx" 
                        ServiceMethod="GetStormWaterAssetsByStormWaterAssetTypeAndOtherStuff" SelectedValue='<%# Bind("StormWaterAssetID") %>'
                        />
                    <asp:RequiredFieldValidator ID="rfvAssets" runat="server" 
                        ControlToValidate="ddlStormWaterAssets" 
                        ClientIDMode="AutoID" 
                        InitialValue="" 
                        ErrorMessage="Required" />
                </EditItemTemplate>
            </asp:TemplateField>
            <mapcall:BoundField DataField="InspectionDate" HeaderText="Inspection Date" DataType="Date" />
            <mapcall:BoundField DataField="NeedsVacuuming" HeaderText="Needs Vacuuming" DataType="Bit"  />
            <mapcall:BoundField DataField="DateVacuumed" HeaderText="Date Vacuumed" DataType="Date" />
            <mapcall:BoundField DataField="IsTurtleMarkerPresent" HeaderText="Is Turtle Marker Present?" DataType="Bit" />
            <mapcall:BoundField DataField="MainCleaningFootageNumber" HeaderText="Footage of Main Cleaned" DataType="NVarChar" MaxLength="50" />
            <mapcall:BoundField DataField="DateMainCleaned" HeaderText="Date Main Cleaned" DataType="Date" />
            <mapcall:BoundField DataField="CreatedBy" HeaderText="Created By" DataType="NVarChar" 
               InsertVisible="false" ReadOnly="true" />
            <mapcall:BoundField DataField="CreatedOn" HeaderText="Created On" DataType="DateTime" 
               InsertVisible="false" ReadOnly="true"  />
        </Fields>
    </DetailsView>
    <DetailsDataSource DeleteCommand="DELETE FROM [StormWaterAssetCleanings] WHERE [StormWaterAssetCleaningID] = @StormWaterAssetCleaningID" 
        InsertCommand="INSERT INTO [StormWaterAssetCleanings] ([StormWaterAssetID], [InspectionDate], [NeedsVacuuming], [DateVacuumed], [IsTurtleMarkerPresent], [MainCleaningFootageNumber], [DateMainCleaned], [CreatedBy]) VALUES (@StormWaterAssetID, @InspectionDate, @NeedsVacuuming, @DateVacuumed, @IsTurtleMarkerPresent, @MainCleaningFootageNumber, @DateMainCleaned, @CreatedBy);  SELECT @StormWaterAssetCleaningID = (SELECT @@IDENTITY)" 
        UpdateCommand="UPDATE [StormWaterAssetCleanings] SET [StormWaterAssetID] = @StormWaterAssetID, [InspectionDate] = @InspectionDate, [NeedsVacuuming] = @NeedsVacuuming, [DateVacuumed] = @DateVacuumed, [IsTurtleMarkerPresent] = @IsTurtleMarkerPresent, [MainCleaningFootageNumber] = @MainCleaningFootageNumber, [DateMainCleaned] = @DateMainCleaned WHERE [StormWaterAssetCleaningID] = @StormWaterAssetCleaningID"
        SelectCommand="SELECT 
                            swac.*,
                            asset.AssetNumber,
                            asset.StreetID,
                            asset.IntersectingStreetID,
                            atype.Description as AssetTypeDescription,
                            atype.StormWaterAssetTypeID,
                            njtown.State as StateText,
                            njtown.Town as CityText,
                            njtown.TownID as CityID,
                            njstreet.FullStName as StreetText,
                            njstreetintersect.FullStName as IntersectingStreetText,
                            ops.OperatingCenterCode as OperatingCenterText,
                            ops.OperatingCenterID
                       FROM 
                            [StormWaterAssetCleanings] swac 
                       LEFT JOIN [StormWaterAssets] asset           ON asset.StormWaterAssetID = swac.StormWaterAssetID
                       LEFT JOIN [StormWaterAssetTypes] atype       ON atype.StormWaterAssetTypeID = asset.StormWaterAssetTypeID
                       LEFT JOIN Towns njtown                       ON njtown.TownID = asset.TownID
                       LEFT JOIN Streets njstreet                   ON njstreet.StreetID = asset.StreetID
                       LEFT JOIN Streets njstreetintersect          ON njstreetintersect.StreetID = asset.IntersectingStreetID
                       LEFT JOIN OperatingCenters ops               ON ops.OperatingCenterID = asset.OperatingCenterID
                       WHERE
                            ([StormWaterAssetCleaningID] = @StormWaterAssetCleaningID)" 
        >
        <DeleteParameters>
            <asp:Parameter Name="StormWaterAssetCleaningID" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="StormWaterAssetCleaningID" Type="Int32" Direction="Output"  />
            <asp:Parameter Name="StormWaterAssetID" Type="Int32" />
            <asp:Parameter Name="InspectionDate" Type="DateTime" />
            <asp:Parameter Name="NeedsVacuuming" Type="Boolean" />
            <asp:Parameter Name="DateVacuumed" Type="DateTime" />
            <asp:Parameter Name="IsTurtleMarkerPresent" Type="Boolean" />
            <asp:Parameter Name="MainCleaningFootageNumber" Type="String" />
            <asp:Parameter Name="DateMainCleaned" Type="DateTime" />
            <asp:Parameter Name="CreatedBy" Type="String" />
        </InsertParameters>
        <SelectParameters>
            <asp:Parameter Name="StormWaterAssetCleaningID" Type="Int32" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="StormWaterAssetID" Type="Int32" />
            <asp:Parameter Name="InspectionDate" Type="DateTime" />
            <asp:Parameter Name="NeedsVacuuming" Type="Boolean" />
            <asp:Parameter Name="DateVacuumed" Type="DateTime" />
            <asp:Parameter Name="IsTurtleMarkerPresent" Type="Boolean" />
            <asp:Parameter Name="MainCleaningFootageNumber" Type="String" />
            <asp:Parameter Name="DateMainCleaned" Type="DateTime" />
            <asp:Parameter Name="StormWaterAssetCleaningID" Type="Int32" />
        </UpdateParameters>
    </DetailsDataSource>
</mapcall:DetailsViewDataPageTemplate>
</asp:Content>
