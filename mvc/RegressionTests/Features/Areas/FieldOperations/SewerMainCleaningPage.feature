Feature: SewerMainCleaningPage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a town "one" exists with name: "Loch Arbour"
	And operating center: "nj7" exists in town: "one" with abbreviation: "LA"
    And a street "one" exists with town: "one", is active: true
	And a sewer opening "one" exists with opening number: "ABC123", town: "one"
	And a sewer opening "two" exists with opening number: "XYZ789", town: "one"
	And a street "two" exists with town: "one", full st name: "Easy St", is active: true
	And a sewer main cleaning "one" exists with operating center: "nj7", town: "one", street: "one", date: "today", inspected date: "today"
    And a sewer main cleaning "two" exists with operating center: "nj7", town: "one", street: "two"
    And sewer main inspection types exist
    And sewer main inspection grades exist
    And I am logged in as "admin"

Scenario: user can search for a sewer main cleaning
    When I visit the FieldOperations/SewerMainCleaning/Search page
    And I press Search
    Then I should see a link to the Show page for sewer main cleaning: "one"
    When I follow the Show link for sewer main cleaning "one"
    Then I should be at the Show page for sewer main cleaning: "one"

Scenario: user can view a sewer main cleaning
    When I visit the Show page for sewer main cleaning: "one"
    Then I should see a display for sewer main cleaning: "one"'s TableNotes

Scenario: user can add a sewer main cleaning
    When I visit the FieldOperations/SewerMainCleaning/New page
	And I enter "" into the Date field
    And I press Save
	Then I should see the validation message "The OperatingCenter field is required."
	And I should see the validation message "The Date field is required."
	And I should see the validation message "The InspectedDate field is required."
    And I should see the validation message "The InspectionType field is required."
	And I should see the validation message "The FootageOfMainInspected field is required."
	And I should see the validation message "The Opening1 field is required."
	And I should see the validation message "The Opening2 field is required."
	When I select operating center "nj7" from the OperatingCenter dropdown
    And I press Save
	Then I should see the validation message "The Town field is required."
	When I select town "one" from the Town dropdown
	And I enter today's date into the Date field
	And I enter today's date into the InspectedDate field
	And I select sewer main inspection type "smoke test" from the InspectionType dropdown
	And I enter "foo" into the TableNotes field
	And I select "No" from the Overflow dropdown
	And I enter "123" into the FootageOfMainInspected field
	And I enter "123" and select "ABC123" from the Opening1 autocomplete field 
	And I enter "XYZ" and select "XYZ789" from the Opening2 autocomplete field 
	And I press Save
    Then the currently shown sewer main cleaning will now be referred to as "alastair"
	And I should see a display for TableNotes with "foo"
    And I should see a display for InspectionType with sewer main inspection type "smoke test"

Scenario: user can edit a sewer main cleaning
    When I visit the Edit page for sewer main cleaning: "one"
    And I enter "bar" into the TableNotes field
	And I enter "123" into the FootageOfMainInspected field
    And I press Save
	Then I should see the validation message "The Opening1 field is required."
	And I should see the validation message "The Opening2 field is required."
	When I enter "123" and select "ABC123" from the Opening1 autocomplete field 
	And I enter "XYZ" and select "XYZ789" from the Opening2 autocomplete field 
    And I select sewer main inspection type "smoke test" from the InspectionType dropdown
	And I press Save
    Then I should be at the Show page for sewer main cleaning: "one"
    And I should see a display for TableNotes with "bar"
    And I should see a display for InspectionType with sewer main inspection type "smoke test"
	
Scenario: User can add / edit new sewer main cleaning with sewer openings attached to the record
	When I visit the FieldOperations/SewerMainCleaning/New page
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select town "one" from the Town dropdown
	And I enter today's date into the Date field
	And I enter today's date into the InspectedDate field
    And I select sewer main inspection type "smoke test" from the InspectionType dropdown
	And I enter "foo" into the TableNotes field
	And I select "No" from the Overflow dropdown
	And I enter "123" into the FootageOfMainInspected field
	And I enter "123" and select "ABC123" from the Opening1 autocomplete field 
	And I enter "XYZ" and select "XYZ789" from the Opening2 autocomplete field 
	And I press Save
	Then the currently shown sewer main cleaning will now be referred to as "autostuff"
	And I should be at the show page for sewer main cleaning: "autostuff"
	Then I should see a display for Opening1 with "ABC123"
	And I should see a link to the Show page for sewer opening: "one"
	And I should see a display for Opening2 with "XYZ789"
	And I should see a link to the Show page for sewer opening: "two"
	# Verify that the autocomplete gets repopulating correctly and that saving again doesn't wipe it out.
	When I visit the Edit page for sewer main cleaning: "autostuff"
	Then I should see "ABC123" in the Opening1_AutoComplete field
	And I should see "XYZ789" in the Opening2_AutoComplete field
	When I press Save
	Then I should be at the Show page for sewer main cleaning: "autostuff"
	And I should see a display for Opening1 with "ABC123"
	And I should see a link to the Show page for sewer opening: "one"
	And I should see a display for Opening2 with "XYZ789"
	And I should see a link to the Show page for sewer opening: "two"
	When I visit the Edit page for sewer main cleaning: "autostuff"
	Then I should see "ABC123" in the Opening1_AutoComplete field
	And I should see "XYZ789" in the Opening2_AutoComplete field

Scenario: user sees complex visibility/validation rules for main inspection grade
    When I visit the FieldOperations/SewerMainCleaning/New page
    And I select sewer main inspection type "acoustic" from the InspectionType dropdown
    Then I should see the InspectionGrade field
    When I select sewer main inspection type "cctv" from the InspectionType dropdown
    Then I should see the InspectionGrade field
    When I select sewer main inspection type "main cleaning pm" from the InspectionType dropdown
    Then I should not see the InspectionGrade field
    When I select sewer main inspection type "smoke test" from the InspectionType dropdown
    Then I should not see the InspectionGrade field
    When I check the BlockageFound checkbox
    Then I should see the CauseOfBlockage field
    When I uncheck the BlockageFound checkbox
    Then I should not see the CauseOfBlockage field 