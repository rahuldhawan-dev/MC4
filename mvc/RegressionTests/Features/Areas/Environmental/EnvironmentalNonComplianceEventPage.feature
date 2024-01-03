Feature: EnvironmentalNonComplianceEventPage

Background: Things exist
	Given a state "one" exists with name: "New Jersey", abbreviation: "NJ"
	And an operating center "nj4" exists with opcode: "NJ4", name: "Lakewood", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true", state: "one"
	And a user "user" exists with username: "user"
	And a user "testuser" exists with username: "stuff", default operating center: "nj4", full name: "Boaty McBoatface"
	And a role "roleRead" exists with action: "Read", module: "EnvironmentalGeneral", user: "user", operating center: "nj4"
	And a role "roleEdit" exists with action: "Edit", module: "EnvironmentalGeneral", user: "user", operating center: "nj4"
	And a role "roleAdd" exists with action: "Add", module: "EnvironmentalGeneral", user: "user", operating center: "nj4"
	And a role "roleDelete" exists with action: "Delete", module: "EnvironmentalGeneral", user: "user", operating center: "nj4"
	And public water supply statuses exist
	And waste water system statuses exist
	And a water type "one" exists with description: "Water"
	And a water type "wastewater" exists with description: "Waste Water"
	And a public water supply "one" exists with operating area: "AA", identifier: "1111", status: "active", state: "one", system: "System"
	And operating center: "nj4" exists in public water supply: "one"
	And a waste water system "one" exists
	And a waste water system "two" exists with id: 1, waste water system name: "Water System 1", operating center: "nj4", status: "active"
	And a facility "one" exists with operating center: "nj4"
	And environmental non compliance event statuses exist
	And environmental non compliance event types exist
	And environmental non compliance event entity levels exist
	And environmental non compliance event responsibilities exist
	And environmental non compliance event action item types exist
	And environmental non compliance event root causes exist
	And environmental non compliance event counts against targets exist
	And a environmental non compliance event failure type "one" exists

Scenario: user can see waste water system dropdown description
	Given I am logged in as "user"
	When I visit the Environmental/EnvironmentalNonComplianceEvent/Search page
	And I select state "one" from the State dropdown
	And I wait for ajax to finish loading
	And I select operating center "nj4" from the OperatingCenter dropdown
	And I wait for ajax to finish loading	
	Then I should see "NJ4WW0002 - Water System 1" in the WasteWaterSystem dropdown

Scenario: user can view a environmental non compliance event
	Given a environmental non compliance event "one" exists with state: "one", operating center: "nj4", public water supply: "one", waste water system: "one", name of entity: "whatevs"
	And I am logged in as "user"
	When I visit the Show page for environmental non compliance event: "one"
	Then I should see a display for NameOfEntity with "whatevs"
	
Scenario: user should see a bunch of validation creating a new environmental non compliance event
	Given I am logged in as "user"
	And I am at the Environmental/EnvironmentalNonComplianceEvent/New page
	When I press Save
	Then I should be at the Environmental/EnvironmentalNonComplianceEvent/New page
	And I should see a validation message for State with "The State field is required."
	When I select state "one" from the State dropdown
	And I wait for ajax to finish loading
	And I press Save
	Then I should be at the Environmental/EnvironmentalNonComplianceEvent/New page
	And I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
	When I select operating center "nj4" from the OperatingCenter dropdown
	And I wait for ajax to finish loading
	Then I should see "NJ4WW0002 - Water System 1" in the WasteWaterSystem dropdown
	When I press Save
	Then I should be at the Environmental/EnvironmentalNonComplianceEvent/New page
	And I should see a validation message for WaterType with "The WaterType field is required."
	When I select water type "one" from the WaterType dropdown
	And I wait for ajax to finish loading
	And I press Save
	Then I should be at the Environmental/EnvironmentalNonComplianceEvent/New page
	And I should see a validation message for PublicWaterSupply with "The PublicWaterSupply field is required."
	And I should not see the WasteWaterSystem field 
	When I select public water supply "one" from the PublicWaterSupply dropdown
	And I press Save
	And I wait for ajax to finish loading
	Then I should be at the Environmental/EnvironmentalNonComplianceEvent/New page
	And I should see a validation message for EventDate with "The EventDate field is required."
	And I should see a validation message for AwarenessDate with "The AwarenessDate field is required."
	And I should see a validation message for IssueStatus with "The IssueStatus field is required."
	And I should see a validation message for IssueType with "The IssueType field is required."
	When I select "Confirmed" from the IssueStatus dropdown 
	And I press Save
	Then I should see a validation message for IssuingEntity with "The IssuingEntity field is required."
	And I should see a validation message for SummaryOfEvent with "The SummaryOfEvent field is required."
	And I should see a validation message for RootCauses with "The Root Cause field is required."
	When I enter 1/1/2020 into the EventDate field
	And I enter today's date into the AwarenessDate field
	And I select environmental non compliance event status "pending" from the IssueStatus dropdown
	And I select environmental non compliance event type "environmental" from the IssueType dropdown
	And I select environmental non compliance event entity level "state" from the IssuingEntity dropdown
	And I select "Lack of SOP" from the RootCauses multiselect
	And I select "TBD" from the RootCauses multiselect
	And I enter "foobar" into the SummaryOfEvent field
	And I enter 1/1/2020 into the EventDate field
	And I enter 1.232 into the FineAmount field 
	And I press Save
	Then I should see a validation message for FineAmount with "Decimal must be no larger than the hundredths place ex. 500.00"
	When I enter 1.23 into the FineAmount field
	And I press Save 
    Then the currently shown environmental non compliance event shall henceforth be known throughout the land as "meh"
    And I should see a display for State with state "one"
    And I should see a display for OperatingCenter with operating center "nj4"
    And I should see a display for PublicWaterSupply with public water supply "one"
    And I should see a display for EventDate with "1/1/2020 12:00:00 AM"
	And I should see a display for IssueYear with "2020"
    And I should see a display for AwarenessDate with today's date
    And I should see a display for IssueStatus with environmental non compliance event status "pending"
    And I should see a display for IssueType with environmental non compliance event type "environmental"
    And I should see a display for IssuingEntity with environmental non compliance event entity level "state"
	And I should see a display for Responsibility with environmental non compliance event responsibility "tbd"
	And I should see a display for DisplayRootCause with "Lack of SOP, TBD"
	And I should see a display for SummaryOfEvent with "foobar"
	And I should see a display for NOVWorkGroupReviewDate with ""
	And I should see a display for ChiefEnvOfficerApprovalDate with ""

Scenario: user can edit a environmental non compliance event
	Given an environmental non compliance event "one" exists with state: "one", operating center: "nj4", public water supply: "one", facility: "one", name of entity: "whatevs"
	And I am logged in as "user"
	When I visit the Edit page for environmental non compliance event: "one"
	Then I should not see the EventDate field
	And I should see a display for EventDate with today's date
	And I should not see the AwarenessDate field
	And I should see a display for AwarenessDate with today's date
	And I should see the SummaryOfEvent field	
	When I enter "foobar" into the NameOfEntity field
	And I select "TBD" from the RootCauses multiselect
	And I select "Lack of SOP" from the RootCauses multiselect
	And I select environmental non compliance event responsibility "third party nov" from the Responsibility dropdown
	And I select "Expected to Count" from the CountsAgainstTarget dropdown
	And I press Save
	Then I should see a validation message for NameOfThirdParty with "The Name Of Third Party field is required."	
	When I enter "Jabberwocky" into the NameOfThirdParty field
	And I enter "1/1/2021" into the DateOfEnvironmentalLeadershipTeamReview field
	And I press Save
	And I wait for the page to reload
	Then I should be at the Show page for environmental non compliance event "one"
	And I should see a display for NameOfEntity with "foobar"
	And I should see a display for Responsibility with environmental non compliance event responsibility "third party nov"
	And I should see a display for NameOfThirdParty with "Jabberwocky"
	And I should see a display for CountsAgainstTarget with "Expected to Count"

Scenario: user can see waste water system dropdown description while editing
	Given a environmental non compliance event "one" exists with state: "one", operating center: "nj4", public water supply: "one", facility: "one", name of entity: "whatevs"
	And I am logged in as "user"
	When I visit the Edit page for environmental non compliance event: "one"
	And I select state "one" from the State dropdown
	And I wait for ajax to finish loading
	And I select operating center "nj4" from the OperatingCenter dropdown
	And I wait for ajax to finish loading	
	Then I should see "NJ4WW0002 - Water System 1" in the WasteWaterSystem dropdown

Scenario: user can add, edit and delete action items for environmental non compliance event
	Given a environmental non compliance event "one" exists with state: "one", operating center: "nj4", public water supply: "one", waste water system: "one", facility: "one", name of entity: "whatevs"
	And I am logged in as "user"
    When I visit the Show page for environmental non compliance event: "one"
	And I click the "Action Items" tab
    And I press "Add New Action Item"
	And I press "Save Action Item"
	Then I should see the validation message The Type field is required.
	And I should see the validation message The ActionItem field is required.
	And I should see the validation message The TargetedCompletionDate field is required.
	And I should see the validation message The ResponsibleOwner field is required.
	When I select environmental non compliance event action item type "capital improvement" from the Type dropdown
	And I type "foobar" into the ActionItem field
	And I enter today's date into the TargetedCompletionDate field
	And I select user "testuser"'s FullName from the ResponsibleOwner dropdown
	And I press "Save Action Item"
	And I wait for the page to reload
	Then I should be at the Show page for environmental non compliance event "one"
	And I should see a link to the /Environmental/EnvironmentalNonComplianceEventActionItem/Edit/1 page
	When I visit the /Environmental/EnvironmentalNonComplianceEventActionItem/Edit/1 page
	And I type "baz" into the ActionItem field
	And I select "-- Select --" from the ResponsibleOwner dropdown
	And I press Save
	Then I should see the validation message The ResponsibleOwner field is required.
	When I select user "testuser"'s FullName from the ResponsibleOwner dropdown
	And I press Save
	And I wait for the page to reload
	Then I should be at the Show page for environmental non compliance event "one"
	When I click the "Action Items" tab
	Then I should see "baz" in the table actionItemsTable's "Action Item" column
	And I should see "Boaty McBoatface" in the table actionItemsTable's "Responsible Owner" column
	And I should not see the link "Remove"

Scenario: User can Search for a Environmental Noncompliance Event
	Given I am logged in as "user"
	And a environmental non compliance event "one" exists with issue year: 2021, name of third party: "Flip Liquid", event date: "1/1/2021", date finalized: "1/1/2021", state: "one", operating center: "nj4", public water supply: "one", waste water system: "one", facility: "one", name of entity: "whatevs"
	And I am logged in as "user"
	When I visit the /Environmental/EnvironmentalNonComplianceEvent/Edit/1 page
	And I select "Yes" from the CountsAgainstTarget dropdown
	And I select water type "wastewater" from the WaterType dropdown
	And I select environmental non compliance event failure type "one" from the FailureType dropdown
	And I select "Failure to follow SOP" from the RootCauses multiselect
	And I enter "My failure type description" into the FailureTypeDescription field
	And I select waste water system "two" from the WasteWaterSystem dropdown
	And I press Save
	And I visit the Environmental/EnvironmentalNonComplianceEvent/Search page
	Then I should see "New Acquisition NOV" in the Responsibility dropdown
	And I should not see "New Acquisition NOV" in the IssueStatus dropdown
	When I enter "2021" into the IssueYear field
	And I select "Yes" from the CountsAgainstTarget dropdown
	And I press Search
	Then I should see a link to the Show page for environmental non compliance event: "one"
	And I should see the following values in the environmentalNonComplianceEventTable table
	| State | Operating Center | Public Water Supply  | Wastewater System | Facility | Event Date           | Issue Year | Awareness Date | Date NOV Issued      | Issue Status                            | Issue Type                            | Issue Sub Type | Issuing Entity                               | Name Of Entity | Name Of Third Party | Root Cause            | Failure Type |
	| NJ    | NJ4 - Lakewood   | *1111 - AA - System* | *NJ4WW00*          | *        | 1/1/2021 12:00:00 AM | 2021       | *              | 1/1/2021 12:00:00 AM | *EnvironmentalNonComplianceEventStatus* | *EnvironmentalNonComplianceEventType* |                | *EnvironmentalNonComplianceEventEntityLevel* | whatevs        | Flip Liquid         | Failure to follow SOP |*Environmental Non Compliance Event Failure Type*|

Scenario: User can Search for Environmental Noncompliance Event with Date Of Environmental Leadership Team Review
	Given I am logged in as "user"
	And a environmental non compliance event "one" exists with issue year: 2021, name of third party: "Flip Liquid", event date: "1/1/2021", date finalized: "1/1/2021", state: "one", operating center: "nj4", public water supply: "one", waste water system: "one", facility: "one", name of entity: "whatevs", date of environmental leadership team review: "1/1/2021"
	And I am logged in as "user"
	And I am at the Environmental/EnvironmentalNonComplianceEvent/Search page
	When I enter "1/1/2021" into the DateOfEnvironmentalLeadershipTeamReview_Start field
	And I enter "1/1/2021" into the DateOfEnvironmentalLeadershipTeamReview_End field
	And I press Search
	Then I should see a link to the Show page for environmental non compliance event: "one"
	And I should see the following values in the environmentalNonComplianceEventTable table
	| State | Operating Center | Public Water Supply  | Wastewater System | Facility            | Event Date           | Issue Year | Awareness Date | Date NOV Issued      | Issue Status                            | Issue Type                            | Issue Sub Type | Issuing Entity                               | Name Of Entity | Name Of Third Party | Root Cause | Failure Type | Created At | Created By
	| NJ    | NJ4 - Lakewood   | *1111 - AA - System* | *NJ7WW00*          | *Facility 0 - NJ4-* | 1/1/2021 12:00:00 AM | 2021       | *              | 1/1/2021 12:00:00 AM | *EnvironmentalNonComplianceEventStatus* | *EnvironmentalNonComplianceEventType* |                | *EnvironmentalNonComplianceEventEntityLevel* | whatevs        | Flip Liquid         |            |              |	*	       | *	