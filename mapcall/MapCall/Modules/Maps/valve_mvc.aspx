<%@ Page Theme="bender" MasterPageFile="~/MapCall.Master" Language="C#" AutoEventWireup="true" CodeBehind="valve_mvc.aspx.cs" Inherits="MapCall.Modules.Maps.valve_mvc" %>
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
    <asp:FormView runat="server" ID="FormView1"
        DataKeyNames="RecID" DataSourceID="SqlDataSource1" Height="100%" Width="100%"
        OnDataBound="FormView1_DataBound" BackColor="White" RowStyle-VerticalAlign="Top"
        EmptyDataText="Invalid ID"
        >
        <ItemTemplate>
            <table class="map-popup">
                <tr>
                    <td>
                        <b>Valve #:</b> 
                        <asp:HyperLink runat="server" ID="hlValve" Target="_new" />
                    </td>
                    <td>
                        <b>Status:</b>
                        <%# Eval("ValveStatus") %>
                    </td>
                </tr>
                <tr>    
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblCritical" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Type:</b> <asp:Label runat="server" ID="lblValType" Text='<%# Bind("ValveType") %>' />
                    </td>
                    <td>
                        <b>Size:</b> <asp:Label runat="server" ID="lblValveSize" Text='<%# Bind("ValveSize") %>'></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Turns:</b> <asp:Label runat="server" ID="lblTurns" Text='<%# Bind("Turns") %>'></asp:Label>
                    </td>
                    <td>
                        <b>Valve Zone:</b> <%# Eval("ValveZone") %>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Last Inspected:</b>
                        </td><td>
                        <asp:Label runat="server" id="lblLastInspection" Text='<%# Bind("DateInspected") %>'></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Functional Location:</b>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblFunctionalLocation" Text='<%#Eval("FunctionalLocation") %>'></asp:Label>
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:FormView>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        CancelSelectOnNullParameter="false"
        ConnectionString="<%$ ConnectionStrings:MCProd %>"
        ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>"
        SelectCommand="
            SELECT distinct
                V.Id as RecID, 
	            V.ValveNumber,
	            V.Critical,
	            V.CriticalNotes,
	            vs.Description as ValveStatus,
	            (Select Top 1 DateInspected from ValveInspections VI where VI.ValveID = V.ID order by DateInspected desc) as DateInspected,
	            (Select Top 1 Description from ValveInspections VI left join ValveWorkOrderRequests vwo on vwo.Id = vi.WorkOrderRequest1Id where VI.ValveID = V.ID order by DateInspected desc) as LastInspectionWorkOrder,
	            vt.Description as ValveType,
	            sz.Size as ValveSize,
	            V.Turns,
	            vz.Description as ValveZone,
                fl.Description as FunctionalLocation
            FROM Valves V 
	            LEFT JOIN FunctionalLocations fl on fl.FunctionalLocationID = V.FunctionalLocationID
	            LEFT JOIN AssetStatuses vs on vs.AssetStatusId = V.AssetStatusId
	            LEFT JOIN ValveTypes vt on vt.Id = ValveTypeId
	            LEFT JOIN ValveSizes sz on sz.Id = ValveSizeId
	            LEFT JOIN ValveZones vz on vz.Id = ValveZoneId
            where 
                V.Id = @RecID">
        <SelectParameters>
            <asp:Parameter Name="RecID" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
