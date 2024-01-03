<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MapCallSite.Master" Theme="bender" CodeBehind="CustomerSurvey.aspx.cs" Inherits="MapCall.Modules.Customer.CustomerSurvey" %>

<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphHeader">
CustomerSurvey
</asp:Content>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphInstructions">
Use this page to search for Customer Survey Data
</asp:Content>

<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cphMain">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
            <table style="text-align:left;" border="0">
                <mmsi:DataField runat="server" ID="dfOpCode" DataType="DropDownList" 
                    DataFieldName="OperatingCenterID" HeaderText="OpCode : " 
                    ConnectionString="<%$ ConnectionStrings:MCProd %>" 
                    SelectCommand="SELECT DISTINCT OperatingCenterCode + ' - ' + OperatingCenterName AS [Txt], [OperatingCenterID] AS [Val] FROM [OperatingCenters];" />
                <mmsi:DataField runat="server" ID="dfServiceCity" DataType="DropDownList" 
                    DataFieldName="ServiceCity" HeaderText="Service City : " 
                    ConnectionString="<%$ ConnectionStrings:MCProd %>" 
                    SelectCommand="SELECT DISTINCT UPPER(ServiceCity) AS [Txt], ServiceCity AS [Val] FROM [CustomerSurveys] ORDER BY 2;" />
                <mmsi:DataField runat="server" ID="dfOverallServiceQuality" 
                    HeaderText="Overall Service Quality" DataType="DropDownList"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    DataFieldName="OverallServiceQuality"
                    SelectCommand="select distinct OverallServiceQuality as Txt, OverallServiceQuality as Val from CustomerSurveys order by 1" />
                <mmsi:DataField runat="server" ID="dfFSRServiceQuality" 
                    HeaderText="FSR Service Quality" DataType="DropDownList"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    DataFieldName="FSRServiceQuality"
                    SelectCommand="select distinct FSRServiceQuality as Txt, FSRServiceQuality as Val from CustomerSurveys order by 1" />
                <tr>
                    <td></td>
                    <td>
                        <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                        <asp:Button runat="server" ID="btnReset" Text="Reset" OnClick="btnReset_Click" />
                        <asp:Button runat="server" ID="btnAdd" Text="Add" CausesValidation="False" OnClick="btnAdd_Click" />
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>

    <asp:Panel runat="server" ID="pnlResults" Visible="false">
        <asp:HiddenField runat="server" ID="hidFilter" />
        <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Export" />
        <%--<asp:Button runat="server" ID="btnMap" Visible="False" PostBackUrl="~/Modules/Maps/Maps.aspx" Text="Map" />--%>
        <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
        <asp:Label runat="server" ID="lblRecordCount" />
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
            DataKeyNames="CustomerSurveyID" AllowSorting="true" AllowPaging="true" PageSize="500">
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>" 
            SelectCommand="
SELECT 
	[CustomerSurveyID],
	[OperatingCenterCode],
	[RespondentID], 
	[SurveyDate], 
	[ReportingMonth], 
	[Quarter], 
	[CustomerNumber], 
	[ServiceOrderNumber], 
	[CustomerName], 
	[PhoneNumber], 
	cs.[Address], 
	[CityState], 
	[ServiceAddress], 
	[ServiceCity], 
	[ServiceZip], 
	cs.[OperatingCenterID], 
	[SurveySubState], 
	cs.[CoordinateID], 
	[ContactReason], 
	[ContactDescription], 
	[OverallServiceQuality], 
	[FSRServiceQuality]
FROM 
	CustomerSurveys cs
LEFT JOIN
	[OperatingCenters] oc
ON
	oc.OperatingCenterID = cs.OperatingCenterID
            " />
    </asp:Panel>

    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <div id="divContent" class="container">
            <ul class="ui-tabs-nav">
                <li><a href="#customerSurvey" class="tab"><span>Customer Survey</span></a></li>
                <li><a href="#surveydata" class="tab"><span>Survey Data</span></a></li>
                <li><a href="#notes" class="tab"><span>Notes</span></a></li>
                <li><a href="#documents" class="tab"><span>Documents</span></a></li>
            </ul>
            <div id="customerSurvey" class="ui-tabs-panel">
                <mmsi:DataElement runat="server" ID="DataElement1" OnItemInserted="DataElement1_ItemInserted" 
                    DataElementName="CustomerSurveys"
                    DataElementParameterName="CustomerSurveyID"
                    DataElementTableName="CustomerSurveys"
                    ConnectionString="MCProd" ShowKey="true"
                />
            </div>
            <div id="surveydata" class="ui-tabs-panel">  
                <asp:GridView runat="server" ID="gvSurveyData"
                    DataSourceID="dsSurveyData" EmptyDataText="No survey information available.">
                </asp:GridView>
                <asp:SqlDataSource runat="server" ID="dsSurveyData"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    SelectCommand="
SELECT 
	Q.Question,
	A.CustomerAnswer
FROM 
	CustomerSurveyAnswers A
INNER JOIN
	CustomerSurveyQuestions Q
ON
	Q.CustomerSurveyQuestionID = A.CustomerSurveyQuestionID
WHERE 
	CustomerSurveyID = @CustomerSurveyID
                    ">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="DataElement1$DetailsView1" PropertyName="SelectedValue" Name="CustomerSurveyID" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>            
            <div id="notes" class="ui-tabs-panel">
                <mmsi:Notes ID="Notes1" runat="server" DataTypeID="76" />
            </div>
            <div id="documents" class="ui-tabs-panel">
                <mmsi:Documents ID="Documents1" runat="server" DataTypeID="76" />
            </div>
        </div>
        <div class="buttonContainer">
            <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </div>
    </asp:Panel>
    
    <script type="text/javascript" src="CustomerSurvey.js"></script>  

</asp:Content>