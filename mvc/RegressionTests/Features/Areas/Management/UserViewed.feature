Feature: UserViewed

Background:
	Given a user "user" exists with username: "user"
	And an operating center "nj7" exists
	And a role exists with action: "Read", module: "ManagementGeneral", user: "user", operating center: "nj7"

Scenario: Management user can view images viewed by day and also view each of those individually
	Given a tap image "one" exists with operating center: "nj7"
	And a tap image "two" exists with operating center: "nj7"
	And a valve image "one" exists with operating center: "nj7"
	And a as built image "one" exists with operating center: "nj7"
	And a user viewed exists with tap image: "one", user: "user", viewed at: "12/12/2018 12:04 AM"
	And a user viewed exists with tap image: "two", user: "user", viewed at: "12/12/2018 1:05 AM"
	And a user viewed exists with valve image: "one", user: "user", viewed at: "12/12/2018 12:05 AM"
	And a user viewed exists with as built image: "one", user: "user", viewed at: "12/12/2018 12:45 AM"
	# This should not show up in any of the results
	And a user viewed exists with as built image: "one", user: "user", viewed at: "12/13/2018 12:45 AM"
	Given I am logged in as "user" 
	And I am at the Management/UserViewedDailyReport/Search page
	When I enter "12/12/2018" into the ViewedAt_End field
	# Not selecting the "Equal" operator because it should be selected by default on this page.
	And I press "Search"
	Then I should see the following values in the search-results table
         | Viewed At  | Username    | Tap Images | Valve Images | As Built Images |
         | 12/12/2018 | user        | 2          | 1            | 1               |
	When I follow "View"
	And I click the "Taps" tab
	Then I should see the following values in the search-results-tap table
	| Viewed At              |
	| 12/12/2018 1:05:00 AM  |
	| 12/12/2018 12:04:00 AM |
	And I should see a link to the FieldOperations/TapImage/Show page for tap image "one"
	And I should see a link to the FieldOperations/TapImage/Show page for tap image "two"
	When I click the "Valves" tab
	Then I should see the following values in the search-results-valve table
	| Viewed At              |
	| 12/12/2018 12:05:00 AM |
	And I should see a link to the FieldOperations/ValveImage/Show page for valve image "one"
	When I click the "As Builts" tab
	Then I should see the following values in the search-results-asbuilt table
	| Viewed At              |
	| 12/12/2018 12:45:00 AM |
	And I should see a link to the FieldOperations/AsBuiltImage/Show page for as built image "one"
	And I should not see "12/13/2018"

# ie: Searching on UserViewedDailyReport, seeing results, then going to those specific reports on UserViewed/Index
