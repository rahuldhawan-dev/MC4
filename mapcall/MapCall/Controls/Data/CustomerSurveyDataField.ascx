<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerSurveyDataField.ascx.cs" Inherits="MapCall.Controls.Data.CustomerSurveyDataField1" %>
<tr>
    <td class="label">
        <asp:Label runat="server" ID="lblHeaderText"></asp:Label>
    </td>
    <td class="field">
        <asp:TextBox runat="server" ID="txtDataField" Visible="false" />
        
        <asp:DropDownList runat="server" ID="ddlDataField" Visible="false" 
            DataSourceID="dsDataField" DataTextField="txt" DataValueField="val" 
            AppendDataBoundItems="true" 
            >
            <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
        </asp:DropDownList>
        <asp:SqlDataSource runat="server" ID="dsDataField"
            SelectCommand="Select 0 as txt, 0 as val"    
            >
        </asp:SqlDataSource>
        
    </td>
</tr>