Feature: HelpTopicsPage

Background: things exist
	Given an admin user "admin" exists with username: "admin"
	And a help category "one" exists
	And a help category "two" exists
    And a help topic subject matter "one" exists with description: "one"
    And a help topic subject matter "two" exists with description: "two"
    And a help topic "one" exists with category: "one"
    And a help topic "two" exists with category: "two"
    And I am logged in as "admin"

Scenario: admin can search for help topics
    And a help topic "three" exists with category: "one", subject matter: "one"
    And a help topic "four" exists with category: "two", subject matter: "two"
    And I am logged in as "admin"
    When I search for HelpTopics with no conditions chosen
    Then I should be at the HelpTopic page
    And I should see a link to the Show page for help topic "one"
    And I should see a link to the Show page for help topic "two"
    And I should see a link to the Show page for help topic "three"
    And I should see a link to the Show page for help topic "four"
    When I visit the /HelpTopic/Search page
    And I select help category "one" from the Category dropdown
    And I select help topic subject matter "one" from the SubjectMatter dropdown
    And I press Search
    Then I should be at the HelpTopic page
    And I should see a link to the Show page for help topic "three"
    And I should not see a link to the Show page for help topic "one"
    And I should not see a link to the Show page for help topic "two"
    And I should not see a link to the Show page for help topic "four"

Scenario: admin can search for help topics with documents
    And a help topic "three" exists
    And a help topic "four" exists
    And a help topic "five" exists
    And a data type "data type" exists with table name: "HelpTopics"
    And a document status "one" exists with description: "active"
    And a document type "one" exists with data type: "data type", name: "HelpTopic Document Type"
    And a document type "two" exists with data type: "data type", name: "HelpTopic Document Type"
    And a document "one" exists with document type: "one", file name: "your papers one"
    And a document "two" exists with document type: "two", file name: "your papers two"
    And a document "three" exists with document type: "one", file name: "your papers three 3456 blah"
    And a help topic document link "doculinky" exists with document: "one", help topic: "three", document status: "one"
    And I am logged in as "admin"
    When I visit the /HelpTopic/Search page
    And I select document type "one" from the DocumentType dropdown
    And I press Search
    Then I should be at the HelpTopic page
    And I should see a link to the Show page for help topic "three"
    And I should not see a link to the Show page for help topic "one"
    And I should not see a link to the Show page for help topic "two"
    And I should not see a link to the Show page for help topic "four"
    And I should not see a link to the Show page for help topic "five"
    Given a help topic document link "doculinky2" exists with document: "two", help topic: "four", document status: "one"
    When I visit the /HelpTopic/Search page
    And I select document status "one" from the Active dropdown
	And I select "=" from the DocumentUpdated_Operator dropdown
    And I enter "today's date" into the DocumentUpdated_End field
    And I press Search
    Then I should be at the HelpTopic page
    And I should see a link to the Show page for help topic "three"
    And I should see a link to the Show page for help topic "four"
    And I should not see a link to the Show page for help topic "one"
    And I should not see a link to the Show page for help topic "two"
    And I should not see a link to the Show page for help topic "five"
    Given a help topic document link "doculinky3" exists with document: "three", help topic: "five", document status: "one"
    When I visit the /HelpTopic/Search page
	And I enter "*3456*" into the DocumentTitle_Value field
	And I select "Wildcard" from the DocumentTitle_MatchType dropdown
    And I press Search
    Then I should be at the HelpTopic page
    And I should see a link to the Show page for help topic "five"
    And I should not see a link to the Show page for help topic "one"
    And I should not see a link to the Show page for help topic "two"
    And I should not see a link to the Show page for help topic "three"
    And I should not see a link to the Show page for help topic "four"

Scenario: admin can create an help topic
    When I visit the /HelpTopic/New page
    And I press Save
    Then I should be at the HelpTopic/New page
    And I should see the validation message "The Title field is required."
    And I should see the validation message "The Description field is required."
    And I should see the validation message "The Category field is required."
    And I should see the validation message "The SubjectMatter field is required."
    When I enter "asdf" into the Title field
    And I enter "blah" into the Description field
    And I select help category "one" from the Category dropdown
    And I select help topic subject matter "one" from the SubjectMatter dropdown
    And I press Save
    Then the currently shown help topic will now be referred to as "new"
    And I should be at the Show page for help topic: "new"
    And I should see a display for Title with "asdf"
    And I should see a display for Description with "blah"
    And I should see a display for Category with help category "one"
    And I should see a display for SubjectMatter with help topic subject matter "one"

Scenario: admin can edit an help topic
    When I visit the Show page for help topic: "one"
    And I follow "Edit"
    And I enter "foobar" into the Title field
    And I enter "blah" into the Description field
    And I select help category "one" from the Category dropdown
    And I select help topic subject matter "one" from the SubjectMatter dropdown
    And I press Save
    Then I should be at the Show page for help topic: "one"
    And I should see a display for Title with "foobar"
    And I should see a display for Description with "blah"
    And I should see a display for Category with help category "one"
    And I should see a display for SubjectMatter with help topic subject matter "one"