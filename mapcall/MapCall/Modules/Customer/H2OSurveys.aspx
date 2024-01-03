<%@ Page Title="H2O Surveys" Language="C#" MasterPageFile="~/MapCallSite.Master"
    Theme="bender" AutoEventWireup="true" CodeBehind="H2OSurveys.aspx.cs" Inherits="MapCall.Modules.Customer.H2OSurveys" %>

<%@ Register Assembly="MapCall" Namespace="MapCall.Controls" TagPrefix="mapcall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.Data" TagPrefix="mapcall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.DropDowns" TagPrefix="mapcall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.SearchFields" TagPrefix="search" %>
<%@ Register Assembly="MMSINC.Core.WebForms" Namespace="MMSINC.Controls" TagPrefix="mmsi" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>
<%@ Register Src="~/Controls/DetailsViewDataPageTemplate.ascx" TagPrefix="mapcall"
    TagName="DetailsViewDataPageTemplate" %>

<%-- TODO: Put number of documents in results view

    TODO: If they select yes/no for "Is Customer Qualified" then the date MUST BE ENTERED
 --%>
<asp:Content ContentPlaceHolderID="cphHeadTag" runat="server">
    <%--This is for the tables under Actions.--%>
    <style type="text/css">
        .kitTable
        {
            width: 100%;
            border: solid 1px #cccccc;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
    H2O Surveys
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphInstructions" runat="server">
    Help to Others Program
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">
    <mapcall:DetailsViewDataPageTemplate ID="template" runat="server"
        DefaultPageMode="Home" DataTypeId="144"
        DataElementTableName="H2OSurveys"
        DataElementPrimaryFieldName="H2OSurveyID" Label="H2O Survey">
        <HomePlaceHolder>
           <div class="container bc-box">
                <h2>Quick Search</h2>

                <div class="boxContainer">
                    <div>
                        <div>
                            <table class="simpleTable">
                    <thead>
                        <tr>
                            <th></th>
                            <th>This Week</th>
                            <th>Last Week</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>
                                Number of Customers Enrolled:
                            </td>
                            <td>
                                <asp:LinkButton runat="server" CommandArgument="CustomersEnrolledCountThisWeek">
                                <%= Report.CustomersEnrolledThisWeekCount %>
                                </asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton runat="server" CommandArgument="CustomersEnrolledCountLastWeek">
                                <%= Report.CustomersEnrolledLastWeekCount%>
                                </asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Number of Water Saving Kits Provided:
                            </td>
                            <td>
                                <asp:LinkButton runat="server" CommandArgument="WaterSavingKitsProvidedThisWeek">
                                <%= Report.WaterSavingKitsProvidedThisWeekCount%>
                                </asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton runat="server" CommandArgument="WaterSavingKitsProvidedLastWeek">
                                <%= Report.WaterSavingKitsProvidedLastWeekCount%>
                                </asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Number of Customers First Contacted Through NJ Shares:
                            </td>
                            <td>
                                <asp:LinkButton runat="server" CommandArgument="NjSharesCountThisWeek">
                                <%= Report.NjSharesCountThisWeek%>
                                </asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton runat="server" CommandArgument="NjSharesCountLastWeek">
                                <%= Report.NjSharesCountLastWeek%>
                                </asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Number of Customers First Successfully Contacted By Phone:
                            </td>
                            <td>
                                <asp:LinkButton runat="server" CommandArgument="PhoneCountThisWeek">
                                <%= Report.PhoneCountThisWeek%>
                                </asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton runat="server" CommandArgument="PhoneCountLastWeek">
                                <%= Report.PhoneCountLastWeek%>
                                </asp:LinkButton>
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                Number of Audits Performed:
                            </td>
                            <td>
                                <asp:LinkButton runat="server" CommandArgument="AuditPerformedThisWeek">
                                <%= Report.AuditPerformedThisWeekCount%>
                                </asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton  runat="server" CommandArgument="AuditPerformedLastWeek">
                                <%= Report.AuditPerformedLastWeekCount%>
                                </asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Number of Renters:
                            </td>
                            <td>
                                <asp:LinkButton runat="server" CommandArgument="RenterCountThisWeek">
                                <%= Report.RenterCountThisWeek%>
                                </asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton runat="server" CommandArgument="RenterCountLastWeek">
                                <%= Report.RenterCountLastWeek%>
                                </asp:LinkButton>
                            </td>
                        </tr>
                    </tbody>
                </table>
                        </div>
                    </div>
                        <div>
                        <div>
                            <table class="simpleTable">
                                <thead>
                                    <tr>
                                        <th></th>
                                        <th>Total</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td>Customers on Call List:</td>
                                        <td>
                                            <asp:LinkButton runat="server" CommandArgument="CallListCount">
                                            <%= Report.CallListCount %>
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Customers on Water Kit Follow Up Call List:</td>
                                        <td>
                                            <asp:LinkButton runat="server" CommandArgument="WaterKitFollowUpCallListCount">
                                            <%= Report.WaterKitFollowUpCallListCount  %>
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Audited Customers Waiting For Approval:</td>
                                        <td>
                                            <asp:LinkButton runat="server" CommandArgument="PendingQualificationApprovalCount">
                                            <%= Report.PendingQualificationApprovalCount %>
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Customers that Require Additional Scheduling:</td>
                                        <td>
                                            <asp:LinkButton runat="server" CommandArgument="CustomersThatRequireAdditionalScheduling">
                                            <%= Report.CustomersThatRequireAdditionalSchedulingCount %>
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
           </div>
        </HomePlaceHolder>

        <SearchBox runat="server">
            <Fields>
                <search:TextSearchField DataFieldName="FirstName" />
                <search:TextSearchField DataFieldName="LastName" />
                <search:BooleanSearchField DataFieldName="IsDuplicate" SearchType="DropDownList" />
                <search:TemplatedSearchField Label="State" 
                                             BindingControlID="ddlStateSearch" 
                                             BindingPropertyName="SelectedValue" 
                                             BindingDataType="Int32" 
                                             DataFieldName="h.StateID">
                    <Template>
                        <mapcall:StatesDropDownList ID="ddlStateSearch" runat="server" />
                    </Template>
                </search:TemplatedSearchField>
                <search:TemplatedSearchField Label="Town" 
                                             DataFieldName="h.CityID"
                                             BindingControlID="ddlTownSearch"
                                             BindingPropertyName="SelectedValue"
                                             BindingDataType="Int32">
                    <Template>
                        <mmsi:MvpDropDownList runat="server" ID="ddlTownSearch" />
                        <atk:CascadingDropDown runat="server" ID="cddTownSearch" TargetControlID="ddlTownSearch"
                            ParentControlID="ddlStateSearch" Category="Town" EmptyText="None Found" EmptyValue=""
                            PromptText="-- Select City --" PromptValue="" LoadingText="[Loading Cities...]"
                            ServicePath="~/Modules/Data/DropDowns.asmx" ServiceMethod="GetTownsByState" />
                    </Template>
                </search:TemplatedSearchField>
                <search:TemplatedSearchField Label="Town Section"
                                             DataFieldName="h.TownSectionID"
                                             BindingControlID="ddlTownSectionSearch"
                                             BindingPropertyName="SelectedValue"
                                             BindingDataType="Int32">
                    <Template>
                        <mmsi:MvpDropDownList runat="server" ID="ddlTownSectionSearch" />
                        <atk:CascadingDropDown runat="server" ID="cddTownSectionSearch" TargetControlID="ddlTownSectionSearch" ParentControlID="ddlTownSearch"
                            Category="Town" EmptyText="None Found" EmptyValue="" PromptText="-- Select Town Section --"
                            PromptValue="" LoadingText="[Loading Town Sections...]" ServicePath="~/Modules/Data/DropDowns.asmx"
                            ServiceMethod="GetTownSectionsByTownDefined" />
                    </Template>
                </search:TemplatedSearchField>
                <search:DropDownSearchField Label="Customer Received Through" 
                                            DataFieldName="h.H2OSurveyReceivedThroughTypeID"
                                            TableName="H2OSurveyReceivedThroughTypes" 
                                            TextFieldName="Description"
                                            ValueFieldName="H2OSurveyReceivedThroughTypeID" />
                <search:DateTimeSearchField DataFieldName="CustomerReceivedDate" />
                <search:DateTimeSearchField DataFieldName="LastContactDate" />
                <search:DropDownSearchField Label="Contact Status Result" 
                                            DataFieldName="h.H2OSurveyContactStatusTypeID"
                                            TableName="H2OSurveyContactStatusTypes" 
                                            TextFieldName="Description"
                                            ValueFieldName="H2OSurveyContactStatusTypeID" />
                                            
                <search:DateTimeSearchField DataFieldName="EnrollmentDate" />
                <search:DateTimeSearchField DataFieldName="FirstLetterSent" />
                <search:DateTimeSearchField DataFieldName="SecondLetterSent" />
                <search:DateTimeSearchField DataFieldName="ThirdLetterSent" />
                <search:BooleanSearchField DataFieldName="QualifiesForKit" 
                                           SearchType="DropDownList" />
                <search:DateTimeSearchField DataFieldName="QualifiesForKitDate" />
                <search:BooleanSearchField DataFieldName="DoesCustomerWantToParticpate" 
                                           Label="Custom Wants To Participate"
                                           SearchType="DropDownList" />
                <search:BooleanSearchField DataFieldName="IsHomeOwner" 
                                           SearchType="DropDownList" />
                <search:DropDownSearchField Label="Dwelling Type" 
                                            DataFieldName="h.DwellingTypeID"
                                            TableName="DwellingTypes" 
                                            TextFieldName="Description"
                                            ValueFieldName="DwellingTypeID" />
                <search:NumericSearchField SearchType="Range" DataFieldName="Toilets" Label="Number of Toilets" />
                <search:NumericSearchField SearchType="Range" DataFieldName="ShowerBathtubs" Label="Number of combination shower bathtubs" />
                <search:NumericSearchField SearchType="Range" DataFieldName="ShowerOnly" Label="Number of showers only" />
                <search:NumericSearchField SearchType="Range" DataFieldName="BathroomSinks" Label="Number of bathroom sinks" />
                <search:NumericSearchField SearchType="Range" DataFieldName="KitchenSinks" Label="Number of kitchen sinks" />
                <search:NumericSearchField SearchType="Range" DataFieldName="MiscSinks" Label="Number of misc sinks" />
                <search:BooleanSearchField SearchType="DropDownList" DataFieldName="HasWasher" />
                <search:BooleanSearchField SearchType="DropDownList" DataFieldName="IsWasherFrontLoader" />
                <search:NumericSearchField SearchType="Range" DataFieldName="IceMakers" Label="Number of ice makers" />
                <search:BooleanSearchField SearchType="DropDownList" DataFieldName="HasDishwasher" />
                <search:NumericSearchField SearchType="Range" DataFieldName="DishLoadsPerWeek" Label="Number of dishwasher loads per week" />
                <search:NumericSearchField SearchType="Range" DataFieldName="AdultFamilyMembers" Label="Adult family members" />
                <search:NumericSearchField SearchType="Range" DataFieldName="Children6AndUnder" Label="Children ages 6 and under" />
                <search:NumericSearchField SearchType="Range" DataFieldName="Children7To12" Label="Children ages 7 to 12" />
                <search:NumericSearchField SearchType="Range" DataFieldName="Children13To17" Label="Children ages 13 to 17" />
                <search:NumericSearchField SearchType="Range" DataFieldName="MembersOver65" Label="Family members over 65" />
                <search:NumericSearchField SearchType="Range" DataFieldName="TypicalUsage" />
                <search:NumericSearchField SearchType="Range" DataFieldName="CustomerUsage" />
                <search:NumericSearchField SearchType="Range" DataFieldName="UsagePercentageDiff" />
                <search:DropDownSearchField Label="Audit Performed By"
                                            DataFieldName="h.H2OSurveyAuditPerformedByTypeID"
                                            TableName="H2OSurveyAuditPerformedByTypes"
                                            TextFieldName="Description"
                                            ValueFieldName="H2OSurveyAuditPerformedByTypeID" />
                <search:DateTimeSearchField DataFieldName="AuditPerformedDate" />
                <search:BooleanSearchField SearchType="DropDownList" DataFieldName="CustomerWithHighWaterUsage" Label="Customer With High Water Usage" />
                <search:DateTimeSearchField DataFieldName="SiteVisitDate" />
                <search:DateTimeSearchField DataFieldName="WaterSavingKitProvidedDate" />
                <search:DateTimeSearchField DataFieldName="WaterSavingKitFollowupDate" Label="Water Saving Kit Follow Up Date" />
                <search:DropDownSearchField Label="Water Saving Kit Followup Result" 
                                            DataFieldName="h.WaterSavingKitFollowupStatusTypeID"
                                            TableName="H2OSurveyContactStatusTypes" 
                                            TextFieldName="Description"
                                            ValueFieldName="H2OSurveyContactStatusTypeID" />
                <search:DropDownSearchField Label="Water Saving Kit Followup Outcome" 
                                            DataFieldName="h.H2OSurveyWaterKitFollowUpOutcomeTypeID"
                                            TableName="H2OSurveyWaterKitFollowUpOutcomeTypes" 
                                            TextFieldName="Description"
                                            ValueFieldName="H2OSurveyWaterKitFollowUpOutcomeTypeID" />
                <search:NumericSearchField SearchType="Range" DataFieldName="KitShowerHeads" Label="Number of Shower Heads" />
                <search:NumericSearchField SearchType="Range" DataFieldName="KitDyeKits" Label="Number of Dye Kits" />
                <search:NumericSearchField SearchType="Range" DataFieldName="KitKitchenAerator" Label="Number of Kitchen Aerators" />
                <search:NumericSearchField SearchType="Range" DataFieldName="KitBathAerator" Label="Number of Bath Aerators" />
                <search:NumericSearchField SearchType="Range" DataFieldName="KitTankBags" Label="Number of Tank Bags" />
                <search:NumericSearchField SearchType="Range" DataFieldName="KitTeflonTape" Label="Number of Teflon Tape" />
                <search:DateTimeSearchField DataFieldName="SmallRepairsCompletedDate" />
                <search:DateTimeSearchField DataFieldName="TechnicianNeededForWaterKitDate" />
                <search:DropDownSearchField Label="Site Visit Scheduling Status" 
                                            DataFieldName="h.SiteVisitScheduleStatusTypeID"
                                            TableName="H2OSurveyContactStatusTypes" 
                                            TextFieldName="Description"
                                            ValueFieldName="H2OSurveyContactStatusTypeID" />
                <search:DropDownSearchField Label="Site Visit Scheduling Outcome" 
                                            DataFieldName="h.H2OSurveySiteVisitSchedulingOutcomeTypeID"
                                            TableName="H2OSurveySiteVisitSchedulingOutcomeTypes" 
                                            TextFieldName="Description"
                                            ValueFieldName="H2OSurveySiteVisitSchedulingOutcomeTypeID" />
            </Fields>
        </SearchBox>

        <ResultsGridView>
            <Columns>
                <mapcall:BoundField DataField="H2OSurveyID" HeaderText="H2OSurveyID" SortExpression="H2OSurveyID" />
                <mapcall:BoundField DataField="NJAWAccountNumber" HeaderText="NJAW Account Number" SortExpression="NJAWAccountNumber" />
                <mapcall:TemplateBoundField HeaderText="Full Name" SortExpression="LastName">
                    <ItemTemplate>
                        <%# Eval("LastName") %>, <%# Eval("FirstName") %>
                    </ItemTemplate>
                </mapcall:TemplateBoundField>
                <mapcall:TemplateBoundField HeaderText="Address"> 
                    <ItemTemplate>
                        <pre><%# FormatAddress(Eval("HouseNumber").ToString(),
                                               Eval("ApartmentNumber").ToString(), 
                                               Eval("StreetText").ToString(),
                                               Eval("AddressLine2").ToString(),
                                               Eval("CityText").ToString(),
                                               Eval("TownSectionText").ToString(),
                                               Eval("StateText").ToString(),
                                               Eval("Zip").ToString()) %></pre>
                    </ItemTemplate>
                </mapcall:TemplateBoundField>
                <mapcall:BoundField DataField="HomePhone" HeaderText="Home Phone" SortExpression="HomePhone" />
                <mapcall:BoundField DataField="Mobile" HeaderText="Mobile" SortExpression="Mobile" />
                <mapcall:BoundField DataField="EmailAddress" HeaderText="Email" SortExpression="EmailAddress" />
                <mapcall:BoundField DataField="ContactStatusDescription" DataType="NVarChar" />
                <mapcall:BoundField DataField="EnrollmentDate" DataType="Date" />
                <mapcall:BoundField DataField="FirstLetterSent" DataType="Date" />
                <mapcall:BoundField DataField="SecondLetterSent" DataType="Date" />
                <mapcall:BoundField DataField="ThirdLetterSent" DataType="Date" />
                <mapcall:BoundField DataField="DoesCustomerWantToParticpate" 
                                    HeaderText="Customer Wants To Particpate"
                                    SortExpression="DoesCustomerWantToParticpate"
                                    DataType="Bit" />
                <mapcall:BoundField DataField="QualifiesForKit"
                                    DataType="Bit" />
                <mapcall:BoundField DataField="DwellingTypeDescription" HeaderText="Dwelling Type" SortExpression="DwellingTypeDescription" />
                <mapcall:BoundField DataField="CreatedBy" HeaderText="CreatedBy" SortExpression="CreatedBy" />
                <mapcall:BoundField DataField="CreatedOn" HeaderText="CreatedOn" SortExpression="CreatedOn" />
            </Columns>
        </ResultsGridView>
        <ResultsDataSource SelectCommand="SELECT
                        h.*,
                        cstat.Description as [ContactStatusDescription],
                        dt.Description as [DwellingTypeDescription],
                        states.Abbreviation as StateText,
                        njtown.Town as CityText,
                        njtownsect.Name as TownSectionText,
                        njstreet.FullStName as StreetText
                    FROM 
                        [H2OSurveys] h
                    LEFT JOIN [H2OSurveyContactStatusTypes] cstat ON cstat.H2OSurveyContactStatusTypeID = h.H2OSurveyContactStatusTypeID
                    LEFT JOIN [States] states                     ON states.StateID = h.StateID
                    LEFT JOIN Towns njtown                        ON njtown.TownID = h.CityID
                    LEFT JOIN TownSections njtownsect             ON njtownsect.TownSectionID = h.TownSectionID
                    LEFT JOIN Streets njstreet                    ON njstreet.StreetID = h.StreetID
                    LEFT JOIN [DwellingTypes] dt                  ON dt.DwellingTypeID = h.DwellingTypeID">
        </ResultsDataSource>
        <DetailsView>
            <Fields>
                <mapcall:TemplateBoundField HeaderText="CreatedBy" InsertVisible="false">
                    <ItemTemplate>
                        <%# Eval("CreatedBy") %>
                    </ItemTemplate>
                </mapcall:TemplateBoundField>
                <mapcall:BoundField DataField="NJAWAccountNumber" DataType="NVarChar" MaxLength="20" Required="true" />
                <mapcall:BoundField DataField="FirstName" DataType="NVarChar" MaxLength="20" Required="true" />
                <mapcall:BoundField DataField="LastName" DataType="NVarChar" MaxLength="20" Required="true" />
                <mapcall:BoundField DataField="IsDuplicate" DataType="Bit" />
                <mapcall:TemplateBoundField HeaderText="State">
                    <ItemTemplate>
                        <%#Eval("StateText") %></ItemTemplate>
                    <EditItemTemplate>
                        <mapcall:StatesDropDownList ID="ddlState" runat="server" SelectedValue='<%# Bind("StateID") %>' Required="true" />
                    </EditItemTemplate>
                </mapcall:TemplateBoundField>
                <mapcall:TemplateBoundField HeaderText="City">
                    <ItemTemplate>
                        <%#Eval("CityText") %></ItemTemplate>
                    <EditItemTemplate>
                        <mmsi:MvpDropDownList runat="server" ID="ddlTown" />
                        <atk:CascadingDropDown runat="server" ID="cddTowns" TargetControlID="ddlTown" ParentControlID="ddlState"
                            Category="Town" EmptyText="None Found" EmptyValue="" PromptText="-- Select City --"
                            PromptValue="" LoadingText="[Loading Cities...]" ServicePath="~/Modules/Data/DropDowns.asmx"
                            ServiceMethod="GetTownsByState" SelectedValue='<%# Bind("CityID") %>' />
                        <asp:RequiredFieldValidator ID="rfvTown" runat="server" ControlToValidate="ddlTown"
                            ErrorMessage="Required" InitialValue="" ClientIDMode="AutoID" />
                    </EditItemTemplate>
                </mapcall:TemplateBoundField>
                <mapcall:TemplateBoundField HeaderText="Town Section">
                    <ItemTemplate>
                        <%#Eval("TownSectionText") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <mmsi:MvpDropDownList runat="server" ID="ddlTownSection" />
                        <atk:CascadingDropDown runat="server" ID="cddTownSection" TargetControlID="ddlTownSection" ParentControlID="ddlTown"
                            Category="Town" EmptyText="None Found" EmptyValue="" PromptText="-- Select Town Section --"
                            PromptValue="" LoadingText="[Loading Town Sections...]" ServicePath="~/Modules/Data/DropDowns.asmx"
                            ServiceMethod="GetTownSectionsByTownDefined" SelectedValue='<%# Bind("TownSectionID") %>' />
                    </EditItemTemplate>
                </mapcall:TemplateBoundField>
                <mapcall:BoundField DataField="HouseNumber" DataType="NVarChar" MaxLength="20" />
                <mapcall:BoundField DataField="ApartmentNumber" DataType="NVarChar" MaxLength="20" />
                <mapcall:TemplateBoundField HeaderText="Street">
                    <ItemTemplate>
                        <%# Eval("StreetText")%></ItemTemplate>
                    <EditItemTemplate>
                        <mmsi:MvpDropDownList runat="server" ID="ddlStreet" />
                        <atk:CascadingDropDown runat="server" ID="cddStreets" TargetControlID="ddlStreet"
                            ParentControlID="ddlTown" Category="Street" EmptyText="None Found" EmptyValue=""
                            PromptText="--Select Here--" PromptValue="" LoadingText="[Loading Streets...]"
                            ServicePath="~/Modules/Data/DropDowns.asmx" ServiceMethod="GetStreetsByTown"
                            SelectedValue='<%# Bind("StreetID") %>' />
                        <asp:RequiredFieldValidator ID="rfvStreets" runat="server" ControlToValidate="ddlStreet"
                            ErrorMessage="Required" InitialValue="" ClientIDMode="AutoID" />
                    </EditItemTemplate>
                </mapcall:TemplateBoundField>
                <mapcall:BoundField DataField="AddressLine2" HeaderText="Address Line 2" DataType="NVarChar" MaxLength="50" />
                <mapcall:BoundField DataField="Zip" DataType="NVarChar" MaxLength="12" />
                <mapcall:BoundField DataField="HomePhone" DataType="NVarChar" MaxLength="15" />
                <mapcall:BoundField DataField="Mobile" HeaderText="Mobile Phone Number" DataType="NVarChar" MaxLength="15" />
                <mapcall:BoundField DataField="EmailAddress" DataType="NVarChar" MaxLength="50" />
                <mapcall:TemplateBoundField HeaderText="Customer Received Through">
                    <ItemTemplate><%#Eval("ReceivedThroughDescription") %></ItemTemplate>
                    <EditItemTemplate>
                        <mapcall:DataSourceDropDownList ID="ddlReceivedThrough" runat="server" 
                            TableName="H2OSurveyReceivedThroughTypes" 
                            TextFieldName="Description"
                            ValueFieldName="H2OSurveyReceivedThroughTypeID" EnableCaching="true" 
                            SelectedValue='<%# Bind("H2OSurveyReceivedThroughTypeID") %>'/>
                    </EditItemTemplate>
                </mapcall:TemplateBoundField>
                <mapcall:BoundField DataField="CustomerReceivedDate" DataType="DateTime" />
                <mapcall:BoundField DataField="LastContactDate" DataType="Date" />
                <mapcall:TemplateBoundField HeaderText="Last Contact Result">
                    <ItemTemplate><%# Eval("ContactStatusDescription") %></ItemTemplate>
                    <EditItemTemplate>
                        <mapcall:DataSourceDropDownList ID="ddlStatus" runat="server" 
                            TableName="H2OSurveyContactStatusTypes" 
                            TextFieldName="Description"
                            ValueFieldName="H2OSurveyContactStatusTypeID" EnableCaching="true" 
                            SelectedValue='<%# Bind("H2OSurveyContactStatusTypeID") %>'/>
                    </EditItemTemplate>
                </mapcall:TemplateBoundField>
                <mapcall:BoundField DataType="DateTime2" DataField="EnrollmentDate" />
                <mapcall:BoundField DataType="DateTime2" DataField="FirstLetterSent" />
                <mapcall:BoundField DataType="DateTime2" DataField="SecondLetterSent" />
                <mapcall:BoundField DataType="DateTime2" DataField="ThirdLetterSent" />
                <mapcall:TemplateBoundField InsertVisible="false">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="btnPrint" OnClick="BtnPrint_Click" Text="Print Letter" />
                    </ItemTemplate>
                </mapcall:TemplateBoundField>
                <mapcall:TemplateBoundField InsertVisible="false">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="btnPrintNoWaterKitLetter" OnClick="BtnPrintNoWaterKit_Click" Text="Print 'No Water Kit' Letter"  />
                    </ItemTemplate>
                </mapcall:TemplateBoundField>
            </Fields>
        </DetailsView>
        <DetailsDataSource runat="server" DeleteCommand="DELETE FROM [H2OSurveys] WHERE [H2OSurveyID] = @H2OSurveyID"
            InsertCommand="INSERT INTO [H2OSurveys] ([NJAWAccountNumber], [LastName], [FirstName], [IsDuplicate], [HouseNumber], [ApartmentNumber], [StreetID], [AddressLine2], [CityID], [TownSectionID], [StateID], [Zip], [HomePhone], [Mobile], [EmailAddress], [LastContactDate], [H2OSurveyContactStatusTypeID], [H2OSurveyReceivedThroughTypeID], [CustomerReceivedDate], [FirstLetterSent], [ThirdLetterSent], [SecondLetterSent], [EnrollmentDate], [CreatedBy]) VALUES (@NJAWAccountNumber, @LastName, @FirstName, @IsDuplicate, @HouseNumber, @ApartmentNumber, @StreetID, @AddressLine2, @CityID, @TownSectionID, @StateID, @Zip, @HomePhone, @Mobile, @EmailAddress, @LastContactDate, @H2OSurveyContactStatusTypeID, @H2OSurveyReceivedThroughTypeID, @CustomerReceivedDate, @FirstLetterSent, @ThirdLetterSent, @SecondLetterSent, @EnrollmentDate, @CreatedBy); SELECT @H2OSurveyID = (SELECT @@IDENTITY)"
            UpdateCommand="UPDATE [H2OSurveys]
                              SET [NJAWAccountNumber] = @NJAWAccountNumber, 
                                  [LastName] = @LastName, 
                                  [FirstName] = @FirstName, 
                                  [IsDuplicate] = @IsDuplicate, 
                                  [HouseNumber] = @HouseNumber, 
                                  [ApartmentNumber] = @ApartmentNumber, 
                                  [StreetID] = @StreetID, 
                                  [AddressLine2] = @AddressLine2, 
                                  [CityID] = @CityID, 
                                  [TownSectionID] = @TownSectionID, 
                                  [StateID] = @StateID,
                                  [Zip] = @Zip,
                                  [HomePhone] = @HomePhone, 
                                  [Mobile] = @Mobile, 
                                  [EmailAddress] = @EmailAddress, 
                                  [LastContactDate] = @LastContactDate, 
                                  [H2OSurveyContactStatusTypeID] = @H2OSurveyContactStatusTypeID, 
                                  [H2OSurveyReceivedThroughTypeID] = @H2OSurveyReceivedThroughTypeID, 
                                  [CustomerReceivedDate] = @CustomerReceivedDate, 
                                  [EnrollmentDate] = @EnrollmentDate,
                                  [FirstLetterSent] = @FirstLetterSent, 
                                  [ThirdLetterSent] = @ThirdLetterSent, 
                                  [SecondLetterSent] = @SecondLetterSent 
                            WHERE [H2OSurveyID] = @H2OSurveyID"
            SelectCommand="SELECT
                            h.[H2OSurveyID], 
                            h.[NJAWAccountNumber], 
                            h.[LastName], 
                            h.[FirstName], 
                            h.[IsDuplicate],
                            h.[HouseNumber], 
                            h.[ApartmentNumber], 
                            h.[StreetID], 
                            h.[AddressLine2], 
                            h.[CityID], 
                            h.[TownSectionID],
                            h.[StateID], 
                            h.[Zip], 
                            h.[HomePhone], 
                            h.[Mobile], 
                            h.[EmailAddress], 
                            h.[H2OSurveyReceivedThroughTypeID],
                            h.[CustomerReceivedDate], 
                            h.[LastContactDate],
                            h.[H2OSurveyContactStatusTypeID],
                            h.[EnrollmentDate],
                            h.[FirstLetterSent], 
                            h.[ThirdLetterSent],
                            h.[SecondLetterSent],
                            h.[CreatedBy],
                            cstat.Description as [ContactStatusDescription],
                            states.Abbreviation as StateText,
                            njtown.Town as CityText,
                            njtownsect.Name as TownSectionText,
                            njstreet.FullStName as StreetText,
                            srt.Description as [ReceivedThroughDescription]
                      FROM 
                            [H2OSurveys] h
                      LEFT JOIN [H2OSurveyContactStatusTypes] cstat    ON cstat.H2OSurveyContactStatusTypeID = h.H2OSurveyContactStatusTypeID
                      LEFT JOIN [States] states                        ON states.StateID = h.StateID
                      LEFT JOIN Towns njtown                           ON njtown.TownID = h.CityID
                      LEFT JOIN TownSections njtownsect                ON njtownsect.TownSectionID = h.TownSectionID
                      LEFT JOIN Streets njstreet                       ON njstreet.StreetID = h.StreetID
                      LEFT JOIN [H2OSurveyReceivedThroughTypes] srt    ON srt.H2OSurveyReceivedThroughTypeID = h.H2OSurveyReceivedThroughTypeID
                      WHERE
                            ([H2OSurveyID] = @H2OSurveyID)">
            <SelectParameters>
                <asp:Parameter Name="H2OSurveyID" Type="Int32" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="H2OSurveyID" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="H2OSurveyID" Type="Int32" Direction="Output" />
                <asp:Parameter Name="NJAWAccountNumber" Type="String" />
                <asp:Parameter Name="LastName" Type="String" />
                <asp:Parameter Name="FirstName" Type="String" />
                <asp:Parameter Name="IsDuplicate" Type="Boolean" />
                <asp:Parameter Name="HouseNumber" Type="String" />
                <asp:Parameter Name="ApartmentNumber" Type="String" />
                <asp:Parameter Name="StreetID" Type="Int32" />
                <asp:Parameter Name="AddressLine2" Type="String" />
                <asp:Parameter Name="CityID" Type="Int32" />
                <asp:Parameter Name="TownSectionID" Type="Int32" />
                <asp:Parameter Name="StateID" Type="Int32" />
                <asp:Parameter Name="Zip" Type="String" />
                <asp:Parameter Name="HomePhone" Type="String" />
                <asp:Parameter Name="Mobile" Type="String" />
                <asp:Parameter Name="EmailAddress" Type="String" />
                <asp:Parameter Name="LastContactDate" Type="DateTime" />
                <asp:Parameter Name="H2OSurveyContactStatusTypeID" Type="Int32" />
                <asp:Parameter Name="H2OSurveyReceivedThroughTypeID" Type="Int32" />
                <asp:Parameter Name="CustomerReceivedDate" Type="DateTime" />
                <asp:Parameter Name="EnrollmentDate" Type="DateTime" />
                <asp:Parameter Name="FirstLetterSent" Type="DateTime" />
                <asp:Parameter Name="ThirdLetterSent" Type="DateTime" />
                <asp:Parameter Name="SecondLetterSent" Type="DateTime" />
                <asp:Parameter Name="CreatedBy" Type="String" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="NJAWAccountNumber" Type="String" />
                <asp:Parameter Name="LastName" Type="String" />
                <asp:Parameter Name="FirstName" Type="String" />
                <asp:Parameter Name="IsDuplicate" Type="Boolean" />
                <asp:Parameter Name="HouseNumber" Type="String" />
                <asp:Parameter Name="ApartmentNumber" Type="String" />
                <asp:Parameter Name="StreetID" Type="Int32" />
                <asp:Parameter Name="AddressLine2" Type="String" />
                <asp:Parameter Name="CityID" Type="Int32" />
                <asp:Parameter Name="TownSectionID" Type="Int32" />
                <asp:Parameter Name="StateID" Type="Int32" />
                <asp:Parameter Name="Zip" Type="String" />
                <asp:Parameter Name="HomePhone" Type="String" />
                <asp:Parameter Name="Mobile" Type="String" />
                <asp:Parameter Name="EmailAddress" Type="String" />
                <asp:Parameter Name="LastContactDate" Type="DateTime" />
                <asp:Parameter Name="H2OSurveyContactStatusTypeID" Type="Int32" />
                <asp:Parameter Name="H2OSurveyReceivedThroughTypeID" Type="Int32" />
                <asp:Parameter Name="CustomerReceivedDate" Type="DateTime" />
                <asp:Parameter Name="EnrollmentDate" Type="DateTime" />
                <asp:Parameter Name="FirstLetterSent" Type="DateTime" />
                <asp:Parameter Name="ThirdLetterSent" Type="DateTime" />
                <asp:Parameter Name="SecondLetterSent" Type="DateTime" />
                <asp:Parameter Name="H2OSurveyID" Type="Int32" />
            </UpdateParameters>
        </DetailsDataSource>
        <Tabs>
            <mapcall:Tab runat="server" ID="tabAudit" 
                Label="Audit"
                VisibleDuringInsert="false" 
                VisibleDuringUpdate="true">
                <asp:UpdatePanel ID="upAuditTab" runat="server" ClientIDMode="Static">
                    <ContentTemplate>
                        <asp:DetailsView ID="dvAudit" runat="server" DataSourceID="dsAudit" DataKeyNames="H2OSurveyID"
                            AutoGenerateRows="false" DefaultMode="ReadOnly">
                            <Fields>
                                <mapcall:BoundField DataField="NJAWAccountNumber" DataType="NVarChar" ReadOnly="true" />
                                <mapcall:BoundField DataField="FirstName" DataType="NVarChar" ReadOnly="true" />
                                <mapcall:BoundField DataField="LastName" DataType="NVarChar" ReadOnly="true" />
                                <mapcall:TemplateBoundField HeaderText="Audit Performed By">
                                    <ItemTemplate>
                                        <%# Eval("AuditPerformedByDescription") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <mapcall:DataSourceDropDownList ID="ddlAuditPerformedBy" runat="server" EnableViewState="true" 
                                           TextFieldName="Description" ValueFieldName="H2OSurveyAuditPerformedByTypeID" TableName="H2OSurveyAuditPerformedByTypes"
                                           SelectedValue='<%# Bind("H2OSurveyAuditPerformedByTypeID") %>' />
                                    </EditItemTemplate>
                                </mapcall:TemplateBoundField>
                                <mapcall:BoundField DataField="AuditPerformedDate" DataType="Date" />
                                <mapcall:BoundField DataField="DoesCustomerWantToParticpate"
                                                    HeaderText="Customer Wants To Participate" DataType="Bit">
                                    <BooleanBoundFieldOptions ControlType="RadioButtonList" />
                                </mapcall:BoundField>
                                <mapcall:BoundField DataField="IsHomeOwner" DataType="Bit">
                                    <BooleanBoundFieldOptions ControlType="RadioButtonList" />
                                </mapcall:BoundField>
                                <mapcall:TemplateBoundField HeaderText="Dwelling Type">
                                    <ItemTemplate>
                                        <%# Eval("DwellingTypeDescription") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <mapcall:DataSourceDropDownList ID="ddlDw" runat="server" EnableViewState="true"
                                            TextFieldName="Description" ValueFieldName="DwellingTypeID" TableName="DwellingTypes"
                                            SelectedValue='<%# Bind("DwellingTypeID") %>' />
                                    </EditItemTemplate>
                                </mapcall:TemplateBoundField>
                                <mapcall:BoundField DataField="Toilets" HeaderText="Number of toilets" DataType="Int" />
                                <mapcall:BoundField DataField="BathroomSinks" HeaderText="Number of bathroom sinks" DataType="Int" />
                                <mapcall:BoundField DataField="ShowerBathtubs" HeaderText="Number of combination shower/bathtubs" DataType="Int" />
                                <mapcall:BoundField DataField="ShowerOnly" HeaderText="Number of standalone showers" DataType="Int" />
                                <mapcall:BoundField DataField="KitchenSinks" HeaderText="Number of kitchen sinks" DataType="Int" />
                                <mapcall:BoundField DataField="MiscSinks" HeaderText="Number of other sinks" DataType="Int" />
                                <mapcall:BoundField DataField="HasWasher" DataType="Bit">
                                    <BooleanBoundFieldOptions ControlType="RadioButtonList" />
                                </mapcall:BoundField>
                                <mapcall:BoundField DataField="IsWasherFrontLoader" DataType="Bit">
                                    <BooleanBoundFieldOptions ControlType="RadioButtonList" />
                                </mapcall:BoundField>
                                <mapcall:BoundField DataField="IceMakers" HeaderText="Number of ice makers" DataType="Int" />
                                <mapcall:BoundField DataField="HasDishwasher" HeaderText="Has dishwasher?" DataType="Bit">
                                    <BooleanBoundFieldOptions ControlType="RadioButtonList" />
                                </mapcall:BoundField>
                                <mapcall:BoundField DataField="DishLoadsPerWeek" HeaderText="Number of dishwasher loads per week"
                                    DataType="Int" />
                                <mapcall:BoundField DataField="AdultFamilyMembers" HeaderText="Number of adult family members(ages 18 and up) living in residence"
                                    DataType="Int" />
                                <mapcall:BoundField DataField="Children6AndUnder" HeaderText="Number of children ages 6 and under living in residence"
                                    DataType="Int" />
                                <mapcall:BoundField DataField="Children7To12" HeaderText="Number of children ages 7 to 12 living in residence"
                                    DataType="Int" />
                                <mapcall:BoundField DataField="Children13To17" HeaderText="Number of children ages 13 to 17 living in residence"
                                    DataType="Int" />
                                <mapcall:BoundField DataField="MembersOver65" HeaderText="Number of family members 65 and older living in residence"
                                    DataType="Int" />
                                <mapcall:BoundField DataField="TypicalUsage" DataType="Decimal" />
                                <mapcall:BoundField DataField="CustomerUsage" DataType="Decimal" />
                                <mapcall:BoundField DataField="UsagePercentageDiff" DataType="Decimal" />
                                <mapcall:TemplateBoundField ShowHeader="False">
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="btnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                            Text="Update" />
                                        <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                            Text="Cancel" />
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <%--Empty so EditItemTemplate doesn't take its place --%>
                                    </InsertItemTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnEdit" runat="server" Visible='<%# Permissions.EditAccess.IsAllowed %>'
                                            CausesValidation="False" CommandName="Edit" Text="Edit" />
                                        <%--Deletes are done from the custom tab only--%>
                                    </ItemTemplate>
                                </mapcall:TemplateBoundField>
                            </Fields>
                        </asp:DetailsView>
                        <mapcall:McProdDataSource ID="dsAudit" runat="server" CancelSelectOnNullParameter="true"
                            EnableViewState="false" UpdateCommand="UPDATE [H2OSurveys] SET [H2OSurveyAuditPerformedByTypeID] = @H2OSurveyAuditPerformedByTypeID, [AuditPerformedDate] = @AuditPerformedDate, [DoesCustomerWantToParticpate] = @DoesCustomerWantToParticpate, [IsHomeOwner] = @IsHomeOwner, [DwellingTypeID] = @DwellingTypeID, [Toilets] = @Toilets, [BathroomSinks] = @BathroomSinks, [ShowerBathtubs] = @ShowerBathtubs, [ShowerOnly] = @ShowerOnly, [KitchenSinks] = @KitchenSinks, [MiscSinks] = @MiscSinks, [HasWasher] = @HasWasher, [IceMakers] = @IceMakers, [IsWasherFrontLoader] = @IsWasherFrontLoader, [HasDishwasher] = @HasDishwasher, [DishLoadsPerWeek] = @DishLoadsPerWeek, [AdultFamilyMembers] = @AdultFamilyMembers, [Children6AndUnder] = @Children6AndUnder, [Children7To12] = @Children7To12, [Children13To17] = @Children13To17, [MembersOver65] = @MembersOver65, [TypicalUsage] = @TypicalUsage, [CustomerUsage] = @CustomerUsage, [UsagePercentageDiff] = @UsagePercentageDiff  WHERE [H2OSurveyID] = @H2OSurveyID"
                            SelectCommand="SELECT 
                                            h.[H2OSurveyAuditPerformedByTypeID],
                                            h.[NJAWAccountNumber],
                                            h.[FirstName],
                                            h.[LastName],
                                            h.[AuditPerformedDate],
                                            h.[DoesCustomerWantToParticpate],
                                            h.[H2OSurveyID],
                                            h.[IsHomeOwner], 
                                            h.[DwellingTypeID], 
                                            h.[Toilets], 
                                            h.[BathroomSinks], 
                                            h.[ShowerBathtubs], 
                                            h.[ShowerOnly], 
                                            h.[KitchenSinks], 
                                            h.[MiscSinks],
                                            h.[HasWasher], 
                                            h.[IceMakers], 
                                            h.[IsWasherFrontLoader], 
                                            h.[HasDishwasher], 
                                            h.[DishLoadsPerWeek], 
                                            h.[AdultFamilyMembers], 
                                            h.[Children6AndUnder], 
                                            h.[Children7To12], 
                                            h.[Children13To17], 
                                            h.[MembersOver65],
                                            h.[TypicalUsage],
                                            h.[CustomerUsage],
                                            h.[UsagePercentageDiff],
                                            dt.Description as [DwellingTypeDescription],
                                            apb.Description as [AuditPerformedByDescription]
                                       FROM 
                                            [H2OSurveys] h
                                       LEFT JOIN [DwellingTypes] dt ON dt.DwellingTypeID = h.DwellingTypeID
                                       LEFT JOIN [H2OSurveyAuditPerformedByTypes] apb ON apb.H2OSurveyAuditPerformedByTypeID = h.H2OSurveyAuditPerformedByTypeID
                                       WHERE
                                            ([H2OSurveyID] = @H2OSurveyID)">
                            <SelectParameters>
                                <asp:ControlParameter Name="H2OSurveyID" DbType="Int32" ControlID="detailView" PropertyName="SelectedValue" />
                            </SelectParameters>
                            <UpdateParameters>
                                <asp:ControlParameter Name="H2OSurveyID" DbType="Int32" ControlID="detailView" PropertyName="SelectedValue" />
                                <asp:Parameter Name="H2OSurveyAuditPerformedByTypeID" Type="Int32" />
                                <asp:Parameter Name="AuditPerformedDate" Type="DateTime" />
                                <asp:Parameter Name="DoesCustomerWantToParticpate" Type="Boolean" />
                                <asp:Parameter Name="IsHomeOwner" Type="Boolean" />
                                <asp:Parameter Name="DwellingType" Type="Int32" />
                                <asp:Parameter Name="Toilets" Type="Int32" />
                                <asp:Parameter Name="BathroomSinks" Type="Int32" />
                                <asp:Parameter Name="ShowerBathtubs" Type="Int32" />
                                <asp:Parameter Name="ShowerOnly" Type="Int32" />
                                <asp:Parameter Name="KitchenSinks" Type="Int32" />
                                <asp:Parameter Name="MiscSinks" Type="Int32" />
                                <asp:Parameter Name="HasWasher" Type="Boolean" />
                                <asp:Parameter Name="IceMakers" Type="Int32" />
                                <asp:Parameter Name="IsWasherFrontLoader" Type="Boolean" />
                                <asp:Parameter Name="HasDishwasher" Type="Boolean" />
                                <asp:Parameter Name="DishLoadsPerWeek" Type="Int32" />
                                <asp:Parameter Name="AdultFamilyMembers" Type="Int32" />
                                <asp:Parameter Name="Children6AndUnder" Type="Int32" />
                                <asp:Parameter Name="Children7To12" Type="Int32" />
                                <asp:Parameter Name="Children13To17" Type="Int32" />
                                <asp:Parameter Name="MembersOver65" Type="Int32" />
                                <asp:Parameter Name="TypicalUsage" Type="Decimal" />
                                <asp:Parameter Name="CustomerUsage" Type="Decimal" />
                                <asp:Parameter Name="UsagePercentageDiff" Type="Decimal" />
                            </UpdateParameters>
                        </mapcall:McProdDataSource>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </mapcall:Tab>
            <mapcall:Tab runat="server" ID="tabProgramActions" 
                Label="Program Actions"
                VisibleDuringInsert="false" 
                VisibleDuringUpdate="true">

                <%--This script has to be outside the UpdatePanel or else the custom
                validator will just throw null references about being unable to find
                the function. -Ross 9/13/11--%>
                <script type="text/javascript">
                    var validateQualifiedDate = function(source, args) {
                        var radioValue = $('input[name="ctl00$ctl00$ctl00$content$cphMain$cphMain$template$tabView$dvActions$radQualifiesForKit"]:checked').val();
                        if (radioValue != "") { // != "Not Answered"
                            var qualDate = $('#content_cphMain_cphMain_template_tabView_dvActions_txtQualifiesForKitDate').val();
                            args.IsValid = (qualDate != "");
                        }
                    };
                </script>

                <asp:UpdatePanel ID="upActionTab" runat="server" ClientIDMode="Static">
                    <ContentTemplate>
                        <asp:DetailsView ID="dvActions" runat="server" DataSourceID="dsActions" DataKeyNames="H2OSurveyID"
                        AutoGenerateRows="false" DefaultMode="ReadOnly">
                        <Fields>
                            <mapcall:BoundField DataField="NJAWAccountNumber" DataType="NVarChar" ReadOnly="true" />
                            <mapcall:BoundField DataField="FirstName" DataType="NVarChar" ReadOnly="true" />
                            <mapcall:BoundField DataField="LastName" DataType="NVarChar" ReadOnly="true" />
                            <mapcall:BoundField DataField="QualifiesForKit" DataType="Bit">
                                <BooleanBoundFieldOptions ControlType="RadioButtonList" />
                            </mapcall:BoundField>
                            <mapcall:BoundField
                                 DataField="QualifiesForKitDate"
                                 DataType="Date"
                                 HelpText="The date that the Qualifies For Kit field was answered." />
                            <mapcall:BoundField DataField="CustomerWithHighWaterUsage" DataType="Bit">
                                <BooleanBoundFieldOptions ControlType="RadioButtonList" />
                            </mapcall:BoundField>
                            <mapcall:TemplateBoundField HeaderText="Water Savings Kit">
                                <ItemTemplate>
                                    <%--This checkbox is only gonna work because I set the field to non-nullable
                                and will default to false. Otherwise it throws a cast error for null rows--%>
                                    <asp:CheckBox ID="chkKitProvided" runat="server" Text="Was Provided" Checked='<%# Bind("WaterSavingKitProvided") %>'
                                        Enabled="false" /> 
                                    <table class="kitTable">
                                        <tr>
                                            <td class="label">
                                                Qty of Tank Bags:
                                            </td>
                                            <td class="field">
                                                <%# Eval("KitTankBags") %>
                                            </td>
                                            <td class="label">
                                                Qty of Dye Kits:
                                            </td>
                                            <td class="field">
                                                <%# Eval("KitDyeKits") %>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="label">
                                                Qty of Shower Heads:
                                            </td>
                                            <td class="field">
                                                <%# Eval("KitShowerHeads")%>
                                            </td>
                                            <td class="label">
                                                Qty of Teflon Tape:
                                            </td>
                                            <td class="field">
                                                <%# Eval("KitTeflonTape")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="label">
                                                Qty of Kitchen Aerators:
                                            </td>
                                            <td class="field">
                                                <%# Eval("KitKitchenAerator")%>
                                            </td>
                                            <td class="label">
                                                Qty of Bathroom Aerators:
                                            </td>
                                            <td class="field">
                                                <%# Eval("KitBathAerator")%>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkKitProvided" runat="server" Text="Was Provided" Checked='<%# Bind("WaterSavingKitProvided") %>' />
                                    <table class="kitTable">
                                        <tr>
                                            <td class="label">
                                                Qty of Tank Bags
                                            </td>
                                            <td class="field">
                                                <mapcall:NumericTextBox ID="txtQntTankBags" runat="server" Text='<%# Bind("KitTankBags") %>' />
                                            </td>
                                            <td class="label">
                                                Qty of Dye Kits
                                            </td>
                                            <td class="field">
                                                <mapcall:NumericTextBox ID="txtQntDyeKits" runat="server" Text='<%# Bind("KitDyeKits") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="label">
                                                Qty of Shower Heads:
                                            </td>
                                            <td class="field">
                                                <mapcall:NumericTextBox ID="txtQntShowerHeads" runat="server" Text='<%# Bind("KitShowerHeads") %>' />
                                            </td>
                                            <td class="label">
                                                Qty of Teflon Tape:
                                            </td>
                                            <td class="field">
                                                <mapcall:NumericTextBox ID="TextBox2" runat="server" Text='<%# Bind("KitTeflonTape") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="label">
                                                Qty of Kitchen Aerators:
                                            </td>
                                            <td class="field">
                                                <mapcall:NumericTextBox ID="TextBox3" runat="server" Text='<%# Bind("KitKitchenAerator") %>' />
                                            </td>
                                            <td class="label">
                                                Qty of Bathroom Aerators:
                                            </td>
                                            <td class="field">
                                                <mapcall:NumericTextBox ID="TextBox4" runat="server" Text='<%# Bind("KitBathAerator") %>' />
                                            </td>
                                        </tr>
                                    </table>
                                </EditItemTemplate>
                            </mapcall:TemplateBoundField>
                            <mapcall:BoundField DataField="WaterSavingKitProvidedDate" DataType="Date" />
                            <mapcall:BoundField DataField="WaterSavingKitFollowupDate" DataType="Date" />
                            <mapcall:TemplateBoundField HeaderText="Water Saving Kit Followup Result">
                                <ItemTemplate><%# Eval("WaterKitFollowupStatusDescription") %></ItemTemplate>
                                <EditItemTemplate>
                                    <mapcall:DataSourceDropDownList ID="ddlWaterKitFollowupStatus" runat="server" 
                                        TableName="H2OSurveyContactStatusTypes" 
                                        TextFieldName="Description"
                                        ValueFieldName="H2OSurveyContactStatusTypeID" EnableCaching="true" 
                                        SelectedValue='<%# Bind("WaterSavingKitFollowupStatusTypeID") %>'/>
                                </EditItemTemplate>
                            </mapcall:TemplateBoundField>
                            <mapcall:TemplateBoundField HeaderText="Water Saving Kit Followup Outcome">
                                <ItemTemplate><%# Eval("WaterKitFollowupOutcomeDescription") %></ItemTemplate>
                                <EditItemTemplate>
                                    <mapcall:DataSourceDropDownList ID="ddlWaterKitFollowupOutcome" runat="server" 
                                        TableName="H2OSurveyWaterKitFollowUpOutcomeTypes" 
                                        TextFieldName="Description"
                                        ValueFieldName="H2OSurveyWaterKitFollowUpOutcomeTypeID" EnableCaching="true" 
                                        SelectedValue='<%# Bind("H2OSurveyWaterKitFollowUpOutcomeTypeID") %>'/>
                                </EditItemTemplate>
                            </mapcall:TemplateBoundField>
                            <mapcall:BoundField DataField="SmallRepairsCompletedDate" DataType="Date" />
                            <mapcall:BoundField DataField="TechnicianNeededForWaterKitDate" DataType="Date" />
                            <mapcall:TemplateBoundField HeaderText="Site Visit Scheduling Status">
                                <ItemTemplate><%# Eval("SiteVisitScheduleStatusDescription") %></ItemTemplate>
                                <EditItemTemplate>
                                    <mapcall:DataSourceDropDownList ID="ddlSiteVisitScheduleStatus" runat="server" 
                                        TableName="H2OSurveyContactStatusTypes" 
                                        TextFieldName="Description"
                                        ValueFieldName="H2OSurveyContactStatusTypeID" EnableCaching="true" 
                                        SelectedValue='<%# Bind("SiteVisitScheduleStatusTypeID") %>'/>
                                </EditItemTemplate>
                            </mapcall:TemplateBoundField>     
                            <mapcall:TemplateBoundField HeaderText="Site Visit Scheduling Outcome">
                                <ItemTemplate><%# Eval("SiteVisitScheduleOutcomeDescription") %></ItemTemplate>
                                <EditItemTemplate>
                                    <mapcall:DataSourceDropDownList ID="ddlSiteVisitScheduleOutcome" runat="server" 
                                        TableName="H2OSurveySiteVisitSchedulingOutcomeTypes" 
                                        TextFieldName="Description"
                                        ValueFieldName="H2OSurveySiteVisitSchedulingOutcomeTypeID" EnableCaching="true" 
                                        SelectedValue='<%# Bind("H2OSurveySiteVisitSchedulingOutcomeTypeID") %>'/>
                                </EditItemTemplate>
                            </mapcall:TemplateBoundField>                            
                            <mapcall:BoundField DataField="SiteVisitDate" DataType="Date" />
                            <mapcall:BoundField DataField="SiteVisitOutcome" DataType="Text" />
                            <mapcall:BoundField DataField="SiteVisitNumberOfFixtures" DataType="Text" />
                            <mapcall:TemplateBoundField ShowHeader="False">
                                <EditItemTemplate>
                                    <asp:LinkButton ID="btnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                        Text="Update" ClientIDMode="Static" />
                                    <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                        Text="Cancel" />
                                    <asp:HiddenField ID="ugh" runat="server" />
                                    <asp:CustomValidator ID="cust" runat="server" 
                                        ClientValidationFunction="validateQualifiedDate"
                                        ErrorMessage="Customer Qualified Date must be set when Customer Is Qualified is set to either Yes or No" />
                                </EditItemTemplate>
                                <InsertItemTemplate>
                                    <%--Empty so EditItemTemplate doesn't take its place --%>
                                </InsertItemTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server" Visible='<%# Permissions.EditAccess.IsAllowed %>'
                                        CausesValidation="False" CommandName="Edit" Text="Edit" />
                                    <%--Deletes are done from the customer tab only--%>
                                </ItemTemplate>
                            </mapcall:TemplateBoundField>
                        </Fields>
                        </asp:DetailsView>
                        <mapcall:McProdDataSource ID="dsActions" runat="server" CancelSelectOnNullParameter="true"
                            EnableViewState="false" 
                            SelectCommand="SELECT  
                                                h.[NJAWAccountNumber],
                                                h.[FirstName],
                                                h.[LastName],
                                                h.[KitTankBags], 
                                                h.[CustomerWithHighWaterUsage], 
                                                h.[WaterSavingKitProvided], 
                                                h.[WaterSavingKitProvidedDate], 
                                                h.[WaterSavingKitFollowupDate], 
                                                h.[WaterSavingKitFollowupStatusTypeID],
                                                h.[H2OSurveyWaterKitFollowUpOutcomeTypeID],
                                                h.[SiteVisitDate], 
                                                h.[SiteVisitScheduleStatusTypeID],
                                                h.[SiteVisitOutcome],
                                                h.[H2OSurveySiteVisitSchedulingOutcomeTypeID],
                                                h.[SiteVisitNumberOfFixtures],
                                                h.[KitShowerHeads], 
                                                h.[KitDyeKits], 
                                                h.[KitKitchenAerator], 
                                                h.[KitBathAerator], 
                                                h.[KitTeflonTape], 
                                                h.[SmallRepairsCompletedDate], 
                                                h.[TechnicianNeededForWaterKitDate], 
                                                h.[QualifiesForKit], 
                                                h.[QualifiesForKitDate], 
                                                h.[CreatedBy], 
                                                h.[H2OSurveyID],
                                                cStat.[Description] as [WaterKitFollowupStatusDescription],
                                                cStatSchedule.[Description] as [SiteVisitScheduleStatusDescription],
                                                wkFollowup.[Description] as [WaterKitFollowupOutcomeDescription],
                                                siteOutcome.[Description] as [SiteVisitScheduleOutcomeDescription]
                                          FROM
                                                [H2OSurveys] h 
                                          LEFT JOIN [H2OSurveyContactStatusTypes] cStat                       on cStat.[H2OSurveyContactStatusTypeID] = h.[WaterSavingKitFollowupStatusTypeID]
                                          LEFT JOIN [H2OSurveyContactStatusTypes] cStatSchedule               on cStatSchedule.[H2OSurveyContactStatusTypeID] = h.[SiteVisitScheduleStatusTypeID]
                                          LEFT JOIN [H2OSurveyWaterKitFollowUpOutcomeTypes] wkFollowup        on wkFollowup.[H2OSurveyWaterKitFollowUpOutcomeTypeID] = h.[H2OSurveyWaterKitFollowUpOutcomeTypeID]
                                          LEFT JOIN [H2OSurveySiteVisitSchedulingOutcomeTypes] siteOutcome   on siteOutcome.[H2OSurveySiteVisitSchedulingOutcomeTypeID] = h.[H2OSurveySiteVisitSchedulingOutcomeTypeID]
                                          WHERE
                                                (h.[H2OSurveyID] = @H2OSurveyID)"
                            UpdateCommand="UPDATE [H2OSurveys] 
                                              SET [KitTankBags] = @KitTankBags,
                                                  [CustomerWithHighWaterUsage] = @CustomerWithHighWaterUsage, 
                                                  [WaterSavingKitProvided] = @WaterSavingKitProvided, 
                                                  [WaterSavingKitProvidedDate] = @WaterSavingKitProvidedDate, 
                                                  [WaterSavingKitFollowupDate] = @WaterSavingKitFollowupDate, 
                                                  [WaterSavingKitFollowupStatusTypeID] = @WaterSavingKitFollowupStatusTypeID,
                                                  [H2OSurveyWaterKitFollowUpOutcomeTypeID] = @H2OSurveyWaterKitFollowUpOutcomeTypeID,
                                                  [SiteVisitDate] = @SiteVisitDate,
                                                  [SiteVisitScheduleStatusTypeID] = @SiteVisitScheduleStatusTypeID,
                                                  [H2OSurveySiteVisitSchedulingOutcomeTypeID] = @H2OSurveySiteVisitSchedulingOutcomeTypeID,
                                                  [SiteVisitOutcome] = @SiteVisitOutcome,
                                                  [SiteVisitNumberOfFixtures] = @SiteVisitNumberOfFixtures,
                                                  [KitShowerHeads] = @KitShowerHeads,
                                                  [KitDyeKits] = @KitDyeKits, 
                                                  [KitKitchenAerator] = @KitKitchenAerator,
                                                  [KitBathAerator] = @KitBathAerator, 
                                                  [KitTeflonTape] = @KitTeflonTape, 
                                                  [SmallRepairsCompletedDate] = @SmallRepairsCompletedDate, 
                                                  [TechnicianNeededForWaterKitDate] = @TechnicianNeededForWaterKitDate,
                                                  [QualifiesForKit] = @QualifiesForKit, 
                                                  [QualifiesForKitDate] = @QualifiesForKitDate 
                                            WHERE [H2OSurveyID] = @H2OSurveyID">
                            <SelectParameters>
                                <asp:ControlParameter Name="H2OSurveyID" DbType="Int32" ControlID="detailView" PropertyName="SelectedValue" />
                            </SelectParameters>
                            <UpdateParameters>
                                <asp:ControlParameter Name="H2OSurveyID" DbType="Int32" ControlID="detailView" PropertyName="SelectedValue" />
                                <asp:Parameter Name="CustomerWithHighWaterUsage" Type="Boolean" />
                                <asp:Parameter Name="SiteVisitDate" Type="DateTime" />
                                <asp:Parameter Name="WaterSavingKitProvided" Type="Boolean" />
                                <asp:Parameter Name="WaterSavingKitProvidedDate" Type="DateTime" />
                                <asp:Parameter Name="WaterSavingKitFollowupDate" Type="DateTime" />
                                <asp:Parameter Name="WaterSavingKitFollowupStatusTypeID" Type="Int32" />
                                <asp:Parameter Name="H2OSurveyWaterKitFollowUpOutcomeTypeID" Type="Int32" />
                                <asp:Parameter Name="SiteVisitScheduleStatusTypeID" Type="Int32" />
                                <asp:Parameter Name="H2OSurveySiteVisitSchedulingOutcomeTypeID" Type="Int32" />
                                <asp:Parameter Name="KitTankBags" Type="Int32" />
                                <asp:Parameter Name="KitShowerHeads" Type="Int32" />
                                <asp:Parameter Name="KitDyeKits" Type="Int32" />
                                <asp:Parameter Name="KitKitchenAerator" Type="Int32" />
                                <asp:Parameter Name="KitBathAerator" Type="Int32" />
                                <asp:Parameter Name="KitTeflonTape" Type="Int32" />
                                <asp:Parameter Name="SmallRepairsCompletedDate" Type="DateTime" />
                                <asp:Parameter Name="TechnicianNeededForWaterKitDate" Type="DateTime" />
                                <asp:Parameter Name="QualifiesForKit" Type="Boolean" />
                                <asp:Parameter Name="QualifiesForKitDate" Type="DateTime" />
                                <asp:Parameter Name="SiteVisitOutcome" Type="String" />
                                <asp:Parameter Name="SiteVisitNumberOfFixtures" Type="String" />
                            </UpdateParameters>
                        </mapcall:McProdDataSource>
                     </ContentTemplate>
                </asp:UpdatePanel>
            </mapcall:Tab>
        </Tabs>
    </mapcall:DetailsViewDataPageTemplate>
</asp:Content>