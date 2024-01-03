require_relative 'common'
require 'json'
require 'stomp'

SERVER = 'dev'

config = Config.new[SERVER]
topic = 'com.amwater.work1v.so-completion.topic'
hash = {
  WorkOrderNumber: "514205175",
  MiscInvoice: "MISC",
  BackOfficeReview: "",
  CompletionStatus: "COMP",
  Notes: "Device REPLACE Device Location",
  Activity1: "I17",
  Activity2: "I10",
  Activity3: "I15",
  AdditionalWorkNeeded: "F23",
  Purpose: "I03",
  TechnicalInspectedOn: "20180703",
  TechnicalInspectedBy: "SS3",
  ServiceFound: "I01",
  ServiceLeft: "I01",
  OperatedPointOfControl: "C01",
  AdditionalInformation: "Install",
  CurbBoxMeasurementDescription: "Point of Control",
  Safety: "Safety Text 1",
  HeatType: "N",
  MeterPositionLocation: "1A",
  MeterDirectionalLocation: "FR",
  MeterSupplementalLocation: "IS",
  ReadingDevicePositionalLocation: "OS",
  ReadingDeviceSupplementalLocation: "1B",
  ReadingDeviceDirectionalLocation: "LS",
  FSRID: "60003847",
  SerialNumber: "26798875",
  TestResults: [],
  MeterSerialNumber: "26798875",
  OldMeterSerialNumber: "26790535",
  DeviceCategory: "1102952",
  Installation: "7002227995",
  Registers: [
    { Size: "", MIUNumber: "", EncoderId: "", OldRead: "0020", NewRead: "0000", ReadType: "", Dials: "04" }
  ],
  ActionFlag: "E",
  ActivityReason: "12",
  QualityIssue: "Q01"
}

client = Stomp::Client.new config['login'], config['password'], config['host'], config['port'], true
client.publish topic, JSON.generate(hash), {:persistent => true}
