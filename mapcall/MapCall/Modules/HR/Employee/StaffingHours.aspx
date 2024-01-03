<%@ Page Title="Staffing Hours" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="StaffingHours.aspx.cs" Inherits="MapCall.Modules.HR.Employee.StaffingHours" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls" TagPrefix="mapcall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.SearchFields" TagPrefix="search" %>
<%@ Register Src="~/Controls/DetailsViewDataPageTemplate.ascx" TagPrefix="mapcall" TagName="DetailsViewDataPageTemplate" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
Staffing Hours
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">
<mapcall:DetailsViewDataPageTemplate ID="template" runat="server" 
    DataElementTableName="StaffingHours" 
    DataTypeId="148"
    DataElementPrimaryFieldName="StaffingHourID"
    DefaultPageMode="Home"
    Label="Staffing Hour">
    <SearchBox>
        <Fields>
            <search:TextSearchField DataFieldName="tp.UserName" ID="sfUserName" />
            <search:DateTimeSearchField DataFieldName="sh.WorkDate" />
            <search:NumericSearchField DataFieldName="HoursWorked" SearchType="Range" />
            <search:DateTimeSearchField DataFieldName="sh.WorkApprovedDate" />
            <search:TextSearchField DataFieldName="sh.InvoiceNumber" />
        </Fields>
    </SearchBox>

    <ResultsGridView>
        <Columns>
            <mapcall:BoundField DataField="UserName" />
            <mapcall:BoundField DataField="WorkDate" DataType="Date" />
            <mapcall:BoundField DataField="HoursWorked" /> <%-- Validate that it's a positive number. --%>
            <mapcall:BoundField DataField="WorkDescription" />
            <mapcall:BoundField DataField="WorkApprovedDate" />
            <mapcall:BoundField DataField="InvoiceNumber" />
        </Columns>
    </ResultsGridView>

    <ResultsDataSource
        SelectCommand="SELECT 
                           sh.*,
                           tp.UserName
                       FROM [StaffingHours] sh 
                       JOIN [tblPermissions] tp ON tp.RecID = sh.UserID" />

    <DetailsViewPlaceHolder>
        Work Date, Hours Worked, and Work Description can be edited until the Work Approved Date has been set.
    </DetailsViewPlaceHolder>
    <DetailsView>
        <Fields>
            <mapcall:BoundField DataField="UserName" ReadOnly="true" InsertVisible="false" />
            <mapcall:BoundField ID="bfWorkDate" DataField="WorkDate" DataType="Date" Required="true"  />
            <mapcall:BoundField ID="bfHoursWorked" DataField="HoursWorked" DataType="Decimal" Required="true"  /> <%-- Validate that it's a positive number. --%>
            <mapcall:BoundField ID="bfWorkDescription" DataField="WorkDescription" DataType="Text" Required="true" />

            <%-- Admin only! --%>
            <mapcall:BoundField ID="bfWorkApprovedDate" DataField="WorkApprovedDate" DataType="Date" />
            <mapcall:BoundField ID="bfInvoiceNumber" DataField="InvoiceNumber" MaxLength="20" />
        </Fields>
    </DetailsView>

    <DetailsDataSource 
        DeleteCommand="DELETE FROM [StaffingHours] WHERE [StaffingHourID] = @StaffingHourID" 
        InsertCommand="INSERT INTO [StaffingHours] ([UserID], [WorkDate], [HoursWorked], [WorkDescription], [WorkApprovedDate], [InvoiceNumber], [CreatedBy]) VALUES (@UserID, @WorkDate, @HoursWorked, @WorkDescription, @WorkApprovedDate, @InvoiceNumber, @CreatedBy); SELECT @StaffingHourID = (SELECT @@IDENTITY)" 
        UpdateCommand="UPDATE [StaffingHours] SET [WorkDate] = @WorkDate, [HoursWorked] = @HoursWorked, [WorkDescription] = @WorkDescription, [WorkApprovedDate] = @WorkApprovedDate, [InvoiceNumber] = @InvoiceNumber WHERE [StaffingHourID] = @StaffingHourID"
        SelectCommand="SELECT 
                           sh.*,
                           tp.UserName
                       FROM [StaffingHours] sh 
                       JOIN [tblPermissions] tp ON tp.RecID = sh.UserID
                       WHERE sh.StaffingHourID = @StaffingHourID">
        <DeleteParameters>
            <asp:Parameter Name="StaffingHourID" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="StaffingHourID" Type="Int32" Direction="Output" />
            <asp:Parameter Name="UserID" Type="Int32" />
            <asp:Parameter Name="WorkDate" Type="DateTime" />
            <asp:Parameter Name="HoursWorked" Type="Decimal" />
            <asp:Parameter Name="WorkDescription" Type="String" />
            <asp:Parameter Name="WorkApprovedDate" Type="DateTime" />
            <asp:Parameter Name="InvoiceNumber" Type="String" />
            <asp:Parameter Name="CreatedBy" Type="String" />
        </InsertParameters>
        <SelectParameters>
            <asp:Parameter Name="StaffingHourID" Type="Int32" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="StaffingHourID" Type="Int32" />
            <asp:Parameter Name="WorkDate" Type="DateTime" />
            <asp:Parameter Name="HoursWorked" Type="Decimal" />
            <asp:Parameter Name="WorkDescription" Type="String" />
            <asp:Parameter Name="WorkApprovedDate" Type="DateTime" />
            <asp:Parameter Name="InvoiceNumber" Type="String" />
        </UpdateParameters>
    </DetailsDataSource>

</mapcall:DetailsViewDataPageTemplate>

</asp:Content>
