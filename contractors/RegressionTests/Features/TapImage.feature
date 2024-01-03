Feature: TapImage

Background: users exist
    Given an operating center "one" exists with opcntr: "NJX", opcntrname: "Shrewsburgh"
    And a contractor "one" exists with name: "one", operating center: "one"
    And an admin user exists with email: "admin@site.com", password: "testpassword#1", contractor: "one"
	And a state "one" exists with name: "New Jersey Again", abbreviation: "NJ", scada table: "njscada"
	And a county "one" exists with name: "Monmouth", state: "one"
	And a town "one" exists with name: "Loch Arbour", county: "one", state: "one"
	And operating center: "one" exists in town: "one" 
	
Scenario: User can do wildcard searches on service number for tap images
	Given a tap image "one" exists with service number: "123456"
	And a tap image "two" exists with service number: "3456789"
	And a tap image "nope" exists with service number: "never find me"
	Given I am logged in as "admin@site.com", password: "testpassword#1"
	When I visit the /TapImage/Search page
	And I enter "3456" into the ServiceNumber_Value field
	And I select "Wildcard" from the ServiceNumber_MatchType dropdown
	And I press Search
	Then I should see "Records found: 2"
	And I should see the following values in the search-results-table table
	| Service Number |
	| 123456         |
	| 3456789        |
	When I visit the /TapImage/Search page
	And I enter "*3456*" into the ServiceNumber_Value field
	And I select "Wildcard" from the ServiceNumber_MatchType dropdown
	And I press Search
	Then I should see "Records found: 2"
	And I should see the following values in the search-results-table table
	| Service Number |
	| 123456         |
	| 3456789        |

Scenario: User can do exact searches on service number for tap images
	Given a tap image "one" exists with service number: "123456"
	And a tap image "two" exists with service number: "123456"
	And a tap image "three" exists with service number: "345678"
	Given I am logged in as "admin@site.com", password: "testpassword#1"
	When I visit the /TapImage/Search page
	And I enter "123456" into the ServiceNumber_Value field
	And I select "Exact" from the ServiceNumber_MatchType dropdown
	And I press Search
	Then I should see "Records found: 2"
	And I should see the following values in the search-results-table table
	| Service Number |
	| 123456         |
	| 123456         |

