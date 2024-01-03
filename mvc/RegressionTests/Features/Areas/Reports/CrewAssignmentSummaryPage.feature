Feature: CrewAssignmentSummaryPage

Background: admin user exists
	Given an admin user "admin" exists with username: "admin"
    And a user "user" exists with username: "user"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a contractor "one" exists with operating center: "nj7"
    And a town "one" exists with operating center: "nj7"
    And a town "two" exists with operating center: "nj7"
	And a town "three" exists with operating center: "nj7"
    And a street "one" exists with town: "one", is active: true
    And a street "two" exists with town: "two", is active: true
	And a street "three" exists with town: "three", is active: true
	And a role "read" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a work description "hydrant installation" exists with description: "hydrant installation"
	And a work description "hydrant investigation" exists with description: "hydrant investigation"
	And a work description "hydrant leaking" exists with description: "hydrant leaking"
	And a crew "one" exists with description: "one", availability: "8", operating center: "nj7", active: true
	And a crew "two" exists with description: "two", availability: "8", operating center: "nj7", active: true
	And a crew "three" exists with description: "three", availability: "8", operating center: "nj7", active: true
	And a crew "contractor" exists with description: "contractor", availability: "8", contractor: "one", active: true 		
	And a work order "one" exists with operating center: "nj7", town: "one", street: "one", NearestCrossStreet: "two", work description: "hydrant installation", sap notification number: "19942945", sap work order number: "91139231"
    And a work order "two" exists with operating center: "nj7", town: "one", street: "two", NearestCrossStreet: "three", work description: "hydrant investigation", sap notification number: "324467818", sap work order number: "91138778"
    And a work order "three" exists with operating center: "nj7", town: "one", street: "three", NearestCrossStreet: "one", work description: "hydrant leaking", sap notification number: "324467809", sap work order number: "91138813"   	
    And a work order "four" exists with operating center: "nj7", town: "one", street: "three", NearestCrossStreet: "one", work description: "hydrant leaking", sap notification number: "324467809", sap work order number: "91138813"   	
	And a crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "today", assigned on: "yesterday", priority: "1", date started: "4/29/2022 09:00:00 AM", date ended: "4/30/2022 03:00:00 AM"
	And a crew assignment "ca2" exists with work order: "two", crew: "two", assigned for: "4/24/2022", assigned on: "4/24/1984", priority: "2", date started: "4/26/2022 03:00:00 PM", date ended: "4/27/2022 03:00:00 AM"
    And a crew assignment "ca3" exists with work order: "three", crew: "three", assigned for: "4/24/2022", assigned on: "4/24/1984", priority: "4", date started: "4/27/2022 06:00:00 PM", date ended: "4/28/2022 03:00:00 AM"
    And a crew assignment "ca4" exists with work order: "four", crew: "contractor", assigned for: "11/30/2022", assigned on: "4/24/1984", priority: "4", date started: "4/27/2022 06:00:00 PM", date ended: "4/28/2022 03:00:00 AM"

Scenario: user can search and view results
	And I am logged in as "user"
	When I visit the Reports/CrewAssignmentSummary/Search page
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select ">" from the AssignedFor_Operator dropdown
	And I enter "3/24/2022" into the AssignedFor_End field
	And I press "Search"
	Then I should see the following values in the results table
	| Work Order Number | Priority | Street Number | Description of Job    | Est. TTC | Assigned For | Start Time    | End Time      | TTC    | SAP Work Order | SAP Notification |
    | 4                 | 4        | 1234          | HYDRANT LEAKING       | 6        | 11/30/2022   | 6:00 PM (EST) | 3:00 AM (EST) | 9h 0m  | 91138813       | 324467809        |
    | 1                 | 1        | 1234          | HYDRANT INSTALLATION  | 3.5      | today        | 9:00 AM (EST) | 3:00 AM (EST) | 18h 0m | 91139231       | 19942945         |
    | 3                 | 4        | 1234          | HYDRANT LEAKING       | 6        | 4/24/2022    | 6:00 PM (EST) | 3:00 AM (EST) | 9h 0m  | 91138813       | 324467809        |
    | 2                 | 2        | 1234          | HYDRANT INVESTIGATION | 0        | 4/24/2022    | 3:00 PM (EST) | 3:00 AM (EST) | 12h 0m | 91138778       | 324467818        |
	And I should see a link to the show page for work order "one"
	And I should see a link to the show page for work order "three"
	And I should see a link to the show page for work order "two"