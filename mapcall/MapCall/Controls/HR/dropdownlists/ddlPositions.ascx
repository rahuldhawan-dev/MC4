<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ddlPositions.ascx.cs" Inherits="MapCall.Controls.HR.dropdownlists.ddlPositions" %>
<asp:DropDownList runat="server" ID="ddl_Positions" 
    DataSourceID="ds_Positions" 
    AppendDataBoundItems="true"
    DataTextField="PositionText"
    DataValueField="PositionID"
    >
    <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
</asp:DropDownList>
<asp:SqlDataSource runat="server" ID="ds_Positions"
    ConnectionString="<%$ ConnectionStrings:MCProd %>"
    SelectCommand="
SELECT distinct 
	positionID, 
	isNull(coalesce(operatingcentercode, opcode),'') + '-' + isNull(position, '') + ' [' + cast(positionID as varchar(15)) + ']' as [PositionText], 
	coalesce(operatingcentercode, opcode) as opCode,
	position
from 
	tblPositions_Classifications 
left join 
	LocalBargainingUnits
on
	LocalBargainingUnits.Id = tblPositions_Classifications.localiD
left join 
	OperatingCenters
on
	OperatingCenters.OperatingCenterID = LocalBargainingUnits.OperatingCenterId
order by 
	opcode, position">
</asp:SqlDataSource>
<asp:RequiredFieldValidator runat="server" ID="rfv_ddl_Positions" ControlToValidate="ddl_Positions" InitialValue="" Text="Required" SetFocusOnError="true" Enabled="false"></asp:RequiredFieldValidator>