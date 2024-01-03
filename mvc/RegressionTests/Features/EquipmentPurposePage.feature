Feature: EquipmentPurposePage
    In order to form a more perfect union, establish Justice, insure domestic Tranquility, provide for the common defense, promote the general Welfare, and secure the Blessings of Liberty to ourselves and our Posterity
    As We the People of the United States
    I want to ordain and establish this Constitution of the United States of America

Background: Congress shall make no law
    Given an operating center "nj7" exists with opcode: "NJ7", name: "or abridging the freedom of speech, or of the press"
    And an operating center "nj4" exists with opcode: "NJ4", name: "or the right of the people peaceably to assemble"
	And a role "roleReadNj7" exists with action: "Read", module: "FieldServicesEstimatingProjects", operating center: "nj7"
	And a role "roleEditNj7" exists with action: "Edit", module: "FieldServicesEstimatingProjects", operating center: "nj7"
	And a role "roleAddNj7" exists with action: "Add", module: "FieldServicesEstimatingProjects", operating center: "nj7"
	And a role "roleDeleteNj7" exists with action: "Delete", module: "FieldServicesEstimatingProjects", operating center: "nj7"
	And a role "roleReadNj4" exists with action: "Read", module: "FieldServicesEstimatingProjects", operating center: "nj4"
	And a role "roleEditNj4" exists with action: "Edit", module: "FieldServicesEstimatingProjects", operating center: "nj4"
	And a role "roleAddNj4" exists with action: "Add", module: "FieldServicesEstimatingProjects", operating center: "nj4"
	And a role "roleDeleteNj4" exists with action: "Delete", module: "FieldServicesEstimatingProjects", operating center: "nj4"
    And an admin user "admin" exists with username: "admin"
    And a user "user" exists with username: "user"
	And a user "invalid" exists with username: "invalid"
    And a user "read-only-nj7" exists with username: "read-only-nj7", roles: roleReadNj7
    And a user "user-admin-nj7" exists with username: "user-admin-nj7", roles: roleReadNj7;roleEditNj7;roleAddNj7;roleDeleteNj7
    And a user "read-only-nj4" exists with username: "read-only-nj4", roles: roleReadNj4
    And a user "user-admin-nj4" exists with username: "user-admin-nj4", roles: roleReadNj4;roleEditNj4;roleAddNj4;roleDeleteNj4
    And a user "user-admin-both" exists with username: "user-admin-both", roles: roleReadNj4;roleEditNj4;roleAddNj4;roleDeleteNj4;roleReadNj7;roleEditNj7;roleAddNj7;roleDeleteNj7
    And equipment types exist
	And equipment manufacturers exist
    And an equipment purpose "one" exists with description: "some kinda equipment", equipment type: "generator"
    And an equipment purpose "two" exists with description: "some other kinda equipment"
    And an equipment characteristic field type "string" exists with data type: "String"
    And an equipment characteristic field type "dropdown" exists with data type: "DropDown"

Scenario: admin can search for equipment purposes which have no equipment type set and set equipment type
    Given I am logged in as "admin"
    When I visit the /EquipmentPurpose/Search page
    And I check the HasNoEquipmentType field
    And I press Search
	Then I should be at the EquipmentPurpose page
    And I should not see a link to the Show page for equipment purpose "one"
    And I should see a link to the Show page for equipment purpose "two"
    When I follow the Show link for equipment purpose "two"
    Then I should be at the Show page for equipment purpose: "two"
	When I follow "Edit"
    Then I should be at the Edit page for equipment purpose: "two"
    And I should not see equipment purpose "one"'s Description in the EquipmentType dropdown
    When I select equipment type "generator"'s Display from the EquipmentType dropdown
    And I press Save
    Then I should be at the Show page for equipment purpose: "two"
    And I should see a display for EquipmentType with equipment type "generator"

Scenario: admin can add and edit an equipment model
	Given I am logged in as "admin"
	When I visit the /EquipmentModel/Search page
	And I follow "Add"
	And I press Save
	Then I should see the validation message "The EquipmentManufacturer field is required."
	And I should see the validation message "The Description field is required."
	When I select equipment manufacturer "engine"'s Display from the EquipmentManufacturer dropdown
	And I enter "foo" into the Description field
	And I press Save
	Then the currently shown equipment model shall henceforth be known throughout the land as "one"
	And I should see a display for Description with "foo"
	And I should see a display for EquipmentManufacturer with equipment manufacturer "engine"'s Description
	When I follow "Edit"
	And I enter "bar" into the Description field
	And I select equipment manufacturer "generator"'s Display from the EquipmentManufacturer dropdown
	And I press Save
	Then I should see a display for Description with "bar"
	And I should see a display for EquipmentManufacturer with equipment manufacturer "generator"'s Description