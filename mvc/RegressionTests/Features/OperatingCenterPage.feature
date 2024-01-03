Feature: OperatingCenterPage
	As a developer
	Does anything we write here matter?

Background: users exist
	Given an admin user "admin" exists with username: "admin"
	And a user "user" exists with username: "user"
	And a role "role" exists with action: "Read", module: "FieldServicesDataLookups", user: "user"
	And a user "noroles" exists with username: "noroles"
	And a recurring frequency unit "day" exists with description: "Day"
	And a recurring frequency unit "week" exists with description: "Week"
	And a recurring frequency unit "month" exists with description: "Month"
	And a recurring frequency unit "year" exists with description: "Year"
	And an operating center "opc" exists with opcode: "NJ4", name: "Lakewood", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "year", largeValveInspFreq: "2", largeValveInspFreqUnit: "month", smallValveInspFreq: "3", smallValveInspFreqUnit: "week", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true", hydrantPaintFreq: "4", hydrantPaintFreqUnit: "day"
	And a role "image" exists with action: "Read", module: "FieldServicesImages", user: "user"
    And a state "one" exists
	And a time zone "est" exists with zone: "EST", description: "Eastern Standard Time", u t c off set: -5
	And a time zone "cst" exists with zone: "CST", description: "Central Standard Time", u t c off set: -6

# AUTHORIZATION	
Scenario: user with read field services datalookups role can access operating center index and show pages
	Given I am logged in as "user"
	When I visit the OperatingCenter page
	Then I should at least see "Operating Centers" in the bodyHeader element
	And I should see a link to the Show page for operating center: "opc"
	When I visit the Show page for operating center: "opc"
	Then I should at least see "Operating Centers" in the bodyHeader element
	And I should at least see "NJ4" in the bodyHeader element
	And I should not see a link to the Edit page for operating center: "opc"
	# Edit page requires site admin access, role authorize filter won't redirect here.
	When I try to access the Edit page for operating center: "opc"
	Then I should be at the Error/Forbidden screen

Scenario: user without proper role should see missing role error for all operating center pagess
	Given I am logged in as "noroles"
	When I visit the OperatingCenter page
	Then I should see the missing role error
	When I visit the Show page for operating center: "opc"
	Then I should see the missing role error

Scenario: user should see forbidden page if they access the edit operating center page
	Given I am logged in as "noroles"
	When I try to visit the Edit page for operating center: "opc"
	Then I should be at the Error/Forbidden screen

Scenario: site admin can access all operating center pages
	Given I am logged in as "admin"
	When I visit the OperatingCenter page
	Then I should at least see "Operating Centers" in the bodyHeader element
	And I should see a link to the Show page for operating center: "opc"
	When I visit the Show page for operating center: "opc"
	Then I should at least see "Operating Center" in the bodyHeader element
	And I should at least see "NJ4" in the bodyHeader element
	And I should see a link to the Edit page for operating center: "opc"
	When I visit the Edit page for operating center: "opc"
	Then I should at least see "Operating Centers" in the bodyHeader element
	And I should see a link to the Show page for operating center: "opc"
	And I should at least see "Editing" in the bodyHeader element


# CONTENT
Scenario: user should see a bunch of stuff on the Operating Centers Show page
	Given I am logged in as "user"
	When I visit the Show page for operating center: "opc"
	Then I should see a display for OperatingCenterCode with "NJ4"
	And I should see a display for OperatingCenterName with "Lakewood"
	And I should see a display for WorkOrdersEnabled with "Yes"
	And I should see a display for CompanyInfo with "Some Info"
	And I should see a display for PhoneNumber with "123-234-5678"
	And I should see a display for FaxNumber with "987-654-3210"
	And I should see a display for MailingAddressName with "The Mailing Address Name"
	And I should see a display for MailingAddressStreet with "The Mailing Street"
	And I should see a display for MailingAddressCityStateZip with "The Mailing Town ST 12345"
	And I should see a display for ServiceContactPhoneNumber with "000-000-0000"
	And I should see a display for HydrantInspectionFrequency with "1"
	And I should see a display for HydrantInspectionFrequencyUnit with "Year"
	And I should see a display for LargeValveInspectionFrequency with "2"
	And I should see a display for LargeValveInspectionFrequencyUnit with "Month"
	And I should see a display for SmallValveInspectionFrequency with "3"
	And I should see a display for SmallValveInspectionFrequencyUnit with "Week"
    And I should see a display for HydrantPaintingFrequency with "4"
    And I should see a display for HydrantPaintingFrequencyUnit with "Day"
	And I should see a display for PermitsOMUserName with "john"
	And I should see a display for PermitsCapitalUserName with "not john"

Scenario: site admin can edit operating centers
	Given I am logged in as "admin"
	And I am at the Edit page for operating center: "opc"
	And I can see a display for OperatingCenterCode with "NJ4"
	And I can see "Lakewood" in the OperatingCenterName field
	And the WorkOrdersEnabled field is checked
	And I can see "Some Info" in the CompanyInfo field
	And I can see "123-234-5678" in the PhoneNumber field
	And I can see "987-654-3210" in the FaxNumber field
	And I can see "The Mailing Address Name" in the MailingAddressName field
	And I can see "The Mailing Street" in the MailingAddressStreet field
	And I can see "The Mailing Town ST 12345" in the MailingAddressCityStateZip field
	And I can see "000-000-0000" in the ServiceContactPhoneNumber field
	And I can see "1" in the HydrantInspectionFrequency field
	And "Year" is selected in the HydrantInspectionFrequencyUnit dropdown
	And I can see "2" in the LargeValveInspectionFrequency field
	And "Month" is selected in the LargeValveInspectionFrequencyUnit dropdown
	And I can see "3" in the SmallValveInspectionFrequency field
	And "Week" is selected in the SmallValveInspectionFrequencyUnit dropdown
	And I can see "4" in the HydrantPaintingFrequency field
	And "Day" is selected in the HydrantPaintingFrequencyUnit dropdown
	And I can see "john" in the PermitsOMUserName field
	And I can see "not john" in the PermitsCapitalUserName field
	When I enter "Not Lakewood" into the OperatingCenterName field
	And I uncheck the WorkOrdersEnabled field
    And I select state "one" from the State dropdown
	And I enter "A different company" into the CompanyInfo field
	And I enter "111-111-1111" into the PhoneNumber field
	And I enter "222-222-2222" into the FaxNumber field
	And I enter "A different mailing name" into the MailingAddressName field
	And I enter "A different mailing street" into the MailingAddressStreet field
	And I enter "Town ST Zip" into the MailingAddressCityStateZip field
	And I enter "333-333-3333" into the ServiceContactPhoneNumber field
	And I enter "5" into the HydrantInspectionFrequency field
	And I select "Day" from the HydrantInspectionFrequencyUnit dropdown
	And I enter "6" into the LargeValveInspectionFrequency field
	And I select "Week" from the LargeValveInspectionFrequencyUnit dropdown
	And I enter "7" into the SmallValveInspectionFrequency field
	And I select "Year" from the SmallValveInspectionFrequencyUnit dropdown
	And I enter "someone" into the PermitsOMUserName field
	And I enter "no one" into the PermitsCapitalUserName field
	And I enter "1234" into the RSADivisionNumber field
	And I uncheck the IsActive field
	And I select time zone "cst" from the TimeZone dropdown
	And I enter "15fdc279b4234fcb85f455ee70897a9e" into the ArcMobileMapId field
	And I enter "e8ae5ea9dc304901b35f07d12854a6ba" into the MapId field
	And I enter "123e5ea9dc304901b35f07d12854a6ba" into the InfoMasterMapId field
	And I press Save
	Then I should be at the Show page for operating center: "opc"
	And I should see a display for OperatingCenterCode with "NJ4"
	And I should see a display for OperatingCenterName with "Not Lakewood"
	And I should see a display for WorkOrdersEnabled with "No"
	And I should see a display for CompanyInfo with "A different company"
	And I should see a display for PhoneNumber with "111-111-1111"
	And I should see a display for FaxNumber with "222-222-2222"
	And I should see a display for MailingAddressName with "A different mailing name"
	And I should see a display for MailingAddressStreet with "A different mailing street"
	And I should see a display for MailingAddressCityStateZip with "Town ST Zip"
	And I should see a display for ServiceContactPhoneNumber with "333-333-3333"
	And I should see a display for HydrantInspectionFrequency with "5"
	And I should see a display for HydrantInspectionFrequencyUnit with "Day"
	And I should see a display for LargeValveInspectionFrequency with "6"
	And I should see a display for LargeValveInspectionFrequencyUnit with "Week"
	And I should see a display for SmallValveInspectionFrequency with "7"
	And I should see a display for SmallValveInspectionFrequencyUnit with "Year"
	And I should see a display for PermitsOMUserName with "someone"
	And I should see a display for PermitsCapitalUserName with "no one"
	And I should see a display for RSADivisionNumber with "1234"
	And I should see a display for IsActive with "No"
    And I should see a display for State with state "one"
	And I should see a display for TimeZone with time zone "cst"
	And I should see a display for MapId with "e8ae5ea9dc304901b35f07d12854a6ba"
	And I should see a display for ArcMobileMapId with "15fdc279b4234fcb85f455ee70897a9e"
	And I should see a display for InfoMasterMapId with "123e5ea9dc304901b35f07d12854a6ba"

Scenario: admin user should see a lot of validators when editing an operating center
	Given I am logged in as "admin"
	And I am at the Edit page for operating center: "opc"
	When I enter "" into the OperatingCenterName field
	And I press Save
	Then I should see the validation message The OperatingCenterName field is required.
	When I enter "notanumber" into the HydrantInspectionFrequency field
	And I press Save
	Then I should see the validation message The field HydrantInspectionFrequency must be a number.
	When I enter "notanumber" into the LargeValveInspectionFrequency field
	And I enter "2" into the HydrantInspectionFrequency field
	And I press Save
	Then I should see the validation message The field LargeValveInspectionFrequency must be a number.
	When I enter "notanumber" into the SmallValveInspectionFrequency field
	And I enter "2" into the LargeValveInspectionFrequency field
	And I press Save
	Then I should see the validation message The field SmallValveInspectionFrequency must be a number.
	When I enter "12346" into the RSADivisionNumber field
	And I press Save
	Then I should see the validation message The field RSA/Division # must be between 0 and 9999.
