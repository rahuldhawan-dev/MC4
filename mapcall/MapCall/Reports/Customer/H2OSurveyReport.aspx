<%@ Page Title="H2O Survey Reports" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="H2OSurveyReport.aspx.cs" Inherits="MapCall.Reports.Customer.H2OSurveyReport" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls" TagPrefix="mapcall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.SearchFields" TagPrefix="search" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Reports.Customer" TagPrefix="mapcall" %>
<%@ Register Assembly="MMSINC.Core.WebForms" Namespace="MMSINC.DataPages" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/DetailsViewDataPageTemplate.ascx" TagPrefix="mapcall"
    TagName="DetailsViewDataPageTemplate" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
H2O Survey Reports
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">
<mapcall:DetailsViewDataPageTemplate ID="template" runat="server"
        DefaultPageMode="Search"
        DataElementTableName="H2OSurveys"
        DataElementPrimaryFieldName="H2OSurveyID" 
        IsReadOnlyPage="true"
        Label="H2O Survey Report">

        <SearchBox>
            <Fields>
                <search:DateTimeSearchField ID="sfEnrollmentDate" Label="Week" DataFieldName="EnrollmentDate" ShowTime="false" />
            </Fields>
        </SearchBox>

       <%-- Disable this because we're gonna customize it.  --%>
        <ResultsGridView Visible="false" Enabled="false" />
        
      
        <ResultsPlaceHolder>
            <mapcall:TabView ID="reportTabs" runat="server">
                <mapcall:Tab ID="tabLettersSent" runat="server" Label="Customers Enrolled">
                    <mapcall:H2OSurveyReportResult ID="repLettersSent" runat="server" AutoGenerateColumns="false"
                     SelectCommand="SELECT
                        h.*,
                        cstat.Description as [ContactStatusDescription],
                        states.Abbreviation as StateText,
                        njtown.Town as CityText,
                        njtownsect.Name as TownSectionText,
                        njstreet.FullStName as StreetText
                    FROM 
                        [H2OSurveys] h
                    LEFT JOIN [H2OSurveyContactStatusTypes] cstat on cstat.H2OSurveyContactStatusTypeID = h.H2OSurveyContactStatusTypeID
                    LEFT JOIN [States] states ON states.StateID = h.StateID
                    LEFT JOIN Towns njtown ON njtown.TownID = h.CityID
                    LEFT JOIN TownSections njtownsect ON njtownsect.TownSectionID = h.TownSectionID
                    LEFT JOIN Streets njstreet ON njstreet.StreetID = h.StreetID">
                        <Columns>
                            <mapcall:TemplateBoundField HeaderText="">
                                <ItemTemplate>
                                    <mmsi:DataPageViewRecordLink ID="dLink" runat="server" 
                                     DataRecordId='<%# Bind("H2OSurveyID") %>' 
                                     LinkUrl="~/Modules/Customer/H2OSurveys.aspx" LinkText="View" />
                                </ItemTemplate>
                            </mapcall:TemplateBoundField>
                            <mapcall:BoundField DataField="H2OSurveyID" HeaderText="H2OSurveyID" />
                            <mapcall:BoundField DataField="NJAWAccountNumber" />
                            <mapcall:BoundField DataField="FirstName" />
                            <mapcall:BoundField DataField="LastName" />
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

                            <mapcall:BoundField DataField="HomePhone" />
                            <mapcall:BoundField DataField="CreatedBy" />
                            <mapcall:BoundField DataField="EnrollmentDate" />
                        </Columns>
                    </mapcall:H2OSurveyReportResult>
                   
                </mapcall:Tab>
                <mapcall:Tab runat="server" Label="Renters Audited">
                    <mapcall:H2OSurveyReportResult ID="repRenters" runat="server" AutoGenerateColumns="false"
                        SelectCommand="SELECT
                            h.*,
                            cstat.Description as [ContactStatusDescription],
                            states.Abbreviation as StateText,
                            njtown.Town as CityText,
                            njtownsect.Name as TownSectionText,
                            njstreet.FullStName as StreetText
                        FROM 
                            [H2OSurveys] h
                        LEFT JOIN [H2OSurveyContactStatusTypes] cstat on cstat.H2OSurveyContactStatusTypeID = h.H2OSurveyContactStatusTypeID
                        LEFT JOIN [States] states ON states.StateID = h.StateID
                        LEFT JOIN Towns njtown ON njtown.TownID = h.CityID
                        LEFT JOIN TownSections njtownsect ON njtownsect.TownSectionID = h.TownSectionID
                        LEFT JOIN Streets njstreet ON njstreet.StreetID = h.StreetID">
                        <Columns>
                            <mapcall:TemplateBoundField HeaderText="">
                                <ItemTemplate>
                                    <mmsi:DataPageViewRecordLink ID="dLink" runat="server" 
                                     DataRecordId='<%# Bind("H2OSurveyID") %>' 
                                     LinkUrl="~/Modules/Customer/H2OSurveys.aspx" LinkText="View" />
                                </ItemTemplate>
                            </mapcall:TemplateBoundField>
                            <mapcall:BoundField DataField="H2OSurveyID" HeaderText="H2OSurveyID" />
                            <mapcall:BoundField DataField="NJAWAccountNumber" />
                            <mapcall:BoundField DataField="FirstName" />
                            <mapcall:BoundField DataField="LastName" />
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
                            <mapcall:BoundField DataField="LastContactDate" HeaderText="Date Called" DataType="Date" />

                        </Columns>
                    </mapcall:H2OSurveyReportResult>
                </mapcall:Tab>
                <mapcall:Tab runat="server" Label="Homeowners Audited">
                    <mapcall:H2OSurveyReportResult ID="repAudited" runat="server" AutoGenerateColumns="false"
                        SelectCommand="SELECT
                            h.*,
                            cstat.Description as [ContactStatusDescription],
                            states.Abbreviation as StateText,
                            njtown.Town as CityText,
                            njtownsect.Name as TownSectionText,
                            njstreet.FullStName as StreetText
                        FROM 
                            [H2OSurveys] h
                        LEFT JOIN [H2OSurveyContactStatusTypes] cstat on cstat.H2OSurveyContactStatusTypeID = h.H2OSurveyContactStatusTypeID
                        LEFT JOIN [States] states ON states.StateID = h.StateID
                        LEFT JOIN Towns njtown ON njtown.TownID = h.CityID
                        LEFT JOIN TownSections njtownsect ON njtownsect.TownSectionID = h.TownSectionID
                        LEFT JOIN Streets njstreet ON njstreet.StreetID = h.StreetID">
                        <Columns>
                            <mapcall:TemplateBoundField HeaderText="">
                                <ItemTemplate>
                                    <mmsi:DataPageViewRecordLink ID="dLink" runat="server" 
                                     DataRecordId='<%# Bind("H2OSurveyID") %>' 
                                     LinkUrl="~/Modules/Customer/H2OSurveys.aspx" LinkText="View" />
                                </ItemTemplate>
                            </mapcall:TemplateBoundField>
                            <mapcall:BoundField DataField="H2OSurveyID" HeaderText="H2OSurveyID" />
                            <mapcall:BoundField DataField="NJAWAccountNumber" />
                            <mapcall:BoundField DataField="FirstName" />
                            <mapcall:BoundField DataField="LastName" />
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
                            <mapcall:BoundField DataField="AuditPerformedDate" DataType="Date" />

                        </Columns>
                    </mapcall:H2OSurveyReportResult>
                </mapcall:Tab>
                <mapcall:Tab runat="server" Label="Kits Sent">
                    <strong>Total fixtures</strong>
                    <mapcall:H2OSurveyReportResult ID="repKitSentTotals" runat="server" AutoGenerateColumns="false" ShowRecordCount="false"
                        SelectCommand="SELECT
                            SUM(KitTankBags) as KitTankBags,
                            SUM(KitDyeKits) as KitDyeKits,
                            SUM(KitShowerHeads) as KitShowerHeads,
                            SUM(KitTeflonTape) as KitTeflonTape,
                            SUM(KitKitchenAerator) as KitKitchenAerator,
                            SUM(KitBathAerator) as KitBathAerator
                        FROM 
                            [H2OSurveys] h
                        ">
                        <Columns>
                            <mapcall:BoundField DataField="KitTankBags" />
                            <mapcall:BoundField DataField="KitDyeKits" />
                            <mapcall:BoundField DataField="KitShowerHeads" />
                            <mapcall:BoundField DataField="KitTeflonTape" />
                            <mapcall:BoundField DataField="KitKitchenAerator" />
                            <mapcall:BoundField DataField="KitBathAerator" />
                        </Columns>
                    </mapcall:H2OSurveyReportResult>
                    
                    <div style="margin-top:6px;">
                    <mapcall:H2OSurveyReportResult ID="repKitSent" runat="server" AutoGenerateColumns="false"
                        SelectCommand="SELECT
                            h.*,
                            cstat.Description as [ContactStatusDescription],
                            states.Abbreviation as StateText,
                            njtown.Town as CityText,
                            njtownsect.Name as TownSectionText,
                            njstreet.FullStName as StreetText
                        FROM 
                            [H2OSurveys] h
                        LEFT JOIN [H2OSurveyContactStatusTypes] cstat on cstat.H2OSurveyContactStatusTypeID = h.H2OSurveyContactStatusTypeID
                        LEFT JOIN [States] states ON states.StateID = h.StateID
                        LEFT JOIN Towns njtown ON njtown.TownID = h.CityID
                        LEFT JOIN TownSections njtownsect ON njtownsect.TownSectionID = h.TownSectionID
                        LEFT JOIN Streets njstreet ON njstreet.StreetID = h.StreetID">
                        <Columns>
                            <mapcall:TemplateBoundField HeaderText="">
                                <ItemTemplate>
                                    <mmsi:DataPageViewRecordLink ID="dLink" runat="server" 
                                     DataRecordId='<%# Bind("H2OSurveyID") %>' 
                                     LinkUrl="~/Modules/Customer/H2OSurveys.aspx" LinkText="View" />
                                </ItemTemplate>
                            </mapcall:TemplateBoundField>
                            <mapcall:BoundField DataField="H2OSurveyID" HeaderText="H2OSurveyID" />
                            <mapcall:BoundField DataField="NJAWAccountNumber" />
                            <mapcall:BoundField DataField="FirstName" />
                            <mapcall:BoundField DataField="LastName" />
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
                            <mapcall:BoundField DataField="WaterSavingKitProvidedDate" DataType="Date"  />
                            <mapcall:BoundField DataField="KitTankBags" />
                            <mapcall:BoundField DataField="KitDyeKits" />
                            <mapcall:BoundField DataField="KitShowerHeads" />
                            <mapcall:BoundField DataField="KitTeflonTape" />
                            <mapcall:BoundField DataField="KitKitchenAerator" />
                            <mapcall:BoundField DataField="KitBathAerator" />
                        </Columns>
                    </mapcall:H2OSurveyReportResult>
                    </div>
                </mapcall:Tab>
                <mapcall:Tab runat="server" Label="Average Use Customers">
                    <mapcall:H2OSurveyReportResult ID="repAverageUserCustomers" runat="server" AutoGenerateColumns="false"
                        SelectCommand="SELECT
                            h.*,
                            cstat.Description as [ContactStatusDescription],
                            dt.Description as [DwellingTypeDescription],
                            states.Abbreviation as StateText,
                            njtown.Town as CityText,
                            njtownsect.Name as TownSectionText,
                            njstreet.FullStName as StreetText
                        FROM 
                            [H2OSurveys] h
                        LEFT JOIN [H2OSurveyContactStatusTypes] cstat on cstat.H2OSurveyContactStatusTypeID = h.H2OSurveyContactStatusTypeID
                        LEFT JOIN [States] states ON states.StateID = h.StateID
                        LEFT JOIN Towns njtown ON njtown.TownID = h.CityID
                        LEFT JOIN TownSections njtownsect ON njtownsect.TownSectionID = h.TownSectionID
                        LEFT JOIN Streets njstreet ON njstreet.StreetID = h.StreetID
                        LEFT JOIN [DwellingTypes] dt ON dt.DwellingTypeID = h.DwellingTypeID">
                        <Columns>
                            <mapcall:TemplateBoundField HeaderText="">
                                <ItemTemplate>
                                    <mmsi:DataPageViewRecordLink ID="dLink" runat="server" 
                                     DataRecordId='<%# Bind("H2OSurveyID") %>' 
                                     LinkUrl="~/Modules/Customer/H2OSurveys.aspx" LinkText="View" />
                                </ItemTemplate>
                            </mapcall:TemplateBoundField>
                            <mapcall:BoundField DataField="H2OSurveyID" HeaderText="H2OSurveyID" />
                            <mapcall:BoundField DataField="NJAWAccountNumber" />
                            <mapcall:BoundField DataField="FirstName" />
                            <mapcall:BoundField DataField="LastName" />
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
                            <mapcall:BoundField DataField="HomePhone" />
                            <mapcall:BoundField DataField="ContactStatusDescription" />
                            <mapcall:BoundField DataField="EnrollmentDate" DataType="Date" />
                            <mapcall:BoundField DataField="CreatedBy" />
                            <mapcall:BoundField DataField="CreatedOn" />
                        </Columns>
                    </mapcall:H2OSurveyReportResult>
                </mapcall:Tab>
                
                <mapcall:Tab runat="server" Label="Site Visits">
                    <mapcall:H2OSurveyReportResult ID="repSiteVisits" runat="server" AutoGenerateColumns="false"
                        SelectCommand="SELECT
                            h.*,
                            states.Abbreviation as StateText,
                            njtown.Town as CityText,
                            njtownsect.Name as TownSectionText,
                            njstreet.FullStName as StreetText
                        FROM 
                            [H2OSurveys] h
                        LEFT JOIN [States] states ON states.StateID = h.StateID
                        LEFT JOIN Towns njtown ON njtown.TownID = h.CityID
                        LEFT JOIN TownSections njtownsect ON njtownsect.TownSectionID = h.TownSectionID
                        LEFT JOIN Streets njstreet ON njstreet.StreetID = h.StreetID">
                        <Columns>
                            <mapcall:TemplateBoundField HeaderText="">
                                <ItemTemplate>
                                    <mmsi:DataPageViewRecordLink ID="dLink" runat="server" 
                                     DataRecordId='<%# Bind("H2OSurveyID") %>' 
                                     LinkUrl="~/Modules/Customer/H2OSurveys.aspx" LinkText="View" />
                                </ItemTemplate>
                            </mapcall:TemplateBoundField>
                            <mapcall:BoundField DataField="H2OSurveyID" HeaderText="H2OSurveyID" />
                            <mapcall:BoundField DataField="NJAWAccountNumber" />
                            <mapcall:BoundField DataField="FirstName" />
                            <mapcall:BoundField DataField="LastName" />
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
                            <mapcall:BoundField DataField="SiteVisitOutcome" DataType="Text" />
                            <mapcall:BoundField DataField="SiteVisitNumberOfFixtures" DataType="Text" />
                            <mapcall:BoundField DataField="SiteVisitDate" DataType="Date" />
                        </Columns>
                    </mapcall:H2OSurveyReportResult>
                </mapcall:Tab>
                
                <mapcall:Tab runat="server" Label="High Water Usage Customers">
                    <mapcall:H2OSurveyReportResult ID="repHighUsageCustomers" runat="server" AutoGenerateColumns="false"
                        SelectCommand="SELECT
                            h.*,
                            cstat.Description as [FollowUpStatusDescription],
                            cOutcome.Description as [OutcomeDescription],
                            states.Abbreviation as StateText,
                            njtown.Town as CityText,
                            njtownsect.Name as TownSectionText,
                            njstreet.FullStName as StreetText
                        FROM 
                            [H2OSurveys] h
                        LEFT JOIN [H2OSurveyContactStatusTypes] cstat on cstat.H2OSurveyContactStatusTypeID = h.WaterSavingKitFollowupStatusTypeID
                        LEFT JOIN [H2OSurveyWaterKitFollowUpOutcomeTypes] cOutcome on cOutcome.H2OSurveyWaterKitFollowUpOutcomeTypeID = h.H2OSurveyWaterKitFollowUpOutcomeTypeID
                        LEFT JOIN [States] states ON states.StateID = h.StateID
                        LEFT JOIN Towns njtown ON njtown.TownID = h.CityID
                        LEFT JOIN TownSections njtownsect ON njtownsect.TownSectionID = h.TownSectionID
                        LEFT JOIN Streets njstreet ON njstreet.StreetID = h.StreetID">
                        <Columns>
                            <mapcall:TemplateBoundField HeaderText="">
                                <ItemTemplate>
                                    <mmsi:DataPageViewRecordLink ID="dLink" runat="server" 
                                     DataRecordId='<%# Bind("H2OSurveyID") %>' 
                                     LinkUrl="~/Modules/Customer/H2OSurveys.aspx" LinkText="View" />
                                </ItemTemplate>
                            </mapcall:TemplateBoundField>
                            <mapcall:BoundField DataField="H2OSurveyID" HeaderText="H2OSurveyID" />
                            <mapcall:BoundField DataField="NJAWAccountNumber" />
                            <mapcall:BoundField DataField="FirstName" />
                            <mapcall:BoundField DataField="LastName" />
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
                            <mapcall:BoundField DataField="FollowUpStatusDescription" />
                            <mapcall:BoundField DataField="OutcomeDescription" />
                            <mapcall:BoundField DataField="KitTankBags" />
                            <mapcall:BoundField DataField="KitDyeKits" />
                            <mapcall:BoundField DataField="KitShowerHeads" />
                            <mapcall:BoundField DataField="KitTeflonTape" />
                            <mapcall:BoundField DataField="KitKitchenAerator" />
                            <mapcall:BoundField DataField="KitBathAerator" />
                        </Columns>
                    </mapcall:H2OSurveyReportResult>
                </mapcall:Tab>

            </mapcall:TabView>
        </ResultsPlaceHolder>

</mapcall:DetailsViewDataPageTemplate>
</asp:Content>
