<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderTrafficControlForm.ascx.cs" Inherits="LINQTo271.Controls.WorkOrders.WorkOrderTrafficControlForm" %>

<mmsinc:MvpFormView runat="server" ID="fvWorkOrder" DataSourceID="odsWorkOrder" DataKeyNames="WorkOrderID"
    OnItemUpdating="fvWorkOrder_ItemUpdating">
    <ItemTemplate>
        <table>
            <tr>
                <td>Is Traffic Control Required?</td>
                <td>
                    <asp:CheckBox runat="server" ID="chkTrafficControlRequired" Checked='<%# Eval("TrafficControlRequired") %>'
                        Enabled="false" />
                </td>
            </tr>
            <tr>
                <td>Number of Officers Required:</td>
                <td>
                    <asp:Label runat="server" ID="lblNumberOfOfficersRequired" Text='<%# Eval("NumberOfOfficersRequired") %>' />
                </td>
            </tr>
            <tr>
                <td>Current Notes:</td>
                <td>
                    <mmsinc:MvpLabel runat="server" ID="lblCurrentNotes" Text='<%# (Eval("Notes")!=null) ? ((string)Eval("Notes")).Replace("\n", "<br />") : String.Empty %>' />
                </td>
            </tr>
        </table>
    </ItemTemplate>
    <EditItemTemplate>
        <table>
            <tr>
                <td>Is Traffic Control Required?</td>
                <td>
                    <asp:CheckBox runat="server" ID="chkTrafficControlRequired" Checked='<%# Eval("TrafficControlRequired") %>'
                        Enabled="false" />
                </td>
            </tr>
            <tr>
                <td>Number of Officers Required:</td>
                <td>
                    <asp:TextBox runat="server" ID="txtNumberOfOfficersRequired" Text='<%# Bind("NumberOfOfficersRequired") %>' />
                </td>
            </tr>
            <tr>
                <td>Current Notes:</td>
                <td>
                    <mmsinc:MvpLabel runat="server" ID="lblCurrentNotes" Text='<%# (Eval("Notes")!=null) ? ((string)Eval("Notes")).Replace("\n", "<br />") : String.Empty %>' />
                </td>
            </tr>
            <tr>
                <td>Append Notes:</td>
                <td>
                    <mmsinc:MvpTextBox runat="server" ID="txtAppendNotes" TextMode="MultiLine" Width="278px" />
                </td>
            </tr>
        </table>
        
        <asp:Button runat="server" ID="btnSave" Text="Save" OnClick="btnSave_Click" />

    </EditItemTemplate>
</mmsinc:MvpFormView>

<mmsinc:MvpObjectContainerDataSource runat="server" ID="odsWorkOrder" 
    DataObjectTypeName="WorkOrders.Model.WorkOrder" OnUpdated="ods_Updated"
/>
