Feature: SpoilRemoval

Background: stuff exists
    Given an admin user "admin" exists with username: "admin"
    And a state "NJ" exists with name: "New Jersey", abbreviation: "NJ"
    And a town "Sometown" exists with name: "Anytown", state: "NJ"
    And a town "Sometown2" exists with name: "Anytown2", state: "NJ"
    And an operating center "one" exists with opcode: "one", state: "NJ"
    And an operating center "two" exists with opcode: "two"
    And a spoil storage location "one" exists with operating center: "one", town: "Sometown", active: true, name: "ssl1"
    And a spoil storage location "two" exists with operating center: "two", town: "Sometown2", active: true, name: "ssl2"
    And a spoil storage location "three" exists with operating center: "one", town: "Sometown", active: true, name: "ssl3"
    And a spoil final processing location "one" exists with operating center: "one", town: "Sometown", name: "sfpl"
    And a spoil final processing location "two" exists with operating center: "two", town: "Sometown2", name: "sfpl2"
    And a spoil final processing location "three" exists with operating center: "one", town: "Sometown", name: "sfpl3"
    And a spoil removal "one" exists with removed from: "one", final destination: "one", date removed: "07/12/2023", quantity: "200.02"
    And I am logged in as "admin"

Scenario: admin can search for spoil removal
    When I visit the FieldOperations/Spoilremoval/Search page
    And I select state "NJ" from the State dropdown
    And I wait for ajax to finish loading
    And I select operating center "one" from the OperatingCenter dropdown
    And I press "Search"
    Then I should be at the FieldOperations/SpoilRemoval page
    And I should see a link to the Show page for spoil removal "one"

Scenario: admin can create Spoil Removal
    When I visit the FieldOperations/SpoilRemoval/New page
    And I press Save
    Then I should see a validation message for State with "The State field is required."
    Then I should see a validation message for DateRemoved with "The DateRemoved field is required."
    When I select state "NJ" from the State dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
    When I select operating center "one" from the OperatingCenter dropdown
    And I enter 07/13/2023 into the DateRemoved field
    And I wait for ajax to finish loading
    And I press Save
    Then I should see a validation message for RemovedFrom with "The RemovedFrom field is required."
    Then I should see a validation message for FinalDestination with "The FinalDestination field is required."
    When I select spoil storage location "one" from the RemovedFrom dropdown  
    And I select spoil final processing location "one"'s Name from the FinalDestination dropdown
    And I press Save
    Then I should be at the FieldOperations/SpoilRemoval/Show/2 page

Scenario: admin can edit spoil removal
    When I visit the Edit page for spoil removal "one"
    When I select spoil storage location "three" from the RemovedFrom dropdown
    And I select spoil final processing location "three"'s Name from the FinalDestination dropdown
    And I press Save
    Then I should be at the Show page for spoil removal "one"
    And I should see a display for RemovedFrom with spoil storage location "three"'s Name
