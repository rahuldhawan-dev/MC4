Feature: CovidStatusReportPage

Background: users and supporting data exists
	Given a state "one" exists with name: "New Jersey", abbreviation: "NJ"
	And a state "two" exists with name: "New York", abbreviation: "NY"
	And an operating center "nj4" exists with opcode: "NJ4", name: "Lakewood", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true", state: "one"
	And an operating center "ny1" exists with opcode: "NY1", name: "Rockland, companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true", state: "two"
	And an employee status "active" exists with description: "Active"
	And a personnel area "one" exists
	And a personnel area "two" exists
	And an employee "one" exists with first name: "Theodore", last name: "Logan", employee id: "11111111", operating center: "nj4", status: "active", personnel area: "one"
	And an employee "two" exists with first name: "Bill", last name: "Preston", employee id: "11111117", operating center: "ny1", status: "active", personnel area: "two"
	And a user "user" exists with username: "user", employee: "two"
	And a role "roleRead" exists with action: "Read", module: "HumanResourcesCovid", user: "user", operating center: "ny1"
	And a role "roleEdit" exists with action: "Edit", module: "HumanResourcesCovid", user: "user", operating center: "ny1"
	And a role "roleAdd" exists with action: "Add", module: "HumanResourcesCovid", user: "user", operating center: "ny1"
	And a role "roleDelete" exists with action: "Delete", module: "HumanResourcesCovid", user: "user", operating center: "ny1"
	And a covid request type "one" exists with description: "one"
	And a covid request type "two" exists with description: "two"
	And a covid submission status "one" exists with description: "NEW"
	And a covid submission status "two" exists with description: "IN PROGRESS"
	And a covid submission status "three" exists with description: "COMPLETE"
	And a covid outcome category "one" exists with description: "one"
	And a covid outcome category "two" exists with description: "two"
	And a release reason "one" exists with description: "one"

Scenario: user can search and view results
	Given a covid issue "one" exists with employee: "one", submission status: "one"
	And a covid issue "two" exists with employee: "two", submission status: "two"
	And I am logged in as "user"
	When I visit the Reports/CovidStatusReport/Search page
	And I select state "two" from the State dropdown
	And I press "Search"
	Then I should see the following values in the results table
	| Id | Employee     | State | Employee ID | Operating Center | Personnel Area        | Submission Date | Submission Status | Start Date | Estimated Release Date | Release Date | Total Days |
	| 2  | Bill Preston | NY    | 11111117    | NY1 - "Rockland  | personnel area: "two" | 4/1/2020        | IN PROGRESS       |            |                        |              |            |