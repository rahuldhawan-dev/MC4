Feature: Town Page
    In order to manage and view towns and related data
    As a user
    I want to be able to view towns and related data
    And I want to be able to view towns and related data right now.

Background: being and nothingness
	Given a role "roleRead" exists with action: "Read", module: "GeneralTowns"
	And a role "roleEdit" exists with action: "Edit", module: "GeneralTowns"
	And a role "roleAdd" exists with action: "Add", module: "GeneralTowns"
	And a role "roleDelete" exists with action: "Delete", module: "GeneralTowns"
	And an admin user "admin" exists with username: "admin"
	And a user "user" exists with username: "user"
    And a user "authorized" exists with username: "authorized", roles: roleRead;roleEdit;roleAdd;roleDelete
	And a state "one" exists with name: "New Jersey", abbreviation: "NJ", scada table: "njscada"
	And a county "one" exists with name: "Monmouth", state: "one"
    And a town "one" exists with name: "Anytown", county: "one"
	And an operating center "opc" exists with opcode: "NJ7", name: "Shrewsbury", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
	And operating center: "opc" exists in town: "one"
    And a data type "data type" exists with table name: "Towns"
    And a document type "document type" exists with data type: "data type", name: "Town Document"
	And a document type "other" exists with data type: "data type", name: "Town Other Document"
	And an asset type "main" exists with description: "main"
	And an asset type "sewer main" exists with description: "sewer main"

#search/show
Scenario: admin can use dropdowns to find a town
	Given a town "two" exists with name: "one", county: "one"
	And I am logged in as "admin"
	When I visit the Town/Search page
	And I select state "one"'s Name from the State dropdown 
	And I select county "one"'s Name from the County dropdown
	And I select town "one"'s ShortName from the Id dropdown
	And I press Search
	And I wait for the page to reload
	Then I should be at the Show page for town: "one"

#edit
Scenario: admin can edit a town
    Given I am logged in as "admin"
	And a state "two" exists with name: "New York", abbreviation: "NY", scada table: "foo scada foo"
	And a county "two" exists with name: "Nassau", state: "two"
	And a town "two" exists with name: "other town", county: "two"
    When I visit the Show page for town: "one"
	And I follow "Edit"
	Then I should be at the Edit page for town: "one"
	When I enter "NewTown" into the ShortName field
	And I select state "two"'s Name from the State dropdown
	And I select county "two"'s Name from the County dropdown
	And I enter "123" into the DistrictId field
	And I press Save
	Then I should see a display for ShortName with "NewTown"  
	And I should see a display for State_Abbreviation with "NY"
	And I should see a display for County_Name with "Nassau"
	And I should see a display for DistrictId with "123" 
	
#add
Scenario: authorized can not add a town
	Given I am logged in as "authorized"
	When I visit the Town/New page expecting an error
    Then I should see "Access to the webpage was denied"

Scenario: admin can add a town
	Given an abbreviation type "one" exists 
	And I am logged in as "admin"
	When I visit the Town/New page expecting an error
	And I press "Save"
	Then I should see a validation message for ShortName with "The Township Name field is required."
	And I should see a validation message for AbbreviationType with "The AbbreviationType field is required."
	And I should see a validation message for FullName with "The FullName field is required."
	And I should see a validation message for State with "The State field is required."
	When I enter "Silent Hill" into the ShortName field
	And I select abbreviation type "one" from the AbbreviationType dropdown
	And I enter "Borough of Silent Hill" into the FullName field
	And I select state "one"'s Name from the State dropdown
	And I press "Save"
	Then I should see a validation message for County with "The County field is required."
	When I select county "one"'s Name from the County dropdown
	And I press "Save"
	Then the currently shown town shall henceforth be known throughout the land as "Phillip"
	And I should see a display for AbbreviationType with abbreviation type "one"
	And I should see a display for FullName with "Borough of Silent Hill"
	And I should see a display for ShortName with "Silent Hill"
	And I should see a display for State_Abbreviation with state "one"


#these two scenarios are covered below under child authorization
#Scenario: user cannot delete a document
#Scenario: user cannot link a document

# Notes
Scenario: admin adds a note to a town
    Given I am logged in as "admin"
    When I visit the Show page for town: "one"
    And I click the "Notes" tab
    And I press "New Note"
    And I enter "foo bar baz" into the Text field
    And I press "Add Note"
    And I click the "Notes" tab
    Then I should see "foo bar baz" in the table notesTable's "Note" column
    And I should see admin user "admin"'s UserName in the table notesTable's "Created By" column

Scenario: admin edits an existing note on a town
    Given a note "note" exists with text: "foo bar", town: "one", data type: "data type"
	And I am logged in as "admin"
    When I visit the Show page for town: "one"
    And I click the "Notes" tab
    Then I should see "foo bar" in the table notesTable's "Note" column
    When I click the "Edit" button in the 1st row of notesTable
    And I enter "asdf" into the #notesTable #Text field
    And I click the "Update" button in the 1st row of notesTable
    Then I should see "asdf" in the table notesTable's "Note" column

Scenario: admin deletes an existing note from a town
    Given a note "note" exists with text: "foo bar", town: "one", data type: "data type"
	And I am logged in as "admin"
    When I visit the Show page for town: "one"
    And I click the "Notes" tab
    Then I should see "foo bar" in the table notesTable's "Note" column
    When I click the "Delete" button in the 1st row of notesTable and then click ok in the confirmation dialog
    And I wait for the page to reload
    Then I should not see "foo bar" in the table notesTable's "Note" column

#these two scenarios are covered below under child authorization
#Scenario: user cannot delete a note
#Scenario: user cannot edit a note
#Scenario: user cannot add a note

#contacts 
Scenario: admin can click on the Add Contact button, see the form, and cancel adding the contact 
	Given I am logged in as "admin"
    When I visit the Show page for town: "one"
    And I click the "Contacts" tab
	Then I should not see the Contact_AutoComplete field
	And I should not see the ContactType field
	When I press "Add Contact for Town"
	Then I should see the Contact_AutoComplete field
	And I should see the ContactType field
	And I should not see the addContactButton element
	When I press cancelAddContactButton
	#And I wait for ajax to finish loading
	And I wait for animations to complete
	#And I wait 5 seconds 
	Then I should not see the Contact_AutoComplete field
	And I should not see the ContactType field

Scenario: admin can add a contact
	Given a contact "contact" exists with first name: "Tina", last name: "Belcher", email: "buns@onburner.com"
	And a contact type "ct" exists with description: "yowzah!"
	And I am logged in as "admin"
	When I visit the Show page for town: "one"
	And I click the "Contacts" tab
	And I press "Add Contact for Town"
	And I enter "Tina" and select "Belcher, Tina" from the Contact autocomplete field
	And I select contact type "ct"'s Description from the ContactType dropdown
	And I press saveTownContactButton
	And I wait for the page to reload
	Then I should be at the Show page for town: "one"
	When I click the "Contacts" tab
	Then I should see "Belcher, Tina"
	
Scenario: admin can delete a contact
	Given a contact "contact" exists with first name: "Tina", last name: "Belcher", email: "buns@onburner.com"
	And a contact type "ct" exists with description: "yowzah!"
	And a town contact "tc" exists with contact: "contact", town: "one"
	And I am logged in as "admin"
	When I visit the Show page for town: "one"
	And I click the "Contacts" tab
    And I click the "Remove" button in the 1st row of contactsTable and then click ok in the confirmation dialog
	And I wait for the page to reload
	And I click the "Contacts" tab
	Then I should not see "Belcher, Tina"

Scenario: authorized user can click on the Add Contact button, see the form, and cancel adding the contact 
	Given I am logged in as "authorized"
    When I visit the Show page for town: "one"
    And I click the "Contacts" tab
	Then I should not see the Contact_AutoComplete field
	And I should not see the ContactType field
	When I press "Add Contact for Town"
	Then I should see the Contact_AutoComplete field
	And I should see the ContactType field
	And I should not see the addContactButton element
	When I press cancelAddContactButton
	#And I wait for ajax to finish loading
	And I wait for animations to complete
	#And I wait 5 seconds 
	Then I should not see the Contact_AutoComplete field
	And I should not see the ContactType field

Scenario: authorized user can add a contact
	Given a contact "contact" exists with first name: "Tina", last name: "Belcher", email: "buns@onburner.com"
	And a contact type "ct" exists with description: "yowzah!"
	And I am logged in as "authorized"
	When I visit the Show page for town: "one"
	And I click the "Contacts" tab
	And I press "Add Contact for Town"
	And I enter "Tina" and select "Belcher, Tina" from the Contact autocomplete field
	And I select contact type "ct"'s Description from the ContactType dropdown
	And I press saveTownContactButton
	And I wait for the page to reload
	Then I should be at the Show page for town: "one"
	When I click the "Contacts" tab
	Then I should see "Belcher, Tina"
	
Scenario: authorized user can delete a contact
	Given a contact "contact" exists with first name: "Tina", last name: "Belcher", email: "buns@onburner.com"
	And a contact type "ct" exists with description: "yowzah!"
	And a town contact "tc" exists with contact: "contact", town: "one"
	And I am logged in as "authorized"
	When I visit the Show page for town: "one"
	And I click the "Contacts" tab
    And I click the "Remove" button in the 1st row of contactsTable and then click ok in the confirmation dialog
	And I wait for the page to reload
	And I click the "Contacts" tab
	Then I should not see "Belcher, Tina"

Scenario: unauthorized user can not add a new contact
	Given I am logged in as "user"
    When I visit the Show page for town: "one"
    And I click the "Contacts" tab
	Then I should not see the button "Add Contact for Town"
	Then I should not see the Contact_AutoComplete field
	And I should not see the ContactType field
	
Scenario: unauthorized user can not delete a contact
	Given a contact "contact" exists with first name: "Tina", last name: "Belcher", email: "buns@onburner.com"
	And a contact type "ct" exists with description: "yowzah!"
	And a town contact "tc" exists with contact: "contact", town: "one"
	And I am logged in as "user"
	When I visit the Show page for town: "one"
	And I click the "Contacts" tab
    Then I should not see the button "Remove"

#pwsid
Scenario: admin can add a public water supply
	Given a public water supply "one" exists with Identifier: "1234", operating area: "north", system: "System"
	And I am logged in as "admin"
	When I visit the Show page for town: "one"
	And I click the "PWSID" tab
	And I select public water supply "one"'s Description from the PublicWaterSupplyId dropdown
	And I press "Add PWSID"
	And I wait for the page to reload
	And I click the "PWSID" tab
	Then I should be at the Show page for town: "one"
	And I should see public water supply "one"'s Description in the table publicWaterSupplyTable's "Public Water Supplies" column

Scenario: admin can remove a public water supply
	Given a public water supply "one" exists with Identifier: "1234", operating area: "north", system: "System"
	And I am logged in as "admin"
	When I visit the Show page for town: "one"
	And I click the "PWSID" tab
	And I select public water supply "one"'s Description from the PublicWaterSupplyId dropdown
	And I press "Add PWSID"
	And I wait for the page to reload
	Then I should see public water supply "one"'s Description in the table publicWaterSupplyTable's "Public Water Supplies" column
	When I click the "PWSID" tab 
	And I click the "Remove PWSID" button in the 1st row of publicWaterSupplyTable and then click ok in the confirmation dialog
	And I wait for the page to reload
	And I click the "PWSID" tab
	Then I should not see public water supply "one"'s Description in the table publicWaterSupplyTable's "Public Water Supplies" column

#waste water system
Scenario: admin can add a waste water system
	Given a waste water system "one" exists
	And I am logged in as "admin"
	When I visit the Show page for town: "one"
	And I click the "Waste Water Systems" tab
	And I select waste water system "one"'s Description from the WasteWaterSystemId dropdown
	And I press "Add Waste Water System"
	And I wait for the page to reload
	Then I should be at the Show page for town: "one"
	And I should see waste water system "one"'s Description in the table wasteWaterSystemTable's "Waste Water Systems" column

Scenario: admin can remove a waste water system
	Given a waste water system "one" exists
	And I am logged in as "admin"
	When I visit the Show page for town: "one"
	And I click the "Waste Water Systems" tab
	And I select waste water system "one"'s Description from the WasteWaterSystemId dropdown
	And I press "Add Waste Water System"
	And I wait for the page to reload
	Then I should see waste water system "one"'s Description in the table wasteWaterSystemTable's "Waste Water Systems" column
	When I click the "Waste Water Systems" tab 
	And I click the "Remove Waste Water System" button in the 1st row of wasteWaterSystemTable and then click ok in the confirmation dialog
	And I wait for the page to reload
	And I click the "Waste Water Systems" tab
	Then I should not see waste water system "one"'s Description in the table wasteWaterSystemTable's "Waste Water Systems" column

#gradient
Scenario: admin can add a gradient
	Given a gradient "one" exists with description: "blah"
	And I am logged in as "admin"
	When I visit the Show page for town: "one"
	And I click the "Gradients" tab
	And I press "Add Gradient"
	And I select gradient "one"'s Description from the Gradient dropdown
	And I press "addGradientButton"
	And I wait for the page to reload
	Then I should see gradient "one"'s Description in the table gradients-table's "Description" column


Scenario: admin can remove a gradient
	Given a gradient "one" exists with description: "blah"
	And I am logged in as "admin"
	When I visit the Show page for town: "one"
	And I click the "Gradients" tab
	And I press "Add Gradient"
	And I select gradient "one"'s Description from the Gradient dropdown
	And I press "addGradientButton"
	And I wait for the page to reload
	Then I should see gradient "one"'s Description in the table gradients-table's "Description" column
	When I click the "Gradients" tab
	And I click the "Remove" button in the 1st row of gradients-table and then click ok in the confirmation dialog	
	And I wait for the page to reload
	And I click the "Gradients" tab
	Then I should not see gradient "one"'s Description in the table gradients-table's "Description" column

#operating centers
Scenario: admin can add an operating center town
	Given an operating center "nj4" exists with opcode: "NJ4", name: "Lakewood", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
	And a planning plant "one" exists with operating center: "nj4", code: "D217", description: "PPF1"
	And a planning plant "two" exists with operating center: "nj4", code: "P217", description: "PPF2"
	And a functional location "one" exists with description: "NJAC-AB-OH-0001", town: "one", asset type: "main"
	And a functional location "two" exists with description: "NJAC-AB-MG-0002", town: "one", asset type: "sewer main"
	And I am logged in as "admin"
	When I visit the Show page for town: "one"
	And I click the "Operating Centers" tab
	And I press "Add New Operating Center"
	And I select operating center "nj4" from the OperatingCenter dropdown
	And I enter "FOO" into the Abbreviation field
	And I enter "123" into the MainSAPEquipmentId field
	And I enter "124" into the SewerMainSAPEquipmentId field
	And I select functional location "one" from the MainSAPFunctionalLocation dropdown
	And I select functional location "two" from the SewerMainSAPFunctionalLocation dropdown
	And I select planning plant "one" from the DistributionPlanningPlant dropdown
	And I select planning plant "two" from the SewerPlanningPlant dropdown
	And I press "Add Operating Center"
	And I wait for the page to reload
	And I click the "Operating Centers" tab
	Then I should be at the Show page for town: "one"
	And I should see the following values in the operatingCenterTable table
	| Operating Center | Abbreviation | Main SAP Equipment | Main SAP Functional Location | Sewer Main SAP Equipment | Sewer Main SAP Functional Location | Distribution Planning Plant | Sewer Planning Plant |
	| NJ7 - Shrewsbury | XX           |                    |                              |                          |                                    |                             |                      |
	| NJ4 - Lakewood   | FOO          | 123                | NJAC-AB-OH-0001              | 124                      | NJAC-AB-MG-0002                    | D217 - NJ4 - PPF1           | P217 - NJ4 - PPF2    |

Scenario: admin can utterly destroy an operating center town
	Given an operating center "nj4" exists with opcode: "NJ4", name: "Lakewood", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
	And I am logged in as "admin"
	When I visit the Show page for town: "one"
	And I click the "Operating Centers" tab
	And I click ok in the dialog after pressing "Remove Operating Center" 
	And I wait for the page to reload
	Then I should not see operating center "nj4"'s Name in the table operatingCenterTable's "Operating Center" column

#child authorization
Scenario: user can not add, alter, or delete operating centers, fire districts, pwsid, contacts, notes, or documents
	Given I am logged in as "user"
	And a document "your papers" exists with document type: "document type", file name: "your papers"
    And a town document link "doculinky" exists with document: "your papers", town: "one"
	And a fire district "one" exists
	And a fire district town "foo" exists with town: "one", fire district: "one"
	And a note "note" exists with text: "foo bar", town: "one", data type: "data type"
	And a public water supply "one" exists with Identifier: "1234", operating area: "north"
	And a contact "contact" exists with first name: "Tina", last name: "Belcher", email: "buns@onburner.com"
	And a town contact "tc" exists with contact: "contact", town: "one"
	And public water supply: "one" exists in town: "one"
    When I visit the Show page for town: "one"
    And I click the "Operating Centers" tab
    Then I should not see the button "Delete"
    And I should not see the addOperatingCenterButton element
    When I click the "Fire Districts" tab
    Then I should not see "Set Default"
    And I should not see the button "Remove"
    And I should not see the addFireDistrictButton element
	When I click the "PWSID" tab
	Then I should not see the button "Add PWSID"
	And I should not see the button "Remove PWSID"
    When I click the "Contacts" tab
    Then I should not see the button "Remove"
	And I should not see the addContactButton element
    When I click the "Notes" tab
    Then I should not see the toggleNewNote element
	And I should not see the button "Delete"
	And I should not see the button "Edit"
    When I click the "Documents" tab
    Then I should not see the toggleLinkDocument element
	And I should not see the button "Delete"
    And I should not see the toggleNewDocument element

Scenario: authorized user can manipulate documents and notes
    Given a document status "one" exists with description: "active"
	Given a document "your papers" exists with document type: "document type", file name: "your papers"
    And a note "note" exists with text: "foo bar", town: "one", data type: "data type"
	And a contact "contact" exists with first name: "Tina", last name: "Belcher", email: "buns@onburner.com"
	And a contact type "ct" exists with description: "yowzah!"
	And I am logged in as "authorized"
    #  link existing document	
    When I visit the Show page for town: "one"
    And I click the "Documents" tab
	And I wait for ajax to finish loading
    And I press "Link Existing Document"
    And I enter "papers" into the docName field
    And I press List
    And I wait for ajax to finish loading
    And I select "Town Document -> your papers" from the DocumentId dropdown
    And I select document type "document type"'s Name from the DocumentType dropdown
    And I press "Link Document"
    And I click the "Documents" tab
	And I wait for ajax to finish loading
    Then I should see a secure link to the Download page for document: "your papers"
    # NOTE: Skipped CreatedAt because it ends with (EST) in this table and we don't have a way to test for that, and
    #       skipped UpdatedAt because it's the document link's UpdatedAt rather than the document's UpdatedAt, which we
    #       don't have a way to access here.
    And I should see the following values in the documentsTable table
        | File Name   | Created By | Document Type | Updated By |
        | your papers | *user*     | Town Document | authorized |
    # delete existing document
    When I click the "Delete" button in the 1st row of documentsTable and then click ok in the confirmation dialog
    And I wait for the page to reload
	And I click the "Documents" tab
	And I wait for ajax to finish loading
	Then I should not see "your papers" in the table documentsTable's "File Name" column
    # add new document
    When I visit the Show page for town: "one"
    And I click the "Documents" tab
	And I wait for ajax to finish loading
	And I press "New Document"
    And I select "Town Document" from the #pnlNewDocument #DocumentType dropdown
	And I upload "..\..\mmsinc\MapCall.CommonTest\TestFile.bin"
	And I wait 2 second
	Then I should see "TestFile.bin" in the FileName field
	When I enter "asdf" into the FileName field
    And I press Save
    And I click the "Documents" tab
	And I wait for ajax to finish loading
    # NOTE: Same note as above table about CreatedAt/UpdatedAt.
    Then I should see the following values in the documentsTable table
      | File Name | Created By | Document Type | Updated By |
      | asdf      | authorized | Town Document | authorized |
    And I should see "asdf" in the table documentsTable's "File Name" column
    And I should see document type "document type"'s Name in the table documentsTable's "Document Type" column
    # add new note
    When I visit the Show page for town: "one"
    And I click the "Notes" tab
    And I press "New Note"
    And I enter "this is a new note" into the Text field
    And I press "Add Note"
    And I click the "Notes" tab
    Then I should see "this is a new note" in the table notesTable's "Note" column
    And I should see user "authorized"'s UserName in the table notesTable's "Created By" column
    # edit existing note
    When I visit the Show page for town: "one"
    And I click the "Notes" tab
    Then I should see "foo bar" in the table notesTable's "Note" column
    When I click the "Edit" button in the 1st row of notesTable
    And I enter "foo bar baz" into the #notesTable #Text field
    And I click the "Update" button in the 1st row of notesTable
    Then I should see "foo bar baz" in the table notesTable's "Note" column
    # delete existing note
    When I visit the Show page for town: "one"
    And I click the "Notes" tab
    Then I should see "foo bar baz" in the table notesTable's "Note" column
    When I click the "Delete" button in the 1st row of notesTable and then click ok in the confirmation dialog
	And I wait for the page to reload
    Then I should not see "foo bar baz" in the table notesTable's "Note" column
