<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="iSampleSite.aspx.cs" Inherits="MapCall.Modules.Maps.iSampleSite" Theme="bender"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server" id="head">
    <title>Untitled Page</title>
    
    <script src="<%#ResolveClientUrl("~/resources/scripts/jquery.js")%>" type="text/javascript"></script>
    <style type="text/css">
        body {font-family:arial;font-size:12px;margin:0px;padding:0px;text-align:left;}
        .leftCol {vertical-align:top;text-align:right;font-weight:bold;border-bottom:1px solid black;width:150px;}
        .rightCol {vertical-align:top;text-align:left;font-weight:normal;border-bottom:1px solid black}
        .pnlImageLink {position:absolute;top:0px;right:0px;cursor:pointer;color:navy;text-decoration:underline;padding-right:9px;}	
    </style>
</head>
<body style="background-color:White;background-image:none;">
    <form id="form1" runat="server">
    <div style="background-color:White;text-align:left;font-size:smaller;">
        <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" >
            <cc1:TabPanel runat="server" HeaderText="Sample Site" ID="tabSampleSite">
                <ContentTemplate>
                    <asp:ScriptManager runat="server" ID="sm1"></asp:ScriptManager>
                    <a href='<%= ResolveClientUrl("~/Modules/mvc/WaterQuality/SampleSite/Show/" + Request.QueryString["RecordID"]) %>' target="_blank">View</a>
                    <mmsi:DataElement runat="server" ID="DataElement1" OnItemInserted="DataElement1_ItemInserted" 
                        DataElementName = "SampleSite"
                        DataElementParameterName = "Id"
                        DataElementTableName = "SampleSites"
                        AllowDelete="false" AllowEdit="false" AllowNew="false"
                    />
                    <mmsi:Notes ID="Notes1" runat="server" DataTypeID="37" />
                    <mmsi:Documents ID="Documents1" runat="server" DataTypeID="37" />
                </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel runat="server" HeaderText="Constituents" ID="TabPanel2">
                <ContentTemplate>
                    <asp:UpdatePanel runat="server" ID="popConstituents"><ContentTemplate>
                    <asp:GridView AllowSorting="true" runat="server" ID="gvConstituents" AutoGenerateColumns="False" DataSourceID="dsConstituents">
                        <Columns>
                            <asp:BoundField DataField="Sample_ID" HeaderText="SampleID" SortExpression="Sample_ID" />
                            <asp:BoundField DataField="ProcessStage" HeaderText="Process Stage" SortExpression="ProcessStage" />
                            <asp:BoundField DataField="ProcessStageSequence" HeaderText="Process Stage Sequence" SortExpression="ProcessStageSequence" />
                            <asp:BoundField DataField="WaterConstituent" HeaderText="Water Constituent" SortExpression="WaterConstituent" />
                            <asp:BoundField DataField="Parameter" HeaderText="Parameter" SortExpression="Parameter" />
                            <asp:BoundField DataField="ParameterSequence" HeaderText="Parameter Sequence" SortExpression="ParameterSequence" />
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="dsConstituents" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:MCProd %>" 
                        SelectCommand="
                            select 
                                sim.Id as Sample_ID, 
                                sim.ProcessStage, 
                                sim.ProcessStageSequence, 
                                wc.WaterConstituent, 
                                sim.Parameter,
                                sim.ParameterSequence
                            from 
                                SampleIdMatrices sim, WaterConstituents wc
                            where 
                                sim.WaterConstituentId = wc.Id
                            and 
                                sim.SampleSiteID = @sampleSiteID">
                        <SelectParameters>
                            <asp:QueryStringParameter DefaultValue="1" Name="sampleSiteID" QueryStringField="recordID" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    </ContentTemplate></asp:UpdatePanel>
                </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel runat="server" HeaderText="Sample Results Bacti" ID="tpSampleResultsBACTI">
                <ContentTemplate>
                    <asp:GridView runat="server" AllowSorting="true" ID="gvSampleResultsBACTI" DataSourceID="dsSampleResultsBACTI">
                    </asp:GridView>
                    <asp:SqlDataSource ID="dsSampleResultsBACTI" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>" 
                        SelectCommand="
                            SELECT a.[Id], l1.[Description] AS [Bacti_Sample_TYPE], a.[Sample_Date], a.[Collected_By],
                                a.[Analysis_Performed_By], a.[SampleSiteID], a.[Sample_ID], a.[Cl2_Free], a.[Cl2_Total], a.[pH], a.[Temp_Celsius],
                                a.[Value_Fe], a.[Value_Mn], a.[Value_Turb], a.[OrthophosphateAsP], a.[Value_Conductivity], a.[ColiformSetupDTM],
                                a.[ColiformReadDTM], a.[Non_Sheen_Colony_Count], l2.[LookupValue] AS [Non_Sheen_Colony_Count_Operator],
                                a.[Sheen_Colony_Count], l3.[LookupValue] as [Sheen_Colony_Count_Operator],
                                a.[Coliform_Confirm], a.[E_Coli_Confirm], a.[Notes], 
                                SampleIDMatrices.WaterconstituentId,
                                WaterConstituents.WaterConstituent, 
                                SampleIDMatrices.Parameter, OC.OperatingCenterCode as opCode,
                                ss.town, ss.Id, BactiSite
                            FROM [BacterialWaterSamples] as a
	                            LEFT JOIN SampleSites SS ON a.samplesiteid = SS.Id
								LEFT JOIN OperatingCenters OC on OC.OperatingCenterID = SS.OperatingCenterId
	                            LEFT JOIN SampleIDMatrices ON SampleIDMatrices.Id = a.sample_id
	                            LEFT JOIN WaterConstituents ON SampleIDMatrices.waterConstituentId = WaterConstituents.Id
	                            LEFT JOIN BacterialSampleTypes as l1 on l1.Id = a.[BacterialSampleTypeId]
	                            LEFT JOIN Lookup as l2 ON l2.LookupID = a.[Non_Sheen_Colony_Count_Operator]
	                            LEFT JOIN Lookup as l3 on l3.LookupID = a.[Sheen_Colony_Count_Operator]
                            WHERE a.[SampleSiteID] = @SampleSiteID
                            ORDER BY a.[Sample_Date] DESC">
                        <SelectParameters>
                            <asp:QueryStringParameter DefaultValue="1" Name="SampleSiteID" QueryStringField="recordID" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </ContentTemplate>
            </cc1:TabPanel>
        </cc1:TabContainer>
    
    </div>
    </form>
</body>
</html>
