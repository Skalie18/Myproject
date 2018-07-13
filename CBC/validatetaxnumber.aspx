<%@ Page Language="C#" AutoEventWireup="true" CodeFile="validatetaxnumber.aspx.cs" Inherits="validatetaxnumber" %>

<%@ Register assembly="Sars.Systems" namespace="Sars.Systems.Controls" tagprefix="sars" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
<%--    <link href="styles/Site.css" rel="stylesheet" />--%>
 <%--   <link href="styles/survey.css" rel="stylesheet" />--%>
    <script>
        doModulus10 = function (taxNumber) {
            debugger;
            var num = 0;
            var result = false;
            var text = taxNumber;

            try {
                if (text == '') {
                    return true;
                } else {
                    if (text != '0000000000' && text != '9999999999' && text.length == 10) {

                        if (text.substr(0, 1) == 'U' || text.substr(0, 1) == 'L') {
                            text = "4" + text.substr(1);
                        }
                        var num2;

                        for (var i = 1; i <= 9; i++) {
                            var value = text.substr(i - 1, 1);
                            num2 = parseInt(value.charCodeAt()) - parseInt('0'.charCodeAt());
                            if (num2 < 0 || num2 > 9) {
                                break;
                            }
                            if ((i % 2) == 1) {
                                num2 *= 2;
                                num2 =parseInt( num2 / 10) + parseInt( num2 % 10);
                            }
                            num += num2;
                        }
                        num = (10 - num % 10) % 10;
                        num2 = parseInt(text.substr(9, 1).charCodeAt()) - parseInt('0'.charCodeAt());
                        result = (num == num2);
                    }
                }
            } catch (e) {
                result = false;
            }
            return result;
        };

        var isPAYE = function (payeNumber) {
            if (payeNumber == '') {
                return true;
            }
            var result = false;
            var text = payeNumber.toUpperCase();  
            if (text.substr(0, 1) == '7') {
                text = "4" + text.substr(1);
                result = DoModulus10(text);
            }
            return result;
        };

        var IsVAT = function (vatNumber) {
            if (vatNumber == '') {
                return true;
            }
            var text = vatNumber.toUpperCase();
            var isValid =text.substr(0, 1) == '4' && DoModulus10(text);
            return isValid;
        };

        var isValidSAIDNumber = function (idNumber) {
            if (idNumber == '') {
                return true;
            }
            if (idNumber.length != 13) {
                return false;
            }
            var text = idNumber;
            var oddnumberSum = 0;
            var evennumbertext = '';
            var evennumbertextBy2 = 0;


            /*
            * Add all the digits in the odd positions (excluding last digit).  = oddnumberSum
            */
            
            for (var i = 1; i < idNumber.length; i++) {
                
                if (i % 2 != 0) {
                    oddnumberSum += parseInt(text.substr(i - 1, 1));
                }
            }

            /*
            * Move the even positions into a field and multiply the number by 2. = evennumbertextBy2
            */
            for (var i = 1; i <= idNumber.length; i++) {

                if (i % 2 == 0) {
                    evennumbertext += text.substr(i - 1, 1);
                }
            }
            evennumbertextBy2 = parseInt(evennumbertext) * 2;


            /*
            *Add the digits of the result in [2]. = sumofevennumbertextBy2ToString
            */
            var evennumbertextBy2ToString = evennumbertextBy2.toString();
            var sumofevennumbertextBy2ToString = 0;
            for (var i = 0; i < evennumbertextBy2ToString.length; i++) {
                sumofevennumbertextBy2ToString += parseInt(evennumbertextBy2ToString.substr(i, 1));
            }


            /*
             * Add the answer in [3] to the answer in [1] = 
             */

            var varsumofallanswersabove = parseInt(oddnumberSum) + parseInt(sumofevennumbertextBy2ToString);

            //Subtract the second digit of [4](i.e. 3) from 10
            var aa = 10 - parseInt(varsumofallanswersabove.toString().substr(1, 1));

            var lastdigit;

            if (aa.toString().length == 2) {
                lastdigit = aa.toString().substr(1, 1);
            } else {
                lastdigit = aa.toString();
            }
            if (lastdigit == text.substr(12, 1)) {
                return true;
            }
            return false;
        };
        var isEmailAddress = function(email) {
            var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

            if (!filter.test(email)) {
                return false;
            }
            return true;
        };

        var isNumber = function(number) {
            var isnum = /^\d+$/.test(number);
            return isnum;
        };
    </script>
    </head>
<body>
    <form id="form1" runat="server">
    <div style="width: 90% !important; margin-left: auto; margin-right: auto; ">
     <table>
         <tr>
             <td>PAYE:</td>
             <td>
                 <sars:NumberField ID="NumberField2" runat="server" MaxLength="10" NumberTypes="PAYE" Width="100%" />
             </td>
         </tr>
         <tr>
             <td>VAT:</td>
             <td>
                 <sars:NumberField ID="NumberField3" runat="server" MaxLength="10" NumberTypes="VAT" Width="100%" />
             </td>
         </tr>
         <tr>
             <td>SDL</td>
             <td></td>
         </tr>
         <tr>
             <td>ITS</td>
             <td>
                 <sars:NumberField ID="NumberField4" runat="server" MaxLength="10" NumberTypes="PIT" Width="100%" />
             </td>
         </tr>
         <tr>
             <td>Email Address:&nbsp; </td>
             <td>
                 <sars:TextField ID="TextField1" runat="server" TextValidationType="EmailAddress" Width="100%" />
             </td>
         </tr>
         <tr>
             <td>Number</td>
             <td>
                 <sars:NumberField ID="NumberField5" runat="server" MaxLength="20" Width="100%" />
             </td>
         </tr>
         <tr>
             <td>&nbsp;</td>
             <td>
                 <sars:DropDownField ID="DropDownField1" runat="server" Width="250px" AutoPostBack="True" OnSelectedIndexChanged="DropDownField1_SelectedIndexChanged">
                     <Items>
                         <asp:ListItem Text="One" Value="1"></asp:ListItem>
                          <asp:ListItem Text="Two" Value="2"></asp:ListItem>
                          <asp:ListItem Text="Three" Value="3"></asp:ListItem>
                          <asp:ListItem Text="Four" Value="4"></asp:ListItem>
                     </Items>
                 </sars:DropDownField>
             </td>
         </tr>
         <tr>
             <td>&nbsp;</td>
             <td>
                 &nbsp;</td>
         </tr>
         <tr>
             <td>Date Of Birth:</td>
             <td>
        <sars:SarsCalendar ID="SarsCalendar1" runat="server"  MaximumDate="-60M" Width="100%" />
             </td>
         </tr>
         <tr>
             <td>Id Number:</td>
             <td>
                 <sars:NumberField ID="NumberField1" runat="server" NumberTypes="SAIDNUMBER" MaxLength="13" Width="100%"  />
             </td>
         </tr>
         <tr>
             <td>&nbsp;</td>
             <td>
                 &nbsp;</td>
         </tr>
         <tr>
             <td>Date Range:</td>
             <td>
                 <sars:SarsDatePeriod ID="SarsDatePeriod1" runat="server" MaximumDateFrom="0" MaximumDateTo="0" Width="100%" />
             </td>
         </tr>
         <tr>
             <td>Mobile Number:</td>
             <td>
                 <sars:TelephoneField ID="TelephoneField1" runat="server"  TelMaxLength="10"  DialingMaxLength="10"  TelMinLength="7"  />
             </td>
         </tr>
       
         <tr>
             <td>Money:</td>
             <td>
                 <sars:MoneyField ID="MoneyField1" runat="server" Width="100%" />
             </td>
         </tr>
         <tr>
             <td>Select a country:</td>
             <td>
                 <sars:CountryList ID="CountryList1" runat="server" Width="100%" AddJQuery="True">
                 </sars:CountryList>
             </td>
         </tr>
         <tr>
             <td>Multi Text:</td>
             <td>
                
                 <sars:MultiTextField ID="MultiTextField2" runat="server" NumberOfBlocks="10" />
                
             </td>
         </tr>
         <tr>
             <td>Some More:</td>
             <td>
                
                 <sars:MultiTextField ID="MultiTextField3" runat="server" NumberOfBlocks="5" />
                
             </td>
         </tr>
         <tr>
             <td>Get Date:</td>
             <td>
                
                 <sars:SarsCalendar ID="SarsCalendar2" runat="server" MaximumDate="0" MinimumDate="-5" />
                
             </td>
         </tr>
           <tr>
             <td>Some More:</td>
             <td>
                
                 <sars:MultiTextField ID="MultiTextField1" runat="server" NumberOfBlocks="15" />
                
             </td>
         </tr>
         
           <tr>
             <td>Some More:</td>
             <td>
                
                 <sars:MultiTextField ID="MultiTextField4" runat="server" NumberOfBlocks="25" />
                
             </td>
         </tr>
     </table>
    </div>
    <p>
        &nbsp;</p>
    </form>
    </body>
</html>
