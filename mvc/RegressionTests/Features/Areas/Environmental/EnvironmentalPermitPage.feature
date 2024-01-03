Feature: EnvironmentalPermitPage
	Don't Go There : Cautionary Sign Store
	Dr. Rangelove : Stove Store
	Fire Distinguishers : Put out your fire with flair

Background: users and supporting data exists
	Given a user "user" exists with username: "user"
	And a role "roleRead" exists with action: "Read", module: "EnvironmentalGeneral", user: "user"
	And a role "roleEdit" exists with action: "Edit", module: "EnvironmentalGeneral", user: "user"
	And a role "roleAdd" exists with action: "Add", module: "EnvironmentalGeneral", user: "user"
	And a role "roleDelete" exists with action: "Delete", module: "EnvironmentalGeneral", user: "user"
	And a state "one" exists with name: "New Jersey", abbreviation: "NJ"
	And an operating center "nj4" exists with opcode: "NJ4", name: "Lakewood", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true", state: "one"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true", state: "one"
	And an operating center "nj3" exists with opcode: "NJ3", name: "Fire Road", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true", state: "one"
	And public water supply statuses exist
	And waste water system statuses exist
	And a water type "one" exists with description: "Water"
	And a public water supply "one" exists with operating area: "AA", identifier: "1111", status: "active", state: "one", system: "System"
	And operating center: "nj4" exists in public water supply: "one"
	And operating center: "nj7" exists in public water supply: "one"
	And operating center: "nj3" exists in public water supply: "one"
	And a waste water system "one" exists with waste water system name: "Water System 1", status: "active", operating center: "nj7"
	And a waste water system "two" exists with waste water system name: "Water System 2", status: "active", operating center: "nj4"
    And an environmental permit requirement type "one" exists
    And an environmental permit requirement value unit "one" exists
    And an environmental permit requirement value definition "one" exists
    And an environmental permit requirement tracking frequency "one" exists
    And an environmental permit requirement reporting frequency "one" exists
    And an employee "one" exists with operating center: "nj7"
	And communication types exist
	And a role "roleFacilityReadNj4" exists with action: "Read", module: "ProductionFacilities", user: "user", operating center: "nj4"
	And a role "roleFacilityReadNj7" exists with action: "Read", module: "ProductionFacilities", user: "user", operating center: "nj7"
	And a role "roleEquipmentReadNj4" exists with action: "Read", module: "ProductionEquipment", user: "user", operating center: "nj4"
	And a role "roleEquipmentReadNj7" exists with action: "Read", module: "ProductionEquipment", user: "user", operating center: "nj7"

Scenario: user can view environmental permit
	Given an environmental permit type "one" exists with description: "Water Quality"
	And an environmental permit "one" exists with environmental permit type: "one", description: "Give a hoot- dont polute!"
	And I am logged in as "user"
	When I visit the Show page for environmental permit: "one"
	Then I should see a display for environmental permit: "one"'s Description
	And I should see a display for environmental permit: "one"'s EnvironmentalPermitType
	
Scenario: user can view environmental permit log
    Given the test flag "allow audits" exists
	And an environmental permit type "one" exists with description: "Water Quality"
	And an environmental permit "one" exists with environmental permit type: "one", description: "Give a hoot- dont polute!"
	And I am logged in as "user"
	When I visit the Show page for environmental permit: "one"
	Then I should see a display for environmental permit: "one"'s Description
	And I should see a display for environmental permit: "one"'s EnvironmentalPermitType
	When I click the "Log" tab
	Then I should see the following values in the LogTab table
	| User | Entity Name         | Entity | Audit Entry Type | Time Stamp (EST)| Field Name | Old Value | New Value |
	|      | EnvironmentalPermit | 1      | Show             | **              |            |           |           |

Scenario: user can add an environmental permit
	Given an environmental permit type "one" exists with description: "Water Quality"
	And an environmental permit status "one" exists with description: "Active"
	And a facility "one" exists with operating center: "nj4", facility name: "The Facility1"
	And a facility "two" exists with operating center: "nj7", facility name: "The Facility2"
	And a facility "three" exists with operating center: "nj3", facility name: "The Facility3"
	And an equipment "one" exists with facility: "one"
	And I am logged in as "user"
	When I visit the Environmental/EnvironmentalPermit page
	And I follow "Add"
	And I press Save
	Then I should see the validation message "The State field is required."
	And I should see the validation message "The PermitNumber field is required."
	And I should see the validation message "The Permit Type field is required."
	And I should see the validation message "The FacilityType field is required."
	And I should see the validation message "The Effective Date field is required."
	And I should see the validation message "The ReportingRequired field is required."
	When I select state "one" from the State dropdown
	And I check operating center "nj4" in the OperatingCenters checkbox list
	And I check operating center "nj7" in the OperatingCenters checkbox list
	And I press Save
	Then I should see the validation message "Please select either a PWSID or a WWSID"
	When I select waste water system "one"'s Description from the WasteWaterSystem dropdown
	And I press Save
	Then I should not see the validation message "Please select either a PWSID or a WWSID"
	When I select "-- Select --" from the WasteWaterSystem dropdown
	And I press Save
	Then I should see the validation message "Please select either a PWSID or a WWSID"
	When I select public water supply "one"'s Description from the PublicWaterSupply dropdown
	And I press Save
	Then I should not see the validation message "Please select either a PWSID or a WWSID"
	When I select water type "one" from the FacilityType dropdown
	And I select environmental permit type "one" from the EnvironmentalPermitType dropdown
	And I select environmental permit status "one" from the EnvironmentalPermitStatus dropdown
	And I enter "123-6" into the PermitName field
	And I select public water supply "one" from the PublicWaterSupply dropdown
	And I enter "123-4" into the PermitNumber field
	And I enter "123-5" into the ProgramInterestNumber field
	And I enter "123-6" into the PermitCrossReferenceNumber field
	And I enter "2/4/2014" into the PermitEffectiveDate field
	And I enter "2/5/2014" into the PermitRenewalDate field
	And I enter "2/6/2014" into the PermitExpirationDate field
	And I enter "that's a terrible slogan" into the Description field
	And I check the RequiresFees field
	And I select "Yes" from the ReportingRequired dropdown
	And I select "No" from the RequiresRequirements dropdown
	And I select facility "one"'s Description from the Facilities dropdown
	And I select equipment "one"'s Display from the Equipment dropdown
	And I press Save
	Then I should see a display for "EnvironmentalPermitType" with environmental permit type: "one"'s Description
	And I should see a display for "EnvironmentalPermitStatus" with environmental permit status: "one"'s Description
	And I should see a link to the Show page for public water supply: "one"
	And I should see a display for PermitNumber with "123-4"
	And I should see a display for ProgramInterestNumber with "123-5"
	And I should see a display for PermitCrossReferenceNumber with "123-6"
	And I should see a display for PermitEffectiveDate with "2/4/2014"
	And I should see a display for PermitRenewalDate with "2/5/2014"
	And I should see a display for PermitExpirationDate with "2/6/2014"
	And I should see a display for Description with "that's a terrible slogan"
	And I should see a display for RequiresFees with "Yes"
	And I should see a display for ReportingRequired with "Yes"
	And I should see "NJ7 - Shrewsbury"
	And I should see "NJ4 - Lakewood"
	And I should not see "NJ3 - Fire Road"
	And I should see equipment "one"'s Identifier in the table equipmentTable's "Equipment" column
	And I should see facility "one"'s Description in the table facilityTable's "Facility" column

Scenario: user can add an environmental permit with a requirement with validation checks
	Given an environmental permit type "one" exists with description: "Water Quality"
	And an environmental permit status "one" exists with description: "Active"
	And a facility "one" exists with operating center: "nj4"
	And a facility "two" exists with operating center: "nj7"
	And a facility "three" exists with operating center: "nj3"
	And I am logged in as "user"
	When I visit the Environmental/EnvironmentalPermit page
	And I follow "Add"
	And I press Save
	Then I should see the validation message "The State field is required."
	And I should see the validation message "The PermitNumber field is required."
	And I should see the validation message "The Permit Type field is required."
	And I should see the validation message "The FacilityType field is required."
	And I should see the validation message "The Effective Date field is required."
	And I should see the validation message "The ReportingRequired field is required."
	When I select state "one" from the State dropdown
	And I check operating center "nj4" in the OperatingCenters checkbox list
	And I check operating center "nj7" in the OperatingCenters checkbox list
	And I press Save
	Then I should see the validation message "Please select either a PWSID or a WWSID"
	When I select waste water system "one"'s Description from the WasteWaterSystem dropdown
	And I press Save
	Then I should not see the validation message "Please select either a PWSID or a WWSID"
	When I select "-- Select --" from the WasteWaterSystem dropdown
	And I press Save
	Then I should see the validation message "Please select either a PWSID or a WWSID"
	When I select public water supply "one"'s Description from the PublicWaterSupply dropdown
	And I press Save
	Then I should not see the validation message "Please select either a PWSID or a WWSID"
	When I select water type "one" from the FacilityType dropdown
	And I select environmental permit type "one" from the EnvironmentalPermitType dropdown
	And I select environmental permit status "one" from the EnvironmentalPermitStatus dropdown
	And I select public water supply "one" from the PublicWaterSupply dropdown
	And I enter "123-4" into the PermitNumber field
	And I enter "123-5" into the ProgramInterestNumber field
	And I enter "123-6" into the PermitCrossReferenceNumber field
	And I enter "2/4/2014" into the PermitEffectiveDate field
	And I enter "2/5/2014" into the PermitRenewalDate field
	And I enter "2/6/2014" into the PermitExpirationDate field
	And I enter "that's a terrible slogan" into the Description field
	And I select "Yes" from the ReportingRequired dropdown
	And I check operating center "nj4" in the OperatingCenters checkbox list
	And I check operating center "nj7" in the OperatingCenters checkbox list
	And I select "Yes" from the RequiresRequirements dropdown
	And I press Save
	Then I should see the validation message "The RequirementType field is required."
	And I should see the validation message "The Requirement field is required."
	And I should see the validation message "The ValueUnit field is required."
	And I should see the validation message "The ValueDefinition field is required."
	And I should see the validation message "The TrackingFrequency field is required."
	And I should see the validation message "The ReportingFrequency field is required."
	And I should see the validation message "The ReportingOwner field is required."
	When I select environmental permit requirement type "one" from the CreateEnvironmentalPermitRequirement.RequirementType dropdown
    And I enter "foobar" into the CreateEnvironmentalPermitRequirement.Requirement field
    And I select environmental permit requirement value unit "one" from the CreateEnvironmentalPermitRequirement.ValueUnit dropdown
    And I select environmental permit requirement value definition "one" from the CreateEnvironmentalPermitRequirement.ValueDefinition dropdown
    And I select environmental permit requirement tracking frequency "one" from the CreateEnvironmentalPermitRequirement.TrackingFrequency dropdown
    And I select environmental permit requirement reporting frequency "one" from the CreateEnvironmentalPermitRequirement.ReportingFrequency dropdown
    And I select employee "one"'s Description from the CreateEnvironmentalPermitRequirement.ReportingOwner dropdown
	And I enter "this is a reporting detail thingus" into the CreateEnvironmentalPermitRequirement.ReportSendTo field
	And I press Save
	Then I should see the validation message "The CommunicationType field is required."
	When I select communication type "email" from the CreateEnvironmentalPermitRequirement.CommunicationType dropdown
	And I press Save
	Then I should see the validation message "The CommunicationEmail field is required."
	When I select communication type "agency submittal form" from the CreateEnvironmentalPermitRequirement.CommunicationType dropdown
	And I press Save
	Then I should see the validation message "The CommunicationLink field is required."
	When I enter "foo@foo.com" into the CreateEnvironmentalPermitRequirement.CommunicationEmail field
	And I enter "https://foo.foo.com" into the CreateEnvironmentalPermitRequirement.CommunicationLink field
	When I press Save
	Then I should see a display for "EnvironmentalPermitType" with environmental permit type: "one"'s Description
	And I should see a display for "EnvironmentalPermitStatus" with environmental permit status: "one"'s Description
	And I should see a link to the Show page for public water supply: "one"
	And I should see a display for PermitNumber with "123-4"
	And I should see a display for ProgramInterestNumber with "123-5"
	And I should see a display for PermitCrossReferenceNumber with "123-6"
	And I should see a display for PermitEffectiveDate with "2/4/2014"
	And I should see a display for PermitRenewalDate with "2/5/2014"
	And I should see a display for PermitExpirationDate with "2/6/2014"
	And I should see a display for Description with "that's a terrible slogan"
	And I should see a display for ReportingRequired with "Yes"
	And I should see "NJ7 - Shrewsbury"
	And I should see "NJ4 - Lakewood"
	And I should not see "NJ3 - Fire Road"
	When I click the "Requirements" tab
	Then I should see the following values in the requirementsTable table
	| Requirement Type                     | Requirement | Value Unit                                | Value Definition                                | Tracking Frequency                                | Reporting Frequency                                | Communication Type  | Communication Email | Communication Link  |
	| *EnvironmentalPermitRequirementType* | foobar      | *EnvironmentalPermitRequirementValueUnit* | *EnvironmentalPermitRequirementValueDefinition* | *EnvironmentalPermitRequirementTrackingFrequency* | *EnvironmentalPermitRequirementReportingFrequency* | AgencySubmittalForm | foo@foo.com         | https://foo.foo.com |
		
Scenario: user can edit an environmental permit
	Given an environmental permit type "one" exists with description: "Water Quality"
	And an environmental permit status "one" exists with description: "active"
	And an environmental permit "one" exists with description: "Give a hoot- dont polute!", state: "one", facility type: "one"
	And a facility "one" exists with facility id: "NJSB-01"
	And a facility "two" exists with facility id: "NJSB-02"
	And a facility "three" exists with facility id: "NJSB-03"
	And I am logged in as "user"
	When I visit the Show page for environmental permit: "one"
	And I follow "Edit"
	When I select state "one" from the State dropdown
	And I check operating center "nj4" in the OperatingCenters checkbox list
	And I press Save
	Then I should see waste water system "two"'s Description in the WasteWaterSystem dropdown
	When I select environmental permit type "one" from the EnvironmentalPermitType dropdown
	And I select environmental permit status "one" from the EnvironmentalPermitStatus dropdown
	And I select public water supply "one" from the PublicWaterSupply dropdown
	And I enter "123-6 edited" into the PermitName field
	And I enter "123-4" into the PermitNumber field
	And I enter "123-5" into the ProgramInterestNumber field
	And I enter "123-6" into the PermitCrossReferenceNumber field
	And I check the PermitExpires field 
	And I enter "2/4/2014" into the PermitEffectiveDate field
	And I enter "2/5/2014" into the PermitRenewalDate field
	And I enter "that's a terrible slogan" into the Description field
	And I select "No" from the ReportingRequired dropdown
	And I press Save
	Then I should see the validation message "The Expiration Date field is required."
	When I enter "2/6/2014" into the PermitExpirationDate field
	And I press Save
	Then I should be at the Show page for environmental permit: "one"
	And I should see a display for "EnvironmentalPermitType" with environmental permit type: "one"'s Description
	And I should see a display for "EnvironmentalPermitStatus" with environmental permit status: "one"'s Description
	And I should see a link to the Show page for public water supply: "one"
	And I should see a display for PermitNumber with "123-4"
	And I should see a display for ProgramInterestNumber with "123-5"
	And I should see a display for PermitCrossReferenceNumber with "123-6"
	And I should see a display for PermitEffectiveDate with "2/4/2014"
	And I should see a display for PermitRenewalDate with "2/5/2014"
	And I should see a display for PermitExpirationDate with "2/6/2014"
	And I should see a display for Description with "that's a terrible slogan"
	And I should see a display for ReportingRequired with "No"
	And I should see "NJ4 - Lakewood"
	And I should not see "NJ7 - Shrewsbury"
	When I follow "Edit"
	And I uncheck operating center "nj4" in the OperatingCenters checkbox list
	And I check operating center "nj7" in the OperatingCenters checkbox list
	And I select waste water system "one" from the WasteWaterSystem dropdown
	And I press Save
	Then I should not see "NJ4 - Lakewood"
	And I should see "NJ7 - Shrewsbury"
	When I follow "Edit"
	Then the PermitExpires field should be checked
	When I follow "Show"
	When I follow "Edit"
	And I select "Yes" from the RequiresRequirements dropdown
	And I press Save
	Then I should see the validation message "Please enter the requirements from the regular view for this permit before setting this value."

Scenario: user can destroy an environmental permit
	Given an environmental permit type "one" exists with description: "Water Quality"
	And an environmental permit "one" exists with environmental permit type: "one", description: "Give a hoot- dont polute!"
	And I am logged in as "user"
	When I visit the Show page for environmental permit: "one"
	And I click ok in the dialog after pressing "Delete"
	Then I should be at the Environmental/EnvironmentalPermit/Search page
	When I try to access the Show page for environmental permit: "one" expecting an error
	Then I should see a 404 error message

Scenario: user can add and remove facilities and equipment for an environmental permit
	Given an environmental permit type "one" exists with description: "Water Quality"
	And an environmental permit "one" exists with environmental permit type: "one", description: "Give a hoot- dont polute!", state: "one", facility type: "one", public water supply: "one", permit effective date: "today", permit expiration date: "today"
	And operating center: "nj7" exists in environmental permit: "one"
	And a facility "facility" exists with operating center: "nj7", facility name: "The Facility"
	And an equipment "one" exists with identifier: "NJSB-1-EQID-1", facility: "facility", operating center: "nj7"
	And I am logged in as "user"
	When I visit the Edit page for environmental permit: "one"
	And I check operating center "nj7" in the OperatingCenters checkbox list
	And I press Save
	When I visit the Show page for environmental permit: "one"
	And I click the "Facilities" tab
	And I select facility "facility"'s DescriptionWithDepartment from the FacilityId dropdown
	And I press "Add Facility"
	And I wait for the page to reload
	Then I should be at the show page for environmental permit: "one"
	And I should see facility "facility"'s Description in the table facilityTable's "Facility" column
	When I click the "Facilities" tab
	And I click the "Remove Facility" button in the 1st row of facilityTable and then click ok in the confirmation dialog
	And I wait for the page to reload
	And I click the "Facilities" tab
	Then I should not see facility "facility"'s Description in the table facilityTable's "Facility" column
	When I select facility "facility"'s DescriptionWithDepartment from the FacilityId dropdown
	And I press "Add Facility"
	And I wait for the page to reload
	And I click the "Equipment" tab
	And I select equipment "one"'s Display from the EquipmentId dropdown
	And I press "Add Equipment"
	And I wait for the page to reload
	Then I should be at the show page for environmental permit: "one"
	And I should see equipment "one"'s Identifier in the table equipmentTable's "Equipment" column
	When I click the "Equipment" tab
	And I click the "Remove Equipment" button in the 1st row of equipmentTable and then click ok in the confirmation dialog
	And I wait for the page to reload
	And I click the "Equipment" tab
	Then I should not see equipment "one"'s Identifier in the table equipmentTable's "Equipment" column

Scenario: user can add and remove requirements to and from an environmental permit
	Given an environmental permit type "one" exists with description: "Water Quality"
	And an environmental permit status "one" exists with description: "active"
	And an environmental permit "one" exists with environmental permit type: "one", description: "Give a hoot- dont polute!", state: "one"
	And operating center: "nj7" exists in environmental permit: "one"
	And I am logged in as "user"
	When I visit the Show page for environmental permit: "one"
	And I click the "Requirements" tab
    And I press "Add New Requirement"
    And I press "Save Requirement"
	Then I should see the validation message The RequirementType field is required.
	And I should see the validation message The Requirement field is required.
	And I should see the validation message The ValueUnit field is required.
	And I should see the validation message The ValueDefinition field is required.
	And I should see the validation message The TrackingFrequency field is required.
	And I should see the validation message The ReportingFrequency field is required.
	And I should see the validation message The ReportingOwner field is required.
    When I select environmental permit requirement type "one" from the RequirementType dropdown
    And I enter "foobar" into the Requirement field
    And I select environmental permit requirement value unit "one" from the ValueUnit dropdown
    And I select environmental permit requirement value definition "one" from the ValueDefinition dropdown
    And I select environmental permit requirement tracking frequency "one" from the TrackingFrequency dropdown
    And I select environmental permit requirement reporting frequency "one" from the ReportingFrequency dropdown
    And I select employee "one"'s Description from the ReportingOwner dropdown
	And I press "Save Requirement"
    And I wait for the page to reload
    Then I should be at the Show page for environmental permit "one"
    And I should see a link to the /Environmental/EnvironmentalPermitRequirement/Edit/1 page
	When I follow "Edit"
	And I select "Yes" from the RequiresRequirements dropdown
	And I select state "one" from the State dropdown
	And I select water type "one" from the FacilityType dropdown
	And I select environmental permit type "one" from the EnvironmentalPermitType dropdown
	And I select environmental permit status "one" from the EnvironmentalPermitStatus dropdown
	And I select public water supply "one" from the PublicWaterSupply dropdown
	And I enter "123-4" into the PermitNumber field
	And I enter "123-5" into the ProgramInterestNumber field
	And I enter "123-6" into the PermitCrossReferenceNumber field
	And I enter "2/4/2014" into the PermitEffectiveDate field
	And I enter "2/5/2014" into the PermitRenewalDate field
	And I enter "2/6/2014" into the PermitExpirationDate field
	And I enter "that's a terrible slogan" into the Description field
	And I select "Yes" from the ReportingRequired dropdown
	And I press Save
	Then I should not see the validation message "Please enter the requirements from the regular view for this permit before setting this value."
	When I click the "Requirements" tab
	And I press "Remove"
    And I wait for the page to reload
	Then I should see "You cannot remove all the requirements when the permit has requirements."
	When I follow "Edit"
	And I select "No" from the RequiresRequirements dropdown
	And I press Save
	And I wait for the page to reload
	And I click the "Requirements" tab
	And I press "Remove"
	And I wait for the page to reload
    Then I should be at the Show page for environmental permit "one"
    And I should not see a link to the /Environmental/EnvironmentalPermitRequirement/Edit/1 page

Scenario: The process owner and reporting owner dropdowns correctly cascade off of operating center
	Given an environmental permit type "one" exists with description: "Water Quality"
	And an environmental permit status "one" exists with description: "Active"
	And a facility "one" exists with operating center: "nj4"
	And a facility "two" exists with operating center: "nj7"
	And a facility "three" exists with operating center: "nj3"
	And an employee "two" exists with operating center: "nj4"
	And I am logged in as "user"
	When I visit the Environmental/EnvironmentalPermit/New page
	And I check operating center "nj7" in the OperatingCenters checkbox list
	And I select "Yes" from the RequiresRequirements dropdown
	And I click the "Requirements" tab
	Then I should see employee "one"'s Description in the CreateEnvironmentalPermitRequirement_ProcessOwner dropdown
	And I should see employee "one"'s Description in the CreateEnvironmentalPermitRequirement_ReportingOwner dropdown
	And I should not see employee "two"'s Description in the CreateEnvironmentalPermitRequirement_ProcessOwner dropdown
	And I should not see employee "two"'s Description in the CreateEnvironmentalPermitRequirement_ReportingOwner dropdown
	When I click the "General" tab
	And I uncheck operating center "nj7" in the OperatingCenters checkbox list
	And I check operating center "nj4" in the OperatingCenters checkbox list
	And I click the "Requirements" tab
	Then I should not see employee "one"'s Description in the CreateEnvironmentalPermitRequirement_ProcessOwner dropdown
	And I should not see employee "one"'s Description in the CreateEnvironmentalPermitRequirement_ReportingOwner dropdown
	And I should see employee "two"'s Description in the CreateEnvironmentalPermitRequirement_ProcessOwner dropdown
	And I should see employee "two"'s Description in the CreateEnvironmentalPermitRequirement_ReportingOwner dropdown

Scenario: User should not see fees tab if permit does not require fees
	Given an environmental permit type "one" exists with description: "Water Quality"
	And an environmental permit "one" exists with environmental permit type: "one", description: "Give a hoot- dont polute!", requires fees: "false"
	And an environmental permit "two" exists with environmental permit type: "one", description: "Give a twoot- dont twolute!", requires fees: "true"
	And I am logged in as "user"
	When I visit the Show page for environmental permit: "one"
	Then I should not see the "Fees" tab
	When I visit the Show page for environmental permit: "two"
	Then I should see the "Fees" tab

Scenario: User without edit role can not add, edit, or delete fees to/from permit
	Given a user "add-only-user" exists with username: "add-only-user"
	And a role exists with action: "Read", module: "EnvironmentalGeneral", user: "add-only-user"
	And a role exists with action: "Add", module: "EnvironmentalGeneral", user: "add-only-user"
	And an environmental permit "one" exists with requires fees: "true"
	And an environmental permit fee "one" exists with environmental permit: "one"
	And I am logged in as "add-only-user"
	When I visit the Show page for environmental permit: "one"
	And I click the "Fees" tab
	Then I should not see the button "Add Fee"
	And I should not see a link to the Edit page for environmental permit fee "one"
	And I should not see the button "Delete"

Scenario: User with edit role should be able to add, edit, and delete fees to/from a permit that requires fees
	Given an environmental permit "one" exists with requires fees: "true"
	And a mail environmental permit fee payment method "mail" exists
	And I am logged in as "user"
	When I visit the Show page for environmental permit: "one"
	And I click the "Fees" tab
	And I press "Add Fee"
	And I press "Save Fee"
	Then I should see a validation message for Fee with "The Fee field is required."
	And I should see a validation message for PaymentMethod with "The PaymentMethod field is required."
	When I enter "424.84" into the Fee field
	And I select "Mail" from the PaymentMethod dropdown
	And I press "Save Fee"
	Then I should see a validation message for PaymentMethodMailAddress with "The PaymentMethodMailAddress field is required."
	When I enter "123 Fake St" into the PaymentMethodMailAddress field
	And I press "Save Fee"
	And I click the "Fees" tab
	Then I should see the following values in the fees-table table
         | Fee     | Payment Method |
         | $424.84 | 123 Fake St    |
	When I click the "Edit" link in the 1st row of fees-table
	And I enter "421.34" into the Fee field
	And I press "Save Fee"
	And I click the "Fees" tab
	Then I should see the following values in the fees-table table
         | Fee     | Payment Method |
         | $421.34 | 123 Fake St    |
	When I click the "Delete" button in the 1st row of fees-table and then click ok in the confirmation dialog
	And I wait for the page to reload
	And I click the "Fees" tab
	Then the fees-table table should be empty 

Scenario: User should see different payment fields based on payment method for both editing and displaying
	Given an environmental permit "one" exists with requires fees: "true"
	And a mail environmental permit fee payment method "mail" exists
	And a phone environmental permit fee payment method "phone" exists
	And a url environmental permit fee payment method "url" exists
	And I am logged in as "user"
	When I visit the Show page for environmental permit: "one"
	And I click the "Fees" tab
	And I press "Add Fee"
	And I enter "424.84" into the Fee field

	# Test Mail
	And I select "Mail" from the PaymentMethod dropdown
	And I press "Save Fee"
	Then I should see a validation message for PaymentMethodMailAddress with "The PaymentMethodMailAddress field is required."
	And I should not see a validation message for PaymentMethodPhone with "The PaymentMethodPhone field is required." 
	And I should not see a validation message for PaymentMethodUrl with "The PaymentMethodUrl field is required." 
	When I enter "123 Fake St" into the PaymentMethodMailAddress field
	And I press "Save Fee"
	And I click the "Fees" tab
	Then I should see the following values in the fees-table table
         | Payment Method |
         | 123 Fake St    |

	# Test Phone
	When I click the "Edit" link in the 1st row of fees-table
	And I select "Phone" from the PaymentMethod dropdown
	And I press "Save Fee"
	Then I should not see a validation message for PaymentMethodMailAddress with "The PaymentMethodMailAddress field is required."
	And I should see a validation message for PaymentMethodPhone with "The PaymentMethodPhone field is required." 
	And I should not see a validation message for PaymentMethodUrl with "The PaymentMethodUrl field is required." 
	When I enter "(555) 555-5555" into the PaymentMethodPhone field
	And I press "Save Fee"
	And I click the "Fees" tab
	Then I should see the following values in the fees-table table
         | Payment Method |
         | (555) 555-5555 |

	# Test URL
	When I click the "Edit" link in the 1st row of fees-table
	And I select "URL" from the PaymentMethod dropdown
	And I press "Save Fee"
	Then I should not see a validation message for PaymentMethodMailAddress with "The PaymentMethodMailAddress field is required."
	And I should not see a validation message for PaymentMethodPhone with "The PaymentMethodPhone field is required." 
	And I should see a validation message for PaymentMethodUrl with "The PaymentMethodUrl field is required." 
	When I enter "not a url" into the PaymentMethodUrl field
	And I press "Save Fee"
	Then I should see a validation message for PaymentMethodUrl with "The PaymentMethodUrl field is not a valid fully-qualified http, https, or ftp URL."
	When I enter "http://www.example.com" into the PaymentMethodUrl field
	And I press "Save Fee"
	And I click the "Fees" tab
	Then I should see the following values in the fees-table table
         | Payment Method         |
         | http://www.example.com |
	And I should see a link to "http://www.example.com"

Scenario: User can Search for an Environmental Permit
	Given I am logged in as "user"
	And I am at the Environmental/EnvironmentalPermit/Search page
	Then I should see "NJ7WW0001 - Water System 1" in the WasteWaterSystem dropdown
	Given a facility "one" exists with operating center: "nj4", facility name: "The Facility1"
	And an environmental permit type "one" exists with description: "Water Quality"
	And an environmental permit "one" exists with public water supply: "one", environmental permit type: "one", permit cross reference number: "1234", permit effective date: 10/2/2020, permit expiration date: 10/2/2020, permit renewal date: 10/3/2020, program interest number: "1234",environmental permit type: "one", description: "Give a hoot- dont polute!", waste water system: "one"
	When I press Search
    Then I should see a link to the Show page for environmental permit: "one"
	And I should see the following values in the environmentalPermitTable table
	| Permit ID | State | Permit Type   | Permit Status | Program Interest #                  | Facilities | Equipment | Permit Number | Description               | PublicWaterSupply          | Wastewater System | Cross Reference # | Effective Date | Expiration Date | Renewal Date | Requires Fees |
	| 1         | NJ    | Water Quality |               | 1234",environmental permit type"one |            |           | P#1123        | Give a hoot- dont polute! | public water supply: "one" | *NJ7WW000*         | 1234              | 10/2/2020      | 10/2/2020       | 10/3/2020           |    No         |