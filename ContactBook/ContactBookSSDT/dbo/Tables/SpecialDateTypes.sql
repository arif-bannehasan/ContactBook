﻿CREATE TABLE [dbo].[SpecialDateTypes] (
    [SpecialDateTpId] INT            IDENTITY (1, 1) NOT NULL,
    [Date_TypeName]   NVARCHAR (100) NOT NULL,
    [BookId]          BIGINT         NULL,
    CONSTRAINT [PK_CB_SpecialDateType] PRIMARY KEY CLUSTERED ([SpecialDateTpId] ASC),
    CONSTRAINT [FK_CB_ContactBookCB_SpecialDateType] FOREIGN KEY ([BookId]) REFERENCES [dbo].[ContactBooks] ([BookId])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_CB_ContactBookCB_SpecialDateType]
    ON [dbo].[SpecialDateTypes]([BookId] ASC);

