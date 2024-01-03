Feature: RecurringProjectPage

Background: data exists
    Given an admin user "admin" exists with username: "admin", full name: "Admin McAdminy"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a town abbreviation type "town" exists
	And a town "one" exists with name: "Loch Arbour"
	And operating center: "nj7" exists in town: "one" with abbreviation: "LA"
	And an asset category "one" exists with description: "asset category"
	And an asset type "facility" exists with description: "facility"
	And a recurring project type "one" exists with description: "no"
	And a pipe diameter "one" exists with diameter: "12.01"
	And a pipe material "one" exists with description: "Cast Iron"
	And an asset investment category "one" exists with description: "aic 1"
	And an asset investment category "two" exists with description: "aic 2"
	And a foundational filing period "one" exists with description: "ffp1"
	And a recurring project regulatory status "one" exists with description: "krs-one"
	And a recurring project status "proposed" exists with description: "proposed"
	And a recurring project status "complete" exists with description: "complete"
	And a recurring project status "ap approved" exists with description: "ap approved"
	And a coordinate "one" exists
    And a recurring project "one" exists with operating center: "nj7", status: "proposed"
    And a recurring project "two" exists with operating center: "nj7", status: "proposed"
	And a pipe data lookup type "one" exists with description: "Coordination with Others"
	And a pipe data lookup type "two" exists with description: "Insurance Claims"
	And a pipe data lookup value "one" exists with pipe data lookup type: "one", is default: true, is enabled: true, variable score: 10, priority weighted score: 1, description: "None"
	And a pipe data lookup value "two" exists with pipe data lookup type: "two", is default: true, is enabled: true, variable score: 10, priority weighted score: 10, description: "Time Sensitive Opportunity"
	And a pipe data lookup value "three" exists with pipe data lookup type: "one", is default: false, is enabled: false, variable score: 10, priority weighted score: 10, description: "Time Sensitive Opportunity"
	And a pipe data lookup value "four" exists with pipe data lookup type: "two", is default: false, is enabled: true, variable score: 10, priority weighted score: 10, description: "Time Sensitive Opportunity"
	And an override info master reason "moratorium" exists with description: "moratorium"
	And an override info master reason "politics" exists with description: "local politics"
	And an override info master reason "gis" exists with description: "GIS Data Incorrect"
	And an override info master reason "hydraulics" exists with description: "hydraulics"
	And an override info master reason "relocation" exists with description: "relocation needed"
	And a g i s data inaccuracy type "material" exists with description: "Material"
	And a g i s data inaccuracy type "date" exists with description: "Date"
	And a g i s data inaccuracy type "diameter" exists with description: "Diameter"
	And a g i s data inaccuracy type "mainbreak" exists with description: "Main Break"
	And work descriptions exist
	And a work order "one" exists with work description: "water main break repair", approved on: "today", town: "one", operating center: "nj7"

Scenario: user can search for a recurring project
    Given I am logged in as "admin"
    When I visit the ProjectManagement/RecurringProject/Search page
    And I press Search
    Then I should see a link to the Show page for recurring project: "one"
    When I follow the Show link for recurring project "one"
    Then I should be at the Show page for recurring project: "one"

Scenario: user can view a recurring project
    Given I am logged in as "admin"
	When I visit the Show page for recurring project: "one"
    Then I should see a display for recurring project: "one"'s ProjectTitle

#Scenario: user can add a recurring project
#    When I visit the ProjectManagement/RecurringProject/New page
#	And I select "Yes" from the OverrideInfoMasterDecision dropdown
#	And I press Save
#	Then I should see the validation message "The OverrideInfoMasterReason field is required."
#	And I should see the validation message "The OverrideInfoMasterJustification field is required."
#	When I select "No" from the OverrideInfoMasterDecision dropdown
#	And I enter "1234" into the WBSNumber field
#	And I select operating center "nj7" from the OperatingCenter dropdown
#	And I select town "one" from the Town dropdown
#	And I enter "project title" into the ProjectTitle field
#	And I enter "project description" into the ProjectDescription field
#	And I select asset category "one" from the AssetCategory dropdown
#	And I select asset type "facility" from the AssetType dropdown
#	And I enter "12" into the District field
#	And I enter "2016" into the OriginationYear field
#	And I enter "historic" into the HistoricProjectID field
#	And I enter "1000" into the NJAWEstimate field
#    And I select recurring project type "one" from the RecurringProjectType dropdown
#	And I enter "300" into the ProposedLength field
#	And I select pipe diameter "one" from the ProposedDiameter dropdown
#	And I select pipe material "one" from the ProposedPipeMaterial dropdown
#	And I enter "foobarbaz" into the Justification field
#	And I select asset investment category "one" from the AcceleratedAssetInvestmentCategory dropdown
#	And I select asset investment category "two" from the SecondaryAssetInvestmentCategory dropdown
#	And I enter "14" into the EstimatedProjectDuration field
#    And I enter today's date into the EstimatedInServiceDate field
#	#And I enter today's date into the ActualInServiceDate field
#	And I select foundational filing period "one" from the FoundationalFilingPeriod dropdown
#	And I select recurring project regulatory status "one" from the RegulatoryStatus dropdown
#	And I enter coordinate "one"'s Id into the Coordinate field
#	And I press Save
#    Then the currently shown recurring project will now be referred to as "urcelia"
#	And I should see a display for WBSNumber with "1234"
#	And I should see a display for OperatingCenter with operating center "nj7"
#	And I should see a display for Town with town "one"
#	And I should see a display for ProjectTitle with "project title"
#	And I should see a display for ProjectDescription with "project description"
#	And I should see a display for AssetCategory with asset category "one"
#	And I should see a display for AssetType with asset type "facility"
#	And I should see a display for District with "12"
#	And I should see a display for OriginationYear with "2016"
#	And I should see a display for HistoricProjectID with "historic"
#	And I should see a display for NJAWEstimate with "$1,000"
#	And I should see a display for AssetType with asset type "facility"'s Description
#	And I should see a display for RecurringProjectType with recurring project type "one"
#	And I should see a display for ProposedLength with "300"
#	And I should see a display for ProposedDiameter with pipe diameter "one"
#	And I should see a display for ProposedPipeMaterial with pipe material "one"
#	And I should see a display for Justification with "foobarbaz"
#	And I should see a display for AcceleratedAssetInvestmentCategory with asset investment category "one"
#	And I should see a display for SecondaryAssetInvestmentCategory with asset investment category "two"
#	And I should see a display for EstimatedProjectDuration with "14"
#	And I should see a display for EstimatedInServiceDate with today's date
#	#And I should see a display for ActualInServiceDate with today's date
#	And I should see a display for FoundationalFilingPeriod with foundational filing period "one"
#	And I should see a display for RegulatoryStatus with recurring project regulatory status "one"
#	And I should see a display for Status with recurring project status "proposed"
#	When I click the "Pipe Data" tab
#	Then I should see the following values in the pipe-data table
#	| Type                     | Value                      | Variable Score | Priority Weighted Score |
#	| Coordination with Others | None                       | 10             | 1                       |
#	| Insurance Claims         | Time Sensitive Opportunity | 10             | 10                      |
#	|                          | Estimated Final Scores     | 10             | 5.5                     |

Scenario: user can edit a recurring project
	Given I am logged in as "admin"
    When I visit the Edit page for recurring project: "one"
	And I press Save
	When I enter "1234" into the WBSNumber field
	And I select town "one" from the Town dropdown
	And I enter "project title" into the ProjectTitle field
	And I enter "project description" into the ProjectDescription field
	And I select asset category "one" from the AssetCategory dropdown
	And I select asset type "facility" from the AssetType dropdown
	And I enter "12" into the District field
	And I enter "2016" into the OriginationYear field
	And I enter "historic" into the HistoricProjectID field
	And I enter "1000" into the NJAWEstimate field
    And I select recurring project type "one" from the RecurringProjectType dropdown
	And I enter "300" into the ProposedLength field
	And I select pipe diameter "one" from the ProposedDiameter dropdown
	And I select pipe material "one" from the ProposedPipeMaterial dropdown
	And I enter "foobarbaz" into the Justification field
	And I select asset investment category "one" from the AcceleratedAssetInvestmentCategory dropdown
	And I select asset investment category "two" from the SecondaryAssetInvestmentCategory dropdown
	And I enter "14" into the EstimatedProjectDuration field
    And I enter today's date into the EstimatedInServiceDate field
	And I enter today's date into the ActualInServiceDate field
	And I select foundational filing period "one" from the FoundationalFilingPeriod dropdown
	And I select recurring project regulatory status "one" from the RegulatoryStatus dropdown
	And I enter coordinate "one"'s Id into the Coordinate field
	And I enter "some reasons" into the OverrideInfoMasterJustification field
	And I select "GIS Data Incorrect" from the OverrideInfoMasterReason dropdown
	And I press Save
	Then I should see a validation message for GISDataInaccuracies with "The GISDataInaccuracies field is required."
	When I select g i s data inaccuracy type "date" from the GISDataInaccuracies dropdown
	And I press Save
	Then I should see a validation message for CorrectInstallationDate with "The CorrectInstallationDate field is required."
	When I select g i s data inaccuracy type "diameter" from the GISDataInaccuracies dropdown
	And I press Save
	Then I should see a validation message for CorrectDiameter with "The CorrectDiameter field is required."
	When I select g i s data inaccuracy type "material" from the GISDataInaccuracies dropdown
	And I press Save
	Then I should see a validation message for CorrectMaterial with "The CorrectMaterial field is required."
	When I select g i s data inaccuracy type "mainbreak" from the GISDataInaccuracies dropdown
	And I press Save
	Then I should see a validation message for MainBreakOrders with "The MainBreakOrders field is required."
	When I select work order "one" from the MainBreakOrders dropdown
	And I enter today's date into the CorrectInstallationDate field
	And I select pipe diameter "one" from the CorrectDiameter dropdown
	And I select pipe material "one" from the CorrectMaterial dropdown
	And I press Save
	Then I should be at the Show page for recurring project: "one"
	And I should see a display for WBSNumber with "1234"
	And I should see a display for OperatingCenter with operating center "nj7"
	And I should see a display for Town with town "one"
	And I should see a display for ProjectTitle with "project title"
	And I should see a display for ProjectDescription with "project description" 
	And I should see a display for AssetCategory with asset category "one"
	And I should see a display for AssetType with asset type "facility"
	And I should see a display for District with "12"
	And I should see a display for OriginationYear with "2016"
	And I should see a display for HistoricProjectID with "historic"
	And I should see a display for NJAWEstimate with "$1,000"
	And I should see a display for AssetType with asset type "facility"'s Description
	And I should see a display for RecurringProjectType with recurring project type "one"
	And I should see a display for ProposedLength with "300"
	And I should see a display for ProposedDiameter with pipe diameter "one"
	And I should see a display for ProposedPipeMaterial with pipe material "one"
	And I should see a display for Justification with "foobarbaz"
	And I should see a display for AcceleratedAssetInvestmentCategory with asset investment category "one"
	And I should see a display for SecondaryAssetInvestmentCategory with asset investment category "two"
	And I should see a display for EstimatedProjectDuration with "14"
	And I should see a display for EstimatedInServiceDate with today's date
	And I should see a display for ActualInServiceDate with today's date
	And I should see a display for FoundationalFilingPeriod with foundational filing period "one"
	And I should see a display for RegulatoryStatus with recurring project regulatory status "one"
	And I should see a display for Status with recurring project status "complete"

Scenario: user can endorse a project and remove said endorsement if they so choose
	Given an endorsement status "one" exists with description: "Endorsed"
	And an endorsement status "two" exists with description: "Not Endorsed"
	And an endorsement status "three" exists with description: "Requires More Information"
	And I am logged in as "admin"
	When I visit the Show page for recurring project: "one"
	And I click the "Endorsements" tab
	And I press "Add Endorsement"
	And I select endorsement status "one" from the EndorsementStatus dropdown
	And I enter "This is a comment" into the Comment field
	And I press "Endorse"
	And I wait for the page to reload
	Then I should see the following values in the endorsements table
	| Employee | Endorsement Status | Comment           |
	| Admin McAdminy   | Endorsed           | This is a comment |
	When I click the "Endorsements" tab
	And I click the "Edit" link in the 1st row of endorsements
	And I select endorsement status "two" from the EndorsementStatus dropdown
	And I enter "This is not a comment" into the Comment field
	And I press Save
	And I wait for the page to reload
	When I click the "Endorsements" tab
	Then I should see the following values in the endorsements table
	| Employee       | Endorsement Status | Comment               |
	| Admin McAdminy | Not Endorsed       | This is not a comment |
	When I click ok in the dialog after pressing "Remove"
	And I wait for the page to reload
	Then I should not see "Admin McAdminy"

Scenario: Row is highlighted after completed 
	Given I am logged in as "admin"
    When I visit the Edit page for recurring project: "one"
	And I enter "1234" into the WBSNumber field
	And I select town "one" from the Town dropdown
	And I enter "project title" into the ProjectTitle field
	And I enter "project description" into the ProjectDescription field
	And I select asset category "one" from the AssetCategory dropdown
	And I select asset type "facility" from the AssetType dropdown
	And I enter "12" into the District field
	And I enter "2016" into the OriginationYear field
	And I enter "historic" into the HistoricProjectID field
	And I enter "1000" into the NJAWEstimate field
    And I select recurring project type "one" from the RecurringProjectType dropdown
	And I enter "300" into the ProposedLength field
	And I select pipe diameter "one" from the ProposedDiameter dropdown
	And I select pipe material "one" from the ProposedPipeMaterial dropdown
	And I enter "foobarbaz" into the Justification field
	And I select asset investment category "one" from the AcceleratedAssetInvestmentCategory dropdown
	And I select asset investment category "two" from the SecondaryAssetInvestmentCategory dropdown
	And I enter "14" into the EstimatedProjectDuration field
    And I enter today's date into the EstimatedInServiceDate field
	And I enter today's date into the ActualInServiceDate field
	And I select foundational filing period "one" from the FoundationalFilingPeriod dropdown
	And I select recurring project regulatory status "one" from the RegulatoryStatus dropdown
	And I enter coordinate "one"'s Id into the Coordinate field
	And I enter "some reasons" into the OverrideInfoMasterJustification field
	And I press "Save"
	And I visit the ProjectManagement/RecurringProject/Index page 
	Then the td elements in the 1st row of the "recurring-projects" table should have a "background-color" value of "#add8e6"
	# test that a null Status doesn't cause a null ref exception in Index view
	When I visit the Edit page for recurring project: "one"
	And I select "-- Select --" from the Status dropdown
	# need to null this out too or else the Status gets reset to Completed.
	And I enter "" into the ActualInServiceDate field
	And I press "Save"
	And I visit the ProjectManagement/RecurringProject/Index page 
	Then the td elements in the 1st row of the "recurring-projects" table should have a "background-color" value of "#ffffff"