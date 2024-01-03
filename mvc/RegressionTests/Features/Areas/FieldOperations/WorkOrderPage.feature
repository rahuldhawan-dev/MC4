Feature: WorkOrderPage

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
    And a asset status "pending" exists with description: "PENDING"
    And a asset status "active" exists with description: "ACTIVE", is user admin only: "true"
    And a asset status "removed" exists with description: "REMOVED", is user admin only: "true"
    And a asset status "retired" exists with description: "RETIRED", is user admin only: "true"
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
    And a work order "one" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "valve", valve: "one"
    
Scenario: User cannot create a work order for an inactive town section 
    Given I am logged in as "user"
    And I am at the FieldOperations/WorkOrder/New page
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I select town "nj7burg" from the Town dropdown
    Then I should not see town section "inactive" in the TownSection dropdown

Scenario: User cannot create a non revisit work order using a revisit work description
    Given I am logged in as "user"
    And I am at the FieldOperations/WorkOrder/New page
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I select town "nj7burg" from the Town dropdown
    And I select asset type "hydrant" from the AssetType dropdown
    Then I should not see work description "hydrant landscaping" in the WorkDescription dropdown

Scenario: User can create a work order from a hydrant
    Given I am logged in as "user"
    And I am at the Show page for hydrant "one"
    When I click the "Work Orders" tab
    And I follow "Create New Work Order"
    Then I should see work description "hydrant leaking" in the WorkDescription dropdown

Scenario: User can add a work order for a hydrant
    Given I am logged in as "user"
    And I am at the FieldOperations/WorkOrder/New page
    Then I should not see the validation message "The OperatingCenter field is required."
    When I select "-- Select --" from the OperatingCenter dropdown
    And I press Save
    Then I should be at the FieldOperations/WorkOrder/New page
    And I should see the validation message "The OperatingCenter field is required."
    And I should see the validation message "Please enter the nearest (or customer) house number."
    And I should see the validation message "Please enter the location for this order using the globe icon."
    And I should see the validation message "The RequestedBy field is required."
    And I should see the validation message "The Purpose field is required."
    And I should see the validation message "The Priority field is required."
    And I should see the validation message "The MarkoutRequirement field is required."
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Town field is required."
    When I select town "nj7burg" from the Town dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Street field is required."
    And I should see the validation message "The NearestCrossStreet field is required."
    When I enter "123" into the StreetNumber field
    And I enter "Garage" into the ApartmentAddtl field 
    And I enter "eas" and select "N Easy St" from the Street autocomplete field 
    And I wait for ajax to finish loading
    And I enter "kid" and select "N Skid Row" from the NearestCrossStreet autocomplete field 
    And I wait for ajax to finish loading
    And I press Save
    When I select work order requester "call center" from the RequestedBy dropdown
    And I select work order purpose "customer" from the Purpose dropdown
    And I select work order priority "routine" from the Priority dropdown
    And I select markout requirement "none" from the MarkoutRequirement dropdown
    And I enter coordinate "one"'s Id into the CoordinateId field
    And I select asset type "hydrant" from the AssetType dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Description of Job field is required."
    And I should see the validation message "The Hydrant field is required."
    When I select work description "hydrant flushing" from the WorkDescription dropdown
    And I select hydrant "one"'s DescriptionWithStatus from the Hydrant dropdown
    #And I select "HAB-1 - ACTIVE" from the Hydrant dropdown
    And I wait for ajax to finish loading
    And I press Save
    # would be nice if we could include the querystring here
    Then the currently shown work order will now be referred to as "new"
    And I should be at the FieldOperations/GeneralWorkOrder/Show page for work order "new"
    And I should see a link to the show page for hydrant "one"

Scenario: User can add a work order for a valve
    Given I am logged in as "user"
    And I am at the FieldOperations/WorkOrder/New page
    When I select "-- Select --" from the OperatingCenter dropdown
    And I press Save
    Then I should be at the FieldOperations/WorkOrder/New page
    And I should see the validation message "The OperatingCenter field is required."
    And I should see the validation message "Please enter the nearest (or customer) house number."
    And I should see the validation message "Please enter the location for this order using the globe icon."
    And I should see the validation message "The RequestedBy field is required."
    And I should see the validation message "The Purpose field is required."
    And I should see the validation message "The Priority field is required."
    And I should see the validation message "The MarkoutRequirement field is required."
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Town field is required."
    When I select town "nj7burg" from the Town dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Street field is required."
    And I should see the validation message "The NearestCrossStreet field is required."
    When I enter "123" into the StreetNumber field
    And I enter "eas" and select "N Easy St" from the Street autocomplete field 
    And I wait for ajax to finish loading
    And I enter "kid" and select "N Skid Row" from the NearestCrossStreet autocomplete field 
    And I wait for ajax to finish loading
    And I press Save
    When I select work order requester "call center" from the RequestedBy dropdown
    And I select work order purpose "customer" from the Purpose dropdown
    And I select work order priority "routine" from the Priority dropdown
    And I select markout requirement "none" from the MarkoutRequirement dropdown
    And I enter coordinate "one"'s Id into the CoordinateId field
    And I select asset type "valve" from the AssetType dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Description of Job field is required."
    And I should see the validation message "The Valve field is required."
    When I select work description "valve repair" from the WorkDescription dropdown
    And I select valve "one"'s DescriptionWithStatus from the Valve dropdown
    # wo history and coordinate (null in this case) will load for the asset
    And I wait for ajax to finish loading
    And I press Save
    # would be nice if we could include the querystring here
    Then the currently shown work order will now be referred to as "new"
    And I should be at the FieldOperations/GeneralWorkOrder/Show page for work order "new"
    And I should see a link to the show page for valve "one"

Scenario: User can add a work order for a main
    Given I am logged in as "user"
    And I am at the FieldOperations/WorkOrder/New page
    When I select "-- Select --" from the OperatingCenter dropdown
    And I press Save
    Then I should be at the FieldOperations/WorkOrder/New page
    And I should see the validation message "The OperatingCenter field is required."
    And I should see the validation message "Please enter the nearest (or customer) house number."
    And I should see the validation message "Please enter the location for this order using the globe icon."
    And I should see the validation message "The RequestedBy field is required."
    And I should see the validation message "The Purpose field is required."
    And I should see the validation message "The Priority field is required."
    And I should see the validation message "The MarkoutRequirement field is required."
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Town field is required."
    When I select town "nj7burg" from the Town dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Street field is required."
    And I should see the validation message "The NearestCrossStreet field is required."
    When I enter "123" into the StreetNumber field
    And I enter "eas" and select "N Easy St" from the Street autocomplete field 
    And I wait for ajax to finish loading
    And I enter "kid" and select "N Skid Row" from the NearestCrossStreet autocomplete field 
    And I wait for ajax to finish loading
    And I press Save
    When I select work order requester "call center" from the RequestedBy dropdown
    And I select work order purpose "customer" from the Purpose dropdown
    And I select work order priority "routine" from the Priority dropdown
    And I select markout requirement "none" from the MarkoutRequirement dropdown
    And I enter coordinate "one"'s Id into the CoordinateId field
    And I select asset type "main" from the AssetType dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Description of Job field is required."
    When I select work description "new main flushing" from the WorkDescription dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The plant maintenance activity code is required when selecting the current work description."
    When I select work description "main investigation" from the WorkDescription dropdown
    And I press Save
    # would be nice if we could include the querystring here
    Then the currently shown work order will now be referred to as "new"
    And I should be at the FieldOperations/GeneralWorkOrder/Show page for work order "new"

Scenario: User can add a work order for a service
    Given I am logged in as "user"
    And I am at the FieldOperations/WorkOrder/New page
    When I select "-- Select --" from the OperatingCenter dropdown
    And I press Save
    Then I should be at the FieldOperations/WorkOrder/New page
    And I should see the validation message "The OperatingCenter field is required."
    And I should see the validation message "Please enter the nearest (or customer) house number."
    And I should see the validation message "Please enter the location for this order using the globe icon."
    And I should see the validation message "The RequestedBy field is required."
    And I should see the validation message "The Purpose field is required."
    And I should see the validation message "The Priority field is required."
    And I should see the validation message "The MarkoutRequirement field is required."
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Town field is required."
    When I select town "nj7burg" from the Town dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Street field is required."
    And I should see the validation message "The NearestCrossStreet field is required."
    When I enter "123" into the StreetNumber field
    And I enter "eas" and select "N Easy St" from the Street autocomplete field 
    And I wait for ajax to finish loading
    And I enter "kid" and select "N Skid Row" from the NearestCrossStreet autocomplete field 
    And I wait for ajax to finish loading
    And I press Save
    When I select work order requester "call center" from the RequestedBy dropdown
    And I select work order purpose "customer" from the Purpose dropdown
    And I select work order priority "routine" from the Priority dropdown
    And I select markout requirement "none" from the MarkoutRequirement dropdown
    And I enter coordinate "one"'s Id into the CoordinateId field
    And I select asset type "service" from the AssetType dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Description of Job field is required."
    And I should see the validation message "The PremiseNumber field is required."
    And I should see the validation message "The DeviceLocation field is required."
    And I should see the validation message "The Installation field is required."
    When I enter "1234567890" into the PremiseNumber field
    And I select work description "leak survey" from the WorkDescription dropdown    
    And I press Save
    Then I should see the validation message "The DeviceLocation field is required."
    And I should see the validation message "The Installation field is required."
    When I enter "1234567890" into the DeviceLocation field
    And I enter "1234567891" into the Installation field
    And I enter yesterday's date into the PlannedCompletionDate field
    And I press Save
    Then I should see the validation message "The field Planned Completion Date must be greater than or equal to today for an Emergency priority. For all other priorities it must be between 2 days from now and 12/31/9999 11:59:59 PM."
    When I enter the date "2 days from now" into the PlannedCompletionDate field
    And I press Save
    # would be nice if we could include the querystring here
    Then the currently shown work order will now be referred to as "new"
    And I should be at the FieldOperations/GeneralWorkOrder/Show page for work order "new"
    And I should see a link to the /Customer/Premise?PremiseNumber.MatchType=Exact&PremiseNumber.Value=1234567890 page
    And I should see a link to the /Customer/SAPTechnicalMasterAccount?PremiseNumber=1234567890 page
    And I should only see "2 days from now" in the PlannedCompletionDate element

Scenario: user cannot add a planned completion date before today
    Given I am logged in as "user"
    And I am at the FieldOperations/WorkOrder/New page
    When I select work order priority "routine" from the Priority dropdown
    And I enter yesterday's date into the PlannedCompletionDate field
    And I press Save
    Then I should see the validation message "The field Planned Completion Date must be greater than or equal to today for an Emergency priority. For all other priorities it must be between 2 days from now and 12/31/9999 11:59:59 PM."
    When I select work order priority "emergency" from the Priority dropdown
    And I press Save
    Then I should see the validation message "The field Planned Completion Date must be greater than or equal to today for an Emergency priority. For all other priorities it must be between 2 days from now and 12/31/9999 11:59:59 PM."
    When I enter today's date into the PlannedCompletionDate field
    And I press Save
    Then I should not see the validation message "The field Planned Completion Date must be greater than or equal to today for an Emergency priority. For all other priorities it must be between 2 days from now and 12/31/9999 11:59:59 PM."  

Scenario: User can add a work order for a sewer opening
    Given I am logged in as "user"
    And I am at the FieldOperations/WorkOrder/New page
    When I select "-- Select --" from the OperatingCenter dropdown
    And I press Save
    Then I should be at the FieldOperations/WorkOrder/New page
    And I should see the validation message "The OperatingCenter field is required."
    And I should see the validation message "Please enter the nearest (or customer) house number."
    And I should see the validation message "Please enter the location for this order using the globe icon."
    And I should see the validation message "The RequestedBy field is required."
    And I should see the validation message "The Purpose field is required."
    And I should see the validation message "The Priority field is required."
    And I should see the validation message "The MarkoutRequirement field is required." 
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Town field is required."
    When I select town "nj7burg" from the Town dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Street field is required."
    And I should see the validation message "The NearestCrossStreet field is required."
    When I enter "123" into the StreetNumber field
    And I enter "eas" and select "N Easy St" from the Street autocomplete field 
    And I wait for ajax to finish loading
    And I enter "kid" and select "N Skid Row" from the NearestCrossStreet autocomplete field 
    And I wait for ajax to finish loading
    And I press Save
    When I select work order requester "call center" from the RequestedBy dropdown
    And I select work order purpose "customer" from the Purpose dropdown
    And I select work order priority "routine" from the Priority dropdown
    And I select markout requirement "none" from the MarkoutRequirement dropdown
    And I enter coordinate "one"'s Id into the CoordinateId field
    And I select asset type "sewer opening" from the AssetType dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Description of Job field is required."
    And I should see the validation message "The SewerOpening field is required."
    When I select work description "sewer opening repair" from the WorkDescription dropdown
    And I select sewer opening "one"'s DescriptionWithStatus from the SewerOpening dropdown
    And I press Save
    # would be nice if we could include the querystring here
    Then the currently shown work order will now be referred to as "new"
    And I should be at the FieldOperations/GeneralWorkOrder/Show page for work order "new"
    And I should see a link to the show page for sewer opening "one"

Scenario: User can add a work order for a sewer lateral
    Given I am logged in as "user"
    And I am at the FieldOperations/WorkOrder/New page
    When I select "-- Select --" from the OperatingCenter dropdown
    And I press Save
    Then I should be at the FieldOperations/WorkOrder/New page
    And I should see the validation message "The OperatingCenter field is required."
    And I should see the validation message "Please enter the nearest (or customer) house number."
    And I should see the validation message "Please enter the location for this order using the globe icon."
    And I should see the validation message "The RequestedBy field is required."
    And I should see the validation message "The Purpose field is required."
    And I should see the validation message "The Priority field is required."
    And I should see the validation message "The MarkoutRequirement field is required."
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Town field is required."
    When I select town "nj7burg" from the Town dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Street field is required."
    And I should see the validation message "The NearestCrossStreet field is required."
    When I enter "123" into the StreetNumber field
    And I enter "eas" and select "N Easy St" from the Street autocomplete field 
    And I wait for ajax to finish loading
    And I enter "kid" and select "N Skid Row" from the NearestCrossStreet autocomplete field 
    And I wait for ajax to finish loading
    And I press Save
    When I select work order requester "call center" from the RequestedBy dropdown
    And I select work order purpose "customer" from the Purpose dropdown
    And I select work order priority "routine" from the Priority dropdown
    And I select markout requirement "none" from the MarkoutRequirement dropdown
    And I enter coordinate "one"'s Id into the CoordinateId field
    And I select asset type "sewer lateral" from the AssetType dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Description of Job field is required."
    And I should see the validation message "The Installation field is required."
    When I enter "1234567890" into the DeviceLocation field
    And I enter "1234567891" into the Installation field
    And I select work description "sewer lateral repair" from the WorkDescription dropdown
    And I press Save
    # would be nice if we could include the querystring here
    Then the currently shown work order will now be referred to as "new"
    And I should be at the FieldOperations/GeneralWorkOrder/Show page for work order "new"

Scenario: User can add a work order for a sewer main
    Given I am logged in as "user"
    And I am at the FieldOperations/WorkOrder/New page
    When I select "-- Select --" from the OperatingCenter dropdown
    And I press Save
    Then I should be at the FieldOperations/WorkOrder/New page
    And I should see the validation message "The OperatingCenter field is required."
    And I should see the validation message "Please enter the nearest (or customer) house number."
    And I should see the validation message "Please enter the location for this order using the globe icon."
    And I should see the validation message "The RequestedBy field is required."
    And I should see the validation message "The Purpose field is required."
    And I should see the validation message "The Priority field is required."
    And I should see the validation message "The MarkoutRequirement field is required."
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Town field is required."
    When I select town "nj7burg" from the Town dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Street field is required."
    And I should see the validation message "The NearestCrossStreet field is required."
    When I enter "123" into the StreetNumber field
    And I enter "eas" and select "N Easy St" from the Street autocomplete field 
    And I wait for ajax to finish loading
    And I enter "kid" and select "N Skid Row" from the NearestCrossStreet autocomplete field 
    And I wait for ajax to finish loading
    And I press Save
    When I select work order requester "call center" from the RequestedBy dropdown
    And I select work order purpose "customer" from the Purpose dropdown
    And I select work order priority "routine" from the Priority dropdown
    And I select markout requirement "none" from the MarkoutRequirement dropdown
    And I enter coordinate "one"'s Id into the CoordinateId field
    And I select asset type "sewer main" from the AssetType dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Description of Job field is required."
    When I select work description "sewer investigation main" from the WorkDescription dropdown
    And I press Save
    # would be nice if we could include the querystring here
    Then the currently shown work order will now be referred to as "new"
    And I should be at the FieldOperations/GeneralWorkOrder/Show page for work order "new"

Scenario: User can add a work order for a storm catch
    Given I am logged in as "user"
    And I am at the FieldOperations/WorkOrder/New page
    When I select "-- Select --" from the OperatingCenter dropdown
    And I press Save
    Then I should be at the FieldOperations/WorkOrder/New page
    And I should see the validation message "The OperatingCenter field is required."
    And I should see the validation message "Please enter the nearest (or customer) house number."
    And I should see the validation message "Please enter the location for this order using the globe icon."
    And I should see the validation message "The RequestedBy field is required."
    And I should see the validation message "The Purpose field is required."
    And I should see the validation message "The Priority field is required."
    And I should see the validation message "The MarkoutRequirement field is required."
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Town field is required."
    When I select town "nj7burg" from the Town dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Street field is required."
    And I should see the validation message "The NearestCrossStreet field is required."
    When I enter "123" into the StreetNumber field
    And I enter "eas" and select "N Easy St" from the Street autocomplete field 
    And I wait for ajax to finish loading
    And I enter "kid" and select "N Skid Row" from the NearestCrossStreet autocomplete field 
    And I wait for ajax to finish loading
    And I press Save
    When I select work order requester "call center" from the RequestedBy dropdown
    And I select work order purpose "customer" from the Purpose dropdown
    And I select work order priority "routine" from the Priority dropdown
    And I select markout requirement "none" from the MarkoutRequirement dropdown
    And I enter coordinate "one"'s Id into the CoordinateId field
    And I select asset type "storm catch" from the AssetType dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Description of Job field is required."
    When I select work description "storm catch investigation" from the WorkDescription dropdown
    And I press Save
    # would be nice if we could include the querystring here
    Then the currently shown work order will now be referred to as "new"
    And I should be at the FieldOperations/GeneralWorkOrder/Show page for work order "new"

Scenario: User can add a work order for a piece of equipment
    Given I am logged in as "user"
    And a role "roleFacilityReadNj7" exists with action: "Read", module: "ProductionFacilities", user: "user", operating center: "nj7"
    And a role "roleEquipmentReadNj7" exists with action: "Read", module: "ProductionEquipment", user: "user", operating center: "nj7"
    And I am at the FieldOperations/WorkOrder/New page
    When I select "-- Select --" from the OperatingCenter dropdown
    And I press Save
    Then I should be at the FieldOperations/WorkOrder/New page
    And I should see the validation message "The OperatingCenter field is required."
    And I should see the validation message "Please enter the nearest (or customer) house number."
    And I should see the validation message "Please enter the location for this order using the globe icon."
    And I should see the validation message "The RequestedBy field is required."
    And I should see the validation message "The Purpose field is required."
    And I should see the validation message "The Priority field is required."
    And I should see the validation message "The MarkoutRequirement field is required."
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Town field is required."
    When I select town "nj7burg" from the Town dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Street field is required."
    And I should see the validation message "The NearestCrossStreet field is required."
    When I enter "123" into the StreetNumber field
    And I enter "eas" and select "N Easy St" from the Street autocomplete field 
    And I wait for ajax to finish loading
    And I enter "kid" and select "N Skid Row" from the NearestCrossStreet autocomplete field 
    And I wait for ajax to finish loading
    And I press Save
    When I select work order requester "call center" from the RequestedBy dropdown
    And I select work order purpose "customer" from the Purpose dropdown
    And I select work order priority "routine" from the Priority dropdown
    And I select markout requirement "none" from the MarkoutRequirement dropdown
    And I enter coordinate "one"'s Id into the CoordinateId field
    And I select asset type "equipment" from the AssetType dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Description of Job field is required."
    And I should see the validation message "The Equipment field is required."
    When I select work description "misc repair" from the WorkDescription dropdown
    And I select equipment "one"'s Display from the Equipment dropdown
    And I press Save
    # would be nice if we could include the querystring here
    Then the currently shown work order will now be referred to as "new"
    And I should be at the FieldOperations/GeneralWorkOrder/Show page for work order "new"

Scenario: User can add a work order for a main crossing
    Given I am logged in as "user"
    And I am at the FieldOperations/WorkOrder/New page
    Then I should not see the validation message "The OperatingCenter field is required."
    When I select "-- Select --" from the OperatingCenter dropdown
    And I press Save
    Then I should be at the FieldOperations/WorkOrder/New page
    And I should see the validation message "The OperatingCenter field is required."
    And I should see the validation message "Please enter the nearest (or customer) house number."
    And I should see the validation message "Please enter the location for this order using the globe icon."
    And I should see the validation message "The RequestedBy field is required."
    And I should see the validation message "The Purpose field is required."
    And I should see the validation message "The Priority field is required."
    And I should see the validation message "The MarkoutRequirement field is required."
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Town field is required."
    When I select town "nj7burg" from the Town dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Street field is required."
    And I should see the validation message "The NearestCrossStreet field is required."
    When I enter "123" into the StreetNumber field
    And I enter "eas" and select "N Easy St" from the Street autocomplete field 
    And I wait for ajax to finish loading
    And I enter "kid" and select "N Skid Row" from the NearestCrossStreet autocomplete field 
    And I wait for ajax to finish loading
    And I press Save
    When I select work order requester "call center" from the RequestedBy dropdown
    And I select work order purpose "customer" from the Purpose dropdown
    And I select work order priority "routine" from the Priority dropdown
    And I select markout requirement "none" from the MarkoutRequirement dropdown
    And I enter coordinate "one"'s Id into the CoordinateId field
    And I select asset type "main crossing" from the AssetType dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Description of Job field is required."
    And I should see the validation message "The MainCrossing field is required."
    When I select work description "crossing investigation" from the WorkDescription dropdown
    And I select main crossing "one"'s Description from the MainCrossing dropdown
    And I wait for ajax to finish loading
    And I press Save
    # would be nice if we could include the querystring here
    Then the currently shown work order will now be referred to as "new"
    And I should be at the FieldOperations/GeneralWorkOrder/Show page for work order "new"

Scenario: User cannot select a revisit description for a non-revisit order
    Given a work order "revisit" exists with operating center: "nj7", town: "nj7burg", street: "one", hydrant: "one", asset type: "hydrant", work description: "hydrant leaking", coordinate: "one"
    And I am logged in as "user"
    When I visit the Edit page for work order "revisit"
    Then I should not see work description "hydrant landscaping" in the WorkDescription dropdown
    And I should see work description "hydrant leaking" in the WorkDescription dropdown

Scenario: User cannot select an initial description for a revisit order
    Given I do not currently function
    # this test is flaky as of 2021-02-03
    #Given a work order "revisit" exists with asset type: "hydrant", work description: "hydrant landscaping"
    #And I am logged in as "user"
    #When I visit the Edit page for work order "revisit"
    #Then I should not see work description "hydrant leaking" in the WorkDescription dropdown
    #And I should see work description "hydrant landscaping" in the WorkDescription dropdown

Scenario: User is prompted about the sample site when creating a work order with a sample site
    Given I am logged in as "user"
    And a premise "one" exists with premise number: "1234567890", service address house number: "7", service address apartment: "garbage", service address street: "EaSy St", service address fraction: "1/2", equipment: "123", meter serial number: "123", operating center: "nj7"
    And a sample site "one" exists with premise: "one", town: "nj7burg", premise number: "1234567890", sample site name: "Cravings", operating center: "nj7"
    And I am at the FieldOperations/WorkOrder/New page
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I select town "nj7burg" from the Town dropdown
    And I enter "123" into the StreetNumber field
    And I enter "eas" and select "N Easy St" from the Street autocomplete field 
    And I wait for ajax to finish loading
    And I enter "kid" and select "N Skid Row" from the NearestCrossStreet autocomplete field 
    And I wait for ajax to finish loading
    And I select work order requester "call center" from the RequestedBy dropdown
    And I select work order purpose "customer" from the Purpose dropdown
    And I select work order priority "routine" from the Priority dropdown
    And I select markout requirement "none" from the MarkoutRequirement dropdown
    And I enter coordinate "one"'s Id into the CoordinateId field
    And I select asset type "service" from the AssetType dropdown
    And I wait for ajax to finish loading
    And I select work description "leak survey" from the WorkDescription dropdown
    And I enter "1234567890" into the DeviceLocation field
    And I enter "1234567891" into the Installation field
    And I click ok in the alert after typing "1234567890" into the PremiseNumber field
    And I press Save
    Then the currently shown work order will now be referred to as "new"
    And I should be at the FieldOperations/GeneralWorkOrder/Show page for work order "new"

Scenario: User is not prompted about the sample site when creating a work order without a sample site
    Given I am logged in as "user"
    And a premise "one" exists with premise number: "1234567890", service address house number: "7", service address apartment: "garbage", service address street: "EaSy St", service address fraction: "1/2", equipment: "123", meter serial number: "123", operating center: "nj7"
    And a premise "two" exists with premise number: "0987654321", service address house number: "7", service address apartment: "garbage", service address street: "EaSy St", service address fraction: "1/2", equipment: "123", meter serial number: "123", operating center: "nj7"
    And a sample site "one" exists with premise: "two", town: "nj7burg", premise number: "0987654321", sample site name: "Cravings", operating center: "nj7"
    And I am at the FieldOperations/WorkOrder/New page
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I select town "nj7burg" from the Town dropdown
    And I enter "123" into the StreetNumber field
    And I enter "eas" and select "N Easy St" from the Street autocomplete field 
    And I wait for ajax to finish loading
    And I enter "kid" and select "N Skid Row" from the NearestCrossStreet autocomplete field 
    And I wait for ajax to finish loading
    And I select work order requester "call center" from the RequestedBy dropdown
    And I select work order purpose "customer" from the Purpose dropdown
    And I select work order priority "routine" from the Priority dropdown
    And I select markout requirement "none" from the MarkoutRequirement dropdown
    And I enter coordinate "one"'s Id into the CoordinateId field
    And I select asset type "service" from the AssetType dropdown
    And I wait for ajax to finish loading
    When I enter "1234567890" into the DeviceLocation field
    And I enter "1234567891" into the Installation field
    And I select work description "leak survey" from the WorkDescription dropdown
    And I enter "1234567890" into the PremiseNumber field
    And I press Save
    Then the currently shown work order will now be referred to as "new"
    And I should be at the FieldOperations/GeneralWorkOrder/Show page for work order "new"

Scenario: User is prompted about the sample site when editing a work order with a sample site
    Given I am logged in as "user"
    And a work order "wo-01" exists with operating center: "nj7", town: "nj7burg", street: "one", hydrant: "one", asset type: "hydrant", work description: "hydrant leaking", coordinate: "one"
    And a premise "one" exists with premise number: "1234567890", service address house number: "7", service address apartment: "garbage", service address street: "EaSy St", service address fraction: "1/2", equipment: "123", meter serial number: "123", operating center: "nj7"
    And a sample site "one" exists with premise: "one", town: "nj7burg", premise number: "1234567890", sample site name: "Cravings", operating center: "nj7"
    When I visit the Edit page for work order "wo-01"
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I select town "nj7burg" from the Town dropdown
    And I enter "123" into the StreetNumber field
    And I enter "eas" and select "N Easy St" from the Street autocomplete field 
    And I wait for ajax to finish loading
    And I enter coordinate "one"'s Id into the CoordinateId field
    And I select asset type "service" from the AssetType dropdown
    And I wait for ajax to finish loading
    And I click ok in the alert after typing "1234567890" into the PremiseNumber field
    When I enter "1234567890" into the DeviceLocation field
    And I enter "1234567891" into the Installation field
    And I select work description "leak survey" from the WorkDescription dropdown
    And I enter coordinate "one"'s Id into the CoordinateId field
    And I press Save
    Then I should see "No historical work orders found for selected asset."

Scenario: User is not prompted about the sample site when editing a work order without a sample site
    Given I am logged in as "user"
    And a work order "wo-01" exists with operating center: "nj7", town: "nj7burg", street: "one", hydrant: "one", asset type: "hydrant", work description: "hydrant leaking", coordinate: "one"
    And a premise "one" exists with premise number: "1234567890", service address house number: "7", service address apartment: "garbage", service address street: "EaSy St", service address fraction: "1/2", equipment: "123", meter serial number: "123", operating center: "nj7"
    And a premise "two" exists with premise number: "0987654321", service address house number: "7", service address apartment: "garbage", service address street: "EaSy St", service address fraction: "1/2", equipment: "123", meter serial number: "123", operating center: "nj7"
    And a sample site "one" exists with premise: "two", town: "nj7burg", premise number: "0987654321", sample site name: "Cravings", operating center: "nj7"
    When I visit the Edit page for work order "wo-01"
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I select town "nj7burg" from the Town dropdown
    And I enter "123" into the StreetNumber field
    And I enter "eas" and select "N Easy St" from the Street autocomplete field 
    And I wait for ajax to finish loading
    And I enter coordinate "one"'s Id into the CoordinateId field
    And I select asset type "service" from the AssetType dropdown
    And I wait for ajax to finish loading
    And I select work description "leak survey" from the WorkDescription dropdown
    And I enter coordinate "one"'s Id into the CoordinateId field
    And I enter "1234567890" into the PremiseNumber field
    And I press Save
    Then I should see "No historical work orders found for selected asset."

Scenario: Digital as-built requirement is driven by work description on create
    Given I am logged in as "user"
    When I visit the FieldOperations/WorkOrder/New page
    Then the DigitalAsBuiltRequired field should be unchecked
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I select town "nj7burg" from the Town dropdown
    And I wait for ajax to finish loading
    And I enter "123" into the StreetNumber field
    And I enter "eas" and select "N Easy St" from the Street autocomplete field 
    And I enter "kid" and select "N Skid Row" from the NearestCrossStreet autocomplete field 
    And I select work order requester "call center" from the RequestedBy dropdown
    And I select work order purpose "customer" from the Purpose dropdown
    And I select work order priority "routine" from the Priority dropdown
    And I select markout requirement "none" from the MarkoutRequirement dropdown
    And I enter coordinate "one"'s Id into the CoordinateId field
    And I select asset type "service" from the AssetType dropdown
    And I wait for ajax to finish loading
    And I select work description "service line renewal" from the WorkDescription dropdown
    Then the DigitalAsBuiltRequired field should be checked
    When I enter "000000000" into the PremiseNumber field
    And I enter "0000000000" into the PremiseNumber field
    And I enter "1234567890" into the DeviceLocation field
    And I enter "1234567891" into the Installation field
    And I enter "asdf asdf asdf" into the Notes field
    And I press Save
    # would be nice if we could include the querystring here
    Then the currently shown work order will now be referred to as "new"
    And I should be at the FieldOperations/GeneralWorkOrder/Show page for work order "new"
    And the DigitalAsBuiltRequired value for work order "new" should be "True"

Scenario: Digital as-built requirement is driven by work description on update
    Given I am logged in as "user"
    And I am at the Edit page for work order "one"
    When I select work description "valve box replacement" from the WorkDescription dropdown
    Then the DigitalAsBuiltRequired field should be checked
    # can't take this any further, page makes a synchronous AJAX call on save which times out

Scenario: User can add a work order for a service with no device location and installation number and contracted opc
    Given I am logged in as "user"
    And an operating center "sap enabled contracted" exists with opcode: "NJ7-contracted", name: "Shrewsbury", is contracted operations: "true", sap enabled: "true", arc mobile map id: "15fdc279b4234fcb85f455ee70897a9e"
	And a town "ah" exists with name: "Loch Arbour"
	And operating center: "sap enabled contracted" exists in town: "ah" with abbreviation: "LA"
    And operating center: "sap enabled contracted" has asset type "service"
    And a role "workorder-read-sap-enabled-contracted" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user", operating center: "sap enabled contracted"
    And a street "three" exists with town: "ah", full st name: "EASY STREET", is active: true, name: "Third", prefix: "north", suffix: "st"
    And a street "four" exists with town: "ah", is active: true, name: "Forth", prefix: "north", suffix: "st"
    And I am at the FieldOperations/WorkOrder/New page
    When I select "-- Select --" from the OperatingCenter dropdown
    And I press Save
    Then I should be at the FieldOperations/WorkOrder/New page
    And I should see the validation message "The OperatingCenter field is required."
    And I should see the validation message "Please enter the nearest (or customer) house number."
    And I should see the validation message "Please enter the location for this order using the globe icon."
    And I should see the validation message "The RequestedBy field is required."
    And I should see the validation message "The Purpose field is required."
    And I should see the validation message "The Priority field is required."
    And I should see the validation message "The MarkoutRequirement field is required."
    When I select operating center "sap enabled contracted" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Town field is required."
    When I select town "ah" from the Town dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Street field is required."
    And I should see the validation message "The NearestCrossStreet field is required."
    When I enter "123" into the StreetNumber field
    And I enter "thi" and select "N Third St" from the Street autocomplete field 
    And I wait for ajax to finish loading
    And I enter "for" and select "N Forth St" from the NearestCrossStreet autocomplete field 
    And I wait for ajax to finish loading
    And I press Save
    When I select work order requester "call center" from the RequestedBy dropdown
    And I select work order purpose "customer" from the Purpose dropdown
    And I select work order priority "routine" from the Priority dropdown
    And I select markout requirement "none" from the MarkoutRequirement dropdown
    And I enter coordinate "one"'s Id into the CoordinateId field
    And I select asset type "service" from the AssetType dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Description of Job field is required."
    And I should see the validation message "The PremiseNumber field is required."
    And I should not see the validation message "The DeviceLocation field is required."
    And I should not see the validation message "The Installation field is required."
    When I select work description "leak survey" from the WorkDescription dropdown
    And I enter "1234567890" into the PremiseNumber field
    And I press Save
    Then the currently shown work order will now be referred to as "new"
    And I should be at the FieldOperations/GeneralWorkOrder/Show page for work order "new"

Scenario: User can add a work order for a service with no device location and installation number and placeholder premise
    Given I am logged in as "user"
    And I am at the FieldOperations/WorkOrder/New page
    When I select "-- Select --" from the OperatingCenter dropdown
    And I press Save
    Then I should be at the FieldOperations/WorkOrder/New page
    And I should see the validation message "The OperatingCenter field is required."
    And I should see the validation message "Please enter the nearest (or customer) house number."
    And I should see the validation message "Please enter the location for this order using the globe icon."
    And I should see the validation message "The RequestedBy field is required."
    And I should see the validation message "The Purpose field is required."
    And I should see the validation message "The Priority field is required."
    And I should see the validation message "The MarkoutRequirement field is required."
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Town field is required."
    When I select town "nj7burg" from the Town dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Street field is required."
    And I should see the validation message "The NearestCrossStreet field is required."
    When I enter "123" into the StreetNumber field
    And I enter "eas" and select "N Easy St" from the Street autocomplete field 
    And I wait for ajax to finish loading
    And I enter "kid" and select "N Skid Row" from the NearestCrossStreet autocomplete field 
    And I wait for ajax to finish loading
    And I press Save
    When I select work order requester "call center" from the RequestedBy dropdown
    And I select work order purpose "customer" from the Purpose dropdown
    And I select work order priority "routine" from the Priority dropdown
    And I select markout requirement "none" from the MarkoutRequirement dropdown
    And I enter coordinate "one"'s Id into the CoordinateId field
    And I select asset type "service" from the AssetType dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Description of Job field is required."
    And I should see the validation message "The PremiseNumber field is required."
    And I should see the validation message "The DeviceLocation field is required."
    And I should see the validation message "The Installation field is required."
    When I select work description "leak survey" from the WorkDescription dropdown
    And I enter "000000000" into the PremiseNumber field
    And I press Save
    Then I should see the validation message "The field PremiseNumber must be a string with a minimum length of 10 and a maximum length of 10."
    When I enter "0000000000" into the PremiseNumber field
    And I press Save
    Then I should see the validation message "The premise number is not valid. Please enter notes explaining why a placeholder premise number was used."
    When I enter "asdf asdf asdf" into the Notes field
    And I press Save
    Then the currently shown work order will now be referred to as "new"
    And I should be at the FieldOperations/GeneralWorkOrder/Show page for work order "new"

Scenario: User can create a revisit work order for a valve with data populating from original work order
    Given a work order "original" exists with operating center: "nj7", town: "nj7burg", town section: "one", street: "one", apartment addtl: "Testing Additional Apartment", asset type: "valve", valve: "one", zip code: "85023", work order requester: "customer", secondary phone number: "123-456-7890", work description: "water main break repair", customer name: "Smith", significant traffic impact: "true", alert id: "4567", s o p required: "true", markout requirement: "none", created by: "user", completed by: "user", sap notification number: "123456789", sap work order number: "987654321", approved on: "03/12/2023", materials approved on: "03/15/2023", material planning completed on: "03/18/2023", s a p error code: "Success", approved by: "user", materials doc id: "122333444", material posting date: "03/10/2023 10:10:25 AM", materials approved by: "user", business unit: "Liberty", date completed: "03/22/2023"
    And a street opening permit "one" exists with StreetOpeningPermitNumber: "1234", DateRequested: "01/01/2021", DateIssued: "01/02/2021", WorkOrder: "original"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrder/New page
    When I select "Revisit" from the IsRevisit dropdown
    And I enter work order "original"'s Id into the OriginalOrderNumber field 
    And I wait for ajax to finish loading
    Then the OperatingCenter field should be disabled
    And the Town field should be disabled
    And the TownSection field should be disabled
    And the Street field should be disabled
    And the NearestCrossStreet field should be disabled
    And the ZipCode field should be disabled
    And the StreetNumber field should be disabled
    When I enter coordinate "one"'s Id into the CoordinateId field
    And I select work order requester "call center" from the RequestedBy dropdown
    And I select work order purpose "customer" from the Purpose dropdown
    And I select work order priority "routine" from the Priority dropdown
    And I select markout requirement "none" from the MarkoutRequirement dropdown
    And I select work description "valve restoration repair" from the WorkDescription dropdown
    And I press Save
    Then the currently shown work order will now be referred to as "new"
    And I should be at the FieldOperations/GeneralWorkOrder/Show page for work order "new"

Scenario: User can create a revisit work order for a hydrant with data populating from original work order
    Given a work order "original" exists with operating center: "nj7", town: "nj7burg", town section: "one", street: "one", apartment addtl: "Testing Additional Apartment", asset type: "hydrant", hydrant: "one", zip code: "85023", work order requester: "customer", secondary phone number: "123-456-7890", work description: "water main break repair", customer name: "Smith", significant traffic impact: "true", alert id: "4567", s o p required: "true", markout requirement: "none", created by: "user", completed by: "user", sap notification number: "123456789", sap work order number: "987654321", approved on: "03/12/2023", materials approved on: "03/15/2023", material planning completed on: "03/18/2023", s a p error code: "Success", approved by: "user", materials doc id: "122333444", material posting date: "03/10/2023 10:10:25 AM", materials approved by: "user", business unit: "Liberty", date completed: "03/22/2023"
    And a markout "one" exists with WorkOrder: "original", DateOfRequest: "02/20/2023", ReadyDate: "02/21/2023", ExpirationDate: "02/28/2023"
    And a markout "two" exists with WorkOrder: "original", DateOfRequest: "03/10/2023", ReadyDate: "03/12/2023", ExpirationDate: "03/19/2023"
    And a street opening permit "one" exists with StreetOpeningPermitNumber: "1234", DateRequested: "01/01/2021", DateIssued: "01/02/2021", WorkOrder: "original"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrder/New page
    When I select "Revisit" from the IsRevisit dropdown
    And I enter work order "original"'s Id into the OriginalOrderNumber field 
    And I wait for ajax to finish loading
    Then the OperatingCenter field should be disabled
    And the Town field should be disabled
    And the TownSection field should be disabled
    And the Street field should be disabled
    And the NearestCrossStreet field should be disabled
    And the ZipCode field should be disabled
    And the StreetNumber field should be disabled
    When I enter coordinate "one"'s Id into the CoordinateId field
    And I select work order requester "call center" from the RequestedBy dropdown
    And I select work order purpose "customer" from the Purpose dropdown
    And I select work order priority "routine" from the Priority dropdown
    And I select markout requirement "none" from the MarkoutRequirement dropdown
    And I select work description "hydrant restoration repair" from the WorkDescription dropdown
    And I press Save
    Then the currently shown work order will now be referred to as "new"
    And I should be at the FieldOperations/GeneralWorkOrder/Show page for work order "new"

Scenario: User cannot create a work order with incorrect streets
    Given a town "nearby" exists
    And operating center: "nj7" exists in town: "nearby"
    And a street "oneContinued" exists with town: "nearby", full st name: "EASY STREET", is active: true, name: "Easy", prefix: "north", suffix: "st"
    And a street "twoContinued" exists with town: "nearby", is active: true, full st name: "HIGH STREET", name: "Skid", prefix: "north", suffix: "row"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrder/New page
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I select town "nj7burg" from the Town dropdown
    And I enter "123" into the StreetNumber field
    And I enter "eas" and select "N Easy St" from the Street autocomplete field 
    And I wait for ajax to finish loading
    And I enter "kid" and select "N Skid Row" from the NearestCrossStreet autocomplete field 
    And I wait for ajax to finish loading
    And I select town "nearby" from the Town dropdown
    Then I should not see "N Easy St" in the Street_AutoComplete field
    And I should not see "N Skid Row" in the NearestCrossStreet_AutoComplete field
    When I enter "eas" and select "N Easy St" from the Street autocomplete field 
    And I wait for ajax to finish loading
    And I enter "kid" and select "N Skid Row" from the NearestCrossStreet autocomplete field 
    And I wait for ajax to finish loading
    And I select work order requester "call center" from the RequestedBy dropdown
    And I select work order purpose "customer" from the Purpose dropdown
    And I select work order priority "routine" from the Priority dropdown
    And I select markout requirement "none" from the MarkoutRequirement dropdown
    And I enter coordinate "one"'s Id into the CoordinateId field
    And I select asset type "main" from the AssetType dropdown
    And I select work description "new main flushing" from the WorkDescription dropdown
    And I wait for ajax to finish loading
    And I select work description "main investigation" from the WorkDescription dropdown
    And I press Save
    Then the currently shown work order will now be referred to as "new"
    And I should be at the FieldOperations/GeneralWorkOrder/Show page for work order "new"

Scenario: user can view and not edit valve details
    Given I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "one"
    When I click the "Valve" tab
    Then I should not see the valveEditButton element 
    When I switch to the valveFrame frame
    Then I should see a display for ValveType with ""

Scenario: user can view and edit valve details
    Given a role "asset-useradmin" exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user", operating center: "nj7"
    And a valve type "one" exists with description: "GATE"
    And a valve normal position "one" exists with description: "CLOSED"
    And a functional location "one" exists with town: "nj7burg", asset type: "valve"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "one"
    When I click the "Valve" tab
    When I press "valveEditButton"
    And I switch to the valveFrame frame
    And I press Save
    Then I should see a validation message for ValveType with "Valve Type is required for active / installed valves." 
    When I select valve type "one" from the ValveType dropdown
    And I select valve normal position "one" from the NormalPosition dropdown
    And I select functional location "one" from the FunctionalLocation dropdown
    And I enter today's date into the DateInstalled field
    And I enter 4 into the Turns field
    And I press Save
    Then I should see a display for ValveType with valve type "one"
    And I should see a display for NormalPosition with valve normal position "one"

Scenario: user can view and not edit hydrant details
    Given a hydrant "hydrant" exists with street: "one", town: "nj7burg", operating center: "nj7", hydrant suffix: "42" 
    And a work order "hydrant" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "hydrant", hydrant: "hydrant"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "hydrant"
    When I click the "Hydrant" tab
    Then I should not see the hydrantEditButton element 
    When I switch to the hydrantFrame frame
    Then I should see a display for OperatingCenter with operating center "nj7"
    And I should see a display for HydrantSuffix with "42"

Scenario: user can view and edit hydrant details
    Given a role "asset-useradmin" exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user", operating center: "nj7"
    And a hydrant "hydrant" exists with street: "one", town: "nj7burg", operating center: "nj7", hydrant suffix: "2", hydrant number: "HAB-2"
    And a work order "hydrant" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "hydrant", hydrant: "hydrant"
    And a functional location "one" exists with town: "nj7burg", asset type: "hydrant"
    And a fire district "one" exists with district name: "meh"
    And a fire district town "foo" exists with town: "nj7burg", fire district: "one"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "hydrant"
    When I click the "Hydrant" tab
    When I press "hydrantEditButton"
    And I switch to the hydrantFrame frame
    And I select fire district "one" from the FireDistrict dropdown
    And I select functional location "one" from the FunctionalLocation dropdown
    And I press Save
    And I wait for the page to reload
    Then I should see a display for FunctionalLocation with functional location "one"
    Then I should see a display for OperatingCenter with operating center "nj7"

Scenario: user can view and not edit sewer opening details
    Given a sewer opening "opening" exists with operating center: "nj7", town: "nj7burg", street: "one", opening number: "MAD-42"
    And a work order "opening" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "sewer opening", sewer opening: "opening"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "opening"
    When I click the "Sewer Opening" tab
    Then I should not see the sewerOpeningEditButton element 
    When I switch to the sewerOpeningFrame frame
    Then I should see a display for OpeningNumber with "MAD-42"

Scenario: user can view and edit sewer opening details
    Given a role "asset-useradmin" exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user", operating center: "nj7"
    And a sewer opening "opening" exists with operating center: "nj7", town: "nj7burg", street: "one", opening number: "MAD-42"
    And a work order "opening" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "sewer opening", sewer opening: "opening"
    And a functional location "one" exists with town: "nj7burg", asset type: "sewer opening"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "opening"
    When I click the "Sewer Opening" tab
    When I press "sewerOpeningEditButton"
    And I switch to the sewerOpeningFrame frame
    And I select functional location "one" from the FunctionalLocation dropdown
    And I press Save
    Then I should see a display for FunctionalLocation with functional location "one"

Scenario: user can view and not edit service details
    Given a sewer opening "opening" exists with operating center: "nj7", town: "nj7burg", street: "one", opening number: "MAD-42"
	And a service category "one" exists with description: "Neato"
    And a service "unique" exists with service number: "123456", premise number: "9876543", date installed: "4/24/1984", service category: "one"
    And a work order "opening" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "sewer opening", sewer opening: "opening", service: "unique"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "opening"
    When I click the "Service" tab
    Then I should not see the serviceEditButton element 
    When I switch to the serviceFrame frame
    Then I should see a display for ServiceType with service "unique"'s ServiceType

Scenario: UserAdmin can view and edit service details
    Given a role "asset-useradmin" exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user", operating center: "nj7"
    And a sewer opening "opening" exists with operating center: "nj7", town: "nj7burg", street: "one", opening number: "MAD-42"
    And a service category "one" exists with description: "Neato"
    And a service "unique" exists with service number: "123456", date installed: "4/24/1984", service category: "one"
    And a work order "opening" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "sewer opening", sewer opening: "opening", service: "unique"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "opening"
    When I click the "Service" tab
    When I press "serviceEditButton"
    When I switch to the serviceFrame frame
    And I select town "nj7burg" from the Town dropdown
    And I follow "Cancel"
    Then I should see a display for ServiceNumber with "123456"

Scenario: user can create a work order requested by employee and it defaults to their username
    Given I am logged in as "user"
    And I am at the Show page for hydrant "one"
    When I click the "Work Orders" tab
    And I follow "Create New Work Order"
    And I select work order requester "employee" from the RequestedBy dropdown
    And I press Save
    Then I should not see a validation message for RequestingEmployee with "The RequestingEmployee field is required."

Scenario: user is not presented with an asset type when visiting the new page
    Given I am logged in as "user"
    And I am at the FieldOperations/WorkOrder/New page
    Then "-- Select --" should be selected in the AssetType dropdown