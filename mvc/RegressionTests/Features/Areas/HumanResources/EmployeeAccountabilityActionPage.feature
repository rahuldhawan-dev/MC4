Feature: EmployeeAccountabilityAction Page

Background: here you go have some test data
    Given a state "one" exists with name: "New Jersey", abbreviation: "NJ"
    And an operating center "nj7" exists with opcode: "NJ7"
    And an operating center "nj4" exists with opcode: "NJ4", name: "Lakewood", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true", state: "one"
    And a user "user" exists with username: "user"
    And a role "roleReadNj7" exists with action: "Read", module: "HumanResourcesAccountabilityAction", operating center: "nj7", user: "user"
    And a role "roleEditNj7" exists with action: "Edit", module: "HumanResourcesAccountabilityAction", operating center: "nj7", user: "user"
    And a role "roleAddNj7" exists with action: "Add", module: "HumanResourcesAccountabilityAction", operating center: "nj7", user: "user"
    And a role "roleDeleteNj7" exists with action: "Delete", module: "HumanResourcesAccountabilityAction", operating center: "nj7", user: "user"
    And a role "roleReadNj4" exists with action: "Read", module: "HumanResourcesAccountabilityAction", operating center: "nj4", user: "user"
    And a role "roleEditNj4" exists with action: "Edit", module: "HumanResourcesAccountabilityAction", operating center: "nj4", user: "user"
    And a role "roleAddNj4" exists with action: "Add", module: "HumanResourcesAccountabilityAction", operating center: "nj4", user: "user"
    And a role "roleDeleteNj4" exists with action: "Delete", module: "HumanResourcesAccountabilityAction", operating center: "nj4", user: "user"
    And a role "EmproleReadNj7" exists with action: "Read", module: "HumanResourcesEmployee", operating center: "nj7", user: "user"
    And a role "EmproleEditNj7" exists with action: "Edit", module: "HumanResourcesEmployee", operating center: "nj7", user: "user"
    And an admin user "admin" exists with username: "admin"
    And a user "invalid" exists with username: "invalid"
    And a user "read-only-nj7" exists with username: "read-only-nj7", roles: roleReadNj7
    And a user "user-admin-nj7" exists with username: "user-admin-nj7", roles: roleReadNj7;roleEditNj7;roleAddNj7;roleDeleteNj7
    And a user "read-only-nj4" exists with username: "read-only-nj4", roles: roleReadNj4
    And a user "user-admin-nj4" exists with username: "user-admin-nj4", roles: roleReadNj4;roleEditNj4;roleAddNj4;roleDeleteNj4
    And a user "user-admin-both" exists with username: "user-admin-both", roles: roleReadNj4;roleEditNj4;roleAddNj4;roleDeleteNj4;roleReadNj7;roleEditNj7;roleAddNj7;roleDeleteNj7
    And an employee status "active" exists with description: "Active"
    And an employee "nj7-1" exists with operating center: "nj7", status: "active", first name: "Bill", last name: "S. Preston"
    And an employee "nj7-2" exists with operating center: "nj7", status: "active", first name: "Johnny", last name: "Hotdog"
    And an employee "nj7-7" exists with operating center: "nj7", status: "active", first name: "Mahi", last name: "Dhoni"
    And an employee "nj7-3" exists with operating center: "nj7", status: "active"
    And an employee "nj4-1" exists with operating center: "nj4", status: "active"
    And an employee "nj4-2" exists with operating center: "nj4", status: "active"
    And an employee "nj4-3" exists with operating center: "nj4", status: "active"
    And an employee "vOv" exists
    And an AccountabilityActionTakenType "VerbalCounseling" exists with Id: 1, Description: "Verbal Counseling"
    And an AccountabilityActionTakenType "WrittenWarning" exists with Id: 2, Description: "Written Warning"
    And an employee accountability action "one" exists with operating center: "nj7", Employee: "nj7-1", DisciplineAdministeredBy: "nj7-2", AccountabilityActionTakenDescription: "Descriptive words", DateAdministered: 10/2/2020
    And a data type "data type" exists with table name: "UnionGrievances", name: "Grievance"
    And a grievance category "one" exists with description: things
	And a grievance categorization "gc1" exists with description: this is a thing, grievance category: "one"
	And a grievance categorization "gc2" exists with description: this is also a thing, grievance category: "one"
	And a grievance categorization "gc3" exists with description: this is the third thing, grievance category: "one"

Scenario: user can add an Employee Accountability Action
    Given I am logged in as "user"
    When I visit the HumanResources/EmployeeAccountabilityAction page
    And I follow "Add"
    And I press Save
    Then I should see the validation message "The OperatingCenter field is required."
    Then I should see the validation message "The DisciplineAdministeredBy field is required."
    Then I should see the validation message "The AccountabilityActionTakenDescription field is required."
    Then I should see the validation message "The DateAdministered field is required."
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I select employee "nj7-1"'s Description from the DisciplineAdministeredBy dropdown
    And I select employee "nj7-2"'s Description from the Employee dropdown
    And I select AccountabilityActionTakenType "WrittenWarning" from the AccountabilityActionTakenType dropdown
    And I enter "2/4/2014" into the DateAdministered field
    And I enter "Alexander" into the AccountabilityActionTakenDescription field
    And I press Save
    Then I should see a display for "DisciplineAdministeredBy" with employee: "nj7-1"
    And I should see a display for DateAdministered with "2/4/2014"
    And I should see a display for AccountabilityActionTakenDescription with "Alexander"
    And I should see a display for AccountabilityActionTakenType with AccountabilityActionTakenType "WrittenWarning"
   
Scenario: user can add an Employee Accountability Action with Grievance
    Given a grievance "grievance1" exists with categorization: "gc1", grievance category: "one"
    And a grievance "grievance2" exists with categorization: "gc2", grievance category: "one"
    And a grievance "grievance3" exists with categorization: "gc3", grievance category: "one"
    And I am logged in as "admin"
    When I visit the /Grievance/Show/2 page
    And I click the "Employees" tab
    And I press "Link Employee"
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I check employee "nj7-7"'s Description in the EmployeeIds checkbox list
    And I press "Link Employees"
    When I visit the HumanResources/EmployeeAccountabilityAction page
    And I follow "Add"
    And I press Save
    Then I should see the validation message "The OperatingCenter field is required."
    Then I should see the validation message "The DisciplineAdministeredBy field is required."
    Then I should see the validation message "The AccountabilityActionTakenDescription field is required."
    Then I should see the validation message "The DateAdministered field is required."
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I select employee "nj7-1"'s Description from the DisciplineAdministeredBy dropdown
    And I select employee "nj7-7"'s Description from the Employee dropdown
    And I select AccountabilityActionTakenType "WrittenWarning" from the AccountabilityActionTakenType dropdown
    And I enter "2/4/2014" into the DateAdministered field
    And I enter "Alexander" into the AccountabilityActionTakenDescription field
    And I select "2" from the Grievance dropdown
    And I press Save
    Then I should see "2 - NJ7 -"
    Then I should see a display for "DisciplineAdministeredBy" with employee: "nj7-1"
    And I should see a display for DateAdministered with "2/4/2014"
    And I should see a display for AccountabilityActionTakenDescription with "Alexander"
    And I should see a display for AccountabilityActionTakenType with AccountabilityActionTakenType "WrittenWarning"


Scenario: user can edit an Employee Accountability Action and add modify data
    Given I am logged in as "user"
    When I visit the Edit page for employee accountability action: "one"
    And I check the HasModifiedDiscipline field
    And I press Save
    Then I should see the validation message "The ModifiedDisciplineAdministeredBy field is required."
    And I should see the validation message "The ModifiedAccountabilityActionTakenType field is required."
    And I should see the validation message "The ModifiedAccountabilityActionTakenDescription field is required."
    And I should see the validation message "The DateModified field is required."
    When I select employee "nj7-2"'s Description from the ModifiedDisciplineAdministeredBy dropdown
    And I select AccountabilityActionTakenType "WrittenWarning" from the ModifiedAccountabilityActionTakenType dropdown
    And I enter "2/4/2014" into the DateModified field
    And I enter "Modifed Descriptive words" into the ModifiedAccountabilityActionTakenDescription field
    And I press Save
    Then I should see a display for "ModifiedDisciplineAdministeredBy" with employee: "nj7-2"
    And I should see a display for DateModified with "2/4/2014"
    And I should see a display for ModifiedAccountabilityActionTakenDescription with "Modifed Descriptive words"
    And I should see a display for ModifiedAccountabilityActionTakenType with AccountabilityActionTakenType "WrittenWarning"

Scenario: user can see the Accountability Action in the Employee's Accountability Action tab
    Given I am logged in as "user"
    When I visit the Show page for employee accountability action: "one"
    Then I should see a display for AccountabilityActionTakenDescription with "Descriptive words"
    When I visit the show page for employee: "nj7-1"
    Then I should see the "Accountability Actions" tab
    When I click the "Accountability Actions" tab
    Then I should see the following values in the employeeAccountabilityActionsTable table
| Operating Center | Discipline Administered By | Accountability Action Taken Type | Accountability Action Taken Description | Date Administered |
| NJ7              | Johnny Hotdog              | **                               | Descriptive words                       | 10/2/2020         |

Scenario: user can see the Multiple Accountability Action in the Employee's Accountability Action tab
    Given I am logged in as "user"
    When I visit the HumanResources/EmployeeAccountabilityAction page
    And I follow "Add"
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I select employee "nj7-2"'s Description from the DisciplineAdministeredBy dropdown
    And I select employee "nj7-1"'s Description from the Employee dropdown
    And I select AccountabilityActionTakenType "WrittenWarning" from the AccountabilityActionTakenType dropdown
    And I enter "2/4/2014" into the DateAdministered field
    And I enter "Alexander" into the AccountabilityActionTakenDescription field
    And I press Save
    Then I should see a display for AccountabilityActionTakenDescription with "Alexander"
    And I should see a display for AccountabilityActionTakenType with "Written Warning"
    When I visit the show page for employee: "nj7-1"
    Then I should see the "Accountability Actions" tab
    When I click the "Accountability Actions" tab
    Then I should see the following values in the employeeAccountabilityActionsTable table
| Operating Center | Discipline Administered By | Accountability Action Taken Type | Accountability Action Taken Description | Date Administered |
| NJ7              | Johnny Hotdog              | **   | Descriptive words                       | 10/2/2020         |
| NJ7              | Johnny Hotdog              | Written Warning                  | Alexander                               | 2/4/2014          | 