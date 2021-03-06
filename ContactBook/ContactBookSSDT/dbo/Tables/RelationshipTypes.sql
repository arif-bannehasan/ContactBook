﻿CREATE TABLE [dbo].[RelationshipTypes] (
    [RelationshipTypeId]    INT            IDENTITY (1, 1) NOT NULL,
    [Relationship_TypeName] NVARCHAR (MAX) NOT NULL,
    [BookId]                BIGINT         NULL,
    CONSTRAINT [PK_CB_RelationshipType] PRIMARY KEY CLUSTERED ([RelationshipTypeId] ASC),
    CONSTRAINT [FK_CB_ContactBookCB_RelationshipType] FOREIGN KEY ([BookId]) REFERENCES [dbo].[ContactBooks] ([BookId])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_CB_ContactBookCB_RelationshipType]
    ON [dbo].[RelationshipTypes]([BookId] ASC);

