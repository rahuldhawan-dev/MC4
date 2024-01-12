Feature: CrewPage

Background: admin user exists
	Given an admin user "admin" exists with username: "admin"
    And a user "user" exists with username: "user"
	And a state "NJ" exists with abbreviation: "NJ"
	And a town "Swedesboro" exists with state: "NJ"	
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", state: "NJ"
	And a role "workorder-read" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a role "workorder-add" exists with action: "Add", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a role "workorder-edit" exists with action: "Edit", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a role "workorder-useradmin" exists with action: "UserAdministrator", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a role "asset-read" exists with action: "Read", module: "FieldServicesAssets", user: "user", operating center: "nj7"
	And a work description "hydrant installation" exists with description: "hydrant installation"
	And a work order "one" exists with operating center: "nj7", work description: "hydrant installation"
	And a work order "two" exists with operating center: "nj7", work description: "hydrant installation"	
	And a work order "three" exists with operating center: "nj7", work description: "hydrant installation"	
	And a work order "four" exists with operating center: "nj7", work description: "hydrant installation"	
	And a crew "one" exists with description: "one", availability: "8", operating center: "nj7", active: true	
	And a crew "two" exists with description: "one", availability: "8", operating center: "nj7", active: false	
    And a crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"
    And a crew assignment "ca2" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca3" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca4" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca5" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca6" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca7" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca8" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca9" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca10" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca11" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca12" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca13" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca14" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca15" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca16" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca17" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca18" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca19" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca20" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca21" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca22" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca23" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca24" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca25" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca26" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca027" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca028" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca0028" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca00028" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca29" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca30" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca31" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca32" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca33" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca34" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca35" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca36" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca37" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca38" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca39" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca40" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca41" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca42" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca43" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca44" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca45" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca46" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca47" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca48" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca49" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca50" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca51" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca52" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca53" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca54" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca55" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca56" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca57" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca58" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca59" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca60" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca61" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca62" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca63" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca64" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca65" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca66" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca67" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca68" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca69" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca70" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca71" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca72" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca73" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca74" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca75" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca76" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca77" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca78" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca79" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca80" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca81" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca82" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca83" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca84" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca85" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca86" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca87" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca88" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca89" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca90" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca91" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca92" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca93" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca94" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca95" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca96" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca97" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca98" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca99" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca100" exists with work order: "two", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"   
    And a crew assignment "ca1" exists with work order: "one", crew: "two", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"
          
@headful	
Scenario: user can search the crew
	Given I am logged in as "user"
	And I am at the FieldOperations/Crew/Search page
	When I select state "NJ" from the State dropdown	
	And I select operating center "nj7" from the OperatingCenter dropdown	
	And I enter "one" into the Description field		
	And I press Search	
	Then I should see the following values in the results table
	| Crew Name | Availability (hours) | Operating Center | Active |
	| one       | 8.00                 | NJ7 - Shrewsbury | Yes    |
	| one       | 8.00                 | NJ7 - Shrewsbury | No     |

Scenario: user can search the active crew
	Given I am logged in as "user"
	And I am at the FieldOperations/Crew/Search page
	When I select state "NJ" from the State dropdown	
	And I select operating center "nj7" from the OperatingCenter dropdown		 
	And I enter "one" into the Description field	
	And I check the Active field
	And I press Search	
	Then I should see the following values in the results table
	| Crew Name | Availability (hours) | Operating Center | Active |
	| one       | 8.00                 | NJ7 - Shrewsbury | Yes    |

Scenario: user can add new crew
    Given I am logged in as "user"    
    And I am at the FieldOperations/Crew/New page   
	When I enter "one" into the Description field
	And I select operating center "nj7" from the OperatingCenter dropdown	
	And I enter "2" into the Availability field
	And I check the Active field
	And I press Save
    Then I should be at the FieldOperations/Crew/Show/3 page   

Scenario: User gets a validation error if they save without crew information
	Given I am logged in as "user"
	And I am at the FieldOperations/Crew/New page
	When I press Save
	Then I should see the validation message "The Name field is required."
	And  I should see the validation message "The Availability (hours) field is required."
	And  I should see the validation message "The OperatingCenter field is required."

Scenario: User can edit crew
    Given I am logged in as "user"
	When I visit the FieldOperations/Crew/Edit/1 page
	And I enter "New demo" into the Description field	
	And I press Save
	Then I should be at the FieldOperations/Crew/Show/1 page

Scenario: User gets a validation error if they not enter number in availability
	Given I am logged in as "user"
	And I am at the FieldOperations/Crew/New page
	When I enter "one" into the Description field
	And I select operating center "nj7" from the OperatingCenter dropdown	
	And I enter "demo" into the Availability field
	When I press Save
	Then I should see the validation message "The field Availability (hours) must be a number."	