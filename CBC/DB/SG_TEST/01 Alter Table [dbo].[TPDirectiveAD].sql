USE [SARSeFilingDEV]
GO

Alter Table [dbo].[TPDirectiveAD] add MemFundContrAfter20160301 money null DEFAULT (0)

Alter Table [dbo].[TPDirectiveAD] add AnnuityPolicyNum1 varchar(20) null

Alter Table [dbo].[TPDirectiveAD] add AnnuityPolicyNum2 varchar(20) null

Alter Table [dbo].[TPDirectiveAD] add AnnuityPolicyNum3 varchar(20) null

Alter Table [dbo].[TPDirectiveAD] add FundAnnuityAmt1 money null DEFAULT (0)

Alter Table [dbo].[TPDirectiveAD] add FundAnnuityAmt2 money null DEFAULT (0)

Alter Table [dbo].[TPDirectiveAD] add FundAnnuityAmt3 money null DEFAULT (0)

Alter Table [dbo].[TPDirectiveAD] add AnnuityRegInsurer1 varchar(120) null

Alter Table [dbo].[TPDirectiveAD] add AnnuityRegInsurer2 varchar(120) null

Alter Table [dbo].[TPDirectiveAD] add AnnuityRegInsurer3 varchar(120) null

Alter Table [dbo].[TPDirectiveAD] add AnnuityRegInsurerEmail1 varchar(50) null

Alter Table [dbo].[TPDirectiveAD] add AnnuityRegInsurerEmail2 varchar(50) null

Alter Table [dbo].[TPDirectiveAD] add AnnuityRegInsurerEmail3 varchar(50) null

Alter Table [dbo].[TPDirectiveAD] add AnnuityRegInsurerEmail4 varchar(50) null

Alter Table [dbo].[TPDirectiveAD] add AnnuityRegInsurerFSBNum1 varchar(12) null

Alter Table [dbo].[TPDirectiveAD] add AnnuityRegInsurerFSBNum2 varchar(12) null

Alter Table [dbo].[TPDirectiveAD] add AnnuityRegInsurerFSBNum3 varchar(12) null

Alter Table [dbo].[TPDirectiveAD] add AnnuityRegInsurerFSBNum4 varchar(12) null

Alter Table [dbo].[TPDirectiveAD] add AnnuityRegInsurerTelNumber1 varchar(20) null

Alter Table [dbo].[TPDirectiveAD] add AnnuityRegInsurerTelNumber2 varchar(20) null

Alter Table [dbo].[TPDirectiveAD] add AnnuityRegInsurerTelNumber3 varchar(20) null

Alter Table [dbo].[TPDirectiveAD] add AnnuityRegInsurerTelNumber4 varchar(20) null

Alter Table [dbo].[TPDirectiveAD] add AnnuityRegInsurerCellNumber1 varchar(20) null

Alter Table [dbo].[TPDirectiveAD] add AnnuityRegInsurerCellNumber2 varchar(20) null

Alter Table [dbo].[TPDirectiveAD] add AnnuityRegInsurerCellNumber3 varchar(20) null

Alter Table [dbo].[TPDirectiveAD] add AnnuityRegInsurerCellNumber4 varchar(20) null

Alter Table [dbo].[TPDirectiveAD] add SpecialCondInstr varchar(120) null

Alter Table [dbo].[TPDirectiveAD] add DeclarationInd char(1) null




