<%@ Page Title="Employee Position Control Report" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="EmployeePositionControlReport.aspx.cs" Inherits="MapCall.Reports.HR.Employee.EmployeePositionControlReport" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls" TagPrefix="mapcall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.DropDowns" TagPrefix="mapcall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.SearchFields" TagPrefix="search" %>
<%@ Register Src="~/Controls/DetailsViewDataPageTemplate.ascx" TagPrefix="mapcall"
    TagName="DetailsViewDataPageTemplate" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
Employee Position Control Report
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
            <mapcall:BoundField DataField="EmployeePositionControlID" />
            <mapcall:BoundField DataField="PositionControlNumber" />
            <mapcall:BoundField DataField="TemporaryPositionControlNumber" HeaderText="Alias Position Control Number" />
            <mapcall:BoundField DataField="BU" />
            <mapcall:BoundField DataField="Department" HeaderText="BU Department" />
            <mapcall:BoundField DataField="Area" HeaderText="BU Area" />
            <mapcall:BoundField DataField="PositionText" HeaderText="Position" />
            <mapcall:BoundField DataField="PositionTypeDescription" HeaderText="Position Type" />
            <mapcall:BoundField DataField="PCNApprovedDate" DataType="Date" />
            <mapcall:BoundField DataField="PCNEliminatedDate" DataType="Date" />
            <mapcall:BoundField DataField="Position_History_ID" />
            <mapcall:BoundField DataField="EmployeeID" />
            <mapcall:BoundField DataField="Last_Name" />
            <mapcall:BoundField DataField="First_Name" />
            <mapcall:BoundField DataField="Position_ID" />
            <mapcall:BoundField DataField="Position_Start_Date" DataType="Date" />
            <mapcall:BoundField DataField="Position_End_Date" DataType="Date" />
        </Columns>
    </ResultsGridView>
    <ResultsDataSource ConnectionString='<%$ ConnectionStrings:MCProd %>' 
        SelectCommand="SELECT 
                        epc.*,
                        bu.BU,
                        bu.Description,
                        dep.Description as Department,
                        lookup.Description as Area,
                        stat.Description as StatusText,
                        posTypes.Description as PositionTypeDescription,
                        pos.Position as PositionText,
                        pos.OpCode as OperatingCenter,
                        hist.Position_History_ID,
                        hist.Position_ID,
                        hist.Position_Start_Date,
                        hist.Position_End_Date,
                        employee.tblEmployeeID as EmployeeID,
                        employee.Last_Name,
                        employee.First_Name
                    FROM
                        [EmployeePositionControls] epc
                    LEFT JOIN [tblPosition_History] hist
                        ON hist.EmployeePositionControlID = epc.EmployeePositionControlID
                    LEFT JOIN [tblEmployee] employee
                        on employee.tblEmployeeID = hist.tblEmployeeID
                    LEFT JOIN [BusinessUnits] bu
                        ON bu.BusinessUnitID = epc.BusinessUnitID
                    LEFT JOIN [Departments] dep
                        on dep.DepartmentID = bu.DepartmentID
                    LEFT JOIN [OperatingCenters] opc
                        ON opc.OperatingCenterID = bu.OperatingCenterID
                    LEFT JOIN [EmployeePositionControlStatusTypes] stat 
                        ON stat.[EmployeePositionControlStatusTypeID] = epc.[EmployeePositionControlStatusTypeID]
                    LEFT JOIN [EmployeePositionTypes] posTypes
                        ON posTypes.[EmployeePositionTypeID] = epc.[EmployeePositionTypeID]
                    LEFT JOIN [tblPositions_Classifications] pos
                        ON pos.PositionID = epc.[PositionID]
                    LEFT JOIN [BusinessUnitAreas] AS lookup
                        ON lookup.Id = bu.BusinessUnitAreaId" />

    </mapcall:DetailsViewDataPageTemplate>

</asp:Content>
