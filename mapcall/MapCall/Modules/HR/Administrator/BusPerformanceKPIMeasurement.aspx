<%@ Page Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="BusPerformanceKPIMeasurement.aspx.cs" Inherits="MapCall.Modules.HR.Administrator.BusPerformanceKPIMeasurement" Title="KPI Measurements" Theme="bender" EnableEventValidation="false" %>
<%@ Register Src="~/Controls/ChartWithSettings.ascx" TagName="ChartWithSettings" TagPrefix="uc1" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.DropDowns" TagPrefix="mapcall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.SearchFields" TagPrefix="search" %>
<%@ Register Assembly="MMSINC.Core.WebForms" Namespace="MMSINC.DataPages" TagPrefix="mmsinc" %>
<%@ Register Src="~/Controls/DetailsViewDataPageTemplate.ascx" TagPrefix="mapcall" TagName="DetailsViewDataPageTemplate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
Business Performance KPI Measurements
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    
    <mapcall:DetailsViewDataPageTemplate ID="template" runat="server" 
        DataTypeId="61"
        DataElementTableName="tblBusinessPerformance_KPI_Measurement" 
        DataElementPrimaryFieldName="KPI_Measurement_ID"
        Label="KPI Measurement">
        <SearchBox>
            <Fields>
                <search:DropDownSearchField 
                    Label="KPI ID"
                    SelectMode="Multiple"
                    DataFieldName="t1.KPI_ID"
                    TextFieldName="txt"
                    ValueFieldName="val"
                    SelectCommand="select KPI_ID as Val, cast(KPI_ID as varchar) + IsNull(', ' + KPI_Measurement, '') as txt from tblBusinessPerformance_KPI order by KPI_Measurement"
                />
                <search:DropDownSearchField 
                    Label="KPI Measurement Category"
                    DataFieldName="KPIMeasurementCategory"
                    TextFieldName="txt"
                    ValueFieldName="val"
                    SelectCommand="select distinct lookupID as Val, lookupvalue as txt from Lookup where LookupType='KPIMeasurementCategory'"
                />
                <search:NumericSearchField SearchType="Range" DataFieldName="KPIYear" />
                <search:TemplatedSearchField 
                    Label="Operating Center"
                    DataFieldName="OperatingCenterCode"
                    BindingControlID="searchOperatingCenter"
                    BindingDataType="String"
                    BindingPropertyName="SelectedValue">
                    <Template>
                        <mapcall:DataSourceDropDownList ID="searchOperatingCenter" runat="server" 
                            EnableCaching="true"
                            TextFieldName="txt"
                            ValueFieldName="Val"
                            SelectCommand="Select OperatingCenterCode as Val, OperatingCenterCode + ' - ' + OperatingCenterName as txt from OperatingCenters order by 2"
                        />
                    </Template>
                </search:TemplatedSearchField>
                <search:DropDownSearchField 
                    Label="BU" 
                    DataFieldName="t.BU"
                    TableName="BusinessUnits"
                    TextFieldName="BU"
                    ValueFieldName="BusinessUnitID"
                />
                <search:DropDownSearchField 
                    Label="Employee Responsible"
                    DataFieldName="te.tblEmployeeID"
                    TextFieldName="txt"
                    ValueFieldName="val"
                    SelectCommand="SELECT 
                                        distinct tblEmployeeID as val, 
                                        replace(isNull(Last_Name,'') + ', ' + isNull(First_name, '') + ' ' + isNull(Middle_Name,'') + ' - '  + isNull(employeeID,''),'  ', ' ') as [txt], 
                                        Last_Name 
                                   from tblEmployee order by Last_Name"
                />
                <search:NumericSearchField DataFieldName="Grouping" SearchType="Double" />
            </Fields>
        </SearchBox>
    
        <ResultsGridView AllowSorting="true" AllowPaging="true" PageSize="500" AutoGenerateColumns="true">
          
        </ResultsGridView>
        
        <ResultsPlaceHolder>
            <uc1:ChartWithSettings runat="server" id="cws" Visible="true" />
        </ResultsPlaceHolder>
        
        <ResultsDataSource ConnectionString="<%$ ConnectionStrings:MCProd %>" 
             SelectCommand="SELECT
	            cast(t1.[KPI_ID] as varchar(255)) + ' - ' + cast(t1.[KPI_Measurement] as varchar(255)) AS [KPI],
				oc.OperatingCenterCode as OpCode,
				tbu.BU,
				t.KPIYear,
				(Select LookupValue from Lookup where LookupID = t.KPIMeasurementCategory) as [KPI Measurement Category], 
                t.Jan, t.Feb, t.Mar, t.Apr,
                t.May, t.Jun, t.Jul, t.Aug, 
                t.Sep, t.Oct, t.Nov, t.Dec, 
                t.Total, 
                replace(isNull(Last_Name,'') + ', ' + isNull(First_name, '') + ' ' + isNull(Middle_Name,'') + ' - '  + isNull(employeeID,''),'  ', ' ') as [Employee Responsible],
                t.EmployeeResponsible,
	            t.[KPI_Measurement_ID],
				t.[KPIMeasurementCategory],
                t.[KPI_ID], t1.Grouping
            FROM
	            [tblbusinessperformance_kpi_measurement] AS t
            LEFT JOIN
	            [tblBusinessPerformance_KPI] AS t1
            ON
	            t.[KPI_ID] = t1.[KPI_ID]
	        LEFT JOIN 
	            [OperatingCenters] as oc
	        ON
	            oc.OperatingCenterID = t.OpCode 
	        LEFT JOIN
	            [tblEmployee] te
	        ON
	            te.tblEmployeeID = t.employeeResponsible
	        LEFT JOIN   
	            BusinessUnits tbu
	        ON
	            tbu.BusinessUnitID = t.BU	            
	        ">
        </ResultsDataSource>
    
        <DetailsView>
            <Fields>
                <asp:BoundField DataField="KPI_Measurement_ID" HeaderText="KPI_Measurement_ID" 
                    InsertVisible="False" ReadOnly="True" SortExpression="KPI_Measurement_ID" />
                <asp:TemplateField HeaderText="KPI">
                    <ItemTemplate>
                        <mmsinc:DataPageViewRecordLink runat="server" id="dpvrlKPI_ID"
                            LinkUrl="~/Modules/HR/Administrator/BusPerformanceKPI.aspx"
                            DataRecordId='<%# Eval("KPI_ID") %>'
                            LinkText='<%# Eval("kpiText") %>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList runat="server" ID="ddlKPI"
                            DataTextField="kpiText" DataValueField="KPI_ID"
                            SelectedValue='<%# Bind("KPI_ID")%>'
                            AppendDataBoundItems="true" DataSourceID="dsKPI"
                        >
                            <asp:ListItem Text="--Select Here--" Value="" />
                        </asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="dsKPI"
                            ConnectionString="<%$ ConnectionStrings:MCProd %>" 
                            SelectCommand="
                                select
                                    KPI_ID,
                                    cast(kpi.[KPI_ID] as varchar(255)) + ' - ' + cast(kpi.[KPI_Measurement] as varchar(255))AS [kpiText], *
                                from
                                    tblBusinessPerformance_KPI kpi
                                order by
                                    KPI_Measurement
                            "
                        />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="KPI Measurement Category">
                    <ItemTemplate><%# Eval("KPIMeasurementCategoryText") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList runat="server" ID="ddlKPIMeasurementCategory"
                            DataTextField="LookupValue"
                            DataValueField="LookupID"
                            SelectedValue='<%# Bind("KPIMeasurementCategory") %>'
                            AppendDataBoundItems="true" DataSourceID="dsKPIMeasurementCategory">
                            <asp:ListItem Text="--Select Here--" Value="" />
                        </asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="dsKPIMeasurementCategory"
                            ConnectionString="<%$ ConnectionStrings:MCProd %>"
                            SelectCommand="SELECT LookupID,LookupValue FROM Lookup WHERE LookupType='KPIMeasurementCategory' and TableName='tblBusinessPerformance_KPI_Measurement' order by LookupValue"
                        />
                    </EditItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="OpCode">
                    <ItemTemplate><%# Eval("OperatingCenterCode") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList runat="server" ID="ddlOpCntr"
                            DataTextField="opcntr"
                            DataValueField="OperatingCenterID"
                            SelectedValue='<%# Bind("OpCode") %>'
                            AppendDataBoundItems="true" DataSourceID="dsOpCntr">
                            <asp:ListItem Text="--Select Here--" Value="" />
                        </asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="dsOpCntr"
                            ConnectionString="<%$ ConnectionStrings:MCProd %>"
                            SelectCommand="select OperatingCenterID, OperatingCenterCode + ' - ' + OperatingCenterName as opcntr from OperatingCenters order by OperatingCenterCode"
                        />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="BU">
                    <ItemTemplate><%# Eval("BusinessUnit") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList runat="server" ID="ddlBu"
                            DataTextField="BU"
                            DataValueField="BusinessUnitID"
                            SelectedValue='<%# Bind("BU") %>'
                            AppendDataBoundItems="true" DataSourceID="dsBu">
                            <asp:ListItem Text="--Select Here--" Value="" />
                        </asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="dsBu"
                            ConnectionString="<%$ ConnectionStrings:MCProd %>"
                            SelectCommand="select BusinessUnitID, BU from BusinessUnits order by BU"
                        />
                    </EditItemTemplate>
                </asp:TemplateField>                        
                
                <asp:BoundField DataField="UnitOfMeasure" HeaderText="Unit of Measure" ReadOnly="true" InsertVisible="false" />
                <asp:BoundField DataField="KPIYear" HeaderText="KPI Year" 
                    SortExpression="KPIYear" />
                <asp:BoundField DataField="Jan" HeaderText="Jan" SortExpression="Jan" />
                <asp:BoundField DataField="Feb" HeaderText="Feb" SortExpression="Feb" />
                <asp:BoundField DataField="Mar" HeaderText="Mar" SortExpression="Mar" />
                <asp:BoundField DataField="Apr" HeaderText="Apr" SortExpression="Apr" />
                <asp:BoundField DataField="May" HeaderText="May" SortExpression="May" />
                <asp:BoundField DataField="Jun" HeaderText="Jun" SortExpression="Jun" />
                <asp:BoundField DataField="Jul" HeaderText="Jul" SortExpression="Jul" />
                <asp:BoundField DataField="Aug" HeaderText="Aug" SortExpression="Aug" />
                <asp:BoundField DataField="Sep" HeaderText="Sep" SortExpression="Sep" />
                <asp:BoundField DataField="Oct" HeaderText="Oct" SortExpression="Oct" />
                <asp:BoundField DataField="Nov" HeaderText="Nov" SortExpression="Nov" />
                <asp:BoundField DataField="Dec" HeaderText="Dec" SortExpression="Dec" />
                <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" />
                
                <asp:TemplateField HeaderText="Employee Responsible">
                    <ItemTemplate><%# Eval("Employee") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList runat="server" ID="ddlEmployee"
                            DataTextField="employee"
                            DataValueField="tblEmployeeID"
                            SelectedValue='<%# Bind("EmployeeResponsible") %>'
                            AppendDataBoundItems="true" DataSourceID="dsEmployee">
                            <asp:ListItem Text="--Select Here--" Value="" />
                        </asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="dsEmployee"
                            ConnectionString="<%$ ConnectionStrings:MCProd %>"
                            SelectCommand="select tblEmployeeID, isNull(Last_Name,'') + ', ' + isNull(First_Name,'') + ' - ' + isNull(EmployeeID,'') as employee from tblEMployee order by Last_Name, First_Name"
                        />
                    </EditItemTemplate>
                </asp:TemplateField>  
            </Fields>     
        </DetailsView>
    
        <DetailsDataSource ConnectionString="<%$ ConnectionStrings:MCProd %>" 
                    SelectCommand="
                        SELECT 
	                        kpim.*,
							cast(kpi.[KPI_ID] as varchar(255)) + ' - ' + cast(kpi.[KPI_Measurement] as varchar(255))AS [kpiText],
	                        l1.lookupvalue as UnitOfMeasure,
							l2.lookupvalue as KPIMeasurementCategoryText,
							oc.OperatingCenterCode, 
							bu.bu as BusinessUnit,
							isNull(e.Last_Name,'') + ', ' + isNull(e.First_Name,'') + ' - ' + isNull(e.EmployeeID,'') as employee
                        FROM 
	                        tblBusinessPerformance_KPI_Measurement kpim
                        LEFT JOIN
	                        tblBusinessPerformance_KPI kpi
                        ON
	                        kpi.kpi_id = kpim.kpi_id
                        LEFT JOIN
	                        lookup l1
                        ON
	                        l1.lookupID = kpi.unitofmeasure
						LEFT JOIN
							lookup l2
						ON
							l2.LookupID = kpim.KPIMeasurementCategory
						LEFT JOIN
							OperatingCenters oc
						ON
							oc.OperatingCenterID = kpim.opCode
						LEFT JOIN
							BusinessUnits bu
						ON
							bu.BusinessUnitID = kpim.BU
						LEFT JOIN
							tblEMployee e
						ON
							e.tblEmployeeID = kpim.EmployeeResponsible
                        WHERE
                            KPI_Measurement_ID = @KPI_Measurement_ID
                        "
                    DeleteCommand="DELETE FROM [tblBusinessPerformance_KPI_Measurement] WHERE [KPI_Measurement_ID] = @KPI_Measurement_ID"
                    UpdateCommand="UPDATE [tblBusinessPerformance_KPI_Measurement] SET [KPI_ID] = @KPI_ID, [KPIMeasurementCategory] = @KPIMeasurementCategory, [OpCode] = @OpCode, [BU] = @BU, [KPIYear] = @KPIYear, [Jan] = @Jan, [Feb] = @Feb, [Mar] = @Mar, [Apr] = @Apr, [May] = @May, [Jun] = @Jun, [Jul] = @Jul, [Aug] = @Aug, [Sep] = @Sep, [Oct] = @Oct, [Nov] = @Nov, [Dec] = @Dec, [Total] = @Total, [EmployeeResponsible] = @EmployeeResponsible WHERE [KPI_Measurement_ID] = @KPI_Measurement_ID"
                    InsertCommand="INSERT INTO [tblBusinessPerformance_KPI_Measurement] ([KPI_ID], [KPIMeasurementCategory], [OpCode], [BU], [KPIYear], [Jan], [Feb], [Mar], [Apr], [May], [Jun], [Jul], [Aug], [Sep], [Oct], [Nov], [Dec], [Total], [EmployeeResponsible], [CreatedBy]) VALUES (@KPI_ID, @KPIMeasurementCategory, @OpCode, @BU, @KPIYear, @Jan, @Feb, @Mar, @Apr, @May, @Jun, @Jul, @Aug, @Sep, @Oct, @Nov, @Dec, @Total, @EmployeeResponsible, @CreatedBy);SELECT @KPI_Measurement_ID = (SELECT @@IDENTITY)"
                >
                    <SelectParameters>
                        <asp:Parameter Name="KPI_Measurement_ID" Type="Int32" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="KPI_Measurement_ID" Type="Int32" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:Parameter Name="KPI_ID" Type="Int32" />
                        <asp:Parameter Name="KPIMeasurementCategory" Type="Int32" />
                        <asp:Parameter Name="OpCode" Type="Int32" />
                        <asp:Parameter Name="BU" Type="Int32" />
                        <asp:Parameter Name="KPIYear" Type="Int32" />
                        <asp:Parameter Name="Jan" Type="Double" />
                        <asp:Parameter Name="Feb" Type="Double" />
                        <asp:Parameter Name="Mar" Type="Double" />
                        <asp:Parameter Name="Apr" Type="Double" />
                        <asp:Parameter Name="May" Type="Double" />
                        <asp:Parameter Name="Jun" Type="Double" />
                        <asp:Parameter Name="Jul" Type="Double" />
                        <asp:Parameter Name="Aug" Type="Double" />
                        <asp:Parameter Name="Sep" Type="Double" />
                        <asp:Parameter Name="Oct" Type="Double" />
                        <asp:Parameter Name="Nov" Type="Double" />
                        <asp:Parameter Name="Dec" Type="Double" />
                        <asp:Parameter Name="Total" Type="Double" />
                        <asp:Parameter Name="EmployeeResponsible" Type="Int32" />
                        <asp:Parameter Name="CreatedBy" Type="String" />
                        <asp:Parameter Name="KPI_Measurement_ID" Type="Int32" Direction="Output" />
                    </InsertParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="KPI_ID" Type="Int32" />
                        <asp:Parameter Name="KPIMeasurementCategory" Type="Int32" />
                        <asp:Parameter Name="OpCode" Type="Int32" />
                        <asp:Parameter Name="BU" Type="Int32" />
                        <asp:Parameter Name="KPIYear" Type="Int32" />
                        <asp:Parameter Name="Jan" Type="Double" />
                        <asp:Parameter Name="Feb" Type="Double" />
                        <asp:Parameter Name="Mar" Type="Double" />
                        <asp:Parameter Name="Apr" Type="Double" />
                        <asp:Parameter Name="May" Type="Double" />
                        <asp:Parameter Name="Jun" Type="Double" />
                        <asp:Parameter Name="Jul" Type="Double" />
                        <asp:Parameter Name="Aug" Type="Double" />
                        <asp:Parameter Name="Sep" Type="Double" />
                        <asp:Parameter Name="Oct" Type="Double" />
                        <asp:Parameter Name="Nov" Type="Double" />
                        <asp:Parameter Name="Dec" Type="Double" />
                        <asp:Parameter Name="Total" Type="Double" />
                        <asp:Parameter Name="EmployeeResponsible" Type="Int32" />
                        <asp:Parameter Name="KPI_Measurement_ID" Type="Int32" />
                    </UpdateParameters>
             </DetailsDataSource>
    </mapcall:DetailsViewDataPageTemplate>
</asp:Content>