use [SARSeFilingDEV]
go

if not exists (select * from sys.objects where object_id = object_id(N'TPDirectiveFORMAD_S_FormInfo') and type in (N'P', N'PC')) begin

              execute ('create procedure TPDirectiveFORMAD_S_FormInfo as')

       end else begin

              Exec DBA.dbo.objBackup_B_V_S  @dbName = 'SARSeFilingDEV', -- Target database
                                                  @objName = 'TPDirectiveFORMAD_S_FormInfo',  -- sp, view or function name ** returns last backup if cripted
                                                  @objBUSavedBy = 'Admin', -- person who does the backup
                                                  @obJBUID = null,   -- Script with the objBUID instead of objName 
                                                  @actType = 'B';    -- B - Backup, V - View, S – Script

end
go
        
-- exec TPDirectiveFORMAD_S_FormInfo 389088, 15165, 263289        
--***********************************************************************************        
-- Description: Selects Directive content        
-- Input: Unique DirectiveRequestID (MANDATORY) or directive Key (OPTIONAL)        
-- Output: All fields        
ALTER PROC [dbo].[TPDirectiveFORMAD_S_FormInfo]        
(   @DirectiveRequestID             decimal(18,0),        
    @DirectiveKey                   int,        
    @TaxUserID                      decimal(10,0)  --limit what user can see        
    )        
        
as        
begin        
        
/*        
Modified: Vanessa Botha 20080111        
@TaxUserID decimal(10,0) from decimal(18,0)        
        
Modified: Nativ Ben-Meir 20090822 (Form Change)        
        
Set Nocount on         
with (nolock) --VB        
*/        
        
        
Set Nocount on         
        
  declare @DirectiveTypeCode  char(8),        
          @TaxOffDescription  varchar(50),        
          @TaxOffAddr1        varchar(50),        
          @TaxOffAddr2        varchar(50),        
          @TaxOffPCode        varchar(50),        
          @TaxOffTel          varchar(15),        
          @TaxOffFax          varchar(15),        
          @ErrorCheckCount    int        
        
  declare @SPName varchar(50),        
          @ErrorCheck tinyint,        
          @SPReturn tinyint,        
          @Err varchar(255)        
        
        
  select @SPName = 'TPDirectiveFORMAD_S_FormInfo',        
         @ErrorCheck = 0,        
         @SPReturn = 0,        
         @Err = '',        
         @DirectiveTypeCode = 'FORMAD'        
        
  -- to be added later lookup that the Taxuser passsed belongs to DirectiveGroupUser        
        
  -- retrieve the DirectiveKey if not provided        
  if @DirectiveKey = 0 begin        
    if (select count(*)         
          from DirectiveLookup        
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
        
 -- Check if Errors exist        
        
  select @ErrorCheckCount = count(*)        
    from DirectiveDeclined        
   where DirectiveRequestID = @DirectiveRequestID        
     
        
  -- Tax Office details        
        
     select @TaxOffDescription  = isnull(ta.Description, ''),        
            @TaxOffAddr1        = isnull(ta.PostalAddress1, ''),        
            @TaxOffAddr2        = isnull(ta.PostalAddress2, ''),        
            @TaxOffPCode        = isnull(ta.PostalPostCode, ''),        
            @TaxOffTel          = isnull(ta.TelNumberDirective, ''),        
            @TaxOffFax          = isnull(ta.FaxNumberDirective, '')        
       from TPDirectiveAD d with (nolock)        
      inner join TaxArea ta with (nolock) --VB        
         on d.ITArea = right('00' + ta.AreaCode,4)        
        and FORMADID = @DirectiveKey        
        
      
        
  -- select fields required        
      select FORMADID,        
             DirectiveRequestID,        
             d.TaxUserID,        
             d.TaxPayerID,        
             d.DirectiveGroupID,        
             d.DirectiveSystemStatus,        
             d.BusinessProcessExecutionNum,        
             --LTRIM(RTRIM(CONVERT(char,YEAR(isnull(d.CompleteDt, getdate()))))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(isnull(d.CompleteDt, getdate()))))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(isnull(d.CompleteDt, getdate())))))        
             convert(char, d.CompleteDt, 111) as CompleteDt,        
             --LTRIM(RTRIM(CONVERT(char,YEAR(SubmitDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(SubmitDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(SubmitDt))))        
             convert(char, SubmitDt, 111) as SubmitDt,        
             PeriodYear,        
             PeriodNum,        
			 DirectiveApplicationID,        
             --LTRIM(RTRIM(CONVERT(char,YEAR(YearOfAssesmentDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(YearOfAssesmentDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(YearOfAssesmentDt))))                    
             convert(char, YearOfAssesmentDt, 111) as YearOfAssesmentDt,        
             IncomeTaxReferenceNum,        
             NoIncomeTaxCode,        
             NoIncomeTaxText,        
             AnnualIncomeAmt,        
             EmployeeNum,        
             d.PersonTitleCode,        
             d.Initials,        
             d.Surname,        
             d.FirstName,        
             --LTRIM(RTRIM(CONVERT(char,YEAR(DateOfBirth)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(DateOfBirth)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(DateOfBirth))))                    
             convert(char, DateOfBirth, 111) as DateOfBirth,        
             d.IDNumber,        
             d.IDNumberOther,        
             d.PhysicalAddress1,        
             d.PhysicalAddress2,        
             d.PhysicalAddress3,        
             d.PhysicalAddress4,        
             d.PhysicalPostCode,        
             d.PostalAddress1,        
             d.PostalAddress2,        
             d.PostalAddress3,        
             d.PostalAddress4,        
             d.PostalPostCode,        
             d.TelAreaCode,        
             d.TelNumber,        
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
             --LTRIM(RTRIM(CONVERT(char,YEAR(MembershipDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(MembershipDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(MembershipDt))))        
             convert(char, MembershipDt, 111) as MembershipDt,        
             FundTypeCode,        
             FundCreateReasonCode,        
             GrossLumpAmt,        
             d.ReasonForDirectiveCode,        
             --LTRIM(RTRIM(CONVERT(char,YEAR(AccrualDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(AccrualDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(AccrualDt))))        
             convert(char, AccrualDt, 111) as AccrualDt,        
             TotalContributionAmt ,        
             --ExcessContributionAmt ,        
             CalculationMethodCode,        
             --LTRIM(RTRIM(CONVERT(char,YEAR(CalculationYearFromDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(CalculationYearFromDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(CalculationYearFromDt))))        
			 convert(char, CalculationYearFromDt, 111) as CalculationYearFromDt,        
             --LTRIM(RTRIM(CONVERT(char,YEAR(CalculationYearToDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(CalculationYearToDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(CalculationYearToDt))))        
             convert(char, CalculationYearToDt, 111)as CalculationYearToDt,        
             CalculationCompletedYears,        
             SalariesEnteredNum ,        
             --LTRIM(RTRIM(CONVERT(char,YEAR(Salary1StartDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(Salary1StartDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(Salary1StartDt))))        
             convert(char, Salary1StartDt, 111)as Salary1StartDt,        
             --LTRIM(RTRIM(CONVERT(char,YEAR(Salary1EndDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(Salary1EndDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(Salary1EndDt))))        
             convert(char, Salary1EndDt, 111) as Salary1EndDt,        
             Salary1Amt,        
             --LTRIM(RTRIM(CONVERT(char,YEAR(Salary2StartDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(Salary2StartDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(Salary2StartDt))))        
             convert(char, Salary2StartDt, 111) as Salary2StartDt,        
             --LTRIM(RTRIM(CONVERT(char,YEAR(Salary2EndDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(Salary2EndDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(Salary2EndDt))))        
             convert(char, Salary2EndDt, 111) as Salary2EndDt,        
             Salary2Amt,        
             --LTRIM(RTRIM(CONVERT(char,YEAR(Salary3StartDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(Salary3StartDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(Salary3StartDt))))        
             convert(char, Salary3StartDt, 111) as Salary3StartDt,        
             --LTRIM(RTRIM(CONVERT(char,YEAR(Salary3EndDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(Salary3EndDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(Salary3EndDt))))        
             convert(char, Salary3EndDt, 111) as Salary3EndDt,        
             Salary3Amt ,        
             --LTRIM(RTRIM(CONVERT(char,YEAR(Salary4StartDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(Salary4StartDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(Salary4StartDt))))        
             convert(char, Salary4StartDt, 111) as Salary4StartDt,        
             --LTRIM(RTRIM(CONVERT(char,YEAR(Salary4EndDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(Salary4EndDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(Salary4EndDt))))        
             convert(char, Salary4EndDt, 111) as Salary4EndDt,        
             Salary4Amt,        
             --LTRIM(RTRIM(CONVERT(char,YEAR(Salary5StartDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(Salary5StartDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(Salary5StartDt))))        
             convert(char, Salary5StartDt, 111) as Salary5StartDt,        
             --LTRIM(RTRIM(CONVERT(char,YEAR(Salary5EndDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(Salary5EndDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(Salary5EndDt))))        
             convert(char, Salary5EndDt, 111) as Salary5EndDt,        
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
             d.ITArea,        
             DirectiveStatusCode,        
             --LTRIM(RTRIM(CONVERT(char,YEAR(IssueDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(IssueDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(IssueDt))))                    
             convert(char, IssueDt, 111) as IssueDt,        
             --LTRIM(RTRIM(CONVERT(char,YEAR(ValidStartDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(ValidStartDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(ValidStartDt))))                    
             convert(char, ValidStartDt, 111) as ValidStartDt,        
             --LTRIM(RTRIM(CONVERT(char,YEAR(ValidEndDt)))) + '/' + LTRIM(RTRIM(CONVERT(char,MONTH(ValidEndDt)))) +'/' + LTRIM(RTRIM(CONVERT(char,DAY(ValidEndDt))))                    
             convert(char, ValidEndDt, 111) as ValidEndDt,        
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
             DirectiveID,        
             SARSIncomeTaxReferenceNum,        
             rtrim(tu.PersonTitleCode) + ' ' + rtrim(tu.Initials) + ' ' + tu.Surname as TaxUserName,        
             tu.Occupation as Capacity,        
             @TaxOffDescription  as RORAreaName,        
             @TaxOffAddr1        as RORAddress1,        
             @TaxOffAddr2        as RORAddress2,        
             @TaxOffPCode        as RORPostalCode,        
             @TaxOffTel          as RORTelNum,        
             @TaxOffFax          as RORFaxNum,        
             @ErrorCheckCount    as ErrorCheckCount,        
             dr.Description      as ReasonForDirectiveDesc,        
             d.GrossTotalBenefitAmt,        
             d.DivorceSpouseAmt,        
             d.BenefitCalcInd,        
             d.FundAnnuityInd,        
             d.FundAnnuityAmt,        
             d.AnnuityRegInsurer,        
             d.AnnuityPolicyNum,        
             d.FundPayAnnuityInd,        
             d.FundRemainAnnuityAmt,        
			 --d.UnclaimedBenefPrevTaxedAmt,  -- NBM 20090822        
			 d.EmailAddressAdministrator,        
			 d.TaxedTransferNonMemberSpouse,  
			 d.ParticipatingEmployerName,  
			 d.FSBNumber,
			 d.AIPFDeemedContrib,
			 d.NonResidentInd,
			 d.ServRendOutsideRepInd,
			 d.ServRendContribFundMonth,
			 d.ServRendOutsideRepMonth,
			 T.VersionCode,
			 d.SourceCode,
			 d.YearOfAssessment,
			 d.Pre1March1998Amt,
			 d.TrfAmt,
			 d.MemFundContr,
			 d.ExcContribAmt,
			 d.TaxedTransfNonMembSpouse,
			 d.ExemptServices,
			 d.AIPFMemberContributions ,
			 d.AdminPenalty,
			 d.MemFundContrAfter20160301,
			 d.AnnuityPolicyNum1,
			 d.AnnuityPolicyNum2,
			 d.AnnuityPolicyNum3,
			 d.FundAnnuityAmt1,
			 d.FundAnnuityAmt2,
			 d.FundAnnuityAmt3,
			 d.AnnuityRegInsurer1,
			 d.AnnuityRegInsurer2,
			 d.AnnuityRegInsurer3,
			 d.AnnuityRegInsurerEmail1,
			 d.AnnuityRegInsurerEmail2,
			 d.AnnuityRegInsurerEmail3,
			 d.AnnuityRegInsurerEmail4,
			 d.AnnuityRegInsurerFSBNum1,
			 d.AnnuityRegInsurerFSBNum2,
			 d.AnnuityRegInsurerFSBNum3,
			 d.AnnuityRegInsurerFSBNum4,
			 d.AnnuityRegInsurerTelNumber1,
			 d.AnnuityRegInsurerTelNumber2,
			 d.AnnuityRegInsurerTelNumber3,
			 d.AnnuityRegInsurerTelNumber4,
			 d.AnnuityRegInsurerCellNumber1,
			 d.AnnuityRegInsurerCellNumber2,
			 d.AnnuityRegInsurerCellNumber3,
			 d.AnnuityRegInsurerCellNumber4,
			 d.SpecialCondInstr,
			 d.DeclarationInd 
        from TPDirectiveAD d with (nolock) --VB        
       inner join TaxUser tu with (nolock) --VB        
          on d.TaxUserID = tu.TaxUserID        
         and d.FORMADID = @DirectiveKey        
       inner join DirectiveReason dr with (nolock) --VB        
          on d.ReasonForDirectiveCode = dr.ReasonForDirectiveCode        
         and dr.DirectiveTypeCode = @DirectiveTypeCode        
       inner join DirectiveGroupUser  GU with (nolock)                   
          on GU.TaxUserID = @TaxUserID                    
         --and GU.DirectivesActiveInd = 1       
         and GU.DirectiveGroupID = d.DirectiveGroupID      
      join Template T with(nolock) 
		 on T.TemplateID = d.FormID 
       
        
  --**********************************************************        
  return        
        
  -- Error handler        
  ErrHandler:        
    raiserror (100000, 16, 1, @Err)       
    Return 1        
end        
   

