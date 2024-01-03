require_relative 'common'
require 'json'
require 'stomp'

SERVER = 'dev'

config = Config.new[SERVER]
topic = 'com.amwater.work1v.so-completion.topic'
hash = {
  WorkOrderNumber: "514190003",
  MiscInvoice: "MISC",
  BackOfficeReview: "BACK",
  CompletionStatus: "",
  Notes: "Removed from  Device Location",
  AdditionalWorkNeeded: "F23",
  Purpose: "I03",
  TechnicalInspectedOn: "20180703",
  TechnicalInspectedBy: "AGRAWAA",
  ServiceFound: "I02",
  ServiceLeft: "I06",
  OperatedPointOfControl: "C01",
  AdditionalInformation: "TEST FOR SO COMPLETION",
  CurbBoxMeasurementDescription: "Control",
  Safety: "Safety Text 1",
  HeatType: "N",
  MeterPositionLocation: "1A",
  MeterDirectionalLocation: "FR",
  MeterSupplementalLocation: "IS",
  ReadingDevicePositionalLocation: "OS",
  ReadingDeviceSupplementalLocation: "1B",
  ReadingDeviceDirectionalLocation: "LS",
  FSRID: "18502527",
  SerialNumber: "14514030",
  TestResults: [],
  MeterSerialNumber: "12693277",
  OldMeterSerialNumber: "12693277",
  DeviceCategory: "1101509",
  Installation: "7002024528",
  Registers: [
    { Size: "asd", MIUNumber: "12asda12", EncoderId: "ASDASD", OldRead: "", NewRead: "0900", ReadType: "12", Dials: "" }
  ],
  ActionFlag: "O",
  ActivityReason: "",
  Activities: [{ "Description":"I01"},{"Description":"I02"},{"Description":"I03"},{"Description":"I04"},{"Description":"I05"},{"Description":"I06"},{"Description":"I07"},{"Description":"I08"},{"Description":"I09"},{"Description":"I10"}]
}

client = Stomp::Client.new config['login'], config['password'], config['host'], config['port'], true
client.publish topic, JSON.generate(hash), {:persistent => true}
