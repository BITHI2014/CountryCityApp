
USE [CountryCityDB]
GO
/****** Object:  StoredProcedure [dbo].[SqlQueryNotificationStoredProcedure-cb0f2f79-3707-4560-b208-fb351158079d]    Script Date: 4/30/2015 5:17:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SqlQueryNotificationStoredProcedure-cb0f2f79-3707-4560-b208-fb351158079d] AS BEGIN BEGIN TRANSACTION; RECEIVE TOP(0) conversation_handle FROM [SqlQueryNotificationService-cb0f2f79-3707-4560-b208-fb351158079d]; IF (SELECT COUNT(*) FROM [SqlQueryNotificationService-cb0f2f79-3707-4560-b208-fb351158079d] WHERE message_type_name = 'http://schemas.microsoft.com/SQL/ServiceBroker/DialogTimer') > 0 BEGIN if ((SELECT COUNT(*) FROM sys.services WHERE name = 'SqlQueryNotificationService-cb0f2f79-3707-4560-b208-fb351158079d') > 0)   DROP SERVICE [SqlQueryNotificationService-cb0f2f79-3707-4560-b208-fb351158079d]; if (OBJECT_ID('SqlQueryNotificationService-cb0f2f79-3707-4560-b208-fb351158079d', 'SQ') IS NOT NULL)   DROP QUEUE [SqlQueryNotificationService-cb0f2f79-3707-4560-b208-fb351158079d]; DROP PROCEDURE [SqlQueryNotificationStoredProcedure-cb0f2f79-3707-4560-b208-fb351158079d]; END COMMIT TRANSACTION; END
GO
/****** Object:  Table [dbo].[tbl_country]    Script Date: 4/30/2015 5:17:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_country](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[about] [varchar](max) NOT NULL,
	[images] [varchar](max) NOT NULL,
 CONSTRAINT [PK_tbl_country] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
USE [master]
GO
ALTER DATABASE [CountryCityDB] SET  READ_WRITE 
GO
