Feature: SampleSitePage

Background: data exists
	Given states exist
	And an operating center "nj7" exists with opcode: "nj7", state: "nj"
	And an operating center "nj4" exists with opcode: "nj4", state: "nj"
	And an admin user "admin" exists with username: "admin"
	And an asset type "valve" exists with description: "valve"
	And an asset type "hydrant" exists with description: "hydrant"
	And an employee status "active" exists with description: "Active"
	And an employee "one" exists with status: "active", first name: "Bill", last name: "S. Preston", employee id: "1000001"
	And a user "user" exists with username: "user", operating center: "nj7", employee: "one", full name: "anakin skywalker"
	And a role "wq-useradmin" exists with action: "UserAdministrator", module: "WaterQualityGeneral", user: "user", operating center: "nj7"
	And a role "fsa-read" exists with action: "Read", module: "FieldServicesAssets", user: "user", operating center: "nj7"
	And a role "fswm-read-nj4" exists with action: "UserAdministrator", module: "FieldServicesWorkManagement", user: "user", operating center: "nj4"
	And a county "one" exists with state: "nj"
	And a town "one" exists with county: "one"
	And a town "two" exists with county: "one"
	And a hydrant billing "public" exists with description: "Public"
	And a hydrant "nj4" exists with operating center: "nj4", town: "one", hydrant billing: "public", hydrant number: "hab-4"
	And a hydrant "nj7" exists with operating center: "nj7", town: "two", hydrant billing: "public", hydrant number: "hab-7"
	And a valve billing "public" exists with description: "PUBLIC"
	And a valve "nj4" exists with operating center: "nj4", town: "one", valve billing: "public", valve number: "vab-4"
	And a valve "nj7" exists with operating center: "nj7", town: "two", valve billing: "public", valve number: "vab-7"
	And an operating center "pa4" exists with opcode: "pa4", state: "pa"
	And operating center: "nj4" exists in town: "one"
	And operating center: "nj7" exists in town: "two"
	And operating center: "pa4" exists in town: "two"
	And a town section "one" exists with name: "A section", town: "two"
	And a town section "inactive" exists with town: "two", active: false
	And a street "one" exists with town: "one", is active: true
	And a street "two" exists with town: "two", is active: true
	And a street "cs-01" exists with town: "one", is active: true
	And a street "cs-02" exists with town: "two", is active: true
	And a service category "one" exists with description: "Fire Service Installation"
	And a service material "one" exists with description: "Mercury", code: "Hg"
	And a service material "two" exists with description: "Silver", code: "Ag"
	And a service type "one" exists with operating center: "nj7", service category: "one", description: "Water NJ7"
	And service utility types exist
	And sample site point of use treatment types exist
	And a coordinate "one" exists
	And a town "nj4burg" exists with name: "Swedesboro"
	And a town section "ts-02" exists with town: "nj4burg"
	And a region code "one" exists with s a p code: "1300", description: "nj7burg"
	And a premise "p" exists with premise number: "8675309", is major account: "true", service address house number: "7", service address apartment: "garbage", service address street: "EaSy St", service address fraction: "1/2", equipment: "123", meter serial number: "123", operating center: "nj4", region code: "one", device location: "1234", device category: "00000000091111111", installation: "12345", coordinate: "one", service city: "nj4burg", connection object: "test", service zip: "85023", service utility type: "irrigation"
	And a service "one" exists with operating center: "nj7", town: "one", service category: "one", service number: 1, street: "one", service material: "one", customer side material: "two", premise: "p", date installed: "1/1/2023"
	And a service "two" exists with operating center: "pa4", town: "two", service category: "one", service number: 2, street: "two"
	And a sample site lead copper tier classification "tier 1 - nj" exists with description: "tier one"
	And a sample site lead copper tier classification "tier 2 - pa" exists with description: "tier two"
	And a sample site lead copper tier classification "tier 3 - nj" exists with description: "tier three"
	And a sample site lead copper tier classification "tier 1 no text - il" exists with description: "tier one no text"
	And a sample site lead copper tier sample category "one - nj" exists with description: "one - nj", display value: "1"
	And a sample site lead copper tier sample category "two - pa" exists with description: "two - pa", display value: "2"
	And a sample site lead copper tier sample category "three - nj" exists with description: "three - nj", display value: "3"
	And a sample site lead copper tier sample category "four - il" exists with description: "four - il", display value: "4"
	And sample site lead copper tier classification: "tier 1 - nj" exists in state: "nj"
	And sample site lead copper tier classification: "tier 2 - pa" exists in state: "pa"
	And sample site lead copper tier classification: "tier 3 - nj" exists in state: "nj"
	And sample site lead copper tier classification: "tier 1 no text - il" exists in state: "il"
	And sample site lead copper tier sample category: "one - nj" exists in sample site lead copper tier classification: "tier 1 - nj"
	And sample site lead copper tier sample category: "two - pa" exists in sample site lead copper tier classification: "tier 2 - pa"
	And sample site lead copper tier sample category: "three - nj" exists in sample site lead copper tier classification: "tier 3 - nj"
	And sample site lead copper tier sample category: "four - il" exists in sample site lead copper tier classification: "tier 1 no text - il"
	And sample site lead copper validation methods exist
	And sample site customer contact methods exist
	And public water supply statuses exist
	And a sample site status "active" exists with description: "Active"
	And a sample site status "pending" exists with description: "Pending"
	And a public water supply "one" exists with identifier: "NJ1345001", system: "Coastal North Monmouth/Lakewood", status: "active"
	And a sample site profile analysis type "one" exists with description: "chem"
	And a sample site profile "one" exists with number: 100, public water supply: "one", sample site profile analysis type: "one"
	And a sample plan "one" exists with p w s i d: "one"
	And operating center: "nj4" exists in public water supply: "one"
	And operating center: "nj7" exists in public water supply: "one"
	And a sample site "one" exists with operating center: "nj4", town: "one", public water supply: "one", sample site profile: "one", premise: "p"
	And a sample site "two" exists with operating center: "nj7", town: "two", premise: "p"		
	And a sample site location type "primary" exists with description: "Primary"
	And a sample site location type "upstream" exists with description: "Upstream"
	And a facility "parent-sample-site-facility" exists with operating center: "nj4", town: "one"
	And a sample site "parent-sample-site" exists with operating center: "nj4", town: "one", facility: "parent-sample-site-facility", location type: "primary", premise: "p"
	And a sample site availability "one" exists with description: "Year Round"
	And a sample site availability "two" exists with description: "Seasonal-Summer"
	And a sample site availability "three" exists with description: "Seasonal-Winter"
	And sample site collection types exist
	And a sample site sub collection type "one" exists with description: "sub type"
	And sample site address location types exist
	And a sample site validation status "validated" exists with description: "Validated"
	And a sample site validation status "notvalidated" exists with description: "Not Validated"

Scenario: user can search for a sample site
	Given I am logged in as "user"
    When I visit the WaterQuality/SampleSite/Search page
    And I press Search
    Then I should see a link to the Show page for sample site: "one"
    When I follow the Show link for sample site "one"
    Then I should be at the Show page for sample site: "one"
    
Scenario: user can search for and find a sample site where outofservicearea is yes without selecting an outofservicearea search option
	Given I am logged in as "user"
	And a sample site "four" exists with operating center: "pa4", town: "two", premise: "p", out of service area: true
	When I visit the WaterQuality/SampleSite/Search page
	And I press Search
	Then I should see a link to the Show page for sample site: "four"
	When I follow the Show link for sample site "four"
	Then I should be at the Show page for sample site: "four"

Scenario: user can view a sample site
	Given I am logged in as "user"
    When I visit the Show page for sample site: "one"
    Then I should see a display for sample site: "one"'s LocationNameDescription
	And I should see a display for sample site: "one"'s SampleSiteValidationStatus

Scenario: user can add and remove a sample plan to a sample site
	Given I am logged in as "user"
    When I visit the Show page for sample site: "one"
    And I click the "Sample Plans" tab
	And I press "Add Sample Plan"
	And I select sample plan "one"'s Description from the SamplePlan dropdown
	And I press "Save Sample Plan"
	Then I should be at the Show page for sample site "one"
	And I should see a link to the Show page for sample plan "one"
	When I click the "Sample Plans" tab
	And I click ok in the dialog after pressing "Remove Sample Plan"
	Then I should be at the Show page for sample site "one"
	And I should not see a link to the Show page for sample plan "one"

Scenario: user can add a sample site
	Given I am logged in as "user"
    When I visit the WaterQuality/SampleSite/New page
	And I press Save
	Then I should see a validation message for CommonSiteName with "The CommonSiteName field is required."
	And I should see a validation message for LocationNameDescription with "The LocationNameDescription field is required."
	And I should see a validation message for AgencyId with "The Agency Id field is required."
	When I select sample site status "active" from the Status dropdown
	And I select "No" from the IsComplianceSampleSite dropdown
	And I enter "sample site" into the CommonSiteName field
	And I enter "location name description" into the LocationNameDescription field
	And I enter "agency id" into the AgencyId field
	And I press Save
	Then I should not see a validation message for AgencyId with "The Agency Id field is required."
	And I should not see a validation message for CommonSiteName with "The CommonSiteName is required."
	And I should not see a validation message for LocationNameDescription with "The LocationNameDescription field is required."	
	When I select "Yes" from the IsComplianceSampleSite dropdown
	And I select "Yes" from the IsLimsLocation dropdown
	And I press Save
	When I select "Yes" from the LeadCopperSite dropdown
	And I click ok in the dialog after pressing "Save"
	And I wait 1 second
	And I select "Yes" from the ResourceDistributionMaps dropdown
	And I select state "nj" from the State dropdown
	Then I should see sample site lead copper tier classification "tier 1 - nj"'s Description in the LeadCopperTierClassification dropdown
	And I should see sample site lead copper tier classification "tier 3 - nj"'s Description in the LeadCopperTierClassification dropdown
	And I should not see sample site lead copper tier classification "tier 1 no text - il"'s Description in the LeadCopperTierClassification dropdown
	And I should not see sample site lead copper tier classification "tier 2 - pa"'s Description in the LeadCopperTierClassification dropdown
	When I select "Tier 1- Single Family Residences with Lead Pipe or Lead Service Lines" from the LeadCopperTierClassification dropdown
	Then I should see sample site lead copper tier sample category "one - nj"'s ToString in the LeadCopperTierSampleCategory dropdown
	And I should not see sample site lead copper tier sample category "three - nj"'s ToString in the LeadCopperTierSampleCategory dropdown
	And I should not see sample site lead copper tier sample category "four - il"'s ToString in the LeadCopperTierSampleCategory dropdown
	And I should not see sample site lead copper tier sample category "two - pa"'s ToString in the LeadCopperTierSampleCategory dropdown
	When I select state "il" from the State dropdown
	Then I should not see sample site lead copper tier classification "tier 1 - nj"'s Description in the LeadCopperTierClassification dropdown
	And I should not see sample site lead copper tier classification "tier 2 - pa"'s Description in the LeadCopperTierClassification dropdown
	And I should not see sample site lead copper tier classification "tier 3 - nj"'s Description in the LeadCopperTierClassification dropdown
	And I should see sample site lead copper tier classification "tier 1 no text - il"'s Description in the LeadCopperTierClassification dropdown
	When I select "Tier 1" from the LeadCopperTierClassification dropdown
	Then I should not see sample site lead copper tier sample category "one - nj"'s ToString in the LeadCopperTierSampleCategory dropdown
	And I should not see sample site lead copper tier sample category "two - pa"'s ToString in the LeadCopperTierSampleCategory dropdown
	And I should not see sample site lead copper tier sample category "three - nj"'s ToString in the LeadCopperTierSampleCategory dropdown
	And I should see sample site lead copper tier sample category "four - il"'s ToString in the LeadCopperTierSampleCategory dropdown
	When I select state "ca" from the State dropdown
	And I select "4 - four - il" from the LeadCopperTierSampleCategory dropdown
	And I press Save
	Then I should not see a validation message for LimsFacilityId with "The LIMS Facility Id field is required."
	And I should not see a validation message for LimsSiteId with "The LIMS Site Id field is required."
	And I should see a validation message for LimsPrimaryStationCode with "The LIMS Primary Station Code field is required."
	When I select state "nj" from the State dropdown
	And I enter "facility-id" into the LimsFacilityId field
	And I enter "site-id" into the LimsSiteId field
	And I select state "nj" from the State dropdown
    And I select operating center "nj4" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
	And I select public water supply "one" from the PublicWaterSupply dropdown
	And I click the "Supplemental Validation" tab
	And I select "Yes" from the ResourceDistributionMaps dropdown
	And I press Save
	Then I should see a validation message for SampleSiteProfile with "The Profile field is required."
	And I should see a validation message for IsAlternateSite with "The IsAlternateSite field is required."
	When I select "Yes" from the BactiSite dropdown
	And I press Save
	Then I should see a validation message for LocationType with "The LocationType field is required."
	And I should see a validation message for CollectionType with "The CollectionType field is required."
	When I select "No" from the LeadCopperSite dropdown
	And  I select "No" from the BactiSite dropdown
	Then I should not see the LeadCopperTierClassification field
	When I select "Yes" from the LeadCopperSite dropdown
	And I select "Yes" from the IsAlternateSite dropdown
	And I press Save
	Then I should see a validation message for LeadCopperValidationMethod with "The Validation Method field is required."
	When I select "Tier 3- Single Family Residences with Copper Pipe & Lead Solder installed before 1983" from the LeadCopperTierClassification dropdown
    And I select sample site lead copper tier sample category "three - nj" from the LeadCopperTierSampleCategory dropdown
	And I press Save
	Then I should see a validation message for LeadCopperTierThreeExplanation with "The Tier 3 Explanation field is required."
	When I select "Lead swab test on customer plumbing and three (3) non-consecutive joints" from the LeadCopperValidationMethod dropdown
	And I enter "tier three explanation" into the LeadCopperTierThreeExplanation field
	And I select sample site profile "one" from the SampleSiteProfile dropdown
	And I select "Yes" from the IsAlternateSite dropdown
	And I select sample site availability "one" from the Availability dropdown
	And I select "Yes" from the IsProcessSampleSite dropdown
	And I select "Yes" from the IsResearchSampleSite dropdown
	And I select sample site location type "upstream" from the LocationType dropdown
	And I select sample site collection type "raw" from the CollectionType multiselect
	And I select sample site sub collection type "one" from the SubCollectionType dropdown
	And I select "Yes" from the IsLimsLocation dropdown
	And I select "Custom" from the SampleSiteAddressLocationType dropdown
	And I enter coordinate "one"'s Id into the Coordinate field
    And I select town "one" from the Town dropdown
	And I select street "one" from the Street dropdown	
	And I select street "cs-01" from the CrossStreet dropdown	
	And I select sample site validation status "validated" from the SampleSiteValidationStatus dropdown
	And I press Save
	Then I should see a validation message for ParentSite with "The ParentSite field is required."
	And I should not see a validation message for SampleSiteProfile with "The SampleSiteProfile field is required."
	When I select sample site "parent-sample-site"'s ToString from the ParentSite dropdown
	And I click the "Supplemental Validation" tab
	And I select "Other" from the SampleSitePointOfUseTreatmentType dropdown
    And I press Save
	Then I should see a validation message for PointOfUseTreatmentTypeOtherReason with "The Other Reason field is required."
	When I enter "because reasons" into the PointOfUseTreatmentTypeOtherReason field
    When I press Save
	And I wait for the page to reload
    Then the currently shown sample site will now be referred to as "new"
    And I should see a display for OperatingCenter with operating center "nj4"
	And I should see a display for Street with street "one"
	And I should see a display for CrossStreet with street "cs-01"
	And I should see a display for LeadCopperSite with "Yes"
	And I should see a display for IsAlternateSite with "Yes"
	And I should see a display for BactiSite with "No"
	And I should see a display for LeadCopperTierClassification with "Tier 3- Single Family Residences with Copper Pipe & Lead Solder installed before 1983"
	And I should see a display for LeadCopperTierSampleCategory with "3 - three - nj"
	And I should see a display for LeadCopperTierThreeExplanation with "tier three explanation"
	And I should see a display for LeadCopperValidationMethod with "Lead swab test on customer plumbing and three (3) non-consecutive joints"
	And I should see a display for PublicWaterSupply with public water supply "one"
	And I should see a display for Availability with "Year Round"
	And I should see a display for IsComplianceSampleSite with "Yes"
	And I should see a display for IsProcessSampleSite with "Yes"
	And I should see a display for IsResearchSampleSite with "Yes"
	And I should see a display for LocationType with sample site location type "upstream"
	And I should see a display for ParentSite with sample site "parent-sample-site"'s ToString
	And I should see a display for CollectionType with sample site collection type "raw"
	And I should see a display for SubCollectionType with sample site sub collection type "one"
	And I should see a display for CommonSiteName with "sample site"
	And I should see a display for LocationNameDescription with "location name description"
	And I should see a display for AgencyId with "agency id"
	And I should see a display for IsLimsLocation with "Yes"
	And I should see a display for SampleSiteValidationStatus with "Validated"
	And I should see a display for LimsFacilityId with "facility-id"
	And I should see a display for LimsSiteId with "site-id"
	And I should see a display for SampleSiteProfile with sample site profile "one"'s ToString
	And I should see a display for SampleSiteAddressLocationType with "Custom"
	When I click the "Supplemental Validation" tab
	Then I should see a display for ResourceDistributionMaps with "Yes"
	And I should see a display for ResourceCapitalImprovement with "n/a"
	And I should see a display for ResourceUtilityRecords with "n/a"
	And I should see a display for ResourceSamplingResults with "n/a"
	And I should see a display for ResourceInterviewsPersonnel with "n/a"
	And I should see a display for ResourceCommunitySurvey with "n/a"
	And I should see a display for ResourceCountyAppraisal with "n/a"
	And I should see a display for ResourceContacts with "n/a"
	And I should see a display for ResourceSurveyResults with "n/a"
	And I should see a display for ResourceInterviewsResidents with "n/a"
	And I should see a display for ResourceInterviewsContractors with "n/a"
	And I should see a display for ResourceLeadCheckSwabs with "n/a"
	And I should see a display for SampleSitePointOfUseTreatmentType with "Other"
	And I should see a display for PointOfUseTreatmentTypeOtherReason with "because reasons"
	When I click the "Details" tab
	
Scenario: user can open premise details in new tab
	Given I am logged in as "user"
	When I visit the Show page for sample site: "one"
	Then I should see a link to the Show page for premise "p" and fragment of "#CurrentMaterial/SizeTab"
	When I follow the "8675309" link to the Show page for premise "p" and fragment of "#CurrentMaterial/SizeTab"
	And I switch to the last browser tab
	Then I should be at the Show page for premise "p" and fragment of "#CurrentMaterial/SizeTab"
	And the CurrentMaterial/SizeTab tab should be active

Scenario: user cannot add a sample site to an inactive town section
	Given I am logged in as "user"
    When I visit the WaterQuality/SampleSite/New page
	And I select state "nj" from the State dropdown
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I select town "two" from the Town dropdown
    And I wait for ajax to finish loading
    Then I should not see town section "inactive" in the TownSection dropdown

Scenario: user can edit a sample site
	Given I am logged in as "user"
	When I visit the Edit page for sample site: "one"
	And I select state "nj" from the State dropdown
	And I select operating center "nj4" from the OperatingCenter dropdown
	And I select "Custom" from the SampleSiteAddressLocationType dropdown
	And I select town "one" from the Town dropdown
	And I select street "one" from the Street dropdown
	And I select street "cs-01" from the CrossStreet dropdown
    And I enter "bar" into the LocationNameDescription field 
	And I enter "facility-id" into the LimsFacilityId field
	And I enter "site-id" into the LimsSiteId field
	And I enter "agency-id" into the AgencyId field
	And I select "No" from the BactiSite dropdown
	And I select public water supply "one" from the PublicWaterSupply dropdown
	And I select sample site profile "one" from the SampleSiteProfile dropdown
	And I select "Yes" from the LeadCopperSite dropdown	
	And I click ok in the dialog after pressing "Save"
	And I wait 1 second
	And I select "Yes" from the ResourceDistributionMaps dropdown
	And I select "Yes" from the IsAlternateSite dropdown
	And I select "Tier 3- Single Family Residences with Copper Pipe & Lead Solder installed before 1983" from the LeadCopperTierClassification dropdown
	And I enter "tier three explanation" into the LeadCopperTierThreeExplanation field
	And I select "Lead swab test on customer plumbing and three (3) non-consecutive joints" from the LeadCopperValidationMethod dropdown
	And I select sample site lead copper tier sample category "three - nj" from the LeadCopperTierSampleCategory dropdown
	When I click the "Service Record" tab
	Then I should see a link to the Show page for service "one" 
    Then I should see a display for DisplayPremise_MostRecentService_ServiceMaterial with "Mercury"
    Then I should see a display for DisplayPremise_MostRecentService_CustomerSideMaterial with "Silver"
	When I click the "Supplemental Validation" tab
	And I select "Yes" from the ResourceDistributionMaps dropdown
	And I select "Yes" from the ResourceCapitalImprovement dropdown
	And I select "Yes" from the ResourceUtilityRecords dropdown
	And I select "Yes" from the ResourceSamplingResults dropdown
	And I select "Yes" from the ResourceInterviewsPersonnel dropdown
	And I select "Yes" from the ResourceCommunitySurvey dropdown
	And I select "Yes" from the ResourceCountyAppraisal dropdown
	And I select "Yes" from the ResourceContacts dropdown
	And I select "Yes" from the ResourceSurveyResults dropdown
	And I select "Yes" from the ResourceInterviewsResidents dropdown
	And I select "Yes" from the ResourceInterviewsContractors dropdown
	And I select "Yes" from the ResourceLeadCheckSwabs dropdown
	And I select public water supply "one" from the PublicWaterSupply dropdown
	And I enter coordinate "one"'s Id into the Coordinate field
	And I select sample site validation status "notvalidated" from the SampleSiteValidationStatus dropdown
	And I enter "Test" into the CommonSiteName field
    And I select sample site lead copper tier sample category "three - nj" from the LeadCopperTierSampleCategory dropdown
	And I select "No" from the IsLimsLocation dropdown
    And I press Save
    Then I should be at the Show page for sample site: "one"
    And I should see a display for LocationNameDescription with "bar"
	And I should see a display for PublicWaterSupply with public water supply "one"
	And I should see a display for CommonSiteName with "Test"
	And I should see a display for SampleSiteValidationStatus with "Not Validated"
	And I should see a display for SampleSiteAddressLocationType with "Custom"
	And I should see a display for LeadCopperSite with "Yes"
	And I should see a display for IsAlternateSite with "Yes"
	And I should see a display for LeadCopperTierClassification with "Tier 3- Single Family Residences with Copper Pipe & Lead Solder installed before 1983"
	And I should see a display for LeadCopperTierThreeExplanation with "tier three explanation"
	And I should see a display for LeadCopperValidationMethod with "Lead swab test on customer plumbing and three (3) non-consecutive joints"
	When I click the "Supplemental Validation" tab
	Then I should see a display for ResourceDistributionMaps with "Yes"
	And I should see a display for ResourceCapitalImprovement with "Yes"
	And I should see a display for ResourceUtilityRecords with "Yes"
	And I should see a display for ResourceSamplingResults with "Yes"
	And I should see a display for ResourceInterviewsPersonnel with "Yes"
	And I should see a display for ResourceCommunitySurvey with "Yes"
	And I should see a display for ResourceCountyAppraisal with "Yes"
	And I should see a display for ResourceContacts with "Yes"
	And I should see a display for ResourceSurveyResults with "Yes"
	And I should see a display for ResourceInterviewsResidents with "Yes"
	And I should see a display for ResourceInterviewsContractors with "Yes"
	And I should see a display for ResourceLeadCheckSwabs with "Yes"
	
Scenario: User can add and remove a bracket site 
    Given a public water supply "two" exists with identifier: "NJ1345003", system: "The Second System", status: "active"
	And operating center: "nj4" exists in public water supply: "two"
    And a sample site "three" exists with operating center: "nj4", town: "one", public water supply: "one", availability: "one"
	And a sample site bracket site location type "one" exists with description: "Some Location"
	And I am logged in as "user"
	When I visit the Show page for sample site: "one"
	And I click the "Bracket Sites" tab
	And I press "Add Bracket Site"
	Then I should see sample site "three"'s ToString in the BracketSite dropdown
	And I should not see sample site "two" in the BracketSite dropdown
	When I select sample site "three"'s ToString from the BracketSite dropdown
	And I select sample site bracket site location type "one" from the LocationType dropdown
	And I press "Save Bracket Site"
	And I click the "Bracket Sites" tab 
	Then I should see the following values in the bracket-sites-table table 
	| Bracket Sample Site  | Bracket Site Location Type                    |
	| sample site: "three" | sample site bracket site location type: "one" |
	When I press "Remove"
	And I click the "Bracket Sites" tab 
	Then the bracket-sites-table table should be empty

Scenario: A user f not certify a sample site if it has not been three years since the last certification
	Given a sample site "notcertifiable" exists with operating center: "nj4", town: "one", public water supply: "one", certified by: "user", certified date: "1/14/2120", lead copper site: true
	And I am logged in as "user"
	And I am at the show page for sample site: "notcertifiable"
	Then I should see "anakin skywalker"
	When I visit the Edit page for sample site: "notcertifiable"
	Then I should not see "I certify that the Tier Selection for this site was selected and documented"

Scenario: A user can certify a sample site
	Given a sample site "certifiable" exists with operating center: "nj4", town: "one", public water supply: "one", common site name "test"
	And I am logged in as "user"
	When I visit the Edit page for sample site: "certifiable"
	And I select state "nj" from the State dropdown
	And I select operating center "nj4" from the OperatingCenter dropdown
	And I select "Custom" from the SampleSiteAddressLocationType dropdown
	And I select town "one" from the Town dropdown
	And I select street "one" from the Street dropdown
	And I select street "cs-01" from the CrossStreet dropdown
    And I enter "bar" into the LocationNameDescription field 
	And I enter "facility-id" into the LimsFacilityId field
	And I enter "site-id" into the LimsSiteId field
	And I enter "agency-id" into the AgencyId field
	And I select public water supply "one" from the PublicWaterSupply dropdown
	And I select sample site profile "one" from the SampleSiteProfile dropdown
	And I select "Yes" from the LeadCopperSite dropdown
	And I select "Yes" from the IsAlternateSite dropdown
	And I check the CertificationAuthorization field
	And I select "Tier 3- Single Family Residences with Copper Pipe & Lead Solder installed before 1983" from the LeadCopperTierClassification dropdown
	And I enter "tier three explanation" into the LeadCopperTierThreeExplanation field
	And I select "Lead swab test on customer plumbing and three (3) non-consecutive joints" from the LeadCopperValidationMethod dropdown
	When I click the "Supplemental Validation" tab
	And I select "Yes" from the ResourceDistributionMaps dropdown
	And I select public water supply "one" from the PublicWaterSupply dropdown
	And I enter coordinate "one"'s Id into the Coordinate field
	And I select sample site validation status "notvalidated" from the SampleSiteValidationStatus dropdown
	And I select sample site lead copper tier sample category "three - nj" from the LeadCopperTierSampleCategory dropdown
	And I select "No" from the IsLimsLocation dropdown
    And I press Save
    Then I should be at the Show page for sample site: "certifiable"
	And I should see "anakin skywalker"

Scenario: A user can validate a sample site
	Given a sample site "validatable" exists with operating center: "nj4", town: "one", public water supply: "one", street: "one", cross street: "cs-01", availability: "one", sample site profile: "one", lims facility id: "f-id", lims site id: "s-id", lead copper site: false, agency id: "agency id", location name description: "location name description", is lims location: false
	And I am logged in as "user"
	And I am at the edit page for sample site: "validatable"
	When I click ok in the dialog after clicking "IsBeingValidated"
	And I select "Custom" from the SampleSiteAddressLocationType dropdown
	And I enter coordinate "one"'s Id into the Coordinate field
	And I select sample site validation status "validated" from the SampleSiteValidationStatus dropdown
	And I press Save
	And I wait for the page to reload
	Then I should see a display for ValidatedBy with employee "one"
	And I should see a display for ValidatedAt with today's date
	And I should see a display for SampleSiteValidationStatus with "Validated"

Scenario: facility fields populate address when selected
	Given facility statuses exist
	And a facility "one" exists with operating center: "nj4", town: "one", coordinate: "one", street number: "817", street: "one", zip code: "07711", facility status: "active"
	And I am logged in as "admin"
    When I visit the WaterQuality/SampleSite/New page
	And I select state "nj" from the State dropdown
	And I select operating center "nj4" from the OperatingCenter dropdown
	And I select "Facility" from the SampleSiteAddressLocationType dropdown
	And I select facility "one" from the Facility dropdown
	And I wait for ajax to finish loading
	Then I should see town "one" in the Town dropdown
	And I should see "817" in the StreetNumber field
	And I should see coordinate "one"'s Id in the Coordinate field
	And I should see "07711" in the ZipCode field
	And I should see street "one" in the Street dropdown 
	And I should see street "cs-01" in the CrossStreet dropdown 

Scenario: fields need to be manually entered when custom is selected in address location type
	Given I am logged in as "admin"
	When I visit the WaterQuality/SampleSite/New page
	And I select state "nj" from the State dropdown
	And I select operating center "nj4" from the OperatingCenter dropdown
	And I select "Custom" from the SampleSiteAddressLocationType dropdown
	Then I should see "-- Select --" in the Town dropdown
	And I should see "" in the StreetNumber field
	And I should see "" in the ZipCode field
	And I should see "Please select a town above." in the Street dropdown

Scenario: Premise number should be removed when address location type changed
	Given I am logged in as "admin"
	When I visit the WaterQuality/SampleSite/New page
	And I select state "nj" from the State dropdown
	And I select operating center "nj4" from the OperatingCenter dropdown
	And I select "Premise" from the SampleSiteAddressLocationType dropdown
	And I enter "12345678" into the Premise field
	And I select "Custom" from the SampleSiteAddressLocationType dropdown
	And I select "Premise" from the SampleSiteAddressLocationType dropdown
	Then I should see "" in the Premise field

Scenario: user should not see the service record tab when sample site address location type is not premise
	Given a sample site "x" exists with sample site address location type: "custom"
	And I am logged in as "user"
	And I am at the show page for sample site: "x"
	Then I should not see the "Service Record" tab

Scenario: LIMS Facility Id is not visible for CA and PA 
	Given I am logged in as "admin"
	When I visit the WaterQuality/SampleSite/New page
	And I select "Yes" from the IsComplianceSampleSite dropdown
	And I select "Yes" from the IsLimsLocation dropdown
	And I select "PA" from the State dropdown
	Then I should not see "Facility Id *" in the horizon-lims-fields element
	When I select "CA" from the State dropdown
	Then I should not see "Facility Id *" in the horizon-lims-fields element
	When I select "TN" from the State dropdown
	Then I should see "Facility Id *" in the horizon-lims-fields element
	And I should see "" in the LimsFacilityId field
	When I select "GA" from the State dropdown
	Then I should see "Facility Id *" in the horizon-lims-fields element
	And I should see "" in the LimsFacilityId field

Scenario: LIMS Site Id is required for complaint PA sample sites
	Given I am logged in as "admin"
	When I visit the WaterQuality/SampleSite/New page
	And I select "Yes" from the IsComplianceSampleSite dropdown
	And I select "Yes" from the IsLimsLocation dropdown
	And I select "PA" from the State dropdown
	Then I should see "Site Id *" in the horizon-lims-fields element
	When I press Save
	Then I should see the validation message "The LIMS Site Id field is required."
	When I select "No" from the IsComplianceSampleSite dropdown
	Then I should see "Site Id" in the horizon-lims-fields element
	When I press Save
    Then I should not see the validation message "The LIMS Site Id field is required."

Scenario: Primary Station Code is required for complaint CA sample sites
	Given I am logged in as "admin"
	When I visit the WaterQuality/SampleSite/New page
	And I select "Yes" from the IsComplianceSampleSite dropdown
	And I select "Yes" from the IsLimsLocation dropdown
	And I select "CA" from the State dropdown
	Then I should see "Primary Station Code *" in the horizon-lims-fields element
	When I press Save
	Then I should see the validation message "The LIMS Primary Station Code field is required."
	When I select "No" from the IsComplianceSampleSite dropdown
	Then I should see "Primary Station Code" in the horizon-lims-fields element
	When I press Save
    Then I should not see the validation message "The LIMS Primary Station Code field is required."

Scenario: Both LIMS Facility Id and LIMS Site Id are required for complaint sample sites not PA or CA
	Given I am logged in as "admin"
	When I visit the WaterQuality/SampleSite/New page
	And I select "Yes" from the IsComplianceSampleSite dropdown
	And I select "Yes" from the IsLimsLocation dropdown
	And I select "TN" from the State dropdown
	Then I should see "Facility Id *" in the horizon-lims-fields element
	And I should see "Site Id *" in the horizon-lims-fields element
	When I press Save
	Then I should see the validation message "The LIMS Facility Id field is required."
	And I should see the validation message "The LIMS Site Id field is required."
	When I select "No" from the IsComplianceSampleSite dropdown
	Then I should not see "Facility Id *" in the horizon-lims-fields element
	And I should not see "Site Id *" in the horizon-lims-fields element
	When I press Save
	Then I should not see the validation message "The LIMS Facility Id field is required."
	And I should not see the validation message "The LIMS Site Id field is required."

Scenario: admin can update a sample site and the LIMS fields are populated for TN
	Given an operating center "tn1" exists with opcode: "tn1", state: "tn"
	And a town "chat" exists with name: "Chattanooga"
	And operating center: "tn1" exists in town: "chat"
	And a public water supply "tn" exists with identifier: "TN1345001", system: "Coastal", status: "active"
	And a sample site profile "tn" exists with number: 1001, public water supply: "tn", sample site profile analysis type: "one"
	And operating center: "tn1" exists in public water supply: "tn"
	And a sample site "tn" exists with operating center: "tn1", town: "chat", premise number: "42", public water supply: "tn", sample site profile: "tn"
	And a facility "tn-parent-sample-site-facility" exists with operating center: "tn1", town: "chat"
	And a sample site "tn-parent-sample-site" exists with operating center: "tn1", town: "chat", facility: "tn-parent-sample-site-facility", location type: "primary", premise number: "42"
	And a sample site "TN" exists with state: "tn", is lims location: true, operating center: "tn1", town: "chat", public water supply: "tn", sample site profile: "tn", premise number: "42", is compliance sample site: true
	And I am logged in as "admin"
	When I visit the Edit page for sample site "TN"
	And I select facility "tn-parent-sample-site-facility" from the Facility dropdown
	And I select "No" from the LeadCopperSite dropdown
	And I enter "foo" into the LocationNameDescription field
	And I enter "ATF" into the AgencyId field
	And I enter "f123" into the LimsFacilityId field
	And I enter "s123" into the LimsSiteId field
	And I press Save
	And I wait for the page to reload
	Then I should see a display for LimsFacilityId with "f123"
	Then I should see a display for LimsSiteId with "s123"

Scenario: user can search for a sample site for valve and see it filtered by operating center
	Given a sample site "nj7" exists with valve: "nj7", operating center: "nj7", town: "two"
	And a sample site "nj4" exists with valve: "nj4", operating center: "nj4", town: "one"
	And I am logged in as "user"
	When I visit the WaterQuality/SampleSite/Search page
	And I select "NJ" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select town "two" from the Town dropdown
	And I wait for ajax to finish loading
	Then I should see "Valve"
	And I should see "-- Select --" in the Valve dropdown
	And I should see valve "nj7" in the Valve dropdown
	And I should not see valve "nj4" in the Valve dropdown
	When I press Search
	Then I should see a link to the Show page for sample site: "nj7"
	Then I should not see a link to the Show page for sample site: "nj4"
	When I follow the Show link for sample site "nj7"
	Then I should be at the Show page for sample site: "nj7"

Scenario: user can search for a sample site for valve with no valve selected
	Given a sample site "nj7" exists with valve: "nj7", operating center: "nj7", town: "two"
	And a sample site "nj4" exists with valve: "nj4", operating center: "nj4", town: "one"
	And I am logged in as "user"
	When I visit the WaterQuality/SampleSite/Search page
	And I select "NJ" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I wait for ajax to finish loading
	Then I should see "Valve"
	And I should not see "Please select a town above." in the Valve dropdown
	When I press Search
	Then I should see a link to the Show page for sample site: "nj7"
	When I follow the Show link for sample site "nj7"
	Then I should be at the Show page for sample site: "nj7"

Scenario: user can search for a sample site for hydrant and see it filtered by operating center
	Given a sample site "nj7" exists with hydrant: "nj7", operating center: "nj7", town: "two"
	And a sample site "nj4" exists with hydrant: "nj4", operating center: "nj4", town: "one"
	And I am logged in as "user"
	When I visit the WaterQuality/SampleSite/Search page
	And I select "NJ" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select town "two" from the Town dropdown
	And I wait for ajax to finish loading
	Then I should see "Hydrant"
	And I should see "-- Select --" in the Hydrant dropdown
	And I should see hydrant "nj7" in the Hydrant dropdown
	And I should not see hydrant "nj4" in the Hydrant dropdown
	When I press Search
	Then I should see a link to the Show page for sample site: "nj7"
	Then I should not see a link to the Show page for sample site: "nj4"
	When I follow the Show link for sample site "nj7"
	Then I should be at the Show page for sample site: "nj7"

Scenario: user can search for a sample site for hydrant with no hydrant selected
	Given a sample site "nj7" exists with hydrant: "nj7", operating center: "nj7", town: "two"
	And a sample site "nj4" exists with hydrant: "nj4", operating center: "nj4", town: "one"
	And I am logged in as "user"
	When I visit the WaterQuality/SampleSite/Search page
	And I select "NJ" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I wait for ajax to finish loading
	Then I should see "Hydrant"
	And I should not see "Please select a town above." in the Hydrant dropdown
	When I press Search
	Then I should see a link to the Show page for sample site: "nj7"
	When I follow the Show link for sample site "nj7"
	Then I should be at the Show page for sample site: "nj7"

Scenario: user can search for a sample site with multiple collection types
	Given a sample site "nj7" exists with sample site collection type: "entry point"
	And a sample site "nj4" exists with sample site collection type: "raw"
	And I am logged in as "user"
	When I visit the WaterQuality/SampleSite/Search page
	And I select sample site collection type "raw" from the CollectionType multiselect
	When I press Search
	And I wait for ajax to finish loading
	Then I should not see a link to the Show page for sample site: "nj7"
	Then I should see a link to the Show page for sample site: "nj4"
	When I follow the Show link for sample site "nj4"
	Then I should be at the Show page for sample site: "nj4"

Scenario: user should see the service record tab when sample site address location type is premise
	Given a sample site "x" exists with sample site address location type: "premise"
	And I am logged in as "user"
	And I am at the show page for sample site: "x"
	Then I should see the "Service Record" tab
