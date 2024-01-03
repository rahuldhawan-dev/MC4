Feature: LockoutFormPage
	Fern baby Fern, discount fern store
	Shut your mouse
	Wild Wild Pest

Background: users exist and such
	Given a lockout form question category "return to service" exists with description: "return to service"
	And a lockout form question category "out of service" exists with description: "out of service"
	And a lockout form question category "management" exists with description: "management"
	And a lockout form question category "lockout conditions" exists with description: "lockout conditions"
	And a lockout form question "two" exists with question: "How do you play that?", category: "out of service", is active: true, display order: 5
	And a lockout form question "three" exists with question: "You have to ask a question.", category: "return to service", is active: true, display order: 10
	And a lockout form question "four" exists with question: "Statement. One. Love.", category: "management", is active: true, display order: 15
	And a lockout form question "five" exists with question: "Cheating.", category: "management", is active: true, display order: 20
	And a lockout form question "six" exists with question: "How?", category: "management", is active: true, display order: 25
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsburghy"
	And an operating center "nj4" exists with opcode: "NJ4", name: "Lakewood", is active: false
	And an employee status "active" exists with description: "Active"
	And an employee "bill" exists with first name: "Bill", last name: "Preston", employee id: "11111117", operating center: "nj7", status: "active"
	And a role "roleReadNj7" exists with action: "Read", module: "OperationsLockoutForms", operating center: "nj7"
	And a role "roleEditNj7" exists with action: "Edit", module: "OperationsLockoutForms", operating center: "nj7"
	And a role "roleAddNj7" exists with action: "Add", module: "OperationsLockoutForms", operating center: "nj7"
	And a role "roleDeleteNj7" exists with action: "Delete", module: "OperationsLockoutForms", operating center: "nj7"
	And a role "roleReadNj4" exists with action: "Read", module: "OperationsLockoutForms", operating center: "nj4"
	And a role "roleEditNj4" exists with action: "Edit", module: "OperationsLockoutForms", operating center: "nj4"
	And a role "roleDeleteNj4" exists with action: "Delete", module: "OperationsLockoutForms", operating center: "nj4"
	And a role "roleAddNj4" exists with action: "Add", module: "OperationsLockoutForms", operating center: "nj4"
	And a user "user" exists with username: "user" 
	And a user "bill" exists with username: "bill", employee: "bill", roles: roleEditNj7
	And a user "read-only-nj7" exists with username: "read-only-nj7", roles: roleReadNj7
	And a user "user-admin-nj7" exists with username: "user-admin-nj7", roles: roleReadNj7;roleEditNj7;roleAddNj7;roleDeleteNj7
	And a user "read-only-nj4" exists with username: "read-only-nj4", roles: roleReadNj4;
    And a user "user-admin-nj4" exists with username: "user-admin-nj4", full name: "user admin nj4", roles: roleReadNj4;roleEditNj4;roleAddNj4;roleDeleteNj4
	And a user "user-admin-both" exists with username: "user-admin-both", roles: roleReadNj4;roleEditNj4;roleAddNj4;roleDeleteNj4;roleReadNj7;roleEditNj7;roleAddNj7;roleDeleteNj7
	And a role "roleReadnj4" exists with action: "Read", module: "ProductionFacilities", user: "user-admin-nj7", operating center: "nj4"
	And a role "roleReadnj7" exists with action: "Read", module: "ProductionFacilities", user: "user-admin-nj7", operating center: "nj7"
	And a role "roleReadnj4Eq" exists with action: "Read", module: "ProductionEquipment", user: "user-admin-nj7", operating center: "nj4"
	And a role "roleReadnj7Eq" exists with action: "Read", module: "ProductionEquipment", user: "user-admin-nj7", operating center: "nj7"
	And equipment types exist
	And a lockout device color "blue" exists with description: "blue"
	And a lockout device "one" exists with person: "user-admin-nj7", lockout device color: "blue", description: "one", serial number: "123" 
	And a way to remove locks "one" exists with description: "Bolt Cutters"
	And a contractor "one" exists
	
Scenario: user without role cannot access the search/index/new/edit/show pages
	Given a lockout form "one" exists with lockout device: "one"
	And I am logged in as "user"
	When I visit the HealthAndSafety/LockoutForm/Search page
	Then I should see "You do not have the roles necessary to access this resource."
	And I should see "OperationsLockoutForms"
	When I visit the HealthAndSafety/LockoutForm/ page
	Then I should see "You do not have the roles necessary to access this resource."
	And I should see "OperationsLockoutForms"
	When I visit the HealthAndSafety/LockoutForm/New page
	Then I should see "You do not have the roles necessary to access this resource."
	And I should see "OperationsLockoutForms"
	When I visit the Show page for lockout form: "one"
	Then I should see "You do not have the roles necessary to access this resource."
	And I should see "OperationsLockoutForms"
	When I visit the Edit page for lockout form: "one"
	Then I should see "You do not have the roles necessary to access this resource."
	And I should see "OperationsLockoutForms"

Scenario: user with rights can view a form and not view another operating centers form
	Given a lockout form "nj7" exists with operating center: "nj7", lockout device: "one"
	And a lockout form "nj4" exists with operating center: "nj4", lockout device: "one"
	And I am logged in as "read-only-nj7"
	When I visit the Show page for lockout form: "nj7"
	Then I should see a display for OperatingCenter with operating center "nj7"
	When I try to access the Show page for lockout form: "nj4" expecting an error
	Then I should see a 404 error message

Scenario: create validation works properly
	Given a coordinate "one" exists
	And a lockout form question "one" exists with question: "Would you like to play a game of questions?", category: "lockout conditions", is active: true, display order: 1
	And a facility "facility" exists with operating center: "nj7", facility id: "NJSB-1", facility name: "The Facility", coordinate: "one"
	And an equipment "one" exists with identifier: "NJSB-1-EQID-1", facility: "facility", equipment type: "generator", description: "equipment one"
	And an equipment "two" exists with identifier: "NJSB-1-EQID-1", facility: "facility", equipment type: "generator", description: "equipment two"
	And a lockout reason "one" exists with description: "Repair"
	And a lockout device location "one" exists with description: "Valve"
	And a data type "equipment" exists with table name: "Equipment", name: "Equipment"
	And a document type "sop" exists with data type: "equipment", name: "sop", id: 441
	And a document "sop" exists with document type: "sop", file name: "some file name"
	And an equipment document link "doculinky" exists with document: "sop", equipment: "one", data type: "equipment", document type: "sop"
	And I am logged in as "user-admin-nj7"
	When I visit the /HealthAndSafety/LockoutForm/New page
	Then I should not see operating center "nj4" in the OperatingCenter dropdown
	When I select "-- Select --" from the OperatingCenter dropdown
	And I press Save
	Then I should see the validation message "The Operating Center field is required."
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I press Save
	Then I should see the validation message "The Facility field is required."
	And I should see the validation message "The Lockout Reason field is required."
	And I should see the validation message "The Reason For Lockout field is required."
	And I should see the validation message "The IsolationPoint field is required."
	And I should see the validation message "The Location of Lockout Notes field is required."
	And I should see the validation message "The Lockout Device field is required."
	And I should see a validation message for CreateLockoutFormAnswers[0].Answer with "The Answer field is required."
	And I should see a validation message for CreateLockoutFormAnswers[1].Answer with "The Answer field is required."
	And I should see the validation message "The Out Of Service Authorized Employee field is required."
	When I select "No" from the CreateLockoutFormAnswers_0__Answer dropdown
	And I select "No" from the CreateLockoutFormAnswers_1__Answer dropdown
	And I press Save
	Then I should see a validation message for CreateLockoutFormAnswers[0].Comments with "The Comments field is required."
	And I should see a validation message for CreateLockoutFormAnswers[1].Comments with "The Comments field is required."
	And I should not see a validation message for CreateLockoutFormAnswers[0].Answer with "The Answer field is required."
	And I should not see a validation message for CreateLockoutFormAnswers[1].Answer with "The Answer field is required."
	When I enter "foo" into the CreateLockoutFormAnswers[0].Comments field
	And I enter "bar" into the CreateLockoutFormAnswers[1].Comments field
	And I press Save
	Then I should not see a validation message for CreateLockoutFormAnswers[0].Comments with "The Comments field is required."
	And I should not see a validation message for CreateLockoutFormAnswers[1].Comments with "The Comments field is required."
	
Scenario: user can create a lockout form
	Given a coordinate "one" exists
	And a facility "facility" exists with operating center: "nj7", facility id: "NJSB-1", facility name: "The Facility", coordinate: "one"
	And an equipment "one" exists with identifier: "NJSB-1-EQID-1", facility: "facility", equipment type: "generator"
	And a lockout reason "one" exists with description: "Repair"
	And a lockout device location "one" exists with description: "Valve"
	And I am logged in as "user-admin-nj7"
	When I visit the /HealthAndSafety/LockoutForm/New page
	And I select facility "facility"'s Description from the Facility dropdown
	And I select equipment type "generator"'s Description from the EquipmentType dropdown
	And I select equipment "one"'s Description from the Equipment dropdown
	And I enter "a bunch of notes" into the ReasonForLockout field
	And I select employee "bill"'s Description from the OutOfServiceAuthorizedEmployee dropdown
	And I select "Repair" from the LockoutReason dropdown
	And I select lockout device location "one" from the IsolationPoint dropdown
	And I select lockout device "one" from the LockoutDevice dropdown
	And I enter "abcdef" into the LocationOfLockoutNotes field
	And I select "No" from the CreateLockoutFormAnswers_0__Answer dropdown
	And I enter "custom comments xyz" into the CreateLockoutFormAnswers[0].Comments field
	And I press Save
	Then the currently shown lockout form shall henceforth be known throughout the land as "one"
	And I should be at the Show page for lockout form: "one"
	And I should see a display for lockout form question: "two" with "No"
	And I should see "custom comments xyz"
	And I should see a display for LocationOfLockoutNotes with "abcdef"
	And I should see a display for LockoutDevice with lockout device "one"
	And I should see a display for IsolationPoint with lockout device location "one"
	And I should see a display for LockoutReason with "Repair"
	And I should see a display for SameAsInstaller with "n/a"
	And I should see a display for OutOfServiceAuthorizedEmployee with employee "bill"
	And I should see a display for LocationOfLockoutNotes with "abcdef"
	And I should see a display for ReasonForLockout with "a bunch of notes"
	And I should see a link to the Show page for equipment "one"
	And I should see a display for EquipmentType with equipment type "generator"'s Display
	And I should see a link to the Show page for facility "facility"
	When I follow "Edit"
	Then I should not see "YSOD"

Scenario: user can download pdf
	Given a lockout form "nj7" exists with operating center: "nj7"
	And I am logged in as "read-only-nj7"
	When I visit the Show page for lockout form: "nj7"
	Then I should be able to download lockout form "nj7"'s pdf

Scenario: user can create and update a lockout form with management questions
	Given a coordinate "one" exists
	And a facility "facility" exists with operating center: "nj7", facility id: "NJSB-1", facility name: "The Facility", coordinate: "one"
	And an equipment "one" exists with identifier: "NJSB-1-EQID-1", facility: "facility", equipment type: "generator"
	And a lockout reason "one" exists with description: "Repair"
	And a lockout device location "one" exists with description: "Valve"
	And I am logged in as "user-admin-nj7"
	When I visit the /HealthAndSafety/LockoutForm/New page
	And I select facility "facility"'s Description from the Facility dropdown
	And I select equipment type "generator"'s Description from the EquipmentType dropdown
	And I select equipment "one"'s Description from the Equipment dropdown
	And I enter "a bunch of notes" into the ReasonForLockout field
	And I select employee "bill"'s Description from the OutOfServiceAuthorizedEmployee dropdown
	And I select "Repair" from the LockoutReason dropdown
	And I select lockout device location "one" from the IsolationPoint dropdown
	And I select lockout device "one" from the LockoutDevice dropdown
	And I enter "abcdef" into the LocationOfLockoutNotes field
	And I select "No" from the CreateLockoutFormAnswers_0__Answer dropdown
	And I enter "custom comments xyz" into the CreateLockoutFormAnswers[0].Comments field	
	And I press Save
	Then the currently shown lockout form shall henceforth be known throughout the land as "one"
	And I should be at the Show page for lockout form: "one"
	And I should see a display for lockout form question: "two" with "No"
	And I should see "custom comments xyz"
	And I should see a display for LocationOfLockoutNotes with "abcdef"
	And I should see a display for LockoutDevice with lockout device "one"
	And I should see a display for IsolationPoint with lockout device location "one"
	And I should see a display for LockoutReason with "Repair"
	And I should see a display for SameAsInstaller with "n/a"
	And I should see a display for OutOfServiceAuthorizedEmployee with employee "bill"
	And I should see a display for LocationOfLockoutNotes with "abcdef"
	And I should see a display for ReasonForLockout with "a bunch of notes"
	And I should see a link to the Show page for equipment "one"
	And I should see a display for EquipmentType with equipment type "generator"'s Display
	And I should see a link to the Show page for facility "facility"
	When I follow "Edit"
	And I select "No" from the SameAsInstaller dropdown 
	And I select "Yes" from the EditLockoutFormAnswers_2__Answer dropdown
	And I select "Yes" from the EditLockoutFormAnswers_3__Answer dropdown
	And I select "No" from the EditLockoutFormAnswers_4__Answer dropdown
	And I select employee "bill"'s Description from the SupervisorInvolved dropdown
	And I enter today's date into the DateOfContact field
	And I enter "phone" into the MethodOfContact field
	And I enter "nothing, absolutely nothing!" into the OutcomeOfContact field
	And I enter "custom comments 6" into the EditLockoutFormAnswers_4__Comments field
	And I select employee "bill"'s Description from the AuthorizedManagementPerson dropdown
	And I select way to remove locks "one" from the LockRemovalMethod dropdown
	And I press Save
	Then I should see a display for lockout form question: "four" with "Yes"
	And I should see a display for lockout form question: "five" with "Yes"
	And I should see a display for lockout form question: "six" with "No"
	And I should see a display for SupervisorInvolved with employee "bill"
	And I should see a display for DateOfContact with today's date
	And I should see a display for MethodOfContact with "phone"
	And I should see "custom comments 6"
	And I should see a display for OutcomeOfContact with "nothing, absolutely nothing!"
	And I should see a display for AuthorizedManagementPerson with employee "bill"
	And I should see a display for LockRemovalMethod with way to remove locks "one"

Scenario: user can update a lockout form with management questions
	Given a coordinate "one" exists
	And a facility "x" exists with operating center: "nj7", facility id: "NJSB-11", facility name: "The Facility 2", coordinate: "one"
	And an equipment category "one" exists with description: "Cat"
	And an equipment subcategory "one" exists with description: "SubCat
	And an equipment purpose "gen" exists with equipment category: "one", equipment subcategory: "one", description: "GEN", abbreviation: "abba", equipment type: "generator"
	And an equipment "one" exists with identifier: "NJSB-1-EQID-1", facility: "x", equipment type: "generator", EquipmentPurpose: "gen"
	And a production work order "one" exists with equipment: "one", operating center: "nj7", equipment type: "generator"
	And a production work order equipment "one" exists with production work order: "one", equipment: "one"
	And a lockout reason "one" exists with description: "Repair"
	And a lockout device location "one" exists with description: "Valve"
	And a lockout form "nj7" exists with operating center: "nj7", production work order: "one", facility: "x", equipment type: "generator", equipment: "one", lockout device: "one", out of service authorized employee: "bill"
	And a lockout form answer "two" exists with lockout form: "nj7", lockout form question: "two", answer: "true"
	And a lockout form answer "three" exists with lockout form: "nj7", lockout form question: "three", answer: "true"
	And a lockout form answer "four" exists with lockout form: "nj7", lockout form question: "four", answer: "false"
	And a lockout form answer "five" exists with lockout form: "nj7", lockout form question: "five", answer: "false"
	And a lockout form answer "size" exists with lockout form: "nj7", lockout form question: "six", answer: "false"
	And I am logged in as "user-admin-nj7"
	When I visit the Show page for lockout form: "nj7"
	And I follow "Edit"
	When I select lockout device "one" from the LockoutDevice dropdown
	And I select employee "bill"'s Description from the ReturnToServiceAuthorizedEmployee dropdown
	And I select "No" from the EditLockoutFormAnswers_0__Answer dropdown
	And I enter "custom comments 0" into the EditLockoutFormAnswers[0].Comments field
	And I select "No" from the EditLockoutFormAnswers_1__Answer dropdown
	And I enter "custom comments 1" into the EditLockoutFormAnswers[1].Comments field
	And I select "No" from the EditLockoutFormAnswers_2__Answer dropdown
	And I enter "custom comments 2" into the EditLockoutFormAnswers[2].Comments field
	When I select "No" from the SameAsInstaller dropdown
	And I select "Yes" from the EditLockoutFormAnswers_3__Answer dropdown
	And I select "Yes" from the EditLockoutFormAnswers_4__Answer dropdown
	And I select employee "bill"'s Description from the SupervisorInvolved dropdown
	And I enter today's date into the DateOfContact field
	And I enter "phone" into the MethodOfContact field
	And I enter "nothing, absolutely nothing!" into the OutcomeOfContact field
	And I select employee "bill"'s Description from the AuthorizedManagementPerson dropdown
	And I select way to remove locks "one" from the LockRemovalMethod dropdown
	And I check the EmployeeAcknowledgedTraining field
	And I press Save
	And I wait for the page to reload
	Then I should see a display for lockout form question: "two" with "No"
	And I should see "custom comments 1"
	And I should see a display for lockout form question: "three" with "No"
	And I should see "custom comments 2"
	And I should see a display for lockout form question: "four" with "No"
	And I should see a display for lockout form question: "five" with "Yes"
	And I should see a display for lockout form question: "six" with "Yes"
	And I should see a display for SupervisorInvolved with employee "bill"
	And I should see a display for DateOfContact with today's date
	And I should see a display for MethodOfContact with "phone"
	And I should see a display for OutcomeOfContact with "nothing, absolutely nothing!"
	And I should see a display for AuthorizedManagementPerson with employee "bill"
	And I should see a display for LockRemovalMethod with way to remove locks "one"

Scenario: user can delete a lockout form
	Given a lockout form "nj7" exists with operating center: "nj7", lockout device: "one"
	And I am logged in as "user-admin-nj7"
	When I visit the Show page for lockout form: "nj7"
	And I click ok in the dialog after pressing "Delete"
	Then I should be at the HealthAndSafety/LockoutForm/Search page
	When I try to access the Show page for lockout form: "nj7" expecting an error
	Then I should see a 404 error message

Scenario: user can not simply add a lockout form
	Given a lockout form "nj7" exists with operating center: "nj7", lockout device: "one"
	And I am logged in as "user-admin-nj7"
	When I visit the Show page for lockout form: "nj7"
	Then I should not see the "add" button in the action bar
	 
Scenario: admin can get to equipment page
	Given an admin user "admin" exists with username: "admin"
	And an equipment "one" exists with identifier: "NJSB-1-EQID-1"
	And a data type "equipment" exists with table name: "Equipment", name: "Equipment"
	And a document type "sop" exists with data type: "equipment", name: "sop", id: 441
	And a document "sop" exists with document type: "sop", file name: "some file name"
	And an equipment document link "doculinky" exists with document: "sop", equipment: "one", data type: "equipment", document type: "sop"
	And I am logged in as "admin"
	When I visit the Show page for equipment: "one"
	Then I should not see "Documents (0)"

Scenario: user is prompted to review SOP for equipment if it has an SOP document
	Given a facility "facility" exists with operating center: "nj7", facility id: "NJSB-1", facility name: "The Facility"
	And an equipment "one" exists with identifier: "NJSB-1-EQID-1", facility: "facility", equipment type: "generator"
	And a data type "equipment" exists with table name: "Equipment", name: "Equipment"
	And a document type "sop" exists with data type: "equipment", name: "sop"
	And a document "sop" exists with document type: "sop", file name: "some file name"
	And an equipment document link "doculinky" exists with document: "sop", equipment: "one", data type: "equipment", document type: "sop"
	And a lockout reason "one" exists with description: "Repair"
	And a lockout device location "one" exists with description: "Valve"
	And I am logged in as "user-admin-nj7"
	When I visit the /HealthAndSafety/LockoutForm/New page
	And I select facility "facility"'s Description from the Facility dropdown
	And I select equipment type "generator"'s Description from the EquipmentType dropdown
	And I select equipment "one"'s Description from the Equipment dropdown
	Then I should see the link "Equipment Documents" ends with "#DocumentsTab"

Scenario: user can create and update a lockout form with contractor
	Given a coordinate "one" exists
	And a facility "facility" exists with operating center: "nj7", facility id: "NJSB-1", facility name: "The Facility", coordinate: "one"
	And an equipment "one" exists with identifier: "NJSB-1-EQID-1", facility: "facility", equipment type: "generator"
	And a lockout reason "one" exists with description: "Repair"
	And a lockout device location "one" exists with description: "Valve"
	And I am logged in as "user-admin-nj7"
	When I visit the /HealthAndSafety/LockoutForm/New page
	And I select facility "facility"'s Description from the Facility dropdown
	And I select equipment type "generator"'s Description from the EquipmentType dropdown
	And I select equipment "one"'s Description from the Equipment dropdown
	And I enter "a bunch of notes" into the ReasonForLockout field
	And I select employee "bill"'s Description from the OutOfServiceAuthorizedEmployee dropdown
	And I select "Repair" from the LockoutReason dropdown
	And I select lockout device location "one" from the IsolationPoint dropdown
	And I select lockout device "one" from the LockoutDevice dropdown
	And I enter "abcdef" into the LocationOfLockoutNotes field
	And I select "No" from the CreateLockoutFormAnswers_0__Answer dropdown
	And I enter "custom comments xyz" into the CreateLockoutFormAnswers[0].Comments field
	And I select "Yes" from the ContractorLockOutTagOut dropdown
	And I select contractor "one" from the Contractor dropdown
	And I enter "john" into the ContractorFirstName field
	And I enter "wick" into the ContractorLastName field
	And I enter "3" into the ContractorPhone field
	And I press Save
	Then the currently shown lockout form shall henceforth be known throughout the land as "one"
	And I should be at the Show page for lockout form: "one"
	And I should see a display for ContractorLockOutTagOut with "Yes"
	And I should see a display for Contractor with contractor "one"
	And I should see a display for ContractorFirstName with "john"
	And I should see a display for ContractorLastName with "wick"
	And I should see a display for ContractorPhone with "3"
	And I should see a display for EquipmentType with equipment type "generator"'s Display
	And I should see a link to the Show page for facility "facility"
	When I follow "Edit"
	And I press "ContractorNameToggle"
	And I enter "ABC" into the ContractorName field
	And I press Save
	Then I should see a display for Contractor with "ABC"

Scenario: user can copy a lockout form
	Given a coordinate "one" exists
	And a facility "x" exists with operating center: "nj7", facility id: "NJSB-11", facility name: "The Facility 2", coordinate: "one"
	And an equipment category "one" exists with description: "Cat"
	And an equipment subcategory "one" exists with description: "SubCat
	And an equipment purpose "gen" exists with equipment category: "one", equipment subcategory: "one", description: "GEN", abbreviation: "abba", equipment type: "generator"
	And an equipment "one" exists with identifier: "NJSB-1-EQID-1", facility: "x", equipment type: "generator", EquipmentPurpose: "gen"
	And a production work order "1" exists with equipment: "one", operating center: "nj7", equipment type: "generator"
	And a production work order equipment "one" exists with production work order: "1", equipment: "one"
	And a lockout reason "one" exists with description: "Repair"
	And a lockout device location "one" exists with description: "Valve"
	And a lockout form "nj7" exists with operating center: "nj7", production work order: "1", facility: "x", equipment type: "generator", equipment: "one", lockout device: "one", lockout reason: "one", reason for lockout: "foo", employee acknowledged training: true, contractor lock out tag out: false
	And a lockout form answer "two" exists with lockout form: "nj7", lockout form question: "two", answer: "true"
	And a lockout form answer "three" exists with lockout form: "nj7", lockout form question: "three", answer: "true"
	And a lockout form answer "four" exists with lockout form: "nj7", lockout form question: "four", answer: "false"
	And a lockout form answer "five" exists with lockout form: "nj7", lockout form question: "five", answer: "false"
	And a lockout form answer "size" exists with lockout form: "nj7", lockout form question: "six", answer: "false"
	And I am logged in as "user-admin-nj7"
	When I visit the Show page for lockout form: "nj7"
	And I click ok in the dialog after pressing "Copy"
	And I wait for the page to reload
	Then operating center "nj7" should be selected in the OperatingCenter dropdown
	And facility "x" should be selected in the Facility dropdown
	And equipment type "generator" should be selected in the EquipmentType dropdown
	And equipment "one" should be selected in the Equipment dropdown
	And the EmployeeAcknowledgedTraining field should be unchecked
	And "No" should be selected in the ContractorLockOutTagOut dropdown
	And lockout reason "one" should be selected in the LockoutReason dropdown
	And I should see "foo" in the ReasonForLockout field
	#lockout device location
	And "-- Select --" should be selected in the IsolationPoint dropdown 
	And lockout device "one" should not be selected in the LockoutDevice dropdown 
	When I enter "foo bar" into the ReasonForLockout field
	And I select lockout device location "one" from the IsolationPoint dropdown
	And I select lockout device "one" from the LockoutDevice dropdown
	And I enter "foo bar baz" into the LocationOfLockoutNotes field
	And I select employee "bill"'s Description from the OutOfServiceAuthorizedEmployee dropdown
	When I press Save
	Then the currently shown lockout form shall henceforth be known throughout the land as "phillipe"
	And I should be at the Show page for lockout form: "phillipe"
	And I should see a display for lockout form question: "two" with "Yes"
	And I should see a display for lockout form question: "three" with "n/a"