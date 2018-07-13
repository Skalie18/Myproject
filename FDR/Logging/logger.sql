CREATE TABLE [dbo].[ErrorDetails]
(
	[ErrorId] [uniqueidentifier] NOT NULL DEFAULT(NEWID()) PRIMARY KEY,
	[Timestamp] [datetime] NOT NULL DEFAULT (GETDATE()) ,
	[Message] [nvarchar](MAX) NOT NULL,
	[Source] [nvarchar](550) NOT NULL,
	[StackTrace] [ntext] NOT NULL,
	[SesstionId] [nvarchar](1000) NOT NULL,
	[InnerErrorMessage] [ntext] NULL,
	[InnerErrorStackTrace] [ntext] NULL,
	[SystemName] [nvarchar](350) NOT NULL
)

GO

CREATE TABLE [dbo].[RequestDetails]
(
	[RequestId] [uniqueidentifier] NOT NULL DEFAULT (NEWID()) PRIMARY KEY ,
	[Timestamp] [datetime] NOT NULL DEFAULT (getdate()),
	[SessionID] [nvarchar](1000) NOT NULL,
	[AppRelativeCurrentExecutionFilePath] [nvarchar](550) NULL,
	[FilePath] [nvarchar](550) NULL,
	[PhysicalApplicationPath] [nvarchar](550) NULL,
	[RawUrl] [nvarchar](1000) NOT NULL,
	[UserAgent] [ntext] NULL,
	[IsAuthenticated] [bit] NULL,
	[MachineName] [nvarchar](250) NULL
)

GO

CREATE PROCEDURE [dbo].[spINSERT_ErrorDetails]
(
	  @Message             			nvarchar(MAX)
	, @Source              			nvarchar(550)
	, @StackTrace          			ntext
	, @SesstionId          			nvarchar(1000)
	, @InnerErrorMessage   			ntext
	, @InnerErrorStackTrace			ntext
	, @SystemName          			nvarchar(350)
)
AS
INSERT [ErrorDetails]
(
	 [Message]
	,[Source]
	,[StackTrace]
	,[SesstionId]
	,[InnerErrorMessage]
	,[InnerErrorStackTrace]
	,[SystemName]
)
VALUES
(
	 @Message
	,@Source
	,@StackTrace
	,@SesstionId
	,@InnerErrorMessage
	,@InnerErrorStackTrace
	,@SystemName
)

GO

CREATE PROCEDURE [dbo].[spINSERT_RequestDetails]
(
	  @SessionID                          			nvarchar(1000)
	, @AppRelativeCurrentExecutionFilePath			nvarchar(550)	 = null
	, @FilePath                           			nvarchar(550)	 = null
	, @PhysicalApplicationPath            			nvarchar(550)	 = null
	, @RawUrl                             			nvarchar(1000)
	, @UserAgent                          			ntext
	, @IsAuthenticated                    			bit
	, @MachineName                        			nvarchar(250)	 = null
)
AS
INSERT [RequestDetails]
(

	 [SessionID]
	,[AppRelativeCurrentExecutionFilePath]
	,[FilePath]
	,[PhysicalApplicationPath]
	,[RawUrl]
	,[UserAgent]
	,[IsAuthenticated]
	,[MachineName]

)
VALUES
(
	 @SessionID
	,@AppRelativeCurrentExecutionFilePath
	,@FilePath
	,@PhysicalApplicationPath
	,@RawUrl
	,@UserAgent
	,@IsAuthenticated
	,@MachineName
)

GO

CREATE PROCEDURE [dbo].[uspREAD_ErrorDetails]
(
	 @SystemName	nvarchar(350)
)
AS
SELECT     
ErrorDetails.Timestamp
, ErrorDetails.Message
, ErrorDetails.[Source]
, ErrorDetails.StackTrace
, ErrorDetails.SesstionId
, ErrorDetails.InnerErrorMessage
, ErrorDetails.InnerErrorStackTrace
, ErrorDetails.SystemName
, RequestDetails.AppRelativeCurrentExecutionFilePath
, RequestDetails.FilePath
, RequestDetails.PhysicalApplicationPath
, RequestDetails.RawUrl
, RequestDetails.UserAgent
, RequestDetails.IsAuthenticated
, RequestDetails.MachineName
FROM ErrorDetails 
LEFT OUTER JOIN RequestDetails ON ErrorDetails.SesstionId = RequestDetails.SessionID
WHERE     (ErrorDetails.SystemName = @SystemName)

GO


CREATE PROCEDURE [dbo].[uspREAD_ErrorDetailsAll]

AS
SELECT

ErrorDetails.[Timestamp]
, ErrorDetails.[Message]
, ErrorDetails.[Source]
, ErrorDetails.StackTrace
, ErrorDetails.SesstionId
, ErrorDetails.InnerErrorMessage
, ErrorDetails.InnerErrorStackTrace
, ErrorDetails.SystemName
, RequestDetails.AppRelativeCurrentExecutionFilePath
, RequestDetails.FilePath
, RequestDetails.PhysicalApplicationPath
, RequestDetails.RawUrl
, RequestDetails.UserAgent
, RequestDetails.IsAuthenticated
, RequestDetails.MachineName
FROM ErrorDetails 
LEFT OUTER JOIN  RequestDetails ON ErrorDetails.SesstionId = RequestDetails.SessionID

GO

CREATE PROCEDURE [dbo].[uspREAD_ErrorDetailsByDates]
(
	 @StarDate Datetime,
	 @EndDate DateTime
)
AS
SELECT     
ErrorDetails.[Timestamp]
, ErrorDetails.[Message]
, ErrorDetails.[Source]
, ErrorDetails.StackTrace
, ErrorDetails.SesstionId
, ErrorDetails.InnerErrorMessage
, ErrorDetails.InnerErrorStackTrace
, ErrorDetails.SystemName
, RequestDetails.AppRelativeCurrentExecutionFilePath
, RequestDetails.FilePath
, RequestDetails.PhysicalApplicationPath
, RequestDetails.RawUrl
, RequestDetails.UserAgent
, RequestDetails.IsAuthenticated
, RequestDetails.MachineName
FROM ErrorDetails 
LEFT OUTER JOIN RequestDetails ON ErrorDetails.SesstionId = RequestDetails.SessionID
WHERE     (CONVERT(VARCHAR(8), ErrorDetails.Timestamp, 112) BETWEEN CONVERT(VARCHAR(8), @StarDate, 112) AND CONVERT(VARCHAR(8), @EndDate, 112))

GO

CREATE PROCEDURE [dbo].[uspREAD_ErrorDetailsReport]
(
	 @SystemName nvarchar(350) = Null,
	 @StarDate Datetime = Null,
	 @EndDate DateTime = Null
)
AS

if(@SystemName IS NULL)
	BEGIN
		IF(@StarDate IS NULL OR @EndDate IS NULL)
			BEGIN
				SELECT 
				  ErrorDetails.Timestamp	AS [TIME LOGGED]
				, ErrorDetails.Message	AS [MESSAGE]
				, ErrorDetails.[Source] AS [SOURCE]
				, ErrorDetails.StackTrace AS [STACK TRACE]
				, ErrorDetails.SesstionId AS [USER SESSION]
				, ErrorDetails.InnerErrorMessage AS [INNER MESSAGE]
				, ErrorDetails.InnerErrorStackTrace AS [INNER STACK TRACE]
				, ErrorDetails.SystemName AS [SYSTEM NAME]
				, RequestDetails.AppRelativeCurrentExecutionFilePath [APP RELATIVE CURRENT EXECUTION FILE PATH] 
				, RequestDetails.FilePath AS [FILE PATH]
				, RequestDetails.PhysicalApplicationPath AS[PHYSICAL APPLICATION PATH]
				, RequestDetails.RawUrl AS [RAW URL]
				, RequestDetails.UserAgent AS [USER AGENT]
				, RequestDetails.IsAuthenticated AS [AUTHENTICATED]
				, RequestDetails.MachineName AS [HOST SERVER NAME]
				FROM ErrorDetails LEFT OUTER JOIN
				  RequestDetails ON ErrorDetails.SesstionId = RequestDetails.SessionID
			END
		ELSE
			BEGIN
				SELECT
				  ErrorDetails.Timestamp	AS [TIME LOGGED]
				, ErrorDetails.Message	AS [MESSAGE]
				, ErrorDetails.[Source] AS [SOURCE]
				, ErrorDetails.StackTrace AS [STACK TRACE]
				, ErrorDetails.SesstionId AS [USER SESSION]
				, ErrorDetails.InnerErrorMessage AS [INNER MESSAGE]
				, ErrorDetails.InnerErrorStackTrace AS [INNER STACK TRACE]
				, ErrorDetails.SystemName AS [SYSTEM NAME]
				, RequestDetails.AppRelativeCurrentExecutionFilePath [APP RELATIVE CURRENT EXECUTION FILE PATH] 
				, RequestDetails.FilePath AS [FILE PATH]
				, RequestDetails.PhysicalApplicationPath AS[PHYSICAL APPLICATION PATH]
				, RequestDetails.RawUrl AS [RAW URL]
				, RequestDetails.UserAgent AS [USER AGENT]
				, RequestDetails.IsAuthenticated AS [AUTHENTICATED]
				, RequestDetails.MachineName AS [HOST SERVER NAME]
				FROM         ErrorDetails LEFT OUTER JOIN
									  RequestDetails ON ErrorDetails.SesstionId = RequestDetails.SessionID
				WHERE     (CONVERT(VARCHAR(8), ErrorDetails.Timestamp, 112) BETWEEN CONVERT(VARCHAR(8), @StarDate, 112) AND CONVERT(VARCHAR(8), @EndDate, 112))
				RETURN;
			END
	END	
ELSE
	BEGIN
		  IF(@StarDate IS NULL OR @EndDate IS NULL)
			BEGIN
			   SELECT 
				  ErrorDetails.Timestamp	AS [TIME LOGGED]
				, ErrorDetails.Message	AS [MESSAGE]
				, ErrorDetails.[Source] AS [SOURCE]
				, ErrorDetails.StackTrace AS [STACK TRACE]
				, ErrorDetails.SesstionId AS [USER SESSION]
				, ErrorDetails.InnerErrorMessage AS [INNER MESSAGE]
				, ErrorDetails.InnerErrorStackTrace AS [INNER STACK TRACE]
				, ErrorDetails.SystemName AS [SYSTEM NAME]
				, RequestDetails.AppRelativeCurrentExecutionFilePath [APP RELATIVE CURRENT EXECUTION FILE PATH] 
				, RequestDetails.FilePath AS [FILE PATH]
				, RequestDetails.PhysicalApplicationPath AS[PHYSICAL APPLICATION PATH]
				, RequestDetails.RawUrl AS [RAW URL]
				, RequestDetails.UserAgent AS [USER AGENT]
				, RequestDetails.IsAuthenticated AS [AUTHENTICATED]
				, RequestDetails.MachineName AS [HOST SERVER NAME]
			FROM         ErrorDetails LEFT OUTER JOIN
									  RequestDetails ON ErrorDetails.SesstionId = RequestDetails.SessionID
				WHERE     ErrorDetails.SystemName = @SystemName
				RETURN;
		   END
		 ELSE
			BEGIN
			  SELECT 
				  ErrorDetails.Timestamp	AS [TIME LOGGED]
				, ErrorDetails.Message	AS [MESSAGE]
				, ErrorDetails.[Source] AS [SOURCE]
				, ErrorDetails.StackTrace AS [STACK TRACE]
				, ErrorDetails.SesstionId AS [USER SESSION]
				, ErrorDetails.InnerErrorMessage AS [INNER MESSAGE]
				, ErrorDetails.InnerErrorStackTrace AS [INNER STACK TRACE]
				, ErrorDetails.SystemName AS [SYSTEM NAME]
				, RequestDetails.AppRelativeCurrentExecutionFilePath [APP RELATIVE CURRENT EXECUTION FILE PATH] 
				, RequestDetails.FilePath AS [FILE PATH]
				, RequestDetails.PhysicalApplicationPath AS[PHYSICAL APPLICATION PATH]
				, RequestDetails.RawUrl AS [RAW URL]
				, RequestDetails.UserAgent AS [USER AGENT]
				, RequestDetails.IsAuthenticated AS [AUTHENTICATED]
				, RequestDetails.MachineName AS [HOST SERVER NAME]
			FROM         ErrorDetails LEFT OUTER JOIN
								  RequestDetails ON ErrorDetails.SesstionId = RequestDetails.SessionID
			WHERE     (CONVERT(VARCHAR(8), ErrorDetails.Timestamp, 112) BETWEEN CONVERT(VARCHAR(8), @StarDate, 112) AND CONVERT(VARCHAR(8), @EndDate, 112)) AND ErrorDetails.SystemName = @SystemName
			RETURN;
			END
		END

		GO
ALTER PROCEDURE [dbo].[upsGetSystems]
As

Select Distinct SystemName
from dbo.ErrorDetails
order by SystemName asc