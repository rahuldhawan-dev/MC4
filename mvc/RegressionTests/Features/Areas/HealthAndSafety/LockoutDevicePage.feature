Feature: LockoutDevicePage
	The Miceman cometh
	Adios mice-chachos
	Hugs not bugs

Background: users exist and such
	Given an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsburghy"
	And an operating center "nj4" exists with opcode: "NJ4", name: "Lakewood"
	And a lockout device color "red" exists with description: "red"
	And a lockout device color "blue" exists with description: "blue"
	And a role "roleRead" exists with action: "Read", module: "OperationsHealthAndSafety", operating center: "nj7"
	And a role "roleLockoutFormNj7" exists with action: "Read", module: "OperationsLockoutForms", operating center: "nj7"
	And a role "roleLockoutFormNj4" exists with action: "Read", module: "OperationsLockoutForms", operating center: "nj4"
	And a user "user" exists with username: "user", full name: "Foo", roles: roleRead;roleLockoutFormNj7;roleLockoutFormNj4
	
Scenario: user with role can view a device
	Given a lockout device "one" exists with operating center: "nj7"
	And I am logged in as "user"
	When I visit the Show page for lockout device: "one"
	Then I should see a display for "Description" with lockout device: "one"'s Description

Scenario: admin can add, edit, destroy a device
	Given an admin user "admin" exists with username: "admin"
	And a user "nj4" exists with username: "nj4", default operating center: "nj4", full name: "Bar"
	And I am logged in as "admin"
	When I visit the HealthAndSafety/LockoutDevice/New page
	And I select "NJ" from the State dropdown
	And I press Save
	Then I should see the validation message "The OperatingCenter field is required."
	And I should see the validation message "The Description field is required."
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I wait for ajax to finish loading
	And I enter "foo" into the Description field
	And I press Save
	Then I should see the validation message "The Person field is required."
	When I select user "user"'s FullName from the Person dropdown
	And I press Save
	Then the currently shown lockout device shall henceforth be known throughout the land as "one"
	And I should be at the Show page for lockout device: "one"
	And I should see a display for OperatingCenter with operating center "nj7"'s Description
	And I should see a display for Person_FullName with user "user"'s FullName
	And I should see a display for Description with "foo"
	When I follow "Edit"
	And I wait for the page to reload
	Then I should see a display for Person_FullName with user "user"'s FullName
	When I select "NJ" from the State dropdown
	And I select operating center "nj4" from the OperatingCenter dropdown
	And I wait for ajax to finish loading
	#Person drop down is no longer there, Person's FullName is display-only on Edit page
	And I select lockout device color "red" from the LockoutDeviceColor dropdown
	And I enter "bar" into the Description field
	And I enter "1234" into the SerialNumber field
	And I press Save
	Then I should see a display for OperatingCenter with operating center "nj4"'s Description
	And I should see a display for Person_FullName with user "user"'s FullName
	And I should see a display for LockoutDeviceColor with lockout device color "red"'s Description
	And I should see a display for SerialNumber with "1234"
	And I should see a display for Description with "bar"
	When I click ok in the dialog after pressing "Delete"
	Then I should be at the HealthAndSafety/LockoutDevice/Search page
	When I try to access the Show page for lockout device: "one" expecting an error
	Then I should see a 404 error message

Scenario: admin can search lockout devices
	Given an admin user "admin" exists with username: "admin"
	And a user "nj4" exists with username: "nj4", default operating center: "nj7", full name: "Boaty McBoatFace the third", roles: roleLockoutFormNj7
	And a lockout device "one" exists with operating center: "nj7", person: "nj4"
	And a lockout device "two" exists with operating center: "nj4", person: "nj4"
	And I am logged in as "admin"
	When I visit the HealthAndSafety/LockoutDevice/Search page
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select user "nj4"'s FullName from the Person dropdown
	And I press Search
	Then I should see a link to the show page for lockout device "one"
	And I should not see a link to the show page for lockout device "two"
	When I visit the HealthAndSafety/LockoutDevice/Search page
	And I press Search
	Then I should see a link to the show page for lockout device "one"
	And I should see a link to the show page for lockout device "two"

Scenario: user can search lockout devices
	Given a user "nj4" exists with username: "nj4", default operating center: "nj7", full name: "Boaty McBoatFace the third", roles: roleLockoutFormNj7
	And a lockout device "one" exists with operating center: "nj7", person: "nj4"
	And a lockout device "two" exists with operating center: "nj4", person: "nj4"
	And a user "basicuser" exists with username: "basicuser", full name: "Foo", roles: roleRead;roleLockoutFormNj7;
	And I am logged in as "basicuser"
	When I visit the HealthAndSafety/LockoutDevice/Search page
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select user "nj4"'s FullName from the Person dropdown
	And I press Search
	Then I should see a link to the show page for lockout device "one"
	And I should not see a link to the show page for lockout device "two"
	When I visit the HealthAndSafety/LockoutDevice/Search page
	And I press Search
	Then I should see a link to the show page for lockout device "one"
	#We don't appear to be filtering the search by user role here
	And I should see a link to the show page for lockout device "two"