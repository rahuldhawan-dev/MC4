<%@ Page Title="Flushing Schedules" Theme="bender" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="FlushingSchedules.aspx.cs" Inherits="MapCall.Modules.FieldServices.FlushingSchedules" %>
<%@ Register Assembly="MMSINC.Core.WebForms" Namespace="MMSINC.Controls" TagPrefix="mmsinc" %>
<%@ Register Assembly="MMSINC.Core.WebForms" Namespace="MMSINC.DataPages" TagPrefix="mmsinc" %>
<%@ Register  Src="~/Controls/DetailsViewDataPageTemplate.ascx" TagPrefix="mapcall" TagName="DetailsViewDataPageTemplate" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls" TagPrefix="mapcall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.DropDowns" TagPrefix="mapcall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.SearchFields" TagPrefix="search" %>
<%@ Register assembly="MMSINC" namespace="MMSINC" tagPrefix="mmsinc" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
Flushing Schedules
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">

<mapcall:DetailsViewDataPageTemplate ID="template" runat="server" 
    DataElementTableName="FlushingSchedules" 
    DataTypeId="128"
    DataElementPrimaryFieldName="FlushingScheduleID"
    Label="Flushing Schedule"
    ShowMapButton="true">
    <SearchBox runat="server">
        <Fields>
            <search:TemplatedSearchField 
                Label="Operating Center"
                DataFieldName="oc.OperatingCenterID"
                BindingControlID="searchOpCenter"
                BindingDataType="Int32"
                BindingPropertyName="SelectedValue">
                <Template>
                    <mapcall:OperatingCenterDropDownList ID="searchOpCenter" runat="server" />
                </Template>
            </search:TemplatedSearchField>
            <search:DropDownSearchField 
                Label="Town"
                DataFieldName="municipality"
                TableName="Towns" 
                TextFieldName="Town" 
                ValueFieldName="TownID" />
            <search:DropDownSearchField 
                Label="Town Section"
                DataFieldName="fs.TownSectionID"
                TableName="TownSections" 
                TextFieldName="Name" 
                ValueFieldName="TownSectionID" />
            <search:DateTimeSearchField DataFieldName="StartDate" />
            <search:DateTimeSearchField DataFieldName="EndDate" />
            <search:TextSearchField DataFieldName="MeterRoutes" Label="Call Group" />
        </Fields>
    </SearchBox>
    
    <ResultsGridView>
        <Columns>
            <asp:BoundField DataField="FlushingScheduleID" HeaderText="FlushingScheduleID" SortExpression="FlushingScheduleID" />
            <asp:BoundField DataField="OperatingCenterCode" HeaderText="OperatingCenterCode" SortExpression="OperatingCenterCode" />
            <asp:BoundField DataField="Municipality" HeaderText="Municipality" SortExpression="Municipality" />
            <asp:BoundField DataField="TownSection" HeaderText="Town Section" SortExpression="TownSection" />
            <asp:BoundField DataField="StartDate" HeaderText="Start Date" SortExpression="StartDate" />
            <asp:BoundField DataField="EndDate" HeaderText="End Date" SortExpression="EndDate" />
            <asp:BoundField DataField="Radius" HeaderText="Radius" SortExpression="Radius" />
            <asp:BoundField DataField="Supervisor" HeaderText="Supervisor" SortExpression="Supervisor" />
            <asp:CheckBoxField DataField="Complete" HeaderText="Complete" SortExpression="Complete" />
            <asp:BoundField DataField="MeterRoutes" HeaderText="Meter Routes" SortExpression="MeterRoutes" />
        </Columns>
    </ResultsGridView>
    <ResultsDataSource ConnectionString="<%$ ConnectionStrings:MCProd %>"
        SelectCommand="
            SELECT 
	            FlushingScheduleID, 
	            oc.OperatingCenterCode,
	            t.Town as Municipality,
                tsect.Name as TownSection, 
	            fs.StartDate,
	            fs.EndDate,
	            fs.Radius, 
	            e.FullName as [Supervisor], 
	            Complete, 
	            MeterRoutes, 
	            fs.*
            FROM 
	            FlushingSchedules fs
            LEFT JOIN
	            OperatingCenters oc
            ON
	            fs.OperatingCenterID = oc.OperatingCenterID
            LEFT JOIN
	            Towns t
            ON
	            fs.Municipality = t.TownID
            LEFT JOIN
                TownSections tsect
            ON 
                tsect.TownSectionID = fs.TownSectionID
            LEFT JOIN
	            Employees e
            ON
	            fs.SupervisorID = e.tblEmployeeID" />
        
        <DetailsView ID="detailView">
            <Fields>
                <asp:BoundField DataField="FlushingScheduleID" HeaderText="FlushingScheduleID" 
                    InsertVisible="False" ReadOnly="True" />
                <asp:TemplateField HeaderText="Operating Center">
                    <ItemTemplate><%# Eval("OperatingCenterCode") %></ItemTemplate>
                    <EditItemTemplate>
                        <mmsinc:MvpDropDownList runat="server" id="ddlOpCntr"
                            DataSourceID="dsOpCntr"
                            AppendDataBoundItems="true"
                            DataTextField="txt" DataValueField="val"
                            SelectedValue='<%# Bind("OperatingCenterID")%>'
                        >
                            <asp:ListItem Text="--Select Here--" Value="" />
                        </mmsinc:MvpDropDownList>
                        <asp:SqlDataSource runat="server" ID="dsOpCntr"
                            ConnectionString="<%$ ConnectionStrings:MCProd %>"
                            SelectCommand="select distinct operatingCenterCode + ' - ' + OperatingCenterName as txt, OperatingCenterID as val from OperatingCenters order by 1"
                        >
                        </asp:SqlDataSource>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Municipality">
                    <ItemTemplate><%# Eval("Town") %></ItemTemplate>
                    <EditItemTemplate>
                        <mmsinc:MvpDropDownList runat="server" id="ddlTowns"
                            DataSourceID="dsTowns"
                            AppendDataBoundItems="true"
                            DataTextField="txt" DataValueField="val"
                            SelectedValue='<%# Bind("Municipality")%>'
                        >
                            <asp:ListItem Text="--Select Here--" Value="" />
                        </mmsinc:MvpDropDownList>
                        <asp:SqlDataSource runat="server" ID="dsTowns"
                            ConnectionString="<%$ ConnectionStrings:MCProd %>"
                            SelectCommand="select TownID as val, Town as txt from Towns order by 2"
                        >
                        </asp:SqlDataSource>
                    </EditItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Town Section">
                    <ItemTemplate><%# Eval("TownSection") %></ItemTemplate>
                    <EditItemTemplate>
                        <mmsinc:MvpDropDownList runat="server" id="ddlTownSections"
                            DataSourceID="dsTownSections"
                            AppendDataBoundItems="true"
                            DataTextField="txt" DataValueField="val"
                            SelectedValue='<%# Bind("TownSectionID")%>'
                        >
                            <asp:ListItem Text="--Select Here--" Value="" />
                        </mmsinc:MvpDropDownList>
                        <asp:SqlDataSource runat="server" ID="dsTownSections"
                            ConnectionString="<%$ ConnectionStrings:MCProd %>"
                            SelectCommand="select TownSectionID as val, Name as txt from TownSections order by Name"
                        >
                        </asp:SqlDataSource>
                    </EditItemTemplate>
                </asp:TemplateField>

                <mmsinc:BoundField SqlDataType="DateTime2" DataField="StartDate" HeaderText="Start Date"  />
                <mmsinc:BoundField SqlDataType="DateTime2" DataField="EndDate" HeaderText="End Date" />
                <mmsinc:LatLonPickerField DataField="CoordinateID" />
                <asp:BoundField DataField="Radius" HeaderText="Radius" />
                <mmsinc:BoundField SqlDataType="Text" DataField="Comments" HeaderText="Comments"  />

                <asp:TemplateField HeaderText="Supervisor">
                    <ItemTemplate><%# Eval("FullName") %></ItemTemplate>
                    <InsertItemTemplate>
                        <mmsinc:MvpDropDownList runat="server" id="ddlEmployees"
                            DataSourceID="dsEmployees"
                            AppendDataBoundItems="true"
                            DataTextField="txt" DataValueField="val"
                            SelectedValue='<%# Bind("SupervisorID")%>'
                        >
                            <asp:ListItem Text="--Select Here--" Value="" />
                        </mmsinc:MvpDropDownList>
                        <asp:SqlDataSource runat="server" ID="dsEmployees"
                            ConnectionString="<%$ ConnectionStrings:MCProd %>"
                            SelectCommand="select tblEmployeeID as val, FullName as txt from ActiveEmployees order by Last_Name"
                        >
                        </asp:SqlDataSource>
                    </InsertItemTemplate>
                    <EditItemTemplate>
                        <mmsinc:MvpDropDownList runat="server" id="ddlEmployees"
                            DataSourceID="dsEmployees"
                            AppendDataBoundItems="true"
                            DataTextField="txt" DataValueField="val"
                            SelectedValue='<%# Bind("SupervisorID")%>'
                        >
                            <asp:ListItem Text="--Select Here--" Value="" />
                        </mmsinc:MvpDropDownList>
                        <asp:SqlDataSource runat="server" ID="dsEmployees"
                            ConnectionString="<%$ ConnectionStrings:MCProd %>"
                            SelectCommand="select tblEmployeeID as val, FullName as txt from Employees order by Last_Name"
                        >
                        </asp:SqlDataSource>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:CheckBoxField DataField="Complete" HeaderText="Complete" />
                <mmsinc:BoundField DataField="MeterRoutes" HeaderText="MeterRoutes" MaxLength="25" />
            </Fields>
        </DetailsView>

        <DetailsDataSource ConnectionString="<%$ ConnectionStrings:MCProd %>" 
            DeleteCommand="DELETE FROM [FlushingSchedules] WHERE [FlushingScheduleID] = @FlushingScheduleID" 
            InsertCommand="INSERT INTO [FlushingSchedules] ([OperatingCenterID], [Municipality], [StartDate], [EndDate], [CoordinateID], [Radius], [Comments], [SupervisorID], [Complete], [MeterRoutes], [TownSectionID]) VALUES (@OperatingCenterID, @Municipality, @StartDate, @EndDate, @CoordinateID, @Radius, @Comments, @SupervisorID, @Complete, @MeterRoutes, @TownSectionID);SELECT @FlushingScheduleID = @@IDENTITY;" 
            SelectCommand="
                SELECT 
	                    fs.*,
		                oc.OperatingCenterCode,
		                t.Town,
                        tsect.Name as TownSection,
		                e.FullName	
                FROM 
                    FlushingSchedules fs
                LEFT JOIN
                    OperatingCenters oc
                ON
                    fs.OperatingCenterID = oc.OperatingCenterID
                LEFT JOIN
                    Towns t
                ON
                    fs.Municipality = t.TownID
                LEFT JOIN
                    TownSections tsect
                ON 
                    tsect.TownSectionID = fs.TownSectionID
                LEFT JOIN
                    Employees e
                ON
                    fs.SupervisorID = e.tblEmployeeID
                WHERE 
                    [FlushingScheduleID] = @FlushingScheduleID"
            UpdateCommand="UPDATE [FlushingSchedules] SET [OperatingCenterID] = @OperatingCenterID, [Municipality] = @Municipality, [StartDate] = @StartDate, [EndDate] = @EndDate, [CoordinateID] = @CoordinateID, [Radius] = @Radius, [Comments] = @Comments, [SupervisorID] = @SupervisorID, [Complete] = @Complete, [MeterRoutes] = @MeterRoutes, [TownSectionID] = @TownSectionID WHERE [FlushingScheduleID] = @FlushingScheduleID">
            <SelectParameters>
                <asp:Parameter Name="FlushingScheduleID" Type="Int32" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="OperatingCenterID" Type="Int32" />
                <asp:Parameter Name="Municipality" Type="Int32" />
                <asp:Parameter Name="StartDate" Type="DateTime" />
                <asp:Parameter Name="EndDate" Type="DateTime" />
                <asp:Parameter Name="CoordinateID" Type="Int32" />
                <asp:Parameter Name="Radius" Type="Double" />
                <asp:Parameter Name="Comments" Type="String" />
                <asp:Parameter Name="SupervisorID" Type="Int32" />
                <asp:Parameter Name="Complete" Type="Boolean" />
                <asp:Parameter Name="MeterRoutes" Type="String" />
                <asp:Parameter Name="TownSectionID" Type="Int32" />
                <asp:Parameter Name="FlushingScheduleID" Type="Int32" Direction="Output" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="OperatingCenterID" Type="Int32" />
                <asp:Parameter Name="Municipality" Type="Int32" />
                <asp:Parameter Name="StartDate" Type="DateTime" />
                <asp:Parameter Name="EndDate" Type="DateTime" />
                <asp:Parameter Name="CoordinateID" Type="Int32" />
                <asp:Parameter Name="Radius" Type="Double" />
                <asp:Parameter Name="Comments" Type="String" />
                <asp:Parameter Name="SupervisorID" Type="Int32" />
                <asp:Parameter Name="Complete" Type="Boolean" />
                <asp:Parameter Name="MeterRoutes" Type="String" />
                <asp:Parameter Name="TownSectionID" Type="Int32" />
                <asp:Parameter Name="FlushingScheduleID" Type="Int32" />
            </UpdateParameters>
            <DeleteParameters>
                <asp:Parameter Name="FlushingScheduleID" Type="Int32" />
            </DeleteParameters>
        </DetailsDataSource>

        <Tabs>
            <mapcall:Tab runat="server" ID="routes" Label="Meter Routes">
                <mmsinc:MvpGridView runat="server" ID="gvFlushingSchedulesMeterRoutes"
                    DataSourceID="dsFlushingSchedulesMeterRoutes"
                    EmptyDataText="No Meter Routes are linked to this Flushing Schedule."
                    AutoGenerateColumns="false"
                    DataKeyNames="FlushingScheduleID,MeterRouteID">
                    <Columns>
                        <asp:TemplateField HeaderText="Meter Route Number">
                            <ItemTemplate>
                                <mmsinc:DataPageViewRecordLink ID="DataPageViewRecordLink1" runat="server" 
                                    LinkText='<%# Eval("RouteNumber") %>'
                                    LinkUrl="~/Modules/Customer/MeterRoutes.aspx" 
                                    DataRecordId='<%# Eval("MeterRouteID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <mmsinc:BoundField HeaderText="Meter Route Radius(miles)" DataField="Radius" />
                        <mmsinc:BoundField HeaderText="Flushging Schedule Radius(miles)" DataField="RadiusFS" />
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
                            mr.MeterRouteID,
	                        mr.RouteNumber, 
                            mr.Radius / 5280 as Radius,
                            fs.Radius / 5280 as RadiusFS,
                            fs.FlushingScheduleID, 
                            dbo.distance(
		                        c.latitude, 
		                        c.longitude, 
		                        c2.latitude, 
		                        c2.longitude
	                        ) as Distance
                        from
	                        meterRoutes mr
                        inner join
	                        coordinates c
                        on 
	                        mr.coordinateID = c.CoordinateID
                        join 
	                        flushingschedules fs
                        on 
	                        1=1
                        inner join
	                        coordinates c2
                        on
	                        c2.coordinateID = fs.coordinateID
                        where
	                        fs.flushingScheduleID = @flushingScheduleID
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
                        <asp:ControlParameter Name="FlushingScheduleID" ControlID="detailView" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </mapcall:Tab>
        </Tabs>
    </mapcall:DetailsViewDataPageTemplate>





</asp:Content>
