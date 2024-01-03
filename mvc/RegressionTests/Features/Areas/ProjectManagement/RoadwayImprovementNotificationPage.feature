Feature: RoadwayImprovementNotificationPage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
	And an operating center "nj7" exists with opcode: "NJ7"
	And a town "one" exists
	And a street "one" exists with town: "one", full st name: "Easy St", is active: true
	And operating center: "nj7" exists in town: "one"
	And a roadway improvement notification entity "one" exists with description: "rine1"
	And a roadway improvement notification status "one" exists with description: "complete"
	And a coordinate "one" exists
	And a coordinate "two" exists
    And a roadway improvement notification "one" exists with town: "one"
    And a roadway improvement notification "two" exists with town: "one"   

Scenario: user can search for a roadway improvement notification
	Given I am logged in as "admin"
    When I visit the ProjectManagement/RoadwayImprovementNotification/Search page
    And I press Search
    Then I should see a link to the Show page for roadway improvement notification: "one"
    When I follow the Show link for roadway improvement notification "one"
    Then I should be at the Show page for roadway improvement notification: "one"

Scenario: user can view a roadway improvement notification
    Given I am logged in as "admin"
	When I visit the Show page for roadway improvement notification: "one"
    Then I should see a display for roadway improvement notification: "one"'s Description

Scenario: user can add a roadway improvement notification
	Given I am logged in as "admin"
    When I visit the ProjectManagement/RoadwayImprovementNotification/New page
    And I press Save
	Then I should see a validation message for Description with "The Description field is required."
	Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
	And I should see a validation message for RoadwayImprovementNotificationEntity with "The Entity field is required."
	And I should see a validation message for ExpectedProjectStartDate with "The ExpectedProjectStartDate field is required."
	And I should see a validation message for DateReceived with "The DateReceived field is required."
	And I should see a validation message for Coordinate with "The Coordinate field is required."
	And I should see a validation message for RoadwayImprovementNotificationStatus with "The Status field is required."
	When I select operating center "nj7" from the OperatingCenter dropdown
    And I enter "foo" into the Description field
	And I enter coordinate "one"'s Id into the Coordinate field
	And I select roadway improvement notification entity "one" from the RoadwayImprovementNotificationEntity dropdown
	And I select roadway improvement notification status "one" from the RoadwayImprovementNotificationStatus dropdown
	And I enter "12/8/1980" into the DateReceived field
	And I enter "12/8/2015" into the ExpectedProjectStartDate field
	And I press Save 
	Then I should see a validation message for Town with "The Town field is required."
	When I select town "one" from the Town dropdown
	And I press Save
	Then the currently shown roadway improvement notification shall henceforth be known throughout the land as "Sir Robin"
    And I should see a display for Description with "foo"
	And I should see a display for Town with town "one"
	And I should see a display for RoadwayImprovementNotificationEntity with roadway improvement notification entity "one"
	And I should see a display for ExpectedProjectStartDate with "12/8/2015"
	And I should see a display for DateReceived with "12/8/1980"
	And I should see a display for Coordinate with "40.32246702, -74.1481018"
	And I should see a display for RoadwayImprovementNotificationStatus with roadway improvement notification status "one"

Scenario: user can edit a roadway improvement notification
    Given I am logged in as "admin"
	When I visit the Edit page for roadway improvement notification: "one"
    And I enter "bar" into the Description field
	And I select town "one" from the Town dropdown
    And I press Save
    Then I should be at the Show page for roadway improvement notification: "one"
    And I should see a display for Description with "bar"

Scenario: user can add a street to a roadway improvement notification
	Given a main type "one" exists with description: "asbestos"
	And a main type "two" exists with description: "cement"
	And a main size "one" exists with description: "12"
	And a main size "two" exists with description: "16"
	And a street "two" exists with town: "one", full st name: "Main St"
	And a roadway improvement notification street status "one" exists with description: "Pending"
	And a roadway improvement notification street status "two" exists with description: "Completed"
	And I am logged in as "admin"
	When I visit the Show page for roadway improvement notification: "one"
	And I click the "Streets" tab
	And I press "Add New Street"
	And I select street "one" from the Street dropdown
	And I enter coordinate "one"'s Id into the Coordinate field
	And I enter "terminus" into the Terminus field
	And I enter "start point" into the StartPoint field
	And I select main size "one" from the MainSize dropdown
	And I select main type "one" from the MainType dropdown
	And I enter "3" into the MainBreakActivity field
	And I enter "13" into the NumberOfServices field
	And I select roadway improvement notification street status "one" from the RoadwayImprovementNotificationStreetStatus dropdown
	And I enter "12/8/1980" into the MoratoriumEndDate field
	And I press "Save Street"
	Then I should be at the Show page for roadway improvement notification: "one"
	When I click the "Streets" tab
	Then I should see the following values in the streetsTable table
	| Street  | Main Size | Main Type | Status  | Main Break Activity | Moratorium End Date | # of Services to be Replaced |
	| *St*    | 12        | asbestos  | Pending | 3                   | 12/8/1980           | 13                           |
	When I click the "Edit" link in the 1st row of streetsTable
	And I wait for the page to reload
	And I select street "two" from the Street dropdown
	And I select main size "two" from the MainSize dropdown
	And I select main type "two" from the MainType dropdown
	And I enter "5" into the MainBreakActivity field
	And I enter "15" into the NumberOfServicesToBeReplaced field
	And I select roadway improvement notification street status "two" from the RoadwayImprovementNotificationStreetStatus dropdown
	And I press "Save"
	Then I should see a display for Street with street "two"'s FullStName
	And I should see a display for Coordinate with "40.32246702, -74.1481018"
	And I should see a display for StartPoint with "start point"
	And I should see a display for Terminus with "terminus"
	And I should see a display for MainSize with main size "two"'s Description
	And I should see a display for MainType with main type "two"'s Description
	And I should see a display for RoadwayImprovementNotificationStreetStatus with roadway improvement notification street status "two"'s Description
	When I visit the Show page for roadway improvement notification: "one"
	And I click the "Streets" tab
	When I click the "Remove" button in the 1st row of streetsTable and then click ok in the confirmation dialog
	And I wait for the page to reload
	And I click the "Streets" tab
	Then I should not see "Easy St"
	And I should not see "Main St"