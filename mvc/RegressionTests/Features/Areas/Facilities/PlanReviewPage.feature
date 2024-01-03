Feature: PlanReviewPage

Background: data exists
    Given a state "one" exists with abbreviation: "NJ"
    And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", state: "one"
	And a facility "one" exists with operating center: "nj7"
	And an emergency plan category "one" exists with description: "epc one"
	And a review frequency "one" exists with description: "rf one"
	And a emergency response plan "one" exists with Id: "200"
    And a emergency response plan "two" exists
	And a plan review "one" exists with emergency response plan: "one", review change notes: "some note"
	And an employee "nj7-1" exists with operating center: "nj7", first name: "Flip", last name: "Lighter", employee id: "2000001"
	And an employee "nj7-2" exists with operating center: "nj7", first name: "Johnny", last name: "Hotdog", employee id: "2000002"
	And an admin user "admin" exists with username: "admin", full name: "Graham Crunchy", employee: "nj7-2"
	And I am logged in as "admin"

Scenario: user can search for a plan review
    When I visit the Facilities/PlanReview/Search page
    And I press Search
    Then I should see a link to the Show page for plan review: "one"
    When I follow the Show link for plan review "one"
    Then I should be at the Show page for plan review: "one"

Scenario: user can access the emergency response plan from the plan review
	Given I am logged in as "admin"
	And a plan review "three" exists with plan: "one", review change notes: "some note"
	When I visit the Facilities/PlanReview/Search page
	And I press Search
	Then I should see a link to the Show page for plan review: "three"
	When I follow the Show link for plan review "three"
	Then I should be at the Show page for plan review: "three"
	When I follow "1"
	Then I should be at the Show page for emergency response plan: "one"

Scenario: user can create a plan review
    Given I am logged in as "admin"
	When I visit the Show page for emergency response plan: "one"
	Then I should see a display for emergency response plan: "one"'s Description
	When I click the "Review" tab
	And I follow "Add Review"
	And I wait for the page to reload
	And I enter "4/24/2020" into the ReviewDate field
	And I enter "ligh" and select "Lighter, Flip - 2000001 - NJ7" from the ReviewedBy autocomplete field
	And I press "Save"
	Then I should see the validation message "The ReviewChangeNotes field is required."
	And I should see the validation message "The NextReviewDate field is required."
	When I enter "The plan was reviewed" into the ReviewChangeNotes field
	And I enter "4/24/2024" into the NextReviewDate field
	And I press "Save"
	Then I should see a link to the Show page for emergency response plan: "one"
	And I should see a display for ReviewDate with "4/24/2020"
	And I should see a display for ReviewedBy with "Flip Lighter"
	And I should see a display for ReviewChangeNotes with "The plan was reviewed"
	And I should see a display for NextReviewDate with "4/24/2024"
	And I should see a display for CreatedBy_FullName with "Graham Crunchy"
	And I should see a display for CreatedAt with today's date

Scenario: user can edit a plan review
    When I visit the Edit page for plan review: "one"
	And I enter "8/24/2020" into the ReviewDate field
	And I enter "8/24/2028" into the NextReviewDate field
	And I enter "tdog" and select "Hotdog, Johnny - 2000002 - NJ7" from the ReviewedBy autocomplete field
	And I enter "" into the ReviewChangeNotes field
	And I press Save
	Then I should see the validation message "The ReviewChangeNotes field is required."
	When I enter "the review was done" into the ReviewChangeNotes field
	And I press Save
	Then I should see a display for ReviewChangeNotes with "the review was done"

Scenario: Plan Review Add should not be visible in the action bar
	Given I am logged in as "admin"
	And I am at the Facilities/PlanReview/Search page
	Then I should not see the "new" button in the action bar
