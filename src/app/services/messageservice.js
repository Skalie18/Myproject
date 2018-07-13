(function(){
    // 'use strict';
    angular.module('cbcproject').factory('messageservices', messageservices);

    function messageservices(){
  
        return{
            negativeNumberMsg:negativeNumberMsg,
            alphaNumericMsg:alphaNumericMsg,
            maskPITRIncomeSourceCodeMsg:maskPITRIncomeSourceCodeMsg,
            maskPITRDeductionSourceCodeMsg:maskPITRDeductionSourceCodeMsg,
            crossReasonCodeCheckMsg:crossReasonCodeCheckMsg,
            crossEmploymentDatesMsg:crossEmploymentDatesMsg,
            DisabilitySeverity_Date_CheckMsg:DisabilitySeverity_Date_CheckMsg,
            crossPayPeriodsAssessmentYearMsg:crossPayPeriodsAssessmentYearMsg,
            dateRangeMsg:dateRangeMsg,
            residenceCeaseMsg:residenceCeaseMsg,
            crossUnemployedContainerChange:crossUnemployedContainerChange,
            unemploymentUnbrokenPeriodsMsg:unemploymentUnbrokenPeriodsMsg,
            autoCalcAnnualExclusionsMessage:autoCalcAnnualExclusionsMessage,
            crossLockNonPrimaryResFieldsMessage:crossLockNonPrimaryResFieldsMessage,
            crossCheckProceedsAmountR2mLessMessage:crossCheckProceedsAmountR2mLessMessage,
            crossTravelLogBookMessage:crossTravelLogBookMessage,
            exitFieldMessage:exitFieldMessage,
            repeatRetirementMessage:repeatRetirementMessage,
            mask_Modulus10CheckPAYENo:mask_Modulus10CheckPAYENo,
            mask_Modulus10CheckTrustNo:mask_Modulus10CheckTrustNo,
            localRemunerationSourceCode:localRemunerationSourceCode,
            localAnnuitiesSourceCode:localAnnuitiesSourceCode,
            localCapitalSourceCode:localCapitalSourceCode,
            localBusinessTradingSourceCode:localBusinessTradingSourceCode,
            localFarmingOperationsSourceCode:localFarmingOperationsSourceCode,
            localOtherIncomeSourceCode:localOtherIncomeSourceCode,
            foreignCapitalGainLossSourceCode:foreignCapitalGainLossSourceCode,
            CrossIncomeProtectionMsg:CrossIncomeProtectionMsg,
            Cross_MedicalSchemePerMonthMsg:Cross_MedicalSchemePerMonthMsg,
            Allow_MainMember_SelectMsg:Allow_MainMember_SelectMsg,
            percentageGreaterThan100Percent:percentageGreaterThan100Percent,
            donationRef_Validation2015:donationRef_Validation2015,
            donationRef_Validation2016:donationRef_Validation2016,
            notAllDigitsProvidedMessage:notAllDigitsProvidedMessage,
            invalidSourceCodeEnteredMessage:invalidSourceCodeEnteredMessage,
            invalidProfitLossPercentageMessage:invalidProfitLossPercentageMessage,
            displayFringeBenefitMoreThanContributionMessage:displayFringeBenefitMoreThanContributionMessage,
            disabilityDate_ValidationMessage:disabilityDate_ValidationMessage,
            check_UniqueIdentifierMessage:check_UniqueIdentifierMessage,
            check_LessAmountDeductedMessage:check_LessAmountDeductedMessage,
            crossAllowance_DeductionMessage:crossAllowance_DeductionMessage,
            cross_CheckMessage:cross_CheckMessage,
            cross_RuleMessage:cross_RuleMessage,
            check_LessReoupmentsMessage:check_LessReoupmentsMessage,
            check_AllowableDeductionsMessage:check_AllowableDeductionsMessage,
            cellNumberMessage:cellNumberMessage,
            passportNumberMessage:passportNumberMessage,
            declarationDateMessage:declarationDateMessage,
            idNumberMessage:idNumberMessage,
            postalCodeMessage:postalCodeMessage,
            emailAddressTelephoneMsg:emailAddressTelephoneMsg,
            emailAddressMsg:emailAddressMsg,
            taxRefNumberMessage:taxRefNumberMessage,
            ReportingPeriodMessage:ReportingPeriodMessage,
            deletingRecordMessage:deletingRecordMessage,
            numericCheckTelNumberMessage:numericCheckTelNumberMessage,
            giinNumberCheckMsg:giinNumberCheckMsg

        }
  

        // Private functions
        function errorAlert(massage){
            swal('Error',massage);
        }

        function warningAlert(massage){
            swal('Warning',massage);
        }

        function infoAlert(massage){
            swal('Information',massage);
        }

        function  negativeNumberMsg (fieldName, fieldValue) {
            if (angular.isNumber(fieldValue)) {
                if (fieldValue < 0) {
                    swal(fieldName + " with value " + fieldValue + " may not be less than 0");
                }
            }
        }

        function warningWithComformtion(resolve, reject){

            return swal({
                title: "Warning",
                text: "By selecting the Deletion option your record will be deleted, do you want to continue? Yes or No?",
                showCancelButton: true,
                confirmButtonText: 'Yes',
                cancelButtonText:'No',
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                allowOutsideClick: false
            })

        }

        function emailAddressTelephoneMsg (){
            var message = "Please update your email address and telephone numbers and confirm that it is correct before submitting your return.";
            infoAlert(message);
        }
        
        function emailAddressMsg (fieldname){
            var message = "The "+ fieldname +" you have entered does not seem to be valid. Please ensure that it is correct. <br><br><b>HINTS:</b><br>- Ensure that it has an @ sign and a .domain at the end.";
         errorAlert(message);
            
        }
        
         function alphaNumericMsg (fieldName){
            var message = "The " + fieldName + " you have entered does not seem to be valid. Please ensure that it is correct.<br><br><b>HINTS:</b><br>- Only alphanumeric digits may be used.";
            errorAlert(message);
        }
        
        function maskPITRIncomeSourceCodeMsg (yearOfAssessment) {
            var message = "The Source Code you have entered does not seem to be valid. Please ensure that it is correct.<br><br>HINTS:<br>";
            message += "Only numeric digits may be used.<br>";
            message += "Ensure that the source code falls between the following ranges:<br>";
    
            if (Number(yearOfAssessment) >= 2017) {
                message += "3601 – 3621, 3651 – 3670, 3701 - 3721, 3751 – 3771, 3801 – 3810, 3813, 3815, 3816, 3817, 3820,3821,3822, 3825, 3828, 3851 – 3860, 3863, 3865, 3866, 3867, 3870, 3871, 3872, 3875, 3878, 3901 – 3909, 3915, 3920 – 3923, 3951 – 3957";
            }
    
            if (Number(yearOfAssessment) >= 2014) {
                message += "3601 – 3617, 3651 – 3667, 3701 - 3718, 3751 – 3768, 3801 – 3810, 3813, 3815, 3816, 3820, 3821, 3822, 3851 – 3860, 3863, 3865, 3866, 3870, 3871, 3872, 3901 – 3909, 3915, 3920 – 3922, 3951 – 3957";
            }
    
            if (Number(yearOfAssessment) < 2014) {
                message += "3601 – 3617, 3651 – 3667, 3701 - 3718, 3751 – 3768, 3801 – 3810, 3813, 3815, 3851 – 3860, 3863, 3865, 3901 – 3909, 3915, 3920 – 3922, 3951 – 3957";
            }
    
            errorAlert(message);
        }
    
        function maskPITRDeductionSourceCodeMsg (yearOfAssessment) {
            var message = "The Source Code you have entered does not seem to be valid. Please ensure that it is correct.<br><br>HINTS:<br>";
            message += "Only numeric digits may be used.<br>";
            message += "Ensure that the source code falls between the following ranges:<br>";
    
            if (Number(yearOfAssessment) >= 2017) {
                message += "4001, 4003 – 4006, 4024 – 4026, 4030, 4472 – 4476, 4485 – 4487, 4493, 4582, 4583";
            }
    
            if (Number(yearOfAssessment) >= 2015) {
                message += "4001 – 4007, 4018, 4024 – 4026, 4030, 4472 – 4476, 4485 – 4487, 4493";
            }
    
            if (Number(yearOfAssessment) < 2014) {
                message += "4001 – 4007, 4018, 4024 – 4026, 4030, 4472 – 4476, 4485 – 4487, 4493";
            }
    
            errorAlert(message);
        }
    
         function crossReasonCodeCheckMsg () {
            var message = "The Reason code you have entered does not seem to be valid. Please ensure that is is correct.<br><br>HINTS:<br>Only numeric digits may be used.<br>Ensure the reason code falls between 01 and 09";
            errorAlert(message);
        }
    
    
         function crossEmploymentDatesMsg (yearOfAssessment, field) {
            var message = "The " + field + " you have entered does not seem to be valid. Please ensure that it is correct.";
            message += "<br><br>";
            message += "<br>HINTS:";
            message += "Only numeric digits may be used.<br>";
            message += "The date may not be in the future.<br>";
            message += "This date cannot be earlier that the 1st of March " + (yearOfAssessment-1);
            message += ".<br>The date cannot be later than the last day of February " + yearOfAssessment;
    
            errorAlert(message);
    
        }
    
         function DisabilitySeverity_Date_CheckMsg (yearOfAssessment, field,noOfYrs) {
            var message = "The " + field + " you have entered does not seem to be valid. Please ensure that it is correct.";
            message += "<br><br>";
            message += "<br>HINTS:<br>";
            message += "The date must fall within the current year of assessment<br>";
            message += "or in one of the previous " + noOfYrs + " years of assessments<br>";
            message += "The date must not be earlier that the 1st of March " + ((yearOfAssessment - 1) - noOfYrs);
            message += ".<br>The date may not be in the future";
    
            errorAlert(message);
    
        }
    
         function crossPayPeriodsAssessmentYearMsg () {
    
            var message = "The period Employed to you have entered does not seem to be valid. Please ensure that it is correct,";
            message += "<br><br>";
            message += "<br>HINTS:<br>";
            message += "Only numeric digits may be used.<br>";
            message += "The value cannot be greater than the Periods in Year of Assessment value.<br>";
            message += "The number specified should include decimals.<br>";
            message += "The value should be greater than 0.";
    
            errorAlert(message);
    
    
        }
    
        function dateRangeMsg (fieldName) {
            errorAlert( "The " + fieldName + " you have entered does not seem to be valid.  Please ensure that it is correct.<br><br>HINTS:<br>The “Unemployed to” date may not be prior to the “Unemployed from” Date");
        }
    
         function residenceCeaseMsg  () {
            errorAlert( "The Date you have entered does not seem to be valid.  Please ensure that it is correct.<br><br>HINTS:<br>The date must fall within the Year of Assessment of this return");
        }
    
         function crossUnemployedContainerChange () {
            errorAlert( 'Please select the type of income accrued / received by answering the applicable question in the wizard.');
        }
    
        function unemploymentUnbrokenPeriodsMsg (fieldName) {
            errorAlert( "The " + fieldName + " you have entered does not seem to be valid.  Please ensure that it is correct.<br><br>HINTS:<br>- The period of unemployment must fall within the current tax period");
        }
    

        function autoCalcAnnualExclusionsMessage () {
            warningAlert("Please ensure that you have not included the annual inclusion rate as the applicable inclusion rate will be applied by SARS.");
        }
    
       function crossLockNonPrimaryResFieldsMessage () {
        swal("Info", "As the primary residence is held jointly it will be considered to be held in equal shares.");
        }
    
        function crossCheckProceedsAmountR2mLessMessage  () {
            swal("Warning", "As the proceeds on the disposal of the primary residence does not exceed R2 million and it resulted in a capital gain, the transaction is disregarded.");
        }
    
        function crossTravelLogBookMessage () {
    
            errorAlert( 'The claim can only be calculated based on a logbook');
    
        }
    
        function exitFieldMessage() {
            swal("INFORMATION MESSAGE", 'Please note that you must complete all Fringe Benefit fields with a value and the total should match the amount from your employer.');
        }
    
        function repeatRetirementMessage () {
            errorAlert("The number you have entered does not seem to be valid. Please ensure that it is correct. <br/><br/><b>HINTS:</b><br/><br/>- Only numeric digits may be used <br/>-The number must be greater than zero.");
        }
    
        function mask_Modulus10CheckPAYENo (fieldName) {
            errorAlert("The " + fieldName + " you have entered does not seem to be valid. Please ensure that it is correct.<br>HINTS:<br>Only numeric digits may be used.");
        }
    
        function mask_Modulus10CheckTrustNo (fieldName) {
            errorAlert("The " + fieldName + " you have entered does not seem to be valid. Please ensure that it is correct.<br>HINTS:<br>Only numeric digits may be used.");
        }
    
        function localRemunerationSourceCode (fieldName) {
            errorAlert(
                "The " + fieldName +
                " you have entered does not seem to be valid. Please ensure that it is correct." +
                "<br>HINTS:<br>Only numeric digits may be used." +
                "<br><br>Ensure that the source code falls in the following ranges:" +
                "<br>3601 – 3606" +
                "<br>3616, 3617 and 3667"
                );
        }
    
        function localAnnuitiesSourceCode (fieldName) {
            errorAlert(
                "The " + fieldName +
                " you have entered does not seem to be valid. Please ensure that it is correct." +
                "<br>HINTS:<br>Only numeric digits may be used." +
                "<br><br>Ensure that the source code falls in the following ranges:" +
                "<br>3610 or 3611"
                );
        }
    
        function localCapitalSourceCode  (fieldName) {
            errorAlert(
                "The " + fieldName +
                " you have entered does not seem to be valid. Please ensure that it is correct." +
                "<br>HINTS:<br>Only numeric digits may be used." +
                "<br><br>Ensure that the source code falls in the following ranges:" +
                "<br>4250 or 4250"
                );
        }
    
        function localBusinessTradingSourceCode (fieldName) {
            errorAlert(
                "The " + fieldName +
                " you have entered does not seem to be valid. Please ensure that it is correct." +
                "<br>HINTS:<br>Only numeric digits may be used." +
                "<br><br>Ensure that a profit source code is entered and falls within the respective ranges (refer to guide)."
                );
        }
    
        function localFarmingOperationsSourceCode (fieldName) {
            errorAlert(
                "The " + fieldName +
                " you have entered does not seem to be valid. Please ensure that it is correct." +
                "<br>HINTS:<br>Only numeric digits may be used." +
                "<br><br>Ensure that a profit source code is entered and falls within the respective ranges (refer to guide)."
                );
        }
    
        function localOtherIncomeSourceCode (fieldName) {
            errorAlert(
                "The " + fieldName +
                " you have entered does not seem to be valid. Please ensure that it is correct." +
                "<br>HINTS:<br>Only numeric digits may be used." +
                "<br><br>Ensure that the source code falls in the following ranges:" +
                "<br>4212 or 4214"
                );
        }
    
        function foreignCapitalGainLossSourceCode (fieldName) {
            errorAlert(
                "The " + fieldName +
                " you have entered does not seem to be valid. Please ensure that it is correct." +
                "<br>HINTS:<br>Only numeric digits may be used." +
                "<br><br>Ensure that the source code falls in the following ranges:" +
                "<br>4252 or 4253"
                );
        }
    
        function CrossIncomeProtectionMsg () {
            errorAlert("Please ensure that the amount entered is the total amount in respect of all policies that you yourself will derive a taxable benefit from, including the amount represented on your employees’ tax certificate.");
        }
    
        function Cross_MedicalSchemePerMonthMsg () {
            errorAlert("The Number of Dependants you have entered does not seem to be valid. Please ensure that it is correct.<br/><br/>HINTS: <br/>- Only numeric digits may be used At least one value must be greater than 0");
        }
    
        function Allow_MainMember_SelectMsg () {
            errorAlert("The option “Yourself ” may only be selected once");
        }
    
        function percentageGreaterThan100Percent (fieldName) {
    
            var message = "The " + fieldName + " you have entered does not seem to be valid. Please ensure that it is correct,";
            message += "<br><br>";
            message += "<br>HINTS:<br>";
            message += "Only numeric digits may be used.<br>";
            message += "Ensure that the percentage is not greater than 100%.<br>";
            message += "Amount specified should include decimals.";
    
            errorAlert(message);
        }
         
         function donationRef_Validation2015  (fieldName) {
            swal("WARNING", "The "+fieldName+" you have entered does not seem to be valid. Please ensure that it is correct.<br/><br/><b>HINTS:</b><br/>- The field length must be 9 or 10 or 13");
         }
         
         function donationRef_Validation2016 (fieldName) {
            swal("WARNING", "The "+fieldName+" you have entered does not seem to be valid. Please ensure that it is correct.");
         }
         
         function notAllDigitsProvidedMessage(fieldName, numberOfDigits) {
    
            var message = "The " + fieldName + " you have entered does not seem to be valid. Please ensure that it is correct,";
            message += "<br><br>";
            message += "<br>HINTS:<br>";
            message += "Only numeric digits may be used.<br>";
            message += "All " + numberOfDigits + "digits must be completed.";
            errorAlert(message);
         }
    
         function invalidSourceCodeEnteredMessage() {
    
            var message = "The source code you have entered does not seem to be valid. Please ensure that it is correct.";
            message += "<br>";
            message += "<br>HINTS:<br>";
            message += "Only numeric digits may be used.<br>";
            message += "Ensure that the source code falls within the respective ranges (refer to Source Code Booklet available on the SARS website)";
            
            errorAlert(message);
        }
    
         function invalidProfitLossPercentageMessage () {
    
            var message = "The profit/loss percentage you have entered does not seem to be valid. Please ensure that it is correct.";
            message += "<br>";
            message += "<br>HINTS:<br>";
            message += "Only numeric digits may be used.<br>";
            message += "Ensure that the percentage is not less than 0.01 and not greater or equals to 100%.<br>";
            message += "Amount specified should include decimal values.";
            
            errorAlert(message);
        }
    
         function displayFringeBenefitMoreThanContributionMessage () {
    
            var message = "Amount completed doesn’t seem to be valid. Please correct.";
            message += "<br>";
            message += "<br>HINTS:<br>";
            message += "This amount cannot be more than the contributions made by the partnership.";
            
            errorAlert(message);
        }
    
         function disabilityDate_ValidationMessage (fieldname) {
             var message = " The " + fieldname + " you have entered does not seem to be valid Please ensure that it is correct<br> Hints:<br> The date cannot be if future and may not be greater than year of assessment";
             errorAlert(message);
         }
    
         function check_UniqueIdentifierMessage (fieldname) {
             var message = " The " + fieldname + " you have entered does not seem to be valid Please ensure that it is correct<br> Hints:<br> Only numeric digits may be used. <br>Only numeric digits may be used";
             errorAlert(message);
         }
    
         function check_LessAmountDeductedMessage (fieldname) {
             var message = " The " + fieldname + " (Less: Amount deducted) > cannot be larger than the <fieldname (SUB-TOTAL (ii))> above.";
             errorAlert(message);
         }
    
         function crossAllowance_DeductionMessage  (fieldname1, fieldname2) {
             var message = " The value of " + fieldname1 + " must be the same as the value of " + fieldname2;
             errorAlert(message);
         }
    
         function cross_CheckMessage (fieldname1) {
             var message = "The " + fieldname1 + " be larger than the sum of the Balance b/f Previous Year – Subsequent Year and the Deductions i.r.o. Purchases Current Year – Subsequent Year";
             errorAlert(message);
         }
    
         function cross_RuleMessage (fieldname1, fieldname2) {
             var message = " The value of " + fieldname1 + " cannot be larger than " + fieldname2;
             errorAlert(message);
         }
    
         function check_LessReoupmentsMessage () {
             var message = "This field cannot be greater than the “Balance b/f from Previous Year: field.";
             errorAlert(message);
         }
    
        function check_AllowableDeductionsMessage (fieldName1, fieldName2) {
             var message = "The " + fieldName1 + " cannot be greater than the " + fieldName2;
             errorAlert(message);
         }

        function cellNumberMessage (){
            errorAlert("The Cell No. you have entered does not seem to be valid. Please ensure that it is correct.<br/>" +
            "HINTS:<br/><br/>- Only numeric digits may be used.<br/>" +
            "At least 10 digits must be completed.");
        } 

        function passportNumberMessage(){
            var errorMessage  = "The Passport No. you have entered does not seem to be valid. Please ensure that it is correct.<br><br><b>HINTS:</b><br>- No spaces allowed.<br>-  Only alphanumeric digits may be used.<br>-  At least 8 digits must be completed.";
            errorAlert(message);
        }

        function declarationDateMessage (){
            var errorMessage = "The Declaration Date you have entered does not seem to be valid. Please ensure that it is correct.<br/><br/>HINTS:<br/><br/>- Only numeric digits may be used.<br/>- The date may not be in the future.";
            errorAlert(errorMessage);
        }

        function idNumberMessage (){
            var errorMessage = "The ID No. you have entered does not seem to be valid. Please ensure that it is correct.<br><br><b>HINTS:</b><br>- Only numeric digits may be used.";
            errorAlert(errorMessage);
        }


        function postalCodeMessage (fieldName){
            var errorMessage = "The " + fieldName + " you have entered does not seem to be valid. Please ensure that it is correct.<br> <b>HINTS:</b><br>Only alphanumeric digits may be used.<br> At least 4 digits must be completed.";
            errorAlert(errorMessage);
        }

        function taxRefNumberMessage(){
            var errorMessage = "The Tax Ref No. you have entered does not seem to be valid. Please ensure that it is correct.<br/>HINTS:<br/>Only numeric digits may be used.";
            errorAlert(errorMessage);

        }
        function ReportingPeriodMessage(){
            var errorMessage = "The Reporting Period (CCYYMMDD) you have entered does not seem to be valid. Please ensure that it is correct.<br/>HINTS:<br/>  Format:CCYYMMDD. <br/> Only numeric digits may be used.";
            errorAlert(errorMessage);

        }

        function deletingRecordMessage (){
            return warningWithComformtion();
        }

        function numericCheckTelNumberMessage(fieldID){
            var errorMessage = "The " + fieldID+ " you have entered does not seem to be valid. Please ensure that it is correct.<br/>HINTS:<br/> Only numeric digits may be used. 3 or 4 digits must be completed for the area code.<br/>6 or 7 digits must be completed for the telephone number."
            errorAlert(errorMessage);
        }

        function giinNumberCheckMsg() {
            var message = "The GIIN No. you have entered does not seem to be valid. Please ensure that is is correct.<br><br>HINTS:<br>Only alphanumeric digits may be used.<br>Ensure the format is XXXXXX.XXXXX.XX.XXX";
            errorAlert(message);
        }
        
        function giinNumberCheckMsg() {
            var message = "The GIIN No. you have entered does not seem to be valid. Please ensure that is is correct.<br><br>HINTS:<br>Only alphanumeric digits may be used.<br>Ensure the format is XXXXXX.XXXXX.XX.XXX";
            errorAlert(message);
        }
    }
})();