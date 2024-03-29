﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderMarkoutDamageForm.ascx.cs" Inherits="LINQTo271.Controls.WorkOrders.WorkOrderMarkoutDamageForm" %>

<asp:HyperLink runat="server" Text="Create Markout Damage" 
               NavigateUrl='<%# String.Format("../../../../Modules/mvc/FieldOperations/MarkoutDamage/New/{0}", 
                  Eval("WorkOrderId"), Eval("OperatingCenterID")) %>' />

<mmsinc:MvpGridView runat="server" ID="gvMarkoutDamages" DataSourceID="odsMarkoutDamages"
                    DataKeyNames="Id" ShowFooter="true" UseAccessibleHeader="true" ShowEmptyTable="true"
                    AutoGenerateColumns="false" AutoGenerateSelectButton="true" AutoGenerateDeleteButton="true"
                    OnClientDelete="return confirm('Are you sure you\'d like to delete this MarkoutDamage record?')"
                    OnClientSelect="return WorkOrderMarkoutDamageForm.gvMarkoutDamages_Select(this)">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <a href="/Modules/Mvc/FieldOperations/MarkoutDamage/Show/<%# Eval("Id") %>" target="_blank">View</a>
            </ItemTemplate>
        </asp:TemplateField>
        <mmsinc:DynamicBoundField ReadOnly="true" HeaderText="Request Number" DataField="RequestNumber"
                                  SortExpression="RequestNumber" />
        <mmsinc:DynamicBoundField ReadOnly="true" HeaderText="Damage On Date" DataField="DamageOn"
                                  SortExpression="DamageOn" />
        <mmsinc:DynamicBoundField ReadOnly="true" HeaderText="Excavator" DataField="Excavator"
                                  SortExpression="Excavator" />
        <mmsinc:DynamicBoundField ReadOnly="true" HeaderText="Utilites Damaged" DataField="UtilitiesDamaged"
                                  SortExpression="UtilitiesDamaged" />
        
    </Columns>
</mmsinc:MvpGridView>

<mmsinc:MvpObjectDataSource runat="server" ID="odsMarkoutDamages" TypeName="WorkOrders.Model.MarkoutDamageRepository"
                            SelectMethod="GetMarkoutDamagesByWorkOrder">
    <SelectParameters>
        <asp:Parameter DbType="Int32" Name="WorkOrderID" />
    </SelectParameters>
</mmsinc:MvpObjectDataSource>