use [SARSeFilingDEV]
go

if not exists (select * from sys.objects where object_id = object_id(N'TPDirectiveFORMAD_I_U_SaveRequest') and type in (N'P', N'PC')) begin

              execute ('create procedure TPDirectiveFORMAD_I_U_SaveRequest as')

       end else begin

              Exec DBA.dbo.objBackup_B_V_S  @dbName = 'SARSeFilingDEV', -- Target database
                                                  @objName = 'TPDirectiveFORMAD_I_U_SaveRequest',  -- sp, view or function name ** returns last backup if cripted
                                                  @objBUSavedBy = 'Admin', -- person who does the backup
                                                  @obJBUID = null,   -- Script with the objBUID instead of objName 
                                                  @actType = 'B';    -- B - Backup, V - View, S – Script

end
go

  --***********************************************************************************            
-- Description: INserts the FORMAD directive as well as updates, note not all fields             
--               are updated            
-- Input: All fields to insert as well as FORMADID key and Unique DirectiveID (for update)            
-- Output: FORMADID and DirectiveRequestID for both INsert and update            
--***********************************************************************************            
ALTER PROC [dbo].[TPDirectiveFORMAD_I_U_SaveRequest]          
(            
    @FORMADID                       int,             -- 0 if insert            
    @DirectiveRequestID             decimal(18,0),   -- 0 if insert            
    @SystemStatus                   char(5),         -- PEND, SUBMTO            
    @TaxUserID                      decimal(10,0),            
    @TaxPayerID                     decimal(10,0),            
    @DirectiveGroupID               decimal(18,0),            
    @BusinessProcessExecutionNum    int,            
    @CompleteDt                     varchar(25),            
    @SubmitDt                       varchar(25),            
    @PeriodYear                     int,            
    @PeriodNum                      tinyint,            
    @DirectiveApplicationID         varchar(50),            
    @YearOfAssesmentDt              varchar(25),            
    @IncomeTaxReferenceNum          varchar(25),            
    @NoIncomeTaxCode                char(2),            
    @NoIncomeTaxText                varchar(65),            
    @AnnualIncomeAmt                money,            
    @EmployeeNum                    varchar(15),            
    @PersonTitleCode                char(8),            
    @Initials                       varchar(15),            
    @Surname                        varchar(120),            
    @FirstName                      varchar(50),            
    @DateOfBirth                    varchar(25),            
    @IDNumber                       varchar(25),            
    @IDNumberOther                  varchar(25),            
    @PhysicalAddress1               varchar(50),            
    @PhysicalAddress2               varchar(50),            
    @PhysicalAddress3               varchar(50),            
    @PhysicalAddress4               varchar(50),            
    @PhysicalPostCode               varchar(50),            
    @PostalAddress1                 varchar(50),            
    @PostalAddress2                 varchar(50),            
    @PostalAddress3                 varchar(50),            
    @PostalAddress4                 varchar(50),            
    @PostalPostCode                 varchar(50),            
    @TelAreaCode                    varchar(10),            
    @TelNumber                      varchar(15),            
    @FundName                       varchar(120),            
    @FundNumber                     varchar(25),            
    @FundPAYENum                    varchar(25),            
    @FundContactPersonTitleCode     char(8),            
    @FundContactPersonInitials      varchar(15),            
    @FundContactPersonFirstName     varchar(50),            
    @FundContactPersonSurname       varchar(50),            
    @FundTelAreaCode                varchar(10),            
    @FundTelNumber                  varchar(15),            
    @FundPostalAddress1             varchar(50),            
    @FundPostalAddress2             varchar(50),            
    @FundPostalAddress3             varchar(50),            
    @FundPostalAddress4             varchar(50),            
    @FundPostalPostCode             varchar(50),            
    @MembershipNum                  varchar(15),            
    @MembershipDt                   varchar(25),            
    @FundTypeCode                   char(2),            
    @FundCreateReasonCode           char(2),            
    @GrossLumpAmt                   money,            
    @ReasonForDirectiveCode         char(2),            
    @AccrualDt                      varchar(25),            
    @TotalContributionAmt   money,            
   -- @ExcessContributionAmt          money,            
    @CalculationMethodCode          char(2) = null,            
    @CalculationYearFromDt          varchar(25),            
    @CalculationYearToDt            varchar(25),            
    @CalculationCompletedYears      int,            
    @SalariesEnteredNum             int,            
    @Salary1StartDt                 varchar(25),            
    @Salary1EndDt                   varchar(25),           
    @Salary1Amt                     money,            
    @Salary2StartDt                 varchar(25),            
    @Salary2EndDt                   varchar(25),            
    @Salary2Amt                     money,            
    @Salary3StartDt                 varchar(25),            
    @Salary3EndDt                   varchar(25),            
    @Salary3Amt                     money,            
    @Salary4StartDt                 varchar(25),            
    @Salary4EndDt                   varchar(25),            
    @Salary4Amt                     money,            
    @Salary5StartDt                 varchar(25),            
    @Salary5EndDt                   varchar(25),            
    @Salary5Amt                     money,            
    @SalaryTotalAmt                 money,            
    @SalaryAverageAmt               money,            
    @Salary12MonthPreDeathAmt       money,            
    @EmployerName                   varchar(120),            
    @EmployerPAYENum                varchar(25),            
    @EmployerContactPersonTitleCode char(8),            
    @EmployerContactPersonInitials  varchar(15),            
    @EmployerContactPersonFirstName varchar(50),            
    @EmployerContactPersonSurname   varchar(50),            
    @EmployerTelAreaCode            varchar(10),            
    @EmployerTelNumber              varchar(10),            
    @EmployerPhysicalAddress1       varchar(50),            
    @EmployerPhysicalAddress2       varchar(50),            
    @EmployerPhysicalAddress3       varchar(50),            
    @EmployerPhysicalAddress4       varchar(50),            
    @EmployerPhysicalPostCode       varchar(50),            
    @EmployerPostalAddress1         varchar(50),            
    @EmployerPostalAddress2         varchar(50),            
    @EmployerPostalAddress3         varchar(50),            
    @EmployerPostalAddress4         varchar(50),            
    @EmployerPostalPostCode         varchar(50),            
    @TaxFreePortionAmt              money,            
    @GrossPayablePAYEAmt            money,            
    @DeductionFrequency             char(1),            
    @AllowedContributionAmt         money,            
    @Comment                        varchar(255),            
    @ApplicantSaveInd               tinyint,            
    @EmployerSaveInd                tinyint,            
    @FundSaveInd                    tinyint,            
    @GrossTotalBenefitAmt           money,            
    @DivorceSpouseAmt               money,            
    @BenefitCalcInd                 tinyint,            
    @FundAnnuityInd                 tinyint,            
    @FundAnnuityAmt                 money,            
    @AnnuityRegInsurer              varchar(120),            
    @AnnuityPolicyNum               varchar(20),            
    @FundPayAnnuityInd              tinyint,            
    @FundRemainAnnuityAmt           money,          
	--@UnclaimedBenefPrevTaxedAmt     money,          
	@EmailAddressAdministrator      varchar(50),          
	@TaxedTransferNonMemberSpouse   money,      
	@ParticipatingEmployerName      varchar(120),      
	@FSBNumber                      varchar(25),
	@AIPFDeemedContrib              varchar(15),
	@NonResidentInd					char(1),
	--@ServRendOutsideRepInd			char(1),
	--@ServRendContribFundMonth		varchar(15),
	--@ServRendOutsideRepMonth		varchar(15),
	@MemFundContr money,
	@MemFundContrAfter20160301  money,
	@AnnuityPolicyNum1 varchar(20),
	@AnnuityPolicyNum2 varchar(20),
	@AnnuityPolicyNum3 varchar(20),
	@FundAnnuityAmt1 money,
	@FundAnnuityAmt2 money,
	@FundAnnuityAmt3 money,
	@AnnuityRegInsurer1 varchar(120),
	@AnnuityRegInsurer2 varchar(120),
	@AnnuityRegInsurer3 varchar(120),
	@AnnuityRegInsurerEmail1 varchar(50),
	@AnnuityRegInsurerEmail2 varchar(50),
	@AnnuityRegInsurerEmail3 varchar(50),
	@AnnuityRegInsurerEmail4 varchar(50),
	@AnnuityRegInsurerFSBNum1  varchar(12),
	@AnnuityRegInsurerFSBNum2  varchar(12),
	@AnnuityRegInsurerFSBNum3  varchar(12),
	@AnnuityRegInsurerFSBNum4  varchar(12),
	@AnnuityRegInsurerTelNumber1 varchar(20),
	@AnnuityRegInsurerTelNumber2 varchar(20),
	@AnnuityRegInsurerTelNumber3 varchar(20),
	@AnnuityRegInsurerTelNumber4 varchar(20),
	@AnnuityRegInsurerCellNumber1 varchar(20),
	@AnnuityRegInsurerCellNumber2 varchar(20),
	@AnnuityRegInsurerCellNumber3 varchar(20),
	@AnnuityRegInsurerCellNumber4 varchar(20),
	@SpecialCondInstr      varchar(120),
	@DeclarationInd        char(1) )            
as            
begin            
            
/*          
Modified: Vanessa Botha 20080111          
@TaxUserID and @TaxPayerID decimal(10,0) from decimal(18,0)          
          
Modified: Nativ Ben-Meir 20090822 (Form Change)          
          
Set Nocount on           
with (nolock) --VB          
*/          
Set Nocount on           
          
          
  declare @DirectiveID           decimal(18,0), -- DirectiveRequestID generated here            
          @FORMADKey             int,           
          @OldSystemStatus       char(5),            
          @DirectiveTypeCode     char(8),            
          @DirectiveStatusCode   char(2),            
          @PaperRespInd          tinyint,          
          @CalcGrossLumpAmt      money          
             
  declare @SPName varchar(50),            
          @ErrorCheck tinyint,            
          @SPReturn tinyint,            
          @Err varchar(255),            
          @BusRuleCheck int,            
          @BusRuleErr varchar(255)            
            
            
            
  begin transaction            
            
  select @SPName = 'TPDirectiveFORMAD_I_U_SaveRequest',            
         @ErrorCheck = 0,            
         @SPReturn = 0,           
         @Err = '',            
         @DirectiveID = @DirectiveRequestID,            
         @FORMADKey = @FORMADID,            
         @DirectiveTypeCode = 'FORMAD',            
         @PaperRespInd = 0,            
         @BusRuleCheck = 0,          
         @CalcGrossLumpAmt = 0 
		 
 declare @FormID int

	select @FormID = TemplateID 
	from Template
	where FunctionalityCode = 'DIRECT'
	and CurrentInd = 1	            
            
-- set the date values            
            
    if @CompleteDt = '' begin            
      select @CompleteDt = null            
    end                                 
    if @SubmitDt = '' begin            
      select @SubmitDt = null            
    end                                             
    if @YearOfAssesmentDt = '' begin            
      select @YearOfAssesmentDt = null            
    end                                    
    if @MembershipDt = '' begin            
      select @MembershipDt = null            
    end                                         
    if @AccrualDt = '' begin            
      select @AccrualDt = null            
    end                                            
    if @CalculationYearFromDt = '' begin            
      select @CalculationYearFromDt = null            
    end                                
    if @CalculationYearToDt = '' begin            
      select @CalculationYearToDt = null            
    end                                  
    if @Salary1StartDt = '' begin            
      select @Salary1StartDt = null            
    end                                       
    if @Salary1EndDt = '' begin            
      select @Salary1EndDt = null            
    end                                         
    if @Salary2StartDt = '' begin            
      select @Salary2StartDt = null            
    end                                       
    if @Salary2EndDt = '' begin            
      select @Salary2EndDt = null            
    end                                         
    if @Salary3StartDt = '' begin            
      select @Salary3StartDt = null            
    end                                       
    if @Salary3EndDt = '' begin            
      select @Salary3EndDt = null            
    end                                         
    if @Salary4StartDt = '' begin            
      select @Salary4StartDt = null            
    end                                       
    if @Salary4EndDt = '' begin            
      select @Salary4EndDt = null            
    end                                         
    if @Salary5StartDt = '' begin            
      select @Salary5StartDt = null            
    end                                       
    if @Salary5EndDt = '' begin            
      select @Salary5EndDt = null            
    end            
            
-- Add the TaxPayerID based on tax user default and allow selection            
            
  --default System Status            
  if (@SystemStatus = '') or (@SystemStatus is null) begin            
     select @SystemStatus = 'PEND'            
  end            
            
  if @SystemStatus = 'PEND' begin            
    select @CompleteDt = convert(varchar(25), getdate(), 113),            
           @SubmitDt = null,            
           @DirectiveStatusCode = 'SA'            
  end            
  else if @SystemStatus = 'SBMTO' begin            
    select @CompleteDt = convert(varchar(25), getdate(), 113),            
           @SubmitDt = convert(varchar(25), getdate(), 113),            
           @DirectiveStatusCode = 'SB'            
  end            
              
            
-- Check that the business rule.            
            
   if isnull(@SubmitDt, '') <> '' begin            
            
     select @BusRuleErr = ''            
          
     select @CalcGrossLumpAmt = @GrossTotalBenefitAmt          
     if isnull(@DivorceSpouseAmt, 0) > 0 begin          
       set @CalcGrossLumpAmt = @CalcGrossLumpAmt - @DivorceSpouseAmt          
       set @CalcGrossLumpAmt = @CalcGrossLumpAmt/3          
       set @CalcGrossLumpAmt = @CalcGrossLumpAmt + @DivorceSpouseAmt          
       set @CalcGrossLumpAmt = round(@CalcGrossLumpAmt, 2)          
     end          
          
     if isnull(@YearOfAssesmentDt, '') = '' or isdate(@YearOfAssesmentDt) = 0 begin            
       select @BusRuleErr = @BusRuleErr + 'Year of Assesment date, '            
       select @BusRuleCheck = 99               
     end            
            
     if isnull(@EmailAddressAdministrator, '') = ''begin            
       select @BusRuleErr = @BusRuleErr + 'Administrator EmailAddress, '            
       select @BusRuleCheck = 99               
     end            
          
--     if isnull(@FundNumber, '') = '' and @FundCreateReasonCode <> '99' begin            
--select @BusRuleErr = @BusRuleErr + 'Fund Number, '            
--       select @BusRuleCheck = 99               
--     end  
  -- NM

   if  isnull(@FundNumber, '') <>'' and isnull(@FSBNumber, '') <> '' begin
	   select @BusRuleErr = @BusRuleErr + 'Either Fund Number or FSB Number must be provided, '            
	   select @BusRuleCheck = 99 
  end

   if  isnull(@FundNumber, '') = '' and isnull(@FSBNumber, '') = '' begin
	   select @BusRuleErr = @BusRuleErr + 'Either Fund Number or FSB Number must be provided, '            
	   select @BusRuleCheck = 99 
  end
  
    
  if isnull(@FSBNumber, '') = '' and @FundCreateReasonCode = '02' begin            
   select @BusRuleErr = @BusRuleErr + 'FSB Registration Number, '            
   select @BusRuleCheck = 99               
  end   
    
  --if isnull(@FundNumber, '') = '' and isnull(@FSBNumber, '') = '' and @FundCreateReasonCode = '01' begin            
  -- select @BusRuleErr = @BusRuleErr + 'Fund/FSB Number, '            
  -- select @BusRuleCheck = 99               
  --end 

  -- NM 
   if isnull(@FundNumber, '') = '' and @FundCreateReasonCode = '01' begin            
   select @BusRuleErr = @BusRuleErr + 'Fund Number, '            
   select @BusRuleCheck = 99               
  end                      
            
  if isnull(@FSBNumber, '') <> '' and SUBSTRING(@FSBNumber,14,6) > 0 and isnull(@ParticipatingEmployerName, '') = '' begin              
select @BusRuleErr = @BusRuleErr + 'Participating Employer Name, '              
   select @BusRuleCheck = 99                  
  end   
         
     if isnull(@EmployerName, '') = '' begin            
       select @BusRuleErr = @BusRuleErr + 'Employer Name, '            
       select @BusRuleCheck = 99               
     end            
            
     if isnull(@EmployerPAYENum, '') = '' begin            
       select @BusRuleErr = @BusRuleErr + 'Employer PAYE Num, '            
       select @BusRuleCheck = 99               
     end            
            
     --if isnull(@AnnualIncomeAmt, 0) = 0 begin            
     --  select @BusRuleErr = @BusRuleErr + 'Annual Income, '            
     --  select @BusRuleCheck = 99               
     --end    
       
     if isnull(@AnnualIncomeAmt, 0) = 0 and convert(datetime,@AccrualDt) < convert(datetime,'2007/10/01') begin          
       select @BusRuleErr = @BusRuleErr + 'Annual Income, '          
       select @BusRuleCheck = 99             
     end     
	 

     if isnull(@AccrualDt, '') = '' or isdate(@AccrualDt) = 0  begin            
       select @BusRuleErr = @BusRuleErr + 'Accrual date, '            
       select @BusRuleCheck = 99               
     end            
            
     if isnull(@MembershipDt, '') = '' or isdate(@MembershipDt) = 0  begin            
       select @BusRuleErr = @BusRuleErr + 'Membership date, '            
       select @BusRuleCheck = 99               
     end            
            
     if isnull(@DateOfBirth, '') = '' or isdate(@DateOfBirth) = 0  begin    
       select @BusRuleErr = @BusRuleErr + 'Date Of Birth, '            
     select @BusRuleCheck = 99               
     end            
            
     if isdate(@DateOfBirth) = 1 and isdate(@AccrualDt) = 1  begin            
       if (select datediff(dd, convert(datetime,@DateOfBirth),  convert(datetime,@AccrualDt))) < 0 begin            
         select @BusRuleErr = @BusRuleErr + 'Date Of Birth after Accrual Date, '            
         select @BusRuleCheck = 99               
       end            
     end            
            
     if isnull(@GrossLumpAmt, 0) = 0 begin            
       select @BusRuleErr = @BusRuleErr + 'Gross Lump Sum amount, '            
       select @BusRuleCheck = 99               
     end            
            
     if isnull(@Salary1Amt, 0) = 0 begin            
       select @BusRuleErr = @BusRuleErr + 'Salary 1 amount, '            
       select @BusRuleCheck = 99               
     end            
            
     --if isnull(@FundNumber, '') = '' and @FundCreateReasonCode <> '99' begin            
     --  select @BusRuleErr = @BusRuleErr + 'Fund Number, '            
     --  select @BusRuleCheck = 99               
     --end     
                    
     if isnull(@FundPAYENum, '') = '' begin            
       select @BusRuleErr = @BusRuleErr + 'Fund PAYE Number, '            
       select @BusRuleCheck = 99               
     end            
            
     if isnull(@FundName, '') = '' begin            
       select @BusRuleErr = @BusRuleErr + 'Fund Name, '            
       select @BusRuleCheck = 99               
     end            
            
     if isnull(@FundContactPersonSurname, '') = '' begin            
       select @BusRuleErr = @BusRuleErr + 'Fund Contact Person Surname, '            
       select @BusRuleCheck = 99               
     end            
            
     if isnull(@MembershipNum, '') = '' begin            
       select @BusRuleErr = @BusRuleErr + 'Membership Number, '            
       select @BusRuleCheck = 99               
     end            
            
     if isnull(@FundTypeCode, '') = '' begin            
       select @BusRuleErr = @BusRuleErr + 'Fund Type Code, '            
       select @BusRuleCheck = 99               
     end            
            
     if isnull(@FundCreateReasonCode, '') = '' begin            
       select @BusRuleErr = @BusRuleErr + 'Fund Create Reason Code, '       
       select @BusRuleCheck = 99               
     end            
            
     if isnull(@ReasonForDirectiveCode, '') = '' begin            
       select @BusRuleErr = @BusRuleErr + 'Reason For Directive Code, '            
       select @BusRuleCheck = 99               
     end            
            
     if isnull(@CalculationYearFromDt, '') = '' or isdate(@CalculationYearFromDt) = 0  begin            
       select @BusRuleErr = @BusRuleErr + 'Benefit Calculation Start Date, '            
       select @BusRuleCheck = 99               
     end            
            
     if isnull(@CalculationYearToDt, '') = '' or isdate(@CalculationYearToDt) = 0  begin            
       select @BusRuleErr = @BusRuleErr + 'Benefit Calculation End Date, '            
       select @BusRuleCheck = 99               
     end            
            
     if isdate(@CalculationYearFromDt) = 1 and isdate(@CalculationYearToDt) = 1  begin            
       if (select datediff(dd, convert(datetime,@CalculationYearFromDt),  convert(datetime,@CalculationYearToDt))) < 0 begin            
         select @BusRuleErr = @BusRuleErr + 'Benefit Calculation Start Date after Benefit Calculation End Date, '            
         select @BusRuleCheck = 99               
       end            
     end            
            
     if isnull(@GrossTotalBenefitAmt, 0) = 0 begin            
       select @BusRuleErr = @BusRuleErr + 'Gross Total Benefit Amount, '            
       select @BusRuleCheck = 99               
     end            
          
     if @ReasonForDirectiveCode = '25' and @GrossLumpAmt <> @GrossTotalBenefitAmt begin            
       select @BusRuleErr = @BusRuleErr + 'Gross Lump Sum Payable and the Gross Amount of Total Benefit must be equal, '            
       select @BusRuleCheck = 99               
     end              
          
  if @ReasonForDirectiveCode = '25' and (isnull(@FundAnnuityInd, 0) = 1 or isnull(@FundPayAnnuityInd, 0) = 1) begin            
       select @BusRuleErr = @BusRuleErr + 'Neither Annuity Purchased/Transferred nor Fund Paying Annuity can be selected when the Directive Reason is Surplus Apportionment, '            
       select @BusRuleCheck = 99               
     end   
	-- NM 
	if isnull(@Salary12MonthPreDeathAmt,0) = 0 and @ReasonForDirectiveCode = '01'and convert(datetime,@AccrualDt) < convert(datetime,'2007/10/01') begin
	   select @BusRuleErr = @BusRuleErr + 'Salary 12 Month Preceding Death Amount, '            
       select @BusRuleCheck = 99
	
	end        
            
     if isnull(@FundAnnuityInd, 0) = 1 and  isnull(@FundPayAnnuityInd, 0) = 1 begin            
       select @BusRuleErr = @BusRuleErr + 'Annuity Purchased/Transferred and Fund Paying Annuity cannot both be selected, '            
       select @BusRuleCheck = 99               
     end          
          
     if isnull(@FundAnnuityInd, 0) = 0 and  isnull(@FundPayAnnuityInd, 0) = 0 and @FundTypeCode = '01' and @GrossLumpAmt <> @CalcGrossLumpAmt begin            
       select @BusRuleErr = @BusRuleErr + 'Annuity Purchased/Transferred and Fund Paying Annuity cannot both be not selected because the total benefit of the Provident fund has not all been taken, '          
       select @BusRuleCheck = 99               
     end          
          
     --if isnull(@FundAnnuityInd, 0) = 0 and  isnull(@FundPayAnnuityInd, 0) = 0 and @FundTypeCode = '02' and @GrossLumpAmt > 25199 begin            
     --  select @BusRuleErr = @BusRuleErr + 'Annuity Purchased/Transferred and Fund Paying Annuity cannot both be not selected because the gross amount of lump sum is not less than 25200, '            
     --  select @BusRuleCheck = 99               
     --end          
            
     if isnull(@FundAnnuityInd, 0) = 1 begin            
       if isnull(@FundAnnuityAmt, 0) <= 0 begin            
         select @BusRuleErr = @BusRuleErr + 'Fund Annuity Amount, '            
         select @BusRuleCheck = 99            
       end            
            
       if isnull(@AnnuityRegInsurer, '') = '' begin            
         select @BusRuleErr = @BusRuleErr + 'Name Of Registered Insurer, '            
         select @BusRuleCheck = 99            
       end            
            
       if isnull(@AnnuityPolicyNum, '') = '' begin            
         select @BusRuleErr = @BusRuleErr + 'Annuity Policy Number, '            
         select @BusRuleCheck = 99            
       end            
                   
     end            
            
     if isnull(@FundPayAnnuityInd, 0) = 1 begin            
       if isnull(@FundRemainAnnuityAmt, 0) <= 0 begin            
select @BusRuleErr = @BusRuleErr + 'Remaining Fund Amount, '            
         select @BusRuleCheck = 99            
       end              
     end            
          
            
     if @BusRuleCheck <> 0 begin            
       select @BusRuleErr = 'Mandatory field/s: ' + @BusRuleErr + ' please correct and submit again.',            
              @CompleteDt = convert(varchar(25), getdate(), 113),            
              @SubmitDt = null,            
              @DirectiveStatusCode = 'SA',            
              @SystemStatus = 'PEND'            
     end            
            
   end              
            
-- Try to see if directive already exists - for update if not provided            
            
  -- retrieve the DirectiveKey if not provided            
  if (isnull(@FORMADID, 0) = 0)and (isnull(@DirectiveRequestID, 0) <> 0) begin            
    if (select count(*)             
          from DirectiveLookup  with (nolock) --VB          
         where DirectiveRequestID = @DirectiveRequestID) > 0            
    begin            
      select @FORMADID = DirectiveKey            
        from DirectiveLookup  with (nolock) --VB          
       where DirectiveTypeCode = @DirectiveTypeCode            
         and DirectiveRequestID = @DirectiveRequestID            
            
           
    end            
    else            
    begin            
      select @FORMADID = 0            
    end            
  end            
            
    --**********************************************************            
    -- Add FORMAD record             
    if ((select count(*)             
          from TPDirectiveAD    with (nolock) --VB          
         where FORMADID = @FORMADID) = 0) or (@FORMADID = 0)            
    begin            
            
      -- get next directive sequence number            
    begin try    
		    
      exec @SPReturn = Process_S_GetNextDirectiveKey 'NXDIRECT', @DirectiveID output            
            

	 end try
	 begin catch
	    Select @Err = 'Error get Unique Directive Request ID'
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
             GrossTotalBenefitAmt,            
             DivorceSpouseAmt,            
             BenefitCalcInd,            
             FundAnnuityInd,            
             FundAnnuityAmt,            
             AnnuityRegInsurer,            
             AnnuityPolicyNum,            
             FundPayAnnuityInd,            
             FundRemainAnnuityAmt,          
			-- UnclaimedBenefPrevTaxedAmt,  -- NBM 20090822          
			 EmailAddressAdministrator,          
			 TaxedTransferNonMemberSpouse,      
			 ParticipatingEmployerName,      
			 FSBNumber,
			 AIPFDeemedContrib,
			 NonResidentInd,
			 --ServRendOutsideRepInd,
			 --ServRendContribFundMonth,
			 --ServRendOutsideRepMonth,
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
			 DeclarationInd,
			 FormID)   
			       
      values(@DirectiveID,  -- unique generated number across directives            
             @TaxUserID,            
             @TaxPayerID,            
             @DirectiveTypeCode,            
             @SystemStatus,            
             @DirectiveGroupID,            
             @BusinessProcessExecutionNum,            
             convert(datetime,@CompleteDt),            
             convert(datetime, @SubmitDt),            
             @PeriodYear,            
             @PeriodNum,            
             @DirectiveApplicationID,            
             convert(datetime, @YearOfAssesmentDt),            
             @IncomeTaxReferenceNum,            
             @NoIncomeTaxCode,            
             @NoIncomeTaxText,            
             round(@AnnualIncomeAmt,0),            
             @EmployeeNum,            
             @PersonTitleCode,            
             @Initials,            
             @Surname,            
             @FirstName,            
             convert(datetime, @DateOfBirth),            
             @IDNumber,            
             @IDNumberOther,            
             @PhysicalAddress1,            
             @PhysicalAddress2,            
             @PhysicalAddress3,            
             @PhysicalAddress4,            
             @PhysicalPostCode,            
             @PostalAddress1,            
             @PostalAddress2,            
             @PostalAddress3,            
             @PostalAddress4,            
             @PostalPostCode,            
             @TelAreaCode,            
             @TelNumber,            
             @FundName,            
             @FundNumber ,            
             @FundPAYENum ,             
             @FundContactPersonTitleCode ,            
             @FundContactPersonInitials ,            
             @FundContactPersonFirstName ,            
             @FundContactPersonSurname,            
             @FundTelAreaCode,            
             @FundTelNumber,            
             @FundPostalAddress1,            
             @FundPostalAddress2,            
             @FundPostalAddress3,            
             @FundPostalAddress4,            
             @FundPostalPostCode,            
             @MembershipNum,            
             convert(datetime, @MembershipDt),            
             @FundTypeCode,            
             @FundCreateReasonCode,            
             @GrossLumpAmt,            
             @ReasonForDirectiveCode,            
             convert(datetime, @AccrualDt),            
             @TotalContributionAmt ,            
            -- @ExcessContributionAmt ,            
             @CalculationMethodCode,            
             convert(datetime, @CalculationYearFromDt),            
             convert(datetime, @CalculationYearToDt),            
             @CalculationCompletedYears,            
             @SalariesEnteredNum ,            
             convert(datetime, @Salary1StartDt),            
             convert(datetime, @Salary1EndDt),            
             @Salary1Amt,            
             convert(datetime, @Salary2StartDt),            
             convert(datetime, @Salary2EndDt),            
             @Salary2Amt,            
             convert(datetime, @Salary3StartDt),            
             convert(datetime, @Salary3EndDt),            
             @Salary3Amt ,            
             convert(datetime, @Salary4StartDt),            
             convert(datetime, @Salary4EndDt),            
             @Salary4Amt,            
             convert(datetime, @Salary5StartDt),            
             convert(datetime, @Salary5EndDt),            
             @Salary5Amt,            
             @SalaryTotalAmt,            
             @SalaryAverageAmt ,            
             @Salary12MonthPreDeathAmt,            
             @EmployerName,            
             @EmployerPAYENum,            
             @EmployerContactPersonTitleCode,            
             @EmployerContactPersonInitials,            
             @EmployerContactPersonFirstName,            
             @EmployerContactPersonSurname,            
             @EmployerTelAreaCode,            
             @EmployerTelNumber,            
             @EmployerPhysicalAddress1,            
             @EmployerPhysicalAddress2,            
             @EmployerPhysicalAddress3,            
             @EmployerPhysicalAddress4,            
             @EmployerPhysicalPostCode,            
             @EmployerPostalAddress1,            
             @EmployerPostalAddress2,            
             @EmployerPostalAddress3,            
             @EmployerPostalAddress4,            
             @EmployerPostalPostCode,            
             @TaxFreePortionAmt,            
             @GrossPayablePAYEAmt,            
             @DeductionFrequency,            
             @AllowedContributionAmt,            
             @Comment,            
             @DirectiveStatusCode,           
             @PaperRespInd,            
             @GrossTotalBenefitAmt,            
             @DivorceSpouseAmt,            
             @BenefitCalcInd,            
             @FundAnnuityInd,            
             @FundAnnuityAmt,            
             @AnnuityRegInsurer,            
             @AnnuityPolicyNum,            
             @FundPayAnnuityInd,            
             @FundRemainAnnuityAmt,          
			-- @UnclaimedBenefPrevTaxedAmt,  -- NBM 20090822          
			 @EmailAddressAdministrator,          
			 @TaxedTransferNonMemberSpouse,      
			 @ParticipatingEmployerName,      
			 @FSBNumber,
			 @AIPFDeemedContrib,
			 @NonResidentInd,
			 --@ServRendOutsideRepInd,
			 --@ServRendContribFundMonth,
			 --@ServRendOutsideRepMonth,
			 @MemFundContr,
			 @MemFundContrAfter20160301,
			 @AnnuityPolicyNum1,
			 @AnnuityPolicyNum2,
			 @AnnuityPolicyNum3,
			 @FundAnnuityAmt1,
			 @FundAnnuityAmt2,
			 @FundAnnuityAmt3,
			 @AnnuityRegInsurer1,
			 @AnnuityRegInsurer2,
			 @AnnuityRegInsurer3,
			 @AnnuityRegInsurerEmail1,
			 @AnnuityRegInsurerEmail2,
			 @AnnuityRegInsurerEmail3,
			 @AnnuityRegInsurerEmail4,
			 @AnnuityRegInsurerFSBNum1,
			 @AnnuityRegInsurerFSBNum2,
			 @AnnuityRegInsurerFSBNum3,
			 @AnnuityRegInsurerFSBNum4,
			 @AnnuityRegInsurerTelNumber1,
			 @AnnuityRegInsurerTelNumber2,
			 @AnnuityRegInsurerTelNumber3,
			 @AnnuityRegInsurerTelNumber4,
			 @AnnuityRegInsurerCellNumber1,
			 @AnnuityRegInsurerCellNumber2,
			 @AnnuityRegInsurerCellNumber3,
			 @AnnuityRegInsurerCellNumber4,
			 @SpecialCondInstr,
			 @DeclarationInd,
			 @FormID)  
			          
      select @FORMADKey = scope_identity()            
            
            
     end try
	 begin catch
	    Select @Err = 'Error on FORMAD insert'
	    goto ErrHandler
	 end catch 
	 
	        
     begin try 
	            
      -- update add key to DirectiveLookup            
      update DirectiveLookup             
         set DirectiveKey =@FORMADKey            
       where DirectiveRequestID = @DirectiveID            
         and DirectiveTypeCode = @DirectiveTypeCode            
            
   
	 end try
	 begin catch
	    Select @Err = 'Error on FORMAD Directive Lookup update with key'
	    goto ErrHandler
	 end catch          
            
      -- insert status history record           
      begin try 
	        
      insert DirectiveStatusHistory            
              (DirectiveRequestID,            
               DirectiveTypeCode,            
               DirectiveKey,            
               DirectiveSystemStatus,            
               StatusHistoryDt)            
      values ( @DirectiveID,            
               @DirectiveTypeCode,            
               @FORMADKey,            
               @SystemStatus,            
               getdate())            
        
     end try
	 begin catch
	    Select @Err = 'Error on FORMAD Directive Status History Insert'
	    goto ErrHandler
	 end catch 
	          
    end            
    else            
    begin            
            
      -- get old system status 
	 begin try            
            
      select @OldSystemStatus = DirectiveSystemStatus            
        from TPDirectiveAD  with (nolock) --VB          
       where FORMADID = @FORMADID            

	 end try
	 begin catch
	    Select @Err = 'Error on FORMAD getting old system status'
	    goto ErrHandler
	 end catch              
            
      -- update FORMAD record            
     begin try 
	       
      update TPDirectiveAD            
         set DirectiveGroupID = @DirectiveGroupID,            
             TaxUserID = @TaxUserID,  -- keeps track of last            
             TaxPayerID = @TaxPayerID,            
             DirectiveSystemStatus = @SystemStatus,            
             CompleteDt = convert(datetime, @CompleteDt),            
             SubmitDt = convert(datetime, @SubmitDt),            
             PeriodYear = @PeriodYear,            
             PeriodNum = @PeriodNum,            
             DirectiveApplicationID = @DirectiveApplicationID,            
             YearOfAssesmentDt = convert(datetime, @YearOfAssesmentDt),            
             IncomeTaxReferenceNum = @IncomeTaxReferenceNum,            
             NoIncomeTaxCode = @NoIncomeTaxCode,            
             NoIncomeTaxText = @NoIncomeTaxText,            
             AnnualIncomeAmt = round(@AnnualIncomeAmt,0),            
             EmployeeNum = @EmployeeNum,            
             PersonTitleCode = @PersonTitleCode,            
             Initials = @Initials,            
             Surname = @Surname,            
             FirstName = @FirstName,            
             DateOfBirth = convert(datetime, @DateOfBirth),            
             IDNumber = @IDNumber,            
             IDNumberOther = @IDNumberOther,            
             PhysicalAddress1 = @PhysicalAddress1,            
             PhysicalAddress2 = @PhysicalAddress2,            
             PhysicalAddress3 = @PhysicalAddress3,            
             PhysicalAddress4 = @PhysicalAddress4,            
             PhysicalPostCode = @PhysicalPostCode,            
             PostalAddress1 = @PostalAddress1,            
             PostalAddress2 = @PostalAddress2,            
             PostalAddress3 = @PostalAddress3,            
             PostalAddress4 = @PostalAddress4,            
             PostalPostCode = @PostalPostCode,            
             TelAreaCode = @TelAreaCode,            
             TelNumber = @TelNumber,            
             FundName = @FundName,            
             FundNumber = @FundNumber ,            
             FundPAYENum = @FundPAYENum ,             
             FundContactPersonTitleCode = @FundContactPersonTitleCode ,            
             FundContactPersonInitials = @FundContactPersonInitials ,            
             FundContactPersonFirstName = @FundContactPersonFirstName ,            
             FundContactPersonSurname = @FundContactPersonSurname,            
             FundTelAreaCode = @FundTelAreaCode,            
             FundTelNumber = @FundTelNumber,            
             FundPostalAddress1 = @FundPostalAddress1,            
             FundPostalAddress2 = @FundPostalAddress2,            
             FundPostalAddress3 = @FundPostalAddress3,            
             FundPostalAddress4 = @FundPostalAddress4,            
             FundPostalPostCode = @FundPostalPostCode,            
             MembershipNum = @MembershipNum,            
             MembershipDt = convert(datetime, @MembershipDt),            
             FundTypeCode = @FundTypeCode,            
             FundCreateReasonCode = @FundCreateReasonCode,            
             GrossLumpAmt = @GrossLumpAmt,            
             ReasonForDirectiveCode = @ReasonForDirectiveCode,            
             AccrualDt = convert(datetime, @AccrualDt),            
             TotalContributionAmt = @TotalContributionAmt ,            
             --ExcessContributionAmt = @ExcessContributionAmt ,            
             CalculationMethodCode = @CalculationMethodCode,            
             CalculationYearFromDt = convert(datetime, @CalculationYearFromDt),            
             CalculationYearToDt = convert(datetime, @CalculationYearToDt),            
             CalculationCompletedYears = @CalculationCompletedYears,            
             SalariesEnteredNum = @SalariesEnteredNum ,            
             Salary1StartDt = convert(datetime, @Salary1StartDt),            
             Salary1EndDt = convert(datetime, @Salary1EndDt),            
             Salary1Amt = @Salary1Amt,            
             Salary2StartDt = convert(datetime, @Salary2StartDt),            
             Salary2EndDt = convert(datetime, @Salary2EndDt),            
             Salary2Amt = @Salary2Amt,            
             Salary3StartDt = convert(datetime, @Salary3StartDt),            
             Salary3EndDt = convert(datetime, @Salary3EndDt),            
             Salary3Amt = @Salary3Amt ,            
             Salary4StartDt = convert(datetime, @Salary4StartDt),            
             Salary4EndDt = convert(datetime, @Salary4EndDt),            
             Salary4Amt = @Salary4Amt,            
             Salary5StartDt = convert(datetime, @Salary5StartDt),            
             Salary5EndDt = convert(datetime, @Salary5EndDt),            
             Salary5Amt = @Salary5Amt,            
             SalaryTotalAmt = @SalaryTotalAmt,            
             SalaryAverageAmt = @SalaryAverageAmt ,            
             Salary12MonthPreDeathAmt = @Salary12MonthPreDeathAmt,            
             EmployerName = @EmployerName,            
             EmployerPAYENum = @EmployerPAYENum,            
             EmployerContactPersonTitleCode = @EmployerContactPersonTitleCode,            
             EmployerContactPersonInitials = @EmployerContactPersonInitials,            
             EmployerContactPersonFirstName = @EmployerContactPersonFirstName,            
             EmployerContactPersonSurname = @EmployerContactPersonSurname,            
             EmployerTelAreaCode = @EmployerTelAreaCode,            
             EmployerTelNumber = @EmployerTelNumber,            
             EmployerPhysicalAddress1 = @EmployerPhysicalAddress1,            
             EmployerPhysicalAddress2 = @EmployerPhysicalAddress2,            
             EmployerPhysicalAddress3 = @EmployerPhysicalAddress3,            
             EmployerPhysicalAddress4 = @EmployerPhysicalAddress4,            
             EmployerPhysicalPostCode = @EmployerPhysicalPostCode,            
             EmployerPostalAddress1 = @EmployerPostalAddress1,            
             EmployerPostalAddress2 = @EmployerPostalAddress2,            
             EmployerPostalAddress3 = @EmployerPostalAddress3,            
             EmployerPostalAddress4 = @EmployerPostalAddress4,            
             EmployerPostalPostCode = @EmployerPostalPostCode,            
             TaxFreePortionAmt = @TaxFreePortionAmt,            
             GrossPayablePAYEAmt= @GrossPayablePAYEAmt,            
             DeductionFrequency = @DeductionFrequency,            
             AllowedContributionAmt = @AllowedContributionAmt,                         
             Comment = @Comment,            
             DirectiveStatusCode = @DirectiveStatusCode,            
             GrossTotalBenefitAmt = @GrossTotalBenefitAmt,            
             DivorceSpouseAmt = @DivorceSpouseAmt,            
             BenefitCalcInd = @BenefitCalcInd,            
             FundAnnuityInd = @FundAnnuityInd,            
             FundAnnuityAmt = @FundAnnuityAmt,            
             AnnuityRegInsurer = @AnnuityRegInsurer,            
             AnnuityPolicyNum = @AnnuityPolicyNum,            
             FundPayAnnuityInd = @FundPayAnnuityInd,            
             FundRemainAnnuityAmt = @FundRemainAnnuityAmt,          
			-- UnclaimedBenefPrevTaxedAmt = @UnclaimedBenefPrevTaxedAmt,  -- NBM 20090822          
			 EmailAddressAdministrator = @EmailAddressAdministrator,          
			 TaxedTransferNonMemberSpouse = @TaxedTransferNonMemberSpouse,      
			 ParticipatingEmployerName = @ParticipatingEmployerName,      
			 FSBNumber = @FSBNumber ,
			 AIPFDeemedContrib = @AIPFDeemedContrib,
			NonResidentInd = @NonResidentInd,
			--ServRendOutsideRepInd = @ServRendOutsideRepInd,
			--ServRendContribFundMonth = @ServRendContribFundMonth,
			--ServRendOutsideRepMonth = @ServRendOutsideRepMonth,
			MemFundContr =  @MemFundContr,
			MemFundContrAfter20160301 =  @MemFundContrAfter20160301,
			AnnuityPolicyNum1 =  @AnnuityPolicyNum1,
			AnnuityPolicyNum2 = @AnnuityPolicyNum2,
			AnnuityPolicyNum3 = @AnnuityPolicyNum3,
			FundAnnuityAmt1 = @FundAnnuityAmt1,
			FundAnnuityAmt2 = @FundAnnuityAmt2,
			FundAnnuityAmt3 = @FundAnnuityAmt3,
			AnnuityRegInsurer1 = @AnnuityRegInsurer1,
			AnnuityRegInsurer2 = @AnnuityRegInsurer2,
			AnnuityRegInsurer3 = @AnnuityRegInsurer3,
			AnnuityRegInsurerEmail1 = @AnnuityRegInsurerEmail1,
			AnnuityRegInsurerEmail2 = @AnnuityRegInsurerEmail2,
			AnnuityRegInsurerEmail3 = @AnnuityRegInsurerEmail3,
			AnnuityRegInsurerEmail4 = @AnnuityRegInsurerEmail4,
			AnnuityRegInsurerFSBNum1 = @AnnuityRegInsurerFSBNum1,
			AnnuityRegInsurerFSBNum2 = @AnnuityRegInsurerFSBNum2,
			AnnuityRegInsurerFSBNum3 = @AnnuityRegInsurerFSBNum3,
			AnnuityRegInsurerFSBNum4 = @AnnuityRegInsurerFSBNum4,
			AnnuityRegInsurerTelNumber1 = @AnnuityRegInsurerTelNumber1,
			AnnuityRegInsurerTelNumber2 = @AnnuityRegInsurerTelNumber2,
			AnnuityRegInsurerTelNumber3 = @AnnuityRegInsurerTelNumber3,
			AnnuityRegInsurerTelNumber4 = @AnnuityRegInsurerTelNumber4,
			AnnuityRegInsurerCellNumber1 = @AnnuityRegInsurerCellNumber1,
			AnnuityRegInsurerCellNumber2 = @AnnuityRegInsurerCellNumber2,
			AnnuityRegInsurerCellNumber3 = @AnnuityRegInsurerCellNumber3,
			AnnuityRegInsurerCellNumber4 = @AnnuityRegInsurerCellNumber4,
			SpecialCondInstr = @SpecialCondInstr,
			DeclarationInd = @DeclarationInd
			    
       where FORMADID = @FORMADID            
            

	  end try
	  begin catch
	    Select @Err = 'Error on FORMAD update'
	    goto ErrHandler
	  end catch          
            
      -- track change in system status            
      if @OldSystemStatus <> @SystemStatus begin 
	  
	  begin try           
            
         insert DirectiveStatusHistory            
                 (DirectiveRequestID,            
                  DirectiveTypeCode,            
                  DirectiveKey,            
                  DirectiveSystemStatus,            
                  StatusHistoryDt)            
         values ( @DirectiveID,            
                  @DirectiveTypeCode,            
                  @FORMADKey,            
                  @SystemStatus,            
                  getdate())            
            
	  end try
	  begin catch
	    Select @Err = 'Error on FORMAD System status insert on update'
	    goto ErrHandler
	  end catch     
		           
       end            
            
    end            
            
            
-- Set taxuser on Lookup            
     begin try
	        
      update DirectiveLookup             
         set TaxUserID =@TaxUserID            
       where DirectiveRequestID = @DirectiveID            
         and DirectiveTypeCode = @DirectiveTypeCode            
            
  
	end try
	begin catch
	  Select @Err = 'Error on TaxUserID update '
	  goto ErrHandler
	end catch          
            
  --**********************************************************            
  commit transaction            
            
            
  if @BusRuleCheck <> 0 begin            
	raiserror (100000, 16, 1, @BusRuleErr)            
    return 1            
  end            
            
  select @DirectiveID, @FORMADKey            
  return            
            
  -- Error handler            
  ErrHandler:            
    rollback            
    raiserror (100000, 16, 1, @Err)            
    Return @Err --1            
end            