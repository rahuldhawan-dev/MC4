<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Schedule.aspx.cs" Inherits="MapCall.Modules.HR.Employee.Schedule" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body style="font-family:Arial;font-size:12px;margin:0px;padding:0px;">
    <form id="form1" runat="server">
    <div>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="ScheduleTypeDetailsID" DataSourceID="SqlDataSource1"
            EmptyDataText="This schedule does not exist." CellPadding="4" ForeColor="#333333" GridLines="Both" Width="100%">
            <Columns>
                <asp:BoundField DataField="wd" HeaderText="Day" SortExpression="wd" />
                <asp:BoundField Visible="False" DataField="Day" HeaderText="Day" SortExpression="Day" />
                <asp:BoundField DataField="StartTime" HeaderText="StartTime" SortExpression="StartTime" />
                <asp:BoundField DataField="EndTime" HeaderText="EndTime" SortExpression="EndTime" />
            </Columns>
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <EditRowStyle BackColor="#999999" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:MCProd%>"  ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>" 
            DeleteCommand="DELETE FROM [ScheduleTypeDetails] WHERE [ScheduleTypeDetailsID] = @ScheduleTypeDetailsID"
            InsertCommand="INSERT INTO [ScheduleTypeDetails] ([ScheduleTypeID], [Day], [StartTime], [EndTime]) VALUES (@ScheduleTypeID, @Day, @StartTime, @EndTime)"
            SelectCommand="SELECT ScheduleTypeDetailsID,DateName(weekday, [Day]) as wd, [Day], [StartTime], [EndTime] FROM [ScheduleTypeDetails] Where ScheduleTypeID = @ScheduleTypeID"
            UpdateCommand="UPDATE [ScheduleTypeDetails] SET [ScheduleTypeID] = @ScheduleTypeID, [Day] = @Day, [StartTime] = @StartTime, [EndTime] = @EndTime WHERE [ScheduleTypeDetailsID] = @ScheduleTypeDetailsID"
            >
            <SelectParameters>
                <asp:Parameter Name="ScheduleTypeID" Type="int32" DefaultValue="1" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="ScheduleTypeID" Type="Int32" />
                <asp:Parameter Name="Day" Type="Int32" />
                <asp:Parameter Name="StartTime" Type="String" />
                <asp:Parameter Name="EndTime" Type="String" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="ScheduleTypeID" Type="Int32" />
                <asp:Parameter Name="Day" Type="Int32" />
                <asp:Parameter Name="StartTime" Type="String" />
                <asp:Parameter Name="EndTime" Type="String" />
                <asp:Parameter Name="ScheduleTypeDetailsID" Type="Int32" />
            </UpdateParameters>
            <DeleteParameters>
                <asp:Parameter Name="ScheduleTypeDetailsID" Type="Int32" />
            </DeleteParameters>
        </asp:SqlDataSource>
        
    </div>
    </form>
</body>
</html>
