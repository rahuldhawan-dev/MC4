Feature: Site
	Tests for things that are site-wide
	
Background: users exist
	Given a user "user" exists with username: "user", email: "user@site.com"
	And a role "image" exists with action: "Read", module: "FieldServicesImages", user: "user"
	And an asset type "valve" exists with description: "valve"
    And an asset type "hydrant" exists with description: "hydrant"
    And an asset type "sewer opening" exists with description: "sewer opening"

Scenario: Visiting a non-existant controller should return the pretty 404
	Given I am logged in as "user"
	When I try to access the Huzzabaloo page expecting an error
    Then I should see a 404 error message

Scenario: Menu visibility should persist through page loads thanks to the magic of cookies
	Given I am logged in as "user"
	And I can see the navigation menu
	And I am at the Home/Index page
	And I can see the nav element
	When I press toggleMenuButton
	And I wait 1 second
	Then I should not see the nav element
	When I reload the page
	Then I should not see the nav element
	When I press toggleMenuButton
	And I wait 1 second
	Then I should see the nav element
	When I reload the page
	Then I should see the nav element

Scenario: Menu visibility should persist for the entire domain because javascript by default likes to set cookies for the current directory because it is a dumb
	Given I am logged in as "user"
	And I can see the navigation menu
	And I am at the Home/Index page
	And I can see the nav element
	And I have pressed toggleMenuButton
	And I can not see the nav element
	When I go to the Layout page
	Then I should not see the nav element
	When I go to the State/Index page
	Then I should not see the nav element 
	When I press toggleMenuButton
	Then I should see the nav element
	
Scenario: A user can not edit or delete a note for an operating center they do not have edit rights to but have other rights to
	Given an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
	And an operating center "nj4" exists with opcode: "NJ4", name: "Shrewsbury", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
	And a role "roleEditNJ7" exists with action: "Edit", module: "FieldServicesAssets", operating center: nj7
	And a role "roleReadNJ7" exists with action: "Read", module: "FieldServicesAssets", operating center: nj7
	And a role "roleEditNJ4" exists with action: "Edit", module: "FieldServicesAssets", operating center: nj4
	And a role "roleReadNJ4" exists with action: "Read", module: "FieldServicesAssets", operating center: nj4
    And a user "authorized" exists with username: "authorized", roles: roleEditNJ7
	And a user "unauthorized" exists with username: "unauthorized", roles: roleEditNJ4;roleReadNJ7
	And a asset status "active" exists with description: "ACTIVE"
	And a asset status "retired" exists with description: "RETIRED"
	And a hydrant billing "public" exists with description: "Public"
	And a hydrant inspection type "flush" exists with description: "FLUSH"
	And a hydrant tag status "tag" exists with description: "Tag!"
	And a town "one" exists with name: "Loch Arbour"
	And operating center: "nj7" exists in town: "one" with abbreviation: "LA"
    And a data type "data type" exists with table name: "Hydrants"
	And a hydrant "one" exists with operating center: "nj7" 
	And a note "note" exists with text: "foo bar", hydrant: "one", data type: "data type"
	And I am logged in as "authorized"
    When I visit the Show page for hydrant: "one"
    And I click the "Notes" tab
    Then I should see "foo bar" in the table notesTable's "Note" column
	And I should see the button "Edit"
	And I should see the button "Delete"
	When I log in as "unauthorized"
	And I visit the Show page for hydrant: "one"
	And I click the "Notes" tab
	Then I should see "foo bar" in the table notesTable's "Note" column
	And I should not see the button "Edit"
	And I should not see the button "Delete"

Scenario: Cascading dropdowns remember their previously selected values for the current page
	Given an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
	And an operating center "nj4" exists with opcode: "NJ4", name: "Shrewsbury", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
    And an operating center "sov" exists with opcode: "SOV", name: "SOV"
	And an employee status "active" exists with description: "Active"
	And an employee "nj7-one" exists with operating center: "nj7", status: "active"
	And an employee "nj7-two" exists with operating center: "nj7", status: "active"
	And an employee "nj4-one" exists with operating center: "nj4", status: "active"
	And an employee "nj4-two" exists with operating center: "nj4", status: "active"
	And an admin user "admin" exists with username: "admin"
	And a job site check list pressurized risk restrained type "yes" exists with description: "Yes"	
	And a job site check list pressurized risk restrained type "no" exists with description: "No"	
	And a job site check list no restraint reason type "one" exists with description: "No restraints for this guy!"
	And I am logged in as "admin"
	And I am at the HealthAndSafety/JobSiteCheckList/New page
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I select employee "nj7-two"'s Description from the CompetentEmployee dropdown
	And I select operating center "nj4" from the OperatingCenter dropdown
	And I select employee "nj4-one"'s Description from the CompetentEmployee dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
	Then I should see employee "nj7-two"'s Description in the CompetentEmployee dropdown
	When I select operating center "nj4" from the OperatingCenter dropdown
 	Then I should see employee "nj4-one"'s Description in the CompetentEmployee dropdown

# Checkbox lists work slightly different, otherwise this test is almost the same as 
# the test below
Scenario: Checkbox lists remember their previously selected values for the current page
	Given I am at the Layout page 
	When I click the "Cascades" tab
	And I select "Third" from the ParentOfCheckBoxList dropdown
	And I check "You selected 3" in the CascadingCheckBoxList checkbox list
	And I select "Second" from the ParentOfCheckBoxList dropdown
	And I select "Third" from the ParentOfCheckBoxList dropdown
	Then "You selected 3" should be checked in the CascadingCheckBoxList checkbox list

Scenario: Multiselect cascades remember their previously selected values when the parent value is already selected on page load
	Given I am at the Layout page 
	When I click the "Cascades" tab
	Then "Dummy value 201" should not be selected in the CascadeListBox dropdown
	And "Dummy value 101" should not be selected in the CascadeListBox dropdown
	When I select "Dummy value 201" from the CascadeListBox dropdown
	And I select "-- Select --" from the RegularDropDownPreSelectedValue dropdown
	Then I should not see "Dummy value 201" in the CascadeListBox dropdown
	When I select "A thing" from the RegularDropDownPreSelectedValue dropdown
	Then "Dummy value 201" should be selected in the CascadeListBox dropdown
	And "Dummy value 101" should not be selected in the CascadeListBox dropdown

Scenario: MultiString plugin works
	Given I am logged in as "user"
	# Test that a child cascade with two dependents that are both required functions correctly.
	When I visit the Layout page
	And I click the "Editor For" tab
	Then I should see "Some value" in the MultiString[0] field
	And I should see "And another value" in the MultiString[1] field
	When I press "MultiString_0_Remove"
	Then I should see "And another value" in the MultiString[0] field

Scenario: Cascades function correctly when dealing with multiple dependent values
	Given I am logged in as "user"
	# Test that a child cascade with two dependents that are both required functions correctly.
	When I visit the Layout page
	And I click the "Cascades" tab
	Then "-- Select --" should be selected in the CascadeParent1 dropdown
	And "-- Select --" should be selected in the CascadeParent2 dropdown
	And "Select a thing above" should be selected in the ChildRequiresBothNonPopulatedParents dropdown
	When I select "A thing" from the CascadeParent1 dropdown
	Then "Select a thing above" should be selected in the ChildRequiresBothNonPopulatedParents dropdown
	When I select "A thing" from the CascadeParent2 dropdown
	Then I should see "Both parents have values." in the ChildRequiresBothNonPopulatedParents dropdown
	When I select "-- Select --" from the CascadeParent1 dropdown
	Then "Select a thing above" should be selected in the ChildRequiresBothNonPopulatedParents dropdown

	# Test that a child cascade with two dependents where only one is required
	When I visit the Layout page
	And I click the "Cascades" tab
	Then "-- Select --" should be selected in the CascadeParent1 dropdown
	And "-- Select --" should be selected in the CascadeParent2 dropdown
	And "Select a thing above" should be selected in the ChildRequiresAtleastOnePopulatedParent dropdown
	When I select "A thing" from the CascadeParent1 dropdown
	Then I should see "Only first parent has value." in the ChildRequiresAtleastOnePopulatedParent dropdown
	When I select "A thing" from the CascadeParent2 dropdown
	Then I should see "Both parents have values." in the ChildRequiresAtleastOnePopulatedParent dropdown
	When I select "-- Select --" from the CascadeParent1 dropdown
	Then I should see "Only second parent has value." in the ChildRequiresAtleastOnePopulatedParent dropdown

	# Test that a child with two dependents, only one is required, and one of them is prepopulated already.
	When I visit the Layout page
	And I click the "Cascades" tab
	Then "A thing" should be selected in the CascadeParent3 dropdown
	And "-- Select --" should be selected in the CascadeParent4 dropdown
	And I should see "Only first parent has value." in the ChildPrepopulatesWithOnePopulatedParent dropdown

	# Test that a child with two dependents, all required, only has one parent with a prepopulated value, no child items should be displayed.
	When I visit the Layout page
	And I click the "Cascades" tab
	Then "A thing" should be selected in the CascadeParent5 dropdown
	And "-- Select --" should be selected in the CascadeParent6 dropdown
	Then "Select a thing above" should be selected in the ChildHasOneParentPrepopulatedButNeedsBoth dropdown

	# Test that a child that has non-required dependents can select values when the dependent doesn't have a selected value.
	When I visit the Layout page 
	And I click the "Cascades" tab 
	Then "-- Select --" should be selected in the CascadeParentNotRequired dropdown
	And the ChildDoesNotRequireParent dropdown should be enabled
	And I should see "Parent does not have value." in the ChildDoesNotRequireParent dropdown
	When I select "A thing" from the CascadeParentNotRequired dropdown
	Then the ChildDoesNotRequireParent dropdown should be enabled
	And I should see "Parent does have value." in the ChildDoesNotRequireParent dropdown
	When I select "-- Select --" from the CascadeParentNotRequired dropdown
	Then the ChildDoesNotRequireParent dropdown should be enabled
	And I should see "Parent does not have value." in the ChildDoesNotRequireParent dropdown

Scenario: Regular non-cascading checkbox lists should autopopulate with selected values
	Given I am at the Layout page
	When I click the "Editor For" tab
	Then "First" should be checked in the CheckBoxing checkbox list
	And "Second" should not be checked in the CheckBoxing checkbox list
	And "Third" should be checked in the CheckBoxing checkbox list

Scenario: Cascading checkbox lists work as a middle child
	Given I am at the Layout page 
	When I click the "Cascades" tab
	Then I should see "-- Select --" in the CascadingCheckBoxList checkbox list
	When I select "Third" from the ParentOfCheckBoxList dropdown
	Then I should see "You selected 3" in the CascadingCheckBoxList checkbox list
	And I should not see "You selected 2" in the CascadingCheckBoxList checkbox list
	When I check "You selected 3" in the CascadingCheckBoxList checkbox list
	Then I should see "You selected 3" in the ChildOfCheckBoxList dropdown

Scenario: Cascading checkbox lists should not start checking all the boxes when the parent value gets changed a second time
	Given I am at the Layout page 
	When I click the "Cascades" tab
	Then I should see "-- Select --" in the CascadingCheckBoxList checkbox list
	When I select "Third" from the ParentOfCheckBoxList dropdown
	Then "You selected 3" should not be checked in the CascadingCheckBoxList checkbox list
	When I select "Second" from the ParentOfCheckBoxList dropdown
	Then "You selected 2" should not be checked in the CascadingCheckBoxList checkbox list

Scenario: Pre-rendered cascades where filtering results in zero results should not cause an ajax request on page load
	# This test is setup to use a child that doesn't have a parent rendered on the page.
	# This dropdown will have an error message if this breaks.
	Given I am at the Layout page 
	When I click the "Cascades" tab 
	Then I should see "No Results" in the PreRenderedChildShouldDisplayNoResultsAndNotMakeAServerRequest dropdown
	
Scenario: An ajax tab should load its ajax content if its the first tab to be visible on page load
	Given a role "roleRead" exists with action: "Read", module: "GeneralTowns"
	And a role "roleEdit" exists with action: "Edit", module: "GeneralTowns"
	And a role "roleAdd" exists with action: "Add", module: "GeneralTowns"
	And a role "roleDelete" exists with action: "Delete", module: "GeneralTowns"
	And an admin user "admin" exists with username: "admin"
	And a town "one" exists
	And a data type "data type" exists with table name: "Towns"
    And a document type "document type" exists with data type: "data type", name: "Town Document"
	And a document type "other" exists with data type: "data type", name: "Town Other Document"
	And I am logged in as "admin"
	When I visit the Show page for town: "one" to see the Documents tab
	And I wait for ajax to finish loading
	# This message only exists prior to initial ajax load
	Then I should not see "Please reload the page if the documents table has not loaded."
	# This element only exists after ajax load
	And I should see the documentsTable element

Scenario: An autocomplete should clear its value when its dependsOn changes
	Given I am at the Layout page
	When I select "Neat" from the DropDown dropdown
	And I click the "Controls" tab
	And I enter "one" and select "one" from the AutoCompleteDependsOn autocomplete field
	And I select "-- Select --" from the DropDown dropdown
	Then I should not see "one" in the AutoCompleteDependsOn_AutoComplete field
