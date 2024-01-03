Feature: OperatorLicenseReportPage
	

Background: AllYourSetupAreBelongsToMeAgain
	Given an admin user "admin" exists with username: "admin"
	And an operating center "nj7" exists with opcode: "nj7"
	And an operating center "nj4" exists with opcode: "nj4"
	And an employee status "active" exists with description: "Active"
	And an employee "D:" exists with operating center: "nj7", status: "active", first name: "Bill", last name: "S. Preston Esq."
	And an employee ":(" exists with operating center: "nj4", status: "active", first name: "NotBill", last name: "NotS. Preston Esq."
	And a operator license type "one" exists with description: "The best license type"
	And a operator license type "two" exists with description: "The worst license type"
	And a state "one" exists with abbreviation: "NJ"
	And a operator license "one" exists with employee: "D:", state: "one", operator license type: "one", "operating center: "nj7", expiration date: "yesterday", validation date: "today"
	And a operator license "two" exists with employee: ":(", state: "one", operator license type: "two", "operating center: "nj7", expiration date: "tomorrow", validation date: "today"
	And a operator license "four" exists with employee: ":(", operator license type: "two", licensed operator of record: "true", "operating center: "nj4", expiration date: "tomorrow", validation date: "today", state: "one"


Scenario: Operator License can be searched
	Given I am logged in as "admin"
	And I am at the Reports/OperatorLicenseReport/Search page
	When I select state "one" from the State dropdown
	And I press Search
	Then I should see the following values in the results table 
	| State | Operator License Type  | Employee |
	| NJ    | The best license type | Bill S. Preston Esq. |

Scenario: Operator License can be searched by employee status
	Given I am logged in as "admin"
	And an employee status "inactive" exists with description: "Inactive"
	And an employee "In" exists with operating center: "nj7", status: "inactive", first name: "Harry", last name: "Potter"
	And a operator license "three" exists with employee: "In", state: "one", operator license type: "one", "operating center: "nj7", expiration date: "tomorrow", validation date: "today"
	And I am at the Reports/OperatorLicenseReport/Search page
	When I select employee status "active" from the EmployeeStatus dropdown
	And I press Search
	Then I should see the following values in the results table 
	| State | Employee             | Operator License Type | Expired |
	| NJ    | Bill S. Preston Esq. | The best license type | Yes     |
	When I go back
	And I select employee status "inactive" from the EmployeeStatus dropdown
	And I press Search
	Then I should see the following values in the results table 
	| State | Employee		| Operator License Type | Expired |
	| NJ    | Harry Potter	| The best license type | No      |

Scenario: Operator License can be searched by expired field
	Given I am logged in as "admin"
	And I am at the Reports/OperatorLicenseReport/Search page
	When I select "Yes" from the Expired dropdown
	And I press Search
	Then I should see the following values in the results table 
	| State | Employee			   | Operator License Type | Expired |
	| NJ    | Bill S. Preston Esq. | The best license type | Yes     |
	When I go back
	And I select "No" from the Expired dropdown
	And I press Search
	Then I should see the following values in the results table 
	| State | Employee					| Operator License Type | Expired |
	| NJ    | NotBill NotS. Preston Esq.| The worst license type| No      |

Scenario: Operator License can be searched by Licensed Operator Of Record
	Given I am logged in as "admin"
	And I am at the Reports/OperatorLicenseReport/Search page
	When I select "Yes" from the LicensedOperatorOfRecord dropdown
	And I press Search
	Then I should see the following values in the results table 
	| State | Employee                      | Operator License Type  | Expired | Licensed Operator Of Record |
	| NJ    | NotBill NotS. Preston Esq.	| The worst license type |No       | Yes                         |
	When I go back
	And I select "Yes" from the Expired dropdown
	And I select "No" from the LicensedOperatorOfRecord dropdown
	And I press Search
		Then I should see the following values in the results table 
	| State | Employee			  | Operator License Type | Expired | Licensed Operator Of Record |
	| NJ    | Bill S. Preston Esq.| The best license type | Yes     | No                          |

