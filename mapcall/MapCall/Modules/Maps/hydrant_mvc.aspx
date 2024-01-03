<%@ Page Language="C#" AutoEventWireup="true" Theme="bender" MasterPageFile="~/MapCall.Master" CodeBehind="hydrant_mvc.aspx.cs" Inherits="MapCall.Modules.Maps.hydrant_mvc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .map-popup 
        {
            font-family:'Trebuchet MS', Helvetica, sans-serif; 
            font-size:12px;
            border-collapse: collapse;
            width: 100%;
        }
        .map-popup td 
        {
            padding:3px;
            border-bottom:1px solid #DDDDDD;
        }
    </style>

    <asp:FormView ID="FormView1" runat="server"
        DataKeyNames="RecID" DataSourceID="SqlDataSource1" Height="100%" Width="100%"
        OnDataBound="FormView1_DataBound" BackColor="White" RowStyle-VerticalAlign="Top"
        >
        <ItemTemplate>
            <table class="map-popup">
                <tbody>
                <tr>
                    <td>
                        <b>Hydrant #:</b> 
                        <asp:HyperLink runat="server" ID="hlHydrant" Target="_new" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblCritical" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Manufacturer:</b> <asp:Label runat="server" ID="lblManufacturer" Text='<%# Bind("Manufacturer") %>' />
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Last Inspected:</b>
                        <asp:Label runat="server" id="lblLastInspection" Text='<%# Bind("LastInspection") %>'></asp:Label><br/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Inspection Issues:</b>
                        <asp:Label runat="server" id="lblWorkOrder" Text='<%# Bind("WorkOrderRequest") %>'/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Functional Location</b>: 
                        <asp:Label runat="server" id="lblFunctionalLocation" Text='<%#Eval("FunctionalLocation")%>'/> 
                    </td>
                                    
                </tr>
                </tbody>
            </table>
        </ItemTemplate>
    </asp:FormView>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
        CancelSelectOnNullParameter="false" 
        ConnectionString="<%$ ConnectionStrings:MCProd %>"
        ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>"
        SelectCommand="
            select 
			    h.HydrantNumber, 
			    (select top 1 DateInspected from HydrantInspections where HydrantID = H.Id order by DateInspected Desc) as [LastInspection], 
			    Critical, 
			    criticalNotes, 
			    H.Id as RecID,
                (select top 1 wor.Description from HydrantInspections left join workOrderRequests wor on wor.Id = HydrantInspections.WorkOrderRequest1 where HydrantID = H.Id order by DateInspected Desc) as WorkOrderRequest, 
                hm.Description as Manufacturer, 
			    fl.Description as FunctionalLocation
			
            from 
			    Hydrants H 
            LEFT JOIN FunctionalLocations fl on fl.FunctionalLocationID = H.FunctionalLocationID 
		    LEFT JOIN HydrantManufacturers hm on hm.Id = H.ManufacturerID
            where 
                H.Id = @RecId">
        <SelectParameters>
            <asp:Parameter Name="RecID" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>        
</asp:Content>
