USE [McProd]
GO

CREATE TABLE [PublicRTOViewings] (
	[PublicRTOViewingID] int unique identity not null,
	[ViewDate] datetime unique not null,
	CONSTRAINT [PK_PublicRTOViewings] PRIMARY KEY CLUSTERED (
		[PublicRTOViewingID] ASC
	) ON [PRIMARY]
) ON [PRIMARY];
GO

GRANT ALL ON [PublicRTOViewings] TO wateroutage;