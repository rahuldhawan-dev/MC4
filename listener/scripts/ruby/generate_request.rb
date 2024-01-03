require_relative 'common'
require 'json'
require 'stomp'

SERVER = 'dev'

config = Config.new[SERVER]
topic = 'com.amwater.work1v.so-create.topic'
hash = {
  BusinessPartnerNumber: "1102209708",
  ContractAccount: "210023048924",
  Equipment: "52428562",
  FunctionalLocation: "6001991114",
  Installation: "7001945175",
  MaintenanceActivityType: "RDM",
  ManufacturerSerialNumber: "86257827",
  WorkOrderLongText: "Test for WO long text",
  UniqueId: "123-12A",
  DeviceLocation:"6003691819",
  SerialNumber:"000000000013555767",
  FSRId: 6001234
}

client = Stomp::Client.new config['login'], config['password'], config['host'], config['port'], true
client.publish topic, JSON.generate(hash), {:persistent => true}
