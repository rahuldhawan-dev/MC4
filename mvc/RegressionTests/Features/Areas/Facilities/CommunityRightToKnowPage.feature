Feature: CommunityRightToKnowPage

Background: users and supporting data exist
	Given a user "user" exists with username: "user"
	And an admin user "admin" exists with username: "admin"
	And a state "nj" exists
	And a town "one" exists with state: "nj"
	And an operating center "nj4" exists with opcode: "NJ4", name: "Lakewood", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true", state: "nj"
	And a role "roleRead" exists with action: "Read", module: "ProductionFacilities", user: "user", operating center: "nj4"
	And a role "roleEdit" exists with action: "Edit", module: "ProductionFacilities", user: "user", operating center: "nj4"
	And a role "roleAdd" exists with action: "Add", module: "ProductionFacilities", user: "user", operating center: "nj4"
	And a role "roleDelete" exists with action: "Delete", module: "ProductionFacilities", user: "user", operating center: "nj4"	
	And operating center: "nj4" exists in town: "one"
    And a facility "one" exists with facility id: "NJSB-01", entity notes: "this is an entity note", operating center: "nj4", CommunityRightToKnow: true

Scenario: User can create a Community Right To Know and gets the proper validation
	Given I am logged in as "user"
	And a role "roleFacilityReadNj4" exists with action: "Read", module: "ProductionFacilities", user: "user", operating center: "nj4"
	And a community right to know "one" exists with facility: "one"
	When I visit the Facilities/CommunityRightToKnow/New page
	And I enter "08/17/2019" into the ExpirationDate field
	And I enter "08/17/2019" into the SubmissionDate field
	And I press Save
	Then I should see a validation message for Facility with "The Facility field is required."
	When I select state "nj" from the State dropdown
	And I select operating center "nj4" from the OperatingCenter dropdown
	And I select facility "one"'s Description from the Facility dropdown
	And I press Save
	Then I should see a link to the Show page for facility: "one"
	And I should see a display for ExpirationDate with "8/17/2019"

Scenario: User can edit Community Right To Know
	Given a community right to know "one" exists with facility: "one", ExpirationDate: "8/17/2019"
	And I am logged in as "user"
	When I visit the Show page for community right to know "one"
	And I follow "Edit"
	Then I should be at the Edit page for community right to know "one"
	When I enter "10/27/2020" into the ExpirationDate field
	And I enter "10/27/2020" into the SubmissionDate field
	And I select state "nj" from the State dropdown
	And I select operating center "nj4" from the OperatingCenter dropdown
	And I select facility "one"'s Description from the Facility dropdown
	And I press Save
	Then I should see a link to the Show page for facility: "one"
	And I should see a display for ExpirationDate with "10/27/2020"

Scenario: User can delete an arc flash study
	Given a community right to know "one" exists with facility: "one"
	And I am logged in as "user"
	When I visit the Show page for community right to know "one"
	And I click ok in the dialog after pressing "Delete"
	Then I should be at the Facilities/CommunityRightToKnow page
	When I try to access the Show page for community right to know: "one" expecting an error
	Then I should see a 404 error message

Scenario: User can search community right to know and view index
	Given a community right to know "one" exists with facility: "one"
	 And a facility "two" exists with facility id: "NJSB-02", entity notes: "this is an entity note", operating center: "nj4", CommunityRightToKnow: true
	 And a community right to know "two" exists with facility: "two"
	 And I am logged in as "user"
	 When I visit the Facilities/CommunityRightToKnow/Search page
	 And I select state "nj" from the State dropdown
	 And I select operating center "nj4" from the OperatingCenter dropdown
	 And I press "Search"
	 Then I should see a link to the Show page for community right to know "one"
	 And I should see a link to the Show page for community right to know "two"
	 When I visit the Facilities/CommunityRightToKnow/Search page
	 And I select state "nj" from the State dropdown
	 And I select operating center "nj4" from the OperatingCenter dropdown
	 And I select facility "two" from the Facility dropdown
	 And I press "Search"
	 Then I should not see a link to the Show page for community right to know "one"
	 And I should see a link to the Show page for community right to know "two"

Scenario: User can see the community right to know submission has not expired
	Given a community right to know "one" exists with facility: "one"
	And I am logged in as "user"
	When I visit the Show page for community right to know "one"
	And I follow "Edit"
	Then I should be at the Edit page for community right to know "one"
	When I enter the date "tomorrow" into the ExpirationDate field
	And I enter the date "today" into the SubmissionDate field
	And I select state "nj" from the State dropdown
	And I select operating center "nj4" from the OperatingCenter dropdown
	And I select facility "one" from the Facility dropdown
	And I press Save
	Then I should see a link to the Show page for facility: "one"
	And I should see a display for Expired with "No"

Scenario: User can see the community right to know submission has expired
	Given a community right to know "one" exists with facility: "one"
	And I am logged in as "user"
	When I visit the Show page for community right to know "one"
	And I follow "Edit"
	Then I should be at the Edit page for community right to know "one"
	When I enter the date "yesterday" into the ExpirationDate field
	And I enter the date "yesterday" into the SubmissionDate field
	And I select state "nj" from the State dropdown
	And I select operating center "nj4" from the OperatingCenter dropdown
	And I select facility "one" from the Facility dropdown
	And I press Save
	Then I should see a link to the Show page for facility: "one"
	And I should see a display for Expired with "Yes"
