Feature: RestorationProcessingPage

Background: 
    Given an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", sap enabled: "true", sap work orders enabled: "true", arc mobile map id: "15fdc279b4234fcb85f455ee70897a9e"
    And a town "nj7burg" exists with name: "TOWN"
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
    And a restoration type "one" exists with description: "ASPHALT-STREET"
    And a work order "one" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "valve", valve: "one", work description: "hydrant installation"
    And a restoration "one" exists with work order: "one", paving square footage: "52.00", restoration type: "one"
	
Scenario: User can search for a work order and select a Restoration Processing to view
    Given I am logged in as "user"
    When I visit the FieldOperations/RestorationProcessing/Search page
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I select asset type "valve" from the AssetType multiselect
    And I press Search
    Then I should see a link to the FieldOperations/RestorationProcessing/show/1 page 
    When I follow "View"
    Then I should be at the FieldOperations/RestorationProcessing/Show/1 page

Scenario: user can view a restoration processing entry
    Given I am logged in as "user"
    When I visit the FieldOperations/RestorationProcessing/show/1 page 
    Then I should only see "Valve" in the AssetType element
    And I should only see "hey this is a note" in the Notes element
    And I should only see "N Easy St" in the Street element
    And I should only see "1234" in the StreetNumber element
    And I should only see "HYDRANT INSTALLATION" in the WorkDescription element

Scenario: user can add a restoration from the restoration processing page
    Given I am logged in as "user"
    When I visit the FieldOperations/RestorationProcessing/show/1 page
    And I click the "Restoration" tab
    Then I should see the link "Create Restoration" 
   
Scenario: user can delete a restoration from the restoration processing page
    Given a work order "two" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "valve", valve: "one", work description: "hydrant installation"
    And a contractor "one" exists with name: "one", operating center: "nj7"
    And a restoration "two" exists with work order: "two", paving square footage: "52.00", restoration type: "one", assigned contractor: "one"
    And I am logged in as "user"
    And I am at the FieldOperations/RestorationProcessing/Show/2 page
    When I click the "Restoration" tab
    Then I should see the button "Delete"
   