Feature: MarkoutDamagePage

Background:
    Given a user "user" exists with username: "user"
    And a state "one" exists with abbreviation: "QQ", name: "Q State"
    And a county "one" exists with state: "one", name: "Count Countula"
    And a town "one" exists with county: "one", name: "Townie"
    And an operating center "one" exists with opcode: "QQ1", name: "Wawa", state: "one"
    And a role "readrole" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user", operating center: "one"
    And a role "addrole" exists with action: "Add", module: "FieldServicesWorkManagement", user: "user", operating center: "one"
    And a role "editrole" exists with action: "Edit", module: "FieldServicesWorkManagement", user: "user", operating center: "one"
    And an employee status "active" exists with description: "Active"
    And an employee "one" exists with first name: "Theodore", last name: "Logan", employee id: "11111111", operating center: "one", status: "active"
    And an employee "supervisor" exists with operating center: "one", status: "active"
    And a markout damage to type "ours" exists with description: "Ours"
    And a markout damage to type "other" exists with description: "Other"
    And a work order "one" exists with date completed: "8/12/2019", id: "1", operating center: "one", town: "one"
    And a work order "two" exists with cancelled at: now, date completed: "8/12/2019"
    And a gas markout damage utility damage type "gas" exists

Scenario: Creating a new record should display a whole lotta validation
    Given I am logged in as "user"
    And I am at the FieldOperations/MarkoutDamage/New page 
    When I press Save
    Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
    And I should see a validation message for Street with "The Street field is required."
    And I should see a validation message for NearestCrossStreet with "The NearestCrossStreet field is required."
    And I should see a validation message for Coordinate with "The Coordinate field is required."
    And I should see a validation message for DamageOn with "The DamageOn field is required."
    And I should see a validation message for MarkoutDamageToType with "The MarkoutDamageToType field is required."
    And I should see a validation message for IsMarkedOut with "The IsMarkedOut field is required."
    And I should see a validation message for IsMismarked with "The IsMismarked field is required."
    And I should see a validation message for ExcavatorDiscoveredDamage with "The ExcavatorDiscoveredDamage field is required."
    And I should see a validation message for ExcavatorCausedDamage with "The ExcavatorCausedDamage field is required."
    And I should see a validation message for Was911Called with "The Was 911 Called? field is required."
    And I should see a validation message for WerePicturesTaken with "The WerePicturesTaken field is required."
    When I select operating center "one" from the OperatingCenter dropdown
    And I press Save
    Then I should see a validation message for State with "The State field is required."
    When I select state "one" from the State dropdown
    And I press Save
    Then I should see a validation message for County with "The County field is required."
    When I select county "one" from the County dropdown
    And I press Save
    Then I should see a validation message for Town with "The Town field is required."
    When I select markout damage to type "other" from the MarkoutDamageToType dropdown
    And I press Save
    Then I should see a validation message for RequestNumber with "Required when markout damage is to others."
    And I should see a validation message for UtilityDamages with "Required when markout damage is to others." 
    And I should not see a validation message for Excavator with "Required when markout damage is to our own." 
    When I select markout damage to type "ours" from the MarkoutDamageToType dropdown
    And I press Save
    Then I should not see a validation message for RequestNumber with "Required when markout damage is to others."
    And I should not see a validation message for UtilityDamages with "Required when markout damage is to others." 
    And I should see a validation message for Excavator with "Required when markout damage is to our own." 
    When I select "Yes" from the IsMismarked dropdown
    And I press Save
    Then I should see a validation message for MismarkedByInches with "The MismarkedByInches field is required."

Scenario: User can edit a markout damage record
    Given a markout damage "one" exists with operating center: "one", town: "one", street: "Some Street", created by: "someuser", excavator: "some excavator"
    And I am logged in as "user"
    And I am at the edit page for markout damage: "one"
    When I select employee "supervisor"'s Description from the SupervisorSignOffEmployee dropdown
    And I enter work order "two"'s Id into the WorkOrder field
    And I press Save
    Then I should see a validation message for WorkOrder with "WorkOrderID's value does not match an existing object."
    When I enter "666" into the WorkOrder field
    And I press Save
    Then I should see a validation message for WorkOrder with "WorkOrderID's value does not match an existing object."
    When I enter work order "one"'s Id into the WorkOrder field
    And I press Save
    Then I should see a display for "SupervisorSignOffEmployee" with employee: "supervisor"'s FullName
    And I should see a display for ApprovedOn with today's date
    And I should see a link to the Show page for work order "one"

Scenario: User can create a new markout damage record
    Given a coordinate "one" exists
    And I am logged in as "user"
    And I am at the FieldOperations/MarkoutDamage/New page 
    When I select operating center "one" from the OperatingCenter dropdown
    And I select state "one" from the State dropdown
    And I select county "one" from the County dropdown
    And I select town "one" from the Town dropdown
    And I enter "52 Some Road" into the Street field
    And I enter "Some Street" into the NearestCrossStreet field
    And I enter coordinate "one"'s Id into the Coordinate field
    And I enter "some work order" into the SAPWorkOrderId field
    And I enter "4/24/1984 4:04 AM" into the DamageOn field
    And I select markout damage to type "ours" from the MarkoutDamageToType dropdown
    And I enter "123456789" into the RequestNumber field
    And I enter "This is a comment about markout damage. It's called Markout Damage's Theme." into the DamageComments field 
    And I select gas markout damage utility damage type "gas" from the UtilityDamages dropdown
    And I enter "Rick" into the EmployeesOnJob field
    And I select "Yes" from the IsMarkedOut dropdown
    And I select "Yes" from the IsMismarked dropdown
    And I enter "42" into the MismarkedByInches field
    And I select "Yes" from the ExcavatorDiscoveredDamage dropdown
    And I select "Yes" from the ExcavatorCausedDamage dropdown
    And I select "No" from the Was911Called dropdown
    And I select "No" from the WerePicturesTaken dropdown
    And I enter "Bob" into the Excavator field
    And I enter "123 Fake St" into the ExcavatorAddress field
    And I enter "555-555-5555" into the ExcavatorPhone field
    And I press Save
    Then I should see a display for OperatingCenter with operating center "one"
    And I should see a display for Town_County_State with state "one"
    And I should see a display for Town_County with county "one"
    And I should see a display for Town with town "one"
    And I should see a display for Street with "52 Some Road"
    And I should see a display for NearestCrossStreet with "Some Street"
    And I should see a display for SAPWorkOrderId with "some work order"
    And I should see a display for DamageOn with "4/24/1984 4:04 AM"
    And I should see a display for MarkoutDamageToType with markout damage to type "ours"
    And I should see a link to "/FieldOperations/OneCallMarkoutTicket?RequestNumber=123456789"
    And I should see a display for DamageComments with "This is a comment about markout damage. It's called Markout Damage's Theme."
    And I should see "Gas" in the UtilityDamages element
    And I should see a display for EmployeesOnJob with "Rick"
    And I should see a display for IsMarkedOut with "Yes"
    And I should see a display for IsMismarked with "Yes"
    And I should see a display for MismarkedByInches with "42"
    And I should see a display for ExcavatorDiscoveredDamage with "Yes"
    And I should see a display for ExcavatorCausedDamage with "Yes"
    And I should see a display for Was911Called with "No"
    And I should see a display for WerePicturesTaken with "No"
    And I should see a display for Excavator with "Bob"
    And I should see a display for ExcavatorAddress with "123 Fake St"
    And I should see a display for ExcavatorPhone with "(555) 555-5555"

Scenario: User can search for stuff
    Given a markout damage "one" exists with operating center: "one", town: "one", street: "Some Street", created by: "someuser", excavator: "some excavator", work order: "one"
    And I am logged in as "user"
    And I am at the FieldOperations/MarkoutDamage/Search page 
    When I press Search
    Then I should see the following values in the search-results table
    | Operating Center | Town   | Street      | Created By | Is Signed Off By Supervisor | Excavator      |
    | QQ1 - Wawa       | Townie | Some Street | someuser   | No                          | some excavator |
    # split this up so it's not scrolling across the page
    And I should see the following values in the search-results table
    | Is Marked Out | Is Mismarked | Excavator Caused Damage | Excavator Discovered Damage | Was 911 Called? |
    | No            | No           | No                      | No                          | No              |
    And I should see a link to the Show page for work order "one"

Scenario: User can see map popup sorta kinda
    Given a markout damage "one" exists with operating center: "one", town: "one", street: "Some Street", created by: "someuser", excavator: "some excavator"
    And I am logged in as "user"
    When I go to the Show frag for markout damage: "one"
    Then I should see markout damage: "one"'s Town on the page
    And I should see markout damage: "one"'s Street on the page

Scenario: User can prepopulate markout damage details from one call tickets
    Given a one call ticket "awful" exists with request number: "111111111", town: "TOWNIE", county: "COUNT COUNTULA", state: "QQ", excavator: "Some guy who excavates", excavator address: "He lives somewhere", excavator phone: "444-444-4444", street: "Some street", nearest cross street: "Some other street"
    And I am logged in as "user"
    And I am at the FieldOperations/MarkoutDamage/New page 
    When I enter "111111111" into the RequestNumber field
    And I select operating center "one" from the OperatingCenter dropdown
    And I press populate-ticket-button
    # The ajax request this button fires off has async: false on it, so there shouldn't be a need to do a wait.
    Then I should see "Some guy who excavates" in the Excavator field
    And I should see "He lives somewhere" in the ExcavatorAddress field
    And I should see "444-444-4444" in the ExcavatorPhone field
    And I should see "Some street" in the Street field
    And I should see "Some other street" in the NearestCrossStreet field
    And I should see state "one" in the State dropdown
    And I should see county "one" in the County dropdown
    And I should see town "one" in the Town dropdown

Scenario: User can search by work order number
    Given a markout damage "one" exists with operating center: "one", town: "one", street: "Any Street", created by: "anyuser", work order: "one"
    And a markout damage "two" exists with operating center: "one", town: "one", street: "Another Street", created by: "someuser", work order: "two"
    And I am logged in as "user"
    And I am at the FieldOperations/MarkoutDamage/Search page 
    When I enter work order "one"'s Id into the WorkOrder field
    And I press Search
    Then I should see the following values in the search-results table
    | Id | Operating Center | Town   | Street     | Created By |
    | 1  | QQ1 - Wawa       | Townie | Any Street | anyuser   | 

Scenario: User can create a markout damage from a work order
    Given a coordinate "one" exists
    And I am logged in as "user"
    And I am at the FieldOperations/MarkoutDamage/New/1 page
    Then operating center "one" should be selected in the OperatingCenter dropdown
    And state "one" should be selected in the State dropdown
    And county "one" should be selected in the County dropdown
    And town "one" should be selected in the Town dropdown
    And I should see work order "one"'s Id in the WorkOrder field
    When I enter "foo st." into the Street field
    And I enter "bar way" into the NearestCrossStreet field
    And I select markout damage to type "ours" from the MarkoutDamageToType dropdown
    And I enter coordinate "one"'s Id into the Coordinate field
    And I enter today's date into the DamageOn field
    And I enter "damage comments" into the DamageComments field
    And I select "No" from the IsMarkedOut dropdown
    And I select "No" from the IsMismarked dropdown
    And I select "No" from the ExcavatorDiscoveredDamage dropdown
    And I select "No" from the ExcavatorCausedDamage dropdown
    And I select "No" from the Was911Called dropdown
    And I select "No" from the WerePicturesTaken dropdown
    And I enter "baz co." into the Excavator field
    When I press Save
    And I wait for the page to reload
    Then I should see a link to the Show page for work order "one"