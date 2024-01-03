Feature: Documents tab
	The documents tab does a lot of things that we want to make sure do not break

Background: being and nothingness
	# This is so we don't have to create 25+ docs for every test
	Given the test flag "document page size 5" exists
	And a role "roleRead" exists with action: "Read", module: "GeneralTowns"
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
    And a document status "one" exists with description: "active"
    And public water supply statuses exist
	And a recurring frequency unit "year" exists with description: "Year"

# Documents
Scenario: admin links an existing document to a town
    Given a document "your papers" exists with document type: "document type", file name: "your papers"
    And I am logged in as "admin"
    When I visit the Show page for town: "one"
    And I click the "Documents" tab
    And I press "Link Existing Document"
    And I enter "papers" into the docName field
    And I press List
    And I wait for ajax to finish loading
    And I select "Town Document -> your papers" from the DocumentId dropdown
    And I select document type "document type"'s Name from the DocumentType dropdown
    And I press "Link Document"
    And I click the "Documents" tab
    Then I should see a secure link to the Download page for document: "your papers"

Scenario: admin creates a new document
    Given I am logged in as "admin"
    When I visit the Show page for town: "one"
    And I click the "Documents" tab
	And I press "New Document"
    And I select document type "document type"'s Name from the #pnlNewDocument #DocumentType dropdown
	And I upload "..\..\mmsinc\MapCall.CommonTest\TestFile.bin"
	And I wait 2 second
	Then I should see "TestFile.bin" in the FileName field
	When I enter "asdf" into the FileName field
    And I press Save
    And I click the "Documents" tab
    Then I should see "asdf" in the table documentsTable's "File Name" column
    And I should see document type "document type"'s Name in the table documentsTable's "Document Type" column

Scenario: admin creates a new document for help topic
	And a help category "one" exists
    And a help topic subject matter "one" exists with description: "one"
    And a help topic "one" exists with category: "one"
    And a data type "HelpTopics" exists with table name: "HelpTopics"
    And a document type "HelpTopics" exists with data type: "HelpTopics", name: "HelpTopics"
    Given I am logged in as "admin"
    When I visit the Show page for help topic: "one"
    And I click the "Documents" tab
	And I press "New Document"
    And I select document type "HelpTopics"'s Name from the #pnlNewDocument #DocumentType dropdown
	And I upload "..\..\mmsinc\MapCall.CommonTest\TestFile.bin"
	And I wait 2 second
	Then I should see "TestFile.bin" in the FileName field
	When I enter "asdf" into the FileName field
	And I select "-- Select --" from the DocumentStatus dropdown
	When I enter "" into the ReviewFrequency field
	And I select "-- Select --" from the ReviewFrequencyUnit dropdown
    And I press Save
    And I click the "Documents" tab
    Then I should see "asdf" in the table documentsTable's "File Name" column
    Then I shouldn't see "active" in the table documentsTable's "Document Status" column
    Then I shouldn't see "Year" in the table documentsTable's "Review Frequency" column
    When I visit the Show page for help topic: "one"
    When I click the "Documents" tab
	And I press "New Document"
    And I select document type "HelpTopics"'s Name from the #pnlNewDocument #DocumentType dropdown
	And I upload "..\..\mmsinc\MapCall.CommonTest\TestFile.bin"
	And I wait 2 second
	Then I should see "TestFile.bin" in the FileName field
	When I enter "asdf" into the FileName field
    And I press Save
    And I click the "Documents" tab
    Then I should see "asdf" in the table documentsTable's "File Name" column
    Then I should see "active" in the table documentsTable's "Document Status" column
    Then I should see "5 Year" in the table documentsTable's "Review Frequency" column
    And I should see document type "HelpTopics"'s Name in the table documentsTable's "Document Type" column
    When I visit the Show page for help topic: "one"
    When I click the "Documents" tab
	And I press "New Document"
    And I select document type "HelpTopics"'s Name from the #pnlNewDocument #DocumentType dropdown
	And I upload "..\..\mmsinc\MapCall.CommonTest\TestFile.bin"
	And I wait 2 second
	Then I should see "TestFile.bin" in the FileName field
	When I enter "asdf" into the FileName field
    And I select document status "one" from the DocumentStatus dropdown
	When I enter "1" into the ReviewFrequency field
	And I select recurring frequency unit "year"'s Description from the ReviewFrequencyUnit dropdown
    And I press Save
    And I click the "Documents" tab
    Then I should see "asdf" in the table documentsTable's "File Name" column
    Then I should see "active" in the table documentsTable's "Document Status" column
    Then I should see "1 Year" in the table documentsTable's "Review Frequency" column
    When I visit the Show page for help topic: "one"
    When I click the "Documents" tab
	And I press "New Document"

Scenario: admin edits a document link
	Given a document "your papers" exists with document type: "document type", file name: "your papers"
    And a town document link "doculinky" exists with document: "your papers", town: "one"
    And I am logged in as "admin"
    When I visit the Show page for town: "one"
    And I click the "Documents" tab
	And I click the "Edit" link in the 1st row of documentsTable
	And I select document type "other"'s Name from the DocumentType dropdown 
	And I press "Save"
	Then I should be at the Show page for town: "one"
	When I click the "Documents" tab
	Then I should see document type "other"'s Name in the table documentsTable's "Document Type" column

Scenario: admin edits a document link for help topic
	And a help category "one" exists
    And a help topic subject matter "one" exists with description: "one"
    And a help topic "one" exists with category: "one"
    And a data type "HelpTopics" exists with table name: "HelpTopics"
    And a document type "HelpTopics" exists with data type: "HelpTopics", name: "HelpTopics"
	Given a document "one" exists with document type: "HelpTopics", file name: "your papers"
    And a help topic document link "doculinky" exists with document: "one", help topic: "one", document status: "one"
    And I am logged in as "admin"
    When I visit the Show page for help topic: "one"
    And I click the "Documents" tab
	And I click the "Edit" link in the 1st row of documentsTable
	And I select document type "HelpTopics"'s Name from the DocumentType dropdown 
    And I select document status "one" from the DocumentStatus dropdown
	When I enter "1" into the ReviewFrequency field
	And I select recurring frequency unit "year"'s Description from the ReviewFrequencyUnit dropdown
	And I press "Save"
	Then I should be at the Show page for help topic: "one"
	When I click the "Documents" tab
	Then I should see document type "HelpTopics"'s Name in the table documentsTable's "Document Type" column
    Then I should see "active" in the table documentsTable's "Document Status" column
    Then I should see "1 Year" in the table documentsTable's "Review Frequency" column

Scenario: admin deletes a document from a town
    Given a document "your papers" exists with document type: "document type", file name: "your papers"
    And a town document link "doculinky" exists with document: "your papers", town: "one"
    And I am logged in as "admin"
    When I visit the Show page for town: "one"
    And I click the "Documents" tab
    When I click the "Delete" button in the 1st row of documentsTable and then click ok in the confirmation dialog
    And I wait for the page to reload
	And I click the "Documents" tab
	Then I should not see "your papers" in the table documentsTable's "File Name" column

Scenario: user without roles can not add, edit, or delete documents
	Given a document "your papers" exists with document type: "document type", file name: "your papers"
    And a town document link "doculinky" exists with document: "your papers", town: "one"
	And a fire district "one" exists
	And a fire district town "foo" exists with town: "one", fire district: "one"
	And a note "note" exists with text: "foo bar", town: "one", data type: "data type"
	And a public water supply "one" exists with Identifier: "1234", operating area: "north", status: "active"
	And a contact "contact" exists with first name: "Tina", last name: "Belcher", email: "buns@onburner.com"
	And a town contact "tc" exists with contact: "contact", town: "one"
	And public water supply: "one" exists in town: "one"
	And I am logged in as "user"
    When I visit the Show page for town: "one"
    When I click the "Documents" tab
    Then I should not see the toggleLinkDocument element
	And I should not see the button "Delete"
    And I should not see the toggleNewDocument element

Scenario: user without roles should be able to paginate documents
	Given a document "doc1" exists with document type: "document type", file name: "doc 1"
    And a town document link "doclink1" exists with document: "doc1", town: "one"
	And a document "doc2" exists with document type: "document type", file name: "doc 2"
    And a town document link "doclink2" exists with document: "doc2", town: "one"
	And a document "doc3" exists with document type: "document type", file name: "doc 3"
    And a town document link "doclink3" exists with document: "doc3", town: "one"
	And a document "doc4" exists with document type: "document type", file name: "doc 4"
    And a town document link "doclink4" exists with document: "doc4", town: "one"
	And a document "doc5" exists with document type: "document type", file name: "doc 5"
    And a town document link "doclink5" exists with document: "doc5", town: "one"
	And a document "doc6" exists with document type: "document type", file name: "doc 6"
    And a town document link "doclink6" exists with document: "doc6", town: "one"
    And I am logged in as "admin"
    When I visit the Show page for town: "one"
    And I click the "Documents" tab
	Then I should see the following values in the documentsTable table
         | File Name |
         | doc 1     |
         | doc 2     |
         | doc 3     |
         | doc 4     |
         | doc 5     |
	When I follow "2"
	Then I should see the following values in the documentsTable table
         | File Name |
         | doc 6     |
      
Scenario: authorized user should be able to delete a paginated document
    Given I do not currently function
    # this test is flaky as of 2021-02-03
	#Given a document "doc1" exists with document type: "document type", file name: "doc 1"
    #And a town document link "doclink1" exists with document: "doc1", town: "one"
	#And a document "doc2" exists with document type: "document type", file name: "doc 2"
    #And a town document link "doclink2" exists with document: "doc2", town: "one"
	#And a document "doc3" exists with document type: "document type", file name: "doc 3"
    #And a town document link "doclink3" exists with document: "doc3", town: "one"
	#And a document "doc4" exists with document type: "document type", file name: "doc 4"
    #And a town document link "doclink4" exists with document: "doc4", town: "one"
	#And a document "doc5" exists with document type: "document type", file name: "doc 5"
    #And a town document link "doclink5" exists with document: "doc5", town: "one"
	#And a document "doc6" exists with document type: "document type", file name: "doc 6"
    #And a town document link "doclink6" exists with document: "doc6", town: "one"
    #And I am logged in as "admin"
    #When I visit the Show page for town: "one"
    #And I click the "Documents" tab
	#And I follow "2"
	## deleting doc11
	#And I click the "Delete" button in the 1st row of documentsTable and then click ok in the confirmation dialog
	#And I click the "Documents" tab
	## There won't be pagination anymore, so check the tab text for record count
	#Then I should see "Documents (5)"
	#And I should see the following values in the documentsTable table
    #    | File Name |
    #    | doc 1     |
    #    | doc 2     |
    #    | doc 3     |
    #    | doc 4     |
    #    | doc 5     |


Scenario: authorized user should be able to edit a paginated document
	Given a document "doc1" exists with document type: "document type", file name: "doc 1"
    And a town document link "doclink1" exists with document: "doc1", town: "one"
	And a document "doc2" exists with document type: "document type", file name: "doc 2"
    And a town document link "doclink2" exists with document: "doc2", town: "one"
	And a document "doc3" exists with document type: "document type", file name: "doc 3"
    And a town document link "doclink3" exists with document: "doc3", town: "one"
	And a document "doc4" exists with document type: "document type", file name: "doc 4"
    And a town document link "doclink4" exists with document: "doc4", town: "one"
	And a document "doc5" exists with document type: "document type", file name: "doc 5"
    And a town document link "doclink5" exists with document: "doc5", town: "one"
	And a document "doc6" exists with document type: "document type", file name: "doc 6"
    And a town document link "doclink6" exists with document: "doc6", town: "one"
    And I am logged in as "admin"
    When I visit the Show page for town: "one"
    And I click the "Documents" tab
	And I follow "2"
	# editing doc11
	And I click the "Edit" link in the 1st row of documentsTable
	And I select document type "other"'s Name from the DocumentType dropdown 
	And I press "Save"
	Then I should be at the Show page for town: "one"
	When I click the "Documents" tab
	And I follow "2"
	Then I should see the following values in the documentsTable table
        | File Name | Document Type       |
        | doc 6     | Town Other Document |
       
Scenario: authorized user can create a new document after paginating
	Given a document "doc1" exists with document type: "document type", file name: "doc 1"
    And a town document link "doclink1" exists with document: "doc1", town: "one"
	And a document "doc2" exists with document type: "document type", file name: "doc 2"
    And a town document link "doclink2" exists with document: "doc2", town: "one"
	And a document "doc3" exists with document type: "document type", file name: "doc 3"
    And a town document link "doclink3" exists with document: "doc3", town: "one"
	And a document "doc4" exists with document type: "document type", file name: "doc 4"
    And a town document link "doclink4" exists with document: "doc4", town: "one"
	And a document "doc5" exists with document type: "document type", file name: "doc 5"
    And a town document link "doclink5" exists with document: "doc5", town: "one"
	And a document "doc6" exists with document type: "document type", file name: "doc 6"
    And a town document link "doclink6" exists with document: "doc6", town: "one"
    And I am logged in as "admin"
    When I visit the Show page for town: "one"
    And I click the "Documents" tab
	And I follow "2"
	And I press "New Document"
    And I select document type "document type"'s Name from the #pnlNewDocument #DocumentType dropdown
	And I upload "..\..\mmsinc\MapCall.CommonTest\TestFile.bin"
	And I wait 2 second
	Then I should see "TestFile.bin" in the FileName field
	When I enter "asdf" into the FileName field
    And I press Save
    And I click the "Documents" tab
	And I follow "2"
	Then I should see the following values in the documentsTable table
        | File Name | Document Type |
        | doc 6     | Town Document |
        | asdf      | Town Document |

Scenario: authorized user can link a document after paginating
	Given a document "doc1" exists with document type: "document type", file name: "doc 1"
    And a town document link "doclink1" exists with document: "doc1", town: "one"
	And a document "doc2" exists with document type: "document type", file name: "doc 2"
    And a town document link "doclink2" exists with document: "doc2", town: "one"
	And a document "doc3" exists with document type: "document type", file name: "doc 3"
    And a town document link "doclink3" exists with document: "doc3", town: "one"
	And a document "doc4" exists with document type: "document type", file name: "doc 4"
    And a town document link "doclink4" exists with document: "doc4", town: "one"
	And a document "doc5" exists with document type: "document type", file name: "doc 5"
    And a town document link "doclink5" exists with document: "doc5", town: "one"
	And a document "doc6" exists with document type: "document type", file name: "doc 6"
    And a town document link "doclink6" exists with document: "doc6", town: "one"
    And a document "your papers" exists with document type: "document type", file name: "your papers"
    And I am logged in as "admin"
    When I visit the Show page for town: "one"
    And I click the "Documents" tab
	And I follow "2"
    And I press "Link Existing Document"
    And I enter "papers" into the docName field
    And I press List
    And I wait for ajax to finish loading
    And I select "Town Document -> your papers" from the DocumentId dropdown
    And I select document type "document type"'s Name from the DocumentType dropdown
    And I press "Link Document"
    And I click the "Documents" tab
    And I wait for ajax to finish loading
	And I follow "2"
    Then I should see a secure link to the Download page for document: "your papers"
	Then I should see the following values in the documentsTable table
        | File Name   | Document Type |
        | doc 6       | Town Document |
        | your papers | Town Document |