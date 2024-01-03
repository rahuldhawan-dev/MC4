Feature: ProductionWorkOrderPerformance
	To ensure this reports calculations and search features work correctly
	As a developer
	I'm going to write a ton of tests

Background: 
	Given a state "nj" exists with abbreviation: "NJ"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", sap enabled: "true", sap work orders enabled: "true", state: "nj"
	And production work order priorities exist
	And an employee status "active" exists with description: "Active"
	And an employee "one" exists with operating center: "nj7", status: "active", employee id: "12345678", first name: "jay", last name: "bob"
	And a role "productionworkorder-read-nj7" exists with action: "Read", module: "ProductionWorkManagement", operating center: "nj7"
	And a user "user" exists with username: "user", roles: productionworkorder-read-nj7
    And a town "nj7burg" exists
    And operating center: "nj7" exists in town: "nj7burg"
	And a planning plant "one" exists with operating center: "nj7", code: "D217", description: "Argh"
	And a facility "one" exists with operating center: "nj7", facility id: "NJSB-1", facility name: "A Facility", planning plant: "one"
	And order types exist
	And a production work description "one" exists with description: "one", order type: "operational activity"
	And a production work description "two" exists with description: "two", order type: "pm work order"
	And a production work description "three" exists with description: "three", order type: "corrective action"
	And a production work description "four" exists with description: "four", order type: "rp capital"

Scenario: User can search with only by date 
	Given a production work order "unscheduled" exists with date received: "10/1/2019", productionWorkDescription: "one", operating center: "nj7", planning plant: "one", facility: "one"
	And a production work order "scheduled" exists with date received: "10/1/2019", productionWorkDescription: "one", operating center: "nj7", planning plant: "one", facility: "one"
	And an employee assignment "scheduled" exists with production work order: "scheduled", assigned to: "one"
	And a production work order "incomplete" exists with date received: "10/1/2019", productionWorkDescription: "one", operating center: "nj7", planning plant: "one", facility: "one"
	And an employee assignment "incomplete" exists with production work order: "incomplete", assigned to: "one", date started: "10/1/2019"
	And a production work order "canceled" exists with date received: "10/1/2019", productionWorkDescription: "one", operating center: "nj7", planning plant: "one", facility: "one", date cancelled: "10/1/2019"
	And a production work order "completed" exists with date received: "10/1/2019", productionWorkDescription: "one", operating center: "nj7", planning plant: "one", facility: "one", date completed: "10/1/2019"
	And I am logged in as "user"
	And I am at the Reports/ProductionWorkOrderPerformance/Search page
	When I select "=" from the DateReceived_Operator dropdown
	And I enter "10/1/2019" into the DateReceived_End field
	And I press Search
	# Not Approved column is tested in another test
	Then I should not see the "# of WO's Not Approved" column in the "results" table
	And I should not see the "Operating Center" column in the "results" table
	And I should not see the "Planning Plant" column in the "results" table
	And I should not see the "Facility" column in the "results" table
	And I should see the following values in the results table
         | State | Order Type                      | # of WO's Created | # of WO's Unscheduled | # of WO's Scheduled | # of WO's Incomplete | # of WO's Canceled | # of WO's Completed | % of WO's Unscheduled | % of WO's Canceled | % of WO's Completed |
         | NJ    | order type: "operational activity"'s Description | 5                 | 1                     | 1                   | 1                    | 1                  | 1                   | 20.00 %               | 20.00 %            | 20.00 %             |
	When I click the "5" link under "# of WO's Created" in the 1st row of results 
	Then I should see a link to the Show page for production work order: "unscheduled"
	And I should see a link to the Show page for production work order: "scheduled"
	And I should see a link to the Show page for production work order: "incomplete"
	And I should see a link to the Show page for production work order: "canceled"
	And I should see a link to the Show page for production work order: "completed"
	When I go back
	And I click the "1" link under "# of WO's Unscheduled" in the 1st row of results 
	Then I should see a link to the Show page for production work order: "unscheduled"
	And I should not see a link to the Show page for production work order: "scheduled"
	And I should not see a link to the Show page for production work order: "incomplete"
	And I should not see a link to the Show page for production work order: "canceled"
	And I should not see a link to the Show page for production work order: "completed"
	When I go back
	And I click the "1" link under "# of WO's Scheduled" in the 1st row of results 
	Then I should not see a link to the Show page for production work order: "unscheduled"
	And I should see a link to the Show page for production work order: "scheduled"
	And I should not see a link to the Show page for production work order: "incomplete"
	And I should not see a link to the Show page for production work order: "canceled"
	And I should not see a link to the Show page for production work order: "completed"
	When I go back
	And I click the "1" link under "# of WO's Incomplete" in the 1st row of results 
	Then I should not see a link to the Show page for production work order: "unscheduled"
	And I should not see a link to the Show page for production work order: "scheduled"
	And I should see a link to the Show page for production work order: "incomplete"
	And I should not see a link to the Show page for production work order: "canceled"
	And I should not see a link to the Show page for production work order: "completed"
	When I go back
	And I click the "1" link under "# of WO's Canceled" in the 1st row of results 
	Then I should not see a link to the Show page for production work order: "unscheduled"
	And I should not see a link to the Show page for production work order: "scheduled"
	And I should not see a link to the Show page for production work order: "incomplete"
	And I should see a link to the Show page for production work order: "canceled"
	And I should not see a link to the Show page for production work order: "completed"
	When I go back
	And I click the "1" link under "# of WO's Completed" in the 1st row of results 
	Then I should not see a link to the Show page for production work order: "unscheduled"
	And I should not see a link to the Show page for production work order: "scheduled"
	And I should not see a link to the Show page for production work order: "incomplete"
	And I should not see a link to the Show page for production work order: "canceled"
	And I should see a link to the Show page for production work order: "completed"

Scenario: User sees the non approved columns when specifically searching for capital or corrective order types
	Given a production work order "not-approved" exists with date received: "10/1/2019", date completed: "10/1/2019", productionWorkDescription: "three", operating center: "nj7", planning plant: "one", facility: "one"
	And I am logged in as "user"
	And I am at the Reports/ProductionWorkOrderPerformance/Search page
	When I select "=" from the DateReceived_Operator dropdown
	And I enter "10/1/2019" into the DateReceived_End field
	And I select order type "corrective action"'s Description from the OrderType dropdown
	And I press Search
	Then I should see the "# of WO's Not Approved" column in the "results" table
	And I should see the following values in the results table
         | State | Order Type                             | # of WO's Created | # of WO's Unscheduled | # of WO's Scheduled | # of WO's Incomplete | # of WO's Canceled | # of WO's Completed | # of WO's Not Approved | % of WO's Unscheduled | % of WO's Canceled | % of WO's Completed |
         | NJ    | order type: "corrective action"'s Description | 1                 | 0                     | 0                   | 0                    | 0                  | 1                   | 1                      | 0.00 %                | 0.00 %             | 100.00 %            |
	When I click the "1" link under "# of WO's Not Approved" in the 1st row of results 
	Then I should see a link to the Show page for production work order: "not-approved"
	

# The following four only need to test the table results. No need to link test
Scenario: User should see operating center column when searching only by state
	Given a production work order "completed" exists with date received: "10/1/2019", productionWorkDescription: "one", operating center: "nj7", planning plant: "one", facility: "one", date completed: "10/1/2019"
	And an operating center "xx" exists with opcode: "XX"
	And a production work order "otherorder" exists with date received: "10/1/2019", productionWorkDescription: "one", operating center: "xx", planning plant: "one", facility: "one", date completed: "10/1/2019"
	And I am logged in as "user"
	And I am at the Reports/ProductionWorkOrderPerformance/Search page
	When I select "=" from the DateReceived_Operator dropdown
	And I enter "10/1/2019" into the DateReceived_End field
	And I select state "nj" from the State dropdown
	And I press Search
	Then I should not see the "Planning Plant" column in the "results" table
	And I should not see the "Facility" column in the "results" table
	And I should see the following values in the results table
         | State | Operating Center        | Order Type                      | # of WO's Created | # of WO's Unscheduled | # of WO's Scheduled | # of WO's Incomplete | # of WO's Canceled | # of WO's Completed | % of WO's Unscheduled | % of WO's Canceled | % of WO's Completed |
         | NJ    | operating center: "nj7" | order type: "operational activity"'s Description | 1                 | 0                     | 0                   | 0                    | 0                  | 1                   | 0.00 %                | 0.00 %             | 100.00 %            |
	When I click the "1" link under "# of WO's Created" in the 1st row of results 
	Then I should see a link to the Show page for production work order: "completed"
	And I should not see a link to the Show page for production work order: "otherorder"

Scenario: User should see the planning plant column when searching by operating center
	Given a production work order "completed" exists with date received: "10/1/2019", productionWorkDescription: "one", operating center: "nj7", planning plant: "one", facility: "one", date completed: "10/1/2019"
	And a planning plant "other" exists with operating center: "nj7", code: "D218", description: "Yargh"
	And a production work order "otherorder" exists with date received: "10/1/2019", productionWorkDescription: "one", operating center: "nj7", planning plant: "other", facility: "one", date completed: "10/1/2019"
	And I am logged in as "user"
	And I am at the Reports/ProductionWorkOrderPerformance/Search page
	When I select "=" from the DateReceived_Operator dropdown
	And I enter "10/1/2019" into the DateReceived_End field
	And I select state "nj" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I press Search
	Then I should not see the "Facility" column in the "results" table
	And I should see the following values in the results table
         | State | Operating Center        | Planning Plant | Order Type                      | # of WO's Created | # of WO's Unscheduled | # of WO's Scheduled | # of WO's Incomplete | # of WO's Canceled | # of WO's Completed | % of WO's Unscheduled | % of WO's Canceled | % of WO's Completed |
         | NJ    | operating center: "nj7" | D217 - Argh    | order type: "operational activity"'s Description | 1                 | 0                     | 0                   | 0                    | 0                  | 1                   | 0.00 %                | 0.00 %             | 100.00 %            |
	When I click the "1" link under "# of WO's Created" in the 1st row of results 
	Then I should see a link to the Show page for production work order: "completed"
	And I should not see a link to the Show page for production work order: "otherorder"

Scenario: User should see the facility column when searching by facility
	Given a production work order "completed" exists with date received: "10/1/2019", productionWorkDescription: "one", operating center: "nj7", planning plant: "one", facility: "one", date completed: "10/1/2019"
	And a facility "other" exists with operating center: "nj7", facility id: "NJSB-2", facility name: "Another Facility", planning plant: "one"
	And a production work order "otherorder" exists with date received: "10/1/2019", productionWorkDescription: "one", operating center: "nj7", planning plant: "one", facility: "other", date completed: "10/1/2019"
	And a role "roleFacilityReadNj7" exists with action: "Read", module: "ProductionFacilities", user: "user", operating center: "nj7"
	And I am logged in as "user"
	And I am at the Reports/ProductionWorkOrderPerformance/Search page
	When I select "=" from the DateReceived_Operator dropdown
	And I enter "10/1/2019" into the DateReceived_End field
	And I select state "nj" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select planning plant "one" from the PlanningPlant dropdown
	And I select facility "one"'s Description from the Facility dropdown
	And I press Search
	Then I should see the following values in the results table
         | State | Operating Center        | Planning Plant | Facility        | Order Type                      | # of WO's Created | # of WO's Unscheduled | # of WO's Scheduled | # of WO's Incomplete | # of WO's Canceled | # of WO's Completed | % of WO's Unscheduled | % of WO's Canceled | % of WO's Completed |
         | NJ    | operating center: "nj7" | D217 - Argh    | facility: "one"'s FacilityName | order type: "operational activity"'s Description | 1                 | 0                     | 0                   | 0                    | 0                  | 1                   | 0.00 %                | 0.00 %             | 100.00 %            |
	When I click the "1" link under "# of WO's Created" in the 1st row of results 
	Then I should see a link to the Show page for production work order: "completed"
	And I should not see a link to the Show page for production work order: "otherorder"

Scenario: User should see the correct results when searching by operating center and the production work order's planning plant is null
	Given a production work order "noplant" exists with date received: "10/1/2019", productionWorkDescription: "one", operating center: "nj7", planning plant: "null", facility: "one", date completed: "10/1/2019"
	And a production work order "hasplant" exists with date received: "10/1/2019", productionWorkDescription: "one", operating center: "nj7", facility: "one", date completed: "10/1/2019"
	And I am logged in as "user"
	And I am at the Reports/ProductionWorkOrderPerformance/Search page
	When I select "=" from the DateReceived_Operator dropdown
	And I enter "10/1/2019" into the DateReceived_End field
	And I select state "nj" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I press Search
	Then I should not see the "Facility" column in the "results" table
	And I should see the following values in the results table
         | State | Operating Center        | Planning Plant | Order Type                      | # of WO's Created | # of WO's Unscheduled | # of WO's Scheduled | # of WO's Incomplete | # of WO's Canceled | # of WO's Completed | % of WO's Unscheduled | % of WO's Canceled | % of WO's Completed |
         | NJ    | operating center: "nj7" |                | order type: "operational activity"'s Description | 1                 | 0                     | 0                   | 0                    | 0                  | 1                   | 0.00 %                | 0.00 %             | 100.00 %            |
	When I click the "1" link under "# of WO's Created" in the 1st row of results 
	Then I should see a link to the Show page for production work order: "noplant"
	And I should not see a link to the Show page for production work order: "hasplant"