<%@ Page Language="C#" MasterPageFile="~/MapCallSite.Master" Theme="bender" AutoEventWireup="true" CodeBehind="home.aspx.cs" Inherits="MapCall.Modules.HR.home" Title="MapCall - American Water - Operations"%>
<%@ Register Assembly="MMSINC.Core.WebForms" Namespace="MMSINC.Controls" TagPrefix="mmsinc" %>

<asp:Content ContentPlaceHolderID="cphHeadTagScripts" runat="server">
    <mmsinc:ScriptInclude runat="server" IncludesPath="~/includes/" ScriptFileName="jquery.cycle.all.js" />
</asp:Content>
<asp:Content ContentPlaceHolderID="cphHeadTag" runat="server">
    <link rel="Stylesheet" href="<%#ResolveUrl("~/Modules/HR/home.css") %>" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
<table style="width: 100%;">
    <tr>
        <td>Operations</td>
        <td style="text-align: right;"><a href="mailto:mapcall@amwater.com" style="color: white;">Email Help/Support</a></td>
    </tr>
</table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
<div class="container subContainer">
    <div class="body">
        <div id="notifications">
            <asp:Repeater runat="server" ID="blNotifications"
                DataSourceID="dsNotifications" EnableViewState="false">
                <ItemTemplate>
                    <div>
                        <h2>
                            <%#Eval("Category") %> - <%#Eval("Title") %>
                        </h2>
                        <p>
                            <%#Eval("Description") %>    
                        </p>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <asp:SqlDataSource runat="server" ID="dsNotifications"
            ConnectionString="<%$ ConnectionStrings:MCProd %>" 
            SelectCommand = "
                Select 
                    Notifications.*,
                    LookupValue as Category
                from 
                    Notifications 
                left join
                    lookup l
                on
                    l.lookupID = NotificationCategory
                where 
                    getDate() &lt; TimeoutDate 
                and
                    getDate() &gt;= isNull(startDate,getDate())
                and
                    IsPublic = 0
                order by 
                    NotificationID
                "/>
        </div>
        <div id="pager"></div>
        <div style="color: gray;">[To pause the notification, place your mouse over the message.]</div>
    </div>
</div>

<div class="container boxContainer bc-2col">
    <div>
        <div class="bc-box bc-box-field-services container">
            <h2>Field Services</h2>
            <div class="boxContainer bc-2col">
                <div>
                    <ul>
                        <li><a href="<%= ResolveUrl("~/Modules/Mvc/FieldOperations/Hydrant/Search") %>">Hydrants</a></li>
                        <li><a href="<%= ResolveUrl("~/Modules/Mvc/FieldOperations/Valve/Search") %>">Valves</a></li>
                        <li><a href="<%= ResolveUrl("~/Modules/Mvc/FieldOperations/Service/Search") %>">Services</a></li>
                        <li><a href="<%= ResolveUrl("~/Modules/Mvc/Customer/Premise/Search") %>">Premises</a></li>
                        <li><a href="<%= ResolveUrl("~/Modules/mvc/FieldOperations/SewerOpening/Search") %>">Sewer Opening</a></li>
                        <li><a href="<%= ResolveUrl("~/Modules/mvc/FieldOperations/SAPNotification/Search") %>">SAP Notifications</a></li>
                    </ul>
                </div>
                <div>
                    <ul>
                        <li><a href="https://wateroutages.awapps.com/" target="_blank" rel="noopener noreferrer">Water Outages</a></li>
                        <li><a href="<%= ResolveUrl("~/Modules/mvc/FieldOperations/OneCallMarkoutTicket/Search") %>">Markout Tickets</a></li>
                        <li><a href="<%= ResolveUrl("~/Modules/Maps/RealTimeOperations.aspx") %>">RTO</a></li>
                        <li><a href="<%= ResolveUrl("~/Modules/Mvc/Reports/") %>">Reports</a></li>
                        <li><a href="<%= ResolveUrl("~/Modules/Mvc/FieldOperations/TapImage/Search") %>">Tap Images</a></li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="bc-box bc-box-waterquality container">
            <h2>Water Quality</h2>
            <div class="boxContainer bc-2col">
                <div>
                    <ul>
                        <li><a href="<%= ResolveUrl("~/Modules/Mvc/WaterQuality/WaterQualityComplaint/Search") %>">Complaints</a></li>
                        <li><a href="<%= ResolveUrl("~/Modules/Mvc/WaterQuality/WaterSample/Search") %>">Samples</a></li>
                        <li><a href="<%= ResolveUrl("~/Modules/Mvc/WaterQuality/SampleSite/Search") %>">Sample Sites</a></li>
                        <li><a href="<%= ResolveUrl("~/Modules/Mvc/WaterQuality/BacterialWaterSample/Search") %>">Samples Bacti</a></li>
                    </ul>
                </div>
                <div>
                    <ul>
                        <li><a href="<%= ResolveUrl("~/Modules/mvc/Facility/Search") %>">Facilities</a></li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="bc-box newDoc container">
            <h2>Business Performance</h2>
            <ul>
                <li><a href="<%= ResolveUrl("~/Modules/HR/Administrator/BusPerformanceKPI.aspx") %>">KPI</a></li>
            </ul>
        </div>
        <div class="bc-box bc-box-training container">
            <h2>Training</h2>
            <ul>
                <li><a href="<%= ResolveUrl("~/Modules/mvc/TrainingRecord/Search") %>">Training Records</a></li>
            </ul>
        </div>
                        
        <div class="bc-box bc-box-field-services container">
            <h2>Production/Treatment</h2>
            <div class="boxContainer bc-2col">
                <div>
                    <ul>
                        <li><a href="<%= ResolveUrl("~/Modules/mvc/Equipment/New") %>">Create Equipment</a></li>
                        <li><a href="<%= ResolveUrl("~/Modules/mvc/Equipment/Search") %>">Equipment</a></li>
                        <li><a href="<%= ResolveUrl("~/Modules/mvc/SAP/SAPMaintenancePlan/Search") %>">Maintenance Plans</a></li>
                        <li><a href="<%= ResolveUrl("~/Modules/mvc/Production/MaintenancePlan/Search") %>">Planned Work - Maintenance Plans</a></li>
                        <li><a href="<%= ResolveUrl("~/Modules/mvc/Facility/Search") %>">Facilities</a></li>
                    </ul>
                </div>
                <div>
                    <ul>
                        <li><a href="<%= ResolveUrl("~/Modules/mvc/Production/Scheduling/Search") %>">Scheduling</a></li>
                        <li><a href="<%= ResolveUrl("~/Modules/mvc/Production/EmployeeAssignment/Search") %>">Assignments</a></li>
                        <li><a href="<%= ResolveUrl("~/Modules/mvc/Production/ProductionWorkOrder/New") %>">Create Work Order</a></li>
                        <li><a href="<%= ResolveUrl("~/Modules/mvc/Production/ProductionPreJobSafetyBrief/New") %>">Create Prod Daily Task Pre Job Safety Brief</a></li>
                        <li><a href="<%= ResolveUrl("~/Modules/mvc/Production/ProductionWorkOrder/Search") %>">Work Order Search</a></li>
                    </ul>
                </div>
            </div>

        </div>                        

        <div class="bc-box bc-box-field-services container">
            <h2>Short Cycle</h2>
            <div class="boxContainer bc-2col">
                <div>
                    <ul>
                        <li><a href="<%= ResolveUrl("~/Modules/mvc/HealthAndSafety/ShortCycleWorkOrderSafetyBrief/Search") %>">Pre Job Safety Brief</a></li>
                        <li><a href="<%= ResolveUrl("~/Modules/mvc/HealthAndSafety/ConfinedSpaceForm/Search") %>">Confined Space Forms</a></li>
                    </ul>
                </div>
                <div>
                    <ul>
                    </ul>
                </div>
            </div>
        </div>    
    </div>
    <div>
        <div class="bc-box bc-box-work-management container">
            <h2>Work Management</h2>
            <div class="boxContainer bc-2col">
                <div>
                    <ul>
                        <li><a href="<%= ResolveUrl("~/Modules/mvc/FieldOperations/CrewAssignment/ShowCalendar") %>">Crew Assignments</a></li>
                        <li><a href="<%= ResolveUrl("~/Modules/mvc/FieldOperations/WorkOrder/New")%>">Create Work Order</a></li>
                        <li>
                            <a href="<%= ResolveUrl("~/Modules/mvc/FieldOperations/WorkOrderPlanning/Search") %>">Planning</a>
                        </li>
                        <li><a href="<%= ResolveUrl("~/Modules/mvc/FieldOperations/WorkOrderScheduling/Search") %>">Scheduling</a></li>
                        <li><a href="<%= ResolveUrl("~/Modules/WorkOrders/Views/WorkOrders/General/WorkOrderGeneralResourceView.aspx") %>">General</a></li>
                        <li><a href="<%= ResolveUrl("~/Modules/mvc/FieldOperations/GeneralWorkOrder/Search") %>">General - with mapping</a></li>
                    </ul>
                </div>
                <div>
                    <ul>
                        <li><em>Forms</em></li>
                        <li><a href="<%= ResolveUrl("~/Modules/mvc/HealthAndSafety/JobSiteCheckList/Search") %>">Job Site Check Lists</a></li>
                        <li><a href="<%= ResolveUrl("~/Modules/mvc/FieldOperations/MarkoutDamage/Search") %>">Markout Damages</a></li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="bc-box bc-box-vehicles container">
            <h2>Vehicles</h2>
            <ul>
                <li><a href="<%= ResolveUrl("~/Modules/Mvc/FleetManagement/Vehicle/Search") %>">Vehicles</a></li>
            </ul>
        </div>
        <div class="bc-box bc-box-contractors container">
            <h2>Contractors</h2>
            <ul>
                <li><a href="<%= ResolveUrl("~/Modules/mvc/Contractors/Contractor/Search") %>">Contractors</a></li>
                <li><a href="<%= ResolveUrl("~/Modules/mvc/Contractors/ContractorAgreement/Search") %>">Agreements</a></li>
            </ul>
        </div>
   
        <div class="bc-box bc-box-health container">
            <h2>Health & Safety</h2>
            <div class="boxContainer bc-2col">
                <div>
                    <ul>
                        <li><a href="<%= ResolveUrl("~/Modules/mvc/HealthAndSafety/TailgateTalk/Search") %>">Tailgate Talks</a></li>
                        <li><a href="<%= ResolveUrl("~/Modules/mvc/HealthAndSafety/JobObservation/Search") %>">Job Observations</a></li>
                        <li><a href="<%= ResolveUrl("~/Modules/Mvc/Incident/Search") %>">Incidents</a></li>
                        <li><a href="<%= ResolveUrl("~/Modules/mvc/HealthAndSafety/NearMiss/Search") %>">Near Misses</a></li>
                    </ul>
                </div>
                <div>
                    <ul>
                        <li><a href="<%= ResolveUrl("~/Modules/mvc/HealthAndSafety/LockoutForm/Search") %>">Lockout Form Search</a></li>
                        <li><a href="<%= ResolveUrl("~/Modules/mvc/Production/ProductionPreJobSafetyBrief/New") %>">Create Prod Daily Task Pre Job Safety Brief</a></li>
                    </ul>
                </div>
            </div>
        </div>
       
        <div class="bc-box bc-box-waterquality">
            <h2>Monitoring</h2>
            <ul>
                <li><a href="https://app.trimbleunity.com/login" target="_new">Telog</a></li>
                <li><a href="https://www.omnicoll.net/datagate/PermanetWebLogin.aspx">FCS</a></li>
            </ul>
        </div>
    </div>
</div>
    
<div>
    <div class="footer-body">
        <div class="copyright"><a href="https://amwater.com">&copy; <%= DateTime.Now.Year %> American Water</a></div>
        <div class="links"><a href="https://amwater.com/corp/privacy-policy">Privacy Policy</a> | <a href="https://amwater.com/corp/terms-of-use">Terms of Use</a></div>
    </div>
</div>

<script type="text/javascript">
    jQuery(document).ready(function() {
        var n = jQuery('#notifications');
        // cycle throws an error if the container has 0 children.
        if (n.find('div').length > 0) {
          n.cycle({
              timeout: 10000,
              fx: 'scrollUp',
              pager: jQuery("#pager"),
              pause: 1
          });
        }
    });
</script>

</asp:Content>
