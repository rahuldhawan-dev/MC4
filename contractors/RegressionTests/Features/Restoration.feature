Feature: Restoration

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

Scenario: can view a restoration
	Given a finalization work order "one" exists with valve: "one", contractor: "one"
	And a restoration type "one" exists with description: "ASPHALT-STREET"
	And a restoration "one" exists with finalization work order: "one", paving square footage: "52.00", restoration type: "one", assigned contractor: "one", initial purchase order number: "123456789"
	And I am logged in as "user@site.com", password: "testpassword#1"
	When I visit the show page for restoration: "one"
	Then I should see a display for InitialPurchaseOrderNumber with "123456789"
