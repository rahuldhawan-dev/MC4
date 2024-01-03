Feature: MainCrossingPage
	Pass the cranberry sauce
	We're having mashed potatoes!
	Oh the turkey looks great

Background: users and supporting data exists
	Given a user "user" exists with username: "user"
	And a role "roleRead" exists with action: "Read", module: "FieldServicesAssets", user: "user"
	And a role "roleEdit" exists with action: "Edit", module: "FieldServicesAssets", user: "user"
	And a role "roleAdd" exists with action: "Add", module: "FieldServicesAssets", user: "user"
	And a role "roleDelete" exists with action: "Delete", module: "FieldServicesAssets", user: "user"
	And an operating center "one" exists with opcode: "NJ7", name: "Shrewsbury", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
	And a state "one" exists with name: "New Jersey", abbreviation: "NJ", scada table: "njscada"
	And a county "one" exists with name: "Monmouth", state: "one"
	And a town "one" exists with name: "Loch Arbour", county: "one"
	And operating center: "one" exists in town: "one"
	And a street "one" exists with town: "one", is active: true
	And a street "foo" exists with town: "one", full st name: "Foo St", is active: true
	And a body of water "one" exists with name: "crystal lake", operating center: "one"
	And public water supply statuses exist
	And a public water supply "one" exists with identifier: "NJ00001", operating center: "one", status: "active"
	And operating center: "one" exists in public water supply: "one"
	And a pipe diameter "one" exists with diameter: "12.01"
	And a pipe material "one" exists with description: "Cast Iron"
	And a customer range "one" exists with description: "< 50"
	And a pressure zone "one" exists with description: "middletown sub"
	And a support structure "one" exists with description: "timber"
	And a crossing category "one" exists with description: "Stream"
	And a crossing category "two" exists with description: "Railroad"
	And a crossing category "thre" exists with description: "Highway"
	And a construction type "one" exists with description: "Unknown"
	And a crossing type "one" exists with description: "Subsurface"
	And a recurring frequency unit "year" exists with description: "Year"
	And a recurring frequency unit "month" exists with description: "Month"
	And a main crossing status "one" exists with description: "In Service"
	And an asset category "one" exists with description: "Asset Cat"
	And a main crossing consequence of failure type "one" exists with description: "Failure"
	And a typical operating pressure range "one" exists  
	And a pressure surge potential type "one" exists 

Scenario: user with readonly access should not be able to add/edit/delete documents/notes
	Given a user "readonly" exists with username: "readonly", roles: roleRead
	And a main crossing "one" exists with length of segment: "100.01", stream: "one", operating center: "one", town: "one"
	And I am logged in as "readonly"
	When I visit the Show page for main crossing: "one"
	And I click the "Notes" tab
	Then I should not see the toggleNewNote element
	When I click the "Documents" tab
	Then I should not see the toggleLinkDocument element
	And I should not see the toggleNewDocument element

Scenario: user can view main crossing
	Given a main crossing "one" exists with length of segment: "100.01", stream: "one", operating center: "one", town: "one"
	And I am logged in as "user"
	When I visit the Show page for main crossing: "one"
	Then I should see a display for main crossing: "one"'s LengthOfSegment

Scenario: user can add a main crossing
	Given I am logged in as "user"
	When I visit the Facilities/MainCrossing/New page
	And I press Save
	Then I should see the validation message The Crossing Category field is required.
	When I select operating center "one"'s Name from the OperatingCenter dropdown
	And I select town "one"'s ShortName from the Town dropdown
	And I press Save
	Then I should see a validation message for ClosestCrossStreet with "The Closest Cross Street field is required."
	When I select "Yes" from the IsCompanyOwned dropdown
	And I select asset category "one" from the AssetCategory dropdown
	And I select crossing category "one"'s Description from the CrossingCategory dropdown
	And I select body of water "one"'s Name from the BodyOfWater dropdown
	And I select public water supply "one"'s Description from the PWSID dropdown
	And I select pipe diameter "one"'s Diameter from the MainDiameter dropdown
	And I select pipe material "one"'s Description from the MainMaterial dropdown
	And I enter "10.1" into the LengthOfSegment field
	And I select street "one"'s FullStName from the ClosestCrossStreet dropdown
	And I select street "foo"'s FullStName from the Street dropdown
	And I select "Yes" from the IsCriticalAsset dropdown
	And I enter "101" into the MaximumDailyFlow field
	And I select customer range "one"'s Description from the CustomerRange dropdown
	And I enter "These are comments" into the Comments field
	And I select pressure zone "one"'s Description from the PressureZone dropdown
	And I select support structure "one"'s Description from the SupportStructure dropdown
	And I select crossing type "one"'s Description from the CrossingType dropdown
	And I select construction type "one"'s Description from the ConstructionType dropdown
	And I enter "42" into the InspectionFrequency field
	And I select recurring frequency unit "year"'s Description from the InspectionFrequencyUnit dropdown
	And I select main crossing status "one"'s Description from the MainCrossingStatus dropdown
	And I select main crossing consequence of failure type "one"'s Description from the ConsequenceOfFailure dropdown
	And I select pressure surge potential type "one"'s Description from the PressureSurgePotentialType dropdown
	And I select typical operating pressure range "one"'s Description from the TypicalOperatingPressureRange dropdown
	And I press Save
	Then I should see a display for OperatingCenter_OperatingCenterCode with "NJ7"
	And I should see a display for Town_ShortName with "Loch Arbour"
	And I should see a display for CrossingCategory with "Stream" 
	And I should see a display for BodyOfWater_Name with "crystal lake"
	And I should see a display for PWSID_Identifier with "NJ00001"
	And I should see a display for MainDiameter_Diameter with "12.01"
	And I should see a display for MainMaterial_Description with "Cast Iron"
	And I should see a display for IsCompanyOwned with "Yes"
	And I should see a display for LengthOfSegment with "10.1"
	And I should see a display for ClosestCrossStreet_FullStName with street "one"'s FullStName
	And I should see a display for Street_FullStName with street "foo"'s FullStName
	And I should see a display for IsCriticalAsset with "Yes"
	And I should see a display for MaximumDailyFlow with "101"
	And I should see a display for CustomerRange with "< 50"
	And I should see a display for Comments with "These are comments"
	And I should see a display for PressureZone with "middletown sub"
	And I should see a display for SupportStructure with "timber"
	And I should see a display for ConstructionType with "Unknown"
	And I should see a display for CrossingType with "Subsurface"
	And I should see a display for InspectionFrequencyDisplay with "42 Year"
	And I should see a display for MainCrossingStatus with main crossing status "one"'s Description
	And I should see a display for AssetCategory with "Asset Cat"
	And I should see a display for PressureSurgePotentialType with pressure surge potential type "one"'s Description
	And I should see a display for TypicalOperatingPressureRange with typical operating pressure range "one"'s Description

Scenario: user can edit a main crossing
	Given a main crossing "one" exists with length of segment: "100.01", stream: "one", operating center: "one", town: "one"
	And an operating center "two" exists with opcode: "NJ4", name: "Lakewood", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
	And a town "two" exists with name: "Allenhurst", county: "one"
	And operating center: "two" exists in town: "two"
	And a public water supply "two" exists with identifier: "NJ00002", operating center: "two", status: "active"
	And operating center: "two" exists in public water supply: "two"
	And a street "two" exists with town: "two", full st name: "Easy St", is active: true
	And a street "bar" exists with town: "two", full st name: "Bar St", is active: true
	And a body of water "two" exists with name: "crystal stream", operating center: "two"
	And a role "roleRead2" exists with action: "Read", module: "FieldServicesAssets", user: "user", operating center: "two"
	And I am logged in as "user"
	When I visit the Show page for main crossing: "one"
	And I follow "Edit"
	And I select "-- Select --" from the CrossingCategory dropdown
	And I press Save
	Then I should see the validation message The Crossing Category field is required.
	When I select operating center "two"'s Name from the OperatingCenter dropdown
	And I select town "two"'s ShortName from the Town dropdown
	And I select "Yes" from the IsCompanyOwned dropdown
	And I select crossing category "one"'s Description from the CrossingCategory dropdown
	And I select body of water "two"'s Name from the BodyOfWater dropdown
	And I select public water supply "two"'s Description from the PWSID dropdown
	And I select pipe diameter "one"'s Diameter from the MainDiameter dropdown
	And I select pipe material "one"'s Description from the MainMaterial dropdown
	And I enter "10.1" into the LengthOfSegment field
	And I select street "two"'s FullStName from the ClosestCrossStreet dropdown
	And I select street "bar"'s FullStName from the Street dropdown
	And I select "Yes" from the IsCriticalAsset dropdown
	And I enter "101" into the MaximumDailyFlow field
	And I select customer range "one"'s Description from the CustomerRange dropdown
	And I enter "These are comments" into the Comments field
	And I select pressure zone "one"'s Description from the PressureZone dropdown
	And I select support structure "one"'s Description from the SupportStructure dropdown
	And I select crossing type "one"'s Description from the CrossingType dropdown
	And I select construction type "one"'s Description from the ConstructionType dropdown
	And I enter "42" into the InspectionFrequency field
	And I select recurring frequency unit "month"'s Description from the InspectionFrequencyUnit dropdown
	And I select pressure surge potential type "one"'s Description from the PressureSurgePotentialType dropdown
	And I select typical operating pressure range "one"'s Description from the TypicalOperatingPressureRange dropdown
	And I press Save
	Then I should see a display for OperatingCenter_OperatingCenterCode with "NJ4"
	And I should see a display for Town_ShortName with "Allenhurst"
	And I should see a display for CrossingCategory with "Stream"
	And I should see a display for BodyOfWater_Name with "crystal stream"
	And I should see a display for PWSID_Identifier with "NJ00002"
	And I should see a display for MainDiameter_Diameter with "12.01"
	And I should see a display for MainMaterial_Description with "Cast Iron"
	And I should see a display for IsCompanyOwned with "Yes"
	And I should see a display for LengthOfSegment with "10.1"
	And I should see a display for ClosestCrossStreet_FullStName with street "two"'s FullStName
	And I should see a display for Street_FullStName with street "bar"'s FullStName
	And I should see a display for IsCriticalAsset with "Yes"
	And I should see a display for MaximumDailyFlow with "101"
	And I should see a display for CustomerRange with "< 50"
	And I should see a display for Comments with "These are comments"
	And I should see a display for PressureZone with "middletown sub"
	And I should see a display for SupportStructure with "timber"
	And I should see a display for ConstructionType with "Unknown"
	And I should see a display for CrossingType with "Subsurface"
	And I should see a display for InspectionFrequencyDisplay with "42 Month"
	And I should see a display for PressureSurgePotentialType with pressure surge potential type "one"'s Description
	And I should see a display for TypicalOperatingPressureRange with typical operating pressure range "one"'s Description

Scenario: Editing a main crossing should not replace the existing inspection frequency values with default values
	Given a main crossing "one" exists with length of segment: "100.01", stream: "one", inspection frequency: "24", inspection frequency unit: "month", operating center: "one", town: "one"
	And I am logged in as "user"
	When I visit the Show page for main crossing: "one"
	Then I should see a display for InspectionFrequencyDisplay with "24 Month"
	When I follow "Edit"	
	Then I should see "24" in the InspectionFrequency field
	And "Month" should be selected in the InspectionFrequencyUnit dropdown 

Scenario: admin can destroy a main crossing
	Given a main crossing "one" exists with length of segment: "100.01", stream: "one", operating center: "one", town: "one"
	And an admin user "admin" exists with username: "admin"
	And I am logged in as "admin"
	When I visit the Show page for main crossing: "one"
	And I click ok in the dialog after pressing "Delete"
	Then I should be at the Facilities/MainCrossing/Search page
	When I try to access the Show page for main crossing: "one" expecting an error
    Then I should see a 404 error message

Scenario: user should see default values for inspection frequency and unit on new page
	Given I am logged in as "user"
	When I visit the Facilities/MainCrossing/New page
	Then I should see "1" in the InspectionFrequency field
	And recurring frequency unit "year"'s ToString should be selected in the InspectionFrequencyUnit dropdown 

Scenario: user can view main crossing inspections in the inspections tab
	Given a main crossing "one" exists with length of segment: "100.01", stream: "one", inspection frequency: "24", inspection frequency unit: "month", operating center: "one", town: "one"
	And a user "billybob" exists with username: "billybob"
	And a main crossing inspection assessment rating "one" exists with description: "Pretty sweet"
	And a main crossing inspection "one" exists with main crossing: "one", assessment rating: "one", inspected by: "billybob", inspected on: "10/3/2014"
	And I am logged in as "user"
	When I visit the Show page for main crossing: "one"
	And I click the "Inspections" tab
	Then I should see a link to the Show page for main crossing inspection "one"
	And I should see the following values in the inspections-table table
         | Inspected By | Inspected On | Assessment Rating |
         | billybob     | 10/3/2014   | Pretty sweet      |

# Yes this is a long scenario, deal with it. It's telling a story about the life of a single record
Scenario: user can add, view, and edit a main crossing inspection
	Given a main crossing "one" exists with length of segment: "100.01", stream: "one", inspection frequency: "24", inspection frequency unit: "month", operating center: "one", town: "one"
	And a main crossing inspection assessment rating "one" exists with description: "Pretty sweet"
	And a main crossing inspection assessment rating "two" exists with description: "Not sweet at all :("
	And I am logged in as "user"
	When I visit the Show page for main crossing: "one"
	And I click the "Inspections" tab
	And I follow "Add Inspection"
	Then I should see a display for DisplayMainCrossing with main crossing "one"'s ToString
	When I select main crossing inspection assessment rating "one" from the AssessmentRating dropdown
	And I enter "These are some comments I have about this" into the Comments field
	And I select "Yes" from the AdjacentFacilityHasBankErosion dropdown
	And I select "No" from the AdjacentFacilityHasBridgeDamage dropdown
	And I select "Yes" from the AdjacentFacilityHasPavementFailure dropdown
	And I select "No" from the AdjacentFacilityOverheadPowerLinesAreDown dropdown
	And I select "Yes" from the AdjacentFacilityHasPropertyDamage dropdown
	And I select "No" from the EnvironmentIsInHazardousLocation dropdown
	And I select "Yes" from the EnvironmentHasDebrisBuildUp dropdown
	And I select "No" from the EnvironmentIsSubmergedInWater dropdown
	And I select "Yes" from the EnvironmentIsExposedToVehicleImpact dropdown
	And I select "No" from the EnvironmentIsNotSecuredFromPublic dropdown
	And I select "Yes" from the EnvironmentIsSusceptibleToStormDamage dropdown
	And I select "No" from the JointsAreLeaking dropdown
	And I select "Yes" from the JointsFailedSeparated dropdown
	And I select "No" from the JointsRestraintDamaged dropdown
	And I select "Yes" from the JointsBondStrapsDamaged dropdown
	And I select "No" from the PipeIsInService dropdown
	And I select "Yes" from the PipeHasExcessiveCorrosion dropdown
	And I select "No" from the PipeHasDelaminatedSteel dropdown
	And I select "Yes" from the PipeIsDamaged dropdown
	And I select "No" from the PipeHasCracks dropdown
	And I select "Yes" from the PipeHasConcreteSpools dropdown
	And I select "No" from the PipeLacksInsulation dropdown
	And I select "Yes" from the SupportsHaveDeficientSupport dropdown
	And I select "No" from the SupportsAreDamaged dropdown
	And I select "Yes" from the SupportsHaveCorrosion dropdown
	And I press Save
	# redirects to main crossing page
	Then I should be at the Show page for main crossing: "one"
	When I click the "Inspections" tab
	And I follow "View"
	Then I should see a link to the Show page for main crossing "one"
	And I should see a display for InspectedBy with user "user"'s UserName
	And I should see a display for AssessmentRating with main crossing inspection assessment rating "one"'s ToString
	And I should see a display for Comments with "These are some comments I have about this"
	And I should see a display for AdjacentFacilityHasBankErosion with "Yes"
	And I should see a display for AdjacentFacilityHasBridgeDamage with "No"
	And I should see a display for AdjacentFacilityHasPavementFailure with "Yes"
	And I should see a display for AdjacentFacilityOverheadPowerLinesAreDown with "No"
	And I should see a display for AdjacentFacilityHasPropertyDamage with "Yes"
	And I should see a display for EnvironmentIsInHazardousLocation with "No"
	And I should see a display for EnvironmentHasDebrisBuildUp with "Yes"
	And I should see a display for EnvironmentIsSubmergedInWater with "No"
	And I should see a display for EnvironmentIsExposedToVehicleImpact with "Yes"
	And I should see a display for EnvironmentIsNotSecuredFromPublic with "No"
	And I should see a display for EnvironmentIsSusceptibleToStormDamage with "Yes"
	And I should see a display for JointsAreLeaking with "No"
	And I should see a display for JointsFailedSeparated with "Yes"
	And I should see a display for JointsRestraintDamaged with "No"
	And I should see a display for JointsBondStrapsDamaged with "Yes"
	And I should see a display for PipeIsInService with "No"
	And I should see a display for PipeHasExcessiveCorrosion with "Yes"
	And I should see a display for PipeHasDelaminatedSteel with "No"
	And I should see a display for PipeIsDamaged with "Yes"
	And I should see a display for PipeHasCracks with "No"
	And I should see a display for PipeHasConcreteSpools with "Yes"
	And I should see a display for PipeLacksInsulation with "No"
	And I should see a display for SupportsHaveDeficientSupport with "Yes"
	And I should see a display for SupportsAreDamaged with "No"
	And I should see a display for SupportsHaveCorrosion with "Yes"
	When I follow "Edit"
	Then main crossing inspection assessment rating "one"'s Description should be selected in the AssessmentRating dropdown
	And I should see "These are some comments I have about this" in the Comments field
	And I should see a display for DisplayMainCrossing with main crossing "one"'s ToString
	And "Yes" should be selected in the AdjacentFacilityHasBankErosion dropdown
	And "No" should be selected in the AdjacentFacilityHasBridgeDamage dropdown
	And "Yes" should be selected in the AdjacentFacilityHasPavementFailure dropdown
	And "No" should be selected in the AdjacentFacilityOverheadPowerLinesAreDown dropdown
	And "Yes" should be selected in the AdjacentFacilityHasPropertyDamage dropdown
	And "No" should be selected in the EnvironmentIsInHazardousLocation dropdown
	And "Yes" should be selected in the EnvironmentHasDebrisBuildUp dropdown
	And "No" should be selected in the EnvironmentIsSubmergedInWater dropdown
	And "Yes" should be selected in the EnvironmentIsExposedToVehicleImpact dropdown
	And "No" should be selected in the EnvironmentIsNotSecuredFromPublic dropdown
	And "Yes" should be selected in the EnvironmentIsSusceptibleToStormDamage dropdown
	And "No" should be selected in the JointsAreLeaking dropdown
	And "Yes" should be selected in the JointsFailedSeparated dropdown
	And "No" should be selected in the JointsRestraintDamaged dropdown
	And "Yes" should be selected in the JointsBondStrapsDamaged dropdown
	And "No" should be selected in the PipeIsInService dropdown
	And "Yes" should be selected in the PipeHasExcessiveCorrosion dropdown
	And "No" should be selected in the PipeHasDelaminatedSteel dropdown
	And "Yes" should be selected in the PipeIsDamaged dropdown
	And "No" should be selected in the PipeHasCracks dropdown
	And "Yes" should be selected in the PipeHasConcreteSpools dropdown
	And "No" should be selected in the PipeLacksInsulation dropdown
	And "Yes" should be selected in the SupportsHaveDeficientSupport dropdown
	And "No" should be selected in the SupportsAreDamaged dropdown
	And "Yes" should be selected in the SupportsHaveCorrosion dropdown
	When I select main crossing inspection assessment rating "two" from the AssessmentRating dropdown
	And I enter "No I didn't like those last comments" into the Comments field
	And I select "No" from the AdjacentFacilityHasBankErosion dropdown
	And I select "Yes" from the AdjacentFacilityHasBridgeDamage dropdown
	And I select "No" from the AdjacentFacilityHasPavementFailure dropdown
	And I select "Yes" from the AdjacentFacilityOverheadPowerLinesAreDown dropdown
	And I select "No" from the AdjacentFacilityHasPropertyDamage dropdown
	And I select "Yes" from the EnvironmentIsInHazardousLocation dropdown
	And I select "No" from the EnvironmentHasDebrisBuildUp dropdown
	And I select "Yes" from the EnvironmentIsSubmergedInWater dropdown
	And I select "No" from the EnvironmentIsExposedToVehicleImpact dropdown
	And I select "Yes" from the EnvironmentIsNotSecuredFromPublic dropdown
	And I select "No" from the EnvironmentIsSusceptibleToStormDamage dropdown
	And I select "Yes" from the JointsAreLeaking dropdown
	And I select "No" from the JointsFailedSeparated dropdown
	And I select "Yes" from the JointsRestraintDamaged dropdown
	And I select "No" from the JointsBondStrapsDamaged dropdown
	And I select "Yes" from the PipeIsInService dropdown
	And I select "No" from the PipeHasExcessiveCorrosion dropdown
	And I select "Yes" from the PipeHasDelaminatedSteel dropdown
	And I select "No" from the PipeIsDamaged dropdown
	And I select "Yes" from the PipeHasCracks dropdown
	And I select "No" from the PipeHasConcreteSpools dropdown
	And I select "Yes" from the PipeLacksInsulation dropdown
	And I select "No" from the SupportsHaveDeficientSupport dropdown
	And I select "Yes" from the SupportsAreDamaged dropdown
	And I select "No" from the SupportsHaveCorrosion dropdown
	And I press Save
	Then I should be at the Show page for main crossing: "one"
	When I click the "Inspections" tab
	And I follow "View"
	Then I should see a link to the Show page for main crossing "one"
	And I should see a display for InspectedBy with user "user"'s UserName
	And I should see a display for AssessmentRating with main crossing inspection assessment rating "two"'s ToString
	And I should see a display for Comments with "No I didn't like those last comments"
	And I should see a display for AdjacentFacilityHasBankErosion with "No"
	And I should see a display for AdjacentFacilityHasBridgeDamage with "Yes"
	And I should see a display for AdjacentFacilityHasPavementFailure with "No"
	And I should see a display for AdjacentFacilityOverheadPowerLinesAreDown with "Yes"
	And I should see a display for AdjacentFacilityHasPropertyDamage with "No"
	And I should see a display for EnvironmentIsInHazardousLocation with "Yes"
	And I should see a display for EnvironmentHasDebrisBuildUp with "No"
	And I should see a display for EnvironmentIsSubmergedInWater with "Yes"
	And I should see a display for EnvironmentIsExposedToVehicleImpact with "No"
	And I should see a display for EnvironmentIsNotSecuredFromPublic with "Yes"
	And I should see a display for EnvironmentIsSusceptibleToStormDamage with "No"
	And I should see a display for JointsAreLeaking with "Yes"
	And I should see a display for JointsFailedSeparated with "No"
	And I should see a display for JointsRestraintDamaged with "Yes"
	And I should see a display for JointsBondStrapsDamaged with "No"
	And I should see a display for PipeIsInService with "Yes"
	And I should see a display for PipeHasExcessiveCorrosion with "No"
	And I should see a display for PipeHasDelaminatedSteel with "Yes"
	And I should see a display for PipeIsDamaged with "No"
	And I should see a display for PipeHasCracks with "Yes"
	And I should see a display for PipeHasConcreteSpools with "No"
	And I should see a display for PipeLacksInsulation with "Yes"
	And I should see a display for SupportsHaveDeficientSupport with "No"
	And I should see a display for SupportsAreDamaged with "Yes"
	And I should see a display for SupportsHaveCorrosion with "No"

Scenario: Admin can edit the inspected by value
	Given a main crossing "one" exists with length of segment: "100.01", stream: "one", inspection frequency: "24", inspection frequency unit: "month", operating center: "one", town: "one"
	And a user "billybob" exists with username: "billybob"
	And an admin user "admin" exists with username: "admin"
	And a main crossing inspection assessment rating "one" exists with description: "Pretty sweet"
	And a main crossing inspection "one" exists with main crossing: "one", assessment rating: "one", inspected by: "billybob", inspected on: "10/3/2014"
	And I am logged in as "user"
	And I am at the Edit page for main crossing inspection: "one"
	Then I should not see the InspectedBy field
	Given I am logged in as "admin"
	And I am at the Edit page for main crossing inspection: "one"
	Then I should see the InspectedBy field

Scenario: User should see a lot of validation for main crossing inspections
	Given a main crossing "one" exists with length of segment: "100.01", stream: "one", inspection frequency: "24", inspection frequency unit: "month", operating center: "one", town: "one"
	And a main crossing inspection assessment rating "one" exists with description: "Pretty sweet"
	And a main crossing inspection assessment rating "two" exists with description: "Not sweet at all :("
	And I am logged in as "user"
	When I visit the Show page for main crossing: "one"
	And I click the "Inspections" tab
	And I follow "Add Inspection"
	And I enter "" into the InspectedOn field
	And I press Save
	Then I should see a validation message for InspectedOn with "The InspectedOn field is required."
	And I should see a validation message for AdjacentFacilityHasBankErosion with "The Has bank erosion field is required."
	And I should see a validation message for AdjacentFacilityHasBridgeDamage with "The Has bridge damage field is required."
	And I should see a validation message for AdjacentFacilityHasPavementFailure with "The Has pavement failure field is required."
	And I should see a validation message for AdjacentFacilityOverheadPowerLinesAreDown with "The Overhead power lines are down field is required."
	And I should see a validation message for AdjacentFacilityHasPropertyDamage with "The Has property damage field is required."
	And I should see a validation message for EnvironmentIsInHazardousLocation with "The Is in hazardous location field is required."
	And I should see a validation message for EnvironmentHasDebrisBuildUp with "The Has debris build up field is required."
	And I should see a validation message for EnvironmentIsSubmergedInWater with "The Is submerged in water field is required."
	And I should see a validation message for EnvironmentIsExposedToVehicleImpact with "The Is exposed to vehicle impact field is required."
	And I should see a validation message for EnvironmentIsNotSecuredFromPublic with "The Is not secured from public field is required."
	And I should see a validation message for EnvironmentIsSusceptibleToStormDamage with "The Is susceptible to storm damage field is required."
	And I should see a validation message for JointsAreLeaking with "The Are leaking field is required."
	And I should see a validation message for JointsFailedSeparated with "The Failed separated field is required."
	And I should see a validation message for JointsRestraintDamaged with "The Restraint damaged field is required."
	And I should see a validation message for JointsBondStrapsDamaged with "The Bond straps damaged field is required."
	And I should see a validation message for PipeIsInService with "The Is in service field is required."
	And I should see a validation message for PipeHasExcessiveCorrosion with "The Has excessive corrosion field is required."
	And I should see a validation message for PipeHasDelaminatedSteel with "The Has delaminated steel field is required."
	And I should see a validation message for PipeIsDamaged with "The Pipe is damaged field is required."
	And I should see a validation message for PipeHasCracks with "The Has cracks field is required."
	And I should see a validation message for PipeHasConcreteSpools with "The Has concrete spools field is required."
	And I should see a validation message for PipeLacksInsulation with "The Lacks insulation field is required."
	And I should see a validation message for SupportsHaveDeficientSupport with "The Have deficient support field is required."
	And I should see a validation message for SupportsAreDamaged with "The Supports are damaged field is required."
	And I should see a validation message for SupportsHaveCorrosion with "The Have corrosion field is required."
	
Scenario: Some field visibility is toggled based on the selected Crossing Category
	Given I am logged in as "user"
	When I visit the Facilities/MainCrossing/New page
	Then I should not see the RailwayOwnerType field
	And I should not see the RailwayCrossingId field
	And I should not see the EmergencyPhoneNumber field
	When I select "Railroad" from the CrossingCategory dropdown
	Then I should see the RailwayOwnerType field
	And I should see the RailwayCrossingId field
	And I should see the EmergencyPhoneNumber field
	When I select "Highway" from the CrossingCategory dropdown
	Then I should not see the RailwayOwnerType field
	And I should not see the RailwayCrossingId field
	And I should not see the EmergencyPhoneNumber field
	
Scenario: User can search for a main crossing
	Given a main crossing "one" exists with length of segment: "100.01", stream: "one", operating center: "one", town: "one"
	And a main crossing "two" exists with length of segment: "100.01", stream: "one", operating center: "one", town: "one", crossing type: "one", support structure: "one"
	Given I am logged in as "user"
	When I visit the Facilities/MainCrossing/Search page
	When I press Search
	Then I should see a link to the Show page for main crossing "one"
	Then I should see a link to the Show page for main crossing "two"
	When I follow the Show link for main crossing "one"
	Then I should be at the Show page for main crossing "one"
	When I visit the Facilities/MainCrossing/Search page
	And I select crossing type "one" from the CrossingType dropdown
	And I select support structure "one" from the SupportStructure dropdown
	When I press Search
	Then I should see "Subsurface" in the "Crossing Type" column
	Then I should see a link to the Show page for main crossing "two"
	Then I should not see a link to the Show page for main crossing "one"
	When I follow the Show link for main crossing "two"
	Then I should be at the Show page for main crossing "two"