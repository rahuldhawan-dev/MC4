<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="iFacility.aspx.cs" Inherits="MapCall.Modules.Maps.iFacility" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
        body {font-family:arial;font-size:12px;margin:0px;padding:0px;text-align:left;}
        .leftCol {vertical-align:top;text-align:right;font-weight:bold;border-bottom:1px solid black;width:150px;}
        .rightCol {vertical-align:top;text-align:left;font-weight:normal;border-bottom:1px solid black}
        .pnlImageLink {position:absolute;top:0px;right:0px;cursor:pointer;color:navy;text-decoration:underline;padding-right:9px;}	
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
        <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
            <cc1:TabPanel runat="server" HeaderText="Facility Info" ID="TabPanel1">
                <ContentTemplate>
                    <asp:DetailsView id="DetailsView1" runat="server" ForeColor="#333333" CellPadding="4" Width="100%" AutoGenerateRows="False" DataSourceID="SqlDataSource1" >
                        <AlternatingRowStyle BackColor="#E9ECF1"></AlternatingRowStyle>
                        <Fields >
                        <asp:BoundField DataField="facilityID" HeaderText="Facility ID : " />
                        <asp:BoundField DataField="FacilityName" HeaderText="Facility Name : " />
                        <asp:BoundField DataField="Notes" HeaderText="Description : " />
                        <asp:BoundField DataField="CompanySubsidiary" HeaderText="Company Subsidiary : " />
                        <asp:BoundField DataField="Operations" HeaderText="Operations : " />
                        <asp:BoundField DataField="District" HeaderText="District : " />
                        <asp:BoundField DataField="System" HeaderText="System : " />
                        <asp:BoundField DataField="PWSID" HeaderText="PWSID : " />
                        <asp:BoundField DataField="OpCode" HeaderText="OpCode : " />
                        </Fields>
                        </asp:DetailsView> 
                        <asp:SqlDataSource id="SqlDataSource1"  runat="server" 
                        SelectCommand="
                            Select tblFacilities.*, oc.OperatingCenterCode as OpCode from tblFacilities left join operatingCenters oc on oc.OperatingCenterID = tblFacilities.OperatingCenterID where recordID = @recordID" 
                            ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>" 
                            ConnectionString="<%$ ConnectionStrings:MCProd %>">
                            <SelectParameters>
                                <asp:QueryStringParameter Name="recordID" QueryStringField="recordID" Type="Int32" />
                            </SelectParameters>
                    </asp:SqlDataSource> 
                </ContentTemplate>
            </cc1:TabPanel>
        </cc1:TabContainer>
    </div>
    </form>
    
</body>
</html>
