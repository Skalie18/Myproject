use [SARSeFilingDEV]
go
 
 if not exists (select * from sys.objects where object_id = object_id(N'Process_S_SendToSARSFORMAD') and type in (N'P', N'PC')) begin

              execute ('create procedure Process_S_SendToSARSFORMAD as')

       end else begin

              Exec DBA.dbo.objBackup_B_V_S  @dbName = 'SARSeFilingDEV', -- Target database
                                                  @objName = 'Process_S_SendToSARSFORMAD',  -- sp, view or function name ** returns last backup if cripted
                                                  @objBUSavedBy = 'Admin', -- person who does the backup
                                                  @obJBUID = null,   -- Script with the objBUID instead of objName 
                                                  @actType = 'B';    -- B - Backup, V - View, S – Script

end
go
   
/****** Object:  Stored Procedure dbo.Process_S_SendToSARSFORMAD    Script Date: 2/27/2006 12:08:28 PM ******/    
    
    
    
-- drop proc Process_S_SendToSARSFORMAD    
-- exec Process_S_SendToSARSFORMAD 'WBTest001'    
--***********************************************************************************    
-- Description: Selects Directive content    
-- Input: Unique DirectiveRequestID (MANDATORY) or directive Key (OPTIONAL)    
-- Output: All fields                                  
-- Change: 20071022 Added New Fiedls - Nativ Ben-Meir      
-- Modified: Nativ Ben-Meir 20090822 (Form Change)    
--***********************************************************************************    
CREATE PROC [dbo].[Process_S_SendToSARSFORMAD]    
                   (   @TestInd                        tinyint, --not used , to conform to other interface    
                       @BusinessProcessExecutionNum    int,  -- used    
                       @sReturnType                    int, --not used , to conform to other interface    
                       @SignIn                         int --not used , to conform to other interface    
    
                          )    
    
as    
begin    
    
  declare @DirectiveTypeCode     char(8),    
          @SARSFileType          char(8),    
          @SysStatusSentFile     char(5),    
          @SysStatusReady        char(5),    
          @SysStatusInterimFile  char(5),    
          @SystemDate            datetime    
    
  declare @SPName varchar(50),    
          @ErrorCheck tinyint,    
          @SPReturn tinyint,    
          @Err varchar(255)    
    
    
  select @SPName = 'Process_S_SendToSARSFORMAD',    
         @ErrorCheck = 0,    
         @SPReturn = 0,    
         @Err = '',    
         @DirectiveTypeCode = 'FORMAD',    
         @SARSFileType = 'OFORMAD',    
         @SysStatusReady = 'SBMTO',    
         @SysStatusInterimFile = 'SBMTX',    
         @SysStatusSentFile = 'SBMTS',    
         @SystemDate = getdate()    
    
    
  -- check before execution    
    
    if (select count(*)    
          from TPDirectiveAD    
         where DirectiveSystemStatus = @SysStatusInterimFile) > 0 begin 
		    
      select @Err = ': Procedure is already running please wait'    
      goto ErrHandler    
    end     
    
  -- Update directives status based on submitted to ready for file    
    
    begin transaction 
	   
   begin try
    
    update TPDirectiveAD    
       set DirectiveSystemStatus = @SysStatusInterimFile    
     where DirectiveSystemStatus = @SysStatusReady    
    

    end try
	begin catch
	rollback
	Select @Err = ': Error on setting Directive status to sent'
	goto ErrHandler
	end catch

	begin try

    insert TrackDirectiveSARS (                 
                SARSFileType,     
                DirectiveTypeCode,    
                DirectiveRequestID,    
                DirectiveKey,    
                SubmissionDt,    
                BusinessProcessExecutionNum)    
    select @SARSFileType,     
           @DirectiveTypeCode,    
           DirectiveRequestID,    
           FORMADID,    
           @SystemDate,    
           @BusinessProcessExecutionNum    
      from TPDirectiveAD    
     where DirectiveSystemStatus = @SysStatusInterimFile    
    
    end try
	begin catch
	rollback
	Select @Err = ': Error on setting Directive status to sent'
	goto ErrHandler
	end catch 
	         
    commit transaction    
    
    begin try

    -- insert status history record    
    
      insert DirectiveStatusHistory    
              (DirectiveRequestID,    
               DirectiveTypeCode,    
               DirectiveKey,    
               DirectiveSystemStatus,    
               StatusHistoryDt)    
      select DirectiveRequestID,    
             @DirectiveTypeCode,    
             FORMADID,    
  @SysStatusSentFile,    
             @SystemDate    
        from TPDirectiveAD    
       where DirectiveSystemStatus = @SysStatusInterimFile    
       
    end try
	begin catch
	rollback
	Select @Err = ': Error on FORMAD Directive Status History Insert'
	goto ErrHandler
	end catch 
    
      select FORMADID,    
             DirectiveRequestID,    
             TaxUserID,    
             TaxPayerID,    
             DirectiveGroupID,    
             DirectiveSystemStatus,    
             BusinessProcessExecutionNum,    
--             LTRIM(RTRIM(CONVERT(char,YEAR(CompleteDt)))) + '/' + LTRIM('0'+RTRIM(CONVERT(char,MONTH(CompleteDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(CompleteDt))))    
             convert(char, CompleteDt, 113)    
             as CompleteDt,    
--             LTRIM(RTRIM(CONVERT(char,YEAR(SubmitDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(SubmitDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(SubmitDt))))    
             convert(char, SubmitDt, 113)    
             as SubmitDt,    
             PeriodYear,    
             PeriodNum,    
             DirectiveApplicationID,    
             PaperRespInd,    
--             LTRIM(RTRIM(CONVERT(char,YEAR(YearOfAssesmentDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(YearOfAssesmentDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(YearOfAssesmentDt))))                
             convert(char, YearOfAssesmentDt, 113)    
             as YearOfAssesmentDt,    
             IncomeTaxReferenceNum,    
             NoIncomeTaxCode,    
             NoIncomeTaxText,    
             round(AnnualIncomeAmt, 0) as AnnualIncomeAmt,    
             EmployeeNum,    
             PersonTitleCode,    
             Initials,    
             Surname,    
             FirstName,    
--             LTRIM(RTRIM(CONVERT(char,YEAR(DateOfBirth)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(DateOfBirth)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(DateOfBirth))))                
             convert(char, DateOfBirth, 113)    
             as DateOfBirth,    
             IDNumber,    
             IDNumberOther,    
             PhysicalAddress1,    
             PhysicalAddress2,    
             PhysicalAddress3,    
             PhysicalAddress4,    
             PhysicalPostCode,    
             PostalAddress1,    
             PostalAddress2,    
             PostalAddress3,    
             PostalAddress4,    
             PostalPostCode,    
             TelAreaCode,    
             TelNumber,    
             rtrim(FundContactPersonTitleCode)+ ' ' + rtrim(FundContactPersonFirstName) + ' '     
                 + rtrim(FundContactPersonSurname)    
             as FundContactPerson,     
             FundName,    
             FundNumber ,    
             FundPAYENum ,     
             FundContactPersonTitleCode ,    
             FundContactPersonInitials ,    
             FundContactPersonFirstName ,    
             FundContactPersonSurname,    
             FundTelAreaCode,    
             FundTelNumber,    
             FundPostalAddress1,    
             FundPostalAddress2,    
             FundPostalAddress3,    
             FundPostalAddress4,    
             FundPostalPostCode,    
             MembershipNum,    
--             LTRIM(RTRIM(CONVERT(char,YEAR(MembershipDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(MembershipDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(MembershipDt))))    
             convert(char, MembershipDt, 113)    
             as MembershipDt,    
             FundTypeCode,    
             FundCreateReasonCode,    
             GrossLumpAmt,    
             ReasonForDirectiveCode,    
--             LTRIM(RTRIM(CONVERT(char,YEAR(AccrualDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(AccrualDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(AccrualDt))))    
             convert(char, AccrualDt, 113)    
             as AccrualDt,    
             TotalContributionAmt,    
             --ExcessContributionAmt,   
             CalculationMethodCode,    
--             LTRIM(RTRIM(CONVERT(char,YEAR(CalculationYearFromDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(CalculationYearFromDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(CalculationYearFromDt))))    
             convert(char, CalculationYearFromDt, 113)    
             as CalculationYearFromDt,    
--             LTRIM(RTRIM(CONVERT(char,YEAR(CalculationYearToDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(CalculationYearToDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(CalculationYearToDt))))    
             convert(char, CalculationYearToDt, 113)    
             as CalculationYearToDt,    
             CalculationCompletedYears,    
--             LTRIM(RTRIM(CONVERT(char,YEAR(Salary1StartDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(Salary1StartDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(Salary1StartDt))))    
--             convert(char, Salary1StartDt, 113)    
--             as Salary1StartDt,    
--             LTRIM(RTRIM(CONVERT(char,YEAR(Salary1EndDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(Salary1EndDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(Salary1EndDt))))    
             LTRIM(RTRIM(CONVERT(char,YEAR(Salary1EndDt))))    
             as SalaryYear1,    
             round(Salary1Amt, 0) as Salary1Amt,    
--             LTRIM(RTRIM(CONVERT(char,YEAR(Salary2StartDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(Salary2StartDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(Salary2StartDt))))    
--             LTRIM(RTRIM(CONVERT(char,YEAR(Salary2StartDt)))    
--             as Salary2StartDt,    
--             LTRIM(RTRIM(CONVERT(char,YEAR(Salary2EndDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(Salary2EndDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(Salary2EndDt))))    
             LTRIM(RTRIM(CONVERT(char,YEAR(Salary2EndDt))))    
             as SalaryYear2,    
             round(Salary2Amt, 0) as Salary2Amt,    
--             LTRIM(RTRIM(CONVERT(char,YEAR(Salary3StartDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(Salary3StartDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(Salary3StartDt))))    
--             convert(char, Salary3StartDt, 113)    
--             as Salary3StartDt,    
--             LTRIM(RTRIM(CONVERT(char,YEAR(Salary3EndDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(Salary3EndDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(Salary3EndDt))))    
             LTRIM(RTRIM(CONVERT(char,YEAR(Salary3EndDt))))    
             as SalaryYear3,    
             round(Salary3Amt, 0) as Salary3Amt,    
--             LTRIM(RTRIM(CONVERT(char,YEAR(Salary4StartDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(Salary4StartDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(Salary4StartDt))))    
--             convert(char, Salary4StartDt, 113)    
--             as Salary4StartDt,    
--             LTRIM(RTRIM(CONVERT(char,YEAR(Salary4EndDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(Salary4EndDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(Salary4EndDt))))    
             LTRIM(RTRIM(CONVERT(char,YEAR(Salary4EndDt))))    
             as SalaryYear4,    
             round(Salary4Amt, 0) as Salary4Amt,    
--             LTRIM(RTRIM(CONVERT(char,YEAR(Salary5StartDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(Salary5StartDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(Salary5StartDt))))    
             convert(char, Salary5StartDt, 113)    
             as Salary5StartDt,    
--             LTRIM(RTRIM(CONVERT(char,YEAR(Salary5EndDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(Salary5EndDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(Salary5EndDt))))    
             LTRIM(RTRIM(CONVERT(char,YEAR(Salary5EndDt))))    
             as SalaryYear5,    
             round(Salary5Amt, 0) as Salary5Amt,    
             SalaryTotalAmt,    
             SalaryAverageAmt ,    
             round(Salary12MonthPreDeathAmt, 0) as Salary12MonthPreDeathAmt,    
             case    
               when Salary1Amt > 0 and Salary2Amt > 0 and Salary3Amt > 0 and Salary4Amt > 0 and  Salary5Amt > 0 then 5    
               when Salary1Amt > 0 and Salary2Amt > 0 and Salary3Amt > 0 and Salary4Amt > 0 then 4    
               when Salary1Amt > 0 and Salary2Amt > 0 and Salary3Amt > 0 then 3    
               when Salary1Amt > 0 and Salary2Amt > 0 then 2    
               when Salary1Amt > 0  then 1    
               else 0    
             end     
             as SalariesEnteredNum,    
             EmployerName,    
             EmployerPAYENum,    
             rtrim(EmployerContactPersonTitleCode)+ ' ' + rtrim(EmployerContactPersonFirstName) + ' '     
                 + rtrim(EmployerContactPersonSurname)    
             as EmployerContactPerson,    
--             EmployerContactPersonTitleCode,    
--             EmployerContactPersonInitials,    
--             EmployerContactPersonFirstName,    
--             EmployerContactPersonSurname,    
             EmployerTelAreaCode,    
             EmployerTelNumber,    
             EmployerPhysicalAddress1,    
             EmployerPhysicalAddress2,    
             EmployerPhysicalAddress3,    
             EmployerPhysicalAddress4,    
             EmployerPhysicalPostCode,    
             EmployerPostalAddress1,    
             EmployerPostalAddress2,    
             EmployerPostalAddress3,    
             EmployerPostalAddress4,    
             EmployerPostalPostCode,    
             ITArea,    
             DirectiveStatusCode,    
--             LTRIM(RTRIM(CONVERT(char,YEAR(IssueDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(IssueDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(IssueDt))))                
             convert(char, IssueDt, 113)    
             as IssueDt,    
--             LTRIM(RTRIM(CONVERT(char,YEAR(ValidStartDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(ValidStartDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(ValidStartDt))))                
             convert(char, ValidStartDt, 113)    
             as ValidStartDt,    
--             LTRIM(RTRIM(CONVERT(char,YEAR(ValidEndDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(ValidEndDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(ValidEndDt))))                
             convert(char, ValidEndDt, 113)    
             as ValidEndDt,    
             TaxFreePortionAmt,    
             GrossPayablePAYEAmt,    
             SOAssessedTaxAmt,    
             SOProvisionalTax1Amt,    
             SOProvisionalTax1Period,    
             SOProvisionalTax2Amt,    
             DeductionFrequency,    
             AllowedContributionAmt,    
             SOIT88LRefNum,    
             SOProvisionalTax2Period,    
             SOProvisionalTax3Amt,    
             SOProvisionalTax3Period,    
             SOVATNum,    
             SOVATAmt,    
             SOPAYENum,    
             SOPAYEAmt,    
             Comment,    
             SARSIncomeTaxReferenceNum,    
             GrossTotalBenefitAmt,    
             DivorceSpouseAmt,    
             BenefitCalcInd,    
             FundAnnuityInd,    
             FundAnnuityAmt,    
             AnnuityRegInsurer,    
             AnnuityPolicyNum,    
             FundPayAnnuityInd,    
             FundRemainAnnuityAmt,     
             --// Added new fields 20071022 Nativ Ben-Meir      
             convert(char, PublicSectorFromDt, 113)  as PublicSectorFromDt,      
             convert(char, PublicSectorToDt, 113)  as PublicSectorToDt,      
             isnull(PublicSectorAmt, 0) PublicSectorAmt,      
             convert(char, PublicSectorTransferDt, 113)  as PublicSectorTransferDt,           
			 --UnclaimedBenefPrevTaxedAmt,  -- NBM 20090822    
			 EmailAddressAdministrator,    
			 TaxedTransferNonMemberSpouse,  
			 ParticipatingEmployerName,  
			 FSBNumber ,
			 AIPFDeemedContrib,
			 NonResidentInd,
			 ServRendOutsideRepInd,
			 ServRendContribFundMonth,
			 ServRendOutsideRepMonth,
			 MemFundContr,
			 MemFundContrAfter20160301,
			 AnnuityPolicyNum1,
			 AnnuityPolicyNum2,
             AnnuityPolicyNum3,
             FundAnnuityAmt1,
             FundAnnuityAmt2,
             FundAnnuityAmt3,
             AnnuityRegInsurer1,
             AnnuityRegInsurer2,
             AnnuityRegInsurer3,
             AnnuityRegInsurerEmail1,
             AnnuityRegInsurerEmail2,
             AnnuityRegInsurerEmail3,
             AnnuityRegInsurerEmail4,
             AnnuityRegInsurerFSBNum1,
             AnnuityRegInsurerFSBNum2,
             AnnuityRegInsurerFSBNum3,
             AnnuityRegInsurerFSBNum4,
             AnnuityRegInsurerTelNumber1,
             AnnuityRegInsurerTelNumber2,
             AnnuityRegInsurerTelNumber3,
             AnnuityRegInsurerTelNumber4,
             AnnuityRegInsurerCellNumber1,
             AnnuityRegInsurerCellNumber2,
             AnnuityRegInsurerCellNumber3,
             AnnuityRegInsurerCellNumber4,
			 SpecialCondInstr,
             DeclarationInd   
        from TPDirectiveAD    
       where DirectiveSystemStatus = @SysStatusInterimFile    
    
   begin try
    
    update TPDirectiveAD    
       set DirectiveSystemStatus = @SysStatusSentFile    
     where DirectiveSystemStatus = @SysStatusInterimFile    
  
    end try
	begin catch
	rollback
	Select @Err = ': Error Final Indicator Directive status to sent'
	goto ErrHandler
	end catch 
    
  --**********************************************************    
  return    
    
  -- Error handler    
  ErrHandler:    
    raiserror (100000, 16, 1, @Err)   
    Return 1    
end    
