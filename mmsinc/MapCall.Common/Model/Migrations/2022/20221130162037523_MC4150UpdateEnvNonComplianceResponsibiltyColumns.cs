using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221130162037523), Tags("Production")]
    public class MC4150UpdateEnvNonComplianceResponsibiltyColumns : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                UPDATE [dbo].[EnvironmentalNonComplianceEventResponsibilities]
                SET [Description] = 'New Acquisition NOV'
                WHERE [Description] = 'New Acquisition';

                UPDATE [dbo].[EnvironmentalNonComplianceEventResponsibilities]
                SET [Description] = 'Third Party NOV'
                WHERE [Description] = 'Third Party';

                SET IDENTITY_INSERT [EnvironmentalNonComplianceEventResponsibilities] ON

                INSERT INTO [dbo].[EnvironmentalNonComplianceEventResponsibilities]
                           (Id, [Description])
                     VALUES
                           (5, 'Administrative NOV')
		                   ,(6, 'Consent Agreement NOV')
		                   ,(7, 'Erroneous NOV')
		                   ,(8, 'External Inspection with NOV')
		                   ,(9, 'Long-Term Recurrence (LRAA)')
		                   ,(10, 'Not Applicable');

                SET IDENTITY_INSERT [EnvironmentalNonComplianceEventResponsibilities] OFF
            ");

            Execute.Sql(@"
                UPDATE EnvironmentalNonComplianceEvents
                   SET  ResponsibilityId =
	                CASE
		                WHEN StatusId = 11 THEN 5
		                WHEN StatusId = 7 THEN 6
		                WHEN StatusId IN (3, 6, 8) THEN 10
		                WHEN StatusId = 9 THEN 7
		                WHEN StatusId = 12 THEN 9
		                WHEN StatusId = 13 THEN 3
		                WHEN StatusId IN (1, 2, 4, 5, 10) THEN 4
		                ELSE ResponsibilityId
	                END;

                UPDATE EnvironmentalNonComplianceEvents
                   SET  StatusId =
	                CASE
		                WHEN StatusId IN (7, 9, 10, 11, 12, 13) THEN 2
		                WHEN StatusId = 8 THEN 6
		                ELSE StatusId
	                END;

                DELETE FROM [dbo].[EnvironmentalNonComplianceEventStatuses]
	                WHERE [Description] IN (
                           'Administrative NOV'
		                   ,'Consent Agreement NOV'
		                   ,'Environmental Audit Issue in Progress'
		                   ,'Erroneous NOV'
		                   ,'External agency inspection with NOV'
		                   ,'Long Term NOV'
		                   ,'New Acquisition NOV');           
            ");

            Execute.Sql(@"
                UPDATE [dbo].[EnvironmentalNonComplianceEvents]
	                SET	StatusId = 2, ResponsibilityId = 1
	                FROM EnvironmentalNonComplianceEvents
	                WHERE Id IN (1, 2, 3, 5, 6, 7, 11, 15, 19, 21, 22, 23, 38, 175, 176, 177, 178, 179, 180, 182, 183, 184, 185, 188, 189, 190, 191, 192,
		                193, 195, 196, 197, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220,
		                221, 222, 223, 224, 225, 226, 227, 228, 233, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 253, 254, 255, 256, 257,
		                258, 259, 260, 261, 262, 263, 263, 265, 266, 267, 268, 269, 270, 271, 272, 273, 274, 275, 276, 277, 278, 279, 280, 281, 288, 289,
		                290, 291, 292, 293, 294, 295, 296, 297, 298, 299, 300, 301, 302, 303, 304, 305, 306, 307, 308, 312, 316, 317, 319, 320, 321, 325,
		                326, 327, 329, 336, 337, 339, 340, 343, 345, 346, 373, 376, 378, 380, 382, 389, 394, 395, 396, 397, 398, 400, 410, 412, 415, 418,
		                420, 421, 423, 425, 427, 428, 430, 431, 433, 436, 437, 439, 441, 442, 443, 445, 448, 452, 453, 454, 455, 456, 457, 458, 462, 464,
		                471, 478, 480, 481, 483, 485, 490, 491, 492, 493, 494, 496, 501, 502, 504, 508, 510);

                UPDATE [dbo].[EnvironmentalNonComplianceEvents]
	                SET	StatusId = 5, ResponsibilityId = 4
	                FROM EnvironmentalNonComplianceEvents
	                WHERE Id IN (8, 9, 10, 12, 13, 31, 198, 338, 374, 375, 377, 383, 385, 401, 406, 407, 408, 416, 424, 426, 444, 461, 482, 505);

                UPDATE [dbo].[EnvironmentalNonComplianceEvents]
	                SET	StatusId = 2, ResponsibilityId = 2
	                FROM EnvironmentalNonComplianceEvents
	                WHERE Id IN (16, 17, 18, 24, 25, 26, 27, 28, 36, 181, 186, 187, 234, 235, 248, 249, 250, 251, 252, 282, 283, 284, 285, 286, 287, 309,
		                311, 313, 314, 318, 332, 342, 381, 388, 390, 391, 392, 411, 432, 434, 449, 474, 477, 509);

                UPDATE [dbo].[EnvironmentalNonComplianceEvents]
	                SET	StatusId = 5, ResponsibilityId = 2
	                FROM EnvironmentalNonComplianceEvents
	                WHERE Id IN (20, 333, 403, 404);

                UPDATE [dbo].[EnvironmentalNonComplianceEvents]
	                SET	StatusId = 2, ResponsibilityId = 3
	                FROM EnvironmentalNonComplianceEvents
	                WHERE Id IN (194, 229, 230, 231, 232, 330, 379, 463, 465);

                UPDATE [dbo].[EnvironmentalNonComplianceEvents]
	                SET	StatusId = 5, ResponsibilityId = 3
	                FROM EnvironmentalNonComplianceEvents
	                WHERE Id IN (419, 470);

                UPDATE [dbo].[EnvironmentalNonComplianceEvents]
	                SET	StatusId = 2, ResponsibilityId = 8
	                FROM EnvironmentalNonComplianceEvents
	                WHERE Id IN (438, 446, 447, 466, 472, 473, 475, 484, 503, 511, 513);
            ");
        }

        public override void Down()
        {
            Execute.Sql(@"
                UPDATE [dbo].[EnvironmentalNonComplianceEventResponsibilities]
                SET [Description] = 'New Acquisition'
                WHERE [Description] = 'New Acquisition NOV';

                UPDATE [dbo].[EnvironmentalNonComplianceEventResponsibilities]
                SET [Description] = 'Third Party'
                WHERE [Description] = 'Third Party NOV';
            ");

            Execute.Sql(@"
                SET IDENTITY_INSERT [EnvironmentalNonComplianceEventStatuses] ON

                INSERT INTO [dbo].[EnvironmentalNonComplianceEventStatuses]
                           (Id, [Description])
                     VALUES
                           (11, 'Administrative NOV')
		                   ,(7, 'Consent Agreement NOV')
		                   ,(9, 'Erroneous NOV')
		                   ,(10, 'External agency inspection with NOV')
		                   ,(12, 'Long Term NOV')
		                   ,(8, 'Environmental Audit Issue in Progress')
		                   ,(13, 'New Acquisition NOV');

                SET IDENTITY_INSERT [EnvironmentalNonComplianceEventResponsibilities] OFF

                SELECT Id, [Description]
	                FROM [EnvironmentalNonComplianceEventStatuses];

                SELECT Id, ResponsibilityId, StatusId
	                FROM [EnvironmentalNonComplianceEvents];

                UPDATE [dbo].[EnvironmentalNonComplianceEvents]
	                SET	StatusId = 2, ResponsibilityId = 1
	                FROM EnvironmentalNonComplianceEvents
	                WHERE Id IN (1, 2, 3, 5, 6, 7, 11, 15, 19, 21, 22, 23, 38, 175, 176, 177, 178, 179, 180, 182, 183, 184, 185, 188, 189, 190, 191, 192,
		                193, 195, 196, 197, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220,
		                221, 222, 223, 224, 225, 226, 227, 228, 233, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 253, 254, 255, 256, 257,
		                258, 259, 260, 261, 262, 263, 263, 265, 266, 267, 268, 269, 270, 271, 272, 273, 274, 275, 276, 277, 278, 279, 280, 281, 288, 289,
		                290, 291, 292, 293, 294, 295, 296, 297, 298, 299, 300, 301, 302, 303, 304, 305, 306, 307, 308, 312, 316, 317, 319, 320, 321, 325,
		                326, 327, 329, 336, 337, 339, 340, 343, 345, 346, 373, 376, 378, 380, 382, 389, 394, 395, 396, 397, 398, 400, 410, 412, 415, 418,
		                420, 421, 423, 425, 427, 428, 430, 431, 433, 436, 437, 439, 441, 442, 443, 445, 448, 452, 453, 454, 455, 456, 457, 458, 462, 464,
		                471, 478, 480, 481, 483, 485, 490, 491, 492, 493, 494, 496, 501, 502, 504, 508, 510);

                UPDATE [dbo].[EnvironmentalNonComplianceEvents]
	                SET	StatusId = 5, ResponsibilityId = 1
	                FROM EnvironmentalNonComplianceEvents
	                WHERE Id IN (8, 9, 10, 12, 13, 31, 198, 338, 374, 375, 377, 383, 385, 401, 406, 407, 408, 416, 424, 426, 444, 461, 482, 505);

                UPDATE [dbo].[EnvironmentalNonComplianceEvents]
	                SET	StatusId = 2, ResponsibilityId = 2
	                FROM EnvironmentalNonComplianceEvents
	                WHERE Id IN (16, 17, 18, 24, 25, 26, 27, 28, 36, 181, 186, 187, 234, 235, 248, 249, 250, 251, 252, 282, 283, 284, 285, 286, 287, 309,
		                311, 313, 314, 318, 332, 342, 381, 388, 390, 391, 392, 411, 432, 434, 477, 509);

                UPDATE [dbo].[EnvironmentalNonComplianceEvents]
	                SET	StatusId = 5, ResponsibilityId = 2
	                FROM EnvironmentalNonComplianceEvents
	                WHERE Id IN (20, 333, 403, 404);

                UPDATE [dbo].[EnvironmentalNonComplianceEvents]
	                SET	StatusId = 2, ResponsibilityId = 3
	                FROM EnvironmentalNonComplianceEvents
	                WHERE Id IN (194, 229, 230, 231, 232, 330, 379, 463, 465);

                UPDATE [dbo].[EnvironmentalNonComplianceEvents]
	                SET	StatusId = 5, ResponsibilityId = 3
	                FROM EnvironmentalNonComplianceEvents
	                WHERE Id IN (419, 470);

                UPDATE [dbo].[EnvironmentalNonComplianceEvents]
	                SET	StatusId = 10, ResponsibilityId = 1
	                FROM EnvironmentalNonComplianceEvents
	                WHERE Id IN (438, 446, 447, 466, 472, 473, 475, 484, 503, 511, 513);

                UPDATE [dbo].[EnvironmentalNonComplianceEvents]
	                SET	StatusId = 10, ResponsibilityId = 2
	                FROM EnvironmentalNonComplianceEvents
	                WHERE Id IN (449, 474);
            ");
        }
    }
}