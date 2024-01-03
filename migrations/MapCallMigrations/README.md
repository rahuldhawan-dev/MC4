# MapCall Migrations

This project exists as a way to compile the database migrations separately from all the other code in
MapCall.Common as well as the rest of the mmsinc library.  This migration assembly can be deployed
separately from the rest of the project along with a FluentMigrator console runner to run migrations in the
various deployment environments, as well as stand in for the much larger (takes longer to compile)
MapCall.Common project when running migrations locally.

There should be no need to ever make any adjustments or additions directly to this project, the workflow
regarding adding new migrations to MapCall.Common remains the same.  Any new migrations added there should
appear here automatically.

Should the team ever decide to create a separate assembly within the mmsinc project containing only the
migrations (probably a good idea) this project should serve as a starting point for the effort.