Feature: ContactPage
	If ifs and buts
	were candy and nuts
	We'd all have a page that lets you search, add, and edit contact-uts.

Background: users exist
	Given a user "user" exists with username: "user", email: "user@site.com"
	And a state "state" exists with name: "Canada", abbreviation: "CAN"
	And a county "county" exists with name: "Blah", state: "state"
	And a town "funky" exists with name: "Funkytown", county: "county"
	And a contact "contact" exists with first name: "Tina", last name: "Belcher", email: "buns@onburner.com", business phone number: "222-222-2222", home phone number: "333-333-3333", mobile phone number: "444-444-4444", fax number: "555-555-5555"
	And a contact "gene" exists with first name: "Gene", last name: "Belcher"
	And a role "image" exists with action: "Read", module: "FieldServicesImages", user: "user"

Scenario: A user can search for contacts
	Given I am logged in as "user"
	And I am at the Contact/Search page
	When I enter "Tina" into the FirstName field
	And I press Search
	Then I should see "Belcher, Tina" in the table contactsTable's "Name" column
	And I should see the link "buns@onburner.com" with the url "mailto:buns@onburner.com"
	And I should see a link to the Show page for contact: "contact"
	And I should not see a link to the Show page for contact: "gene"

Scenario: A user can view a user
	Given I am logged in as "user"
	When I go to the Show page for contact: "contact"
	Then I should see the link "buns@onburner.com" with the url "mailto:buns@onburner.com"
	And I should see a display for ContactName with "Belcher, Tina"
	And I should see a display for BusinessPhoneNumber with "(222) 222-2222"
	And I should see a display for HomePhoneNumber with "(333) 333-3333"
	And I should see a display for MobilePhoneNumber with "(444) 444-4444"
	And I should see a display for FaxNumber with "(555) 555-5555"

Scenario: A user with the field services data lookups add role can create a contact
	Given a role "role" exists with action: "Add", module: "FieldServicesDataLookups", user: "user"
	And I am logged in as "user"
	And I am at the Contact/New page
	When I enter "Bob" into the FirstName field
	And I enter "Q" into the MiddleInitial field
	And I enter "Belcher" into the LastName field
	And I enter "bob@burgers.com" into the Email field
	And I enter "555-555-5555" into the BusinessPhoneNumber field
	And I enter "6666666666" into the HomePhoneNumber field
	And I enter "(777) 777 7777" into the MobilePhoneNumber field
	And I enter "888-888-8888 x8313" into the FaxNumber field
	And I check the chkIncludeAddress field
	And I enter "Some Street" into the Address_Address1 field
	And I enter "PO Box 3" into the Address_Address2 field
	And I select "Canada" from the Address_State dropdown
	And I select "Blah" from the Address_County dropdown
	And I select "Funkytown" from the Address_Town dropdown
	And I enter "10001" into the Address_ZipCode field
	And I press Save
    And I wait for the page to reload	
	Then I should see a display for ContactName with "Belcher, Bob Q."
	And I should see the link "bob@burgers.com" with the url "mailto:bob@burgers.com"
	And I should see a display for BusinessPhoneNumber with "(555) 555-5555"
	And I should see a display for HomePhoneNumber with "(666) 666-6666"
	And I should see a display for MobilePhoneNumber with "(777) 777-7777"
	And I should see a display for FaxNumber with "(888) 888-8888 x8313"
	And I should see a display for Address with "Some Street PO Box 3 Funkytown, CAN 10001"

Scenario: The address fields should be disabled when the Include Address checkbox is not checked
	Given a role "role" exists with action: "Add", module: "FieldServicesDataLookups", user: "user"
	And I am logged in as "user"
	And I am at the Contact/New page
	When I check the chkIncludeAddress field
	Then the Address_Address1 field should be enabled
	And the Address_Address2 field should be enabled
	And the Address_State field should be enabled
	#County and Town are disabled cause of cascading.
	And the Address_County field should be disabled
	And the Address_Town field should be disabled
	When I uncheck the chkIncludeAddress field
	Then the Address_Address1 field should be disabled
	And the Address_Address2 field should be disabled
	And the Address_State field should be disabled
	And the Address_County field should be disabled
	And the Address_Town field should be disabled

Scenario: A user with the field services data lookups edit role can edit a contact
	Given a role "role" exists with action: "Edit", module: "FieldServicesDataLookups", user: "user"
	And I am logged in as "user"
	When I go to the edit page for contact: "contact"
	And I enter "Bob" into the FirstName field
	And I enter "Q" into the MiddleInitial field
	And I enter "Belcher" into the LastName field
	And I enter "bob@burgers.com" into the Email field
	And I enter "555-555-5555" into the BusinessPhoneNumber field
	And I enter "6666666666" into the HomePhoneNumber field
	And I enter "(777) 777 7777" into the MobilePhoneNumber field
	And I enter "888-888-8888 x8313" into the FaxNumber field
	And I check the chkIncludeAddress field
	And I enter "Some Street" into the Address_Address1 field
	And I enter "PO Box 3" into the Address_Address2 field
	And I select "Canada" from the Address_State dropdown
	And I select "Blah" from the Address_County dropdown
	And I select "Funkytown" from the Address_Town dropdown 
	And I enter "10001" into the Address_ZipCode field
	And I press Save
	Then I should be at the Show page for contact: "contact"
	And I should see a display for ContactName with "Belcher, Bob Q."
	And I should see the link "bob@burgers.com" with the url "mailto:bob@burgers.com"
	And I should see a display for BusinessPhoneNumber with "(555) 555-5555"
	And I should see a display for HomePhoneNumber with "(666) 666-6666"
	And I should see a display for MobilePhoneNumber with "(777) 777-7777"
	And I should see a display for FaxNumber with "(888) 888-8888 x8313"
	And I should see a display for Address with "Some Street PO Box 3 Funkytown, CAN 10001"

Scenario: Editing a contact with an address should have the checkbox checked and the fields enabled
	Given an address "addy" exists with town: "funky"
	And a contact "c" exists with address: "addy"
	And a role "role" exists with action: "Edit", module: "FieldServicesDataLookups", user: "user"
	And I am logged in as "user"
	When I go to the Edit page for contact: "c"
	Then the chkIncludeAddress field should be checked

Scenario: Editing a contact without an address should not have the checkbox checked and the fields should be disabled
	Given a contact without address "c" exists
	And a role "role" exists with action: "Edit", module: "FieldServicesDataLookups", user: "user"
	And I am logged in as "user"
	When I go to the Edit page for contact without address: "c"
	Then the chkIncludeAddress field should be unchecked
	
Scenario: A user with the field services data lookups delete role can delete a contact
    Given a role "role" exists with action: "Delete", module: "FieldServicesDataLookups", user: "user"
	And I am logged in as "user"
	And I am at the Show page for contact: "contact"
	When I click ok in the dialog after pressing "Delete"
	Then I should be at the Contact/Search page

Scenario: A user sees an error message when deleting a contact that has references
    Given a role "role" exists with action: "Delete", module: "FieldServicesDataLookups", user: "user"
	And a town contact "tc" exists with contact: "contact", town: "funky"
	And I am logged in as "user"
	And I am at the Show page for contact: "contact"
	When I click ok in the dialog after pressing "Delete"
	Then I should be at the Show page for contact: "contact"
	And I should see "This contact can not be deleted because it is currently referenced by another record."