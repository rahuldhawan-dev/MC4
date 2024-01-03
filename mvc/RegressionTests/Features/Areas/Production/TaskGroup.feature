Feature: TaskGroup
	In order to manage task groups
	I need for the pages to not be broken
	So I can manage the task groups

Background: IfIWasCreativeIWouldNameThisTaskMaster
	Given an admin user "admin" exists with username: "admin"
	And a role "proddataadminread" exists with action: "Read", module: "ProductionDataAdministration"
	And a role "proddataadminadd" exists with action: "Add", module: "ProductionDataAdministration"
	And a role "proddataadminedit" exists with action: "Edit", module: "ProductionDataAdministration"
	And a user "user" exists with username: "user", roles: proddataadminread;proddataadminadd;proddataadminedit
	And equipment types exist
    And a task group category "one" exists with description: "This is the description", type: "Chemical", abbreviation: "CHEM", is active: "true"
    And a task group category "two" exists with description: "This is the description two", type: "Electrical", abbreviation: "ELEC", is active: "true"
    And a task group category "three" exists with description: "This is the description three", type: "Power", abbreviation: "POW", is active: "true"
    And a maintenance plan task type "one" exists with description: "This is the plan info", is active: "true"
    And a maintenance plan task type "two" exists with description: "This is the new plan info", is active: "true"
	And a task group "tasky" exists with task group id: "Tasky Id", task group name: "TaskyName", task details: "Tasky details", task details summary: "Tasky summary", equipment types: "rtu", task group category: "one", maintenance plan task type: "one"
	And a task group "NotSoTasky" exists with task group id: "NotSo Id", task group name: "NotSoTaskyName", task details: "NotSoTasky details", task details summary: "NotSoTasky summary", equipment types: "rtu", task group category: "two", maintenance plan task type: "one"
	And a task group "NewSoTasky" exists with task group id: "NewTask Id", task group name: "NewTaskyName", task details: "NewSoTasky details", task details summary: "NewSoTasky summary", equipment types: "rtu", task group category: "three", maintenance plan task type: "two"
	
Scenario: Admin can view a task group if they really wish hard enough
	Given I am logged in as "Admin"
	And I am at the Production/TaskGroup/Show page for task group "tasky"
	When I click the "Task Group" tab
	Then I should see a display for TaskGroupId with "Tasky Id"
	And I should see a display for TaskGroupName with "TaskyName"
	And I should see a display for TaskDetails with "Tasky details"
	And I should see a display for TaskGroupCategory with task group category "one"
	And I should see a display for MaintenancePlanTaskType with maintenance plan task type "one"

Scenario: Admin can edit a task group, but probably shouldn't
	Given I am logged in as "admin"
	And I am at the show page for task group "tasky"
	When I follow "Edit"
	And I enter "Long Long Id Field" into the TaskGroupId field
	And I press "Save"
	Then I should see a validation message for TaskGroupId with "The field TaskGroupId must be a string with a maximum length of 8."
	When I enter "12345" into the TaskGroupId field
	And I enter "Doesnt matter how long" into the TaskDetails field 
	And I enter "This is a really long description really long really long really long really long really long I'm tired of copying and pasting text" into the TaskGroupName field
	And I press "Save"
	Then I should see a validation message for TaskGroupName with "The field TaskGroupName must be a string with a maximum length of 50."
	When I enter "Shorter Name 2" into the TaskGroupName field
	And I enter "This is a really long description really long really long really long really long really long really long really long really long really long really long really long really long really long really long really long really long really long really long really long really long really long really long really long really long really long I'm tired of copying and pasting text" into the TaskDetailsSummary field
	And I press "Save"
	Then I should see a validation message for TaskDetailsSummary with "The field TaskDetailsSummary must be a string with a maximum length of 250."
	When I enter "Shorter summary" into the TaskDetailsSummary field 	
	And I select task group category "two"'s Type from the TaskGroupCategory dropdown
	And I select maintenance plan task type "one"'s Description from the MaintenancePlanTaskType dropdown
	And I press "Save"
	Then I should be at the show page for task group: "tasky"
	And I should see a display for TaskGroupId with "12345"
	And I should see a display for TaskGroupName with "Shorter Name 2"
	And I should see a display for TaskDetails with "Doesnt matter how long"
	And I should see a display for TaskDetailsSummary with "Shorter summary"
	And I should see a display for TaskGroupCategory with task group category "two"
	And I should see a display for MaintenancePlanTaskType with maintenance plan task type "one"

Scenario: Admin can search for task group
	Given I am logged in as "admin"
	And I am at the Production/TaskGroup/Search page
	When I enter "NotSoTaskyName" into the TaskGroupName field
	And I press "Search"
	Then I should be at the Production/TaskGroup page
	And I should see a link to the show page for task group: "NotSoTasky"
	When I visit the Production/TaskGroup/Search page
	And I press "Search"
	Then I should be at the Production/TaskGroup page
	And I should see a link to the show page for task group: "tasky"
	And I should see a link to the show page for task group: "NotSoTasky"
	When I visit the Production/TaskGroup/Search page
	And I select maintenance plan task type "one"'s Description from the MaintenancePlanTaskType dropdown
	And I press "Search"
	Then I should be at the Production/TaskGroup page
	And I should see a link to the show page for task group: "tasky"
	And I should see a link to the show page for task group: "NotSoTasky"
	And I should not see a link to the show page for task group: "NewSoTasky"

Scenario: Admin can create a task group
	Given I am logged in as "admin"
	When I visit the Production/TaskGroup/New page
	And I press Save
	Then I should be at the Production/TaskGroup/New page
	And I should see the validation message "The TaskGroupId field is required."
	And I should see the validation message "The TaskGroupName field is required."
	When I enter "12345" into the TaskGroupId field
	And I enter "Task Name" into the TaskGroupName field
	And I enter "New Task Details" into the TaskDetails field
	And I enter "New Task Summary" into the TaskDetailsSummary field
	And I select task group category "one"'s Type from the TaskGroupCategory dropdown
	And I select maintenance plan task type "one"'s Description from the MaintenancePlanTaskType dropdown
	And I press Save
	Then the currently shown task group will now be known throughout the land as "tasker"
	And I should be at the show page for task group: "tasker"
	And I should see a display for TaskGroupId with "12345"
	And I should see a display for TaskGroupName with "Task Name"
	And I should see a display for TaskDetails with "New Task Details"
	And I should see a display for TaskDetailsSummary with "New Task Summary"
	And I should see a display for TaskGroupCategory with task group category "one"
	And I should see a display for MaintenancePlanTaskType with maintenance plan task type "one"

Scenario: User can view a task group
	Given I am logged in as "user"
	And I am at the show page for task group: "NotSoTasky"
	Then I should see a display for TaskGroupId with "NotSo Id"
	And I should see a display for TaskGroupName with "NotSoTaskyName"
	And I should see a display for TaskDetails with "NotSoTasky details"
	And I should see a display for TaskDetailsSummary with "NotSoTasky summary"
	And I should see a display for TaskGroupCategory with task group category "two"
	And I should see a display for MaintenancePlanTaskType with maintenance plan task type "one"

Scenario: User can edit a task group
	Given I am logged in as "user"
	And I am at the show page for task group "tasky"
	When I follow "Edit"
	When I enter "12345" into the TaskGroupId field
	And I enter "Task Name Again" into the TaskGroupName field
	When I enter "Task Details text" into the TaskDetails field 
	When I enter "Summary text" into the TaskDetailsSummary field
	And I select task group category "two"'s Type from the TaskGroupCategory dropdown
	And I select maintenance plan task type "one"'s Description from the MaintenancePlanTaskType dropdown
	And I press "Save"
	Then I should be at the show page for task group: "tasky"
	And I should see a display for TaskGroupId with "12345"
	And I should see a display for TaskGroupName with "Task Name Again"
	And I should see a display for TaskDetails with "Task Details text"
	And I should see a display for TaskDetailsSummary with "Summary text"
	And I should see a display for TaskGroupCategory with task group category "two"
	And I should see a display for MaintenancePlanTaskType with maintenance plan task type "one"

Scenario: User can search for task group
	Given I am logged in as "user"
	And I am at the Production/TaskGroup/Search page
	When I enter "TaskyName" into the TaskGroupName field
	And I press "Search"
	Then I should be at the Production/TaskGroup page
	And I should see a link to the show page for task group: "tasky"
	When I visit the Production/TaskGroup/Search page
	And I press "Search"
	Then I should be at the Production/TaskGroup page
	And I should see a link to the show page for task group: "tasky"
	And I should see a link to the show page for task group: "NotSoTasky"
	When I visit the Production/TaskGroup/Search page
	And I select maintenance plan task type "one"'s Description from the MaintenancePlanTaskType dropdown
	And I press "Search"
	Then I should be at the Production/TaskGroup page
	And I should see a link to the show page for task group: "tasky"
	And I should see a link to the show page for task group: "NotSoTasky"
	And I should not see a link to the show page for task group: "NewSoTasky"

Scenario: User can create a task group
	Given I am logged in as "user"
	When I visit the Production/TaskGroup/New page
	And I press Save
	Then I should be at the Production/TaskGroup/New page
	And I should see the validation message "The TaskGroupId field is required."
	And I should see the validation message "The TaskGroupName field is required."
	When I enter "ACEGI" into the TaskGroupId field
	And I enter "Task Name 2" into the TaskGroupName field
	And I enter "Task Details 2" into the TaskDetails field
	And I enter "Task Summary 2" into the TaskDetailsSummary field
	And I select task group category "two"'s Type from the TaskGroupCategory dropdown
	And I select maintenance plan task type "one"'s Description from the MaintenancePlanTaskType dropdown
	And I press Save
	And I wait for the page to reload
	Then the currently shown task group will now be known throughout the land as "tasker1"
	And I should be at the show page for task group: "tasker1"
	And I should see a display for TaskGroupId with "ACEGI"
	And I should see a display for TaskGroupName with "Task Name 2"
	And I should see a display for TaskDetails with "Task Details 2"
	And I should see a display for TaskDetailsSummary with "Task Summary 2"
	And I should see a display for TaskGroupCategory with task group category "two"
	And I should see a display for MaintenancePlanTaskType with maintenance plan task type "one"