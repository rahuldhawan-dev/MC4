Adding New File Types
---------------------

1. Add sample files to "doc\Samples\\\<entity name plural>".  At the very least there should be a file full of valid records which will import cleanly called "Valid \<entity name plural>.xlsx".
2. Add a model representing a record in your input file as "MapCallImporter.Core\\Models\\\<entity name>ExcelRecord.cs".  Field names in the model should match the names in the column headers (case-sensitive), and basic value types (string, int, DateTime) are mapped automatically (including nullable values).
    - Inherit from ExcelRecord\<TEntity, TThis> where "TEntity" is the MapCall entity class being imported and "TThis" is the name of the class itself..
    - Implement MapToEntity to map any necessary fields from the excel records to MapCall entities, using the helper and IUnitOfWork to lookup any referenced entities.
    - Implement Validate to test any necessary validation rules for the entity.  It's usually useful to call MapToEntity from validate to avoid redundancy.
    - In both methods, call helper.AddFailure to note any validation failures or lookup errors and prevent importing bad data.  Be sure to note the current index and whatever column names are involved in the failure in your message.
3. Add tests for cases involving validation issues (including no issues/file valid) to MapCallImporter.Tests\\Validation\\ExcelFileValidationServiceTest.cs in a region specific to the MapCall entity being imported.
4. Add tests for cases involving import issues (including no issues/import success) to MapCallImporter.Tests\\Importing\\ExcelFileImportingServiceTest.cs in a region specific to the MapCall entity being imported.
5. The simplest way to add test data is to add new query methods to MapCallImporter.Tests\\TestDataHelper.cs (be sure to add constant values to the structs up top).
6. As of this writing test coverage for the MapCallImporter.Core project is at 89% with no mocking or stubbing.  It should remain at that level or higher.
