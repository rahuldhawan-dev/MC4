Feature: EndOfPipeExceedancePage

Background: AllTheThings
	Given an admin user "admin" exists with username: "admin"
	And a state "nj" exists with abbreviation: "NJ"
	And a state "pa" exists with abbreviation: "PA"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", State: "nj"
	And an operating center "pa1" exists with opcode: "PA1", name: "Not Shrewsbury", state: "pa"
	And an employee status "active" exists with description: "Active"
	And a user "user" exists with username: "user"
	And a role "roleRead" exists with action: "Read", module: "EnvironmentalGeneral", user: "user", operating center: "nj7"
	And a role "roleEdit" exists with action: "Edit", module: "EnvironmentalGeneral", user: "user", operating center: "nj7"
	And a role "roleAdd" exists with action: "Add", module: "EnvironmentalGeneral", user: "user", operating center: "nj7"
	And a role "roleDelete" exists with action: "Delete", module: "EnvironmentalGeneral", user: "user", operating center: "nj7"
	And a waste water system "one" exists with id: 1, waste water system name: "Water System 1", operating center: "nj7"
	And a waste water system "two" exists with operating center: "pa1"
	And a waste water system "three" exists with operating center: "nj7"
	And a facility "one" exists with operating center: "nj7"
	And a facility "two" exists with operating center: "pa1"
	And end of pipe exceedance types exist
	And end of pipe exceedance root causes exist
	And an end of pipe exceedance "one" exists with state: "nj", operating center: "nj7", waste water system: "one"
	And an end of pipe exceedance "two" exists with state: "pa", operating center: "pa1", waste water system: "two"
	And an end of pipe exceedance "three" exists with state: "nj", operating center: "nj7", waste water system: "three"
	And a limitation type "one" exists with description: "daily"

Scenario: user can see waste water system dropdown description
	Given I am logged in as "user"
	And I am at the Environmental/EndOfPipeExceedance/Search page
	When I select state "nj" from the State dropdown
	And I wait for ajax to finish loading
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I wait for ajax to finish loading	
	Then I should see "NJ7WW0001 - Water System 1" in the WasteWaterSystem dropdown

Scenario: user can see waste water system dropdown description while editing
	Given I am logged in as "user"
	And I am at the Show page for end of pipe exceedance: "one"
	When I follow "Edit"
	And I select state "nj" from the State dropdown
	And I wait for ajax to finish loading
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I wait for ajax to finish loading	
	Then I should see "NJ7WW0001 - Water System 1" in the WasteWaterSystem dropdown

Scenario: End of pipe exceedance can be created By Admin
	Given I am logged in as "admin"
	And I am at the Environmental/EndOfPipeExceedance/New page
	When I press Save
	Then I should see a validation message for State with "The State field is required."
	When I select state "nj" from the State dropdown
	And I press Save
	Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
	When I select operating center "nj7" from the OperatingCenter dropdown 
	And I press Save
	Then I should see a validation message for WasteWaterSystem with "The Wastewater System field is required."
	And I should see a validation message for EventDate with "The EventDate field is required."
	And I should see a validation message for EndOfPipeExceedanceType with "The EndOfPipeExceedanceType field is required."
	And I should see a validation message for EndOfPipeExceedanceRootCause with "The EndOfPipeExceedanceRootCause field is required."
	And I should see a validation message for ConsentOrder with "The ConsentOrder field is required."
	And I should see a validation message for NewAcquisition with "The NewAcquisition field is required."
	And I should see a validation message for BriefDescription with "The BriefDescription field is required."
	And I should see a validation message for LimitationType with "The LimitationType field is required."
	When I select waste water system "one"'s Description from the WasteWaterSystem dropdown
	And I enter today's date into the EventDate field 
	And I select "Ammonia" from the EndOfPipeExceedanceType dropdown
	And I select "Other" from the EndOfPipeExceedanceRootCause dropdown
	And I select "Yes" from the ConsentOrder dropdown
	And I select "Yes" from the NewAcquisition dropdown
	And I enter "things" into the BriefDescription field
	And I select limitation type "one" from the LimitationType dropdown
	And I press Save 
	Then I should see a validation message for EndOfPipeExceedanceRootCauseOtherReason with "The EndOfPipeExceedanceRootCauseOtherReason field is required."
	When I enter "I Tried To Make Ramen In The Coffee Pot And I Broke Everything" into the EndOfPipeExceedanceRootCauseOtherReason field
	And I press Save
	Then the currently shown end of pipe exceedance shall henceforth be known throughout the land as "lolz"
	And I should be at the show page for end of pipe exceedance "lolz"
	And I should see a display for EventDate with today's date
	And I should see a display for WasteWaterSystem with waste water system "one"
	And I should see a display for State with state "nj"
	And I should see a display for OperatingCenter with operating center "nj7"
	And I should see a display for EndOfPipeExceedanceType with "Ammonia"
	And I should see a display for EndOfPipeExceedanceRootCause with "Other"
	And I should see a display for EndOfPipeExceedanceRootCauseOtherReason with "I Tried To Make Ramen In The Coffee Pot And I Broke Everything"
	And I should see a display for ConsentOrder with "Yes"
	And I should see a display for NewAcquisition with "Yes"
	And I should see a display for BriefDescription with "things"
	And I should see a display for LimitationType with limitation type "one"

Scenario: End of pipe exceedance can be created by user
	Given I am logged in as "user"
	And I am at the Environmental/EndOfPipeExceedance/New page
	When I press Save
	Then I should see a validation message for State with "The State field is required."
	When I select state "nj" from the State dropdown
	And I press Save
	Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
	When I select operating center "nj7" from the OperatingCenter dropdown 
	And I press Save
	Then I should see a validation message for WasteWaterSystem with "The Wastewater System field is required."
	And I should see a validation message for EventDate with "The EventDate field is required."
	And I should see a validation message for EndOfPipeExceedanceType with "The EndOfPipeExceedanceType field is required."
	And I should see a validation message for EndOfPipeExceedanceRootCause with "The EndOfPipeExceedanceRootCause field is required."
	And I should see a validation message for ConsentOrder with "The ConsentOrder field is required."
	And I should see a validation message for NewAcquisition with "The NewAcquisition field is required."
	And I should see a validation message for BriefDescription with "The BriefDescription field is required."
	And I should see a validation message for LimitationType with "The LimitationType field is required."
	And I should see "NJ7WW0001 - Water System 1" in the WasteWaterSystem dropdown
	When I select waste water system "one"'s Description from the WasteWaterSystem dropdown
	And I enter today's date into the EventDate field 
	And I select "Ammonia" from the EndOfPipeExceedanceType dropdown
	And I select "Other" from the EndOfPipeExceedanceRootCause dropdown
	And I select "Yes" from the ConsentOrder dropdown
	And I select "Yes" from the NewAcquisition dropdown
	And I enter "things" into the BriefDescription field
	And I select limitation type "one" from the LimitationType dropdown
	And I press Save
	Then I should see a validation message for EndOfPipeExceedanceRootCauseOtherReason with "The EndOfPipeExceedanceRootCauseOtherReason field is required."
	When I enter "I Tried To Make Ramen In The Coffee Pot And I Broke Everything" into the EndOfPipeExceedanceRootCauseOtherReason field
	And I press Save
	Then the currently shown end of pipe exceedance shall henceforth be known throughout the land as "lolz"
	And I should be at the show page for end of pipe exceedance "lolz"
	And I should see a display for EventDate with today's date
	And I should see a display for WasteWaterSystem with waste water system "one"
	And I should see a display for State with state "nj"
	And I should see a display for OperatingCenter with operating center "nj7"
	And I should see a display for EndOfPipeExceedanceType with "Ammonia"
	And I should see a display for EndOfPipeExceedanceRootCause with "Other"
	And I should see a display for EndOfPipeExceedanceRootCauseOtherReason with "I Tried To Make Ramen In The Coffee Pot And I Broke Everything"
	And I should see a display for ConsentOrder with "Yes"
	And I should see a display for NewAcquisition with "Yes"
	And I should see a display for BriefDescription with "things"
	And I should see a display for LimitationType with limitation type "one"

Scenario: End of pipe exceedance can be edited by admin
	Given I am logged in as "admin"
	And I am at the Show page for end of pipe exceedance: "two"
	When I follow "Edit"
	And I enter "09/25/1990" into the EventDate field
	And I press Save
	Then I should be at the show page for end of pipe exceedance "two"
	And I should see a display for EventDate with "9/25/1990"

Scenario: End of pipe exceedance can be edited by user
	Given I am logged in as "user"
	And I am at the Show page for end of pipe exceedance: "three"
	When I follow "Edit"
	And I enter "09/25/1990" into the EventDate field
	And I press Save
	Then I should be at the show page for end of pipe exceedance "three"
	And I should see a display for EventDate with "9/25/1990"

Scenario: user should only be able to search within operating centers they have access to
	Given I am logged in as "user"
	And I am at the Environmental/EndOfPipeExceedance/Search page
	When I press "Search"
	Then I should be at the Environmental/EndOfPipeExceedance page
	And I should see a link to the show page for end of pipe exceedance "one"
	And I should not see a link to the show page for end of pipe exceedance "two"
	And I should see a link to the show page for end of pipe exceedance "three"

Scenario: admin should only be able to search all records
	Given I am logged in as "admin"
	And I am at the Environmental/EndOfPipeExceedance/Search page
	When I press "Search"
	Then I should be at the Environmental/EndOfPipeExceedance page
	And I should see a link to the show page for end of pipe exceedance "one"
	And I should see a link to the show page for end of pipe exceedance "two"
	And I should see a link to the show page for end of pipe exceedance "three"
