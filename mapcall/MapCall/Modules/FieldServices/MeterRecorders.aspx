<%@ Page Title="Meter Recorders" Language="C#" AutoEventWireup="true" MasterPageFile="~/MapCallSite.Master" Theme="bender" CodeBehind="MeterRecorders.aspx.cs" Inherits="MapCall.Modules.FieldServices.MeterRecorders" %>

<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.Data" TagPrefix="cc2" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>
<%@ Register TagPrefix="dotnet" Namespace="dotnetCHARTING" Assembly="dotnetCHARTING" %>
<%@ Register assembly="MMSINC" namespace="MMSINC" tagPrefix="mmsinc" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphHeader">
    Meter Recorders
</asp:Content>

<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cphMain">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <table class="prettyTable noBorders" style="margin:0px auto;">
            <tr>
                <td class="label">Search For</td>
                <td class="field">
                    <asp:DropDownList runat="server" ID="ddlInstallOptions">
                        <%-- These ListItem values coorespond to the InstallationSearchOptions
                            enum in code-behind.  --%>
                        <asp:ListItem Value="0">All Recorders</asp:ListItem>
                        <asp:ListItem Value="1">Installed Recorders</asp:ListItem>
                        <asp:ListItem Value="2">Uninstalled Recorders</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <mmsi:DataField ID="DataField1" runat="server" DataFieldName="SerialNumber" HeaderText="Meter Recorder Serial Number" />
            <tr>
                <td class="label">Manufacturer</td>
                <td class="field">
                    <asp:DropDownList runat="server" id="ddlManufacturerSearch"
                        DataSourceID="dsMeterRecorderManufacturer"
                        DataTextField="Description"
                        DataValueField="MeterRecorderManufacturerID"
                        AppendDataBoundItems="true">
                        <asp:ListItem Text="--Select Manufacturer--" Value="" />
                    </asp:DropDownList>
                    <asp:SqlDataSource runat="server" ID="dsMeterRecorderManufacturer" 
                        ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        SelectCommand="SELECT MeterRecorderManufacturerID, Description FROM MeterRecorderManufacturers" />
                </td>
            </tr>
            <mmsi:DataField runat="server" DataFieldName="Model" HeaderText="Model" />
        </table>
        <div class="buttonContainer">
            <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
            <asp:Button runat="server" ID="btnReset" Text="Reset" OnClick="btnReset_Click" />
            <asp:Button runat="server" ID="btnAdd" Text="Add" CausesValidation="False" OnClick="btnAdd_Click" />
        </div>
    </asp:Panel>
    
    <asp:Panel runat="server" ID="pnlResults" Visible="false">
        <div class="container">
            <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Export" />
            <asp:Button runat="server" ID="btnMap" Visible="true" PostBackUrl="~/Modules/Maps/Maps.aspx" Text="Map" />
            <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Label runat="server" ID="lblRecordCount"></asp:Label>
        </div>
        <asp:HiddenField ID="hidFilter" runat="server" />
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" 
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="MeterRecorderID" AllowSorting="true"
            AutoGenerateColumns="false"
            >
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
                <asp:BoundField HeaderText="Serial #"  DataField="SerialNumber" SortExpression="SerialNumber" />
                <asp:BoundField HeaderText="Meter Recorder Manufacturer" DataField="ManufacturerName" SortExpression="MeterRecorderManufacturerID"  />
                <asp:BoundField HeaderText="Meter Recorder Type" DataField="RecorderType" SortExpression="MeterRecorderTypeID"  />
                <asp:BoundField HeaderText="Model" DataField="Model" SortExpression="Model"  />
                <asp:BoundField HeaderText="Setup Requirements" DataField="SetupRequirements" SortExpression="SetupRequirements"  />
                <asp:BoundField HeaderText="Data Transfer Instructions" DataField="DataTransferInstructions" SortExpression="DataTransferInstructions"  />
                <asp:BoundField HeaderText="Premise #" DataField="PremiseNumber" SortExpression="PremiseNumber"  />
                <asp:BoundField HeaderText="Service Address House Number" DataField="ServiceAddressHouseNumber" SortExpression="ServiceAddressHouseNumber"  />
                <asp:BoundField HeaderText="Service Address Apartment Number" DataField="ServiceAddressApartmentNumber" SortExpression="ServiceAddressApartmentNumber"  />
                <asp:BoundField HeaderText="Service Zip" DataField="ServiceZip" SortExpression="ServiceZip"  />
                <asp:BoundField HeaderText="Street Name" DataField="FullStName" SortExpression="FullStName"  />
                <asp:BoundField HeaderText="Town" DataField="Town" SortExpression="Town"  />
                <asp:BoundField HeaderText="State" DataField="State" SortExpression="State"  />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
           SelectCommand="SELECT distinct
                                rec.[MeterRecorderID],
                                rec.[MeterRecorderManufacturerID],
                                rec.[MeterRecorderTypeID],
                                rec.[Model],
                                cast(rec.[SetupRequirements] as varchar(200))as [SetupRequirements],
                                cast(rec.[DataTransferInstructions] as varchar(200)) as [DataTransferInstructions],
                                rec.[SerialNumber],
                                rec.[CreatedOn],
                                rec.[CreatedBy],
                                prem.PremiseNumber, 
                                prem.ServiceAddressHouseNumber,
                                prem.ServiceAddressApartmentNumber,
                                prem.ServiceZip,
                                njstreet.FullStName,
	                            njtown.Town,
	                            state.Abbreviation as State,
	                            'ManufacturerName' = manu.Description,
	                            'RecorderType' = mrt.Description
                            FROM
                                [MeterRecorders] rec
                            LEFT JOIN
                                [MeterRecorderHistory] hist
                            ON
                                hist.MeterRecorderID = rec.MeterRecorderID
                            LEFT JOIN
                                [Meters] meter
                            ON
                                meter.MeterID = hist.MeterID
                            LEFT JOIN
                                [Premises] prem
                            ON
                                prem.PremiseID = meter.PremiseID
                            LEFT JOIN
                                [MeterRecorderManufacturers] manu
                            ON
                                manu.MeterRecorderManufacturerID = rec.MeterRecorderManufacturerID
                            LEFT JOIN
                                [MeterRecorderTypes] mrt
                            ON
                                mrt.MeterRecorderTypeID = rec.MeterRecorderTypeID
                            LEFT JOIN
                                Streets njstreet
                            ON
                                prem.ServiceAddressStreet = njstreet.StreetID
                            LEFT JOIN
                                Towns njtown
                            ON
                                prem.ServiceCity = njtown.TownID
                            LEFT JOIN
                                [States] state
                            ON
                                prem.ServiceState = state.StateID
                      "
        >
        </asp:SqlDataSource>
    </asp:Panel>
    
    <asp:Panel runat="server" ID="pnlDetail" CssClass="detailPanel" Visible="false">
        <div class="tabsContainer">
            <ul class="ui-tabs-nav">
                <li><a href="#meterRecorder" class="tab"><span>Meter Recorder</span></a></li>
                <li><a href="#changeOrders" class="tab"><span>Change Orders</span></a></li>
                <li><a href="#readings" class="tab"><span>Readings</span></a></li>
                <li><a href="#chart" class="tab"><span>Chart</span></a></li>
                <li><a href="#notes" class="tab"><span>Notes</span></a></li>
                <li><a href="#documents" class="tab"><span>Documents</span></a></li>
            </ul>
            <div id="meterRecorder">
                <div class="container">
                    <asp:Button ID="btnCreateChangeOrder" Text="Create Change Order" OnClick="btnCreateChangeOrder_Click" runat="server" />
                </div>
                <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="false" 
                    DataKeyNames="MeterRecorderID" DataSourceID="dsDetailView" Width="100%"
                    OnItemInserting="DetailView_ItemInserting"
                    OnItemInserted="DetailView_ItemInserted"
                    OnItemUpdating="DetailView_ItemUpdating"
                    OnDataBound="DetailView_DataBound"
                    >
                    <EmptyDataTemplate>
                        Record Not Found
                    </EmptyDataTemplate>
                    <Fields>
                        <asp:BoundField Visible="false" HeaderText="Meter Recorder ID" DataField="MeterRecorderID" InsertVisible="False" ReadOnly="True" />
                       
                        <asp:TemplateField HeaderText="Meter Recorder Serial Number">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#Eval("SerialNumber") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                               <asp:TextBox runat="server" ID="txtSerialEdit" 
                                    Text='<%#Bind("SerialNumber") %>'
                                    MaxLength="50" />
                               <asp:RequiredFieldValidator ControlToValidate="txtSerialEdit" runat="server" ErrorMessage="Required" />
                               <cc2:UniqueFieldValidator ID="UniqueFieldValidator1" 
                                    ControlToValidate="txtSerialEdit"
                                    ErrorMessage="A meter recorder already exists with this serial number." 
                                    PrimaryKeyName="MeterRecorderID" 
                                    PrimaryKeyValue='<%#Bind("MeterRecorderID") %>'
                                    UniqueFieldName="SerialNumber" 
                                    ConnectionString="<%$ ConnectionStrings:MCProd %>" 
                                    SelectCommand="SELECT * FROM MeterRecorders
                                                       WHERE MeterRecorderID &lt;&gt; @MeterRecorderID
                                                       AND SerialNumber = @SerialNumber"
                                    runat="server" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                       
                        <asp:TemplateField HeaderText="Meter Recorder Manufacturer">
                            <ItemTemplate><%#Eval("ManufacturerDescription")%></ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList runat="server" ID="ddlManufacturer"
                                    SelectedValue='<%#Bind("MeterRecorderManufacturerID") %>'
                                    DataSourceID="dsManufacturer"
                                    DataTextField="Description"
                                    DataValueField="MeterRecorderManufacturerID"
                                    AppendDataBoundItems="true"
                                    >
                                    <asp:ListItem Text="--Select Here--" Value="" />
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvManu" ControlToValidate="ddlManufacturer" runat="server" ErrorMessage="Required" />
                                <asp:SqlDataSource runat="server" ID="dsManufacturer"
                                    ConnectionString='<%$ ConnectionStrings:MCProd %>'
                                    SelectCommand="Select * from MeterRecorderManufacturers order by 2"
                                />
                            </EditItemTemplate>
                        </asp:TemplateField>
                       
                        <asp:TemplateField HeaderText="Meter Recorder Type">
                            <ItemTemplate><%#Eval("MeterRecorderTypeID")%></ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList runat="server" ID="ddlRecorderType"
                                    SelectedValue='<%#Bind("MeterRecorderTypeID") %>'
                                    DataSourceID="dsRecorderType"
                                    DataTextField="Description"
                                    DataValueField="MeterRecorderTypeID"
                                    AppendDataBoundItems="true"
                                    >
                                    <asp:ListItem Text="--Select Here--" Value="" />
                                </asp:DropDownList>
                                <asp:SqlDataSource runat="server" ID="dsRecorderType"
                                    ConnectionString='<%$ ConnectionStrings:MCProd %>'
                                    SelectCommand="SELECT * FROM MeterRecorderTypes ORDER BY Description"
                                />
                            </EditItemTemplate>
                        </asp:TemplateField>
                       
                        <asp:BoundField HeaderText="Model" DataField="Model" />
                        <mmsinc:BoundField  HeaderText="Setup Requirements" SqlDataType="NVarChar" DataField="SetupRequirements" TextMode="MultiLine"  />
                        <mmsinc:BoundField  HeaderText="Data Transfer Instructions" SqlDataType="NVarChar" DataField="DataTransferInstructions" TextMode="MultiLine"  />
                       
                        <asp:TemplateField HeaderText="Current Location">
                            <ItemTemplate>
                                <div>
                                    <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%# (Eval("MeterRecorderStorageLocationID") != DBNull.Value) %>'>
                                        AT STORAGE LOCATION: <a title="View full storage location information" href='<%#String.Format("{0}?storageId={1}", 
                                                        ResolveClientUrl("~/Modules/FieldServices/MeterRecorderStorageLocations.aspx"), 
                                                        Eval("MeterRecorderStorageLocationID")) %>'><%#Eval("StorageLocationName") %> </a>
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible='<%# (Eval("PremiseID") != DBNull.Value) %>'>
                                        AT PREMISE: <a title="View full Premise information" href='<%#String.Format("{0}?premiseId={1}", 
                                                        ResolveClientUrl("~/Modules/Customer/Premises.aspx"), 
                                                        Eval("PremiseID")) %>'><%#Eval("PremiseNumber") %> </a>
                                    </asp:PlaceHolder>
                                </div>
                                <div><%#Eval("StorageLocationName") %></div>
                                <div><%#Eval("HouseNumber")%> <%#Eval("Street")%></div>
                                <div><%# DataBinder.Eval(Container.DataItem, "ApartmentNumber", "APT #: {0}") %></div>
                                <div><%#Eval("City") %>, <%#Eval("State") %>, <%#Eval("Zip") %></div>
                            </ItemTemplate>
                            <InsertItemTemplate>
                                <%--This is in order to set the initial MeterRecorderChangeOrder when this is inserted.--%>
                                <asp:DropDownList runat="server" ID="ddlStorageLocations"
                                    DataSourceID="dsLocations" AppendDataBoundItems="true"
                                    DataTextField="Name"
                                    DataValueField="MeterRecorderStorageLocationID"
                                    SelectedValue='<%#Bind("MeterRecorderStorageLocationID")%>'
                                    >
                                    <asp:ListItem Value="" Text="--Select Here--" />
                                </asp:DropDownList>
                                <asp:SqlDataSource runat="server" ID="dsLocations"
                                    ConnectionString="<%$ConnectionStrings:MCProd%>"
                                    SelectCommand="SELECT wd.MeterRecorderStorageLocationID, wd.Name FROM MeterRecorderStorageLocations wd"
                                />
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField ShowHeader="False">
                            <EditItemTemplate>
                                <asp:LinkButton ID="btnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                    Text="Update"></asp:LinkButton>
                                <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                    Text="Cancel"></asp:LinkButton>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:LinkButton ID="btnInsert" runat="server" CausesValidation="True" CommandName="Insert"
                                    Text="Insert"></asp:LinkButton>
                                <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="false" CommandName="Cancel"
                                    Text="Cancel" OnClick="btnCancelInsert_Click"></asp:LinkButton>
                            </InsertItemTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="False" Visible="false"
                                    CommandName="Edit" Text="Edit"></asp:LinkButton>
                                <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False" Visible="false"
                                    CommandName="Delete" Text="Delete" OnClientClick="return confirm('Are you sure?');"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                    </Fields>
                    
                </asp:DetailsView>
            
            <asp:SqlDataSource ID="dsDetailView" runat="server" 
            ConnectionString="<%$ ConnectionStrings:MCProd %>" CancelSelectOnNullParameter="false"
            OnInserted="SqlDataSource1_Inserted"
            DeleteCommand="DELETE FROM [MeterRecorders] WHERE [MeterRecorderID] = @MeterRecorderID" 
            InsertCommand="INSERT INTO [MeterRecorders] ([MeterRecorderManufacturerID], [MeterRecorderTypeID], [Model], [SetupRequirements], [DataTransferInstructions], [SerialNumber], [CreatedBy]) VALUES (@MeterRecorderManufacturerID, @MeterRecorderTypeID, @Model, @SetupRequirements, @DataTransferInstructions, @SerialNumber, @CreatedBy);SELECT @MeterRecorderID = (Select @@IDENTITY)" 
            UpdateCommand="UPDATE [MeterRecorders] SET [MeterRecorderManufacturerID] = @MeterRecorderManufacturerID, [MeterRecorderTypeID] = @MeterRecorderTypeID, [Model] = @Model, [SetupRequirements] = @SetupRequirements, [DataTransferInstructions] = @DataTransferInstructions, [SerialNumber] = @SerialNumber WHERE [MeterRecorderID] = @MeterRecorderID"
            SelectCommand="SELECT distinct
                            rec.[MeterRecorderID],
                            rec.[MeterRecorderManufacturerID],
                            rec.[MeterRecorderTypeID],
                            rec.[Model],
                            cast(rec.[SetupRequirements] as varchar(200))as [SetupRequirements],
                            cast(rec.[DataTransferInstructions] as varchar(200)) as [DataTransferInstructions],
                            rec.[SerialNumber],
                            rec.[CreatedOn],
                            rec.[CreatedBy],
                            'ManufacturerDescription' = manu.Description,
	                        'RecorderType' = mrt.Description,
	                        prem.PremiseID,
	                        prem.PremiseNumber, 
	                        storage.MeterRecorderStorageLocationID as MeterRecorderStorageLocatioNID,
                            storage.Name as StorageLocationName,
                            'HouseNumber' = CASE WHEN (cOrder.PremiseID Is Not Null) THEN prem.ServiceAddressHouseNumber ELSE storage.AddressHouseNumber END,
                            'ApartmentNumber' = CASE WHEN (cOrder.PremiseID Is Not Null) THEN prem.ServiceAddressApartmentNumber ELSE storage.AddressApartmentNumber END,
	                        'Street' = CASE WHEN (cOrder.PremiseID Is Not Null) THEN streetPremise.FullStName ELSE streetStorage.FullStName END,
	                        'City' = CASE WHEN (cOrder.PremiseID Is Not Null) THEN townPremise.Town ELSE townStorage.Town END,
	                        'Zip' = CASE WHEN (cOrder.PremiseID Is Not Null) THEN prem.ServiceZip ELSE storage.Zip END,
	                        'State' = CASE WHEN (cOrder.PremiseID Is Not Null) THEN statePremise.Abbreviation ELSE stateStorage.Abbreviation END
                        FROM
                            [MeterRecorders] rec
                        LEFT JOIN
                            [MeterRecorderChangeOrders] cOrder
                        ON
                            cOrder.MeterRecorderID = rec.MeterRecorderID
                        LEFT JOIN
                            [Premises] prem
                        ON
                            prem.PremiseID = cOrder.PremiseID
                        LEFT JOIN
                            [MeterRecorderStorageLocations] storage
                        ON
                            storage.MeterRecorderStorageLocationID = cOrder.MeterRecorderStorageLocationID
                        LEFT JOIN
                            [MeterRecorderManufacturers] manu
                        ON
                            manu.MeterRecorderManufacturerID = rec.MeterRecorderManufacturerID
                        LEFT JOIN
                            [MeterRecorderTypes] mrt
                        ON
                            mrt.MeterRecorderTypeID = rec.MeterRecorderTypeID
                            -- Premise Address Join
                        LEFT JOIN
                            Streets streetPremise
                        ON
                            prem.ServiceAddressStreet = streetPremise.StreetID
                        LEFT JOIN
                            Towns townPremise
                        ON
                            prem.ServiceCity = townPremise.TownID
                        LEFT JOIN
                            [States] statePremise
                        ON
                            prem.ServiceState = statePremise.StateID

                        -- Storage address join
                        LEFT JOIN
                            Streets streetStorage
                        ON
                            storage.AddressStreetID = streetStorage.StreetID
                        LEFT JOIN
                            Towns townStorage
                        ON
                            storage.CityID = townStorage.TownID
                        LEFT JOIN
                            [States] stateStorage
                        ON
                            storage.StateID = stateStorage.StateID
                        WHERE
                            rec.MeterRecorderID = @MeterRecorderID"
                >
            <SelectParameters>
                <asp:Parameter Name="MeterRecorderID" Type="Int32" />
            </SelectParameters>            
            <DeleteParameters>
                <asp:Parameter Name="MeterRecorderID" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="MeterRecorderManufacturerID" Type="Int32" />
                <asp:Parameter Name="MeterRecorderTypeID" Type="Int32" />
                <asp:Parameter Name="Model" Type="String" />
                <asp:Parameter Name="SetupRequirements" Type="String" />
                <asp:Parameter Name="DataTransferInstructions" Type="String" />
                <asp:Parameter Name="MeterRecorderID" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="MeterRecorderID" Type="Int32" Direction="Output" />
                <asp:Parameter Name="MeterRecorderManufacturerID" Type="Int32" />
                <asp:Parameter Name="MeterRecorderTypeID" Type="Int32" />
                <asp:Parameter Name="Model" Type="String" />
                <asp:Parameter Name="SetupRequirements" Type="String" />
                <asp:Parameter Name="DataTransferInstructions" Type="String" />
                <asp:Parameter Name="SerialNumber" Type="String" />
                <asp:Parameter Name="CreatedBy" Type="String" />
                <asp:Parameter Name="MeterRecorderStorageLocationID" Type="Int32" Direction="Input" />
            </InsertParameters>
        </asp:SqlDataSource>
   
            </div>
            
            <div id="changeOrders">
                
                <asp:GridView ID="gridViewChangeOrders" runat="server" DataSourceID="dsChangeOrders" 
                DataKeyNames="MeterRecorderChangeOrderID" AllowSorting="true" OnSelectedIndexChanged="gridViewChangeOrders_OnSelectedIndexChanged"
                AutoGenerateColumns="false" OnRowDataBound="gridViewChangeOrders_OnRowDataBound">
                <Columns>
                    <asp:CommandField ShowSelectButton="True" SelectText="View" />
                    <asp:BoundField DataField="CreatedOn" SortExpression="CreatedOn" HeaderText="Created On" />
                    <asp:BoundField DataField="CreatedBy" SortExpression="CreatedBy" HeaderText="Created By" />
                    <asp:BoundField DataField="DatePerformed" SortExpression="DatePerformed" HeaderText="Date Performed" />
                    <asp:BoundField DataField="MeterRecorderSerialNumber" SortExpression="MeterRecorderSerialNumber" HeaderText="Meter Recorder Serial #" />
                    
                   <asp:TemplateField>
                    <HeaderTemplate>
                        <%--Mimics the sorting stuff that's done automatically otherwise--%>
                        <asp:LinkButton runat="server" Text="Meter Serial #" CommandName="Sort" CommandArgument="MeterSerialNumber"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Literal ID="litMeterSerialNumber" runat="server" Text='<%# Bind("MeterSerialNumber") %>' />
                    </ItemTemplate>
                   </asp:TemplateField>
                    <asp:BoundField DataField="WorkDescription" SortExpression="WorkDescription" HeaderText="Work Description" />
                    <asp:BoundField DataField="ChangeOrderDescription" SortExpression="ChangeOrderDescription" HeaderText="Additional Order Information" />
                    <asp:BoundField DataField="PremiseNumber" SortExpression="PremiseNumber" HeaderText="Premise #" />
                    <asp:BoundField DataField="StorageLocationName" SortExpression="StorageLocationName" HeaderText="Storage Location" />
                    <asp:BoundField DataField="HouseNumber" HeaderText="House #" />
                    <asp:BoundField DataField="ApartmentNumber" HeaderText="Apartment #" />
                    <asp:BoundField DataField="Street" HeaderText="Street" />
                    <asp:BoundField DataField="City" HeaderText="City" />
                    <asp:BoundField DataField="State" HeaderText="State" />
                    <asp:BoundField DataField="Zip" HeaderText="Zip" />
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="dsChangeOrders" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
               SelectCommand="
                SELECT 
                    cOrder.*,
	                recorders.SerialNumber as MeterRecorderSerialNumber,
                    meters.SerialNumber as MeterSerialNumber,
                    workDescript.Name as WorkDescription,
                    prem.PremiseNumber, 
                    storage.Name as StorageLocationName,
                    'HouseNumber' = CASE WHEN (cOrder.PremiseID Is Not Null) THEN prem.ServiceAddressHouseNumber ELSE storage.AddressHouseNumber END,
                    'ApartmentNumber' = CASE WHEN (cOrder.PremiseID Is Not Null) THEN prem.ServiceAddressApartmentNumber ELSE storage.AddressApartmentNumber END,
	                'Street' = CASE WHEN (cOrder.PremiseID Is Not Null) THEN streetPremise.FullStName ELSE streetStorage.FullStName END,
	                'City' = CASE WHEN (cOrder.PremiseID Is Not Null) THEN townPremise.Town ELSE townStorage.Town END,
	                'Zip' = CASE WHEN (cOrder.PremiseID Is Not Null) THEN prem.ServiceZip ELSE storage.Zip END,
	                'State' = CASE WHEN (cOrder.PremiseID Is Not Null) THEN statePremise.Abbreviation ELSE stateStorage.Abbreviation END
                FROM 
                    [MeterRecorderChangeOrders] cOrder
                LEFT JOIN [MeterRecorders] recorders                         ON recorders.MeterRecorderID = cOrder.MeterRecorderID
                LEFT JOIN [Meters] meters                                    ON	meters.MeterID = cOrder.MeterID
                LEFT JOIN [Premises] prem                                    ON prem.PremiseID = cOrder.PremiseID
                LEFT JOIN [MeterRecorderStorageLocations] storage            ON storage.MeterRecorderStorageLocationID = cOrder.MeterRecorderStorageLocationID
                LEFT JOIN [MeterRecorderWorkDescriptions] workDescript       ON workDescript.MeterRecorderWorkDescriptionID = cOrder.MeterRecorderWorkDescriptionID

                -- Premise Address Join
                LEFT JOIN Streets streetPremise             ON prem.ServiceAddressStreet = streetPremise.StreetID
                LEFT JOIN Towns townPremise                 ON prem.ServiceCity = townPremise.TownID
                LEFT JOIN [States] statePremise             ON prem.ServiceState = statePremise.StateID

                -- Storage address join
                LEFT JOIN Streets streetStorage             ON storage.AddressStreetID = streetStorage.StreetID
                LEFT JOIN Towns townStorage                 ON storage.CityID = townStorage.TownID
                LEFT JOIN [States] stateStorage             ON storage.StateID = stateStorage.StateID
                WHERE
                    cOrder.MeterRecorderID = @MeterRecorderID
                ">
                <SelectParameters>
                    <%--I have no unearthly idea why this works.--%>
                    <asp:ControlParameter ControlID="DetailsView1" DbType="Int32" Name="MeterRecorderID" PropertyName="DataItem.MeterRecorderID" />
                </SelectParameters>
                </asp:SqlDataSource>

            </div>
            
            <div id="readings">
                <asp:UpdatePanel runat="server" ID="pnlReadings" UpdateMode="Conditional">
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnExportReadings" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:SqlDataSource runat="server" ID="dsReadings" 
                            SelectCommand="
                                select
	                                S.SensorID,
	                                S.SensorName, 
	                                S.SensorDesc,
	                                R.DateTimeStamp,
	                                R.ScaledData as Value
                                From 
	                                [Readings] R
                                inner join
	                                [Sensors] S
                                on
	                                S.SensorID = R.SensorID
                                inner join
	                                [Boards] B
                                on
	                                B.BoardID = S.BoardID
                                inner join
	                                MeterRecordersBoards mrb
                                on
	                                mrb.BoardID = B.BoardID
                                where
	                                MeterRecorderID = @MeterRecorderID
	                            order by datetimestamp desc
                            "
                            ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        >
                            <SelectParameters>
                                <asp:ControlParameter ControlID="Notes1" PropertyName="DataLinkID" Name="MeterRecorderID" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <asp:GridView runat="server" ID="gvReadings"
                            DataSourceID="dsReadings"
                            PageSize="15" AllowPaging="true"
                        >
                        </asp:GridView>
                        <asp:Button runat="server" ID="btnExportReadings" Text="Export"
                            OnClick="btnExportReadings_Click" 
                         />
                        
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            
            <div id="chart">
                <asp:UpdatePanel runat="server" ID="upChart" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <dotnet:Chart ID="Chart" runat="server" />
                        <div class="flowButtonsDiv">                        
                            <asp:TextBox runat="server" id="txtDate" Text='<%# CurrentlySearchedDate.ToShortDateString() %>' autocomplete="off" />
                            <asp:ImageButton OnClientClick="return false;" runat="server" ID="imgCal" ImageUrl="~/images/calendar.png" style="top:5px;" />
                            
                            <cc1:CalendarExtender TargetControlID="txtDate" PopupButtonID="imgCal" ID="cal1" runat="server" />
                            <asp:Button runat="server" id="btnUpdateChart" onclick="btnUpdateChart_Click" Text='Update' />
                            <br />
                            <asp:LinkButton ID="lbDatePrev" runat="server" OnClick="lbDatePrev_Click">&lt;</asp:LinkButton>
                            <asp:LinkButton ID="lbToday" runat="server" OnClick="lbToday_Click">Today</asp:LinkButton>
                            <asp:LinkButton ID="lbDateNext" runat="server" OnClick="lbDateNext_Click">&gt;</asp:LinkButton>
                            &nbsp;|&nbsp;
                            <asp:LinkButton ID="lbSizeMinus" runat="server" OnClick="lbSizeMinus_Click">-</asp:LinkButton>
                            <asp:LinkButton ID="lbSizePlus" runat="server" OnClick="lbSizePlus_Click">+</asp:LinkButton>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <atk:UpdatePanelAnimationExtender ID="upExt" runat="server" TargetControlID="upChart">
                    <Animations>
                        <OnUpdating>
                            <FadeOut Duration=".25" Fps="20"></FadeOut>
                        </OnUpdating>
                        <OnUpdated>
                            <FadeIn Duration=".1" Fps="20" />
                        </OnUpdated>
                    </Animations>
                </atk:UpdatePanelAnimationExtender>
            </div>
            
            <div id="notes">
                <mmsi:Notes ID="Notes1" runat="server" DataTypeID="132" />
            </div>
            
            <div id="documents">
                <mmsi:Documents ID="Documents1" runat="server" DataTypeID="132" />
            </div>
        </div>
        <center>
            <asp:Button runat="server" ID="Button4" CausesValidation="false" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="Button5" CausesValidation="false" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
        
    </asp:Panel>
   
</asp:Content>