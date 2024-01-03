Feature: Work order read only page

Background: stuff exists
	Given a contractor "one" exists with name: "one"
    And a user exists with email: "user@site.com", password: "testpassword#1", contractor: "one"
    And an operating center "one" exists with opcntr: "NJX", opcntrname: "Shrewsburgh"
	And an asset type "valve" exists with description: "valve", operating center: "one"
	And a valve "one" exists
	And a read only work order "one" exists with asset type: "valve", contractor: "one", date completed: "4/23/1984", date started: "4/25/1984", markout requirement: "none", valve: "one"
	And a markout "current" exists with expiration date: "tomorrow", markout number: "4", read only work order: "one", date of request: "4/26/1984 4:04:00 AM", ready date: "4/27/1984", date completed: "5/1/1984"
	And a markout "last" exists with expiration date: "today", markout number: "3", read only work order: "one", date of request: "4/25/1984", ready date: "4/27/1984"
	
Scenario: user tries to view a work order that does not exist or is otherwise unreachable
    Given a contractor "bad contractor" exists with name: "bad contractor"
	And a read only work order "bad" exists with street number: "Bad Number", contractor: "bad contractor"
	And I am logged in as "user@site.com", password: "testpassword#1"
	When I visit the show page for read only work order: "bad"
	Then I should see "Work Order Does Not Exist" in the heading element

Scenario: user tries to view a work order that they can see
    Given I am logged in as "user@site.com", password: "testpassword#1"
	When I visit the show page for read only work order: "one"
	Then I should only see read only work order "one"'s AccountCharged in the AccountCharged element
	And I should only see valve "one"'s ValveNumber in the AssetID element
	And I should only see read only work order "one"'s DateCompleted in the DateCompleted element as a date
	And I should only see read only work order "one"'s DateReceived in the DateReceived element as a date
	And I should only see read only work order "one"'s DateStarted in the DateStarted element as a date
	And I should only see read only work order "one"'s ExcavationDate in the ExcavationDate element as a date
	And I should only see read only work order "one"'s LostWater in the LostWater element
	And I should only see read only work order "one"'s Notes in the Notes element
	And I should only see read only work order "one"'s ORCOMServiceOrderNumber in the ORCOMServiceOrderNumber element
	And I should only see read only work order "one"'s PhoneNumber in the PhoneNumber element
	And I should only see read only work order "one"'s PremiseNumber in the PremiseNumber element
	And I should only see read only work order "one"'s StreetAddress in the StreetAddress element
	And I should only see read only work order "one"'s TownAddress in the TownAddress element
	And I should only see read only work order "one"'s Id in the WorkOrderID element
	And I should only see read only work order "one"'s WorkDescription in the WorkDescription element
	And I should only see markout "current"'s MarkoutNumber in the CurrentMarkoutNumber element
	And I should only see "4/26/1984" in the CurrentMarkoutDateOfRequest element
	And I should only see "4/27/1984 12:00:00 AM" in the CurrentMarkoutReadyDate element
	And I should only see "04:04:00" in the CurrentMarkoutTimeOfDay element
	And I should only see markout "last"'s MarkoutNumber in the LastMarkoutNumber element
	And I should only see "None" in the MarkoutRequirement element
