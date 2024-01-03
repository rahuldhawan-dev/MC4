<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MeterTestResults.ascx.cs" Inherits="MapCall.Controls.HR.MeterTestResults" %>
<%@ Register TagPrefix="dotnet" Namespace="dotnetCHARTING" Assembly="dotnetCHARTING" %>
<%@ Register assembly="MMSINC" namespace="MMSINC" tagPrefix="mmsinc" %>

<asp:UpdatePanel ID="UpdatePanelEmployee1" runat="server">
    <ContentTemplate>
    <div style="font-size:0.9em;border:1px solid #284E98;">
        <table border=0 cellpadding="0" cellspacing="2" style="background-color:#B5C7DE;width:100%;border-bottom:1px solid #284E98;">
            <tr>
                <td style="font-weight:bold;width:90%;">
                    Test Results
                </td>
                <td style="text-align:left;font-size:0.9em;" nowrap>
                    <asp:Button runat="server" ID="btnAddResult" Text="Add Test Result" 
                        OnClientClick="document.getElementById('tblNewTestResult').style.display='';return false;" />
                </td>
            </tr>
        </table>

        <table cellpadding="0" cellspacing="0" style="width:100%;"><tr><td>
            <asp:GridView Width="100%" SkinID="Note" ID="gvTestResults" runat="server" 
                AutoGenerateColumns="False" 
                DataKeyNames="MeterTestResultID"
                DataSourceID="dsMeterTestResults" 
                OnRowDataBound="GridView1_RowDataBound"
                OnDataBinding="gvTestResults_DataBinding"
                EmptyDataText="There are no test results for this test." AllowPaging="False" AllowSorting="True">
                <Columns>
                    <mmsinc:BoundField DataField="PointNumber" HeaderText="Point Number(GPM):" SortExpression="PointNumber" />
                    <mmsinc:BoundField DataField="SubjectMeterLowVolume" HeaderText="Subject Meter Low Volume:" SortExpression="SubjectMeterLowVolume" />
                    <mmsinc:BoundField DataField="SubjectMeterHighVolume" HeaderText="Subject Meter High Volume:" SortExpression="SubjectMeterHighVolume" />
                    <mmsinc:BoundField ReadOnly="true" DataField="SubjectMeterTotalVolume" HeaderText="Subject Meter Total Volume:" SortExpression="SubjectMeterTotalVolume" />
                    <mmsinc:BoundField DataField="TestMeterVolume" HeaderText="Test Meter Volume:" SortExpression="TestMeterVolume" />
                    <asp:BoundField DataField="TestMeterAccuracy" HeaderText="Test Meter Accuracy:" SortExpression="TestMeterAccuracy" />
                    <asp:BoundField DataField="SubjectMeterAccuracy" HeaderText="Subject Meter Accuracy:" SortExpression="SubjectMeterAccuracy" ReadOnly="true" />
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="true" CommandName="Edit" Text="Edit" />
                            <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                Text="Delete" OnClientClick="return confirm('Are you sure you want to remove this test result?');" ></asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton ID="btnUpdate" runat="server" CausesValidation="true" CommandName="Update" Text="Update" />
                            <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="false" CommandName="Cancel" Text="Cancel" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="dsMeterTestResults" runat="server" 
                ConnectionString="<%$ ConnectionStrings:MCProd %>" 
                SelectCommand="
                    SELECT *
	                    FROM [MeterTestResults]
	                where 
	                    [MeterTestID] = @MeterTestID
	                ORDER BY
	                    SubjectMeterTotalVolume, TestMeterVolume
	                "
                DeleteCommand="DELETE FROM [MeterTestResults] WHERE [MeterTestResultID] = @MeterTestResultID"
                UpdateCommand="
                DECLARE @MeterTestComparisonMeterID INT
                DECLARE @SubjectMeterTotalVolume float
                SELECT @SubjectMeterTotalVolume = isnull(@SubjectMeterHighVolume,0) + isNull(@SubjectMeterLowVolume, 0)
                SELECT @MeterTestComparisonMeterID = (SELECT MeterTestComparisonMeterID FROM MeterTests where MeterTestID = @MeterTestID)

                    SELECT @SubjectMeterAccuracy = 
		                (@SubjectMeterTotalVolume * @TestMeterAccuracy / @TestMeterVolume)
		                
		            UPDATE 
                        [MeterTestResults] 
                    SET 
                        [MeterTestID] = @MeterTestID, 
                        [PointNumber] = @PointNumber, 
                        [SubjectMeterLowVolume] = @SubjectMeterLowVolume, 
                        [SubjectMeterHighVolume] = @SubjectMeterHighVolume, 
                        [SubjectMeterTotalVolume] = @SubjectMeterTotalVolume, 
                        [TestMeterVolume] = @TestMeterVolume, 
                        [TestMeterAccuracy] = @TestMeterAccuracy, 
                        [SubjectMeterAccuracy] = @SubjectMeterAccuracy 
                    WHERE 
                        [MeterTestResultID] = @MeterTestResultID
                "
                OnDeleted="SqlDataSourceEmployee1_Deleted"
            >
                <UpdateParameters>
                    <asp:Parameter Name="MeterTestID" Type="Int32" />
                    <asp:Parameter Name="PointNumber" Type="Int32" />
                    <asp:Parameter Name="SubjectMeterLowVolume" Type="Double" />
                    <asp:Parameter Name="SubjectMeterHighVolume" Type="Double" />
                    <asp:Parameter Name="TestMeterVolume" Type="Double" />
                    <asp:FormParameter Name="TestMeterAccuracy" Type="Double" />
                    <asp:Parameter Name="SubjectMeterAccuracy" Type="Double" />
                    <asp:Parameter Name="MeterTestResultID" Type="Int32" />
                </UpdateParameters>
                <DeleteParameters>
                    <asp:Parameter Name="MeterTestResultID" Type="Int32" />
                </DeleteParameters>
                <SelectParameters>
                    <asp:Parameter Name="MeterTestID" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>
        </td></tr></table>        
        
        
        <table id="tblNewTestResult" style="display:none;background-color:#B5C7DE;width:100%;margin-top:1px;font-size:0.9em;border-top:1px solid #284E98;">
            <tr>
                <td style="width:50%;">
                    <asp:DetailsView runat="server" ID="dvMeterTestResult"
                        DataSourceID="dsMeterTestResult"
                        EmptyDataText="No data"
                        OnItemInserting="dsMeterTestResult_Inserting"
                        OnItemInserted="dsMeterTestResult_Inserted"
                        OnDataBinding="dvMeterTestResult_DataBinding"
                        AutoGenerateRows="false"
                    >
                        <Fields>
                            <asp:BoundField DataField="PointNumber" HeaderText="Point Number:" SortExpression="PointNumber" />
                            <mmsinc:BoundField DataField="SubjectMeterLowVolume" HeaderText="Subject Meter Low Volume:" SortExpression="SubjectMeterLowVolume" />
                            <asp:BoundField DataField="SubjectMeterHighVolume" HeaderText="Subject Meter High Volume:" SortExpression="SubjectMeterHighVolume" />
                            <asp:BoundField DataField="TestMeterVolume" HeaderText="Test Meter Volume:" SortExpression="TestMeterVolume" />
                            <asp:BoundField DataField="TestMeterAccuracy" HeaderText="Test Meter Accuracy:" SortExpression="TestMeterAccuracy" />
                            <asp:TemplateField HeaderText="Subject Meter Accuracy:">
                                <ItemTemplate><%# Eval("SubjectMeterAccuracy")%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowInsertButton="true" />
                        </Fields>
                    </asp:DetailsView>
                </td>
            </tr>
        </table>

        <asp:SqlDataSource ID="dsMeterTestResult" runat="server" 
            ConnectionString="<%$ ConnectionStrings:MCProd %>" 
            InsertCommand="
                DECLARE @MeterTestComparisonMeterID INT
                DECLARE @SubjectMeterTotalVolume float
                SELECT @SubjectMeterTotalVolume = isnull(@SubjectMeterHighVolume,0) + isNull(@SubjectMeterLowVolume, 0)
                SELECT @MeterTestComparisonMeterID = (SELECT MeterTestComparisonMeterID FROM MeterTests where MeterTestID = @MeterTestID)

                SELECT @SubjectMeterAccuracy = 
		                (@SubjectMeterTotalVolume * @TestMeterAccuracy / @TestMeterVolume)

                INSERT INTO [MeterTestResults]
                       ([MeterTestID],[PointNumber],[SubjectMeterLowVolume],[SubjectMeterHighVolume],[SubjectMeterTotalVolume],[TestMeterVolume],[TestMeterAccuracy],[SubjectMeterAccuracy])
                VALUES
                       (@MeterTestID,@PointNumber,@SubjectMeterLowVolume,@SubjectMeterHighVolume,@SubjectMeterTotalVolume,@TestMeterVolume,@TestMeterAccuracy,@SubjectMeterAccuracy)
                "
        >
            <InsertParameters>
                <asp:Parameter Name="MeterTestID" Type="Int32" />
                <asp:Parameter Name="PointNumber" Type="Int32" />
                <asp:Parameter Name="SubjectMeterLowVolume" Type="Double" />
                <asp:Parameter Name="SubjectMeterHighVolume" Type="Double" />
                <asp:Parameter Name="TestMeterVolume" Type="Double" />
                <asp:FormParameter Name="TestMeterAccuracy" Type="Double" />
                <asp:Parameter Name="SubjectMeterAccuracy" Type="Double" />
            </InsertParameters>
        </asp:SqlDataSource>
        
        <dotnet:Chart runat="server" ID="Chart" />    
        <dotnet:Chart runat="server" ID="Chart2" />    
        
        <asp:SqlDataSource runat="server" ID="dsMeterOutputs"
            ConnectionString="<%$ ConnectionStrings:MCProd %>"
            SelectCommand="
                select 
                    mo.outputs
                from 
                    MeterTests mt
                inner join
                    Meters m
                on
                    m.MeterID = mt.MeterID
                inner join
                    MeterProfiles mp
                on
                    mp.MeterProfileID = m.MeterProfileID
                inner join
                    MeterOutputs mo
                on
                    mo.MeterOutputID = mp.MeterOutputs
                where 
                    mt.MeterTestID = @MeterTestID
            ">
            <SelectParameters>
                <asp:Parameter Name="MeterTestID" Type="Int32" />
            </SelectParameters>    
        </asp:SqlDataSource>
    </div>
    </ContentTemplate>
</asp:UpdatePanel>