Feature: FieldOperations/TrafficControlTicketTicketPage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
    And an operating center "one" exists
    And a town "one" exists with operating center: "one"
    And operating center: "one" exists in town: "one"	
    And a street "one" exists with town: "one", is active: true
    And an operating center "two" exists
    And a town "two" exists with operating center: "two"
    And operating center: "two" exists in town: "two"	
    And a street "two" exists with town: "two", is active: true
	And an address "addy" exists with town: "one"
	And a contact "contact" exists with first name: "Tina", last name: "Belcher", email: "buns@onburner.com", address: "addy"
	And a contact type "ct" exists with description: "yowzah!"
	And a billing party "one" exists
	And a billing party contact "tc" exists with contact: "contact", billing party: "one", contact type: "ct"
    And a work order "one" exists with operating center: "one", town: "one", street: "one"
    And a work order "two" exists with operating center: "two", town: "two", street: "two"
	And a merchant total fee "one" exists with fee: 0.034, is current: true
	And a traffic control ticket "one" exists with operating center: "one", town: "one", street: "one", work order: "one", invoice number: "1234", invoice amount: "12.50", invoice date: today, invoice total hours: "5", date approved: today, billing party: "one", merchant total fee: "one"
    And a traffic control ticket "two" exists with operating center: "two", town: "two", street: "two", work order: "two", merchant total fee: "one"
	And a traffic control ticket status "one" exists with description: "Open"
	And a traffic control ticket status "two" exists with description: "Awaiting Payment"
	And a traffic control ticket status "three" exists with description: "Pending Submittal"
	And a traffic control ticket status "four" exists with description: "Submitted"
	And a traffic control ticket status "five" exists with description: "Canceled"
	And a traffic control ticket status "six" exists with description: "Cleared"
	And a traffic control ticket status "seven" exists with description: "Paid"
	And a coordinate "one" exists
	And a notification purpose "one" exists with purpose: "Traffic Control Payment Submitted", module: "FieldServicesWorkManagement"

Scenario: user can search for a traffic control ticket
	Given I am logged in as "admin"
	When I visit the FieldOperations/TrafficControlTicket/Search page
    And I press Search
    Then I should see a link to the Show page for traffic control ticket: "one"
    And I should see a link to the Show page for traffic control ticket: "two"
    When I visit the FieldOperations/TrafficControlTicket/Search page
    And I select operating center "one" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I select town "one" from the Town dropdown
    And I press Search	
    Then I should see a link to the Show page for traffic control ticket: "one"
    And I should not see a link to the Show page for traffic control ticket: "two"
    When I visit the FieldOperations/TrafficControlTicket/Search page
	And I enter work order "two"'s Id into the WorkOrder field
    And I press Search 
    Then I should not see a link to the Show page for traffic control ticket: "one"
    And I should see a link to the Show page for traffic control ticket: "two"
    When I visit the FieldOperations/TrafficControlTicket/Search page
    And I select "Yes" from the HasInvoice dropdown
    And I press Search	
    Then I should see a link to the Show page for traffic control ticket: "one"
    And I should not see a link to the Show page for traffic control ticket: "two"

Scenario: user can view a traffic control ticket
	Given I am logged in as "admin"
    When I visit the Show page for traffic control ticket: "one"
    Then I should see a display for traffic control ticket: "one"'s SAPWorkOrderNumber

Scenario: user can add a traffic control ticket
	Given I am logged in as "admin"
    When I visit the FieldOperations/TrafficControlTicket/New page
    And I enter today's date into the WorkStartDate field
    And I select operating center "one" from the OperatingCenter dropdown
    And I enter "123" into the SAPWorkOrderNumber field
    And I wait for ajax to finish loading
    And I select town "one" from the Town dropdown
    And I wait for ajax to finish loading
    And I select street "one" from the Street dropdown
    And I enter 321 into the StreetNumber field	
    And I enter 3 into the TotalHours field
    And I enter 2 into the NumberOfOfficers field
    And I enter 12343210 into the AccountingCode field
	And I enter coordinate "one"'s Id into the Coordinate field
	And I enter billing party "one"'s Description and select billing party "one"'s Description from the BillingParty combobox
	And I wait for ajax to finish loading
    And I press Save
    Then the currently shown traffic control ticket will now be referred to as "new"
    And I should see a display for SAPWorkOrderNumber with "123"

Scenario: user can edit a traffic control ticket
	Given I am logged in as "admin"
	When I visit the Show page for traffic control ticket: "one"
	Then I should see a link to the show page for billing party "one"
    When I visit the Edit page for traffic control ticket: "one"
    And I enter "321" into the SAPWorkOrderNumber field
    And I press Save
    Then I should be at the Show page for traffic control ticket: "one"
    And I should see a display for SAPWorkOrderNumber with "321"
	Then I should see a link to the show page for billing party "one"

Scenario: user cannot set any of the invoice properties without setting all of them
	Given I am logged in as "admin"
    When I visit the Edit page for traffic control ticket: "two"
    And I enter "123" into the InvoiceNumber field
	And I press Save
	Then I should be at the Edit page for traffic control ticket: "two"
	And I should see the validation message "The InvoiceAmount field is required."
	And I should see the validation message "The InvoiceDate field is required."
    When I visit the Edit page for traffic control ticket: "two"
    And I enter "123" into the InvoiceAmount field
	And I press Save
	Then I should be at the Edit page for traffic control ticket: "two"
	And I should see the validation message "The InvoiceNumber field is required."
	And I should see the validation message "The InvoiceDate field is required."
    When I visit the Edit page for traffic control ticket: "two"
    And I enter today's date into the InvoiceDate field
	And I press Save
	Then I should be at the Edit page for traffic control ticket: "two"
	And I should see the validation message "The InvoiceNumber field is required."
	And I should see the validation message "The InvoiceAmount field is required."

@cleanup_users
Scenario: user can pay for an invoiced traffic control ticket
	Given a user "otheruser" exists with username: "otheruser", special_email: "user@mapcall.com", password: "testpassword", company: "company", has profileId: true, credit card number: "4012888818888"
	And a role "roleRead" exists with action: "Read", module: "FieldServicesWorkManagement", user: "otheruser"
	And a role "roleEdit" exists with action: "Edit", module: "FieldServicesWorkManagement", user: "otheruser"
	And I am logged in as "otheruser"
	When I visit the Show page for traffic control ticket: "one"
	And I follow "Begin Payment"
	And I wait for ajax to finish loading
    And I click ok in the dialog after pressing "Complete Payment"
    And I wait for ajax to finish loading
	#skipping the verification note, due to notifications, notifications already tested in controller test
    And I visit the show page for traffic control ticket: "one"
    Then the PaymentTransactionId value for traffic control ticket "one" should not be null
    And the PaymentAuthorizationCode value for traffic control ticket "one" should not be null
    And the PaymentReceivedAt value for traffic control ticket "one" should not be null
	And I should see a display for TotalCharged with "$25.33"
	And I should see a display for MTOTFee with "$0.83"
	
@cleanup_users
Scenario: user can get rejected trying to pay for an invoiced traffic control ticket with a jcb card
	Given a user "otheruser" exists with username: "otheruser", special_email: "user@mapcall.com", password: "testpassword", company: "company", has profileId: true, credit card number: "370000000000002"
	And a role "roleRead" exists with action: "Read", module: "FieldServicesWorkManagement", user: "otheruser"
	And a role "roleEdit" exists with action: "Edit", module: "FieldServicesWorkManagement", user: "otheruser"
	And I am logged in as "otheruser"
	When I visit the Show page for traffic control ticket: "one"
	And I follow "Begin Payment"
	And I wait for ajax to finish loading
    And I click ok in the dialog after pressing "Complete Payment"
    And I wait for ajax to finish loading
    Then I should see "Payment Rejected"
	And I should see "The merchant does not accept this type of credit card"

Scenario: user cannot submit payment if credit card not setup
	Given a user "otheruser" exists with username: "otheruser", special_email: "user@mapcall.com", password: "testpassword", company: "company", has profileId: false
	And a role "roleRead" exists with action: "Read", module: "FieldServicesWorkManagement", user: "otheruser"
	And a role "roleEdit" exists with action: "Edit", module: "FieldServicesWorkManagement", user: "otheruser"
	And I am logged in as "otheruser"
	When I visit the Show page for traffic control ticket: "one"
	Then I should not see a link to the BeginPayment page for traffic control ticket "one"
	And I should see "You must have a credit card setup in order to submit payment for traffic control tickets"

Scenario: user administrator can enter check
	Given a traffic control ticket "paid" exists with operating center: "one", town: "one", street: "one", work order: "one", invoice number: "1234", invoice amount: "12.50", invoice date: today, invoice total hours: "5", date approved: today, billing party: "one", payment transaction id: "123465", payment received at: "today", payment authorization code: "ABC", payment profile id: "123"
	And I am logged in as "admin"
	When I visit the Show page for traffic control ticket: "paid"
	And I follow "Enter Check"
	And I enter "1234" into the CheckNumber field
	And I enter "ticket 1" into the Memo field
	And I press "Save"
	And I wait for the page to reload
	Then the currently shown traffic control ticket check shall henceforth be known throughout the land as "check one two, one two"
	And I should see a display for CheckNumber with "1234"
	And I should see a display for Memo with "ticket 1"
	And I should see a display for Amount with "$12.50"	

Scenario: user administrator can enter tracking number
	Given a traffic control ticket "paid" exists with operating center: "one", town: "one", street: "one", work order: "one", invoice number: "1234", invoice amount: "12.50", invoice date: today, invoice total hours: "5", date approved: today, billing party: "one", payment transaction id: "123465", payment received at: "today", payment authorization code: "ABC", payment profile id: "123"
	And a traffic control ticket check exists with traffic control ticket: "paid", amount: "12.50", check number: "1234"
	And I am logged in as "admin"
	When I visit the Show page for traffic control ticket: "paid"
	Then I should see "n/a" 
	When I follow "Edit"
	And I enter "123456567890" into the TrackingNumber field
	And I press "Save"
	And I wait for the page to reload
	Then I should see "123456567890"
	
Scenario: user administrator can enter mark as submitted
	Given a traffic control ticket "paid" exists with operating center: "one", town: "one", street: "one", work order: "one", invoice number: "1234", invoice amount: "12.50", invoice date: today, invoice total hours: "5", date approved: today, billing party: "one", payment transaction id: "123465", payment received at: "today", payment authorization code: "ABC", payment profile id: "123", tracking number: "1235", processing fee: "12.0", total charged: "25.33", m t o t fee: "0.83"
	And a traffic control ticket check exists with traffic control ticket: "paid", amount: "12.50", check number: "1234"
	And I am logged in as "admin"
	When I visit the Show page for traffic control ticket: "paid"
	And I press "Mark As Submitted"
	And I wait for the page to reload
	And I press "Reset"
	And I enter traffic control ticket "paid"'s Id into the EntityId field
	And I press "Search"
	And I follow the Show link for traffic control ticket "paid"
	Then I should see a display for SubmittedAt with today's date