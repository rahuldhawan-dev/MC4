<%@ Page Title="" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="StreetOpeningPermits.aspx.cs" Inherits="MapCall.Reports.Permits.StreetOpeningPermits" %>
<%@ Register TagPrefix="mapcall" TagName="DetailsViewDataPageTemplate" Src="~/Controls/DetailsViewDataPageTemplate.ascx" %>
<%@ Register TagPrefix="mapcall" Namespace="MapCall.Controls" Assembly="MapCall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.SearchFields" TagPrefix="search" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
    Street Opening Permits
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">
    <mapcall:DetailsViewDataPageTemplate ID="template" runat="server" 
        DefaultPageMode="Search"
		DataElementPrimaryFieldName="PermitID"
		IsReadOnlyPage="True"
        Label="Street Opening Permits">
        <SearchBox>
            <Fields>
                <search:DropDownSearchField 
                    DataFieldName="OperatingCenter"
                    TableName="OperatingCenters"
                    TextFieldName="OperatingCenterCode"
                    ValueFieldName="OperatingCenterCode" />
                <search:DateTimeSearchField DataFieldName="CreatedAt" HeaderText="CreatedAt" />
            </Fields>
        </SearchBox>
        <ResultsGridView>
            <Columns>
                <asp:HyperLinkField DataNavigateUrlFields="PermitID" DataTextField="PermitID" SortExpression="PermitID" 
                    HeaderText="PermitID" DataNavigateUrlFormatString="https://permits.mapcall.net/permit/show/{0}" Target="_new" />
                <asp:HyperLinkField DataNavigateUrlFields="WorkOrderID" DataTextField="WorkOrderID" SortExpression="WorkOrderID" 
                    HeaderText="WorkOrderID" DataNavigateUrlFormatString="~/modules/WorkOrders/Views/WorkOrders/General/WorkOrderGeneralResourceRPCPage.aspx?cmd=view&arg={0}" />
                <mapcall:BoundField DataField="OperatingCenter" HeaderText="Operating Center" SortExpression="OperatingCenter" />
                <mapcall:BoundField DataField="StreetAddress"/>
                <mapcall:BoundField DataField="NearestCrossStreet"/>
                <mapcall:BoundField DataField="CountyTown" HeaderText="County/Town" SortExpression="CountyTown" />  
                <mapcall:BoundField DataField="CreatedAt" SortExpression="CreatedAt" HeaderText="CreatedAt" />
                <mapcall:BoundField DataField="PermitReceivedDate" DataFormatString="{0:d}"/>
                <mapcall:BoundField DataField="PaymentReceivedAt" SortExpression="PaymentReceivedAt" HeaderText="PaymentReceivedAt" />
                <mapcall:BoundField DataField="SubmittedAt" SortExpression="SubmittedAt" HeaderText="SubmittedAt" />
                <mapcall:BoundField DataField="TotalCharged" DataFormatString="{0:c}" SortExpression="TotalCharged" HeaderText="TotalCharged" />
                <mapcall:BoundField DataField="PermitFee" DataFormatString="{0:c}" SortExpression="PermitFee" HeaderText="PermitFee" />
                <mapcall:BoundField DataField="InspectionFee" DataFormatString="{0:c}" SortExpression="InspectionFee" HeaderText="InspectionFee" />
                <mapcall:BoundField DataField="BondFee" DataFormatString="{0:c}" SortExpression="BondFee" HeaderText="BondFee" />
                <mapcall:BoundField DataField="Reconciled"/>
                <mapcall:BoundField DataField="WorkDescription"/>
                <mapcall:BoundField DataField="AccountCharged"/>
                <mapcall:BoundField DataField="AccountingType"/>
                <mapcall:BoundField DataField="CanceledAt" SortExpression="CanceledAt" HeaderText="CanceledAt" />
                <mapcall:BoundField DataField="Refunded"/>
                <asp:HyperLinkField/>
            </Columns>
        </ResultsGridView>
        <ResultsDataSource
            SelectCommand="
              SELECT * FROM 
                    (select
	                P.id as PermitID,
                    (select top 1 operatingCenterCode from mcprod.dbo.OperatingCenters oc join mcprod.dbo.OperatingCentersTowns oct on oct.OperatingCenterID = oc.OperatingCenterID and oct.TownID = T.TownID) as OperatingCenter,
					--isNull(wo.StreetNumber, '') + ' ' + S.FullStName as WorkOrderStreetAddress,
					(ltrim(isNull((select pv.Value from permits.dbo.permitValues pv join permits.dbo.fields F1 on F1.Id = pv.FieldId join permits.dbo.FieldTemplates ft on ft.Id = f1.FieldTemplateId where pv.PermitId = p.id and ft.name = 'LocationStreetNumber'), '') + ' ' + isNull((select pv.Value from permits.dbo.permitValues pv join permits.dbo.fields F1 on F1.Id = pv.FieldId join permits.dbo.FieldTemplates ft on ft.Id = f1.FieldTemplateId where pv.PermitId = p.id and ft.name = 'LocationStreetName'),''))) as StreetAddress,
					CS.FullStName as NearestCrossStreet,
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
					sop.DateIssued as PermitReceivedDate,
                    p.PaymentReceivedAt,
                    case when (select count(1) from permits.dbo.Refunds where permitID = p.Id) > 0 then 'Yes' else '' end as Refunded,
					wo.WorkOrderID,
					wd.Description as WorkDescription, 
					wo.AccountCharged,
					at.Description as AccountingType
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
				left join	
					mcprod.dbo.workorders wo on wo.WorkOrderID = (Case WHEN (isNull(isNumeric(p.arbitraryIdentifier), 0) = 1 and charindex(',', p.ArbitraryIdentifier) = 0) then p.arbitraryIdentifier else 0 end)
				--left join	
				--	mcprod.dbo.Streets S on S.StreetID = wo.StreetID
				left join
					mcprod.dbo.Streets CS on CS.StreetID = wo.NearestCrossStreetID
				left join
					mcprod.dbo.WorkDescriptions wd on wd.WorkDescriptionID = wo.WorkDescriptionID
				left join	
					mcprod.dbo.AccountingTypes at on at.AccountingTypeID = wd.AccountingTypeID
				left join
					mcprod.dbo.StreetOpeningPermits sop on sop.PermitId = p.Id
                where
	                U.CompanyId = 2
                and
	                MunicipalityId is not null
                and
	                T.StateID = permits.dbo.counties.StateId
                and
	                mcCounties.Name = permits.dbo.counties.Name
                UNION ALL

                select
	                P.id as PermitID,
	                (select top 1 OperatingCenterCode from operatingCenters occ where occ.operatingCenterId = wo.OperatingCenterID) as OperatingCenter,
					--isNull(wo.StreetNumber, '') + ' ' + S.FullStName as WorkOrderStreetAddress,
					(ltrim(isNull((select pv.Value from permits.dbo.permitValues pv join permits.dbo.fields F1 on F1.Id = pv.FieldId join permits.dbo.FieldTemplates ft on ft.Id = f1.FieldTemplateId where pv.PermitId = p.id and ft.name = 'LocationStreetNumber'), '') + ' ' + isNull((select pv.Value from permits.dbo.permitValues pv join permits.dbo.fields F1 on F1.Id = pv.FieldId join permits.dbo.FieldTemplates ft on ft.Id = f1.FieldTemplateId where pv.PermitId = p.id and ft.name = 'LocationStreetName'),''))) as StreetAddress,
					CS.FullStName as NearestCrossStreet,
	                ct.Name as CountyTown,
	                PermitFee,
	                InspectionFee,
	                BondFee,
	                TotalCharged,
	                (Select top 1 Reconciled from permits.dbo.Checks where permits.dbo.Checks.PermitId = P.Id) as Reconciled,
	                u.email,
	                p.CreatedAt, p.CanceledAt, p.SubmittedAt, 
					sop.DateIssued as PermitReceivedDate,
					p.PaymentReceivedAt,
                    case when (select count(1) from permits.dbo.Refunds where permitID = p.Id) > 0 then 'Yes' else '' end as Refunded,
					wo.WorkOrderID,
					wd.Description as WorkDescription, 
					wo.AccountCharged,
					at.Description as AccountingType
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
				left join	
					mcprod.dbo.workorders wo on wo.WorkOrderID = (Case WHEN (isNull(isNumeric(p.arbitraryIdentifier), 0) = 1 and charindex(',', p.ArbitraryIdentifier) = 0) then p.arbitraryIdentifier else 0 end)
				--left join	
				--	mcprod.dbo.Streets S on S.StreetID = wo.StreetID
				left join
					mcprod.dbo.Streets CS on CS.StreetID = wo.NearestCrossStreetID
				left join
					mcprod.dbo.WorkDescriptions wd on wd.WorkDescriptionID = wo.WorkDescriptionID
				left join	
					mcprod.dbo.AccountingTypes at on at.AccountingTypeID = wd.AccountingTypeID
				left join
					mcprod.dbo.StreetOpeningPermits sop on sop.PermitId = p.Id
                where
	                U.CompanyId = 2
                and
	                F.CountyId is not null
				) t
            ">
        </ResultsDataSource>
    </mapcall:DetailsViewDataPageTemplate>
</asp:Content>
