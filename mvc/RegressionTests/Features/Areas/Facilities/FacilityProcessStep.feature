Feature: FacilityProcessStep

Background: users and supporting data exist
	Given a user "user" exists with username: "user"
	And a role "roleRead" exists with action: "Read", module: "ProductionFacilities", user: "user"
	And a role "roleEdit" exists with action: "Edit", module: "ProductionFacilities", user: "user"
	And a role "roleAdd" exists with action: "Add", module: "ProductionFacilities", user: "user"
	And a role "roleDelete" exists with action: "Delete", module: "ProductionFacilities", user: "user"
	And an operating center "opc" exists with opcode: "NJ4", name: "Lakewood", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
	And a town "lazytown" exists
	And operating center: "opc" exists in town: "lazytown"
	And a role "image" exists with action: "Read", module: "FieldServicesImages", user: "user"
	And a department "production" exists with description: "Production"
	And an asset type "facility" exists with description: "facility"
	And a functional location "one" exists with description: "NJRB-UN-FAC-0003", asset type: "facility", town: "lazytown"

Scenario: user without edit rights should not see the add or remove buttons for adding processes
	Given a user "readonly" exists with username: "readonly"
	And a role exists with action: "Read", module: "ProductionFacilities", user: "readonly"
	And a facility "one" exists with facility id: "NJSB-01"
	And a process "one" exists with description: "I am a process"
	And a facility process "one" exists with facility: "one", process: "one"
	And a facility process step "one" exists with facility process: "one"
    And I am logged in as "readonly"
    When I visit the Show page for facility process: "one"
	And I click the "Process Steps" tab
	Then I should not see "Add Process Step"
	And I should not see "Remove"
	And I should see "I am a process"