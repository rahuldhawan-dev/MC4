Feature: RoleGroups

Background: 
	Given role actions exist
	And role applications exist
	And role modules exist
	And a state "nj" exists with name: "New Jersey", abbreviation: "NJ"	
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsburghy", state: "nj"
	And an operating center "nj4" exists with opcode: "NJ4", name: "Lakewood", state: "nj"
	And an admin user "site-admin" exists with username: "site-admin"
	And a role group "one" exists

#
# ADDING NEW ROLE GROUP ROLES
#

Scenario: Site admin should be able to add roles to a role group
	Given a role group role "user-admin-role1" exists with action: "UserAdministrator", module: "BPUGeneral", operating center: "nj7", role group: "one"
	And I am logged in as "site-admin"
	And I am at the Edit page for role group: "one"
	When I press "Add Roles"
	And I check operating center "nj7" in the OperatingCenters checkbox list
	And I check module "BPUGeneral" in the Modules checkbox list
	And I check role action "Read" in the Actions checkbox list 
	And I check role action "Edit" in the Actions checkbox list 
	# This first part ensures we're seeing this table updated on the edit screen.
	And I press save-roles-button
	Then I should see the following values in the application-BPU table
	| Module     | State | Operating Center | Action            |
	| BPUGeneral | NJ    | NJ7              | Edit              |
	| BPUGeneral | NJ    | NJ7              | Read              |
	| BPUGeneral | NJ    | NJ7              | UserAdministrator |
	# This second part ensures we're seeing it on the show screen
	When I click ok in the dialog after pressing "Save"
	Then I should be at the Show page for role group "one"
	And I should see the following values in the application-BPU table
	| Module     | State | Operating Center | Action            |
	| BPUGeneral | NJ    | NJ7              | Edit              |
	| BPUGeneral | NJ    | NJ7              | Read              |
	| BPUGeneral | NJ    | NJ7              | UserAdministrator |

Scenario: Site admin should be able to add new roles for wildcard operating center
	Given I am logged in as "site-admin"
	And I am at the Edit page for role group: "one"
	When I press "Add Roles"
	And I check the IsForAllOperatingCenters checkbox
	And I check module "BPUGeneral" in the Modules checkbox list
	And I check role action "Read" in the Actions checkbox list 
	And I check role action "Edit" in the Actions checkbox list 
	# This first part ensures we're seeing this table updated on the edit screen.
	And I press save-roles-button
	Then I should see the following values in the application-BPU table
	| Module     | State | Operating Center | Action |
	| BPUGeneral | ALL   | ALL              | Edit   |
	| BPUGeneral | ALL   | ALL              | Read   |
	# This second part ensures we're seeing it on the show screen
	When I click ok in the dialog after pressing "Save"
	Then I should be at the Show page for role group "one"
	And I should see the following values in the application-BPU table
	| Module     | State | Operating Center | Action |
	| BPUGeneral | ALL   | ALL              | Edit   |
	| BPUGeneral | ALL   | ALL              | Read   |

Scenario: Site admin should be able to use the add roles dialog multiple times before saving
	Given I am logged in as "site-admin"
	And I am at the Edit page for role group: "one"
	When I press "Add Roles"
	And I check operating center "nj7" in the OperatingCenters checkbox list
	And I check module "BPUGeneral" in the Modules checkbox list
	And I check role action "Read" in the Actions checkbox list 
	# This first part ensures we're seeing this table updated on the edit screen.
	And I press save-roles-button
	And I press "Add Roles"
	And I press "Reset"
	And I check operating center "nj4" in the OperatingCenters checkbox list
	And I check module "BusinessPerformanceGeneral" in the Modules checkbox list
	And I check role action "Read" in the Actions checkbox list 
	And I check role action "Edit" in the Actions checkbox list 
	# This again ensures we're seeing this table updated on the edit screen.
	And I press save-roles-button
	Then I should see the following values in the application-BPU table
	| Module     | State | Operating Center | Action |
	| BPUGeneral | NJ    | NJ7              | Read   |
	And I should see the following values in the application-BusinessPerformance table
	| Module                     | State | Operating Center | Action |
	| BusinessPerformanceGeneral | NJ    | NJ4              | Edit   |
	| BusinessPerformanceGeneral | NJ    | NJ4              | Read   |
	# This last part ensures we're seeing it on the show screen after actually saving.
	When I click ok in the dialog after pressing "Save"
	Then I should be at the Show page for role group "one"
	And I should see the following values in the application-BPU table
	| Module     | State | Operating Center | Action |
	| BPUGeneral | NJ    | NJ7              | Read   |
	And I should see the following values in the application-BusinessPerformance table
	| Module                     | State | Operating Center | Action |
	| BusinessPerformanceGeneral | NJ    | NJ4              | Edit   |
	| BusinessPerformanceGeneral | NJ    | NJ4              | Read   |

Scenario: Selecting a state will automatically select all of that state's operating centers that are not contracted operations
	Given a state "ny" exists with name: "New York", abbreviation: "NY"	
	And an operating center "ny1" exists with opcode: "NY1", name: "Some City 1", state: "ny"
	And an operating center "ny2" exists with opcode: "NY2", name: "Some Town 2", state: "ny"
	And an operating center "ny3" exists with opcode: "NY3", name: "Some Town 2", state: "ny", is contracted operations: "true"
	And I am logged in as "site-admin"
	And I am at the Edit page for role group: "one"
	When I press "Add Roles"
	And I check state "nj" in the States checkbox list
	Then operating center "nj4" should be checked in the OperatingCenters checkbox list
	And operating center "nj7" should be checked in the OperatingCenters checkbox list
	And operating center "ny1" should not be checked in the OperatingCenters checkbox list
	And operating center "ny2" should not be checked in the OperatingCenters checkbox list
	And operating center "ny3" should not be checked in the OperatingCenters checkbox list
	When I check state "ny" in the States checkbox list
	Then operating center "nj4" should be checked in the OperatingCenters checkbox list
	And operating center "nj7" should be checked in the OperatingCenters checkbox list
	And operating center "ny1" should be checked in the OperatingCenters checkbox list
	And operating center "ny2" should be checked in the OperatingCenters checkbox list
	And operating center "ny3" should not be checked in the OperatingCenters checkbox list
	When I uncheck state "nj" in the States checkbox list
	Then operating center "nj4" should not be checked in the OperatingCenters checkbox list
	And operating center "nj7" should not be checked in the OperatingCenters checkbox list
	And operating center "ny1" should be checked in the OperatingCenters checkbox list
	And operating center "ny2" should be checked in the OperatingCenters checkbox list
	And operating center "ny3" should not be checked in the OperatingCenters checkbox list

Scenario: The all operating center checkbox should be available
# The dialog is rendered through the same process that renders it for the User/Roles tab
# so we want to be sure this field is visible here.
	Given I am logged in as "site-admin"
	And I am at the Edit page for role group: "one"
	When I press "Add Roles"
	Then I should see the IsForAllOperatingCenters field

Scenario: Checking the "Select All Modules" checkbox should check all modules
	Given I am logged in as "site-admin"
	And I am at the Edit page for role group: "one"
	When I press "Add Roles"
	And I check the select-all-modules checkbox
	# this test creates a million modules, so it's not realistic to test *all* of them
	Then module "BPUGeneral" should be checked in the Modules checkbox list
	And module "OperationsHealthAndSafety" should be checked in the Modules checkbox list
	When I uncheck the select-all-modules checkbox
	Then module "BPUGeneral" should not be checked in the Modules checkbox list
	And module "OperationsHealthAndSafety" should not be checked in the Modules checkbox list

Scenario: The reset button should not erase checkbox values
# This can be tested by ensuring the selecting an operating center by state still works.
	Given I am logged in as "site-admin"
	And I am at the Edit page for role group: "one"
	When I press "Add Roles"
	And I press "Reset"
	And I check state "nj" in the States checkbox list
	Then operating center "nj7" should be checked in the OperatingCenters checkbox list

Scenario: Site admin should not be able to add duplicate roles 
# This is purely a front-end test to make sure that we aren't adding multiple roles.
# The dialog won't prevent users from doing it, but once they hit save in the dialog
# those dupes shouldn't be added to the display tables. Server-side validation will
# fail if dupes show up too.
	Given I am logged in as "site-admin"
	And I am at the Edit page for role group: "one"
	When I press "Add Roles"
	And I check operating center "nj7" in the OperatingCenters checkbox list
	And I check module "BPUGeneral" in the Modules checkbox list
	And I check role action "Read" in the Actions checkbox list 
	And I press save-roles-button
	And I press "Add Roles"
	And I press "Reset"
	And I check operating center "nj7" in the OperatingCenters checkbox list
	And I check module "BPUGeneral" in the Modules checkbox list
	And I check role action "Read" in the Actions checkbox list 
	And I press save-roles-button
	Then I should see the following values in the application-BPU table
	| Module     | State | Operating Center | Action |
	| BPUGeneral | NJ    | NJ7              | Read   |
	And the application-BPU table should have 1 rows

#
# REMOVING ROLE GROUP ROLES
#

Scenario: Site admin should be able to remove roles from a role group
	Given a role group role "role-to-keep" exists with action: "Read", module: "BPUGeneral", operating center: "nj7", role group: "one"
	And a role group role "role-to-remove" exists with action: "UserAdministrator", module: "BPUGeneral", operating center: "nj7", role group: "one"
	And I am logged in as "site-admin"
	And I am at the Edit page for role group: "one"
	When I press details-BPU
	And I click the checkbox named RolesToRemove with role group role "role-to-remove"'s Id
	And I click ok in the dialog after pressing "Save"
	Then I should be at the Show page for role group "one"
	And I should see the following values in the application-BPU table
	| Module     | State | Operating Center | Action            |
	| BPUGeneral | NJ    | NJ7              | Read              |
	And I should not see "UserAdministrator" in the application-BPU element

Scenario: Site admin should be able to remove newly added roles before saving 
# This is testing that if a user adds roles they ultimately don't want before saving,
# they can still remove them and the removed roles won't end up being saved.
	Given a role group role "role-to-keep" exists with action: "Read", module: "BPUGeneral", operating center: "nj7", role group: "one"
	And I am logged in as "site-admin"
	And I am at the Edit page for role group: "one"
	When I press "Add Roles"
	And I check operating center "nj7" in the OperatingCenters checkbox list
	And I check module "BPUGeneral" in the Modules checkbox list
	And I check role action "UserAdministrator" in the Actions checkbox list 
	And I press save-roles-button
	When I press details-BPU
	# This is fragile. It must select the "UserAdministrator" row checkbox
	And I click the "RolesToRemove" checkbox in the 2nd row of application-BPU
	And I press Save
	Then I should be at the Show page for role group "one"
	And I should see the following values in the application-BPU table
	| Module     | State | Operating Center | Action            |
	| BPUGeneral | NJ    | NJ7              | Read              |
	And I should not see "UserAdministrator" in the application-BPU element

Scenario: Site admin should be able use the "Select All" checkbox for an application section and see all the individual roles get selected
	Given a role group role "bpu-role" exists with action: "Read", module: "BPUGeneral", operating center: "nj7", role group: "one"
	And a role group role "business-performance-role" exists with action: "Read", module: "BusinessPerformanceGeneral", operating center: "nj7", role group: "one"
	And I am logged in as "site-admin"
	And I am at the Edit page for role group: "one"
	When I press details-BPU
	And I check the select-all-BPU checkbox
	Then the RolesToRemove checkbox for role group role "bpu-role" should be checked
	And the RolesToRemove checkbox for role group role "business-performance-role" should not be checked
		
#
# SAVING
#

Scenario: Site admin should not be prompted with a confirm dialog if they didn't make changes to roles
	Given I am logged in as "site-admin"
	And I am at the Edit page for role group: "one"
	When I press Save
	Then I should be at the Show page for role group "one"

Scenario: Site admin should not save record if they deny the save confirmation dialog
	Given I am logged in as "site-admin"
	And I am at the Edit page for role group: "one"
	When I press "Add Roles"
	And I check operating center "nj7" in the OperatingCenters checkbox list
	And I check module "BPUGeneral" in the Modules checkbox list
	And I check role action "UserAdministrator" in the Actions checkbox list 
	And I press save-roles-button
	And I click cancel in the dialog after pressing "Save"
	Then I should be at the Edit page for role group "one"

#
# VIEWING THE ROLE GROUP
#

Scenario: User admin should be able to see which roles the template has 
	Given a role group role "role-to-keep" exists with action: "Read", module: "BPUGeneral", operating center: "nj7", role group: "one"
	And a user "user-admin" exists with username: "user-admin", is user admin: "true"
	And I am logged in as "user-admin"
	And I am at the Show page for role group: "one"
	Then I should see the following values in the application-BPU table
	| Module     | State | Operating Center | Action            |
	| BPUGeneral | NJ    | NJ7              | Read              |

Scenario: User admin should be able to see which users belong to a role template
	Given a user "someuser" exists with username: "someuser"
	And a user "user-admin" exists with username: "user-admin", is user admin: "true"
	And user "someuser" has role group "one"
	And I am logged in as "user-admin"
	When I visit the Show page for role group: "one"
	And I click the "Users" tab
	Then I should see the following values in the users-table table 
	| Username |
	| someuser |