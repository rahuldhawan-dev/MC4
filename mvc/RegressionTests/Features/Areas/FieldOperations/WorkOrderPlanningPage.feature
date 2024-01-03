Feature: WorkOrderPlanningPage

Background:
    Given an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", sap enabled: "true", sap work orders enabled: "true"
    And a town "nj7burg" exists
    And operating center: "nj7" exists in town: "nj7burg"
    And a town section "one" exists with town: "nj7burg"
    And a town section "inactive" exists with town: "nj7burg", active: false
    And a street "one" exists with town: "nj7burg", full st name: "Easy St", is active: true
    And a street "two" exists with town: "nj7burg", is active: true, full st name: "HIGH STREET"
    And an asset type "valve" exists with description: "valve"
    And operating center: "nj7" has asset type "valve"
    And an asset type "hydrant" exists with description: "hydrant"
    And operating center: "nj7" has asset type "hydrant"
    And a functional location "one" exists with town: "nj7burg", asset type: "valve"
    And a valve "one" exists with operating center: "nj7", town: "nj7burg", street: "one", date installed: "yesterday", turns: 4, functional location: "one"
    And work order requesters exist
    And work order purposes exist
    And work order priorities exist
    And work descriptions exist
    And markout requirements exist
    And an asset type "sewer opening" exists with description: "sewer opening"
    And operating center: "nj7" has asset type "sewer opening"
    And an admin user "admin" exists with username: "admin"
    And a user "user" exists with username: "user"
    And a role "workorder-read" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
    And a role "workorder-add" exists with action: "Add", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
    And a role "workorder-edit" exists with action: "Edit", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
    And a role "asset-read" exists with action: "Read", module: "FieldServicesAssets", user: "user", operating center: "nj7"
    And a planning work order "requires markout" exists with operating center: "nj7", sap notification number: "222222", sap work order number: "111111", date received: "12/01/2019", town: "nj7burg", work description: "hydrant flushing", work order priority: "emergency", work order purpose: "customer", markout requirement: "routine", town section: "one", s o p required: false
    And a planning work order "requires sop" exists with valve: "one", asset type: "valve", operating center: "nj7", sap notification number: "333333", sap work order number: "222222", date received: "12/01/2019", town: "nj7burg", work description: "valve repair", work order priority: "emergency", work order purpose: "customer", markout requirement: "none", town section: "one", s o p required: true

Scenario: user can search for orders and add markouts
    Given a markout type "one" exists with description: "C TO C"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderPlanning/Search page
    When I select markout requirement "routine" from the MarkoutRequirement dropdown
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I press "Search"
    Then I should be at the FieldOperations/WorkOrderPlanning page
    And I should see a link to the show page for planning work order "requires markout"
    And I should not see a link to the show page for planning work order "requires sop"
    When I follow the show link for planning work order "requires markout"
    Then I should be at the show page for planning work order "requires markout"
    And I should not see the "Street Opening Permits" tab
    When I click the "Markouts" tab
    And I follow "Add New Markout"
    And I wait for the dialog to open
    And I enter "987654321" into the MarkoutNumber field
    And I select markout type "one" from the MarkoutType dropdown
    And I press "Save Markout"
    Then I should see the following values in the markoutsTable table
      | Markout Type | Date Of Request | Note       |
      | C TO C       | today           |            |

Scenario: user can search for orders and add street opening permits
    Given I am logged in as "user"
    And I am at the FieldOperations/WorkOrderPlanning/Search page
    When I select "Required" from the StreetOpeningPermitRequired dropdown
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I press "Search"
    Then I should be at the FieldOperations/WorkOrderPlanning page
    And I should see a link to the show page for planning work order "requires sop"
    And I should not see a link to the show page for planning work order "requires markout"
    When I follow the show link for planning work order "requires sop"
    Then I should be at the show page for planning work order "requires sop"
    And I should not see the "Markouts" tab
    When I click the "Street Opening Permits" tab
    And I follow "Add Street Opening Permit"
    And I wait for the dialog to open
    And I press "Save Permit"
    Then I should see a validation message for StreetOpeningPermitNumber with "The Permit # field is required."
    And I should see a validation message for DateRequested with "The DateRequested field is required."
    When I type "1234567890" into the StreetOpeningPermitNumber field
    And I enter today's date into the DateRequested field
    And I press "Save Permit"
    And I wait for ajax to finish loading
    Then I should see the following values in the streetOpeningPermitsTable table
      | Permit #   | Date Requested |
      | 1234567890 | today          |

Scenario: user with edit access can update notes and traffic control information
    Given I am logged in as "user"
    And I am at the show page for planning work order "requires sop"
    Then I should not see a link to the edit page for planning work order "requires sop"
    When I click the "Traffic Control/Notes" tab
    And I check the TrafficControlRequiredPlanning field
    And I enter 3 into the NumberOfOfficersRequired field
    And I enter "these are my new notes" into the AdditionalNotes field
    And I press "Update"
    And I wait for the page to reload
    #if we weren't redirected to the Traffic Control/Notes tab these would fail
    Then I should see "3" in the NumberOfOfficersRequired field
    And I should see a display for DisplayWorkOrder_Notes containing "(EST) these are my new notes"
    And the TrafficControlRequiredPlanning field should be checked
    When I press btnEdit
    Then I should be at the FieldOperations/GeneralWorkOrder/Edit page for planning work order "requires sop"

Scenario: user can view and not edit hydrant details
    Given a hydrant "hydrant" exists with street: "one", town: "nj7burg", operating center: "nj7", hydrant suffix: "42" 
    And a work order "hydrant" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "hydrant", hydrant: "hydrant"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "hydrant"
    When I click the "Hydrant" tab
    Then I should not see the hydrantEditButton element 
    When I switch to the hydrantFrame frame
    Then I should see a display for HydrantSuffix with "42"

Scenario: user can view and edit hydrant details
    Given a role "asset-useradmin" exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user", operating center: "nj7"
    And a hydrant "hydrant" exists with street: "one", town: "nj7burg", operating center: "nj7", hydrant suffix: "42", hydrant number: "HAB-42"
    And a work order "hydrant" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "hydrant", hydrant: "hydrant"
    And a functional location "meow" exists with town: "nj7burg", asset type: "hydrant"
    And a fire district "one" exists with district name: "meh"
    And a fire district town "foo" exists with town: "nj7burg", fire district: "one"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "hydrant"
    When I click the "Hydrant" tab
    When I press "hydrantEditButton"
    And I switch to the hydrantFrame frame
    And I select fire district "one" from the FireDistrict dropdown
    And I select functional location "meow" from the FunctionalLocation dropdown
    And I press Save
    Then I should see a display for FunctionalLocation with functional location "meow"

Scenario: user can view and not edit valve details
    Given I am logged in as "user"
    And I am at the show page for planning work order "requires sop"
    When I click the "Valve" tab
    Then I should not see the valveEditButton element 
    When I switch to the valveFrame frame
    Then I should see a display for ValveType with ""

Scenario: user can view and edit valve details
    Given a role "asset-useradmin" exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user", operating center: "nj7"
    And a valve type "one" exists with description: "GATE"
    And a valve normal position "one" exists with description: "CLOSED"
    And a asset status "retired" exists with description: "RETIRED", is user admin only: "true"
    And I am logged in as "user"
    And I am at the show page for planning work order "requires sop"
    When I click the "Valve" tab
    When I press "valveEditButton"
    And I switch to the valveFrame frame
    And I press Save
    Then I should see a validation message for ValveType with "Valve Type is required for active / installed valves." 
    When I select valve type "one" from the ValveType dropdown
    And I select valve normal position "one" from the NormalPosition dropdown
    And I enter "208" into the ValveSuffix field
    And I enter "VAB-208" into the ValveNumber field
    And I press Save
    Then I should see a display for ValveType with valve type "one"
    And I should see a display for NormalPosition with valve normal position "one"

Scenario: admin can search and change planned completion by dates for work orders with emergency priority
	Given I am logged in as "admin"
	And I am at the FieldOperations/WorkOrderPlanning/Search page
	When I select operating center "nj7"'s Description from the OperatingCenter dropdown
	And I press "Search"
	Then I should be at the FieldOperations/WorkOrderPlanning page
	Then I should see the following values in the workOrdersTable table
	  | Order # |  Planned Completion Date (back office use only) |
	  | 1       |                                                 |
	  | 2       |                                                 |
	When I click the checkbox named WorkOrderIds with planning work order "requires markout"'s Id
	And I click the checkbox named WorkOrderIds with planning work order "requires sop"'s Id
    And I enter the date "yesterday" into the PlannedCompletionDate field
    And I press "Update"
    Then I should see the validation message "Planned Completion Date must be either current or future date."
	When I enter the date "3 days from now" into the PlannedCompletionDate field
	And I click cancel in the dialog after pressing "Update"
    And I enter the date "2 days from now" into the PlannedCompletionDate field
	And I click ok in the dialog after pressing "Update"
	And I wait for the page to reload
	Then I should see the following values in the workOrdersTable table
	  | Order # |  Planned Completion Date (back office use only) |
	  | 1       | 2 days from now                                 |
	  | 2       | 2 days from now                                 |

Scenario: admin can search and change planned completion dates for work orders with Routine priority
	Given I am logged in as "admin"
    And a planning work order "routine" exists with valve: "one", asset type: "valve", operating center: "nj7", sap notification number: "333333", sap work order number: "222222", date received: "12/01/2019", town: "nj7burg", work description: "valve repair", work order priority: "routine", work order purpose: "customer", markout requirement: "none", town section: "one", s o p required: true
	And I am at the FieldOperations/WorkOrderPlanning/Search page
	When I select operating center "nj7"'s Description from the OperatingCenter dropdown
	And I press "Search"
	Then I should be at the FieldOperations/WorkOrderPlanning page
	Then I should see the following values in the workOrdersTable table
	  | Order # |  Planned Completion Date (back office use only) |
	  | 1       |                                                 |
	  | 2       |                                                 |
      | 3       |                                                 |
	When I click the checkbox named WorkOrderIds with planning work order "routine"'s Id
    And I enter the date "yesterday" into the PlannedCompletionDate field
    And I press "Update"    
    Then I should see the validation message "The field Planned Completion Date must be greater than or equal to today for an Emergency priority. For all other priorities it must be between 2 days from now and 12/31/9999 11:59:59 PM."
    When I enter the date "3 days from now" into the PlannedCompletionDate field
    And I press "Update"  
    And I wait for the page to reload
	Then I should see the following values in the workOrdersTable table
	  | Order # |  Planned Completion Date (back office use only) |
	  | 1       |                                                 |
	  | 2       |                                                 |
      | 3       | 3 days from now                                 |
    When I click the checkbox named WorkOrderIds with planning work order "requires markout"'s Id
	And I click the checkbox named WorkOrderIds with planning work order "requires sop"'s Id
    And I click the checkbox named WorkOrderIds with planning work order "routine"'s Id
	When I enter the date "yesterday" into the PlannedCompletionDate field
    And I press "Update"
    Then I should see the validation message "The field Planned Completion Date must be greater than or equal to today for an Emergency priority. For all other priorities it must be between 2 days from now and 12/31/9999 11:59:59 PM."
	When I enter the date "3 days from now" into the PlannedCompletionDate field
    And I click cancel in the dialog after pressing "Update"
    And I enter the date "2 days from now" into the PlannedCompletionDate field
	And I click ok in the dialog after pressing "Update"
	And I wait for the page to reload
	Then I should see the following values in the workOrdersTable table
	  | Order # | Planned Completion Date (back office use only) |
	  | 1       | 2 days from now                                |
	  | 2       | 2 days from now                                |
      | 3       | 2 days from now                                |

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
    And a asset status "retired" exists with description: "RETIRED", is user admin only: "true"
    And a functional location "meow" exists with town: "nj7burg", asset type: "sewer opening"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "opening"
    When I click the "Sewer Opening" tab
    When I press "sewerOpeningEditButton"
    And I switch to the sewerOpeningFrame frame
    And I select functional location "meow" from the FunctionalLocation dropdown
    And I press Save
    Then I should see a display for FunctionalLocation with functional location "meow"

Scenario: User can view special instruction on work order planning show page
	Given I am logged in as "user"
	And I am at the show page for planning work order "requires sop"
	Then I should only see planning work order "requires sop"'s SpecialInstructions in the WorkOrderSpecialInstructions element

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
