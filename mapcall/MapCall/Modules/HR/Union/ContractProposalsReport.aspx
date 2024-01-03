<%@ Page Title="Contract Proposals" Theme="bender" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="ContractProposalsReport.aspx.cs" Inherits="MapCall.Modules.HR.Union.ContractProposalsReport" EnableEventValidation="false" %>
<%@ Register TagPrefix="mapcall" Namespace="MapCall.Controls" Assembly="MapCall" %>
<%@ Register TagPrefix="mapcall" Namespace="MapCall.Controls.SearchFields" Assembly="MapCall" %>
<%@ Register Src="~/Controls/DetailsViewDataPageTemplate.ascx" TagPrefix="mapcall"
    TagName="DetailsViewDataPageTemplate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Contract Proposals Report
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">

    <mapcall:DetailsViewDataPageTemplate ID="template" runat="server"
        DefaultPageMode="Search"
        DataElementTableName="UnionContractProposals"
        DataElementPrimaryFieldName="Proposal_ID" 
        IsReadOnlyPage="true"
        Label="Contract Proposals Report">
        <SearchBox>
            <Fields>
                <mapcall:DropDownSearchField 
                    Label="Contract ID"
                    Required="true"
                    TextFieldName="txt"
                    ValueFieldName="val"
                    DataFieldName="UnionContractProposals.ContractId"
                    SelectCommand="select distinct UnionContracts.ID as val, isNull(cast(UnionContracts.ID as varchar(20)),'') + ' - ' + isNull(OperatingCenterCode,'') + ' - ' + isNull(Name,'') + ' - ' + isNull(cast(startdate as varchar(12)),'') + ' - ' + isNull(cast(enddate as varchar(12)),'') as txt from UnionContracts, LocalBargainingUnits INNER JOIN OperatingCenters ON LocalBargainingUnits.OperatingCenterId = OperatingCenters.OperatingCenterId where LocalBargainingUnits.Id = UnionContracts.LocalID order by 1" />
               <mapcall:DropDownSearchField 
                    SelectMode="Multiple"
                    TextFieldName="txt"
                    ValueFieldName="val"
                    DataFieldName="CrossReferenceNumber"
                    SelectCommand="select distinct CrossReferenceNumber as val, CrossReferenceNumber as txt from UnionContractProposals order by CrossReferenceNumber" />
                <mapcall:DropDownSearchField
                    Label="Management or Union"
                    TextFieldName="txt"
                    ValueFieldName="val"
                    DataFieldName ="ManagementOrUnionId"
                    SelectCommand="select distinct Description as txt, Id as val from ManagementOrUnion" />
                <mapcall:TextSearchField DataFieldName="#4.LookupValue" Label="Status" />
                <mapcall:DropDownSearchField 
                    SelectMode="Multiple"
                    TextFieldName="txt"
                    ValueFieldName="val"
                    DataFieldName="PrioritizationId"
                    SelectCommand="select distinct Id as val, Description as txt from UnionContractProposalPrioritizations" />
                <mapcall:BooleanSearchField SearchType="DropDownList" DataFieldName="Flag" />
            </Fields>
        </SearchBox>

        <ResultsGridView>
            <Columns>
                <mapcall:BoundField DataField="Contract_ID"  />
                <mapcall:BoundField DataField="Management_Or_Union" />
                <mapcall:BoundField DataField="Cross_Reference_Number" />
                <mapcall:BoundField DataField="Printing_Sequence" />
                <mapcall:BoundField DataField="Grouping" />
                <mapcall:BoundField DataField="Status"  />
                <mapcall:BoundField DataField="Prioritization"  />
                <mapcall:BoundField DataField="Proposal Description" />
                <mapcall:BoundField DataField="Flag" />
                <mapcall:BoundField DataField="ImplementationItems" />
                <mapcall:BoundField DataField="Notes" />
            </Columns>
        </ResultsGridView>
        
        <ResultsDataSource SelectCommand="
            SELECT 
                UnionContractProposals.Id as Proposal_ID,
	            UnionContractProposals.ContractID as Contract_Id,
                UnionContractProposals.Notes,
                UnionContractProposals.ImplementationItems,
	            #5.Description as Management_Or_Union,
	            CrossReferenceNumber as Cross_Reference_Number,
	            cast(#3.Description as int) as [Printing_Sequence],
	            Flag,
	            #2.Description as [Grouping],
	            #4.Description as [Status],
	            #1.Description as [Prioritization],
	            ProposalDescription as [Proposal Description]
            from 
	            UnionContractProposals 
            left join
                UnionContracts
            on
                UnionContracts.Id = UnionContractProposals.ContractID
            left join
                LocalBargainingUnits
            on
                LocalBargainingUnits.Id = UnionContracts.LocalID 
            LEFT JOIN 
	            UnionContractProposalPrioritizations #1 
            on 
	            #1.Id = [PrioritizationId]
            LEFT JOIN
	            UnionContractProposalGroupings #2
            ON
	            #2.Id = [GroupingId]
            LEFT JOIN 
	            UnionContractProposalPrintingSequences #3 
            on 
	            #3.Id = [PrintingSequenceId]
            LEFT JOIN 
	            UnionContractProposalStatuses #4 
            on 
	            #4.Id = [StatusId]
            LEFT JOIN
	            ManagementOrUnion #5
            on 
	            #5.Id = [ManagementOrUnionId]"  />

    </mapcall:DetailsViewDataPageTemplate>
</asp:Content>
