<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Employees.ascx.cs" Inherits="MapCall.Controls.HR.Employees" %>
<%@ Register Src="~/Controls/HR/dropdownlists/ddlOpCode.ascx" TagName="ddlOpCode" TagPrefix="mmsi" %>

<br />
<asp:UpdatePanel ID="UpdatePanelEmployee1" runat="server">
    <ContentTemplate>
        <div style="font-size:0.9em;border:1px solid #284E98;">
        <table border=1 style="background-color:#B5C7DE;width:100%;border-bottom:1px solid #284E98;">
            <tr>
                <td style="font-weight:bold;width:90%;">
                    Employees
                </td>
                <td style="text-align:left;font-size:0.9em;" nowrap>
                    <asp:Button runat="server" ID="btnAddToggleEmployee" Text="Link Employee" 
                        OnClientClick="document.getElementById('tblEmployeeUnique').style.display='';return false;" />
                    <asp:Button runat="server" ID="btnNotify" OnClientClick="return confirm('Are you sure you would like to send the user list notification email? This may take a few moments. The button will notify you once it is sent.');"
                        OnClick="btnNotify_Click" />
                </td>
            </tr>
        </table>

        <table cellpadding="0" cellspacing="0" style="width:100%;"><tr><td>
        <asp:GridView Width="100%" SkinID="Note" ID="GridView1_Employee" runat="server" 
            AutoGenerateColumns="False" 
            DataKeyNames="EmployeeLinkID"
            DataSourceID="SqlDataSourceEmployee1" 
            OnRowDataBound="GridView1_RowDataBound"
            EmptyDataText="There are no employees for this record." AllowPaging="False" AllowSorting="True">
            <Columns>
                <asp:BoundField DataField="EmployeeID" HeaderText="EmployeeID" SortExpression="EmployeeID" />
                <asp:BoundField DataField="FullName" HeaderText="Full Name" SortExpression="FullName" />
                <asp:BoundField DataField="CreatedOn" HeaderText="Linked On" SortExpression="CreatedOn" />
                <asp:BoundField DataField="CreatedBy" HeaderText="Linked By" SortExpression="CreatedBy" />
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnDeleteEmployee" runat="server" CausesValidation="False" CommandName="Delete"
                            Text="Delete" OnClientClick="return confirm('Are you sure you want to remove this employee?');" ></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        </td></tr></table>
        
        <table id="tblEmployeeUnique" style="display:none;background-color:#B5C7DE;width:100%;margin-top:1px;font-size:0.9em;border-top:1px solid #284E98;">
            <tr>
                <td style="width:50%;">
                    <div class="inlineScroll">
                    <asp:CheckBoxList runat="server" ID="ddlEmployees" 
                        DataSourceID="dsEmployees" 
                        AppendDataBoundItems="true"
                        DataValueField="tblEmployeeID"
                        DataTextField="fullname"
                        Width="50%"
                        
                        />
                    </div>
                    
                    <asp:SqlDataSource runat="server" ID="dsEmployees"
                        ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>"
                        SelectCommand="SELECT distinct tblEmployeeID, replace(isNull(Last_Name,'') + ', ' + isNull(First_name, '') + ' ' + isNull(Middle_Name,'') + ' - '  + isNull(employeeID,''),'  ', ' ') + ' - ' + isNull(OperatingCenterCode, '')  as [FullName], Last_Name, OperatingCenterCode as OpCode from tblEmployee LEFT JOIN OperatingCenters on OperatingCenters.OperatingCenterId = tblEmployee.OperatingCenterId where inactive_date is null order by Last_Name"
                        >
                    </asp:SqlDataSource>
                    <mmsi:ddlOpCode runat="server" ID="ddlOpCode" />
                    <asp:Button runat="server" ID="btnFilterOpCode" Text="Filter" OnClick="btnFilterOpCode_Click" />
                   <asp:Button runat="server" ID="btnAddEmployee" Text="Link Employees" OnClick="btnAddEmployee_Click" />
                   <asp:Button runat="server" id="btnCancelEmployee" Text="Cancel" OnClientClick="document.getElementById('tblEmployeeUnique').style.display = 'none';return false;" />
                </td>
            </tr>
        </table>

        <asp:SqlDataSource ID="SqlDataSourceEmployee1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:MCProd %>" 
            ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>" 
            DeleteCommand="DELETE FROM [EmployeeLink] WHERE [EmployeeLinkID] = @EmployeeLinkID"
            InsertCommand="INSERT INTO [EmployeeLink] ([DataLinkID], [DataTypeID], [CreatedAt], [CreatedBy], [tblEmployeeID]) VALUES (@DataLinkID, @DataTypeID, @CreatedOn, @CreatedBy, @tblEmployeeID)"
            SelectCommand="
                SELECT [EmployeeLinkID], [CreatedOn], [CreatedBy], replace(isNull(First_Name,'') + ' ' + isNull(Middle_Name, '') + ' ' + isNull(Last_Name,''),'  ', ' ') as [FullName], EmployeeID
	                FROM [EmployeeLink]  
	                LEFT JOIN tblEmployee on tblEmployee.tblEmployeeID = EmployeeLink.tblEmployeeID
	                where [DataLinkID] = @DataLinkID and [DataTypeID] = @DataTypeID
	            "
            UpdateCommand="UPDATE [EmployeeLink] SET [DataLinkID] = @DataLinkID, [DataTypeID] = @DataTypeID, [CreatedAt] = @CreatedOn, [CreatedBy] = @CreatedBy, [tblEmployeeID] = @tblEmployeeID WHERE [EmployeeLinkID] = @EmployeeLinkID"
            OnDeleted="SqlDataSourceEmployee1_Deleted"
            >
            <InsertParameters>
                <asp:Parameter Name="DataLinkID" Type="Int32" />
                <asp:Parameter Name="DataTypeID" Type="Int32" />
                <asp:Parameter Name="CreatedOn" Type="DateTime" />
                <asp:Parameter Name="CreatedBy" Type="String" />
                <asp:Parameter Name="tblEmployeeID" Type="Int32" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="DataLinkID" Type="Int32" />
                <asp:Parameter Name="DataTypeID" Type="Int32" />
                <asp:Parameter Name="CreatedOn" Type="DateTime" />
                <asp:Parameter Name="CreatedBy" Type="String" />
                <asp:Parameter Name="tblEmployeeID" Type="Int32" />
                <asp:Parameter Name="EmployeeLinkID" Type="Int32" />
            </UpdateParameters>
            <DeleteParameters>
                <asp:Parameter Name="EmployeeLinkID" Type="Int32" />
            </DeleteParameters>
            <SelectParameters>
                <asp:Parameter Name="DataTypeID" Type="Int32" />
                <asp:Parameter Name="DataLinkID" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
    
    </div>
    
    </ContentTemplate>
</asp:UpdatePanel>


    



