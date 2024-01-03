Feature: SkillSetPage
	In order to manage skill sets
	As a user I need to be able to add, edit and delete skill sets.

Background:
Given a role "skill-read" exists with action: "Read", module: "ProductionDataAdministration"
And a role "skill-edit" exists with action: "Edit", module: "ProductionDataAdministration"
And a role "skill-add" exists with action: "Add", module: "ProductionDataAdministration"
And a role "skill-delete" exists with action: "Delete", module: "ProductionDataAdministration"
And a user "user" exists with username: "user", roles: skill-read;skill-edit;skill-add;skill-delete
And a skill set "specialist" exists with name: "Maintenance Services - Specialist", abbreviation: "MSS", isActive: true, description: "MS Specialist description"
And a skill set "mechanic" exists with name: "Maintenance Services - Mechanic", abbreviation: "MSM", isActive: true, description: "MS Mechanic description"

Scenario: user can view a skill set
	Given I am logged in as "user"
	And I am at the Production/SkillSet/Show page for skill set: "specialist"
	Then I should see a display for Name with "Maintenance Services - Specialist"
	And I should see a display for Abbreviation with "MSS"
	And I should see a display for Description with "MS Specialist description"

Scenario: user can search for a skill set
	Given I am logged in as "user"
	When I visit the Production/SkillSet/Search page
	And I enter "Mechanic" into the Name.Value field
	And I press Search
	Then I should be at the Production/SkillSet page
	And I should see "Maintenance Services - Mechanic" in the "Name" column
	And I should see "MSM" in the "Abbreviation" column
	And I should see "MS Mechanic description" in the "Description" column

Scenario: user can create a skill set
	Given I am logged in as "user"
	When I visit the Production/SkillSet/New page
	And I press Create
	Then I should see the validation message "The Name field is required."
	And I should see the validation message "The Abbreviation field is required."
	And I should see the validation message "The IsActive field is required."
	When I enter "Test Skill" into the Name field
	And I enter "TS" into the Abbreviation field
	And I enter "Description of a Test Skill" into the Description field
	And I press Create
	Then I should see the validation message "The IsActive field is required."
	When I select "Yes" from the IsActive dropdown
	And I press Create
	Then the currently shown skill set will now be known throughout the land as "testskill"
	And I should be at the Production/SkillSet/Show page for skill set: "testskill"
	And I should see a display for Name with "Test Skill"
	And I should see a display for Abbreviation with "TS"
	And I should see a display for Description with "Description of a Test Skill"

	Scenario: user can edit a skill set
	Given I am logged in as "user"
	When I visit the Production/SkillSet/Edit page for skill set "specialist"
	And I enter "New description for Maintenance Services Specialist" into the Description field
	And I press Save
	Then I should see a display for Description with "New description for Maintenance Services Specialist"

