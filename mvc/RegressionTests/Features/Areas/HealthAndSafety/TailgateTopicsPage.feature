Feature: Tailgate Topics Page

Background: users and supporting data exists
	Given an operating center "nj7" exists with opcode: "NJ7"
	And an operating center "nj4" exists with opcode: "NJ4"
	And a role "roleReadNj7" exists with action: "Read", module: "OperationsHealthAndSafety", operating center: "nj7"
	And a role "roleEditNj7" exists with action: "Edit", module: "OperationsHealthAndSafety", operating center: "nj7"
	And a role "roleAddNj7" exists with action: "Add", module: "OperationsHealthAndSafety", operating center: "nj7"
	And a role "roleDeleteNj7" exists with action: "Delete", module: "OperationsHealthAndSafety", operating center: "nj7"
	And a role "roleReadNj4" exists with action: "Read", module: "OperationsHealthAndSafety", operating center: "nj4"
	And a role "roleEditNj4" exists with action: "Edit", module: "OperationsHealthAndSafety", operating center: "nj4"
	And a role "roleAddNj4" exists with action: "Add", module: "OperationsHealthAndSafety", operating center: "nj4"
	And a role "roleDeleteNj4" exists with action: "Delete", module: "OperationsHealthAndSafety", operating center: "nj4"
	And an admin user "admin" exists with username: "admin"
	And a user "user" exists with username: "user"
	And a user "invalid" exists with username: "invalid"
	And a user "read-only-nj7" exists with username: "read-only-nj7", roles: roleReadNj7
	And a user "user-admin-nj7" exists with username: "user-admin-nj7", roles: roleReadNj7;roleEditNj7;roleAddNj7;roleDeleteNj7
	And a user "read-only-nj4" exists with username: "read-only-nj4", roles: roleReadNj4
	And a user "user-admin-nj4" exists with username: "user-admin-nj4", roles: roleReadNj4;roleEditNj4;roleAddNj4;roleDeleteNj4
	And a user "user-admin-both" exists with username: "user-admin-both", roles: roleReadNj4;roleEditNj4;roleAddNj4;roleDeleteNj4;roleReadNj7;roleEditNj7;roleAddNj7;roleDeleteNj7
    And a tailgate talk topic "one" exists
    And a tailgate topic category "one" exists
    And a tailgate topic month "one" exists

Scenario: user can view tailgate talk topics
    Given I am logged in as "user-admin-nj7"
	When I visit the Show page for tailgate talk topic: "one"
    Then I should see a display for tailgate talk topic: "one"'s Topic
    And I should see a display for tailgate talk topic: "one"'s Category
    And I should see a display for tailgate talk topic: "one"'s Month
    And I should see a display for tailgate talk topic: "one"'s TargetDeliveryDate
    And I should see a display for tailgate talk topic: "one"'s TopicLevel
    And I should see a display for tailgate talk topic: "one"'s OrmReferenceNumber
	When I visit the HealthAndSafety/TailgateTalkTopic/Search page
	And I press Search
    Then I should see tailgate talk topic "one"'s Topic in the "Topic" column
    And I should see tailgate talk topic "one"'s Category in the "Category" column
    And I should see tailgate talk topic "one"'s Month in the "Month" column
    And I should see tailgate talk topic "one"'s TargetDeliveryDate in the "Target Delivery Date" column
    And I should see tailgate talk topic "one"'s TopicLevel in the "Topic Level" column
    And I should see tailgate talk topic "one"'s OrmReferenceNumber in the "Orm Reference Number" column

Scenario: user can add a tailgate talk topic
	Given I am logged in as "user-admin-nj7"
	When I visit the HealthAndSafety/TailgateTalkTopic/New page
    And I select tailgate topic category "one"'s Description from the Category dropdown
    And I select tailgate topic month "one"'s Description from the Month dropdown
    And I press Save
    Then I should see a validation message for TopicLevel with "The TopicLevel field is required."
    And I should see a validation message for IsActive with "The IsActive field is required."
    When I select "State" from the TopicLevel dropdown
    And I select "Yes" from the IsActive dropdown
	And I press Save
    Then I should see a validation message for OrmReferenceNumber with "ORM Reference Number is required when Topic Level is 'State'."
    And I should see a validation message for TargetDeliveryDate with "The TargetDeliveryDate field is required."
    And I should see a validation message for Topic with "The Topic field is required."
    When I enter "1234321" into the OrmReferenceNumber field
    And I enter "topic" into the Topic field
    And I enter "8/17/2022" into the TargetDeliveryDate field
    And I press Save
	And I wait for the page to reload
    Then the currently shown tailgate talk topic will now be referred to as "new"
    And I should be at the Show page for tailgate talk topic: "new"
    Then I should see a display for tailgate talk topic: "new"'s Topic
    And I should see a display for Category with tailgate topic category "one"'s Description
    And I should see a display for Month with tailgate topic month "one"'s Description
    And I should see a display for tailgate talk topic: "new"'s TargetDeliveryDate
    And I should see a display for tailgate talk topic: "new"'s TopicLevel
    And I should see a display for tailgate talk topic: "new"'s OrmReferenceNumber

Scenario: user can edit a tailgate talk topic
    Given I am logged in as "user-admin-nj7"
	When I visit the Edit page for tailgate talk topic: "one"
    And I enter "foo" into the Topic field
	And I press Save
    Then I should be at the Show page for tailgate talk topic: "one"
    And I should see a display for Topic with "foo"

Scenario: user can destroy a tailgate talk topic
    Given I am logged in as "user-admin-nj7"
	When I visit the Show page for tailgate talk topic: "one"
	And I click ok in the dialog after pressing "Delete"
	Then I should be at the HealthAndSafety/TailgateTalkTopic/Search page
	When I try to access the Show page for tailgate talk topic: "one" expecting an error
	Then I should see a 404 error message
