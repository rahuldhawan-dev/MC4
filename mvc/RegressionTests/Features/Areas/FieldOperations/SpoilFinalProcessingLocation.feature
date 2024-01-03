Feature: SpoilFinalProcessingLocation

Background: stuff exists
    Given an admin user "admin" exists with username: "admin"
	And a state "one" exists with name: "New Jersey", abbreviation: "NJ", scada table: "njscada"
    And a town "one" exists with name: "Anytown", state: "one"
    And a street "one" exists with town: "one"
	And an operating center "opc" exists with opcode: "NJ7", name: "Shrewsbury", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
	And operating center: "opc" exists in town: "one"
    And a spoil final processing location "one" exists with operating center: "opc", town: "one"
    And a spoil final processing location "two" exists with operating center: "opc", town: "one"
    And I am logged in as "admin"

Scenario: admin can search for spoil final processing locations
    When I visit the FieldOperations/SpoilFinalProcessingLocation/Search page
    And I select state "one" from the State dropdown
    And I wait for ajax to finish loading
    And I select operating center "opc" from the OperatingCenter dropdown
    And I press "Search"
    Then I should be at the FieldOperations/SpoilFinalProcessingLocation page
    And I should see a link to the Show page for spoil final processing location "one"
    And I should see a link to the Show page for spoil final processing location "two"

Scenario: admin can view spoil final processing locations
    When I visit the Show page for spoil final processing location "one"
    Then I should see a display for OperatingCenter with operating center "opc"
    And I should see a display for Town with town "one"

Scenario: admin can create spoil final processing locations
    When I visit the FieldOperations/SpoilFinalProcessingLocation/New page
    And I press Save
    Then I should see a validation message for State with "The State field is required."
    When I select state "one" from the State dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
    When I select operating center "opc" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see a validation message for Name with "The Name field is required."
    When I enter "blah" into the Name field
    And I press Save
    Then I should be at the FieldOperations/SpoilFinalProcessingLocation/Show/3 page

Scenario: admin can edit spoil final processing locations
    When I visit the Edit page for spoil final processing location "one"
    And I enter "meh" into the Name field
    And I press Save
    Then I should be at the Show page for spoil final processing location "one"
    And I should see a display for Name with "meh"