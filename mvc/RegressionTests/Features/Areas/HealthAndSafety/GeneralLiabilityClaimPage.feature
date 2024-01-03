Feature: GeneralLiabilityClaimPage
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: users exist and such
	Given a liability type "one" exists
	And a claims representative "one" exists with description: "Ned Ryerson"
	And a state "one" exists with name: "New Jersey", abbreviation: "NJ"	
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsburghy", state: "one"
	And an operating center "nj4" exists with opcode: "NJ4", name: "Lakewood", state: "one"
	And a town "lazytown" exists
	And operating center: "nj7" exists in town: "lazytown"
	And a role "roleReadNj7" exists with action: "Read", module: "OperationsHealthAndSafety", operating center: "nj7"
	And a role "roleEditNj7" exists with action: "Edit", module: "OperationsHealthAndSafety", operating center: "nj7"
	And a role "roleAddNj7" exists with action: "Add", module: "OperationsHealthAndSafety", operating center: "nj7"
	And a role "roleDeleteNj7" exists with action: "Delete", module: "OperationsHealthAndSafety", operating center: "nj7"
	And a role "roleReadNj4" exists with action: "Read", module: "OperationsHealthAndSafety", operating center: "nj4"
	And a role "roleEditNj4" exists with action: "Edit", module: "OperationsHealthAndSafety", operating center: "nj4"
	And a role "roleDeleteNj4" exists with action: "Delete", module: "OperationsHealthAndSafety", operating center: "nj4"
	And a role "roleAddNj4" exists with action: "Add", module: "OperationsHealthAndSafety", operating center: "nj4"
	And a user "user" exists with username: "user"
	And a user "read-only-nj7" exists with username: "read-only-nj7", roles: roleReadNj7
	And a user "user-admin-nj7" exists with username: "user-admin-nj7", roles: roleReadNj7;roleEditNj7;roleAddNj7;roleDeleteNj7
	And a user "read-only-nj4" exists with username: "read-only-nj4", roles: roleReadNj4
    And a user "user-admin-nj4" exists with username: "user-admin-nj4", full name: "user admin nj4", roles: roleReadNj4;roleEditNj4;roleAddNj4;roleDeleteNj4
	And a user "user-admin-both" exists with username: "user-admin-both", roles: roleReadNj4;roleEditNj4;roleAddNj4;roleDeleteNj4;roleReadNj7;roleEditNj7;roleAddNj7;roleDeleteNj7
	And an employee status "active" exists with description: "Active"
	And a general liability claim type "preventable" exists with description: "Preventable"
    And a general liability claim type "non-preventable" exists with description: "Non-Preventable"
	And a crash type "frontal" exists with description: "Frontal"
	And a crash type "rear-end" exists with description: "Rear-End"
    And a crash type "other" exists with description: "Other"
	And an employee "one" exists with first name: "Theodore", last name: "Logan", employee id: "11111111", operating center: "nj4", status: "active"
	And an employee "supervisor" exists with operating center: "nj4", status: "active"
		
Scenario: user without role cannot access the search/index/new/edit/show pages
	Given a general liability claim "one" exists with description: "this is never reached"
	And I am logged in as "user"
	When I visit the HealthAndSafety/GeneralLiabilityClaim/Search page
	Then I should see "You do not have the roles necessary to access this resource."
	And I should see "OperationsHealthAndSafety"
	When I visit the HealthAndSafety/GeneralLiabilityClaim/ page
	Then I should see "You do not have the roles necessary to access this resource."
	And I should see "OperationsHealthAndSafety"
	When I visit the HealthAndSafety/GeneralLiabilityClaim/New page
	Then I should see "You do not have the roles necessary to access this resource."
	And I should see "OperationsHealthAndSafety"
	When I visit the Show page for general liability claim: "one"
	Then I should see "You do not have the roles necessary to access this resource."
	And I should see "OperationsHealthAndSafety"
	When I visit the Edit page for general liability claim: "one"
	Then I should see "You do not have the roles necessary to access this resource."
	And I should see "OperationsHealthAndSafety"

Scenario: User with rights can view a claim and not view another operating centers claim
	Given a general liability claim "one" exists with description: "this is my description.", operating center: "nj7"
	And a general liability claim "two" exists with description: "This is my description. There are many like it, but this one is MINE. My description is my best friend. It is my life. I must master it as I must master my life. Without me, my description is useless. Without my description, I am useless.", operating center: "nj4"
	And I am logged in as "read-only-nj7"
	When I visit the Show page for general liability claim: "one"
	Then I should see a display for Description with "this is my description."
	When I try to access the Show page for general liability claim: "two" expecting an error
	Then I should see a 404 error message

Scenario: User can create a claim
	Given a coordinate "one" exists 
	And I am logged in as "user-admin-nj4"
	When I visit the /HealthAndSafety/GeneralLiabilityClaim/New page
	And I press Save
	Then I should see the validation message "The District field is required."
	And I should see the validation message "The ClaimsRepresentative field is required."
	And I should see the validation message "The LiabilityType field is required."
	And I should see the validation message "The Coordinate field is required."
	And I should see the validation message "The Description field is required."
	And I should see the validation message "The IncidentDateTime field is required."
	And I should see the validation message "The ReportedBy field is required."
	And I should see the validation message "The IncidentNotificationDate field is required."
	And I should see the validation message "The GeneralLiabilityClaimType field is required."
	And I should see the validation message "The CrashType field is required."
	And I should not see the validation message "The PoliceDepartment field is required."
	And I should not see the validation message "The PoliceCaseNumber field is required."
	When I select operating center "nj4" from the OperatingCenter dropdown
	And I select "Yes" from the PoliceCalled dropdown
	And I select general liability claim type "preventable" from the GeneralLiabilityClaimType dropdown
	And I select crash type "frontal" from the CrashType dropdown
	And I press Save
	Then I should see the validation message "The CompanyContact field is required."
	And I should see the validation message "The PoliceDepartment field is required."
	And I should see the validation message "The PoliceCaseNumber field is required."
	When I select liability type "one" from the LiabilityType dropdown
	And I select employee "one"'s Description from the CompanyContact dropdown
	And I select claims representative "one" from the ClaimsRepresentative dropdown
	And I enter "1993-02-02-001" into the ClaimNumber field
	And I enter coordinate "one"'s Id into the Coordinate field
	And I enter "this is my description. there are many like it." into the Description field
	And I enter "Phil Connors" into the ReportedBy field
	And I enter "2/2/1993 06:00" into the IncidentDateTime field
	And I enter "2/3/1993" into the IncidentNotificationDate field
	And I enter "2/4/1993" into the IncidentReportedDate field
	And I enter "Punxsutawney" into the PoliceDepartment field
	And I enter "38" into the PoliceCaseNumber field
    And I click ok in the dialog after pressing "Save"
	#And I wait for the page to reload
	Then the currently shown general liability claim shall henceforth be known throughout the land as "one"
	And I should see a display for CompanyContact with "Theodore Logan"
	And I should see a display for OperatingCenter with "NJ4 - Lakewood"
	And I should see a display for ClaimsRepresentative with claims representative "one"'s ToString
	And I should see a display for ClaimNumber with "1993-02-02-001"
	And I should see a display for Description with "this is my description. there are many like it."
	And I should see a display for ReportedBy with "Phil Connors"
	And I should see a display for IncidentDateTime with "2/2/1993 6:00 AM"
	And I should see a display for IncidentNotificationDate with "2/3/1993 12:00 AM"
	And I should see a display for IncidentReportedDate with "2/4/1993 12:00 AM"
	And I should see a display for LiabilityType with liability type "one"'s ToString
	And I should see a display for PoliceDepartment with "Punxsutawney"
	And I should see a display for PoliceCaseNumber with "38"
	And I should see a display for CreatedBy with "user admin nj4"

Scenario: User can download pdf
	Given an general liability claim "one" exists with operating center: "nj4"
	And I am logged in as "read-only-nj4"
	When I visit the show page for general liability claim: "one"
	Then I should be able to download general liability claim "one"'s pdf

Scenario: User can update a claim
	Given a general liability claim "one" exists with description: "this is my description."
	And a liability type "two" exists
	And a coordinate "two" exists
	And I am logged in as "user-admin-both"
	When I visit the Show page for general liability claim: "one"
	And I follow "Edit"
	And I select operating center "nj4" from the OperatingCenter dropdown
	And I select employee "one"'s Description from the CompanyContact dropdown
	And I select claims representative "one" from the ClaimsRepresentative dropdown
	And I enter "1994-02-02-001" into the ClaimNumber field
	And I select liability type "two" from the LiabilityType dropdown
	And I select general liability claim type "preventable" from the GeneralLiabilityClaimType dropdown
	And I select crash type "other" from the CrashType dropdown
	And I check the MeterBox field
	And I check the CurbValveBox field
	And I check the Excavation field
	And I check the Barricades field
	And I check the Vehicle field
	And I check the WaterMeter field
	And I check the FireHydrant field
	And I check the Backhoe field
	And I check the WaterQuality field
	And I check the WaterPressure field
	And I check the WaterMain field
	And I check the ServiceLine field
	And I enter "this is my description. there are many like it." into the Description field
	And I enter "Jon Snow" into the Name field
	And I enter "4390116" into the PhoneNumber field
	And I enter "1 Castle Black, The Wall, The North" into the Address field
	And I enter "knowsnothing@gmail.com" into the Email field
	And I enter "driver name" into the DriverName field
	And I enter "driver phone" into the DriverPhone field
	And I select "Yes" from the PhhContacted dropdown
	And I enter "other driver" into the OtherDriver field
	And I enter "od phone" into the OtherDriverPhone field
	And I enter "od address" into the OtherDriverAddress field
	And I enter "location of incident" into the LocationOfIncident field
	And I enter "2/2/1994 06:00" into the IncidentDateTime field
	And I enter "1913" into the VehicleYear field
	And I enter "Ford" into the VehicleMake field
	And I enter "1111-1111" into the VehicleVin field
	And I enter "license number" into the LicenseNumber field
	And I select "Yes" from the PoliceCalled dropdown
	And I enter "Punxsutawney" into the PoliceDepartment field
	And I enter "39" into the PoliceCaseNumber field
	And I select "Yes" from the WitnessStatement dropdown
	And I enter "witness" into the Witness field
	And I enter "witness phone" into the WitnessPhone field
	And I select "Yes" from the AnyInjuries dropdown
	And I enter "Phil Connors" into the ReportedBy field
	And I enter "1234567890" into the ReportedByPhone field
	And I enter coordinate "two"'s Id into the Coordinate field
	And I enter "2/3/1994" into the IncidentNotificationDate field
	And I enter "2/4/1994" into the IncidentReportedDate field
	And I press Save
	#And I wait for the page to reload
	Then I should see the validation message "The Describe Other field is required."
	When I enter "Other Type of Crash description." into the OtherTypeOfCrash field
	And I press Save
	Then I should see a display for OperatingCenter with "NJ4 - Lakewood"
	And I should see a display for CompanyContact with "Theodore Logan"
	And I should see a display for ClaimsRepresentative with claims representative "one"'s ToString
	And I should see a display for ClaimNumber with "1994-02-02-001"
	And I should see a display for LiabilityType with liability type "two"'s ToString
	And I should see a display for GeneralLiabilityClaimType with "preventable"
	And I should see a display for MeterBox with "Yes"
	And I should see a display for CurbValveBox with "Yes"
	And I should see a display for Excavation with "Yes"
	And I should see a display for Barricades with "Yes"
	And I should see a display for Vehicle with "Yes"
	And I should see a display for WaterMeter with "Yes"
	And I should see a display for FireHydrant with "Yes"
	And I should see a display for Backhoe with "Yes"
	And I should see a display for WaterQuality with "Yes"
	And I should see a display for WaterPressure with "Yes"
	And I should see a display for WaterMain with "Yes"
	And I should see a display for ServiceLine with "Yes"
	And I should see a display for Description with "this is my description. there are many like it."
	And I should see a display for Name with "Jon Snow"
	And I should see a display for PhoneNumber with "4390116"
	And I should see a display for Address with "1 Castle Black, The Wall, The North"
	And I should see a display for Email with "knowsnothing@gmail.com"
	And I should see a display for DriverName with "driver name"
	And I should see a display for DriverPhone with "driver phone"
	And I should see a display for PhhContacted with "Yes"
	And I should see a display for OtherDriver with "other driver"
	And I should see a display for OtherDriverPhone with "od phone"
	And I should see a display for OtherDriverAddress with "od address"
	And I should see a display for LocationOfIncident with "location of incident"
	And I should see a display for IncidentDateTime with "2/2/1994 6:00 AM"
	And I should see a display for VehicleYear with "1913"
	And I should see a display for VehicleMake with "Ford"
	And I should see a display for VehicleVin with "1111-1111"
	And I should see a display for LicenseNumber with "license number"
	And I should see a display for Witness with "witness"
	And I should see a display for WitnessPhone with "witness phone"
	And I should see a display for PoliceCalled with "Yes"
	And I should see a display for PoliceDepartment with "Punxsutawney"
	And I should see a display for PoliceCaseNumber with "39"
	And I should see a display for WitnessStatement with "Yes"
	And I should see a display for AnyInjuries with "Yes"
	And I should see a display for ReportedBy with "Phil Connors"
	And I should see a display for ReportedByPhone with "1234567890"
	And I should see a display for IncidentNotificationDate with "2/3/1994 12:00 AM"
	And I should see a display for IncidentReportedDate with "2/4/1994 12:00 AM"
	And I should see a display for CreatedBy with "Factory Test"

Scenario: User can delete a claim
	Given a general liability claim "one" exists with description: "this is my description."
	And I am logged in as "user-admin-both"
	When I visit the Show page for general liability claim: "one"
	And I click ok in the dialog after pressing "Delete"
	Then I should be at the HealthAndSafety/GeneralLiabilityClaim/Search page
	When I try to access the Show page for general liability claim: "one" expecting an error
	Then I should see a 404 error message

	Scenario: User can search by state and Operating center multiselect based on state
	Given a general liability claim "one" exists with description: "this is my description."
	And I am logged in as "user-admin-both"
	When I visit the HealthAndSafety/GeneralLiabilityClaim/Search page
	And I select state "one" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I press Search

Scenario: user should not see five whys tab when five whys is false
	Given a general liability claim "one" exists with description: "this is my description."
	And I am logged in as "user-admin-both"
	When I visit the Show page for general liability claim: "one"
	Then I should not see the "Five Whys" tab

Scenario: user should see five whys tab when five whys is true
	Given a general liability claim "one" exists with description: "this is my description.", FiveWhysCompleted: true
	And I am logged in as "user-admin-both"
	When I visit the Show page for general liability claim: "one"
	Then I should see the "Five Whys" tab

Scenario: User edits an general liability claim and adds the five whys
	Given a general liability claim "one" exists with description: "this is my description."
	And a liability type "two" exists
	And a coordinate "two" exists
	And I am logged in as "user-admin-both"
	When I visit the Show page for general liability claim: "one"
	And I follow "Edit"
	And I select operating center "nj4" from the OperatingCenter dropdown
	And I select employee "one"'s Description from the CompanyContact dropdown
	And I click the "Five Whys" tab
	And I select "Yes" from the FiveWhysCompleted dropdown
	When I enter "I fell onto the ground" into the Why1 field
	And I enter "I slipped on my hoagie" into the Why2 field
	And I enter "I tried to punt kick my hoagie but i missed" into the Why3 field
	And I enter "the hoagie had sundried tomatoes" into the Why4 field
	And I enter "I am allergic to sundried tomatoes" into the Why5 field
	When I press "Save"
	Then I should see the validation message "This field is required when 'Five Whys Completed' is Yes."
	When I enter "2/21/2023" into the DateSubmitted field
	And  I press "Save"
	Then I should see the "Five Whys" tab
	When I click the "Five Whys" tab
	Then I should see a display for Why1 with "I fell onto the ground"
	And I should see a display for Why2 with "I slipped on my hoagie"
	And I should see a display for Why3 with "I tried to punt kick my hoagie but i missed"
	And I should see a display for Why4 with "the hoagie had sundried tomatoes"
	And I should see a display for Why5 with "I am allergic to sundried tomatoes"
	And I should see a display for DateSubmitted with "2/21/2023"
