Feature: BacterialWaterSamplePage

Background: data exists
    Given a user "user" exists with username: "user"
	And an operating center "nj7" exists with opcode: "NJ7"
	And a role "wqgeneral-useradmin" exists with action: "UserAdministrator", module: "WaterQualityGeneral", user: "user", operating center: "nj7"
	And a bacterial sample type "routine" exists with description: "Routine"
	And a bacterial sample type "repeat" exists with description: "Repeat"
	And a bacterial sample type "shipping blank" exists with description: "Shipping Blank"
	And a town "one" exists with name: "Townie"
	And operating center: "nj7" exists in town: "one" with abbreviation: "LA"
	And public water supply statuses exist
	And a public water supply "one" exists with op code: "NJ7", status: "active"
	And a sample site status "active" exists with description: "Active"
	And a sample site "one" exists with town: "one", sample site name: "Grateful Deli", public water supply: "one", sample site status: "active", bacti site: "true", operating center: "nj7"
	And a sample id matrix "one" exists with sample site: "one"
	And a l i m s status "ready" exists with description: "Ready to Send"
    And a bacterial water sample "one" exists with sample site: "one", operating center: "nj7"
    And a bacterial water sample "two" exists
    And I am logged in as "user"

Scenario: user can search for a bacterial water sample
    When I visit the WaterQuality/BacterialWaterSample/Search page
    And I press Search
    Then I should see a link to the Show page for bacterial water sample: "one"
    When I follow one of the Show links for bacterial water sample "one"
    Then I should be at the Show page for bacterial water sample: "one"

Scenario: user can inline-edit a bacterial water sample
    When I visit the WaterQuality/BacterialWaterSample/Index page
    And I follow one of the InlineEdit links for bacterial water sample "one"
    And I wait for ajax to finish loading
    And I enter 1 into the Nitrate field
	And I select "Yes" from the IsReadyForLIMS dropdown
    And I press "Save"
    And I wait for ajax to finish loading
    Then I should see "1" in the table bacterialWaterSamples's "Nitrate" column
	And I should see "Ready to Send" in the table bacterialWaterSamples's "LIMS Status" column

Scenario: user can view a bacterial water sample
    When I visit the Show page for bacterial water sample: "one"
	And I click the "Field Values" tab
    Then I should see a display for bacterial water sample: "one"'s Cl2Total

Scenario: user can add a bacterial water sample
    When I visit the WaterQuality/BacterialWaterSample/New page
    And I press Save
	Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
	When I select bacterial sample type "routine" from the BacterialSampleType dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I press Save
	Then I should see a validation message for SampleSite with "The SampleSite field is required."
	When I select bacterial sample type "repeat" from the BacterialSampleType dropdown
	And I enter "1235 Main st" into the Address field
	And I press Save
	Then I should see a validation message for SampleCoordinate with "The SampleCoordinate field is required."
	When I enter "" into the Address field
	And I select sample site "one"'s ToString from the SampleSite dropdown
	And I wait for ajax to finish loading
	And I press "Save"
	Then I should see a validation message for OriginalBacterialWaterSample with "The Original Bacterial Water Sample ID field is required."
	When I select bacterial sample type "shipping blank" from the BacterialSampleType dropdown
	And I click the "Field Values" tab
	And I enter "0.2" into the Cl2Free field
	And I enter "0.1" into the Cl2Total field
	And I press "Save"
	Then I should see a validation message for SampleCollectionDTM with "The SampleCollectionDTM field is required."
	When I enter today's date into the SampleCollectionDTM field
	And I press "Save"
	Then the currently shown bacterial water sample will now be referred to as "Rufford"
	And I should see a display for SampleSite with sample site "one"'s ToString    
	When I click the "Field Values" tab
	Then I should see a display for Cl2Free with "0.2"
	And I should see a display for Cl2Total with "0.1"

Scenario: user can edit a bacterial water sample
    When I visit the Edit page for bacterial water sample: "one"
	And I click the "Field Values" tab
    And I enter "0.2" into the Cl2Free field
	And I enter "0.1" into the Cl2Total field
    And I press Save
    Then I should be at the Show page for bacterial water sample: "one"
	When I click the "Field Values" tab
	Then I should see a display for Cl2Free with "0.2"
	And I should see a display for Cl2Total with "0.1"

Scenario: user can edit a bacterial water sample's sample lims status when it is not ready or ready to send
	# by default, the bws should already have a LIMS Status of "Not Ready"
    When I visit the Edit page for bacterial water sample: "one"
	And I click the "Bacterial Results" tab
	Then "No" should be selected in the IsReadyForLIMS dropdown
	When I select "Yes" from the IsReadyForLIMS dropdown 
	And I press Save 
	And I click the "Bacterial Results" tab 
	Then I should see a display for LIMSStatus with "Ready to Send"
	When I visit the Edit page for bacterial water sample: "one"
	And I click the "Bacterial Results" tab
	Then "Yes" should be selected in the IsReadyForLIMS dropdown
	When I select "No" from the IsReadyForLIMS dropdown 
	And I press Save 
	And I click the "Bacterial Results" tab 
	Then I should see a display for LIMSStatus with "Not Ready"

Scenario: user can not edit a bacterial water sample lims status when the sample has already been sent to lims
	Given a l i m s status "sentsuccess" exists with description: "Sent Successfully"
    And a bacterial water sample "three" exists with sample site: "one", operating center: "nj7", l i m s status: "sentsuccess"
    When I visit the Edit page for bacterial water sample: "three"
	And I click the "Bacterial Results" tab
	Then I should not see the IsReadyForLIMS field
	When I press Save 
	And I click the "Bacterial Results" tab 
	Then I should see a display for LIMSStatus with "Sent Successfully"

Scenario: site admin users should be able to see LIMS tab on bacterial water sample show page, regular users should not.
    Given an admin user "admin" exists with username: "admin"
	# Start off doing the normal user check check since we're already logged in as that user in the background
	When I visit the Show page for bacterial water sample: "one"
	Then I should not see the "LIMS" tab
	When I log in as "admin" 
	And I visit the Show page for bacterial water sample: "one"
	Then I should see the "LIMS" tab

Scenario: User should only see reason for invalidation when the sample is invalid
	Given a bacterial water sample reason for invalidation "reason" exists with description: "Because I said so"
	And a bacterial water sample "test" exists with sample site: "one", operating center: "nj7", is invalid: "true", reason for invalidation: "reason"
	# Check Show page for existing record that's already invalid. 
	When I visit the Show page for bacterial water sample: "test"
	And I click the "Bacterial Results" tab
	Then I should see a display for ReasonForInvalidation with "Because I said so"
	# Check Edit page toggles the fields correctly
	When I follow "Edit"
	And I click the "Bacterial Results" tab
	Then I should see the ReasonForInvalidation field
	When I uncheck the IsInvalid field
	Then I should not see the ReasonForInvalidation field
	# Check Show page again that it's not visible cause it's valid
	When I press Save
	And I click the "Bacterial Results" tab
	Then I should not see "Reason for Invalidation"
	# Check the New page toggles correctly too
	When I visit the WaterQuality/BacterialWaterSample/New page
	And I click the "Bacterial Results" tab
	Then I should not see the ReasonForInvalidation field 
	When I check the IsInvalid field
	Then I should see the ReasonForInvalidation field