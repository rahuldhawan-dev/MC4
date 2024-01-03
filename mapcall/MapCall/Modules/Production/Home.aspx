<%@ Page Title="Production" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="MapCall.Modules.Production.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadTag" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
    Production
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">
    <div class="container boxContainer bc-2col">
        <div>
            <div class="bc-box bc-box-field-services container">
                <h2>Links</h2>
                <div class="boxContainer bc-2col">
                    <div>
                        <ul>
                            <li><a href="<%= ResolveUrl("~/Modules/mvc/Facility/Search") %>">Facilities</a></li>
                            <li><a href="<%= ResolveUrl("~/Modules/mvc/Equipment/Search") %>">Equipment</a></li>
                            <li><a href="<%= ResolveUrl("~/Modules/mvc/Interconnection/Search") %>">Interconnections</a></li>
                            <li><a href="<%= ResolveUrl("~/Modules/mvc/FilterMedia/Search") %>">Filter Media</a></li>
                            <li><a href="<%= ResolveUrl("~/Modules/mvc/Town")%>">Towns</a></li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <div>
            <div class="bc-box bc-box-waterquality container">
                <h2>Water Quality</h2>
                <ul>
                    <li><a href="<%= ResolveUrl("~/Modules/WaterQuality/Complaints.aspx") %>">Complaints</a></li>
                    <li><a href="<%= ResolveUrl("~/Modules/Mvc/WaterQuality/WaterSample/Search") %>">Samples</a></li>
                    <li><a href="<%= ResolveUrl("~/Modules/Mvc/WaterQuality/BacterialWaterSample/Search") %>">Samples Bacti</a></li>
                </ul>
            </div>
        </div>
    </div>
    <div class="container boxContainer bc-2col">
        <div>
            <div class="bc-box bc-box-field-services container">
                <h2>Work Orders</h2>
                <div class="boxContainer bc-2col">
                    <div>
                        <ul>
                            <li><a href="<%= ResolveUrl("~/Modules/mvc/Production/ProductionWorkOrder/New") %>">Create Work Order</a></li>
                            <li><a href="<%= ResolveUrl("~/Modules/mvc/Production/ProductionWorkOrder/Search") %>">Work Orders</a></li>
                            <li><a href="<%= ResolveUrl("~/Modules/mvc/Production/EmployeeAssignment/Search") %>">Employee Assignment</a></li>
                            <li><a href="<%= ResolveUrl("~/Modules/mvc/SAP/SAPMaintenancePlan/Search") %>">SAP Maintenance Plans</a></li>                        
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>    
</asp:Content>
