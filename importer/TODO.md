# TODO:

## Optimization

- [X] Try out StatelessSession (didn't pan out once MapCall ViewModels came into play)
- [X] Save mapped entities from validation pass to speed up insert
- [ ] Maybe generate a script of inserts at the end for live import
- [ ] Memoize many more things
  - [ ] Need samples of large asset files (Hydrants, Valves, etc.) from Doug


## Other Assets

- [ ] Manholes
  - [ ] Obtain Sample

# DONE:

## Assets
- [X] Equipment
- [X] Hydrants
- [X] Streets
- [X] Valve Inspections
- [X] Valves
- [X] Hydrant Inspections
- [X] Facilities

## Misc
- [X] Installer
- [X] Blocking
  - [X] Progress bar
- [X] Resizing
- [X] Additional File Types
- [X] Installer

# Version 2.0:
- [ ] Replace ExcelImport<>
  - [ ] Use strings for column names instead of properties when reading
    - [ ] Maybe proper mapper class with attributes to specify ColumnName, validation, etc.
  - [ ] Promote SAPEquipmentType/EquipmentTypeId in EquipmentExcelRecords to public and
        get rid of the reflection calls in related tests.
- [ ] Replace InMemoryTestDatabase<>
  - [ ] ClassInitialize creates session and inserts basic data necessary for most tests
    - [ ] Maybe use that new dump/load thing added to sqlite recently
    - [ ] At the very least avoid schema regeneration at each TestInitialize call
- [ ] Automate loading into production
  - [ ] Generate an email with the successfully validated and imported (into QA) file and an md5 hash of same
  - [ ] Automatically load and import emails in the scheduler using the MapCallImporter.Core library
- [ ] Update release process
  - [X] Separate out Build & Test from building/packaging Release
    - [ ] Auto-tag with version number on release build
  - [ ] Include version check on Validate click, refuse to run if newer version is available
