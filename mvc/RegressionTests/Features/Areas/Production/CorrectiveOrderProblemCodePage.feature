Feature: CorrectiveOrderProblemCode
	Hi, this is Tim from Tooltime. In order for me to fix the order
	I need for the pages to not be broken
	Turn on the Nuclear-Powered Chainsaw and it will be as good as new! 

Background:
	Given an admin user "admin" exists with username: "admin"
	And a role "proddataadminread" exists with action: "Read", module: "ProductionDataAdministration"
	And a role "proddataadminadd" exists with action: "Add", module: "ProductionDataAdministration"
	And a role "proddataadminedit" exists with action: "Edit", module: "ProductionDataAdministration"
	And a user "user" exists with username: "user", roles: proddataadminread;proddataadminadd;proddataadminedit
	And a corrective order problem code "one" exists with code: "Alpha", description: "This is just DEF-CON 5"
    And a corrective order problem code "two" exists with code: "Upsilon", description: "Move it to DEF-CON 2!"
	And an equipment type "one" exists with description: "rtu"
	And an equipment type "two" exists with description: "engine"
	And equipment type: "one" exists in corrective order problem code: "one"
	And equipment type: "one" exists in corrective order problem code: "two"
	
Scenario: Admin can view a corrective order problem code
	Given I am logged in as "admin"
	And I am at the Show page for corrective order problem code: "one"
    Then I should see a display for Code with "Alpha"
    Then I should see a display for Description with "This is just DEF-CON 5"
	
Scenario: Admin can edit a corrective order problem code
	Given I am logged in as "admin"
	And I am at the Show page for corrective order problem code: "two"
	When I follow "Edit"
    And I enter "Mellon" into the Code field
	And I enter "Speak, Friend, and Enter" into the Description field
	And I select equipment type "two"'s Display from the EquipmentTypes dropdown
    And I press "Save"
    Then I should be at the show page for corrective order problem code: "two"
	And I should see a display for Code with "Mellon"
    And I should see a display for Description with "Speak, Friend, and Enter"
	And I should see a display for EquipmentTypes with "EngineRTU"

Scenario: Admin can search for a corrective order problem code
	Given I am logged in as "admin"
	And I am at the Production/CorrectiveOrderProblemCode/Search page
	When I enter "Upsilon" into the Code field
	And I press "Search"
	Then I should be at the Production/CorrectiveOrderProblemCode page
	And I should not see a link to the show page for corrective order problem code: "one"
	And I should see a link to the show page for corrective order problem code: "two"
	When I visit the Production/CorrectiveOrderProblemCode/Search page
	And I press "Search"
	Then I should be at the Production/CorrectiveOrderProblemCode page
	And I should see a link to the show page for corrective order problem code: "one"
	And I should see a link to the show page for corrective order problem code: "two"

Scenario: Admin can create a corrective order problem code
	Given I am logged in as "admin"
	When I visit the Production/CorrectiveOrderProblemCode/New page
	And I press Save
	Then I should be at the Production/CorrectiveOrderProblemCode/New page	
	And I should see the validation message "The Code field is required."
    And I should see the validation message "The Description field is required."	
	When I enter "Sarah Connor" into the Code field
	And I enter "You have been targeted for termination" into the Description field
    And I press Save
	Then the currently shown corrective order problem code will now be known throughout the land as "skynet"
	And I should be at the show page for corrective order problem code: "skynet"
	And I should see a display for Code with "Sarah Connor"
	And I should see a display for Description with "You have been targeted for termination"