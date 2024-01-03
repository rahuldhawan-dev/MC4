Feature: FilterMedia
    In order to maintain information on Filter Mediae attached to various pieces of equipment
	As a user
	I want to be able to add, remove, and update records pertaining to Filter Mediae

Background: facility, equipment, and users exist
	Given a user "user" exists with username: "user"
	And a town "lazytown" exists
	And an operating center "opc" exists with opcode: "NJ4", name: "Lakewood", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
	And a role "roleRead" exists with action: "Read", module: "ProductionFacilities", user: "user", operating center: "opc"
	And a role "roleReadEq" exists with action: "Read", module: "ProductionEquipment", user: "user", operating center: "opc"
	And a role "roleEdit" exists with action: "Edit", module: "ProductionFacilities", user: "user"
	And a role "roleAdd" exists with action: "Add", module: "ProductionFacilities", user: "user"
	And a role "roleDelete" exists with action: "Delete", module: "ProductionFacilities", user: "user"
    And an equipment type "filter" exists with description: "Filter" 
    And an filter equipment lifespan "filter" exists
    And an equipment model "filter" exists with equipment manufacturer: "filter", description: "filter model"
    And an equipment purpose "filter" exists with filter equipment lifespan: "filter", description: "filter", equipment type: "filter"
    And a facility "facility" exists with operating center: "opc", facility id: "NJSB-01", facility name: "The Facility"
    And an equipment "one" exists with identifier: "NJSB-1-EQID-1", facility: "facility"
    And a filter media "one" exists with equipment: "one"

Scenario: user views a Facility with no Equipment
    Given a facility "facility0" exists with operating center: "opc", facility id: "NJSB-00"
    And I am logged in as "user"
    When I visit the show page for facility: "facility0"
    And I click the "Filter Media" tab
    Then I should see "This facility has no equipment listed. A piece of equipment is required to attach a filter media record to. Please add at least one equipment record first."
    And I should see a link to the Equipment/New page with querystring facility=facility: "facility0"'s Id

Scenario: user views a Facility with FilterMediae
    Given I am logged in as "user"	
    When I visit the show page for facility: "facility"
    And I click the "Filter Media" tab
    Then I should not see "This facility has no equipment listed. A piece of equipment is required to attach a filter media record to. Please add at least one equipment record first."
    And I should see a link to the Show page for filter media: "one"
    And I should see a link to the FilterMedia/New page with querystring facility=facility: "facility"'s Id

Scenario: user adds a FilterMedia record to a Facility
    Given I am logged in as "user"	
    When I visit the show page for facility: "facility"
    And I click the "Filter Media" tab
    And I follow "Add New"
    Then I should see a link to the Show page for facility: "facility"	
    When I select equipment "one"'s Display from the Equipment dropdown
    And I select "Some Kinda Media" from the MediaType dropdown
    And I select "Some Kinda Wash" from the WashType dropdown
    And I select "Some Kinda Way of Controlling Levels" from the LevelControlMethod dropdown
    And I select "Some Kinda Filter" from the FilterType dropdown
    And I select "Could've sworn it was here somewhere..." from the Location dropdown
    And I press Save
    Then I should be at the FilterMedia/Show/2 page

Scenario: user views a piece of Equipment with no FilterMediae
    Given a facility "facility0" exists with operating center: "opc", facility id: "NJSB-00"
    And an equipment "equipment0" exists with identifier: "NJSB-0-EQID-0", facility: "facility0", equipment type: "filter", equipment purpose: "filter"
    And I am logged in as "user"
    When I visit the show page for equipment: "equipment0"
    And I click the "Filter Media" tab
    Then I should see a link to the FilterMedia/New page with querystring facility=facility: "facility0"'s Id&equipment=equipment: "equipment0"'s Id

Scenario: user adds a FilterMedia record to a piece of Equipment
    Given a facility "facility0" exists with operating center: "opc", facility id: "NJSB-00"
    Given an equipment "two" exists with identifier: "NJSB-0-EQID-0", facility: "facility0", equipment type: "filter", equipment purpose: "filter"
    And I am logged in as "user" 
    When I visit the show page for equipment: "two" 
    And I click the "Filter Media" tab
    And I follow "Add New"
    Then I should see a link to the Show page for facility: "facility0"
    And I should see a link to the Show page for equipment: "two"
    When I select "Some Kinda Media" from the MediaType dropdown
    And I select "Some Kinda Wash" from the WashType dropdown
    And I select "Some Kinda Way of Controlling Levels" from the LevelControlMethod dropdown
    And I select "Some Kinda Filter" from the FilterType dropdown
    And I select "Could've sworn it was here somewhere..." from the Location dropdown
    And I press Save
    Then I should be at the FilterMedia/Show/2 page

Scenario: user views a FilterMedia with no Equipment and sets Facility/Equipment
    Given a filter media "zero" exists with equipment: null
    And I am logged in as "user"
    When I visit the Show page for filter media: "zero"
    Then I should be at the Show page for filter media: "zero"
    When I follow "Edit"
    And I select facility "facility" from the Facility dropdown
    And I select equipment "one"'s Display from the Equipment dropdown
    And I press Save
    Then I should be at the Show page for filter media: "zero"
    And I should see a link to the Show page for facility: "facility"
    And I should see a link to the Show page for equipment: "one"