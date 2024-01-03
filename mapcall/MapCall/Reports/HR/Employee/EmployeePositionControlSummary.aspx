<%@ Page Title="Employee Position Control Summary" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="EmployeePositionControlSummary.aspx.cs" Inherits="MapCall.Reports.HR.Employee.EmployeePositionControlSummary" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls" TagPrefix="mapcall" %>
<%@ Import Namespace="MapCall.Controls.Contacts" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.DropDowns" TagPrefix="mapcall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.SearchFields" TagPrefix="search" %>
<%@ Register Src="~/Controls/DetailsViewDataPageTemplate.ascx" TagPrefix="mapcall"
    TagName="DetailsViewDataPageTemplate" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
Employee Position Control 
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">
<mapcall:DetailsViewDataPageTemplate ID="template" runat="server" 
    DataElementTableName="EmployeePositionControls" 
    DataElementPrimaryFieldName="EmployeePositionControlID"
    IsReadOnlyPage="true"
    Label="Employee Position Control Report">
        <SearchBox runat="server">
            <Fields>
                <search:TemplatedSearchField 
                    Label="Operating Center"
                    DataFieldName="bu.OperatingCenterID"
                    BindingControlID="searchOpCenter"
                    BindingDataType="Int32"
                    BindingPropertyName="SelectedValue">
                    <Template>
                        <mapcall:OperatingCenterDropDownList ID="searchOpCenter" runat="server" />
                    </Template>    
                </search:TemplatedSearchField>
                <search:DropDownSearchField 
                    Label="Business Unit"
                    DataFieldName="epc.BusinessUnitID"
                    TextFieldName="txt"
                    ValueFieldName="val"
                    SelectCommand="select distinct tbu.BusinessUnitID as val,tbu.BU as txt
                               from BusinessUnits tbu
                               inner join EmployeePositionControls epc
                               on epc.BusinessUnitID = tbu.BusinessUnitID
                                order by tbu.BU" />
                <search:TextSearchField DataFieldName="epc.PositionControlNumber" />
                <search:TextSearchField 
                    DataFieldName="epc.TemporaryPositionControlNumber"
                    Label="Alias Position Control Number" />
                <search:DropDownSearchField 
                    Label="BU Department" 
                    DataFieldName="bu.DepartmentID"
                    TableName="Departments"
                    TextFieldName="Description"
                    ValueFieldName="DepartmentID"
                />
                <search:DropDownSearchField 
                    Label="BU Area"
                    DataFieldName="bu.BusinessUnitAreaId"
                    SelectCommand="select distinct tbu.BusinessUnitAreaId as Area, lookup.Description as LookupValue
                                    from BusinessUnits tbu
                                    left join [BusinessUnitAreas] lookup on lookup.Id = tbu.BusinessUnitAreaId"
                    TextFieldName="LookupValue"
                    ValueFieldName="Area"
                />
                <search:TextSearchField DataFieldName="pos.Position" />
                <search:DropDownSearchField 
                    Label="Position Type" 
                    DataFieldName="epc.EmployeePositionTypeID"
                    TableName="EmployeePositionTypes"
                    TextFieldName="Description"
                    ValueFieldName="EmployeePositionTypeID"
                />
                <search:DateTimeSearchField DataFieldName="epc.PCNApprovedDate" />
                <search:DateTimeSearchField DataFieldName="epc.PCNEliminatedDate" />
            </Fields>
        </SearchBox>

    <ResultsGridView>
        <Columns>
            <mapcall:BoundField DataField="Flag" />
            <mapcall:BoundField DataField="OperatingCenterCode" />
            <mapcall:BoundField DataField="BU" DataType="NVarChar" />
            <mapcall:BoundField DataField="Description" DataType="NVarChar" />
            <mapcall:BoundField DataField="ReportsToEmployeeName" HeaderText="Reports To Employee" />
            <mapcall:BoundField DataField="OrgLevel" />
            <mapcall:BoundField DataField="PositionControlNumber" DataType="VarChar" />
            <asp:TemplateField HeaderText="Employee" SortExpression="Last_Name">
                <ItemTemplate><%# ContactsAutoComplete.FormatContactName(Eval("First_Name").ToString(), Eval("Middle_Name").ToString(), Eval("Last_Name").ToString())  %></ItemTemplate>
            </asp:TemplateField>
            <mapcall:BoundField DataField="Area" HeaderText="BU Area" />
            <mapcall:BoundField DataField="EmployeePositionControlID" DataType="Int" ReadOnly="true" HeaderText="PCN ID" />
        </Columns>
    </ResultsGridView>
    <ResultsDataSource ConnectionString='<%$ ConnectionStrings:MCProd %>' 
        SelectCommand="SELECT 
                        epc.*,
                        bu.BU,
                        bu.Description,
                        dep.Description as Department,
                        lookup.Description as Area,
                        lookupOrgLevel.LookupValue as OrgLevel,
                        stat.Description as StatusText,
                        posTypes.Description as PositionTypeDescription,
                        pos.Position as PositionText,
                        empl.EmployeeID as EmployeeNumber,
                        empl.First_Name,
                        empl.Middle_Name,
                        empl.Last_Name,
                        emplReports.Last_Name + ', ' + emplReports.First_Name as ReportsToEmployeeName,
                        emplDotted.Last_Name + ', ' + emplDotted.First_Name as DottedReportsToEmployeeName,
                        hist.Position_ID as PositionHistoryPositionID,
                        classy.Position PositionClassificationName,
                        opc.OperatingCenterID,
                        opc.OperatingCenterCode
                    FROM
                        [EmployeePositionControls] epc
                    LEFT JOIN [BusinessUnits] bu                          ON bu.BusinessUnitID = epc.BusinessUnitID
                    LEFT JOIN [OperatingCenters] opc                      ON opc.OperatingCenterID = bu.OperatingCenterID
                    LEFT JOIN [Departments] dep                           ON dep.DepartmentID = bu.DepartmentID
                    LEFT JOIN [EmployeePositionControlStatusTypes] stat   ON stat.[EmployeePositionControlStatusTypeID] = epc.[EmployeePositionControlStatusTypeID]
                    LEFT JOIN [EmployeePositionTypes] posTypes            ON posTypes.[EmployeePositionTypeID] = epc.[EmployeePositionTypeID]
                    LEFT JOIN [tblPositions_Classifications] pos          ON pos.PositionID = epc.[PositionID]
                    LEFT JOIN [BusinessUnitAreas] AS lookup               ON lookup.Id = bu.BusinessUnitAreaId
                    LEFT JOIN [Lookup] AS lookupOrgLevel                  ON lookupOrgLevel.LookupID = epc.OrgLevelID
                    LEFT JOIN [tblEmployee] empl                          ON empl.tblEmployeeId = epc.EmployeeID
                    LEFT JOIN [tblEmployee] emplReports                   ON emplReports.tblEmployeeId = epc.ReportsToEmployeeID
                    LEFT JOIN [tblEmployee] emplDotted                    ON emplDotted.tblEmployeeId = epc.DottedLineReportsToEmployeeID
                    LEFT JOIN [tblPosition_History] hist                  ON hist.Position_History_ID = (select top 1 Position_History_ID from tblPosition_History tph where tph.tblEmployeeID = epc.EmployeeID and tph.Position_End_Date is null order by Position_Start_Date DESC)
                    LEFT JOIN [tblPositions_classifications] classy       ON classy.positionID = hist.position_id
" />

    </mapcall:DetailsViewDataPageTemplate>


</asp:Content>
