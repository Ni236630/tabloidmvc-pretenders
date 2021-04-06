
SET IDENTITY_INSERT [Comment] ON
INSERT INTO Comment (Id, PostId, UserProfileId, [Subject], Content, CreateDateTime)
	VALUES (2, 1, 1, 'another comment subject', 'ALSO THIS IS A COMMENT', '4/2/2021 12:00:00 AM')
SET IDENTITY_INSERT [Comment] OFF