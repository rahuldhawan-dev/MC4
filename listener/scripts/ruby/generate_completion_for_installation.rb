require_relative 'common'
require 'json'
require 'stomp'

SERVER = 'qa'

config = Config.new[SERVER]
topic = 'com.amwater.work1v.so-completion.topic'
hash = {
  WorkOrderNumber: "512924954",
  MiscInvoice: "MISC",
  BackOfficeReview: "BACK",
  CompletionStatus: "COMP",
  Notes: "Device Installed Device Location",
  Activity1: "I18",
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
  SerialNumber: "14514030",
  TestResults: [],
  MeterSerialNumber: "14514030",
  OldMeterSerialNumber: "14514029",
  DeviceCategory: "1101801",
  Installation: "7001467783",
  Registers: [
    { Size: "", MIUNumber: "", EncoderId: "", OldRead: "00076", NewRead: "00001", ReadType: "", Dials: "04" }
  ],
  ActionFlag: "I",
  ActivityReason: "ABC"
}

client = Stomp::Client.new config['login'], config['password'], config['host'], config['port'], true
client.publish topic, JSON.generate(hash), {:persistent => true}
