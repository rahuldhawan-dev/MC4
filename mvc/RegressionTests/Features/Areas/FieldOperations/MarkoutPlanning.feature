Feature: MarkoutPlanningPage

Background: 
    Given a state "nj" exists
    And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", sap enabled: "true", sap work orders enabled: "false", arc mobile map id: "15fdc279b4234fcb85f455ee70897a9e", state: "nj"
    And a town "nj7burg" exists with name: "TOWN", state: "nj"
    And operating center: "nj7" exists in town: "nj7burg"
    And a town section "one" exists with town: "nj7burg", name: "Tucson"
    And a town section "inactive" exists with town: "nj7burg", active: false
    And a street prefix "north" exists with description: "N"
    And a street suffix "st" exists with description: "St"
    And a street suffix "row" exists with description: "Row"
    And a street "one" exists with town: "nj7burg", full st name: "EASY STREET", is active: true, name: "Easy", prefix: "north", suffix: "st"
    And a street "two" exists with town: "nj7burg", is active: true, full st name: "HIGH STREET", name: "Skid", prefix: "north", suffix: "row"
    And an asset type "valve" exists with description: "valve"
    And an asset type "hydrant" exists with description: "hydrant"
    And an asset type "main" exists with description: "main"
    And an asset type "service" exists with description: "service"
    And an asset type "sewer opening" exists with description: "sewer opening"
    And an asset type "sewer lateral" exists with description: "sewer lateral"
    And an asset type "sewer main" exists with description: "sewer main"
    And an asset type "storm catch" exists with description: "storm catch"
    And an asset type "equipment" exists with description: "equipment"
    And an asset type "facility" exists with description: "facility"
    And an asset type "main crossing" exists with description: "main crossing"
    And operating center: "nj7" has asset type "valve"
    And operating center: "nj7" has asset type "hydrant"
    And operating center: "nj7" has asset type "main"
    And operating center: "nj7" has asset type "service"
    And operating center: "nj7" has asset type "sewer opening"
    And operating center: "nj7" has asset type "sewer lateral"
    And operating center: "nj7" has asset type "sewer main"
    And operating center: "nj7" has asset type "storm catch"
    And operating center: "nj7" has asset type "equipment"
    And operating center: "nj7" has asset type "facility"
    And operating center: "nj7" has asset type "main crossing"
    And a hydrant "one" exists with street: "one", town: "nj7burg", operating center: "nj7"
    And a valve "one" exists with operating center: "nj7", town: "nj7burg", street: "one"
    And a sewer opening "one" exists with street: "one"
    And a facility "one" exists with town: "nj7burg", operating center: "nj7"
    And an equipment "one" exists with facility: "one"
    And a main crossing status "active" exists with description: "Active"
    And a main crossing "one" exists with length of segment: "100.01", stream: "one", town: "nj7burg", operating center: "nj7", main crossing status: "active"
    And work order requesters exist
    And work order purposes exist
    And work order priorities exist
    And work descriptions exist
    And markout requirements exist
    And a coordinate "one" exists
    And a user "user" exists with username: "user"
    And a role "workorder-read" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
    And a role "workorder-add" exists with action: "Add", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
    And a role "workorder-useradmin" exists with action: "UserAdministrator", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
    And a role "asset-read" exists with action: "Read", module: "FieldServicesAssets", user: "user", operating center: "nj7"
    And a role "hydrant-useradmin" exists with action: "UserAdministrator", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
    And a work order "one" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "valve", valve: "one", work description: "hydrant installation", markout requirement: "routine"
    And a markout "one" exists with expiration date: "yesterday", work order: "one"

Scenario: User can search for a work order and is able to see the edit button on the index page
    Given I am logged in as "user"
    When I visit the FieldOperations/MarkoutPlanning/Search page
    And I select state "nj" from the State dropdown
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I select asset type "valve" from the AssetType multiselect
    And I press Search
    When I click the "Edit Markout Plan" link in the 1st row of markoutPlanningTable
    And I wait for ajax to finish loading
    Then I should see "" in the DateMarkoutNeeded field

Scenario: User can edit the markout plan for a work order
    Given I am logged in as "user"
    And a markout type "one" exists with description: "C TO C"
    When I visit the FieldOperations/MarkoutPlanning/Index page
    And I click the "Edit Markout Plan" link in the 1st row of markoutPlanningTable
    And I wait for ajax to finish loading
    And I enter "10/20/2023" into the DateMarkoutNeeded field
    And I enter "meow" into the RequiredMarkoutNote field
    And I select markout type "one" from the MarkoutTypeNeeded dropdown
    And I press Save
    Then I should be at the FieldOperations/MarkoutPlanning/Index page
    And I should see the following values in the markoutPlanningTable table
	| Order # | Date Markout Needed | Markout To Be Called | Markout Type | Markout Note | Edit              |
    | 1       | 10/20/2023          | 10/16/2023           | C TO C       | meow         | Edit Markout Plan |