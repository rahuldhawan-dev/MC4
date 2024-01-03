Feature: SystemDeliveryEntryPage

Background:
	Given a state "nj" exists
	And an operating center "nj7" exists with opcode: "NJ7", state: "nj"
	And an operating center "nj8" exists with opcode: "NJ8", state: "nj"
	And a system delivery type "water" exists with description: "water"
	And a state "one" exists with name: "New Jersey", abbreviation: "NJ"
	And a public water supply "water" exists with op code: "NJ7", operating area: "nj7", identifier: "1111", state: "one", system: "System"
	And a system delivery type "waste water" exists with description: "waste water"
	And a waste water system "waste water1" exists with waste water system name: "Waste Water 11", operating center: "nj7"
	And a waste water system "waste water2" exists with waste water system name: "Waste Water 22", operating center: "nj7"
	And equipment types exist
	And equipment subcategories exist
	And equipment categories exist
	And system delivery entry types exist with system delivery type: "water"
	And system delivery entry types exist with system delivery type: "waste water"
	And public water supply statuses exist
	And a public water supply "one" exists with operating area: "nj7", identifier: "1112", status: "active", aw owned: "true", state: "nj"
	And a facility "one" exists with operating center: "nj7", facility id: "NJSB-1", facility name: "A Facility", point of entry: "true", system delivery type: "water", regional planning area: "abc-123Testing", public water supply: "water"
	And a facility "two" exists with operating center: "nj7", facility id: "NJSB-2", facility name: "A Facility 2: return of the facility", point of entry: "true", system delivery type: "water", regional planning area: "abc-123Testing", public water supply: "water"
	And a facility "three" exists with operating center: "nj7", facility id: "NJSB-3", facility name: "A Facility 3: return of the facility3", point of entry: "true", system delivery type: "waste water", regional planning area: "abc-123Testing", waste water system: "waste water1"
	And a facility "fourfour" exists with operating center: "nj7", facility id: "NJSB-4", facility name: "A Facility 4: return of the facility4", point of entry: "true", system delivery type: "waste water", regional planning area: "abc-123444Testing", waste water system: "waste water1"
	And a facility "editfacility" exists with operating center: "nj8", facility id: "NJSB-0", facility name: "A Facility 3: When will this end", point of entry: "true", system delivery type: "water"
	And a facility system delivery entry type "one" exists with facility: "one", system delivery entry type: "delivered water", is enabled: true, minimum value: -8.618, maximum value: 20.141, is injection site: false
	And a facility system delivery entry type "two" exists with facility: "two", system delivery entry type: "purchased water", is enabled: true, minimum value: -8.618, maximum value: 120.141, purchase supplier: "You know the sound a fork makes in the garbage disposal? That’s the sound my brain makes"
	And a facility system delivery entry type "three" exists with facility: "editfacility", system delivery entry type: "delivered water", is enabled: true, minimum value: -8.618, maximum value: 20.141, is injection site: true
	And facility system delivery entry type "one" exists in facility "one"
	And facility system delivery entry type "two" exists in facility "two"
	And facility system delivery entry type "three" exists in facility "editfacility"
	And a system delivery entry "one" exists 
	And a system delivery entry "validated" exists with IsValidated: "true", WeekOf: "5/23/2022", system delivery type: "water", IsHyperionFileCreated: true
	And a system delivery entry "editentry" exists with IsValidated: "false", WeekOf: "12/14/2020", system delivery type: "water"
	And a system delivery facility entry "mondaydelivered" exists with system delivery entry: "editentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/11/2022, entry value: 3.251
	And a system delivery facility entry "tuesdaydelivered" exists with system delivery entry: "editentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/12/2022, entry value: 6.661
	And a system delivery facility entry "wednesdaydelivered" exists with system delivery entry: "editentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/13/2022, entry value: 8.212
	And a system delivery facility entry "thursdaydelivered" exists with system delivery entry: "editentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/14/2022, entry value: 2.122
	And a system delivery facility entry "fridaydelivered" exists with system delivery entry: "editentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/15/2022, entry value: 12.116
	And a system delivery facility entry "saturdaydelivered" exists with system delivery entry: "editentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/16/2022, entry value: 4.726
	And a system delivery facility entry "sundaydelivered" exists with system delivery entry: "editentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/17/2022, entry value: 9.121
	And a system delivery facility entry "mondaypurchased" exists with system delivery entry: "editentry", system delivery entry type: "purchased water", facility: "two", entry date: 7/11/2022, entry value: 3.251
	And a system delivery facility entry "tuesdaypurchased" exists with system delivery entry: "editentry", system delivery entry type: "purchased water", facility: "two", entry date: 7/12/2022, entry value: 6.661
	And a system delivery facility entry "wednesdaypurchased" exists with system delivery entry: "editentry", system delivery entry type: "purchased water", facility: "two", entry date: 7/13/2022, entry value: 8.212
	And a system delivery facility entry "thursdaypurchased" exists with system delivery entry: "editentry", system delivery entry type: "purchased water", facility: "two", entry date: 7/14/2022, entry value: 2.122
	And a system delivery facility entry "fridaypurchased" exists with system delivery entry: "editentry", system delivery entry type: "purchased water", facility: "two", entry date: 7/15/2022, entry value: 12.116
	And a system delivery facility entry "saturdaypurchased" exists with system delivery entry: "editentry", system delivery entry type: "purchased water", facility: "two", entry date: 7/16/2022, entry value: 4.726
	And a system delivery facility entry "sundaypurchased" exists with system delivery entry: "editentry", system delivery entry type: "purchased water", facility: "two", entry date: 7/17/2022, entry value: 9.121
	And facility "one" exists in system delivery entry "editentry"
	And facility "one" exists in system delivery entry "validated"
	And facility "two" exists in system delivery entry "editentry"
	And operating center "nj7" exists in system delivery entry "editentry"
	And operating center "nj7" exists in system delivery entry "validated"
	And a role "prodsystemdeliveryentryread" exists with action: "Read", module: "ProductionSystemDeliveryEntry", operating center: "nj7"
	And a role "prodsystemdeliveryentryedit" exists with action: "Edit", module: "ProductionSystemDeliveryEntry", operating center: "nj7"
	And a role "prodsystemdeliveryentryeditnj8" exists with action: "Edit", module: "ProductionSystemDeliveryEntry", operating center: "nj8"
	And a role "prodsystemdeliveryentryadd" exists with action: "Add", module: "ProductionSystemDeliveryEntry", operating center: "nj7"
	And a role "prodsystemdeliveryvalidationedit" exists with action: "Add", module: "ProductionSystemDeliveryApprover", operating center: "nj7"
	And a role "prodsystemdeliveryentryuseradmin" exists with action: "UserAdministrator", module: "ProductionSystemDeliveryEntry", operating center: "nj7"
	And a role "prodsystemdeliveryvalidationuseradmin" exists with action: "UserAdministrator", module: "ProductionSystemDeliveryApprover", operating center: "nj7"
	And a role "prodequipmentread" exists with action: "Read", module: "ProductionEquipment" 
	And a role "prodfacilitiesread" exists with action: "Read", module: "ProductionFacilities"	
	
	# This role is used to allow users to Adjust an entry after it has been sent to Hyperion. It is associated with the "useradmin" user
	And a role "productionsystemdeliveryadmin" exists with action: "UserAdministrator", module: "ProductionSystemDeliveryAdmin", operating center: "nj7"
	
	# This role's sole purpose is to ensure a user given this role still cannot delete a System Delivery Entry - Deleting is for Site Admins only
	And a role "prodsystemdeliveryentrydelete" exists with action: "Delete", module: "ProductionSystemDeliveryEntry", operating center: "nj7"
	
	And an employee "one" exists
	And an employee "two" exists
	And an employee "three" exists
	And an employee "four" exists
	And a user "user" exists with username: "user", employee: "one", roles: prodsystemdeliveryentryread;prodsystemdeliveryentryedit;prodsystemdeliveryentryadd;prodequipmentread;prodfacilitiesread;prodsystemdeliveryentryeditnj8
	And a user "validater" exists with username: "validater", employee: "two", roles: prodsystemdeliveryentryread;prodsystemdeliveryentryedit;prodsystemdeliveryentryadd;prodequipmentread;prodfacilitiesread;prodsystemdeliveryvalidationedit
	And a user "useradmin" exists with username: "useradmin", employee: "four", roles: prodsystemdeliveryentryuseradmin;prodsystemdeliveryvalidationuseradmin;prodequipmentread;prodfacilitiesread;productionsystemdeliveryadmin;prodsystemdeliveryentrydelete
	And an admin user "admin" exists with username: "admin", employee: "three"
	And a role "facilitrolenj8user" exists with action: "Read", module: "ProductionFacilities", operating center: "nj8", user: "user"

Scenario: User can create an entry
	Given I am logged in as "user"
	And a facility system delivery entry type "wastewater" exists with facility: "three", system delivery entry type: "wastewater collected", is enabled: true, minimum value: 1.618, maximum value: 20.141
	And facility system delivery entry type "wastewater" exists in facility "three"
	When I visit the Production/SystemDeliveryEntry/New page
	And I press "Select"
	Then I should see a validation message for WeekOf with "The WeekOf field is required."
	When I enter "12/7/2020" into the WeekOf field	
	And I press "Select"
	Then I should see a validation message for OperatingCenters with "The OperatingCenters field is required."
	When I select operating center "nj7" from the OperatingCenters dropdown
	And I press "Select"
	Then I should see a validation message for SystemDeliveryType with "The SystemDeliveryType field is required."
	When I select system delivery type "water" from the SystemDeliveryType dropdown
	And I press "Select"
	Then I should see a validation message for Facilities with "The Facilities field is required."
	And I should see a display for PublicWaterSupplies with "No Results"
	When I select system delivery type "waste water" from the SystemDeliveryType dropdown
	When I select facility "three"'s FacilityIdWithFacilityName from the Facilities dropdown
	And I press "Select"
	Then the currently shown system delivery entry shall henceforth be known throughout the land as "Carl"
	And I should be at the Edit page for system delivery entry "Carl"
	And I should see a display for WeekOf with "12/7/2020"
	And I should see a display for WasteWaterSystems with "NJ7WW0001 - Waste Water 11NJ7WW0002 - Waste Water 22"
	When I visit the Show page for system delivery entry: "Carl"
	Then I should see "NJ7WW0001 - Waste Water 11"

Scenario: The public water supplies and waste water systems fields display on the system delivery entry show page
	Given I am logged in as "user"
	And a facility system delivery entry type "wastewater" exists with facility: "three", system delivery entry type: "wastewater collected", is enabled: true, minimum value: 1.618, maximum value: 20.141
	And facility system delivery entry type "wastewater" exists in facility "three"
	When I visit the Production/SystemDeliveryEntry/New page
	And I press "Select"
	Then I should see a validation message for WeekOf with "The WeekOf field is required."
	When I enter "12/7/2020" into the WeekOf field	
	And I press "Select"
	Then I should see a validation message for OperatingCenters with "The OperatingCenters field is required."
	When I select operating center "nj7" from the OperatingCenters dropdown
	And I press "Select"
	Then I should see a validation message for SystemDeliveryType with "The SystemDeliveryType field is required."
	When I select system delivery type "water" from the SystemDeliveryType dropdown
	And I press "Select"
	Then I should see a validation message for Facilities with "The Facilities field is required."
	And I should see a display for PublicWaterSupplies with "No Results"
	When I select system delivery type "waste water" from the SystemDeliveryType dropdown
	When I select facility "three"'s FacilityIdWithFacilityName from the Facilities dropdown
	And I press "Select"
	Then the currently shown system delivery entry shall henceforth be known throughout the land as "Carl"
	And I should be at the Edit page for system delivery entry "Carl"
	And I should see a display for WeekOf with "12/7/2020"
	And I should see a display for WasteWaterSystems with "NJ7WW0001 - Waste Water 11NJ7WW0002 - Waste Water 22"

Scenario: Public water supply and waste water system fields show and hide on the new page based on the system delivery type field
	Given I am logged in as "user"
	When I visit the Production/SystemDeliveryEntry/New page
	And I enter "12/7/2020" into the WeekOf field	
	And I select operating center "nj7" from the OperatingCenters dropdown
	And I select system delivery type "water" from the SystemDeliveryType dropdown
	Then I should see a display for PublicWaterSupplies with "No Results"
	And I should not see the field WasteWaterSystems
	When I select system delivery type "waste water" from the SystemDeliveryType dropdown
	Then I should not see the field PublicWaterSupplies
	When I select waste water system "waste water1"'s Description from the WasteWaterSystems dropdown
	Then I should see a display for Facilities with "No Results"

Scenario: Public water supply and waste water system fields show and hide on the Edit page based on the system delivery type field
	Given I am logged in as "user"
	When I visit the Edit page for system delivery entry "editentry"
	And I enter "12/7/2020" into the WeekOf field	
	And I select operating center "nj7" from the OperatingCenters dropdown
	And I select system delivery type "water" from the SystemDeliveryType dropdown
	Then I should see a display for PublicWaterSupplies with "No Results"
	And I should not see the field WasteWaterSystems
	When I select system delivery type "waste water" from the SystemDeliveryType dropdown
	Then I should not see the field PublicWaterSupplies
	When I select waste water system "waste water1"'s Description from the WasteWaterSystems dropdown
	Then I should see a display for Facilities with "No Results"

Scenario: There should be daily totals for each operating center on the show page
	Given I am logged in as "admin"
	When I visit the Edit page for system delivery entry "editentry"
	Then I should see "You know the sound a fork makes in the garbage disposal? That’s the sound my brain makes"
	When I enter 2.00 into the FacilityEntries_0__EntryValue field
	And I enter 3.00 into the FacilityEntries_1__EntryValue field
	And I enter 4.00 into the FacilityEntries_2__EntryValue field
	And I enter 5.00 into the FacilityEntries_3__EntryValue field
	And I enter 6.00 into the FacilityEntries_4__EntryValue field
	And I enter 7.00 into the FacilityEntries_5__EntryValue field
	And I enter 8.00 into the FacilityEntries_6__EntryValue field
	And I enter 20.00 into the FacilityEntries_7__EntryValue field
	And I enter 30.00 into the FacilityEntries_8__EntryValue field
	And I enter 40.00 into the FacilityEntries_9__EntryValue field
	And I enter 50.00 into the FacilityEntries_10__EntryValue field
	And I enter 60.00 into the FacilityEntries_11__EntryValue field
	And I enter 70.00 into the FacilityEntries_12__EntryValue field
	And I enter 18.00 into the FacilityEntries_13__EntryValue field
	And I press "Save" 
	Then I should be at the Show page for system delivery entry "editentry"
	And I should see a display for WeekOf with "12/14/2020"
	And I should see a display for IsValidated with "n/a"
	And I should see the notification message "This record has not been validated"
	And I should see "NJ7-1 - A Facility, NJ7-2 - A Facility 2return of the facility"
	And I should see "NJ7 -" in the entryTable element
	And I should see "Water" in the entryTable element
	And I should see "2" in the entryTable element
	And I should see "3" in the entryTable element
	And I should see "4" in the entryTable element
	And I should see "5" in the entryTable element
	And I should see "6" in the entryTable element
	And I should see "7" in the entryTable element
	And I should see "8" in the entryTable element
	And I should see "35" in the entryTable element
	And I should see "20" in the entryTable element
	And I should see "30" in the entryTable element
	And I should see "40" in the entryTable element
	And I should see "50" in the entryTable element
	And I should see "60" in the entryTable element
	And I should see "70" in the entryTable element
	And I should see "18" in the entryTable element
	And I should see "22.000" in the entryTable element
	And I should see "33.000" in the entryTable element
	And I should see "44.000" in the entryTable element
	And I should see "55.000" in the entryTable element
	And I should see "66.000" in the entryTable element
	And I should see "77.000" in the entryTable element
	And I should see "26.000" in the entryTable element
	And I should see "NJ7 - Total" in the entryTable element
	And I should see "323.000" in the entryTable element

Scenario: Admin can create a entry
	Given I am logged in as "admin"
	And a facility system delivery entry type "wastewater" exists with facility: "three", system delivery entry type: "wastewater collected", is enabled: true, minimum value: 1.618, maximum value: 20.141
	And facility system delivery entry type "wastewater" exists in facility "three"
	When I visit the Production/SystemDeliveryEntry/New page
	And I enter "12/7/2020" into the WeekOf field
	And I press "Select"
	Then I should see a validation message for OperatingCenters with "The OperatingCenters field is required."
	When I select operating center "nj7" from the OperatingCenters dropdown
	And I press "Select"
	Then I should see a validation message for SystemDeliveryType with "The SystemDeliveryType field is required."
	When I select system delivery type "water" from the SystemDeliveryType dropdown
	And I press "Select"
	Then I should see a validation message for Facilities with "The Facilities field is required."
	And I should see a display for PublicWaterSupplies with "No Results"
	When I select system delivery type "waste water" from the SystemDeliveryType dropdown
	And I select facility "three"'s FacilityIdWithFacilityName from the Facilities dropdown
	And I press "Select"
	Then the currently shown system delivery entry shall henceforth be known throughout the land as "Carl"
	And I should be at the Edit page for system delivery entry "Carl"
	And I should see a display for WasteWaterSystems with "NJ7WW0001 - Waste Water 11NJ7WW0002 - Waste Water 22"
	When I visit the Show page for system delivery entry: "Carl"
	Then I should see "NJ7WW0001 - Waste Water 11"

Scenario: Waste water categories are totalled individually instead of all together
	Given I am logged in as "user"
	And a facility system delivery entry type "wastewater" exists with facility: "three", system delivery entry type: "wastewater collected", is enabled: true, minimum value: 1.618, maximum value: 100.141
	And facility system delivery entry type "wastewater" exists in facility "three"
	And a facility system delivery entry type "wastewater11" exists with facility: "fourfour", system delivery entry type: "wastewater collected", is enabled: true, minimum value: 1.618, maximum value: 100.141
	And facility system delivery entry type "wastewater11" exists in facility "fourfour"
	And a facility system delivery entry type "wastewater2" exists with facility: "fourfour", system delivery entry type: "wastewater treated", is enabled: true, minimum value: 1.618, maximum value: 100.141
	And facility system delivery entry type "wastewater2" exists in facility "fourfour"
	And a facility system delivery entry type "wastewater21" exists with facility: "three", system delivery entry type: "wastewater treated", is enabled: true, minimum value: 1.618, maximum value: 100.141
	And facility system delivery entry type "wastewater21" exists in facility "three"
	When I visit the Production/SystemDeliveryEntry/New page
	When I enter "12/7/2020" into the WeekOf field	
	When I select operating center "nj7" from the OperatingCenters dropdown
	When I select system delivery type "waste water" from the SystemDeliveryType dropdown
	When I select facility "three"'s FacilityIdWithFacilityName from the Facilities dropdown
	And I select facility "fourfour"'s FacilityIdWithFacilityName from the Facilities dropdown
	And I press "Select"
	Then the currently shown system delivery entry shall henceforth be known throughout the land as "Carl"
	And I should be at the Edit page for system delivery entry "Carl"
	When I enter 12.00 into the FacilityEntries_0__EntryValue field
	And I enter 13.00 into the FacilityEntries_1__EntryValue field
	And I enter 13.00 into the FacilityEntries_2__EntryValue field
	And I enter 14.00 into the FacilityEntries_3__EntryValue field
	And I enter 15.00 into the FacilityEntries_4__EntryValue field
	And I enter 16.00 into the FacilityEntries_5__EntryValue field
	And I enter 17.00 into the FacilityEntries_6__EntryValue field
	And I enter 21.00 into the FacilityEntries_7__EntryValue field
	And I enter 22.00 into the FacilityEntries_8__EntryValue field
	And I enter 23.00 into the FacilityEntries_9__EntryValue field
	And I enter 24.00 into the FacilityEntries_10__EntryValue field
	And I enter 25.00 into the FacilityEntries_11__EntryValue field
	And I enter 26.00 into the FacilityEntries_12__EntryValue field
	And I enter 27.00 into the FacilityEntries_13__EntryValue field
	And I enter 31.00 into the FacilityEntries_14__EntryValue field
	And I enter 32.00 into the FacilityEntries_15__EntryValue field
	And I enter 33.00 into the FacilityEntries_16__EntryValue field
	And I enter 34.00 into the FacilityEntries_17__EntryValue field
	And I enter 35.00 into the FacilityEntries_18__EntryValue field
	And I enter 36.00 into the FacilityEntries_19__EntryValue field
	And I enter 37.00 into the FacilityEntries_20__EntryValue field
	And I enter 41.00 into the FacilityEntries_21__EntryValue field
	And I enter 42.00 into the FacilityEntries_22__EntryValue field
	And I enter 43.00 into the FacilityEntries_23__EntryValue field
	And I enter 44.00 into the FacilityEntries_24__EntryValue field
	And I enter 45.00 into the FacilityEntries_25__EntryValue field
	And I enter 46.00 into the FacilityEntries_26__EntryValue field
	And I enter 47.00 into the FacilityEntries_27__EntryValue field
	And I press "Save" 
	Then I should be at the Show page for system delivery entry "Carl"
	Then I should see "NJ7-4 - A Facility 4return of the facility"
	And I should see "NJ7 - Totals" in the entryTable element
	And I should see "WasteWater collected Total:" in the entryTable element
	And I should see "338.000" in the entryTable element
	And I should see "WasteWater treated Total:" in the entryTable element
	And I should see "476.000" in the entryTable element

Scenario: user can edit facilities and operating center and save without error
	Given I am logged in as "user"
	When I visit the Edit page for system delivery entry "editentry"
	And I select operating center "nj8" from the OperatingCenters dropdown
	And I select system delivery type "water" from the SystemDeliveryType dropdown
	And I select facility "editfacility"'s FacilityIdWithFacilityName from the Facilities dropdown 
	And I press "Save"
	Then I should be at the Edit page for system delivery entry "editentry"
	When I visit the Edit page for system delivery entry "editentry"
	Then I should see "0" in the FacilityEntries_0__EntryValue field
	And I should see "0" in the FacilityEntries_1__EntryValue field
	And I should see "0" in the FacilityEntries_2__EntryValue field
	And I should see "0" in the FacilityEntries_3__EntryValue field
	And I should see "0" in the FacilityEntries_4__EntryValue field
	And I should see "0" in the FacilityEntries_5__EntryValue field
	And I should see "0" in the FacilityEntries_6__EntryValue field
	And I should not see "INJECTION?"

Scenario: User can edit facility list on a record with entries and entries should clear and have to be reentered
	Given I am logged in as "user"
	When I visit the Production/SystemDeliveryEntry/New page
	And I enter "12/7/2020" into the WeekOf field	
	And I select operating center "nj7" from the OperatingCenters dropdown
	And I select system delivery type "water" from the SystemDeliveryType dropdown
	And I select facility "one"'s FacilityIdWithFacilityName from the Facilities dropdown
	And I press "Select"
	Then the currently shown system delivery entry shall henceforth be known throughout the land as "Daryl"
	And I should be at the Edit page for system delivery entry "Daryl"
	And I should see "A Facility"
	When I select operating center "nj8" from the OperatingCenters dropdown 
	And I select facility "editfacility"'s FacilityIdWithFacilityName from the Facilities dropdown
	And I press "Save"
	Then I should be at the Edit page for system delivery entry "Daryl"
	And I should see "A Facility 3When will this end"
	When I enter -2.00 into the FacilityEntries_0__EntryValue field
	And I check the FacilityEntries_0__IsInjection field
	And I enter -3.00 into the FacilityEntries_1__EntryValue field
	And I check the FacilityEntries_1__IsInjection field
	And I enter -4.00 into the FacilityEntries_2__EntryValue field
	And I check the FacilityEntries_2__IsInjection field
	And I enter -5.00 into the FacilityEntries_3__EntryValue field
	And I check the FacilityEntries_3__IsInjection field
	And I enter -6.00 into the FacilityEntries_4__EntryValue field
	And I check the FacilityEntries_4__IsInjection field
	And I enter -7.00 into the FacilityEntries_5__EntryValue field
	And I check the FacilityEntries_5__IsInjection field
	And I enter -8.00 into the FacilityEntries_6__EntryValue field
	And I check the FacilityEntries_6__IsInjection field
	And I press "Save" 
	Then I should be at the Show page for system delivery entry "Daryl"
	And I should see a display for WeekOf with "12/7/2020"
    And I should see the notification message "This record has not been validated"
	And I should see a display for IsValidated with "n/a"
	And I should see "A Facility 3When will this end" in the entryTable element
	And I should see "NJ8 -" in the entryTable element
	And I should see "Water" in the entryTable element
	And I should see "-2.000" in the entryTable element
	And I should see "Yes" in the entryTable element
	And I should see "-3.000" in the entryTable element
	And I should see "-4.000" in the entryTable element
	And I should see "-5.000" in the entryTable element
	And I should see "-6.000" in the entryTable element
	And I should see "-7.000" in the entryTable element
	And I should see "-8.000" in the entryTable element
	And I should see "-35.000" in the entryTable element
	And I should see "Category" in the entryTable element
	And I should see "Monday" in the entryTable element
	And I should see "Tuesday" in the entryTable element
	And I should see "Wednesday" in the entryTable element
	And I should see "Thursday" in the entryTable element
	And I should see "Friday" in the entryTable element
	And I should see "Saturday" in the entryTable element
	And I should see "Sunday" in the entryTable element
	And I should see "A Facility 3When will this end Total" in the entryTable element

Scenario: Admin can edit entry and fill out form entries and validate
	Given I am logged in as "admin"
	When I visit the Edit page for system delivery entry "editentry"
	Then I should see "You know the sound a fork makes in the garbage disposal? That’s the sound my brain makes"
	When I enter 2.00 into the FacilityEntries_0__EntryValue field
	And I enter 3.00 into the FacilityEntries_1__EntryValue field
	And I enter 4.00 into the FacilityEntries_2__EntryValue field
	And I enter 5.00 into the FacilityEntries_3__EntryValue field
	And I enter 6.00 into the FacilityEntries_4__EntryValue field
	And I enter 7.00 into the FacilityEntries_5__EntryValue field
	And I enter 8.00 into the FacilityEntries_6__EntryValue field
	And I enter 20.00 into the FacilityEntries_7__EntryValue field
	And I enter 30.00 into the FacilityEntries_8__EntryValue field
	And I enter 40.00 into the FacilityEntries_9__EntryValue field
	And I enter 50.00 into the FacilityEntries_10__EntryValue field
	And I enter 60.00 into the FacilityEntries_11__EntryValue field
	And I enter 70.00 into the FacilityEntries_12__EntryValue field
	And I enter 18.00 into the FacilityEntries_13__EntryValue field
	And I press "Save" 
	Then I should be at the Show page for system delivery entry "editentry"
	And I should see a display for WeekOf with "12/14/2020"
	And I should see a display for IsValidated with "n/a"
	And I should see the notification message "This record has not been validated"
	And I should see "NJ7-1 - A Facility, NJ7-2 - A Facility 2return of the facility"
	And I should see "NJ7 -" in the entryTable element
	And I should see "Water" in the entryTable element
	And I should see "2" in the entryTable element
	And I should see "3" in the entryTable element
	And I should see "4" in the entryTable element
	And I should see "5" in the entryTable element
	And I should see "6" in the entryTable element
	And I should see "7" in the entryTable element
	And I should see "8" in the entryTable element
	And I should see "35" in the entryTable element
	And I should see "20" in the entryTable element
	And I should see "30" in the entryTable element
	And I should see "40" in the entryTable element
	And I should see "50" in the entryTable element
	And I should see "60" in the entryTable element
	And I should see "70" in the entryTable element
	And I should see "18" in the entryTable element
	And I should see "288" in the entryTable element
	And I should see "NJ7-1"
	And I should see "NJ7-2"
	And I should see "Category" in the entryTable element
	And I should see "Monday" in the entryTable element
	And I should see "Tuesday" in the entryTable element
	And I should see "Wednesday" in the entryTable element
	And I should see "Thursday" in the entryTable element
	And I should see "Friday" in the entryTable element
	And I should see "Saturday" in the entryTable element
	And I should see "Sunday" in the entryTable element
	And I should see "A Facility 2return of the facility Total" in the entryTable element
	And I should see "A Facility Total" in the entryTable element
	And I should see "288.000" in the entryTable element
	And I should see "35.000" in the entryTable element
	And I should see "You know the sound a fork makes in the garbage disposal? That’s the sound my brain makes" in the entryTable element
	When I click ok in the dialog after pressing "Validate and submit"
	And I wait for the page to reload
	Then I should be at the show page for system delivery entry "editentry"
	And I should see the notification message "This record is now locked, if changes need to be made please enter a adjustment."

Scenario: user admin can edit entry and fill out form entries and validate
	Given I am logged in as "useradmin"
	When I visit the Edit page for system delivery entry "editentry"
	Then I should see "You know the sound a fork makes in the garbage disposal? That’s the sound my brain makes"
	When I enter 2.00 into the FacilityEntries_0__EntryValue field
	And I enter 3.00 into the FacilityEntries_1__EntryValue field
	And I enter 4.00 into the FacilityEntries_2__EntryValue field
	And I enter 5.00 into the FacilityEntries_3__EntryValue field
	And I enter 6.00 into the FacilityEntries_4__EntryValue field
	And I enter 7.00 into the FacilityEntries_5__EntryValue field
	And I enter 8.00 into the FacilityEntries_6__EntryValue field
	And I enter 20.00 into the FacilityEntries_7__EntryValue field
	And I enter 30.00 into the FacilityEntries_8__EntryValue field
	And I enter 40.00 into the FacilityEntries_9__EntryValue field
	And I enter 50.00 into the FacilityEntries_10__EntryValue field
	And I enter 60.00 into the FacilityEntries_11__EntryValue field
	And I enter 70.00 into the FacilityEntries_12__EntryValue field
	And I enter 18.00 into the FacilityEntries_13__EntryValue field
	And I press "Save" 
	Then I should be at the Show page for system delivery entry "editentry"
	And I should see a display for WeekOf with "12/14/2020"
	And I should see a display for IsValidated with "n/a"
    And I should see the notification message "This record has not been validated"
	And I should see "NJ7-1 - A Facility, NJ7-2 - A Facility 2return of the facility"
	And I should see "NJ7 -" in the entryTable element
	And I should see "Water" in the entryTable element
	And I should see "2" in the entryTable element
	And I should see "3" in the entryTable element
	And I should see "4" in the entryTable element
	And I should see "5" in the entryTable element
	And I should see "6" in the entryTable element
	And I should see "7" in the entryTable element
	And I should see "8" in the entryTable element
	And I should see "35" in the entryTable element
	And I should see "20" in the entryTable element
	And I should see "30" in the entryTable element
	And I should see "40" in the entryTable element
	And I should see "50" in the entryTable element
	And I should see "60" in the entryTable element
	And I should see "70" in the entryTable element
	And I should see "18" in the entryTable element
	And I should see "288" in the entryTable element
	And I should see "NJ7-1"
	And I should see "NJ7-2"
	And I should see "Category" in the entryTable element
	And I should see "Monday" in the entryTable element
	And I should see "Tuesday" in the entryTable element
	And I should see "Wednesday" in the entryTable element
	And I should see "Thursday" in the entryTable element
	And I should see "Friday" in the entryTable element
	And I should see "Saturday" in the entryTable element
	And I should see "Sunday" in the entryTable element
	And I should see "A Facility 2return of the facility Total" in the entryTable element
	And I should see "A Facility Total" in the entryTable element
	And I should see "288.000" in the entryTable element
	And I should see "35.000" in the entryTable element
	And I should see "You know the sound a fork makes in the garbage disposal? That’s the sound my brain makes" in the entryTable element
	When I click ok in the dialog after pressing "Validate and submit"
	Then I should be at the show page for system delivery entry "editentry"
	And I should see the notification message "This record is now locked, if changes need to be made please enter a adjustment."

Scenario: user with approval can edit entry and fill out form entries and validate
	Given I am logged in as "validater"
	When I visit the Edit page for system delivery entry "editentry"
	Then I should see "You know the sound a fork makes in the garbage disposal? That’s the sound my brain makes"
	When I enter 2.00 into the FacilityEntries_0__EntryValue field
	And I enter 3.00 into the FacilityEntries_1__EntryValue field
	And I enter 4.00 into the FacilityEntries_2__EntryValue field
	And I enter 5.00 into the FacilityEntries_3__EntryValue field
	And I enter 6.00 into the FacilityEntries_4__EntryValue field
	And I enter 7.00 into the FacilityEntries_5__EntryValue field
	And I enter 8.00 into the FacilityEntries_6__EntryValue field
	And I enter 20.00 into the FacilityEntries_7__EntryValue field
	And I enter 30.00 into the FacilityEntries_8__EntryValue field
	And I enter 40.00 into the FacilityEntries_9__EntryValue field
	And I enter 50.00 into the FacilityEntries_10__EntryValue field
	And I enter 60.00 into the FacilityEntries_11__EntryValue field
	And I enter 70.00 into the FacilityEntries_12__EntryValue field
	And I enter 18.00 into the FacilityEntries_13__EntryValue field
	And I press "Save" 
	Then I should be at the Show page for system delivery entry "editentry"
	And I should see a display for WeekOf with "12/14/2020"
	And I should see a display for IsValidated with "n/a"
    And I should see the notification message "This record has not been validated"
	And I should see "NJ7-1 - A Facility, NJ7-2 - A Facility 2return of the facility"
	And I should see "NJ7 -" in the entryTable element
	And I should see "Water" in the entryTable element
	And I should see "2" in the entryTable element
	And I should see "3" in the entryTable element
	And I should see "4" in the entryTable element
	And I should see "5" in the entryTable element
	And I should see "6" in the entryTable element
	And I should see "7" in the entryTable element
	And I should see "8" in the entryTable element
	And I should see "35" in the entryTable element
	And I should see "20" in the entryTable element
	And I should see "30" in the entryTable element
	And I should see "40" in the entryTable element
	And I should see "50" in the entryTable element
	And I should see "60" in the entryTable element
	And I should see "70" in the entryTable element
	And I should see "18" in the entryTable element
	And I should see "288" in the entryTable element
	And I should see "NJ7-1"
	And I should see "NJ7-2"
	And I should see "Category" in the entryTable element
	And I should see "Monday" in the entryTable element
	And I should see "Tuesday" in the entryTable element
	And I should see "Wednesday" in the entryTable element
	And I should see "Thursday" in the entryTable element
	And I should see "Friday" in the entryTable element
	And I should see "Saturday" in the entryTable element
	And I should see "Sunday" in the entryTable element
	And I should see "A Facility 2return of the facility Total" in the entryTable element
	And I should see "A Facility Total" in the entryTable element
	And I should see "288.000" in the entryTable element
	And I should see "35.000" in the entryTable element
	Then I should see "You know the sound a fork makes in the garbage disposal? That’s the sound my brain makes" in the entryTable element
	When I click ok in the dialog after pressing "Validate and submit"
	Then I should be at the show page for system delivery entry "editentry"
	And I should see the notification message "This record is now locked, if changes need to be made please enter a adjustment."

Scenario: User should see a validation fail if week of is not a Monday
	Given I am logged in as "user"
	When I visit the Production/SystemDeliveryEntry/New page
	And I select operating center "nj7" from the OperatingCenters dropdown
	And I select system delivery type "water" from the SystemDeliveryType dropdown
	And I select facility "one"'s FacilityIdWithFacilityName from the Facilities dropdown
	And I select facility "two"'s FacilityIdWithFacilityName from the Facilities dropdown
	And I enter "12/10/2020" into the WeekOf field
	And I press "Select"
	Then I should see "Week Of Date must be a Monday and must not be in the future."

Scenario: User should see a validation fail if week of is a Monday but in the Future
	Given I am logged in as "user"
	When I visit the Production/SystemDeliveryEntry/New page
	And I select operating center "nj7" from the OperatingCenters dropdown
	And I select system delivery type "water" from the SystemDeliveryType dropdown
	And I select facility "one"'s FacilityIdWithFacilityName from the Facilities dropdown
	And I select facility "two"'s FacilityIdWithFacilityName from the Facilities dropdown
	And I enter "12/16/2030" into the WeekOf field
	And I press "Select"
	Then I should see "Week Of Date must be a Monday and must not be in the future."

Scenario: User should see a validation fail if an entry already exists for operating center / facility / WeekOf
	Given I am logged in as "user"
	When I visit the Production/SystemDeliveryEntry/New page
	And I enter "12/14/2020" into the WeekOf field
	And I select operating center "nj7" from the OperatingCenters dropdown
	And I select system delivery type "water" from the SystemDeliveryType dropdown
	And I select facility "one"'s FacilityIdWithFacilityName from the Facilities dropdown
	And I select facility "two"'s FacilityIdWithFacilityName from the Facilities dropdown
	And I press "Select"
	Then I should see "Entry already exists for this week for operating center / facility"

Scenario: When a user presses distribute on edit it evenly distributes the weekly total into each days editor except when weekly total is empty
	Given I am logged in as "user"
	When I visit the Edit page for system delivery entry "editentry"
	And I enter 99.99 into the FacilityEntries_0__EntryValue field
	And I enter 99.99 into the FacilityEntries_2__EntryValue field
	And I enter 99.99 into the FacilityEntries_4__EntryValue field
	And I enter 800.00 into the FacilityEntries_13__WeeklyTotal field
	And I press "Weekly Distribution"
	Then I should see "99.99" in the FacilityEntries_0__EntryValue field
	And I should see "6.661" in the FacilityEntries_1__EntryValue field 
	And I should see "99.99" in the FacilityEntries_2__EntryValue field
	And I should see "2.122" in the FacilityEntries_3__EntryValue field
	And I should see "99.99" in the FacilityEntries_4__EntryValue field
	And I should see "4.726" in the FacilityEntries_5__EntryValue field
	And I should see "9.121" in the FacilityEntries_6__EntryValue field
	And I should see "114.28571" in the FacilityEntries_7__EntryValue field
	And I should see "114.28571" in the FacilityEntries_8__EntryValue field
	And I should see "114.28571" in the FacilityEntries_9__EntryValue field
	And I should see "114.28571" in the FacilityEntries_10__EntryValue field
	And I should see "114.28571" in the FacilityEntries_11__EntryValue field
	And I should see "114.28571" in the FacilityEntries_12__EntryValue field
	And I should see "114.28571" in the FacilityEntries_13__EntryValue field
	
Scenario: user can copy a system delivery entry and it takes them to the new page with values prefilled
	Given I am logged in as "user"
	And a facility system delivery entry type "water" exists with facility: "one", system delivery entry type: "wastewater collected", is enabled: true, minimum value: 1.618, maximum value: 100.141
	And facility system delivery entry type "water" exists in facility "one"
	And operating center: "nj7" exists in public water supply: "water"
	When I visit the Production/SystemDeliveryEntry/New page
	When I enter "12/7/2020" into the WeekOf field	
	When I select operating center "nj7" from the OperatingCenters dropdown
	When I select system delivery type "water" from the SystemDeliveryType dropdown
	When I select facility "one"'s FacilityIdWithFacilityName from the Facilities dropdown
	And I press "Select"
	Then the currently shown system delivery entry shall henceforth be known throughout the land as "Carl"
	And I should be at the Edit page for system delivery entry "Carl"
	When I visit the show page for system delivery entry "Carl"
	Then I should see the button "Copy"
    When I click ok in the dialog after pressing "Copy"
	Then operating center "nj7" should be selected in the OperatingCenters dropdown
	And facility "one" should be selected in the Facilities dropdown
	And public water supply "water" should be selected in the PublicWaterSupplies dropdown
	When I enter "08/16/2021" into the WeekOf field
	And I press "Select"
	Then the currently shown system delivery entry shall henceforth be known throughout the land as "Copy"
	And I should be at the Edit page for system delivery entry "Copy"
	And I should see "NJ7-1" 
	And I should see "NJ7-2"

Scenario: User with edit rights can see and enter reversals on a locked entry
	Given a system delivery entry "reversal" exists with IsValidated: "true"
	And facility "one" exists in system delivery entry "reversal"
	And operating center "nj7" exists in system delivery entry "reversal"
	And a system delivery facility entry "monday" exists with system delivery entry: "reversal", system delivery entry type: "delivered water", facility: "one", entry date: 7/11/2022, entry value: 3.251
	And a system delivery facility entry "tuesday" exists with system delivery entry: "reversal", system delivery entry type: "delivered water", facility: "one", entry date: 7/12/2022, entry value: 6.661
	And a system delivery facility entry "wednesday" exists with system delivery entry: "reversal", system delivery entry type: "delivered water", facility: "one", entry date: 7/13/2022, entry value: 8.212
	And a system delivery facility entry "thursday" exists with system delivery entry: "reversal", system delivery entry type: "delivered water", facility: "one", entry date: 7/14/2022, entry value: 2.122
	And a system delivery facility entry "friday" exists with system delivery entry: "reversal", system delivery entry type: "delivered water", facility: "one", entry date: 7/15/2022, entry value: 12.116
	And a system delivery facility entry "saturday" exists with system delivery entry: "reversal", system delivery entry type: "delivered water", facility: "one", entry date: 7/16/2022, entry value: 4.726
	And a system delivery facility entry "sunday" exists with system delivery entry: "reversal", system delivery entry type: "delivered water", facility: "one", entry date: 7/17/2022, entry value: 30.121
	And I am logged in as "user"
	When I visit the Show page for system delivery entry "reversal"
	Then I should see "Adjustments are in red; For adjustment values see the adjustments tab." in the reversalNote element
	Then I should see the "Adjustments" tab
	When I click the "Adjustments" tab 
	And I press "Make Adjustment"
	And I check the FacilityEntries_0__IsBeingAdjusted field
	And I enter "2.500" into the FacilityEntries_0__AdjustedEntryValue field 
    And I enter "new comment1" into the FacilityEntries_0__AdjustmentComment field
	And I check the FacilityEntries_2__IsBeingAdjusted field
	And I enter "5.360" into the FacilityEntries_2__AdjustedEntryValue field 
	And I enter "new comment2" into the FacilityEntries_2__AdjustmentComment field
	And I check the FacilityEntries_4__IsBeingAdjusted field
	And I enter "10.160" into the FacilityEntries_4__AdjustedEntryValue field 
	And I enter "new comment3" into the FacilityEntries_4__AdjustmentComment field
	And I check the FacilityEntries_6__IsBeingAdjusted field
	And I enter "19.120" into the FacilityEntries_6__AdjustedEntryValue field 
	And I enter "new comment4" into the FacilityEntries_6__AdjustmentComment field
	And I click ok in the dialog after pressing "Add Adjustments"
	Then I should be at the Show page for system delivery entry "reversal"
	When I click the "Adjustments" tab 
	Then I should see the following values in the system-delivery-entry-reversals-table table
         | Operating Center | Facility           | Entered By                 | Date For Adjustment | Original Entry Value       | Updated Value | Comment        |
         | NJ7 -            | A Facility - NJ7-1 | employee: "one"'s FullName |  *7/11/2022*          | 3.251                      | 2.500         | new comment1   |
         | NJ7 -            | A Facility - NJ7-1 | employee: "one"'s FullName |  *7/13/2022*          | 8.212                      | 5.360         | new comment2   |
         | NJ7 -            | A Facility - NJ7-1 | employee: "one"'s FullName |  *7/15/2022*          | 12.116                     | 10.160        | new comment3   |
         | NJ7 -            | A Facility - NJ7-1 | employee: "one"'s FullName |  *7/17/2022*          | 30.121                     | 19.120        | new comment4   |
	When I click the "System Delivery Entry" tab
	Then I should see "2.500" in the entryTable element
	And I should see "6.661" in the entryTable element
	And I should see "5.36" in the entryTable element
	And I should see "2.122" in the entryTable element
	And I should see "10.16" in the entryTable element
	And I should see "4.726" in the entryTable element
	And I should see "19.12" in the entryTable element
	And I should see "50.649" in the entryTable element
	And I should see "A Facility Total" in the entryTable element
	And I should see "NJ7 - Total" in the entryTable element

Scenario: User should see first entry value as updated value on the reversals screen
	Given a system delivery entry "reversal" exists with IsValidated: "true"
	And facility "one" exists in system delivery entry "reversal"
	And operating center "nj7" exists in system delivery entry "reversal"
	And a system delivery facility entry "monday" exists with system delivery entry: "reversal", system delivery entry type: "delivered water", facility: "one", entry date: 7/11/2022, entry value: 3.251
	And a system delivery facility entry "tuesday" exists with system delivery entry: "reversal", system delivery entry type: "delivered water", facility: "one", entry date: 7/12/2022, entry value: 6.661
	And a system delivery facility entry "wednesday" exists with system delivery entry: "reversal", system delivery entry type: "delivered water", facility: "one", entry date: 7/13/2022, entry value: 8.212
	And a system delivery facility entry "thursday" exists with system delivery entry: "reversal", system delivery entry type: "delivered water", facility: "one", entry date: 7/14/2022, entry value: 2.122
	And a system delivery facility entry "friday" exists with system delivery entry: "reversal", system delivery entry type: "delivered water", facility: "one", entry date: 7/15/2022, entry value: 12.116
	And a system delivery facility entry "saturday" exists with system delivery entry: "reversal", system delivery entry type: "delivered water", facility: "one", entry date: 7/16/2022, entry value: 4.726
	And a system delivery facility entry "sunday" exists with system delivery entry: "reversal", system delivery entry type: "delivered water", facility: "one", entry date: 7/17/2022, entry value: 30.121
	And I am logged in as "user"
	When I visit the Show page for system delivery entry "reversal"
	Then I should see the "Adjustments" tab
	When I click the "Adjustments" tab 
	And I press "Make Adjustment"
	And I check the FacilityEntries_0__IsBeingAdjusted field
	And I enter "2.500" into the FacilityEntries_0__AdjustedEntryValue field
	And I enter "new comment" into the FacilityEntries_0__AdjustmentComment field
	And I click ok in the dialog after pressing "Add Adjustments"
	Then I should be at the Show page for system delivery entry "reversal"
	When I click the "Adjustments" tab 
	Then I should see the following values in the system-delivery-entry-reversals-table table
         | Operating Center | Facility           | Entered By                 | Date For Adjustment | Original Entry Value | Updated Value | Comment        |
         | NJ7 -            | A Facility - NJ7-1 | employee: "one"'s FullName | *7/11/2022*          | 3.251                | 2.500         | new comment    |
	When I press "Make Adjustment"
	And I check the FacilityEntries_0__IsBeingAdjusted field
	And I enter "1.700" into the FacilityEntries_0__AdjustedEntryValue field
	And I enter "new comment1" into the FacilityEntries_0__AdjustmentComment field
	And I click ok in the dialog after pressing "Add Adjustments"
	Then I should be at the Show page for system delivery entry "reversal"
	When I click the "Adjustments" tab 
	Then I should see the following values in the system-delivery-entry-reversals-table table
         | Operating Center | Facility           | Entered By                 | Date For Adjustment | Original Entry Value | Updated Value | Comment        |
         | NJ7 -            | A Facility - NJ7-1 | employee: "one"'s FullName | *7/11/2022*          | 3.251                | 2.500         | new comment    |
         | NJ7 -            | A Facility - NJ7-1 | employee: "one"'s FullName | *7/11/2022*          | 2.500                | 1.700         | new comment1   |

Scenario: User should see a validation error if updated value on reversal is out of range
	Given a system delivery entry "reversal" exists with IsValidated: "true"
	And facility "one" exists in system delivery entry "reversal"
	And operating center "nj7" exists in system delivery entry "reversal"
	And a system delivery facility entry "monday" exists with system delivery entry: "reversal", system delivery entry type: "delivered water", facility: "one", entry date: 7/11/2022, entry value: 3.251
	And a system delivery facility entry "tuesday" exists with system delivery entry: "reversal", system delivery entry type: "delivered water", facility: "one", entry date: 7/12/2022, entry value: 6.661
	And a system delivery facility entry "wednesday" exists with system delivery entry: "reversal", system delivery entry type: "delivered water", facility: "one", entry date: 7/13/2022, entry value: 8.212
	And a system delivery facility entry "thursday" exists with system delivery entry: "reversal", system delivery entry type: "delivered water", facility: "one", entry date: 7/14/2022, entry value: 2.122
	And a system delivery facility entry "friday" exists with system delivery entry: "reversal", system delivery entry type: "delivered water", facility: "one", entry date: 7/15/2022, entry value: 12.116
	And a system delivery facility entry "saturday" exists with system delivery entry: "reversal", system delivery entry type: "delivered water", facility: "one", entry date: 7/16/2022, entry value: 4.726
	And a system delivery facility entry "sunday" exists with system delivery entry: "reversal", system delivery entry type: "delivered water", facility: "one", entry date: 7/17/2022, entry value: 30.121
	And I am logged in as "user"
	When I visit the Show page for system delivery entry "reversal"
	Then I should see the "Adjustments" tab
	When I click the "Adjustments" tab 
	And I press "Make Adjustment"
	And I check the FacilityEntries_0__IsBeingAdjusted field
	And I enter "42.5" into the FacilityEntries_0__AdjustedEntryValue field
	And I enter "new comment1" into the FacilityEntries_0__AdjustmentComment field
	And I check the FacilityEntries_2__IsBeingAdjusted field
	And I enter "5.36" into the FacilityEntries_2__AdjustedEntryValue field
	And I enter "new comment2" into the FacilityEntries_2__AdjustmentComment field
	And I check the FacilityEntries_4__IsBeingAdjusted field
	And I enter "10.16" into the FacilityEntries_4__AdjustedEntryValue field
	And I enter "new comment3" into the FacilityEntries_4__AdjustmentComment field
	And I check the FacilityEntries_6__IsBeingAdjusted field
	And I enter "19.12" into the FacilityEntries_6__AdjustedEntryValue field
	And I enter "new comment4" into the FacilityEntries_6__AdjustmentComment field
	And I click ok in the dialog after pressing "Add Adjustments"
	And I wait for the page to reload
	Then I should see "Value not within range, please correct."

Scenario: User should see a validation error if Comment character exceeds more than 100 characters 
	Given a system delivery entry "reversal" exists with IsValidated: "true"
	And facility "one" exists in system delivery entry "reversal"
	And operating center "nj7" exists in system delivery entry "reversal"
	And a system delivery facility entry "monday" exists with system delivery entry: "reversal", system delivery entry type: "delivered water", facility: "one", entry date: 7/11/2022, entry value: 3.251
	And a system delivery facility entry "tuesday" exists with system delivery entry: "reversal", system delivery entry type: "delivered water", facility: "one", entry date: 7/12/2022, entry value: 6.661
	And a system delivery facility entry "wednesday" exists with system delivery entry: "reversal", system delivery entry type: "delivered water", facility: "one", entry date: 7/13/2022, entry value: 8.212
	And a system delivery facility entry "thursday" exists with system delivery entry: "reversal", system delivery entry type: "delivered water", facility: "one", entry date: 7/14/2022, entry value: 2.122
	And a system delivery facility entry "friday" exists with system delivery entry: "reversal", system delivery entry type: "delivered water", facility: "one", entry date: 7/15/2022, entry value: 12.116
	And a system delivery facility entry "saturday" exists with system delivery entry: "reversal", system delivery entry type: "delivered water", facility: "one", entry date: 7/16/2022, entry value: 4.726
	And a system delivery facility entry "sunday" exists with system delivery entry: "reversal", system delivery entry type: "delivered water", facility: "one", entry date: 7/17/2022, entry value: 30.121
	And I am logged in as "user"
	When I visit the Show page for system delivery entry "reversal"
	Then I should see the "Adjustments" tab
	When I click the "Adjustments" tab 
	And I press "Make Adjustment"
	And I check the FacilityEntries_0__IsBeingAdjusted field
	And I enter "5.5" into the FacilityEntries_0__AdjustedEntryValue field
	And I enter "This has been created as per the business requirement. Initially there is no funstionality and was added later" into the FacilityEntries_0__AdjustmentComment field
	And I click ok in the dialog after pressing "Add Adjustments"
	And I wait for the page to reload
	Then I should see "Comment cannot exceed 100 characters."
	
Scenario: User can enter a transfer
	Given a facility "four" exists with operating center: "nj7", facility id: "NJSB-3", facility name: "A Facility 3", point of entry: "true", system delivery type: "water", regional planning area: "abc-123Testing"
	And a facility "five" exists with operating center: "nj7", facility id: "NJSB-4", facility name: "A Facility 4", point of entry: "true", system delivery type: "water", regional planning area: "abc-123Testing"
	And a facility system delivery entry type "four" exists with facility: "four", system delivery entry type: "transferred to", supplier facility: "four", is enabled: true, minimum value: 1.618, maximum value: 20.141
	And a facility system delivery entry type "five" exists with facility: "five", system delivery entry type: "delivered water", is enabled: true, minimum value: 1.618, maximum value: 120.141
	And I am logged in as "user"
	When I visit the Production/SystemDeliveryEntry/New page
	And I enter "12/7/2020" into the WeekOf field
	And I select operating center "nj7" from the OperatingCenters dropdown
	And I select system delivery type "water" from the SystemDeliveryType dropdown
	When I select facility "four"'s FacilityIdWithFacilityName from the Facilities dropdown
	And I press "Select"
	Then the currently shown system delivery entry shall henceforth be known throughout the land as "Carl"
	And I should be at the Edit page for system delivery entry "Carl"
	Then I should see "NJ7-6"
	When I enter 100.00 into the FacilityEntries_6__WeeklyTotal field
	And I press "Weekly Distribution"
	
	# Test Supplier Facility Name (A Facility 3 - NJ7-5) is visible when validation errors occur
	And I enter 21 into the FacilityEntries_0__EntryValue field
	And I press "Save" 
	Then I should see "Value not within range, please correct."
	And I should see "A Facility 3 - NJ7-6"
	When I press "Weekly Distribution"
	And I press "Save"
	Then I should be at the Show page for system delivery entry "Carl"
	Then I should see "14.286"
	And I should see "-14.286"

Scenario: User can reverse a transfer
	Given a system delivery entry "reversal" exists with IsValidated: "true"
	And facility "one" exists in system delivery entry "reversal"
	And a facility system delivery entry type "four" exists with facility: "one", system delivery entry type: "transferred to", is enabled: true, minimum value: 1.618, maximum value: 20.141
	And a facility "supplier" exists with operating center: "nj7", facility id: "NJSB-5", facility name: "A Facility 5", point of entry: "true", system delivery type: "water"
	And a facility system delivery entry type "five" exists with facility: "supplier", system delivery entry type: "transferred from", is enabled: true, minimum value: 1.618, maximum value: 20.141
	And operating center "nj7" exists in system delivery entry "reversal"
	And a system delivery facility entry "monday1" exists with system delivery entry: "reversal", system delivery entry type: "transferred to", facility: "one", entry date: 7/11/2022, entry value: 3.251, supplier facility: "supplier"
	And a system delivery facility entry "tuesday1" exists with system delivery entry: "reversal", system delivery entry type: "transferred to", facility: "one", entry date: 7/12/2022, entry value: 6.661, supplier facility: "supplier"
	And a system delivery facility entry "wednesday1" exists with system delivery entry: "reversal", system delivery entry type: "transferred to", facility: "one", entry date: 7/13/2022, entry value: 8.212, supplier facility: "supplier"
	And a system delivery facility entry "thursday1" exists with system delivery entry: "reversal", system delivery entry type: "transferred to", facility: "one", entry date: 7/14/2022, entry value: 2.122, supplier facility: "supplier"
	And a system delivery facility entry "friday1" exists with system delivery entry: "reversal", system delivery entry type: "transferred to", facility: "one", entry date: 7/15/2022, entry value: 12.116, supplier facility: "supplier"
	And a system delivery facility entry "saturday1" exists with system delivery entry: "reversal", system delivery entry type: "transferred to", facility: "one", entry date: 7/16/2022, entry value: 4.726, supplier facility: "supplier"
	And a system delivery facility entry "sunday1" exists with system delivery entry: "reversal", system delivery entry type: "transferred to", facility: "one", entry date: 7/17/2022, entry value: 30.121, supplier facility: "supplier"
	And a system delivery facility entry "monday2" exists with system delivery entry: "reversal", system delivery entry type: "transferred from", facility: "supplier", entry date: 7/11/2022, entry value: -3.251
	And a system delivery facility entry "tuesday2" exists with system delivery entry: "reversal", system delivery entry type: "transferred from", facility: "supplier", entry date: 7/12/2022, entry value: -6.661
	And a system delivery facility entry "wednesday2" exists with system delivery entry: "reversal", system delivery entry type: "transferred from", facility: "supplier", entry date: 7/13/2022, entry value: -8.212
	And a system delivery facility entry "thursday2" exists with system delivery entry: "reversal", system delivery entry type: "transferred from", facility: "supplier", entry date: 7/14/2022, entry value: -2.122
	And a system delivery facility entry "friday2" exists with system delivery entry: "reversal", system delivery entry type: "transferred from", facility: "supplier", entry date: 7/15/2022, entry value: -12.116
	And a system delivery facility entry "saturday2" exists with system delivery entry: "reversal", system delivery entry type: "transferred from", facility: "supplier", entry date: 7/16/2022, entry value: -4.726
	And a system delivery facility entry "sunday2" exists with system delivery entry: "reversal", system delivery entry type: "transferred from", facility: "supplier", entry date: 7/17/2022, entry value: -30.121
	And I am logged in as "user"
	When I visit the Show page for system delivery entry "reversal"
	Then I should see the "Adjustments" tab
	When I click the "Adjustments" tab 
	And I press "Make Adjustment"
	And I check the FacilityEntries_0__IsBeingAdjusted field
	And I enter "2.500" into the FacilityEntries_0__AdjustedEntryValue field
	And I enter "new comment" into the FacilityEntries_0__AdjustmentComment field
	And I click ok in the dialog after pressing "Add Adjustments"
	Then I should be at the Show page for system delivery entry "reversal"
	When I click the "Adjustments" tab 
	Then I should see the following values in the system-delivery-entry-reversals-table table
         | Operating Center | Facility             | Entered By                 | Date For Adjustment | Original Entry Value | Updated Value | Comment     |
         | NJ7 -            | A Facility - NJ7-1   | employee: "one"'s FullName | 7/11/2022           | 3.251                | 2.500         | new comment |
         | NJ7 -            | A Facility 5 - NJ7-6 | employee: "one"'s FullName | 7/11/2022           | -3.251               | -2.500        | new comment |

Scenario: User can search and see public water supply value for all system delivery entry types
	Given I am logged in as "user"
	And a system delivery entry "entry" exists with IsValidated: "true"
	And facility "one" exists in system delivery entry "entry"
	And operating center "nj7" exists in system delivery entry "entry"
	And a system delivery facility entry "monday" exists with system delivery entry: "entry", system delivery entry type: "delivered water", facility: "one", entry date: 7/11/2022, entry value: 3.251
	And a system delivery facility entry "tuesday" exists with system delivery entry: "entry", system delivery entry type: "delivered water", facility: "one", entry date: 7/12/2022, entry value: 6.661
	And a system delivery facility entry "wednesday" exists with system delivery entry: "entry", system delivery entry type: "delivered water", facility: "one", entry date: 7/13/2022, entry value: 8.212
	And a system delivery facility entry "thursday" exists with system delivery entry: "entry", system delivery entry type: "delivered water", facility: "one", entry date: 7/14/2022, entry value: 2.122
	And a system delivery facility entry "friday" exists with system delivery entry: "entry", system delivery entry type: "delivered water", facility: "one", entry date: 7/15/2022, entry value: 12.116
	And a system delivery facility entry "saturday" exists with system delivery entry: "entry", system delivery entry type: "delivered water", facility: "one", entry date: 7/16/2022, entry value: 4.726
	And a system delivery facility entry "sunday" exists with system delivery entry: "entry", system delivery entry type: "delivered water", facility: "one", entry date: 7/17/2022, entry value: 30.121
	When I visit the Production/SystemDeliveryEntry/Search page 
	And  I select state "nj" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown 
	And I press "Search"
	Then I should see the table-caption "Records found: 7"
	Then I should see a link to the Show page for system delivery entry: "entry"
	And I should see "1111 - nj7 -"	

Scenario: User can search and selection is limited
	Given I am logged in as "user"
	And a operating center "nj9" exists with opcode: "NJ9", state: "nj"
	And a facility "five" exists with operating center: "nj9", facility id: "NJSB-2", facility name: "A Facility 5: This is the way", point of entry: "true", system delivery type: "water"
	And a system delivery entry "entry" exists with IsValidated: "true", IsHyperionFileCreated: "true"
	And a system delivery entry "otherentry" exists with IsValidated: "true", IsHyperionFileCreated: "false"
	And facility "one" exists in system delivery entry "entry"
	And operating center "nj7" exists in system delivery entry "entry"
	And operating center "nj9" exists in system delivery entry "otherentry"
	And facility "five" exists in system delivery entry "otherentry"
	And a system delivery facility entry "monday1" exists with system delivery entry: "entry", system delivery entry type: "delivered water", facility: "one", entry date: 7/11/2022, entry value: 3.251
	And a system delivery facility entry "tuesday1" exists with system delivery entry: "entry", system delivery entry type: "delivered water", facility: "one", entry date: 7/12/2022, entry value: 6.661
	And a system delivery facility entry "wednesday1" exists with system delivery entry: "entry", system delivery entry type: "delivered water", facility: "one", entry date: 7/13/2022, entry value: 8.212
	And a system delivery facility entry "thursday1" exists with system delivery entry: "entry", system delivery entry type: "delivered water", facility: "one", entry date: 7/14/2022, entry value: 2.122
	And a system delivery facility entry "friday1" exists with system delivery entry: "entry", system delivery entry type: "delivered water", facility: "one", entry date: 7/15/2022, entry value: 12.116
	And a system delivery facility entry "saturday1" exists with system delivery entry: "entry", system delivery entry type: "delivered water", facility: "one", entry date: 7/16/2022, entry value: 4.726
	And a system delivery facility entry "sunday1" exists with system delivery entry: "entry", system delivery entry type: "delivered water", facility: "one", entry date: 7/17/2022, entry value: 30.121
	And a system delivery facility entry "monday2" exists with system delivery entry: "otherentry", system delivery entry type: "delivered water", facility: "five", entry date: 7/11/2022, entry value: 3.251
	And a system delivery facility entry "tuesday2" exists with system delivery entry: "otherentry", system delivery entry type: "delivered water", facility: "five", entry date: 7/12/2022, entry value: 6.661
	And a system delivery facility entry "wednesday2" exists with system delivery entry: "otherentry", system delivery entry type: "delivered water", facility: "five", entry date: 7/13/2022, entry value: 8.212
	And a system delivery facility entry "thursday2" exists with system delivery entry: "otherentry", system delivery entry type: "delivered water", facility: "five", entry date: 7/14/2022, entry value: 2.122
	And a system delivery facility entry "friday2" exists with system delivery entry: "otherentry", system delivery entry type: "delivered water", facility: "five", entry date: 7/15/2022, entry value: 12.116
	And a system delivery facility entry "saturday2" exists with system delivery entry: "otherentry", system delivery entry type: "delivered water", facility: "five", entry date: 7/16/2022, entry value: 4.726
	And a system delivery facility entry "sunday2" exists with system delivery entry: "otherentry", system delivery entry type: "delivered water", facility: "five", entry date: 7/17/2022, entry value: 30.121
	When I visit the Production/SystemDeliveryEntry/Search page 
	And I select state "nj" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown 
	And I press "Search"
	Then I should see a link to the show page for system delivery entry "entry"
	And I should not see a link to the show page for system delivery entry "otherentry"
	When I visit the Production/SystemDeliveryEntry/Search page
	And  I select state "nj" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select system delivery type "water" from the SystemDeliveryType dropdown
	And I select facility "one"'s FacilityIdWithFacilityName from the Facility dropdown 
	And I press "Search"
	Then I should see a link to the show page for system delivery entry "entry"
	And I should not see a link to the show page for system delivery entry "otherentry"
	When I visit the Production/SystemDeliveryEntry/Search page 
	And I press "Search"
	Then I should see a link to the show page for system delivery entry "entry"
	And I should see a link to the show page for system delivery entry "otherentry"
	
	# Test IsHyperionFileCreated search
	When I visit the Production/SystemDeliveryEntry/Search page
	And I select "Yes" from the IsHyperionFileCreated dropdown
	And I press "Search"
	Then I should see a link to the show page for system delivery entry "entry"
	And I should not see a link to the show page for system delivery entry "otherentry"
	
	# Test date range
	When I visit the Production/SystemDeliveryEntry/Search page
	And I select "=" from the EntryDate_Operator dropdown
	And I enter "07/16/2022" into the EntryDate_End field
	And I press "Search"
	Then I should see a link to the show page for system delivery entry "entry"
	And I should see "Records found: 4"
	And I should not see "7/15/2022"
	And I should see "7/16/2022"
	And I should not see "7/17/2022"

Scenario: User can search by WWSID 
	Given I am logged in as "user"
	And a facility system delivery entry type "wastewater" exists with facility: "three", system delivery entry type: "wastewater collected", is enabled: true, minimum value: 1.618, maximum value: 100.141
	And facility system delivery entry type "wastewater" exists in facility "three"
	When I visit the Production/SystemDeliveryEntry/New page
	When I enter "12/7/2020" into the WeekOf field	
	When I select operating center "nj7" from the OperatingCenters dropdown
	When I select system delivery type "waste water" from the SystemDeliveryType dropdown
	When I select facility "three"'s FacilityIdWithFacilityName from the Facilities dropdown
	And I press "Select"
	Then the currently shown system delivery entry shall henceforth be known throughout the land as "Carl"
	And I should be at the Edit page for system delivery entry "Carl"
	When I enter 12.00 into the FacilityEntries_0__EntryValue field
	And I enter 13.00 into the FacilityEntries_1__EntryValue field
	And I enter 13.00 into the FacilityEntries_2__EntryValue field
	And I enter 14.00 into the FacilityEntries_3__EntryValue field
	And I enter 15.00 into the FacilityEntries_4__EntryValue field
	And I enter 16.00 into the FacilityEntries_5__EntryValue field
	And I enter 17.00 into the FacilityEntries_6__EntryValue field
	And I press "Save" 
	Then I should be at the Show page for system delivery entry "Carl"
	When I visit the Production/SystemDeliveryEntry/Search page 
	And  I select state "nj" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown 
	And I select system delivery type "waste water" from the SystemDeliveryType dropdown
	And I select waste water system "waste water1" from the WasteWaterSystems dropdown
	And I select facility "three"'s FacilityIdWithFacilityName from the Facility dropdown 
	And I press "Search"
	Then I should see a link to the show page for system delivery entry "Carl"

Scenario: User can search by PWSID 
	Given I am logged in as "user"
	And a facility system delivery entry type "water" exists with facility: "one", system delivery entry type: "wastewater collected", is enabled: true, minimum value: 1.618, maximum value: 100.141
	And facility system delivery entry type "water" exists in facility "one"
	And operating center: "nj7" exists in public water supply: "water"
	When I visit the Production/SystemDeliveryEntry/New page
	When I enter "12/7/2020" into the WeekOf field	
	When I select operating center "nj7" from the OperatingCenters dropdown
	When I select system delivery type "water" from the SystemDeliveryType dropdown
	When I select facility "one"'s FacilityIdWithFacilityName from the Facilities dropdown
	And I press "Select"
	Then the currently shown system delivery entry shall henceforth be known throughout the land as "Carl"
	And I should be at the Edit page for system delivery entry "Carl"
	When I enter 12.00 into the FacilityEntries_0__EntryValue field
	And I enter 13.00 into the FacilityEntries_1__EntryValue field
	And I enter 13.00 into the FacilityEntries_2__EntryValue field
	And I enter 14.00 into the FacilityEntries_3__EntryValue field
	And I enter 15.00 into the FacilityEntries_4__EntryValue field
	And I enter 16.00 into the FacilityEntries_5__EntryValue field
	And I enter 17.00 into the FacilityEntries_6__EntryValue field
	And I press "Save" 
	Then I should be at the Show page for system delivery entry "Carl"
	When I visit the Production/SystemDeliveryEntry/Search page 
	And  I select state "nj" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown 
	And I select system delivery type "water" from the SystemDeliveryType dropdown
	And I select public water supply "water" from the PublicWaterSupplies dropdown
	And I select facility "one"'s FacilityIdWithFacilityName from the Facility dropdown 
	And I press "Search"
	Then I should see a link to the show page for system delivery entry "Carl"
	
Scenario: Admin can search and see all the entries
	Given I am logged in as "admin"
	And a operating center "nj9" exists with opcode: "NJ9", state: "nj"
	And a facility "five" exists with operating center: "nj9", facility id: "NJSB-2", facility name: "A Facility 5: This is the way", point of entry: "true", system delivery type: "water"
	And a system delivery entry "entry" exists with IsValidated: "true"
	And a system delivery entry "otherentry" exists with IsValidated: "true"
	And a facility system delivery entry type "five" exists with facility: "five", system delivery entry type: "delivered water", is enabled: true, minimum value: 1.618, maximum value: 20.141
	And facility system delivery entry type "five" exists in facility "five"
	And facility "one" exists in system delivery entry "entry"
	And operating center "nj7" exists in system delivery entry "entry"
	And operating center "nj9" exists in system delivery entry "otherentry"
	And facility "five" exists in system delivery entry "otherentry"
	And a system delivery facility entry "monday1" exists with system delivery entry: "entry", system delivery entry type: "delivered water", facility: "one", entry date: 7/11/2022, entry value: 3.251
	And a system delivery facility entry "tuesday1" exists with system delivery entry: "entry", system delivery entry type: "delivered water", facility: "one", entry date: 7/12/2022, entry value: 6.661
	And a system delivery facility entry "wednesday1" exists with system delivery entry: "entry", system delivery entry type: "delivered water", facility: "one", entry date: 7/13/2022, entry value: 8.212
	And a system delivery facility entry "thursday1" exists with system delivery entry: "entry", system delivery entry type: "delivered water", facility: "one", entry date: 7/14/2022, entry value: 2.122
	And a system delivery facility entry "friday1" exists with system delivery entry: "entry", system delivery entry type: "delivered water", facility: "one", entry date: 7/15/2022, entry value: 12.116
	And a system delivery facility entry "saturday1" exists with system delivery entry: "entry", system delivery entry type: "delivered water", facility: "one", entry date: 7/16/2022, entry value: 4.726
	And a system delivery facility entry "sunday1" exists with system delivery entry: "entry", system delivery entry type: "delivered water", facility: "one", entry date: 7/17/2022, entry value: 30.121
	And a system delivery facility entry "monday2" exists with system delivery entry: "otherentry", system delivery entry type: "delivered water", facility: "five", entry date: 7/11/2022, entry value: 3.251
	And a system delivery facility entry "tuesday2" exists with system delivery entry: "otherentry", system delivery entry type: "delivered water", facility: "five", entry date: 7/12/2022, entry value: 6.661
	And a system delivery facility entry "wednesday2" exists with system delivery entry: "otherentry", system delivery entry type: "delivered water", facility: "five", entry date: 7/13/2022, entry value: 8.212
	And a system delivery facility entry "thursday2" exists with system delivery entry: "otherentry", system delivery entry type: "delivered water", facility: "five", entry date: 7/14/2022, entry value: 2.122
	And a system delivery facility entry "friday2" exists with system delivery entry: "otherentry", system delivery entry type: "delivered water", facility: "five", entry date: 7/15/2022, entry value: 12.116
	And a system delivery facility entry "saturday2" exists with system delivery entry: "otherentry", system delivery entry type: "delivered water", facility: "five", entry date: 7/16/2022, entry value: 4.726
	And a system delivery facility entry "sunday2" exists with system delivery entry: "otherentry", system delivery entry type: "delivered water", facility: "five", entry date: 7/17/2022, entry value: 30.121
	When I visit the Production/SystemDeliveryEntry/Search page 
	And I select state "nj" from the State dropdown
	And I select operating center "nj9" from the OperatingCenter dropdown
	And I select system delivery type "water" from the SystemDeliveryType dropdown
	And I select facility "five"'s FacilityIdWithFacilityName from the Facility dropdown 
	And I press "Search"
	Then I should not see a link to the show page for system delivery entry "entry"
	And I should see a link to the show page for system delivery entry "otherentry"
	When I visit the Production/SystemDeliveryEntry/Search page 
	And I press "Search"
	Then I should see a link to the show page for system delivery entry "entry"
	And I should see a link to the show page for system delivery entry "otherentry"

Scenario: user with regular access should not be able to add an adjustment after the 3rd of the next month 
	Given a system delivery entry "facilityentry" exists with IsValidated: "true", WeekOf: "1/4/2020"
	And facility "one" exists in system delivery entry "facilityentry"
	And operating center "nj7" exists in system delivery entry "facilityentry"
	And a system delivery facility entry "monday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/11/2022, entry value: 3.251
	And a system delivery facility entry "tuesday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/12/2022, entry value: 6.661
	And a system delivery facility entry "wednesday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/13/2022, entry value: 8.212
	And a system delivery facility entry "thursday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/14/2022, entry value: 2.122
	And a system delivery facility entry "friday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/15/2022, entry value: 12.116
	And a system delivery facility entry "saturday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/16/2022, entry value: 4.726
	And a system delivery facility entry "sunday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/17/2022, entry value: 9.121
	And I am logged in as "user"
	And I am at the show page for system delivery entry "facilityentry"
	When I click the "Adjustments" tab 
	Then I should not see "Make Adjustment" 

Scenario: user with regular access should be able to add an adjustment before the 3rd of the next month 
	Given a system delivery entry "facilityentry" exists with IsValidated: "true"
	And facility "one" exists in system delivery entry "facilityentry"
	And operating center "nj7" exists in system delivery entry "facilityentry"
	And a system delivery facility entry "monday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/11/2022, entry value: 3.251
	And a system delivery facility entry "tuesday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/12/2022, entry value: 6.661
	And a system delivery facility entry "wednesday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/13/2022, entry value: 8.212
	And a system delivery facility entry "thursday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/14/2022, entry value: 2.122
	And a system delivery facility entry "friday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/15/2022, entry value: 12.116
	And a system delivery facility entry "saturday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/16/2022, entry value: 4.726
	And a system delivery facility entry "sunday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/17/2022, entry value: 9.121
	And I am logged in as "user"
	And I am at the show page for system delivery entry "facilityentry"
	When I click the "Adjustments" tab 
	Then I should see "Make Adjustment"

Scenario: user with validator access should be able to add an adjustment after the 3rd of the next month 
	Given a system delivery entry "facilityentry" exists with IsValidated: "true", WeekOf: "1/4/2020"
	And facility "one" exists in system delivery entry "facilityentry"
	And operating center "nj7" exists in system delivery entry "facilityentry"
	And a system delivery facility entry "monday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/11/2022, entry value: 3.251
	And a system delivery facility entry "tuesday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/12/2022, entry value: 6.661
	And a system delivery facility entry "wednesday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/13/2022, entry value: 8.212
	And a system delivery facility entry "thursday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/14/2022, entry value: 2.122
	And a system delivery facility entry "friday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/15/2022, entry value: 12.116
	And a system delivery facility entry "saturday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/16/2022, entry value: 4.726
	And a system delivery facility entry "sunday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/17/2022, entry value: 9.121
	And I am logged in as "validater"
	And I am at the show page for system delivery entry "facilityentry"
	When I click the "Adjustments" tab 
	Then I should see "Make Adjustment"

Scenario: admin should be able to add an adjustment after the 3rd of the next month 
	Given a system delivery entry "facilityentry" exists with IsValidated: "true", WeekOf: "1/4/2020"
	And facility "one" exists in system delivery entry "facilityentry"
	And operating center "nj7" exists in system delivery entry "facilityentry"
	And a system delivery facility entry "monday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/11/2022, entry value: 3.251
	And a system delivery facility entry "tuesday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/12/2022, entry value: 6.661
	And a system delivery facility entry "wednesday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/13/2022, entry value: 8.212
	And a system delivery facility entry "thursday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/14/2022, entry value: 2.122
	And a system delivery facility entry "friday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/15/2022, entry value: 12.116
	And a system delivery facility entry "saturday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/16/2022, entry value: 4.726
	And a system delivery facility entry "sunday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 7/17/2022, entry value: 9.121
	And I am logged in as "admin"
	And I am at the show page for system delivery entry "facilityentry"
	When I click the "Adjustments" tab 
	Then I should see "Make Adjustment"

Scenario: User can create/edit and view a entry for waste water entries
	Given a facility "wastewater" exists with operating center: "nj7", facility id: "NJSB-1", facility name: "Waste Water Facility", point of entry: "true", system delivery type: "waste water"
	And a facility system delivery entry type "wastewater" exists with facility: "wastewater", system delivery entry type: "wastewater collected", is enabled: true, minimum value: 1.618, maximum value: 20.141
	And facility system delivery entry type "wastewater" exists in facility "wastewater"
	And I am logged in as "user"
	When I visit the Production/SystemDeliveryEntry/New page
	And I enter "12/7/2020" into the WeekOf field
	And I press "Select"
	Then I should see a validation message for OperatingCenters with "The OperatingCenters field is required."
	When I select operating center "nj7" from the OperatingCenters dropdown
	And I press "Select"
	Then I should see a validation message for SystemDeliveryType with "The SystemDeliveryType field is required."
	When I select system delivery type "waste water" from the SystemDeliveryType dropdown
	And I press "Select"
	Then I should see a validation message for Facilities with "The Facilities field is required."
	When I select facility "wastewater"'s FacilityIdWithFacilityName from the Facilities dropdown
	And I press "Select"
	Then the currently shown system delivery entry shall henceforth be known throughout the land as "NotCarl"
	And I should be at the Edit page for system delivery entry "NotCarl"
	And I should see a display for WeekOf with "12/7/2020"
	When I enter "100.000" into the FacilityEntries_6__WeeklyTotal field 
	And I press "Weekly Distribution"
	And I press "Save"
	Then I should be at the Show page for system delivery entry "NotCarl"
	And I should see "NJ7-6 - Waste Water Facility"
	And I should see "n/a"
	And I should see "WasteWater collected" in the entryTable element
	And I should see "14.286" in the entryTable element
	And I should see "100.000" in the entryTable element
	And I should not see "Waste Water Facility Total" in the entryTable element
	And I should see "NJ7 - Total" in the entryTable element

Scenario: Validation should stop a third entry on a split week:
	Given a system delivery entry "first" exists with IsValidated: "false", WeekOf: "6/28/2021"
	And facility "one" exists in system delivery entry "first"
	And facility "two" exists in system delivery entry "first"
	And operating center "nj7" exists in system delivery entry "first"
	And a system delivery entry "second" exists with IsValidated: "false", WeekOf: "6/28/2021"
	And facility "one" exists in system delivery entry "second"
	And facility "two" exists in system delivery entry "second"
	And operating center "nj7" exists in system delivery entry "second"
	And I am logged in as "user"
	When I visit the Production/SystemDeliveryEntry/New page
	And I enter "6/28/2021" into the WeekOf field
	And I select operating center "nj7" from the OperatingCenters dropdown
	And I select system delivery type "water" from the SystemDeliveryType dropdown
	And I select facility "one"'s FacilityIdWithFacilityName from the Facilities dropdown
	And I select facility "two"'s FacilityIdWithFacilityName from the Facilities dropdown
	And I press "Select"
	Then I should see "Two entries already exist for this week for operating center / facility."

Scenario: User can enter multiple entries on a split week and each entry is a split week:
	Given I am logged in as "user"
	When I visit the Production/SystemDeliveryEntry/New page
	And I enter "6/28/2021" into the WeekOf field
	And I select operating center "nj7" from the OperatingCenters dropdown
	And I select system delivery type "water" from the SystemDeliveryType dropdown
	And I select facility "one"'s FacilityIdWithFacilityName from the Facilities dropdown
	And I select facility "two"'s FacilityIdWithFacilityName from the Facilities dropdown
	And I press "Select"
	Then the currently shown system delivery entry shall henceforth be known throughout the land as "Carl"
	And I should be at the Edit page for system delivery entry "Carl"
	And I should see the FacilityEntries_0__EntryValue field
	And I should see the FacilityEntries_1__EntryValue field
	And I should see the FacilityEntries_2__EntryValue field
	And I should see the FacilityEntries_3__EntryValue field
	And I should see the FacilityEntries_4__EntryValue field
	And I should see the FacilityEntries_5__EntryValue field
	And I should not see the FacilityEntries_6__EntryValue field
	And I should not see the FacilityEntries_7__EntryValue field
	And I should not see the FacilityEntries_8__EntryValue field
	And I should not see the FacilityEntries_9__EntryValue field
	And I should not see the FacilityEntries_10__EntryValue field
	And I should not see the FacilityEntries_11__EntryValue field
	And I should not see the FacilityEntries_12__EntryValue field
	And I should not see the FacilityEntries_13__EntryValue field
	And I should not see the FacilityEntries_14__EntryValue field

Scenario: user must see a message on the entry screen
	Given I am logged in as "user"
	When I visit the Edit page for system delivery entry "editentry"
	Then I should see "THE UNIT OF MEASURE FOR ALL ENTRIES IS THOUSAND GALLONS"
	
Scenario: Admin can delete a system delivery entry
	Given I am logged in as "admin"
	When I visit the Show page for system delivery entry: "one"
	And I click ok in the dialog after pressing "Delete"
	Then I should be at the Production/SystemDeliveryEntry/Search page
	When I try to access the Show page for system delivery entry: "one" expecting an error
	Then I should see a 404 error message
	
Scenario: User without delete role cannot delete system delivery entries
	Given I am logged in as "user"
	When I visit the Show page for system delivery entry: "editentry"
	Then I should not see the button "Delete"
	
Scenario: User Admin cannot delete system delivery entries
	Given I am logged in as "useradmin"
	When I visit the Show page for system delivery entry: "editentry"
	Then I should not see the button "Delete"

Scenario: System Delivery Admin User can adjust an entry after it has been sent to hyperion
	Given I am logged in as "useradmin"
	When I visit the Show page for system delivery entry "validated"
	Then I should see the "Adjustments" tab

Scenario: User cannot adjust an entry after it has been sent to hyperion
	Given I am logged in as "user"
	When I visit the Show page for system delivery entry "validated"
	Then I should not see the "Adjustments" tab
	
# This test exists due to a bug (MC-4971) that generated a sql error when attempting to delete a system delivery entry
# that contained adjustments.	
Scenario: Admin can delete a system delivery entry that contains adjustments
	Given a system delivery entry "facilityentry" exists with IsValidated: "true", WeekOf: "10/3/2022"
	And facility "one" exists in system delivery entry "facilityentry"
	And operating center "nj7" exists in system delivery entry "facilityentry"
	And a system delivery facility entry "monday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 10/3/2022, entry value: 3.251
	And a system delivery facility entry "tuesday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 10/4/2022, entry value: 6.661
	And a system delivery facility entry "wednesday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 10/5/2022, entry value: 8.212
	And a system delivery facility entry "thursday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 10/6/2022, entry value: 2.122
	And a system delivery facility entry "friday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 10/7/2022, entry value: 12.116
	And a system delivery facility entry "saturday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 10/8/2022, entry value: 4.726
	And a system delivery facility entry "sunday" exists with system delivery entry: "facilityentry", system delivery entry type: "delivered water", facility: "one", entry date: 10/9/2022, entry value: 9.121
	And a system delivery facility entry adjustment "adjustments" exists with system delivery entry: "facilityentry", adjusted date: "10/3/0222"	
	And I am logged in as "admin"
	And I am at the show page for system delivery entry "facilityentry"
	When I click ok in the dialog after pressing "Delete"
	Then I should be at the Production/SystemDeliveryEntry/Search page
	When I try to access the Show page for system delivery entry: "facilityentry" expecting an error
	Then I should see a 404 error message	
	
Scenario: user can create an entry and facility name does not disappear when validation errors occur
	Given I am logged in as "user"
	When I visit the Production/SystemDeliveryEntry/New page
	And I enter "12/7/2020" into the WeekOf field
	And I select operating center "nj7" from the OperatingCenters dropdown	
	And I select system delivery type "water" from the SystemDeliveryType dropdown
	And I select facility "one"'s FacilityIdWithFacilityName from the Facilities dropdown
	And I press "Select"
	Then the currently shown system delivery entry shall henceforth be known throughout the land as "Carl"
	And I should be at the Edit page for system delivery entry "Carl"
	And I should see a display for WeekOf with "12/7/2020"
	When I enter 25.00 into the FacilityEntries_0__EntryValue field
	And I press "Save"
	Then I should be at the Update page for system delivery entry "Carl"
	And I should see "Value not within range, please correct."
	And I should see "A Facility"
	When I enter 20.00 into the FacilityEntries_0__EntryValue field
	And I press "Save"
	Then I should be at the Show page for system delivery entry "Carl"

Scenario: user can't view purchase supplier and supplier facility information for wastewater type entries
	Given I am logged in as "user"
	And a system delivery entry "entry" exists with system delivery type: "waste water"
	And facility "one" exists in system delivery entry "entry"
	And operating center "nj7" exists in system delivery entry "entry"
	And a system delivery facility entry "monday" exists with system delivery entry: "entry", system delivery entry type: "delivered water", facility: "one", entry date: 7/11/2022, entry value: 3.251
	And a system delivery facility entry "tuesday" exists with system delivery entry: "entry", system delivery entry type: "delivered water", facility: "one", entry date: 7/12/2022, entry value: 6.661
	And a system delivery facility entry "wednesday" exists with system delivery entry: "entry", system delivery entry type: "delivered water", facility: "one", entry date: 7/13/2022, entry value: 8.212
	And a system delivery facility entry "thursday" exists with system delivery entry: "entry", system delivery entry type: "delivered water", facility: "one", entry date: 7/14/2022, entry value: 2.122
	And a system delivery facility entry "friday" exists with system delivery entry: "entry", system delivery entry type: "delivered water", facility: "one", entry date: 7/15/2022, entry value: 12.116
	And a system delivery facility entry "saturday" exists with system delivery entry: "entry", system delivery entry type: "delivered water", facility: "one", entry date: 7/16/2022, entry value: 4.726
	And a system delivery facility entry "sunday" exists with system delivery entry: "entry", system delivery entry type: "delivered water", facility: "one", entry date: 7/17/2022, entry value: 30.121
	When I visit the Production/SystemDeliveryEntry/Search page 
	And  I select state "nj" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown 
	And I press "Search"
	Then I should see a link to the Show page for system delivery entry: "entry"
	When I visit the Production/SystemDeliveryEntry/Show page for system delivery entry: "entry"
	Then I should be at the Show page for system delivery entry "entry"
	And I should not see the SupplierFacility field
	And I should not see the PurchaseSupplier field
