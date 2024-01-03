<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SystemDelivery.ascx.cs" Inherits="MapCall.Controls.HR.SystemDelivery" %>
<asp:DetailsView 
    ID="DetailsView1" runat="server" 
    AutoGenerateRows="False" 
    DataKeyNames="SysDelID" 
    DataSourceID="SqlDataSource1"
    OnDataBound="DetailsView1_DataBound"
    Width="100%" FieldHeaderStyle-Width="100px"
        >
    <Fields>
        <asp:BoundField DataField="SysDelID" HeaderText="SysDelID" InsertVisible="False" ReadOnly="True" SortExpression="SysDelID" />
        <asp:BoundField DataField="OpCode" HeaderText="OpCode" SortExpression="OpCode" />
        <asp:BoundField DataField="Budget_Year" HeaderText="Budget_Year" SortExpression="Budget_Year" />
        <asp:TemplateField ControlStyle-Width="50%" HeaderText="Budget_Actual" SortExpression="Budget_Actual" >
            <ItemTemplate><asp:Label runat="server" ID="lblBudget_Actual_Text" Text='<%# Bind("Budget_Actual_Text") %>' /></ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList runat="server" ID="ddlBudget_Actual" 
                    DataSourceID="dsBudget_Actual" 
                    AppendDataBoundItems="true"
                    DataTextField="LookupValue"
                    DataValueField="LookupID"
                    SelectedValue='<%# Bind("Budget_Actual") %>'
                    >
                    <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                </asp:DropDownList>                    
                
                <asp:SqlDataSource runat="server" ID="dsBudget_Actual"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    SelectCommand="select LookupID, LookupValue from Lookup where Lookuptype = 'Budget_Category' order by 1"
                    >
                </asp:SqlDataSource>
            </EditItemTemplate>
        </asp:TemplateField>
        
        <asp:BoundField ControlStyle-Width="50%" DataField="Region" HeaderText="Region" SortExpression="Region" />
        <asp:BoundField ControlStyle-Width="50%" DataField="PWSID" HeaderText="PWSID" SortExpression="PWSID" />
        <asp:BoundField ControlStyle-Width="50%" DataField="PWSID_Name" HeaderText="PWSID_Name" SortExpression="PWSID_Name" />
        <asp:TemplateField ControlStyle-Width="50%" HeaderText="Orcom_District" SortExpression="Orcom_District" >
            <ItemTemplate><asp:Label runat="server" ID="lblOrcom_District" Text='<%# Bind("Orcom_District_Text") %>' /></ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList runat="server" ID="ddlOrcom_District" 
                    DataSourceID="dsOrcom_District" 
                    AppendDataBoundItems="true"
                    DataTextField="LookupValue"
                    DataValueField="LookupID"
                    SelectedValue='<%# Bind("Orcom_District") %>'
                    >
                    <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                </asp:DropDownList>                    
                
                <asp:SqlDataSource runat="server" ID="dsOrcom_District"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    SelectCommand="select LookupID, LookupValue from Lookup where Lookuptype = 'Orcom_District' order by 1"
                    >
                </asp:SqlDataSource>
            </EditItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField ControlStyle-Width="50%" HeaderText="System_Delivery_Category" SortExpression="System_Delivery_Category" >
            <ItemTemplate><asp:Label runat="server" ID="lblSystem_Delivery_Category" Text='<%# Bind("System_Delivery_Category_Text") %>' /></ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList runat="server" ID="ddlSystem_Delivery_Category" 
                    DataSourceID="dsSystem_Delivery_Category" 
                    AppendDataBoundItems="true"
                    DataTextField="LookupValue"
                    DataValueField="LookupID"
                    SelectedValue='<%# Bind("System_Delivery_Category") %>'
                    >
                    <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                </asp:DropDownList>                    
                
                <asp:SqlDataSource runat="server" ID="dsSystem_Delivery_Category"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    SelectCommand="select LookupID, LookupValue from Lookup where Lookuptype = 'System_Delivery_Category' order by 1"
                    >
                </asp:SqlDataSource>
            </EditItemTemplate>
        </asp:TemplateField>
        
        <asp:BoundField DataField="Jan" HeaderText="Jan" SortExpression="Jan" />
        <asp:BoundField DataField="Feb" HeaderText="Feb" SortExpression="Feb" />
        <asp:BoundField DataField="Mar" HeaderText="Mar" SortExpression="Mar" />
        <asp:BoundField DataField="Apr" HeaderText="Apr" SortExpression="Apr" />
        <asp:BoundField DataField="May" HeaderText="May" SortExpression="May" />
        <asp:BoundField DataField="Jun" HeaderText="Jun" SortExpression="Jun" />
        <asp:BoundField DataField="Jul" HeaderText="Jul" SortExpression="Jul" />
        <asp:BoundField DataField="Aug" HeaderText="Aug" SortExpression="Aug" />
        <asp:BoundField DataField="Sep" HeaderText="Sep" SortExpression="Sep" />
        <asp:BoundField DataField="Oct" HeaderText="Oct" SortExpression="Oct" />
        <asp:BoundField DataField="Nov" HeaderText="Nov" SortExpression="Nov" />
        <asp:BoundField DataField="Dec" HeaderText="Dec" SortExpression="Dec" />
        <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" />
        
        <asp:TemplateField ShowHeader="False">
            <EditItemTemplate>
                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton>
                <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
            </EditItemTemplate>
            <InsertItemTemplate>
                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Insert" Text="Insert"></asp:LinkButton>
                <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
            </InsertItemTemplate>
            <ItemTemplate>
                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit"></asp:LinkButton>
                <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete"
                    OnClientClick="return confirm('Are you sure you want to delete this record?');"
                ></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Fields>
</asp:DetailsView>
<asp:Label runat="server" ID="lblResults" ForeColor="Green"></asp:Label>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>" 
    ConnectionString="<%$ ConnectionStrings:MCProd %>" 
    DeleteCommand="DELETE FROM [tblAccounting_Budget_SysDel_PWSID] WHERE [SysDelID] = @SysDelID"
    InsertCommand="
        INSERT INTO [tblAccounting_Budget_SysDel_PWSID] ([OpCode], [Budget_Year], [Budget_Actual], [Region], [PWSID], [PWSID_Name], [System_Delivery_Category], [Jan], [Feb], [Mar], [Apr], [May], [Jun], [Jul], [Aug], [Sep], [Oct], [Nov], [Dec], [Orcom_District]) 
            VALUES (@OpCode, @Budget_Year, @Budget_Actual, @Region, @PWSID, @PWSID_Name, @System_Delivery_Category, @Jan, @Feb, @Mar, @Apr, @May, @Jun, @Jul, @Aug, @Sep, @Oct, @Nov, @Dec, @Orcom_District)
        ;Select @SysDelID = @@IDENTITY;
        "
    SelectCommand="
        SELECT [SysDelID], [OpCode], [Budget_Year], [Budget_Actual], [Region], [PWSID], [PWSID_Name], [System_Delivery_Category], [Jan], [Feb], [Mar], [Apr], [May], [Jun], [Jul], [Aug], [Sep], [Oct], [Nov], [Dec], ORCoM_District, jan+feb+mar+apr+may+jun+jul+aug+sep+oct+nov+dec as Total 
            , #1.LookupValue as [Budget_Actual_Text]
            , #2.LookupValue as [OrCom_District_Text]
            , #3.LookupValue as [System_Delivery_Category_Text]
            FROM [tblAccounting_Budget_SysDel_PWSID]
            LEFT JOIN Lookup #1 on #1.LookupID = Budget_Actual
            LEFT JOIN Lookup #2 on #2.LookupID = OrCom_district
            LEFT JOIN Lookup #3 on #3.LookupID = System_Delivery_Category
            WHERE [SysDelID] = @SysDelID
            "
    UpdateCommand="UPDATE [tblAccounting_Budget_SysDel_PWSID] SET [OpCode] = @OpCode, [Budget_Year] = @Budget_Year, [Budget_Actual] = @Budget_Actual, [Region] = @Region, [PWSID] = @PWSID, [PWSID_Name] = @PWSID_Name, [System_Delivery_Category] = @System_Delivery_Category, [Jan] = @Jan, [Feb] = @Feb, [Mar] = @Mar, [Apr] = @Apr, [May] = @May, [Jun] = @Jun, [Jul] = @Jul, [Aug] = @Aug, [Sep] = @Sep, [Oct] = @Oct, [Nov] = @Nov, [Dec] = @Dec, [Orcom_District] = @Orcom_District WHERE [SysDelID] = @SysDelID"
    OnInserted="SqlDataSource1_Inserted"
    OnDeleted="SqlDataSource1_Deleted"
    OnUpdated="SqlDataSource1_Updated"
    >
    <InsertParameters>
        <asp:Parameter Name="OpCode" Type="String" />
        <asp:Parameter Name="Budget_Year" Type="Int32" />
        <asp:Parameter Name="Budget_Actual" Type="String" />
        <asp:Parameter Name="Region" Type="String" />
        <asp:Parameter Name="PWSID" Type="String" />
        <asp:Parameter Name="PWSID_Name" Type="String" />
        <asp:Parameter Name="System_Delivery_Category" Type="String" />
        <asp:Parameter Name="Jan" Type="Decimal" />
        <asp:Parameter Name="Feb" Type="Decimal" />
        <asp:Parameter Name="Mar" Type="Decimal" />
        <asp:Parameter Name="Apr" Type="Decimal" />
        <asp:Parameter Name="May" Type="Decimal" />
        <asp:Parameter Name="Jun" Type="Decimal" />
        <asp:Parameter Name="Jul" Type="Decimal" />
        <asp:Parameter Name="Aug" Type="Decimal" />
        <asp:Parameter Name="Sep" Type="Decimal" />
        <asp:Parameter Name="Oct" Type="Decimal" />
        <asp:Parameter Name="Nov" Type="Decimal" />
        <asp:Parameter Name="Dec" Type="Decimal" />
        <asp:Parameter Name="Orcom_District" Type="Int32" />
        <asp:Parameter Name="SysDelID" Type="Int32" Direction="Output" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="OpCode" Type="String" />
        <asp:Parameter Name="Budget_Year" Type="Int32" />
        <asp:Parameter Name="Budget_Actual" Type="String" />
        <asp:Parameter Name="Region" Type="String" />
        <asp:Parameter Name="PWSID" Type="String" />
        <asp:Parameter Name="PWSID_Name" Type="String" />
        <asp:Parameter Name="System_Delivery_Category" Type="String" />
        <asp:Parameter Name="Jan" Type="Decimal" />
        <asp:Parameter Name="Feb" Type="Decimal" />
        <asp:Parameter Name="Mar" Type="Decimal" />
        <asp:Parameter Name="Apr" Type="Decimal" />
        <asp:Parameter Name="May" Type="Decimal" />
        <asp:Parameter Name="Jun" Type="Decimal" />
        <asp:Parameter Name="Jul" Type="Decimal" />
        <asp:Parameter Name="Aug" Type="Decimal" />
        <asp:Parameter Name="Sep" Type="Decimal" />
        <asp:Parameter Name="Oct" Type="Decimal" />
        <asp:Parameter Name="Nov" Type="Decimal" />
        <asp:Parameter Name="Dec" Type="Decimal" />
        <asp:Parameter Name="Orcom_District" Type="Int32" />
        <asp:Parameter Name="SysDelID" Type="Int32" />
    </UpdateParameters>
    <SelectParameters>
        <asp:Parameter Name="SysDelID" Type="Int32" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="SysDelID" Type="Int32" />
    </DeleteParameters>
</asp:SqlDataSource>
