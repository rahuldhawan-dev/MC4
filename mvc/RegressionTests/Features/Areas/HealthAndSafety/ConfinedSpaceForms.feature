Feature: ConfinedSpaceForms

Background:
	Given an employee status "active" exists with description: "Active"
	And an employee "one" exists with status: "active", first name: "Bill", last name: "S. Preston", employee id: "1000001"
	And an employee "two" exists with status: "active", first name: "Johnny", last name: "Hotdog", employee id: "2000002"
	And an employee "three" exists with status: "active", first name: "Tom", last name: "Third", employee id: "3000003"
	And a user "user" exists with username: "user", employee: "one"
	And a user "other-user" exists with username: "other-user", employee: "two"
	And a state "nj" exists with abbreviation: "NJ"
    And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a facility "one" exists with operating center: "nj7"
	And a equipment type "generator" exists with description: "generator"
	And an equipment manufacturer "generator" exists with equipment type: "generator", description: "a description"
	And a role "fieldservicesworkmanagement-useradmin" exists with action: "UserAdministrator", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a role "operationslockoutforms-useradmin" exists with action: "UserAdministrator", module: "OperationsLockoutForms", user: "user", operating center: "nj7"
	And a role "operationslockoutforms-useradmin2" exists with action: "UserAdministrator", module: "OperationsLockoutForms", user: "other-user", operating center: "nj7"
	And a role "productionworkmanagement-useradmin" exists with action: "UserAdministrator", module: "ProductionWorkManagement", user: "user", operating center: "nj7"
	And a role "productionworkmanagement-useradmin2" exists with action: "UserAdministrator", module: "ProductionWorkManagement", user: "other-user", operating center: "nj7"
	And production prerequisites exist
	And a production work order "one" exists with operating center: "nj7"	
	And a confined space form hazard type "one" exists with description: "Hazard Type One"
	And a confined space form hazard type "two" exists with description: "Hazard Type Two"
	And an other method of communication "other" exists
	And a voice method of communication "voice" exists
	And a production work order production prerequisite "one" exists with production work order: "one", production prerequisite: "is confined space"	
	And a data type "confinedspaceform" exists with table name: "ConfinedSpaceForms"
	And a confined space form reading capture time "pre" exists with description: "Pre-Entry"
	And a confined space form reading capture time "during" exists with description: "During Entry"
	And a confined space form reading capture time "post" exists with description: "Post Entry"

Scenario: User must have operations lockout form role to see the create confined space form button on production work order record
	Given a user "user-without-necessary-role" exists with username: "user-without-necessary-role", employee: "three"
	And a role "productionworkmanagement-useradmin-for-bad-user-no-one-likes" exists with action: "UserAdministrator", module: "ProductionWorkManagement", user: "user-without-necessary-role", operating center: "nj7"
	And I am logged in as "user"
	And I am at the Show page for production work order "one"
	When I click the "Confined Space Forms" tab
	Then I should see the link "Create Confined Space Form"
	When I log in as "user-without-necessary-role"
	And I visit the Show page for production work order "one"
	When I click the "Confined Space Forms" tab
	Then I should not see the link "Create Confined Space Form"

Scenario: Once the hazard section is signed then no users should be able to sign it again
	Given a confined space form "one" exists with production work order: "one"
	# an out-of-range test is needed for the hazard section to be enabled
	And a confined space form atmospheric test "one" exists with confined space form: "one"
	And I am logged in as "user"
	And I am at the Edit page for confined space form "one"
	When I click the "Section 4" tab
	And I check the IsHazardSectionSigned field
	And I press Save
	Then I should be at the Edit page for confined space form "one"
	When I visit the Show page for confined space form "one"
	And I click the "Section 4" tab
	Then I should see a display for HazardSignedBy with employee "one"
	When I visit the Edit page for confined space form "one"
	And I click the "Section 4" tab
	Then I should not see the IsHazardSectionSigned field
	And I should see a display for Original_HazardSignedBy with employee "one"
	# and test that another user can't overwrite this
	Given I am logged in as "other-user"
	And I am at the Edit page for confined space form "one"
	When I press Save
	Then I should be at the Edit page for confined space form "one"
	When I click the "Section 4" tab
	Then I should see a display for Original_HazardSignedBy with employee "one"

Scenario: Once the permit cancellation section is signed then no users should be able to sign it again
	Given a confined space form "one" exists with production work order: "one" 
	And a confined space form atmospheric test "one" exists with confined space form: "one"
	And an confined space form entrant type "attendant" exists with description: "Attendant"
	And an confined space form entrant type "entrant" exists with description: "Entrant"
	And I am logged in as "user"
	And I am at the Edit page for confined space form "one"
	When I click the "Section 4" tab
	And I select "No" from the CanBeControlledByVentilationAlone dropdown
	And I check the IsHazardSectionSigned field
	And I click the "Section 5" tab
	And I enter "4/24/2020 4:04 AM" into the PermitBeginsAt field
	And I enter "4/24/2024 4:04 AM" into the PermitEndsAt field
	And I select "Yes" from the HasRetrievalSystem dropdown
	And I select "Yes" from the HasContractRescueService dropdown
	And I enter "Space unconfiners inc" into the EmergencyResponseAgency field
	And I enter "Chuckford Steaksly" into the EmergencyResponseContact field
	And I check the IsPermitCancelledSectionSigned field
	And I enter "notes lolololol" into the PermitCancellationNote field 
	And I click the "Role Assignment" tab
	And I press "Add An Assignment"
	And I check the NewEntrants[0].IsEmployee field
	And I select confined space form entrant type "attendant" from the NewEntrants[0].EntrantType dropdown
	And I enter "pres" and select "S. Preston, Bill - 1000001 - NJ7" from the NewEntrants[0]_Employee autocomplete field
	And I press Save
	Then I should be at the Show page for confined space form "one"
	When I click the "Section 5" tab
	Then I should see a display for PermitCancelledBy with employee "one"
	When I try to visit the HealthAndSafety/ConfinedSpaceForm/Edit/1 page 
	Then I should be at the Show page for confined space form "one"
	When I click the "Section 5" tab
	Then I should not see the IsPermitCancelledSectionSigned field
	And I should see a display for PermitCancelledBy with employee "one"
	When I click the "Notes" tab 
	Then I should see "notes lolololol"

Scenario: User can add and edit hazard checklist
	Given a confined space form "one" exists with production work order: "one", can be controlled by ventilation alone: "false"
	And a confined space form atmospheric test "one" exists with confined space form: "one"
	And a confined space form hazard type "three" exists with description: "Hazard Type Three"
	And an confined space form entrant type "attendant" exists with description: "Attendant"
	And an confined space form entrant type "entrant" exists with description: "Entrant"
	And I am logged in as "user"
	And I am at the Edit page for confined space form "one"
	When I click the "Section 5" tab
	And I check the Hazards_0__IsChecked field
	And I enter "some hazard type one notes" into the Hazards_0__Notes field
	And I check the Hazards_2__IsChecked field
	And I enter "some hazard type three notes" into the Hazards_2__Notes field
	And I click the "Section 4" tab
	And I select "No" from the CanBeControlledByVentilationAlone dropdown
	And I check the IsHazardSectionSigned field
	And I click the "Section 5" tab
	And I enter "4/24/2020 4:04 AM" into the PermitBeginsAt field
	And I enter "4/24/2024 4:04 AM" into the PermitEndsAt field
	And I select "Yes" from the HasRetrievalSystem dropdown
	And I select "Yes" from the HasContractRescueService dropdown
	And I enter "Space unconfiners inc" into the EmergencyResponseAgency field
	And I enter "Chuckford Steaksly" into the EmergencyResponseContact field
	And I click the "Role Assignment" tab
	And I press "Add An Assignment"
	And I check the NewEntrants[0].IsEmployee field
	And I select confined space form entrant type "attendant" from the NewEntrants[0].EntrantType dropdown
	And I enter "pres" and select "S. Preston, Bill - 1000001 - NJ7" from the NewEntrants[0]_Employee autocomplete field
	And I press Save
	Then I should be at the /HealthAndSafety/ConfinedSpaceForm/PostCompletionEdit page for confined space form "one"
	When I visit the Show page for confined space form "one"
	And I click the "Section 5" tab
	# This is very specific to confined space forms and it can't use the normal display steps
	Then I should see "Hazard Type One"
	And I should see "some hazard type one notes"
	And the HazardType checkbox for confined space form hazard type "one" should be checked
	And I should see "Hazard Type Two" 
	And the HazardType checkbox for confined space form hazard type "two" should not be checked
	And I should see "Hazard Type Three"
	And I should see "some hazard type three notes"
	And the HazardType checkbox for confined space form hazard type "three" should be checked

Scenario: Selecting from the hazard checklist should toggle visibility of notes field
    Given a confined space form "one" exists with production work order: "one", can be controlled by ventilation alone: "false"
	And a confined space form atmospheric test "one" exists with confined space form: "one"
	And I am logged in as "user"
	And I am at the Edit page for confined space form "one"
	When I click the "Section 5" tab
	# These are based on the two hazard types created in the Background section
	And I check the Hazards_0__IsChecked field
	Then I should see the Hazards_0__Notes field
	And I should not see the Hazards_1__Notes field
	When I uncheck the Hazards_0__IsChecked field
	Then I should not see the Hazards_0__Notes field
	And I should not see the Hazards_1__Notes field

Scenario: User should only see the method of communication other notes field when method of communication is set to Other
	Given a confined space form "one" exists with production work order: "one", can be controlled by ventilation alone: "false"
	And a confined space form atmospheric test "one" exists with confined space form: "one"
	And I am logged in as "user"
	And I am at the Edit page for confined space form "one"
	When I click the "Section 5" tab
	And I select other method of communication "other" from the MethodOfCommunication dropdown
	Then I should see the MethodOfCommunicationOtherNotes field
	When I select voice method of communication "voice" from the MethodOfCommunication dropdown
	Then I should not see the MethodOfCommunicationOtherNotes field

Scenario: User should only see the has other safety equipment notes field when has other safety equipment is set to Yes
	Given a confined space form "one" exists with production work order: "one", can be controlled by ventilation alone: "false"
	And a confined space form atmospheric test "one" exists with confined space form: "one"
	And I am logged in as "user"
	And I am at the Edit page for confined space form "one"
	When I click the "Section 5" tab
	And I select "Yes" from the HasOtherSafetyEquipment dropdown
	Then I should see the HasOtherSafetyEquipmentNotes field
	When I select "No" from the HasOtherSafetyEquipment dropdown
	Then I should not see the HasOtherSafetyEquipmentNotes field

Scenario: User can add an entrant for an employee
	Given a confined space form "one" exists with production work order: "one", can be controlled by ventilation alone: "false", permit begins at: "today", permit ends at: "today", has retrieval system: "true", has contract rescue service: "true", emergency response agency: "some agency", emergency response contact: "some contact"
	And a confined space form atmospheric test "one" exists with confined space form: "one"
	And an confined space form entrant type "attendant" exists with description: "Attendant"
	And an confined space form entrant type "entrant" exists with description: "Entrant"
	And I am logged in as "user"
	And I am at the Edit page for confined space form "one"
	When I click the "Section 4" tab
	And I check the IsHazardSectionSigned field
	And I click the "Role Assignment" tab
	And I press "Add An Assignment"
	And I check the NewEntrants[0].IsEmployee field
	And I select confined space form entrant type "attendant" from the NewEntrants[0].EntrantType dropdown
	And I enter "pres" and select "S. Preston, Bill - 1000001 - NJ7" from the NewEntrants[0]_Employee autocomplete field
	And I press "Save"
	And I click the "Role Assignment" tab
	Then I should see the following values in the entrantsTable table
         | Entry Assignment/Role | Employee        | Contracting Company | Contractor Name |
         | Attendant             | Bill S. Preston |                     |                 |

Scenario: User can add an entrant for a contractor
	Given a confined space form "one" exists with production work order: "one", can be controlled by ventilation alone: "false", permit begins at: "today", permit ends at: "today", has retrieval system: "true", has contract rescue service: "true", emergency response agency: "some agency", emergency response contact: "some contact"
	And a confined space form atmospheric test "one" exists with confined space form: "one"
	And an confined space form entrant type "attendant" exists with description: "Attendant"
	And an confined space form entrant type "entrant" exists with description: "Entrant"
	And I am logged in as "user"
	And I am at the Edit page for confined space form "one"
	When I click the "Section 4" tab
	And I check the IsHazardSectionSigned field
	And I click the "Role Assignment" tab
	And I press "Add An Assignment"
	And I uncheck the NewEntrants[0].IsEmployee field
	And I enter "Some company" into the NewEntrants[0].ContractingCompany field
	And I enter "Some name" into the NewEntrants[0].ContractorName field
	And I select confined space form entrant type "attendant" from the NewEntrants[0].EntrantType dropdown
	And I press "Save"
	And I click the "Role Assignment" tab
	Then I should see the following values in the entrantsTable table
         | Entry Assignment/Role | Employee | Contracting Company | Contractor Name |
         | Attendant             |          | Some company        | Some name       |

Scenario: User can remove an existing entrant
	Given a confined space form "one" exists with production work order: "one", can be controlled by ventilation alone: "false", permit begins at: "today", permit ends at: "today", has retrieval system: "true", has contract rescue service: "true", emergency response agency: "some agency", emergency response contact: "some contact"
	And a confined space form atmospheric test "one" exists with confined space form: "one"
	And an confined space form entrant type "attendant" exists with description: "Attendant"
	And an confined space form entrant type "entrant" exists with description: "Entrant"
	And a confined space form entrant "one" exists with confined space form: "one", contracting company: "some company 1", contractor name: "some contractor 1"
	And a confined space form entrant "two" exists with confined space form: "one", contracting company: "some company 2", contractor name: "some contractor 2"
	And a confined space form entrant "three" exists with confined space form: "one", contracting company: "some company 3", contractor name: "some contractor 3"
	And I am logged in as "user"
	And I am at the Edit page for confined space form "one"
	When I click the "Section 4" tab
	And I check the IsHazardSectionSigned field
	And I click the "Role Assignment" tab
	And I click the checkbox named RemovedEntrants with confined space form entrant "two"'s Id
	And I press "Save"
	And I click the "Role Assignment" tab
	Then I should see the following values in the entrantsTable table
         | Contracting Company | Contractor Name   |
         | some company 1      | some contractor 1 |
         | some company 3      | some contractor 3 |

Scenario: User can add multiple entrants at a time
	Given a confined space form "one" exists with production work order: "one", can be controlled by ventilation alone: "false", permit begins at: "today", permit ends at: "today", has retrieval system: "true", has contract rescue service: "true", emergency response agency: "some agency", emergency response contact: "some contact"
	And a confined space form atmospheric test "one" exists with confined space form: "one"
	And an confined space form entrant type "entrant" exists with description: "Entrant"
	And an confined space form entrant type "attendant" exists with description: "Attendant"
	And I am logged in as "user"
	And I am at the Edit page for confined space form "one"
	When I click the "Section 4" tab
	And I check the IsHazardSectionSigned field
	And I click the "Role Assignment" tab
	And I press "Add An Assignment"
	And I check the NewEntrants[0].IsEmployee field
	And I select confined space form entrant type "attendant" from the NewEntrants[0].EntrantType dropdown
	And I enter "pres" and select "S. Preston, Bill - 1000001 - NJ7" from the NewEntrants[0]_Employee autocomplete field
	And I press "Add An Assignment"
	And I check the NewEntrants[1].IsEmployee field
	And I select confined space form entrant type "entrant" from the NewEntrants[1].EntrantType dropdown
	And I enter "hot" and select "Hotdog, Johnny - 2000002 - NJ7" from the NewEntrants[1]_Employee autocomplete field
	And I press "Add An Assignment"
	And I uncheck the NewEntrants[2].IsEmployee field
	And I select confined space form entrant type "attendant" from the NewEntrants[2].EntrantType dropdown
	And I enter "Some company" into the NewEntrants[2].ContractingCompany field
	And I enter "Some name" into the NewEntrants[2].ContractorName field
	And I press "Save"
	Then I should be at the /HealthAndSafety/ConfinedSpaceForm/PostCompletionEdit page for confined space form "one"
	When I visit the Show page for confined space form "one"
	And I click the "Role Assignment" tab
	Then I should see the following values in the entrantsTable table
         | Entry Assignment/Role | Employee        | Contracting Company | Contractor Name |
         | Attendant             | Bill S. Preston |                     |                 |
         | Attendant             |                 | Some company        | Some name       |
         | Entrant               | Johnny Hotdog   |                     |                 |

Scenario: User can add multiple new entrants, then remove one of them before saving and it won't cause saving to fail
# this is testing the fragility of dealing with List properties and the model binder. The server-side won't
# know if the client-side messed up, so this test is important.
	Given a confined space form "one" exists with production work order: "one", can be controlled by ventilation alone: "false", permit begins at: "today", permit ends at: "today", has retrieval system: "true", has contract rescue service: "true", emergency response agency: "some agency", emergency response contact: "some contact"
	And a confined space form atmospheric test "one" exists with confined space form: "one"
	And an confined space form entrant type "attendant" exists with description: "Attendant"
	And an confined space form entrant type "entrant" exists with description: "Entrant"
	And I am logged in as "user"
	And I am at the Edit page for confined space form "one"
	When I click the "Section 4" tab
	And I check the IsHazardSectionSigned field
	And I click the "Role Assignment" tab
	And I press "Add An Assignment"
	And I check the NewEntrants[0].IsEmployee field
	And I select confined space form entrant type "attendant" from the NewEntrants[0].EntrantType dropdown
	And I enter "pres" and select "S. Preston, Bill - 1000001 - NJ7" from the NewEntrants[0]_Employee autocomplete field
	And I press "Add An Assignment"
	And I press RemoveEntrant1
	And I press "Add An Assignment"
	And I uncheck the NewEntrants[2].IsEmployee field
	And I select confined space form entrant type "attendant" from the NewEntrants[2].EntrantType dropdown
	And I enter "Some company" into the NewEntrants[2].ContractingCompany field
	And I enter "Some name" into the NewEntrants[2].ContractorName field
	And I press "Save"
	And I click the "Role Assignment" tab
	Then I should see the following values in the entrantsTable table
         | Entry Assignment/Role | Employee        | Contracting Company | Contractor Name |
         | Attendant             | Bill S. Preston |                     |                 |
         | Attendant             |                 | Some company        | Some name       |

Scenario: User can add an employee as an attendant and a supervisor but not also as an entrant
	Given a confined space form "one" exists with production work order: "one", can be controlled by ventilation alone: "false", permit begins at: "today", permit ends at: "today", has retrieval system: "true", has contract rescue service: "true", emergency response agency: "some agency", emergency response contact: "some contact"
	And a confined space form atmospheric test "one" exists with confined space form: "one"
	And an confined space form entrant type "entrant" exists with description: "Entrant"
	And an confined space form entrant type "attendant" exists with description: "Attendant"
	And an confined space form entrant type "supervisor" exists with description: "Entry Supervisor"
	And I am logged in as "user"
	And I am at the Edit page for confined space form "one"
	When I click the "Role Assignment" tab
	And I press "Add An Assignment"
	And I select confined space form entrant type "entrant" from the NewEntrants[0].EntrantType dropdown
	And I enter "pres" and select "S. Preston, Bill - 1000001 - NJ7" from the NewEntrants[0]_Employee autocomplete field
	And I press "Add An Assignment"
	And I select confined space form entrant type "entrant" from the NewEntrants[1].EntrantType dropdown
	# this next line is an assertion, despite the 'When'
	# cannot be an entrant twice
	And I enter "pres" not expecting to see "S. Preston, Bill - 1000001 - NJ7" in the NewEntrants[1]_Employee autocomplete field
	And I press RemoveEntrant1
	And I press RemoveEntrant0
	And I press "Add An Assignment"
	And I select confined space form entrant type "attendant" from the NewEntrants[2].EntrantType dropdown
	And I enter "pres" and select "S. Preston, Bill - 1000001 - NJ7" from the NewEntrants[2]_Employee autocomplete field
	And I press "Add An Assignment"
	And I select confined space form entrant type "entrant" from the NewEntrants[3].EntrantType dropdown
	# can't be an attendant and also an entrant
	And I enter "pres" not expecting to see "S. Preston, Bill - 1000001 - NJ7" in the NewEntrants[3]_Employee autocomplete field
	And I select confined space form entrant type "attendant" from the NewEntrants[3].EntrantType dropdown
	# can't be an attendant twice
	And I enter "pres" not expecting to see "S. Preston, Bill - 1000001 - NJ7" in the NewEntrants[3]_Employee autocomplete field
	And I select confined space form entrant type "supervisor" from the NewEntrants[3].EntrantType dropdown
	# assert value does appear
	And I enter "pres" and select "S. Preston, Bill - 1000001 - NJ7" from the NewEntrants[3]_Employee autocomplete field
	And I press RemoveEntrant3
	And I press RemoveEntrant2
	And I press "Add An Assignment"
	And I select confined space form entrant type "supervisor" from the NewEntrants[4].EntrantType dropdown
	And I enter "pres" and select "S. Preston, Bill - 1000001 - NJ7" from the NewEntrants[4]_Employee autocomplete field
	And I press "Add An Assignment"
	And I select confined space form entrant type "entrant" from the NewEntrants[5].EntrantType dropdown
	# can't be a supervisor and also an entrant
	And I enter "pres" not expecting to see "S. Preston, Bill - 1000001 - NJ7" in the NewEntrants[5]_Employee autocomplete field
	And I select confined space form entrant type "supervisor" from the NewEntrants[5].EntrantType dropdown
	# can't be a supervisor twice
	And I enter "pres" not expecting to see "S. Preston, Bill - 1000001 - NJ7" in the NewEntrants[5]_Employee autocomplete field
	And I select confined space form entrant type "attendant" from the NewEntrants[5].EntrantType dropdown
	# assert value does appear
	And I enter "pres" and select "S. Preston, Bill - 1000001 - NJ7" from the NewEntrants[5]_Employee autocomplete field

Scenario: Toggling the Is Employee checkbox should change field visibility and validation requirements when adding an entrant
	Given a confined space form "one" exists with production work order: "one", can be controlled by ventilation alone: "false", permit begins at: "today", permit ends at: "today", has retrieval system: "true", has contract rescue service: "true", emergency response agency: "some agency", emergency response contact: "some contact", is hazard section signed: "true"
	And a confined space form atmospheric test "one" exists with confined space form: "one"
	And an confined space form entrant type "attendant" exists with description: "Attendant"
	And an confined space form entrant type "entrant" exists with description: "Entrant"
	And I am logged in as "user"
	And I am at the Edit page for confined space form "one"
	When I click the "Section 4" tab
	And I check the IsHazardSectionSigned field
	And I click the "Role Assignment" tab
	And I press "Add An Assignment"
	And I press "Save"
	Then I should see a validation message for NewEntrants[0].EntrantType with "The Entry Assignment/Role field is required."
	And I should see a validation message for NewEntrants[0].Employee with "The Employee field is required."
	And I should see the NewEntrants[0]_Employee_AutoComplete field
	And I should not see a validation message for NewEntrants[0].ContractingCompany with "The ContractingCompany field is required."
	And I should not see the NewEntrants[0]_ContractingCompany field
	And I should not see a validation message for NewEntrants[0].ContractorName with "The ContractorName field is required."
	And I should not see the NewEntrants[0]_ContractorName field
	When I uncheck the NewEntrants[0].IsEmployee field
	And I press "Save"
	Then I should see a validation message for NewEntrants[0].EntrantType with "The Entry Assignment/Role field is required."
	And I should not see a validation message for NewEntrants[0].Employee with "The Employee field is required."
	And I should not see the NewEntrants[0]_Employee_AutoComplete field
	And I should see a validation message for NewEntrants[0].ContractingCompany with "The ContractingCompany field is required."
	And I should see the NewEntrants[0]_ContractingCompany field
	And I should see a validation message for NewEntrants[0].ContractorName with "The ContractorName field is required."
	And I should see the NewEntrants[0]_ContractorName field

Scenario: User selects Hot Work Permit required to Yes sets  Fire Watch Required to Yes
	Given a confined space form "one" exists with production work order: "one", can be controlled by ventilation alone: "false"
	And a confined space form atmospheric test "one" exists with confined space form: "one"
	And I am logged in as "user"
	And I am at the Edit page for confined space form "one"
	When I click the "Section 5" tab
	And I select "Yes" from the IsHotWorkPermitRequired dropdown
	Then "Yes" should be selected in the IsFireWatchRequired dropdown

Scenario: Section 5 should only be visible on the show page when CanBeControlledByVentilationAlone is false
	Given a confined space form "showsection5" exists with can be controlled by ventilation alone: "false"
	And a confined space form atmospheric test "one" exists with confined space form: "showsection5"
	And a confined space form "dontshowsection5" exists with can be controlled by ventilation alone: "true"
	And a confined space form atmospheric test "two" exists with confined space form: "dontshowsection5"
	# this is null by default
	And a confined space form "stilldontshowsection5" exists
	And I am logged in as "user"
	When I visit the Show page for confined space form "showsection5"
	Then I should see the "Section 5" tab
	And I should see the "Role Assignment" tab
	When I visit the Show page for confined space form "dontshowsection5"
	Then I should not see the "Section 5" tab
	And I should not see the "Role Assignment" tab
	When I visit the Show page for confined space form "stilldontshowsection5"
	Then I should not see the "Section 5" tab
	And I should not see the "Role Assignment" tab

Scenario: Section 5 should only be enabled for editing when CanBeControlledByVentilationAlone is false
	Given a confined space form "one" exists with production work order: "one"
	And a confined space form atmospheric test "one" exists with confined space form: "one"
	And I am logged in as "user"
	And I am at the Edit page for confined space form "one"
	When I click the "Section 4" tab
	And I select "-- Select --" from the CanBeControlledByVentilationAlone dropdown
	Then the "Section 5" tab should be disabled
	And the "Role Assignment" tab should be disabled
	When I select "No" from the CanBeControlledByVentilationAlone dropdown
	Then the "Section 5" tab should be enabled
	And the "Role Assignment" tab should be enabled
	When I select "Yes" from the CanBeControlledByVentilationAlone dropdown
	Then the "Section 5" tab should be disabled
	And the "Role Assignment" tab should be disabled

Scenario: Section 5 has required fields only when it is enabled
	Given a confined space form "one" exists with production work order: "one"
	And a confined space form atmospheric test "one" exists with confined space form: "one"
	And I am logged in as "user"
	And I am at the Edit page for confined space form "one"
	When I click the "Section 4" tab
	And I select "No" from the CanBeControlledByVentilationAlone dropdown
	And I click the "Section 5" tab
	And I press Save
	And I click the "Section 5" tab
	Then I should see the validation message The PermitBeginsAt field is required.
	Then I should see the validation message The PermitEndsAt field is required.
	Then I should see the validation message The HasRetrievalSystem field is required.
	Then I should see the validation message The HasContractRescueService field is required.
	Then I should see the validation message The EmergencyResponseAgency field is required.
	Then I should see the validation message The EmergencyResponseContact field is required.
	Then I should not see the HasOtherSafetyEquipmentNotes field

Scenario: User can add an atmospheric test to a confined space form
	Given a confined space form "one" exists with production work order: "one"
	And I am logged in as "user"
	And I am at the Edit page for confined space form "one"
	When I click the "Section 2" tab
	And I press "Add New Test"
	And I press "Save"
	Then I should see a validation message for NewAtmosphericTests[0].TestedAt with "The TestedAt field is required."
	And I should see a validation message for NewAtmosphericTests[0].ConfinedSpaceFormReadingCaptureTime with "The When Was Reading Taken field is required."
	When I enter "8/24/2020 4:24 PM" into the NewAtmosphericTests[0].TestedAt field
	And I select confined space form reading capture time "pre" from the NewAtmosphericTests[0].ConfinedSpaceFormReadingCaptureTime dropdown
	And I enter "21" into the NewAtmosphericTests[0]_OxygenPercentageTop field
	And I enter "22" into the NewAtmosphericTests[0]_OxygenPercentageMiddle field
	And I enter "23" into the NewAtmosphericTests[0]_OxygenPercentageBottom field
	And I enter "1" into the NewAtmosphericTests[0]_LowerExplosiveLimitPercentageTop field
	And I enter "2" into the NewAtmosphericTests[0]_LowerExplosiveLimitPercentageMiddle field
	And I enter "3" into the NewAtmosphericTests[0]_LowerExplosiveLimitPercentageBottom field
	And I enter "4" into the NewAtmosphericTests[0]_CarbonMonoxidePartsPerMillionTop field
	And I enter "5" into the NewAtmosphericTests[0]_CarbonMonoxidePartsPerMillionMiddle field
	And I enter "6" into the NewAtmosphericTests[0]_CarbonMonoxidePartsPerMillionBottom field
	And I enter "7" into the NewAtmosphericTests[0]_HydrogenSulfidePartsPerMillionTop field
	And I enter "8" into the NewAtmosphericTests[0]_HydrogenSulfidePartsPerMillionMiddle field
	And I enter "9" into the NewAtmosphericTests[0]_HydrogenSulfidePartsPerMillionBottom field
	And I press "Save"
	And I click the "Section 2" tab
	# The table id is just the array index. So first item is 0, etc.
	Then I should see "Pre-Entry" in the ConfinedSpaceFormReadingCaptureTime-0 element
	And I should see the following values in the atmospheric-test-0 table
	| Top     | Middle  | Bottom  |
	| 21.00 % | 22.00 % | 23.00 % |
	| 1.00 %  | 2.00 %  | 3.00 %  |
	| 4       | 5       | 6       |
	| 7       | 8       | 9       |
	
Scenario: User can add an atmospheric test with out of range acceptable concentrations
	Given a confined space form "one" exists with production work order: "one"
	And I am logged in as "user"
	And I am at the Edit page for confined space form "one"
	When I click the "Section 2" tab
	And I press "Add New Test"
	Then I should not see the NewAtmosphericTests[0]_AcknowledgedValuesAreOutOfRange field
	When I enter "8/24/2020 4:24 PM" into the NewAtmosphericTests[0].TestedAt field
	And I select confined space form reading capture time "pre" from the NewAtmosphericTests[0].ConfinedSpaceFormReadingCaptureTime dropdown
	And I enter "11" into the NewAtmosphericTests[0]_OxygenPercentageTop field
	Then I should see the NewAtmosphericTests[0]_AcknowledgedValuesAreOutOfRange field
	When I enter "22" into the NewAtmosphericTests[0]_OxygenPercentageMiddle field
	And I enter "23" into the NewAtmosphericTests[0]_OxygenPercentageBottom field
	And I enter "1" into the NewAtmosphericTests[0]_LowerExplosiveLimitPercentageTop field
	And I enter "2" into the NewAtmosphericTests[0]_LowerExplosiveLimitPercentageMiddle field
	And I enter "3" into the NewAtmosphericTests[0]_LowerExplosiveLimitPercentageBottom field
	And I enter "4" into the NewAtmosphericTests[0]_CarbonMonoxidePartsPerMillionTop field
	And I enter "5" into the NewAtmosphericTests[0]_CarbonMonoxidePartsPerMillionMiddle field
	And I enter "6" into the NewAtmosphericTests[0]_CarbonMonoxidePartsPerMillionBottom field
	And I enter "7" into the NewAtmosphericTests[0]_HydrogenSulfidePartsPerMillionTop field
	And I enter "8" into the NewAtmosphericTests[0]_HydrogenSulfidePartsPerMillionMiddle field
	And I enter "9" into the NewAtmosphericTests[0]_HydrogenSulfidePartsPerMillionBottom field
	And I check the NewAtmosphericTests[0]_AcknowledgedValuesAreOutOfRange field
	And I press "Save"
	And I click the "Section 2" tab
	# The table id is just the array index. So first item is 0, etc.
	Then I should see the following values in the atmospheric-test-0 table
	| Top     | Middle  | Bottom  |
	| 11.00 % | 22.00 % | 23.00 % |
	| 1.00 %  | 2.00 %  | 3.00 %  |
	| 4       | 5       | 6       |
	| 7       | 8       | 9       |

Scenario: Sections 3, 4, and 5 are only accessible after a user has saved a pre-entry atmospheric test with valid concentrations.
	Given a confined space form "section2isNotComplete" exists with production work order: "one"
	And a confined space form "section2IsComplete" exists with production work order: "one"
	And a confined space form atmospheric test "one" exists with confined space form: "section2IsComplete"
	And I am logged in as "user"
	When I visit the Edit page for confined space form "section2isNotComplete"
	Then I should not see the "Section 3" tab
	And I should not see the "Section 4" tab
	And I should not see the "Section 5" tab
	And I should not see the "Role Assignment" tab
	When I visit the Edit page for confined space form "section2IsComplete"
	Then I should see the "Section 3" tab
	And I should see the "Section 4" tab
	And I should see the "Section 5" tab
	And I should see the "Role Assignment" tab

Scenario: Form should not be able to actually submit if the user doesn't check the acknowledge button on an atmospheric test
	Given a confined space form "one" exists with production work order: "one"
	And I am logged in as "user"
	When I visit the Edit page for confined space form "one"
	And I click the "Section 2" tab
	And I press "Add New Test"
	And I enter "8/24/2020 4:24 PM" into the NewAtmosphericTests[0].TestedAt field
	And I enter "11" into the NewAtmosphericTests[0]_OxygenPercentageTop field
	And I enter "22" into the NewAtmosphericTests[0]_OxygenPercentageMiddle field
	And I enter "23" into the NewAtmosphericTests[0]_OxygenPercentageBottom field
	And I enter "1" into the NewAtmosphericTests[0]_LowerExplosiveLimitPercentageTop field
	And I enter "2" into the NewAtmosphericTests[0]_LowerExplosiveLimitPercentageMiddle field
	And I enter "3" into the NewAtmosphericTests[0]_LowerExplosiveLimitPercentageBottom field
	And I enter "4" into the NewAtmosphericTests[0]_CarbonMonoxidePartsPerMillionTop field
	And I enter "5" into the NewAtmosphericTests[0]_CarbonMonoxidePartsPerMillionMiddle field
	And I enter "6" into the NewAtmosphericTests[0]_CarbonMonoxidePartsPerMillionBottom field
	And I enter "7" into the NewAtmosphericTests[0]_HydrogenSulfidePartsPerMillionTop field
	And I enter "8" into the NewAtmosphericTests[0]_HydrogenSulfidePartsPerMillionMiddle field
	And I enter "9" into the NewAtmosphericTests[0]_HydrogenSulfidePartsPerMillionBottom field
	And I press Save
	Then I should see a validation message for NewAtmosphericTests[0].AcknowledgedValuesAreOutOfRange with "Please confirm the readings entered are accurate."

Scenario: User must confirm bump test before being able to add tests
	Given a confined space form "one" exists with production work order: "one", bump test confirmed by: null
	And I am logged in as "user"
	When I visit the Edit page for confined space form "one"
	And I click the "Section 2" tab
	Then the IsBumpTestConfirmed field should be unchecked 
	And I should not see the add-atmospheric-test element
	When I check the IsBumpTestConfirmed field
	Then I should see the add-atmospheric-test element
	When I uncheck the IsBumpTestConfirmed field
	Then I should not see the add-atmospheric-test element

Scenario: Once the bump test confirmation is signed then no users should be able to sign it again
	Given a confined space form "one" exists with production work order: "one", bump test confirmed by: null
	And a gas monitor "one" exists with operating center: "nj7"
	And I am logged in as "user"
	And I am at the Edit page for confined space form "one"
	When I click the "Section 2" tab
	And I select " - NJ7 - Shrewsbury - Equipment - In Service" from the GasMonitor dropdown
	And I check the IsBumpTestConfirmed field
	And I press Save
	Then I should be at the Edit page for confined space form "one"
	When I visit the Show page for confined space form "one"
	And I click the "Section 2" tab
	Then I should see a display for BumpTestConfirmedBy with employee "one"
	When I visit the Edit page for confined space form "one"
	And I click the "Section 2" tab
	Then I should not see the IsBumpTestConfirmed field
	And I should see a display for Original_BumpTestConfirmedBy with employee "one"
	# and test that another user can't overwrite this
	Given I am logged in as "other-user"
	And I am at the Edit page for confined space form "one"
	When I press Save
	Then I should be at the Edit page for confined space form "one"
	When I click the "Section 2" tab
	Then I should see a display for Original_BumpTestConfirmedBy with employee "one"

Scenario: Section 2 on the confined space form should display the correct things
	Given a equipment model "one" exists with description: "All the things"
	And a equipment "one" exists with equipment model: "one", equipment manufacturer: "generator"	
	And a gas monitor "one" exists with equipment: "one"	
	And a confined space form "one" exists with production work order: "one", bump test confirmed by: null, gas monitor: "one"
	And I am logged in as "user"
	And I am at the Show page for confined space form "one"
	When I click the "Section 2" tab
	Then I should see a display for GasMonitor_EquipmentDescription with equipment "one"'s Description	
	And I should see a display for GasMonitor_Manufacturer with equipment manufacturer "generator"
	And I should see a display for GasMonitor_EquipmentModel with equipment "one"'s EquipmentModel
	And I should see a display for GasMonitor_SerialNumber with equipment "one"'s SerialNumber 

Scenario: User should see a notification and not be able to edit once form is complete when section five was not needed
	Given a confined space form "one" exists with production work order: "one"
	And a confined space form atmospheric test "one" exists with confined space form: "one"
	And I am logged in as "user"
	And I am at the Edit page for confined space form "one"
	When I click the "Section 4" tab
	And I select "Yes" from the CanBeControlledByVentilationAlone dropdown
	And I check the IsHazardSectionSigned field
	And I press "Save" 
	And I visit the Show page for confined space form "one"
	Then I should see "No Permit Required – Entry may commence"
	And I should see "The form has been completed and sections 1,2,3,4,5 can no longer be edited. If applicable, Employee Assignments can still be added"

Scenario: User should still be able to add a new test post completion
	Given a equipment model "one" exists with description: "All the things"
	And a equipment "one" exists with equipment model: "one", operating center: "nj7"
	And a gas monitor "one" exists with operating center: "nj7", equipment: "one"
	And a confined space form "one" exists with production work order: "one", gas monitor: "one"
	And a confined space form atmospheric test "one" exists with confined space form: "one"
	And I am logged in as "user"
	And I am at the Edit page for confined space form "one"
	When I click the "Section 2" tab
	Then I should see a display for Original_GasMonitor_Display with gas monitor "one"'s Display
	When I press "Add New Test"
	And I press "Save"
	Then I should see a validation message for NewAtmosphericTests[0].TestedAt with "The TestedAt field is required."
	When I enter "8/24/2020 4:24 PM" into the NewAtmosphericTests[0].TestedAt field
	And I select confined space form reading capture time "pre" from the NewAtmosphericTests[0].ConfinedSpaceFormReadingCaptureTime dropdown
	And I enter "21" into the NewAtmosphericTests[0]_OxygenPercentageTop field
	And I enter "22" into the NewAtmosphericTests[0]_OxygenPercentageMiddle field
	And I enter "23" into the NewAtmosphericTests[0]_OxygenPercentageBottom field
	And I enter "1" into the NewAtmosphericTests[0]_LowerExplosiveLimitPercentageTop field
	And I enter "2" into the NewAtmosphericTests[0]_LowerExplosiveLimitPercentageMiddle field
	And I enter "3" into the NewAtmosphericTests[0]_LowerExplosiveLimitPercentageBottom field
	And I enter "4" into the NewAtmosphericTests[0]_CarbonMonoxidePartsPerMillionTop field
	And I enter "5" into the NewAtmosphericTests[0]_CarbonMonoxidePartsPerMillionMiddle field
	And I enter "6" into the NewAtmosphericTests[0]_CarbonMonoxidePartsPerMillionBottom field
	And I enter "7" into the NewAtmosphericTests[0]_HydrogenSulfidePartsPerMillionTop field
	And I enter "8" into the NewAtmosphericTests[0]_HydrogenSulfidePartsPerMillionMiddle field
	And I enter "9" into the NewAtmosphericTests[0]_HydrogenSulfidePartsPerMillionBottom field
	And I press "Save"
	And I click the "Section 2" tab
	# The table id is just the array index. So first item is 0, etc.
	Then I should see the following values in the atmospheric-test-1 table
	| Top     | Middle  | Bottom  |
	| 21.00 % | 22.00 % | 23.00 % |
	| 1.00 %  | 2.00 %  | 3.00 %  |
	| 4       | 5       | 6       |
	| 7       | 8       | 9       |

Scenario: User should see a notification and still be able to add entrants on edit
	Given a confined space form "one" exists with production work order: "one"
	And a confined space form atmospheric test "one" exists with confined space form: "one"
	And an confined space form entrant type "attendant" exists with description: "Attendant"
	And an confined space form entrant type "entrant" exists with description: "Entrant"
	And an confined space form entrant type "supervisor" exists with description: "Entry Supervisor"
	And I am logged in as "user"
	And I am at the Edit page for confined space form "one"
	When I click the "Section 4" tab
	And I select "No" from the CanBeControlledByVentilationAlone dropdown
	And I check the IsHazardSectionSigned field
	And I click the "Section 5" tab
	And I enter "10/24/2020" into the PermitBeginsAt field
	And I enter "10/25/2020" into the PermitEndsAt field
	And I select "Yes" from the HasRetrievalSystem dropdown
	And I select "Yes" from the HasContractRescueService dropdown
	And I enter "Emergency response agency" into the EmergencyResponseAgency field
	And I enter "The responsible guy" into the EmergencyResponseContact field 
	And I check the IsBeginEntrySectionSigned field
	And I click the "Role Assignment" tab
	And I press "Add An Assignment"
	And I check the NewEntrants[0].IsEmployee field
	And I select confined space form entrant type "attendant" from the NewEntrants[0].EntrantType dropdown
	And I enter "pres" and select "S. Preston, Bill - 1000001 - NJ7" from the NewEntrants[0]_Employee autocomplete field
	And I press "Save" 
	And I visit the Show page for confined space form "one"
	Then I should see "Permit Authorized – Entry may commence"
	And I should see "The form has been completed and sections 1,2,3,4,5 can no longer be edited. If applicable, Employee Assignments can still be added"
	When I visit the /HealthAndSafety/ConfinedSpaceForm/PostCompletionEdit page for confined space form "one"
	Then the "Role Assignment" tab should be enabled
	And the "Section 1" tab should be enabled
	And the "Section 2" tab should be enabled
	And the "Section 3" tab should be enabled
	And the "Section 4" tab should be enabled
	And the "Section 5" tab should be enabled
	When I click the "Role Assignment" tab
	And I press "Add An Assignment"
	And I check the NewEntrants[1].IsEmployee field
	And I select confined space form entrant type "supervisor" from the NewEntrants[1].EntrantType dropdown
	And I enter "pres" and select "S. Preston, Bill - 1000001 - NJ7" from the NewEntrants[1]_Employee autocomplete field
	And I press "Save"
	And I visit the Show page for confined space form "one"
	And I click the "Role Assignment" tab
	Then I should see the following values in the entrantsTable table
         | Entry Assignment/Role | Employee        | Contracting Company | Contractor Name |
         | Attendant             | Bill S. Preston |                     |                 |
         | Entry Supervisor      | Bill S. Preston |                     |                 |

Scenario: User can create a confined space form from a short cycle work order
	Given a planning plant "one" exists with operating center: "nj7"
	And a gas monitor "one" exists with operating center: "nj7"
	And I am logged in as "user"
	When I visit the HealthAndSafety/ConfinedSpaceForm/New?shortCycleWorkOrderNumber=817 page
	And  I enter "4/24/1984 4:04 AM" into the GeneralDateTime field
	And I select state "nj" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I enter "some location of sorts" into the LocationAndDescriptionOfConfinedSpace field
	And I enter "some purpose of entry" into the PurposeOfEntry field
	And I press Save
	Then the currently shown confined space form will now be referred to as "one"
	And I should be at the Edit page for confined space form "one"
	When I visit the Show page for confined space form "one"
	Then I should see a display for ShortCycleWorkOrderNumber with "817"
	And I should see a display for OperatingCenter with operating center "nj7"
	And I should see a display for State with state "nj"
	And I should see a display for GeneralDateTime with "4/24/1984 4:04:00 AM"
	And I should see a display for LocationAndDescriptionOfConfinedSpace with "some location of sorts"
	And I should see a display for PurposeOfEntry with "some purpose of entry"
	# Don't test section 2 as part of this. It's redundant and is tested with the "user can create ... from a production work order" test below.
	
Scenario: User can create a confined space form from a work order
	Given a work order "one" exists with operating center: "nj7"
	And I am logged in as "user"
	When I visit the HealthAndSafety/ConfinedSpaceForm/New?workOrderId=1 page
	Then I should see a display for WorkOrderDisplay_OperatingCenter with operating center "nj7"
	And I should see a display for WorkOrderDisplay_OperatingCenter_State with state "nj"
	And I should see a link to the Show page for work order: "one"
	When I enter "4/24/1984 4:04 AM" into the GeneralDateTime field
	And I enter "some location of sorts" into the LocationAndDescriptionOfConfinedSpace field
	And I enter "some purpose of entry" into the PurposeOfEntry field
	And I press Save
	Then the currently shown confined space form will now be referred to as "one"
	And I should be at the Edit page for confined space form "one"
	When I visit the Show page for confined space form "one"
	Then I should see a link to the Show page for work order "one"
	And I should see a display for OperatingCenter with operating center "nj7"
	And I should see a display for State with state "nj"
	And I should see a display for GeneralDateTime with "4/24/1984 4:04:00 AM"
	And I should see a display for LocationAndDescriptionOfConfinedSpace with "some location of sorts"
	And I should see a display for PurposeOfEntry with "some purpose of entry"
	# Don't test section 2 as part of this. It's redundant and is tested with the "user can create ... from a production work order" test below.

# Section 2 stuff is irrelevent to the pwo/scwo/wo part of creation. So we're gonna
# cover that as part of the "create for pwo" test and not repeat it in the other similar tests.
Scenario: user can create a confined space form from a production work order with section 2 values at the same time
	Given a production work order: "one" exists
	And an equipment status "one" exists with description: "Retired"
	And an equipment status "two" exists with description: "Cancelled"
	And a equipment "one" exists with operating center: "nj7"
	And a equipment "two" exists with operating center: "nj7", description: "Generator", equipment status: "one"
	And a equipment "three" exists with operating center: "nj7", description: "Generator", equipment status: "two"
	And a gas monitor "one" exists with operating center: "nj7", equipment: "one"
	And a gas monitor "two" exists with operating center: "nj7", equipment: "two"
	And a gas monitor "three" exists with operating center: "nj7", equipment: "three"
	And I am logged in as "user"
	When I visit the Show page for production work order "one"
	And I click the "Confined Space Forms" tab
	And I follow "Create Confined Space Form"
	Then I should see a link to the Show page for production work order: "one"
	When I enter "4/24/1984 4:04 AM" into the GeneralDateTime field
	And I enter "some location of sorts" into the LocationAndDescriptionOfConfinedSpace field
	And I enter "some purpose of entry" into the PurposeOfEntry field
	And I click the "Section 2" tab		
	When I select gas monitor "one" from the GasMonitor dropdown	
	Then I should not see "- NJ7 - Shrewsbury - Generator - Retired" in the GasMonitor dropdown	
	Then I should not see "- NJ7 - Shrewsbury - Generator - Cancelled" in the GasMonitor dropdown	
	When I check the IsBumpTestConfirmed field
	And I press "Add New Test"
	And I enter "8/24/2020 4:24 PM" into the NewAtmosphericTests[0].TestedAt field
	And I select confined space form reading capture time "pre" from the NewAtmosphericTests[0].ConfinedSpaceFormReadingCaptureTime dropdown
	And I enter "21" into the NewAtmosphericTests[0]_OxygenPercentageTop field
	And I enter "22" into the NewAtmosphericTests[0]_OxygenPercentageMiddle field
	And I enter "23" into the NewAtmosphericTests[0]_OxygenPercentageBottom field
	And I enter "1" into the NewAtmosphericTests[0]_LowerExplosiveLimitPercentageTop field
	And I enter "2" into the NewAtmosphericTests[0]_LowerExplosiveLimitPercentageMiddle field
	And I enter "3" into the NewAtmosphericTests[0]_LowerExplosiveLimitPercentageBottom field
	And I enter "4" into the NewAtmosphericTests[0]_CarbonMonoxidePartsPerMillionTop field
	And I enter "5" into the NewAtmosphericTests[0]_CarbonMonoxidePartsPerMillionMiddle field
	And I enter "6" into the NewAtmosphericTests[0]_CarbonMonoxidePartsPerMillionBottom field
	And I enter "7" into the NewAtmosphericTests[0]_HydrogenSulfidePartsPerMillionTop field
	And I enter "8" into the NewAtmosphericTests[0]_HydrogenSulfidePartsPerMillionMiddle field
	And I enter "9" into the NewAtmosphericTests[0]_HydrogenSulfidePartsPerMillionBottom field
	And I press "Save"
	And I wait for the page to reload
	And I click the "Section 2" tab
	# The table id is just the array index. So first item is 0, etc.
	Then I should see the following values in the atmospheric-test-0 table
	| Top     | Middle  | Bottom  |
	| 21.00 % | 22.00 % | 23.00 % |
	| 1.00 %  | 2.00 %  | 3.00 %  |
	| 4       | 5       | 6       |
	| 7       | 8       | 9       |

Scenario: User must confirm dialog when checking the reclassifiction checkbox in section 3
	Given a confined space form "one" exists with production work order: "one", can be controlled by ventilation alone: "false"
	And a confined space form atmospheric test "one" exists with confined space form: "one"
	And I am logged in as "user"
	And I am at the Edit page for confined space form "one"
	When I click the "Section 3" tab
	# sanity
	Then the IsReclassificationSectionSigned field should be unchecked
	When I click ok in the dialog after clicking "IsReclassificationSectionSigned"
	Then the IsReclassificationSectionSigned field should be checked
	When I uncheck the IsReclassificationSectionSigned field
	And I click cancel in the dialog after clicking "IsReclassificationSectionSigned"
	Then the IsReclassificationSectionSigned field should be unchecked

Scenario: User can download pdf
	Given a confined space form "one" exists with production work order: "one", can be controlled by ventilation alone: "false"
	And I am logged in as "user"
	When I visit the show page for confined space form: "one"
	Then I should be able to download confined space form "one"'s pdf