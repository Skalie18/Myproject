use [SARSeFilingDEV]
go

if not exists (select * from sys.objects where object_id = object_id(N'dbo.Process_I_DuplicateFORMAD') and type in (N'P', N'PC')) begin
              execute ('create procedure dbo.Process_I_DuplicateFORMAD as')
end else begin
              Exec DBA.dbo.objBackup_B_V_S  @dbName = 'SARSeFilingDEV', -- Target database
                                                                @objName = 'Process_I_DuplicateFORMAD',  -- sp, view or function name ** returns last backup if cripted
                                                                @objBUSavedBy = 'Admin', -- person who does the backup
                                                                @obJBUID = null,   -- Script with the objBUID instead of objName 
                                                                @actType = 'B';    -- B - Backup, V - View, S – Script
       end
go

  -- drop proc Process_I_DuplicateFORMAD    
-- exec Process_I_DuplicateFORMAD 10007, 0, 123, 308    
--***********************************************************************************    
-- Description: Selects Directive content    
-- Input: Unique DirectiveRequestID (MANDATORY) or directive Key (OPTIONAL)    
-- Output: All fields    
--***********************************************************************************    
ALTER PROC [dbo].[Process_I_DuplicateFORMAD]    
(   @DirectiveRequestID             decimal(18,0),    
    @DirectiveKey                   int,   -- optional may be zero    
    @BusinessProcessExecutionNum    int,    
    @TaxUserID                      decimal(10,0)  --limit what user can see    
    )    
    
as    
begin    
    
/*    
Modified: Vanessa Botha 20080111    
@TaxUserID decimal(10,0) from decimal(18,0)    
    
Set nocount on     
with (nolock) --VB    
*/    
    
Set nocount on     
    
  declare @DirectiveTypeCode     char(8),    
          @SystemStatus          char(5),    
          @DirectiveStatusCode   char(2),    
          @CompleteDt            datetime,    
          @DirectiveID           decimal(18,0),    
          @PrevDirectiveInd      char(1),    
          @FORMADKey             int    
     
  declare @SPName varchar(50),    
          @ErrorCheck tinyint,    
          @SPReturn tinyint,    
          @Err varchar(255)    
    
    
  select @SPName = 'Process_I_DuplicateFORMAD',    
         @ErrorCheck = 0,    
         @SPReturn = 0,    
         @Err = '',    
         @DirectiveTypeCode = 'FORMAD',    
         @SystemStatus = 'PEND',    
         @DirectiveStatusCode = 'SA',    
         @CompleteDt = getdate(),    
         @PrevDirectiveInd = 'Y'   -- default on duplicate 

    	 
		    
    
  -- retrieve the DirectiveKey if not provided    
  if @DirectiveKey = 0 begin    
    if (select count(*)     
          from DirectiveLookup with (nolock) --VB    
         where DirectiveRequestID = @DirectiveRequestID) > 0    
    begin    
      select @DirectiveKey = DirectiveKey    
        from DirectiveLookup with (nolock) --VB    
       where DirectiveTypeCode = @DirectiveTypeCode    
         and DirectiveRequestID = @DirectiveRequestID    
   
    end    
    else    
    begin    
      select @Err = @SPName + ': DirectiveRequestID does not exist'    
      goto ErrHandler    
    end    
  end    
    
   begin transaction    
    
      -- get next directive sequence number    
     begin try

      exec @SPReturn = Process_S_GetNextDirectiveKey 'NXDIRECT', @DirectiveID output    
    
	 end try
	 begin catch
	    Select @Err = 'Error get Unique Directive Request ID'
		rollback transaction 
	    goto ErrHandler
	 end catch      
    
	begin try

      -- add to DirectiveLookup    
      insert DirectiveLookup     
             (DirectiveRequestID,    
              DirectiveTypeCode,    
              DirectiveKey)    
      values (@DirectiveID,    
              @DirectiveTypeCode,    
              null)    
      
     end try
	 begin catch
	    Select @Err = 'Error on FORMAD Directive Lookup insert'
		rollback transaction 
	    goto ErrHandler
	 end catch

	begin try

      -- Add directive record.    
      insert TPDirectiveAD    
            (DirectiveRequestID,    
             TaxUserID,    
             TaxPayerID,    
             DirectiveTypeCode,    
             DirectiveSystemStatus,    
             DirectiveGroupID,    
             BusinessProcessExecutionNum,    
             CompleteDt,    
             SubmitDt,    
             PeriodYear,    
             PeriodNum,    
             DirectiveApplicationID,    
			 YearOfAssesmentDt,    
             IncomeTaxReferenceNum,    
             NoIncomeTaxCode,    
             NoIncomeTaxText,    
             AnnualIncomeAmt,    
             EmployeeNum,    
             PersonTitleCode,    
             Initials,    
             Surname,    
             FirstName,    
             DateOfBirth,    
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
             MembershipDt,    
             FundTypeCode,    
             FundCreateReasonCode,    
             GrossLumpAmt,    
             ReasonForDirectiveCode,    
             AccrualDt,    
             TotalContributionAmt ,    
             --ExcessContributionAmt ,    
             CalculationMethodCode,    
             CalculationYearFromDt,    
             CalculationYearToDt,    
             CalculationCompletedYears,    
             SalariesEnteredNum ,    
             Salary1StartDt,    
             Salary1EndDt,    
             Salary1Amt,    
             Salary2StartDt,    
             Salary2EndDt,    
             Salary2Amt,    
             Salary3StartDt,    
             Salary3EndDt,    
             Salary3Amt ,    
             Salary4StartDt,    
             Salary4EndDt,    
             Salary4Amt,    
             Salary5StartDt,    
             Salary5EndDt,    
             Salary5Amt,    
             SalaryTotalAmt,    
             SalaryAverageAmt ,    
             Salary12MonthPreDeathAmt,    
             EmployerName,    
             EmployerPAYENum,    
             EmployerContactPersonTitleCode,    
             EmployerContactPersonInitials,    
             EmployerContactPersonFirstName,    
             EmployerContactPersonSurname,    
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
             TaxFreePortionAmt,    
             GrossPayablePAYEAmt,    
             DeductionFrequency,    
             AllowedContributionAmt,    
             Comment,    
             DirectiveStatusCode,    
             PaperRespInd,  
             ParticipatingEmployerName,  
             FSBNumber,
			 AIPFDeemedContrib,
			 NonResidentInd,
			 ServRendOutsideRepInd,
			 ServRendContribFundMonth,
			 ServRendOutsideRepMonth,
			 MemFundContr,
			 MemFundContrAfter20160301,
			 AnnuityPolicyNum,
			 AnnuityPolicyNum1,
			 AnnuityPolicyNum2,
             AnnuityPolicyNum3,
			 FundAnnuityAmt,
             FundAnnuityAmt1,
             FundAnnuityAmt2,
             FundAnnuityAmt3,
			 AnnuityRegInsurer,
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
             DeclarationInd,
			 FormID)
			     
      select @DirectiveID,    
             @TaxUserID,    
             TaxPayerID,    
             @DirectiveTypeCode,    
             @SystemStatus,    
             DirectiveGroupID,    
             @BusinessProcessExecutionNum,    
             @CompleteDt,    
             null,  --SubmitDt,    
             PeriodYear,    
             PeriodNum,    
             DirectiveApplicationID,    
             YearOfAssesmentDt,    
			 IncomeTaxReferenceNum,    
             NoIncomeTaxCode,    
             NoIncomeTaxText,    
             AnnualIncomeAmt,    
             EmployeeNum,    
             PersonTitleCode,    
             Initials,    
             Surname,    
             FirstName,    
             DateOfBirth,    
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
             MembershipDt,    
             FundTypeCode,    
             FundCreateReasonCode,    
             GrossLumpAmt,    
             ReasonForDirectiveCode,    
             AccrualDt,    
             TotalContributionAmt ,    
             --ExcessContributionAmt ,    
             CalculationMethodCode,    
             CalculationYearFromDt,    
             CalculationYearToDt,    
             CalculationCompletedYears,    
             SalariesEnteredNum ,    
             Salary1StartDt,    
             Salary1EndDt,    
             Salary1Amt,    
             Salary2StartDt,    
             Salary2EndDt,    
             Salary2Amt,    
             Salary3StartDt,    
             Salary3EndDt,    
             Salary3Amt ,    
             Salary4StartDt,    
             Salary4EndDt,    
             Salary4Amt,    
             Salary5StartDt,    
             Salary5EndDt,    
             Salary5Amt,    
             SalaryTotalAmt,    
             SalaryAverageAmt ,    
             Salary12MonthPreDeathAmt,    
             EmployerName,    
             EmployerPAYENum,    
             EmployerContactPersonTitleCode,    
             EmployerContactPersonInitials,    
             EmployerContactPersonFirstName,    
             EmployerContactPersonSurname,    
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
             TaxFreePortionAmt,    
             GrossPayablePAYEAmt,    
             DeductionFrequency,    
             AllowedContributionAmt,    
             Comment,    
             @DirectiveStatusCode,    
             PaperRespInd,  
             ParticipatingEmployerName,  
             FSBNumber,
			 AIPFDeemedContrib,
			 NonResidentInd,
			 ServRendOutsideRepInd,
			 ServRendContribFundMonth,
			 ServRendOutsideRepMonth ,
			 MemFundContr
			 MemFundContrAfter20160301,
			 AnnuityPolicyNum,
			 AnnuityPolicyNum1,
			 AnnuityPolicyNum2,
             AnnuityPolicyNum3,
			 FundAnnuityAmt,
             FundAnnuityAmt1,
             FundAnnuityAmt2,
             FundAnnuityAmt3,
			 AnnuityRegInsurer,
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
             DeclarationInd,
			 FormID   
        from TPDirectiveAD     
       where FORMADID = @DirectiveKey    
    
      select @FORMADKey = SCOPE_IDENTITY()    
    
	 end try
	 begin catch
	    Select @Err = 'Error on FORMAD insert'
		rollback transaction 
	    goto ErrHandler
	 end catch    
    
     begin try 
	    
      -- update add key to DirectiveLookup    
      update DirectiveLookup     
         set DirectiveKey =@FORMADKey,    
             OriginalDirectiveKey = @DirectiveKey  -- set the parent key    
       where DirectiveRequestID = @DirectiveID    
         and DirectiveTypeCode = @DirectiveTypeCode    
    
    
     end try
	 begin catch
	    Select @Err = 'Error on FORMAD Directive Lookup update with key'
		rollback transaction 
	    goto ErrHandler
	 end catch

      -- insert status history record    
    begin try

      insert DirectiveStatusHistory    
              (DirectiveRequestID,    
               DirectiveTypeCode,    
               DirectiveKey,    
               DirectiveSystemStatus,    
               StatusHistoryDt,    
               BusinessProcessExecutionNum,    
               TaxUserID)    
      values ( @DirectiveID,    
               @DirectiveTypeCode,    
               @FORMADKey,    
               @SystemStatus,    
               getdate(),    
               @BusinessProcessExecutionNum,    
               @TaxUserID)    
    

	 end try
	 begin catch
	    Select @Err = 'Error on FORMAD Directive Status History Insert'
		rollback transaction 
	    goto ErrHandler
	 end catch  
  --**********************************************************    
   commit transaction    
  return    
    
  -- Error handler    
  ErrHandler:    
    raiserror (100000, 16, 1, @Err)   
    Return 1    
end    
    
  

