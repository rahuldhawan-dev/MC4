Feature: WaterQualityComplaintPage

Background: data exists
    Given an operating center "nj7" exists with opcode: "QQ7", name: "Shrewsbury", sap enabled: "true"
    And an operating center "nj4" exists with opcode: "NJ4", name: "Howell"
    And a state "one" exists with name: "New Jersey", abbreviation: "NJ", scada table: "njscada"
    And a county "one" exists with name: "Monmouth", state: "one"
    And a town "one" exists with name: "Loch Arbour", county: "one"
    And operating center: "nj7" exists in town: "one"
    And operating center: "nj4" exists in town: "one"
    And a town section "active" exists with town: "one"
    And a town section "inactive" exists with town: "one", active: false
    And a coordinate "one" exists
    And an employee status "active" exists with description: "Active"
    And an employee "one" exists with first name: "first", last name: "last", operating center: "nj7", status: "active"
    And an employee "two" exists with first name: "second", last name: "last", operating center: "nj7", status: "active"
    And an employee "three" exists with first name: "admin", last name: "last", operating center: "nj7", status: "active"
    And a water quality complaint "one" exists with state: "one", town: "one", operating center: "nj7", premise number: "123456789", coordinate: "one", initial local contact: "one"
    And a water quality complaint "two" exists with state: "one", town: "one", operating center: "nj7"
    And a water quality complaint "thre" exists with state: "one", town: "one", operating center: "nj7", initial local contact: "three"
    And an admin user "admin" exists with username: "admin", first name: "admin", last name: "last", employee: "three"
    And I am logged in as "admin"

Scenario: user can search for a water quality complaint
    When I visit the WaterQuality/WaterQualityComplaint/Search page
    And I press Search
    Then I should see the following values in the waterQualityComplaints table
        | Complaint Number | Initial Local Contact | Operating Center |
        | 1                | first last            | QQ7 - Shrewsbury |
        | 2                |                       | QQ7 - Shrewsbury |
    When I visit the WaterQuality/WaterQualityComplaint/Search page
    And I select state "one" from the State dropdown
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I enter "first" and select employee "one"'s Description from the InitialLocalContact autocomplete field
    And I press Search
    Then I should see the following values in the waterQualityComplaints table
        | Complaint Number | Initial Local Contact | Operating Center |
        | 1                | first last            | QQ7 - Shrewsbury |
    And I should see "Records Found: 1"
    And I should see a link to the Show page for water quality complaint: "one"
    When I follow the Show link for water quality complaint "one"
    Then I should be at the Show page for water quality complaint: "one"
	
Scenario: user can view a water quality complaint
    When I visit the Show page for water quality complaint: "one"
    Then I should see a display for water quality complaint: "one"'s CustomerName
    And I should see a display for InitialLocalContact with employee "one"'s FullName

Scenario: user can add a water quality complaint
    When I visit the WaterQuality/WaterQualityComplaint/New page
    And I enter "foo" into the CustomerName field
    And I press Save
    And I wait for the page to reload
    Then I should see a validation message for State with "The State field is required."
    And I should see a validation message for PremiseNumber with "The PremiseNumber field is required."
    And I should see a validation message for CoordinateId with "The Coordinates field is required."
    When I select state "one" from the State dropdown
    And I press Save
    Then I should see a validation message for Town with "The Town field is required."
    When I select town "one" from the Town dropdown
    And I press Save
    Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I enter coordinate "one"'s Id into the CoordinateId field
    And I enter "12345678" into the PremiseNumber field
    And I press Save
    Then I should see a validation message for InitialLocalContact with "The Initial Local Contact By field is required."
    When I enter "first" and select employee "one"'s Description from the InitialLocalContact autocomplete field
    And I press Save
    Then the currently shown water quality complaint will now be referred to as "new"
    And I should see a display for CustomerName with "foo"
    And I should see a display for InitialLocalContact with employee "one"'s FullName

Scenario: user cannot create a water quality complaint for an inactive town section
    When I visit the WaterQuality/WaterQualityComplaint/New page
    And I select state "one" from the State dropdown
    And I wait for ajax to finish loading
    And I select town "one" from the Town dropdown
    And I wait for ajax to finish loading
    Then I should not see town section "inactive" in the TownSection dropdown

Scenario: user can edit a water quality complaint
    When I visit the Edit page for water quality complaint: "one"
    And I select operating center "nj4" from the OperatingCenter dropdown
    And I enter "bar" into the CustomerName field
    And I enter "second" and select employee "two"'s Description from the InitialLocalContact autocomplete field
    And I enter "second" and select employee "two"'s Description from the NotificationCompletedBy autocomplete field
    And I press Save
    Then I should be at the Show page for water quality complaint: "one"
    And I should see a display for OperatingCenter with operating center "nj4"'s Description
    And I should see a display for CustomerName with "bar"
    And I should see a display for InitialLocalContact with employee "two"'s FullName
    And I should see a display for NotificationCompletedBy with employee "two"'s FullName

Scenario: user can and edit and remove sample results to and from water quality complaint
    Given a water constituent "one" exists
    And I am logged in as "admin"
    When I visit the Show page for water quality complaint: "one"
    And I click the "Sample Results" tab
    And I press "Add New Sample Result"
    And I press "Save Sample Result"
    Then I should see the validation message "The WaterConstituent field is required."
    And I should see the validation message "The SampleDate field is required."
    And I should see the validation message "The SampleValue field is required."
    And I should see the validation message "The AnalysisPerformedBy field is required."
    When I select water constituent "one" from the WaterConstituent dropdown
    And I enter today's date into the SampleDate field
    And I enter "asdf" into the SampleValue field
    And I enter "asdf" into the AnalysisPerformedBy field
    And I press "Save Sample Result"
    Then I should be at the Show page for water quality complaint: "one"
    When I click the "Sample Results" tab
    Then I should see "asdf" in the table sampleResultsTable's "Sample Value" column
    When I click the "Edit" link in the 1st row of sampleResultsTable
    And I enter "fdsa" into the SampleValue field
    And I press "Save"
    Then I should be at the Show page for water quality complaint: "one"
    When I click the "Sample Results" tab
    Then I should see "fdsa" in the table sampleResultsTable's "Sample Value" column
    When I click ok in the dialog after pressing "Remove"
    And I wait for the page to reload
    Then I should be at the Show page for water quality complaint: "one"
    When I click the "Sample Results" tab
    Then I should not see "fdsa" in the table sampleResultsTable's "Sample Value" column
	
Scenario: user should see SAP features when record has SAP Notification that is not complete or cancelled
    Given the test flag "show valid sap notification" exists
    And a water quality complaint "sap" exists with state: "one", town: "one", operating center: "nj7", orcom order number: "123456"
    And I am at the show page for water quality complaint "sap"
    Then I should see the button "Cancel SAP Notification"
    And I should see the button "Complete SAP Notification"
    And I should see the link "Create Work Order"
    When I press "Cancel SAP Notification"
    Then I should see "Cancel SAP Notification?"
    And I should see the button "Cancel Notification"
    When I press "sap-dialog-cancel-button"
    And I press "Complete SAP Notification"
    Then I should see "Complete SAP Notification?"
    And I should see the button "Complete Notification"

Scenario: user should NOT see SAP features when record is not associated with SAP
    Given a water quality complaint "sap" exists with state: "one", town: "one", operating center: "nj7"
    And I am at the show page for water quality complaint "sap"
    Then I should not see the button "Cancel SAP Notification"
    And I should not see the button "Complete SAP Notification"
    And I should not see the link "Create Work Order"

Scenario: user should NOT see SAP features when SAP record is cancelled or completed or not found
    Given the test flag "show not found sap notification" exists
    And a water quality complaint "sap" exists with state: "one", town: "one", operating center: "nj7", orcom order number: "123456"
    And I am at the show page for water quality complaint "sap"
    Then I should not see the button "Cancel SAP Notification"
    And I should not see the button "Complete SAP Notification"
    And I should not see the link "Create Work Order"

Scenario: user can search for their first and last name and select in Initial Local Contact autocomplete field
    When I visit the WaterQuality/WaterQualityComplaint/Search page
    And I select state "one" from the State dropdown
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I enter "last" and select employee "three"'s Description from the InitialLocalContact autocomplete field
    And I enter "admin" and select employee "three"'s Description from the InitialLocalContact autocomplete field
    And I press Search
    Then I should see the following values in the waterQualityComplaints table
      | Complaint Number | Initial Local Contact | Operating Center |
      | 3                | admin last            | QQ7 - Shrewsbury |
    And I should see "Records Found: 1"
 
Scenario: user can view the work order link when a water quality complaint has a related work order
    Given a water quality complaint "waterqualitycomplaintwithworkorder" exists with state: "one", town: "one", operating center: "nj7", orcom order number: "1234567"
    And a work order "waterqualityworkorder" exists with sap notification number: "1234567"
    And I am at the show page for water quality complaint "waterqualitycomplaintwithworkorder"
    Then I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order: "waterqualityworkorder"

Scenario: user does not see a work order link when a water quality complaint does not have a related work order
        Given a water quality complaint "waterqualitycomplaintwithoutworkorder" exists with state: "one", town: "one", operating center: "nj7", orcom order number: "1234567"
        And a work order "workorder1" exists with sap notification number: "12345679"
        And I am at the show page for water quality complaint "waterqualitycomplaintwithoutworkorder"
        Then I should not see a link to the FieldOperations/GeneralWorkOrder/Show page for work order: "workorder1"