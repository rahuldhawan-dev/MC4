Feature: Planning Page
	In order to plan work
	As a user
	I want to be able to enter planning information

Background: admin and non-admin users exist
	Given an operating center "one" exists with opcntr: "NJX", opcntrname: "Shrewsburgh"
	And a town "Foo" exists with shortname: "Foo", operating center: "one"
    And operating center "one" exists in town "Foo"
	And a town section "Little Foo" exists with name: "Little Foo", town: "Foo"
	And a contractor "one" exists with name: "one", operating center: "one"
	And a user exists with email: "user@site.com", password: "testpassword#1", contractor: "one"
	And an admin user exists with email: "admin@site.com", password: "testpassword#1", contractor: "one"
	And a data type "work order" exists with table name: "WorkOrders", name: "Work Order"

Scenario: user cannot access the planning page
	Given I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the WorkOrderPlanning/Search page
    Then I should be at the forbidden screen

Scenario: admin access the planning search
	Given I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderPlanning/Search page
	Then I should see "Work Orders - Planning - Search"
	And I should see "Work Order"
	And I should see "Town" 
	And I should see "Town Section"
	And I should see "Street"
	And I should see "Street Number"
    And I should see "Nearest Cross Street"
    And I should see "Asset Type"
    And I should see "Work Description"
    And I should see "Priority"
    And I should see "Date Received"
    And I should see "Requested By"
    And I should see "Markout Requirement"
    And I should see "Street Opening Permit Required"
	And I should see "Purpose"

# There are race conditions that can't be managed here. The ajax calls cause the 
# Session to be accessed simultaneously and we haven't developed a work around
Scenario: search cascades street off town
	Given a street "one" exists with name: "Wall St", town: "Foo"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderPlanning/Search page
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
	And I am at the WorkOrderPlanning/Search page
	When I select asset type "hydrant"'s Description from the AssetType dropdown
	And I wait for element WorkDescription to be enabled
	Then I should see hydrant work description "one"'s Description in the WorkDescription dropdown
	And I should not see valve work description "two"'s Description in the WorkDescription dropdown
	When I select asset type "valve"'s Description from the AssetType dropdown
	And I wait for element WorkDescription to be enabled
	Then I should see valve work description "two"'s Description in the WorkDescription dropdown
	And I should not see hydrant work description "one"'s Description in the WorkDescription dropdown

Scenario: can access a work order after searching
	Given a planning work order with valve "one" exists with valve: "one", contractor: "one"
	And a markout "one" exists with expiration date: "yesterday", planning work order with valve: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderPlanning/Search page
	When I enter planning work order with valve "one"'s Id into the Id field
	And I press Search
	And I wait for the page to reload
	And I follow the show link for planning work order with valve "one"
	And I wait for the page to reload
	Then I should be at the WorkOrderPlanning/Show page for planning work order with valve: "one"
	And I should see "Order Number: " 

# Searches
Scenario: admin searches for an order by number
	Given a planning work order with valve "one" exists with valve: "one", contractor: "one"
	And a planning work order with valve "two" exists with valve: "one", contractor: "one"
	And a markout "one" exists with expiration date: "yesterday", planning work order with valve: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderPlanning/Search page
	When I enter planning work order with valve "one"'s Id into the Id field
	And I press Search
	#TODO 
	Then I should see planning work order with valve: "one"'s Id on the page
	And I should see "Markout Expiration Date"
	And I should see "Description of Job"
	And I should see markout "one"'s ExpirationDate as a date without seconds in the "Markout Expiration Date" column

Scenario: search returns correct work orders when searching by operating center
	Given a planning work order with valve "one" exists with valve: "one", operating center: "one", contractor: "one"
	And an operating center "two" exists with opcntr: "NJY", opcntrname: "Lakebury"
	And a town "Bar" exists with shortname: "Bar", operating center: "two"
	And a planning work order with main "one" exists with operating center: "two", town: "Bar", contractor: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderPlanning/Search page
	When I select operating center "one"'s Name from the OperatingCenter dropdown
	And I press Search
	Then I should see planning work order with valve "one"'s Id in the "Order #" column 
	And I should not see planning work order with main "one"'s Id in the "Order #" column	
	
Scenario: search returns correct work orders when searching by town
	Given a planning work order with valve "one" exists with valve: "one", operating center: "one", contractor: "one", town: "Foo"
	And an operating center "two" exists with opcntr: "NJY", opcntrname: "Lakebury"
	And a town "Bar" exists with shortname: "Bar", operating center: "two"
	And a planning work order with main "one" exists with operating center: "two", town: "Bar", contractor: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderPlanning/Search page
	When I select operating center "one"'s Name from the OperatingCenter dropdown
	And I wait for element Town to exist
	And I select town "Foo"'s ShortName from the Town dropdown
	And I press Search
	Then I should see planning work order with valve "one"'s Id in the "Order #" column
	And I should not see planning work order with main "one"'s Id in the "Order #" column	

Scenario: search returns correct work orders when searching by town section
	Given a planning work order with valve "one" exists with valve: "one", operating center: "one", contractor: "one", town: "Foo", town section: "Little Foo"
	And an operating center "two" exists with opcntr: "NJY", opcntrname: "Lakebury"
	And a town "Bar" exists with shortname: "Bar", operating center: "two"
	And a planning work order with main "one" exists with operating center: "two", town: "Bar", contractor: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderPlanning/Search page
	When I select operating center "one"'s Name from the OperatingCenter dropdown
	And I wait for element Town to exist
	And I select town "Foo"'s ShortName from the Town dropdown
	And I wait for element TownSection to exist
	And I select town section "Little Foo"'s Name from the TownSection dropdown
	And I press Search
	Then I should see planning work order with valve "one"'s Id in the "Order #" column
	And I should not see planning work order with main "one"'s Id in the "Order #" column	

Scenario: search returns correct work orders when searching by street
	Given a street "one" exists with name: "Easy St", town: "Foo"
	And a planning work order with valve "one" exists with valve: "one", operating center: "one", contractor: "one", town: "Foo", town section: "Little Foo", street: "one"
	And an operating center "two" exists with opcntr: "NJY", opcntrname: "Lakebury"
	And a town "Bar" exists with shortname: "Bar", operating center: "two"
	And a planning work order with main "one" exists with operating center: "two", town: "Bar", contractor: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderPlanning/Search page
	When I select operating center "one"'s Name from the OperatingCenter dropdown
	And I wait for element Town to exist
	And I select town "Foo"'s ShortName from the Town dropdown
	And I wait for element Street to exist
	And I select street "one"'s FullStName from the Street dropdown
	And I press Search
	Then I should see planning work order with valve "one"'s Id in the "Order #" column
	And I should not see planning work order with main "one"'s Id in the "Order #" column	

Scenario: search returns correct work orders when searching by street number
	Given a planning work order with valve "one" exists with valve: "one", operating center: "one", contractor: "one", street number: "808"
	And a planning work order with main "one" exists with operating center: "one", contractor: "one", street number: "666"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderPlanning/Search page
	When I enter planning work order with valve "one"'s StreetNumber into the StreetNumber field
	And I press Search
	Then I should see planning work order with valve "one"'s Id in the "Order #" column 
	And I should not see planning work order with main "one"'s Id in the "Order #" column	

Scenario: search returns correct work orders when searching by nearest cross street
	Given a street "one" exists with name: "Easy St", town: "Foo"
	And a street "two" exists with name: "Shakedown St", town: "Foo"
	And a planning work order with valve "one" exists with valve: "one", operating center: "one", contractor: "one", town: "Foo", town section: "Little Foo", nearest cross street: "one"
	And a planning work order with main "one" exists with operating center: "one", contractor: "one", town: "Foo", town section: "Little Foo", nearest cross street: "two"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderPlanning/Search page
	When I select operating center "one"'s Name from the OperatingCenter dropdown
	And I wait for element Town to exist
	And I select town "Foo"'s ShortName from the Town dropdown
	And I wait for element NearestCrossStreet to exist
	And I select street "one"'s FullStName from the NearestCrossStreet dropdown
	And I press Search
	Then I should see planning work order with valve "one"'s Id in the "Order #" column
	And I should not see planning work order with main "one"'s Id in the "Order #" column	

Scenario: search returns correct work orders when searching by asset type
	Given a planning work order with valve "one" exists with valve: "one", operating center: "one", contractor: "one"
	And a planning work order with main "one" exists with operating center: "one", town: "Foo", contractor: "one"
	And an asset type "valve" exists with description: "valve", operating center: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderPlanning/Search page
	When I select asset type "valve"'s Description from the AssetType dropdown
	And I press Search
	Then I should see planning work order with valve "one"'s Id in the "Order #" column
	And I should not see planning work order with main "one"'s Id in the "Order #" column	

Scenario: search returns correct work orders when searching by work description
	Given an asset type "valve" exists with description: "valve", operating center: "one"
	And a valve work description "one" exists with time to complete: "1.1234", asset type: "valve", description: "turn valve"
	And a planning work order with valve "one" exists with valve: "one", operating center: "one", contractor: "one", valve work description: "one"
	And a planning work order with valve "two" exists with operating center: "one", town: "Foo", contractor: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderPlanning/Search page
	When I select asset type "valve"'s Description from the AssetType dropdown
	And I wait for element WorkDescription to exist
	And I select valve work description "one"'s Description from the WorkDescription dropdown
	And I press Search
	Then I should see planning work order with valve "one"'s Id in the "Order #" column
	And I should not see planning work order with valve "two"'s Id in the "Order #" column	

Scenario: search returns correct work orders when searching by priority
	Given a work order priority "one" exists with description: "Emergency"
	And a planning work order with valve "one" exists with valve: "one", operating center: "one", contractor: "one", priority: "emergency"
	And a planning work order with main "one" exists with operating center: "one", town: "Foo", contractor: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderPlanning/Search page
	When I select work order priority "one"'s Description from the Priority dropdown
	And I press Search
	Then I should see planning work order with valve "one"'s Id in the "Order #" column
	And I should not see planning work order with main "one"'s Id in the "Order #" column	

Scenario: search returns correct work orders when searching by date received
	Given a planning work order with valve "one" exists with valve: "one", operating center: "one", contractor: "one", date received: "01/02/2012"
	And a planning work order with main "one" exists with operating center: "one", town: "Foo", contractor: "one", date received: "01/01/2012"
	And an asset type "valve" exists with description: "valve", operating center: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderPlanning/Search page
	When I enter planning work order with valve "one"'s DateReceived into the DateReceived field
	And I press Search
	Then I should see planning work order with valve "one"'s Id in the "Order #" column
	And I should not see planning work order with main "one"'s Id in the "Order #" column	

Scenario: search returns correct work orders when searching by requested by
	Given a requested by "one" exists with description: "Local Government"
	And a requested by "two" exists with description: "Employee"
	And a planning work order with valve "one" exists with valve: "one", operating center: "one", contractor: "one", requested by: "one"
	And a planning work order with valve "two" exists with operating center: "one", town: "Foo", contractor: "one", requested by: "two"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderPlanning/Search page
	When I select "Local Government" from the Requester dropdown
	And I press Search
	Then I should see planning work order with valve "one"'s Id in the "Order #" column
	And I should not see planning work order with valve "two"'s Id in the "Order #" column	

Scenario: search returns correct work orders when searching by markout requirement
	Given a planning work order with valve "one" exists with valve: "one", operating center: "one", contractor: "one", markout requirement: "emergency"
	And a planning work order with valve "two" exists with operating center: "one", town: "Foo", contractor: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderPlanning/Search page
	When I select "Emergency" from the MarkoutRequirement dropdown
	And I press Search
	Then I should see planning work order with valve "one"'s Id in the "Order #" column
	And I should not see planning work order with valve "two"'s Id in the "Order #" column	

Scenario: search returns correct work orders when searching by purpose
	Given a work order purpose "one" exists with description: "revenue above 1000"
	And a work order purpose "two" exists with description: "customer"
	And a planning work order with valve "one" exists with valve: "one", operating center: "one", contractor: "one", work order purpose: "one"
	And a planning work order with main "one" exists with operating center: "one", town: "Foo", contractor: "one", work order purpose: "two"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderPlanning/Search page
	When I select work order purpose "one"'s Description from the Purpose dropdown
	And I press Search
	Then I should see planning work order with valve "one"'s Id in the "Order #" column
	And I should not see planning work order with main "one"'s Id in the "Order #" column	

Scenario: search returns correct valve work orders when markout required
	Given an asset type "valve" exists with description: "valve", operating center: "one"
	And a planning work order with valve "one" exists with valve: "one", contractor: "one"
	And a markout "one" exists with expiration date: "tomorrow", planning work order with valve: "one"
	And a planning work order with valve "two" exists with valve: "one", contractor: "one"
	And a markout "two" exists with expiration date: "yesterday", planning work order with valve: "two"
	And a work order "three" exists with valve: "one", permit required: "false", contractor: "one", markout requirement: "none"
	And a planning work order with valve "four" exists with valve: "one", permit required: "true", contractor: "one"
	And a planning work order with valve "five" exists with valve: "one", permit required: "true", contractor: "one"
	And a street opening permit exists with expiration date: "tomorrow", date issued: "yesterday", planning work order with valve: "five" 
	And a markout "five" exists with expiration date: "tomorrow", planning work order with valve: "five"
	And a planning work order with valve "six" exists with valve: "one", permit required: "true", contractor: "one"
	And a street opening permit exists with expiration date: "yesterday", date issued: "yesterday", planning work order with valve: "six"
	And I am logged in as "admin@site.com", password: "testpassword#1" 
	And I am at the WorkOrderPlanning/Search page
	When I select "Valve" from the AssetType dropdown
	And I wait for ajax to finish loading
	And I press Search
	Then I should not see planning work order with valve "one"'s Id in the "Order #" column 
	And I should see planning work order with valve "two"'s Id in the "Order #" column
	And I should not see work order "three"'s Id in the "Order #" column
	And I should see planning work order with valve "two"'s StreetNumber in the "Street Number" column
	And I should see planning work order with valve "four"'s Id in the "Order #" column
	And I should not see planning work order with valve "five"'s Id in the "Order #" column
	And I should see planning work order with valve "six"'s Id in the "Order #" column

Scenario: search returns correct work orders when street opening permit required
	Given an asset type "valve" exists with description: "valve", operating center: "one"
	And a planning work order with valve "one" exists with valve: "one", contractor: "one", permit required: "true"
	And a planning work order with valve "two" exists with valve: "one", contractor: "one", permit required: "false"
	And I am logged in as "admin@site.com", password: "testpassword#1" 
	And I am at the WorkOrderPlanning/Search page
	When I select "Required" from the StreetOpeningPermitRequired dropdown
	And I press Search
	Then I should see planning work order with valve "one"'s Id in the "Order #" column 
	And I should not see planning work order with valve "two"'s Id in the "Order #" column

# paging/sorting
Scenario: user can page and sort by id correctly
	Given an asset type "valve" exists with description: "valve", operating center: "one"
	And a work order "one" exists with valve: "one", permit required: "false", contractor: "one", markout requirement: "none"
	And a planning work order with valve "two" exists with valve: "one", contractor: "one", markout requirement: "none", permit required: "true"
	And a planning work order with valve "three" exists with valve: "one", contractor: "one", markout requirement: "none", permit required: "true"
	And I am logged in as "admin@site.com", password: "testpassword#1" 
	And I am at the WorkOrderPlanning/Search page
	When I select "Valve" from the AssetType dropdown
	And I press Search
	Then I should see planning work order with valve "two"'s Id in the "Order #" column
	And I should see planning work order with valve "three"'s Id in the "Order #" column
	And I should not see work order "one"'s Id in the "Order #" column
	When I follow "1"
	Then I should see planning work order with valve "two"'s Id in the "Order #" column
	And I should not see planning work order with valve "three"'s Id in the "Order #" column
	And I should not see work order "one"'s Id in the "Order #" column
	When I follow ">>" 
	Then I should not see planning work order with valve "two"'s Id in the "Order #" column
	And I should see planning work order with valve "three"'s Id in the "Order #" column
	And I should not see work order "one"'s Id in the "Order #" column
	When I follow "Order #"
	Then I should not see planning work order with valve "two"'s Id in the "Order #" column
	And I should see planning work order with valve "three"'s Id in the "Order #" column
	And I should not see work order "one"'s Id in the "Order #" column
	When I follow "Order #"
	Then I should see planning work order with valve "two"'s Id in the "Order #" column
	And I should not see planning work order with valve "three"'s Id in the "Order #" column
	And I should not see work order "one"'s Id in the "Order #" column
	When I follow "All" 
	Then I should see planning work order with valve "two"'s Id in the "Order #" column
	And I should see planning work order with valve "three"'s Id in the "Order #" column
	And I should not see work order "one"'s Id in the "Order #" column

# Tabs
Scenario: cannot see markout tab if markout not required
	Given a planning work order with valve "one" exists with valve: "one", contractor: "one", markout requirement: "none", permit required: "true"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for planning work order with valve: "one"
	When I wait 2 seconds
	Then I should not see the markoutsTab element
	And I should not see the markoutsTabLink element
	
Scenario: cannot see street opening permit tab if street opening permit not required
	Given a planning work order with valve "one" exists with valve: "one", contractor: "one", permit required: "false"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for planning work order with valve: "one"
	When I wait 2 seconds
	Then I should not see the streetPermitTabLink element
	And I should not see the streetPermitTab element

# Map
Scenario: can load a map on show view
	Given a planning work order with valve "one" exists with valve: "one", contractor: "one"
	And a markout "one" exists with expiration date: "yesterday", planning work order with valve: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for planning work order with valve: "one"
	When I click the "Map" tab
	And I wait for element googleMap to exist
	Then The element googleMap should have the attribute data-latitude with planning work order with valve "one"'s Latitude
	And The element googleMap should have the attribute data-longitude with planning work order with valve "one"'s Longitude

# Crew Assignments
Scenario: can see crew assignments for work order
	Given a valve work description "one" exists with time to complete: "1.1234"
	And a planning work order with valve "one" exists with valve: "one", contractor: "one", valve work description: "one"
	And a markout "one" exists with expiration date: "yesterday", planning work order with valve: "one"
	And a crew "one" exists with description: "one", contractor: "one", availability: "8"
    And a crew assignment "ca" exists with planning work order with valve: "one", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000", date started: "today", date ended: "tomorrow", employees on job: "13" 
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for planning work order with valve: "one"
	When I click the "Crew Assignments" tab
	And I wait for element assignmentsTable to exist
	Then I should see valve work description "one"'s TimeToComplete as a decimal rounded to two places in the table assignmentsTable's "Est. TTC (hours)" column
	And I should see crew "one"'s Description in the table assignmentsTable's "Assigned To" column
	And I should see crew assignment "ca"'s AssignedOn as a date in the table assignmentsTable's "Assigned On" column
	And I should see crew assignment "ca"'s AssignedFor as a date in the table assignmentsTable's "Assigned For" column
	And I should see crew assignment "ca"'s DateStarted as a date without seconds in the table assignmentsTable's "Start Time" column
	And I should see crew assignment "ca"'s DateEnded as a date without seconds in the table assignmentsTable's "End Time" column
	And I should see crew assignment "ca"'s EmployeesOnJob in the table assignmentsTable's "Employees On Crew" column

Scenario: user should not see crew assignments for other contractors
	Given a valve work description "one" exists with time to complete: "1.1234"
	And a contractor "bad contractor" exists with name: "bad contractor", operating center: "one"
	And a planning work order with valve "one" exists with valve: "one", contractor: "one", valve work description: "one"
	And a markout "one" exists with expiration date: "yesterday", planning work order with valve: "one"
	And a crew "one" exists with description: "one", contractor: "one", availability: "8"
	And a crew "bad crew" exists with description: "bad crew", contractor: "bad contractor"
    And a crew assignment "ca" exists with planning work order with valve: "one", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000", date started: "today", date ended: "tomorrow", employees on job: "13" 
    And a crew assignment "bad ass" exists with planning work order with valve: "one", crew: "bad crew", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000", date started: "today", date ended: "tomorrow", employees on job: "13" 
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for planning work order with valve: "one"
	When I click the "Crew Assignments" tab
	And I wait for element assignmentsTable to exist
	Then I should see valve work description "one"'s TimeToComplete as a decimal rounded to two places in the table assignmentsTable's "Est. TTC (hours)" column
	And I should see crew "one"'s Description in the table assignmentsTable's "Assigned To" column
	And I should see crew assignment "ca"'s AssignedOn as a date in the table assignmentsTable's "Assigned On" column
	And I should see crew assignment "ca"'s AssignedFor as a date in the table assignmentsTable's "Assigned For" column
	And I should see crew assignment "ca"'s DateStarted as a date without seconds in the table assignmentsTable's "Start Time" column
	And I should see crew assignment "ca"'s DateEnded as a date without seconds in the table assignmentsTable's "End Time" column
	And I should see crew assignment "ca"'s EmployeesOnJob in the table assignmentsTable's "Employees On Crew" column
	And I should not see crew "bad crew"'s Description in the table assignmentsTable's "Assigned To" column

# Traffic 	
Scenario: admin views the traffic tab
	Given a planning work order with valve "one" exists with valve: "one", contractor: "one", notes: "i am notes", traffic control required: "true"
	And a markout "one" exists with expiration date: "yesterday", planning work order with valve: "one"
	And a crew "one" exists with description: "one", contractor: "one", availability: "8"
    And a crew assignment "ca" exists with planning work order with valve: "one", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for planning work order with valve: "one"
	When I click the "Traffic Control/Notes" tab
	And I wait for element trafficDetails to exist
	Then I should at least see planning work order with valve "one"'s Notes in the trafficTab element
	And I should at least see "Yes" in the trafficTab element

Scenario: admin succesfully edits the traffic tab
	Given a planning work order with valve "one" exists with valve: "one", contractor: "one", notes: "i am notes", traffic control required: "true"
	And a markout "one" exists with expiration date: "yesterday", planning work order with valve: "one"
	And a crew "one" exists with description: "one", contractor: "one", availability: "8"
    And a crew assignment "ca" exists with planning work order with valve: "one", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for planning work order with valve: "one"
	When I click the "Traffic Control/Notes" tab
	And I wait for element trafficDetails to exist
	And I follow "Edit Traffic Requirements"
	And I wait for element trafficEdit to exist
	And I enter "42" into the NumberOfOfficersRequired field
	And I enter "some more notes" into the AppendedNotes field
	And I press SaveTrafficRequirements
	And I wait for element trafficDetails to exist
	Then I should at least see planning work order with valve "one"'s Notes in the trafficTab element
	And I should at least see "Yes" in the trafficTab element
	And I should at least see "42" in the trafficTab element

Scenario: admin edits the traffic tab but enters a non-number in the number of officers field
	Given a planning work order with valve "one" exists with valve: "one", contractor: "one", notes: "i am notes", traffic control required: "true"
	And a markout "one" exists with expiration date: "yesterday", planning work order with valve: "one"
	And a crew "one" exists with description: "one", contractor: "one", availability: "8"
    And a crew assignment "ca" exists with planning work order with valve: "one", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for planning work order with valve: "one"
	When I click the "Traffic Control/Notes" tab
	And I wait for element trafficDetails to exist
	And I follow "Edit Traffic Requirements"
	And I wait for element trafficEdit to exist
	And I enter "the brady bunch" into the NumberOfOfficersRequired field
	And I press SaveTrafficRequirements
	Then I should at least see "The field NumberOfOfficersRequired must be a number" in the trafficTab element

# Markouts
Scenario: can view markouts for work order
	Given a planning work order with valve "one" exists with valve: "one", contractor: "one"
	And a markout "one" exists with expiration date: "yesterday", planning work order with valve: "one"
	And a crew "one" exists with description: "one", contractor: "one", availability: "8"
    And a crew assignment "ca" exists with planning work order with valve: "one", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for planning work order with valve: "one"
	When I click the "Markouts" tab
	Then I should see markout "one"'s ExpirationDate as a date without seconds in the table markoutsTable's "Expiration Date" column
	And I should see markout "one"'s MarkoutNumber in the table markoutsTable's "Markout Number" column

Scenario: can edit a markout for a work order
	Given a planning work order with valve "one" exists with valve: "one", contractor: "one"
	And a markout "one" exists with expiration date: "yesterday", planning work order with valve: "one", date of request: "yesterday"
	And a markout type "one" exists with description: "Foo"
	And a crew "one" exists with description: "one", contractor: "one", availability: "8"
    And a crew assignment "ca" exists with planning work order with valve: "one", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for planning work order with valve: "one"
	When I click the "Markouts" tab
	And I follow "Edit"
	And I wait for element MarkoutType to exist
	And I enter "213812345" into the MarkoutNumber field
	And I select markout type "one"'s Description from the MarkoutType dropdown
	And I press "Update Markout"
	And I wait for the dialog to close
	Then I should see "213812345" in the table markoutsTable's "Markout Number" column
	And I should see markout type "one"'s Description in the table markoutsTable's "Markout Type" column
	And I should not see "An error occured while communicating to the server."

Scenario: can edit a markout for a work order with ready date and expiration date 
    Given an operating center "two" exists with opcntr: "NJY", opcntrname: "Giggletown", markouts editable: "true"
	And a town "Foo2" exists with shortname: "Foo", operating center: "two"
	And a town section "Little Foo2" exists with name: "Little Foo", town: "Foo2"
	And a contractor "two" exists with name: "two", operating center: "two"
	And an admin user exists with email: "admin2@site.com", password: "testpassword#1", contractor: "two"
	And a planning work order with valve "one" exists with valve: "one", contractor: "two", operating center: "two"
	And a markout "one" exists with expiration date: "yesterday", planning work order with valve: "one", date of request: "yesterday"
	And a markout type "one" exists with description: "Foo"
	And a crew "one" exists with description: "one", contractor: "two", availability: "8"
    And a crew assignment "ca" exists with planning work order with valve: "one", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"
	And I am logged in as "admin2@site.com", password: "testpassword#1"
	And I am at the show page for planning work order with valve: "one"
	When I click the "Markouts" tab
	And I follow "Edit"
	And I wait for element MarkoutType to exist
	Then I should see the ReadyDate field
	And I should see the ExpirationDate field
	When I enter "notninety" into the MarkoutNumber field
	And I select markout type "one"'s Description from the MarkoutType dropdown
	And I enter "9/30/2011" into the ReadyDate field
	And I enter "10/1/2011" into the ExpirationDate field
	And I press "Update Markout"
	And I wait for the dialog to close
	Then I should see "notninety" in the table markoutsTable's "Markout Number" column
	And I should see markout type "one"'s Description in the table markoutsTable's "Markout Type" column
	And I should see "9/30/2011" in the table markoutsTable's "Ready Date" column
	And I should see "10/1/2011" in the table markoutsTable's "Expiration Date" column
	And I should not see "An error occured while communicating to the server."

Scenario: can create a new markout for a work order
	Given a planning work order with valve "one" exists with valve: "one", contractor: "one"
	And a markout type "one" exists with description: "Foo"
	And a markout type "two" exists with description: "C TO Curbage"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for planning work order with valve: "one"
	When I click the "Markouts" tab
	And I follow "Add New Markout"
	And I wait for element MarkoutType to exist
	Then I should not see the ReadyDate field
	And I should not see the ExpirationDate field
	When I enter "213812345" into the MarkoutNumber field
	And I select markout type "two"'s Description from the MarkoutType dropdown
	And I enter "9/29/2011" into the DateOfRequest field
	And I press "Save Markout"
	And I wait for the dialog to close
	Then I should see "213812345" in the table markoutsTable's "Markout Number" column
	And I should see markout type "two"'s Description in the table markoutsTable's "Markout Type" column
	And I should see "9/29/2011" in the table markoutsTable's "Date Of Request" column

Scenario: can create a new markout for a work order with ready date and expiration date 
	Given an operating center "two" exists with opcntr: "NJY", opcntrname: "Giggletown", markouts editable: "true"
	And a town "Foo2" exists with shortname: "Foo", operating center: "two"
	And a town section "Little Foo2" exists with name: "Little Foo", town: "Foo2"
	And a contractor "two" exists with name: "two", operating center: "two"
	And an admin user exists with email: "user2@site.com", password: "testpassword#1", contractor: "two"
	And a planning work order with valve "one" exists with valve: "two", contractor: "two", operating center: "two"
	And a markout type "one" exists with description: "Foo"
	And a markout type "two" exists with description: "C TO Curbage"
	And I am logged in as "user2@site.com", password: "testpassword#1"
	And I am at the show page for planning work order with valve: "one"
	When I click the "Markouts" tab
	And I follow "Add New Markout"
	And I wait for element MarkoutType to exist
	Then I should see the ReadyDate field
	And I should see the ExpirationDate field
	When I enter "notninety" into the MarkoutNumber field
	And I select markout type "two"'s Description from the MarkoutType dropdown
	And I enter "9/29/2011" into the DateOfRequest field
	And I enter "9/30/2011" into the ReadyDate field
	And I enter "10/1/2011" into the ExpirationDate field
	And I press "Save Markout"
	And I wait for the dialog to close
	Then I should see "notninety" in the table markoutsTable's "Markout Number" column
	And I should see markout type "two"'s Description in the table markoutsTable's "Markout Type" column
	And I should see "9/29/2011" in the table markoutsTable's "Date Of Request" column
	And I should see "9/30/2011" in the table markoutsTable's "Ready Date" column
	And I should see "10/1/2011" in the table markoutsTable's "Expiration Date" column

Scenario: can delete a markout for a work order
	Given a planning work order with valve "one" exists with valve: "one", contractor: "one"
	And a markout "one" exists with markout number: "919123456", expiration date: "yesterday", planning work order with valve: "one", date of request: "yesterday"
	And a markout type "one" exists with description: "Foo"
	And a crew "one" exists with description: "one", contractor: "one", availability: "8"
    And a crew assignment "ca" exists with planning work order with valve: "one", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for planning work order with valve: "one"
	When I click the "Markouts" tab
	And I click ok in the dialog after following "Delete"
	Then I should not see "919123456" in the table markoutsTable's "Markout Number" column
	And I should not see "Error"

Scenario: can cancel deleting a markout for a work order
	Given a planning work order with valve "one" exists with valve: "one", contractor: "one"
	And a markout "one" exists with markout number: "919123456", expiration date: "yesterday", planning work order with valve: "one", date of request: "yesterday"
	And a markout type "one" exists with description: "Foo"
	And a crew "one" exists with description: "one", contractor: "one", availability: "8"
    And a crew assignment "ca" exists with planning work order with valve: "one", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for planning work order with valve: "one"
	When I click the "Markouts" tab
	And I click cancel in the dialog after following "Delete"
	Then I should see "919123456" in the table markoutsTable's "Markout Number" column
	And I should not see "Error"

# Street Opening Permits
Scenario: admin can see street permits for a work order
	Given a planning work order with valve "one" exists with valve: "one", contractor: "one", permit required: "true"
	And a street opening permit "permit" exists with street opening permit number: "27", expiration date: "tomorrow", date issued: "yesterday", planning work order with valve: "one", notes: "notorious"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for planning work order with valve: "one"
	When I click the "Street Opening Permit" tab
	And I wait for element streetOpeningPermitsTable to exist
	Then I should see street opening permit "permit"'s StreetOpeningPermitNumber in the table streetOpeningPermitsTable's "Street Opening Permit Number" column
	And I should see street opening permit "permit"'s DateRequested as a date in the table streetOpeningPermitsTable's "Date Requested" column
	And I should see street opening permit "permit"'s DateIssued as a date in the table streetOpeningPermitsTable's "Date Issued" column
	And I should see street opening permit "permit"'s ExpirationDate as a date in the table streetOpeningPermitsTable's "Expiration Date" column
	And I should see street opening permit "permit"'s Notes in the table streetOpeningPermitsTable's "Notes" column

Scenario: admin should not see street permits for a different work order
	Given a planning work order with valve "one" exists with valve: "one", contractor: "one", permit required: "true"
	And a planning work order with valve "uhoh" exists with valve: "one", contractor: "one", permit required: "true"
	And a street opening permit "uhohpermit" exists with street opening permit number: "27", expiration date: "tomorrow", date issued: "yesterday", planning work order with valve: "uhoh", notes: "notorious"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for planning work order with valve: "one"
	When I click the "Street Opening Permit" tab
	And I wait for element streetOpeningPermitsTable to exist
	Then I should not see street opening permit "uhohpermit"'s StreetOpeningPermitNumber in the table streetOpeningPermitsTable's "Street Opening Permit Number" column
	And I should not see street opening permit "uhohpermit"'s DateRequested as a date in the table streetOpeningPermitsTable's "Date Requested" column
	And I should not see street opening permit "uhohpermit"'s DateIssued as a date in the table streetOpeningPermitsTable's "Date Issued" column
	And I should not see street opening permit "uhohpermit"'s ExpirationDate as a date in the table streetOpeningPermitsTable's "Expiration Date" column
	And I should not see street opening permit "uhohpermit"'s Notes in the table streetOpeningPermitsTable's "Notes" column
	
# Main Breaks
Scenario: can not see main breaks for non-main break order
	Given a planning work order with valve "one" exists with valve: "one", contractor: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	When I visit the show page for planning work order with valve: "one"
	Then I should not see "Main Break"

@no_data_reload
Scenario: can view main breaks for work order
	Given a planning work order with main "one" exists with contractor: "one", WorkDescription: 80
	And a main break "one" exists with planning work order with main: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for planning work order with main: "one"
	When I click the "Main Break" tab
	Then I should see "Main Break"
	And I should see main break "one"'s MainBreakMaterial in the table mainBreaksTable's "Material" column

Scenario: can edit an existing main break
	Given a planning work order with main "one" exists with contractor: "one", WorkDescription: 80
	And a main break "one" exists with planning work order with main: "one"
	And a main break material "foo" exists with description: "bar1"
	And a main condition "foo" exists with description: "bar2"
	And a main failure type "foo" exists with description: "bar3"
	And a main soil condition "foo" exists with description: "bar4"
	And a main disinfection method "foo" exists with description: "bar5"
	And a main flush method "foo" exists with description: "bar6"
	And a service size "foo" exists with service size description: "bar7"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for planning work order with main: "one"
	When I click the "Main Break" tab
	And I follow "Edit"
	And I wait for element MainBreakMaterial to exist
	And I select main break material "foo"'s Description from the MainBreakMaterial dropdown
	And I select main condition "foo"'s Description from the MainCondition dropdown
	And I select main failure type "foo"'s Description from the MainFailureType dropdown
	And I select main soil condition "foo"'s Description from the MainBreakSoilCondition dropdown
	And I select main disinfection method "foo"'s Description from the MainBreakDisinfectionMethod dropdown
	And I select main flush method "foo"'s Description from the MainBreakFlushMethod dropdown
	And I select service size "foo"'s ServiceSizeDescription from the ServiceSize dropdown
	And I enter "2" into the Depth field
	And I enter "3" into the CustomersAffected field
	And I enter "4" into the ShutdownTime field
	And I enter "1.2" into the ChlorineResidual field
	And I press "Update Main Break"
	And I wait for ajax to finish loading
	Then I should see main break material "foo"'s Description in the table mainBreaksTable's "Material" column
	And I should see main condition "foo"'s Description in the table mainBreaksTable's "Main Condition" column
	And I should see main failure type "foo"'s Description in the table mainBreaksTable's "Failure Type" column
	And I should see main soil condition "foo"'s Description in the table mainBreaksTable's "Soil Condition" column
	And I should see main disinfection method "foo"'s Description in the table mainBreaksTable's "Disinfection Method" column
	And I should see main flush method "foo"'s Description in the table mainBreaksTable's "Flush Method" column
	And I should see service size "foo"'s ServiceSizeDescription in the table mainBreaksTable's "Size" column
	And I should see "2" in the table mainBreaksTable's "Depth(in.)" column
	And I should see "3" in the table mainBreaksTable's "Customers Affected" column
	And I should see "4" in the table mainBreaksTable's "Shut Down Time(Hrs)" column
	And I should see "1.2" in the table mainBreaksTable's "Chlorine Residual" column

Scenario: create a new main break
	Given a planning work order with main "one" exists with contractor: "one", WorkDescription: 80
	#And a main break "one" exists with planning work order with main: "one"
	And a main break material "foo" exists with description: "bar1"
	And a main condition "foo" exists with description: "bar2"
	And a main failure type "foo" exists with description: "bar3"
	And a main soil condition "foo" exists with description: "bar4"
	And a main disinfection method "foo" exists with description: "bar5"
	And a main flush method "foo" exists with description: "bar6"
	And a service size "foo" exists with service size description: "bar7"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for planning work order with main: "one"
	When I click the "Main Break" tab
	And I follow "Add New Main Break"
	And I wait for element MainBreakMaterial to exist
	And I select main break material "foo"'s Description from the MainBreakMaterial dropdown
	And I select main condition "foo"'s Description from the MainCondition dropdown
	And I select main failure type "foo"'s Description from the MainFailureType dropdown
	And I select main soil condition "foo"'s Description from the MainBreakSoilCondition dropdown
	And I select main disinfection method "foo"'s Description from the MainBreakDisinfectionMethod dropdown
	And I select main flush method "foo"'s Description from the MainBreakFlushMethod dropdown
	And I select service size "foo"'s ServiceSizeDescription from the ServiceSize dropdown
	And I enter "2" into the Depth field
	And I enter "3" into the CustomersAffected field
	And I enter "4" into the ShutdownTime field
	And I enter "1.2" into the ChlorineResidual field
	And I check the BoilAlertIssued field
	And I press "Save Main Break"
	And I wait for the dialog to close
	Then I should see main break material "foo"'s Description in the table mainBreaksTable's "Material" column
	And I should see main condition "foo"'s Description in the table mainBreaksTable's "Main Condition" column
	And I should see main failure type "foo"'s Description in the table mainBreaksTable's "Failure Type" column
	And I should see main soil condition "foo"'s Description in the table mainBreaksTable's "Soil Condition" column
	And I should see main disinfection method "foo"'s Description in the table mainBreaksTable's "Disinfection Method" column
	And I should see main flush method "foo"'s Description in the table mainBreaksTable's "Flush Method" column
	And I should see service size "foo"'s ServiceSizeDescription in the table mainBreaksTable's "Size" column
	And I should see "2" in the table mainBreaksTable's "Depth(in.)" column
	And I should see "3" in the table mainBreaksTable's "Customers Affected" column
	And I should see "4" in the table mainBreaksTable's "Shut Down Time(Hrs)" column
	And I should see "1.2" in the table mainBreaksTable's "Chlorine Residual" column
	And the BoilAlertIssued field should be checked
	#And I should see "true" in the table mainBreaksTable's "Boil Alert Issued" column
	
Scenario: can delete a main break for a work order
	Given a planning work order with main "one" exists with contractor: "one", WorkDescription: 80
	And a main break "one" exists with planning work order with main: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for planning work order with main: "one"
	When I click the "Main Break" tab
	And I click ok in the dialog after following "Delete"
	Then I should not see main break "one"'s MainBreakMaterial in the table mainBreaksTable's "Material" column
	And I should not see "error"

@no_data_reload
Scenario: can cancel deleting a main break for a work order
	Given a planning work order with main "one" exists with contractor: "one", WorkDescription: 80
	And a main break "one" exists with planning work order with main: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for planning work order with main: "one"
	When I click the "Main Break" tab
	And I click cancel in the dialog after following "Delete"
	Then I should see main break "one"'s MainBreakMaterial in the table mainBreaksTable's "Material" column
	And I should not see "error"

# Documents
Scenario: can view documents for a work order
	Given a planning work order "one" exists with valve: "one", contractor: "one"
	And a document type "one" exists with name: "DocTypeOne"
	And a document "one" exists with document type: "one"
	And a planning work order document link "one" exists with planning work order: "one", document: "one", data type: "work order", document type: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for planning work order: "one"
	When I click the "Documents" tab
	Then I should see "Documents"
	And I should see document "one"'s FileName in the table documentsTable's "File Name" column

Scenario: can edit an existing document
	Given a planning work order "one" exists with valve: "one", contractor: "one"
	And a document type "one" exists with name: "DocTypeOne"
	And a document type "two" exists with name: "DocTypeTwo"
	And a document "one" exists with document type: "one"
	And a planning work order document link "one" exists with planning work order: "one", document: "one", data type: "work order", document type: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for planning work order: "one"
	When I click the "Documents" tab
	And I follow "Edit"
	And I select document type "two"'s Name from the DocumentType dropdown
	And I press Save
	Then I should see document type "two"'s Name in the table documentsTable's "Document Type" column

Scenario: can create a new document
	Given a planning work order "one" exists with valve: "one", contractor: "one"
	And a document type "one" exists with name: "DocTypeOne"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for planning work order: "one"
	When I click the "Documents" tab
	And I press "New Document"
	And I upload "upload-test-image.png" to the file field
	And I wait for ajax to finish loading
	And I select document type "one"'s Name from the DocumentType dropdown
	And I press Save
	Then I should see document type "one"'s Name in the table documentsTable's "Document Type" column
	And I should see "upload-test-image.png" in the table documentsTable's "File Name" column

Scenario: can delete an existing document
	Given a planning work order "one" exists with valve: "one", contractor: "one"
	And a document type "one" exists with name: "DocTypeOne"
	And a document "one" exists with document type: "one"
	And a planning work order document link "one" exists with planning work order: "one", document: "one", data type: "work order", document type: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for planning work order: "one"
	When I click the "Documents" tab
	And I click ok in the dialog after pressing "Delete"
	Then I should not see document "one"'s FileName in the table documentsTable's "File Name" column

Scenario: can cancel deleting an existing document
	Given a planning work order "one" exists with valve: "one", contractor: "one"
	And a document type "one" exists with name: "DocTypeOne"
	And a document "one" exists with document type: "one"
	And a planning work order document link "one" exists with planning work order: "one", document: "one", data type: "work order", document type: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for planning work order: "one"
	When I click the "Documents" tab
	And I click cancel in the dialog after pressing "Delete"
	Then I should see document "one"'s FileName in the table documentsTable's "File Name" column

# TapImages
Scenario: can see linked tapimages with filename with an extension
	Given a planning work order with service "one" exists with contractor: "one", premise number: "123456789", service number: "12345678"
	And a tap image "one" exists with premise number: "123456789", service number: "12345678", town: "Foo"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for planning work order with service: "one"
	When I click the "Tap Images" tab
	Then I should see tap image: "one"'s PremiseNumber on the page
	And I should see the link "View" 

Scenario: can see linked tapimages with filename without an extension
	Given a planning work order with service "one" exists with contractor: "one", premise number: "123456789", service number: "12345678"
	And a tap image "one" exists with premise number: "123456789", service number: "12345678", town: "Foo", operating center: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for planning work order with service: "one"
	When I click the "Tap Images" tab
	Then I should see tap image: "one"'s PremiseNumber on the page
	And I should see the link "View" 

# ValveImages
Scenario: can see link valve images with filename with an extension
	Given a planning work order with valve "one" exists with valve: "one", contractor: "one"
	#And a valve image "one" exists with folder: "fld", filename: "bar.tif", town: "Foo", valve: "one"
	And a valve image "one" exists with town: "Foo", valve: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for planning work order with valve: "one"
	When I click the "Valve Images" tab
	Then I should see valve image "one"'s CreatedAt as a date in the table valveImagesTable's "Created At" column
	And I should see the link "View" 

Scenario: can see link valve images with filename without an extension
	Given a planning work order with valve "one" exists with valve: "one", contractor: "one"
	And a valve image "one" exists with town: "Foo", valve: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for planning work order with valve: "one"
	When I click the "Valve Images" tab
	Then I should see valve image "one"'s CreatedAt as a date in the table valveImagesTable's "Created At" column
	And I should see the link "View" 

# services
Scenario: Admin can view service in tab if work order has service
	Given a service category "one" exists with description: "Neato"
	And a service "one" exists with service number: "123456", premise number: "9876543", date installed: "4/24/1984", service category: "one", operating center: "one"
	And a planning work order with valve "one" exists with valve: "one", contractor: "one", service: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for planning work order with valve "one"
	When I click the "Service" tab
	And I switch to the serviceFrame frame
	Then I should see a display for PremiseNumber with "9876543"
	And I should see a display for DateInstalled with "4/24/1984"
	And I should see a display for ServiceCategory with service category "one"

Scenario: User can view and edit service details
	Given a service category "one" exists with description: "Neato"
	And a service "one" exists with service number: "123456", premise number: "9876543", date installed: "4/24/1984", service category: "one", operating center: "one"
	And a planning work order with valve "one" exists with valve: "one", contractor: "one", service: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for planning work order with valve "one"
	When I click the "Service" tab
	And I press "serviceEditButton"
    And I switch to the serviceFrame frame
	And I enter "123456" into the RetiredAccountNumber field
    And I follow "Cancel"
	Then I should see a display for PremiseNumber with "9876543"
	
Scenario: Admin should not see service tab if work order does not have a service
	Given a planning work order with valve "one" exists with valve: "one", contractor: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for planning work order with valve "one"
	Then I should not see the "Service" tab