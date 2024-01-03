<%@ Page Title="Meters" Language="C#" AutoEventWireup="true" MasterPageFile="~/MapCallSite.Master" Theme="bender" CodeBehind="Meters.aspx.cs" EnableEventValidation="false" Inherits="MapCall.Modules.FieldServices.Meters" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/ClientIDRepository.ascx" TagName="ClientIDRepository" TagPrefix="mmsinc"  %>
<%@ Register Src="~/Controls/Data/PremiseNumberSearch.ascx" TagName="PremiseNumberSearch" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/OpCntrDataField.ascx" TagName="OpCntrDataField" TagPrefix="mmsi" %>
<%@ Register TagPrefix="mapcall" Namespace="MMSINC" Assembly="MMSINC" %>
<%@ Register assembly="MMSINC" namespace="MMSINC" tagPrefix="mmsinc" %>
<%@ Register TagPrefix="cc2" Namespace="MapCall.Controls.Data" Assembly="MapCall" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphHeader">
    Meters
</asp:Content>

<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cphMain">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch" CssClass="searchPanel">
        <table style="text-align:left;" border="0">
            <mmsi:OpCntrDataField runat="server" DataFieldName="OperatingCenterID"
                TownDataFieldName="TownID" UseTowns="true" />
           <tr>
                <td class="label">
                    <label for="<%=ddlMeterProfileSearch.ClientID %>">Meter Profile:</label>
                </td>
                <td class="field">
                
                  <asp:DropDownList runat="server" id="ddlMeterProfileSearch"
                                    DataSourceID="dsMeterProfilesSearch"
                                    DataTextField="ProfileName"
                                    DataValueField="MeterProfileID"
                                    AppendDataBoundItems="true"
                                    >
                                    <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                                </asp:DropDownList>
                                <asp:SqlDataSource runat="server" ID="dsMeterProfilesSearch" 
                                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                                    SelectCommand="Select MeterProfileID, ProfileName from MeterProfiles"
                                />
                </td>
            </tr>
    
            <mmsi:DataField runat="server" ID="dfSerialNumber" DataType="String" DataFieldName="SerialNumber" HeaderText="Serial #:" />
            <mmsi:DataField runat="server" ID="dfOrcomEquipmentNumber" DataType="String" DataFieldName="OrcomEquipmentNumber" HeaderText="Orcom Equipment #:" />
            <mmsi:DataField runat="server" ID="dfDatePurchased" DataType="Date" DataFieldName="DatePurchased" HeaderText="Date Purchased:" />
            <mmsi:DataField runat="server" ID="dfIsInterconnectMeter" DataType="BooleanDropDown" DataFieldName="isInterconnectMeter" HeaderText="Interconnect Meter" />
            <tr>
                <td class="label">
                    <label for="<%=dfPremiseID.ClientID %>">Premise #:</label>
                </td>
                <td class="field">
                    <mmsi:PremiseNumberSearch runat="server" ID="dfPremiseID" />
                </td>
            </tr>
            <tr>
                <td class="label">Status</td>
                <td class="field">
                    <asp:DropDownList runat="server" id="ddlMeterStatus"
                        DataSourceID="dsMeterStatus"
                        DataTextField="Description"
                        DataValueField="MeterStatusID"
                        AppendDataBoundItems="true">
                        <asp:ListItem Text="--Select Status--" Value="" />
                    </asp:DropDownList>
                    <asp:SqlDataSource runat="server" ID="dsMeterStatus" 
                        ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        SelectCommand="SELECT MeterStatusID, Description FROM MeterStatuses" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                    <asp:Button runat="server" ID="btnReset" Text="Reset" OnClick="btnReset_Click" CausesValidation="false" />
                    <asp:Button runat="server" ID="btnAdd" Text="Add" CausesValidation="False" OnClick="btnAdd_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlResults" Visible="false">
        <asp:HiddenField runat="server" ID="hidFilter" />
        <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Export" />
        <asp:Button runat="server" ID="btnMap" PostBackUrl="~/Modules/Maps/Maps.aspx" Text="Map" />
        <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
        <asp:Label runat="server" ID="lblRecordCount"></asp:Label>
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" 
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" 
            DataKeyNames="MeterID" AllowSorting="true"
            AutoGenerateColumns="false" EnableViewState="false"
            >
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
             
                <asp:BoundField DataField="MeterID" Visible="false" HeaderText="MeterID" InsertVisible="False" ReadOnly="True" SortExpression="MeterID" />
                <mmsinc:BoundField DataField="OperatingCenterCode" HeaderText="OperatingCenterCode" SortExpression="OperatingCenterCode" />
                <asp:BoundField DataField="ProfileName" HeaderText="Meter Profile Name" InsertVisible="false" SortExpression="ProfileName" />
                <asp:BoundField DataField="SerialNumber" HeaderText="Serial #" InsertVisible="false" SortExpression="SerialNumber" />
                <asp:BoundField DataField="OrcomEquipmentNumber" HeaderText="Orcom Equipment #" InsertVisible="false" SortExpression="OrcomEquipmentNumber" />
                <asp:BoundField DataField="DatePurchased" HeaderText="Date Purchased" InsertVisible="false" SortExpression="DatePurchased" />
                <asp:BoundField DataField="Status" HeaderText="Status" InsertVisible="false" SortExpression="Status" />
                <asp:CheckBoxField DataField="IsInterconnectMeter" HeaderText="Interconnect Meter" ReadOnly="True" InsertVisible="False"/>
                <asp:BoundField DataField="PremiseNumber" HeaderText="Premise #" InsertVisible="false" SortExpression="PremiseNumber" />
                <asp:BoundField DataField="ServiceAddressHouseNumber" HeaderText="Service Address House #" InsertVisible="false" SortExpression="ServiceAddressHouseNumber" />
                <asp:BoundField DataField="ServiceAddressApartment" HeaderText="Service Address Apartment #" InsertVisible="false" SortExpression="ServiceAddressApartment" />
                <asp:BoundField DataField="ServiceAddressStreet" HeaderText="Street Name" InsertVisible="false" SortExpression="ServiceAddressStreet" />
                <asp:BoundField DataField="Town" HeaderText="Town" InsertVisible="false" SortExpression="Town" />                
                <asp:BoundField DataField="ServiceZip" HeaderText="Service Zip" InsertVisible="false" SortExpression="ServiceZip" />
                <asp:BoundField DataField="State" HeaderText="State" InsertVisible="false" SortExpression="State" />
                
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
           SelectCommand="
                    SELECT 
	                    m.*,
	                    mp.ProfileName,
	                    mm.Description as Manufacturer,
	                    mt.Description as MeterType,
	                    mdc.Description as DialCount,
	                    ms.Size,
	                    uom.Description as UnitOfMeasure,
	                    prem.PremiseNumber, 
	                    prem.ServiceAddressHouseNumber,
                        prem.ServiceAddressApartment,
                        prem.ServiceZip,
                        prem.ServiceAddressStreet,
                        njtown.Town,
                        state.Abbreviation as State, 
						oc.OperatingCenterCode,
						prem.OperatingCenterID,
						njtown.TownID
                    FROM 
	                    [Meters] m
                    LEFT JOIN
	                    [MeterProfiles] mp
                    on
	                    mp.MeterProfileID = m.MeterProfileID
                    LEFT JOIN
	                    MeterManufacturers mm 
                    ON
	                    mm.MeterManufacturerID = mp.MeterManufacturerID
                    LEFT JOIN
	                    MeterTypes mt
                    ON
	                    mt.MeterTypeID = mp.MeterTypeID
                    LEFT JOIn
	                    MeterSizes ms
                    ON
	                    ms.MeterSizeID = mp.MeterSizeID
                    LEFT JOIN
	                    MeterDialCounts mdc
                    ON
	                    mdc.MeterDialCountID = mp.NumberOfDials
                    LEFT JOIN
	                    UnitsOfMeasure uom
                    ON
	                    uom.UnitOfMeasureID = mp.UnitOfMeasureID
                    LEFT JOIN
                        Premises prem
                    ON
                        m.PremiseID = prem.Id
                    LEFT JOIN
                        Towns njtown
                    ON
                        prem.ServiceCityId = njtown.TownID
                    LEFT JOIN
                        [States] state
                    ON
                        prem.ServiceStateId = state.StateID
					LEFT JOIN
						OperatingCenters oc
					ON
						oc.OperatingCenterID = prem.OperatingCenterID
                    "
        >
        </asp:SqlDataSource>
    </asp:Panel>
    
    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <input id="hdnCurrentMode" type="hidden" value="<%=dvMeter.CurrentMode%>" />
        <asp:Label runat="server" ID="lbErrorMsg" runat="server" Visible="false" CssClass="Error" />
        <div id="divContent" class="tabsContainer">
            <ul class="ui-tabs-nav">
                <li><a href="#meter" class="tab"><span>Meter</span></a></li>
                <li><a href="#meterTests" class="tab"><span>Meter Tests</span></a></li>
                <li><a href="#notes" class="tab"><span>Notes</span></a></li>
                <li><a href="#documents" class="tab"><span>Documents</span></a></li>
            </ul>
            <div id="meter">
                <asp:DetailsView runat="server" ID="dvMeter" AutoGenerateRows="false"
                    DataKeyNames="MeterID" DataSourceID="dsMeter"
                    OnItemInserting="DetailView_ItemInserting"
                    OnItemInserted="DetailView_ItemInserted"
                    OnItemUpdating="DetailView_ItemUpdating"
                    OnDataBound="DetailView_DataBound"
                    Width="100%"
                    >
                    <Fields>
                        <asp:BoundField DataField="MeterID" HeaderText="MeterID" InsertVisible="False" 
                            ReadOnly="True" SortExpression="MeterID" Visible="false" />
                            
                        <asp:TemplateField HeaderText="Meter Profile">
                            <ItemTemplate><%#Eval("ProfileName") %></ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList runat="server" id="ddlMeterProfile"
                                    SelectedValue='<%#Bind("MeterProfileID") %>'
                                    DataSourceID="dsMeterProfiles"
                                    DataTextField="ProfileName"
                                    DataValueField="MeterProfileID"
                                    AppendDataBoundItems="true"
                                >
                                    <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                                </asp:DropDownList>
                                <asp:SqlDataSource runat="server" ID="dsMeterProfiles" 
                                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                                    SelectCommand="Select MeterProfileID, ProfileName from MeterProfiles"
                                />
                                <asp:RequiredFieldValidator runat="server" ID="rfvMeterProfile"
                                    ControlToValidate="ddlMeterProfile"
                                    InitialValue="" Text="Required" />                        
                            </EditItemTemplate>
                        </asp:TemplateField>
                        
                                  
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate><%#Eval("MeterStatus") %></ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList runat="server" ID="ddlMeterStatus"
                                    SelectedValue='<%#Bind("Status") %>'
                                    DataSourceID="dsMeterStatuses"
                                    DataTextField="Description"
                                    DataValueField="MeterStatusID"
                                    AppendDataBoundItems="true"
                                    >
                                    <asp:ListItem Text="--Select Here--" Value="" />
                                </asp:DropDownList>
                                <asp:SqlDataSource runat="server" ID="dsMeterStatuses"
                                    ConnectionString='<%$ ConnectionStrings:MCProd %>'
                                    SelectCommand="Select * from MeterStatuses order by 2"
                                />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Premise #">
                            <ItemTemplate>
                                <a title="View full Premise information" href='<%#String.Format("{0}?premiseId={1}", 
                                                ResolveClientUrl("~/Modules/Customer/Premises.aspx"), 
                                                    Eval("PremiseID")) %>'>
                                    <%#Eval("PremiseNumber") %>
                                </a>
                           </ItemTemplate>   
                           <EditItemTemplate>
                                <mmsi:PremiseNumberSearch  
                                    ID="pnsPremNum"
                                    runat="server" 
                                    BindingSelectedPremiseID='<%#Bind("PremiseID") %>'
                                    SelectedPremiseNumber='<%#Eval("PremiseNumber") %>' />
                           </EditItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Premise Service Address">
                            <ItemTemplate>
                                <div><%#Eval("ServiceAddressHouseNumber")%> <%#Eval("ServiceAddressStreet")%></div>
                                
                                <%--Hide this if apartment number is null?--%>
                                <div><%# DataBinder.Eval(Container.DataItem, "ServiceAddressApartment", "APT #: {0}") %></div>
                                <div><%#Eval("Town") %>, <%#Eval("State") %>, <%#Eval("ServiceZip") %></div>
                            </ItemTemplate>
                        </asp:TemplateField>
                       <asp:TemplateField HeaderText="Serial Number">
                           <ItemTemplate>
                               <%#Eval("SerialNumber") %>
                           </ItemTemplate>
                           <EditItemTemplate>
                               <asp:TextBox runat="server" ID="txtSerialNumber" Text='<%# Bind("SerialNumber") %>' />
                                <asp:RequiredFieldValidator runat="server" ID="rfvSerialNumber"
                                    ControlToValidate="txtSerialNumber"
                                    InitialValue="" Text="Required" />   
                                <cc2:UniqueFieldValidator ID="ufvSerialNumber" 
                                    ControlToValidate="txtSerialNumber"
                                    ErrorMessage="A meter already exists with this serial number." 
                                    PrimaryKeyName="MeterID" 
                                    PrimaryKeyValue='<%#Bind("MeterID") %>'
                                    UniqueFieldName="SerialNumber" 
                                    ConnectionString="<%$ ConnectionStrings:MCProd %>" 
                                    SelectCommand="SELECT * FROM Meters
                                                        WHERE MeterID &lt;&gt; @MeterID
                                                        AND SerialNumber = @SerialNumber"
                                    runat="server" />
                           </EditItemTemplate>
                       </asp:TemplateField>
                        <asp:BoundField DataField="OrcomEquipmentNumber" HeaderText="Orcom Equipment Number" 
                            SortExpression="OrcomEquipmentNumber" />
                       
                        <asp:BoundField HeaderText="Manufacturer" DataField="Manufacturer" ReadOnly="true" InsertVisible="false" />
                        <asp:BoundField HeaderText="Meter Type" DataField="MeterType" ReadOnly="true" InsertVisible="false" />
                        <asp:BoundField HeaderText="Size" DataField="Size" ReadOnly="true" InsertVisible="false" />
                        <asp:BoundField HeaderText="Number of Dials" DataField="DialCount" ReadOnly="true" InsertVisible="false" />
                        <asp:BoundField HeaderText="Units of Measure" DataField="UnitOfMeasure" ReadOnly="true" InsertVisible="false" />
                        <asp:CheckBoxField DataField="IsInterconnectMeter" HeaderText="Interconnect Meter" />

                        <mmsinc:BoundField  DataFormatString="{0:d}" SqlDataType="DateTime" DataField="DatePurchased" HeaderText="Date Purchased" 
                            SortExpression="DatePurchased" />
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
                <asp:SqlDataSource ID="dsMeter" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:MCProd %>" 
                    OnInserted="SqlDataSource1_Inserted"
                    InsertCommand="INSERT INTO [Meters] ([MeterProfileID], [SerialNumber], [OrcomEquipmentNumber], [DatePurchased], [CreatedBy], [Status], [PremiseID], [IsInterconnectMeter]) VALUES (@MeterProfileID, @SerialNumber, @OrcomEquipmentNumber, @DatePurchased, @CreatedBy, @Status, @PremiseID, @IsInterconnectMeter);SELECT @MeterID = (Select @@IDENTITY)" 
                    DeleteCommand="DELETE FROM [Meters] WHERE [MeterID] = @MeterID" 
                    UpdateCommand="UPDATE [Meters] SET [MeterProfileID] = @MeterProfileID, [PremiseID] = @PremiseID, [SerialNumber] = @SerialNumber, [OrcomEquipmentNumber] = @OrcomEquipmentNumber, [DatePurchased] = @DatePurchased, Status=@Status, IsInterconnectMeter = @IsInterconnectMeter WHERE [MeterID] = @MeterID"
                    SelectCommand="
                SELECT 
                    m.*,
                    mp.ProfileName,
                    mm.Description as Manufacturer,
                    mt.Description as MeterType,
                    mdc.Description as DialCount,
                    ms.Size,
                    uom.Description as UnitOfMeasure, 
                    stat.Description as MeterStatus,
                    prem.PremiseNumber,
                    prem.ServiceAddressHouseNumber,
                    prem.ServiceAddressApartment,
                    prem.ServiceZip,
                    prem.ServiceAddressStreet,
                    njtown.Town,
                    state.Abbreviation as State
                FROM 
                  [Meters] m
                LEFT JOIN
                  [MeterProfiles] mp
                on
                  mp.MeterProfileID = m.MeterProfileID
                LEFT JOIN
                  MeterManufacturers mm 
                ON
                  mm.MeterManufacturerID = mp.MeterManufacturerID
                LEFT JOIN
                  MeterTypes mt
                ON
                  mt.MeterTypeID = mp.MeterTypeID
                LEFT JOIN
                  MeterSizes ms
                ON
                    ms.MeterSizeID = mp.MeterSizeID
                LEFT JOIN
                    MeterDialCounts mdc
                ON
                    mdc.MeterDialCountID = mp.NumberOfDials
                LEFT JOIN
                    UnitsOfMeasure uom
                ON
                    uom.UnitOfMeasureID = mp.UnitOfMeasureID
                LEFT JOIN
                    MeterStatuses stat
                ON
                    m.Status = stat.MeterStatusID
                LEFT JOIN
                    Premises prem
                ON
                    m.PremiseID = prem.Id
                LEFT JOIN
                    Towns njtown
                ON
                    prem.ServiceCityId = njtown.TownID
                LEFT JOIN
                    [States] state
                ON
                    prem.ServiceStateId = state.StateID
                
                
                WHERE 
                [MeterID] = @MeterID
                    " 
                    
                >
                    <SelectParameters>
                        <asp:Parameter Name="MeterID" Type="Int32" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="MeterID" Type="Int32" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="MeterProfileID" Type="Int32" />
                        <asp:Parameter Name="PremiseID" Type="Int32" />
                        <asp:Parameter Name="SerialNumber" Type="String" />
                        <asp:Parameter Name="OrcomEquipmentNumber" Type="String" />
                        <asp:Parameter Name="DatePurchased" Type="DateTime" />
                        <asp:Parameter Name="MeterID" Type="Int32" />
                        <asp:Parameter Name="Status" Type="Int32" />
                        <asp:Parameter Name="IsInterconnectMeter" Type="Boolean" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:Parameter Name="MeterID" Type="Int32" Direction="Output" />
                        <asp:Parameter Name="MeterProfileID" Type="Int32" />
                        <asp:Parameter Name="Status" Type="Int32" />
                        <asp:Parameter Name="SerialNumber" Type="String" />
                        <asp:Parameter Name="OrcomEquipmentNumber" Type="String" />
                        <asp:Parameter Name="DatePurchased" Type="DateTime" />
                        <asp:Parameter Name="CreatedBy" Type="String" />
                        <asp:Parameter Name="PremiseID" Type="Int32" />
                        <asp:Parameter Name="IsInterconnectMeter" Type="Boolean" />
                    </InsertParameters>
                </asp:SqlDataSource>
            </div>
            <div id="notes">
                <mmsi:Notes ID="Notes1" runat="server" DataTypeID="78" />
            </div>
            <div id="documents">
                <mmsi:Documents ID="Documents1" runat="server" DataTypeID="78" />
            </div>       
            <div id="meterTests">
                <%--Related Meter Tests--%>
                <asp:GridView ID="gvRelatedMeterTests" runat="server" DataSourceID="dsRelatedMeterTests" 
                    AutoGenerateColumns="true" DataKeyNames="MeterTestID" EmptyDataText="No Meter Tests Exist For This Meter">
                                
                    <Columns>
                        <asp:HyperLinkField DataNavigateUrlFields="MeterTestID" DataNavigateUrlFormatString="MeterTests.aspx?arg={0}" Text="View" />
                    </Columns>
                </asp:GridView>
                
                <asp:SqlDataSource ID="dsRelatedMeterTests" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:MCProd %>" 
                    SelectCommand="
                    SELECT 
	        MeterTestID, 
	        --MeterID, MeterTestComparisonMeterID, 
	        DateSurveyed, 
	        --SiteIssue, 
	        --InletValveOperational, OutletValveOperational, 
	        DateScheduledForTest, 
	        --AssignedToContractorID, 
	        DateTested, 
	        --TestedByEmployeeID, 
	        TestedByEmployeeLastName, 
	        --TestedByContractorID, 
	        TestedByContractorEmployeeName, 
	        MeterTestPassed,
	        TestComments
	        --MeterReadingBeforeLow, MeterReadingBeforeHigh, MeterReadingBeforeFire, MeterReadingAfterLow, MeterReadingAfterHigh, 
	        --MeterReadingAfterFire, HydrostaticPressure, ResidualPressure, 
        	
        FROM 
	        MeterTests
        WHERE
            MeterID = @MeterID" 
                    >            
                    <SelectParameters>
                        <asp:Parameter Name="MeterID" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>    
            </div>
        </div>    
        <center>
            <asp:Button runat="server" ID="Button4" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="Button5" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
        
    </asp:Panel>
    
    <mmsinc:ClientIDRepository runat="server" id="clientIDRepository" />

    
</asp:Content>