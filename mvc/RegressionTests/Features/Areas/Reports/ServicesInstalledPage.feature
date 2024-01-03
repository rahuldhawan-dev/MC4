Feature: ServicesInstalledPage
	And, oh, my God, is that a gold bar?
	The wave just washed him out far
	The kids didn't look close enough, in the, taffy butt

Background: users and data exist
	Given a user "user" exists with username: "user"
	And an operating center "opc" exists with opcode: "NJ7", name: "Shrewsbury", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
	And a role "role" exists with action: "Read", module: "FieldServicesAssets", user: "user", operating center: "opc"
	And a user "noroles" exists with username: "noroles"
	And a town "one" exists with name: "Loch Arbour"
	And a service category "one" exists with description: "Foo"
	And a service size "one" exists with service size description: "1/2", size: "0.5"
	And a service restoration contractor "one" exists with contractor: "Hoff Bros", operating center: "opc"

Scenario: user receives a functioning search page with validation and dropdowns
	Given I am logged in as "user"
	When I visit the /Reports/ServicesInstalled/Search page
	Then I should see operating center "opc"'s Name in the OperatingCenter dropdown
	#When I enter "01/01/2013" into the DateInstalled_End field
	#And I press Search
	#Then I should be at the Reports/ServicesInstalled/Search page
	When I select ">" from the DateInstalled_Operator dropdown
	And I enter "" into the DateInstalled_End field
	And I press Search
	Then I should be at the Reports/ServicesInstalled/Search page
	When I enter "01/01/2013" into the DateInstalled_End field
	And I press Search
	And I wait for the page to reload
	Then I should see the error message No results matched your query.

Scenario: user should be able to search and get results
	Given a service "one" exists with service size: "one", service category: "one", date installed: "1/2/2013", town: "one", work issued to: "one"
	And a service size "two" exists with service size description: "2 1/2", size: "2.5"
	And a service "two" exists with service size: "two", service category: "one", date installed: "1/1/1943", town: "one"
	And I am logged in as "user"
	When I visit the /Reports/ServicesInstalled/Search page
	And I select ">" from the DateInstalled_Operator dropdown
	And I enter "01/01/1900" into the DateInstalled_End field
	And I press Search
	And I wait for the page to reload
	Then I should not see "No results matched your query."
	And I should see "Loch Arbour" in the table servicesInstalled's "Town" column
	And I should see "Foo" in the table servicesInstalled's "Category of Service" column
	And I should see "1/2" in the table servicesInstalled's "Service Company Size" column
	And I should not see "Rambo" in the table servicesInstalled's "Service Company Size" column
	And I should see "Hoff Bros" in the table servicesInstalled's "Work Issued To" column
