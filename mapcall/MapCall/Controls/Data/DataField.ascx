<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataField.ascx.cs" Inherits="MapCall.Controls.Data.DataField" %>
<%@ Register Src="number.ascx" TagName="number" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/DateTimeRange.ascx" TagName="DateTimeRange" TagPrefix="mapcall" %>
<%--
TODO: Add in Validators
TODO: Use SqlDBType instead of custom enum
--%>
<tr>
    <td class="leftcol label">
        <asp:Literal runat="server" ID="lblHeaderText" />
    </td>
    <td class="rightcol field">

        <asp:TextBox  runat="server" ID="txtDataField" Visible="false" EnableViewState="false" />
        
        <asp:CheckBox runat="server" ID="chkDataField" Visible="false" EnableViewState="false" />

        <mapcall:DateTimeRange ID="dtDataField" runat="server" SelectedOperatorType="Between" Visible="false" EnableViewState="false" />

        <uc2:number ID="numDataField" runat="server" SelectedIndex="5" Visible="false"  EnableViewState="false" />        

        <asp:DropDownList runat="server" ID="ddlDataField" Visible="false" EnableViewState="false"
            DataSourceID="dsDataField" DataTextField="txt" DataValueField="val" 
            AppendDataBoundItems="true" 
            >
            <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
        </asp:DropDownList>
        <asp:RequiredFieldValidator runat="server" ID="ddlRequired" ControlToValidate="ddlDataField" InitialValue="" Text="Required" Enabled="false" Visible="false" />

        <asp:DropDownList runat="server" ID="ddlBoolean" Visible="false" EnableViewState="false"
            >
            <asp:ListItem Value="" Text="--Select Here--" />
            <asp:ListItem Value="1" Text="Yes" />
            <asp:ListItem Value="0" Text="No" />
        </asp:DropDownList>
        
        <asp:ListBox runat="server" ID="lbDataField" Visible="false" EnableViewState="false"
            DataSourceID="DsDataField" DataTextField="txt" DataValueField="val"
            AppendDataBoundItems="true" SelectionMode="Multiple">
            <asp:ListItem Value="" Text="--Select Here--"></asp:ListItem>
        </asp:ListBox>
                
        <asp:SqlDataSource runat="server" ID="dsDataField" EnableViewState="false"
            SelectCommand="Select 0 as txt, 0 as val"    
            >
        </asp:SqlDataSource>
        
        <asp:CompareValidator runat="server" ID="cvDataFieldDouble" Type="Double" 
            Enabled="false" Visible="false"
            ControlToValidate="txtDataField" Text="Please enter a number" />
        <asp:CompareValidator runat="server" ID="cvDataFieldInteger" Type="Double" 
            Enabled="false" Visible="false"
            ControlToValidate="txtDataField" Text="Please enter a number" />           
    </td>
</tr>
