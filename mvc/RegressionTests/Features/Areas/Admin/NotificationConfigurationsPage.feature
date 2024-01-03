Feature: Notification Configurations

Background: data exists
    Given a state "NJ" exists with abbreviation: "NJ"
    And a town "Swedesboro" exists with state: "NJ"
    And an operating center "oc" exists with opcode: "NJ4", name: "Shrewsbury", state: "NJ"
    And operating center: "oc" exists in town: "Swedesboro"
    And a user "user" exists with username: "user"
    And a contact "c" exists with first name: "Dazzle", last name: "Razzle", email: "dazzle@razzle.com"
    And a contact "c2" exists with first name: "Bob", last name: "LastName", email: "something@somewhere.com"
    And a notification purpose "np-01" exists with module: "EnvironmentalPermitTypesExpiration", purpose: "Land Use Permit"
    And a notification purpose "np-02" exists with module: "EnvironmentalPermitTypesExpiration", purpose: "Soil District Permits"
    And a notification configuration "nc" exists with operating center: "oc", contact: "c"    
    And notification purpose "np-01" exists in notification configuration: "nc"
    And notification purpose "np-02" exists in notification configuration: "nc"

Scenario: User should see validation messages on the create notification configurations page
    Given I am logged in as "user"
    When I visit the Admin/NotificationConfiguration/New page
    And I press Save
    Then I should see a validation message for Contacts with "The Contacts field is required."
    Then I should see a validation message for OperatingCenters with "The OperatingCenters field is required."
    Then I should see a validation message for Application with "The Application field is required."
    When I check the AppliesToAllOperatingCenters field
    And I press Save 
    Then I should not see a validation message for OperatingCenters with "The OperatingCenters field is required."
    When I uncheck the AppliesToAllOperatingCenters field
    When I select contact "c"'s ContactName from the Contacts multiselect
    And I select operating center "oc" from the OperatingCenters multiselect
    And I select "Environmental" from the Application multiselect
    And I press Save
    Then I should see a validation message for Module with "The Module field is required."
    When I select "Environmental - EnvironmentalPermitTypesExpiration" from the Module multiselect
    And I press Save
    Then I should see a validation message for NotificationPurposes with "The NotificationPurposes field is required."

Scenario: User can add a new notification configuration
    Given I am logged in as "user"
    When I visit the Admin/NotificationConfiguration/New page
    And I select contact "c"'s ContactName from the Contacts multiselect
    And I select operating center "oc" from the OperatingCenters multiselect
    And I select "Environmental" from the Application multiselect
    And I select "Environmental - EnvironmentalPermitTypesExpiration" from the Module multiselect
    And I select notification purpose "np-01" from the NotificationPurposes multiselect
    And I press Save
    Then I should see the success message "You've created 1 notifications!"

Scenario: User can create multiple new notification configurations 
    Given I am logged in as "user"
    When I visit the Admin/NotificationConfiguration/New page
    And I select contact "c"'s ContactName from the Contacts multiselect
    And I select contact "c2"'s ContactName from the Contacts multiselect
    And I select operating center "oc" from the OperatingCenters multiselect
    And I select "Environmental" from the Application multiselect
    And I select "Environmental - EnvironmentalPermitTypesExpiration" from the Module multiselect
    And I select notification purpose "np-02" from the NotificationPurposes multiselect
    And I select notification purpose "np-01" from the NotificationPurposes multiselect
    And I press Save
    Then I should see the success message "You've created 4 notifications!"

Scenario: User can edit an existing notification configuration
    Given I am logged in as "user"
    When I visit the Edit page for notification configuration: "nc"
    Then contact "c"'s ContactName should be selected in the Contact dropdown
    And operating center "oc"'s ToString should be selected in the OperatingCenter dropdown
    And notification purpose "np-01"'s ToString should be selected in the NotificationPurposes dropdown
    When I select notification purpose "np-02" from the NotificationPurposes multiselect
    And I press Save
    Then I should see "Land Use Permit, Soil District Permits"

Scenario: A user can delete a notification configuration
    Given I am logged in as "user"
    When I visit the Show page for notification configuration: "nc"
    And I click ok in the dialog after pressing "Delete"
    Then I should be at the Admin/NotificationConfiguration/Search page
    When I try to access the Show page for notification configuration: "nc" expecting an error
    Then I should see a 404 error message

Scenario: User can search for an existing notification configuration
    Given I am logged in as "user"
    When I visit the Admin/NotificationConfiguration/Search page
    And I press Search
    Then I should see a link to the Show page for notification configuration: "nc"

Scenario: User can select a bunch of operating centers by selecting a state
    Given a state "XX" exists with abbreviation: "XX"
    And an operating center "oc-nj2" exists with opcode: "NJ7", name: "Shrewsbury", state: "NJ"
    And an operating center "oc-xx1" exists with opcode: "XX1", name: "Shrewsbury", state: "XX"
    And an operating center "oc-xx2" exists with opcode: "XX2", name: "Shrewsbury", state: "XX"
    And I am logged in as "user"
    When I visit the Admin/NotificationConfiguration/New page
    And I select state "XX" from the States multiselect
    Then operating center "oc-xx1" should be selected in the OperatingCenters multiselect
    And operating center "oc-xx2" should be selected in the OperatingCenters multiselect
    And operating center "oc" should not be selected in the OperatingCenters multiselect
    And operating center "oc-nj2" should not be selected in the OperatingCenters multiselect

Scenario: The operating centers field should be disabled when the user selects AppliesToAllOperatingCenters on the new page
    Given I am logged in as "user"
    When I visit the Admin/NotificationConfiguration/New page
    And I press Save
    Then I should see a validation message for Contacts with "The Contacts field is required."
    Then I should see a validation message for OperatingCenters with "The OperatingCenters field is required."
    Then I should see a validation message for Application with "The Application field is required."
    When I check the AppliesToAllOperatingCenters field
    Then the OperatingCenters field should be disabled
    When I uncheck the AppliesToAllOperatingCenters field 
    Then the OperatingCenters field should be enabled

Scenario: The operating center field should be disabled when the user selects AppliesToAllOperatingCenters on the edit page
    Given I am logged in as "user"
    When I visit the Edit page for notification configuration: "nc"
    When I check the AppliesToAllOperatingCenters field
    Then the OperatingCenter field should be disabled
    When I uncheck the AppliesToAllOperatingCenters field 
    Then the OperatingCenter field should be enabled
