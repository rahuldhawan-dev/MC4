Feature: Training Requirement

Background: 
	Given an admin user "admin" exists with username: "admin"

Scenario: Admin sees validation errors when creating a training requirement
	Given I am logged in as "admin"
	And I am at the TrainingRequirement/New page
	When I press Save
	Then I should see the validation message "The Description field is required."
	And I should not see the validation message "The TrainingFrequency field is required."
	And I should see the validation message "The IsActive field is required."
	And I should see the validation message "The Is DOT Requirement field is required."
	And I should see the validation message "The Is OSHA Requirement field is required."
	And I should see the validation message "The Is DPCC Requirement field is required."
	And I should see the validation message "The Is PSM / TCPA Requirement field is required."
	And I should see the validation message "The IsProductionRequirement field is required."
	And I should see the validation message "The IsFieldOperationsRequirement field is required."
	
Scenario: Admin can create and edit a training requirement
	Given I am logged in as "admin"
	And I am at the TrainingRequirement/New page
	When I enter "A description" into the Description field
	And I enter "365" into the TrainingFrequency_Frequency field
	And I select "Day" from the TrainingFrequency_Unit dropdown 
	And I select "Yes" from the IsActive dropdown
	And I select "Yes" from the IsDOTRequirement dropdown
	And I select "No" from the IsOSHARequirement dropdown
	And I select "Yes" from the IsDPCCRequirement dropdown
	And I select "Yes" from the IsPSMTCPARequirement dropdown
	And I select "Yes" from the IsProductionRequirement dropdown
	And I select "Yes" from the IsFieldOperationsRequirement dropdown
	And I press Save
	Then I should see a display for Description with "A description"
	And I should see a display for TrainingFrequencyObj with "365 Days"
	And I should see a display for IsActive with "Yes"
	And I should see a display for IsDOTRequirement with "Yes"
	And I should see a display for IsOSHARequirement with "No"
	And I should see a display for IsDPCCRequirement with "Yes"
	And I should see a display for IsPSMTCPARequirement with "Yes"
	And I should see a display for IsProductionRequirement with "Yes"
	And I should see a display for IsFieldOperationsRequirement with "Yes"
	When I follow "Edit"
	And I enter "A different description" into the Description field
	And I enter "13" into the TrainingFrequency_Frequency field
	And I select "Month" from the TrainingFrequency_Unit dropdown
	And I select "No" from the IsActive dropdown
	And I select "No" from the IsDOTRequirement dropdown
	And I select "No" from the IsDPCCRequirement dropdown
	And I select "No" from the IsPSMTCPARequirement dropdown
	And I select "No" from the IsProductionRequirement dropdown
	And I select "No" from the IsFieldOperationsRequirement dropdown
	And I press Save
	Then I should see a display for Description with "A different description"
	And I should see a display for TrainingFrequencyObj with "13 Months"
	And I should see a display for IsActive with "No"
	And I should see a display for IsDOTRequirement with "No"
	And I should see a display for IsOSHARequirement with "No"
	And I should see a display for IsDPCCRequirement with "No"
	And I should see a display for IsPSMTCPARequirement with "No"
	And I should see a display for IsProductionRequirement with "No"
	And I should see a display for IsFieldOperationsRequirement with "No"

Scenario: user can add link position group common names to training requirement
	Given a training requirement "one" exists
	And a user "user" exists with username: "user"
	And a town "lazytown" exists
	And a role "roleAdd" exists with action: "Add", module: "OperationsTrainingModules", user: "user"
	And a role "roleRead" exists with action: "Read", module: "OperationsTrainingModules", user: "user"
	And a role "roleEdit" exists with action: "Edit", module: "OperationsTrainingModules", user: "user"
	And a position group common name "one" exists with description: "Surveyor"
	And a position group common name "two" exists with description: "Engineering Supervisor"
	And I am logged in as "user"
	When I visit the Show page for training requirement: "one"
	And I click the "Position Group Common Names" tab
	And I press "Add Position Group Common Name"
	And I select "Engineering Supervisor" from the PositionGroupCommonName dropdown
	And I press add-common-name
	And I wait for the page to reload
	Then I should be at the Show page for training requirement: "one" on the PositionGroupCommonNames tab
	When I click the "Position Group Common Names" tab
	Then I should see the following values in the position-group-common-names-table table
	| Description            |
	| Engineering Supervisor |

Scenario: mcuser cannot add a position group common name to training requirement
	Given a training requirement "one" exists
	And a user "someuser" exists with username: "someuser"
	And a role "roleRead" exists with action: "Read", module: "OperationsTrainingModules", user: "someuser"
	And a position group common name "one" exists with description: "Surveyor"
	And a position group common name "two" exists with description: "Engineering Supervisor"
	And position group common name: "one" exists in training requirement: "one"
	And I am logged in as "someuser"
	When I visit the Show page for training requirement: "one"
	And I click the "Position Group Common Names" tab
	Then I should not see the button "Add Position Group Common Name"
	And I should not see the button "Remove"
	
Scenario: before saving a training requirement with an osha requirement, one must chose an osha standard
    Given a training requirement "one" exists
    And an osha standard "one" exists
    And a regulation "one" exists
    And I am logged in as "admin"
    And I am at the edit page for training requirement: "one"
    When I select "Yes" from the IsOSHARequirement dropdown
    And I press Save
    Then I should be at the edit page for training requirement: "one"
    And I should see the validation message "The OSHA Standard field is required for OSHA-required training."
    And I should not see regulation "one"'s OSHADescription in the OSHAStandard dropdown
    When I select osha standard "one"'s OSHADescription from the OSHAStandard dropdown
    And I press Save
    Then I should be at the show page for training requirement: "one"
    And I should see a display for OSHAStandard_OSHADescription with osha standard "one"'s OSHADescription