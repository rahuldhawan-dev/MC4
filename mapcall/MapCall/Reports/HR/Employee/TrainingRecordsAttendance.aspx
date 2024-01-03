<%@ Page Title="Training Record Attendance" theme="bender" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="TrainingRecordsAttendance.aspx.cs" Inherits="MapCall.Reports.HR.Employee.TrainingRecordsAttendance" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>
<%@ Register assembly="MMSINC" namespace="MMSINC" tagPrefix="mmsinc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Training Records - Attendance Report
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <mmsi:DataField runat="server" ID="DataField5" DataType="Date" DataFieldName="HeldOn" HeaderText="Held On: " />
            <mmsi:DataField runat="server" ID="DataField3" DataType="DropDownList" DataFieldName="PresentedBy"
                HeaderText="Presented By : " ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="SELECT distinct tblEmployeeID as val, replace(isNull(Last_Name,'') + ', ' + isNull(First_name, '') + ' ' + isNull(Middle_Name,'') + ' - '  + isNull(employeeID,''),'  ', ' ') as [txt], Last_Name from tblEmployee order by Last_Name" />
            <mmsi:DataField runat="server" ID="DataField2" DataType="DropDownList"
                HeaderText="Training Module Category : "
                DataFieldName="TrainingModuleCategory"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="
                    SELECT
                        LookupValue AS [Txt],
                        LookupID AS [Val]
                    FROM
                        [Lookup]
                    where
                        LookupType='TrainingModuleCategory'
                    order by LookupValue
                " />
                
            <mmsi:DataField runat="server" ID="DataField1" DataType="Integer" DataFieldName="TrainingModuleID" HeaderText="TrainingModuleID : " />
            
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
            DataKeyNames="TrainingRecordID"
            AutoGenerateColumns="false"
            OnRowDataBound="GridView1_RowDataBound"
            >
            <Columns>
                <asp:BoundField DataField="opCode" HeaderText="OpCode" SortExpression="opCode" />
                <mmsinc:BoundField DataField="HeldOn" SqlDataType="DateTime" SortExpression="HeldOn" HeaderText="Held On" DataFormatString="{0:d}" />
                <asp:BoundField DataField="TrainingModuleCategory" HeaderText="Training Module Category" SortExpression="TrainingModuleCategory" />
                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                <asp:BoundField DataField="trainingModuleID" HeaderText="trainingModuleID" SortExpression="trainingModuleID" />
                <asp:BoundField DataField="Instructor" HeaderText="Instructor" SortExpression="Instructor" />                
                
                <asp:TemplateField HeaderText="Attendees">
                    <ItemTemplate>
                        <asp:GridView runat="server" ID="gvAttendees" 
                            AutoGenerateColumns="false" ShowHeader="false" DataSourceID="dsAttendees">
                            <Columns>
                                <asp:BoundField DataField="Attendee" HeaderText="Attendee" />
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource runat="server" ID="dsAttendees" 
                            ConnectionString="<%$ ConnectionStrings:MCProd %>"
                            ProviderName="System.Data.SqlClient"
                            SelectCommand="
                                select 
                                    replace(isNull(e.First_Name,'') + ' ' + isNull(e.Middle_Name,'') + ' ' + isNull(e.Last_Name,''), '  ', ' ') as Attendee
                                from 
                                    tblemployee e
                                left join
                                     EmployeeLink el on e.tblEmployeeID = el.tblEmployeeID
                                where 
                                    datatypeID = 88
                                and 
                                    datalinkID = @trainingRecordID">
                            <SelectParameters>
                                <asp:Parameter Name="trainingRecordID" Type="String" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="trainingRecordID" HeaderText="trainingRecordID" SortExpression="trainingRecordID" />
                <asp:BoundField DataField="presentedBy" HeaderText="presentedBy" SortExpression="presentedBy" Visible="false" />

            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
            ProviderName="System.Data.SqlClient"
            SelectCommand="
                    select 
                        oc.OperatingCenterCode as OpCode, 
                        t.heldon, 
                        l.lookupvalue as TrainingModuleCategory, 
                        t.trainingRecordID,
                        t.trainingModuleID,
                        tt.TrainingModuleCategory,
						tt.description,
                        isnull(e1.Last_Name + ', ', '') + isnull(e1.First_Name + isnull(' ' + e1.Middle_Name, ''), '') as Instructor,
                        --isnull(e2.Last_Name + ', ', '') + isnull(e2.First_Name + isnull(' ' + e2.Middle_Name, ''), '') as SecondInstructor,
                        isnull(oc.OperatingCenterCode + ' - ', '') + isnull(cl.Description, '') as ClassLocation

                    from 
                        tblTrainingRecords t
                    left join 
                        tblTrainingModules tt on tt.TrainingModuleID = t.TrainingModuleID
                    left join
						opEratingCenters oc on oc.OperatingCenterID = tt.OpCode
                    left join 
                        lookup l on l.lookupid = tt.TrainingModuleCategory
                    LEFT JOIN
                        tblEmployee as e1 on e1.tblEmployeeId = t.InstructorId
                    --LEFT JOIN
                    --    tblEmployee as e2 on e2.tblEmployeeId = t.SecondInstructorId
                    LEFT JOIN
                        ClassLocations as cl on cl.Id = t.ClassLocationId
                ">
        </asp:SqlDataSource>

    </asp:Panel>

</asp:Content>
