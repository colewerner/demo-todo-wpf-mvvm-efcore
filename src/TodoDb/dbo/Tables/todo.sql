CREATE TABLE [dbo].[todo]
(
	[todo_id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [description] NVARCHAR(50) NULL, 
    [is_completed] BIT NOT NULL DEFAULT 0
)
