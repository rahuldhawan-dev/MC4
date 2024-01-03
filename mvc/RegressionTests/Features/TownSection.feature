Feature: TownSection

Background: stuff exists
    Given an admin user "admin" exists with username: "admin"
	And a state "one" exists with name: "New Jersey", abbreviation: "NJ", scada table: "njscada"
    And a county "one" exists with name: "Monmouth", state: "one"
    And a town "one" exists with name: "Allenhurst", county: "one", state: "one"
    And a town section "one" exists with town: "one"
    And a town section "two" exists with town: "one"
	And a Functional location "one" exists with Description: "abc-123-abcd-1234", town: "one"
	And a Functional location "two" exists with Description: "def-456-defg-4567", town: "one"
	And an operating center "opc" exists with opcode: "NJ4"	
	And a Planning Plant "one" exists with operating center: "opc", description: "PPF1"
	And a Planning Plant "two" exists with operating center: "opc", description: "PPF2"	
    And I am logged in as "admin"

Scenario: admin can search for town sections
    When I visit the TownSection/Search page
    And I select state "one" from the State dropdown
	And I wait for ajax to finish loading
    And I select county "one" from the County dropdown
    And I wait for ajax to finish loading
    And I select town "one" from the Town dropdown
    And I press Search
    Then I should be at the TownSection page
    And I should see a link to the Show page for town section "one"
    And I should see a link to the Show page for town section "two"

Scenario: admin can view town sections
    When I visit the Show page for town section "one"
    Then I should see a display for Town_State with state "one"
    And I should see a display for Town_County with county "one"
    And I should see a display for Town with town "one"

Scenario: admin can create town sections
    When I visit the TownSection/New page
	And I press Save
    Then I should see a validation message for State with "The State field is required."
    When I select state "one" from the State dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see a validation message for County with "The County field is required."
    When I select county "one" from the County dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see a validation message for Town with "The Town field is required."
    When I select town "one" from the Town dropdown
    And I press Save
    Then I should see a validation message for Name with "The Town Section Name field is required."
    When I enter "blah" into the Name field  
	When I enter "12345" into the MainSAPEquipmentId field
	When I enter "67890" into the SewerMainSAPEquipmentId field
	When I select Functional location "one" from the MainSAPFunctionalLocation dropdown
	When I select Functional location "one" from the SewerMainSAPFunctionalLocation dropdown
	When I select Planning Plant "one"'s Display from the DistributionPlanningPlant dropdown
	When I select Planning Plant "two"'s Display from the SewerPlanningPlant dropdown
	And I press Save
    Then I should be at the TownSection/Show/3 page

Scenario: admin can edit town sections
    When I visit the Edit page for town section "one"
    And I enter "meh" into the Name field
    And I press Save
    Then I should be at the Show page for town section "one"
    And I should see a display for Name with "meh"