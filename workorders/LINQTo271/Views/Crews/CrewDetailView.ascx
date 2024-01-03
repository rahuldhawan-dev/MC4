<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CrewDetailView.ascx.cs" Inherits="LINQTo271.Views.Crews.CrewDetailView" %>

<%-- IGNORE ANY ERROR MESSAGES ABOUT THE NEXT LINE --%>
<mmsinc:MvpDetailsView runat="server" ID="dvCrew" DataSourceID="odsCrew" DataKeyNames="CrewID"
    OnItemInserting="dvCrew_ItemInserting" OnItemUpdating="dvCrew_ItemUpdating" AutoGenerateRows="false" CellPadding="0" CellSpacing="0">
    <HeaderTemplate>
        <asp:Label runat="server" Text="New Crew" Font-Bold="true" Font-Size="Larger" />
    </HeaderTemplate>
    <Fields>
        <asp:TemplateField HeaderText="Crew Name">
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("Description") %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <mmsinc:MvpTextBox runat="server" ID="txtCrewName" Text='<%# Bind("Description") %>' MaxLength="15" />
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:CheckBoxField DataField="Active" HeaderText="Active?" />
        <asp:BoundField DataField="Availability" HeaderText="Availability (hours)" />
        <asp:TemplateField HeaderText="Assigned Work">
            <InsertItemTemplate>
                &nbsp;
            </InsertItemTemplate>
            <EditItemTemplate>
                &nbsp;
            </EditItemTemplate>
            <ItemTemplate>
                <asp:HiddenField runat="server" ID="hidCrewID" Value='<%# Eval("CrewID") %>' />
                <mmsinc:MvpGridView runat="server" ID="gvAssignedWorkOrders" DataSourceID="odsAssignedWorkOrders"
                    AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField DataField="WorkOrderID" HeaderText="Order Number" SortExpression="WorkOrderID" />
                        <asp:BoundField DataField="DateReceived" HeaderText="Date Received" SortExpression="DateReceived" DataFormatString="{0:g}" />
                        <asp:BoundField DataField="StreetNumber" HeaderText="Street Number" SortExpression="StreetNumber" />
                        <asp:BoundField DataField="Street" HeaderText="Street" SortExpression="Street" />
                        <asp:BoundField DataField="NearestCrossStreet" HeaderText="Nearest Cross Street" SortExpression="NearestCrossStreet" />
                        <asp:BoundField DataField="Town" HeaderText="Town" SortExpression="Town" />
                        <asp:BoundField DataField="TownSection" HeaderText="Town Section" SortExpression="TownSection" />
                        <asp:TemplateField SortExpression="WorkDescription.Description" HeaderText="Description of Job<br/>(Hover for Notes)">        
                            <ItemTemplate>
                                <asp:Label runat="server" Title='<%# Eval("Notes") ?? "No Notes Entered" %>' Text='<%# Eval("WorkDescription") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="MarkoutRequirement" HeaderText="Markout Requirement" SortExpression="MarkoutRequirement" />                        
                    </Columns>
                </mmsinc:MvpGridView>
                <asp:ObjectDataSource runat="server" ID="odsAssignedWorkOrders" TypeName="WorkOrders.Model.WorkOrderRepository"
                    SelectMethod="GetWorkOrdersByCrewID" DataObjectTypeName="WorkOrders.Model.WorkOrder">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hidCrewID" ConvertEmptyStringToNull="true" DbType="Int32" Name="crewID" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Operating Center">            
            <InsertItemTemplate>                                                
                <asp:DropDownList ID="ddlOperatingCenter" runat="server" DataSourceID="odsOperatingCenters"
                    DataTextField="FullDescription" 
                    DataValueField="OperatingCenterID"
                    SelectedValue='<%# Bind("OperatingCenterID") %>'
                    AppendDataBoundItems="true" >
                    <asp:ListItem Text="--Select Here--" Value="" />
                </asp:DropDownList>
            </InsertItemTemplate>
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("OperatingCenter.FullDescription") %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Fields>
</mmsinc:MvpDetailsView><br />

<asp:Button runat="server" ID="btnEdit" Text="Edit" OnClick="btnEdit_Click" />
<asp:Button runat="server" ID="btnSave" Text="Save" OnClick="btnSave_Click" />
<asp:Button runat="server" ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" />

<mmsinc:MvpObjectContainerDataSource runat="server" ID="odsCrew" DataObjectTypeName="WorkOrders.Model.Crew"
    OnInserted="ods_Inserted" OnUpdated="ods_Updated" OnDeleted="ods_Deleted" />
    
<asp:ObjectDataSource runat="server" ID="odsOperatingCenters" TypeName="WorkOrders.Library.Permissions.SecurityService" 
    SelectMethod="SelectUserOperatingCenters" />