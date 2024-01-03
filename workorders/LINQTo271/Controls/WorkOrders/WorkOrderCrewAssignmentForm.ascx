<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderCrewAssignmentForm.ascx.cs" Inherits="LINQTo271.Controls.WorkOrders.WorkOrderCrewAssignmentForm" %>
<%@ Register Assembly="MMSINC.Core" Namespace="MMSINC.Controls" TagPrefix="mmsinc" %>
<%@ Register Assembly="Mapcall.Common" Namespace="MapCall.Common.Controls" TagPrefix="mmsinc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>
<%@ Import Namespace="WorkOrders.Model" %>
<%@ Import namespace="MMSINC.Utilities" %>

<mmsinc:MvpGridView runat="server" ID="gvCrewAssignments" AutoGenerateColumns="false"
    AutoGenerateSelectButton="false" DataKeyNames="CrewAssignmentID" DataSourceID="odsCrewAssignments"
    ShowFooter="false" OnRowCommand="gvCrewAssignments_RowCommand" OnRowDataBound="gvCrewAssignments_RowDataBound"
    OnClientUpdate="return WorkOrderCrewAssignmentForm.lbUpdate_click(this)">
    <Columns>
        <asp:TemplateField>
            <HeaderTemplate>Est. TTC (hours)</HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("WorkOrder.WorkDescription.TimeToComplete") %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("WorkOrder.WorkDescription.TimeToComplete") %>' />
                <asp:HiddenField runat="server" ID="hidCrewAssignmentID" Value='<%# Bind("CrewAssignmentID") %>' />
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>Assigned On</HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("AssignedOn",CommonStringFormats.DATETIME_WITHOUT_SECONDS_WITH_EST_TIMEZONE)%>' />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("AssignedOn", CommonStringFormats.DATETIME_WITHOUT_SECONDS_WITH_EST_TIMEZONE) %>' />
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>Assigned For</HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("AssignedFor", CommonStringFormats.DATETIME_WITHOUT_SECONDS_WITH_EST_TIMEZONE) %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("AssignedFor", CommonStringFormats.DATETIME_WITHOUT_SECONDS_WITH_EST_TIMEZONE) %>' />
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>Assigned To</HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("Crew") %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("Crew") %>' />
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>Start Time</HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" ID="lblStartDate" Text='<%# Eval("DateStarted") %>' Visible="false" />
                <asp:Label runat="server" ID="lblStartNotApplicable" Visible="false" Text="n/a" />
            </ItemTemplate>
            <EditItemTemplate>
                <mmsinc:DateTimePicker runat="server" ID="txtStartDate" SelectedDate='<%# Bind("DateStarted") %>' ShowTimePicker="True" />
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>End Time</HeaderTemplate>
            <ItemTemplate>
                <asp:LinkButton runat="server" ID="lbEnd" CommandName="End" Text="End" CommandArgument='<%# Eval("CrewAssignmentID") %>'
                    OnClientClick="return WorkOrderCrewAssignmentForm.lbEndDate_click(this);" />
                <asp:Label runat="server" ID="lblEndDate" Text='<%# Eval("DateEnded") %>' Visible="false" />
                <asp:Label runat="server" ID="lblEndNotApplicable" Visible="false" Text="n/a" />
            </ItemTemplate>
            <EditItemTemplate>
                <mmsinc:DateTimePicker runat="server" ID="txtEndDate" SelectedDate='<%# Bind("DateEnded") %>' ShowTimePicker="True" />
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>Employees On Crew</HeaderTemplate>
            <ItemTemplate>
                <mmsinc:MvpTextBox runat="server" ID="txtEmployeesOnJob" Width="75px" />
                <asp:Label runat="server" ID="lblEmployeesOnJob" Visible="false" Text='<%# Eval("EmployeesOnJob") %>' />
                <asp:Label runat="server" ID="lblEmployeesNotApplicable" Visible="false" Text="n/a" />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox runat="server" ID="txtEmployeesOnJob" Text='<%# Bind("EmployeesOnJob") %>' />
            </EditItemTemplate>
        </asp:TemplateField>        
    </Columns>
</mmsinc:MvpGridView>

<mmsinc:MvpObjectDataSource runat="server" ID="odsCrewAssignments" TypeName="WorkOrders.Model.CrewAssignmentRepository"
    SelectMethod="GetAssignmentsByWorkOrder" UpdateMethod="Update">
    <SelectParameters>
        <asp:Parameter Name="WorkOrderID" Type="Int32" />
    </SelectParameters>
    <UpdateParameters>
        <asp:Parameter Name="CrewAssignmentID" Type="Int32" />
        <asp:Parameter Name="DateStarted" Type="DateTime" />
        <asp:Parameter Name="DateEnded" Type="DateTime" />
        <asp:Parameter Name="EmployeesOnJob" Type="Single" />
    </UpdateParameters>
</mmsinc:MvpObjectDataSource>
