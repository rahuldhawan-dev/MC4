<%@ Page theme="bender" Title="Tailgate Talks - Attendance Report" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="TailgateTalksAttendance.aspx.cs" Inherits="MapCall.Reports.FieldServices.TailgateTalksAttendance" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>
<%@ Register assembly="MMSINC" namespace="MMSINC" tagPrefix="mmsinc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Tailgate Talks - Attendance Report
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">

    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <mmsi:DataField runat="server" ID="DataField5" DataType="Date" DataFieldName="HeldOn" HeaderText="Held On: " />
            <mmsi:DataField runat="server" DataType="DropDownList" DataFieldName="OpCode"
                HeaderText="Operating Center : " ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="SELECT OperatingCenterCode as val, OperatingCenterCode as [txt] from OperatingCenters" />
            <mmsi:DataField runat="server" ID="DataField3" DataType="DropDownList" DataFieldName="PresentedBy"
                HeaderText="Presented By : " ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="SELECT distinct tblEmployeeID as val, replace(isNull(Last_Name,'') + ', ' + isNull(First_name, '') + ' ' + isNull(Middle_Name,'') + ' - '  + isNull(employeeID,''),'  ', ' ') as [txt], Last_Name from tblEmployee order by Last_Name" />
            <mmsi:DataField runat="server" ID="DataField2" DataType="DropDownList"
                HeaderText="Tailgate Topic Category : "
                DataFieldName="TailgateCategory"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="
                    SELECT
                        LookupValue AS [Txt],
                        LookupID AS [Val]
                    FROM
                        [Lookup]
                    where
                        LookupType='TailgateCategory'
                    order by LookupValue
                " />
            <mmsi:DataField runat="server" ID="dfTopicCategory" DataType="DropDownList"
                    HeaderText="Tailgate Topic : "
                    DataFieldName="TailgateTopicID"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    SelectCommand="
                        SELECT                                          
                            Topic AS [Txt],
                            TailgateTopicID AS [Val]
                        FROM
                            tblTailgateTopics
                        ORDER BY 1                          
                    " 
                />
                
            <mmsi:DataField runat="server" ID="DataField1" DataType="Integer" DataFieldName="TailgateTopicID" HeaderText="Tailgate Topic ID : " />
            
            <tr>
                <td></td>
                <td>
                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                    <asp:Button runat="server" ID="btnReset" Text="Reset" OnClick="btnReset_Click" />
                </td>
            </tr>
        </table>
        </center>
        <br />
    </asp:Panel>

    <asp:Panel runat="server" ID="pnlResults" Visible="false">
        <asp:HiddenField runat="server" ID="hidFilter" />
        <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Export" />
        <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
        <asp:Label runat="server" ID="lblRecordCount"></asp:Label>
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" 
            DataKeyNames="TailgateTalkID"
            AutoGenerateColumns="false"
            OnRowDataBound="GridView1_RowDataBound"
            >
            <Columns>
                <asp:BoundField DataField="opCode" HeaderText="OpCode" SortExpression="opCode" />
                <mmsinc:BoundField DataField="HeldOn" SqlDataType="DateTime" SortExpression="HeldOn" HeaderText="Held On" DataFormatString="{0:d}" />
                <asp:BoundField DataField="TopicCategory" HeaderText="Topic Category" SortExpression="TopicCategory" />
                <asp:BoundField DataField="Topic" HeaderText="Topic" SortExpression="Topic" />
                <asp:BoundField DataField="TopicDescription" HeaderText="Topic Description" SortExpression="TopicDescription" />
                <asp:BoundField DataField="Presenter" HeaderText="Presenter" SortExpression="Presenter" />                
                <asp:TemplateField HeaderText="Attendees">
                    <ItemTemplate>
                        <asp:GridView runat="server" ID="gvAttendees" 
                            AutoGenerateColumns="false" ShowHeader="false" DataSourceID="dsAttendees">
                            <Columns>
                                <asp:BoundField DataField="Attendee" HeaderText="Attendee" />
                                <asp:BoundField DataField="EmployeeID" HeaderText="EmployeeID" />
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource runat="server" ID="dsAttendees" 
                            ConnectionString="<%$ ConnectionStrings:MCProd %>"
                            ProviderName="System.Data.SqlClient"
                            SelectCommand="
                                select 
                                    replace(isNull(e.First_Name,'') + ' ' + isNull(e.Middle_Name,'') + ' ' + isNull(e.Last_Name,''), '  ', ' ') as Attendee, 
                                    employeeID
                                from 
                                    tblemployee e
                                left join
                                     EmployeeLink el on e.tblEmployeeID = el.tblEmployeeID
                                where 
                                    datatypeID = 81
                                and 
                                    datalinkID = @TailgateTalkID
                            "
                        >
                            <SelectParameters>
                                <asp:Parameter Name="TailgateTalkID" Type="String" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="tailgatetalkID" HeaderText="TailgateTalkID" SortExpression="TailgateTalkID" />
                <asp:BoundField DataField="presentedBy" HeaderText="presentedBy" SortExpression="presentedBy" Visible="false" />
                <asp:BoundField DataField="TailgateTopicID" HeaderText="TailgateTopicID" SortExpression="TailgateTopicID" Visible="false" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
            ProviderName="System.Data.SqlClient"
            SelectCommand="
                    select 
                        oc.OperatingCenterCode as opCode, 
                        t.heldon, 
                        l.lookupvalue as TopicCategory, 
                        tt.topic, 
                        replace(isNull(e.First_Name,'') + ' ' + isNull(e.Middle_Name,'') + ' ' + isNull(e.Last_Name,''), '  ', ' ') as Presenter,
                        t.tailgateTalkID,
                        t.topicDescription,
                        presentedBy, 
                        t.tailgatetopicID,
                        tt.tailgatecategory
                    from 
                        tblTailgateTalks t
                    left join 
                        tblEmployee e on e.tblEmployeeID = t.presentedBy
                    left join 
                        tblTailgateTopics tt on tt.tailgatetopicID = t.tailgatetopicID
                    left join 
                        lookup l on l.lookupid = tt.tailgateCategory
                    left join
                        OperatingCenters oc on oc.OperatingCenterId = e.OperatingCenterId
                    order by e.Last_Name, t.heldon
                ">
        </asp:SqlDataSource>

    </asp:Panel>

</asp:Content>
