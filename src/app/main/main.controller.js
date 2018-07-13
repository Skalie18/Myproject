(function () {
  //'use strict';


  angular.module('cbcproject').controller('MainController', MainController);


  function MainController($scope, $rootScope, $timeout, companyregservice, fieldtypeservevices, messageservices, dateservices, dataService, pagerservice) {
    var vm = $scope;
    var rs = $rootScope;
    vm.jurisdictionsArr = []
    vm.constituentArr = [];
    vm.ResidentCountryCodes = [];

    vm.DefaultCBCReport = [
      {
        "ConstEntities": [

          {
            "ConstEntity":
              {
                "Name": []
              },
            "BizActivities": [null]
          }

        ]

      }
    ];

    vm.save = function (formId) {
      $('#test').find('input').each(function (idx, input) {
        console.log('nkm', $(input).attr("disabled", "disabled"));
      });
    }
    vm.delay = 0;
    vm.minDuration = 0;
    vm.templateUrl = '';
    vm.message = 'Please Wait...';
    vm.backdrop = true;
    vm.myPromiseReport = null;
    vm.myPromiseConstituent = null;
    vm.myPromiseOtherInforMAtion = null;
    vm.otherInforArr = [];
    vm.formData = {};
    vm.constEntitiesOnJusArr = [];

    vm.disabled = undefined;
    vm.searchEnabled = undefined;

    vm.setInputFocus = function () {
      vm.$broadcast('uiSelectDemo1');
    };

    vm.enable = function () {
      vm.disabled = false;
    };

    vm.disable = function () {
      vm.disabled = true;
    };

    vm.enableSearch = function () {
      vm.searchEnabled = true;
    };

    vm.disableSearch = function () {
      vm.searchEnabled = false;
    };

    vm.status = {
      isFivmtOpen: true,
      isFivmtDisabled: false
    };

    vm.statusCon = {
      isFivmtOpen: true,
      isFivmtDisabled: false
    };

    vm.statusAdd = {
      isFivmtOpen: true,
      isFivmtDisabled: false
    };
    vm.reportid = null;
    vm.showreportDiv = false;
    vm.showConstEntityDiv = true;
    vm.constEntityId;
    vm.oneAtATime = true;
    vm.otherBusinessActivityIsRequired = false;

    vm.countrycodes = [

      { code: "AF", name: "AFGHANISTAN" },

      { code: "AX", name: "ALAND ISLANDS" },

      { code: "AL", name: "ALBANIA" },

      { code: "DZ", name: "ALGERIA" },

      { code: "AS", name: "AMERICAN SAMOA" },

      { code: "AD", name: "ANDORRA" },

      { code: "AO", name: "ANGOLA" },

      { code: "AI", name: "ANGUILLA" },

      { code: "AQ", name: "ANTARCTICA" },

      { code: "AG", name: "ANTIGUA AND BARBUDA" },

      { code: "AR", name: "ARGENTINA" },

      { code: "AM", name: "ARMENIA" },

      { code: "AW", name: "ARUBA" },

      { code: "AU", name: "AUSTRALIA" },

      { code: "AT", name: "AUSTRIA" },

      { code: "AZ", name: "AZERBAIJAN" },

      { code: "BS", name: "BAHAMAS" },

      { code: "BH", name: "BAHRAIN" },

      { code: "BD", name: "BANGLADESH" },

      { code: "BB", name: "BARBADOS" },

      { code: "BY", name: "BELARUS" },

      { code: "BE", name: "BELGIUM" },

      { code: "BZ", name: "BELIZE" },

      { code: "BJ", name: "BENIN" },

      { code: "BM", name: "BERMUDA" },

      { code: "BT", name: "BHUTAN" },

      { code: "BO", name: "BOLIVIA, PLURINATIONAL STATE OF" },

      { code: "BQ", name: "BONAIRE, SINT EUSTATIUS AND SABA" },

      { code: "BA", name: "BOSNIA AND HERZEGOVINA" },

      { code: "BW", name: "BOTSWANA" },

      { code: "BV", name: "BOUVET ISLAND" },

      { code: "BR", name: "BRAZIL" },

      { code: "IO", name: "BRITISH INDIAN OCEAN TERRITORY" },

      { code: "BN", name: "BRUNEI DARUSSALAM" },

      { code: "BG", name: "BULGARIA" },

      { code: "BF", name: "BURKINA FASO" },

      { code: "BI", name: "BURUNDI" },

      { code: "KH", name: "CAMBODIA" },

      { code: "CM", name: "CAMEROON" },

      { code: "CA", name: "CANADA" },

      { code: "CV", name: "CAPE VERDE" },

      { code: "KY", name: "CAYMAN ISLANDS" },

      { code: "CF", name: "CENTRAL AFRICAN REPUBLIC" },

      { code: "TD", name: "CHAD" },

      { code: "CL", name: "CHILE" },

      { code: "CN", name: "CHINA" },

      { code: "CX", name: "CHRISTMAS ISLAND" },

      { code: "CC", name: "COCOS (KEELING) ISLANDS" },

      { code: "CO", name: "COLOMBIA" },

      { code: "KM", name: "COMOROS" },

      { code: "CG", name: "CONGO" },

      { code: "CD", name: "CONGO, THE DEMOCRATIC REPUBLIC OF THE" },

      { code: "CK", name: "COOK ISLANDS" },

      { code: "CR", name: "COSTA RICA" },

      { code: "CI", name: "COTE D'IVOIRE" },

      { code: "HR", name: "CROATIA" },

      { code: "CU", name: "CUBA" },

      { code: "CW", name: "CURACAO" },

      { code: "CY", name: "CYPRUS" },

      { code: "CZ", name: "CZECH REPUBLIC" },

      { code: "DK", name: "DENMARK" },

      { code: "DJ", name: "DJIBOUTI" },

      { code: "DM", name: "DOMINICA" },

      { code: "DO", name: "DOMINICAN REPUBLIC" },

      { code: "EC", name: "ECUADOR" },

      { code: "EG", name: "EGYPT" },

      { code: "SV", name: "EL SALVADOR" },

      { code: "GQ", name: "EQUATORIAL GUINEA" },

      { code: "ER", name: "ERITREA" },

      { code: "EE", name: "ESTONIA" },

      { code: "ET", name: "ETHIOPIA" },

      { code: "FK", name: "FALKLAND ISLANDS (MALVINAS)" },

      { code: "FO", name: "FAROE ISLANDS" },

      { code: "FJ", name: "FIJI" },

      { code: "FI", name: "FINLAND" },

      { code: "FR", name: "FRANCE" },

      { code: "GF", name: "FRENCH GUIANA" },

      { code: "PF", name: "FRENCH POLYNESIA" },

      { code: "TF", name: "FRENCH SOUTHERN TERRITORIES" },

      { code: "GA", name: "GABON" },

      { code: "GM", name: "GAMBIA" },

      { code: "GE", name: "GEORGIA" },

      { code: "DE", name: "GERMANY" },

      { code: "GH", name: "GHANA" },

      { code: "GI", name: "GIBRALTAR" },

      { code: "GR", name: "GREECE" },

      { code: "GL", name: "GREENLAND" },

      { code: "GD", name: "GRENADA" },

      { code: "GP", name: "GUADELOUPE" },

      { code: "GU", name: "GUAM" },

      { code: "GT", name: "GUATEMALA" },

      { code: "GG", name: "GUERNSEY" },

      { code: "GN", name: "GUINEA" },

      { code: "GW", name: "GUINEA-BISSAU" },

      { code: "GY", name: "GUYANA" },

      { code: "HT", name: "HAITI" },

      { code: "HM", name: "HEARD ISLAND AND MCDONALD ISLANDS" },

      { code: "VA", name: "HOLY SEE (VATICAN CITY STATE)" },

      { code: "HN", name: "HONDURAS" },

      { code: "HK", name: "HONG KONG" },

      { code: "HU", name: "HUNGARY" },

      { code: "IS", name: "ICELAND" },

      { code: "IN", name: "INDIA" },

      { code: "ID", name: "INDONESIA" },

      { code: "IR", name: "IRAN, ISLAMIC REPUBLIC OF" },

      { code: "IQ", name: "IRAQ" },

      { code: "IE", name: "IRELAND" },

      { code: "IM", name: "ISLE OF MAN" },

      { code: "IL", name: "ISRAEL" },

      { code: "IT", name: "ITALY" },

      { code: "JM", name: "JAMAICA" },

      { code: "JP", name: "JAPAN" },

      { code: "JE", name: "JERSEY" },

      { code: "JO", name: "JORDAN" },

      { code: "KZ", name: "KAZAKHSTAN" },

      { code: "KE", name: "KENYA" },

      { code: "KI", name: "KIRIBATI" },

      { code: "KP", name: "KOREA, DEMOCRATIC PEOPLE'S REPUBLIC OF" },

      { code: "KR", name: "KOREA, REPUBLIC OF" },

      { code: "KW", name: "KUWAIT" },

      { code: "KG", name: "KYRGYZSTAN" },

      { code: "LA", name: "LAO PEOPLE'S DEMOCRATIC REPUBLIC" },

      { code: "LV", name: "LATVIA" },

      { code: "LB", name: "LEBANON" },

      { code: "LS", name: "LESOTHO" },

      { code: "LR", name: "LIBERIA" },

      { code: "LY", name: "LIBYA" },

      { code: "LI", name: "LIECHTENSTEIN" },

      { code: "LT", name: "LITHUANIA" },

      { code: "LU", name: "LUXEMBOURG" },

      { code: "MO", name: "MACAO" },

      { code: "MK", name: "MACEDONIA, THE FORMER YUGOSLAV REPUBLIC OF" },

      { code: "MG", name: "MADAGASCAR" },

      { code: "MW", name: "MALAWI" },

      { code: "MY", name: "MALAYSIA" },

      { code: "MV", name: "MALDIVES" },

      { code: "ML", name: "MALI" },

      { code: "MT", name: "MALTA" },

      { code: "MH", name: "MARSHALL ISLANDS" },

      { code: "MQ", name: "MARTINIQUE" },

      { code: "MR", name: "MAURITANIA" },

      { code: "MU", name: "MAURITIUS" },

      { code: "YT", name: "MAYOTTE" },

      { code: "MX", name: "MEXICO" },

      { code: "FM", name: "MICRONESIA, FEDERATED STATES OF" },

      { code: "MD", name: "MOLDOVA, REPUBLIC OF" },

      { code: "MC", name: "MONACO" },

      { code: "MN", name: "MONGOLIA" },

      { code: "ME", name: "MONTENEGRO" },

      { code: "MS", name: "MONTSERRAT" },

      { code: "MA", name: "MOROCCO" },

      { code: "MZ", name: "MOZAMBIQUE" },

      { code: "MM", name: "MYANMAR" },

      { code: "NA", name: "NAMIBIA" },

      { code: "NR", name: "NAURU" },

      { code: "NP", name: "NEPAL" },

      { code: "NL", name: "NETHERLANDS" },

      { code: "NC", name: "NEW CALEDONIA" },

      { code: "NZ", name: "NEW ZEALAND" },

      { code: "NI", name: "NICARAGUA" },

      { code: "NE", name: "NIGER" },

      { code: "NG", name: "NIGERIA" },

      { code: "NU", name: "NIUE" },

      { code: "NF", name: "NORFOLK ISLAND" },

      { code: "MP", name: "NORTHERN MARIANA ISLANDS" },

      { code: "NO", name: "NORWAY" },

      { code: "OM", name: "OMAN" },

      { code: "PK", name: "PAKISTAN" },

      { code: "PW", name: "PALAU" },

      { code: "PS", name: "PALESTINE, STATE OF" },

      { code: "PA", name: "PANAMA" },

      { code: "PG", name: "PAPUA NEW GUINEA" },

      { code: "PY", name: "PARAGUAY" },

      { code: "PE", name: "PERU" },

      { code: "PH", name: "PHILIPPINES" },

      { code: "PN", name: "PITCAIRN" },

      { code: "PL", name: "POLAND" },

      { code: "PT", name: "PORTUGAL" },

      { code: "PR", name: "PUERTO RICO" },

      { code: "QA", name: "QATAR" },

      { code: "RE", name: "REUNION" },

      { code: "RO", name: "ROMANIA" },

      { code: "RU", name: "RUSSIAN FEDERATION" },

      { code: "RW", name: "RWANDA" },

      { code: "BL", name: "SAINT BARTHELEMY" },

      { code: "SH", name: "SAINT HELENA, ASCENSION AND TRISTAN DA CUNHA" },

      { code: "KN", name: "SAINT KITTS AND NEVIS" },

      { code: "LC", name: "SAINT LUCIA" },

      { code: "MF", name: "SAINT MARTIN (FRENCH PART)" },

      { code: "PM", name: "SAINT PIERRE AND MIQUELON" },

      { code: "VC", name: "SAINT VINCENT AND THE GRENADINES" },

      { code: "WS", name: "SAMOA" },

      { code: "SM", name: "SAN MARINO" },

      { code: "ST", name: "SAO TOME AND PRINCIPE" },

      { code: "SA", name: "SAUDI ARABIA" },

      { code: "SN", name: "SENEGAL" },

      { code: "RS", name: "SERBIA" },

      { code: "SC", name: "SEYCHELLES" },

      { code: "SL", name: "SIERRA LEONE" },

      { code: "SG", name: "SINGAPORE" },

      { code: "SX", name: "SINT MAARTEN (DUTCH PART)" },

      { code: "SK", name: "SLOVAKIA" },

      { code: "SI", name: "SLOVENIA" },

      { code: "SB", name: "SOLOMON ISLANDS" },

      { code: "SO", name: "SOMALIA" },

      { code: "ZA", name: "SOUTH AFRICA" },

      { code: "GS", name: "SOUTH GEORGIA AND THE SOUTH SANDWICH ISLANDS" },

      { code: "SS", name: "SOUTH SUDAN" },

      { code: "ES", name: "SPAIN" },

      { code: "LK", name: "SRI LANKA" },

      { code: "SD", name: "SUDAN" },

      { code: "SR", name: "SURIname:" },

      { code: "SJ", name: "SVALBARD AND JAN MAYEN" },

      { code: "SZ", name: "SWAZILAND" },

      { code: "SE", name: "SWEDEN" },

      { code: "CH", name: "SWITZERLAND" },

      { code: "SY", name: "SYRIAN ARAB REPUBLIC" },

      { code: "TW", name: "TAIWAN, PROVINCE OF CHINA" },

      { code: "TJ", name: "TAJIKISTAN" },

      { code: "TZ", name: "TANZANIA, UNITED REPUBLIC OF" },

      { code: "TH", name: "THAILAND" },

      { code: "TL", name: "TIMOR-LESTE" },

      { code: "TG", name: "TOGO" },

      { code: "TK", name: "TOKELAU" },

      { code: "TO", name: "TONGA" },

      { code: "TT", name: "TRINIDAD AND TOBAGO" },

      { code: "TN", name: "TUNISIA" },

      { code: "TR", name: "TURKEY" },

      { code: "TM", name: "TURKMENISTAN" },

      { code: "TC", name: "TURKS AND CAICOS ISLANDS" },

      { code: "TV", name: "TUVALU" },

      { code: "UG", name: "UGANDA" },

      { code: "UA", name: "UKRAINE" },

      { code: "AE", name: "UNITED ARAB EMIRATES" },

      { code: "GB", name: "UNITED KINGDOM" },

      { code: "US", name: "UNITED STATES" },

      { code: "UM", name: "UNITED STATES MINOR OUTLYING ISLANDS" },

      { code: "UY", name: "URUGUAY" },

      { code: "UZ", name: "UZBEKISTAN" },

      { code: "VU", name: "VANUATU" },

      { code: "VE", name: "VENEZUELA, BOLIVARIAN REPUBLIC OF" },

      { code: "VN", name: "VIET NAM" },

      { code: "VG", name: "VIRGIN ISLANDS, BRITISH" },

      { code: "VI", name: "VIRGIN ISLANDS, U.S." },

      { code: "WF", name: "WALLIS AND FUTUNA" },

      { code: "EH", name: "WESTERN SAHARA" },

      { code: "YE", name: "YEMEN" },

      { code: "ZM", name: "ZAMBIA" },

      { code: "ZW", name: "ZIMBABWE" },

      { code: "XK", name: "KOSOVO" },

      { code: "X5", name: "STATELESS" }

    ];

    vm.BusinessActivity = [
      { name: "CBC601- Revenues - Unrelated", code: "CBC601" },
      { name: "CBC602- Revenues – Related", code: "CBC602" },
      { name: "CBC603- Revenues – Total", code: "CBC603" },
      { name: "CBC604- Profit or Loss", code: "CBC604" },
      { name: "CBC605- Tax Paid", code: "CBC605" },
      { name: "CBC606- Tax Accrued", code: "CBC606" },
      { name: "CBC607- Capital", code: "CBC607" },
      { name: "CBC608- Earnings", code: "CBC608" },
      { name: "CBC609- Number of Employees", code: "CBC609" },
      { name: "CBC610- Assets", code: "CBC610" },
      { name: "CBC611- Name of MNE Group", code: "CBC611" }
    ];

    vm.mainBusinessActivitys = [
      { name: "CBC501- Research and Development", code: "CBC501" },
      { name: "CBC502- Holding or Managing intellectual property", code: "CBC502" },
      { name: "CBC503- Purchasing or Procurement", code: "CBC503" },
      { name: "CBC504- Manufacturing or Production", code: "CBC504" },
      { name: "CBC505- Sales, Marketing or Distribution", code: "CBC505" },
      { name: "CBC506- Administrative, Management or Support Services", code: "CBC506" },
      { name: "CBC507- Provision of Services to unrelated parties", code: "CBC507" },
      { name: "CBC508- Internal Group Finance", code: "CBC508" },
      { name: "CBC509- Regulated Financial Services", code: "CBC509" },
      { name: "CBC510- Insurance", code: "CBC510" },
      { name: "CBC511- Holding shares or other equity instruments", code: "CBC511" },
      { name: "CBC512- Dormant", code: "CBC512" },
      { name: "CBC513- Other", code: "CBC513" }

    ];

    vm.addresstypes = [
      { name: "OECD301- Residential Or Business", code: "OECD301" },
      { name: "OECD302- Residential", code: "OECD302" },
      { name: "OECD303- Business", code: "OECD303" },
      { name: "OECD304- Registered Office", code: "OECD304" }
    ];

    vm.reportingRole = [
      { name: "CBC701- Ultimate Parent Entity", code: "CBC701" },
      { name: "CBC702- Surrogate Parent Entity", code: "CBC702" },
      { name: "CBC703- Local Filing", code: "CBC703" }
    ];

    vm.currencyCodes = [

      { code: "AED", name: "UAE Dirham: UNITED ARAB EMIRATES" },
      { code: "AFN", name: "Afghani: AFGHANISTAN" },
      { code: "ALL", name: "Lek: ALBANIA" },
      { code: "AMD", name: "Armenian Dram: ARMENIA	" },
      { code: "ANG", name: "Netherlands Antillean Guilder: CURACAO; SINT MAARTEN (DUTCH PART)" },
      { code: "AOA", name: "Kwanza: ANGOLA" },
      { code: "ARS", name: "Argentine Peso: ARGENTINA" },
      { code: "AUD", name: "Australian Dollar: AUSTRALIA; CHRISTMAS ISLAND; COCOS (KEELING) ISLANDS; HEARD ISLAND AND McDONALD ISLANDS; KIRIBATI; NAURU; NORFOLK ISLAND; TUVALU" },
      { code: "AWG", name: "Aruban Florin: ARUBA" },
      { code: "AZN", name: "Azerbaijanian Manat: AZERBAIJAN" },
      { code: "BAM", name: "Convertible Mark: BOSNIA AND HERZEGOVINA" },
      { code: "BBD", name: "Barbados Dollar: BARBADOS" },
      { code: "BDT", name: "Taka: BANGLADESH" },
      { code: "BGN", name: "Bulgarian Lev: BULGARIA" },
      { code: "BHD", name: "Bahraini Dinar: BAHRAIN" },
      { code: "BIF", name: "Burundi Franc: BURUNDI" },
      { code: "BMD", name: "Bermudian Dollar: BERMUDA" },
      { code: "BND", name: "Brunei Dollar: BRUNEI DARUSSALAM" },
      { code: "BOB", name: "Boliviano: BOLIVIA, PLURINATIONAL STATE OF" },
      { code: "BOV", name: "Mvdol: BOLIVIA, PLURINATIONAL STATE OF" },
      { code: "BRL", name: "Brazilian Real: BRAZIL" },
      { code: "BSD", name: "Bahamian Dollar: BAHAMAS" },
      { code: "BTN", name: "Ngultrum: BHUTAN" },
      { code: "BWP", name: "Pula: BOTSWANA" },
      { code: "BYR", name: "Belarussian Ruble: BELARUS" },
      { code: "BZD", name: "Belize Dollar: BELIZE" },
      { code: "CAD", name: "Canadian Dollar: CANADA" },
      { code: "CDF", name: "Congolese Franc: CONGO, THE DEMOCRATIC REPUBLIC OF" },
      { code: "CHE", name: "WIR Euro: SWITZERLAND" },
      { code: "CHF", name: "Swiss Franc: LIECHTENSTEIN; SWITZERLAND" },
      { code: "CHW", name: "WIR Franc: SWITZERLAND" },
      { code: "CLF", name: "Unidades de fomento: CHILE" },
      { code: "CLP", name: "Chilean Peso: CHILE" },
      { code: "CNY", name: "Yuan Renminbi: CHINA" },
      { code: "COP", name: "Colombian Peso: COLOMBIA" },
      { code: "COU", name: "Unidad de Valor Real: COLOMBIA" },
      { code: "CRC", name: "Costa Rican Colon: COSTA RICA" },
      { code: "CUC", name: "Peso Convertible: CUBA" },
      { code: "CUP", name: "Cuban Peso: CUBA" },
      { code: "CVE", name: "Cape Verde Escudo: CAPE VERDE" },
      { code: "CZK", name: "Czech Koruna: CZECH REPUBLIC" },
      { code: "DJF", name: "Djibouti Franc: DJIBOUTI" },
      { code: "DKK", name: "Danish Krone: DENMARK; FAROE ISLANDS; GREENLAND" },
      { code: "DOP", name: "Dominican Peso: DOMINICAN REPUBLIC" },
      { code: "DZD", name: "Algerian Dinar: ALGERIA" },
      { code: "EGP", name: "Egyptian Pound: EGYPT" },
      { code: "ERN", name: "Nakfa: ERITREA" },
      { code: "ETB", name: "Ethiopian Birr: ETHIOPIA" },
      { code: "EUR", name: "Euro: ALAND ISLANDS; ANDORRA; AUSTRIA; BELGIUM; CYPRUS; ESTONIA; EUROPEAN UNION; FINLAND; FRANCE; FRENCH GUIANA; FRENCH SOUTHERN TERRITORIES; GERMANY; GREECE; GUADELOUPE; HOLY SEE (VATICAN CITY STATE); IRELAND; ITALY; LUXEMBOURG; MALTA; MARTINIQUE; MAYOTTE; MONACO; MONTENEGRO; NETHERLANDS; PORTUGAL; REUNION; SAINT BARTHELEMY; SAINT MARTIN (FRENCH PART); SAINT PIERRE AND MIQUELON; SAN MARINO; SLOVAKIA; SLOVENIA; SPAIN; Vatican City State (HOLY SEE)" },
      { code: "FJD", name: "Fiji Dollar: FIJI" },
      { code: "FKP", name: "Falkland Islands Pound: FALKLAND ISLANDS (MALVINAS)" },
      { code: "GBP", name: "Pound Sterling: GUERNSEY; ISLE OF MAN; JERSEY; UNITED KINGDOM" },
      { code: "GEL", name: "Lari: GEORGIA" },
      { code: "GHS", name: "Ghana Cedi: GHANA" },
      { code: "GIP", name: "Gibraltar Pound: GIBRALTAR" },
      { code: "GMD", name: "Dalasi: GAMBIA" },
      { code: "GNF", name: "Guinea Franc: GUINEA" },
      { code: "GTQ", name: "Quetzal: GUATEMALA" },
      { code: "GYD", name: "Guyana Dollar: GUYANA" },
      { code: "HKD", name: "Hong Kong Dollar: HONG KONG" },
      { code: "HNL", name: "Lempira: HONDURAS" },
      { code: "HRK", name: "Croatian Kuna: CROATIA" },
      { code: "HTG", name: "Gourde: HAITI" },
      { code: "HUF", name: "Forint: HUNGARY" },
      { code: "IDR", name: "Rupiah: INDONESIA" },
      { code: "ILS", name: "New Israeli Sheqel: ISRAEL" },
      { code: "INR", name: "Indian Rupee: BHUTAN; INDIA" },
      { code: "IQD", name: "Iraqi Dinar: IRAQ" },
      { code: "IRR", name: "Iranian Rial: IRAN, ISLAMIC REPUBLIC OF" },
      { code: "ISK", name: "Iceland Krona: ICELAND" },
      { code: "JMD", name: "Jamaican Dollar: JAMAICA" },
      { code: "JOD", name: "Jordanian Dinar: JORDAN" },
      { code: "JPY", name: "Yen: JAPAN" },
      { code: "KES", name: "Kenyan Shilling: KENYA" },

      { code: "KGS", name: "Som: KYRGYZSTAN" },

      { code: "KHR", name: "Riel: CAMBODIA" },

      { code: "KMF", name: "Comoro Franc: COMOROS" },

      { code: "KPW", name: "North Korean Won: KOREA, DEMOCRATIC PEOPLE’S REPUBLIC OF" },

      { code: "KRW", name: "Won: KOREA, REPUBLIC OF" },

      { code: "KWD", name: "Kuwaiti Dinar: KUWAIT" },

      { code: "KYD", name: "Cayman Islands Dollar: CAYMAN ISLANDS" },

      { code: "KZT", name: "Tenge: KAZAKHSTAN" },

      { code: "LAK", name: "Kip: LAO PEOPLE’S DEMOCRATIC REPUBLIC" },

      { code: "LBP", name: "Lebanese Pound: LEBANON" },

      { code: "LKR", name: "Sri Lanka Rupee: SRI LANKA" },

      { code: "LRD", name: "Liberian Dollar: LIBERIA" },

      { code: "LSL", name: "Loti: LESOTHO" },

      { code: "LTL", name: "Lithuanian Litas: LITHUANIA" },

      { code: "LVL", name: "Latvian Lats: LATVIA" },

      { code: "LYD", name: "Libyan Dinar: LIBYA" },

      { code: "MAD", name: "Moroccan Dirham: MOROCCO; WESTERN SAHARA" },

      { code: "MDL", name: "Moldovan Leu: MOLDOVA, REPUBLIC OF" },

      { code: "MGA", name: "Malagasy Ariary: MADAGASCAR" },

      { code: "MKD", name: "Denar: MACEDONIA, THE FORMER YUGOSLAV REPUBLIC OF" },

      { code: "MMK", name: "Kyat: MYANMAR" },

      { code: "MNT", name: "Tugrik: MONGOLIA" },

      { code: "MOP", name: "Pataca: MACAO" },

      { code: "MRO", name: "Ouguiya: MAURITANIA" },

      { code: "MUR", name: "Mauritius Rupee: MAURITIUS" },

      { code: "MVR", name: "Rufiyaa: MALDIVES" },

      { code: "MWK", name: "Kwacha: MALAWI" },

      { code: "MXN", name: "Mexican Peso: MEXICO" },

      { code: "MXV", name: "Mexican Unidad de Inversion (UDI): MEXICO" },

      { code: "MYR", name: "Malaysian Ringgit: MALAYSIA" },

      { code: "MZN", name: "Mozambique Metical: MOZAMBIQUE" },

      { code: "NAD", name: "Namibia Dollar: NAMIBIA" },

      { code: "NGN", name: "Naira: NIGERIA" },

      { code: "NIO", name: "Cordoba Oro: NICARAGUA" },

      { code: "NOK", name: "Norwegian Krone: BOUVET ISLAND; NORWAY; SVALBARD AND JAN MAYEN" },

      { code: "NPR", name: "Nepalese Rupee: NEPAL" },

      { code: "NZD", name: "New Zealand Dollar: COOK ISLANDS; NEW ZEALAND; NIUE; PITCAIRN; TOKELAU" },

      { code: "OMR", name: "Rial Omani: OMAN" },

      { code: "PAB", name: "Balboa: PANAMA" },

      { code: "PEN", name: "Nuevo Sol: PERU" },

      { code: "PGK", name: "Kina: PAPUA NEW GUINEA" },

      { code: "PHP", name: "Philippine Peso: PHILIPPINES" },

      { code: "PKR", name: "Pakistan Rupee: PAKISTAN" },

      { code: "PLN", name: "Zloty: POLAND" },

      { code: "PYG", name: "Guarani: PARAGUAY" },

      { code: "QAR", name: "Qatari Rial: QATAR" },

      { code: "RON", name: "New Romanian Leu: ROMANIA" },

      { code: "RSD", name: "Serbian Dinar: SERBIA" },

      { code: "RUB", name: "Russian Ruble: RUSSIAN FEDERATION" },

      { code: "RWF", name: "Rwanda Franc: RWANDA" },

      { code: "SAR", name: "Saudi Riyal: SAUDI ARABIA" },

      { code: "SBD", name: "Solomon Islands Dollar: SOLOMON ISLANDS" },

      { code: "SCR", name: "Seychelles Rupee: SEYCHELLES" },

      { code: "SDG", name: "Sudanese Pound: SUDAN" },

      { code: "SEK", name: "Swedish Krona: SWEDEN" },

      { code: "SGD", name: "Singapore Dollar: SINGAPORE" },

      { code: "SHP", name: "Saint Helena Pound: SAINT HELENA, ASCENSION AND TRISTAN DA CUNHA" },

      { code: "SLL", name: "Leone: SIERRA LEONE" },

      { code: "SOS", name: "Somali Shilling: SOMALIA" },

      { code: "SRD", name: "Surinam Dollar: SURINAME" },

      { code: "SSP", name: "South Sudanese Pound: SOUTH SUDAN" },

      { code: "STD", name: "Dobra: SAO TOME AND PRINCIPE" },

      { code: "SVC", name: "El Salvador Colon: EL SALVADOR" },

      { code: "SYP", name: "Syrian Pound: SYRIAN ARAB REPUBLIC" },

      { code: "SZL", name: "Lilangeni: SWAZILAND" },

      { code: "THB", name: "Baht: THAILAND" },

      { code: "TJS", name: "Somoni: TAJIKISTAN" },

      { code: "TMT", name: "Turkmenistan New Manat: TURKMENISTAN" },

      { code: "TND", name: "Tunisian Dinar: TUNISIA" },

      { code: "TOP", name: "Pa’anga: TONGA" },

      { code: "TRY", name: "Turkish Lira: TURKEY" },

      { code: "TTD", name: "Trinidad and Tobago Dollar: TRINIDAD AND TOBAGO" },

      { code: "TWD", name: "New Taiwan Dollar: TAIWAN, PROVINCE OF CHINA" },

      { code: "TZS", name: "Tanzanian Shilling: TANZANIA, UNITED REPUBLIC OF" },

      { code: "UAH", name: "Hryvnia: UKRAINE" },

      { code: "UGX", name: "Uganda Shilling: UGANDA" },

      { code: "USD", name: "US Dollar: AMERICAN SAMOA; BONAIRE; SINT EUSTATIUS AND SABA; BRITISH INDIAN OCEAN TERRITORY; ECUADOR; EL SALVADOR; GUAM; HAITI; MARSHALL ISLANDS; MICRONESIA, FEDERATED STATES OF; NORTHERN MARIANA ISLANDS; PALAU; PANAMA; PUERTO RICO; TIMOR-LESTE; TURKS AND CAICOS ISLANDS, UNITED STATES; UNITED STATES MINOR OUTLYING ISLANDS; VIRGIN ISLANDS (BRITISH); VIRGIN ISLANDS (US)" },

      { code: "USN", name: "US Dollar (Next day): UNITED STATES" },

      { code: "USS", name: " US Dollar (Same day): UNITED STATES" },

      { code: "UYI", name: "Uruguay Peso en Unidades Indexadas (URUIURUI): URUGUAY" },

      { code: "UYU", name: "Peso Uruguayo: URUGUAY" },

      { code: "UZS", name: "Uzbekistan Sum: UZBEKISTAN" },

      { code: "VEF", name: "Bolivar: VENEZUELA, BOLIVARIAN REPUBLIC OF" },

      { code: "VND", name: "Dong: VIET NAM" },

      { code: "VUV", name: "Vatu: VANUATU" },

      { code: "WST", name: "Tala: SAMOA" },

      { code: "XAF", name: "CFA Franc BEAC: CAMEROON; CENTRAL AFRICAN REPUBLIC; CHAD; CONGO; EQUATORIAL GUINEA; GABON" },

      { code: "XAG", name: "Silver: ZZ11_Silver" },

      { code: "XAU", name: "Gold: ZZ08_Gold" },

      { code: "XBA", name: "Bond Markets Unit European Composite Unit (EURCO):  ZZ01_Bond Markets Unit European_EURCO" },

      { code: "XBB", name: "Bond Markets Unit European Monetary Unit (E.M.U.-6): ZZ02_Bond Markets Unit European_EMU-6" },

      { code: "XBC", name: "Bond Markets Unit European Unit of Account 9 (E.U.A.-9): ZZ03_Bond Markets Unit European_EUA-9" },

      { code: "XBD", name: "Bond Markets Unit European Unit of Account 17 (E.U.A.-17): ZZ04_Bond Markets Unit European_EUA-17" },

      { code: "XCD", name: "East Caribbean Dollar: ANGUILLA; ANTIGUA AND BARBUDA; DOMINICA; GRENADA; MONTSERRAT; SAINT KITTS AND NEVIS; SAINT LUCIA; SAINT VINCENT AND THE GRENADINES" },

      { code: "XDR", name: "SDR (Special Drawing Right): INTERNATIONAL MONETARY FUND (IMF)" },

      { code: "XFU", name: "UIC-Franc: ZZ05_UIC-Franc" },

      { code: "XOF", name: "CFA Franc BCEAO: BENIN; BURKINA FASO; COTE D'IVOIRE; GUINEA-BISSAU; MALI; NIGER; SENEGAL; TOGO" },

      { code: "XPD", name: "Palladium: ZZ09_Palladium" },

      { code: "XPF", name: "CFP Franc: FRENCH POLYNESIA; NEW CALEDONIA; WALLIS AND FUTUNA" },

      { code: "XPT", name: "Platinum: ZZ10_Platinum" },

      { code: "XSU", name: "Sucre: SISTEMA UNITARIO DE COMPENSACION REGIONAL DE PAGOS 'SUCRE'" },

      { code: "XUA", name: "ADB Unit of Account: MEMBER COUNTRIES OF THE AFRICAN DEVELOPMENT BANK GROUP" },

      { code: "XXX", name: "The codes assigned for transactions where no currency is involved: ZZ07_No_Currency" },

      { code: "YER", name: "Yemeni Rial: YEMEN" },

      { code: "ZAR", name: "Rand: SOUTH AFRICA; LESOTHO; NAMIBIA" },

      { code: "ZMW", name: "Zambian Kwacha: ZAMBIA" },

      { code: "ZWL", name: "Zimbabwe Dollar: ZIMBABWE" }
    ];

    // vm.totalItems = vm.jurisdictionsArr.length;
    // vm.currentPage = 1;
    // vm.numPerPage = 2;
    // vm.imageIndex = vm.$index + (vm.currentPage - 1) * vm.pageSize;

    // vm.paginate = function(value) {
    //   var begin, end, index;
    //   begin = (vm.currentPage - 1) * vm.numPerPage;
    //   end = begin + vm.numPerPage;
    //   index = vm.jurisdictionsArr.indexOf(value);
    //   return (begin <= index && index < end);
    // };

    vm.CancelReport = function () {


      swal({
        title: 'Are you sure?',
        text: 'You will not be able to recover this CBC Report and its Constituent Entities!',
        type: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes',
        cancelButtonText: 'No',
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        allowOutsideClick: false
      }).then(function (result) {
        if (result) {
          // add a function to check if there is atleast one report.
          vm.showreportDiv = false;
          vm.reportid = null;
          vm.$apply();
          vm.DefaultCBCReport = [
            {
              "ConstEntities": [
                {
                  "ConstEntity":
                    {
                      "Name": [],
                      "ResCountryCode": ""
                    },
                  "BizActivities": [null]
                }

              ]

            }
          ];
          vm.constituentArr = [];
        }


      },
        function (dismiss) {
          if (dismiss == 'cancel') {
            // function when cancel button is clicked

          }

        });

    }

    vm.updateCBCReportOnEdit = function () {

      var isvalid = angular.element(CBCReportFormDiv).scope().CBCReportFormValid();
      if (isvalid) {
        vm.jurisdictionsArr[vm.reportid].Summary = angular.copy(vm.DefaultCBCReport[0].Summary);
        vm.jurisdictionsArr[vm.reportid].DocSpec = angular.copy(vm.DefaultCBCReport[0].DocSpec);
        vm.jurisdictionsArr[vm.reportid].ResCountryCode = angular.copy(vm.DefaultCBCReport[0].ResCountryCode);
        if (vm.DefaultCBCReport[0].ConstEntities[0]) {
          var validConstEntity = angular.element(CBCReportFormDiv).scope().ConstituentFormValid();
          if (validConstEntity) {
            vm.constituentArr.push(vm.DefaultCBCReport[0].ConstEntities[0]);
            // vm.DefaultCBCReport[0].ConstEntities = angular.copy(vm.constituentArr);
            vm.jurisdictionsArr[vm.reportid].ConstEntities = angular.copy(vm.constituentArr);
          }

        }
        vm.showConstEntityDiv = false;
        vm.showreportDiv = false;
        vm.constituentArr = [];
        vm.reportid = null;
      }
    }

    vm.CompletetedReport = function () {
      if (vm.jurisdictionsArr.length < 249) {
        if (vm.DefaultCBCReport.length == 1 && vm.showConstEntityDiv == true && vm.reportid !== null && vm.constEntityId == undefined) {
          vm.jurisdictionsArr[vm.reportid].Summary = angular.copy(vm.DefaultCBCReport[0].Summary);
          vm.jurisdictionsArr[vm.reportid].ResCountryCode = angular.copy(vm.DefaultCBCReport[0].ResCountryCode);
          vm.jurisdictionsArr[vm.reportid].DocSpec = angular.copy(vm.DefaultCBCReport[0].DocSpec);
          if (vm.DefaultCBCReport[0].ConstEntities[0]) {
            var validConstEntity = angular.element(CBCReportFormDiv).scope().ConstituentFormValid();
            if (validConstEntity) {
              vm.constituentArr.push(vm.DefaultCBCReport[0].ConstEntities[0]);
              // vm.DefaultCBCReport[0].ConstEntities = angular.copy(vm.constituentArr);
              vm.jurisdictionsArr[vm.reportid].ConstEntities = angular.copy(vm.constituentArr);
            }

          }

        }
        else {

          if (vm.DefaultCBCReport.length == 1 && vm.showreportDiv == true && vm.reportid == null && vm.constEntityId == undefined) {
            if (vm.DefaultCBCReport[0].ConstEntities[0]) {
              if (angular.isUndefined(vm.DefaultCBCReport[0].DocSpec)) {
                vm.DefaultCBCReport[0].DocSpec = {};
                vm.DefaultCBCReport[0].DocSpec.DocTypeIndic = {};
                vm.DefaultCBCReport[0].DocSpec.DocTypeIndic['#text'] = "OECD1";
              }
              var valid = angular.element(CBCReportFormDiv).scope().CBCReportFormValid();
              if (valid) {
                vm.jurisdictionsArr.push(vm.DefaultCBCReport[0]);
                var arrayLen = vm.jurisdictionsArr.length - 1;
                vm.constituentArr.push(vm.DefaultCBCReport[0].ConstEntities[0]);
                vm.DefaultCBCReport[0].ConstEntities = angular.copy(vm.constituentArr);
                vm.jurisdictionsArr[arrayLen] = angular.copy(vm.DefaultCBCReport[0]);
              }
              else {

                return;

              }
            }
          }
        }
        vm.showreportDiv = false;
        vm.showConstEntityDiv = false;
        vm.DefaultCBCReport = [
          {
            "ConstEntities": [
              {
                "ConstEntity":
                  {
                    "Name": [],
                    "ResCountryCode": ""
                  },
                "BizActivities": [null]
              }

            ]

          }
        ];
        vm.constituentArr = [];
        vm.reportid = null;
      }

      else {
        swal("Error", "The maximum number to be captured is 249");
        vm.showreportDiv = false;
        vm.reportid = null;
      }
    }
    vm.ShowCBCContainer = function () {

      if (vm.jurisdictionsArr.length < 249) {
        if (vm.DefaultCBCReport.length == 1 && vm.showConstEntityDiv == true && vm.reportid !== null && vm.constEntityId == undefined) {
          vm.jurisdictionsArr[vm.reportid].Summary = angular.copy(vm.DefaultCBCReport[0].Summary);
          vm.jurisdictionsArr[vm.reportid].DocSpec = angular.copy(vm.DefaultCBCReport[0].DocSpec);
          vm.jurisdictionsArr[vm.reportid].ResCountryCode = angular.copy(vm.DefaultCBCReport[0].ResCountryCode);
          if (vm.DefaultCBCReport[0].ConstEntities[0]) {
            var validConstEntity = angular.element(CBCReportFormDiv).scope().ConstituentFormValid();
            if (validConstEntity) {
              vm.constituentArr.push(vm.DefaultCBCReport[0].ConstEntities[0]);
              // vm.DefaultCBCReport[0].ConstEntities = angular.copy(vm.constituentArr);
              vm.jurisdictionsArr[vm.reportid].ConstEntities = angular.copy(vm.constituentArr);
            }

          }

        }
        else {

          if (vm.DefaultCBCReport.length == 1 && vm.showreportDiv == true && vm.reportid == null && vm.constEntityId == undefined) {
            if (vm.DefaultCBCReport[0].ConstEntities[0]) {
              if (angular.isUndefined(vm.DefaultCBCReport[0].DocSpec)) {
                vm.DefaultCBCReport[0].DocSpec = {};
                vm.DefaultCBCReport[0].DocSpec.DocTypeIndic = {};
                vm.DefaultCBCReport[0].DocSpec.DocTypeIndic['#text'] = "OECD1";
              }
              var valid = angular.element(CBCReportFormDiv).scope().CBCReportFormValid();
              if (valid) {
                vm.jurisdictionsArr.push(vm.DefaultCBCReport[0]);
                var arrayLen = vm.jurisdictionsArr.length - 1;
                vm.constituentArr.push(vm.DefaultCBCReport[0].ConstEntities[0]);
                vm.DefaultCBCReport[0].ConstEntities = angular.copy(vm.constituentArr);
                vm.jurisdictionsArr[arrayLen] = angular.copy(vm.DefaultCBCReport[0]);
              }
              else {

                return;

              }
            }
          }
        }
        vm.showreportDiv = true;
        vm.showConstEntityDiv = true;
        vm.DefaultCBCReport = [
          {
            "ConstEntities": [
              {
                "ConstEntity":
                  {
                    "Name": [],
                    "ResCountryCode": ""
                  },
                "BizActivities": [null]
              }

            ]

          }
        ];
        vm.constituentArr = [];
        vm.reportid = null;
      }

      else {
        swal("Error", "The maximum number to be captured is 249");
        vm.showreportDiv = false;
        vm.reportid = null;
      }


    };
    $scope.tableRowExpanded = false;
    $scope.tableRowIndexExpandedCurr = "";
    $scope.tableRowIndexExpandedPrev = "";
    $scope.storeIdExpanded = "";

    $scope.dayDataCollapseFn = function () {
      $scope.dayDataCollapse = [];
      for (var i = 0; i < vm.jurisdictionsArr.length; i += 1) {
        $scope.dayDataCollapse.push(false);
      }
    };


    $scope.selectTableRow = function (index) {
      if (typeof $scope.dayDataCollapse === 'undefined') {
        $scope.dayDataCollapseFn();
      }

      if ($scope.tableRowExpanded === false && $scope.tableRowIndexExpandedCurr === "") {
        $scope.tableRowIndexExpandedPrev = "";
        $scope.tableRowExpanded = true;
        $scope.tableRowIndexExpandedCurr = index;
        $scope.dayDataCollapse[index] = true;
      } else if ($scope.tableRowExpanded === true) {
        if ($scope.tableRowIndexExpandedCurr === index) {
          $scope.tableRowExpanded = false;
          $scope.tableRowIndexExpandedCurr = "";
          $scope.dayDataCollapse[index] = false;
        } else {
          $scope.tableRowIndexExpandedPrev = $scope.tableRowIndexExpandedCurr;
          $scope.tableRowIndexExpandedCurr = index;
          $scope.dayDataCollapse[$scope.tableRowIndexExpandedPrev] = false;
          $scope.dayDataCollapse[$scope.tableRowIndexExpandedCurr] = true;
        }
      }

    };


    vm.addCBCReport = function (jurisdiction) {


      //     var validEntity= vm.reportConstEntitiesValid();
      //     if(!validEntity){
      //  return;
      //     }

      var valid = angular.element(CBCReportFormDiv).scope().CBCReportFormValid();

      if (valid) {

        vm.myPromiseReport = $timeout(function () {

          if (vm.reportid == null) {

            if (angular.isUndefined(jurisdiction.DocSpec)) {
              jurisdiction.DocSpec = {};
              jurisdiction.DocSpec.DocTypeIndic = {};
              jurisdiction.DocSpec.DocTypeIndic['#text'] = "OECD1";
            }
            if (vm.jurisdictionsArr.length < 249) {
              if (vm.constituentArr.length > 0 && vm.showreportDiv == true) {
                vm.constituentArr.push(jurisdiction.ConstEntities[0]);
                // vm.DefaultCBCReport[0].ConstEntities=angular.copy(vm.constituentArr);
                jurisdiction.ConstEntities = (vm.constituentArr);
                // vm.jurisdictionsArr.push(vm.DefaultCBCReport[0]);

              }
              // else{

              // 
              // }


              vm.jurisdictionsArr.push(jurisdiction);
              noOfReports = vm.jurisdictionsArr.length;
              vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports = vm.jurisdictionsArr;
              vm.formData.CountryByCountryDeclaration.NoOfTaxJurisdictions = noOfReports;

              if (angular.isUndefined(vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[noOfReports - 1])) {
                vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[noOfReports - 1] = {};
                vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[noOfReports - 1].NoOfConstituentEntities = {};
                vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[noOfReports - 1].NoOfConstituentEntities = vm.constituentArr.length; //jurisdiction.ConstEntities.length;
              }
              else if (angular.isUndefined(vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[noOfReports - 1].NoOfConstituentEntities)) {
                vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[noOfReports - 1].NoOfConstituentEntities = {};
                vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[noOfReports - 1].NoOfConstituentEntities = vm.constituentArr.length; //jurisdiction.ConstEntities.length;
              }
              vm.$apply();
              vm.showreportDiv = false;
              vm.DefaultCBCReport = [
                {
                  "ConstEntities": [
                    {
                      "ConstEntity":
                        {
                          "Name": [],
                          "ResCountryCode": ""
                        },
                      "BizActivities": [null]
                    }

                  ]

                }
              ];
              vm.constituentArr = [];

            }
            else {
              swal("Error", "The maximum number to be captured is 249");
            }
          }
          else {

            if (vm.jurisdictionsArr.length < 249) {

              var validEntity = vm.onEditreportConstEntitiesValid();
              if (!validEntity) {
                return;
              }
              if (vm.constituentArr.length > 0 && vm.showreportDiv == true) {

                if (jurisdiction.ConstEntities && jurisdiction.ConstEntities.length) {
                  vm.constituentArr.push(jurisdiction.ConstEntities[0]);
                }
                vm.DefaultCBCReport[0].ConstEntities = angular.copy(vm.constituentArr);
                // vm.jurisdictionsArr[vm.reportid] = angular.copy(vm.DefaultCBCReport[0]);
                // jurisdiction.ConstEntities= angular.copy(vm.constituentArr);

              }

            }


            vm.jurisdictionsArr[vm.reportid] = angular.copy(vm.DefaultCBCReport[0]);
            if (angular.isUndefined(vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[vm.reportid])) {
              vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[vm.reportid] = {};
              vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[vm.reportid].NoOfConstituentEntities = {};
              //vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[vm.reportid].NoOfConstituentEntities = jurisdiction.ConstEntities.length;
            }
            if (jurisdiction.ConstEntities && (jurisdiction.ConstEntities.length >= 0)) {
              //vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[vm.reportid].NoOfConstituentEntities = jurisdiction.ConstEntities.length;
              vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[vm.reportid].NoOfConstituentEntities = vm.constituentArr.length;
            }
            else {
              vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[vm.reportid].NoOfConstituentEntities = 0;
              swal("Error", "Please complete at least one Constituent Entity for this report before proceeding.");
              return;
            }
            vm.$apply();
            vm.showreportDiv = true;
            vm.reportid = null;
            vm.constObj = vm.addNewConstituentObj();
            vm.DefaultCBCReport = [
              {
                "ConstEntities": [
                  {
                    "ConstEntity":
                      {
                        "Name": []
                      },
                    "BizActivities": [null]
                  }

                ]

              }
            ];
            vm.constituentArr = [];
          }

          //vm.$apply();
        }, 2000)
          .then(function () {

          }).then(function () {

          }).then(function () {

          });

        return vm.myPromiseReport;
      }

      else {

        return valid;
      }
    }
    //Edit Report
    vm.EditCBCReport = function (index, e) {
      vm.showreportDiv = true;
      if (e) {
        e.preventDefault();
        e.stopPropagation();
      }
      vm.reportid = index;
      vm.DefaultCBCReport[0] = angular.copy(vm.jurisdictionsArr[index]);

      if (angular.isDefined(vm.DefaultCBCReport[0].ResCountryCode)) {
        vm.resCountryName = vm.getNameOfSelectedCountryCode(vm.DefaultCBCReport[0].ResCountryCode);
      }

      if (vm.jurisdictionsArr[index].ConstEntities) {
        vm.constituentArr = angular.copy(vm.jurisdictionsArr[index].ConstEntities);
      }
      vm.DefaultCBCReport[0].ConstEntities = [
        //   {

        //     "BizActivities": [null]
        //   }

      ];


    }

    //Edit Constituent entity 
    vm.EditConstituentEntity = function (reportIndex, index) {
      vm.showConstEntityDiv = true;
      vm.reportid = reportIndex;
      vm.constEntityId = index;

      if (vm.jurisdictionsArr[reportIndex].ConstEntities[index] == null) {

        vm.jurisdictionsArr[reportIndex].ConstEntities[index] = {};
      }
      vm.DefaultCBCReport[0].ConstEntities[0] = angular.copy(vm.jurisdictionsArr[reportIndex].ConstEntities[index]);
      if (angular.isDefined(vm.DefaultCBCReport[0].ConstEntities[0].ConstEntity.ResCountryCode)) {
        vm.entityResCountryName = vm.getNameOfSelectedCountryCode(vm.DefaultCBCReport[0].ConstEntities[0].ConstEntity.ResCountryCode);
      }

    }

    vm.updateConstituentEntity = function (jurisdiction, index) {

      var isvalid = angular.element(CBCReportFormDiv).scope().ConstituentFormValid();
      if (isvalid) {
        vm.constituentArr[index] = angular.copy(vm.DefaultCBCReport[0].ConstEntities[0]);
        vm.jurisdictionsArr[vm.reportid].ConstEntities[index] = angular.copy(vm.constituentArr[index]);
        vm.DefaultCBCReport[0].ConstEntities = [];
        vm.showConstEntityDiv = false;
        vm.constEntityId = undefined;
      }
    }

    vm.cancelUpdateConstituentEntity = function (jurisdiction, index) {

      swal({
        title: 'Are you sure?',
        text: 'You will not be able to recover the Changes to the Constituent Entity!',
        type: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes,Delete',
        cancelButtonText: 'No, Cancel',
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        allowOutsideClick: false
      }).then(function (result) {
        if (result) {

          vm.showConstEntityDiv = false;
          vm.$apply();
          vm.DefaultCBCReport[0].ConstEntities = [];
          // swal(
          //   'Deleted!',
          //   'The Constituent Entity has been deleted.',
          //   'success'
          // )
        }

        vm.constEntityId = undefined;
      },
        function (dismiss) {
          if (dismiss == 'cancel') {
            // function when cancel button is clicked

          }

        });



    }

    //Delete Constituent entity
    vm.RemoveConstituentEntity = function (reportIndex, index) {//(jurisdictions, index, e) {

      /* if (e) {
         e.preventDefault();
         e.stopPropagation();
       }
       
       if (jurisdictions.ConstEntities.length > 1) {
         // if (index > 0) {
         jurisdictions.ConstEntities.splice(index, 1);
       }*/

      swal({
        title: 'Are you sure?',
        text: 'You will not be able to recover the contents of this Constituent Entity!',
        type: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes,Delete',
        cancelButtonText: 'No, Cancel',
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        allowOutsideClick: false
      }).then(function (result) {
        if (result) {
          if (vm.jurisdictionsArr[reportIndex].ConstEntities.length > 1) {
            vm.constituentArr.splice(index, 1);
            vm.jurisdictionsArr[reportIndex].ConstEntities.splice(index, 1);
            //noOfReports = vm.jurisdictionsArr.length;
            //vm.formData.CountryByCountryDeclaration.NoOfTaxJurisdictions = noOfReports,
            //vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports = vm.jurisdictionsArr;
            vm.$apply();
            swal(
              'Deleted!',
              'The Constituent Entity has been deleted.',
              'success'
            )
          }
          else {
            swal(
              ' ',
              'Your should have atleast one Constituent Entity',
              'warning'
            )

          }
          if (angular.isUndefined(vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[reportIndex])) {
            vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[reportIndex] = {};
            vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[reportIndex].NoOfConstituentEntities = {};
          }
          vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[reportIndex].NoOfConstituentEntities = vm.constituentArr.length;
        }
      },
        function (dismiss) {
          if (dismiss == 'cancel') {
            // function when cancel button is clicked

          }

        });

    };

    vm.constObj = {};
    vm.addConstituentEntity = function (jurisdictions, e) {

      if (e) {
        e.preventDefault();
        e.stopPropagation();
      }

      if (vm.constituentArr.length >= 350) {
        swal("Error", "The maximum number to be captured is 350");
        return;

      }

      if (!vm.reportid == null) {

        var isvalid = vm.onEditreportConstEntitiesValid();
        if (!isvalid) {
          return;
        }

      }


      var isConstituentvalid = angular.element(CBCReportFormDiv).scope().CBCReportFormValid();

      if (isConstituentvalid) {



        if ((jurisdictions.ConstEntities && jurisdictions.ConstEntities.length)) {

          vm.myPromiseConstituent = $timeout(function () {

            // vm.constObj = vm.addNewConstituentObj();

            // if (vm.constituentArr) {


            if (vm.constituentArr.length < 350) {


              // jurisdictions.ConstEntities.push(vm.addNewConstituentObj());
              // vm.constituentArr = angular.copy(jurisdictions.ConstEntities);
              if (jurisdictions.ConstEntities && (jurisdictions.ConstEntities.length > 0) && vm.reportid !== null) {
                vm.constituentArr.push(jurisdictions.ConstEntities[0]);
                vm.jurisdictionsArr[vm.reportid].ConstEntities = angular.copy(vm.constituentArr);
              }
              else {

                if (angular.isUndefined(jurisdictions.DocSpec)) {
                  jurisdictions.DocSpec = {};
                  jurisdictions.DocSpec.DocTypeIndic = {};
                  jurisdictions.DocSpec.DocTypeIndic['#text'] = "OECD1";
                }
                vm.jurisdictionsArr.push(jurisdictions);
                var arrayLen = vm.jurisdictionsArr.length - 1;
                vm.constituentArr.push(jurisdictions.ConstEntities[0]);
                vm.DefaultCBCReport[0].ConstEntities = angular.copy(vm.constituentArr);
                vm.jurisdictionsArr[arrayLen] = angular.copy(vm.DefaultCBCReport[0]);
                vm.reportid = arrayLen;
              }
              vm.showConstEntityDiv = true;
              vm.DefaultCBCReport[0].ConstEntities = [

                {
                  "ConstEntity":
                    {
                      "Name": []
                    },
                  "BizActivities": [null]
                }

              ];

            }

            else {
              swal("Error", "The maximum number to be captured is 350");

            }

            // }


          }, 1000)
            .then(function () {

            }).then(function () {

            }).then(function () {

            });
          return vm.myPromiseConstituent;
        }
        else {
          vm.showConstEntityDiv = true;
          vm.DefaultCBCReport[0].ConstEntities = [

            {
              "ConstEntity":
                {
                  "Name": []
                },
              "BizActivities": [null]
            }

          ];
          return false;
        }
      }

      else {
        //open Constuent Entity View for capturing
        // swal("warning", "Please Capture Constituent entity!");
        vm.showConstEntityDiv = true;
        vm.DefaultCBCReport[0].ConstEntities = [

          {
            "ConstEntity":
              {
                "Name": []
              },
            "BizActivities": [null]
          }

        ];
        return;

      }


    }


    // vm.SaveCBCReport = function () {

    //   if (vm.jurisdictionsArr.length < 249) {
    //     vm.jurisdictionsArr.push(vm.jurisdiction);
    //     // noOfReports = jurisdiction.length;
    //     vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports = vm.jurisdictionsArr;
    //     // vm.formData.CountryByCountryDeclaration.NoOfTaxJurisdictions = noOfReports
    //     vm.showReportDiv = true;
    //     vm.showConstituentDiv = true;
    //   }
    //   else {
    //     swal("Error", "The maximum number to be captured is 249");
    //   }
    //   //vm.jurisdiction = {};


    // }

    vm.RemoveCBCReport = function (index, e) {


      swal({
        title: 'Are you sure?',
        text: 'You will not be able to recover this CBC Report and its Constituent Entities!',
        type: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes,Delete',
        cancelButtonText: 'No, Cancel',
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        allowOutsideClick: false
      }).then(function (result) {
        if (result) {
          if (vm.jurisdictionsArr.length > 1) {
            // var index=vm.jurisdictionsArr.indexOf(jurisdiction);
            vm.jurisdictionsArr.splice(index, 1);
            noOfReports = vm.jurisdictionsArr.length;
            vm.formData.CountryByCountryDeclaration.NoOfTaxJurisdictions = noOfReports,
              vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction.splice(index, 1);
            vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports = vm.jurisdictionsArr;

            swal(
              'Deleted!',
              'Your CBC Report has been deleted.',
              'success'
            )
            vm.$apply();
          }
          else {
            swal(
              ' ',
              'Your should Have atleast one report',
              'warning'
            )

          }
        }
      },
        function (dismiss) {
          if (dismiss == 'cancel') {
            // function when cancel button is clicked

          }

        });
    };


    // vm.ddRemoveCbcReport = function (event) {


    //   //var arry = [];
    //   var index = 0;
    //   var num = Number(event.target.value);
    //   var numOfCom = vm.jurisdictionsArr.length;

    //   if (num > 249) {
    //     swal("Error", "The maximum number to be captured is 249");
    //     vm.formData.CountryByCountryDeclaration.NoOfTaxJurisdictions = numOfCom;
    //     //return;
    //     num = 0;
    //   }
    //   if (num === 0) {

    //     messageservices.repeatRetirementMessage();
    //     vm.formData.CountryByCountryDeclaration.NoOfTaxJurisdictions = "1";
    //     num = 1;
    //   }


    //   var repeateTime = 0;

    //   if (Number(num) > numOfCom) {

    //     vm.updateNoOfConstituentEntities();
    //     if (numOfCom > 0) {
    //       repeateTime = num - vm.jurisdictionsArr.length
    //     } else {
    //       repeateTime = num;
    //     }

    //     for (index = 0; index < repeateTime; index++) {

    //       vm.jurisdictionsArr.push(vm.addnewObj());
    //       vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction.push(vm.addTaxJusObj());

    //     }

    //   }
    //   else {
    //     var diff = vm.jurisdictionsArr.length - Number(num);

    //     for (index; index < diff; index++) {
    //       vm.jurisdictionsArr.splice(vm.jurisdictionsArr.length - 1);
    //       // delete vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports-1;

    //       var arr = vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction;
    //       arr.splice(arr.length - 1);
    //     }
    //   }


    //   if (vm.jurisdictionsArr.length > 0) {
    //     if (angular.isUndefined(vm.formData.CountryByCountryDeclaration.CBC_OECD) || vm.formData.CountryByCountryDeclaration.CBC_OECD == null) {
    //       var objCB = {};
    //       vm.formData.CountryByCountryDeclaration.CBC_OECD = objCB;
    //       var objCbcBody = {};
    //       vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody = objCbcBody;
    //       var obj = {};
    //       vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports = obj;
    //     }

    //     if (vm.jurisdictionsArr.length > 1) {
    //       vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports = vm.jurisdictionsArr;
    //     } else {
    //       vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports = vm.jurisdictionsArr[0];
    //     }
    //   }

    // }

    vm.addRemoveCbcConstituent = function (event, jurisdictions, indx) {

      var index = 0;
      var num = Number(event.target.value);
      var numOfCom = jurisdictions.ConstEntities.length;


      if (num > 350) {
        swal("Error", "The maximum number to be captured is 350");
        vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[indx].NoOfConstituentEntities = numOfCom;
        //return;
        num = 0;

      }

      if (num === 0) {

        messageservices.repeatRetirementMessage();
        vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[indx].NoOfConstituentEntities = "1";
        num = 1;
      }


      var repeateTime = 0;

      if (Number(num) > numOfCom) {

        if (numOfCom > 0) {
          repeateTime = num - numOfCom;
          //  repeateTime = num - jurisdictions.ConstEntities.length;
        } else {
          repeateTime = num;
        }
        for (index = 0; index < repeateTime; index++) {
          jurisdictions.ConstEntities.push(vm.addNewConstituentObj());
        }
      }
      else {
        var diff = jurisdictions.ConstEntities.length - Number(num);
        for (index; index < diff; index++) {
          jurisdictions.ConstEntities.splice(jurisdictions.ConstEntities.length - 1);
        }
      }

      vm.$apply();

      if (jurisdictions.ConstEntities.length > 0) {

        if (angular.isUndefined(vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports.ConstEntities)
          || vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports.ConstEntities == null) {

          var objCons = {};
          vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports.ConstEntities = objCons;
        }

        if (jurisdictions.ConstEntities.length > 1) {
          vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports.ConstEntities = jurisdictions.ConstEntities;
        } else {
          vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports.ConstEntities = jurisdictions.ConstEntities[0];
        }
        vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports.ConstEntities.ConstEntity.Name = [];
        vm.initArray();
      }

    }

    vm.updateCbcReport = function () {


      var cbcReportList = [];
      cbcReportList = vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports;
      var taxJurisdictions = [];
      if (cbcReportList) {
        // if (angular.isUndefined(vm.formData.CountryByCountryDeclaration.TaxJurisdictions)) {
        //   vm.formData.CountryByCountryDeclaration.TaxJurisdictions = {};
        //   vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction = []; //[{'NoOfConstituentEntities':0}];
        //   vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[0].NoOfConstituentEntities = {};
        //   vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[0].NoOfConstituentEntities = 0;
        // }
        if (angular.isArray(cbcReportList)) {
          var n = cbcReportList.length;


          // Removed this for Impala Platinum previously captured data


          //vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction = new Array(n);
          for (var i = 0; i < n; i++) {
            var cbcReport = cbcReportList[i];
            // if (angular.isUndefined(vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[i])) {
            //   vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[i] = {};
            //   vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[i].NoOfConstituentEntities = {};
            //   vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[i].NoOfConstituentEntities = 0;
            // }
            if (angular.isUndefined(cbcReport.ConstEntities) || cbcReport.ConstEntities == null) {
              cbcReport.ConstEntities = [
                {
                  "ConstEntity":
                    {
                      "Name": [null]
                    },
                  "BizActivities": [null]
                }

              ];
              //cbcReport.isValid = false;
            }
            // Check ProfitOrLoss
            if (angular.isDefined(cbcReport.Summary.ProfitOrLoss) && cbcReport.Summary.ProfitOrLoss !== null) {
              if (!angular.isObject(cbcReport.Summary.ProfitOrLoss)) {
                var fieldValue = cbcReport.Summary.ProfitOrLoss;
                cbcReport.Summary.ProfitOrLoss = {};
                if (vm.isCurrencyCode(fieldValue)) {
                  cbcReport.Summary.ProfitOrLoss['@currCode'] = fieldValue;
                }
                else {
                  cbcReport.Summary.ProfitOrLoss['#text'] = fieldValue;
                }
              }
            }
            // Check Capital
            if (angular.isDefined(cbcReport.Summary.Capital) && cbcReport.Summary.Capital !== null) {
              if (!angular.isObject(cbcReport.Summary.Capital)) {
                var fieldValue = cbcReport.Summary.Capital;
                cbcReport.Summary.Capital = {};
                if (vm.isCurrencyCode(fieldValue)) {
                  cbcReport.Summary.Capital['@currCode'] = fieldValue;
                }
                else {
                  cbcReport.Summary.Capital['#text'] = fieldValue;
                }
              }
            }
            // Check TaxPaid
            if (angular.isDefined(cbcReport.Summary.TaxPaid) && cbcReport.Summary.TaxPaid !== null) {
              if (!angular.isObject(cbcReport.Summary.TaxPaid)) {
                var fieldValue = cbcReport.Summary.TaxPaid;
                cbcReport.Summary.TaxPaid = {};
                if (vm.isCurrencyCode(fieldValue)) {
                  cbcReport.Summary.TaxPaid['@currCode'] = fieldValue;
                }
                else {
                  cbcReport.Summary.TaxPaid['#text'] = fieldValue;
                }
              }
            }
            // Check Earnings
            if (angular.isDefined(cbcReport.Summary.Earnings) && cbcReport.Summary.Earnings !== null) {
              if (!angular.isObject(cbcReport.Summary.Earnings)) {
                var fieldValue = cbcReport.Summary.Earnings;
                cbcReport.Summary.Earnings = {};
                if (vm.isCurrencyCode(fieldValue)) {
                  cbcReport.Summary.Earnings['@currCode'] = fieldValue;
                }
                else {
                  cbcReport.Summary.Earnings['#text'] = fieldValue;
                }
              }
            }
            // Check TaxAccrued
            if (angular.isDefined(cbcReport.Summary.TaxAccrued) && cbcReport.Summary.TaxAccrued !== null) {
              if (!angular.isObject(cbcReport.Summary.TaxAccrued)) {
                var fieldValue = cbcReport.Summary.TaxAccrued;
                cbcReport.Summary.TaxAccrued = {};
                if (vm.isCurrencyCode(fieldValue)) {
                  cbcReport.Summary.TaxAccrued['@currCode'] = fieldValue;
                }
                else {
                  cbcReport.Summary.TaxAccrued['#text'] = fieldValue;
                }
              }
            }
            // Check Assets
            if (angular.isDefined(cbcReport.Summary.Assets) && cbcReport.Summary.Assets !== null) {
              if (!angular.isObject(cbcReport.Summary.Assets)) {
                var fieldValue = cbcReport.Summary.Assets;
                cbcReport.Summary.Assets = {};
                if (vm.isCurrencyCode(fieldValue)) {
                  cbcReport.Summary.Assets['@currCode'] = fieldValue;
                }
                else {
                  cbcReport.Summary.Assets['#text'] = fieldValue;
                }
              }
            }
            // Check Revenues.Unrelated
            if (angular.isDefined(cbcReport.Summary.Revenues.Unrelated) && cbcReport.Summary.Revenues.Unrelated !== null) {
              if (!angular.isObject(cbcReport.Summary.Revenues.Unrelated)) {
                var fieldValue = cbcReport.Summary.Revenues.Unrelated;
                cbcReport.Summary.Revenues.Unrelated = {};
                if (vm.isCurrencyCode(fieldValue)) {
                  cbcReport.Summary.Revenues.Unrelated['@currCode'] = fieldValue;
                }
                else {
                  cbcReport.Summary.Revenues.Unrelated['#text'] = fieldValue;
                }
              }
            }
            // Check Revenues.Related
            if (angular.isDefined(cbcReport.Summary.Revenues.Related) && cbcReport.Summary.Revenues.Related !== null) {
              if (!angular.isObject(cbcReport.Summary.Revenues.Related)) {
                var fieldValue = cbcReport.Summary.Revenues.Related;
                cbcReport.Summary.Revenues.Related = {};
                if (vm.isCurrencyCode(fieldValue)) {
                  cbcReport.Summary.Revenues.Related['@currCode'] = fieldValue;
                }
                else {
                  cbcReport.Summary.Revenues.Related['#text'] = fieldValue;
                }
              }
            }
            // Check Revenues.Total
            if (angular.isDefined(cbcReport.Summary.Revenues.Total) && cbcReport.Summary.Revenues.Total !== null) {
              if (!angular.isObject(cbcReport.Summary.Revenues.Total)) {
                var fieldValue = cbcReport.Summary.Revenues.Total;
                cbcReport.Summary.Revenues.Total = {};
                if (vm.isCurrencyCode(fieldValue)) {
                  cbcReport.Summary.Revenues.Total['@currCode'] = fieldValue;
                }
                else {
                  cbcReport.Summary.Revenues.Total['#text'] = fieldValue;
                }
              }
            }

            // Check the Docspec
            if (angular.isDefined(cbcReport.DocSpec.DocTypeIndic) && cbcReport.DocSpec.DocTypeIndic !== null) {
              if (!angular.isObject(cbcReport.DocSpec.DocTypeIndic)) {
                var fieldValue = cbcReport.DocSpec.DocTypeIndic;
                cbcReport.DocSpec.DocTypeIndic = {};
                if (fieldValue.length < 6) {
                  cbcReport.DocSpec.DocTypeIndic['#text'] = fieldValue;
                }
                else {
                  cbcReport.DocSpec.DocTypeIndic['#text'] = "OECD1";
                }
              }

            }

            /* needed to update the Constituent Entities */
            vm.updateconstEntities(cbcReport);
            vm.jurisdictionsArr.push(cbcReport);
            // this is to make sure that the array is updated
            vm.$apply();
          }

        }
        else if (angular.isObject(cbcReportList)) {

          vm.cbcReportList = vm.updateconstEntities(cbcReportList);
          // Check ProfitOrLoss
          if (angular.isDefined(cbcReportList.Summary.ProfitOrLoss) && cbcReportList.Summary.ProfitOrLoss !== null) {
            if (!angular.isObject(cbcReportList.Summary.ProfitOrLoss)) {
              var fieldValue = cbcReportList.Summary.ProfitOrLoss;
              cbcReportList.Summary.ProfitOrLoss = {};
              if (vm.isCurrencyCode(fieldValue)) {
                cbcReportList.Summary.ProfitOrLoss['@currCode'] = fieldValue;
              }
              else {
                cbcReportList.Summary.ProfitOrLoss['#text'] = fieldValue;
              }
            }
          }
          // Check Capital
          if (angular.isDefined(cbcReportList.Summary.Capital) && cbcReportList.Summary.Capital !== null) {
            if (!angular.isObject(cbcReportList.Summary.Capital)) {
              var fieldValue = cbcReportList.Summary.Capital;
              cbcReportList.Summary.Capital = {};
              if (vm.isCurrencyCode(fieldValue)) {
                cbcReportList.Summary.Capital['@currCode'] = fieldValue;
              }
              else {
                cbcReportList.Summary.Capital['#text'] = fieldValue;
              }
            }
          }
          // Check TaxPaid
          if (angular.isDefined(cbcReportList.Summary.TaxPaid) && cbcReportList.Summary.TaxPaid !== null) {
            if (!angular.isObject(cbcReportList.Summary.TaxPaid)) {
              var fieldValue = cbcReportList.Summary.TaxPaid;
              cbcReportList.Summary.TaxPaid = {};
              if (vm.isCurrencyCode(fieldValue)) {
                cbcReportList.Summary.TaxPaid['@currCode'] = fieldValue;
              }
              else {
                cbcReportList.Summary.TaxPaid['#text'] = fieldValue;
              }
            }
          }
          // Check Earnings
          if (angular.isDefined(cbcReportList.Summary.Earnings) && cbcReportList.Summary.Earnings !== null) {
            if (!angular.isObject(cbcReportList.Summary.Earnings)) {
              var fieldValue = cbcReportList.Summary.Earnings;
              cbcReportList.Summary.Earnings = {};
              if (vm.isCurrencyCode(fieldValue)) {
                cbcReportList.Summary.Earnings['@currCode'] = fieldValue;
              }
              else {
                cbcReportList.Summary.Earnings['#text'] = fieldValue;
              }
            }
          }
          // Check TaxAccrued
          if (angular.isDefined(cbcReportList.Summary.TaxAccrued) && cbcReportList.Summary.TaxAccrued !== null) {
            if (!angular.isObject(cbcReportList.Summary.TaxAccrued)) {
              var fieldValue = cbcReportList.Summary.TaxAccrued;
              cbcReportList.Summary.TaxAccrued = {};
              if (vm.isCurrencyCode(fieldValue)) {
                cbcReportList.Summary.TaxAccrued['@currCode'] = fieldValue;
              }
              else {
                cbcReportList.Summary.TaxAccrued['#text'] = fieldValue;
              }
            }
          }
          // Check Assets
          if (angular.isDefined(cbcReportList.Summary.Assets) && cbcReportList.Summary.Assets !== null) {
            if (!angular.isObject(cbcReportList.Summary.Assets)) {
              var fieldValue = cbcReportList.Summary.Assets;
              cbcReportList.Summary.Assets = {};
              if (vm.isCurrencyCode(fieldValue)) {
                cbcReportList.Summary.Assets['@currCode'] = fieldValue;
              }
              else {
                cbcReportList.Summary.Assets['#text'] = fieldValue;
              }
            }
          }
          // Check Revenues.Unrelated
          if (angular.isDefined(cbcReportList.Summary.Revenues.Unrelated) && cbcReportList.Summary.Revenues.Unrelated !== null) {
            if (!angular.isObject(cbcReportList.Summary.Revenues.Unrelated)) {
              var fieldValue = cbcReportList.Summary.Revenues.Unrelated;
              cbcReportList.Summary.Revenues.Unrelated = {};
              if (vm.isCurrencyCode(fieldValue)) {
                cbcReportList.Summary.Revenues.Unrelated['@currCode'] = fieldValue;
              }
              else {
                cbcReportList.Summary.Revenues.Unrelated['#text'] = fieldValue;
              }
            }
          }
          // Check Revenues.Related
          if (angular.isDefined(cbcReportList.Summary.Revenues.Related) && cbcReportList.Summary.Revenues.Related !== null) {
            if (!angular.isObject(cbcReportList.Summary.Revenues.Related)) {
              var fieldValue = cbcReportList.Summary.Revenues.Related;
              cbcReportList.Summary.Revenues.Related = {};
              if (vm.isCurrencyCode(fieldValue)) {
                cbcReportList.Summary.Revenues.Related['@currCode'] = fieldValue;
              }
              else {
                cbcReportList.Summary.Revenues.Related['#text'] = fieldValue;
              }
            }
          }
          // Check Revenues.Total
          if (angular.isDefined(cbcReportList.Summary.Revenues.Total) && cbcReportList.Summary.Revenues.Total !== null) {
            if (!angular.isObject(cbcReportList.Summary.Revenues.Total)) {
              var fieldValue = cbcReportList.Summary.Revenues.Total;
              cbcReportList.Summary.Revenues.Total = {};
              if (vm.isCurrencyCode(fieldValue)) {
                cbcReportList.Summary.Revenues.Total['@currCode'] = fieldValue;
              }
              else {
                cbcReportList.Summary.Revenues.Total['#text'] = fieldValue;
              }
            }
          }

          // Check the Docspec
          if (angular.isDefined(cbcReportList.DocSpec.DocTypeIndic) && cbcReportList.DocSpec.DocTypeIndic !== null) {
            if (!angular.isObject(cbcReportList.DocSpec.DocTypeIndic)) {
              var fieldValue = cbcReportList.DocSpec.DocTypeIndic;
              cbcReportList.DocSpec.DocTypeIndic = {};
              if (fieldValue.length < 6) {
                cbcReportList.DocSpec.DocTypeIndic['#text'] = fieldValue;
              }
              else {
                cbcReportList.DocSpec.DocTypeIndic['#text'] = "OECD1";
              }
            }

          }
          vm.jurisdictionsArr.push(cbcReportList);


          // this is to make sure that the array is updated
          vm.$apply();
        }


      }
      else {
        vm.showreportDiv = true;
        //   vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports = {};

        //   vm.jurisdictionsArr = [
        //     {
        //       "ConstEntities": [

        //         {
        //           "ConstEntity":
        //             {
        //               "Name": [null]
        //             },
        //           "BizActivities": [null]
        //         }

        //       ]

        //     }
        //   ]
        //   vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports = vm.jurisdictionsArr;

      }
      vm.updateNoOfConstituentEntities();
      //swal.close();
    }

    vm.isCurrencyCode = function (fieldValue) {

      if (fieldValue && fieldValue !== null) {

        if (fieldValue.length === 3 && !angular.isNumber(fieldValue)) {
          return true;
        }
        else {

          return false;
        }

      }
      return false;
    }

    vm.isIssuedByCode = function (fieldValue) {

      if (fieldValue && fieldValue !== null) {

        if (fieldValue.length === 2 && !angular.isNumber(fieldValue)) {
          return true;
        }
        else {

          return false;
        }

      }
      return false;
    }

    vm.updateconstEntities = function (cbcReport) {
      var constEntitiesList = [];

      if (angular.isDefined(cbcReport.ConstEntities) && cbcReport.ConstEntities !== null) {
        constEntitiesList = cbcReport.ConstEntities;
        if (angular.isArray(constEntitiesList)) {
          var c = constEntitiesList.length;
          for (var i = 0; i < c; i++) {
            var nameArr = [];
            var constituent = constEntitiesList[i];
            if (angular.isDefined(constituent) && (constituent !== null)) {
              if (angular.isDefined(constituent.ConstEntity) && (constituent.ConstEntity !== null)) {//} && angular.isDefined(constEntiti.ConstEntity.Name)) {
                // Check Name
                if (angular.isDefined(constituent.ConstEntity.Name) && (constituent.ConstEntity.Name !== null)) {
                  if (!angular.isArray(constituent.ConstEntity.Name)) {
                    var nameValue = constituent.ConstEntity.Name;
                    nameArr.push(nameValue);
                    constituent.ConstEntity.Name = nameArr;
                  }
                }
                if (angular.isDefined(constituent.ConstEntity.IN) && constituent.ConstEntity.IN !== null) {
                  if (!angular.isObject(constituent.ConstEntity.IN)) {
                    var fieldValue = constituent.ConstEntity.IN;
                    constituent.ConstEntity.IN = {};
                    if (vm.isIssuedByCode(fieldValue)) {
                      constituent.ConstEntity.IN['@issuedBy'] = fieldValue;
                    }
                    else {
                      constituent.ConstEntity.IN['#text'] = fieldValue;
                    }
                  }
                }
                if (angular.isDefined(constituent.ConstEntity.TIN) && constituent.ConstEntity.TIN !== null) {
                  if (!angular.isObject(constituent.ConstEntity.TIN)) {
                    var fieldValue = constituent.ConstEntity.TIN;
                    constituent.ConstEntity.TIN = {};
                    if (vm.isIssuedByCode(fieldValue)) {
                      constituent.ConstEntity.TIN['@issuedBy'] = fieldValue;
                    }
                    else {
                      constituent.ConstEntity.TIN['#text'] = fieldValue;
                    }
                  }
                }
                if (angular.isDefined(constituent.ConstEntity.Address) && constituent.ConstEntity.Address !== null) {
                  if (!angular.isObject(constituent.ConstEntity.Address)) {
                    var fieldValue = constituent.ConstEntity.Address;
                    constituent.ConstEntity.Address = {};
                    if (vm.isIssuedByCode(fieldValue)) {
                      constituent.ConstEntity.Address['CountryCode'] = fieldValue;
                      constituent.ConstEntity.Address['AddressFree'] = fieldValue;
                    }
                    else {
                      constituent.ConstEntity.Address['@legalAddressType'] = fieldValue;
                    }
                  }
                }

              }
              else if (constituent.ConstEntity == null) {
                constituent.ConstEntity = {};
              }
            }
            else if (constituent == null) {
              constituent = {
                "ConstEntity":
                  {
                    "Name": []
                  },
                "BizActivities": [null]
              };

            }



            if (angular.isDefined(constituent) && (constituent !== null)) {
              constituent = vm.updateMainBusinessAc(constituent);
            }


          }

        }
        else if (angular.isObject(constEntitiesList)) {
          var constEntitiesArr = [];
          var nameArr = [];
          if (angular.isDefined(constEntitiesList.ConstEntity) && angular.isDefined(constEntitiesList.ConstEntity.Name)) {
            if (!angular.isArray(constEntitiesList.ConstEntity.Name)) {
              var nameValue = constEntitiesList.ConstEntity.Name;
              nameArr.push(nameValue);
              constEntitiesList.ConstEntity.Name = nameArr;
            }
            if (angular.isDefined(constEntitiesList.ConstEntity.TIN) && constEntitiesList.ConstEntity.TIN !== null) {
              if (!angular.isObject(constEntitiesList.ConstEntity.TIN)) {
                var fieldValue = constEntitiesList.ConstEntity.TIN;
                constEntitiesList.ConstEntity.TIN = {};
                if (vm.isIssuedByCode(fieldValue)) {
                  constEntitiesList.ConstEntity.TIN['@issuedBy'] = fieldValue;
                }
                else {
                  constEntitiesList.ConstEntity.TIN['#text'] = fieldValue;
                }
              }
            }
            if (angular.isDefined(constEntitiesList.ConstEntity.IN) && constEntitiesList.ConstEntity.IN !== null) {
              if (!angular.isObject(constEntitiesList.ConstEntity.IN)) {
                var fieldValue = constEntitiesList.ConstEntity.IN;
                constEntitiesList.ConstEntity.IN = {};
                if (vm.isIssuedByCode(fieldValue)) {
                  constEntitiesList.ConstEntity.IN['@issuedBy'] = fieldValue;
                }
                else {
                  constEntitiesList.ConstEntity.IN['#text'] = fieldValue;
                }
              }
            }

            if (angular.isDefined(constEntitiesList.ConstEntity.Address) && constEntitiesList.ConstEntity.Address !== null) {
              if (!angular.isObject(constEntitiesList.ConstEntity.Address)) {
                var fieldValue = constEntitiesList.ConstEntity.Address;
                constEntitiesList.ConstEntity.Address = {};
                if (vm.isIssuedByCode(fieldValue)) {
                  constEntitiesList.ConstEntity.Address['CountryCode'] = fieldValue;
                  constEntitiesList.ConstEntity.Address['AddressFree'] = fieldValue;
                }
                else {
                  constEntitiesList.ConstEntity.Address['@legalAddressType'] = fieldValue;
                }
              }
            }

          }


          constEntitiesArr.push(constEntitiesList);
          cbcReport.ConstEntities = constEntitiesArr;
          vm.updateMainBusinessAc(constEntitiesList);
        }
      }
      return cbcReport;
    };


    vm.updateMainBusinessAc = function (constEntiti) {
      var busAccList = [];
      if (angular.isDefined(constEntiti) && constEntiti !== null) {
        busAccList = constEntiti.BizActivities;
        if (busAccList && busAccList !== null) {
          if (angular.isArray(busAccList)) {

            var n = busAccList.length;
            for (var i = 0; i < n; i++) {
              if (busAccList[i] === 'CBC513') {
                if (angular.isUndefined(constEntiti.OtherEntityInfo) || constEntiti.OtherEntityInfo == null) {
                  vm.otherBusinessActivityIsRequired = true;
                }
              }

            }
            // do nothig 
            return;
          }
          else {
            var mainBusinessArr = [];
            mainBusinessArr.push(busAccList);
            constEntiti.BizActivities = mainBusinessArr;
          }
        }
        else {
          constEntiti.BizActivities = [null];
        }
      }

      return constEntiti;
    }


    vm.addmainBusinessAc = function (constituent) {

      if (constituent.BizActivities.length < 13) {

        constituent.BizActivities.push(vm.addmainBusinessAcObj());
      }
      else {
        swal("Error", "The maximum number to be captured is 13");
      }



    }

    vm.removeMainBusinessAc = function (constituent, index) {

      if (constituent.BizActivities.length > 1) {
        constituent.BizActivities.splice(index, 1);

        var n = constituent.BizActivities.length;

        for (var i = 0; i < n; i++) {
          if (constituent.BizActivities[i] === 'CBC513') {
            vm.otherBusinessActivityIsRequired = true;
            return;
          }
          else {

            vm.otherBusinessActivityIsRequired = false;

          }

        }
      }

    }

    vm.addOtherInforMAtionCon = function (e) {
      if (e) {
        e.preventDefault();
        e.stopPropagation();
      }
      vm.myPromiseOtherInforMAtion = $timeout(function () {

        // var blockOtherInforMAtion = blockUI.instances.get('blockOtherInforMAtion');
        // blockOtherInforMAtion.start();

        // $timeout(function() {
        //blockOtherInforMAtion.stop();
        // }, 4000);

        vm.otherInforArr.push(vm.addadditionalInfor());

        if (angular.isUndefined(vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo)
          || vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo == null) {

          vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo = {

          };

        }

        if (vm.otherInforArr.length > 1) {
          vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo = vm.otherInforArr;

        } else {
          vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo = vm.otherInforArr[0];
        }



      }, 4000)
        .then(function () {

        }).then(function () {

        }).then(function () {

        });
      return vm.myPromiseOtherInforMAtion;

    }

    vm.removeOtherInforMAtionCon = function (index) {

      if (vm.otherInforArr.length > 1) {
        vm.otherInforArr.splice(index, 1);
      }
      vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo = vm.otherInforArr;

    }

    vm.updateAdditionalInfor = function () {
      var additionalInforArrs = []

      if (angular.isUndefined(vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo)
        || vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo == null) {

        vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo = {};

      }
      additionalInforArrs = vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo;

      if (additionalInforArrs) {
        if (angular.isArray(additionalInforArrs)) {
          var a = additionalInforArrs.length;
          for (var i = 0; i < a; i++) {
            var additionalInforArr = additionalInforArrs[i];
            if (!angular.isObject(additionalInforArr.ResCountryCode)) {
              vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo[i].ResCountryCode = [additionalInforArr.ResCountryCode];
            }
            if (!angular.isObject(additionalInforArr.DocSpec.DocTypeIndic)) {
              var fieldValue = additionalInforArr.DocSpec.DocTypeIndic;
              additionalInforArr.DocSpec.DocTypeIndic = {};

              if (fieldValue < 6) {
                additionalInforArr.DocSpec.DocTypeIndic["#text"] = fieldValue;
              }
              else {
                additionalInforArr.DocSpec.DocTypeIndic["#text"] = "OECD1";

              }

            }

            if (!angular.isObject(additionalInforArr.SummaryRef)) {
              vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo[i].SummaryRef = [additionalInforArr.SummaryRef];
            }
            vm.otherInforArr.push(additionalInforArr);

          }
        }
        else if (angular.isObject(additionalInforArrs)) {
          vm.otherInforArr.push(additionalInforArrs);
          vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo = vm.otherInforArr;
          if (!angular.isObject(additionalInforArrs.ResCountryCode)) {
            vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo[0].ResCountryCode = [additionalInforArrs.ResCountryCode];
          }

          if (!angular.isObject(additionalInforArrs.SummaryRef)) {
            vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo[0].SummaryRef = [additionalInforArrs.SummaryRef];
          }

        }
      }

    }

    vm.changeIntoAnArr = function () {
      try {

        var InArrs = []
        if (angular.isUndefined(vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity) || vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity == null) {
          if (angular.isUndefined(vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.IN)
            || vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.IN == null) {

            vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.IN = {};

            InArrs = vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.IN;
            if (InArrs) {
              if (angular.isArray(InArrs)) {
                return;
              } else if (angular.isObject(InArrs)) {
                var newArr = [];
                newArr.push(InArrs);
                vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.IN = newArr;

              }
            }

          }

        }


      } catch (error) {

      }


    }


    vm.addResidentCountry = function (res) {
      res.ResCountryCode.push(null);

    }

    vm.RemoveResidentCountry = function (res, index) {

      if (res.ResCountryCode.length > 1) {
        res.ResCountryCode.splice(index, 1);
      }

    }


    vm.addSummaryRef = function (sum) {
      sum.SummaryRef.push(null);

    }

    vm.RemoveRSummaryRef = function (sum, index) {

      if (sum.SummaryRef.length > 1) {
        sum.SummaryRef.splice(index, 1);
      }

    }



    rs.getAngularData = function () {

      if (angular.isDefined(vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.IN[1]['#text']) && vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.IN[1]['#text'] != null) {
        vm.prepopValue = "US";
        vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.IN[1]['@issuedBy'] = vm.prepopValue;

      }

      if (vm.jurisdictionsArr.length < 1) {
        if (vm.DefaultCBCReport.length == 1 && vm.showreportDiv == true) {
          if (vm.constituentArr.length >= 0 && vm.constituentArr.length < 351) {
            if (vm.DefaultCBCReport[0].ConstEntities[0] && vm.DefaultCBCReport[0].ConstEntities.length) {
              vm.constituentArr.push(vm.DefaultCBCReport[0].ConstEntities[0]);
            }
            // vm.constituentArr.push(vm.DefaultCBCReport[0].ConstEntities[0]);
            vm.DefaultCBCReport[0].ConstEntities = angular.copy(vm.constituentArr);

          }
          vm.jurisdictionsArr.push(vm.DefaultCBCReport[0]);
          noOfReports = vm.jurisdictionsArr.length;
          vm.formData.CountryByCountryDeclaration.NoOfTaxJurisdictions = noOfReports;

          if (angular.isDefined(vm.formData.CountryByCountryDeclaration.TaxJurisdictions) && (vm.formData.CountryByCountryDeclaration.TaxJurisdictions !== null)) {
            if (angular.isUndefined(vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[noOfReports - 1])) {
              vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[noOfReports - 1] = {};
              vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[noOfReports - 1].NoOfConstituentEntities = {};
              vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[noOfReports - 1].NoOfConstituentEntities = vm.constituentArr.length;
            }
          }

          vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports = vm.jurisdictionsArr;

        }
      }
      else {
        if (vm.DefaultCBCReport.length == 1 && vm.showreportDiv == true && vm.reportid == null) {

          if (vm.constituentArr.length >= 0 && vm.constituentArr.length < 351) {

            if (vm.DefaultCBCReport[0].ConstEntities[0] && vm.DefaultCBCReport[0].ConstEntities.length) {
              vm.constituentArr.push(vm.DefaultCBCReport[0].ConstEntities[0]);
            }

            vm.DefaultCBCReport[0].ConstEntities = angular.copy(vm.constituentArr);

          }

          var valid = angular.element(CBCReportFormDiv).scope().CBCReportFormValidWithoutMessages();
          if (valid) {
            vm.jurisdictionsArr.push(vm.DefaultCBCReport[0]);
          }

          noOfReports = vm.jurisdictionsArr.length;
          vm.formData.CountryByCountryDeclaration.NoOfTaxJurisdictions = noOfReports;

          if (angular.isDefined(vm.formData.CountryByCountryDeclaration.TaxJurisdictions) && (vm.formData.CountryByCountryDeclaration.TaxJurisdictions !== null)) {
            if (angular.isUndefined(vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[noOfReports - 1])) {
              vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[noOfReports - 1] = {};
              vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[noOfReports - 1].NoOfConstituentEntities = {};
              vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[noOfReports - 1].NoOfConstituentEntities = vm.DefaultCBCReport[0].ConstEntities.length;
            }

          }
          vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports = vm.jurisdictionsArr;

        }
        else {
          if (vm.constituentArr.length >= 0 && vm.constituentArr.length < 351) {

            if (vm.constEntityId == undefined) {

              if (vm.DefaultCBCReport[0].ConstEntities[0] && vm.DefaultCBCReport[0].ConstEntities.length) {
                vm.constituentArr.push(vm.DefaultCBCReport[0].ConstEntities[0]);
              }
            }

            else {
              vm.constituentArr[vm.constEntityId] = angular.copy(vm.DefaultCBCReport[0].ConstEntities[0]);
            }
            vm.DefaultCBCReport[0].ConstEntities = angular.copy(vm.constituentArr);
          }

          vm.jurisdictionsArr[vm.reportid] = angular.copy(vm.DefaultCBCReport[0]);
          vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports = vm.jurisdictionsArr;
        }
      }
      vm.showreportDiv = false;




      /** add DocSpec */


      var b = vm.jurisdictionsArr.length;
      if (angular.isUndefined(vm.formData.CountryByCountryDeclaration.NoOfTaxJurisdictions)) {
        vm.formData.CountryByCountryDeclaration.NoOfTaxJurisdictions = {};
      }
      vm.formData.CountryByCountryDeclaration.NoOfTaxJurisdictions = b;


      if (b > 0) {
        for (var i = 0; i < b; i++) {
          jurisdiction = vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports[i];
          if (angular.isUndefined(jurisdiction.DocSpec)) {
            vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports[i].DocSpec = {};
            vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports[i].DocSpec.DocTypeIndic = {};
            vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports[i].DocSpec.DocTypeIndic['#text'] = "OECD1";

          }
          else {

            if (vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports[i].DocSpec.DocTypeIndic['#text'] == "" || angular.isUndefined(vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports[i].DocSpec.DocTypeIndic['#text'])) {
              vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports[i].DocSpec.DocTypeIndic['#text'] = "OECD1";

            }
          }

          if (angular.isUndefined(vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[i]) || vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[i] == null) {
            vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[i] = {}
            if (angular.isUndefined(vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[i].NoOfConstituentEntities)) {
              vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[i].NoOfConstituentEntities = {};
            }
          }

          if (angular.isDefined(vm.jurisdictionsArr[i].ConstEntities)) {

            vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction[i].NoOfConstituentEntities = vm.jurisdictionsArr[i].ConstEntities.length;
          }

          vm.$apply();
        }
      }



      if (vm.formData.CountryByCountryDeclaration.DeclarationDate != null || vm.formData.CountryByCountryDeclaration.DeclarationDate != undefined) {
        var declerationDate = vm.formData.CountryByCountryDeclaration.DeclarationDate;
        var declerationDateFullDate = moment(declerationDate, 'YYYYMMDD').format('YYYY-MM-DD');


        if (moment(declerationDateFullDate, 'YYYY-MM-DD').isValid()) {

          vm.formData.CountryByCountryDeclaration.DeclarationDate = declerationDateFullDate;
        }

      }

      if (vm.formData.CountryByCountryDeclaration.CBC_OECD.MessageSpec != null || vm.formData.CountryByCountryDeclaration.CBC_OECD.MessageSpec != undefined) {
        vm.formData.CountryByCountryDeclaration.CBC_OECD.MessageSpec.TransmittingCountry = "ZA";
        vm.formData.CountryByCountryDeclaration.CBC_OECD.MessageSpec.ReceivingCountry = "ZA";
        vm.formData.CountryByCountryDeclaration.CBC_OECD.MessageSpec.MessageType = "CBC";

        /*Sending entity In was defualted to SARS before now to the Entity Name*/
        vm.formData.CountryByCountryDeclaration.CBC_OECD.MessageSpec.SendingEntityIN = vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.Name[0];

        vm.formData.CountryByCountryDeclaration.CBC_OECD.MessageSpec.Language = "EN";
        vm.formData.CountryByCountryDeclaration.CBC_OECD.MessageSpec.Warning = "None";
        vm.formData.CountryByCountryDeclaration.CBC_OECD.MessageSpec.Contact = "SARS";

        if (vm.formData.CountryByCountryDeclaration.CBC_OECD.MessageSpec.ReportingPeriod != null || vm.formData.CountryByCountryDeclaration.CBC_OECD.MessageSpec.ReportingPeriod != undefined) {
          var reportingPeriodDate = vm.formData.CountryByCountryDeclaration.CBC_OECD.MessageSpec.ReportingPeriod;
          var periodFullDate = moment(reportingPeriodDate, 'YYYYMMDD').format('YYYY-MM-DD');


          if (moment(periodFullDate, 'YYYY-MM-DD').isValid()) {

            vm.formData.CountryByCountryDeclaration.CBC_OECD.MessageSpec.ReportingPeriod = periodFullDate;
          }
          else {
            vm.formData.CountryByCountryDeclaration.CBC_OECD.MessageSpec.ReportingPeriod = "";
            return;
          }

          if (angular.isUndefined(vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.DocSpec.DocRefId) ||
            vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.DocSpec.DocRefId === null) {
            vm.formData.CountryByCountryDeclaration.CBC_OECD.MessageSpec.MessageTypeIndic = "CBC401";

          }
          else {
            if (vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.DocSpec.DocTypeIndic['#text'] == "") {
              vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.DocSpec.DocTypeIndic['#text'] = "OECD1";

            }
            vm.formData.CountryByCountryDeclaration.CBC_OECD.MessageSpec.MessageTypeIndic = "CBC402";

          }


        }

      }
      /*
      check Docspec indicator is captured after correction
      
      */
      var noOfAdditionalCont = vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo.length;
      if (noOfAdditionalCont > 0) {
        for (var i = 0; i < noOfAdditionalCont; i++) {
          additionaInfo = vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo[i];
          if (angular.isUndefined(additionaInfo.DocSpec)) {
            vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo[i].DocSpec = {};
            vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo[i].DocSpec.DocTypeIndic = {};
            vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo[i].DocSpec.DocTypeIndic['#text'] = "OECD1";
          }
          else {
            if (vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo[i].DocSpec.DocTypeIndic['#text'] == "" || angular.isUndefined(vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo[i].DocSpec.DocTypeIndic['#text'])) {
              vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo[i].DocSpec.DocTypeIndic['#text'] = "OECD1";

            }


          }
        }
      }

      vm.$apply();

      return dataService.getData(vm.formData);

    }


    vm.checkCompanyReg = function (event) {

      var companyReg = event.target.value;
      if (companyReg != "") {
        if (!companyregservice.validateCompanyRegNo(companyReg)) {
          event.target.value = "";
          alert("Number Not Correct");
          return;
        }

      }

    }


    rs.setFormData = function (data, readOnly, prepop) {

      vm.updateOnloadinitArray();
      var json = angular.fromJson(data);


      vm.formData = json;
      vm.arrCreationForEntityName();
      vm.arrCreationForEntityIn();
      if (vm.formData.CountryByCountryDeclaration.CBC_OECD.MessageSpec != null || vm.formData.CountryByCountryDeclaration.CBC_OECD.MessageSpec != undefined) {
        if (vm.formData.CountryByCountryDeclaration.CBC_OECD.MessageSpec.ReportingPeriod != null || vm.formData.CountryByCountryDeclaration.CBC_OECD.MessageSpec.ReportingPeriod != undefined) {

          if (angular.isObject(vm.formData.CountryByCountryDeclaration.CBC_OECD.MessageSpec.ReportingPeriod)) {

            vm.formData.CountryByCountryDeclaration.CBC_OECD.MessageSpec.ReportingPeriod = vm.formData.CountryByCountryDeclaration.CBC_OECD.MessageSpec.ReportingPeriod['#text'];
          }

          var formReportingPeriod = vm.formData.CountryByCountryDeclaration.CBC_OECD.MessageSpec.ReportingPeriod;
          if (vm.formData.CountryByCountryDeclaration.CBC_OECD.MessageSpec.ReportingPeriod.length == 10) {
            var reportingdate = moment(formReportingPeriod, 'YYYY-MM-DD').format('YYYYMMDD');


            if (moment(reportingdate, 'YYYYMMDD').isValid()) {

              vm.formData.CountryByCountryDeclaration.CBC_OECD.MessageSpec.ReportingPeriod = reportingdate;
            }
            else {
              vm.formData.CountryByCountryDeclaration.CBC_OECD.MessageSpec.ReportingPeriod = "";

            }


          }
          else {


            if (moment(formReportingPeriod, 'YYYYMMDD').isValid()) {

              var reportingdate = moment(formReportingPeriod, 'YYYY-MM-DD').format('YYYYMMDD');

              vm.formData.CountryByCountryDeclaration.CBC_OECD.MessageSpec.ReportingPeriod = reportingdate;


            }
            else {

              swal("Error", "The Reporting Period (CCYYMMDD) you have entered does not seem to be valid. Please ensure that it is correct.<br/>HINTS:<br/>  Format:CCYYMMDD. <br/> Only numeric digits may be used.");
              vm.formData.CountryByCountryDeclaration.CBC_OECD.MessageSpec.ReportingPeriod = "";
            }
          }

        }
      }



      if (vm.formData.CountryByCountryDeclaration.DeclarationDate != null || vm.formData.CountryByCountryDeclaration.DeclarationDate != undefined) {
        var formDeclarationDate = vm.formData.CountryByCountryDeclaration.DeclarationDate;
        if (vm.formData.CountryByCountryDeclaration.DeclarationDate.length == 10) {
          var Declarationdate = moment(formDeclarationDate, 'YYYY-MM-DD').format('YYYYMMDD');
          vm.formData.CountryByCountryDeclaration.DeclarationDate = Declarationdate;

        }
        else {


          if (moment(formDeclarationDate, 'YYYYMMDD').isValid()) {

            var Declarationdate = moment(formDeclarationDate, 'YYYY-MM-DD').format('YYYYMMDD');
            vm.formData.CountryByCountryDeclaration.DeclarationDate = Declarationdate;

          }
          else {
            swal("Error", "The Declaration Date (CCYYMMDD) you have entered does not seem to be valid. Please ensure that it is correct.<br/>HINTS:<br/>  Format:CCYYMMDD. <br/> Only numeric digits may be used.");
            vm.formData.CountryByCountryDeclaration.DeclarationDate = "";
          }
        }

      }


      if (angular.isDefined(vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.TIN) && vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.TIN !== null) {
        if (!angular.isObject(vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.TIN)) {
          var fieldValue = vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.TIN;
          vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.TIN = {};
          if (fieldValue.length > 2) {

            vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.TIN['#text'] = fieldValue;
            vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.TIN['@issuedBy'] = "ZA";
          }
          else {
            vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.TIN['#text'] = fieldValue;
            vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.TIN['@issuedBy'] = "ZA";
          }
        }
      }


      if (angular.isDefined(vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.IN) && vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.IN !== null) {
        if (angular.isObject(vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.IN)) {

          if (angular.isDefined(vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.IN[0]) && vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.IN[0] !== null) {
            var fieldValue = vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.IN[0];
            if (fieldValue.length > 2) {
              vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.IN[0] = {};
              vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.IN[0]['#text'] = fieldValue;
              vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.IN[0]['@issuedBy'] = "ZA";
            }
          }

          if (angular.isDefined(vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.IN[1]) && vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.IN[1] !== null) {
            var fieldValue = vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.IN[1];
            if (fieldValue.length > 2) {
              vm.prepopValue = "US";
              vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.IN[1] = {};
              vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.IN[1]['#text'] = fieldValue;
              vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.IN[1]['@issuedBy'] = vm.prepopValue;

            }

          }
        }
      }



      if (angular.isUndefined(vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.DocSpec)) {

        vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.DocSpec = {};
        vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.DocSpec.DocTypeIndic = {};
        vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.DocSpec.DocTypeIndic['#text'] = "OECD1";
      }
      if (angular.isUndefined(vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.DocSpec.DocTypeIndic)) {
        vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.DocSpec.DocTypeIndic = {};
        vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.DocSpec.DocTypeIndic['#text'] = "OECD1";
      }
      // Check the Docspec
      else {

        if (!angular.isObject(vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.DocSpec.DocTypeIndic)) {
          var fieldValue = vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.DocSpec.DocTypeIndic;
          vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.DocSpec.DocTypeIndic = {};
          if (fieldValue.length < 6) {
            vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.DocSpec.DocTypeIndic['#text'] = fieldValue;
          }
          else {
            vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.DocSpec.DocTypeIndic = "OECD1";
          }

        }

      }

      // if (angular.isUndefined(vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports)) {
      //   vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports = vm.addnewObj();

      // }
      // //Create Constituent Entity if not Provided
      // if (angular.isUndefined(vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports.ConstEntities)) {
      //   vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports.ConstEntities = vm.addNewConstituentObj();

      // }



      //      if (angular.isUndefined(vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports)) {
      //       var b;
      //         var jurisdictionsArr = vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports;
      //         if(angular.isUndefined(jurisdictionsArr)){

      // b=0;
      //         }
      //         else{

      //           b= jurisdictionsArr.length;
      //         }

      //         if (b > 0) {
      //           for (var i = 0; i < b; i++) {
      //             jurisdictionsArr = vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports[i];
      //             if (angular.isUndefined(jurisdictionsArr.DocSpec)) {
      //               vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports[i].DocSpec = {};
      //               vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports[i].DocSpec.DocTypeIndic = {};
      //               vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports[i].DocSpec.DocTypeIndic['#text'] = "OECD1";
      //               vm.$apply();
      //             }

      //           }
      //         }
      //         else {
      //           jurisdictionsArr = vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports;
      //           if (angular.isUndefined(jurisdictionsArr.DocSpec)) {
      //             vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports.DocSpec = {};
      //             vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports.DocSpec.DocTypeIndic = {};
      //             vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports.DocSpec.DocTypeIndic['#text'] = "OECD1";
      //             vm.$apply();

      //           }


      //         }
      //      }



      if (angular.isUndefined(vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo)) {
        vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo = vm.addadditionalInfor();
      }


      if (angular.isUndefined(vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo.DocSpec)) {

        var additionalInforArrs = [];
        additionalInforArrs = vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo;
        if (angular.isArray(additionalInforArrs)) {

          var a = additionalInforArrs.length;
          if (a > 0) {
            for (var i = 0; i < a; i++) {
              var additionalInforArr = additionalInforArrs[i];
              if (!angular.isObject(additionalInforArr.DocSpec)) {
                vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo[i].DocSpec = {};
                // vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo[i].DocSpec = [additionalInforArr.DocSpec];
                vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo[i].DocSpec.DocTypeIndic = {};
                vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo[i].DocSpec.DocTypeIndic['#text'] = "OECD1";
                vm.$apply();
              }
            }
          }
          else {
            vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo.DocSpec = {};
            vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo.DocSpec.DocTypeIndic = {};
            vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo.DocSpec.DocTypeIndic['#text'] = "OECD1";
            vm.$apply();
          }
        }
        else if (angular.isObject(additionalInforArrs)) {

          if (angular.isDefined(additionalInforArrs.DocSpec.DocTypeIndic) && additionalInforArrs.DocSpec.DocTypeIndic !== null) {
            if (!angular.isObject(additionalInforArrs.DocSpec.DocTypeIndic)) {
              var fieldValue = additionalInforArrs.DocSpec.DocTypeIndic;
              additionalInforArrs.DocSpec.DocTypeIndic = {};
              if (fieldValue.length < 6) {
                additionalInforArrs.DocSpec.DocTypeIndic['#text'] = fieldValue;
              }
              else {
                additionalInforArrs.DocSpec.DocTypeIndic['#text'] = "OECD1";
              }
            }

          }

        }

      }

      else {
        var additionalInforArrs = [];
        additionalInforArrs = vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo;
        if (angular.isArray(additionalInforArrs)) {
          var a = additionalInforArrs.length;
          if (a > 0) {
            for (var i = 0; i < a; i++) {
              var additionalInforArr = additionalInforArrs[i];
              if (!angular.isObject(additionalInforArr.DocSpec)) {
                vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo[i].DocSpec = {};
                // vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo[i].DocSpec = [additionalInforArr.DocSpec];
                vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo[i].DocSpec.DocTypeIndic = {};
                vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo[i].DocSpec.DocTypeIndic['#text'] = "OECD1";
                vm.$apply();
              }
            }
          }
          else {
            vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo.DocSpec = {};
            vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo.DocSpec.DocTypeIndic = {};
            vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.AdditionalInfo.DocSpec.DocTypeIndic['#text'] = "OECD1";
            vm.$apply();
          }
        }
        else if (angular.isObject(additionalInforArrs)) {

          if (angular.isDefined(additionalInforArrs.DocSpec.DocTypeIndic) && additionalInforArrs.DocSpec.DocTypeIndic !== null) {
            if (!angular.isObject(additionalInforArrs.DocSpec.DocTypeIndic)) {
              var fieldValue = additionalInforArrs.DocSpec.DocTypeIndic;
              additionalInforArrs.DocSpec.DocTypeIndic = {};
              if (fieldValue.length < 6) {
                additionalInforArrs.DocSpec.DocTypeIndic['#text'] = fieldValue;
              }
              else {
                additionalInforArrs.DocSpec.DocTypeIndic['#text'] = "OECD1";
              }
            }

          }

        }

      }



      vm.readOnly = readOnly;
      vm.prepop = prepop;

      vm.changeIntoAnArr();
      vm.updateCbcReport();
      //vm.$apply();
      //vm.updateNoOfConstituentEntities();
      vm.updateAdditionalInfor();

      // vm.addedIitmes();

      vm.$apply();

    }


    vm.addnewObj = function () {

      vm.cbcreportModel = {
        "ConstEntities": [{
          "BizActivities": [null]
        }],
        "DocSpec": {
          "DocTypeIndic": {
            "#text": "OECD1"
          }
        }
      };

      return vm.cbcreportModel;
    }

    vm.addNewConstituentObj = function () {

      vm.constituentModel = {
        "ConstEntity":
          {
            "Name": []
          },
        "BizActivities": [null],

      };

      return vm.constituentModel;
    }

    vm.addTaxJusObj = function () {
      vm.cbcreportModel = {
        "NoOfConstituentEntities": null
      };
      return vm.cbcreportModel;
    }

    vm.addadditionalInfor = function () {

      vm.otherInforObj = {
        'ResCountryCode': [null],
        'SummaryRef': [null],
        "DocSpec": {
          "DocTypeIndic": {
            "#text": "OECD1" //new 
          }
        }

      };

      return vm.otherInforObj;
    }

    vm.addmainBusinessAcObj = function () {
      vm.mainBusAccobj = null;

      return vm.mainBusAccobj
    }


    vm.initArray = function () {
      if (angular.isUndefined(vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ConstEntities)
        || vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ConstEntities == null) {
        vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports.ConstEntities = {};
        vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports.ConstEntities.ConstEntity = {};
        vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports.ConstEntities.ConstEntity.Name = []
      }
    }
    vm.updateOnloadinitArray = function () {

      vm.formData.CountryByCountryDeclaration.CBC_OECD = {};
      vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody = {};
      vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity = {};
      vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity = {};
      vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.Name = [];
      vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.IN = [];

    }

    vm.arrCreationForEntityName = function () {
      try {
        var list = vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.Name;
        var newArr = [];
        if (list) {
          if (angular.isArray(list)) {

          } else {

            newArr.push(list);
            vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.Name = newArr;
          }
        }

      } catch (error) {

      }

    }

    vm.arrCreationForEntityIn = function () {
      try {
        var newArr = [];
        var list = vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.IN
        if (list) {
          if (angular.isArray(list)) {

          } else {
            if (vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.IN['#text'].indexOf("/") == -1) {
              newArr.push(null);
            }
            newArr.push(list);
            vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.ReportingEntity.Entity.IN = newArr;
          }
        }

      } catch (error) {

      }

    }

    vm.updateNoOfConstituentEntities = function () {

      if (angular.isUndefined(vm.formData.CountryByCountryDeclaration.TaxJurisdictions) || vm.formData.CountryByCountryDeclaration.TaxJurisdictions == null) {

        vm.formData.CountryByCountryDeclaration.TaxJurisdictions = {};
        vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction = {};
      }
      var TaxJurisdictionList = [];
      TaxJurisdictionList = vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction;
      if (TaxJurisdictionList) {
        if (angular.isArray(TaxJurisdictionList)) {
          return;
        } else if (angular.isObject(TaxJurisdictionList)) {

          vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction = vm.constEntitiesOnJusArr;
          //vm.formData.CountryByCountryDeclaration.TaxJurisdictions.TaxJurisdiction.push(TaxJurisdictionList);

        }

      }

    }


    vm.taxNumberValidation = function (event) {

      if (event) {

        var fieldValue = event.target.value;

        if (fieldValue != "") {

          fieldtypeservevices.taxReferenceNumberVAlidation(fieldValue).then(null, function () {
            messageservices.taxRefNumberMessage();
            vm.formData.CountryByCountryDeclaration.CBC_OECD.CbcBody.CbcReports.ConstEntities.ConstEntity.TIN['#text'] = "";
          });
        }

      }

    }
    vm.mandatoryErrorMessage = "Please ensure that you complete the following mandatory fields before submitting the form:<br/><br/>";
    vm.isFormValid = function () {

      if (!vm.CBCForm.$valid) {
        var errorMessage = vm.mandatoryErrorMessage
        var counter = 0;
        var requiredElements = angular.element('input,textarea,select,checkbox,.ng-empty').filter('[required]');

        var n = 0;

        n = requiredElements.length;

        //  var n = vm.CBCForm.$error.required.length;

        var uniqueArr = [];
        for (var i = 0; i < n; i++) {

          var elmntID = requiredElements[i].id;
          var fieldName = angular.element('#' + elmntID).attr("data-fieldlabel");



          if (fieldName !== '' && fieldName !== undefined && fieldName !== null && uniqueArr.indexOf(fieldName) == -1) {


            var additionalInfo = "Additional Info";
            if (fieldName.indexOf(additionalInfo) == -1) {
              uniqueArr.push(fieldName);
              counter++;
              if (counter <= 10) {
                // errorMessage += "- " + rs.ITR12Form.$error.required[i].$name + "<br/>";
                errorMessage += "- " + uniqueArr[uniqueArr.length - 1] + "<br/>";
              }

            }


          }
        }
        if (counter > 10) {
          errorMessage += "<br/>Note that you have more than 10 fields that need to be completed. These fields will be displayed once the above mentioned fields have been corrected.";
        }
        if (uniqueArr.length > 0) {

          swal('Error', errorMessage);
          return false;
        }

      }
      if (vm.jurisdictionsArr.length > 0) {
        var len = vm.jurisdictionsArr.length
        for (var i = 0; i < len; i++) {
          var jurisdiction = vm.jurisdictionsArr[i];
          if (!vm.reportSummaryIsValid(jurisdiction)) {
            var errorMsg = 'Please complete all the required fields for CBC Report ' + (i + 1);
            swal('Error', errorMsg);
            return false;
          }
          else if (!vm.reportConstEntitiesValid(jurisdiction, i)) {
            return false;
          }
        }
      }

      if (vm.otherInforArr.length > 0) {

        var additionalInfoLenght = vm.otherInforArr.length;
        for (var j = 0; j < additionalInfoLenght; j++) {
          var additionalInfor = vm.otherInforArr[j];
          if (!vm.isAdditionalInfoValid(additionalInfor)) {
            var errorMsg = 'Please complete all the required fields for Additional Information ' + (j + 1);
            swal('Error', errorMsg);
            return false;
          }


        }

        return true;

      }
      return true;
    }
    vm.isAdditionalInfoValid = function (additionalInfor) {
      if (angular.isUndefined(additionalInfor.OtherInfo) || additionalInfor.OtherInfo === null || angular.isUndefined(additionalInfor.ResCountryCode) ||
        additionalInfor.ResCountryCode === null || angular.isUndefined(additionalInfor.SummaryRef)
        || additionalInfor.SummaryRef === null || !vm.isObjectRepeatingPropValid(additionalInfor.SummaryRef) || !vm.isObjectRepeatingPropValid(additionalInfor.ResCountryCode)) {
        return false;
      }

      return true;
    }
    vm.isObjectRepeatingPropValid = function (repeatinProp) {

      var n = repeatinProp.length;
      if (n == 0) {
        return false;
      }
      for (var i = 0; i < n; i++) {
        var summary = repeatinProp[i];
        if (angular.isUndefined(summary) || summary == null) {
          return false;
        }
      }
      return true;


    }

    vm.reportSummaryIsValid = function (jurisdiction) {
      // TODO - check if necessary to check for empty fields, as this might be unnecessary
      if (angular.isUndefined(jurisdiction.Summary) || angular.isUndefined(jurisdiction.Summary.ProfitOrLoss) ||
        angular.isUndefined(jurisdiction.Summary.ProfitOrLoss['@currCode']) || jurisdiction.Summary.ProfitOrLoss['@currCode'] === '' ||
        angular.isUndefined(jurisdiction.Summary.ProfitOrLoss['#text']) || jurisdiction.Summary.ProfitOrLoss['#text'] === '' ||
        angular.isUndefined(jurisdiction.ResCountryCode) || angular.isUndefined(jurisdiction.Summary.Capital) || jurisdiction.Summary.Capital === null ||
        angular.isUndefined(jurisdiction.Summary.Capital['#text']) || jurisdiction.Summary.Capital['#text'] === '' ||
        angular.isUndefined(jurisdiction.Summary.Capital['@currCode']) || jurisdiction.Summary.Capital['@currCode'] === '' ||
        angular.isUndefined(jurisdiction.Summary.TaxPaid) || jurisdiction.Summary.TaxPaid['#text'] === '' || jurisdiction.Summary.TaxPaid === null ||
        angular.isUndefined(jurisdiction.Summary.TaxPaid['#text']) || jurisdiction.Summary.TaxPaid['@currCode'] === '' ||
        angular.isUndefined(jurisdiction.Summary.TaxPaid['@currCode']) || angular.isUndefined(jurisdiction.Summary.Earnings) || jurisdiction.Summary.Earnings === null ||
        angular.isUndefined(jurisdiction.Summary.Earnings['#text']) || jurisdiction.Summary.Earnings['#text'] === '' ||
        angular.isUndefined(jurisdiction.Summary.Earnings['@currCode']) || jurisdiction.Summary.Earnings['@currCode'] === '' ||
        angular.isUndefined(jurisdiction.Summary.TaxAccrued) || jurisdiction.Summary.TaxAccrued['#text'] === '' || jurisdiction.Summary.TaxAccrued === null ||
        angular.isUndefined(jurisdiction.Summary.TaxAccrued['#text']) || jurisdiction.Summary.TaxAccrued['@currCode'] === '' ||
        angular.isUndefined(jurisdiction.Summary.TaxAccrued['@currCode']) || angular.isUndefined(jurisdiction.Summary.Assets) ||
        angular.isUndefined(jurisdiction.Summary.Assets['#text']) || jurisdiction.Summary.Assets['#text'] === '' ||
        angular.isUndefined(jurisdiction.Summary.Assets['@currCode']) || jurisdiction.Summary.Assets['@currCode'] === '' || jurisdiction.Summary.Assets === null ||
        angular.isUndefined(jurisdiction.Summary.NbEmployees) || jurisdiction.Summary.NbEmployees === '' || jurisdiction.Summary.NbEmployees === null ||
        angular.isUndefined(jurisdiction.Summary.Revenues) || angular.isUndefined(jurisdiction.Summary.Revenues.Unrelated) || jurisdiction.Summary.Revenues.Unrelated === null ||
        angular.isUndefined(jurisdiction.Summary.Revenues.Unrelated['#text']) || jurisdiction.Summary.Revenues.Unrelated['#text'] === '' ||
        angular.isUndefined(jurisdiction.Summary.Revenues.Unrelated['@currCode']) || jurisdiction.Summary.Revenues.Unrelated['@currCode'] === '' ||
        angular.isUndefined(jurisdiction.Summary.Revenues.Related) || jurisdiction.Summary.Revenues.Related['#text'] === '' || jurisdiction.Summary.Revenues.Related === null ||
        angular.isUndefined(jurisdiction.Summary.Revenues.Related['#text']) || jurisdiction.Summary.Revenues.Related['@currCode'] === '' ||
        angular.isUndefined(jurisdiction.Summary.Revenues.Related['@currCode'])) {
        return false;
      }
      return true;
    }

    vm.onEditreportConstEntitiesValid = function () {
      if (angular.isUndefined(vm.constituentArr) || vm.constituentArr.length == 0) {
        swal('Error', 'Please complete at least one Constituent Entity for the CBC Report ');
        return false;
      }
      else {
        var constEntities = vm.constituentArr;
        var n = constEntities.length;
        for (var i = 0; i < n; i++) {
          var entity = constEntities[i];
          if (!vm.reportEntityIsValid(entity)) {
            var errorMsg = 'Please complete all the required fields for Constituent Entity ' + (i + 1) + ' in CBC Report ' + (vm.reportid + 1);
            swal('Error', errorMsg);
            return false;
          }
        }
      }
      return true;
    }

    vm.reportConstEntitiesValid = function (jurisdiction, reportIndex) {
      if (angular.isUndefined(jurisdiction.ConstEntities) || jurisdiction.ConstEntities.length == 0) {
        swal('Error', 'Please complete at least one Constituent Entity for the CBC Report ');
        return false;
      }
      else {
        var constEntities = jurisdiction.ConstEntities
        var n = constEntities.length;
        for (var i = 0; i < n; i++) {
          var entity = constEntities[i];
          if (!vm.reportEntityIsValid(entity) || entity == null) {
            var errorMsg = 'Please complete all the required fields for Constituent Entity ' + (i + 1) + ' in CBC Report ' + (reportIndex + 1);
            swal('Error', errorMsg);
            return false;
          }
        }
      }
      return true;
    }

    vm.reportEntityIsValid = function (constituent) {
      if (angular.isDefined(constituent) && constituent !== null) {
        if (angular.isDefined(constituent.ConstEntity) && constituent.ConstEntity !== null) {
          if (angular.isUndefined(constituent.ConstEntity.Name) || angular.isUndefined(constituent.ConstEntity.Name[0]) || constituent.ConstEntity.Name === null ||
            angular.isUndefined(constituent.ConstEntity.IN) || angular.isUndefined(constituent.ConstEntity.IN['#text']) || constituent.ConstEntity.IN === null ||
            angular.isUndefined(constituent.ConstEntity.IN['@issuedBy']) || angular.isUndefined(constituent.ConstEntity.TIN) || constituent.ConstEntity.TIN === null ||
            angular.isUndefined(constituent.ConstEntity.TIN['#text']) || angular.isUndefined(constituent.ConstEntity.TIN['@issuedBy']) ||
            angular.isUndefined(constituent.ConstEntity.ResCountryCode) || angular.isUndefined(constituent.ConstEntity.Address) || constituent.ConstEntity.ResCountryCode === null ||
            angular.isUndefined(constituent.ConstEntity.Address['@legalAddressType']) || angular.isUndefined(constituent.ConstEntity.Address.AddressFree) || constituent.ConstEntity.Address === null ||
            angular.isUndefined(constituent.BizActivities) || constituent.BizActivities == null || !vm.isBusinessActivitiesValid(constituent.BizActivities)) {
            return false;
          }
        }
        else if (angular.isUndefined(constituent.ConstEntity)) {
          return false;
        }
      }
      else if (angular.isUndefined(constituent) || constituent == null) {
        return false;
      }

      return true;
    }

    vm.isBusinessActivitiesValid = function (buzActivities) {
      var n = buzActivities.length;
      if (n == 0) {
        return false;
      }
      for (var i = 0; i < n; i++) {
        var buzActivity = buzActivities[i];
        if (angular.isUndefined(buzActivity) || buzActivity == null) {
          return false;
        }
      }
      return true;
    }

    vm.runEmailAddressValidationOnExitRules = function (event) {
      if (event.target.value === "" || event.target.value === "null") {
        return;
      }

      var emailField = event.target;//.value;
      fieldtypeservevices.isEmailAddressField(emailField).then(null, function (name) {
        messageservices.emailAddressMsg(name);

        //delete vm.formData.CountryByCountryDeclaration.ContactDetails.EmailAddress;
        vm.formData.CountryByCountryDeclaration.ContactDetails.EmailAddress = "";
      });

    }
    vm.runDateValidationOnExitRules = function (event) {
      if (event.target.value === "" || event.target.value === "null") {
        return;
      }

      var dateField = event.target.value;//.value;
      var name = event.target.tagName;//.value;
      dateservices.isValidPeriodDate(dateField).then(null, function (name) {
        messageservices.ReportingPeriodMessage();

        //delete vm.formData.CountryByCountryDeclaration.ContactDetails.EmailAddress;
        vm.formData.CountryByCountryDeclaration.CBC_OECD.MessageSpec.ReportingPeriod = "";
      });

    }

    vm.runDeclarationDateValidationOnExit = function (event) {
      if (event.target.value === "" || event.target.value === "null") {
        return;
      }

      var dateField = event.target.value;//.value;
      var name = event.target.tagName;//.value;
      dateservices.isValidDate(dateField).then(null, function (name) {
        messageservices.declarationDateMessage();


        vm.formData.CountryByCountryDeclaration.DeclarationDate = "";
      });

    }

    vm.initNameArry = function (constituent) {
      try {
        if (constituent.ConstEntity.Name == null) {
          constituent.ConstEntity.Name = [null];
        }
      } catch (error) {

      }
    }

    vm.deletionMessage = function (otherInfor, event) {

      var fieldValue = event.target.value;

      if (fieldValue == "on" || fieldValue == "OECD3") {
        messageservices.deletingRecordMessage().then(function () {

          //otherInfor.DocSpec.DocTypeIndic = "OECD3";
        }, function () {

          otherInfor.DocSpec.DocTypeIndic = "";
          vm.$apply();

        });
      }

    }

    vm.deletionReportMessage = function (jurisdiction, event) {

      var fieldValue = event.target.value;

      if (fieldValue == "on" || fieldValue == "OECD3") {
        messageservices.deletingRecordMessage().then(function () {

          // jurisdiction.DocSpec.DocTypeIndic = "OECD3";
        }, function () {

          jurisdiction.DocSpec.DocTypeIndic = "";
          vm.$apply();

        });
      }

    }

    vm.deletionEntityMessage = function (entity, event) {

      var fieldValue = event.target.value;

      if (fieldValue == "on" || fieldValue == "OECD3") {
        messageservices.deletingRecordMessage().then(function () {

          // entity.DocSpec.DocTypeIndic = "OECD3";
        }, function () {

          entity.DocSpec.DocTypeIndic = "";
          vm.$apply();

        });
      }

    }

    vm.prepopValueWhenNotEmpty = function (event) {

      var fieldValue = event.target.value;

      if (fieldValue !== "") {
        vm.prepopValue = "US";
      } else {
        vm.prepopValue = "";
      }

    }

    vm.telNumberValidation = function (event) {
      var fieldValue = event.target.value;
      if (fieldValue != "") {
        fieldtypeservevices.isMinNineTelPhone(event.target.value).then(null, function () {
          messageservices.NumericCheckTelNumberMessage(event.target.name);
          event.target.value = "";

        });

      }

    }
    vm.validateGIINNoField = function (event) {
      if (event.target.value === "" || event.target.value === null || event.target.value === "null") {
        return;
      }
      var fieldVal = event.target.value;
      var isValid = fieldtypeservevices.isGIINNoField(fieldVal);
      if (!isValid) {
        messageservices.giinNumberCheckMsg();
        event.target.value = "";
      }
    }
    /* CBC Report form Validation Function */

    vm.CBCReportFormValid = function () {

      if (!vm.CBCReportFormDiv.$valid) {
        var errorMessage = vm.mandatoryErrorMessage
        var counter = 0;
        var requiredElements = vm.CBCReportFormDiv.$error.required;
        var n = 0;
        n = requiredElements.length;

        var uniqueArr = [];
        for (var i = 0; i < n; i++) {


          var fieldName = requiredElements[i].$name;

          if (fieldName !== '' && fieldName !== undefined && fieldName !== null && uniqueArr.indexOf(fieldName) == -1) {
            uniqueArr.push(fieldName);
            counter++;
            if (counter <= 10) {
              errorMessage += "- " + uniqueArr[uniqueArr.length - 1] + "<br/>";
            }
          }
        }
        if (counter > 10) {
          errorMessage += "<br/>Note that you have more than 10 fields that need to be completed. These fields will be displayed once the above mentioned fields have been corrected.";
        }

        swal('Error', errorMessage);
        return false;
      }

      return true;
    }

    vm.ConstituentFormValid = function () {

      if (!vm.CBCReportFormDiv.ConstituentEntity.$valid) {
        var errorMessage = vm.mandatoryErrorMessage
        var counter = 0;
        var requiredElements = vm.CBCReportFormDiv.ConstituentEntity.$error.required;
        var n = 0;
        n = requiredElements.length;

        var uniqueArr = [];
        for (var i = 0; i < n; i++) {

          var fieldName = requiredElements[i].$name;

          if (fieldName !== '' && fieldName !== undefined && fieldName !== null && uniqueArr.indexOf(fieldName) == -1) {
            uniqueArr.push(fieldName);
            counter++;
            if (counter <= 10) {
              errorMessage += "- " + uniqueArr[uniqueArr.length - 1] + "<br/>";
            }
          }
        }
        if (counter > 10) {
          errorMessage += "<br/>Note that you have more than 10 fields that need to be completed. These fields will be displayed once the above mentioned fields have been corrected.";
        }

        swal('Error', errorMessage);
        return false;
      }
      return true;
    }

    vm.onResCountryCodeSelect = function (item) {
      vm.resCountryName = item.name;
    }

    vm.onSelectedBizActivity = function (constituent) {
      var n = constituent.BizActivities.length;

      for (var i = 0; i < n; i++) {
        if (constituent.BizActivities[i] === 'CBC513') {
          vm.otherBusinessActivityIsRequired = true;
          return;

        }
        else {

          vm.otherBusinessActivityIsRequired = false;

        }

      }
    }

    vm.onEntityResCodeSelect = function (item) {
      vm.entityResCountryName = item.name;
    }

    vm.getNameOfSelectedCountryCode = function (code) {
      var countryName = "";
      for (var i = 0; i < vm.countrycodes.length; i++) {
        var countryObj = vm.countrycodes[i];
        if (countryObj.code === code) {
          countryName = countryObj.name;
          break;
        }
      }
      return countryName;
    }


    /**cbc without validation */

    vm.CBCReportFormValidWithoutMessages = function () {

      if (!vm.CBCReportFormDiv.$valid) {
        var errorMessage = vm.mandatoryErrorMessage
        var counter = 0;
        var requiredElements = vm.CBCReportFormDiv.$error.required;
        var n = 0;
        n = requiredElements.length;

        if (n <= 8) {
          return true;

        }

        var uniqueArr = [];
        for (var i = 0; i < n; i++) {


          var fieldName = requiredElements[i].$name;

          if (fieldName !== '' && fieldName !== undefined && fieldName !== null && uniqueArr.indexOf(fieldName) == -1) {
            uniqueArr.push(fieldName);
            counter++;
            if (counter <= 10) {
              errorMessage += "- " + uniqueArr[uniqueArr.length - 1] + "<br/>";

            }
          }
        }
        if (counter > 10) {
          errorMessage += "<br/>Note that you have more than 10 fields that need to be completed. These fields will be displayed once the above mentioned fields have been corrected.";
        }

        return false;
      }

      return true;
    }

  }
})();
