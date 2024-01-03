<%@ Page Title="Email Notification Configuration" Language="C#" AutoEventWireup="true" MasterPageFile="~/MapCallSite.Master" Theme="bender" CodeBehind="EmailNotificationConfiguration.aspx.cs" Inherits="MapCall.Modules.HR.Administrator.EmailNotificationConfiguration" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls" TagPrefix="mapcall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.DropDowns" TagPrefix="mapcall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.SearchFields" TagPrefix="search" %>
<%@ Register Src="~/Controls/DetailsViewDataPageTemplate.ascx" TagPrefix="mapcall" TagName="DetailsViewDataPageTemplate" %>

<asp:Content runat="server" ContentPlaceHolderID="cphHeader"> 
    Email Notification Configuration
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphMain">
    <mapcall:DetailsViewDataPageTemplate ID="template" runat="server"
        DataElementTableName="NotificationConfigurations"
        DataElementPrimaryFieldName="NotificationConfigurationID"
        Label="Notification Configuration">
        <SearchBox runat="server">
            <Fields>
                <search:TemplatedSearchField Label="Operating Center" DataFieldName="o.OperatingCenterID"
                    BindingControlID="searchOperatingCenter" BindingDataType="Int32"
                    BindingPropertyName="SelectedValue">
                    <Template>
                        <mapcall:OperatingCenterDropDownList runat="server" ID="searchOperatingCenter"/>
                    </Template>
                </search:TemplatedSearchField>
                <search:TemplatedSearchField Label="Contact" DataFieldName="c.ContactID"
                    BindingControlID="searchContact" BindingDataType="Int32" BindingPropertyName="SelectedValue">
                    <Template>
                        <mapcall:DataSourceDropDownList runat="server" ID="searchContact" TableName="Contacts"
                            TextFieldName="Text" ValueFieldName="Value" SelectCommand="SELECT
                                                                                         LastName + ', ' + FirstName + ' - ' + Email AS Text,
                                                                                         ContactID AS Value
                                                                                       FROM Contacts ORDER BY LastName" />
                    </Template>
                </search:TemplatedSearchField>
                <search:TemplatedSearchField Label="Purpose" DataFieldName="p.NotificationPurposeID"
                    BindingControlID="searchPurpose" BindingDataType="Int32" BindingPropertyName="SelectedValue">
                    <Template>
                        <mapcall:DataSourceDropDownList runat="server" ID="searchPurpose" TableName="NotificationPurposes"
                            TextFieldName="Text" ValueFieldName="Value" SelectCommand="SELECT
                                                                                         a.Name + ' - ' + m.Name + ' - ' + p.Purpose as [Text],
                                                                                         NotificationPurposeID AS Value
                                                                                       FROM NotificationPurposes p
                                                                                       INNER JOIN Modules m on p.ModuleID = m.ModuleID
                                                                                       INNER JOIN Applications a on m.ApplicationID = a.ApplicationID
                                                                                       ORDER BY a.Name, m.Name, p.Purpose" />
                    </Template>
                </search:TemplatedSearchField>
            </Fields>
        </SearchBox>

        <ResultsGridView>
            <Columns>
                <mapcall:BoundField DataField="OperatingCenter" />
                <mapcall:BoundField DataField="Contact" />
                <mapcall:BoundField DataField="Purpose" />
            </Columns>
        </ResultsGridView>

        <ResultsDataSource SelectCommand="SELECT
                                            nc.NotificationConfigurationID as NotificationConfigurationID,
                                            o.OperatingCenterCode + ' - ' + o.OperatingCenterName as OperatingCenter,
                                            c.LastName + ', ' + c.FirstName + ' - ' + c.Email as Contact,
                                            a.Name + ' - ' + m.Name + ' - ' + p.Purpose as Purpose
                                          FROM
                                            [NotificationConfigurations] nc
                                          JOIN [OperatingCenters] o on o.OperatingCenterID = nc.OperatingCenterID
                                          JOIN [Contacts] c on c.ContactID = nc.ContactID
                                          JOIN [NotificationPurposes] p on p.NotificationPurposeID = nc.NotificationPurposeID
                                          JOIN [Modules] m on p.ModuleID = m.ModuleID
                                          JOIN [Applications] a on m.ApplicationID = a.ApplicationID" />
        <DetailsView>
            <Fields>
                <asp:TemplateField HeaderText="Operating Center">
                    <ItemTemplate>
                        <%# Eval("OperatingCenter") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <mapcall:OperatingCenterDropDownList runat="server" ID="ddlOperatingCenter" SelectedValue='<%# Bind("OperatingCenterID") %>' Required="true" />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Contact">
                    <ItemTemplate>
                        <%# Eval("Contact") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <mapcall:DataSourceDropDownList runat="server" ID="ddlContact" TableName="Contacts" SelectedValue='<%# Bind("ContactID") %>' Required="true"
                            TextFieldName="Text" ValueFieldName="Value" SelectCommand="SELECT
                                                                                         LastName + ', ' + FirstName + ' - ' + Email AS Text,
                                                                                         ContactID AS Value
                                                                                       FROM Contacts ORDER BY LastName" />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Purpose">
                    <ItemTemplate>
                        <%# Eval("Purpose") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <mapcall:DataSourceDropDownList runat="server" ID="ddlPurpose" TableName="NotificationPurposes" SelectedValue='<%# Bind("NotificationPurposeID") %>' Required="true"
                            TextFieldName="Text" ValueFieldName="Value" SelectCommand="SELECT
                                                                                         a.Name + ' - ' + m.Name + ' - ' + p.Purpose as [Text],
                                                                                         NotificationPurposeID AS Value
                                                                                       FROM NotificationPurposes p
                                                                                       INNER JOIN Modules m on p.ModuleID = m.ModuleID
                                                                                       INNER JOIN Applications a on m.ApplicationID = a.ApplicationID
                                                                                       ORDER BY a.Name, m.Name, p.Purpose" />
                    </EditItemTemplate>
                </asp:TemplateField>
            </Fields>
        </DetailsView>

        <DetailsDataSource
            DeleteCommand="DELETE FROM [NotificationConfigurations] WHERE [NotificationConfigurationID] = @NotificationConfigurationID"
            InsertCommand="INSERT INTO [NotificationConfigurations] ([OperatingCenterID], [ContactID], [NotificationPurposeID]) VALUES (@OperatingCenterID, @ContactID, @NotificationPurposeID); SELECT @NotificationConfigurationID = (SELECT @@IDENTITY)"
            UpdateCommand="UPDATE [NotificationConfigurations] SET [OperatingCenterID] = @OperatingCenterID, [ContactID] = @ContactID, [NotificationPurposeID] = @NotificationPurposeID WHERE [NotificationConfigurationID] = @NotificationConfigurationID"
            SelectCommand="SELECT
                             nc.NotificationConfigurationID,
                             o.OperatingCenterCode + ' - ' + o.OperatingCenterName as OperatingCenter,
                             o.OperatingCenterID as OperatingCenterID,
                             c.LastName + ', ' + c.FirstName + ' - ' + c.Email as Contact,
                             c.ContactID as ContactID,
                             a.Name + ' - ' + m.Name + ' - ' + p.Purpose as Purpose,
                             p.NotificationPurposeID as NotificationPurposeID
                           FROM
                             [NotificationConfigurations] nc
                           INNER JOIN [OperatingCenters] o on o.OperatingCenterID = nc.OperatingCenterID
                           INNER JOIN [Contacts] c on c.ContactID = nc.ContactID
                           INNER JOIN [NotificationPurposes] p on p.NotificationPurposeID = nc.NotificationPurposeID
                           INNER JOIN [Modules] m on p.ModuleID = m.ModuleID
                           INNER JOIN [Applications] a on m.ApplicationID = a.ApplicationID
                           WHERE nc.[NotificationConfigurationID] = @NotificationConfigurationID">
            <DeleteParameters>
                <asp:Parameter Name="NotificationConfigurationID" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="NotificationConfigurationID" Type="Int32" Direction="Output" />
                <asp:Parameter Name="OperatingCenterID" Type="Int32" />
                <asp:Parameter Name="ContactID" Type="Int32" />
                <asp:Parameter Name="NotificationPurposeID" Type="Int32" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="NotificationConfigurationID" Type="Int32" />
                <asp:Parameter Name="OperatingCenterID" Type="Int32" />
                <asp:Parameter Name="ContactID" Type="Int32" />
                <asp:Parameter Name="NotificationPurposeID" Type="Int32" />
            </UpdateParameters>
            <SelectParameters>
                <asp:Parameter Name="NotificationConfigurationID" Type="Int32" />
            </SelectParameters>
        </DetailsDataSource>
    </mapcall:DetailsViewDataPageTemplate>
</asp:Content>