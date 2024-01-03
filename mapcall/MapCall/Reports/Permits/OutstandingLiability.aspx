<%@ Page Title="" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" Theme="bender" CodeBehind="OutstandingLiability.aspx.cs" Inherits="MapCall.Reports.Permits.OutstandingLiability" %>
<%@ Import Namespace="MMSINC.Utilities" %>
<%@ Register TagPrefix="mapcall" TagName="DetailsViewDataPageTemplate" Src="~/Controls/DetailsViewDataPageTemplate.ascx" %>
<%@ Register TagPrefix="mapcall" Namespace="MapCall.Controls" Assembly="MapCall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.SearchFields" TagPrefix="search" %>
<%@ Register Src="~/Controls/Data/OpCntrDataField.ascx" TagPrefix="mmsi" TagName="OpCntrDataField" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
    Permits - Outstanding Liability
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">
    <mapcall:DetailsViewDataPageTemplate ID="template" runat="server" 
        DefaultPageMode="Search"
		DataElementPrimaryFieldName="PermitID"
		IsReadOnlyPage="True"
        Label="Permits - Outstanding Liability">
        
        <SearchBox>
            <Fields>
                <search:DropDownSearchField 
                    DataFieldName="OperatingCenter"
                    TableName="OperatingCenters"
                    TextFieldName="OperatingCenterCode"
                    ValueFieldName="OperatingCenterCode" />
                <search:DateTimeSearchField DataFieldName="SubmittedAt" HeaderText="SubmittedAt" />
            </Fields>
        </SearchBox>
        <ResultsGridView OnRowDataBound="rgv_RowDataBound" OnDataBinding="rgv_DataBinding" ShowFooter="True">
            <Columns>
                <mapcall:BoundField DataField="PermitID" SortExpression="PermitID" HeaderText="PermitID" />
                <mapcall:BoundField DataField="OperatingCenter" HeaderText="Operating Center" SortExpression="OperatingCenter" />
                <mapcall:BoundField DataField="CountyTown" HeaderText="County/Town" SortExpression="CountyTown" />  
                <mapcall:BoundField DataField="PermitFee" DataFormatString="{0:c}" SortExpression="PermitFee" HeaderText="PermitFee" />
                <mapcall:BoundField DataField="InspectionFee" DataFormatString="{0:c}" SortExpression="InspectionFee" HeaderText="InspectionFee" />
                <mapcall:BoundField DataField="BondFee" DataFormatString="{0:c}" SortExpression="BondFee" HeaderText="BondFee" />
                <mapcall:BoundField DataField="TotalCharged" DataFormatString="{0:c}" SortExpression="TotalCharged" HeaderText="TotalCharged" />
                <mapcall:BoundField DataField="Reconciled" SortExpression="Reconciled" HeaderText="Reconciled" />
                <mapcall:BoundField DataField="Email" SortExpression="Email" HeaderText="User" />
                <mapcall:BoundField DataField="CreatedAt" SortExpression="CreatedAt" HeaderText="CreatedAt" />
                <mapcall:BoundField DataField="CanceledAt" SortExpression="CanceledAt" HeaderText="CanceledAt" />
                <mapcall:BoundField DataField="SubmittedAt" SortExpression="SubmittedAt" HeaderText="SubmittedAt" />
                <mapcall:BoundField DataField="PaymentReceivedAt" SortExpression="PaymentReceivedAt" HeaderText="PaymentReceivedAt" />
            </Columns>
        </ResultsGridView>
        <ResultsDataSource
            SelectCommand="
                SELECT * FROM 
                (select
	                P.id as PermitID,
                    (select top 1 operatingCenterCode from mcprod.dbo.OperatingCenters oc join mcprod.dbo.OperatingCentersTowns oct on oct.OperatingCenterID = oc.OperatingCenterID and oct.TownID = T.TownID) as OperatingCenter,
	                M.Name as CountyTown, 
	                PermitFee,
	                InspectionFee,
	                BondFee,
	                PermitFee + InspectionFee + BondFee as TotalCharged,
	                (Select top 1 Reconciled from permits.dbo.Checks where permits.dbo.Checks.PermitId = P.Id) as Reconciled,
	                u.email,
	                p.CreatedAt, 
                    p.CanceledAt, 
                    p.SubmittedAt, 
                    p.PaymentReceivedAt
                from 
	                permits.dbo.permits P
                left join
	                permits.dbo.forms F on F.Id = P.FormId
                left join
	                permits.dbo.users U on U.Id = P.CreatedById
                left join
	                permits.dbo.companies C on C.Id = U.CompanyId
                left join
	                permits.dbo.Municipalities m on m.Id = F.MunicipalityId
                left join 
	                permits.dbo.counties on permits.dbo.counties.id = m.CountyId
                left join
	                mcprod.dbo.towns T on rtrim(ltrim(t.Town)) = rtrim(ltrim(m.Name))
                left join
	                mcprod.dbo.counties mcCounties on mcCounties.CountyID = T.CountyID
                where
	                U.CompanyId = 2
                and
	                MunicipalityId is not null
                and
	                T.StateID = permits.dbo.counties.StateId
                and
	                mcCounties.Name = permits.dbo.counties.Name
                and
                    isNull((Select top 1 Reconciled from permits.dbo.Checks where permits.dbo.Checks.PermitId = P.Id), 0) = 0
				and 
					p.TotalCharged > p.ProcessingFee
				and 
					p.SubmittedAt is not null
				and
					(select count(1) from permits.dbo.Refunds where permitID = p.Id) = 0
                UNION ALL

                select
	                P.id as PermitID,
	                '' as OperatingCenter,
	                ct.Name as CountyTown,
	                PermitFee,
	                InspectionFee,
	                BondFee,
	                TotalCharged,
	                (Select top 1 Reconciled from permits.dbo.Checks where permits.dbo.Checks.PermitId = P.Id) as Reconciled,
	                u.email,
	                p.CreatedAt, p.CanceledAt, p.SubmittedAt, p.PaymentReceivedAt
                from 
	                permits.dbo.permits P
                join
	                permits.dbo.forms F on F.Id = P.FormId
                join
	                permits.dbo.users U on U.Id = P.CreatedById
                join
	                permits.dbo.companies C on C.Id = U.CompanyId
                left join
	                permits.dbo.Counties ct on ct.Id = F.CountyId
                where
	                U.CompanyId = 2
                and
	                F.CountyId is not null
                and
                    isNull((Select top 1 Reconciled from permits.dbo.Checks where permits.dbo.Checks.PermitId = P.Id), 0) = 0
				and 
					p.TotalCharged > p.ProcessingFee
				and 
					p.SubmittedAt is not null
				and
					(select count(1) from permits.dbo.Refunds where permitID = p.Id) = 0
                ) t
            ">
        </ResultsDataSource>
    </mapcall:DetailsViewDataPageTemplate>
</asp:Content>