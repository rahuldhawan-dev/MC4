Feature: InterconnectionPage
	In order to manage and view interconnections
	As a user
	I want to be able to view interconnections and related data

Background: being and nothingness
	Given an admin user "admin" exists with username: "admin"
	And a user "user" exists with username: "user"
	And a user "mcuser" exists with username: "mcuser"
	And a role "roleRead" exists with action: "Read", module: "ProductionFacilities", user: "user"
	And a role "roleRead2" exists with action: "Read", module: "ProductionFacilities", user: "mcuser"
	And a role "roleEdit" exists with action: "Edit", module: "ProductionFacilities", user: "user"
	And a role "roleAdd" exists with action: "Add", module: "ProductionFacilities", user: "user"
	And a role "roleDelete" exists with action: "Delete", module: "ProductionFacilities", user: "user"
	And a role "role1Read" exists with action: "Read", module: "ProductionInterconnections", user: "user"
	And a role "role1Read2" exists with action: "Read", module: "ProductionInterconnections", user: "mcuser"
	And a role "role1Edit" exists with action: "Edit", module: "ProductionInterconnections", user: "user"
	And a role "role1Add" exists with action: "Add", module: "ProductionInterconnections", user: "user"
	And a role "role1Delete" exists with action: "Delete", module: "ProductionInterconnections", user: "user"
	And an operating center "nj7" exists with opcode: "NJ7"
	And an operating center "nj4" exists with opcode: "NJ4", is active: false
	And a facility "facility" exists with operating center: "nj7", facility id: "NJSB-1", facility name: "A Facility"
	And a facility "two" exists with operating center: "nj7", facility id: "NJSB-2", facility name: "OtherFacility"
	And an interconnection "one" exists with dep designation: "The Des", facility: "facility"
	And a meter profile "one" exists with profile name: "profile one"
	And a meter "one" exists with serial number: "8675309", profile: "one"

Scenario: user can view an interconnection and add a meter
	Given I am logged in as "user"
	When I visit the Show page for interconnection "one"
	Then I should see a display for DEPDesignation with "The Des"
	When I click the "Meters" tab
	And I press "Add Meter"
	And I select meter profile "one" from the MeterProfile dropdown
	And I wait for ajax to finish loading
	And I select meter "one"'s Description from the Meter dropdown
	And I press "Add"
	And I wait for the page to reload
	When I click the "Meters" tab
	Then I should see "8675309"		

Scenario: user can see the interconnections page
	Given I am logged in as "user"
	When I visit the Facilities/Interconnection/Search page
	Then I should see "Interconnection Details"
	When I press Search
	And I wait for the page to reload
	Then I should not see "No results matched your query."

Scenario: user can view the new page
	Given I am logged in as "user"
	When I visit the Facilities/Interconnection/New page
	Then I should see "Creating"
	And I should not see operating center "nj4" in the OperatingCenter dropdown

Scenario: user can view the edit page
	Given I am logged in as "user"
	And I am at the Show page for interconnection: "one"
	When I follow the edit link for interconnection "one"
	Then I should see "The Des" in the DEPDesignation field
	And I should see operating center "nj4" in the OperatingCenter dropdown

Scenario: mcuser cannot add meters
	Given a meter "two" exists with serial number: "626"
	And meter: "two" exists in interconnection: "one"
	And I am logged in as "mcuser"
	And I am at the Show page for interconnection: "one"
	When I click the "Meters" tab
	Then I should see a display for DEPDesignation with "The Des"
	And I should not see the button "Remove"

Scenario: user can search for an interconnection by facility
	Given an interconnection "two" exists with facility: "two"
	And I am logged in as "user"
	When I visit the Facilities/Interconnection/Search page
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I wait for ajax to finish loading
	And I select "A Facility - NJ7-1" from the Facility dropdown
	And I press Search
	Then I should see "The Des" in the table interconnectionsTable's "DEP Designation" column
	When I follow the Show link for interconnection "one"
	Then I should see a display for DEPDesignation with "The Des"
	And I should see a display for Facility_FacilityName with "A Facility"
	When I follow the Show link for facility "facility"
	Then I should see a display for FacilityName with "A Facility"
