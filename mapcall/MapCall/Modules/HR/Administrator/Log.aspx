<%@ Page Language="C#" MasterPageFile="~/MapCallSite.Master" Theme="bender" AutoEventWireup="true" CodeBehind="Log.aspx.cs" Inherits="MapCall.Modules.HR.Administrator.Log" Title="Log" %>
<%@ Register Src="~/Controls/Data/date.ascx" TagName="date" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Log
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphInstructions" runat="server">
    Use this page to search the log to see what updates have been made to the system.
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            
            <tr>
                <td style="text-align:right;">
                    Category : 
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlCategory" 
                        DataSourceID="dsCategory" 
                        AppendDataBoundItems="true"
                        DataTextField="Category"
                        DataValueField="AuditCategoryID"
                        >
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                    </asp:DropDownList>                    
                    
                    <asp:SqlDataSource runat="server" ID="dsCategory"
                        ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        SelectCommand="SELECT * from AuditCategory order by 1"
                        >
                    </asp:SqlDataSource>
                </td>
            </tr>

            <tr>
                <td style="text-align:right;">
                    Date :
                </td>
                <td>
                    <uc2:date ID="txtDate" runat="server" SelectedIndex="5" />
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    Order By: 
                </td>
                <td style="text-align:left;">
                    <asp:DropDownList runat="server" ID="ddlOrderBy">
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                        <asp:ListItem Text="Date Created" Value="CreatedOn"></asp:ListItem>
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    User:
                </td>
                <td style="text-align:left;">
                    <asp:DropDownList runat="server" ID="ddlUser"
                        DataSourceID="dsUsers"
                        DataTextField="UserFullName"
                        DataValueField="username"
                        AppendDataBoundItems="true"
                        >     
                        <asp:ListItem Value="" Text="--Select Here--"></asp:ListItem>               
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="dsUsers" runat="server"
                        ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>" 
                        SelectCommand="
                            select 
                                aspnet_users.userName,
                                aspnet_Profile.userID, 
                                substring(propertyValuesString, 
		                                cast(substring(
						                                PropertyNames, 
						                                charindex('FullName:S:', PropertyNames)+11, 
						                                charIndex(':', substring(PropertyNames, charindex('FullName:S:', PropertyNames)+11, 100))-1
						                                )  as int)+1,
		                                cast(left(substring(PropertyNames, charindex('FullName:S:', PropertyNames)+11+charIndex(':', substring(PropertyNames, charindex('FullName:S:', PropertyNames)+11, 100)), 100), charindex(':', substring(PropertyNames, charindex('FullName:S:', PropertyNames)+11+charIndex(':', substring(PropertyNames, charindex('FullName:S:', PropertyNames)+11, 100)), 100))-1) as int)
                                ) as [UserFullName]
                                from aspnet_Profile 
                                left join aspnet_users on aspnet_users.userID = aspnet_Profile.userID
                                where aspnet_Profile.userID in (
		                                Select aspnet_Users.UserID from aspnet_Users 
			                                where applicationID in (select applicationID from aspnet_applications where applicationname = '/'))
			                                and charIndex('FullName', propertyNames) &gt; 0
			                    ORDER BY 3,1
		                "
                    />                
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                    <asp:Button runat="server" ID="btnReset" Text="Reset" OnClick="btnReset_Click" />
                </td>
            </tr>
        </table>
        </center>
    </asp:Panel>
    
    <asp:Panel runat="server" ID="pnlResults" Visible="false">
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" DataKeyNames="AuditID" DataSourceID="SqlDataSource1"
            EmptyDataText="There are no data records to display."
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
            OnSorting="GridView1_Sorting"
            OnPageIndexChanged="GridView1_PageIndexChanged"
            PageSize="150"
            AlternatingRowStyle-CssClass="HRAlternatingRow"
            Width="100%"
            >
            <Columns>
                <asp:BoundField DataField="CreatedOn" HeaderText="Created On" SortExpression="CreatedOn" />
                <asp:BoundField DataField="UserFullName" HeaderText="User Name" SortExpression="UserFullName" />
                <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category" />
                <asp:BoundField HeaderStyle-Width="50%" DataField="Details" HeaderText="Details" SortExpression="Details" />
            </Columns>
        </asp:GridView>
        <center>
            <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
        </center>
    </asp:Panel>
    
    <asp:Panel runat="server" ID="pnlDetail" Visible="false">

        <br />
        <center>
            <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
        
    </asp:Panel>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:MCProd %>"
        ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>" 
        SelectCommand="
            select *, 
		        (select top 1 
			        substring(propertyValuesString, 
					        cast(substring(PropertyNames, charindex('FullName:S:', PropertyNames)+11, charIndex(':', substring(PropertyNames, charindex('FullName:S:', PropertyNames)+11, 100))-1)  as int)+1,
					        cast(left(substring(PropertyNames, charindex('FullName:S:', PropertyNames)+11+charIndex(':', substring(PropertyNames, charindex('FullName:S:', PropertyNames)+11, 100)), 100), charindex(':', substring(PropertyNames, charindex('FullName:S:', PropertyNames)+11+charIndex(':', substring(PropertyNames, charindex('FullName:S:', PropertyNames)+11, 100)), 100))-1) as int)
			        )
						        from aspnet_Profile left join aspnet_users on aspnet_users.userID = aspnet_Profile.userID
						        where applicationID = (select applicationID from aspnet_applications where applicationname = '/') and aspnet_users.userName = createdBy
			        ) as [UserFullName]
			    from Audit
                left join AuditCategory on AuditCategory.AuditCategoryID = Audit.AuditCategoryID
        "
        >
    </asp:SqlDataSource> 
</asp:Content>
