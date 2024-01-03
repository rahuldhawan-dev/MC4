Feature: SamplePlanPage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
    And public water supply statuses exist
    And a public water supply "one" exists
    And an employee "one" exists
    And a sample plan "one" exists with p w s i d: "one", contact person: "one"
    And a sample plan "two" exists
    And I am logged in as "admin"

Scenario: user can search for a sample plan
    When I visit the WaterQuality/SamplePlan/Search page
    And I press Search
    Then I should see a link to the Show page for sample plan: "one"
    When I follow the Show link for sample plan "one"
    Then I should be at the Show page for sample plan: "one"

Scenario: user can view a sample plan
    When I visit the Show page for sample plan: "one"
    Then I should see a display for sample plan: "one"'s NameOfCertifiedLaboratory

Scenario: user can add a sample plan
    When I visit the WaterQuality/SamplePlan/New page 
    And I press Save
    Then I should be at the WaterQuality/SamplePlan/New page
    And I should see the validation message "The PWSID field is required."		
    And I should see the validation message "The ContactPerson field is required."		
    And I should see the validation message "The MonitoringPeriodFrom field is required."		
    And I should see the validation message "The MonitoringPeriodTo field is required."		
    And I should see the validation message "The MinimumSamplesRequired field is required."		
    And I should see the validation message "The NameOfCertifiedLaboratory field is required."		
    And I should see the validation message "The Are the same sampling sites used as in the previous monitoring period? If required by your state, complete and submit the appropriate Lead and Copper Change form for your state (i.e. NJ BSDW-56) field is required."		
    And I should see the validation message "The Are all samples from Tier 1 sites? field is required."		
    And I should see the validation message "The If insufficient Tier 1 sites are available, are Tier 2 sites used? field is required."		
    And I should see the validation message "The If insufficient Tier 2 sites are available, are Tier 3 sites used? field is required."		
    And I should see the validation message "The Have the Tier 1 sites been verified to meet the requirements of a Tier 1 site? (i.e. documentation that can be provided proving the site meets the requirements) field is required."		
    And I should see the validation message "The Does the system have lead service lines? If yes, write in comments section how many field is required."		
    And I should see the validation message "The Has the system verified which lines are lead service lines? (i.e. visual inspection, record drawings, county appraisal records, interviews with residents, etc.) field is required."		
    And I should see the validation message "The If the distribution system contains lead service lines, are 50% of the samples collected from sites with lead service lines? field is required."		
    When I select public water supply "one" from the PWSID dropdown
    And I select employee "one"'s Description from the ContactPerson dropdown
    And I enter today's date into the MonitoringPeriodFrom field
    And I enter today's date into the MonitoringPeriodTo field
    And I enter 12 into the MinimumSamplesRequired field
    And I enter "blah" into the NameOfCertifiedLaboratory field
    And I select "Yes" from the SameAsPreviousPeriod dropdown
    And I select "Yes" from the AllSamplesTier1 dropdown
    And I select "Yes" from the Tier2Sites dropdown
    And I select "Yes" from the Tier3Sites dropdown
    And I select "Yes" from the Tier1SitesVerified dropdown
    And I select "Yes" from the LeadServiceLines dropdown
    And I select "Yes" from the LeadLinesVerified dropdown
    And I select "Yes" from the FiftyPercent dropdown
    And I press Save
    Then the currently shown sample plan will now be referred to as "new"
    And I should see a display for NameOfCertifiedLaboratory with "blah"

Scenario: user can edit a sample plan
    When I visit the Edit page for sample plan: "one"
    And I enter "blah" into the NameOfCertifiedLaboratory field
    And I press Save
    Then I should be at the Show page for sample plan: "one"
    And I should see a display for NameOfCertifiedLaboratory with "blah"
