Feature: WorkDescriptionPage
         In order to search for and edit work descriptions
         as an admin
         I want to be able to search for and edit work descriptions as an admin

Background: 
    Given an admin user "admin" exists with username: "admin"  
    And a asset type "one" exists with id: "1"
    And a restoration accounting code "acct1" exists with code: "1"
    And a restoration product code "prod1" exists
    And a work description "work1" exists with description: "hydrant repair", id: "203", asset type: "one"
    And a work description "work2" exists with description: "pump repair", id: "204", asset type: "one", first restoration accounting code: "acct1", first restoration product code: "prod1"
    And I am logged in as "admin"

Scenario: admin can edit work descriptions and interact with checkboxes
    Given I am logged in as "admin"
    And I am at the FieldOperations/WorkDescription/Search page 
    When I select asset type "one" from the AssetType dropdown
    And I press Search
    And I wait for the page to reload 
    Then I should see "203" in the table workDescriptionsTable's "Id" column 
    When I visit the FieldOperations/WorkDescription/Show page for work description: "work2" 
    Then I should see a display for Id with work description "work2"'s Id
    And I should see a display for MarkoutRequired with "n/a"
    When I visit the FieldOperations/WorkDescription/Edit page for work description: "work2"
    Then I should see a checkbox named MarkoutRequired with the value "true"
    When I select restoration accounting code "acct1" from the FirstRestorationAccountingCode dropdown 
    And I select restoration product code "prod1" from the FirstRestorationProductCode dropdown 
    When I press Save
    And I wait for the page to reload
    Then I should see a display for Id with "203"
    And I should see a display for MarkoutRequired with "No"
    And I should see a display for MaterialsRequired with "No"
    And I should see a display for JobSiteCheckListRequired with "No"
    And I should see a display for DigitalAsBuiltRequired with "No"
    When I visit the FieldOperations/WorkDescription/Edit page for work description: "work2"
    And I check the MaterialsRequired field
    And I check the DigitalAsBuiltRequired field
    When I press Save
    And I wait for the page to reload
    Then I should see a display for Id with "203"
    And I should see a display for MarkoutRequired with "No"
    And I should see a display for MaterialsRequired with "Yes"
    And I should see a display for JobSiteCheckListRequired with "No"
    And I should see a display for DigitalAsBuiltRequired with "Yes"
