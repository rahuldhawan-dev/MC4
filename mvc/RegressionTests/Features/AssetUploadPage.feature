Feature: AssetUploadPage
	In order to new assets to mapcall's database
	As a user
	I want to be able to upload and manage asset files

Background:
	Given an admin user "user" exists with username: "user"
    And asset upload statuses exist

Scenario: user can upload an asset file to be imported
    Given I do not currently function
    # this test is flaky as of 2021-02-03
	#Given I am logged in as "user"
    #When I visit the /AssetUpload/New page
    #And I upload "hydrantSAPUpload.xlsx"
    #And I press Save
    #Then the currently shown asset upload shall henceforth be known throughout the land as "meh"
