<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" CodeBehind="ContactsView.ascx.cs"
    Inherits="MapCall.Controls.Contacts.ContactsView" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls" TagPrefix="mapcall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.Data" TagPrefix="mapcall" %>
<%@ Register Src="~/Controls/Contacts/ContactsAutoComplete.ascx" TagName="AutoComplete" TagPrefix="contacts" %>
<%@ Register Assembly="MMSINC.Core.WebForms" Namespace="MMSINC.Controls" TagPrefix="mmsi" %>

<mapcall:WebServiceInclude ID="WebServiceInclude1" runat="server" Service="Contacts" />
<div>

    <asp:UpdatePanel ID="upPan" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <mmsi:MvpPanel ID="pnlAddContactFor" runat="server" CssClass="container subContainer">
                <div class="header">
                    Link Existing Contact
                </div>
                <div class="body">
                     <div class="container">
                            <table>
                                <tr>
                                    <td class="label">Contact</td>
                                    <td class="field">
                                        <div style="float:left;">
                                            <contacts:AutoComplete ID="cacFindContact" runat="server" IsRequired="true" />
                                        </div>
                                        <asp:Button ID="btnViewFullContactInfo" runat="server"
                                            OnClick="btnViewFullContactInfoOnClick" 
                                            CssClass="viewInfo" style="float:left; margin-left:15px;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label">Contact Type</td>
                                    <td class="field">
                                        <asp:DropDownList ID="ddlContactType" runat="server" 
                                            ValidationGroup="AddContactsFor" 
                                            AppendDataBoundItems="true"
                                            DataSourceID="mcProdContactTypesFor" 
                                            DataTextField="Name" 
                                            DataValueField="ContactTypeID">
                                            <asp:ListItem Value="">-- Select Contact Type --</asp:ListItem>
                                        </asp:DropDownList>
                                        <mapcall:McProdDataSource runat="server" 
                                            ID="mcProdContactTypesFor"
                                            SelectCommand="SELECT
                                                                contactTypes.ContactTypeID,
                                                                contactTypes.Name
                                                            FROM
                                                                [{ContactTypesForTableName}] contactTypesFor
                                                            LEFT JOIN
                                                                [ContactTypes] contactTypes 
                                                            ON
                                                                contactTypes.ContactTypeID = contactTypesFor.ContactTypeID" />
                                        <asp:RequiredFieldValidator ID="rfvContactTypesFor" runat="server" 
                                            InitialValue="" 
                                            ControlToValidate="ddlContactType" ValidationGroup="AddContactsFor" ErrorMessage="Required" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                      <asp:Label ID="lblAddContactForError" runat="server" CssClass="error" />
                        
                            <asp:Button ID="btnSaveContactFor" runat="server" 
                                Text="Save Contact"
                                OnClick="btnSaveContactForOnClick" 
                                CausesValidation="true" ValidationGroup="AddContactsFor" UseSubmitBehavior="false" />
                            <asp:Button ID="btnCancelContactFor" runat="server" Text="Cancel" OnClick="btnCancelContactForOnClick" CausesValidation="false"  UseSubmitBehavior="false" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="container">
                          
                        </div>
     
                </div>
            </mmsi:MvpPanel>
        
            <mmsi:MvpPanel ID="pnlResults" runat="server" CssClass="container subContainer">
                <div class="header">
                    <div style="float:left">
                    Contacts For <%= ContactsForDisplayName %>
                    </div>
                    <div style="float:right;">
                        <asp:Button ID="btnLinkContact" runat="server" Text="Link Contact" OnClick="btnLinkContactOnClick" UseSubmitBehavior="true" CausesValidation="false" />
                        <a href="<%= ResolveUrl("~/modules/mvc/contact/new") %>" class="linkButton">Create New Contact</a>
                    </div>
                </div>
                <div class="body">
                
                    <mmsi:MvpGridView ID="gvContacts" runat="server" DataSourceID="dsContactResults" DataKeyNames="ContactID"
                        AutoGenerateColumns="false"  EnableViewState="false" OnSelectedIndexChanged="gvContactsOnSelectedIndexChanged">
                        <Columns>
                            <mapcall:TemplateBoundField>
                                <HeaderStyle CssClass="controls" />
                                <ItemTemplate>
                                    <asp:Button runat="server"
                                         Text="Remove" 
                                         CssClass="warn" 
                                         CommandName="Delete"
                                         OnClientClick="return ContactsView.confirmDelete();" />
                                </ItemTemplate>
                            </mapcall:TemplateBoundField>
                            
                            
                            <mapcall:TemplateBoundField HeaderText="Name">
                                <ItemTemplate>
                                    <a href="<%# ResolveUrl("~/modules/mvc/contact/show/") + Eval("ContactId") %>">
                                    <%# FormatContactName(Eval("FirstName").ToString(), 
                                                          Eval("MiddleInitial").ToString(),
                                                          Eval("LastName").ToString()) %>
                                    </a>
                                </ItemTemplate>
                            </mapcall:TemplateBoundField>
                            
                            <mapcall:BoundField DataField="Name" HeaderText="Contact Type" />
                           
                            <mapcall:TemplateBoundField HeaderText="Address">
                                <ItemTemplate>
                                    <%# FormatAddress(Eval("Address1").ToString(), 
                                                      Eval("Address2").ToString(),
                                                      Eval("Town").ToString(),
                                                      Eval("State").ToString(),
                                                      Eval("ZipCode").ToString()) %>
                                </ItemTemplate>
                            </mapcall:TemplateBoundField>
                            
                            <mapcall:BoundField DataField="BusinessPhone" HeaderText="Business #" />
                            <mapcall:BoundField DataField="Mobile" HeaderText="Mobile #" />
                            <mapcall:BoundField DataField="Fax" HeaderText="Fax" />
                        </Columns>
                    </mmsi:MvpGridView>
                    <mapcall:McProdDataSource ID="dsContactResults" runat="server" EnableViewState="false"
                        ConnectionString="<%$ ConnectionStrings:MCProd %>" 
                        DeleteCommand="DELETE FROM [{ContactsForTableName}] WHERE [{ContactsForTablePrimaryKeyName}] = @{ContactsForTablePrimaryKeyName}"
                        SelectCommand="SELECT 
                                                contactsFor.*,
                                                contactTypes.*,
                                                contacts.*,
                                                addy.*,
                                                town.Town,
                                                state.Abbreviation as State
                                            FROM
                                                [{ContactsForTableName}] contactsFor
                                            INNER JOIN
                                                [Contacts] contacts
                                            ON
                                                contacts.ContactID = contactsFor.ContactID
                                            INNER JOIN
                                                [ContactTypes] contactTypes
                                            ON
                                                contactTypes.ContactTypeID = contactsFor.ContactTypeID
                                            LEFT JOIN [Addresses] addy ON addy.Id = contacts.AddressId
                                            LEFT JOIN [Towns] town on town.TownId = addy.TownId
                                            LEFT JOIN [Counties] county on county.CountyId = town.CountyId
                                            LEFT JOIN
                                                [States] state
                                            ON
                                                state.StateID = county.StateID
                                            WHERE
                                                contactsFor.[{ContactsForTableKeyName}] = @ContactsForTableKeyValue
                                            ORDER BY
                                                contacts.LastName ASC">
                        <SelectParameters>
                            <asp:Parameter Name="ContactsForTableKeyValue" DbType="Int32" />
                        </SelectParameters>
                        <DeleteParameters>
                            <%--GridView performs some voodoo magic that adds the needed parameter here.--%>
                        </DeleteParameters>
                      </mapcall:McProdDataSource>
                </div>
            </mmsi:MvpPanel>
            
            <%-- This is for adding new information to the CONTACT table.  --%>
            
        </ContentTemplate>
    </asp:UpdatePanel>

    <%-- Needs to be on the bottom due to z-order. --%>
    <mapcall:UpdateProgress runat="server" 
        AssociatedUpdatePanelID="upPan" 
        DisplayAfter="250" Visible="false"
        CssClass="contactProgressPanel" />
</div>