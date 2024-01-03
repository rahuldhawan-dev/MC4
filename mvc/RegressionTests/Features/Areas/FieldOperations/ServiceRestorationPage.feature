Feature: ServiceRestorationPage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
	And a user "wk" exists with username: "Waco Kid"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a state "one" exists 
	And a town "one" exists with name: "Loch Arbour"
	And a town "two" exists with name: "Allenhurst"
	And operating center: "nj7" exists in town: "one" 
	And operating center: "nj7" exists in town: "two" 
	And a town section "one" exists with name: "A section", town: "two"
	And a street "one" exists with town: "one"
	And a street "two" exists with town: "two"
	And a service category "one" exists with description: "service category 1"
	And a service type "one" exists with operating center: "nj7", service category: "one", description: "Water NJ7"
	And a service "one" exists with operating center: "nj7", town: "one", service category: "one"
    And a service "two" exists with operating center: "nj7", service category: "one"
	And a restoration type "one" exists with description: "ASPHALT - ALLEY"
	And a restoration method "one" exists with description: "2 inch top"
	And a restoration method "two" exists with description: "infrared"
	And a service restoration contractor "one" exists with contractor: "Henkels", operating center: "nj7", final restoration: true, partial restoration: true
	And a service restoration contractor "two" exists with contractor: "Lafayette", operating center: "nj7", final restoration: true, partial restoration: true
    And a service restoration "one" exists with service: "one", initiated by: "wk"
    And a service restoration "two" exists with service: "two", initiated by: "wk"
    And I am logged in as "admin"

Scenario: user can search for a service restoration
    When I visit the FieldOperations/ServiceRestoration/Search page
    And I press Search
    Then I should see a link to the Show page for service restoration: "one"
    When I follow the Show link for service restoration "one"
    Then I should be at the Show page for service restoration: "one"

Scenario: user can view a service restoration
    When I visit the Show page for service restoration: "one"
    Then I should see a display for service restoration: "one"'s PurchaseOrderNumber

Scenario: user cannot just add a service restoration without a service
	When I visit the FieldOperations/ServiceRestoration/New page expecting an error
	Then I should see "Service with id could not be found. A restoration may only be created for an existing service." in the errMessage element

Scenario: user can edit a service restoration
    When I visit the show page for service restoration: "two"
	And I follow "Edit"
	And I select restoration type "one" from the RestorationType dropdown
	And I enter "5" into the EstimatedRestorationAmount field
	And I enter "6" into the EstimatedValue field
	And I check the Cancel field
	And I enter "7" into the PartialRestorationInvoiceNumber field
	And I enter "8" into the PartialRestorationAmount field
	And I enter "1/1/2016" into the PartialRestorationDate field
	And I select service restoration contractor "one" from the PartialRestorationCompletionBy dropdown
	And I enter "9" into the PartialRestorationCost field
	And I enter "9" into the PartialRestorationTrafficControlHours field
	And I select restoration method "one" from the PartialRestorationMethod dropdown
	And I enter "10" into the FinalRestorationInvoiceNumber field
	And I enter "11" into the FinalRestorationAmount field
	And I enter "11" into the FinalRestorationTrafficControlHours field
	And I enter "1/2/2016" into the FinalRestorationDate field
	And I select service restoration contractor "two" from the FinalRestorationCompletionBy dropdown
	And I enter "13" into the FinalRestorationCost field
	And I enter "14" into the FinalRestorationTrafficControlHours field
	And I select restoration method "two" from the FinalRestorationMethod dropdown
	And I enter "1/2/2016" into the ApprovedOn field
	And I enter "1/3/2016" into the RejectedOn field
	And I enter "16" into the PurchaseOrderNumber field
	And I enter "flerghensbergh" into the Notes field
	And I press Save
	Then I should see a display for RejectedOn with "1/3/2016"
	And I should see a display for RejectedBy with "admin"
	And I should see a display for ApprovedOn with "1/2/2016"
	And I should see a display for ApprovedBy with "admin"
	And I should see a display for PurchaseOrderNumber with "16"
	And I should see a display for FinalRestorationTrafficControlHours with "14"
	And I should see a display for FinalRestorationCost with "$13.00"
	And I should see a display for FinalRestorationMethod with restoration method "two"
	And I should see a display for FinalRestorationCompletionBy with service restoration contractor "two"
	And I should see a display for FinalRestorationDate with "1/2/2016"
	And I should see a display for FinalRestorationAmount with "$11.00"
	And I should see a display for FinalRestorationInvoiceNumber with "10"
	And I should see a display for PartialRestorationTrafficControlHours with "9"
	And I should see a display for PartialRestorationCost with "$9.00"
	And I should see a display for PartialRestorationMethod with restoration method "one"
	And I should see a display for PartialRestorationCompletionBy with service restoration contractor "one"
	And I should see a display for PartialRestorationDate with "1/1/2016"
	And I should see a display for PartialRestorationAmount with "8"
	And I should see a display for PartialRestorationInvoiceNumber with "7"
	And I should see a display for EstimatedValue with "$6.00"
	And I should see a display for EstimatedRestorationAmount with "5"
	And I should see a display for Cancel with "Yes"
	And I should see a display for InitiatedBy with user "wk"
	And I should see a display for RestorationType with restoration type "one"
	And I should see a display for Notes with "flerghensbergh"
	And I should see a link to the Show page for service "two"

Scenario: user can add a service restoration
    When I visit the FieldOperations/ServiceRestoration/New/2 page
	And I select restoration type "one" from the RestorationType dropdown
	And I enter "5" into the EstimatedRestorationAmount field
	And I enter "6" into the EstimatedValue field
	And I check the Cancel field
	And I enter "7" into the PartialRestorationInvoiceNumber field
	And I enter "8" into the PartialRestorationAmount field
	And I enter "1/1/2016" into the PartialRestorationDate field
	And I select service restoration contractor "one" from the PartialRestorationCompletionBy dropdown
	And I enter "9" into the PartialRestorationCost field
	And I enter "9" into the PartialRestorationTrafficControlHours field
	And I select restoration method "one" from the PartialRestorationMethod dropdown
	And I enter "10" into the FinalRestorationInvoiceNumber field
	And I enter "11" into the FinalRestorationAmount field
	And I enter "11" into the FinalRestorationTrafficControlHours field
	And I enter "1/2/2016" into the FinalRestorationDate field
	And I select service restoration contractor "two" from the FinalRestorationCompletionBy dropdown
	And I enter "13" into the FinalRestorationCost field
	And I enter "14" into the FinalRestorationTrafficControlHours field
	And I select restoration method "two" from the FinalRestorationMethod dropdown
	And I enter "1/2/2016" into the ApprovedOn field
	And I enter "1/3/2016" into the RejectedOn field
	And I enter "16" into the PurchaseOrderNumber field
	And I press Save
	Then I should see a display for RejectedOn with "1/3/2016"
	And I should see a display for RejectedBy with "admin"
	And I should see a display for ApprovedOn with "1/2/2016"
	And I should see a display for ApprovedBy with "admin"
	And I should see a display for PurchaseOrderNumber with "16"
	And I should see a display for FinalRestorationTrafficControlHours with "14"
	And I should see a display for FinalRestorationCost with "$13.00"
	And I should see a display for FinalRestorationMethod with restoration method "two"
	And I should see a display for FinalRestorationCompletionBy with service restoration contractor "two"
	And I should see a display for FinalRestorationDate with "1/2/2016"
	And I should see a display for FinalRestorationAmount with "$11.00"
	And I should see a display for FinalRestorationInvoiceNumber with "10"
	And I should see a display for PartialRestorationTrafficControlHours with "9"
	And I should see a display for PartialRestorationCost with "$9.00"
	And I should see a display for PartialRestorationMethod with restoration method "one"
	And I should see a display for PartialRestorationCompletionBy with service restoration contractor "one"
	And I should see a display for PartialRestorationDate with "1/1/2016"
	And I should see a display for PartialRestorationAmount with "8"
	And I should see a display for PartialRestorationInvoiceNumber with "7"
	And I should see a display for EstimatedValue with "$6.00"
	And I should see a display for EstimatedRestorationAmount with "5"
	And I should see a display for Cancel with "Yes"
	And I should see a display for InitiatedBy with admin user "admin"
	And I should see a display for RestorationType with restoration type "one"
	And I should see a link to the Show page for service "two"