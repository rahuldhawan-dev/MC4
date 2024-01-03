<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MeterTestComparisonPoints.ascx.cs" Inherits="MapCall.Controls.HR.MeterTestComparisonPoints" %>
MeterTestComparisonPoints
<asp:UpdatePanel ID="UpdatePanelEmployee1" runat="server">
    <ContentTemplate>
        <div style="font-size:0.9em;border:1px solid #284E98;">
        <table border=0 cellpadding="0" cellspacing="2" style="background-color:#B5C7DE;width:100%;border-bottom:1px solid #284E98;">
            <tr>
                <td style="font-weight:bold;width:90%;">
                    Test Results
                </td>
                <td style="text-align:left;font-size:0.9em;" nowrap>
                    <asp:Button runat="server" ID="btnAddResult" Text="Add Comparison Point" 
                        OnClientClick="document.getElementById('tblNewComparisonPoint').style.display='';return false;" />
                </td>
            </tr>
        </table>

        <table cellpadding="0" cellspacing="0" style="width:100%;"><tr><td>
        <asp:GridView Width="100%" SkinID="Note" ID="gvComparisonPoints" runat="server" 
            AutoGenerateColumns="False" 
            DataKeyNames="MeterTestComparisonPointID"
            DataSourceID="dsMeterTestComparisonPoints" 
            OnRowDataBound="GridView1_RowDataBound"
            OnDataBinding="gvComparisonPoints_DataBinding"
            EmptyDataText="There are no comparison points for this test meter." AllowPaging="False" AllowSorting="True">
            <Columns>
                <asp:BoundField DataField="PointNumber" HeaderText="Point Number:" SortExpression="PointNumber" />
                <asp:BoundField DataField="Volume" HeaderText="Volume:" SortExpression="Volume" />
                <asp:BoundField DataField="Accuracy" HeaderText="Accuracy:" SortExpression="Accuracy" />
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                            Text="Delete" OnClientClick="return confirm('Are you sure you want to remove this test result?');" ></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        </td></tr></table>
        
        <table id="tblNewComparisonPoint" style="display:none;background-color:#B5C7DE;width:100%;margin-top:1px;font-size:0.9em;border-top:1px solid #284E98;">
            <tr>
                <td style="width:50%;">
                    <!-- detailview -->
                    <asp:DetailsView runat="server" ID="dvMeterTestComparisonPoint"
                        DataSourceID="dsMeterTestComparisonPoints"
                        EmptyDataText="No data"
                        OnItemInserting="dsMeterTestComparisonPoints_Inserting"
                        OnItemInserted="dsMeterTestComparisonPoints_Inserted"
                        AutoGenerateRows="false"
                    >
                        <Fields>
                            <asp:BoundField DataField="PointNumber" HeaderText="Point Number:" SortExpression="PointNumber" />
                            <asp:BoundField DataField="Volume" HeaderText="Volume:" SortExpression="Volume" />
                            <asp:BoundField DataField="Accuracy" HeaderText="Accuracy:" SortExpression="Accuracy" />
                            <asp:CommandField ShowInsertButton="true" />
                        </Fields>
                    </asp:DetailsView>
                </td>
            </tr>
        </table>

        <asp:SqlDataSource ID="dsMeterTestComparisonPoints" runat="server" 
            ConnectionString="<%$ ConnectionStrings:MCProd %>" 
            DeleteCommand="DELETE FROM [MeterTestComparisonPoints] WHERE [MeterTestComparisonPointID] = @MeterTestComparisonPointID"
            InsertCommand="
                INSERT INTO [MeterTestComparisonPoints]
                       ([MeterTestComparisonMeterID],[PointNumber],[Volume],[Accuracy])
                VALUES
                       (@MeterTestComparisonMeterID,@PointNumber,@Volume,@Accuracy)
                "
            SelectCommand="
                SELECT *
	                FROM [MeterTestComparisonPoints]
	            where 
	                [MeterTestComparisonMeterID] = @MeterTestComparisonMeterID
	            ORDER BY
	                PointNumber
	            "
            OnDeleted="SqlDataSource_Deleted"
            >
            <InsertParameters>
                <asp:Parameter Name="MeterTestComparisonMeterID" Type="Int32" />
                <asp:Parameter Name="PointNumber" Type="Int32" />
                <asp:Parameter Name="Volume" Type="Double" />
                <asp:Parameter Name="Accuracy" Type="Double" />
            </InsertParameters>
            <DeleteParameters>
                <asp:Parameter Name="MeterTestComparisonPointID" Type="Int32" />
            </DeleteParameters>
            <SelectParameters>
                <asp:Parameter Name="MeterTestComparisonMeterID" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    </ContentTemplate>
</asp:UpdatePanel>