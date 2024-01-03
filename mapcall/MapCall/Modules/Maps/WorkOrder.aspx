<%@ Page Title="" Language="C#" MasterPageFile="~/MapCall.Master" AutoEventWireup="true" CodeBehind="WorkOrder.aspx.cs" Inherits="MapCall.Modules.Maps.WorkOrder" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:DetailsView runat="server" ID="dvWorkOrder"
        DataSourceID="dsWorkOrder" CellPadding="1" ForeColor="#333333" 
        GridLines="None" AutoGenerateRows="False"
    >
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <CommandRowStyle BackColor="#E2DED6" Font-Bold="True" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Names="Arial" Font-Size="Small" />
        <FieldHeaderStyle BackColor="#E9ECF1" Font-Bold="True" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <Fields>
            <asp:HyperLinkField DataTextField="WorkOrderID" HeaderText="WorkOrderID" 
                InsertVisible="False" SortExpression="WorkOrderID" 
                DataNavigateUrlFields="WorkOrderID"
                DataNavigateUrlFormatString="~/Modules/Mvc/FieldOperations/GeneralWorkOrder/Show/{0}"
                Target="_blank"    
            />
            <asp:HyperLinkField HeaderText="" 
                InsertVisible="False" SortExpression="WorkOrderID" 
                DataNavigateUrlFields="WorkOrderID"
                Text="Print"
                DataNavigateUrlFormatString="~/Modules/Mvc/FieldOperations/GeneralWorkOrder/Show/{0}.pdf"
                Target="_blank"    
            />
            <asp:BoundField DataField="Work Description" HeaderText="Work Description" 
                SortExpression="Work Description" />
            <asp:BoundField DataField="Address" HeaderText="Address" />
            <asp:BoundField DataField="ApartmentAddtl" HeaderText="Apartment Addtl" />
            <asp:BoundField DataField="Contractor" HeaderText="Contractor" />
        </Fields>
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#999999" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    
    </asp:DetailsView>
    <hr />
    <asp:GridView runat="server" ID="gvCrewAssignments" 
        DataSourceID="dsCrewAssignments" CellPadding="4" ForeColor="#333333" GridLines="None"
        Font-Name="Arial" Font-Size="Small"
    >
        <RowStyle BackColor="#EFF3FB" Font-Size="Small" />
        <HeaderStyle Font-Bold="True" BorderStyle="Solid" BorderColor="#507CD1" BorderWidth="1" />
        <AlternatingRowStyle BackColor="White" />
    </asp:GridView>
    <asp:SqlDataSource runat="server" ID="dsWorkOrder"
        ConnectionString="<%$ ConnectionStrings:MCProd %>" 
        SelectCommand="
            select
	            wo.WorkOrderID,
	            (select description from WorkDescriptions wd where wd.workdescriptionID = wo.workDescriptionID) as [Work Description], 
				isNull(wo.streetnumber,'') + ' ' + isNull((select top 1 fullstname from Streets s where s.StreetID = wo.streetID),'') + ', ' + isNull(tn.town,'') + ', ' + isNull(tn.State,'') as Address,
                wo.ApartmentAddtl,
                c.Name as Contractor
            from 
	            workorders wo
			left join 
				Towns tn
			on
				tn.TownID = wo.TownID
            left join [Contractors] c on c.ContractorID = wo.AssignedContractorID
	        where
	            wo.workOrderID = @ID
        "
    >
        <SelectParameters>
            <asp:QueryStringParameter Name="ID" QueryStringField="ID" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource runat="server" ID="dsCrewAssignments"
        ConnectionString="<%$ ConnectionStrings:MCProd %>" 
        SelectCommand="
            Select 
                AssignedFor, 
                (select Description from Crews c where c.CrewID = ca.CrewID) as [Crew], 
                DateStarted, 
                DateEnded 
            From 
                CrewAssignments ca 
            where 
                workOrderID = @ID
        "
    >
        <SelectParameters>
            <asp:QueryStringParameter Name="ID" QueryStringField="ID" />
        </SelectParameters>    
    </asp:SqlDataSource>
</asp:Content>
