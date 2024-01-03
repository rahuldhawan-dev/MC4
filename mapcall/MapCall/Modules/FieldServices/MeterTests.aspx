<%@ Page Title="Meter Tests" Language="C#" AutoEventWireup="true" MasterPageFile="~/MapCallSite.Master" Theme="bender" CodeBehind="MeterTests.aspx.cs" Inherits="MapCall.Modules.FieldServices.MeterTests" EnableEventValidation="false" %>
<%@ Register Src="~/Controls/HR/MeterTestResults.ascx" TagName="MeterTestResults" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/ddlMcProdOperatingCenter.ascx" TagName="ddlMcProdOperatingCenter" TagPrefix="mmsi" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>
<%@ Register Src="~/Controls/ClientIDRepository.ascx" TagName="ClientIDRepository" TagPrefix="mmsinc"  %>
<%@ Register assembly="MMSINC" namespace="MMSINC" tagPrefix="mmsinc" %>

<asp:Content ContentPlaceHolderID="cphHeadTag" runat="server">
</asp:Content>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphHeader">
    Meter Tests
</asp:Content>

<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cphMain">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch" CssClass="searchPanel">
        <table style="text-align:left;" border="0">
                <tr>
                    <td class="leftcol">Operating Center : </td>
                    <td class="rightcol">
                        <mmsi:ddlMcProdOperatingCenter runat="server" id="ddlOpCntr" BaseRole="_Field Services_Meters_Read" />
                    </td>
                </tr>
                
                <tr>
                    <td class="leftcol">Town : </td>
                    <td class="rightcol">
                        <asp:DropDownList runat="server" ID="ddlTown" />
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
                    </td>
                </tr>
                <tr>
                    <td class="leftcol">Street :</td>
                    <td class="rightcol">
                        <asp:DropDownList runat="server" ID="ddlStreet" />
                        <atk:CascadingDropDown runat="server" ID="cddStreets"   
                            TargetControlID="ddlStreet"
                            ParentControlID="ddlTown"
                            Category="Street"
                            EmptyText="None Found" EmptyValue=""
                            PromptText="--Select Here--" PromptValue=""
                            LoadingText="[Loading Streets...]"
                            ServicePath="~/Modules/Data/DropDowns.asmx" 
                            ServiceMethod="GetStreetsByTown"
                            SelectedValue='<%# Bind("StreetID") %>' 
                        />
                    </td>
                </tr>  
            <mmsi:DataField runat="server" ID="dfMeterID" DataType="Integer" DataFieldName="MeterID" HeaderText="MeterID:" />
            <mmsi:DataField runat="server" ID="dfMeterProfileID" DataType="Integer" DataFieldName="MeterProfileID" HeaderText="MeterProfileID:" />
            <mmsi:DataField runat="server" ID="dfDateSurveyed" DataType="Date" DataFieldName="DateSurveyed" HeaderText="Date Surveyed:" />
            <mmsi:DataField runat="server" ID="dfSiteIssue" DataType="BooleanDropDown" DataFieldName="SiteIssue" HeaderText="SiteIssue:" />
            <mmsi:DataField runat="server" ID="dfDateScheduledForTest" DataType="Date" DataFieldName="DateScheduledForTest" HeaderText="DateScheduledForTest:" />
            <mmsi:DataField runat="server" ID="dfAssignedToContractorID" 
                DataType="DropDownList" 
                DataFieldName="AssignedToContractorID" 
                HeaderText="Assigned To Contractor:" 
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="Select MeterContractorID as val, Description as txt from MeterContractors"
            />
            <mmsi:DataField runat="server" ID="dfTestByEmployeeID" DataType="String" DataFieldName="TestedByEmployeeID" HeaderText="Tested By EmployeeID:" />
            <mmsi:DataField runat="server" ID="dfTestByEmployeeLastName" DataType="String" DataFieldName="TestedByEmployeeLastName" HeaderText="Tested By Employee Last Name:" />
            <mmsi:DataField runat="server" ID="dfTestedByContractorID" 
                DataType="DropDownList" 
                DataFieldName="TestedByContractorID" 
                HeaderText="Tested By Contractor:" 
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="Select MeterContractorID as val, Description as txt from MeterContractors"
            />
            <mmsi:DataField runat="server" ID="dfDateTested" DataType="Date" DataFieldName="DateTested" HeaderText="DateTested:" />
            <mmsi:DataField runat="server" ID="dfInletValveOperational" DataType="BooleanDropDown" DataFieldName="InletValveOperational" HeaderText="InletValveOperational:" />
            <mmsi:DataField runat="server" ID="dfMeterTestPassed" DataType="BooleanDropDown" DataFieldName="Meter Test Passed" HeaderText="Meter Test Passed:" />
            <mmsi:DataField runat="server" ID="dfIsInterconnectMeter" DataType="BooleanDropDown" DataFieldName="isInterconnectMeter" HeaderText="Interconnect Meter" />
            <tr>
                <td></td>
                <td>
                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                    <asp:Button runat="server" ID="btnReset" Text="Reset" OnClick="btnReset_Click" />
                    <asp:Button runat="server" ID="btnAdd" Text="Add" CausesValidation="False" OnClick="btnAdd_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel runat="server" ID="pnlResults" Visible="false">
        <asp:HiddenField runat="server" ID="hidFilter" />
        <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Export" />
        <asp:Button runat="server" ID="btnMap" Visible="true" PostBackUrl="~/Modules/Maps/Maps.aspx" Text="Map" />
        <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
        <asp:Label runat="server" ID="lblRecordCount"></asp:Label>
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" 
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="MeterTestID" AllowSorting="true"
            AutoGenerateColumns="false" EnableViewState="false"
            >
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
                
                <%--There aren't any InsertVisible/ReadOnly properties set on these fields because 
                    they have no impact when only displaying data. There's also no Insert/Update/Delete
                    statements attached to this GridView's SqlDataSource. If that changes at some point,
                    then all the BoundFields need to have InsertVisible="false" and ReadOnly="true" set
                    on them.--%>
                <asp:BoundField HeaderText="MeterTestID" DataField="MeterTestID" SortExpression="MeterTestID" />
                <asp:BoundField HeaderText="MeterID" DataField="MeterID" SortExpression="MeterID" />
                <asp:BoundField HeaderText="Operating Center" DataField="OperatingCenterCode" SortExpression="OperatingCenterCode" />
                <asp:CheckBoxField HeaderText="Interconnect Meter" DataField="isInterconnectMeter" SortExpression="isInterconnectMeter" />
                <asp:BoundField HeaderText="Premise #" DataField="PremiseNumber" SortExpression="PremiseNumber" />
                <asp:BoundField HeaderText="Service Address House #" DataField="ServiceAddressHouseNumber" SortExpression="ServiceAddressHouseNumber" />
                <asp:BoundField HeaderText="Service Address Apartment #" DataField="ServiceAddressApartment" SortExpression="ServiceAddressApartment" />
                <asp:BoundField HeaderText="Street Name" DataField="ServiceAddressStreet" SortExpression="ServiceAddressStreet" />
                <asp:BoundField HeaderText="Town" DataField="Town" SortExpression="Town" />
                <asp:BoundField HeaderText="State" DataField="State" SortExpression="State" />
                <asp:BoundField HeaderText="Zip" DataField="ServiceZip" SortExpression="ServiceZip" />
                <asp:BoundField HeaderText="CoordinateID" DataField="CoordinateID" SortExpression="CoordinateID" />
                
                <asp:BoundField HeaderText="Meter Test Comparison Meter ID" DataField="MeterTestComparisonMeterID" SortExpression="MeterTestComparisonMeterID" />
                <asp:BoundField HeaderText="Date Surveyed" DataField="DateSurveyed" SortExpression="DateSurveyed" />
                <asp:CheckBoxField HeaderText="Site Issue" DataField="SiteIssue" SortExpression="SiteIssue" />
                <asp:CheckBoxField HeaderText="Inlet Valve Operational" DataField="InletValveOperational" SortExpression="InletValveOperational" />
                <asp:CheckBoxField HeaderText="Outlet Valve Operational" DataField="OutletValveOperational" SortExpression="OutletValveOperational" />
                <asp:BoundField HeaderText="Date Scheduled For Test" DataField="DateScheduledForTest" SortExpression="DateScheduledForTest" />
                <asp:BoundField HeaderText="Assigned To ContractorID" DataField="AssignedToContractorID" SortExpression="AssignedToContractorID" />
                
                <asp:BoundField HeaderText="Date Tested" DataField="DateTested" SortExpression="DateTested" />
                <asp:BoundField HeaderText="Tested By EmployeeID" DataField="TestedByEmployeeID" SortExpression="TestedByEmployeeID" />
                <asp:BoundField HeaderText="Tested By Employee Lastname" DataField="TestedByEmployeeLastName" SortExpression="TestedByEmployeeLastName" />
                <asp:BoundField HeaderText="Tested By ContractorID" DataField="TestedByContractorID" SortExpression="TestedByContractorID" />
                <asp:BoundField HeaderText="Tested By Contractor Employee Name" DataField="TestedByContractorEmployeeName" SortExpression="TestedByContractorEmployeeName" />
                <asp:BoundField HeaderText="Test Comments" DataField="TestComments" SortExpression="TestComments" />
               
                <asp:BoundField HeaderText="Meter Reading Before Low" DataField="MeterReadingBeforeLow" SortExpression="MeterReadingBeforeLow" />
                <asp:BoundField HeaderText="Meter Reading Before High" DataField="MeterReadingBeforeHigh" SortExpression="MeterReadingBeforeHigh" />
                <asp:BoundField HeaderText="Meter Reading Before Fire" DataField="MeterReadingBeforeFire" SortExpression="MeterReadingBeforeFire" />
                <asp:BoundField HeaderText="Meter Reading After Low" DataField="MeterReadingAfterLow" SortExpression="MeterReadingAfterLow" />
                <asp:BoundField HeaderText="Meter Reading After High" DataField="MeterReadingAfterHigh" SortExpression="MeterReadingAfterHigh" />
                <asp:BoundField HeaderText="Meter Reading After Fire" DataField="MeterReadingAfterFire" SortExpression="MeterReadingAfterFire" />
                <asp:BoundField HeaderText="Hydrostatic Pressure" DataField="HydrostaticPressure" SortExpression="HydrostaticPressure" />
                <asp:BoundField HeaderText="Residual Pressure" DataField="ResidualPressure" SortExpression="ResidualPressure" />
                <asp:CheckBoxField HeaderText="Meter Test Passed" DataField="MeterTestPassed" SortExpression="MeterTestPassed" />

            </Columns>
        </asp:GridView>
        <asp:SqlDataSource EnableViewState="false" ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
           SelectCommand="
                select 
	                mt.[MeterTestID],
	                p.[PremiseNumber],
	                o.OperatingCenterCode,
                    m.IsInterconnectMeter,
	                t.Town,
	                p.ServiceZip,
	                p.ServiceAddressHouseNumber, 
	                p.ServiceAddressApartment,
	                p.ServiceAddressStreet,
                    state.Abbreviation as State,
	                p.CoordinateID,
	                mt.[MeterTestComparisonMeterID],
	                mt.[DateSurveyed],
	                mt.[SiteIssue],
	                mt.[InletValveOperational],
	                mt.[OutletValveOperational],
	                mt.[DateScheduledForTest],
	                mt.[AssignedToContractorID],
	                mt.[DateTested],
	                mt.[TestedByEmployeeID],
	                mt.[TestedByEmployeeLastName],
	                mt.[TestedByContractorID],
	                mt.[TestedByContractorEmployeeName],
	                mt.[TestComments],
	                mt.[MeterReadingBeforeLow],
	                mt.[MeterReadingBeforeHigh],
	                mt.[MeterReadingBeforeFire],
	                mt.[MeterReadingAfterLow],
	                mt.[MeterReadingAfterHigh],
	                mt.[MeterReadingAfterFire],
	                mt.[HydrostaticPressure],
	                mt.[ResidualPressure],
	                mt.[MeterTestPassed],
	                p.OperatingCenterID, 
	                t.TownID,
                    M.MeterID
                from
	                MeterTests mt
	            left join 
	                Meters m
	            on
	                m.MeterID = mt.MeterID
                left join
	                Premises p
                on
	                p.Id = m.premiseID
                left join
	                Towns t
                on
	                t.TownID = p.ServiceCityId
                LEFT JOIN
                    [States] state
                ON
                    p.ServiceStateId = state.StateID
                left join
	                OperatingCenters o
                on
	                o.OperatingCenterID = p.OperatingCenterID
           "
        >
        </asp:SqlDataSource>
    </asp:Panel>
    
    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <div class="tabsContainer">
            <ul class="ui-tabs-nav">
                <li><a href="#meterTest" class="tab"><span>Meter Test</span></a></li>
                <li><a href="#meterTestResults" class="tab"><span>Meter Test Results</span></a></li>
                <li><a href="#previousTests"><span>Previous Tests</span></a></li>
                <li><a href="#premiseTests"><span>Premise Tests</span></a></li>
                <li><a href="#meterTests"><span>Meter Tests</span></a></li>
                <li><a href="#notes" class="tab"><span>Notes</span></a></li>
                <li><a href="#documents" class="tab"><span>Documents</span></a></li>
            </ul>
            
            <div id="meterTest">
                <asp:DetailsView ID="dvMeterTest" runat="server" AutoGenerateRows="False" 
                    DataKeyNames="MeterTestID" DataSourceID="dsMeterTest" 
                    OnItemInserting="DetailView_ItemInserting"
                    OnItemInserted="DetailView_ItemInserted"
                    OnItemUpdating="DetailView_ItemUpdating"
                    OnDataBound="DetailView_DataBound" 
                    Width="100%">
                    <Fields>
                        <asp:BoundField DataField="MeterTestID" HeaderText="MeterTestID:" 
                            InsertVisible="False" ReadOnly="True" SortExpression="MeterTestID" />
                        <asp:TemplateField HeaderText="Serial #:">
                            <ItemTemplate><%#Eval("SerialNumber")%></ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList runat="server" ID="ddlMeterTestID"
                                    DataSourceID="dsMeters"
                                    DataTextField="txt"
                                    DataValueField="MeterID"
                                    AppendDataBoundItems="true"
                                    SelectedValue='<%#Bind("MeterID") %>'
                                >
                                    <asp:ListItem Value="" Text="--Select Here--" />
                                </asp:DropDownList>
                                <asp:SqlDataSource runat="server" ID="dsMeters" 
                                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                                    SelectCommand="Select MeterID, isNull(serialNumber,'[No Serial]') as txt from Meters"
                                />
                                <asp:RequiredFieldValidator runat="server" ID="rfvMeterTestID" ControlToValidate="ddlMeterTestID" Text="Required" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Meter Test Comparison Meter ID">
                            <ItemTemplate><%#Eval("MeterTestComparisonMeterID") %></ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList runat="server" ID="ddlMeterTestComparisonMeterID"
                                    DataSourceID="dsMeterTestComparisonMeters"
                                    DataTextField="txt"
                                    DataValueField="MeterTestComparisonMeterID"
                                    AppendDataBoundItems="true"
                                    SelectedValue='<%#Bind("MeterTestComparisonMeterID") %>'
                                >
                                    <asp:ListItem Value="" Text="--Select Here--" />
                                </asp:DropDownList>
                                <asp:SqlDataSource runat="server" ID="dsMeterTestComparisonMeters" 
                                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                                    SelectCommand="Select MeterTestComparisonMeterID, cast(MeterTestComparisonMeterID as varchar) + ',' + isNull(serialNumber,'') as txt from MeterTestComparisonMeters"
                                />
                            </EditItemTemplate>
                        </asp:TemplateField>
                       
                        <asp:BoundField DataField="PremiseNumber" HeaderText="Premise #:" SortExpression="PremiseNumber" ReadOnly="true" InsertVisible="false" />
                        <asp:TemplateField HeaderText="Premise Service Address">
                            <ItemTemplate>
                                <div><%#Eval("ServiceAddressHouseNumber")%> <%#Eval("ServiceAddressStreet")%></div>
                                
                                <%--Hide this if apartment number is null?--%>
                                <div><%# DataBinder.Eval(Container.DataItem, "ServiceAddressApartment", "APT #: {0}") %></div>
                                <div><%#Eval("Town") %>, <%#Eval("State") %>, <%#Eval("ServiceZip") %></div>
                            </ItemTemplate>
                        </asp:TemplateField>
                       
                        <asp:BoundField DataField="OperatingCenterCode" HeaderText="Operating Center:" SortExpression="OperatingCenterCode"  ReadOnly="true" InsertVisible="false"/>
                        <mmsinc:BoundField DataField="CoordinateID" HeaderText="CoordinateID:" SortExpression="CoordinateID" ReadOnly="true" InsertVisible="false" />
                        
                        <mmsinc:BoundField DataField="DateSurveyed" HeaderText="Date Surveyed:" 
                            SortExpression="DateSurveyed" Required="true" SqlDataType="DateTime" />
                        <asp:CheckBoxField DataField="SiteIssue" HeaderText="Site Issue:" 
                            SortExpression="SiteIssue" />
                        <asp:CheckBoxField DataField="InletValveOperational" 
                            HeaderText="Inlet Valve Operational:" SortExpression="InletValveOperational" />
                        <asp:CheckBoxField DataField="OutletValveOperational" 
                            HeaderText="Outlet Valve Operational:" SortExpression="OutletValveOperational" />
                        <mmsinc:BoundField DataField="DateScheduledForTest" Required="true" SqlDataType="DateTime"
                            HeaderText="Date Scheduled For Test:" SortExpression="DateScheduledForTest" />
                        <asp:TemplateField HeaderText="Assigned To Contractor:">
                            <ItemTemplate><%#Eval("AssignedToContractorID")%></ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList runat="server" ID="ddlAssignedToContractorID"
                                    DataSourceID="dsMeterContractors"
                                    DataTextField="txt"
                                    DataValueField="MeterContractorID"
                                    AppendDataBoundItems="true"
                                    SelectedValue='<%#Bind("AssignedToContractorID") %>'
                                >
                                    <asp:ListItem Value="" Text="--Select Here--" />
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>                
                        <mmsinc:BoundField DataField="DateTested" HeaderText="Date Tested:" 
                            SortExpression="DateTested" SqlDataType="DateTime" />
                        <asp:BoundField DataField="TestedByEmployeeID" HeaderText="Tested By EmployeeID:" 
                            SortExpression="TestedByEmployeeID" />
                        <asp:BoundField DataField="TestedByEmployeeLastName" 
                            HeaderText="Tested By Employee Last Name:" 
                            SortExpression="TestedByEmployeeLastName" />
                            
                        <asp:TemplateField HeaderText="Tested By Contractor:">
                            <ItemTemplate><%#Eval("TestedByContractorID")%></ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList runat="server" ID="ddlTestedByContractorID"
                                    DataSourceID="dsMeterContractors"
                                    DataTextField="txt"
                                    DataValueField="MeterContractorID"
                                    AppendDataBoundItems="true"
                                    SelectedValue='<%#Bind("TestedByContractorID") %>'
                                >
                                    <asp:ListItem Value="" Text="--Select Here--" />
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:BoundField DataField="TestedByContractorEmployeeName" 
                            HeaderText="Tested By Contractor Employee Name:" 
                            SortExpression="TestedByContractorEmployeeName" />
                        <asp:BoundField DataField="TestComments" HeaderText="TestComments" 
                            SortExpression="TestComments" />
                        <asp:BoundField DataField="MeterReadingBeforeLow" 
                            HeaderText="Meter Reading Before Low:" SortExpression="MeterReadingBeforeLow" />
                        <asp:BoundField DataField="MeterReadingBeforeHigh" 
                            HeaderText="Meter Reading Before High:" SortExpression="MeterReadingBeforeHigh" />
                        <asp:BoundField DataField="MeterReadingBeforeFire" 
                            HeaderText="Meter Reading Before Fire:" SortExpression="MeterReadingBeforeFire" />
                        <asp:BoundField DataField="MeterReadingAfterLow" 
                            HeaderText="Meter Reading After Low:" SortExpression="MeterReadingAfterLow" />
                        <asp:BoundField DataField="MeterReadingAfterHigh" 
                            HeaderText="Meter Reading After High:" SortExpression="MeterReadingAfterHigh" />
                        <asp:BoundField DataField="MeterReadingAfterFire" 
                            HeaderText="Meter Reading After Fire:" SortExpression="MeterReadingAfterFire" />
                        <asp:BoundField DataField="HydrostaticPressure" 
                            HeaderText="Hydrostatic Pressure:" SortExpression="HydrostaticPressure" />
                        <asp:BoundField DataField="ResidualPressure" HeaderText="Residual Pressure:" 
                            SortExpression="ResidualPressure" />
                        <asp:CheckBoxField DataField="MeterTestPassed" HeaderText="Meter Test Passed:" 
                            SortExpression="MeterTestPassed" />
                        <asp:CheckBoxField DataField="IsInterconnectMeter" HeaderText="Interconnect Meter" ReadOnly="True" InsertVisible="False"/>
                        <asp:TemplateField ShowHeader="False">
                            <EditItemTemplate>
                                <asp:LinkButton ID="btnUpdate" runat="server" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton>
                                <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:LinkButton ID="btnInsert" runat="server" CausesValidation="True" CommandName="Insert" Text="Insert"></asp:LinkButton>
                                <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="false" CommandName="Cancel" Text="Cancel" OnClick="btnCancelInsert_Click"></asp:LinkButton>
                            </InsertItemTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="False" Visible="false" CommandName="Edit" Text="Edit"></asp:LinkButton>
                                <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False" Visible="false" CommandName="Delete" Text="Delete" OnClientClick="return confirm('Are you sure?');"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
                
                <asp:SqlDataSource runat="server" ID="dsMeterContractors" 
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    SelectCommand="Select MeterContractorID, Description as txt from MeterContractors"
                />
                <asp:SqlDataSource ID="dsMeterTest" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:MCProd %>" 
                    OnInserted="SqlDataSource1_Inserted"
                    DeleteCommand="DELETE FROM [MeterTests] WHERE [MeterTestID] = @MeterTestID" 
                    InsertCommand="INSERT INTO [MeterTests] ([MeterID], [MeterTestComparisonMeterID], [DateSurveyed], [SiteIssue], [InletValveOperational], [OutletValveOperational], [DateScheduledForTest], [AssignedToContractorID], [DateTested], [TestedByEmployeeID], [TestedByEmployeeLastName], [TestedByContractorID], [TestedByContractorEmployeeName], [TestComments], [MeterReadingBeforeLow], [MeterReadingBeforeHigh], [MeterReadingBeforeFire], [MeterReadingAfterLow], [MeterReadingAfterHigh], [MeterReadingAfterFire], [HydrostaticPressure], [ResidualPressure], [MeterTestPassed], [CreatedBy]) VALUES (@MeterID, @MeterTestComparisonMeterID, @DateSurveyed, @SiteIssue, @InletValveOperational, @OutletValveOperational, @DateScheduledForTest, @AssignedToContractorID, @DateTested, @TestedByEmployeeID, @TestedByEmployeeLastName, @TestedByContractorID, @TestedByContractorEmployeeName, @TestComments, @MeterReadingBeforeLow, @MeterReadingBeforeHigh, @MeterReadingBeforeFire, @MeterReadingAfterLow, @MeterReadingAfterHigh, @MeterReadingAfterFire, @HydrostaticPressure, @ResidualPressure, @MeterTestPassed, @CreatedBy);Select @MeterTestID = (Select @@IDENTITY)" 
                    UpdateCommand="UPDATE [MeterTests] SET [MeterID] = @MeterID, [MeterTestComparisonMeterID] = @MeterTestComparisonMeterID, [DateSurveyed] = @DateSurveyed, [SiteIssue] = @SiteIssue, [InletValveOperational] = @InletValveOperational, [OutletValveOperational] = @OutletValveOperational, [DateScheduledForTest] = @DateScheduledForTest, [AssignedToContractorID] = @AssignedToContractorID, [DateTested] = @DateTested, [TestedByEmployeeID] = @TestedByEmployeeID, [TestedByEmployeeLastName] = @TestedByEmployeeLastName, [TestedByContractorID] = @TestedByContractorID, [TestedByContractorEmployeeName] = @TestedByContractorEmployeeName, [TestComments] = @TestComments, [MeterReadingBeforeLow] = @MeterReadingBeforeLow, [MeterReadingBeforeHigh] = @MeterReadingBeforeHigh, [MeterReadingBeforeFire] = @MeterReadingBeforeFire, [MeterReadingAfterLow] = @MeterReadingAfterLow, [MeterReadingAfterHigh] = @MeterReadingAfterHigh, [MeterReadingAfterFire] = @MeterReadingAfterFire, [HydrostaticPressure] = @HydrostaticPressure, [ResidualPressure] = @ResidualPressure, [MeterTestPassed] = @MeterTestPassed WHERE [MeterTestID] = @MeterTestID"
                    SelectCommand="
                        select 
	                        mt.[MeterTestID],
	                        mt.[MeterID],
	                        m.SerialNumber,
	                        p.[PremiseNumber],
	                        p.PremiseNumber,
	                        o.OperatingCenterCode,
	                        t.Town,
                            state.Abbreviation as State,
                            p.ServiceZip,
	                        p.ServiceAddressHouseNumber, 
                            p.ServiceAddressApartment,
                            p.ServiceAddressStreet,
	                        mt.[MeterTestComparisonMeterID],
	                        mt.[DateSurveyed],
	                        mt.[SiteIssue],
	                        mt.[InletValveOperational],
	                        mt.[OutletValveOperational],
	                        mt.[DateScheduledForTest],
	                        mt.[AssignedToContractorID],
	                        mt.[DateTested],
	                        mt.[TestedByEmployeeID],
	                        mt.[TestedByEmployeeLastName],
	                        mt.[TestedByContractorID],
	                        mt.[TestedByContractorEmployeeName],
	                        mt.[TestComments],
	                        mt.[MeterReadingBeforeLow],
	                        mt.[MeterReadingBeforeHigh],
	                        mt.[MeterReadingBeforeFire],
	                        mt.[MeterReadingAfterLow],
	                        mt.[MeterReadingAfterHigh],
	                        mt.[MeterReadingAfterFire],
	                        mt.[HydrostaticPressure],
	                        mt.[ResidualPressure],
	                        mt.[MeterTestPassed],
	                        p.OperatingCenterID, 
	                        t.TownID,
	                        p.CoordinateID,
                            m.IsInterconnectMeter
                        from
	                        MeterTests mt
	                    left join
	                        Meters m
	                    on
	                        m.MeterID = mt.MeterID
                        left join
	                        Premises p
                        on
	                        p.Id = m.premiseID
                        left join
	                        Towns t
                        on
	                        t.TownID = p.ServiceCityId
	                    LEFT JOIN
                            [States] state
                        ON
                            p.ServiceStateId = state.StateID
                        left join
	                        OperatingCenters o
                        on
	                        o.OperatingCenterID = p.OperatingCenterID
                        where
	                        [MeterTestID] = @MeterTestID
	                        ">
                    <DeleteParameters>
                        <asp:Parameter Name="MeterTestID" Type="Int32" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:Parameter Name="MeterTestID" Type="Int32" />
                    </SelectParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="MeterID" Type="Int32" />
                        <asp:Parameter Name="MeterTestComparisonMeterID" Type="Int32" />
                        <asp:Parameter Name="DateSurveyed" Type="DateTime" />
                        <asp:Parameter Name="SiteIssue" Type="Boolean" />
                        <asp:Parameter Name="InletValveOperational" Type="Boolean" />
                        <asp:Parameter Name="OutletValveOperational" Type="Boolean" />
                        <asp:Parameter Name="DateScheduledForTest" Type="DateTime" />
                        <asp:Parameter Name="AssignedToContractorID" Type="Int32" />
                        <asp:Parameter Name="DateTested" Type="DateTime" />
                        <asp:Parameter Name="TestedByEmployeeID" Type="String" />
                        <asp:Parameter Name="TestedByEmployeeLastName" Type="String" />
                        <asp:Parameter Name="TestedByContractorID" Type="Int32" />
                        <asp:Parameter Name="TestedByContractorEmployeeName" Type="String" />
                        <asp:Parameter Name="TestComments" Type="String" />
                        <asp:Parameter Name="MeterReadingBeforeLow" Type="String" />
                        <asp:Parameter Name="MeterReadingBeforeHigh" Type="String" />
                        <asp:Parameter Name="MeterReadingBeforeFire" Type="String" />
                        <asp:Parameter Name="MeterReadingAfterLow" Type="String" />
                        <asp:Parameter Name="MeterReadingAfterHigh" Type="String" />
                        <asp:Parameter Name="MeterReadingAfterFire" Type="String" />
                        <asp:Parameter Name="HydrostaticPressure" Type="Double" />
                        <asp:Parameter Name="ResidualPressure" Type="Double" />
                        <asp:Parameter Name="MeterTestPassed" Type="Boolean" />
                        <asp:Parameter Name="MeterTestID" Type="Int32" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:Parameter Name="MeterTestID" Type="Int32" Direction="Output" />
                        <asp:Parameter Name="MeterID" Type="Int32" />
                        <asp:Parameter Name="MeterTestComparisonMeterID" Type="Int32" />
                        <asp:Parameter Name="DateSurveyed" Type="DateTime" />
                        <asp:Parameter Name="SiteIssue" Type="Boolean" />
                        <asp:Parameter Name="InletValveOperational" Type="Boolean" />
                        <asp:Parameter Name="OutletValveOperational" Type="Boolean" />
                        <asp:Parameter Name="DateScheduledForTest" Type="DateTime" />
                        <asp:Parameter Name="AssignedToContractorID" Type="Int32" />
                        <asp:Parameter Name="DateTested" Type="DateTime" />
                        <asp:Parameter Name="TestedByEmployeeID" Type="String" />
                        <asp:Parameter Name="TestedByEmployeeLastName" Type="String" />
                        <asp:Parameter Name="TestedByContractorID" Type="Int32" />
                        <asp:Parameter Name="TestedByContractorEmployeeName" Type="String" />
                        <asp:Parameter Name="TestComments" Type="String" />
                        <asp:Parameter Name="MeterReadingBeforeLow" Type="String" />
                        <asp:Parameter Name="MeterReadingBeforeHigh" Type="String" />
                        <asp:Parameter Name="MeterReadingBeforeFire" Type="String" />
                        <asp:Parameter Name="MeterReadingAfterLow" Type="String" />
                        <asp:Parameter Name="MeterReadingAfterHigh" Type="String" />
                        <asp:Parameter Name="MeterReadingAfterFire" Type="String" />
                        <asp:Parameter Name="HydrostaticPressure" Type="Double" />
                        <asp:Parameter Name="ResidualPressure" Type="Double" />
                        <asp:Parameter Name="MeterTestPassed" Type="Boolean" />
                        <asp:Parameter Name="CreatedBy" Type="String" />
                    </InsertParameters>
                </asp:SqlDataSource>
            </div>
            
            <div id="meterTestResults">
                <mmsi:MeterTestResults id="MeterTestResults1" runat="server" />
            </div>

            <div id="previousTests">
                <asp:GridView runat="server" ID="gvPreviousTests" DataSourceID="dsPreviousTests">
                    <Columns>
                        <asp:HyperLinkField DataNavigateUrlFormatString="MeterTests.aspx?arg={0}" DataNavigateUrlFields="MeterTestID"
                            Text="View" 
                        />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource runat="server" ID="dsPreviousTests" ConnectionString="<%$ ConnectionStrings : MCprod %>"
                    SelectCommand="
                        select 
	                        mt.* 
                        from 
	                        MeterTests mt
	                    left join
	                        Meters m
	                    on
	                        m.MeterID = mt.MeterID 
                        where 
	                        mt.MeterID = (Select MeterID from MeterTests where MeterTestID = @MeterTestID)
	                    and
	                        m.PremiseID = (
					                        SELECT 
						                        m2.PremiseID
					                        FROM 
						                        MeterTests mt2 
					                        LEFT JOIN
						                        Meters m2
					                        ON
						                        m2.MeterID = mt2.MeterID
					                        WHERE 
						                        mt2.MeterTestID = @MeterTestID
				                           )
                        and
	                        MeterTestID &lt;&gt; @MeterTestID
                    ">
                    <SelectParameters>
                        <asp:Parameter Name="MeterTestID" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>

            <div id="premiseTests">
                <asp:GridView runat="server" ID="gvPremiseTests" DataSourceID="dsPremiseTests">
                    <Columns>
                        <asp:HyperLinkField DataNavigateUrlFormatString="MeterTests.aspx?arg={0}" DataNavigateUrlFields="MeterTestID"
                            Text="View" 
                        />
                    </Columns>
                </asp:GridView>


              <%-- dsPremiseTests.SelectCommand is supposed to return all other MeterTests
                   with the same Meter--PremiseID as the @MeterTestID parameter. Hence
                   the big wacky inner select. --%>
               <asp:SqlDataSource runat="server" ID="dsPremiseTests" ConnectionString="<%$ ConnectionStrings : MCprod %>"
                    SelectCommand="
                        SELECT 
                            mt.*
                        FROM 
                            MeterTests mt 
                        LEFT JOIN
	                        Meters m
                        ON
	                        m.MeterID = mt.MeterID
                        WHERE 
	                        m.PremiseID = (
					                        SELECT 
						                        m2.PremiseID
					                        FROM 
						                        MeterTests mt2 
					                        LEFT JOIN
						                        Meters m2
					                        ON
						                        m2.MeterID = mt2.MeterID
					                        WHERE 
						                        mt2.MeterTestID = @MeterTestID
				                           )
                        AND
                            MeterTestID &lt;&gt; @MeterTestID
                    ">
                    <SelectParameters>
                        <asp:Parameter Name="MeterTestID" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
            
            <div id="meterTests">
                <asp:GridView runat="server" ID="gvMeterTests" DataSourceID="dsMeterTests">
                    <Columns>
                        <asp:HyperLinkField DataNavigateUrlFormatString="MeterTests.aspx?arg={0}" DataNavigateUrlFields="MeterTestID"
                            Text="View" 
                        />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource runat="server" ID="dsMeterTests" ConnectionString="<%$ ConnectionStrings : MCprod %>"
                    SelectCommand="
                        select 
	                        * 
                        from 
	                        MeterTests 
                        where 
	                        MeterID = (Select MeterID from MeterTests where MeterTestID = @MeterTestID)
                        and
	                        MeterTestID &lt;&gt; @MeterTestID
	                    " >
                    <SelectParameters>
                        <asp:Parameter Name="MeterTestID" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
            
            <div id="notes">
                <mmsi:Notes ID="Notes1" runat="server" DataTypeID="130" />
            </div>
            
            <div id="documents">
                <mmsi:Documents ID="Documents1" runat="server" DataTypeID="130" />
            </div>
        </div>
        
        <center>
            <asp:Button runat="server" ID="Button4" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="Button5" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
    </asp:Panel>
    
    <mmsinc:ClientIDRepository runat="server" id="clientIDRepository" />

    
</asp:Content>