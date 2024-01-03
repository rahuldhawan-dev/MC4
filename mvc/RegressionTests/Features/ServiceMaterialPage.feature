Feature: ServiceMaterialPage

Background: data exists
	Given an admin user "admin" exists with username: "admin"
	And an e p a code "lead" exists with description: "LEAD"
	And an e p a code "not lead" exists with description: "NON LEAD"
	And a service material "one" exists with description: "LEAD", is edit enabled: true, customer e p a code: "lead", company e p a code: "lead"
	And a service material "two" exists with description: "AC", is edit enabled: false, customer e p a code: "not lead", company e p a code: "not lead"
	And an operating center "nj7" exists with opcode: "NJ7", name: "NJ"
	And operating center: "nj7" exists in service material: "one"	

Scenario: Users can view Service Materials
	Given I am logged in as "admin"
	When I visit the ServiceMaterial page
	Then I should at least see "Service Materials" in the bodyHeader element
	And I should see the table-caption "Records found: 2"
	And I should see the following values in the service-materails-table table
	| Id | Description | Is Edit Enabled | Company EPA Code | Customer EPA Code |
	| 1  | LEAD        | Yes             | LEAD             | LEAD              |
	| 2  | AC          | No              | NON LEAD         | NON LEAD          |
 
Scenario: Users can view Service Materials details
	Given I am logged in as "admin"
	When I visit the ServiceMaterial/Show/1 page 
	Then I should see a display for Description with "LEAD"
	And I should see a display for CompanyEPACode with "LEAD"
	And I should see a display for CustomerEPACode with "LEAD"
	And I should see a display for IsEditEnabled with "Yes"
	