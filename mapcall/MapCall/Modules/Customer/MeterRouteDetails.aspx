<%@ Page Title="Meter Route Details" Theme="bender" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="MeterRouteDetails.aspx.cs" Inherits="MapCall.Modules.Customer.MeterRouteDetails" %>

<%@ Register Assembly="MMSINC.Core.WebForms" Namespace="MMSINC.Controls" TagPrefix="mmsi" %>
<%@ Register Assembly="MMSINC.Core.WebForms" Namespace="MMSINC.DataPages" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/OpCntrDataField.ascx" TagName="OpCntrDataField" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/DetailsViewDataPageTemplate.ascx" TagPrefix="mapcall" TagName="DetailsViewDataPageTemplate" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.SearchFields" TagPrefix="search" %>
<%@ Register assembly="MMSINC" namespace="MMSINC" tagPrefix="mmsinc" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
Meter Route Details
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">


<mapcall:DetailsViewDataPageTemplate ID="template" runat="server" 
    DataElementTableName="MeterRouteDetails" DataTypeId="143"
    DataElementPrimaryFieldName="MeterRouteDetailID"
    Label="Meter Route">
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

        <ResultsGridView AutoGenerateColumns="true" />

        <ResultsDataSource SelectCommand="
            SELECT 
	            mrd.MeterRouteDetailID,
	            mrd.NumberAccounts,
	            mrd.EstimatedReadTime,
	            mrd.PercentAMR,
	            mrd.Comments,
	            mrd.DateUpdated, 
	            oc.OperatingCenterCode,
	            mr.OperatingCenterID,
	            mr.RouteNumber,
	            tw.Town,
	            mr.TownID,
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

                    " />

       <DetailsView>
           <Fields>
                <asp:BoundField DataField="MeterRouteDetailID" HeaderText="MeterRouteDetailID" 
                    InsertVisible="False" ReadOnly="True" SortExpression="MeterRouteDetailID" />
                <asp:TemplateField HeaderText="Meter Route">
                    <ItemTemplate>
                    <mmsi:DataPageViewRecordLink runat="server" ID="ok"
                                LinkUrl="~/Modules/Customer/MeterRoutes.aspx" 
                                DataRecordId='<%# Bind("MeterRouteID") %>' 
                                LinkText='<%# Eval("RouteNumber") %>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <mmsi:MvpDropDownList runat="server" id="ddlMeterRoutes"
                            DataSourceID="dsMeterRoutes"
                            AppendDataBoundItems="true"
                            DataTextField="txt" DataValueField="val"
                            SelectedValue='<%# Bind("MeterRouteID")%>'
                        >
                            <asp:ListItem Text="--Select Here--" Value="" />
                        </mmsi:MvpDropDownList>
                        <asp:SqlDataSource runat="server" id="dsMeterRoutes"
                            ConnectionString="<%$ ConnectionStrings:MCProd %>" 
                            SelectCommand="
                                Select MeterRouteID as val,
	                                (select OperatingCenterCode from OperatingCenters where OperatingCenters.OperatingCenterID = MeterRoutes.OperatingCenterID) + ', ' +
	                                cast(RouteNumber as varchar) as txt 
                                from 
	                                MeterRoutes 
                                order by
	                                (select OperatingCenterCode from OperatingCenters where OperatingCenters.OperatingCenterID = MeterRoutes.OperatingCenterID), RouteNumber
                            "/>
                    </EditItemTemplate>
                </asp:TemplateField>
                <mmsinc:BoundField SqlDataType="Int" DataField="NumberAccounts" HeaderText="Number Accounts" 
                    SortExpression="NumberAccounts" />
                <asp:BoundField DataField="EstimatedReadTime" HeaderText="Estimated Read Time" 
                    SortExpression="EstimatedReadTime" />
                <mmsinc:BoundField SqlDataType="Float" DataField="PercentAMR" HeaderText="Percent AMR" 
                    SortExpression="PercentAMR" />
                <mmsinc:BoundField SqlDataType="Text" DataField="Comments" HeaderText="Comments" 
                    SortExpression="Comments" />
                <mmsinc:BoundField SqlDataType="DateTime2" DataField="DateUpdated" HeaderText="Date Updated" 
                    SortExpression="DateUpdated" />
                <asp:BoundField DataField="CreatedOn" HeaderText="CreatedOn" 
                    SortExpression="CreatedOn" InsertVisible="False" ReadOnly="True" />
                <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" 
                    SortExpression="CreatedBy" InsertVisible="False" ReadOnly="True" />
            </Fields>
       </DetailsView>

       <DetailsDataSource DeleteCommand="DELETE FROM [MeterRouteDetails] WHERE [MeterRouteDetailID] = @MeterRouteDetailID" 
                InsertCommand="INSERT INTO [MeterRouteDetails] ([MeterRouteID], [NumberAccounts], [EstimatedReadTime], [PercentAMR], [Comments], [DateUpdated], [CreatedOn], [CreatedBy]) VALUES (@MeterRouteID, @NumberAccounts, @EstimatedReadTime, @PercentAMR, @Comments, @DateUpdated, @CreatedOn, @CreatedBy);SELECT @MeterRouteDetailID= @@IDENTITY;" 
                SelectCommand="
                    SELECT 
	                    mrd.*, 
	                    mr.*
                    FROM 
	                    [MeterRouteDetails] mrd
                    INNER JOIN
	                    [MeterRoutes] mr
                    ON
                        mr.MeterRouteID = mrd.MeterRouteID
                    WHERE 
                        MeterRouteDetailID = @MeterRouteDetailID
                " 
                UpdateCommand="UPDATE [MeterRouteDetails] SET [MeterRouteID] = @MeterRouteID, [NumberAccounts] = @NumberAccounts, [EstimatedReadTime] = @EstimatedReadTime, [PercentAMR] = @PercentAMR, [Comments] = @Comments, [DateUpdated] = @DateUpdated, [CreatedOn] = @CreatedOn, [CreatedBy] = @CreatedBy WHERE [MeterRouteDetailID] = @MeterRouteDetailID">
                <SelectParameters>
                    <asp:Parameter Name="MeterRouteDetailID" Type="Int32" />
                </SelectParameters>
                <DeleteParameters>
                    <asp:Parameter Name="MeterRouteDetailID" Type="Int32" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:Parameter Name="MeterRouteID" Type="Int32" />
                    <asp:Parameter Name="NumberAccounts" Type="Int32" />
                    <asp:Parameter Name="EstimatedReadTime" Type="String" />
                    <asp:Parameter Name="PercentAMR" Type="Double" />
                    <asp:Parameter Name="Comments" Type="String" />
                    <asp:Parameter Name="DateUpdated" Type="DateTime" />
                    <asp:Parameter Name="CreatedOn" Type="DateTime" />
                    <asp:Parameter Name="CreatedBy" Type="String" />
                    <asp:Parameter Name="MeterRouteDetailID" Type="Int32" Direction="Output" />
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="MeterRouteID" Type="Int32" />
                    <asp:Parameter Name="NumberAccounts" Type="Int32" />
                    <asp:Parameter Name="EstimatedReadTime" Type="String" />
                    <asp:Parameter Name="PercentAMR" Type="Double" />
                    <asp:Parameter Name="Comments" Type="String" />
                    <asp:Parameter Name="DateUpdated" Type="DateTime" />
                    <asp:Parameter Name="CreatedOn" Type="DateTime" />
                    <asp:Parameter Name="CreatedBy" Type="String" />
                    <asp:Parameter Name="MeterRouteDetailID" Type="Int32" />
                </UpdateParameters>
       </DetailsDataSource>
    </mapcall:DetailsViewDataPageTemplate>

</asp:Content>
