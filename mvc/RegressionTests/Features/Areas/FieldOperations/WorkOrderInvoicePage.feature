Feature: WorkOrderInvoicePage

Background: data exists
	# Fun fact: If you create a user *before* an operating center, and you're specifically using
	# operating center code "nj7" then you will end up with the operating center instance that
	# CreateUser would use by default(which is NJ7) and it will not have any of the specific
	# test values that you tried to set.
    Given an admin user "admin" exists with username: "admin"
	# This OperatingCenter must have a permitsOmUserName set to an email address that's 
	# setup as an account on permits.mapcall.info. 
    And an operating center "xx1" exists with opcode: "XX1", name: "XX1 OPC", permitsOmUserName: "ross.dickinson@amwater.com"
	And a work order "one" exists with date completed: "8/12/2019", s o p required: "true", operating center: "xx1"
	And a schedule of value type "one" exists with description: "Time & Materials"
	And a schedule of value type "two" exists with description: "Unit Cost"
	And a schedule of value category "one" exists with description: "Labor", schedule of value type: "one"
	And a schedule of value "one" exists with labor unit cost: "125.50", schedule of value category: "one", description: "Foreman"
	And a work order schedule of value "one" exists with work order: "one", schedule of value: "one", total: 8, labor unit cost: "125"
	And a work order invoice status "pending" exists with description: "Pending"
	And a work order invoice status "submitted" exists with description: "Submitted"
	And a work order invoice status "canceled" exists with description: "Canceled"
	And a work order invoice "one" exists with schedule of value type: "one"
    And a work order invoice "two" exists
    And I am logged in as "admin"

Scenario: user can add a work order invoice for an existing work order
	When I visit the FieldOperations/WorkOrderInvoice/New/1 page
	Then I should see "1" in the WorkOrder field
	When I enter today's date into the InvoiceDate field
	And I select schedule of value type "one" from the ScheduleOfValueType dropdown
	And I press Save
	Then the currently shown work order invoice will now be referred to as "mel b"
	When I click the "Schedule of Values" tab
	Then I should see the following values in the schedule-of-values-table table
         | Schedule Of Value | Total | Unit Price | Total Price |
         | Foreman           | 8     | $158.13    | $1,265.04   |
	
Scenario: user can add a work order invoice for a work order
    When I visit the FieldOperations/WorkOrderInvoice/New page
    And I enter "666" into the WorkOrder field
    And I press Save
	Then I should see the validation message "The InvoiceDate field is required."
	And I should see the validation message "The ScheduleOfValueType field is required."
	When I enter today's date into the InvoiceDate field
	And I select schedule of value type "one" from the ScheduleOfValueType dropdown
	And I press Save
	Then I should see the validation message "The WorkOrderID does not exist. Please remove it or enter a valid WorkOrderID"
	When I enter work order "one"'s Id into the WorkOrder field
    And I press Save
    Then the currently shown work order invoice will now be referred to as "sally sparrow"
	And I should see a display for InvoiceDate with today's date
	And I should see a display for IncludeMaterials with "No"
	And I should see a link to the Show page for work order: "one"
	When I click the "Schedule of Values" tab
	And I press "Add Schedule of Value"
	And I select schedule of value category "one" from the ScheduleOfValueCategory dropdown
	And I wait for ajax to finish loading
	And I select schedule of value "one" from the ScheduleOfValue dropdown
	And I enter "3" into the Total field
	And I select "Yes" from the IncludeWithInvoice dropdown
	And I press "Save Schedule Of Value"
	And I wait for ajax to finish loading
	Then I should see the following values in the schedule-of-values-table table
         | Schedule Of Value | Total |
         | Foreman           | 8     |
         | Foreman           | 3     |
	When I click the "Schedule of Values" tab
	And I press "Add Schedule of Value"
	And I click the "Remove" button in the 1st row of schedule-of-values-table and then click ok in the confirmation dialog
	And I wait for the page to reload
	And I wait for ajax to finish loading
	Then I should see the following values in the schedule-of-values-table table
         | Schedule Of Value | Total |
         | Foreman           | 3     |	
	And I shant see "8" in the table schedule-of-values-table's "Total" column

Scenario: user can add a work order invoice without a work order
    When I visit the FieldOperations/WorkOrderInvoice/New page
    And I press Save
	Then I should see the validation message "The InvoiceDate field is required."
	And I should see the validation message "The ScheduleOfValueType field is required."
	When I enter today's date into the InvoiceDate field
	And I select schedule of value type "one" from the ScheduleOfValueType dropdown
	# If you're suddenly unable to check this checkbox due to some visibility thing, it's probably
	# because the datepicker is stuck open for some reason.
	And I check the IncludeMaterials field
	And I press Save
    Then the currently shown work order invoice will now be referred to as "simone"
    And I should see a display for InvoiceDate with today's date
	And I should see a display for IncludeMaterials with "Yes"

Scenario: user can search for a work order invoice
    When I visit the /FieldOperations/WorkOrderInvoice/Search page
    And I press Search
    Then I should see a link to the Show page for work order invoice: "one"
    When I follow the Show link for work order invoice "one"
    Then I should be at the Show page for work order invoice: "one"

Scenario: user can view a work order invoice
    When I visit the Show page for work order invoice: "one"
    Then I should see a display for work order invoice: "one"'s Id

Scenario: user can edit a work order invoice
    When I visit the Edit page for work order invoice: "one"
    And I select schedule of value type "two" from the ScheduleOfValueType dropdown
	And I enter today's date into the InvoiceDate field
    And I press Save
    Then I should be at the Show page for work order invoice: "one"
    And I should see a display for ScheduleOfValueType with schedule of value type "two"'s Description
	And I should see a display for InvoiceDate with today's date