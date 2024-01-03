<%@ Page Title="Documents" Theme="bender" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="Documents.aspx.cs" Inherits="MapCall.Modules.HR.DocumentsSearch" %>
<%@ Register Src="~/Controls/Data/date.ascx" TagName="date" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/Data/number.ascx" TagName="number" TagPrefix="mmsi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Documents
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphInstructions" runat="server">
    Use this page to search and view documents in the system.
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <tr>
                <td style="text-align:right;"> 
                     Data Type : 
                </td>
                <td style="text-align:left;">
                    <asp:DropDownList runat="server" ID="ddlDataType">
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                        <asp:ListItem Value="21" Text="Contract"></asp:ListItem>
                        <asp:ListItem Value="22" Text="Contract Section"></asp:ListItem>
                        <asp:ListItem Value="23" Text="Contract Proposal"></asp:ListItem>
                        <asp:ListItem Value="24" Text="Employee"></asp:ListItem>
                        <asp:ListItem Value="20" Text="Grievance"></asp:ListItem>
                        <asp:ListItem Value="26" Text="Local"></asp:ListItem>
                        <asp:ListItem Value="27" Text="Position"></asp:ListItem>
                        <asp:ListItem Value="28" Text="Position History"></asp:ListItem>
                        <asp:ListItem Value="29" Text="Position Posting"></asp:ListItem>
                        <asp:ListItem Value="25" Text="Union"></asp:ListItem>
                    </asp:DropDownList>   
                                 
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                     Document Type : 
                </td>
                <td style="text-align:left;">
                    <asp:DropDownList runat="server" ID="ddlDocumentTypeID" 
                        DataSourceID="dsDocumentType" 
                        AppendDataBoundItems="true"
                        DataTextField="Document_Type"
                        DataValueField="DocumentTypeID"
                        >
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                    </asp:DropDownList>   
                    <asp:SqlDataSource runat="server" ID="dsDocumentType"
                        ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        SelectCommand="select * from documentType order by document_type"
                        >
                    </asp:SqlDataSource>                    
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    File Size (bytes):
                </td>
                <td>
                    <mmsi:number ID="txtFileSize" runat="server" SelectedIndex="5" />                   
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    File Name:
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtFile_Name"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    Created :
                </td>
                <td style="text-align:left;">
                    <uc2:date ID="txtCreatedOn" runat="server" SelectedIndex="5" />              
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    Order By: 
                </td>
                <td style="text-align:left;">
                    <asp:DropDownList runat="server" ID="ddlOrderBy">
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                        <asp:ListItem Text="Data Type" Value="data_Type"></asp:ListItem>
                        <asp:ListItem Text="Document Type" Value="document_Type"></asp:ListItem>
                    </asp:DropDownList>
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
        <br />
    </asp:Panel>
    
    <asp:Panel runat="server" ID="pnlResults" Visible="false">
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" DataKeyNames="documentID" DataSourceID="SqlDataSource1"
            EmptyDataText="There are no data records to display."
            OnSorting="GridView1_Sorting"
            OnPageIndexChanged="GridView1_PageIndexChanged"
            OnRowCommand="GridView1_RowCommand"
            PageSize="150"
            AlternatingRowStyle-CssClass="HRAlternatingRow"
            >
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:LinkButton CssClass="RightPadded" ID="btnDocumentCtrlView" runat="server" CausesValidation="False" 
                            CommandName="Select" Text="View" CommandArgument='<%# Eval("documentID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="documentID" HeaderText="DocumentID" InsertVisible="False" ReadOnly="True" SortExpression="documentID" />
                <asp:BoundField DataField="Data_Type" HeaderText="Data Type" SortExpression="Data_Type" />
                <asp:BoundField DataField="document_Type" HeaderText="Document Type" SortExpression="Document_Type" />
                <asp:BoundField DataField="File_Size" HeaderText="File_Size" SortExpression="File_Size" />
                <asp:BoundField DataField="File_Name" HeaderText="File_Name" SortExpression="File_Name" />
                <asp:BoundField DataField="CreatedAt" HeaderText="Created On" SortExpression="CreatedAt" />
            </Columns>
        </asp:GridView>
        <center>
            <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
        </center>
    </asp:Panel>
    
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:MCProd %>"
        ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>" 
        SelectCommand="
	          SELECT 
		        [Document].[documentID],
		        Data_Type,
		        [DocumentType].[document_Type],
		        [Document].[File_Name],
		        [Document].[File_Size],
		        [Document].[CreatedBy],
		        [Document].[CreatedAt],
		        DocumentLink.DataTypeID,
		        DocumentLink.DocumentTypeID
		        FROM DocumentLink
		        JOIN Document on DocumentLink.DocumentID = Document.DocumentID
		        LEFT JOIN DocumentType on DocumentType.DocumentTypeID = DocumentLink.DocumentTypeID
		        LEFT JOIN DataType on DataType.DataTypeID = DocumentLink.DataTypeID
		        ORDER BY Document.DocumentID
            "
        >
    </asp:SqlDataSource>
</asp:Content>
