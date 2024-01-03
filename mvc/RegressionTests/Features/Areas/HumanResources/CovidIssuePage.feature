Feature: CovidIssuePage

Background: users and supporting data exists
	Given a state "one" exists with name: "New Jersey", abbreviation: "NJ"
	And a state "two" exists with name: "New York", abbreviation: "NY"
	And an operating center "nj4" exists with opcode: "NJ4", name: "Lakewood", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true", state: "one"
	And an operating center "ny1" exists with opcode: "NY1", name: "Rockland, companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true", state: "two"
	And an employee status "active" exists with description: "Active"
	And a personnel area "one" exists
	And an employee "nj4" exists with operating center: "nj4", status: "active", personnel area: "one"
	And a user "nj4" exists with username: "nj4", employee: "nj4"
	And a role "nj4roleRead" exists with action: "Read", module: "HumanResourcesCovid", user: "nj4", operating center: "nj4"
	And a role "nj4roleEdit" exists with action: "Edit", module: "HumanResourcesCovid", user: "nj4", operating center: "nj4"
	And a role "nj4roleAdd" exists with action: "Add", module: "HumanResourcesCovid", user: "nj4", operating center: "nj4"
	And a role "nj4roleDelete" exists with action: "Delete", module: "HumanResourcesCovid", user: "nj4", operating center: "nj4"
	And an employee "ny1" exists with operating center: "ny1", status: "active", personnel area: "one"
	And a user "ny1" exists with username: "ny1", employee: "ny1"
	And a role "ny1roleRead" exists with action: "Read", module: "HumanResourcesCovid", user: "ny1", operating center: "ny1"
	And a role "ny1roleEdit" exists with action: "Edit", module: "HumanResourcesCovid", user: "ny1", operating center: "ny1"
	And a role "ny1roleAdd" exists with action: "Add", module: "HumanResourcesCovid", user: "ny1", operating center: "ny1"
	And a role "ny1roleDelete" exists with action: "Delete", module: "HumanResourcesCovid", user: "ny1", operating center: "ny1"
	And a covid request type "one" exists with description: "one"
	And a covid request type "two" exists with description: "two"
	And a covid submission status "one" exists with description: "NEW"
	And a covid submission status "two" exists with description: "IN PROGRESS"
	And a covid submission status "three" exists with description: "COMPLETE"
	And a covid outcome category "one" exists with description: "one"
	And a covid outcome category "two" exists with description: "two"
	And a release reason "one" exists with description: "one"
	And covid answer types exist

Scenario: user can view covid issue
	Given a covid issue "one" exists with employee: "nj4"
	And I am logged in as "nj4"
	When I visit the Show page for covid issue: "one"
	Then I should see a display for covid issue: "one"'s QuestionFromEmail
	And I should see a display for covid issue: "one"'s OutcomeDescription

Scenario: user cannot view a covid issue in another operating center
	Given a covid issue "nj4" exists with employee: "nj4"
	And a covid issue "ny1" exists with employee: "ny1"
	And I am logged in as "nj4"
	When I visit the Show page for covid issue: "nj4"
	Then I should see a display for covid issue: "nj4"'s OutcomeDescription
	When I try to visit the Show page for covid issue: "ny1" expecting an error
	Then I should see a 404 error message

Scenario: user can add an covid issue
	Given I am logged in as "nj4"
	When I visit the HumanResources/CovidIssue page
	And I follow "Add"
	And I press Save
	Then I should see the validation message "The State field is required."
	When I select state "one" from the State dropdown
	And I press Save
	Then I should see the validation message "The Employee field is required."
	And I should see the validation message "The RequestType field is required."
	And I should see the validation message "The SubmissionDate field is required."
	And I should see the validation message "The QuestionFromEmail field is required."
	And I should see the validation message "The SubmissionStatus field is required."
	When I select employee "nj4"'s Description from the Employee dropdown
	And I select covid request type "one" from the RequestType dropdown
	And I enter "2/4/2014" into the SubmissionDate field
	And I enter "question from email" into the QuestionFromEmail field
	And I select covid submission status "three" from the SubmissionStatus dropdown
	And I press Save
	Then I should see the validation message "The OutcomeDescription field is required."
	And I should see the validation message "The OutcomeCategory field is required."
	When I enter "outcome description" into the OutcomeDescription field
	And I select covid outcome category "one" from the OutcomeCategory dropdown
	And I enter "1234567" into the SupervisorsCell field
	And I enter "1233456" into the LocalEmployeeRelationsBusinessPartnerCell field
	And I press Save
	Then I should see a validation message for SupervisorsCell with "Please adjust your number to match one of the following formats: (123)123-1234 or (123)123-1234x1234"
	And I should see a validation message for LocalEmployeeRelationsBusinessPartnerCell with "Please adjust your number to match one of the following formats: (123)123-1234 or (123)123-1234x1234"
	When I enter "(123)123-4567" into the SupervisorsCell field
	And I enter "(123)123-4567x12345" into the LocalEmployeeRelationsBusinessPartnerCell field
	And I enter "2/4/2014" into the ReleaseDate field
	And I press Save
	Then I should see a validation message for ReleaseReason with "The ReleaseReason field is required."
	When I select release reason "one" from the ReleaseReason dropdown
	Then "TBD" should be selected in the WorkExposure dropdown
	And "TBD" should be selected in the AvoidableCloseContact dropdown
	And "TBD" should be selected in the FaceCoveringWorn dropdown
	When I select "No" from the FaceCoveringWorn dropdown
	And I select "Yes" from the WorkExposure dropdown
	And I press "Save"
	Then I should see a display for "Employee" with employee: "nj4"
	And I should see a display for RequestType with covid request type "one"
	And I should see a display for SubmissionDate with "2/4/2014"	
	And I should see a display for QuestionFromEmail with "question from email"
	And I should see a display for SubmissionStatus with covid submission status "three"
	And I should see a display for OutcomeDescription with "outcome description"
	And I should see a display for OutcomeCategory with covid outcome category "one"
	And I should see a display for ReleaseDate with "2/4/2014"
	And I should see a display for ReleaseReason with release reason "one"
	And I should see a display for WorkExposure with "Yes"
	And I should see a display for AvoidableCloseContact with "TBD"
	And I should see a display for FaceCoveringWorn with "No"

Scenario: user can edit an environmental permit
	Given a covid issue "one" exists with employee: "ny1"
	And I am logged in as "ny1"
	When I visit the Edit page for covid issue: "one"
	Then I should see a display for Employee with employee "ny1"
	When I select covid request type "two" from the RequestType dropdown
	And I enter "3/4/2014" into the SubmissionDate field
	And I enter "question from email changed" into the QuestionFromEmail field
	And I select covid submission status "two" from the SubmissionStatus dropdown
	And I enter "outcome description changed" into the OutcomeDescription field
	And I select covid outcome category "two" from the OutcomeCategory dropdown
	And I enter "(123)123-0123" into the SupervisorsCell field
	And I enter "lerbp" into the LocalEmployeeRelationsBusinessPartner field
	And I enter "(123)123-0123x1234" into the LocalEmployeeRelationsBusinessPartnerCell field
	And I enter "3/5/2020" into the StartDate field
	And I enter "3/6/2020" into the ReleaseDate field
	And I enter "quarantine reason" into the QuarantineReason field
	And I select release reason "one" from the ReleaseReason dropdown
	And I select "Yes" from the WorkExposure dropdown
	And I select "No" from the AvoidableCloseContact dropdown
	And I select "No" from the FaceCoveringWorn dropdown
	And I press Save
	And I wait for the page to reload
	Then I should see a display for Employee_OperatingCenter_State with state "two"
	And I should see a display for Employee_OperatingCenter with operating center "ny1"
	And I should see a display for Employee with employee "ny1"
	And I should see a display for RequestType with covid request type "two"
	And I should see a display for SubmissionDate with "3/4/2014"	
	And I should see a display for QuestionFromEmail with "question from email changed"
	And I should see a display for SubmissionStatus with covid submission status "two"
	And I should see a display for OutcomeDescription with "outcome description changed"
	And I should see a display for OutcomeCategory with covid outcome category "two"
	And I should see a display for SupervisorsCell with "(123)123-0123"
	And I should see a display for LocalEmployeeRelationsBusinessPartner with "lerbp"
	And I should see a display for LocalEmployeeRelationsBusinessPartnerCell with "(123)123-0123x1234"
	And I should see a display for StartDate with "3/5/2020"
	And I should see a display for ReleaseDate with "3/6/2020"
	And I should see a display for QuarantineReason with "quarantine reason"
	And I should see a display for WorkExposure with "Yes"
	And I should see a display for AvoidableCloseContact with "No"
	And I should see a display for FaceCoveringWorn with "No"

Scenario: user can destroy an environmental permit
	Given a covid issue "one" exists with employee: "ny1"
	And I am logged in as "ny1"
	When I visit the Show page for covid issue: "one"
	And I click ok in the dialog after pressing "Delete"
	Then I should be at the HumanResources/CovidIssue/Search page
	When I try to access the Show page for covid issue: "one" expecting an error
	Then I should see a 404 error message

Scenario: A user can edit any of the multiline text fields with values that look like html and not create a potential XSS attack
	Given I am logged in as "nj4"
	When I visit the HumanResources/CovidIssue page
	And I follow "Add"
	And I press Save
	Then I should see the validation message "The State field is required."
	When I select state "one" from the State dropdown
	And I select employee "nj4"'s Description from the Employee dropdown
	And I select covid request type "one" from the RequestType dropdown
	And I enter "2/4/2014" into the SubmissionDate field
	And I select covid submission status "three" from the SubmissionStatus dropdown
	When I enter "outcome description" into the OutcomeDescription field
	And I select covid outcome category "one" from the OutcomeCategory dropdown
	And I enter "<no> <xss> <for> </me!>" into the QuestionFromEmail field
	And I enter "me <either></either>&nbsp;" into the QuarantineReason field
	And I select "Yes" from the WorkExposure dropdown
	And I select "No" from the AvoidableCloseContact dropdown
	And I select "No" from the FaceCoveringWorn dropdown
	And I press Save
	Then I should see a display for "Employee" with employee: "nj4"
	And I should see a display for QuestionFromEmail with "<no> <xss> <for> </me!>"
	And I should see a display for QuarantineReason with "me <either></either>&nbsp;"
	When I follow "Edit"
	Then I should see "<no> <xss> <for> </me!>" in the QuestionFromEmail field
	Then I should see "me <either></either>&nbsp;" in the QuarantineReason field
