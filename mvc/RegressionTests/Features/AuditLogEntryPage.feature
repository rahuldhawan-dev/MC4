Feature: AuditLogEntryPage
	Spray Anything Exterminators
	Get a loom you two! Couples Weaving
	Kale Mary Burger

Background: 
    Given an admin user "admin" exists with username: "admin"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"

Scenario: admin user can page through results in an audit log tab
	# This test exists to ensure that the urls generated for paging don't get messed up if the search 
	# model gets changed. -Ross 8/16/2018
	# TODO: make a step for creating audit log entries directly: and X amount of audit log entries exist for object: key with key: "value" perhaps
	Given a bacterial sample type "routine" exists with description: "Routine"
	And a bacterial sample type "repeat" exists with description: "Repeat"
	And a bacterial sample type "shipping blank" exists with description: "Shipping Blank"
	And a town "one" exists with name: "Townie"
	And operating center: "nj7" exists in town: "one" with abbreviation: "LA"
	And a public water supply "one" exists with op code: "NJ7" 
	And a sample site status "active" exists with description: "Active"
	And a sample site "one" exists with town: "one", sample site name: "Grateful Deli", public water supply: "one", sample site status: "active", bacti site: "true", operating center: "nj7"
    And a bacterial water sample "one" exists with sample site: "one", operating center: "nj7"
	And an audit log entry exists with entity name: "BacterialWaterSample", entity id: "1", field name: "Entry 1", timestamp: "8/16/2118 12:59:00 PM"
	And an audit log entry exists with entity name: "BacterialWaterSample", entity id: "1", field name: "Entry 2", timestamp: "8/16/2118 12:58:00 PM"
	And an audit log entry exists with entity name: "BacterialWaterSample", entity id: "1", field name: "Entry 3", timestamp: "8/16/2118 12:57:00 PM"
	And an audit log entry exists with entity name: "BacterialWaterSample", entity id: "1", field name: "Entry 4", timestamp: "8/16/2118 12:56:00 PM"
	And an audit log entry exists with entity name: "BacterialWaterSample", entity id: "1", field name: "Entry 5", timestamp: "8/16/2118 12:55:00 PM"
	And an audit log entry exists with entity name: "BacterialWaterSample", entity id: "1", field name: "Entry 6", timestamp: "8/16/2118 12:54:00 PM"
	And an audit log entry exists with entity name: "BacterialWaterSample", entity id: "1", field name: "Entry 7", timestamp: "8/16/2118 12:53:00 PM"
	And an audit log entry exists with entity name: "BacterialWaterSample", entity id: "1", field name: "Entry 8", timestamp: "8/16/2118 12:52:00 PM"
	And an audit log entry exists with entity name: "BacterialWaterSample", entity id: "1", field name: "Entry 9", timestamp: "8/16/2118 12:51:00 PM"
	And an audit log entry exists with entity name: "BacterialWaterSample", entity id: "1", field name: "Entry 10", timestamp: "8/16/2118 12:50:00 PM"
	And an audit log entry exists with entity name: "BacterialWaterSample", entity id: "1", field name: "Entry 11", timestamp: "8/16/2118 12:49:00 PM"
	And an audit log entry exists with entity name: "BacterialWaterSample", entity id: "1", field name: "Entry 12", timestamp: "8/16/2118 12:48:00 PM"
	And I am logged in as "admin"
	When I visit the Show page for bacterial water sample: "one"
	And I click the "Log" tab
	And I wait for ajax to finish loading
	Then I should see the following values in the auditLogEntryTable table 
	| Entity Name          | Field Name | Time Stamp (EST)            |
	| BacterialWaterSample | Entry 1    | 8/16/2118 12:59:00 PM (EST) |
	| BacterialWaterSample | Entry 2    | 8/16/2118 12:58:00 PM (EST) |
	| BacterialWaterSample | Entry 3    | 8/16/2118 12:57:00 PM (EST) |
	| BacterialWaterSample | Entry 4    | 8/16/2118 12:56:00 PM (EST) |
	| BacterialWaterSample | Entry 5    | 8/16/2118 12:55:00 PM (EST) |
	| BacterialWaterSample | Entry 6    | 8/16/2118 12:54:00 PM (EST) |
	| BacterialWaterSample | Entry 7    | 8/16/2118 12:53:00 PM (EST) |
	| BacterialWaterSample | Entry 8    | 8/16/2118 12:52:00 PM (EST) |
	| BacterialWaterSample | Entry 9    | 8/16/2118 12:51:00 PM (EST) |
	| BacterialWaterSample | Entry 10   | 8/16/2118 12:50:00 PM (EST) |
	When I follow "2" 
	And I wait for ajax to finish loading
	Then I should see the following values in the auditLogEntryTable table 
	| Entity Name          | Field Name | Time Stamp (EST)             |
	| BacterialWaterSample | Entry 11    | 8/16/2118 12:49:00 PM (EST) |
	| BacterialWaterSample | Entry 12    | 8/16/2118 12:48:00 PM (EST) |
	When I follow "<<" 
	And I wait for ajax to finish loading
	Then I should see the following values in the auditLogEntryTable table 
	| Entity Name          | Field Name | Time Stamp (EST)            |
	| BacterialWaterSample | Entry 1    | 8/16/2118 12:59:00 PM (EST) |
	| BacterialWaterSample | Entry 2    | 8/16/2118 12:58:00 PM (EST) |
	| BacterialWaterSample | Entry 3    | 8/16/2118 12:57:00 PM (EST) |
	| BacterialWaterSample | Entry 4    | 8/16/2118 12:56:00 PM (EST) |
	| BacterialWaterSample | Entry 5    | 8/16/2118 12:55:00 PM (EST) |
	| BacterialWaterSample | Entry 6    | 8/16/2118 12:54:00 PM (EST) |
	| BacterialWaterSample | Entry 7    | 8/16/2118 12:53:00 PM (EST) |
	| BacterialWaterSample | Entry 8    | 8/16/2118 12:52:00 PM (EST) |
	| BacterialWaterSample | Entry 9    | 8/16/2118 12:51:00 PM (EST) |
	| BacterialWaterSample | Entry 10   | 8/16/2118 12:50:00 PM (EST) |

Scenario: User can see contractor log results
	Given a bacterial sample type "routine" exists with description: "Routine"
	And a bacterial sample type "repeat" exists with description: "Repeat"
	And a bacterial sample type "shipping blank" exists with description: "Shipping Blank"
	And a town "one" exists with name: "Townie"
	And a user "testuser" exists with username: "stuff", default operating center: "nj7", full name: "Smith"
	And a contractor "nj7" exists with framework operating center: "nj7", operating center: "nj7", name: "Kevin"
	And a contractor user "user" exists with email: "user@site.com", contractor: "nj7"
	And operating center: "nj7" exists in town: "one" with abbreviation: "LA"
	And a public water supply "one" exists with op code: "NJ7"
	And a sample site status "active" exists with description: "Active"
	And a sample site "one" exists with town: "one", sample site name: "Grateful Deli", public water supply: "one", sample site status: "active", bacti site: "true", operating center: "nj7"
	And a bacterial water sample "one" exists with sample site: "one", operating center: "nj7"
	And an audit log entry exists with entity name: "BacterialWaterSample", entity id: "1", field name: "Entry 1", timestamp: "8/16/2118 12:59:00 PM", ContractorUser: "user", User: null
	And an audit log entry exists with entity name: "BacterialWaterSample", entity id: "1", field name: "Entry 2", timestamp: "8/16/2118 12:58:00 PM", ContractorUser: "user", User: null
	And an audit log entry exists with entity name: "BacterialWaterSample", entity id: "1", field name: "Entry 3", timestamp: "8/16/2118 12:57:00 PM", User: "testuser"
	And an audit log entry exists with entity name: "BacterialWaterSample", entity id: "1", field name: "Entry 4", timestamp: "8/16/2118 12:56:00 PM", ContractorUser: "user", User: null
	And an audit log entry exists with entity name: "BacterialWaterSample", entity id: "1", field name: "Entry 5", timestamp: "8/16/2118 12:55:00 PM", ContractorUser: "user", User: null
	And an audit log entry exists with entity name: "BacterialWaterSample", entity id: "1", field name: "Entry 6", timestamp: "8/16/2118 12:54:00 PM", User: "testuser"
	And I am logged in as "admin" 
	When I visit the show page for bacterial water sample "one"
	And I click the "Log" tab
	And I wait for ajax to finish loading
	Then I should see the following values in the auditLogEntryTable table
	| Entity Name          | Field Name | Time Stamp (EST)            | Contractor            | User  |
	| BacterialWaterSample | Entry 1    | 8/16/2118 12:59:00 PM (EST) | Kevin - user@site.com |       |
	| BacterialWaterSample | Entry 2    | 8/16/2118 12:58:00 PM (EST) | Kevin - user@site.com |       |
	| BacterialWaterSample | Entry 3    | 8/16/2118 12:57:00 PM (EST) |                       | Smith |
	| BacterialWaterSample | Entry 4    | 8/16/2118 12:56:00 PM (EST) | Kevin - user@site.com |       |
	| BacterialWaterSample | Entry 5    | 8/16/2118 12:55:00 PM (EST) | Kevin - user@site.com |       |
	| BacterialWaterSample | Entry 6    | 8/16/2118 12:54:00 PM (EST) |                       | Smith |