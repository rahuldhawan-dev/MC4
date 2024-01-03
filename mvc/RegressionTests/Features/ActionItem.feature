Feature: ActionItem
	# We are going to use the general liability claim page for this test, if change is made it should effect with with ActionItems thus this will fail
Background: These are my action items, without them am I nothing, with me they are nothing
	Given an admin user "admin" exists with username: "admin"
	And a state "NJ" exists with abbreviation: "NJ"
	And a operating center "one" exists with State: "NJ", opcode: "one"
	And a user "testuser" exists with username: "stuff", default operating center: "one", full name: "Boaty McBoatface"
	And a general liability claim "one" exists with description: "this is my description.", operating center: "one"
	And a data type "data type" exists with table name: "GeneralLiabilityClaims"	
	And a action item type "type1" exists with data type: "data type"
	And a data type "data type near miss" exists with table name: "NearMisses"
	And a action item type "type2" exists with data type: "data type near miss"
	And I am logged in as "admin"

Scenario: User can add an action item:
	Given I am logged in as "user" 
	When I visit the Show page for general liability claim: "one"
	And I click the "Action Items" tab
	And I press "New Action Item"
	And I select action item type "type1" from the Type dropdown
	And I enter "This is my action item" into the Note field
	And I select user "testuser"'s FullName from the ResponsibleOwner dropdown
	And I enter 3/17/2020 into the TargetedCompletionDate field
	And I enter 3/17/2020 into the DateCompleted field
	And I press "Add Action Item"
	When I click the "Action Items" tab
	Then I should see "This is my action item" in the table actionItemsTable's "Action Item" column
	And I should see "This is my description" in the table actionItemsTable's "Type" column
	And I should see "Boaty McBoatface" in the table actionItemsTable's "Responsible Owner" column
	And I should see "3/17/2020" in the table actionItemsTable's "Targeted Completion Date" column
	And I should see "3/17/2020" in the table actionItemsTable's "Date Completed" column

Scenario: User can edit an action item
	Given I am logged in as "user" 
	And a action item "one" exists with note: "blah", general liability claim: "one", data type: "data type"
	When I visit the Show page for general liability claim: "one"
	And I click the "Action Items" tab
	Then I should see a link to the /ActionItem/Edit/1?State=1 page
	When I click the "Edit" link in the 1st row of actionItemsTable
	And I select action item type "type1" from the Type dropdown
	And I select user "testuser"'s FullName from the ResponsibleOwner dropdown
	And I enter "This is my updated action item" into the Note field
	And I enter 3/18/2020 into the TargetedCompletionDate field 
	And I enter 3/17/2020 into the DateCompleted field
	And I press "Save"
	Then I should see "This is my updated action item" in the table actionItemsTable's "Action Item" column
	And I should see "This is my description" in the table actionItemsTable's "Type" column
	And I should see "Boaty McBoatface" in the table actionItemsTable's "Responsible Owner" column
	And I should see "3/18/2020" in the table actionItemsTable's "Targeted Completion Date" column
	And I should see "3/17/2020" in the table actionItemsTable's "Date Completed" column

Scenario: User sees a bunch of validation errors on new when they do the wrong thing:
	Given I am logged in as "user" 
	When I visit the Show page for general liability claim: "one"
	And I click the "Action Items" tab
	And I press "New Action Item"
	And I press "Add Action Item"
	Then I should see the validation message The Type field is required.
	And I should see the validation message The Action Item field is required.
	And I should see the validation message The TargetedCompletionDate field is required.
	And I should see the validation message The ResponsibleOwner field is required.

Scenario: User sees a bunch of validation errors on edit when they do the wrong thing:
	Given I am logged in as "user" 
	And a action item "one" exists with note: "blah", general liability claim: "one", data type: "data type"
	When I visit the Show page for general liability claim: "one"
	And I click the "Action Items" tab
	Then I should see a link to the /ActionItem/Edit/1?State=1 page
	When I click the "Edit" link in the 1st row of actionItemsTable
	And I select "-- Select --" from the Type dropdown
	And I enter " " into the Note field
	And I enter " " into the TargetedCompletionDate field 
	And I press "Save"
	Then I should see the validation message The Type field is required.
	And I should see the validation message The Action Item field is required.
	And I should see the validation message The TargetedCompletionDate field is required.

Scenario: User can delete an action item 
	Given I am logged in as "user" 
	And a action item "one" exists with note: "blah", general liability claim: "one", data type: "data type"
	When I visit the Show page for general liability claim: "one"
	And I click the "Action Items" tab
	Then I should see "blah" in the table actionItemsTable's "Action Item" column
    When I click the "Delete" button in the 1st row of actionItemsTable and then click ok in the confirmation dialog
	And I wait for the page to reload
    Then I should not see "blah" in the table actionItemsTable's "Action Item" column

Scenario: User cannot add an action item to Near Miss when operating center is empty:
	Given I am logged in as "admin"
	And a near miss "two" exists
	When I visit the Show page for near miss: "two"
	And I click the "Action Items" tab
	Then I should see "Operating center needs to be populated before Action Items can be selected."

Scenario: User can add an action item to Near Miss when operating center is not empty:
	Given I am logged in as "admin"
	And a near miss "one" exists with operating center: "one"
	When I visit the Show page for near miss: "one"
	And I click the "Action Items" tab
	And I press "New Action Item"
	And I select action item type "type2" from the Type dropdown
	And I enter "This is my action item" into the Note field
	And I select user "testuser"'s FullName from the ResponsibleOwner dropdown
	And I enter 6/29/2020 into the TargetedCompletionDate field
	And I enter 6/29/2020 into the DateCompleted field
	And I press "Add Action Item"
	When I click the "Action Items" tab
	Then I should see "This is my action item" in the table actionItemsTable's "Action Item" column
	And I should see "This is my description" in the table actionItemsTable's "Type" column
	And I should see "Boaty McBoatface" in the table actionItemsTable's "Responsible Owner" column
	And I should see "6/29/2020" in the table actionItemsTable's "Targeted Completion Date" column
	And I should see "6/29/2020" in the table actionItemsTable's "Date Completed" column
