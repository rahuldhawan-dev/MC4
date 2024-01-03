Feature: General Page
	In order to view completed orders and attach documents
	As a user
	I want to be view completed orders and attach documents

Background: admin and non-admin users exist
    Given an operating center "one" exists with opcntr: "NJX", opcntrname: "Shrewsburgh"
	And a town "Foo" exists with shortname: "Foo", operating center: "one"
    And operating center "one" exists in town: "Foo"
	And a town section "Little Foo" exists with name: "Little Foo", town: "Foo"
	And a street "one" exists with name: "Easy St", town: "Foo"
    And a contractor "one" exists with name: "one", operating center: "one"
    And an admin user exists with email: "admin@site.com", password: "testpassword#1", contractor: "one"
    And a user exists with email: "user@site.com", password: "testpassword#1", contractor: "one"
    And a crew "one" exists with description: "one", contractor: "one"

Scenario Outline: users can access the general search
	Given I am logged in as "<email>", password: "testpassword#1"
    When I follow "General Search"
	Then I should be at the WorkOrderGeneral/Search page
    And I should see "Work Order - General - Search"
    And I should see "Work Order Number"
    And I should see "Town"
    And I should see "Town Section"
    And I should see "Street"
    And I should see "Street Number"
    And I should see "Nearest Cross Street"
    And I should see "Asset Type"
    And I should see "Work Description"
    Examples:
    | email             |
    | user@site.com     |
    | admin@site.com    |

Scenario: search cascades street off town
	Given I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderGeneral/Search page
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
	And I am at the WorkOrderGeneral/Search page
	When I select asset type "hydrant"'s Description from the AssetType dropdown
	And I wait for element WorkDescription to exist
	Then I should see hydrant work description "one"'s Description in the WorkDescription dropdown
	And I should not see valve work description "two"'s Description in the WorkDescription dropdown
	When I select asset type "valve"'s Description from the AssetType dropdown
	And I wait for element WorkDescription to exist
	Then I should see valve work description "two"'s Description in the WorkDescription dropdown
	And I should not see hydrant work description "one"'s Description in the WorkDescription dropdown

Scenario Outline: users can access a work order after searching
	Given a general work order "one" exists with valve: "one", contractor: "one"
	And a markout "one" exists with expiration date: "yesterday", general work order: "one"
	And I am logged in as "<email>", password: "testpassword#1"
	And I am at the WorkOrderGeneral/Search page
	When I enter general work order "one"'s Id into the Id field
	And I press Search
	And I wait for the page to reload
	And I follow the show link for general work order "one"
	And I wait for the page to reload
	Then I should be at the Show page for general work order: "one"
	And I should see "Order Number: " 
Examples:
    | email             |
    | user@site.com     |
    | admin@site.com    |

# Searches
Scenario: admin searches for an order by number
	Given a work order "one" exists with valve: "one", contractor: "one"
	And a work order "two" exists with valve: "one", contractor: "one"
	And a markout "one" exists with expiration date: "yesterday", work order: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderGeneral/Search page
	When I enter work order "one"'s Id into the Id field
	And I press Search
	Then I should see work order: "one"'s Id on the page
	And I should see "Markout Expiration Date"
	And I should see "Description of Job"
	And I should see markout "one"'s ExpirationDate as a date without seconds in the "Markout Expiration Date" column
	
Scenario: user can see print links in general search results
	Given a read only work order "one" exists with valve: "one", contractor: "one"
	And a markout "one" exists with expiration date: "yesterday", read only work order: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderGeneral/Search page
	When I enter read only work order "one"'s Id into the Id field
	And I press Search
	Then I should see read only work order: "one"'s Id on the page
	Then I should see a link to the show page for read only work order: "one"

Scenario: search returns correct work orders when searching by operating center
	Given a planning work order with valve "one" exists with valve: "one", operating center: "one", contractor: "one"
	And an operating center "two" exists with opcntr: "NJY", opcntrname: "Lakebury"
	And a town "Bar" exists with shortname: "Bar", operating center: "two"
	And a planning work order with main "one" exists with operating center: "two", town: "Bar", contractor: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderGeneral/Search page
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
	And I am at the WorkOrderGeneral/Search page
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
	And I am at the WorkOrderGeneral/Search page
	When I select operating center "one"'s Name from the OperatingCenter dropdown
	And I wait for element Town to exist
	And I select town "Foo"'s ShortName from the Town dropdown
	And I wait for element TownSection to exist
	And I select town section "Little Foo"'s Name from the TownSection dropdown
	And I press Search
	Then I should see planning work order with valve "one"'s Id in the "Order #" column
	And I should not see planning work order with main "one"'s Id in the "Order #" column	

Scenario: search returns correct work orders when searching by street
	Given a planning work order with valve "one" exists with valve: "one", operating center: "one", contractor: "one", town: "Foo", town section: "Little Foo", street: "one"
	And an operating center "two" exists with opcntr: "NJY", opcntrname: "Lakebury"
	And a town "Bar" exists with shortname: "Bar", operating center: "two"
	And a planning work order with main "one" exists with operating center: "two", town: "Bar", contractor: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderGeneral/Search page
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
	And I am at the WorkOrderGeneral/Search page
	When I enter planning work order with valve "one"'s StreetNumber into the StreetNumber field
	And I press Search
	Then I should see planning work order with valve "one"'s Id in the "Order #" column 
	And I should not see planning work order with main "one"'s Id in the "Order #" column	

Scenario: search returns correct work orders when searching by nearest cross street
	Given a street "two" exists with name: "Shakedown St", town: "Foo"
	And a planning work order with valve "one" exists with valve: "one", operating center: "one", contractor: "one", town: "Foo", town section: "Little Foo", nearest cross street: "one"
	And a planning work order with main "one" exists with operating center: "one", contractor: "one", town: "Foo", town section: "Little Foo", nearest cross street: "two"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderGeneral/Search page
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
	And I am at the WorkOrderGeneral/Search page
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
	And I am at the WorkOrderGeneral/Search page
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
	And I am at the WorkOrderGeneral/Search page
	When I select work order priority "one"'s Description from the Priority dropdown
	And I press Search
	Then I should see planning work order with valve "one"'s Id in the "Order #" column
	And I should not see planning work order with main "one"'s Id in the "Order #" column	

Scenario: search returns correct work orders when searching by date received
	Given a planning work order with valve "one" exists with valve: "one", operating center: "one", contractor: "one", date received: "01/02/2012"
	And a planning work order with main "one" exists with operating center: "one", town: "Foo", contractor: "one", date received: "01/01/2012"
	And an asset type "valve" exists with description: "valve", operating center: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderGeneral/Search page
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
	And I am at the WorkOrderGeneral/Search page
	When I select "Local Government" from the Requester dropdown
	And I press Search
	Then I should see planning work order with valve "one"'s Id in the "Order #" column
	And I should not see planning work order with valve "two"'s Id in the "Order #" column	

Scenario: search returns correct work orders when searching by markout requirement
	Given a planning work order with valve "one" exists with valve: "one", operating center: "one", contractor: "one", markout requirement: "emergency"
	And a planning work order with valve "two" exists with operating center: "one", town: "Foo", contractor: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderGeneral/Search page
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
	And I am at the WorkOrderGeneral/Search page
	When I select work order purpose "one"'s Description from the Purpose dropdown
	And I press Search
	Then I should see planning work order with valve "one"'s Id in the "Order #" column
	And I should not see planning work order with main "one"'s Id in the "Order #" column	

Scenario: search returns correct work orders when street opening permit required
	Given an asset type "valve" exists with description: "valve", operating center: "one"
	And a planning work order with valve "one" exists with valve: "one", contractor: "one", permit required: "true"
	And a planning work order with valve "two" exists with valve: "one", contractor: "one", permit required: "false"
	And I am logged in as "admin@site.com", password: "testpassword#1" 
	And I am at the WorkOrderGeneral/Search page
	When I select "Required" from the StreetOpeningPermitRequired dropdown
	And I press Search
	Then I should see planning work order with valve "one"'s Id in the "Order #" column 
	And I should not see planning work order with valve "two"'s Id in the "Order #" column

# paging/sorting
Scenario: user can page and sort by id correctly
	Given an asset type "valve" exists with description: "valve", operating center: "one"
	And a contractor "two" exists with name: "Mann Co.", operating center: "one"
	And a work order "one" exists with valve: "one", contractor: "two", markout requirement: "none"
	And a planning work order with valve "two" exists with valve: "one", contractor: "one", markout requirement: "none", permit required: "true"
	And a planning work order with valve "three" exists with valve: "one", contractor: "one", markout requirement: "none", permit required: "true"
	And I am logged in as "admin@site.com", password: "testpassword#1" 
	And I am at the WorkOrderGeneral/Search page
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

# Documents
Scenario: can view documents for a work order
	Given a work order "one" exists with valve: "one", contractor: "one"
	And a document type "one" exists with name: "DocTypeOne"
	And a document "one" exists with work order: "one", document type: "one"
	And a data type "work order" exists with table name: "WorkOrders", name: "Work Order"
	And a work order document link "one" exists with work order: "one", document: "one", data type: "work order", document type: "one"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the WorkOrderGeneral/Show page for work order: "one"
	When I click the "Documents" tab
	Then I should see "Documents"
	And I should see document "one"'s FileName in the table documentsTable's "File Name" column

Scenario: can edit an existing document
	Given a work order "one" exists with valve: "one", contractor: "one"
	And a data type "work order" exists with table name: "WorkOrders", name: "Work Order"
	And a document type "one" exists with name: "DocTypeOne"
	And a document type "two" exists with name: "DocTypeTwo"
	And a document "one" exists with document type: "one"
	And a work order document link "one" exists with work order: "one", document: "one", data type: "work order", document type: "one"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the WorkOrderGeneral/Show page for work order: "one"
	When I click the "Documents" tab
	And I follow "Edit"
	And I select document type "two"'s Name from the DocumentType dropdown
	And I press Save
	And I wait 2 seconds 
	Then I should see document type "two"'s Name in the table documentsTable's "Document Type" column

Scenario: can add a document
	Given a work order "one" exists with valve: "one", contractor: "one"
	And a document type "one" exists with name: "DocTypeOne"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the WorkOrderGeneral/Show page for work order: "one"
	When I click the "Documents" tab
	And I press "New Document"
	And I wait for element FileName to exist
	And I enter "bar.txt" into the FileName field
	And I select document type "one"'s Name from the DocumentType dropdown
	And I upload "upload-test-image.png"
	And I press Save
	And I wait for ajax to finish loading
	# This keeps failing on TC for some reason
	#And I wait 5 seconds 
	Then I should see document type "one"'s Name in the table documentsTable's "Document Type" column
	And I should see "upload-test-image.png" in the table documentsTable's "File Name" column

#completed order

Scenario: should see completed orders as completed
	Given a work order "one" exists with valve: "one", operating center: "one", contractor: "one", town: "Foo", date completed: "today"
	And an operating center "two" exists with opcntr: "NJY", opcntrname: "Lakebury"
	And a town "Bar" exists with shortname: "Bar", operating center: "two"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderGeneral/Search page
	When I select operating center "one"'s Name from the OperatingCenter dropdown
	And I wait for element Town to exist
	And I select town "Foo"'s ShortName from the Town dropdown
	And I press Search
	Then I should see work order "one"'s Id in the "Order #" column
	And I should see a row for work order "one" in the table generalOrders with the css class "completed"

Scenario: should not see incomplete orders as completed
	Given a work order "one" exists with valve: "one", operating center: "one", contractor: "one", town: "Foo"
	And an operating center "two" exists with opcntr: "NJY", opcntrname: "Lakebury"
	And a town "Bar" exists with shortname: "Bar", operating center: "two"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderGeneral/Search page
	When I select operating center "one"'s Name from the OperatingCenter dropdown
	And I wait for element Town to exist
	And I select town "Foo"'s ShortName from the Town dropdown
	And I press Search
	Then I should see work order "one"'s Id in the "Order #" column
	And I should not see a row for work order "one" in the table generalOrders with the css class "completed"

# requisitions
Scenario: Users can view requisitions
	Given a work order "one" exists with valve: "one", contractor: "one"
	And a markout "one" exists with expiration date: "yesterday", work order: "one"
	And a requisition "one" exists with work order: "one", sap requisition number: "what is this"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the WorkOrderGeneral/Show page for work order: "one"
	When I click the "Purchase Order (PO)" tab
	Then I should see "SAP Purchase Order(PO) #"
	And I should see "what is this"

# services
Scenario: User can view service in tab if work order has service
	Given a service category "one" exists with description: "Neato"
	And a service "one" exists with service number: "123456", premise number: "9876543", date installed: "4/24/1984", service category: "one", operating center: "one"
	And a work order "one" exists with valve: "one", contractor: "one", service: "one", operating center: "one"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the WorkOrderGeneral/Show page for work order: "one"
	When I click the "Service" tab
	And I switch to the serviceFrame frame
	Then I should see a display for PremiseNumber with "9876543"
	And I should see a display for DateInstalled with "4/24/1984"
	And I should see a display for ServiceCategory with service category "one"

Scenario: User can view service in tab if work order has service with service number null
	Given a service category "one" exists with description: "Neato"
	And a town "nj7burg" exists with name: "TOWN"
	And a street prefix "north" exists with description: "N"
    And a street suffix "st" exists with description: "St"
    And a street "two" exists with town: "nj7burg", full st name: "EASY STREET", is active: true, name: "Easy", prefix: "north", suffix: "st"
	#And an asset type "sewer opening" exists with description: "sewer opening"
	And a sewer opening "opening" exists with operating center: "one", town: "nj7burg", street: "two", opening number: "MAD-42"
	And a service "one" exists with premise number: "9876543", date installed: "4/24/1984", service category: "one", operating center: "one"
	And a work order "one" exists with valve: "one", contractor: "one", service: "one", sewer opening: "opening", operating center: "one"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the WorkOrderGeneral/Show page for work order: "one"
	When I click the "Service" tab
	And I switch to the serviceFrame frame
	Then I should see a display for PremiseNumber with "9876543"
	
Scenario: User should not see service tab if work order does not have a service
	Given a work order "one" exists with valve: "one", contractor: "one"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the WorkOrderGeneral/Show page for work order: "one"
	Then I should not see the "Service" tab

Scenario: User can view and edit service details
	Given a service category "one" exists with description: "Neato"
	And a town "nj7burg" exists with name: "TOWN"
	And a street prefix "north" exists with description: "N"
    And a street suffix "st" exists with description: "St"
    And a street "two" exists with town: "nj7burg", full st name: "EASY STREET", is active: true, name: "Easy", prefix: "north", suffix: "st"
	And a sewer opening "opening" exists with operating center: "one", town: "nj7burg", street: "two", opening number: "MAD-42"
	And a service "one" exists with premise number: "9876543", date installed: "4/24/1984", service category: "one", operating center: "one"
	And a work order "one" exists with valve: "one", contractor: "one", service: "one", sewer opening: "opening", operating center: "one"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the WorkOrderGeneral/Show page for work order: "one"
	When I click the "Service" tab
	And I press "serviceEditButton"
    And I switch to the serviceFrame frame
	And I enter "123456" into the RetiredAccountNumber field
    And I follow "Cancel"
    Then I should see a display for PremiseNumber with "9876543"

Scenario: can view a work order
	Given a work order "one" exists with valve: "one", contractor: "one"	
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the WorkOrderGeneral/Show page for work order: "one"
	Then I should see "123456789" in the tdAccountCharged element

# Materials
Scenario: user can view but not enter material for an order
    Given a work order "one" exists with contractor: "one", operating center: "one"
    And a crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "today", assigned on: "today"
    And an operating center "extra" exists with opcntr: "NJX", opcntrname: "Extrasburgh"
    And a material "one" exists with part number: "8675309", description: "something", operating center: "one"
    And a material "two" exists with part number: "8675310", description: "some other thing", operating center: "one"
    And a material "extra" exists with part number: "8675311", description: "some third thing", operating center: "extra"
    And a stock location "one" exists with description: "Some Place", operating center: "one"
    And a stock location "two" exists with description: "Some Other Place", operating center: "one"
    And a stock location "extra" exists with description: "Some Third Place", operating center: "extra"
    And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the WorkOrderGeneral/Show page for work order "one"
    When I click the "Materials Used" tab
	Then I should not see the add-material-used-link element
    
# Spoils
Scenario: user can view spoils but not add or edit them
	Given a work order "one" exists with valve: "one", contractor: "one", operating center: "one"
	And a spoil storage location "one" exists with operating center: "one", name: "location name"
	And a spoil "one" exists with work order: "one", spoil storage location: "one", quantity: "24.84"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the WorkOrderGeneral/Show page for work order "one"
	When I click the "Spoils" tab
    Then I should see spoil "one"'s Quantity in the table spoilsTable's "Quantity (CY)" column
    And I should see spoil storage location "one"'s Name in the table spoilsTable's "Spoil Storage Location" column
	And I should not see the add-new-spoil-link element
	And I should not see a link to the edit page for spoil "one"
	And I should not see a link to the destroy page for spoil "one"

# Markouts
Scenario: user cannot see markout tab if markout not required
	Given a work order "one" exists with contractor: "one", operating center: "one", markout requirement: "none", permit required: "true"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderGeneral/Show page for work order "one"
	When I wait 2 seconds
	Then I should not see the markoutsTab element
	And I should not see the markoutsTabLink element

Scenario: user can only view markouts 
	Given a work order "one" exists with valve: "one", contractor: "one"
	And a crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "today", assigned on: "yesterday", date started: "today"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderGeneral/Show page for work order "one"
	When I click the "Markouts" tab
	Then I should not see "Edit"
	And I should not see "Delete"
	And I should not see "Add New Markout"

# Restorations
Scenario: user can only view a restoration
	Given a work order "one" exists with valve: "one", contractor: "one"
	And a restoration type "one" exists with description: "ASPHALT-STREET"
	And a restoration "one" exists with work order: "one", paving square footage: "52.00", restoration type: "one", assigned contractor: "one"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the WorkOrderGeneral/Show page for work order "one"
	When I click the "Restorations" tab
	Then I should see "Restorations"
	And I should see restoration "one"'s PavingSquareFootage as a decimal rounded to two places in the table restorationsTable's "Paving Square Footage" column
	And I should not see "Create Restoration"
	And I should not see a link to the edit page for restoration "one"
	And I should not see a link to the destroy page for restoration "one"

# Additional Info
Scenario: user can only view the additional tab but not enter anything
    Given a work order "one" exists with contractor: "one", operating center: "one"
    And a closed crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "today", assigned on: "today"
	# Need to explicitly set the id on work descriptions because otherwise it comes up as 0
    And a valve work description "two" exists with description: "Valve Box Blowoff", time to complete: "2", id: "1242"
    And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the WorkOrderGeneral/Show page for work order "one"
    When I click the "Additional" tab
	Then I should see a display for LostWater with work order "one"'s LostWater
	And I should see a display for WorkDescription with work order "one"'s WorkDescription
	And I should see a display for DistanceFromCrossStreet with work order "one"'s DistanceFromCrossStreet

Scenario: User can view special instruction on work order show page
	Given a work order "one" exists with valve: "one", contractor: "one"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the WorkOrderGeneral/Show page for work order: "one"
	Then I should only see work order "one"'s SpecialInstructions in the WorkOrderSpecialInstructions element