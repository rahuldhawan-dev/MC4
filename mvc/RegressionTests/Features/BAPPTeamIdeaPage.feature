Feature: BAPP Team Idea Page
    You know how this goes

Background: necessary data
    Given an admin user "admin" exists with username: "admin"
    And a user "dummy" exists
    And an operating center "nj7" exists with opcode: "nj7"
    And an operating center "nj4" exists with opcode: "nj4"
    And an operating center "no-teams" exists with opcode: "nt1"
    And a bapp team "one" exists with description: "this is some team or another", operating center: "nj7"
    And a bapp team "two" exists with description: "this team is also a team", operating center: "nj4"
    And an employee "vOv" exists
    And an employee "D:" exists
    And a safety implementation category "one" exists with description: "this is some category or another"
    And a safety implementation category "two" exists with description: "this category is here too"
    And a bapp team idea "one" exists with description: "this is the default", bapp team: "one", contact: "vOv", safety implementation category: "one"
    And a bapp team idea "two" exists with description: "this is the default", bapp team: "two", contact: "D:", safety implementation category: "two"
    And a bapp team idea "three" exists with description: "this is the default", bapp team: "one", contact: "vOv", safety implementation category: "one"
    And a bapp team idea "four" exists with description: "this is the default", bapp team: "two", contact: "D:", safety implementation category: "two"

Scenario: admin can search for bapp team ideas like you wouldn't believe
    Given I am logged in as "admin"
    When I search for BappTeamIdeas with no conditions chosen
    Then I should be at the BappTeamIdea page
    And I should see a link to the Show page for bapp team idea "one"		
    And I should see a link to the Show page for bapp team idea "two"		
    And I should see a link to the Show page for bapp team idea "three"		
    And I should see a link to the Show page for bapp team idea "four"
    When I search for BappTeamIdeas with operating center: "nj7" chosen
    Then I should be at the BappTeamIdea page
    And I should see a link to the Show page for bapp team idea "one"		
    And I should not see a link to the Show page for bapp team idea "two"		
    And I should see a link to the Show page for bapp team idea "three"		
    And I should not see a link to the Show page for bapp team idea "four"
    When I search for BappTeamIdeas with contact: "D:" chosen
    Then I should be at the BappTeamIdea page
    And I should not see a link to the Show page for bapp team idea "one"		
    And I should see a link to the Show page for bapp team idea "two"		
    And I should not see a link to the Show page for bapp team idea "three"		
    And I should see a link to the Show page for bapp team idea "four"
    When I search for BappTeamIdeas with safety implementation category: "one" chosen
    Then I should be at the BappTeamIdea page
    And I should see a link to the Show page for bapp team idea "one"		
    And I should not see a link to the Show page for bapp team idea "two"		
    And I should see a link to the Show page for bapp team idea "three"		
    And I should not see a link to the Show page for bapp team idea "four"

Scenario: admin can add a bapp team idea if they feel like it
    Given I am logged in as "admin"
    When I visit the /BappTeamIdea/New page
    And I press Save
    Then I should be at the BappTeamIdea/New page
    And I should not see operating center "no-teams" in the OperatingCenter dropdown
    And I should see the validation message "The OperatingCenter field is required."
	And I should see the validation message "The Contact field is required."
    And I should see the validation message "The SafetyImplementationCategory field is required."
    And I should see the validation message "The Description field is required."
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I select employee "vOv"'s Description from the Contact dropdown
    And I select safety implementation category "one" from the SafetyImplementationCategory dropdown
    And I enter "Some kind of idea" into the Description field
    And I press Save
    Then I should be at the BappTeamIdea/New page
    And I should see the validation message "The BappTeam field is required."
    When I wait for ajax to finish loading
    And I select bapp team "one" from the BappTeam dropdown
    And I press Save
    And I wait for the page to reload
	Then the currently shown bapp team idea will now be referred to as "new idea"
    And I should be at the Show page for bapp team idea: "new idea"

Scenario: what's that?  oh that's just an admin editing a bapp team idea
    Given I am logged in as "admin"
    When I visit the Edit page for bapp team idea: "one"
    And I enter "this would never be the default" into the Description field
    And I press Save
    And I wait for the page to reload
    Then I should be at the Show page for bapp team idea: "one"
    And I should see a display for Description with "this would never be the default"

Scenario: unauthorized user can't just come in here and start messing with bapp team ideas.  didn't your mama teach you better than that, unauthorized user?