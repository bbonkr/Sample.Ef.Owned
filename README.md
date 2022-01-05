Owned entity query problem faced with EneityFrameworkCore 6.

## Entities

### User

```sql
CREATE TABLE [dbo].[User](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](500) NOT NULL,
	[LastName] [nvarchar](500) NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
    (
	    [Id] ASC
    ) 
)
GO
```

### UserProfile

```sql
CREATE TABLE [dbo].[UserProfile](
	[UserId] [bigint] NOT NULL,
	[Height] [float] NULL,
	[Weight] [float] NULL,
    CONSTRAINT [PK_UserProfile] PRIMARY KEY CLUSTERED 
    (
	    [UserId] ASC
    )
)
GO
ALTER TABLE [dbo].[UserProfile]  WITH CHECK ADD  CONSTRAINT [FK_UserProfile_User_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserProfile] CHECK CONSTRAINT [FK_UserProfile_User_UserId]
GO
```

### UserMetadata

```sql
CREATE TABLE [dbo].[UserMetadata](
	[UserId] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[DeletedAt] [datetime2](7) NULL,
    CONSTRAINT [PK_UserMetadata] PRIMARY KEY CLUSTERED 
    (
	    [UserId] ASC
    )
)
GO
ALTER TABLE [dbo].[UserMetadata] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[UserMetadata] ADD  DEFAULT (getutcdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[UserMetadata]  WITH CHECK ADD  CONSTRAINT [FK_UserMetadata_User_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserMetadata] CHECK CONSTRAINT [FK_UserMetadata_User_UserId]
GO
```

### UserAddress

```sql
CREATE TABLE [dbo].[UserAddress](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Street] [nvarchar](500) NOT NULL,
	[Detail] [nvarchar](500) NULL,
	[City] [nvarchar](500) NOT NULL,
	[State] [nvarchar](500) NOT NULL,
	[Country] [nvarchar](500) NOT NULL,
	[Zipcode] [nvarchar](10) NULL,
	[UserId] [bigint] NOT NULL,
	CONSTRAINT [PK_UserAddress] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_UserAddress_UserId] ON [dbo].[UserAddress]
(
	[UserId] ASC
) 
GO
ALTER TABLE [dbo].[UserAddress]  WITH CHECK ADD  CONSTRAINT [FK_UserAddress_User_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserAddress] CHECK CONSTRAINT [FK_UserAddress_User_UserId]
GO
```

## Task

### Add dotnet-ef tool

```bash
# Create manifest file; .config/dotnet-tools.json
$ dotnet new tool-manifest
# Install dotnet-ef tool locally
$ dotnet tool install dotnet-ef
```

### Add Migration

```bash
$ cd src/Sample.Data
$ dotnet ef migrations add "Initialize database" --context AppDbContext --startup-project ../Sample.App/ --project ../Sample.Data.SqlServer/ --json
```

## Run the Sample.App project

Execute GET:/users on Web browser swagger UI


## Problem

Entity framework has been generated wrong query.

* u0 fields does not include in sub query.
* u0 alia is wrong in SELECT statement.

### Generated query


```sql
DECLARE @__p_0 int = 0;
DECLARE @__p_1 int = 10;

SELECT [t].[Id], [t].[FirstName], [t].[LastName], [t].[UserId], [u1].[UserId], [u2].[Id], [u2].[City], [u2].[Country], [u2].[Detail], [u2].[Name], [u2].[State], [u2].[Street], [u2].[UserId], [u2].[Zipcode], [u0].[UserId], [u0].[CreatedAt], [u0].[DeletedAt], [u0].[IsDeleted], [u0].[UpdatedAt], [u1].[Height], [u1].[Weight]
FROM (
    SELECT [u].[Id], [u].[FirstName], [u].[LastName], [u0].[UserId]
    FROM [User] AS [u]
    LEFT JOIN [UserMetadata] AS [u0] ON [u].[Id] = [u0].[UserId]
    WHERE [u0].[IsDeleted] = CAST(0 AS bit)
    ORDER BY [u].[FirstName], [u].[LastName]
    OFFSET @__p_0 ROWS FETCH NEXT @__p_1 ROWS ONLY
) AS [t]
LEFT JOIN [UserProfile] AS [u1] ON [t].[Id] = [u1].[UserId]
LEFT JOIN [UserAddress] AS [u2] ON [t].[Id] = [u2].[UserId]
ORDER BY [t].[FirstName], [t].[LastName], [t].[Id], [t].[UserId], [u1].[UserId]
```

### Exception

```plaintext
Microsoft.Data.SqlClient.SqlException (0x80131904): The multi-part identifier "u0.UserId" could not be bound.
The multi-part identifier "u0.CreatedAt" could not be bound.
The multi-part identifier "u0.DeletedAt" could not be bound.
The multi-part identifier "u0.IsDeleted" could not be bound.
The multi-part identifier "u0.UpdatedAt" could not be bound.
```

### Expected query

```sql
DECLARE @__p_0 int = 0;
DECLARE @__p_1 int = 10;

SELECT [t].[Id], [t].[FirstName], [t].[LastName], [t].[UserId], [u1].[UserId], [u2].[Id], [u2].[City], [u2].[Country], [u2].[Detail], [u2].[Name], [u2].[State], [u2].[Street], [u2].[UserId], [u2].[Zipcode], [t].[UserId], [t].[CreatedAt], [t].[DeletedAt], [t].[IsDeleted], [t].[UpdatedAt], [u1].[Height], [u1].[Weight]
FROM (
    SELECT [u].[Id], [u].[FirstName], [u].[LastName], [u0].[UserId], [u0].[CreatedAt], [u0].[DeletedAt], [u0].[IsDeleted], [u0].[UpdatedAt]
    FROM [User] AS [u]
    LEFT JOIN [UserMetadata] AS [u0] ON [u].[Id] = [u0].[UserId]
    WHERE [u0].[IsDeleted] = CAST(0 AS bit)
    ORDER BY [u].[FirstName], [u].[LastName]
    OFFSET @__p_0 ROWS FETCH NEXT @__p_1 ROWS ONLY
) AS [t]
LEFT JOIN [UserProfile] AS [u1] ON [t].[Id] = [u1].[UserId]
LEFT JOIN [UserAddress] AS [u2] ON [t].[Id] = [u2].[UserId]
ORDER BY [t].[FirstName], [t].[LastName], [t].[Id], [t].[UserId], [u1].[UserId]
```