Feature: RedTagPermitFeature

Background: data exists
    Given a state "NJ" exists with abbreviation: "NJ"
    And a town "Swedesboro" exists with state: "NJ"
    And an operating center "oc" exists with opcode: "NJ7", name: "Shrewsbury", state: "NJ"
    And operating center: "oc" exists in town: "Swedesboro"
    And a facility "f" exists with operating center: "oc", town: "Swedesboro", facility name: "House of Panda"
    And an employee status "active" exists with description: "Active"
    And an employee "employee" exists with operating center: "oc", status: "active"
    And an equipment type "fire suppression" exists with description: "fire suppression"
    And an equipment manufacturer "fire suppression" exists with equipment type: "fire suppression", description: "fire suppression"
    And an equipment model "equipment-model" exists with equipment manufacturer: "fire suppression", description: "equipment model"
    And an equipment "e" exists with facility: "f", equipment type: "fire suppression", description: "fire suppression", equipment manufacturer: "fire suppression", equipment model: "equipment-model", serial number: "sn-01"
    And production prerequisites exist
	And a production work description "pwd" exists with equipment type: "fire suppression", description: "fix the sprinklers"
	And a production work order "pwo" exists with production work description: "pwd", equipment: "e", operating center: "oc", facility: "f", date completed: "02/10/2020 12:00"
    And a production work order "pwo-for-edit" exists with production work description: "pwd", equipment: "e", operating center: "oc", facility: "f", date completed: "02/10/2020 12:00", needs red tag permit authorization: "true"
    And a production work order production prerequisite "one" exists with production work order: "pwo-for-edit", production prerequisite: "red tag permit"	
    And a red tag permit protection type "Sprinkler System" exists with description: "Sprinkler System"
    And a red tag permit protection type "Fire Pump" exists with description: "Fire Pump"
    And a red tag permit protection type "Special Protection" exists with description: "Special Protection"
    And a red tag permit protection type "Other" exists with description: "Other"
	And a user "user-unauthorized" exists with username: "user-unauthorized"
    And a user "user-admin" exists with username: "user-admin", employee: "employee"
    And a role "role-admin" exists with action: "UserAdministrator", module: "ProductionWorkManagement", user: "user-admin", operating center: "oc"
    And a role "role-production-work-management" exists with action: "UserAdministrator", module: "ProductionWorkManagement", user: "user-admin", operating center: "oc"
    And a role "role-production-good cequipment" exists with action: "UserAdministrator", module: "ProductionEquipment", user: "user-admin", operating center: "oc"
    And a red tag permit "rtp" exists with operating center: "oc", facility: "f", equipment: "e", production work order: "pwo-for-edit", person responsible: "employee", protection type: "Fire Pump", area protected: "area-protected-edit-text", number of turns to close: "12", authorized by: "employee", fire protection equipment operator: "fire-protection-equipment-operating-edit-text", equipment impaired on: "4/21/2021 12:13:48 PM", emergency organization notified: true, has other precaution: true, other precaution description: "other-precaution-description-edit-text", reason for impairment: "reason-for-impairment-edit-text"

Scenario: Unauthorized users cannot access any pages
    Given I am logged in as "user-unauthorized"
    And I am at the HealthAndSafety/RedTagPermit/Search page
    Then I should see "You do not have the roles necessary to access this resource."
    When I try to visit the Show page for red tag permit: "rtp"
    Then I should see "You do not have the roles necessary to access this resource."
    When I try to visit the Edit page for red tag permit: "rtp"
    Then I should see "You do not have the roles necessary to access this resource."

Scenario: User sees 404 Not Found response when visiting New without required parameters
    Given I am logged in as "user-admin"
    When I visit the HealthAndSafety/RedTagPermit/New page
    Then I should see a 404 error message

Scenario: User can add a red tag permit
    Given I am logged in as "user-admin"
    When I go to the new page for a red tag permit with production work order: "pwo", operating center: "oc", equipment: "e"
    Then I should see a display for ProductionWorkOrderDisplay_ProductionWorkDescription with production work order "pwo"'s ProductionWorkDescription
    And I should see a display for ProductionWorkOrderDisplay_OperatingCenter_State with operating center "oc"'s State
    And I should see a display for ProductionWorkOrderDisplay_OperatingCenter with operating center "oc"
    And I should see a display for ProductionWorkOrderDisplay_Facility with facility "f"'s Description
    And I should see a display for ProductionWorkOrderDisplay_Facility_Address with facility "f"'s Address
    And I should see a display for EquipmentDisplay_Description with equipment "e"'s Description
    And I should see a display for EquipmentDisplay_EquipmentManufacturer with equipment "e"'s EquipmentManufacturer
    And I should see a display for EquipmentDisplay_EquipmentModel with equipment "e"'s EquipmentModel
    And I should see a display for EquipmentDisplay_SerialNumber with equipment "e"'s SerialNumber
    
    # Test Required Fields
    When I press Save
    Then I should see a validation message for PersonResponsible with "The PersonResponsible field is required."
    And I should see a validation message for ProtectionType with "The Type of Protection field is required."
    And I should see a validation message for AreaProtected with "The AreaProtected field is required."
    And I should see a validation message for ReasonForImpairment with "The ReasonForImpairment field is required."
    And I should see a validation message for NumberOfTurnsToClose with "The NumberOfTurnsToClose field is required."
    And I should see a validation message for AuthorizedBy with "The AuthorizedBy field is required."
    And I should see a validation message for FireProtectionEquipmentOperator with "The FireProtectionEquipmentOperator field is required."
    And I should see a validation message for EquipmentImpairedOn with "The Date Equipment Impaired field is required."
    And I should see a validation message for EmergencyOrganizationNotified with "At least one precaution must be checked."
    
    # Test Required When Fields
    When I select "Other" from the ProtectionType dropdown
    And I check the HasOtherPrecaution field
    And I press Save
    Then I should see a validation message for OtherPrecautionDescription with "The OtherPrecautionDescription field is required."
    And I should see a validation message for AdditionalInformationForProtectionType with "AdditionalInformationForProtectionType is required for the specified Protection Type."
    When I select "Special Protection" from the ProtectionType dropdown
    And I press Save
    Then I should see a validation message for AdditionalInformationForProtectionType with "AdditionalInformationForProtectionType is required for the specified Protection Type."

    # Test min ranges
    When I enter "1/4/2021 12:00 AM" into the EquipmentImpairedOn field
    And I enter "0" into the NumberOfTurnsToClose field
    And I press Save
    Then I should see a validation message for EquipmentImpairedOn with "EquipmentImpairedOn date cannot be before today."
    And I should see a validation message for NumberOfTurnsToClose with "The field NumberOfTurnsToClose must be between 1 and 99999."
    When I enter "1/4/2041 12:00 AM" into the EquipmentImpairedOn field
    Then I should not see a validation message for EquipmentImpairedOn with "EquipmentImpairedOn date cannot be before today."

    # Test max ranges
    When I enter "100000" into the NumberOfTurnsToClose field
    And I press Save
    Then I should see a validation message for NumberOfTurnsToClose with "The field NumberOfTurnsToClose must be between 1 and 99999."

    # Test valid information
    When I select employee "employee"'s Description from the PersonResponsible dropdown
    And I enter "additional-information-for-protection-type-text" into the AdditionalInformationForProtectionType field
    And I enter "area-protected-text" into the AreaProtected field
    And I enter "reason-for-impairment-text" into the ReasonForImpairment field
    And I enter "5" into the NumberOfTurnsToClose field
    And I select employee "employee"'s Description from the AuthorizedBy dropdown
    And I enter "fire-protection-equipment-operator-text" into the FireProtectionEquipmentOperator field
    And I enter "now" into the EquipmentImpairedOn field
    And I uncheck the HasOtherPrecaution field
    And I check the FireHoseLaidOut field
    And I press Save
    And I wait for the page to reload
    Then I should see a display for PersonResponsible with employee "employee"
    And I should see a display for AdditionalInformationForProtectionType with "additional-information-for-protection-type-text"
    And I should see a display for AreaProtected with "area-protected-text"
    And I should see a display for ReasonForImpairment with "reason-for-impairment-text"
    And I should see a display for NumberOfTurnsToClose with "5"
    And I should see a display for AuthorizedBy with employee "employee"
    And I should see a display for FireProtectionEquipmentOperator with "fire-protection-equipment-operator-text"
    And I should see a display for EquipmentImpairedOn with a date time close to now
    And I should see a display for CreatedAt with a date time close to now
    And I should see a display for FireHoseLaidOut with "Yes"

Scenario: User can edit a red tag permit 
    Given I am logged in as "user-admin"
    When I go to the edit page for red tag permit: "rtp"
    Then I should see a display for ProductionWorkOrderDisplay_ProductionWorkDescription with production work order "pwo-for-edit"'s ProductionWorkDescription
    And I should see a display for ProductionWorkOrderDisplay_OperatingCenter_State with operating center "oc"'s State
    And I should see a display for ProductionWorkOrderDisplay_OperatingCenter with operating center "oc"
    And I should see a display for ProductionWorkOrderDisplay_Facility with facility "f"'s Description
    And I should see a display for ProductionWorkOrderDisplay_Facility_Address with facility "f"'s Address
    And I should see a display for EquipmentDisplay_Description with equipment "e"'s Description
    And I should see a display for EquipmentDisplay_EquipmentManufacturer with equipment "e"'s EquipmentManufacturer
    And I should see a display for EquipmentDisplay_EquipmentModel with equipment "e"'s EquipmentModel
    And I should see a display for EquipmentDisplay_SerialNumber with equipment "e"'s SerialNumber
    And employee "employee"'s Description should be selected in the PersonResponsible dropdown
    And employee "employee"'s Description should be selected in the AuthorizedBy dropdown
    And I should see "area-protected-edit-text" in the AreaProtected field
    And I should see "reason-for-impairment-edit-text" in the ReasonForImpairment field
    And I should see "12" in the NumberOfTurnsToClose field
    And I should see "fire-protection-equipment-operating-edit-text" in the FireProtectionEquipmentOperator field
    And I should see "4/21/2021 12:13:48 PM" in the EquipmentImpairedOn field
    And the HasOtherPrecaution field should be checked
    And I should see "other-precaution-description-edit-text" in the OtherPrecautionDescription field

    # Test Required Fields
    When I press Save
    Then I should see a validation message for EquipmentRestoredOn with "The EquipmentRestoredOn field is required."

    # Test comparison validation
    When I enter "3/21/2021 12:13:48 PM" into the EquipmentRestoredOn field
    Then I should see a validation message for EquipmentRestoredOn with "EquipmentRestoredOn must be greater than EquipmentImpairedOn."

    # Test Equipment Restored On Change Reason validation
    When I enter "now" into the EquipmentRestoredOn field
    And I press Save
    Then I should see a validation message for EquipmentRestoredOnChangeReason with "Change Reason is required."

    # Test valid information
    When I enter "Blah Blah Blah" into the EquipmentRestoredOnChangeReason field
    And I press Save
    And I wait for the page to reload
    Then I should see a display for EquipmentRestoredOn with a date time close to now
    And I should see a display for EquipmentRestoredOnChangeReason with "Blah Blah Blah"

    # Validate the work order prerequi is satisified
    When I visit the Show page for production work order "pwo-for-edit"    
    When I click the "Prerequisites" tab
    Then I should see today's date in the table prerequisitesTable's "Satisfied On" column

Scenario: User can view an existing red tag permit
    Given I am logged in as "user-admin"
    When I go to the show page for red tag permit: "rtp"
    Then I should see a display for ProductionWorkOrder_ProductionWorkDescription with production work order "pwo-for-edit"'s ProductionWorkDescription
    And I should see a display for ProductionWorkOrder_OperatingCenter_State with operating center "oc"'s State
    And I should see a display for ProductionWorkOrder_OperatingCenter with operating center "oc"
    And I should see a display for ProductionWorkOrder_Facility with facility "f"'s Description
    And I should see a display for ProductionWorkOrder_Facility_Address with facility "f"'s Address
    And I should see a display for Equipment_Description with equipment "e"'s Description
    And I should see a display for Equipment_EquipmentManufacturer with equipment "e"'s EquipmentManufacturer
    And I should see a display for Equipment_EquipmentModel with equipment "e"'s EquipmentModel
    And I should see a display for Equipment_SerialNumber with equipment "e"'s SerialNumber
    And I should see a display for PersonResponsible with employee "employee"
    And I should see a display for AuthorizedBy with employee "employee"
    And I should see a display for AreaProtected with "area-protected-edit-text"
    And I should see a display for ReasonForImpairment with "reason-for-impairment-edit-text"
    And I should see a display for NumberOfTurnsToClose with "12"
    And I should see a display for FireProtectionEquipmentOperator with "fire-protection-equipment-operating-edit-text"
    And I should see a display for EquipmentImpairedOn with "4/21/2021 12:13:48 PM"
    And I should see a display for CreatedAt with a date time close to now
    And I should see a display for HasOtherPrecaution with "Yes"
    And I should see a display for OtherPrecautionDescription with "other-precaution-description-edit-text"

Scenario: User should not see delete in action bar 
	Given I am logged in as "user-admin"
	And I am at the show page for red tag permit: "rtp"
	Then I should not see "Delete"

Scenario: User can search for an existing red tag permit
    Given I am logged in as "user-admin"
    When I visit the HealthAndSafety/RedTagPermit/Search page
    And I press Search
    Then I should see a link to the Show page for red tag permit: "rtp"
    When I follow the Show link for red tag permit "rtp"
    Then I should be at the Show page for red tag permit: "rtp"

Scenario: User can download pdf
	Given I am logged in as "user-admin"
	When I visit the show page for red tag permit: "rtp"
	Then I should be able to download red tag permit "rtp"'s pdf