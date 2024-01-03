require_relative 'common'
require 'json'
require 'stomp'

SERVER = 'dev'

config = Config.new[SERVER]
topic = 'com.amwater.work1v.so-completion.topic'
client = Stomp::Client.new config['login'], config['password'], config['host'], config['port'], true
jsontext = '    {
        "workOrderNumber": "519327173",
        "miscInvoice": "",
        "backOfficeReview": "",
        "completionStatus": "COMP",
        "notes": " [alkucha] [2020-03-24 19:09:00 IST]\nPremise:\n - Meter Location - Outside, Front Left Side, Curb\n\nMeter:\n - 69232713 Device Location -  10 R OF LHL \n[marienjp] [12/16/2015 12:52 PM]              RF1490493482 \n[reesem] [03/16/2016 03:56 PM]\n\n - 69232713 - Register 01 - RF/MIU - 1490493482\n - 69232713 - Register 01 - Collection Type - AMR Neptune\n - 69232713 - Register 01 -  New Meter Reading -  - 0099\n\nField:\n - Service Found - I05 On at Pit\n - Service Left - I05 ON at Pit\n - 69232713 Meter Activities - Checked Leak at Curb/Street\n - Sewer Inspection Reason - Downspout\n- Window Well Drain\n- Area Drain\n\nCompletion:\n - Status - Complete\n - Miscellaneous/Review Comments - \n\n",
        "activities": [
          {
            "description": "I06"
          }
        ],
        "additionalWorkNeeded": "",
        "purpose": "",
        "technicalInspectedOn": "20200324",
        "technicalInspectedBy": "alkucha",
        "serviceFound": "I05",
        "serviceLeft": "I05",
        "operatedPointOfControl": "C02",
        "additionalInformation": " 10 R OF LHL \n[marienjp] [12/16/2015 12:52 PM]              RF1490493482 \n[reesem] [03/16/2016 03:56 PM]\n",
        "curbBoxMeasurementDescription": "    10 R of LHL RF#1470613878 stop on yoke\n\n",
"safety": "",
"heatType": "",
"meterPositionLocation": "4A",
"meterDirectionalLocation": "FL",
"meterSupplementalLocation": "OS",
"readingDevicePositionalLocation": "",
"readingDeviceSupplementalLocation": "IS",
"readingDeviceDirectionalLocation": "FL",
"fSRId": "18502084",
"fSRInteraction": "",
"serialNumber": "69232713",
"oldMeterSerialNumber": "",
"registers": [
               {
                 "size": "5/8\"",
                "mIUNumber": "1490493482",
                "encoderId": "",
                "oldRead": "0087",
                "readType": "11",
                "newRead": "0099",
                "dials": "04"
               }
             ],
"meterSerialNumber": "69232713",
"manufacturerSerialNumber": "69232713",
"deviceCategory": "000000000001102786",
"installation": "7002254136",
"actionFlag": "O",
"activityReason": "",
"rating": "",
"qualityIssues": [],
"inspectionDate": "20200324",
"inspectionPassed": "N",
"violationCodes": [
                    {
                      "violationCode": 1
                    },
                    {
                      "violationCode": 8
                    },
                    {
                      "violationCode": 3
                    }
                  ],
"register1ReasonCode": "",
"register2ReasonCode": "",
"investigationExpiryDate": "",
"notificationItemText": "",
"latitude": "",
"longitude": "",
"needTwoManCrew": "N",
"leakDetectedNonCompany": "N",
"leakDetectedDate": "",
"InternalLeadPipingIndicator":"Y",
"LeadInspectionDate":"20200501",
"LeadInspectedBy":"F123",
"CustomerSideMaterial":"Copper",
"testResults": [
                 {
                   "registerId": "",
                  "lowMedHighIndicator": "",
                  "initialRepair": "",
                  "accuracy": "",
                  "calculatedVolume": "",
                  "testFlowRate": "",
                  "startRead": "",
                  "endRead": ""
                 },
                 {
                   "registerId": "",
                  "lowMedHighIndicator": "",
                  "initialRepair": "",
                  "accuracy": "",
                  "calculatedVolume": "",
                  "testFlowRate": "",
                  "startRead": "",
                  "endRead": ""
                 }
               ]
}'

puts jsontext

client.publish topic, jsontext, {:persistent => true}
