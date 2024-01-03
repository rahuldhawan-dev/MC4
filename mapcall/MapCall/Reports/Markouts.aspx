<%@ Page Title="Markouts" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="Markouts.aspx.cs" Inherits="MapCall.Reports.Markouts" %>
<%@ Register TagPrefix="mapcall" TagName="DetailsViewDataPageTemplate" Src="~/Controls/DetailsViewDataPageTemplate.ascx" %>
<%@ Register TagPrefix="mapcall" Namespace="MapCall.Controls" Assembly="MapCall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.SearchFields" TagPrefix="search" %>
<%@ Register Src="~/Controls/Data/OpCntrDataField.ascx" TagPrefix="mmsi" TagName="OpCntrDataField" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Markouts
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">
    <mapcall:DetailsViewDataPageTemplate ID="template" runat="server" 
        DefaultPageMode="Search"
        DataElementTableName="WorkOrders" 
		DataElementPrimaryFieldName="WorkOrderID"
		IsReadOnlyPage="True"
        Label="Markouts">
        <SearchBox>
            <Fields>
                <search:TemplatedSearchField FilterMode="Manual">
                    <Template>
                        <mmsi:OpCntrDataField ID="opCntrField" runat="server" 
                            UseTowns="False" 
                            DataFieldName="oc.OperatingCenterID" 
                            TownDataFieldName="TownID" />
                    </Template>
                </search:TemplatedSearchField>
            </Fields>
        </SearchBox>
        <ResultsGridView AllowPaging="false" AutoGenerateColumns="True">

        </ResultsGridView>
        <ResultsDataSource
            SelectCommand="
                SELECT 
	                mt.Description as MarkoutTypeNeeded,
	                cast(RequiredMarkoutNote as varchar(8000)) as RequiredMarkoutNote,
	                WorkOrderID, 
	                '' as MarkoutExpirationDate,
	                c.Name as County,
	                oc.OperatingCenterCode as OperatingCenter,
	                T.Town,
	                ts.Name as TownSection,
	                StreetNumber,
	                S.fullstname as [Street],
	                S2.fullstname as [Nearest Cross Street],
	                AT.Description as AssetType,
	                WD.Description as WorkDescription,
	                WOP.Description as Priority,
	                WP.Description as Purpose
                FROM
	                [WorkOrders] AS wo
                LEFT JOIN
	                OperatingCenters oc on oc.OperatingCenterID = wo.OperatingCenterID
                LEFT JOIN
	                Towns T on T.TownID = wo.TownID
                LEFT JOIN
	                Counties C on C.CountyId = T.CountyID
                LEFT JOIN
	                MarkoutTypes mt on mt.MarkoutTypeID = MarkoutTypeNeededID
                LEFT JOIN 
	                AssetTypes AT on wo.AssetTypeID = AT.AssetTypeID
                LEFT JOIN
	                WorkDescriptions WD on WD.WorkDescriptionID = wo.WorkDescriptionID
                LEFT JOIN
	                WorkORderPriorities WOP on WOP.WorkOrderPriorityID = wo.PriorityID
                LEFT JOIN
	                WorkOrderPurposes WP on WP.WorKorderPurposeID = wo.PurposeID
                LEFT JOIN
	                Streets S on S.StreetID = wo.StreetID
                LEFT JOIN
	                Streets S2 on S2.StreetID = wo.NearestCrossStreetID
                LEFT JOIN
	                TownSections ts on ts.TownSectionID = wo.TownSectionID
            ">
        </ResultsDataSource>
    </mapcall:DetailsViewDataPageTemplate>

</asp:Content>
