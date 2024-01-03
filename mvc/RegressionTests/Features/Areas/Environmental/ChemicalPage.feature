Feature: ChemicalPage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
	And states of matter exist
	And packaging types exist
	And chemical types exist
    And a chemical "one" exists with SdsHyperlink: "https://mysite.com/chlorine", extremely hazardous chemical: true 
    And a chemical "two" exists
    And I am logged in as "admin"
	And a physical hazard "one" exists with description: "explosive"
	And a physical hazard "two" exists with description: "flammable"
	And a health hazard "one" exists with description: "acute toxicity"
	And a health hazard "two" exists with description: "simple asphyxiant"


Scenario: user can search for a chemical
    When I visit the Environmental/Chemical/Search page
    And I press Search
    Then I should see a link to the Show page for chemical: "one"
    When I follow the Show link for chemical "one"
    Then I should be at the Show page for chemical: "one"

Scenario: user can view a chemical
    When I visit the Show page for chemical: "one"
    Then I should see a display for chemical: "one"'s Name
	And I should see a display for chemical: "one"'s SdsHyperlink
	And I should see the link "https://mysite.com/chlorine" with the url "https://mysite.com/chlorine"

Scenario: user can add a chemical
    When I visit the Environmental/Chemical/New page
    And I enter "foo" into the Name field
	And I enter "bar" into the PartNumber field
	And I enter "1234" into the CasNumber field
	And I enter "https://mysite.com/chlorine" into the SdsHyperlink field
    And I enter "17" into the SubNumber field
	And I enter "8" into the DepartmentOfTransportationNumber field
	And I select "Pure" from the IsPure dropdown
	And I select "Yes" from the TradeSecret dropdown
	And I select "Yes" from the EmergencyPlanningCommunityRightToKnowActOnly dropdown
	And I check the ExtremelyHazardousChemical field
	And I check physical hazard "two" in the PhysicalHazards checkbox list
	And I check health hazard "two" in the HealthHazards checkbox list
	And I select "Solid" from the ChemicalStates dropdown
	And I select "Bulk" from the PackagingType dropdown	
	And I select "Algicide" from the ChemicalType dropdown	
	And I enter "1.25" into the SpecificGravityMin field
	And I enter "2.25" into the SpecificGravityMax field
	And I press Save
    Then I should see a display for Name with "foo"
	And I should see a display for PartNumber with "bar"
	And I should see a display for CasNumber with "1234"
	And I should see the link "https://mysite.com/chlorine" with the url "https://mysite.com/chlorine"
	And I should see "Solid"
	And I should see a display for SubNumber with "17"
	And I should see a display for DepartmentOfTransportationNumber with "8"
	And I should see a display for IsPure with "Pure"
	And I should see a display for TradeSecret with "Yes"
	And I should see a display for EmergencyPlanningCommunityRightToKnowActOnly with "Yes"
	And I should see a display for PhysicalHazards with physical hazard "two"
	And I should see a display for HealthHazards with health hazard "two"
	And I should see "Bulk"
	And I should see "Algicide"
	And I should see a display for SpecificGravity with "1.25 - 2.25"
	And I should see a display for ExtremelyHazardousChemical with "Yes"	

Scenario: user can edit a chemical
    When I visit the Edit page for chemical: "one"
    And I enter "bar" into the Name field
	And I enter "1234" into the CasNumber field
	And I enter "https://mysite.com/chlorine" into the SdsHyperlink field
    And I enter "17" into the SubNumber field
	And I enter "8" into the DepartmentOfTransportationNumber field
	And I select "Pure" from the IsPure dropdown
	And I select "Yes" from the TradeSecret dropdown
	And I select "Yes" from the EmergencyPlanningCommunityRightToKnowActOnly dropdown
	And I select "Solid" from the ChemicalStates dropdown
	And I check physical hazard "one" in the PhysicalHazards checkbox list
	And I check health hazard "one" in the HealthHazards checkbox list
	And I select "Bulk" from the PackagingType dropdown
	And I select "Oxidizer" from the ChemicalType dropdown
	And I enter "1.25" into the SpecificGravityMin field
	And I enter "2.25" into the SpecificGravityMax field
	And I press Save
    Then I should be at the Show page for chemical: "one"
    And I should see a display for Name with "bar"
	And I should see a display for CasNumber with "1234"
	And I should see the link "https://mysite.com/chlorine" with the url "https://mysite.com/chlorine"
	And I should see a display for SubNumber with "17"
	And I should see a display for DepartmentOfTransportationNumber with "8"
	And I should see a display for IsPure with "Pure"
	And I should see a display for TradeSecret with "Yes"
	And I should see a display for EmergencyPlanningCommunityRightToKnowActOnly with "Yes"
	And I should see a display for PhysicalHazards with physical hazard "one"
	And I should see a display for HealthHazards with health hazard "one"
	And I should see "Solid"
	And I should see "Bulk"
	And I should see "Oxidizer"
	And I should see a display for SpecificGravity with "1.25 - 2.25"
	
	Scenario: user can search for an extremely hazardous chemical
		When I visit the Environmental/Chemical/Search page
		And I select "Yes" from the ExtremelyHazardousChemical dropdown
		And I press Search
		Then I should see a link to the Show page for chemical: "one"
		When I follow the Show link for chemical "one"
		Then I should be at the Show page for chemical: "one"