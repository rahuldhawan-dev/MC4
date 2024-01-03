<%@ Page Title="" Language="C#" MasterPageFile="~/MapCall.Master" AutoEventWireup="true" CodeBehind="iVehicle.aspx.cs" Inherits="MapCall.Modules.Maps.iVehicle" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:DetailsView runat="server" ID="dvVehicle"
        DataSourceID="dsVehicle"
        CellPadding="1" ForeColor="#333333" GridLines="None"
        EmptyDataText="Could not locate a record for this vehicle."
        Width="100%"
    >
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <CommandRowStyle BackColor="#E2DED6" Font-Bold="True" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Names="Arial" Font-Size="Small" />
        <FieldHeaderStyle BackColor="#E9ECF1" Font-Bold="True" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#999999" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    </asp:DetailsView>
    <asp:SqlDataSource runat="server" ID="dsVehicle"
        ConnectionString="<%$ ConnectionStrings:MCProd %>" 
        SelectCommand="
            select 
	            Vehicle_Label,
	            (select fullname from employees where tblEmployeeID = V.VehicleAssignedTo) as Employee,
	            * 
            from 
	            VEHICLES V 
            left join 
	            tblVehicleLocations VL 
            on 
	            VL.VehicleID = V.Vehicle_Label 
            left join
                VehicleIcons vi
            on
                vi.VehicleIconID = v.VehicleIconID
            where
	            VL.VehicleLocationID = @ID
        "
    >
        <SelectParameters>
            <asp:QueryStringParameter Name="ID" QueryStringField="ID" />
        </SelectParameters>  
    </asp:SqlDataSource>
</asp:Content>
