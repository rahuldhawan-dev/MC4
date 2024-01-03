<%@ Page Title="Meter Routes" Theme="Bender" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="MeterRoutes.aspx.cs" Inherits="MapCall.Modules.Customer.MeterRoutes" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>
<%@ Register Assembly="MMSINC.Core.WebForms" Namespace="MMSINC.DataPages" TagPrefix="mmsi" %>
<%@ Register Assembly="MMSINC.Core.WebForms" Namespace="MMSINC.Controls" TagPrefix="mmsinc" %>
<%@ Register Src="~/Controls/ddlMcProdOperatingCenter.ascx" TagName="ddlMcProdOperatingCenter" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/OpCntrDataField.ascx" TagPrefix="mmsi" TagName="OpCntrDataField" %>
<%@ Register Src="~/Controls/DetailsViewDataPageTemplate.ascx" TagPrefix="mapcall" TagName="DetailsViewDataPageTemplate" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls" TagPrefix="mapcall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.SearchFields" TagPrefix="search" %>
<%@ Register assembly="MMSINC" namespace="MMSINC" tagPrefix="mmsinc" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
Meter Routes
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">

<mapcall:DetailsViewDataPageTemplate ID="template" runat="server"
    DataElementTableName="MeterRoutes" DataTypeId="142"
    DataElementPrimaryFieldName="MeterRouteID"
    Label="Meter Route" ShowMapButton="true">
    <SearchBox runat="server">
            <Fields>
                <search:TemplatedSearchField FilterMode="Manual">
                    <Template>
                        <mmsi:OpCntrDataField ID="opCntrField" runat="server" DataFieldName="oc.OperatingCenterID" UseTowns="True" TownDataFieldName="mr.TownID" />
                    </Template>
                </search:TemplatedSearchField>
                <search:TextSearchField Label="Route # :" DataFieldName="RouteNumber" />
                <search:NumericSearchField DataFieldName="ReadDay" SearchType="Range" />
            </Fields>
        </SearchBox>
    
    <ResultsGridView DataKeyNames="MeterRouteID" AutoGenerateColumns="true" />

    <ResultsDataSource SelectCommand="
        SELECT 
	        mr.MeterRouteID, 
	        oc.OperatingCenterCode,
	        mr.Active,
	        mr.RouteNumber,
	        mr.ReadDay,
	        tw.Town,
	        mr.TownID,
	        mr.CoordinateID,
	        c.Latitude,
	        c.Longitude,
	        mr.Radius,
	        mr.Comments,
	        mr.CreatedOn,
	        mr.CreatedBy
        FROM 
	        [MeterRoutes] mr
        LEFT JOIN
	        OperatingCenters oc
        ON
	        oc.OperatingCenterID = mr.OperatingCenterID
        LEFT JOIN
	        [Towns] tw
        ON
	        tw.TownID = mr.TownID
        LEFT JOIN
	        [Coordinates] c
        ON
	        c.CoordinateID = mr.CoordinateID
        " />

    <DetailsView AutoGenerateRows="false" DataKeyNames="MeterRouteID">
        <Fields>
            <asp:BoundField DataField="MeterRouteID" HeaderText="MeterRouteID" 
                InsertVisible="False" ReadOnly="True" SortExpression="MeterRouteID" />
            <asp:TemplateField HeaderText="Operating Center">
                <ItemTemplate>
                    <%# Eval("OperatingCenterCode")%>
                </ItemTemplate>
                <EditItemTemplate>
                    <mmsi:ddlMcProdOperatingCenter runat="server" id="ddlOpCntr" SelectedValue='<%#Bind("OperatingCenterID") %>' Required="true" />
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:CheckBoxField DataField="Active" HeaderText="Active" 
                SortExpression="Active" />
            <asp:BoundField DataField="RouteNumber" HeaderText="Route #" 
                SortExpression="RouteNumber" />
            <mmsinc:BoundField SqlDataType="Int" DataField="ReadDay" HeaderText="Read Day" 
                SortExpression="ReadDay" />
            <asp:TemplateField HeaderText="Town">
                <ItemTemplate><%# Eval("Town") %></ItemTemplate>
                <EditItemTemplate>
                    <%--This will be a cascading drop-down filtered by opcode --%>
                    <mmsinc:MvpDropDownList runat="server" ID="ddlTown" />
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
            <mmsinc:LatLonPickerField DataField="CoordinateID" DefaultIconID="8" />
            <asp:BoundField DataField="Radius" HeaderText="Radius" 
                SortExpression="Radius" />
            <mmsinc:BoundField SqlDataType="Text" DataField="Comments" HeaderText="Comments" 
                SortExpression="Comments" />
            <asp:BoundField DataField="CreatedOn" InsertVisible="false" ReadOnly="True" HeaderText="Created On" 
                SortExpression="CreatedOn" />
            <asp:BoundField DataField="CreatedBy" InsertVisible="false" ReadOnly="True" HeaderText="Created By" 
                SortExpression="CreatedBy" />
        </Fields>
    </DetailsView>

    <DetailsDataSource
        DeleteCommand="DELETE FROM [MeterRoutes] WHERE [MeterRouteID] = @MeterRouteID" 
        InsertCommand="INSERT INTO [MeterRoutes] ([OperatingCenterID], [Active], [RouteNumber], [ReadDay], [TownID], [CoordinateID], [Radius], [Comments], [CreatedOn], [CreatedBy]) VALUES (@OperatingCenterID, @Active, @RouteNumber, @ReadDay, @TownID, @CoordinateID, @Radius, @Comments, @CreatedOn, @CreatedBy);SELECT @MeterRouteID = @@IDENTITY;" 
        SelectCommand="
                    SELECT 
	                    mr.MeterRouteID, 
	                    oc.OperatingCenterCode,
	                    mr.OperatingCenterID,
	                    mr.Active,
	                    mr.RouteNumber,
	                    mr.ReadDay,
	                    tw.Town,
	                    mr.TownID,
	                    mr.CoordinateID,
	                    c.Latitude,
	                    c.Longitude,
	                    mr.Radius,
	                    mr.Comments,
	                    mr.CreatedOn,
	                    mr.CreatedBy
                    FROM 
	                    [MeterRoutes] mr
                    LEFT JOIN
	                    OperatingCenters oc
                    ON
	                    oc.OperatingCenterID = mr.OperatingCenterID
                    LEFT JOIN
	                    [Towns] tw
                    ON
	                    tw.TownID = mr.TownID
                    LEFT JOIN
	                    [Coordinates] c
                    ON
	                    c.CoordinateID = mr.CoordinateID
                    WHERE 
                        MeterRouteID = @MeterRouteID
                " 
        UpdateCommand="UPDATE [MeterRoutes] SET [OperatingCenterID] = @OperatingCenterID, [Active] = @Active, [RouteNumber] = @RouteNumber, [ReadDay] = @ReadDay, [TownID] = @TownID, [CoordinateID] = @CoordinateID, [Radius] = @Radius, [Comments] = @Comments, [CreatedOn] = @CreatedOn, [CreatedBy] = @CreatedBy WHERE [MeterRouteID] = @MeterRouteID">
        <SelectParameters>
            <asp:Parameter Name="MeterRouteID" Type="Int32" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="MeterRouteID" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="OperatingCenterID" Type="Int32" />
            <asp:Parameter Name="Active" Type="Boolean" />
            <asp:Parameter Name="RouteNumber" Type="Int32" />
            <asp:Parameter Name="ReadDay" Type="Int32" />
            <asp:Parameter Name="TownID" Type="Int32" />
            <asp:Parameter Name="CoordinateID" Type="Int32" />
            <asp:Parameter Name="Radius" Type="Double" />
            <asp:Parameter Name="Comments" Type="String" />
            <asp:Parameter Name="CreatedOn" Type="DateTime" />
            <asp:Parameter Name="CreatedBy" Type="String" />
            <asp:Parameter Name="MeterRouteID" Type="Int32" Direction="Output" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="OperatingCenterID" Type="Int32" />
            <asp:Parameter Name="Active" Type="Boolean" />
            <asp:Parameter Name="RouteNumber" Type="Int32" />
            <asp:Parameter Name="ReadDay" Type="Int32" />
            <asp:Parameter Name="TownID" Type="Int32" />
            <asp:Parameter Name="CoordinateID" Type="Int32" />
            <asp:Parameter Name="Radius" Type="Double" />
            <asp:Parameter Name="Comments" Type="String" />
            <asp:Parameter Name="CreatedOn" Type="DateTime" />
            <asp:Parameter Name="CreatedBy" Type="String" />
            <asp:Parameter Name="MeterRouteID" Type="Int32" />
        </UpdateParameters>
    </DetailsDataSource>

    <Tabs>
        <mapcall:Tab runat="server" ID="meterroutedetails" Label="Meter Route Details">
            <mmsinc:MvpGridView ID="gvMeterRouteDetails" runat="server" EnableViewState="false" 
                DataSourceID="dsMeterRouteDetails" UseAccessibleHeader="true" AllowSorting="true"
                DataKeyNames="MeterRouteDetailID" AutoGenerateColumns="false" CssClass="prettyTable"
                EmptyDataText="There are no records to display">
                <Columns>
                    <asp:TemplateField >
                        <ItemTemplate>
                            <mmsi:DataPageViewRecordLink runat="server" LinkText="View" 
                                LinkUrl="~/Modules/Customer/MeterRouteDetails.aspx" 
                                DataRecordId='<%# Eval("MeterRouteDetailID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="MeterRouteDetailID" DataField="MeterRouteDetailID" />
                    <asp:BoundField HeaderText="Number Accounts" DataField="NumberAccounts" />
                    <asp:BoundField HeaderText="Estimated Read Time" DataField="EstimatedReadTime" />
                    <asp:BoundField HeaderText="Percent AMR" DataField="PercentAMR" />
                    <asp:BoundField HeaderText="Comments" DataField="Comments" />
                    <asp:BoundField HeaderText="Date Updated" DataField="DateUpdated" />
                    <asp:BoundField HeaderText="Created On" DataField="CreatedOn" />
                    <asp:BoundField HeaderText="Created By" DataField="CreatedBy" />
                </Columns>
            </mmsinc:MvpGridView>

            <mmsinc:MvpButton runat="server" ID="btnAddMeterRouteDetail"
                Text="New Meter Route Detail" OnClick="btnAddMeterRouteDetail_Click" />
    
            <asp:SqlDataSource ID="dsMeterRouteDetails" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="
                    SELECT 
	                    mrd.MeterRouteDetailID,
	                    mrd.NumberAccounts,
	                    mrd.EstimatedReadTime,
	                    mrd.PercentAMR,
	                    mrd.Comments,
	                    mrd.DateUpdated, 
	                    mrd.CreatedOn,
	                    mrd.CreatedBy
                    FROM 
	                    [MeterRouteDetails] mrd
                    INNER JOIN
	                    [MeterRoutes] mr
                    ON
	                    mr.MeterRouteID = mrd.MeterRouteID	 
                    LEFT JOIN
	                    OperatingCenters oc
                    ON
	                    oc.OperatingCenterID = mr.OperatingCenterID
                    LEFT JOIN
	                    [Towns] tw
                    ON
	                    tw.TownID = mr.TownID
                    WHERE
                        mrd.MeterRouteID = @MeterRouteID
                ">
                <SelectParameters>
                    <asp:ControlParameter Name="MeterRouteID" ControlID="detailView" PropertyName="SelectedValue" ConvertEmptyStringToNull="true" />
                </SelectParameters>
            </asp:SqlDataSource>
        </mapcall:Tab>

        <mapcall:Tab runat="server" id="flushingschedules" Label="Flushing Schedules">
                <mmsinc:MvpGridView runat="server" ID="gvFlushingSchedulesMeterRoutes"
                    DataSourceID="dsFlushingSchedulesMeterRoutes"
                    EmptyDataText="No Meter Routes are linked to this Flushing Schedule."
                    AutoGenerateColumns="false"
                    DataKeyNames="FlushingScheduleID,MeterRouteID">
                    <Columns>
                        <asp:TemplateField HeaderText="FlushingScheduleID">
                            <ItemTemplate>
                                <mmsi:DataPageViewRecordLink ID="DataPageViewRecordLink1" runat="server" 
                                    LinkText='<%# Eval("FlushingScheduleID") %>'
                                    LinkUrl="~/Modules/FieldServices/FlushingSchedules.aspx" 
                                    DataRecordId='<%# Eval("FlushingScheduleID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <mmsinc:BoundField HeaderText="Flushing Schedule Radius(miles)" DataField="Radius" />
                        <mmsinc:BoundField HeaderText="Meter Route Radius(miles)" DataField="RadiusMR" />
                        <mmsinc:BoundField HeaderText="Distance(miles)" DataField="Distance" />
                    </Columns>    
                </mmsinc:MvpGridView>
                <asp:Button runat="server" ID="btnExportMeterRoutes" OnClick="btnExportMeterRoutes_OnClick" Text="Export" />
               
                <asp:SqlDataSource runat="server" id="dsFlushingSchedulesMeterRoutes" 
                    ConnectionString="<%$ ConnectionStrings:MCProd %>" 
                    SelectCommand="
                        /* this uses haversine formula(dbo.distance) to get the distance between the two points */
                        /* if the sum of the radii is greater than the distance the circles touch */
                        select
                            fs.FlushingScheduleID,
	                        fs.Radius / 5280 as Radius,
                            mr.Radius / 5280 as RadiusMR,
                            mr.MeterRouteID, 
                            dbo.distance(
		                        c.latitude, 
		                        c.longitude, 
		                        c2.latitude, 
		                        c2.longitude
	                        ) as Distance
                        from
	                        FlushingSchedules fs
                        inner join
	                        coordinates c
                        on 
	                        fs.coordinateID = c.CoordinateID
                        join 
	                        meterroutes mr
                        on 
	                        1=1
                        inner join
	                        coordinates c2
                        on
	                        c2.coordinateID = mr.coordinateID
                        where
	                        mr.meterRouteID = @meterRouteID
                        and
	                        ((mr.radius + fs.radius) / 5280) 
	                        &gt; 
	                        dbo.distance(
		                        c.latitude, 
		                        c.longitude, 
		                        c2.latitude, 
		                        c2.longitude
	                        )
                        ">
                    <SelectParameters>
                        <asp:ControlParameter Name="MeterRouteID" ControlID="detailView" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
        </mapcall:Tab>
    </Tabs>
        
</mapcall:DetailsViewDataPageTemplate>
</asp:Content>


