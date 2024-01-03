<%@ Page Title="Employee Position Controls" Language="C#" MasterPageFile="~/MapCallSite.Master" Theme="bender" AutoEventWireup="true" CodeBehind="EmployeePositionControls.aspx.cs" Inherits="MapCall.Modules.HR.Administrator.EmployeePositionControls" ValidateRequest="false" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls" TagPrefix="mapcall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.DropDowns" TagPrefix="mapcall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.SearchFields" TagPrefix="search" %>
<%@ Register Assembly="MMSINC.Core.WebForms" Namespace="MMSINC.Controls" TagPrefix="mmsi" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>
<%@ Register Src="~/Controls/DetailsViewDataPageTemplate.ascx" TagPrefix="mapcall" TagName="DetailsViewDataPageTemplate" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
    Employee Position Controls
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">

<mapcall:DetailsViewDataPageTemplate ID="template" runat="server" 
    DataElementTableName="EmployeePositionControls" 
    DataTypeId="146"
    DataElementPrimaryFieldName="EmployeePositionControlID"
    Label="Employee Position Control">
    <SearchBox>
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
            <search:BooleanSearchField DataFieldName="epc.Flag" 
                            Label="Flag" 
                            SearchType="DropDownList" />
            <search:DropDownSearchField 
                SelectMode="Multiple"
                Label="Status"
                DataFieldName="epc.EmployeePositionControlStatusTypeID"
                TextFieldName="Description"
                ValueFieldName="EmployeePositionControlStatusTypeID"
                SelectCommand="SELECT * FROM [EmployeePositionControlStatusTypes] order by 2"
            />
            <search:DropDownSearchField 
                Label="Business Unit"
                DataFieldName="epc.BusinessUnitID"
                TextFieldName="txt"
                ValueFieldName="val"
                SelectCommand="select distinct tbu.BusinessUnitID as val,tbu.BU as txt
                           from BusinessUnits tbu
                           inner join EmployeePositionControls epc
                           on epc.BusinessUnitID = tbu.BusinessUnitID
                            order by tbu.BU"
            />
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
                DataFieldName="BU.BusinessUnitAreaId"
                SelectCommand="select distinct tbu.BusinessUnitAreaId as Area, lookup.Description as LookupValue
                                from BusinessUnits tbu
                                left join [BusinessUnitAreas] lookup on lookup.Id = tbu.BusinessUnitAreaId"
                TextFieldName="LookupValue"
                ValueFieldName="Area"
            />
            <search:TextSearchField DataFieldName="pos.Position" />
            <search:TextSearchField DataFieldName="epc.SAP_Object_Abbreviation" />
            <search:TextSearchField DataFieldName="epc.SAPParentOrgUnit" />
            <search:BooleanSearchField DataFieldName="epc.SAPChief" 
                SearchType="DropDownList" />
            <search:TextSearchField DataFieldName="epc.SAP_OrgUnit_Long_Desc" />
            <search:TextSearchField DataFieldName="epc.SAPOrgUnit" />
            <search:DropDownSearchField 
                Label="Position Type" 
                DataFieldName="epc.EmployeePositionTypeID"
                TableName="EmployeePositionTypes"
                TextFieldName="Description"
                ValueFieldName="EmployeePositionTypeID"
            />
            <search:DropDownSearchField 
                Label="Org Level"
                DataFieldName="epc.OrgLevelID"
                SelectCommand="SELECT LookupID, LookupValue 
                                      FROM [Lookup] where LookupType = 'OrgLevelID' and TableName = 'EmployeePositionControls'
                                      order by LookupValue"
                TextFieldName="LookupValue"
                ValueFieldName="LookupID"
            />
            <search:TextSearchField DataFieldName="empl.EmployeeID" />
            <search:TextSearchField DataFieldName="empl.Last_Name" Label="Employee Last Name" />
            <search:TextSearchField DataFieldName="emplReports.Last_Name" ControlID="ReportsLastName" Label="Reports To Employee by Last Name" />
            <search:TextSearchField DataFieldName="emplDotted.Last_Name" ControlID="DottedLastName" Label="Dotted Line Reports To Employee by Last Name" />
            <search:TextSearchField DataFieldName="epc.PercentCapitalization" />
            <search:TextSearchField DataFieldName="epc.CapitalLine" />
            <search:DateTimeSearchField DataFieldName="epc.PCNApprovedDate" />
            <search:DateTimeSearchField DataFieldName="epc.PCNEliminatedDate" />
        </Fields>
    </SearchBox>
    <SearchHelpPlaceHolder>
        <style>
             .searchBox input { width:200px; }
             .searchBox .dateTimePicker { width:143px; }
        </style>
    </SearchHelpPlaceHolder>
    <ResultsGridView>
        <Columns>
            <mapcall:BoundField DataField="EmployeePositionControlID" DataType="Int" ReadOnly="true" HeaderText="PCN ID" />
            <mapcall:BoundField DataField="Flag" />
            <mapcall:BoundField DataField="OperatingCenterCode" />
            <mapcall:BoundField DataField="BU" DataType="NVarChar" />
            <mapcall:BoundField DataField="Description" DataType="NVarChar" />
            <mapcall:BoundField DataField="ReportsToEmployeeName" HeaderText="Reports To Employee" />
            <mapcall:BoundField DataField="OrgLevel" />
            <mapcall:BoundField DataField="PositionControlNumber" DataType="VarChar" />
            <mapcall:BoundField DataField="Last_Name" />
            <mapcall:BoundField DataField="First_Name" />
            <mapcall:BoundField DataField="StatusText" DataType="NVarChar" />
            <mapcall:BoundField DataField="EmployeeNumber" />
            <mapcall:BoundField DataField="Area" HeaderText="BU Area" DataType="NVarChar" />
			<mapcall:BoundField DataField="PositionTitle" />
            <mapcall:BoundField DataField="SAP_Object_Abbreviation" />
            <mapcall:BoundField DataField="SAPParentOrgUnit" />
            <mapcall:BoundField DataField="SAPChief" />
            <mapcall:BoundField DataField="SAP_OrgUnit_Long_Desc" />
            <mapcall:BoundField DataField="SAPOrgUnit" />
            <mapcall:BoundField DataField="PositionText" HeaderText="Position" DataType="NVarChar" />
            <mapcall:BoundField DataField="PositionTypeDescription" HeaderText="Position Type" DataType="NVarChar" />
            <mapcall:BoundField DataField="DottedReportsToEmployeeName" HeaderText="Dotted Line Reports To Employee" />
            <mapcall:BoundField DataField="PositionHistoryPositionID" HeaderText="Position History: Position ID" ReadOnly="true" />
            <mapcall:BoundField DataField="PositionClassificationName" HeaderText="Position History: Position Name" ReadOnly="true" />
            <mapcall:BoundField DataField="FTE" HeaderText="FTE" DataType="Decimal" />
            <mapcall:BoundField DataField="PercentCapitalization" />
            <mapcall:BoundField DataField="CapitalLine"  />
            <mapcall:BoundField DataField="PCNApprovedDate" DataType="Date" />
            <mapcall:BoundField DataField="PCNEliminatedDate" DataType="Date" />
            <mapcall:BoundField DataField="Note" /> <%-- This would be NText, except that the <pre> tags blow up the excel file that gets exported. --%>
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
                    LEFT JOIN [tblPositions_classifications] classy       ON classy.positionID = hist.position_id" />
    
    <DetailsView>
        <Fields>
            <mapcall:BoundField DataField="EmployeePositionControlID" DataType="Int" ReadOnly="true" HeaderText="PCN ID" InsertVisible="false" />
            <mapcall:BoundField DataField="Flag" DataType="Bit" />
            <asp:TemplateField HeaderText="Status">
                <ItemTemplate>
                    <%# Eval("StatusText") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <mapcall:DataSourceDropDownList ID="ddlStatus" runat="server" 
                        Required="true" ConnectionString='<%$ ConnectionStrings:MCProd %>' 
                        SelectedValue='<%# Bind("EmployeePositionControlStatusTypeID") %>'
                        TableName="EmployeePositionControlStatusTypes"
                        TextFieldName="Description"
                        ValueFieldName="EmployeePositionControlStatusTypeID"
                        />
                </EditItemTemplate>
            </asp:TemplateField>
            <mapcall:BoundField DataField="PositionControlNumber" DataType="VarChar" MaxLength="20" />
            <mapcall:BoundField DataField="TemporaryPositionControlNumber" HeaderText="Alias Position Control Number" DataType="VarChar" MaxLength="255" />
            <mapcall:BoundField DataField="PCNApprovedDate" DataType="Date" Required="true" />
            <mapcall:BoundField DataField="PCNEliminatedDate" DataType="Date" />
            <mapcall:BoundField DataField="AnticipatedEliminationDate" DataType="Date" />
            <asp:TemplateField HeaderText="Business Unit">
                <ItemTemplate>
                    <%# Eval("BU") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <mapcall:DataSourceDropDownList ID="ddlBusUnit" runat="server"
                        Required="true" 
                        SelectedValue='<%# Bind("BusinessUnitID") %>'
                        TableName="BusinessUnits"
                        TextFieldName="BU"
                        ValueFieldName="BusinessUnitID"
                        />
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Operating Center">
                <ItemTemplate><%# Eval("OperatingCenterCode") %></ItemTemplate>
                <EditItemTemplate>
                    <mapcall:DataSourceDropDownList ID="ddlOps" runat="server" ConnectionString='<%$ ConnectionStrings:MCProd %>' 
                        SelectCommand=" select distinct
                                          OperatingCenters.OperatingCenterID, OperatingCenters.OperatingCenterCode
                                          from BusinessUnits
                                          join OperatingCenters on OperatingCenters.OperatingCenterID = BusinessUnits.OperatingCenterID
                                          order by OperatingCenters.OperatingCenterCode"
                        TextFieldName="OperatingCenterCode" SelectedValue='<%# Bind("OperatingCenterID") %>'
                        ValueFieldName="OperatingCenterID"></mapcall:DataSourceDropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
            <mapcall:BoundField ID="bfDescription" DataField="Description" HeaderText="BU Description" DataType="NVarChar" />
            <mapcall:BoundField ID="bfDepartment" DataField="Department" HeaderText="BU Department" DataType="NVarChar" />
            <mapcall:BoundField ID="bfArea" DataField="Area" HeaderText="BU Area" DataType="NVarChar" />

            <asp:TemplateField HeaderText="Position">
                <ItemTemplate>
                    <%# Eval("PositionText") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <mmsi:MvpDropDownList ID="ddlPosition" runat="server" />
                    <atk:CascadingDropDown runat="server" ID="cddPosition" 
                        TargetControlID="ddlPosition" ParentControlID="ddlOps" 
                        Category="Position" 
                        EmptyText="None Found" EmptyValue=""
                        PromptText="--Select Position--" PromptValue="" 
                        LoadingText="[Loading Positions...]"
                        ServicePath="~/Modules/Data/Employees/EmployeePositionControls.asmx" 
                        ServiceMethod="GetPositionsByOpCode" 
                        SelectedValue='<%# Bind("PositionID") %>'
                        />
                </EditItemTemplate>
            </asp:TemplateField>
            
            <mapcall:BoundField DataField="PositionTitle" MaxLength="100" />
            <mapcall:BoundField DataField="SAP_Object_Abbreviation" MaxLength="100" />
            <mapcall:BoundField DataField="SAPParentOrgUnit" HeaderText="SAP_PARENT_ORG_UNIT" MaxLength="100" />
            <mapcall:BoundField DataField="SAPChief" DataType="Bit" />
            <mapcall:BoundField DataField="SAP_OrgUnit_Long_Desc" MaxLength="100" />
            <mapcall:BoundField DataField="SAPOrgUnit" HeaderText="SAP_ORG_UNIT" MaxLength="100" />
            <asp:TemplateField HeaderText="Org Level">
                <ItemTemplate>
                    <%# Eval("OrgLevel") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <mapcall:DataSourceDropDownList ID="ddlOrgLevelID" runat="server"
                        TextFieldName="LookupValue" ValueFieldName="LookupID"
                        SelectCommand="SELECT LookupID, LookupValue 
                                      FROM [Lookup] where LookupType = 'OrgLevelID' and TableName = 'EmployeePositionControls'
                                      order by LookupValue"
                        SelectedValue='<%# Bind("OrgLevelID") %>' />
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Position Type">
                <ItemTemplate>
                    <%# Eval("PositionTypeDescription")%>
                </ItemTemplate>
                <EditItemTemplate>
                    <mapcall:DataSourceDropDownList ID="ddlPositionType" runat="server"
                        SelectedValue='<%# Bind("EmployeePositionTypeID") %>'
                        TableName="EmployeePositionTypes"
                        TextFieldName="Description"
                        ValueFieldName="EmployeePositionTypeID" Required="true"
                        />
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Employee">
                <ItemTemplate>
                    <%# Eval("EmployeeNumber") %> : <%# Eval("Last_Name") %>, <%# Eval("First_Name") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <mapcall:DataSourceDropDownList ID="ddlEmployees" runat="server"
                        TextFieldName="display" ValueFieldName="value"
                        SelectCommand="SELECT [tblEmployeeID] as value, [EmployeeID] + ': ' + [Last_Name] + ', ' + [First_Name] as display
                                      FROM [tblEmployee]
                                      order by Last_Name"
                        SelectedValue='<%# Bind("EmployeeID") %>' />
                
                </EditItemTemplate>
            </asp:TemplateField>
            <mapcall:BoundField DataField="PositionHistoryPositionID" HeaderText="Position History: Position ID" ReadOnly="true" InsertVisible="false" />
            <mapcall:BoundField DataField="PositionClassificationName" HeaderText="Position History: Position Name" ReadOnly="true" InsertVisible="false" />
            <mapcall:BoundField DataField="FTE" HeaderText="FTE" DataType="Decimal" Required="true" />
            <asp:TemplateField HeaderText="Reports to Employee">
                <ItemTemplate>
                    <%# Eval("ReportsToEmployeeNumber") %> : <%# Eval("ReportsToEmployeeLastName") %>, <%# Eval("ReportsToEmployeeFirstName") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <mapcall:DataSourceDropDownList ID="ddlReportsToEmployeeID" runat="server"
                        TextFieldName="display" ValueFieldName="value"
                        SelectCommand="SELECT [tblEmployeeID] as value, [EmployeeID] + ': ' + [Last_Name] + ', ' + [First_Name] as display
                                      FROM [tblEmployee]
                                      order by Last_Name"
                        SelectedValue='<%# Bind("ReportsToEmployeeID") %>' />
                
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Dotted Line Reports to Employee">
                <ItemTemplate>
                    <%# Eval("DottedLineReportsToEmployeeNumber") %> : <%# Eval("DottedLineReportsToEmployeeLastName") %>, <%# Eval("DottedLineReportsToEmployeeFirstName") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <mapcall:DataSourceDropDownList ID="ddlDottedLineReportsToEmployeeID" runat="server"
                        TextFieldName="display" ValueFieldName="value"
                        SelectCommand="SELECT [tblEmployeeID] as value, [EmployeeID] + ': ' + [Last_Name] + ', ' + [First_Name] as display
                                      FROM [tblEmployee]
                                      order by Last_Name"
                        SelectedValue='<%# Bind("DottedLineReportsToEmployeeID") %>' />
                
                </EditItemTemplate>
            </asp:TemplateField>
            <mapcall:BoundField DataField="PercentCapitalization" DataType="VarChar" MaxLength="50" />
            <mapcall:BoundField DataField="CapitalLine" DataType="VarChar" MaxLength="50" />
            <mapcall:BoundField DataField="Note" DataType="NText" />
        </fields>
    </DetailsView>

    <DetailsDataSource 
        DeleteCommand="DELETE FROM [EmployeePositionControls] WHERE [EmployeePositionControlID] = @EmployeePositionControlID" 
        InsertCommand="INSERT INTO [EmployeePositionControls] ([Flag], [EmployeePositionControlStatusTypeID], [PositionControlNumber], [TemporaryPositionControlNumber], [PCNApprovedDate], [PCNEliminatedDate], [AnticipatedEliminationDate], [BusinessUnitID], [PositionID], [EmployeePositionTypeID], [FTE], [PercentCapitalization], [CapitalLine], [Note], [EmployeeID], [PositionTitle], [SAPParentOrgUnit] ,[SAPOrgUnit], [OrgLevelID], [ReportsToEmployeeID], [DottedLineReportsToEmployeeID], [CreatedBy], [SAPChief], [SAP_Object_Abbreviation], [SAP_OrgUnit_Long_Desc]) VALUES (@Flag, @EmployeePositionControlStatusTypeID, @PositionControlNumber, @TemporaryPositionControlNumber, @PCNApprovedDate, @PCNEliminatedDate, @AnticipatedEliminationDate, @BusinessUnitID, @PositionID, @EmployeePositionTypeID, @FTE, @PercentCapitalization, @CapitalLine, @Note, @EmployeeID, @PositionTitle, @SAPParentOrgUnit, @SAPOrgUnit, @OrgLevelID, @ReportsToEmployeeID, @DottedLineReportsToEmployeeID, @CreatedBy, @SAPChief, @SAP_Object_Abbreviation, @SAP_OrgUnit_Long_Desc); SELECT @EmployeePositionControlID = (SELECT @@IDENTITY)" 
        UpdateCommand="UPDATE [EmployeePositionControls] SET [Flag] = @Flag, [EmployeePositionControlStatusTypeID] = @EmployeePositionControlStatusTypeID, [PositionControlNumber] = @PositionControlNumber, [TemporaryPositionControlNumber] = @TemporaryPositionControlNumber, [PCNApprovedDate] = @PCNApprovedDate, [PCNEliminatedDate] = @PCNEliminatedDate, [AnticipatedEliminationDate] = @AnticipatedEliminationDate, [BusinessUnitID] = @BusinessUnitID, [PositionID] = @PositionID, [EmployeePositionTypeID] = @EmployeePositionTypeID, [FTE] = @FTE, [PercentCapitalization] = @PercentCapitalization, [CapitalLine] = @CapitalLine, [Note] = @Note, [EmployeeID] = @EmployeeID, [PositionTitle] = @PositionTitle, [SAPParentOrgUnit] = @SAPParentOrgUnit, [SAPOrgUnit] = @SAPOrgUnit, [OrgLevelID] = @OrgLevelID, [ReportsToEmployeeID] = @ReportsToEmployeeID, [DottedLineReportsToEmployeeID] = @DottedLineReportsToEmployeeID, [SAPChief] = @SAPChief, [SAP_Object_Abbreviation] = @SAP_Object_Abbreviation, [SAP_OrgUnit_Long_Desc] = @SAP_OrgUnit_Long_Desc WHERE [EmployeePositionControlID] = @EmployeePositionControlID"
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
                        empl.Last_Name,
                        emplReports.EmployeeID as ReportsToEmployeeNumber,
                        emplReports.First_Name as ReportsToEmployeeFirstName,
                        emplReports.Last_Name as ReportsToEmployeeLastName,
                        emplDotted.EmployeeID as DottedLineReportsToEmployeeNumber,
                        emplDotted.First_Name as DottedLineReportsToEmployeeFirstName,
                        emplDotted.Last_Name as DottedLineReportsToEmployeeLastName,
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
                    WHERE 
                        epc.[EmployeePositionControlID] = @EmployeePositionControlID" 
        >
        <SelectParameters>
            <asp:Parameter Name="EmployeePositionControlID" Type="Int32" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="EmployeePositionControlID" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="EmployeePositionControlID" Type="Int32" Direction="Output" />
            <asp:Parameter Name="Flag" Type="Boolean" />
            <asp:Parameter Name="EmployeePositionControlStatusTypeID" Type="Int32" />
            <asp:Parameter Name="PositionControlNumber" Type="String" />
            <asp:Parameter Name="TemporaryPositionControlNumber" Type="String" />
            <asp:Parameter Name="PCNApprovedDate" Type="DateTime" />
            <asp:Parameter Name="PCNEliminatedDate" Type="DateTime" />
            <asp:Parameter Name="AnticipatedEliminationDate" Type="DateTime" />
            <asp:Parameter Name="BusinessUnitID" Type="Int32" />
            <asp:Parameter Name="PositionID" Type="Int32" />
            <asp:Parameter Name="EmployeePositionTypeID" Type="Int32" />
            <asp:Parameter Name="FTE" Type="Decimal" />
            <asp:Parameter Name="PercentCapitalization" Type="String" />
            <asp:Parameter Name="CapitalLine" Type="String" />
            <asp:Parameter Name="Note" Type="String" />
            <asp:Parameter Name="EmployeeID" Type="Int32" />
            <asp:Parameter Name="PositionTitle" Type="String" />
            <asp:Parameter Name="SAPParentOrgUnit" Type="String" />
            <asp:Parameter Name="SAPOrgUnit" Type="String" />
            <asp:Parameter Name="OrgLevelID" Type="Int32" />
            <asp:Parameter Name="ReportsToEmployeeID" Type="Int32" />
            <asp:Parameter Name="DottedLineReportsToEmployeeID" Type="Int32" />
            <asp:Parameter Name="CreatedBy" Type="String" />
            <asp:Parameter Name="SAPChief" Type="Boolean" />
            <asp:Parameter Name="SAP_Object_Abbreviation" Type="String" />
            <asp:Parameter Name="SAP_OrgUnit_Long_Desc" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="Flag" Type="Boolean" />
            <asp:Parameter Name="EmployeePositionControlStatusTypeID" Type="Int32" />
            <asp:Parameter Name="PositionControlNumber" Type="String" />
            <asp:Parameter Name="TemporaryPositionControlNumber" Type="String" />
            <asp:Parameter Name="PCNApprovedDate" Type="DateTime" />
            <asp:Parameter Name="PCNEliminatedDate" Type="DateTime" />
            <asp:Parameter Name="AnticipatedEliminationDate" Type="DateTime" />
            <asp:Parameter Name="BusinessUnitID" Type="Int32" />
            <asp:Parameter Name="PositionID" Type="Int32" />
            <asp:Parameter Name="EmployeePositionTypeID" Type="Int32" />
            <asp:Parameter Name="FTE" Type="Decimal" />
            <asp:Parameter Name="PercentCapitalization" Type="String" />
            <asp:Parameter Name="CapitalLine" Type="String" />
            <asp:Parameter Name="Note" Type="String" />
            <asp:Parameter Name="EmployeeID" Type="Int32" />
            <asp:Parameter Name="PositionTitle" Type="String" />
            <asp:Parameter Name="EmployeePositionControlID" Type="Int32" />
            <asp:Parameter Name="SAPParentOrgUnit" Type="String" />
            <asp:Parameter Name="SAPOrgUnit" Type="String" />
            <asp:Parameter Name="OrgLevelID" Type="Int32" />
            <asp:Parameter Name="ReportsToEmployeeID" Type="Int32" />
            <asp:Parameter Name="DottedLineReportsToEmployeeID" Type="Int32" />
            <asp:Parameter Name="SAPChief" Type="Boolean" />
            <asp:Parameter Name="SAP_Object_Abbreviation" Type="String" />
            <asp:Parameter Name="SAP_OrgUnit_Long_Desc" Type="String" />
        </UpdateParameters>
    </DetailsDataSource>
    </mapcall:DetailsViewDataPageTemplate>


</asp:Content>
