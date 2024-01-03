<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderReadOnlyDetailView.ascx.cs" Inherits="LINQTo271.Views.WorkOrders.ReadOnly.WorkOrderReadOnlyDetailView" %>

<mmsinc:MvpFormView runat="server" ID="fvWorkOrder" DataSourceID="odsWorkOrder">
    <ItemTemplate>
        <div class="container_12" id="mainWrapper">
            <div class="grid_3 bold">
                Order Number:
                <asp:Label runat="server" ID="lblWorkOrderID" Text='<%# Eval("WorkOrderID") %>' />
            </div>
            <div class="grid_4 bold" style="text-align: center">
                271-Field Services Work Order
            </div>
            <div class="grid_2 fine-print" style="text-align: center">
                * Show time in 1/4 hrs (.25, .50, etc.)
            </div>
            <div class="grid_2 fine-print" style="text-align: center">
                Printed On<br />
                <asp:Label runat="server" ID="lblDatePrinted"><%= DateTime.Today.ToString("MM/dd/yyyy") %></asp:Label>
            </div>
            <div class="grid_1 fine-print" style="text-align: center">
                Updated On<br />
                <!-- UPDATED ON -->
                <asp:Label runat="server" ID="lblUpdated" />
            </div>
            <div class="grid_4 full-border" id="dateDiv">
                <table>
                    <tr>
                        <td class="bold">
                            Date Received:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblDateReceived" Text='<%# Eval("DateReceived", "{0:d}") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="bold">
                            Date Started:
                        </td>
                        <!-- DATE STARTED -->
                        <td>
                            <asp:Label runat="server" ID="lblDateStarted" Text='<%# Eval("DateStarted", "{0:d}") %>' />
                        </td>
                    </tr>
                    <tr>
                        <!-- DATE COMPLETED -->
                        <td class="bold">
                            Date Completed:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblDateCompleted" Text='<%# Eval("DateCompleted", "{0:d}") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="bold">
                            Requested By:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblRequestedBy" Text='<%# Eval("RequestingEmployee.FullName") ?? ( string.IsNullOrEmpty((string)Eval("CustomerName")) ? Eval("RequestedBy") : Eval("CustomerName") ) %>' />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="grid_8 full-border" id="locationInfoDiv">
                <div class="grid_4">
                    <table class="padded">
                        <tr>
                            <th>Address</th>
                        </tr>
                        <tr>
                            <td>
                                &nbsp
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblCustomerName" Text='<%# Eval("CustomerName") %>' />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblStreetAddress" Text='<%# Eval("StreetAddress") %>' />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblTownAddress" Text='<%# Eval("TownAddress") %>' />
                            </td>
                        </tr>
                        <tr>
                            <td style="font-size: small;">
                                Nearest Cross St:
                                <asp:Label runat="server" ID="lblNearestCrossStreet" Text='<%# Eval("NearestCrossStreet") %>' />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="grid_4" style="border-left: 2px solid black;">
                    <table class="padded">
                        <tr>
                            <td class="bold">
                                Service Number<br />
                                Hyd/Valve/BO:
                            </td>
                            <!-- SERVICE NUMBER -->
                            <td>
                                <asp:Label runat="server" ID="lblServiceNumber" Text='<%# Eval("AssetID") %>' />
                            </td>
                        </tr>
                        <tr>
                            <td class="bold">
                                Service Order #:
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="bold">
                                Premise Number:
                            </td>
                            <!-- PREMISE NUMBER -->
                            <td>
                                <asp:Label runat="server" ID="lblPremiseNumber" Text='<%# Eval("PremiseNumber") %>' />
                            </td>
                        </tr>
                        <tr>
                            <td class="bold">
                                Phone Number:
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblPhoneNumber" Text='<%# Eval("PhoneNumber") %>' />
                            </td>
                        </tr>
                        <tr>
                            <td class="bold">
                                Markout Required:
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblMarkoutRequired" Text='<%# Eval("MarkoutRequiredStr") %>' />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="grid_12 full-border">
                <div class="grid_5">
                    <table class="padded" id="jobDescriptionTable">
                        <tr style="height: 40px;">
                            <td class="bold">
                                Description of Job:
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblDescriptionOfWork" Text='<%# Eval("WorkDescription") %>' />
                            </td>
                        </tr>
                        <tr>
                            <td class="bold">
                                Created By:
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblCreatedBy" Text='<%# Eval("CreatedBy") %>' />
                            </td>
                        </tr>
                        <tr>
                            <td class="bold">
                                Priority:
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblPriority" Text='<%# Eval("Priority") %>' />
                            </td>
                        </tr>
                        <tr>
                            <td class="bold">
                                Backhoe Operator:
                            </td>
                            <!-- BACKHOE OPERATOR -->
                            <td>
                                <% //TODO: Wire up BackhoeOperator in WorkOrder class %> 
                                <asp:Label runat="server" ID="lblBackhoeOperator" />
                            </td>
                        </tr>
                        <tr>
                            <td class="bold">
                                Date of Excavation:
                            </td>
                            <!-- DATE OF EXCAVATION -->
                            <td>
                                <asp:Label runat="server" ID="lblDateOfExcavation" Text='<%# Eval("ExcavationDate", "{0:d}")%>' />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="grid_7">
                    <table id="jobInfoTable" class="padded">
                        <thead>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <th>
                                    1st
                                </th>
                                <th>
                                    2nd
                                </th>
                                <th>
                                    3rd
                                </th>
                                <th>
                                    4th
                                </th>
                                <th>
                                    5th
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <th>
                                    Employee Assigned:
                                </th>
                                <!-- ASSIGNED TO -->
                                <td>
                                    <asp:Label runat="server" ID="lblAssignedTo" />
                                </td>
                                <td>
                                    &nbsp
                                </td>
                                <td>
                                    &nbsp
                                </td>
                                <td>
                                    &nbsp
                                </td>
                                <td>
                                    &nbsp
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    # Employees Assigned:
                                </th>
                                <!-- # EMPLOYEES ASSIGNED -->
                                <td>
                                    <asp:Label runat="server" ID="lblNumberEmployeesAssigned" />
                                </td>
                                <td>
                                    &nbsp
                                </td>
                                <td>
                                    &nbsp
                                </td>
                                <td>
                                    &nbsp
                                </td>
                                <td>
                                    &nbsp
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Time Arrived on Job:
                                </th>
                                <!-- START TIME -->
                                <td>
                                    <asp:Label runat="server" ID="lblStartTime" />
                                </td>
                                <td>
                                    &nbsp
                                </td>
                                <td>
                                    &nbsp
                                </td>
                                <td>
                                    &nbsp
                                </td>
                                <td>
                                    &nbsp
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    *Total Time to Complete:
                                </th>
                                <!-- TIME TO COMPLETE -->
                                <td>
                                    <asp:Label runat="server" ID="lblTimeToComplete" />
                                </td>
                                <td>
                                    &nbsp
                                </td>
                                <td>
                                    &nbsp
                                </td>
                                <td>
                                    &nbsp
                                </td>
                                <td>
                                    &nbsp
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Truck #:
                                </th>
                                <!-- TRUCK NUMBER (CREW NAME?) -->
                                <td>
                                    <asp:Label runat="server" ID="lblTruckNumber" />
                                </td>
                                <td>
                                    &nbsp
                                </td>
                                <td>
                                    &nbsp
                                </td>
                                <td>
                                    &nbsp
                                </td>
                                <td>
                                    &nbsp
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="grid_12 full-border" id="markoutDiv">
                <!-- row 1 -->
                <div class="grid_4 bold">
                    Type of Markout:&nbsp;
                    <asp:Label runat="server" ID="lblMarkoutRequirement" Text='<%# Eval("MarkoutRequirement") %>' />
                </div>
                <div class="grid_4 bold">
                    Markout Number:&nbsp;
                    <!-- MARKOUT NUMBER -->
                    <asp:Label runat="server" ID="lblMarkoutNumber" Text='<%# Eval("CurrentMarkout.MarkoutNumber") %>' />
                </div>
                <div class="grid_3 bold">
                    2nd Req. Number&nbsp;
                    <asp:Label runat="server" ID="lblSecondMarkoutNumber" Text='<%# Eval("LastMarkout.MarkoutNumber") %>' />
                </div>
                <!-- row 2 -->
                <div class="grid_3">
                    <span class="bold">Date Requested:</span>&nbsp;
                    <!-- DATE MARKOUT REQUESTED -->
                    <asp:Label runat="server" ID="lblDateMarkoutRequested" Text='<%# Eval("CurrentMarkout.DateOfRequest", "{0:d}") %>'/>
                </div>
                <div class="grid_3">
                    <span class="bold">Time of Request:</span>&nbsp;
                    <!-- TIME MARKOUT REQUESTED -->
                    <asp:Label runat="server" ID="lblTimeMarkoutRequested" Text='<%# Eval("CurrentMarkout.DateOfRequest.TimeOfDay", "{0:t}") %>' />
                </div>
                <div class="grid_3">
                    <span class="bold">Due Date:</span>&nbsp;
                    <!-- MARKOUT DUE DATE -->
                    <asp:Label runat="server" ID="lblMarkoutDueDate" Text='<%# Eval("CurrentMarkout.ReadyDate", "{0:d}") %>' />
                </div>
                <div class="grid_2">
                    <span class="bold">*M O Down Time:</span>&nbsp;
                    <!-- MARKOUT DOWN TIME -->
                    <asp:Label runat="server" ID="lblMarkoutDownTime" />
                </div>
                <!-- row 3 -->
                <div class="grid_2">
                    <span class="bold">Gas:</span>&nbsp;
                    <!-- GAS MARKOUT -->
                    <asp:Label runat="server" ID="lblGasMarkout" />
                </div>
                <div class="grid_2">
                    <span class="bold">ATT:</span>&nbsp;
                    <!-- ATT MARKOUT -->
                    <asp:Label ID="lblATTMarkout" runat="server" />
                </div>
                <div class="grid_2">
                    <span class="bold">Electric:</span>&nbsp;
                    <!-- ELECTRIC MARKOUT -->
                    <asp:Label ID="lblElectricMarkout" runat="server" />
                </div>
                <div class="grid_2">
                    <span class="bold">Telephone:</span>&nbsp;
                    <!-- TELEPHONE MARKOUT -->
                    <asp:Label ID="lblTelephoneMarkout" runat="server" />
                </div>
                <div class="grid_2">
                    <span class="bold">Sewer:</span>&nbsp;
                    <!-- SEWER MARKOUT -->
                    <asp:Label ID="lblSewerMarkout" runat="server" />
                </div>
                <div class="grid_1">
                    <span class="bold">Cable:</span>&nbsp;
                    <!-- CABLE MARKOUT -->
                    <asp:Label ID="lblCableMarkout" runat="server" />
                </div>
            </div>
            <div class="grid_12 full-border">
                <div class="grid_4" id="mainMaterialDiv">
                    <table class="padded">
                        <thead>
                            <tr>
                                <th colspan="6">
                                    Main Material
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td class="underline">
                                    &nbsp;
                                </td>
                                <td class="bold">
                                    Cast
                                </td>
                                <td class="underline">
                                    &nbsp;
                                </td>
                                <td class="bold">
                                    PVC
                                </td>
                                <td class="underline">
                                    &nbsp;
                                </td>
                                <td class="bold">
                                    Transite
                                </td>
                            </tr>
                            <tr>
                                <td class="underline">
                                    &nbsp;
                                </td>
                                <td class="bold">
                                    Cement
                                </td>
                                <td class="underline">
                                    &nbsp;
                                </td>
                                <td class="bold">
                                    Galv
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="underline">
                                    &nbsp;
                                </td>
                                <td class="bold">
                                    Ductile
                                </td>
                                <td class="underline">
                                    &nbsp;
                                </td>
                                <td class="bold">
                                    Lock Joint
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="grid_4" id="serviceMaterialDiv">
                    <table class="padded">
                        <thead>
                            <tr>
                                <th colspan="4">
                                    Service Material
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td class="underline">
                                    &nbsp;
                                </td>
                                <td class="bold">
                                    Copper
                                </td>
                                <td class="underline">
                                    &nbsp;
                                </td>
                                <td class="bold">
                                    Galv
                                </td>
                            </tr>
                            <tr>
                                <td class="underline">
                                    &nbsp;
                                </td>
                                <td class="bold">
                                    Lead
                                </td>
                                <td class="underline">
                                    &nbsp;
                                </td>
                                <td class="bold">
                                    Tubaloyd
                                </td>
                            </tr>
                            <tr>
                                <td class="underline">
                                    &nbsp;
                                </td>
                                <td class="bold">
                                    Plastic
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="grid_4" id="typeOfLeakDiv">
                    <table class="padded">
                        <thead>
                            <tr>
                                <th colspan="4">
                                    Type of Leak
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td class="underline">
                                    &nbsp;
                                </td>
                                <td class="bold">
                                    Circle
                                </td>
                                <td class="underline">
                                    &nbsp;
                                </td>
                                <td class="bold">
                                    Physically Damaged
                                </td>
                            </tr>
                            <tr>
                                <td class="underline">
                                    &nbsp;
                                </td>
                                <td class="bold">
                                    Deteriorated
                                </td>
                                <td class="underline">
                                    &nbsp;
                                </td>
                                <td class="bold">
                                    Service Saddle
                                </td>
                            </tr>
                            <tr>
                                <td class="underline">
                                    &nbsp;
                                </td>
                                <td class="bold">
                                    Joint Leak
                                </td>
                                <td class="underline">
                                    &nbsp;
                                </td>
                                <td class="bold">
                                    Split
                                </td>
                            </tr>
                            <tr>
                                <td class="underline">
                                    &nbsp;
                                </td>
                                <td class="bold">
                                    Pin Hole
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="grid_12" id="assetInfoDiv">
                    <!-- row 1 -->
                    <div class="grid_3">
                        <span class="bold">Size of Main/Service:</span>&nbsp;
                        <!-- SIZE OF MAIN/SERVICE -->
                        <asp:Label runat="server" ID="lblSizeOfMain" />
                    </div>
                    <div class="grid_3">
                        <span class="bold">Depth of Main/Serivce (ft):</span>&nbsp;
                        <!-- DEPTH OF MAIN/SERVICE -->
                        <asp:Label runat="server" ID="lblDepthOfMain" />
                    </div>
                    <div class="grid_4">
                        <span class="bold">Condition of Main/Service:</span>&nbsp;
                        <!-- CONDITION OF MAIN/SERVICE -->
                        <asp:Label runat="server" ID="lblConditionOfMain" />
                    </div>
                    <div class="grid_2">
                        <span class="bold">Map Page:</span>&nbsp;
                        <!-- MAP PAGE -->
                        <asp:Label runat="server" ID="lblMapPage" />
                    </div>
                    <!-- row 2 -->
                    <div class="grid_4" style="width: 364px">
                        <span class="bold">Leak Between Or Near Valve Number:</span>&nbsp;
                        <!-- START VALVE NUMBER -->
                        <asp:Label runat="server" ID="lblStartValve" />
                    </div>
                    <div class="grid_4">
                        <span class="bold">And Valve Number:</span>&nbsp;
                        <!-- END VALVE NUMBER -->
                        <asp:Label runat="server" ID="lblEndValve" />
                    </div>
                    <div class="grid_3" style="width: 246px">
                        <span class="bold">Lost Water (Gallons):</span>&nbsp;
                        <asp:Label runat="server" ID="lblLostWater" Text='<%# Eval("LostWater") %>' />
                    </div>
                    <!-- row 3 -->
                    <div class="grid_4">
                        <span class="bold">Safety Markers Left On Site:</span>&nbsp;
                        <!-- MARKERS ON SITE -->
                        <asp:Label runat="server" ID="lblMarkersOnSite" />
                    </div>
                    <div class="grid_4">
                        <span class="bold">Number of Markers:</span>&nbsp;
                        <!-- NUMBER OF MARKERS -->
                        <asp:Label runat="server" ID="lblNumberOfMarkers" />
                    </div>
                    <div class="grid_4" style="width: 304px">
                        <span class="bold">Safety Markers Retrieved:</span>&nbsp;
                        <!-- DATE MARKERS RETRIEVED -->
                        <asp:Label runat="server" ID="lblDateMarkersRetrieved" />
                    </div>
                </div>
            </div>
            <div class="grid_12 full-border" id="materialsUsedDiv">
                <div style="width: 928px;height: 20px;font-weight: bold;text-align: center">
                    Material Used (Include description, quantity, and part number. Put any additional
                    parts on back of form.)
                </div>
                <table style="width: 928px; height: 205px" id="materialsUsedTable">
                    <tbody>
                        <tr>
                            <td style="width: 340px">
                                &nbsp;
                            </td>
                            <td class="truck-cell" style="width: 35px">
                                Truck
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td class="truck-cell">
                                Truck
                            </td>
                            <td rowspan="3" style="border-style: none; width: 130px; height: 120px;">
                                <table id="materialsUsedAccountNumbersTable">
                                    <tr>
                                        <td>
                                            MAINS
                                        </td>
                                        <td style="text-align: right">
                                            \18730174.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            SERV
                                        </td>
                                        <td style="text-align: right">
                                            \18730184.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            MTR SE
                                        </td>
                                        <td style="text-align: right">
                                            \18730186.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            HYD
                                        </td>
                                        <td style="text-align: right">
                                            \18730189.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            CAPITAL\RETIRE\MJ
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="border: 1px solid black">
                                            <asp:Label runat="server" ID="lblAccountCharged" Text='<%# Eval("AccountCharged") %>' />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td class="truck-cell" onclick="alert(this.style.width)">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td class="truck-cell">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td class="truck-cell">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td class="truck-cell">
                                &nbsp;
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="grid_12 full-border" id="notesDiv">
                <span class="bold">Notes:</span><br />
                <asp:Label runat="server" ID="lblNotes" Text='<%# Eval("Notes") %>' />
            </div>
            <div class="grid_4 prefix_4 suffix_4">
                Check if additional notes or paving on back: [&nbsp;&nbsp;]
            </div>
        </div>
    </ItemTemplate>
</mmsinc:MvpFormView>

<mmsinc:MvpObjectContainerDataSource runat="server" ID="odsWorkOrder" DataObjectTypeName="WorkOrders.Model.WorkOrder" />
