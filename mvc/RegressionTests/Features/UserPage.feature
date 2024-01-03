Feature: UserPage
	In order to edit CC info
	As a user
	I want to be edit my CC info
	
Background: 
	Given role actions exist
	And role applications exist
	And role modules exist
	And a state "nj" exists with name: "New Jersey", abbreviation: "NJ"	
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsburghy", state: "nj"
	And an operating center "nj4" exists with opcode: "NJ4", name: "Lakewood", state: "nj"
    And a user "user" exists with username: "user"
	And an admin user "site-admin" exists with username: "site-admin"
	And a user "user-admin" exists with username: "user-admin", is user admin: "true", is site admin: "false"
	And a role exists with user: "user-admin", action: "Read", module: "OperationsHealthAndSafety", operating center: "nj7"

Scenario: User Admin user can create a new user
	# MembershipHelper uses static methods that end up using an sql configuration
	# from web.config. That means the test ends up using the MapCallDev db rather
	# than the in-memory sqlite db. We can't reconfigure that during testing without
	# modifying the web.config or somehow creating a custom MembershipProvider just 
	# for this single test. It's not worth the effort.
    Given the test flag "do not create membership user" exists
	And a user type "whatever" exists
	And I am logged in as "user-admin"
	And I am at the User/New page
	When I enter "thistestuser" into the UserName field
	And I select user type "whatever" from the UserType dropdown
	And I enter "some@email.com" into the Email field
	And I enter "Some Guy" into the FullName field
	And I select operating center "nj7" from the DefaultOperatingCenter dropdown
	And I press Save
	Then I should see a display for UserName with "thistestuser"
	And I should see a display for Email with "some@email.com"
	And I should see a display for FullName with "Some Guy"
	And I should see a display for DefaultOperatingCenter with operating center "nj7"
	And I should see a display for UserType with user type "whatever"

Scenario: Site admin should see unlock and reset api password buttons
	Given I am logged in as "site-admin"
	And I am at the Show page for user "user"
	Then I should see the button "reset-api-password-button"
	And I should see the button "unlock-user-button"
	Given I am logged in as "user-admin"
	And I am at the Show page for user "user"
	Then I should not see the button "reset-api-password-button"
	And I should not see the button "unlock-user-button"
	Given I am logged in as "user"
	And I am at the Show page for user "user"
	Then I should not see the button "reset-api-password-button"
	And I should not see the button "unlock-user-button"

@cleanup_users
Scenario: user can save cc info for authorize.net
    Given a user "user-cc" exists with username: "user-cc", special_email: "user@mapcall.com", password: "testpassword", company: "company", has profileId: true, credit card number: "4012888818888"
    And I am logged in as "user-cc"
	When I visit the Show page for user: "user-cc"
    And I press "Update Payment Information"
    And I wait for element lnkPaymentAdd to exist in frame iframeAuthorizeNet
	And I follow "lnkPaymentAdd" by id in frame iframeAuthorizeNet
    And I wait for element cardNum to exist in frame iframeAuthorizeNet
    And I wait 1 seconds
    And I type "4111111111111111" into the cardNum field in frame iframeAuthorizeNet for authorizenet
	# Don't include the / in the expiration date, it's automatically added in auth.net's javascript 
    And I type "0129" into the expiryDate field in frame iframeAuthorizeNet for authorizenet
    And I type "123" into the cvv field in frame iframeAuthorizeNet for authorizenet
	And I type "456 Fake St" into the address field in frame iframeAuthorizeNet for authorizenet
	And I type "07711" into the zip field in frame iframeAuthorizeNet for authorizenet
	And I force press saveButton in frame iframeAuthorizeNet because it is impossible to scroll into view
	And I wait 2 seconds
	Then I should see "****1111" in the iframeAuthorizeNet frame
	
# ROLE STUFF

Scenario: Non-user-admin and non-site-admin user should not be able to see the tabs related to roles
	Given I am logged in as "user"
	When I visit the Show page for user: "user"
	Then I should not see the "Roles" tab
	And I should not see the "Role Groups" tab

Scenario: Non-user-admin and non-site-admin users should not be able to see user pages that aren't their own
	Given I am logged in as "user"
	When I try to visit the Show page for user: "user-admin" expecting an error 
	Then I should be at the Error/Forbidden screen

Scenario: A site admin can add and remove any roles for any module or operating center
	Given I am logged in as "site-admin"
	And I am at the Show page for user: "user"
	When I click the "Roles" tab
	And I press "Add Roles"
	And I check operating center "nj7" in the OperatingCenters checkbox list
	And I check operating center "nj4" in the OperatingCenters checkbox list
	And I check module "BPUGeneral" in the Modules checkbox list
	And I check module "ContractorsAgreements" in the Modules checkbox list
	And I check role action "Read" in the Actions checkbox list 
	And I check role action "Edit" in the Actions checkbox list 
	And I press save-roles-button
	And I click the "Roles" tab
	Then I should see the following values in the application-BPU table
	| Module     | State | Operating Center   | Action |
	| BPUGeneral | NJ    | NJ4 - Lakewood     | Read   |
	| BPUGeneral | NJ    | NJ4 - Lakewood     | Edit   |
	| BPUGeneral | NJ    | NJ7 - Shrewsburghy | Read   |
	| BPUGeneral | NJ    | NJ7 - Shrewsburghy | Edit   |
	And I should see the following values in the application-Contractors table
	| Module                | State | Operating Center   | Action |
	| ContractorsAgreements | NJ    | NJ4 - Lakewood     | Read   |
	| ContractorsAgreements | NJ    | NJ4 - Lakewood     | Edit   |
	| ContractorsAgreements | NJ    | NJ7 - Shrewsburghy | Read   |
	| ContractorsAgreements | NJ    | NJ7 - Shrewsburghy | Edit   |
	When I check the select-all-roles-checkbox checkbox
	And I press "Remove Selected Roles"
	And I click the "Roles" tab
	Then I should see the no-user-roles element

# This is more thoroughly tested in unit tests for the UserAdministrativeRoleAccess class.
Scenario: A user admin can only add and remove roles for modules and operating centers they have user admin for 
	Given a role "user-admin-role1" exists with action: "UserAdministrator", module: "BPUGeneral", operating center: "nj7", user: "user-admin"
	And a role "user-role1" exists with action: "Read", module: "ContractorsAgreements", operating center: "nj4", user: "user"
	And I am logged in as "user-admin"
	And I am at the Show page for user: "user"
	When I click the "Roles" tab
	And I press "Add Roles"
	Then I should not see module "ContractorsAgreements" in the Modules checkbox list
	Then I should see module "BPUGeneral" in the Modules checkbox list
	And I should not see operating center "nj4" in the OperatingCenters checkbox list
	When I check operating center "nj7" in the OperatingCenters checkbox list
	And I check module "BPUGeneral" in the Modules checkbox list
	And I check role action "Read" in the Actions checkbox list 
	And I check role action "Edit" in the Actions checkbox list 
	And I press save-roles-button
	And I click the "Roles" tab
	Then I should see the following values in the application-BPU table
	| Module     | State | Operating Center   | Action |
	| BPUGeneral | NJ    | NJ7 - Shrewsburghy | Read   |
	| BPUGeneral | NJ    | NJ7 - Shrewsburghy | Edit   |
	And I should see the following values in the application-Contractors table
	| Module                | State | Operating Center   | Action |
	| ContractorsAgreements | NJ    | NJ4 - Lakewood     | Read   |
	When I check the select-all-roles-checkbox checkbox
	And I press "Remove Selected Roles"
	And I click the "Roles" tab
	Then I should not see the application-BPU element
	And I should see the following values in the application-Contractors table
	| Module                | State | Operating Center   | Action |
	| ContractorsAgreements | NJ    | NJ4 - Lakewood     | Read   |

Scenario: A user admin should only see states they have operating center role access for
	Given a state "xx" exists with abbreviation: "xx"
	And a role exists with action: "UserAdministrator", module: "BPUGeneral", operating center: "nj7", user: "user-admin"
	And I am logged in as "user-admin"
	And I am at the Show page for user: "user"
	When I click the "Roles" tab
	And I press "Add Roles"
	Then I should see state "nj" in the States checkbox list
	And I should not see state "xx" in the States checkbox list

Scenario: Selecting a state will automatically select all of that state's operating centers that are not contracted operations
	Given a state "ny" exists with name: "New York", abbreviation: "NY"	
	And an operating center "ny1" exists with opcode: "NY1", name: "Some City 1", state: "ny"
	And an operating center "ny2" exists with opcode: "NY2", name: "Some Town 2", state: "ny"
	And an operating center "ny3" exists with opcode: "NY3", name: "Some Town 2", state: "ny", is contracted operations: "true"
	And I am logged in as "site-admin"
	And I am at the Show page for user: "user"
	When I click the "Roles" tab
	And I press "Add Roles"
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

Scenario: The all operating center checkbox should not be available if user is a site admin without a wildcard operating center role
	Given I am logged in as "user-admin"
	And I am at the Show page for user: "user"
	When I click the "Roles" tab
	And I press "Add Roles"
	Then I should not see the IsForAllOperatingCenters field

Scenario: The all operating center checkbox should be available if user is a user admin with a wildcard operating center role
	Given a wildcard role exists with action: "UserAdministrator", module: "BPUGeneral", user: "user-admin"
	And I am logged in as "user-admin"
	And I am at the Show page for user: "user"
	When I click the "Roles" tab
	And I press "Add Roles"
	Then I should see the IsForAllOperatingCenters field

Scenario: Checking the "Select All Modules" checkbox should check all modules
	Given I am logged in as "site-admin"
	And I am at the Show page for user: "user"
	When I click the "Roles" tab 
	And I press "Add Roles"
	And I check the select-all-modules checkbox
	# this test creates a million modules, so it's not realistic to test *all* of them
	Then module "BPUGeneral" should be checked in the Modules checkbox list
	And module "OperationsHealthAndSafety" should be checked in the Modules checkbox list
	When I uncheck the select-all-modules checkbox
	Then module "BPUGeneral" should not be checked in the Modules checkbox list
	And module "OperationsHealthAndSafety" should not be checked in the Modules checkbox list

Scenario: User should not see the "Remove Role" checkbox if they can not administrate that role
	Given a role "user-admin-role" exists with action: "UserAdministrator", module: "BPUGeneral", operating center: "nj7", user: "user-admin"
	And a role "role-can-remove" exists with action: "Read", module: "BPUGeneral", operating center: "nj7", user: "user"
	And a role "role-can-not-remove" exists with action: "Read", module: "ContractorsAgreements", operating center: "nj4", user: "user"
	And I am logged in as "user-admin"
	And I am at the Show page for user: "user"
	When I click the "Roles" tab
	Then I should see a checkbox named RolesToRemove with role "role-can-remove"'s Id
	And I should not see a checkbox named RolesToRemove with role "role-can-not-remove"'s Id

Scenario: User admin should see if a user's role comes from a role group or if it's a unique user role 
	Given a role group "group1" exists with name: "I'm a Role Group!"
	And a role group role exists with role group: "group1", action: "UserAdministrator", module: "BPUGeneral", operating center: "nj7"
	And a role "role-can-remove" exists with action: "Read", module: "BPUGeneral", operating center: "nj7", user: "user"
	And user "user" has role group "group1"
	And I am logged in as "user-admin"
	And I am at the Show page for user: "user"
	When I click the "Roles" tab
	Then I should see the following values in the application-BPU table
	| Module     | State | Operating Center   | Action            | Origin            |
	| BPUGeneral | NJ    | NJ7 - Shrewsburghy | UserAdministrator | I'm a Role Group! |
	| BPUGeneral | NJ    | NJ7 - Shrewsburghy | Read              |                   |
	# NOTE: No text is displayed in the Origin column when the role is unique to the user.

Scenario: User admin can add a role group to a user 
	Given a role group "group" exists with name: "Some Group"
	And a role group role exists with role group: "group", action: "Read", module: "BPUGeneral", operating center: "nj7"
	And a role exists with action: "UserAdministrator", module: "BPUGeneral", operating center: "nj7", user: "user-admin"
	And I am logged in as "user-admin"
	And I am at the Show page for user: "user"
	When I click the "Role Groups" tab
	And I press add-role-group-collapse
	And I select role group "group" from the RoleGroup dropdown
	And I press "Add Role Group"
	And I click the "Role Groups" tab
	Then I should see the following values in the role-groups table
	| Name       |
	| Some Group |

Scenario: User admin can remove a role group from a user
	Given a role group "group" exists with name: "Some Group"
	And user "user" has role group "group"
	And a role exists with action: "UserAdministrator", module: "BPUGeneral", operating center: "nj7", user: "user-admin"
	And I am logged in as "user-admin"
	And I am at the Show page for user: "user"
	When I click the "Role Groups" tab
	And I click the checkbox named RoleGroupsToRemove with role group "group"'s Id
	And I press "Remove Selected Groups"
	And I click the "Role Groups" tab
	Then I should see the no-user-role-groups element

Scenario: User admin should only see the checkbox to remove a role group when they have complete administrative access to the role group
	Given a role group "group-admin-can-access" exists
	And a role group role exists with role group: "group-admin-can-access", action: "Read", module: "BPUGeneral", operating center: "nj7"
	And user "user" has role group "group-admin-can-access"
	And a role group "group-no-access" exists
	And a role group role exists with role group: "group-no-access", action: "Read", module: "BPUGeneral", operating center: "nj4"
	And user "user" has role group "group-no-access"
	And a role exists with action: "UserAdministrator", module: "BPUGeneral", operating center: "nj7", user: "user-admin"
	And I am logged in as "user-admin"
	And I am at the Show page for user: "user"
	When I click the "Role Groups" tab
	Then I should see a checkbox named RoleGroupsToRemove with role group "group-admin-can-access"'s Id
	And I should not see a checkbox named RoleGroupsToRemove with role group "group-no-access"'s Id