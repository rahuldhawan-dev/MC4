<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContractorCrewAssignmentsListView.ascx.cs" Inherits="LINQTo271.Views.ContractorCrewAssignments.ContractorCrewAssignmentsListView" %>
<%@ Register Assembly="MMSINC.Core" Namespace="MMSINC.Controls" TagPrefix="mmsinc" %>

<%-- this div supports the two column layout used by the controls in this setup --%>
<div id="bottom-row">
    <div style="font-size: smaller">
        * Orders in green are completed</div>
        <mmsinc:MvpGridView runat="server" ID="gvCrewAssignments" AutoGenerateColumns="false">
            <EmptyDataTemplate>
                No data to display.
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Order Number</HeaderTemplate>
                    <ItemTemplate>
                        <a href="../WorkOrders/General/WorkOrderGeneralResourceRPCPage.aspx?cmd=view&arg=<%# Eval("WorkOrderID") %>">
                            <%# Eval("WorkOrderID") %>
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Priority</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("Priority") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Street Number</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Eval("WorkOrder.StreetNumber") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Street</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label4" runat="server" Text='<%# Eval("WorkOrder.Street") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Cross Street</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label5" runat="server" Text='<%# Eval("WorkOrder.NearestCrossStreet") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Town</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label6" runat="server" Text='<%# Eval("WorkOrder.Town") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Town Section</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label7" runat="server" Text='<%# Eval("WorkOrder.TownSection") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="WorkOrder.WorkDescription.Description" HeaderText="Description of Job<br/>(Hover for Notes)">
                    <ItemTemplate>
                        <asp:Label ID="Label8" runat="server" 
                            Title='<%# Eval("WorkOrder.Notes") ?? "No Notes Entered" %>'
                            Text='<%# Eval("WorkOrder.WorkDescription") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Est. TTC (hours)</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label9" runat="server" Text='<%# Eval("WorkOrder.WorkDescription.TimeToComplete") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>Priority</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label10" runat="server" Text='<%#Eval("WorkOrder.Priority.Description") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>Markout Expiration</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label11" runat="server" Text='<%#Eval("WorkOrder.CurrentMarkout.ExpirationDate", "{0:d}") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Assigned On</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label12" runat="server" Text='<%# Eval("AssignedOn", "{0:g}") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Start Time</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblStartDate" Visible="false" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        End Time</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblEndDate" Visible="false" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>Employees On Crew</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblEmployeesOnJob" Visible="false" Text='<%# Eval("EmployeesOnJob") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </mmsinc:MvpGridView>
        <br />
</div>