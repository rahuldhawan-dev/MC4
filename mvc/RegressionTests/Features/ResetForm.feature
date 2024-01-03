Feature: ResetForm
	Some Bob's burgers stuff

Background: 
	Given an admin user "admin" exists with username: "admin"
	And a state "one" exists with abbreviation: "QQ", name: "Q State"
	And a county "one" exists with state: "one", name: "Count Countula"
	And a town "one" exists with county: "one", name: "Townie"
	And an operating center "one" exists with opcode: "QQ1", name: "Wawa", state: "one"
	And a markout damage to type "other" exists with description: "Other"
	And a markout damage to type "ours" exists with description: "Ours"
	And a grievance category "one" exists with description: "things"
	And a grievance categorization "gc1" exists with description: this is a thing, grievance category: "one"

Scenario: user can reset cascades and dateranges and inputs on a form
	Given I am logged in as "admin"
	When I visit the FieldOperations/MarkoutDamage/Search page
	And I select state "one" from the State dropdown
	And I select operating center "one" from the OperatingCenter dropdown	
	And I select county "one" from the County dropdown
	And I enter "12321" into the Street field
	And I enter "4/1/2014" into the DamageOn_Start field
	And I select ">" from the DamageOn_Operator dropdown
	And I enter "4/1/2014" into the DamageOn_End field
	And I press Reset
	Then "Select a state below" should be selected in the OperatingCenter dropdown
	And "-- Select --" should be selected in the State dropdown
	And "Select a state above" should be selected in the County dropdown
	And I should not see "12321" in the Street element
	And I should not see "4/1/2014" in the DamageOn_Start element
	And I should not see "4/1/2014" in the DamageOn_End element
	And "Between" should be selected in the DamageOn_Operator dropdown
	And I should see the DamageOn_Start element

Scenario: user can reset multi select on a form
	Given I am logged in as "admin"
	When I visit the Grievance/Search page
	And I enter 0 into the EntityId field
	And I select grievance category "one" from the GrievanceCategory dropdown
	And I select grievance categorization "gc1"'s Description from the Categorization dropdown
	And I press Search
	And I wait for the page to reload
	And I press Reset
	Then grievance categorization "gc1"'s Description should not be selected in the Categorization dropdown

Scenario: user can reset a combobox
	Given an operating center "nj7" exists with opcode: "NJ7"
	And a position group common name "one" exists with description: "Surveyor"
	And a position group "one" exists with description: "Foo", common name: "one"
	And an employee status "active" exists with description: "Active"
	And an employee "one" exists with first name: "Theodore", last name: "Logan", employee id: "11111111", operating center: "nj7", status: "active", position group: "one"
	And a training module category "one" exists with description: "Safety"
	And a training requirement "one" exists with is o s h a requirement: true, is active: true, training frequency unit: "Y", training frequency: "2"
	And position group common name: "one" exists in training requirement: "one"
	And a training module "one" exists with title: "training module one", course approval number: "123", training module category: "one", training requirement: "one"
	And a data type "employees attended" exists with table name: "tblTrainingRecords", name: "Employees Attended"
	And a data type "employees scheduled" exists with table name: "tblTrainingRecords", name: "Employees Scheduled"
	And a training record "one" exists with training module: "one", held on: "4/1/2015"
	And employee: "one" is scheduled for training record: "one"
	And employee: "one" attended training record: "one"
	And I am logged in as "admin"
	When I visit the Reports/TrainingTotalHours/Search page
	And I enter "Train" and select training module "one"'s Title from the TrainingModule combobox
	And I press "Reset"
	And I enter "2015" into the Year field
	And I press "Search"
	Then I should not see "No results matched your query."
	And I should see training module "one"'s Title in the "Training Module" column