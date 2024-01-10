Feature: Finalization Page
    In order to 
	As a user
	I want to be

Background: admin and regular users exist
    Given an operating center "one" exists with opcntr: "NJ7", opcntrname: "Shrewsburgh"
	And a town "Foo" exists with shortname: "Foo", operating center: "one"
    And operating center "one" exists in town: "Foo"
	And a town section "Little Foo" exists with name: "Little Foo", town: "Foo"
	And a street "one" exists with name: "Easy St", town: "Foo"
	And a contractor "one" exists with name: "one", operating center: "one"
    And an admin user exists with email: "admin@site.com", password: "testpassword#1", contractor: "one"
    And a user exists with email: "user@site.com", password: "testpassword#1", contractor: "one"
    And a crew "one" exists with description: "one", contractor: "one"
    # there needs to be 7 of these
    And a response priority exists with foo: "bar"
    And a response priority exists with foo: "bar"
    And a response priority exists with foo: "bar"
    And a response priority exists with foo: "bar"
    And a response priority exists with foo: "bar"
    And a response priority exists with foo: "bar"
    And a response priority exists with foo: "bar"
	And a data type "work order" exists with table name: "WorkOrders", name: "Work Order"
	And an meter location "one" exists with description: "one", code: "c1"
    And an meter location "two" exists with description: "Unknown", code: "c2"

Scenario: user access the finalization search
    Given I am logged in as "user@site.com", password: "testpassword#1"
    And I am at the WorkOrderFinalization/Search page
    Then I should see "Work Order - Finalization - Search"
    And I should see "Work Order Number"
    And I should see "Town"
    And I should see "Town Section"
    And I should see "Street"
    And I should see "Street Number"
    And I should see "Nearest Cross Street"
    And I should see "Asset Type"
    And I should see "Work Description"

@no_data_reload
Scenario: user searches for an order by number, order has crew assignment
    Given a finalization work order "one" exists with contractor: "one", operating center: "one"
	# assigned for *must* be tomorrow because the finalization work order factory creates an assignment
	# for today by default. This causes a race condition for which crew assignment is most recent.
    And a crew assignment "ca" exists with finalization work order: "one", crew: "one", assigned for: "tomorrow", assigned on: "today"
    And I am logged in as "user@site.com", password: "testpassword#1"
    And I am at the WorkOrderFinalization/Search page
    When I enter finalization work order "one"'s Id into the Id field
    And I press Search
    Then I should see "Work Order - Finalization - Results"
    And I should see a link to the edit page for finalization work order "one"	
    And I should see finalization work order "one"'s Id in the "Order #" column
    And I should see finalization work order "one"'s StreetNumber in the "Street Number" column
    And I should see finalization work order "one"'s Street in the "Street" column
    And I should see finalization work order "one"'s Town in the "Town" column
    And I should see finalization work order "one"'s WorkDescription in the "Description of Job (Hover for Notes)" column		
    And I should see crew "one"'s Description in the "Last Assigned To" column

@no_data_reload
Scenario: user searches for an order by number, order has crew assignment but from another contractor
    Given a work order "one" exists with valve: "one", permit required: "false", contractor: "one", markout requirement: "none"
	And a contractor "two" exists with name: "Black Mesa", operating center: "one"
	And a crew "two" exists with description: "cleanup", contractor: "two"
    And a crew assignment "ca" exists with work order: "one", crew: "two", assigned for: "today", assigned on: "today"
	And I am logged in as "user@site.com", password: "testpassword#1"
    And I am at the WorkOrderFinalization/Search page
    When I enter work order "one"'s Id into the Id field
    And I press Search
    Then I should see "No results matched your query."

Scenario: user successfully finalizes an order
    Given a finalization work order "one" exists with contractor: "one", operating center: "one"
    And a crew assignment "ca" exists with finalization work order: "one", crew: "one", assigned for: "today", assigned on: "today"
    And I am logged in as "user@site.com", password: "testpassword#1"
    And I am at the WorkOrderFinalization/Search page
    And I have entered finalization work order "one"'s Id into the Id field
    And I have pressed Search
    When I follow "Select"
    Then I should see "" in the DateCompleted field
    When I enter today's date into the DateCompleted field
    And I press Submit
    Then I should be at the CrewAssignment/ShowCalendar screen

Scenario: user successfully finalizes a service order
    Given a finalization work order for a service line retire "one" exists with contractor: "one", operating center: "one", meterLocation: one, premise number: "1234567890"
	And a crew assignment "ca" exists with finalization work order for a service line retire: "one", crew: "one", assigned for: "today", assigned on: "today"
    And a service material "one" exists with description: "Copper", is edit enabled: true
    And a service size "one" exists with service size description: "1/2"
    And I am logged in as "user@site.com", password: "testpassword#1"
    And I am at the WorkOrderFinalization/Search page
    And I have entered finalization work order for a service line retire "one"'s Id into the Id field
    And I have pressed Search
    When I follow "Select"
    Then I should see "" in the DateCompleted field
    When I enter today's date into the DateCompleted field
    And I press Submit
	Then I should see the validation message "The MeterLocation field is required."
    When I select "one" from the MeterLocation dropdown
    And I select service material "one" from the CompanyServiceLineMaterial dropdown
    And I select service size "one" from the CompanyServiceLineSize dropdown
    And I press Submit
    Then I should be at the CrewAssignment/ShowCalendar screen

Scenario: user tries to finalize a main break repair order with no main break information
    Given a finalization work order for a main break "one" exists with contractor: "one", operating center: "one"
    And a crew assignment "ca" exists with finalization work order for a main break: "one", crew: "one", assigned for: "today", assigned on: "today"
    And I am logged in as "user@site.com", password: "testpassword#1"
    And I am at the WorkOrderFinalization/Search page
    And I have entered finalization work order for a main break "one"'s Id into the Id field
    And I have pressed Search
    When I follow "Select"
    Then I should see "" in the DateCompleted field
    When I enter today's date into the DateCompleted field
    And I press Submit
    Then I should see the error message "A main break work order cannot be finalized without main break information. Please enter some main break information in the Main Break tab."

Scenario: user tries to finalize a service line renewal 
	Given a finalization work order for a service line renewal "one" exists with contractor: "one", operating center: "one"
	And a service material "one" exists with description: "Copper"
	And a service size "one" exists with service size description: "1/2"
    And a crew assignment "ca" exists with finalization work order for a service line renewal: "one", crew: "one", assigned for: "today", assigned on: "today"
	And a work order flushing notice type "one" exists with description: "foobarbaz"
    And I am logged in as "user@site.com", password: "testpassword#1"
    And I am at the WorkOrderFinalization/Search page
    And I have entered finalization work order for a service line renewal "one"'s Id into the Id field
    And I have pressed Search
    When I follow "Select"
    Then I should see "" in the DateCompleted field
    When I enter today's date into the DateCompleted field
    And I press Submit
	Then I should see the validation message "The DoorNoticeLeftDate field is required."
	When I enter "12/8/1980" into the DoorNoticeLeftDate field
	And I press Submit
	Then I should see the validation message "The CustomerServiceLineSize field is required."
	When I select service size "one" from the CustomerServiceLineSize dropdown
	And I select work order flushing notice type "one" from the FlushingNoticeType dropdown
	And I press Submit
	Then I should see the validation message "The Previous Service Company Material field is required."
	And I should see the validation message "The Previous Service Company Size field is required."
	And I should see the validation message "The CustomerServiceLineMaterial field is required."
	When I select service material "one"'s Description from the CustomerServiceLineMaterial dropdown
	And I select service material "one"'s Description from the PreviousServiceLineMaterial dropdown
	And I select service size "one" from the PreviousServiceLineSize dropdown
	When I press Submit
	Then I should see the validation message "The Service Company Material field is required."
	And I should see the validation message "The Service Company Size field is required."
	When I select service material "one"'s Description from the CompanyServiceLineMaterial dropdown
	And I select service size "one" from the CompanyServiceLineSize dropdown
	And I enter "30" into the InitialServiceLineFlushTime field
	And I select "No" from the HasPitcherFilterBeenProvidedToCustomer dropdown
	When I press Submit
    Then I should see a validation message for MeterLocation with "The MeterLocation field is required."
    When I select "one" from the MeterLocation dropdown
    When I select "Yes" from the IsThisAMultiTenantFacility dropdown
	And I press Submit
	Then I should see a validation message for NumberOfPitcherFiltersDelivered with "The NumberOfPitcherFiltersDelivered field is required."
	And I should see a validation message for DescribeWhichUnits with "The DescribeWhichUnits field is required."
	When I enter "10" into the NumberOfPitcherFiltersDelivered field
	And I enter "See notes for units" into the DescribeWhichUnits field 
	And I press Submit 
	Then I should be at the CrewAssignment/ShowCalendar screen
	When I visit the WorkOrderFinalization/Edit/1 page
	Then work order flushing notice type "one" should be selected in the FlushingNoticeType dropdown
	When I visit the WorkOrderGeneral/Show/1 page
	Then I should see a display for FlushingNoticeType with "foobarbaz"
	And I should see a display for CompanyServiceLineMaterial with "Copper"
	And I should see a display for CompanyServiceLineSize with "1/2"
	And I should see a display for IsThisAMultiTenantFacility with "Yes"
	And I should see a display for NumberOfPitcherFiltersDelivered with "10"
	And I should see a display for DescribeWhichUnits with "See notes for units"

Scenario: user tries to finalize a service line retire 
	Given a finalization work order for a service line retire "one" exists with contractor: "one", operating center: "one"
	And a service material "one" exists with description: "Copper"
	And a service size "one" exists with service size description: "1/2"
	And a crew assignment "ca" exists with finalization work order for a service line retire: "one", crew: "one", assigned for: "today", assigned on: "today"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the WorkOrderFinalization/Search page
	And I have entered finalization work order for a service line retire "one"'s Id into the Id field
	And I have pressed Search
	When I follow "Select"
	Then I should see "" in the DateCompleted field
	When I enter today's date into the DateCompleted field
	And I press Submit
	Then I should see the validation message "The Service Company Material field is required."
	And I should see the validation message "The Service Company Size field is required."
	When I select service material "one"'s Description from the CompanyServiceLineMaterial dropdown
	And I select service size "one" from the CompanyServiceLineSize dropdown    
	When I press Submit	
    Then I should see a validation message for MeterLocation with "The MeterLocation field is required."
    When I select "one" from the MeterLocation dropdown
	When I press Submit
	Then I should be at the CrewAssignment/ShowCalendar screen
	When I visit the WorkOrderGeneral/Show/1 page
	Then I should see a display for CompanyServiceLineMaterial with "Copper"
	And I should see a display for CompanyServiceLineSize with "1/2"

Scenario: search cascades street off town
	Given I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderFinalization/Search page
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
	And I am at the WorkOrderFinalization/Search page
	When I select asset type "hydrant"'s Description from the AssetType dropdown
	And I wait for element WorkDescription to be enabled
	Then I should see hydrant work description "one"'s Description in the WorkDescription dropdown
	And I should not see valve work description "two"'s Description in the WorkDescription dropdown
	When I select asset type "valve"'s Description from the AssetType dropdown
	And I wait for element WorkDescription to be enabled
	Then I should see valve work description "two"'s Description in the WorkDescription dropdown
	And I should not see hydrant work description "one"'s Description in the WorkDescription dropdown

# Search
Scenario: admin can search by operating center
	Given I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderFinalization/Search page
	When I select operating center "one"'s Name from the OperatingCenter dropdown
	And I press Search
	Then I should see "No results matched your query."

Scenario: admin can search by town
	Given I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderFinalization/Search page
	When I select operating center "one"'s Name from the OperatingCenter dropdown
	And I wait for element Town to exist
	And I select town "Foo"'s ShortName from the Town dropdown
	And I press Search
	Then I should see "No results matched your query."

Scenario: admin can search by town section
	Given I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderFinalization/Search page
	When I select operating center "one"'s Name from the OperatingCenter dropdown
	And I wait for element Town to exist
	And I select town "Foo"'s ShortName from the Town dropdown
	And I wait for element TownSection to exist
	And I select town section "Little Foo"'s Name from the TownSection dropdown
	And I press Search
	Then I should see "No results matched your query."

Scenario: admin can search by street
	Given I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderFinalization/Search page
	When I select operating center "one"'s Name from the OperatingCenter dropdown
	And I wait for element Town to exist
	And I select town "Foo"'s ShortName from the Town dropdown
	And I wait for element Street to exist
	And I select street "one"'s FullStName from the Street dropdown
	And I press Search
	Then I should see "No results matched your query."

Scenario: admin can search by street number
	Given I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderFinalization/Search page
	When I enter "123" into the StreetNumber field
	And I press Search
	Then I should see "No results matched your query."

Scenario: admin can search by nearest cross street
	Given I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the WorkOrderFinalization/Search page
	When I select operating center "one"'s Name from the OperatingCenter dropdown
	And I wait for element Town to exist
	And I select town "Foo"'s ShortName from the Town dropdown
	And I wait for element NearestCrossStreet to exist
	And I select street "one"'s FullStName from the NearestCrossStreet dropdown
	And I press Search
	Then I should see "No results matched your query."

Scenario: admin can search by asset type
	Given I am logged in as "admin@site.com", password: "testpassword#1"
	And an asset type "valve" exists with description: "valve", operating center: "one"
	And I am at the WorkOrderFinalization/Search page
	When I select asset type "valve"'s Description from the AssetType dropdown
	And I press Search
	Then I should see "No results matched your query."

Scenario: admin can search by work description
	Given I am logged in as "admin@site.com", password: "testpassword#1"
	And an asset type "valve" exists with description: "valve", operating center: "one"
	And a valve work description "one" exists with time to complete: "1.1234", asset type: "valve", description: "turn valve"
	And I am at the WorkOrderFinalization/Search page
	When I select asset type "valve"'s Description from the AssetType dropdown
	And I wait for element WorkDescription to exist
	And I select valve work description "one"'s Description from the WorkDescription dropdown
	And I press Search
	Then I should see "No results matched your query."

# MaterialUsed
Scenario: user enters material for an order
    Given a finalization work order "one" exists with contractor: "one", operating center: "one"
    And a crew assignment "ca" exists with finalization work order: "one", crew: "one", assigned for: "today", assigned on: "today"
    And an operating center "extra" exists with opcntr: "NJX", opcntrname: "Extrasburgh"
    And a material "one" exists with part number: "8675309", description: "something", operating center: "one"
    And a material "two" exists with part number: "8675310", description: "some other thing", operating center: "one"
    And a material "extra" exists with part number: "8675311", description: "some third thing", operating center: "extra"
    And a stock location "one" exists with description: "Some Place", operating center: "one"
    And a stock location "two" exists with description: "Some Other Place", operating center: "one"
    And a stock location "inactive" exists with description: "Old Place", operating center: "one", is active: "false"
    And a stock location "extra" exists with description: "Some Third Place", operating center: "extra"
    And I am logged in as "user@site.com", password: "testpassword#1"
    And I am at the WorkOrderFinalization/Search page
    And I have entered finalization work order "one"'s Id into the Id field
    And I have pressed Search
    When I follow "Select"
    And I click the "Materials Used" tab
    And I follow "Add"
    And I wait for element Material to exist
    Then I should see "n/a" in the Material dropdown
    And I should see material "one"'s FullDescription in the Material dropdown
    And I should see material "two"'s FullDescription in the Material dropdown
    And I should not see material "extra"'s FullDescription in the Material dropdown
    And I should see stock location "one"'s Description in the StockLocation dropdown
    And I should see stock location "two"'s Description in the StockLocation dropdown
    And I should not see stock location "inactive"'s Description in the StockLocation dropdown
    And I should not see stock location "extra"'s Description in the StockLocation dropdown
    And I should see "" in the Quantity field
    When I press "Save Materials"
    Then I should see the validation message "The Quantity field is required."
    When I select material "one"'s FullDescription from the Material dropdown
    Then I should not see the field NonStockDescription		
    When I select stock location "one"'s Description from the StockLocation dropdown
    And I enter "0" into the Quantity field
    And I press "Save Materials"
    Then I should not see "The StockLocation field is required."
    And I should see the validation message "The field Quantity must be greater than or equal to 1"
    When I enter "1" into the Quantity field
    And I press "Save Materials"
    And I wait for the dialog to close
    Then I should see material "one"'s PartNumber in the table materialsUsedTable's "Part Number" column
    And I should see material "one"'s Description in the table materialsUsedTable's "Description" column
    And I should see stock location "one"'s Description in the table materialsUsedTable's "Stock Location" column
    And I should see "1" in the table materialsUsedTable's "Quantity" column

Scenario: user uses the part number search to add material for an order
    Given a finalization work order "one" exists with contractor: "one", operating center: "one"
    And a crew assignment "ca" exists with finalization work order: "one", crew: "one", assigned for: "today", assigned on: "today"
    And an operating center "extra" exists with opcntr: "NJX", opcntrname: "Extrasburgh"
    And a material "one" exists with part number: "8675309", description: "something", operating center: "one"
    And a material "two" exists with part number: "8675310", description: "some other thing", operating center: "one"
    And a material "extra" exists with part number: "8675311", description: "some third thing", operating center: "extra"
    And a stock location "one" exists with description: "Some Place", operating center: "one"
    And a stock location "two" exists with description: "Some Other Place", operating center: "one"
    And a stock location "extra" exists with description: "Some Third Place", operating center: "extra"
    And I am logged in as "user@site.com", password: "testpassword#1"
    And I am at the WorkOrderFinalization/Search page
    And I have entered finalization work order "one"'s Id into the Id field
    And I have pressed Search
    When I follow "Select"
    And I click the "Materials Used" tab
    And I follow "Add"
    And I wait for element Material to exist
    Then I should see "" in the PartSearch field
    And I should see "No results found." in the PartSearchResults select
    When I enter "867" into the PartSearch field
    And I press DoSearch
    And I wait 1 second
    Then I should see "8675309 - something" in the PartSearchResults select
    And I should see "8675310 - some other thing" in the PartSearchResults select
    And I should not see "8675311 - some third thing" in the PartSearchResults select
    When I select "8675309 - something" from the PartSearchResults dropdown
    Then I should not see the field NonStockDescription		
    When I select stock location "one"'s Description from the StockLocation dropdown
    And I enter "1" into the Quantity field
    And I press "Save Materials"
	And I wait for the dialog to close
    Then I should see material "one"'s PartNumber in the table materialsUsedTable's "Part Number" column
    And I should see material "one"'s Description in the table materialsUsedTable's "Description" column
    And I should see stock location "one"'s Description in the table materialsUsedTable's "Stock Location" column
    And I should see "1" in the table materialsUsedTable's "Quantity" column

Scenario: user adds non-stock material for an order
    Given a finalization work order "one" exists with contractor: "one", operating center: "one"
    And a crew assignment "ca" exists with finalization work order: "one", crew: "one", assigned for: "today", assigned on: "today"
    And an operating center "extra" exists with opcntr: "NJX", opcntrname: "Extrasburgh"
    And a material "one" exists with part number: "8675309", description: "something", operating center: "one"
    And a stock location "one" exists with description: "Some Place", operating center: "one"
    And I am logged in as "user@site.com", password: "testpassword#1"
    And I am at the WorkOrderFinalization/Search page
    And I have entered finalization work order "one"'s Id into the Id field
    And I have pressed Search
    When I follow "Select"
    And I click the "Materials Used" tab
    And I follow "Add"
    And I wait for element Material to exist
    Then I should see "" in the NonStockDescription field
    And the NonStockDescription field should be visible
	When I press "Save Materials"
	Then I should see the validation message "Non-stock materials must have a description."
	When I enter "something" into the NonStockDescription field
	And I press "Save Materials"
	Then I should not see the validation message "Non-stock materials must have a description."
	Then I should not see the validation message "Please choose a part number for stock materials."
	And I should not see the validation message "Please choose a stock location for stock materials."
	When I select stock location "one"'s Description from the StockLocation dropdown
    And I press "Save Materials"
	# This message is "visible" but it's not actually visible to the user. It's a hidden div.
    Then I should see the validation message "Non-stock materials must have a description."
    And I should see the validation message "Please choose a part number for stock materials."
	And I should not see the validation message "Please choose a stock location for stock materials."
    And the NonStockDescription field should not be visible
	# the value in the non stock field should have been erased 
    And I should see "" in the NonStockDescription field
    When I select "n/a" from the StockLocation dropdown
    And I select "n/a" from the Material dropdown
	And I enter "something" into the NonStockDescription field
    And I enter "1" into the Quantity field
    And I press "Save Materials"
    And I wait for the dialog to close
    Then I should see "n/a" in the table materialsUsedTable's "Part Number" column
    And I should see "something" in the table materialsUsedTable's "Description" column
    And I should see "n/a" in the table materialsUsedTable's "Stock Location" column
    And I should see "1" in the table materialsUsedTable's "Quantity" column

Scenario: user edits existing material
    Given a finalization work order "one" exists with contractor: "one", operating center: "one"
    And a crew assignment "ca" exists with finalization work order: "one", crew: "one", assigned for: "today", assigned on: "today"
    And an operating center "extra" exists with opcntr: "NJX", opcntrname: "Extrasburgh"
    And a material "one" exists with part number: "8675309", description: "something", operating center: "one"
    And a material "two" exists with part number: "8675310", description: "some other thing", operating center: "one"
    And a stock location "one" exists with description: "Some Place", operating center: "one"
    And a stock location "two" exists with description: "Some Other Place", operating center: "one"
    And a material used exists with finalization work order: "one", material: "one", stock location: "one", quantity: "2"
    And I am logged in as "user@site.com", password: "testpassword#1"
    And I am at the WorkOrderFinalization/Search page
    And I have entered finalization work order "one"'s Id into the Id field
    And I have pressed Search
    When I follow "Select"
    And I click the "Materials Used" tab
    Then I should see material "one"'s PartNumber in the table materialsUsedTable's "Part Number" column
    And I should see material "one"'s Description in the table materialsUsedTable's "Description" column
    And I should see stock location "one"'s Description in the table materialsUsedTable's "Stock Location" column
    And I should see "2" in the table materialsUsedTable's "Quantity" column
	When I follow "Edit"
    And I wait for element Material to exist
	Then material "one"'s FullDescription should be selected in the Material dropdown
    And stock location "one"'s Description should be selected in the StockLocation dropdown
    And I should see "2" in the Quantity field
    And the NonStockDescription field should not be visible
    When I select "n/a" from the Material dropdown
	And I select "n/a" from the StockLocation dropdown
    Then the NonStockDescription field should be visible
    When I enter "something" into the NonStockDescription field
    And I enter "1" into the Quantity field
    And I press "Save Materials"
    And I wait for the dialog to close
    Then I should see "n/a" in the table materialsUsedTable's "Part Number" column
    And I should see "something" in the table materialsUsedTable's "Description" column
    And I should see "n/a" in the table materialsUsedTable's "Stock Location" column
    And I should see "1" in the table materialsUsedTable's "Quantity" column

Scenario: user deletes an existing material used
	Given a finalization work order "one" exists with contractor: "one", operating center: "one"
    And a crew assignment "ca" exists with finalization work order: "one", crew: "one", assigned for: "today", assigned on: "today"
    And a material "one" exists with part number: "8675309", description: "something", operating center: "one"
    And a stock location "one" exists with description: "Some Place", operating center: "one"
    And a material used "one" exists with finalization work order: "one", material: "one", stock location: "one", quantity: "2"
    And I am logged in as "user@site.com", password: "testpassword#1"
    And I am at the edit page for finalization work order: "one"
	When I click the "Materials Used" tab
	And I click ok in the dialog after following "Delete"
	Then I should not see "Error"
	And I should not see material used "one"'s Quantity in the table materialsUsedTable's "Quantity" column
		
Scenario: user can cancel deleting an existing material used
	Given a finalization work order "one" exists with contractor: "one", operating center: "one"
    And a crew assignment "ca" exists with finalization work order: "one", crew: "one", assigned for: "today", assigned on: "today"
    And a material "one" exists with part number: "8675309", description: "something", operating center: "one"
    And a stock location "one" exists with description: "Some Place", operating center: "one"
    And a material used "one" exists with finalization work order: "one", material: "one", stock location: "one", quantity: "2"
    And I am logged in as "user@site.com", password: "testpassword#1"
    And I am at the edit page for finalization work order: "one"
	When I click the "Materials Used" tab
	And I click cancel in the dialog after following "Delete"
	Then I should not see "Error"
	And I should see material used "one"'s Quantity in the table materialsUsedTable's "Quantity" column

# Documents
Scenario: can view documents for a work order
	Given a finalization work order "one" exists with valve: "one", contractor: "one"
	And a document type "one" exists with name: "DocTypeOne"
	And a document "one" exists with document type: "one"
	And a finalization work order document link "one" exists with finalization work order: "one", document: "one", data type: "work order", document type: "one"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the edit page for finalization work order: "one"
	When I click the "Documents" tab
	Then I should see "Documents"
	And I should see document "one"'s FileName in the table documentsTable's "File Name" column

Scenario: can edit an existing document
	Given a finalization work order "one" exists with valve: "one", contractor: "one"
	And a document type "one" exists with name: "DocTypeOne"
	And a document type "two" exists with name: "DocTypeTwo"
	And a document "one" exists with document type: "one"
	And a finalization work order document link "one" exists with finalization work order: "one", document: "one", data type: "work order", document type: "one"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the edit page for finalization work order: "one"
	When I click the "Documents" tab
	And I follow "Edit"
	And I select document type "two"'s Name from the DocumentType dropdown
	And I press Save
	And I wait for ajax to finish loading
	Then I should see document type "two"'s Name in the table documentsTable's "Document Type" column

Scenario: can create a new document
	Given a finalization work order "one" exists with valve: "one", contractor: "one"
	And a document type "one" exists with name: "DocTypeOne"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the edit page for finalization work order: "one"
	When I click the "Documents" tab
	And I press "New Document"
	And I upload "upload-test-image.png"
	And I select document type "one"'s Name from the DocumentType dropdown
	And I press Save
    And I wait for the page to reload
	Then I should see document type "one"'s Name in the table documentsTable's "Document Type" column
	And I should see "upload-test-image.png" in the table documentsTable's "File Name" column

Scenario: can delete an existing document
	Given a finalization work order "one" exists with valve: "one", contractor: "one"
	And a document type "one" exists with name: "DocTypeOne"
	And a document "one" exists with document type: "one"
	And a finalization work order document link "one" exists with finalization work order: "one", document: "one", data type: "work order", document type: "one"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the edit page for finalization work order: "one"
	When I click the "Documents" tab
	And I click ok in the dialog after pressing "Delete"
	Then I should not see document "one"'s FileName in the table documentsTable's "File Name" column

Scenario: can cancel deleting an existing document
	Given a finalization work order "one" exists with valve: "one", contractor: "one"
	And a document type "one" exists with name: "DocTypeOne"
	And a document "one" exists with finalization work order: "one", document type: "one"
	And a finalization work order document link "one" exists with finalization work order: "one", document: "one", data type: "work order", document type: "one"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the edit page for finalization work order: "one"
	When I click the "Documents" tab
	And I click cancel in the dialog after pressing "Delete"
	Then I should see document "one"'s FileName in the table documentsTable's "File Name" column

# Additional Info
Scenario: user enters additional finalization information
    Given a finalization work order "one" exists with contractor: "one", operating center: "one"
    And a closed crew assignment "ca" exists with finalization work order: "one", crew: "one", assigned for: "today", assigned on: "today"
	# Need to explicitly set the id on work descriptions because otherwise it comes up as 0
    And a valve work description "two" exists with description: "Valve Box Blowoff", time to complete: "2", id: "1242"
    And I am logged in as "user@site.com", password: "testpassword#1"
    And I am at the edit page for finalization work order: "one"
    When I click the "Additional" tab
    And I enter "" into the LostWater field
    Then finalization work order "one"'s WorkDescription should be selected in the WorkDescription dropdown
	And I should see valve work description "two"'s Description in the WorkDescription dropdown
    And I should see "" in the DistanceFromCrossStreet field
    And I should see "" in the LostWater field
    And I should see "" in the AppendNotes field
	When I select valve work description "two"'s Description from the WorkDescription dropdown
    And I enter "20" into the DistanceFromCrossStreet field
    And I enter "10" into the LostWater field
    And I enter "even more notes" into the AppendNotes field
    And I press Update
	And I wait for the page to reload
	And I wait for ajax to finish loading
	And I click the "Additional" tab
	And I wait for ajax to finish loading
	Then valve work description "two"'s Description should be selected in the WorkDescription dropdown
    And I should see "20" in the DistanceFromCrossStreet field
    And I should see "10" in the LostWater field
    And I should see "even more notes"
	And I should not see "Error: Not Found"

Scenario: user enters additional finalization information with no additional notes
    Given a finalization work order "one" exists with contractor: "one", operating center: "one"
    And a closed crew assignment "ca" exists with finalization work order: "one", crew: "one", assigned for: "today", assigned on: "today"
	# Need to explicitly set the id on work descriptions because otherwise it comes up as 0
    And a valve work description "two" exists with description: "some job", time to complete: "2", id: "1242"
    And I am logged in as "user@site.com", password: "testpassword#1"
    And I am at the edit page for finalization work order: "one"
    When I click the "Additional" tab
    And I enter "" into the LostWater field
    Then finalization work order "one"'s WorkDescription should be selected in the WorkDescription dropdown
	And I should see valve work description "two"'s Description in the WorkDescription dropdown
    And I should see "" in the DistanceFromCrossStreet field
    And I should see "" in the LostWater field
    And I should see "" in the AppendNotes field
	When I select valve work description "two"'s Description from the WorkDescription dropdown
    And I enter "20" into the DistanceFromCrossStreet field
    And I enter "10" into the LostWater field
    And I press Update
	And I wait for the page to reload
	And I wait for ajax to finish loading
    And I click the "Additional" tab
	Then valve work description "two"'s Description should be selected in the WorkDescription dropdown
    And I should see "20" in the DistanceFromCrossStreet field
    And I should see "10" in the LostWater field
    # this would have been appended with the append notes if entered
    And I should not see "Some Test Contractor - user@site.com"

Scenario: user tries to update additional finalization information for emergency order without distance from cross street
    Given a finalization work order "one" exists with contractor: "one", operating center: "one", priority: "emergency"
    And a closed crew assignment "ca" exists with finalization work order: "one", crew: "one", assigned for: "today", assigned on: "today"
    And a valve work description "two" exists with description: "some job", time to complete: "2", id: "1242"
    And I am logged in as "user@site.com", password: "testpassword#1"
    And I am at the edit page for finalization work order: "one"
    When I click the "Additional" tab
    And I enter "" into the LostWater field
    Then finalization work order "one"'s WorkDescription should be selected in the WorkDescription dropdown
	And I should see valve work description "two"'s Description in the WorkDescription dropdown
    And I should see "" in the DistanceFromCrossStreet field
    And I should see "" in the LostWater field
    And I should see "" in the AppendNotes field
	When I select valve work description "two"'s Description from the WorkDescription dropdown
    And I enter "10" into the LostWater field
    And I enter "even more notes" into the AppendNotes field
    And I press Update
    Then I should see the validation message "Required when the work order priority is 'Emergency'."
	When I enter "15" into the DistanceFromCrossStreet field
    And I press Update
	And I wait for the page to reload
	And I wait for ajax to finish loading
    And I click the "Additional" tab
	Then valve work description "two"'s Description should be selected in the WorkDescription dropdown
    And I should see "15" in the DistanceFromCrossStreet field
    And I should see "10" in the LostWater field
    And I should see "even more notes"

Scenario: user tries to update additional finalization information for order with street opening permit required without entering distance from cross street
    Given a finalization work order "one" exists with contractor: "one", operating center: "one", permit required: "true"
    And a closed crew assignment "ca" exists with finalization work order: "one", crew: "one", assigned for: "today", assigned on: "today"
    And a valve work description "two" exists with description: "some job", time to complete: "2", id: "1242"
    And I am logged in as "user@site.com", password: "testpassword#1"
    And I am at the edit page for finalization work order: "one"
    When I click the "Additional" tab
    And I enter "" into the LostWater field
    Then finalization work order "one"'s WorkDescription should be selected in the WorkDescription dropdown
	And I should see valve work description "two"'s Description in the WorkDescription dropdown
    And I should see "" in the DistanceFromCrossStreet field
    And I should see "" in the LostWater field
    And I should see "" in the AppendNotes field
	When I select valve work description "two"'s Description from the WorkDescription dropdown
    And I enter "10" into the LostWater field
    And I enter "even more notes" into the AppendNotes field
    And I press Update
    Then I should see the validation message "Required when a street opening permit is required."
	When I enter "15" into the DistanceFromCrossStreet field
    And I press Update
	And I wait for the page to reload
	And I wait for ajax to finish loading
    And I click the "Additional" tab
	Then valve work description "two"'s Description should be selected in the WorkDescription dropdown
    And I should see "15" in the DistanceFromCrossStreet field
    And I should see "10" in the LostWater field
    And I should see "even more notes"

Scenario: user tries to finalize an order with street opening permit required without entering distance from cross street
    Given a finalization work order "one" exists with contractor: "one", operating center: "one", permit required: "true"
    And a closed crew assignment "ca" exists with finalization work order: "one", crew: "one", assigned for: "today", assigned on: "today"
    And a valve work description "two" exists with description: "some job", time to complete: "2"
    And I am logged in as "user@site.com", password: "testpassword#1"
    And I am at the edit page for finalization work order: "one"
    When I enter today's date into the DateCompleted field
    And I press Submit
	Then I should be at the edit page for finalization work order: "one"
    And I should see the error message "This order required a street opening permit, so 'Distance from Cross Street' must be filled in under the 'Additional' tab before it can be finalized."
    When I click the "Additional" tab
	And I enter "15" into the DistanceFromCrossStreet field
    And I press Update
    And I wait for the page to reload
	And I wait for ajax to finish loading
    And I enter today's date into the DateCompleted field
    And I press Submit
    Then I should be at the CrewAssignment/ShowCalendar screen

# Restorations
Scenario: can view a restoration
	Given a finalization work order "one" exists with valve: "one", contractor: "one"
	And a restoration type "one" exists with description: "ASPHALT-STREET"
	And a restoration "one" exists with finalization work order: "one", paving square footage: "52.00", restoration type: "one", assigned contractor: "one"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the edit page for finalization work order: "one"
	When I click the "Restorations" tab
	Then I should see "Restorations"
	And I should see restoration "one"'s PavingSquareFootage as a decimal rounded to two places in the table restorationsTable's "Paving Square Footage" column

# TODO: make this test work
@ignore
Scenario: new restoration toggles footage by restoration type
	Given a finalization work order "one" exists with valve: "one", contractor: "one"
	#And this test does not crash because of the single session thing
	And a restoration type "one" exists with description: "CURB RESTORATION"
	And a restoration type "two" exists with description: "ASPHALT-STREET"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the edit page for finalization work order: "one"
	When I click the "Restorations" tab
	And I follow "Add New Restoration"
	And I wait for element RestorationType to exist
	And I select restoration type "one"'s Description from the RestorationType dropdown
	Then I should not see "(sq. ft.)"
	And I should see "(ft.)"
	When I select restoration type "two"'s Description from the RestorationType dropdown
	Then I should see "(sq. ft.)"
	And I should not see "(ft.)"

#Scenario: can create a new restoration
#	Given a finalization work order "one" exists with valve: "one", contractor: "one"
#	And a restoration type "one" exists with description: "ASPHALT-STREET"
#	And a restoration type "two" exists with description: "blergh"
#	And a restoration type "three" exists with description: "blerghh"
#	And a restoration type "four" exists with description: "CURB RESTORATION"
#	And I am logged in as "user@site.com", password: "testpassword#1"
#	And I am at the edit page for finalization work order: "one"
#	When I click the "Restorations" tab
#	And I follow "Add New Restoration"
#	And I wait for element RestorationType to exist
#	And I select restoration type "four"'s Description from the RestorationType dropdown
#	And I wait for element LinearFeetOfCurb to exist
#	And I press "Save Restoration"
#	Then I should see the validation message "The LinearFeetOfCurb field is required"
#	When I select restoration type "one"'s Description from the RestorationType dropdown
#	And I wait for element PavingSquareFootage to exist
#	And I press "Save Restoration"
#	Then I should see the validation message "The PavingSquareFootage field is required"
#	When I enter "3.00" into the PavingSquareFootage field
#	And I press "Save Restoration"
#	And I wait for the dialog to close
#	Then I should see "3.00" in the table restorationsTable's "Paving Square Footage" column

Scenario: can edit a restoration
	Given a finalization work order "one" exists with valve: "one", contractor: "one"
	And a restoration type "one" exists with description: "ASPHALT-STREET"
	And a restoration "one" exists with finalization work order: "one", restoration type: "one", paving square footage: "52.00", assigned contractor: "one"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the edit page for finalization work order: "one"
	When I click the "Restorations" tab
	And I follow "Edit"
	#And I wait for the dialog to open 
	And I enter "3" into the PartialPavingSquareFootage field
	And I press "Save Restoration"
	And I click the "Base/Temporary Restoration" tab
	Then I should see a display for PartialPavingSquareFootage with "3"
	
#Scenario: can delete a restoration
#	Given a finalization work order "one" exists with valve: "one", contractor: "one"
#	And a restoration type "one" exists with description: "ASPHALT-STREET"
#	And a restoration "one" exists with finalization work order: "one", restoration type: "one", paving square footage: "52.00"
#	And I am logged in as "user@site.com", password: "testpassword#1"
#	And I am at the edit page for finalization work order: "one"
#	When I click the "Restorations" tab
#	And I click ok in the dialog after following "Delete"
#	And I wait for the dialog to close
#	Then I should not see "Error"
#	And I should not see restoration "one"'s PavingSquareFootage in the table restorationsTable's "Paving Square Footage" column
	
#Scenario: can cancel deleting a restoration
#	Given a finalization work order "one" exists with valve: "one", contractor: "one"
#	And a restoration type "one" exists with description: "ASPHALT-STREET"
#	And a restoration "one" exists with finalization work order: "one", restoration type: "one", paving square footage: "52.00"
#	And I am logged in as "user@site.com", password: "testpassword#1"
#	And I am at the edit page for finalization work order: "one"
#	When I click the "Restorations" tab
#	And I click Cancel in the dialog after following "Delete"
#	Then I should not see "Error"
#	And I should see restoration "one"'s PavingSquareFootage as a decimal rounded to two places in the table restorationsTable's "Paving Square Footage" column

# Spoils
Scenario: user can view spoils
	Given a finalization work order "one" exists with valve: "one", contractor: "one", operating center: "one"
	And a spoil storage location "one" exists with operating center: "one", name: "location name"
	And a spoil "one" exists with finalization work order: "one", spoil storage location: "one", quantity: "24.84"
	And I am logged in as "user@site.com", password: "testpassword#1"
    And I am at the edit page for finalization work order: "one"
	When I click the "Spoils" tab
	And I wait for element spoilsTable to exist
    Then I should see spoil "one"'s Quantity in the table spoilsTable's "Quantity (CY)" column
    And I should see spoil storage location "one"'s Name in the table spoilsTable's "Spoil Storage Location" column

Scenario: user can add spoils
	Given a finalization work order "one" exists with valve: "one", contractor: "one", operating center: "one"
	And a spoil storage location "one" exists with operating center: "one", name: "location name"
	And I am logged in as "user@site.com", password: "testpassword#1"
    And I am at the edit page for finalization work order: "one"
	When I click the "Spoils" tab
	And I wait for element spoilsTable to exist
	And I follow "Add New Spoil"
	And I wait for the dialog to open
    And I wait for element Quantity to exist
	And I enter "42.34" into the Quantity field
	And I select spoil storage location "one"'s Name from the SpoilStorageLocation dropdown
	And I press "Save Spoil"
	And I wait for the dialog to close
    Then I should see "42.34" in the table spoilsTable's "Quantity (CY)" column
    And I should see spoil storage location "one"'s Name in the table spoilsTable's "Spoil Storage Location" column

Scenario: user can edit spoils
	Given a finalization work order "one" exists with valve: "one", contractor: "one", operating center: "one"
	And a spoil storage location "one" exists with operating center: "one", name: "location name"
	And a spoil storage location "two" exists with operating center: "one", name: "second location"
	And a spoil "one" exists with finalization work order: "one", spoil storage location: "one", quantity: "24.84"
	And I am logged in as "user@site.com", password: "testpassword#1"
    And I am at the edit page for finalization work order: "one"
	When I click the "Spoils" tab
	And I wait for element spoilsTable to exist
	And I follow the edit link for spoil "one"
	And I wait for the dialog to open
	And I enter "144.31" into the Quantity field
	And I select spoil storage location "two"'s Name from the SpoilStorageLocation dropdown
	And I press "Save Spoil"
	And I wait for the dialog to close
    Then I should not see "42.34" in the table spoilsTable's "Quantity (CY)" column
    And I should not see spoil storage location "one"'s Name in the table spoilsTable's "Spoil Storage Location" column
    And I should see "144.31" in the table spoilsTable's "Quantity (CY)" column
    And I should see spoil storage location "two"'s Name in the table spoilsTable's "Spoil Storage Location" column

Scenario: user can delete spoils
	Given a finalization work order "one" exists with valve: "one", contractor: "one", operating center: "one"
	And a spoil storage location "one" exists with operating center: "one", name: "location name"
	And a spoil "one" exists with finalization work order: "one", spoil storage location: "one", quantity: "24.84"
	And I am logged in as "user@site.com", password: "testpassword#1"
    And I am at the edit page for finalization work order: "one"
	When I click the "Spoils" tab
	And I wait for element spoilsTable to exist
	And I click ok in the dialog after following "Delete"
	Then I should not see spoil "one"'s Quantity in the table spoilsTable's "Quantity (CY)" column
    And I should not see spoil storage location "one"'s Name in the table spoilsTable's "Spoil Storage Location" column

# Crew Assignments
Scenario: can see crew assignments for work order
	Given a valve work description "one" exists with time to complete: "1.1234"
	And a finalization work order "one" exists with contractor: "one", operating center: "one", valve work description: "one"
	And a markout "one" exists with expiration date: "yesterday", finalization work order: "one"
	And a crew "two" exists with description: "one", contractor: "one", availability: "8"
    And a crew assignment "ca" exists with finalization work order: "one", crew: "two", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000", date started: "today", date ended: "tomorrow", employees on job: "13" 
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the edit page for finalization work order: "one"
	When I click the "Crew Assignments" tab
	And I wait for element assignmentsTable to exist
	Then I should see valve work description "one"'s TimeToComplete as a decimal rounded to two places in the table assignmentsTable's "Est. TTC (hours)" column
	And I should see crew "two"'s Description in the table assignmentsTable's "Assigned To" column
	And I should see crew assignment "ca"'s AssignedOn as a date in the table assignmentsTable's "Assigned On" column
	And I should see crew assignment "ca"'s AssignedFor as a date in the table assignmentsTable's "Assigned For" column
	And I should see crew assignment "ca"'s DateStarted as a date without seconds in the table assignmentsTable's "Start Time" column
	And I should see crew assignment "ca"'s DateEnded as a date without seconds in the table assignmentsTable's "End Time" column
	And I should see crew assignment "ca"'s EmployeesOnJob in the table assignmentsTable's "Employees On Crew" column

Scenario: user should not see crew assignments for other contractors
	Given a valve work description "one" exists with time to complete: "1.1234"
	And a contractor "bad contractor" exists with name: "bad contractor", operating center: "one"
	And a finalization work order "one" exists with contractor: "one", operating center: "one", valve work description: "one"
	And a markout "one" exists with expiration date: "yesterday", finalization work order: "one"
	And a crew "two" exists with description: "one", contractor: "one", availability: "8"
	And a crew "bad crew" exists with description: "bad crew", contractor: "bad contractor"
    And a crew assignment "ca" exists with finalization work order: "one", crew: "two", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000", date started: "today", date ended: "tomorrow", employees on job: "13" 
    And a crew assignment "bad ass" exists with finalization work order: "one", crew: "bad crew", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000", date started: "today", date ended: "tomorrow", employees on job: "13" 
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the edit page for finalization work order: "one"
	When I click the "Crew Assignments" tab
	And I wait for element assignmentsTable to exist
	Then I should see valve work description "one"'s TimeToComplete as a decimal rounded to two places in the table assignmentsTable's "Est. TTC (hours)" column
	And I should see crew "two"'s Description in the table assignmentsTable's "Assigned To" column
	And I should see crew assignment "ca"'s AssignedOn as a date in the table assignmentsTable's "Assigned On" column
	And I should see crew assignment "ca"'s AssignedFor as a date in the table assignmentsTable's "Assigned For" column
	And I should see crew assignment "ca"'s DateStarted as a date without seconds in the table assignmentsTable's "Start Time" column
	And I should see crew assignment "ca"'s DateEnded as a date without seconds in the table assignmentsTable's "End Time" column
	And I should see crew assignment "ca"'s EmployeesOnJob in the table assignmentsTable's "Employees On Crew" column
	And I should not see crew "bad crew"'s Description in the table assignmentsTable's "Assigned To" column

Scenario: user ends a crew assignment
	Given a valve work description "one" exists with time to complete: "1.1234"
	And a finalization work order "one" exists with contractor: "one", operating center: "one", valve work description: "one"
	And a markout "one" exists with expiration date: "yesterday", finalization work order: "one"
	And a crew "two" exists with description: "one", contractor: "one", availability: "8"
    And a crew assignment "ca" exists with finalization work order: "one", crew: "two", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000", date started: "today"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the edit page for finalization work order: "one"
	When I click the "Crew Assignments" tab
	And I wait for element EmployeesOnJob to exist
	And I enter "42" into the EmployeesOnJob field 
	And I press the end assignment link for crew assignment: "ca"
	And I wait for element DateCompleted to be enabled
	And I click the "Crew Assignments" tab
	And I wait for element assignmentsTable to exist
	Then I should not see the end assignment link for crew assignment: "ca"
	And I should see "42" in the table assignmentsTable's "Employees On Crew" column
	And I should be at the edit page for finalization work order: "one"

Scenario: user does not enter employees on crew when ending a crew assignment
	Given a valve work description "one" exists with time to complete: "1.1234"
	And a finalization work order "one" exists with contractor: "one", operating center: "one", valve work description: "one"
	And a markout "one" exists with expiration date: "yesterday", finalization work order: "one"
	And a crew "two" exists with description: "one", contractor: "one", availability: "8"
    And a crew assignment "ca" exists with finalization work order: "one", crew: "two", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000", date started: "today"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the edit page for finalization work order: "one"
	When I click the "Crew Assignments" tab
	And I wait for element EmployeesOnJob to exist
	And I press the end assignment link for crew assignment: "ca"
	Then I should see the validation message "The EmployeesOnJob field is required."

Scenario: user tries to finalize an order with an open crew assignment
	Given a finalization work order "one" exists with contractor: "one", operating center: "one"
    And a crew assignment "ca" exists with finalization work order: "one", crew: "one", assigned for: "today", assigned on: "today", date started: "today"
    And I am logged in as "user@site.com", password: "testpassword#1"
    And I am at the WorkOrderFinalization/Search page
    And I have entered finalization work order "one"'s Id into the Id field
    And I have pressed Search
    When I follow "Select"
    Then I should see "" in the DateCompleted field
    When I enter today's date into the DateCompleted field
    And I press Submit
    Then I should see the error message "This order has one or more Crew Assignments that are not closed. Please ensure that all end times are recorded."

# Markouts
Scenario: cannot see markout tab if markout not required
	Given a finalization work order "one" exists with contractor: "one", operating center: "one", markout requirement: "none", permit required: "true"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the edit page for finalization work order: "one"
	When I wait 2 seconds
	Then I should not see the markoutsTab element
	And I should not see the markoutsTabLink element

Scenario: cannot edit markouts if work has been started
	Given a finalization work order "one" exists with valve: "one", contractor: "one"
	And a crew assignment "ca" exists with finalization work order: "one", crew: "one", assigned for: "today", assigned on: "yesterday", date started: "today"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the edit page for finalization work order: "one"
	When I click the "Markouts" tab
	And I wait for ajax to finish loading
	Then I should not see "Edit"
	And I should not see "Delete"

# paging/sorting
Scenario: user can page and sort by id correctly
	Given an asset type "valve" exists with description: "valve", operating center: "one"
	And a work order "one" exists with valve: "one", permit required: "false", contractor: "one", markout requirement: "none"
	And a finalization work order "two" exists with valve: "one", contractor: "one", operating center: "one"
	And a finalization work order "three" exists with valve: "one", contractor: "one", operating center: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1" 
	And I am at the WorkOrderFinalization/Search page
	When I select "Valve" from the AssetType dropdown
	And I press Search
	Then I should see finalization work order "two"'s Id in the "Order #" column
	And I should see finalization work order "three"'s Id in the "Order #" column
	And I should not see work order "one"'s Id in the "Order #" column
	When I follow "1"
	Then I should see finalization work order "two"'s Id in the "Order #" column
	And I should not see finalization work order "three"'s Id in the "Order #" column
	And I should not see work order "one"'s Id in the "Order #" column
	When I follow ">>" 
	Then I should not see finalization work order "two"'s Id in the "Order #" column
	And I should see finalization work order "three"'s Id in the "Order #" column
	And I should not see work order "one"'s Id in the "Order #" column
	When I follow "Order #"
	Then I should not see finalization work order "two"'s Id in the "Order #" column
	And I should see finalization work order "three"'s Id in the "Order #" column
	And I should not see work order "one"'s Id in the "Order #" column
	When I follow "Order #"
	Then I should see finalization work order "two"'s Id in the "Order #" column
	And I should not see finalization work order "three"'s Id in the "Order #" column
	And I should not see work order "one"'s Id in the "Order #" column
	When I follow "All" 
	Then I should see finalization work order "two"'s Id in the "Order #" column
	And I should see finalization work order "three"'s Id in the "Order #" column
	And I should not see work order "one"'s Id in the "Order #" column
	
# services
Scenario: User can view service in tab if work order has service
	Given a service category "one" exists with description: "Neato"
	And a service "one" exists with service number: "123456", premise number: "9876543", date installed: "4/24/1984", service category: "one", operating center: "one"
	And a finalization work order "one" exists with contractor: "one", operating center: "one", markout requirement: "none", permit required: "true", service: "one"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the WorkOrderFinalization/Edit page for finalization work order: "one"
	When I click the "Service" tab
	And I switch to the serviceFrame frame
	Then I should see a display for PremiseNumber with "9876543"
	And I should see a display for DateInstalled with "4/24/1984"
	And I should see a display for ServiceCategory with service category "one"
	
Scenario: User should not see service tab if work order does not have a service
	Given a finalization work order "one" exists with contractor: "one", operating center: "one", markout requirement: "none", permit required: "true"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the WorkOrderFinalization/Edit page for finalization work order: "one"
	Then I should not see the "Service" tab

Scenario: User can view and edit service details
	Given a service category "one" exists with description: "Neato"
	And a service "one" exists with premise number: "9876543", date installed: "4/24/1984", service category: "one", operating center: "one"
	And a finalization work order "one" exists with contractor: "one", operating center: "one", markout requirement: "none", permit required: "true", service: "one"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the WorkOrderFinalization/Edit page for finalization work order: "one"
	When I click the "Service" tab
	When I press "serviceEditButton"
    When I switch to the serviceFrame frame
	And I enter "123456" into the RetiredAccountNumber field
    And I follow "Cancel"
    Then I should see a display for PremiseNumber with "9876543"

Scenario: User can view a meter set in tab if work order has meter sets
    Given an operating center "two" exists with opcntr: "NJ4", opcntrname: "Lakewood", s a p enabled: "true", s a p work orders enabled: "true", is contracted operations: "false"
    And an asset type "service" exists with description: "service", operating center: "two"
    And a work description "sli" exists with description: "service line installation", asset type: "service"
	And a finalization work order "one" exists with contractor: "one", operating center: "two", asset type: "service", markout requirement: "none", permit required: "true", date completed: "11/08/2021", work description: "sli", premise number: "12345678"
	And a meter set "one" exists with finalization work order: "one"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the WorkOrderFinalization/Edit page for finalization work order: "one"
	When I click the "Meter Set" tab
	And I follow "Edit"
	Then I should see a link to the edit page for finalization work order "one"

Scenario: User can view the compliance data section when work description is service line renewal company side
	Given a service category "one" exists with description: "Neato"
	And a service "one" exists with service number: "123456", premise number: "9876543", date installed: "4/24/1984", service category: "one"
	And a finalization work order for a service line renewal company side "one" exists with contractor: "one", operating center: "one", markout requirement: "none", permit required: "true", service: "one"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the WorkOrderFinalization/Edit page for finalization work order for a service line renewal company side: "one"
	When I click the "Initial Information" tab
	Then I should see the InitialServiceLineFlushTime field

Scenario: User does not see the compliance data section when work description is not service line renewal company side
	Given a service category "one" exists with description: "Neato"
	And a service "one" exists with service number: "123456", premise number: "9876543", date installed: "4/24/1984", service category: "one"
	And a finalization work order for a main break "one" exists with contractor: "one", operating center: "one", markout requirement: "none", permit required: "true", service: "one"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the WorkOrderFinalization/Edit page for finalization work order for a main break: "one"
	When I click the "Initial Information" tab
	Then I should not see the InitialServiceLineFlushTime field

Scenario: User can add compliance data values to a finalization work order
	Given a service category "one" exists with description: "Neato"
	And a service "one" exists with service number: "123456", premise number: "9876543", date installed: "4/24/1984", service category: "one"
	And a finalization work order for a service line renewal company side "one" exists with contractor: "one", operating center: "one", markout requirement: "none", permit required: "true"
	And a service material "one" exists with description: "Copper"
	And a pitcher filter delivery method "one" exists with description: "handed to customer"
	And a pitcher filter delivery method "two" exists with description: "left on porch/doorstep"
	And a pitcher filter delivery method "three" exists with description: "other"
	And a service size "one" exists with service size description: "1/2"
	And a crew assignment "ca" exists with finalization work order for a service line renewal company side: "one", crew: "one", assigned for: "today", assigned on: "today"
	And a work order flushing notice type "one" exists with description: "foobarbaz"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the WorkOrderFinalization/Edit page for finalization work order for a service line renewal company side: "one"
	When I click the "Initial Information" tab
	And I select service size "one" from the CustomerServiceLineSize dropdown
	And I select work order flushing notice type "one" from the FlushingNoticeType dropdown
	And I select service material "one"'s Description from the CustomerServiceLineMaterial dropdown
	And I select service material "one"'s Description from the PreviousServiceLineMaterial dropdown
	And I select service size "one"'s Description from the PreviousServiceLineSize dropdown
	And I select service material "one"'s Description from the CompanyServiceLineMaterial dropdown
	And I select service size "one"'s Description from the CompanyServiceLineSize dropdown
	And I enter "11/01/2020" into the DoorNoticeLeftDate field
	# actual text of the button is "Save", but there's more than one (Documents tab also has a "Save" button)
	# the button we want has the id "Submit", so that's what we're doing here
	And I press Submit
	Then I should see the validation message "The InitialServiceLineFlushTime field is required."
	When I enter "29" into the InitialServiceLineFlushTime field
	Then I should see "Below minimum reflush of 30 minutes"
	When I enter "30" into the InitialServiceLineFlushTime field
	And I select "Yes" from the HasPitcherFilterBeenProvidedToCustomer dropdown
	And I enter "" into the DatePitcherFilterDeliveredToCustomer field
	And I select pitcher filter delivery method "two"'s Description from the PitcherFilterCustomerDeliveryMethod dropdown
	And I press Submit
	Then I should see the validation message "The Date Delivered field is required."
	When I enter today's date into the DatePitcherFilterDeliveredToCustomer field
	And I enter today's date into the DateCompleted field
	And I select pitcher filter delivery method "three"'s Description from the PitcherFilterCustomerDeliveryMethod dropdown
	And I press Submit
	Then I should see the validation message "The Explain Other field is required."
	When I enter "my other method reason" into the PitcherFilterCustomerDeliveryOtherMethod field
	And I press Submit
	Then I should see the validation message "The IsThisAMultiTenantFacility field is required."
	When I select "Yes" from the IsThisAMultiTenantFacility dropdown
	And I press Submit
	Then I should see the validation message "The NumberOfPitcherFiltersDelivered field is required."
	And I should see the validation message "The DescribeWhichUnits field is required."
	When I enter "10" into the NumberOfPitcherFiltersDelivered field
	And I enter "See notes for units" into the DescribeWhichUnits field
	And I press Submit
    Then I should see a validation message for MeterLocation with "The MeterLocation field is required."
    When I select "one" from the MeterLocation dropdown
	When I press Submit
	Then I should be at the CrewAssignment/ShowCalendar screen	

Scenario: User can view special instruction on finalized work order edit page
	Given a finalization work order "one" exists with contractor: "one", operating center: "one", markout requirement: "none", permit required: "true"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the WorkOrderFinalization/Edit page for finalization work order: "one"
	Then I should only see finalization work order "one"'s SpecialInstructions in the WorkOrderSpecialInstructions element

Scenario: user sees or doesn't see a validation message for IsThisAMultiTenantFacility depending the value of the haspitcherfilterbeenprovidedtocustomer field
    Given a service category "one" exists with description: "Neato"
	And a service "one" exists with service number: "123456", premise number: "9876543", date installed: "4/24/1984", service category: "one"
	And a finalization work order for a service line renewal company side "one" exists with contractor: "one", operating center: "one", markout requirement: "none", permit required: "true", service: "one"
	And a finalization work order for a service line retire "workordertwo" exists with contractor: "one", operating center: "one", markout requirement: "none", permit required: "true", service: "one"
	And a service material "one" exists with description: "Copper"
	And a pitcher filter delivery method "one" exists with description: "handed to customer"
	And a pitcher filter delivery method "two" exists with description: "left on porch/doorstep"
	And a pitcher filter delivery method "three" exists with description: "other"
	And a service size "one" exists with service size description: "1/2"
	And a crew assignment "ca" exists with finalization work order for a service line renewal company side: "one", crew: "one", assigned for: "today", assigned on: "today"
	And a work order flushing notice type "one" exists with description: "foobarbaz"
	And a valve work description "one" exists with time to complete: "1.1234", asset type: "valve", description: "turn valve"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the WorkOrderFinalization/Edit page for finalization work order for a service line renewal company side: "one"
	When I select "Yes" from the HasPitcherFilterBeenProvidedToCustomer dropdown
	And I click the "Initial Information" tab
	And I press Submit
	Then I should see the validation message "The InitialServiceLineFlushTime field is required."
    Then I should see a validation message for IsThisAMultiTenantFacility with "The IsThisAMultiTenantFacility field is required."
	When I visit the WorkOrderFinalization/Edit page for finalization work order for a service line retire: "workordertwo"    
    And I click the "Additional" tab 
    And I press "Submit"
    Then I should not see a validation message for IsThisAMultiTenantFacility with "The IsThisAMultiTenantFacility field is required."

Scenario: User sees an inline notification when pitcher filter has been delivered    
	Given a premise "one" exists with premise number: "1234567890", service address house number: "7", service address apartment: "garbage", service address street: "EaSy St", service address fraction: "1/2", equipment: "123", meter serial number: "123", operating center: "one"
	And a service "one" exists with service number: "123", operating center: "one", premise number: "1234567890", premise: "one"    
	And a finalization work order for a service line renewal company side "one" exists with contractor: "one", operating center: "one", markout requirement: "none", permit required: "true", service: "one", has pitcher filter been provided to customer: "true", date pitcher filter delivered to customer: "today", premise number: "1234567890", premise: "one"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the WorkOrderFinalization/Edit page for finalization work order for a service line renewal company side: "one"
	When I click the "Additional" tab
	Then I should see "Pitcher filter last delivered on"
	When I visit the WorkOrderGeneral/Show page for finalization work order for a service line renewal company side: "one"
	Then I should see "Pitcher filter last delivered on"

Scenario: User does not see an inline notification when pitcher filter has not been delivered
	Given a premise "one" exists with premise number: "1234567890", service address house number: "7", service address apartment: "garbage", service address street: "EaSy St", service address fraction: "1/2", equipment: "123", meter serial number: "123", operating center: "one"
	And a service "one" exists with service number: "123", operating center: "one", premise number: "1234567890", premise: "one"    
	And a finalization work order for a service line renewal company side "one" exists with contractor: "one", operating center: "one", markout requirement: "none", permit required: "true", service: "one", has pitcher filter been provided to customer: "false", premise number: "1234567890", premise: "one"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the WorkOrderFinalization/Edit page for finalization work order for a service line renewal company side: "one"
	When I click the "Additional" tab
	Then I should not see "Pitcher filter last delivered on"
	When I visit the WorkOrderGeneral/Show page for finalization work order for a service line renewal company side: "one"
	Then I should not see "Pitcher filter last delivered on"		