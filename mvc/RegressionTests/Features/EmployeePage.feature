Feature: Employee Page

Background: here you go have some test data
    Given an operating center "nj7" exists with opcode: "NJ7"
    And an operating center "nj4" exists with opcode: "NJ4"
    And an admin user "admin" exists with username: "admin"
    And a user "user" exists with username: "user"
    And a user "user-no-roles" exists with username: "user-no-roles"
    And a role "roleReadNj7" exists with action: "Read", module: "HumanResourcesEmployee", operating center: "nj7", user: "user"
    And a role "roleEditNj7" exists with action: "Edit", module: "HumanResourcesEmployee", operating center: "nj7", user: "user"
    And a role "roleAddNj7" exists with action: "Add", module: "HumanResourcesEmployee", operating center: "nj7", user: "user"
    And a role "roleDeleteNj7" exists with action: "Delete", module: "HumanResourcesEmployee", operating center: "nj7", user: "user"
	And a role "roleReadNj4" exists with action: "Read", module: "HumanResourcesEmployee", operating center: "nj4"
	And a role "roleEditNj4" exists with action: "Edit", module: "HumanResourcesEmployee", operating center: "nj4"
	And a role "roleAddNj4" exists with action: "Add", module: "HumanResourcesEmployee", operating center: "nj4"
	And a role "roleDeleteNj4" exists with action: "Delete", module: "HumanResourcesEmployee", operating center: "nj4"
    And a role "roleAccReadNj7" exists with action: "Read", module: "HumanResourcesAccountabilityAction", operating center: "nj7", user: "user"
    And a role "roleAccEditNj7" exists with action: "Edit", module: "HumanResourcesAccountabilityAction", operating center: "nj7", user: "user"
    And a role "roleAccAddNj7" exists with action: "Add", module: "HumanResourcesAccountabilityAction", operating center: "nj7", user: "user"
    And a role "roleAccDeleteNj7" exists with action: "Delete", module: "HumanResourcesAccountabilityAction", operating center: "nj7", user: "user"
    And a role "roleAccReadNj4" exists with action: "Read", module: "HumanResourcesAccountabilityAction", operating center: "nj4", user: "user"
    And a role "roleAccEditNj4" exists with action: "Edit", module: "HumanResourcesAccountabilityAction", operating center: "nj4", user: "user"
    And a role "roleAccAddNj4" exists with action: "Add", module: "HumanResourcesAccountabilityAction", operating center: "nj4", user: "user"
    And a user "invalid" exists with username: "invalid"
    And a user "read-only-nj7" exists with username: "read-only-nj7", roles: roleReadNj7
    And a user "user-admin-nj7" exists with username: "user-admin-nj7", roles: roleReadNj7;roleEditNj7;roleAddNj7;roleDeleteNj7
    And a user "read-only-nj4" exists with username: "read-only-nj4", roles: roleReadNj4
    And a user "user-admin-nj4" exists with username: "user-admin-nj4", roles: roleReadNj4;roleEditNj4;roleAddNj4;roleDeleteNj4
    And a user "user-admin-both" exists with username: "user-admin-both", roles: roleReadNj4;roleEditNj4;roleAddNj4;roleDeleteNj4;roleReadNj7;roleEditNj7;roleAddNj7;roleDeleteNj7
    And an employee "nj7-1" exists with operating center: "nj7"
    And an employee "nj7-2" exists with operating center: "nj7"
    And an employee "nj7-3" exists with operating center: "nj7"
    And an employee "nj4-1" exists with operating center: "nj4"
    And an employee "nj4-2" exists with operating center: "nj4"
    And an employee "nj4-3" exists with operating center: "nj4"
    And an employee "vOv" exists	
    And an employee status "active" exists with description: "Active"
    And an employee status "inactive" exists with description: "Inactive"
    And an employee department "department1" exists with description: "Shoes"
    And an employee department "department2" exists with description: "Lingerie"
    And a facility "facility1" exists with operating center: "nj7", facility name: "fac1"
    And a facility "facility2" exists with operating center: "nj7", facility name: "fac2"
    And a facility "facility3" exists with operating center: "nj4"
    And a facility "facility4" exists with operating center: "nj4"
    And a gender "male" exists with description: "Male"
    And a gender "female" exists with description: "Female"
    And an incident "one" exists with operating center: "nj7", employee: "nj7-1", incident date: 10/2/2020, DateInvestigationWillBeCompleted: 10/2/2020
    And an absence status "active" exists with description: "active"
    And a liability type "one" exists
    And a state "one" exists with na
    And an employee absence claim "one" exists
    And a claims representative "one" exists with description: "Ned Ryerson"
    And a general liability claim "one" exists with company contact: "nj7-1", operating center: "nj7", liability type: "one", IncidentDateTime: 10/2/2020, claims representative: "one", claim number: "2000", Name: "Zippy Flippy"
    And an absence notification "one" exists with employee: "nj7-1", start date: 10/2/2020, last day of work: 10/2/2020, end date: 10/2/2020, EmployeeAbsenceClaim: "one", absence status: "active"
    And a reason for departure "reason1" exists with description: "Left the oven on"
    And a reason for departure "reason2" exists with description: "It's not you%2C it's me"
    And a union affiliation "ua1" exists with description: "CPUSA"
    And a union affiliation "ua2" exists with description: "CCCPUSA"
    And an institutional knowledge "ik1" exists with description: "All I wanted was a Pepsi"
    And an institutional knowledge "ik2" exists with description: "Just one Pepsi"
    And a d p c c status "dpcc1" exists with description: "EHS Responder"
    And a t c p a status "tcpa1" exists with description: "EHS Responder"
	And a position group "pg1" exists with group: "Q11"
	And a role "roleFacilityReadNj4" exists with action: "Read", module: "ProductionFacilities", user: "user-admin-nj7", operating center: "nj4"
	And a role "roleFacilityReadNj7" exists with action: "Read", module: "ProductionFacilities", user: "user-admin-nj7", operating center: "nj7"
    And an employee accountability action "one" exists with operating center: "nj7", Employee: "nj7-1", DisciplineAdministeredBy: "nj7-2", AccountabilityActionTakenDescription: "Descriptive words", DateAdministered: 10/2/2020
    And an employee accountability action "two" exists with operating center: "nj7", Employee: "nj7-2", DisciplineAdministeredBy: "nj7-2", AccountabilityActionTakenDescription: "Descriptive words", DateAdministered: 10/2/2020

Scenario: an admin user should be able to view the show page without a compilation error showing up
	Given I am logged in as "admin"
    When I visit the Show page for employee: "nj7-1"
	Then I should see the "Job Observations" tab

Scenario: a user without the role cannot access the employee page
	Given I am logged in as "user-no-roles"
	When I visit the /Employee/Search page
    Then I should see the missing role error

Scenario: admin with green shoes can search by opcode
    Given I am logged in as "admin"
    When I visit the /Employee/Search page
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I press Search
    And I wait for the page to reload
    Then I should see employee "nj7-1"'s EmployeeId in the "Employee ID" column
    And I should see employee "nj7-2"'s EmployeeId in the "Employee ID" column
    And I should not see employee "nj4-1"'s EmployeeId in the "Employee ID" column
    And I should not see employee "nj4-2"'s EmployeeId in the "Employee ID" column
    When I visit the /Employee/Search page
    And I select operating center "nj4" from the OperatingCenter dropdown
    And I press Search
    And I wait for the page to reload
    Then I should not see employee "nj7-1"'s EmployeeId in the "Employee ID" column
    And I should not see employee "nj7-2"'s EmployeeId in the "Employee ID" column
    And I should see employee "nj4-1"'s EmployeeId in the "Employee ID" column
    And I should see employee "nj4-2"'s EmployeeId in the "Employee ID" column

Scenario: user with halitosis should only be able to search within operating centers they have access to
    Given I am logged in as "read-only-nj7"
    When I visit the /Employee/Search page
    Then I should see operating center "nj7" in the OperatingCenter dropdown
    And I should not see operating center "nj4" in the OperatingCenter dropdown
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I press Search
    And I wait for the page to reload
    Then I should see employee "nj7-1"'s EmployeeId in the "Employee ID" column
    And I should see employee "nj7-2"'s EmployeeId in the "Employee ID" column

Scenario: user with edit rights and an arthritic left hand csn efit employee
    Given I am logged in as "user-admin-nj7"
    When I visit the Show page for employee: "nj7-1"
    And I follow "Edit"
    Then I should be at the Edit page for employee: "nj7-1"
    And I should not see operating center "nj4" in the OperatingCenter dropdown
	When I select employee status "active"'s Description from the Status dropdown
    And I select employee department "department1"'s Description from the Department dropdown
	And I enter "Q11" and select position group "pg1"'s Description from the PositionGroup combobox
    And I select gender "male"'s Description from the Gender dropdown
    And I select reason for departure "reason1"'s Description from the ReasonForDeparture dropdown
    And I select institutional knowledge "ik1"'s Description from the InstitutionalKnowledge dropdown
    And I select union affiliation "ua1"'s Description from the UnionAffiliation dropdown
    And I select d p c c status "dpcc1"'s Description from the DPCCStatus dropdown	
    And I select t c p a status "tcpa1"'s Description from the TCPAStatus dropdown	
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I select employee "nj7-2"'s Description from the ReportsTo dropdown
    And I select employee "nj7-3"'s Description from the PurchaseCardReviewer dropdown
    And I select employee "nj7-3"'s Description from the PurchaseCardApprover dropdown
    And I select facility "facility2"'s Description from the ReportingFacility dropdown
    And I press Save
    Then I should be at the show page for employee: "nj7-1"
    And I should see a display for "Status" with employee status: "active"'s Description
	And I should see a display for "Department" with employee department: "department1"'s Description
	And I should see a display for "PositionGroup" with position group: "pg1"'s Description
	And I should see a display for "ReportingFacility" with facility: "facility2"'s Description
    And I should see a display for "Gender" with gender: "male"'s Description
    And I should see a display for "ReasonForDeparture" with reason for departure: "reason1"'s Description
	And I should see a display for "ReportsTo_ProperName" with employee: "nj7-2"'s ProperName
    And I should see a display for "PurchaseCardReviewer_ProperName" with employee: "nj7-3"'s ProperName
    And I should see a display for "PurchaseCardApprover_ProperName" with employee: "nj7-3"'s ProperName
    And I should see a display for "InstitutionalKnowledge" with institutional knowledge: "ik1"'s Description
    And I should see a display for "UnionAffiliation" with union affiliation: "ua1"'s Description
    And I should see a display for "DPCCStatus" with d p c c status: "dpcc1"'s Description
    And I should see a display for "TCPAStatus" with t c p a status: "tcpa1"'s Description

Scenario: user with add rights can add a new employee and lead a horse to water, but can't make him drink from it
    Given a not in program commercial drivers license Program status "one" exists
	And a schedule type "one" exists with description: "7-11"
	And I am logged in as "user-admin-nj7"
    When I visit the /Employee/New page
	And I select employee status "inactive"'s Description from the Status dropdown
    And I select employee department "department2"'s Description from the Department dropdown
	And I enter "Q11" and select position group "pg1"'s Description from the PositionGroup combobox
    And I select gender "male"'s Description from the Gender dropdown
    And I select reason for departure "reason2"'s Description from the ReasonForDeparture dropdown
    And I select institutional knowledge "ik2"'s Description from the InstitutionalKnowledge dropdown
    And I select union affiliation "ua2"'s Description from the UnionAffiliation dropdown
    And I select d p c c status "dpcc1"'s Description from the DPCCStatus dropdown
    And I select t c p a status "tcpa1"'s Description from the TCPAStatus dropdown
    And I enter "655321" into the EmployeeId field
    And I enter "Alexander" into the FirstName field
    And I enter "Delarge" into the LastName field
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I select employee "nj7-2"'s Description from the ReportsTo dropdown
    And I select employee "nj7-3"'s Description from the PurchaseCardReviewer dropdown
    And I select employee "nj7-3"'s Description from the PurchaseCardApprover dropdown
    And I select facility "facility1"'s Description from the ReportingFacility dropdown
	And I select not in program commercial drivers license Program status "one"'s Description from the CommercialDriversLicenseProgramStatus dropdown
	And I select schedule type "one" from the ScheduleType dropdown
    And I press Save
    Then the currently shown employee will now be referred to as "new employee"
    And I should be at the Show page for employee: "new employee"
    And I should see a display for "Status" with employee status: "inactive"'s Description
	And I should see a display for "Department" with employee department: "department2"'s Description
	And I should see a display for "PositionGroup" with position group: "pg1"'s Description
	And I should see a display for "ReportingFacility" with facility: "facility1"'s Description
    And I should see a display for "Gender" with gender: "male"'s Description
    And I should see a display for "ReasonForDeparture" with reason for departure: "reason2"'s Description
	And I should see a display for "ReportsTo_ProperName" with employee: "nj7-2"'s ProperName
    And I should see a display for "PurchaseCardReviewer_ProperName" with employee: "nj7-3"'s ProperName
    And I should see a display for "PurchaseCardApprover_ProperName" with employee: "nj7-3"'s ProperName
    And I should see a display for "InstitutionalKnowledge" with institutional knowledge: "ik2"'s Description
    And I should see a display for "UnionAffiliation" with union affiliation: "ua2"'s Description
    And I should see a display for "DPCCStatus" with d p c c status: "dpcc1"'s Description
    And I should see a display for "TCPAStatus" with t c p a status: "tcpa1"'s Description
	And I should see a display for "ScheduleType" with schedule type: "one"'s Description

Scenario: User must enter restriction date if employee has restriction
    Given I am logged in as "user-admin-nj7"
    When I visit the Edit page for employee: "nj7-1"
	And I check the HasOneDayDoctorsNoteRestriction field
	And I press Save
	Then I should see the validation message "You must set the restriction end date if the employee has a one day doctor's note restriction."
    
Scenario: User should see the Tabs for Incidents and the values
    Given I am logged in as "admin"
    When I visit the Show page for employee: "nj7-1"
    Then I should see the "Incidents" tab
    When I click the "Incidents" tab
    Then I should see the following values in the Incident table
    | Incident Status         | Incident Date/Time | Date Investigation Will Be Completed | Incident Classification         | Injury Type           | SIF | Is OSHA Recordable |
    | *IncidentStatusFactory* | 10/2/2020 12:00 AM | 10/2/2020 12:00:00 AM                | *IncidentClassificationFactory* | *IncidentTypeFactory* | | No                 |

Scenario: User should see the Tabs for Absences and the values
    Given I am logged in as "admin"
    When I visit the Show page for employee: "nj7-1"
    Then I should see the "Absence Notification" tab
    When I click the "Absence Notification" tab
    Then I should see the following values in the AbsenceNotification table
    | Absence Start Date | Last Day Of Work | Date RTW  | Employee Absence Claim  | Absence Status |
    | 10/2/2020          | 10/2/2020        | 10/2/2020 | *EmployeeAbsenceClaim*  | active         |

Scenario: User should see the Tabs for General Liability Claim and the values
    Given I am logged in as "admin"
    When I visit the Show page for employee: "nj7-1"
    Then I should see the "General Liability Claim" tab
    When I click the "General Liability Claim" tab
    Then I should see the following values in the GeneralLiabilityClaim table
    | Incident Date Time | Claims Representative | Claim Number | Claimant Name | Liability Type  |
    | 10/2/2020 12:00 AM | Ned Ryerson           | 2000         | Zippy Flippy  | *LiabilityType* |

Scenario: User can search and see which employees have Employee Accountability Actions 
    Given I am logged in as "user"
    When I visit the /Employee/Search page
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I select "Yes" from the EmployeeAccountabilityActions dropdown
    And I press Search
    And I wait for the page to reload
    Then I should see employee "nj7-1"'s EmployeeId in the "Employee ID" column
    And I should see employee "nj7-2"'s EmployeeId in the "Employee ID" column
    And I should not see employee "nj7-3"'s EmployeeId in the "Employee ID" column

Scenario: Users without the HumanResourcesAccountabilityAction role should not see those fields
    Given I am logged in as "user-admin-nj7"
    When I visit the /Employee/Search page
    Then I should not see the "AccountabilityActionTakenType" field
    And I should not see the "ModifiedAccountabilityActionTakenType" field
    And I should not see the "EmployeeAccountabilityActions" field