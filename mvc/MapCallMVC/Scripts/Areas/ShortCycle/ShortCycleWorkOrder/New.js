var ShortCycleWorkOrder = (function ($) {
  var ELEMENTS = {
    //To test locally: chrome.exe --user-data-dir="C:/Chrome dev session" --disable-web-security
    url: (window.location.toString().indexOf('localhost') === -1) ? 'https://mapcall.amwaternp.com/modules/api/ShortCycleWorkOrder/CreateOrUpdate/' : 'http://localhost:55667/ShortCycleWorkOrder/CreateOrUpdate/'
  };

  var scwo = {
    initialize: function () {
      $('#btnCreate').on('click', scwo.create);
      $('#btnUpdate').on('click', scwo.update);
    }, 
    create: function () {
      $.ajax({
        type: 'POST',
        url: ELEMENTS.url,
        dataType: 'json',
        async: 'true',
        headers: {
          'Authorization': 'Basic ' + btoa('rystroa1:password#1'),
          'Content-Type': 'application/json'
        },
        contentType: 'application/json',
        data: '{"EquipmentId":"000000000051834914","FunctionalLocation":6001841541,"NotificationNumber":313518958,"Priority":"6: Within 30 Days","BackReportingType":32,"CompanyCode":1015,"Premise":9050075945,"WorkOrder":"000512933333","Status":"Dispatched","WBSElement":"E15-1420-153006","PlanningPlant":"D401","MatCode":"CKM","MATCodeDescription":"Check MeterVerif Serial #&Read","OperationText":"Check Meter, Verify Serial #, Read","FSRId":60001272,"FSRName":"Moustachio Featherbottoms","CustomerNumber":1101748652,"ServiceType":"WT","MeterSerialNumber":"000000000085424082","ManufacturerSerialNumber":"0085424082","IsCustomerEnrolledForEmail":false,"AssignmentStart":20180323150700,"AssignmentEnd":20180323152500,"OrderType":"0032","OperationId":"0010","WorkCenter":"FSR","CustomerAccount":210020509469,"NormalDuration":18,"NormalDurationUnit":"MIN","PlannerGroup":"010", "PhoneAhead": "true", "CustomerAtHome":"false", "SecurityThreats":[{"PoliceEscort":"No", "SecurityThreat":"iGnoRe" }, {"PoliceEscort":"Yes", "SecurityThreat":"there is a security threat" }]}',
        success: function (d) {
          console.log('Result:' + d.Success);
        },
        error: function (xmlHttpRequest, textStatus, errorThrown) {
          console.log('Error:' + errorThrown);
        }
      });
      
      //$.ajax({
      //  type: 'POST',
      //  url: ELEMENTS.url,
      //  dataType: 'json',
      //  async: 'true',
      //  headers: {
      //    'Authorization': 'Basic ' + btoa('rystroa1:password#1'),
      //    'Content-Type': 'application/json'
      //  },
      //  contentType: 'application/json',
      //  data: "{" +
      //    "'EquipmentId':53932662" +
      //    ",'FunctionalLocation':6002071353" +
      //    ",'SafetyConcern':' no animals'" +
      //    ",'NotificationNumber':312287724" +
      //    ",'Priority':'1: Emergency 1-2 Hrs'" +
      //    ",'CompanyCode':'1018'" +
      //    ",'Premise':9180456322" +
      //    ",'WorkOrder':511749787" +
      //    ",'Status':'DISP SSTC'" +
      //    ",'WBSElement':'E18-1420-181906'" +
      //    ",'PlanningPlant':'D205'" +
      //    ",'MatCode':'EMR'" +
      //    ",'MatCodeDescription':'Emergency Service Order'" +
      //    ",'OperationText':'Emergency Service Order'" +
      //    ",'FSRId':60001160" +
      //    ",'IsCustomerEnrolledForEmail':'true'" +
      //    ",'SecurityThreat':''" +
      //    ",'PoliceEscort':''" +
      //    ",'AssignmentStart':'3/19/2018 2:32:00 PM'" +
      //    ",'AssignmentEnd':'3/19/2018 14:39:00'" +
      //    ",'OrderType':'0032'" +
      //    ",'OperationId':'0010'" +
      //    ",'WorkCenter':'123'" +
      //    ",'CustomerAccount':'210024483231'" +
      //    ",'NormalDuration':10.0" +
      //    ",'NormalDurationUnit':'MIN'" +
      //    ",'PlannerGroup':'010'" +
      //    "}", 
      //  success: function (d) {
      //    console.log('Result:' + d.Success);
      //  },
      //  error: function (xmlHttpRequest, textStatus, errorThrown) {
      //    console.log('Error:' + errorThrown);
      //  }
      //});
      //$.ajax({
      //  type: 'POST',
      //  url: ELEMENTS.url,
      //  dataType: 'json',
      //  async: 'true',
      //  headers: {
      //    'Authorization': 'Basic ' + btoa('rystroa1:password#1'),
      //    'Content-Type': 'application/json'
      //  },
      //  contentType: 'application/json',
      //  data: "{" +
      //    "'EquipmentId':''" +
      //    ",'FunctionalLocation':6003547770" +
      //    ",'SafetyConcern':''" +
      //    ",'NotificationNumber':313271792" +
      //    ",'Priority':'1: Emergency 1-2 Hrs'" +
      //    ",'CompanyCode':'1018'" +
      //    ",'Premise':9180463863" +
      //    ",'WorkOrder':512694374" +
      //    ",'Status':'ONST SSTC'" +
      //    ",'WBSElement':'E18-1420-181906'" +
      //    ",'PlanningPlant':'D205'" +
      //    ",'MatCode':'EMR'" +
      //    ",'MatCodeDescription':'Emergency Service Order'" +
      //    ",'OperationText':'Emergency Service Order'" +
      //    ",'FSRId':18508494" +
      //    ",'IsCustomerEnrolledForEmail':'false'" +
      //    ",'SecurityThreat':''" +
      //    ",'PoliceEscort':''" +
      //    ",'AssignmentStart':'3/19/2018 1:29:00 PM'" +
      //    ",'AssignmentEnd':'3/19/2018 13:36:00'" +
      //    ",'OrderType':'0032'" +
      //    ",'OperationId':'0010'" +
      //    ",'WorkCenter':'123'" +
      //    ",'CustomerAccount':''" +
      //    ",'NormalDuration':10.0" +
      //    ",'NormalDurationUnit':'MIN'" +
      //    ",'PlannerGroup':'010'" +
      //  "}",
      //  success: function (d) {
      //    console.log('Result:' + d.Success);
      //  },
      //  error: function (xmlHttpRequest, textStatus, errorThrown) {
      //    console.log('Error:' + errorThrown);
      //  }
      //});
      //$.ajax({
      //  type: 'POST',
      //  url: ELEMENTS.url,
      //  dataType: 'json',
      //  async: 'true',
      //  headers: {
      //    'Authorization': 'Basic ' + btoa('rystroa1:password#1'),
      //    'Content-Type': 'application/json'
      //  },
      //  contentType: 'application/json',
      //  data: "{" +
      //    "'EquipmentId':54010281" +
      //    ",'FunctionalLocation':6003695822" +
      //    ",'SafetyConcern':''" +
      //    ",'NotificationNumber':307294055" +
      //    ",'Priority':'3: Customer Appt.'" +
      //    ",'CompanyCode':'1018'" +
      //    ",'Premise':5000034486" +
      //    ",'WorkOrder':506926734" +
      //    ",'Status':'ONST SSTC'" +
      //    ",'WBSElement':'E18-1420-181902'" +
      //    ",'PlanningPlant':'D205'" +
      //    ",'MatCode':'ILK'" +
      //    ",'MatCodeDescription':'Inspect for Leak/High-Low Usage'" +
      //    ",'OperationText':'Inspect for Leak/High-Low Usage'" +
      //    ",'FSRId':18508494" +
      //    ",'IsCustomerEnrolledForEmail':'false'" +
      //    ",'SecurityThreat':''" +
      //    ",'PoliceEscort':''" +
      //    ",'AssignmentStart':'2018-03-2009:27:00'" +
      //    ",'AssignmentEnd':'2018-03-2009:37:00'" +
      //    ",'OrderType':'0032'" +
      //    ",'OperationId':'0010'" +
      //    ",'WorkCenter':'123'" +
      //    ",'CustomerAccount':'220012142084'" +
      //    ",'NormalDuration':20.0" +
      //    ",'NormalDurationUnit':'MIN'" +
      //    ",'PlannerGroup':'010'" +
      //    "}", 
      //  success: function (d) {
      //    console.log('Result:' + d.Success);
      //  },
      //  error: function (xmlHttpRequest, textStatus, errorThrown) {
      //    console.log('Error:' + errorThrown);
      //  }
      //});
      //$.ajax({
      //  type: 'POST',
      //  url: ELEMENTS.url,
      //  dataType: 'json',
      //  async: 'true',
      //  headers: {
      //    'Authorization': 'Basic ' + btoa('rystroa1:password#1'),
      //    'Content-Type': 'application/json'
      //  },
      //  contentType: 'application/json',
      //  data: "{" +
      //    "'EquipmentId':52569290" +
      //    ",'FunctionalLocation':6002324383" +
      //    ",'SafetyConcern':''" +
      //    ",'NotificationNumber':310171999" +
      //    ",'Priority':'3: Customer Appt.'" +
      //    ",'CompanyCode':'1018'" +
      //    ",'Premise':9180734789" +
      //    ",'WorkOrder':509700080" +
      //    ",'Status':'ONST SSTC'" +
      //    ",'WBSElement':'E18-1420-181906'" +
      //    ",'PlanningPlant':'D205'" +
      //    ",'MatCode':'ILK'" +
      //    ",'MatCodeDescription':'Inspect for Leak/High-Low'" +
      //    ",'OperationText':'Inspect for Leak/High-Low Usage'" +
      //    ",'FSRId':18730706" +
      //    ",'IsCustomerEnrolledForEmail':'false'" +
      //    ",'SecurityThreat':''" +
      //    ",'PoliceEscort':''" +
      //    ",'AssignmentStart':'2018-03-1911:18:00'" +
      //    ",'AssignmentEnd':'2018-03-1911:30:00'" +
      //    ",'OrderType':'0032'" +
      //    ",'OperationId':'0010'" +
      //    ",'WorkCenter':'123'" +
      //    ",'CustomerAccount':'210023751139'" +
      //    ",'NormalDuration':20.0" +
      //    ",'NormalDurationUnit':'MIN'" +
      //    ",'PlannerGroup':'010'" +
      //    "" +
      //    "}", 
      //          success: function (d) {
      //    console.log('Result:' + d.Success);
      //  },
      //  error: function (xmlHttpRequest, textStatus, errorThrown) {
      //    console.log('Error:' + errorThrown);
      //  }
      //});

      //$.ajax({
      //  type: 'POST',
      //  url: ELEMENTS.url,
      //  dataType: 'json',
      //  async: 'true',
      //  headers: {
      //    'Authorization': 'Basic ' + btoa('rystroa1:password#1'),
      //    'Content-Type': 'application/json'
      //  },
      //  contentType: 'application/json',
      //  data: "{" +
      //    "'EquipmentId':54010665" +
      //    ",'FunctionalLocation':6003703695" +
      //    ",'SafetyConcern':''" +
      //    ",'NotificationNumber':310963811" +
      //    ",'Priority':'3: Customer Appt.'" +
      //    ",'CompanyCode':'1018'" +
      //    ",'Premise':5000040648" +
      //    ",'WorkOrder':510467800" +
      //    ",'Status':'ONST SSTC'" +
      //    ",'WBSElement':'E18-1420-181902'" +
      //    ",'PlanningPlant':'D205'" +
      //    ",'MatCode':'ILK'" +
      //    ",'MatCodeDescription':'Inspect for Leak/High-Low'" +
      //    ",'OperationText':'Inspect for Leak/High-Low Usage'" +
      //    ",'FSRId':18508486" +
      //    ",'IsCustomerEnrolledForEmail':'false'" +
      //    ",'SecurityThreat':''" +
      //    ",'PoliceEscort':''" +
      //    ",'AssignmentStart':'2018-03-1909:46:00'" +
      //    ",'AssignmentEnd':'2018-03-1909:58:00'" +
      //    ",'OrderType':'0032'" +
      //    ",'OperationId':'0010'" +
      //    ",'WorkCenter':'123'" +
      //    ",'CustomerAccount':'220010747650'" +
      //    ",'NormalDuration':20.0" +
      //    ",'NormalDurationUnit':'MIN'" +
      //    ",'PlannerGroup':'010'" +
      //    "}", 
      //  success: function (d) {
      //    console.log('Result:' + d.Success);
      //  },
      //  error: function (xmlHttpRequest, textStatus, errorThrown) {
      //    console.log('Error:' + errorThrown);
      //  }
      //});
      //$.ajax({
      //  type: 'POST',
      //  url: ELEMENTS.url,
      //  dataType: 'json',
      //  async: 'true',
      //  headers: {
      //    'Authorization': 'Basic ' + btoa('rystroa1:password#1'),
      //    'Content-Type': 'application/json'
      //  },
      //  contentType: 'application/json',
      //  data: "{" +
      //    "'EquipmentId':53844931" +
      //    ",'FunctionalLocation':6003652068" +
      //    ",'SafetyConcern':''" +
      //    ",'NotificationNumber':313495330" +
      //    ",'Priority':'3: Customer Appt.'" +
      //    ",'CompanyCode':'1018'" +
      //    ",'Premise':5000025575" +
      //    ",'WorkOrder':512908959" +
      //    ",'Status':'AKNW SSTC'" +
      //    ",'WBSElement':'E18-1420-181906'" +
      //    ",'PlanningPlant':'D205'" +
      //    ",'MatCode':'MON'" +
      //    ",'MatCodeDescription':'New Cust, Read, Leave On'" +
      //    ",'OperationText':'New Cust, Read, Leave On'" +
      //    ",'FSRId':18502746" +
      //    ",'IsCustomerEnrolledForEmail':'false'" +
      //    ",'SecurityThreat':'Stacey Will w/Eaton 636-939-3808 states FIRE ACCT should not be in their name, her Paragon(Eaton Properties) company does not own" +
      //    "this prms and they never owned or managed this prms but Paragon Certified Restoration does own this prms and should be the" +
      //    "responsible party for the bill for both DOM AND FIRE ACCTS...issued CKM so tech can leave a door tag for the correct ratepayer to" +
      //    "call in and take over the accts..issued CKM 512895645 on the DOM acct" +
      //    "'" +
      //    ",'PoliceEscort':'yes'" +
      //    ",'AssignmentStart':'2018-03-1913:42:00'" +
      //    ",'AssignmentEnd':'2018-03-1913:56:00'" +
      //    ",'OrderType':'0030'" +
      //    ",'OperationId':'0010'" +
      //    ",'WorkCenter':'123'" +
      //    ",'CustomerAccount':'220022852234'" +
      //    ",'NormalDuration':13.0" +
      //    ",'NormalDurationUnit':'MIN'" +
      //    ",'PlannerGroup':'010'" +
      //    "}", 
      //  success: function (d) {
      //    console.log('Result:' + d.Success);
      //  },
      //  error: function (xmlHttpRequest, textStatus, errorThrown) {
      //    console.log('Error:' + errorThrown);
      //  }
      //});
      //$.ajax({
      //  type: 'POST',
      //  url: ELEMENTS.url,
      //  dataType: 'json',
      //  async: 'true',
      //  headers: {
      //    'Authorization': 'Basic ' + btoa('rystroa1:password#1'),
      //    'Content-Type': 'application/json'
      //  },
      //  contentType: 'application/json',
      //  data: "{" +
      //    "'EquipmentId':52439474" +
      //    ",'FunctionalLocation':6002348655" +
      //    ",'SafetyConcern':''" +
      //    ",'NotificationNumber':311175641" +
      //    ",'Priority':'5: Within 15 Days'" +
      //    ",'CompanyCode':'1018'" +
      //    ",'Premise':9180762848" +
      //    ",'WorkOrder':510671709" +
      //    ",'Status':'ONST SSTC'" +
      //    ",'WBSElement':'E18-1420-181906'" +
      //    ",'PlanningPlant':'D205'" +
      //    ",'MatCode':'RBL'" +
      //    ",'MatCodeDescription':'Priority Read for Billing'" +
      //    ",'OperationText':'Priority Read for Billing'" +
      //    ",'FSRId':18508494" +
      //    ",'IsCustomerEnrolledForEmail':'false'" +
      //    ",'SecurityThreat':''" +
      //    ",'PoliceEscort':''" +
      //    ",'AssignmentStart':'2018-03-1915:09:00'" +
      //    ",'AssignmentEnd':'2018-03-1915:19:00'" +
      //    ",'OrderType':'0032'" +
      //    ",'OperationId':'0010'" +
      //    ",'WorkCenter':'123'" +
      //    ",'CustomerAccount':'210026617740'" +
      //    ",'NormalDuration':10.0" +
      //    ",'NormalDurationUnit':'MIN'" +
      //    ",'PlannerGroup':'010'" +
      //    "" +
      //    "}",         success: function (d) {
      //    console.log('Result:' + d.Success);
      //  },
      //  error: function (xmlHttpRequest, textStatus, errorThrown) {
      //    console.log('Error:' + errorThrown);
      //  }
      //});
      //$.ajax({
      //  type: 'POST',
      //  url: ELEMENTS.url,
      //  dataType: 'json',
      //  async: 'true',
      //  headers: {
      //    'Authorization': 'Basic ' + btoa('rystroa1:password#1'),
      //    'Content-Type': 'application/json'
      //  },
      //  contentType: 'application/json',
      //  data: "{" +
      //    "'EquipmentId':54441032" +
      //    ",'FunctionalLocation':6002316078" +
      //    ",'SafetyConcern':''" +
      //    ",'NotificationNumber':311479427" +
      //    ",'Priority':'5: Within 15 Days'" +
      //    ",'CompanyCode':'1018'" +
      //    ",'Premise':9180737210" +
      //    ",'WorkOrder':510966404" +
      //    ",'Status':'ONST SSTC'" +
      //    ",'WBSElement':'E18-1420-181906'" +
      //    ",'PlanningPlant':'D205'" +
      //    ",'MatCode':'RBL'" +
      //    ",'MatCodeDescription':'Priority Read for Billing'" +
      //    ",'OperationText':'Priority Read for Billing'" +
      //    ",'FSRId':18508523" +
      //    ",'IsCustomerEnrolledForEmail':'false'" +
      //    ",'SecurityThreat':''" +
      //    ",'PoliceEscort':''" +
      //    ",'AssignmentStart':'2018-03-1915:09:00'" +
      //    ",'AssignmentEnd':'2018-03-1915:19:00'" +
      //    ",'OrderType':'0032'" +
      //    ",'OperationId':'0010'" +
      //    ",'WorkCenter':'123'" +
      //    ",'CustomerAccount':'210025066943'" +
      //    ",'NormalDuration':10.0" +
      //    ",'NormalDurationUnit':'MIN'" +
      //    ",'PlannerGroup':'010'" +
      //    "" +
      //  "}",
      //  success: function (d) {
      //    console.log('Result:' + d.Success);
      //  },
      //  error: function (xmlHttpRequest, textStatus, errorThrown) {
      //    console.log('Error:' + errorThrown);
      //  }
      //});
      //$.ajax({
      //  type: 'POST',
      //  url: ELEMENTS.url,
      //  dataType: 'json',
      //  async: 'true',
      //  headers: {
      //    'Authorization': 'Basic ' + btoa('rystroa1:password#1'),
      //    'Content-Type': 'application/json'
      //  },
      //  contentType: 'application/json',
      //  data: "{" +
      //    "'EquipmentId':54573087" +
      //    ",'FunctionalLocation':6002121188" +
      //    ",'SafetyConcern':''" +
      //    ",'NotificationNumber':313479750" +
      //    ",'Priority':'5: Within 15 Days'" +
      //    ",'CompanyCode':'1018'" +
      //    ",'Premise':9180467740" +
      //    ",'WorkOrder':512893858" +
      //    ",'Status':'SUSP SSTC'" +
      //    ",'WBSElement':'E18-1420-181906'" +
      //    ",'PlanningPlant':'D205'" +
      //    ",'MatCode':'RBL'" +
      //    ",'MatCodeDescription':'Priority Read for Billing'" +
      //    ",'OperationText':'Priority Read for Billing'" +
      //    ",'FSRId':18508523" +
      //    ",'IsCustomerEnrolledForEmail':'false'" +
      //    ",'SecurityThreat':''" +
      //    ",'PoliceEscort':''" +
      //    ",'AssignmentStart':'2018-03-1909:17:00'" +
      //    ",'AssignmentEnd':'2018-03-1909:29:00'" +
      //    ",'OrderType':'0032'" +
      //    ",'OperationId':'0010'" +
      //    ",'WorkCenter':'123'" +
      //    ",'CustomerAccount':'210025150176'" +
      //    ",'NormalDuration':10.0" +
      //    ",'NormalDurationUnit':'MIN'" +
      //    ",'PlannerGroup':'010'" +
      //    "" +
      //    "}",         success: function (d) {
      //    console.log('Result:' + d.Success);
      //  },
      //  error: function (xmlHttpRequest, textStatus, errorThrown) {
      //    console.log('Error:' + errorThrown);
      //  }
      //});
      //$.ajax({
      //  type: 'POST',
      //  url: ELEMENTS.url,
      //  dataType: 'json',
      //  async: 'true',
      //  headers: {
      //    'Authorization': 'Basic ' + btoa('rystroa1:password#1'),
      //    'Content-Type': 'application/json'
      //  },
      //  contentType: 'application/json',
      //  data: "{" +
      //    "'EquipmentId':52264640" +
      //    ",'FunctionalLocation':6002094264" +
      //    ",'SafetyConcern':' Pets on Premise'" +
      //    ",'NotificationNumber':313479755" +
      //    ",'Priority':'5: Within 15 Days'" +
      //    ",'CompanyCode':'1018'" +
      //    ",'Premise':9180464927" +
      //    ",'WorkOrder':512893863" +
      //    ",'Status':'SUSP SSTC'" +
      //    ",'WBSElement':'E18-1420-181906'" +
      //    ",'PlanningPlant':'D205'" +
      //    ",'MatCode':'RBL'" +
      //    ",'MatCodeDescription':'Priority Read for Billing'" +
      //    ",'OperationText':'Priority Read for Billing'" +
      //    ",'FSRId':18508523" +
      //    ",'IsCustomerEnrolledForEmail':'false'" +
      //    ",'SecurityThreat':''" +
      //    ",'PoliceEscort':''" +
      //    ",'AssignmentStart':'2018-03-2009:16:00'" +
      //    ",'AssignmentEnd':'2018-03-2009:34:00'" +
      //    ",'OrderType':'0032'" +
      //    ",'OperationId':'0010'" +
      //    ",'WorkCenter':'123'" +
      //    ",'CustomerAccount':'210026292523'" +
      //    ",'NormalDuration':10.0" +
      //    ",'NormalDurationUnit':'MIN'" +
      //    ",'PlannerGroup':'010'" +
      //    "}", 
      //  success: function (d) {
      //    console.log('Result:' + d.Success);
      //  },
      //  error: function (xmlHttpRequest, textStatus, errorThrown) {
      //    console.log('Error:' + errorThrown);
      //  }
      //});
      //$.ajax({
      //  type: 'POST',
      //  url: ELEMENTS.url,
      //  dataType: 'json',
      //  async: 'true',
      //  headers: {
      //    'Authorization': 'Basic ' + btoa('rystroa1:password#1'),
      //    'Content-Type': 'application/json'
      //  },
      //  contentType: 'application/json',
      //  data: "{" +
      //    "'EquipmentId':54353742" +
      //    ",'FunctionalLocation':6002082297" +
      //    ",'SafetyConcern':''" +
      //    ",'NotificationNumber':313479752" +
      //    ",'Priority':'5: Within 15 Days'" +
      //    ",'CompanyCode':'1018'" +
      //    ",'Premise':9180464670" +
      //    ",'WorkOrder':512893860" +
      //    ",'Status':'SUSP SSTC'" +
      //    ",'WBSElement':'E18-1420-181906'" +
      //    ",'PlanningPlant':'D205'" +
      //    ",'MatCode':'RBL'" +
      //    ",'MatCodeDescription':'Priority Read for Billing'" +
      //    ",'OperationText':'Priority Read for Billing'" +
      //    ",'FSRId':18508523" +
      //    ",'IsCustomerEnrolledForEmail':'false'" +
      //    ",'SecurityThreat':''" +
      //    ",'PoliceEscort':''" +
      //    ",'AssignmentStart':'2018-03-1909:46:00'" +
      //    ",'AssignmentEnd':'2018-03-1909:58:00'" +
      //    ",'OrderType':'0032'" +
      //    ",'OperationId':'0010'" +
      //    ",'WorkCenter':'123'" +
      //    ",'CustomerAccount':'210025876744'" +
      //    ",'NormalDuration':10.0" +
      //    ",'NormalDurationUnit':'MIN'" +
      //    ",'PlannerGroup':'010'" +
      //  "}",
      //  success: function (d) {
      //    console.log('Result:' + d.Success);
      //  },
      //  error: function (xmlHttpRequest, textStatus, errorThrown) {
      //    console.log('Error:' + errorThrown);
      //  }
      //});
      //$.ajax({
      //  type: 'POST',
      //  url: ELEMENTS.url,
      //  dataType: 'json',
      //  async: 'true',
      //  headers: {
      //    'Authorization': 'Basic ' + btoa('rystroa1:password#1'),
      //    'Content-Type': 'application/json'
      //  },
      //  contentType: 'application/json',
      //  data: "{" +
      //    "'EquipmentId':53990678" +
      //    ",'FunctionalLocation':6002347482" +
      //    ",'SafetyConcern':''" +
      //    ",'NotificationNumber':313483541" +
      //    ",'Priority':'4: Within 5 Days'" +
      //    ",'CompanyCode':'1018'" +
      //    ",'Premise':9180786575" +
      //    ",'WorkOrder':512897402" +
      //    ",'Status':'SUSP SSTC'" +
      //    ",'WBSElement':'E18-1510-181906'" +
      //    ",'PlanningPlant':'D205'" +
      //    ",'MatCode':'RDM'" +
      //    ",'MatCodeDescription':'Read'" +
      //    ",'OperationText':'Read'" +
      //    ",'FSRId':18508523" +
      //    ",'IsCustomerEnrolledForEmail':'false'" +
      //    ",'SecurityThreat':''" +
      //    ",'PoliceEscort':''" +
      //    ",'AssignmentStart':'2018-03-1911:53:00'" +
      //    ",'AssignmentEnd':'2018-03-1912:05:00'" +
      //    ",'OrderType':'0032'" +
      //    ",'OperationId':'0010'" +
      //    ",'WorkCenter':'123'" +
      //    ",'CustomerAccount':'210027288688'" +
      //    ",'NormalDuration':13.0" +
      //    ",'NormalDurationUnit':'MIN'" +
      //    ",'PlannerGroup':'010'" +
      //    "" +
      //    "}", 
      //  success: function (d) {
      //    console.log('Result:' + d.Success);
      //  },
      //  error: function (xmlHttpRequest, textStatus, errorThrown) {
      //    console.log('Error:' + errorThrown);
      //  }
      //});
      //$.ajax({
      //  type: 'POST',
      //  url: ELEMENTS.url,
      //  dataType: 'json',
      //  async: 'true',
      //  headers: {
      //    'Authorization': 'Basic ' + btoa('rystroa1:password#1'),
      //    'Content-Type': 'application/json'
      //  },
      //  contentType: 'application/json',
      //  data: "{" +
      //    "'EquipmentId':52380925" +
      //    ",'FunctionalLocation':6002339732" +
      //    ",'SafetyConcern':''" +
      //    ",'NotificationNumber':303880997" +
      //    ",'Priority':'4: Within 5 Days'" +
      //    ",'CompanyCode':'1018'" +
      //    ",'Premise':9180783245" +
      //    ",'WorkOrder':503652826" +
      //    ",'Status':'CANC ERRO'" +
      //    ",'WBSElement':'E18-1510-181906'" +
      //    ",'PlanningPlant':'D205'" +
      //    ",'MatCode':'REO'" +
      //    ",'MatCodeDescription':'Read_Consec Est - Outside'" +
      //    ",'OperationText':'Read_Consec Est - Outside Set Only'" +
      //    ",'FSRId':18508523" +
      //    ",'IsCustomerEnrolledForEmail':'false'" +
      //    ",'SecurityThreat':''" +
      //    ",'PoliceEscort':''" +
      //    ",'AssignmentStart':'2018-03-2009:27:00'" +
      //    ",'AssignmentEnd':'2018-03-2009:37:00'" +
      //    ",'OrderType':'0032'" +
      //    ",'OperationId':'0010'" +
      //    ",'WorkCenter':'123'" +
      //    ",'CustomerAccount':'210026484131'" +
      //    ",'NormalDuration':7.0" +
      //    ",'NormalDurationUnit':'MIN'" +
      //    ",'PlannerGroup':'010'" +
      //    "" +
      //    "}",
      //  success: function (d) {
      //    console.log('Result:' + d.Success);
      //  },
      //  error: function (xmlHttpRequest, textStatus, errorThrown) {
      //    console.log('Error:' + errorThrown);
      //  }
      //});
      //$.ajax({
      //  type: 'POST',
      //  url: ELEMENTS.url,
      //  dataType: 'json',
      //  async: 'true',
      //  headers: {
      //    'Authorization': 'Basic ' + btoa('rystroa1:password#1'),
      //    'Content-Type': 'application/json'
      //  },
      //  contentType: 'application/json',
      //  data: "{" +
      //    "'EquipmentId':52429599" +
      //    ",'FunctionalLocation':''" +
      //    ",'SafetyConcern':''" +
      //    ",'NotificationNumber':308629520" +
      //    ",'Priority':'4: Within 5 Days'" +
      //    ",'CompanyCode':'1018'" +
      //    ",'Premise':9180473946" +
      //    ",'WorkOrder':508213030" +
      //    ",'Status':'CANC SSTC'" +
      //    ",'WBSElement':''" +
      //    ",'PlanningPlant':'D205'" +
      //    ",'MatCode':'REO'" +
      //    ",'MatCodeDescription':'Read_Consec Est - Outside Set'" +
      //    ",'OperationText':'Read_Consec Est - Outside Set Only'" +
      //    ",'FSRId':18508486" +
      //    ",'IsCustomerEnrolledForEmail':'false'" +
      //    ",'SecurityThreat':''" +
      //    ",'PoliceEscort':''" +
      //    ",'AssignmentStart':'2018-03-1915:14:00'" +
      //    ",'AssignmentEnd':'2018-03-1915:24:00'" +
      //    ",'OrderType':'0032'" +
      //    ",'OperationId':'0010'" +
      //    ",'WorkCenter':'123'" +
      //    ",'CustomerAccount':'210027624123'" +
      //    ",'NormalDuration':14.0" +
      //    ",'NormalDurationUnit':'MIN'" +
      //    ",'PlannerGroup':'010'" +
      //    "}", 
      //  success: function (d) {
      //    console.log('Result:' + d.Success);
      //  },
      //  error: function (xmlHttpRequest, textStatus, errorThrown) {
      //    console.log('Error:' + errorThrown);
      //  }
      //});
      //$.ajax({
      //  type: 'POST',
      //  url: ELEMENTS.url,
      //  dataType: 'json',
      //  async: 'true',
      //  headers: {
      //    'Authorization': 'Basic ' + btoa('rystroa1:password#1'),
      //    'Content-Type': 'application/json'
      //  },
      //  contentType: 'application/json',
      //  data: "{" +
      //    "'EquipmentId':52511431" +
      //    ",'FunctionalLocation':6002060571" +
      //    ",'SafetyConcern':''" +
      //    ",'NotificationNumber':301809926" +
      //    ",'Priority':'5: Within 15 Days'" +
      //    ",'CompanyCode':'1018'" +
      //    ",'Premise':9180463676" +
      //    ",'WorkOrder':501674001" +
      //    ",'Status':'PECA SSTC'" +
      //    ",'WBSElement':'E18-1510-181906'" +
      //    ",'PlanningPlant':'D205'" +
      //    ",'MatCode':'RZC'" +
      //    ",'MatCodeDescription':'Read_Consecutive Zero Usage'" +
      //    ",'OperationText':'Read_Consecutive Zero Usage'" +
      //    ",'FSRId':18508486" +
      //    ",'IsCustomerEnrolledForEmail':'false'" +
      //    ",'SecurityThreat':''" +
      //    ",'PoliceEscort':''" +
      //    ",'AssignmentStart':'2018-03-1913:42:00'" +
      //    ",'AssignmentEnd':'2018-03-1914:00:00'" +
      //    ",'OrderType':'0032'" +
      //    ",'OperationId':'0010'" +
      //    ",'WorkCenter':'123'" +
      //    ",'CustomerAccount':'210025298917'" +
      //    ",'NormalDuration':7.0" +
      //    ",'NormalDurationUnit':'MIN'" +
      //    ",'PlannerGroup':'010'" +
      //    "" +
      //    "}", 
      //  success: function (d) {
      //    console.log('Result:' + d.Success);
      //  },
      //  error: function (xmlHttpRequest, textStatus, errorThrown) {
      //    console.log('Error:' + errorThrown);
      //  }
      //});
      //$.ajax({
      //  type: 'POST',
      //  url: ELEMENTS.url,
      //  dataType: 'json',
      //  async: 'true',
      //  headers: {
      //    'Authorization': 'Basic ' + btoa('rystroa1:password#1'),
      //    'Content-Type': 'application/json'
      //  },
      //  contentType: 'application/json',
      //  data: "{" +
      //    "'EquipmentId':52264559" +
      //    ",'FunctionalLocation':6002094125" +
      //    ",'SafetyConcern':''" +
      //    ",'NotificationNumber':304009706" +
      //    ",'Priority':'5: Within 15 Days'" +
      //    ",'CompanyCode':'1018'" +
      //    ",'Premise':9180464845" +
      //    ",'WorkOrder':503777013" +
      //    ",'Status':'CANC EXPR'" +
      //    ",'WBSElement':'E18-1510-181906'" +
      //    ",'PlanningPlant':'D205'" +
      //    ",'MatCode':'RZC'" +
      //    ",'MatCodeDescription':'Read_Consecutive Zero Usage'" +
      //    ",'OperationText':'Read_Consecutive Zero Usage'" +
      //    ",'FSRId':18508486" +
      //    ",'IsCustomerEnrolledForEmail':'false'" +
      //    ",'SecurityThreat':''" +
      //    ",'PoliceEscort':''" +
      //    ",'AssignmentStart':'2018-03-2009:27:00'" +
      //    ",'AssignmentEnd':'2018-03-2009:37:00'" +
      //    ",'OrderType':'0032'" +
      //    ",'OperationId':'0010'" +
      //    ",'WorkCenter':'123'" +
      //    ",'CustomerAccount':'210025949321'" +
      //    ",'NormalDuration':7.0" +
      //    ",'NormalDurationUnit':'MIN'" +
      //    ",'PlannerGroup':'010'" +
      //    "" +
      //    "}", 
      //  success: function (d) {
      //    console.log('Result:' + d.Success);
      //  },
      //  error: function (xmlHttpRequest, textStatus, errorThrown) {
      //    console.log('Error:' + errorThrown);
      //  }
      //});
      //$.ajax({
      //  type: 'POST',
      //  url: ELEMENTS.url,
      //  dataType: 'json',
      //  async: 'true',
      //  headers: {
      //    'Authorization': 'Basic ' + btoa('rystroa1:password#1'),
      //    'Content-Type': 'application/json'
      //  },
      //  contentType: 'application/json',
      //  data: "{" +
      //    "'EquipmentId':53775009" +
      //    ",'FunctionalLocation':6002314767" +
      //    ",'SafetyConcern':''" +
      //    ",'NotificationNumber':310889793" +
      //    ",'Priority':'5: Within 15 Days'" +
      //    ",'CompanyCode':'1018'" +
      //    ",'Premise':9180721599" +
      //    ",'WorkOrder':510396212" +
      //    ",'Status':'ONST SSTC'" +
      //    ",'WBSElement':'E18-1510-181906'" +
      //    ",'PlanningPlant':'D205'" +
      //    ",'MatCode':'RZC'" +
      //    ",'MatCodeDescription':'Read_Consecutive Zero Usage'" +
      //    ",'OperationText':'Read_Consecutive Zero Usage'" +
      //    ",'FSRId':50013201" +
      //    ",'IsCustomerEnrolledForEmail':'false'" +
      //    ",'SecurityThreat':''" +
      //    ",'PoliceEscort':''" +
      //    ",'AssignmentStart':'2018-03-2010:43:00'" +
      //    ",'AssignmentEnd':'2018-03-2011:01:00'" +
      //    ",'OrderType':'0032'" +
      //    ",'OperationId':'0010'" +
      //    ",'WorkCenter':'123'" +
      //    ",'CustomerAccount':'210025515049'" +
      //    ",'NormalDuration':14.0" +
      //    ",'NormalDurationUnit':'MIN'" +
      //    ",'PlannerGroup':'010'" +
      //    "" +
      //    "}",
      //  success: function (d) {
      //    console.log('Result:' + d.Success);
      //  },
      //  error: function (xmlHttpRequest, textStatus, errorThrown) {
      //    console.log('Error:' + errorThrown);
      //  }
      //});
      //$.ajax({
      //  type: 'POST',
      //  url: ELEMENTS.url,
      //  dataType: 'json',
      //  async: 'true',
      //  headers: {
      //    'Authorization': 'Basic ' + btoa('rystroa1:password#1'),
      //    'Content-Type': 'application/json'
      //  },
      //  contentType: 'application/json',
      //  data: "{" +
      //    "'EquipmentId':52110829" +
      //    ",'FunctionalLocation':6002082318" +
      //    ",'SafetyConcern':''" +
      //    ",'NotificationNumber':310949214" +
      //    ",'Priority':'5: Within 15 Days'" +
      //    ",'CompanyCode':'1018'" +
      //    ",'Premise':9180462570" +
      //    ",'WorkOrder':510453713" +
      //    ",'Status':'ONST SSTC'" +
      //    ",'WBSElement':'E18-1510-181906'" +
      //    ",'PlanningPlant':'D205'" +
      //    ",'MatCode':'RZC'" +
      //    ",'MatCodeDescription':'Read_Consecutive Zero Usage'" +
      //    ",'OperationText':'Read_Consecutive Zero Usage'" +
      //    ",'FSRId':18730706" +
      //    ",'IsCustomerEnrolledForEmail':'false'" +
      //    ",'SecurityThreat':''" +
      //    ",'PoliceEscort':''" +
      //    ",'AssignmentStart':'2018-03-2014:24:00'" +
      //    ",'AssignmentEnd':'2018-03-2014:34:00'" +
      //    ",'OrderType':'0032'" +
      //    ",'OperationId':'0010'" +
      //    ",'WorkCenter':'123'" +
      //    ",'CustomerAccount':'210026558346'" +
      //    ",'NormalDuration':14.0" +
      //    ",'NormalDurationUnit':'MIN'" +
      //    ",'PlannerGroup':'010'" +
      //    "" +
      //    "}",
      //  success: function (d) {
      //    console.log('Result:' + d.Success);
      //  },
      //  error: function (xmlHttpRequest, textStatus, errorThrown) {
      //    console.log('Error:' + errorThrown);
      //  }
      //});
      //$.ajax({
      //  type: 'POST',
      //  url: ELEMENTS.url,
      //  dataType: 'json',
      //  async: 'true',
      //  headers: {
      //    'Authorization': 'Basic ' + btoa('rystroa1:password#1'),
      //    'Content-Type': 'application/json'
      //  },
      //  contentType: 'application/json',
      //  data: "{" +
      //    "'EquipmentId':52110829" +
      //    ",'FunctionalLocation':6002082318" +
      //    ",'SafetyConcern':''" +
      //    ",'NotificationNumber':310949214" +
      //    ",'Priority':'5: Within 15 Days'" +
      //    ",'CompanyCode':'1018'" +
      //    ",'Premise':9180462570" +
      //    ",'WorkOrder':510453713" +
      //    ",'Status':'ONST SSTC'" +
      //    ",'WBSElement':'E18-1510-181906'" +
      //    ",'PlanningPlant':'D205'" +
      //    ",'MatCode':'RZC'" +
      //    ",'MatCodeDescription':'Read_Consecutive Zero Usage'" +
      //    ",'OperationText':'Read_Consecutive Zero Usage'" +
      //    ",'FSRId':18730706" +
      //    ",'IsCustomerEnrolledForEmail':'false'" +
      //    ",'SecurityThreat':''" +
      //    ",'PoliceEscort':''" +
      //    ",'AssignmentStart':'2018-03-2014:24:00'" +
      //    ",'AssignmentEnd':'2018-03-2014:34:00'" +
      //    ",'OrderType':'0032'" +
      //    ",'OperationId':'0010'" +
      //    ",'WorkCenter':'123'" +
      //    ",'CustomerAccount':'210026558346'" +
      //    ",'NormalDuration':14.0" +
      //    ",'NormalDurationUnit':'MIN'" +
      //    ",'PlannerGroup':'010'" +
      //    "}",
      //  success: function (d) {
      //    console.log('Result:' + d.Success);
      //  },
      //  error: function (xmlHttpRequest, textStatus, errorThrown) {
      //    console.log('Error:' + errorThrown);
      //  }
      //});
      //$.ajax({
      //  type: 'POST',
      //  url: ELEMENTS.url,
      //  dataType: 'json',
      //  async: 'true',
      //  headers: {
      //    'Authorization': 'Basic ' + btoa('rystroa1:password#1'),
      //    'Content-Type': 'application/json'
      //  },
      //  contentType: 'application/json',
      //  data: "{" +
      //    "'EquipmentId':52646667" +
      //    ",'FunctionalLocation':6002336606" +
      //    ",'SafetyConcern':''" +
      //    ",'NotificationNumber':307526271" +
      //    ",'Priority':'3: Customer Appt.'" +
      //    ",'CompanyCode':'1018'" +
      //    ",'Premise':9180740258" +
      //    ",'WorkOrder':507150990" +
      //    ",'Status':'ONST SSTC'" +
      //    ",'WBSElement':'E18-1420-181906'" +
      //    ",'PlanningPlant':'D205'" +
      //    ",'MatCode':'FRC'" +
      //    ",'MatCodeDescription':'Read_Move In'" +
      //    ",'OperationText':'Read_Move In'" +
      //    ",'FSRId':18508486" +
      //    ",'IsCustomerEnrolledForEmail':'false'" +
      //    ",'SecurityThreat':''" +
      //    ",'PoliceEscort':''" +
      //    ",'AssignmentStart':'2018-03-1913:58:00'" +
      //    ",'AssignmentEnd':'2018-03-1914:08:00'" +
      //    ",'OrderType':'0030'" +
      //    ",'OperationId':'0010'" +
      //    ",'WorkCenter':'123'" +
      //    ",'CustomerAccount':'220013074849'" +
      //    ",'NormalDuration':10.0" +
      //    ",'NormalDurationUnit':'MIN'" +
      //    ",'PlannerGroup':'010'" +
      //    "" +
      //    "}",
      //  success: function (d) {
      //    console.log('Result:' + d.Success);
      //  },
      //  error: function (xmlHttpRequest, textStatus, errorThrown) {
      //    console.log('Error:' + errorThrown);
      //  }
      //});
      //$.ajax({
      //  type: 'POST',
      //  url: ELEMENTS.url,
      //  dataType: 'json',
      //  async: 'true',
      //  headers: {
      //    'Authorization': 'Basic ' + btoa('rystroa1:password#1'),
      //    'Content-Type': 'application/json'
      //  },
      //  contentType: 'application/json',
      //  data: "{" +
      //    "'EquipmentId':52550524" +
      //    ",'FunctionalLocation':6002325725" +
      //    ",'SafetyConcern':'cust has a dog-req restraining'" +
      //    ",'NotificationNumber':307640742" +
      //    ",'Priority':'3: Customer Appt.'" +
      //    ",'CompanyCode':'1018'" +
      //    ",'Premise':9180742238" +
      //    ",'WorkOrder':507262014" +
      //    ",'Status':'ONST SSTC'" +
      //    ",'WBSElement':'E18-1420-181906'" +
      //    ",'PlanningPlant':'D205'" +
      //    ",'MatCode':'FRC'" +
      //    ",'MatCodeDescription':'Read_Move In'" +
      //    ",'OperationText':'Read_Move In'" +
      //    ",'FSRId':18508486" +
      //    ",'IsCustomerEnrolledForEmail':'false'" +
      //    ",'SecurityThreat':''" +
      //    ",'PoliceEscort':''" +
      //    ",'AssignmentStart':'2018-03-1913:58:00'" +
      //    ",'AssignmentEnd':'2018-03-1914:08:00'" +
      //    ",'OrderType':'0030'" +
      //    ",'OperationId':'0010'" +
      //    ",'WorkCenter':'123'" +
      //    ",'CustomerAccount':'220013242499'" +
      //    ",'NormalDuration':10.0" +
      //    ",'NormalDurationUnit':'MIN'" +
      //    ",'PlannerGroup':'010'" +
      //    "" +
      //    "}",
      //  success: function (d) {
      //    console.log('Result:' + d.Success);
      //  },
      //  error: function (xmlHttpRequest, textStatus, errorThrown) {
      //    console.log('Error:' + errorThrown);
      //  }
      //});
      //$.ajax({
      //  type: 'POST',
      //  url: ELEMENTS.url,
      //  dataType: 'json',
      //  async: 'true',
      //  headers: {
      //    'Authorization': 'Basic ' + btoa('rystroa1:password#1'),
      //    'Content-Type': 'application/json'
      //  },
      //  contentType: 'application/json',
      //  data: "{" +
      //    "'EquipmentId':53517897" +
      //    ",'FunctionalLocation':6002091318" +
      //    ",'SafetyConcern':''" +
      //    ",'NotificationNumber':308236261" +
      //    ",'Priority':'3: Customer Appt.'" +
      //    ",'CompanyCode':'1018'" +
      //    ",'Premise':9180466623" +
      //    ",'WorkOrder':507833095" +
      //    ",'Status':'ONST SSTC'" +
      //    ",'WBSElement':'E18-1420-181906'" +
      //    ",'PlanningPlant':'D205'" +
      //    ",'MatCode':'FRC'" +
      //    ",'MatCodeDescription':'Read_Move In'" +
      //    ",'OperationText':'Read_Move In'" +
      //    ",'FSRId':18508566" +
      //    ",'IsCustomerEnrolledForEmail':'false'" +
      //    ",'SecurityThreat':''" +
      //    ",'PoliceEscort':''" +
      //    ",'AssignmentStart':'2018-03-1908:51:00'" +
      //    ",'AssignmentEnd':'2018-03-1909:04:00'" +
      //    ",'OrderType':'0030'" +
      //    ",'OperationId':'0010'" +
      //    ",'WorkCenter':'123'" +
      //    ",'CustomerAccount':'210026787335'" +
      //    ",'NormalDuration':10.0" +
      //    ",'NormalDurationUnit':'MIN'" +
      //    ",'PlannerGroup':'010'" +
      //    "" +
      //    "}",
      //  success: function (d) {
      //    console.log('Result:' + d.Success);
      //  },
      //  error: function (xmlHttpRequest, textStatus, errorThrown) {
      //    console.log('Error:' + errorThrown);
      //  }
      //});
      //$.ajax({
      //  type: 'POST',
      //  url: ELEMENTS.url,
      //  dataType: 'json',
      //  async: 'true',
      //  headers: {
      //    'Authorization': 'Basic ' + btoa('rystroa1:password#1'),
      //    'Content-Type': 'application/json'
      //  },
      //  contentType: 'application/json',
      //  data: "{" +
      //    "'EquipmentId':53959515" +
      //    ",'FunctionalLocation':6002353381" +
      //    ",'SafetyConcern':''" +
      //    ",'NotificationNumber':309972970" +
      //    ",'Priority':'3: Customer Appt.'" +
      //    ",'CompanyCode':'1018'" +
      //    ",'Premise':9180786807" +
      //    ",'WorkOrder':509507968" +
      //    ",'Status':'ONST SSTC'" +
      //    ",'WBSElement':'E18-1420-181906'" +
      //    ",'PlanningPlant':'D205'" +
      //    ",'MatCode':'FRC'" +
      //    ",'MatCodeDescription':'Read_Move In'" +
      //    ",'OperationText':'Read_Move In'" +
      //    ",'FSRId':18508486" +
      //    ",'IsCustomerEnrolledForEmail':'false'" +
      //    ",'SecurityThreat':''" +
      //    ",'PoliceEscort':''" +
      //    ",'AssignmentStart':'2018-03-1909:03:00'" +
      //    ",'AssignmentEnd':'2018-03-1909:13:00'" +
      //    ",'OrderType':'0030'" +
      //    ",'OperationId':'0010'" +
      //    ",'WorkCenter':'123'" +
      //    ",'CustomerAccount':'220017368362'" +
      //    ",'NormalDuration':10.0" +
      //    ",'NormalDurationUnit':'MIN'" +
      //    ",'PlannerGroup':'010'" +
      //    "" +
      //    "}",
      //  success: function (d) {
      //    console.log('Result:' + d.Success);
      //  },
      //  error: function (xmlHttpRequest, textStatus, errorThrown) {
      //    console.log('Error:' + errorThrown);
      //  }
      //});
      //$.ajax({
      //  type: 'POST',
      //  url: ELEMENTS.url,
      //  dataType: 'json',
      //  async: 'true',
      //  headers: {
      //    'Authorization': 'Basic ' + btoa('rystroa1:password#1'),
      //    'Content-Type': 'application/json'
      //  },
      //  contentType: 'application/json',
      //  data: "{" +
      //    "'EquipmentId':52447041" +
      //    ",'FunctionalLocation':6002066377" +
      //    ",'SafetyConcern':''" +
      //    ",'NotificationNumber':304635076" +
      //    ",'Priority':'3: Customer Appt.'" +
      //    ",'CompanyCode':'1018'" +
      //    ",'Premise':9180464132" +
      //    ",'WorkOrder':504370864" +
      //    ",'Status':'CANC SSTC'" +
      //    ",'WBSElement':'E18-1420-181906'" +
      //    ",'PlanningPlant':'D205'" +
      //    ",'MatCode':'IRD'" +
      //    ",'MatCodeDescription':'Read_Move In-Water is On'" +
      //    ",'OperationText':'Read_Move In-Water is On'" +
      //    ",'FSRId':18508486" +
      //    ",'IsCustomerEnrolledForEmail':'false'" +
      //    ",'SecurityThreat':''" +
      //    ",'PoliceEscort':''" +
      //    ",'AssignmentStart':'2018-03-1911:35:00'" +
      //    ",'AssignmentEnd':'2018-03-1911:55:00'" +
      //    ",'OrderType':'0030'" +
      //    ",'OperationId':'0010'" +
      //    ",'WorkCenter':'123'" +
      //    ",'CustomerAccount':'220008375571'" +
      //    ",'NormalDuration':7.0" +
      //    ",'NormalDurationUnit':'MIN'" +
      //    ",'PlannerGroup':'010'" +
      //    "" +
      //    "}",
      //  success: function (d) {
      //    console.log('Result:' + d.Success);
      //  },
      //  error: function (xmlHttpRequest, textStatus, errorThrown) {
      //    console.log('Error:' + errorThrown);
      //  }
      //});
      //$.ajax({
      //  type: 'POST',
      //  url: ELEMENTS.url,
      //  dataType: 'json',
      //  async: 'true',
      //  headers: {
      //    'Authorization': 'Basic ' + btoa('rystroa1:password#1'),
      //    'Content-Type': 'application/json'
      //  },
      //  contentType: 'application/json',
      //  data: "{" +
      //    "'EquipmentId':54247273" +
      //    ",'FunctionalLocation':6002060661" +
      //    ",'SafetyConcern':''" +
      //    ",'NotificationNumber':308779207" +
      //    ",'Priority':'3: Customer Appt.'" +
      //    ",'CompanyCode':'1018'" +
      //    ",'Premise':9180461686" +
      //    ",'WorkOrder':508357120" +
      //    ",'Status':'ONST SSTC'" +
      //    ",'WBSElement':'E18-1420-181906'" +
      //    ",'PlanningPlant':'D205'" +
      //    ",'MatCode':'MON'" +
      //    ",'MatCodeDescription':'READ_Move In/Off Married'" +
      //    ",'OperationText':'READ_Move In/Off Married'" +
      //    ",'FSRId':18647890" +
      //    ",'IsCustomerEnrolledForEmail':'false'" +
      //    ",'SecurityThreat':''" +
      //    ",'PoliceEscort':''" +
      //    ",'AssignmentStart':'2018-03-1911:07:00'" +
      //    ",'AssignmentEnd':'2018-03-1911:25:00'" +
      //    ",'OrderType':'0030'" +
      //    ",'OperationId':'0010'" +
      //    ",'WorkCenter':'123'" +
      //    ",'CustomerAccount':'220015441797'" +
      //    ",'NormalDuration':13.0" +
      //    ",'NormalDurationUnit':'MIN'" +
      //    ",'PlannerGroup':'010'" +
      //    "" +
      //    "}",
      //  success: function (d) {
      //    console.log('Result:' + d.Success);
      //  },
      //  error: function (xmlHttpRequest, textStatus, errorThrown) {
      //    console.log('Error:' + errorThrown);
      //  }
      //});
      //$.ajax({
      //  type: 'POST',
      //  url: ELEMENTS.url,
      //  dataType: 'json',
      //  async: 'true',
      //  headers: {
      //    'Authorization': 'Basic ' + btoa('rystroa1:password#1'),
      //    'Content-Type': 'application/json'
      //  },
      //  contentType: 'application/json',
      //  data: "{" +
      //    "'EquipmentId':52386639" +
      //    ",'FunctionalLocation':6002064278" +
      //    ",'SafetyConcern':''" +
      //    ",'NotificationNumber':313326755" +
      //    ",'Priority':'5: Within 15 Days'" +
      //    ",'CompanyCode':'1018'" +
      //    ",'Premise':9180472161" +
      //    ",'WorkOrder':512745651" +
      //    ",'Status':'SUSP SSTC'" +
      //    ",'WBSElement':'E18-2435-181906'" +
      //    ",'PlanningPlant':'D205'" +
      //    ",'MatCode':'RID'" +
      //    ",'MatCodeDescription':'Repair/Install Reading Device'" +
      //    ",'OperationText':'Repair/Install Reading Device'" +
      //    ",'FSRId':18508523" +
      //    ",'IsCustomerEnrolledForEmail':'false'" +
      //    ",'SecurityThreat':''" +
      //    ",'PoliceEscort':''" +
      //    ",'AssignmentStart':'2018-03-2009:16:00'" +
      //    ",'AssignmentEnd':'2018-03-2009:34:00'" +
      //    ",'OrderType':'0032'" +
      //    ",'OperationId':'0010'" +
      //    ",'WorkCenter':'123'" +
      //    ",'CustomerAccount':'210026138841'" +
      //    ",'NormalDuration':21.0" +
      //    ",'NormalDurationUnit':'MIN'" +
      //    ",'PlannerGroup':'010'" +
      //    "" +
      //    "}",
      //  success: function (d) {
      //    console.log('Result:' + d.Success);
      //  },
      //  error: function (xmlHttpRequest, textStatus, errorThrown) {
      //    console.log('Error:' + errorThrown);
      //  }
      //});
      //$.ajax({
      //  type: 'POST',
      //  url: ELEMENTS.url,
      //  dataType: 'json',
      //  async: 'true',
      //  headers: {
      //    'Authorization': 'Basic ' + btoa('rystroa1:password#1'),
      //    'Content-Type': 'application/json'
      //  },
      //  contentType: 'application/json',
      //  data: "{" +
      //    "'EquipmentId':53518949" +
      //    ",'FunctionalLocation':6002080934" +
      //    ",'SafetyConcern':''" +
      //    ",'NotificationNumber':313326757" +
      //    ",'Priority':'5: Within 15 Days'" +
      //    ",'CompanyCode':'1018'" +
      //    ",'Premise':9180476550" +
      //    ",'WorkOrder':512745653" +
      //    ",'Status':'SUSP SSTC'" +
      //    ",'WBSElement':'E18-2435-181906'" +
      //    ",'PlanningPlant':'D205'" +
      //    ",'MatCode':'RID'" +
      //    ",'MatCodeDescription':'Repair/Install Reading Device'" +
      //    ",'OperationText':'Repair/Install Reading Device'" +
      //    ",'FSRId':18730706" +
      //    ",'IsCustomerEnrolledForEmail':'false'" +
      //    ",'SecurityThreat':''" +
      //    ",'PoliceEscort':''" +
      //    ",'AssignmentStart':'2018-03-2014:32:00'" +
      //    ",'AssignmentEnd':'2018-03-2014:46:00'" +
      //    ",'OrderType':'0032'" +
      //    ",'OperationId':'0010'" +
      //    ",'WorkCenter':'123'" +
      //    ",'CustomerAccount':'220021024234'" +
      //    ",'NormalDuration':21.0" +
      //    ",'NormalDurationUnit':'MIN'" +
      //    ",'PlannerGroup':'010'" +
      //    "" +
      //    "}",

      //  success: function (d) {
      //    console.log('Result:' + d.Success);
      //  },
      //  error: function (xmlHttpRequest, textStatus, errorThrown) {
      //    console.log('Error:' + errorThrown);
      //  }
      //});
      //$.ajax({
      //  type: 'POST',
      //  url: ELEMENTS.url,
      //  dataType: 'json',
      //  async: 'true',
      //  headers: {
      //    'Authorization': 'Basic ' + btoa('rystroa1:password#1'),
      //    'Content-Type': 'application/json'
      //  },
      //  contentType: 'application/json',
      //  data: "{" +
      //    "'EquipmentId':''" +
      //    ",'FunctionalLocation':6003744055" +
      //    ",'SafetyConcern':''" +
      //    ",'NotificationNumber':307618098" +
      //    ",'Priority':'3: Customer Appt.'" +
      //    ",'CompanyCode':'1018'" +
      //    ",'Premise':5000055050" +
      //    ",'WorkOrder':507240032" +
      //    ",'Status':'INCM 2PSN'" +
      //    ",'WBSElement':''" +
      //    ",'PlanningPlant':'D205'" +
      //    ",'MatCode':'STO'" +
      //    ",'MatCodeDescription':'Set Meter, Turn On,'" +
      //    ",'OperationText':'Set Meter, Turn On, Read_Move In'" +
      //    ",'FSRId':18647890" +
      //    ",'IsCustomerEnrolledForEmail':'false'" +
      //    ",'SecurityThreat':''" +
      //    ",'PoliceEscort':''" +
      //    ",'AssignmentStart':'2018-03-1913:42:00'" +
      //    ",'AssignmentEnd':'2018-03-1913:56:00'" +
      //    ",'OrderType':'0030'" +
      //    ",'OperationId':'0010'" +
      //    ",'WorkCenter':'123'" +
      //    ",'CustomerAccount':'220013222570'" +
      //    ",'NormalDuration':18.0" +
      //    ",'NormalDurationUnit':'MIN'" +
      //    ",'PlannerGroup':'010'" +
      //    "}",
      //  success: function (d) {
      //    console.log('Result:' + d.Success);
      //  },
      //  error: function (xmlHttpRequest, textStatus, errorThrown) {
      //    console.log('Error:' + errorThrown);
      //  }
      //});
      //$.ajax({
      //  type: 'POST',
      //  url: ELEMENTS.url,
      //  dataType: 'json',
      //  async: 'true',
      //  headers: {
      //    'Authorization': 'Basic ' + btoa('rystroa1:password#1'),
      //    'Content-Type': 'application/json'
      //  },
      //  contentType: 'application/json',
      //  data: "{" +
      //    "'EquipmentId':''" +
      //    ",'FunctionalLocation':6003755231" +
      //    ",'SafetyConcern':' Pets on Premise'" +
      //    ",'NotificationNumber':307827131" +
      //    ",'Priority':'3: Customer Appt.'" +
      //    ",'CompanyCode':'1018'" +
      //    ",'Premise':5000061143" +
      //    ",'WorkOrder':507441880" +
      //    ",'Status':'ONST SSTC'" +
      //    ",'WBSElement':'R18-19I1.16-P-0001'" +
      //    ",'PlanningPlant':'D205'" +
      //    ",'MatCode':'STO'" +
      //    ",'MatCodeDescription':'Set Meter, Turn On, '" +
      //    ",'OperationText':'Set Meter, Turn On, Read_Move In'" +
      //    ",'FSRId':18508494" +
      //    ",'IsCustomerEnrolledForEmail':'false'" +
      //    ",'SecurityThreat':''" +
      //    ",'PoliceEscort':''" +
      //    ",'AssignmentStart':'2018-03-1912:51:00'" +
      //    ",'AssignmentEnd':'2018-03-1913:11:00'" +
      //    ",'OrderType':'0030'" +
      //    ",'OperationId':'0010'" +
      //    ",'WorkCenter':'123'" +
      //    ",'CustomerAccount':'220013750518'" +
      //    ",'NormalDuration':18.0" +
      //    ",'NormalDurationUnit':'MIN'" +
      //    ",'PlannerGroup':'010'" +
      //    "}",
      //  success: function (d) {
      //    console.log('Result:' + d.Success);
      //  },
      //  error: function (xmlHttpRequest, textStatus, errorThrown) {
      //    console.log('Error:' + errorThrown);
      //  }
      //});
    },
    update: function () {
      $.ajax({
        type: 'POST',
        url: ELEMENTS.url,
        dataType: 'json',
        async: 'true',
        headers: {
          'Authorization': 'Basic ' + btoa('rystroa1:password#1'),
          'Content-Type': 'application/json'
        },
        contentType: 'application/json',
        data: "{" +
          "'EquipmentId': '121212', " +
          "'FunctionalLocation': '512925255', " +
          "'SafetyConcern': 'Yes', " +
          "'NotificationNumber': '5129', " +
          "'Priority': 'LOW', " +
          "'BackReportingType': 'BT', " +
          "'CompanyCode': 'NJAW', " +
          "'Premise': '1234567890', " +
          "'WorkOrder': '512925255', " +
          "'Status': 'ASSIGNED', " +
          "'WBSElement': '123456', " +
          "'PlanningPlant': 'D218', " +
          "'MATCode': 'PBC', " +
          "'MATCodeDescription': 'Possible Billable Claim', " +
          "'OperationText': '1234567890123456789012345678901234567890', " +
          "'FSRId': '333', " +
          "'FSRName': '333', " +
          "'CustomerNumber': '1234567890', " +
          "'NextReplacementYear': '2020', " +
          "'ServiceType': 'WT', " +
          "'MeterSerialNumber': '123456789012345678', " +
          "'ManufacturerSerialNumber': '12345676', " +
          "'IsCustomerEnrolledForEmail': 'true', " +
          "'SecurityThreat': 'Yes', " +
          "'PoliceEscort': 'Yes', " +
          "'AssignmentStart': '2018-03-06T16:39:00Z', " +
          "'AssignmentEnd': '2018-03-06T17:12:00Z', " +
          "'OrderType': '', " +
          "'OperationId': '123', " +
          "'WorkCenter': '1234', " +
          "'CustomerAccount': '1234', " +
          "'NormalDuration': '1', " +
          "'NormalDurationUnit': 'NDU', " +
          "'PlannerGroup': 'PG'}",
        success: function (d) {
          console.log('Result:' + d.Success);
        },
        error: function (xmlHttpRequest, textStatus, errorThrown) {
          console.log('Error:' + errorThrown);
        }
      });
    }
  };

  $(document).ready(scwo.initialize);
  return scwo;
})(jQuery);