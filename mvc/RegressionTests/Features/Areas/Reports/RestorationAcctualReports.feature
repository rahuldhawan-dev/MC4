Feature: Restoration Accrual Reports

Background: 
	Given a user "user" exists with username: "user"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a role "read" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And accounting types exist
	And work descriptions exist
	And accounting type "o and m" is set for work description "change burst meter"
	And accounting type "capital" is set for work description "curb box repair"
	And a work order "o and m 1" exists with date completed: "04/24/1984", approved on: "04/24/1984", work description: "change burst meter", operating center: "nj7" 
	And a work order "o and m 2" exists with date completed: "04/25/1984", approved on: "04/25/1984", work description: "change burst meter", operating center: "nj7"
	And a work order "capital 1" exists with date completed: "04/26/1984", approved on: "04/26/1984", work description: "curb box repair", operating center: "nj7"
	And a work order "capital 2" exists with date completed: "04/26/1984", approved on: "04/26/1984", work description: "curb box repair", operating center: "nj7"
	And a restoration: "o and m 1" exists with work order: "o and m 1", operating center: "nj7", total accrued cost: "10.00" 
	And a restoration: "o and m 2" exists with work order: "o and m 2", operating center: "nj7", total accrued cost: "20.00"
	And a restoration: "capital 1" exists with work order: "capital 1", operating center: "nj7", total accrued cost: "30.00"
	And a restoration: "capital 2" exists with work order: "capital 2", operating center: "nj7", total accrued cost: "40.00"

Scenario: User can search for O&M restoration accrual reports
	Given I am logged in as "user"
	And I am at the Reports/RestorationAccrualReport/Search page
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I enter "4/23/1984" into the WorkOrderDateCompleted_Start field
	And I enter "4/29/1984" into the WorkOrderDateCompleted_End field
	And I select accounting type "o and m" from the AccountingType dropdown
	When I press Search
	Then I should see the following values in the results table
	| Total Accrued Cost | Accrual Value |
	| $10.00             | $10.00        |
	| $20.00             | $20.00        |
	|                    | $30.00        |

Scenario: User can search for Capital restoration accrual reports
	Given I am logged in as "user"
	And I am at the Reports/RestorationAccrualReport/Search page
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I enter "4/23/1984" into the WorkOrderDateCompleted_Start field
	And I enter "4/29/1984" into the WorkOrderDateCompleted_End field
	And I select accounting type "capital" from the AccountingType dropdown
	When I press Search
	Then I should see the following values in the results table
	| Total Accrued Cost | Accrual Value |
	| $30.00             | $30.00        |
	| $40.00             | $40.00        |
	|                    | $70.00        |