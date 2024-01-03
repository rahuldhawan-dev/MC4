Feature: EnvironmentalNonComplianceEventActionItemPage

Background: Things exist
	Given a state "one" exists with name: "New Jersey", abbreviation: "NJ"
	And a state "two" exists with name: "New York", abbreviation: "NY"
	And an operating center "nj4" exists with opcode: "NJ4", name: "Lakewood", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true", state: "one"
	And an operating center "ny1" exists with opcode: "NY1", name: "Lakewood", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true", state: "two"
	And a user "user" exists with username: "user"
	And a role "roleRead" exists with action: "Read", module: "EnvironmentalGeneral", user: "user", operating center: "nj4"
	And a role "roleEdit" exists with action: "Edit", module: "EnvironmentalGeneral", user: "user", operating center: "nj4"
	And a role "roleAdd" exists with action: "Add", module: "EnvironmentalGeneral", user: "user", operating center: "nj4"
	And a role "roleDelete" exists with action: "Delete", module: "EnvironmentalGeneral", user: "user", operating center: "nj4"
	And public water supply statuses exist
	And a water type "one" exists with description: "Water"
	And a public water supply "one" exists with operating area: "AA", identifier: "1111", status: "active", state: "one"
	And operating center: "nj4" exists in public water supply: "one"
	And a waste water system "one" exists
	And a facility "one" exists with operating center: "nj4"
	And a facility "two" exists with operating center: "ny1"
	And environmental non compliance event statuses exist
	And environmental non compliance event types exist
	And environmental non compliance event entity levels exist
	And environmental non compliance event responsibilities exist
	And environmental non compliance event action item types exist

Scenario: user can search and view results
	Given a environmental non compliance event "one" exists with state: "one", operating center: "nj4", public water supply: "one", waste water system: "one", facility: "one", name of entity: "whatevs"
	Given a environmental non compliance event "two" exists with state: "two", operating center: "ny1", public water supply: "one", waste water system: "one", facility: "two", name of entity: "whatevs"
	Given a environmental non compliance event "three" exists with state: "one", operating center: "nj4", public water supply: "one", waste water system: "one", facility: "one", name of entity: "whatevs"
	And a environmental non compliance event action item "one" exists with environmental non compliance event: "one"
	And a environmental non compliance event action item "two" exists with environmental non compliance event: "two"
	And a environmental non compliance event action item "three" exists with environmental non compliance event: "three"
	And I am logged in as "user"
    When I visit the Environmental/EnvironmentalNonComplianceEventActionItem/Search page
	And I select state "one" from the State dropdown
	And I press "Search"
	Then I should be at the Environmental/EnvironmentalNonComplianceEventActionItem page
	Then I should see the link "1" ends with "Environmental/EnvironmentalNonComplianceEvent/Show/1#ActionItemsTab"
	Then I should see the link "3" ends with "Environmental/EnvironmentalNonComplianceEvent/Show/3#ActionItemsTab"

Scenario: user can search completed and non completed action items
	Given a environmental non compliance event "one" exists with state: "one", operating center: "nj4", public water supply: "one", waste water system: "one", facility: "one", name of entity: "whatevs"
	Given a environmental non compliance event "two" exists with state: "two", operating center: "ny1", public water supply: "one", waste water system: "one", facility: "two", name of entity: "whatevs"
	Given a environmental non compliance event "three" exists with state: "one", operating center: "nj4", public water supply: "one", waste water system: "one", facility: "one", name of entity: "whatevs"
	And a environmental non compliance event action item "one" exists with environmental non compliance event: "one", date completed: today
	And a environmental non compliance event action item "two" exists with environmental non compliance event: "two"
	And a environmental non compliance event action item "three" exists with environmental non compliance event: "three"
	And I am logged in as "user"
    When I visit the Environmental/EnvironmentalNonComplianceEventActionItem/Search page
	And I select state "one" from the State dropdown
	And I select "Yes" from the Completed dropdown
	And I press "Search"
	Then I should be at the Environmental/EnvironmentalNonComplianceEventActionItem page
	Then I should see the link "1" ends with "Environmental/EnvironmentalNonComplianceEvent/Show/1#ActionItemsTab"
	When I visit the Environmental/EnvironmentalNonComplianceEventActionItem/Search page
	And I select state "one" from the State dropdown
	And I select "No" from the Completed dropdown
	And I press "Search"
	Then I should be at the Environmental/EnvironmentalNonComplianceEventActionItem page
	Then I should see the link "3" ends with "Environmental/EnvironmentalNonComplianceEvent/Show/3#ActionItemsTab"