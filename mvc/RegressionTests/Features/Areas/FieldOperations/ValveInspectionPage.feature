Feature: ValveInspectionPage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
	And a user "user" exists with username: "user"
	And an asset type "valve" exists with description: "valve"
	And a asset status "active" exists with description: "ACTIVE"
	And a asset status "retired" exists with description: "RETIRED"
	And a valve billing "public" exists with description: "PUBLIC"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And an operating center "ew4" exists with opcode: "EW4", name: "Edison", is active: "false"
	And a role "hydrant-useradmin" exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user", operating center: "nj7"
	And a role "hydrantinspections-useradmin" exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user", operating center: "nj7"
	And a town abbreviation type "town" exists
	And a town section abbreviation type "town section" exists
	And a town "one" exists with name: "Loch Arbour"
	And operating center: "nj7" exists in town: "one" with abbreviation: "LA"
	And a street "one" exists with town: "one"
	And a street "two" exists with town: "one", full st name: "Easy St"
	And a valve zone "one" exists
	And a valve zone "two" exists
    And a valve "one" exists with valve zone: "one", town: "one", street: "one", valve number: "VLA-1", valve suffix: 1, turns: 10, status: "active", valve billing: "public", critical: true, critical notes: "this is a critical note"
    And a valve "two" exists with operating center: "ew4", valve zone: "one", town: "one", street: "one", valve number: "VLA-3", valve suffix: 3, status: "active", valve billing: "public"
    And a valve "three" exists with valve zone: "one", town: "one", street: "one", valve number: "VLA-3", valve suffix: 3, status: "active", valve billing: "public"
	And a coordinate "one" exists 
    And a valve inspection "one" exists with valve: "one", minimum required turns: 4, date inspected: "12/8/1980 11:07 PM", inspected by: "user"
    And a valve inspection "two" exists with valve: "three", inspected by: "user"
	And a valve normal position "open" exists with description: "NORMALLY OPEN"
	And a valve normal position "closed" exists with description: "NORMALLY CLOSED"

Scenario: Valve Inspection Add should not be visible in the action bar
	Given I am logged in as "user"
	And I am at the FieldOperations/ValveInspection/Search page
	Then I should not see the "new" button in the action bar

Scenario: can search for a valve inspection
	Given I am logged in as "user"
    When I visit the /FieldOperations/ValveInspection page
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select town "one" from the Town dropdown
    And I press Search
	Then I should see the following values in the valveInspectionsTable table
	| Id     | Inspected |
	| 1      | No        |
	| 2      | No        |
	And I should see a link to the Show page for valve inspection: "one"
	And I should see a link to the Show page for valve inspection: "two"
    When I follow the Show link for valve inspection "one"
    Then I should be at the Show page for valve inspection: "one"
	When I visit the /FieldOperations/ValveInspection/Search page
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select "Yes" from the WorkOrderRequired dropdown
	And I press "Search"
	Then I should not see "The given key was not present in the dictionary."

Scenario: can view a valve inspection
	Given I am logged in as "user"
    When I visit the Show page for valve inspection: "one"
    Then I should see a display for Inspected with "No"
	And I should see a display for MinimumRequiredTurns with "4.00"
	And I should see a display for TurnsNotCompleted with "No"

Scenario: can add a valve inspection and gets validation
	Given I am logged in as "user"
    When I visit the Show page for valve: "one"
	And I click the "Inspections" tab
	And I follow "New Inspection"
    And I press "Save"
	Then I should see the validation message "The Inspected field is required."
	And I should see the validation message "The Number of Turns Completed field is required."
	When I select "No" from the Inspected dropdown
	And I press "Save"
	Then I shouldn't see the validation message "The Position Found field is required."
	And I shouldn't see the validation message "The Position Left field is required."
	When I select "Yes" from the Inspected dropdown
	And I press "Save"
	Then I should see the validation message The Position Found field is required."
	And I should see the validation message The Position Left field is required."
	When I check the TurnsNotCompleted field
	And I press "Save"
	Then I shouldn't see the validation message "The Position Found field is required."
	And I shouldn't see the validation message "The Position Left field is required."
	When I uncheck the TurnsNotCompleted field
	When I enter 1 into the Turns field
	And I press "Save"
	Then I should see the validation message You must enter at least the minimum number of turns or check turns not completed."
	When I enter 5 into the Turns field
	#And I enter "12/8/1980 11:07 PM" into the DateInspected field
	And I select "Yes" from the Inspected dropdown
	And I select valve normal position "open" from the PositionFound dropdown
	And I select valve normal position "open" from the PositionLeft dropdown
	And I enter "foo bar baz" into the Remarks field
	And I press "Save"
	Then the currently shown valve inspection shall henceforth be known throughout the land as "the knighted one"
    #And I should see a display for DateInspected with "12/8/1980 11:07 PM"
	And I should see a display for Inspected with "Yes"
	And I should see a display for PositionFound with valve normal position "open"'s Description
	And I should see a display for PositionLeft with valve normal position "open"'s Description
	And I should see a display for Turns with "5.00"
	And I should see a display for MinimumRequiredTurns with "2.00"
	And I should see a display for TurnsNotCompleted with "No"
	And I should see a display for Remarks with "foo bar baz"

Scenario: can edit a valve inspection
	Given I am logged in as "user"
	When I visit the Edit page for valve inspection: "one"
    #And I enter "12/8/1980 11:07 PM" into the DateInspected field
	And I select "Yes" from the Inspected dropdown
	And I select valve normal position "open" from the PositionFound dropdown
	And I select valve normal position "open" from the PositionLeft dropdown
	And I enter 5 into the Turns field
	And I check the TurnsNotCompleted field
	And I enter "foo bar baz" into the Remarks field
	And I press Save
    Then I should be at the Show page for valve inspection: "one"
    And I should see a display for DateInspected with "12/8/1980 11:07 PM"
	And I should see a display for Inspected with "Yes"
	And I should see a display for PositionFound with valve normal position "open"'s Description
	And I should see a display for PositionLeft with valve normal position "open"'s Description
	And I should see a display for Turns with "5.00"
	And I should see a display for MinimumRequiredTurns with "4.00"
	And I should see a display for TurnsNotCompleted with "Yes"
	And I should see a display for Remarks with "foo bar baz"

Scenario: User can't add inspection if operating center is inactive
	Given I am logged in as "admin"
	When I visit the Show page for valve: "two"
	And I click the "Inspections" tab
	Then I should not see "New Inspection"

Scenario: user cant add inspection from index if operating center is inactive
	Given I am logged in as "admin"
    When I visit the FieldOperations/Valve/Search page
    And I press Search
    Then I should see a link to the Show page for valve: "two"
    And I should not see a link to the FieldOperations/ValveInspection/New page for valve: "two"

