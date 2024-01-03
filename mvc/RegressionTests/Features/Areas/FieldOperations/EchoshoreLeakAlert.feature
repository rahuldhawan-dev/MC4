Feature: EchoshoreLeakAlert

Background:
	Given an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a town "one" exists with name: "King's Head"
	And a town section "one" exists with name: "A section", town: "one"
	And operating center: "nj7" exists in town: "one"
	And a street "one" exists with town: "one", is active: true
	And a street "two" exists with town: "one", is active: true
	And a hydrant "one" exists with operating center: "nj7", town: "one", town section: "one", street number: "228", street: "one", cross street: "two"
	And an asset type "hydrant" exists with description: "hydrant"
	And operating center "nj7" has asset type "hydrant"
    And a user "user" exists with username: "user"
	And an admin user "admin" exists with username: "admin"
	And a role "role-read" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a role "role-add" exists with action: "Add", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a role "role-read-assets" exists with action: "Read", module: "FieldServicesAssets", user: "user"
	And an echoshore site "one" exists with operating center: "nj7", town: "one"
	And an echoshore leak alert "one" exists with echoshore site: "one", hydrant 1: "one", persisted correlated noise id: 2, note: "OG", date p c n created: "11/29/2022", field investigation recommended on: "11/30/2022"

Scenario: User can search for a leak alert view it and create a work order from it
	Given I am logged in as "user"
	And I am at the FieldOperations/EchoshoreLeakAlert/Search page
	When I press Search
	Then I should see a link to the Show page for echoshore leak alert: "one"
	When I follow the Show link for echoshore leak alert "one"
	Then I should see a display for PersistedCorrelatedNoiseId with echoshore leak alert "one"'s PersistedCorrelatedNoiseId
	When I click the "Work Orders" tab
	And I follow "Create New Work Order"
	And I wait for ajax to finish loading
    Then I should be at the FieldOperations/WorkOrder/New page
	And operating center "nj7"'s ToString should be selected in the OperatingCenter dropdown
	And town "one"'s ToString should be selected in the Town dropdown
	And town section "one"'s ToString should be selected in the TownSection dropdown
	And I should see hydrant "one"'s StreetNumber in the StreetNumber field
	And I should see street "one"'s FullStName in the Street_AutoComplete field
	And I should see street "two"'s FullStName in the NearestCrossStreet_AutoComplete field
	And asset type "hydrant"'s ToString should be selected in the AssetType dropdown
	And I should see "Created from Echoshore Leak Alert 2 for Hydrants: Hydrant1 Text, Hydrant2 Text. At site: NJ7 - King's Head - EchoshoreSite. Notes: OG. Distance from Hydrant 1: 0. Distance from Hydrant 2: 0" in the Notes field
	And I should see echoshore leak alert "one"'s Id in the EchoshoreLeakAlert field
	And I should see "11/30/2022" in the DateReceived field
