Feature: Scheduling Page
    In order to 
	As a user
	I want to be

Background: admin and non-admin users exist
    Given an operating center "one" exists with opcntr: "NJX", opcntrname: "Shrewsburgh"
	And a town "Foo" exists with shortname: "Foo", operating center: "one"
    And operating center "one" exists in town "Foo"
	And a town section "Little Foo" exists with name: "Little Foo", town: "Foo"
	And a street "one" exists with name: "Easy St", town: "Foo"
    And a contractor "one" exists with name: "one", operating center: "one"
    And an admin user exists with email: "admin@site.com", password: "testpassword#1", contractor: "one"
    And a user exists with email: "user@site.com", password: "testpassword#1", contractor: "one"
    And a crew "one" exists with description: "one", contractor: "one"

Scenario: user cannot access the scheduling page
	Given I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the WorkOrderScheduling/Search page
    Then I should be at the forbidden screen

Scenario: admin accesses the scheduling search
    Given I am logged in as "admin@site.com", password: "testpassword#1"
    And I am at the WorkOrderScheduling/Search page
	Then I should not see "Access to the webpage was denied"
    And I should not see the error message "No search criteria chosen."
	And I should see "Work Order - Scheduling - Search"
    And I should see "Work Order Number"
    And I should see "Town"
    And I should see "Town Section"
    And I should see "Street"
    And I should see "Street Number"
    And I should see "Nearest Cross Street"
    And I should see "Asset Type"
    And I should see "Work Description"
    And I should see "Priority"
    And I should see "Traffic Control Required"
    #And I should see "Time to Complete"
    #And I should see "Markout Required"
    #And I should see "Pending Assignment"
    #And I should see "Markout Expiration Within Next"

@no_data_reload
Scenario: admin searches for an order by number, order has markout and crew assignment
    Given a scheduling work order with valve "one" exists with contractor: "one", markout requirement: "routine"
    And a markout "one" exists with ready date: "yesterday", scheduling work order with valve: "one"
	And a crew assignment "ca" exists with scheduling work order with valve: "one", crew: "one", assigned for: "today", assigned on: "today"
    And I am logged in as "admin@site.com", password: "testpassword#1"
    And I am at the WorkOrderScheduling/Search page
    When I enter scheduling work order with valve "one"'s Id into the Id field
    And I press Search
    Then I should see "Work Order - Scheduling - Results"
    And I should see scheduling work order with valve "one"'s Id in the "Order #" column
    And I should see scheduling work order with valve "one"'s DateReceived as a date in the "Date Received" column
    And I should see scheduling work order with valve "one"'s StreetNumber in the "Street Number" column
    And I should see scheduling work order with valve "one"'s Street in the "Street" column
    And I should see scheduling work order with valve "one"'s Town in the "Town" column
    And I should see scheduling work order with valve "one"'s AssetType in the "Asset Type" column
    And I should see scheduling work order with valve "one"'s WorkDescription in the "Description of Job" column
    And I should see scheduling work order with valve "one"'s Priority in the "Priority" column
    And I should see scheduling work order with valve "one"'s Purpose in the "Purpose" column
    And I should see scheduling work order with valve "one"'s MarkoutRequirement in the "Markout Requirement" column
	And I should see markout "one"'s ReadyDate as a date without seconds in the "Markout Ready Date" column
	And I should see markout "one"'s ExpirationDate as a date without seconds in the "Markout Expiration Date" column
    And I should see crew assignment "ca"'s AssignedFor as a date in the "Assigned Date" column
    And I should see crew assignment "ca"'s Crew in the "Assigned To" column
	And I should see crew "one"'s Description in the Crew dropdown

#Scenario: admin search for an order with an sop that is not ready and not expired
#	Given a scheduling work order with valve "one" exists with contractor: "one", 

# searches
Scenario: admin can search by operating center
	Given I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderScheduling/Search page
	When I select operating center "one"'s Name from the OperatingCenter dropdown
	And I press Search
	Then I should see the error message "No results matched your query."

Scenario: admin can search by town
	Given I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderScheduling/Search page
	When I select operating center "one"'s Name from the OperatingCenter dropdown
	And I wait for element Town to exist
	And I select town "Foo"'s ShortName from the Town dropdown
	And I press Search
	Then I should see the error message "No results matched your query."

Scenario: admin can search by town section
	Given I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderScheduling/Search page
	When I select operating center "one"'s Name from the OperatingCenter dropdown
	And I wait for element Town to exist
	And I select town "Foo"'s ShortName from the Town dropdown
	And I wait for element TownSection to exist
	And I select town section "Little Foo"'s Name from the TownSection dropdown
	And I press Search
	Then I should see the error message "No results matched your query."

Scenario: admin can search by street
	Given I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderScheduling/Search page
	When I select operating center "one"'s Name from the OperatingCenter dropdown
	And I wait for element Town to exist
	And I select town "Foo"'s ShortName from the Town dropdown
	And I wait for element Street to exist
	And I select street "one"'s FullStName from the Street dropdown
	And I press Search
	Then I should see the error message "No results matched your query."

Scenario: admin can search by street number
	Given I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderScheduling/Search page
	When I enter "123" into the StreetNumber field
	And I press Search
	Then I should see the error message "No results matched your query."

Scenario: admin can search by nearest cross street
	Given I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderScheduling/Search page
	When I select operating center "one"'s Name from the OperatingCenter dropdown
	And I wait for element Town to exist
	And I select town "Foo"'s ShortName from the Town dropdown
	And I wait for element NearestCrossStreet to exist
	And I select street "one"'s FullStName from the NearestCrossStreet dropdown
	And I press Search
	Then I should see the error message "No results matched your query."

Scenario: admin can search by asset type
	Given I am logged in as "admin@site.com", password: "testpassword#1"
	And an asset type "valve" exists with description: "valve", operating center: "one"
	And I am at the WorkOrderScheduling/Search page
	When I select asset type "valve"'s Description from the AssetType dropdown
	And I press Search
	Then I should see the error message "No results matched your query."

Scenario: admin can search by work description
	Given I am logged in as "admin@site.com", password: "testpassword#1"
	And an asset type "valve" exists with description: "valve", operating center: "one"
	And a valve work description "one" exists with time to complete: "1.1234", asset type: "valve", description: "turn valve"
	And I am at the WorkOrderScheduling/Search page
	When I select asset type "valve"'s Description from the AssetType dropdown
	#And I wait for element WorkDescription to be enabled
	And I select valve work description "one"'s Description from the WorkDescription dropdown
	And I press Search
	Then I should see the error message "No results matched your query."

Scenario: admin can search by priority
	Given I am logged in as "admin@site.com", password: "testpassword#1"
	# There's something really fishy about this test failing. If you do not create any additional records 
	# after creating the priority, then the priority never ends up in the database. Another record creation, of any type,
	# needs to happen in order to get the priority to persist. I do not know why nor do I have time to figure
	# out why this test has suddenly decided to break. -Ross 2/20/2020
	And a work order priority "one" exists with description: "Emergency"
	And a valve work description "one" exists with time to complete: "1.1234", asset type: "valve", description: "turn valve"
	And I am at the WorkOrderScheduling/Search page
	When I select work order priority "one"'s Description from the Priority dropdown
	And I press Search
	Then I should see the error message "No results matched your query."

# TODO: make this test work
@ignore
Scenario: admin can search by time to complete *
	Given I do not currently function
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderScheduling/Search page
	When I select "< 1" from the CompletionTime dropdown
	And I press Search
	Then I should see the error message "No results matched your query."

Scenario: admin can search by traffic control required *
	Given I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderScheduling/Search page
	When I select "Yes" from the TrafficControlRequired dropdown
	And I press Search
	Then I should see the error message "No results matched your query."

# TODO: make this test work
@ignore
Scenario: admin can search by markout required *
	Given I do not currently function
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderScheduling/Search page
	When I select "True" from the MarkoutRequired dropdown
	And I press Search
	Then I should see the error message "No results matched your query."
	
# TODO: make this test work
@ignore
Scenario: admin can search by pending assignment *
	Given I do not currently function
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderScheduling/Search page
	When I select "True" from the PendingAssignment dropdown
	And I press Search
	Then I should see the error message "No results matched your query."

# TODO: make this test work
@ignore
Scenario: admin can search by markout expiration within next *
	Given I do not currently function
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderScheduling/Search page
	When I enter "foo" into the MarkoutExpirationDays field
	And I press Search
	Then I should see the validation message "Must be a 1 or 2 digit numerical value."
	When I enter "2" into the MarkoutExpirationDays field
	And I press Search
	Then I should see the error message "No results matched your query."

Scenario: admin tries to submit without choosing a crew
    Given a scheduling work order with valve "one" exists with contractor: "one", markout requirement: "routine"
    And a markout "one" exists with ready date: "yesterday", scheduling work order with valve: "one"
	And a crew assignment "ca" exists with scheduling work order with valve: "one", crew: "one", assigned for: "today", assigned on: "today"
    And I am logged in as "admin@site.com", password: "testpassword#1"
    And I am at the WorkOrderScheduling/Search page
    And I have entered scheduling work order with valve "one"'s Id into the Id field
    And I have pressed Search
    When I enter crew assignment "ca"'s AssignedFor into the AssignFor field
    And I click the checkbox named WorkOrderIDs with scheduling work order with valve "one"'s Id
    And I press Submit
    Then I should see the validation message "The Crew field is required."

Scenario: admin tries to submit without entering a date
    Given a scheduling work order with valve "one" exists with contractor: "one", markout requirement: "routine"
    And a markout "one" exists with ready date: "yesterday", scheduling work order with valve: "one"
	And a crew assignment "ca" exists with scheduling work order with valve: "one", crew: "one", assigned for: "today", assigned on: "today"
    And I am logged in as "admin@site.com", password: "testpassword#1"
    And I am at the WorkOrderScheduling/Search page
    And I have entered scheduling work order with valve "one"'s Id into the Id field
    And I have pressed Search
    When I select crew "one"'s Description from the Crew dropdown
    And I click the checkbox named WorkOrderIDs with scheduling work order with valve "one"'s Id
    And I press Submit
    Then I should see the validation message "The AssignFor field is required."

Scenario: admin tries to submit without picking any orders
    Given a scheduling work order with valve "one" exists with contractor: "one", markout requirement: "routine"
    And a markout "one" exists with ready date: "yesterday", scheduling work order with valve: "one"
	And a crew assignment "ca" exists with scheduling work order with valve: "one", crew: "one", assigned for: "today", assigned on: "today"
    And I am logged in as "admin@site.com", password: "testpassword#1"
    And I am at the WorkOrderScheduling/Search page
    And I have entered scheduling work order with valve "one"'s Id into the Id field
    And I have pressed Search
    When I select crew "one"'s Description from the Crew dropdown
    And I enter crew assignment "ca"'s AssignedFor into the AssignFor field
    And I press Submit
    Then I should see the validation message "You must pick at least one order to assign."	

Scenario: admin successfully assigns work
    Given a scheduling work order with valve "one" exists with contractor: "one", markout requirement: "routine"
    And a markout "one" exists with ready date: "yesterday", scheduling work order with valve: "one"
	#And a crew assignment "ca" exists with scheduling work order with valve: "one", crew: "one", assigned for: "today", assigned on: "today"
	And a street opening permit exists with expiration date: "tomorrow", date issued: "yesterday", scheduling work order with valve: "one" 
    And I am logged in as "admin@site.com", password: "testpassword#1"
    And I am at the WorkOrderScheduling/Search page
    And I have entered scheduling work order with valve "one"'s Id into the Id field
    And I have pressed Search
    When I select crew "one"'s Description from the Crew dropdown
    And I enter the date "today" into the AssignFor field
    And I click the checkbox named WorkOrderIDs with scheduling work order with valve "one"'s Id
    And I press Submit
	Then I should not see "You must pick at least one order to assign."
	And I should be at the CrewAssignment/ShowCalendar page 
	And crew "one"'s Description should be selected in the Crew dropdown
	And I should see "today's date" in the Date field
	And I should see scheduling work order with valve "one"'s Id in the table assignmentsTable's "Order Number" column

Scenario: user can schedule a work order when priority is emergency, markout is not required, street opening permit is Required, but no street opening permits
    Given a scheduling work order with valve "one" exists with contractor: "one", markout requirement: "none", permit required: "true", priority: "emergency"
	And a crew assignment "ca" exists with crew: "one", assigned for: "today", assigned on: "today"
	And I am logged in as "admin@site.com", password: "testpassword#1"
    And I am at the WorkOrderScheduling/Search page
    And I have entered scheduling work order with valve "one"'s Id into the Id field
    And I have pressed Search
    When I select crew "one"'s Description from the Crew dropdown
    And I enter crew assignment "ca"'s AssignedFor into the AssignFor field
    And I click the checkbox named WorkOrderIDs with scheduling work order with valve "one"'s Id
    And I press Submit
	Then I should be at the CrewAssignment/ShowCalendar page 
	And crew "one"'s Description should be selected in the Crew dropdown
	And I should see "today's date" in the Date field

Scenario: admin searches by town, no orders exist
    Given a town "one" exists with shortname: "Foo Town", operating center: "one"
    And operating center "one" exists in town "one"
    And I am logged in as "admin@site.com", password: "testpassword#1"
    And I am at the WorkOrderScheduling/Search page
    When I select operating center "one"'s Name from the OperatingCenter dropdown
	And I wait for element Town to exist
	And I select town "one"'s ShortName from the Town dropdown
    And I press Search
   # Then I should be at the WorkOrderScheduling/Search screen
	Then I should see "Work Order - Scheduling - Search"
    And I should see the error message "No results matched your query."

Scenario: admin tries to search without entering any criteria
    Given I am logged in as "admin@site.com", password: "testpassword#1"
    And I am at the WorkOrderScheduling/Search page
    When I press Search
    Then I should see the error message "No search criteria chosen."

Scenario: search cascades street off town
	Given I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderScheduling/Search page
	When I select operating center "one"'s Name from the OperatingCenter dropdown
	And I wait for element Town to exist
	And I select town "Foo"'s ShortName from the Town dropdown
	And I wait for element TownSection to exist
	And I wait for element Street to exist
	And I wait for element NearestCrossStreet to exist
	Then I should see town section "Little Foo"'s Name in the TownSection dropdown
	And I should see street "one"'s FullStName in the Street dropdown
	And I should see street "one"'s FullStName in the NearestCrossStreet dropdown

Scenario: search cascades work descriptions off asset type
	Given an asset type "hydrant" exists with description: "hydrant", operating center: "one"
	And an asset type "valve" exists with description: "valve", operating center: "one"
	And a hydrant work description "one" exists with time to complete: "1"
	And a valve work description "two" exists with time to complete: "2"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderScheduling/Search page
	When I select asset type "hydrant"'s Description from the AssetType dropdown
	And I wait for element WorkDescription to be enabled
	Then I should see hydrant work description "one"'s Description in the WorkDescription dropdown
	And I should not see valve work description "two"'s Description in the WorkDescription dropdown
	When I select asset type "valve"'s Description from the AssetType dropdown
	And I wait for element WorkDescription to be enabled
	Then I should see valve work description "two"'s Description in the WorkDescription dropdown
	And I should not see hydrant work description "one"'s Description in the WorkDescription dropdown

Scenario: admin schedules work when markout requirement emergency and a previous markout exists
    Given a scheduling work order with valve "one" exists with contractor: "one", markout requirement: "emergency"
    And a markout "one" exists with date of request: "95 days ago", ready date: "90 days ago", expiration date: "80 days ago", scheduling work order with valve: "one"
    And I am logged in as "admin@site.com", password: "testpassword#1"
    And I am at the WorkOrderScheduling/Search page
    And I have entered scheduling work order with valve "one"'s Id into the Id field
    And I have pressed Search
    When I select crew "one"'s Description from the Crew dropdown
	And I enter the date "today" into the AssignFor field
    And I click the checkbox named WorkOrderIDs with scheduling work order with valve "one"'s Id
    And I press Submit
	Then I should not see "One or more of the work orders chosen does not have a markout that is valid on the scheduled date" 
	And I should not see "You must pick at least one order to assign."
	And I should not see "Please enter a valid date."
	And I should be at the CrewAssignment/ShowCalendar page 
	And crew "one"'s Description should be selected in the Crew dropdown
	And I should see "today's date" in the Date field

Scenario: user can page and sort by id correctly
	Given an asset type "valve" exists with description: "valve", operating center: "one"
	And a work order "one" exists with valve: "one", permit required: "true", contractor: "one", markout requirement: "none"
	And a scheduling work order with valve "two" exists with contractor: "one", operating center: "one", markout requirement: "emergency"
	And a scheduling work order with valve "three" exists with contractor: "one", operating center: "one", markout requirement: "emergency"
	And I am logged in as "admin@site.com", password: "testpassword#1" 
	And I am at the WorkOrderScheduling/Search page
	When I select "Valve" from the AssetType dropdown
	And I press Search
	Then I should see scheduling work order with valve "two"'s Id in the "Order #" column
	And I should see scheduling work order with valve "three"'s Id in the "Order #" column
	And I should not see work order "one"'s Id in the "Order #" column
	When I follow "1"
	Then I should see scheduling work order with valve "two"'s Id in the "Order #" column
	And I should not see scheduling work order with valve "three"'s Id in the "Order #" column
	And I should not see work order "one"'s Id in the "Order #" column
	When I follow ">>" 
	Then I should not see scheduling work order with valve "two"'s Id in the "Order #" column
	And I should see scheduling work order with valve "three"'s Id in the "Order #" column
	And I should not see work order "one"'s Id in the "Order #" column
	When I follow "Order #"
	Then I should not see scheduling work order with valve "two"'s Id in the "Order #" column
	And I should see scheduling work order with valve "three"'s Id in the "Order #" column
	And I should not see work order "one"'s Id in the "Order #" column
	When I follow "Order #"
	Then I should see scheduling work order with valve "two"'s Id in the "Order #" column
	And I should not see scheduling work order with valve "three"'s Id in the "Order #" column
	And I should not see work order "one"'s Id in the "Order #" column
	When I follow "All" 
	Then I should see scheduling work order with valve "two"'s Id in the "Order #" column
	And I should see scheduling work order with valve "three"'s Id in the "Order #" column
	And I should not see work order "one"'s Id in the "Order #" column
	
Scenario: user can schedule a work order when street opening permit is not required and invalid street opening permits exist
	Given a scheduling work order with valve "one" exists with contractor: "one", markout requirement: "routine", permit required: "false"
	And a street opening permit exists with date issued: "yesterday", scheduling work order with valve: "one"
	And a markout "one" exists with ready date: "today", scheduling work order with valve: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
    And I am at the WorkOrderScheduling/Search page
    And I have entered scheduling work order with valve "one"'s Id into the Id field
    And I have pressed Search
    When I select crew "one"'s Description from the Crew dropdown
    And I enter markout "one"'s ReadyDate into the AssignFor field
    And I click the checkbox named WorkOrderIDs with scheduling work order with valve "one"'s Id
	And I press Submit
	Then I should not see "Nullable object must have a value."
	And I should be at the CrewAssignment/ShowCalendar page
	And crew "one"'s Description should be selected in the Crew dropdown
	And I should see "today's date" in the Date field