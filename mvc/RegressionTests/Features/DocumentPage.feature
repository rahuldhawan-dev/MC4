Feature: DocumentPage
	
Background: 
	Given an admin user "admin" exists with username: "admin"
	And a user "user" exists with username: "user", email: "user@site.com"
	And a data type "data type" exists with table name: "Towns"
	And a document type "document type" exists with data type: "data type", name: "Town Document"
	And a document "your papers" exists with document type: "document type", file name: "your papers"
	
Scenario: admin cannot download document without a token
    Given I am logged in as "admin"
    When I visit the Download page for document: "your papers" expecting an error
    Then I should see "Looks like there was a problem..."
	And I should see "Security token not found or is invalid."

Scenario: user cannot download document without a token
    Given I am logged in as "user"
    When I visit the Download page for document: "your papers" expecting an error
    Then I should see "Looks like there was a problem..."
	And I should see "Security token not found or is invalid."