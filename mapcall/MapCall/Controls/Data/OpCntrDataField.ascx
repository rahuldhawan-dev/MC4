<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OpCntrDataField.ascx.cs" Inherits="MapCall.Controls.Data.OpCntrDataField" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>
<%@ Register Assembly="MMSINC.Core.WebForms" Namespace="MMSINC.Controls" TagPrefix="mmsinc" %>

<tr>
    <td class="leftcol label">
        <asp:Literal runat="server" ID="lblHeaderText" />
    </td>
    <td class="field">
    
        <mmsinc:MvpDropDownList runat="server" ID="ddlOpCntr" 
            DataSourceID="dsDataField" DataTextField="txt" DataValueField="val" 
            AppendDataBoundItems="true" >
            <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
        </mmsinc:MvpDropDownList>
        <asp:RequiredFieldValidator runat="server" ID="ddlRequired" ControlToValidate="ddlOpCntr" 
            InitialValue="" Text="Required" Enabled="false" Visible="false" />
        <asp:SqlDataSource runat="server" ID="dsDataField" />
    </td>
</tr>

<tr runat="server" id="trTown" visible="false">
    <td class="label">Town : </td>
    <td class="field">
        <mmsinc:MvpDropDownList runat="server" ID="ddlTown" />
        <atk:CascadingDropDown runat="server" ID="cddTowns" 
            TargetControlID="ddlTown" ParentControlID="ddlOpCntr" 
            Category="Town" 
            EmptyText="None Found" EmptyValue=""
            PromptText="--Select Here--" PromptValue="" 
            LoadingText="[Loading Towns...]"
            ServicePath="~/Modules/Data/DropDowns.asmx" 
            ServiceMethod="GetTownsByOperatingCenter"
            SelectedValue='<%# Bind("TownID") %>' 
        />
    </td>
</tr>
