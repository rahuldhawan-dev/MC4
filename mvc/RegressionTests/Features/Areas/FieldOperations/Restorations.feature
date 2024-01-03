Feature: Restorations
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background:
	Given a user "user" exists with username: "user"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a role "hydrant-useradmin" exists with action: "UserAdministrator", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And an employee "manager" exists
	And an employee "fleetcontactperson" exists
	And an employee "primarydriver" exists with operating center: "nj7"

Scenario: User should see validation messages 
	Given I am logged in as "user"
	And a work order "one" exists
	And I am at the FieldOperations/Restoration/New page 
	When I press Save
	Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
	#And I should see a validation message for WBSNumber with "The WBS # field is required."
	And I should see a validation message for RestorationType with "The Type of Restoration field is required."
	And I should see a validation message for EstimatedRestorationFootage with "The EstimatedRestorationFootage field is required."
	#And I should see a validation message for RestorationNotes with "The RestorationNotes field is required."
	And I should see a validation message for ResponsePriority with "The Priority field is required."
	#And I should see a validation message for AssignedContractor with "The AssignedContractor field is required."
	When I enter "10" into the EstimatedRestorationFootage field
	And I enter "20" into the PartialPavingSquareFootage field 
	And I press Save 
	Then I should see a validation message for PartialRestorationNotes with "Partial restoration notes are required when actual square footage is greater than estimated square footage."
	When I enter "20" into the FinalPavingSquareFootage field 
	And I press Save 
	Then I should see a validation message for FinalRestorationNotes with "Final restoration notes are required when actual square footage is greater than estimated square footage."
	When I enter "1/1/2000" into the PartialRestorationDate field
	And I press Save
	Then I should see a validation message for PartialRestorationCompletedBy with "The Completed By field is required."
	And I should see a validation message for PartialRestorationInvoiceNumber with "The Invoice Number field is required."
	And I should see a validation message for PartialRestorationActualCost with "The PartialRestorationActualCost field is required."
	When I enter "1/1/2000" into the FinalRestorationDate field
	And I press Save
	Then I should see a validation message for FinalRestorationCompletedBy with "The Completed By field is required."
	And I should see a validation message for FinalRestorationInvoiceNumber with "The Invoice Number field is required."
	And I should see a validation message for FinalRestorationActualCost with "The FinalRestorationActualCost field is required."
