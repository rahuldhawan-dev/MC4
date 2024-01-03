require_relative 'common'
require 'json'
require 'stomp'

SERVER = 'dev'

config = Config.new[SERVER]
topic = 'com.amwater.work1v.so-status-update.topic'
hash = {
  WorkOrderNumber: "522517437",
  OperationNumber: "0010",
  AssignmentStart: "2022-03-2512:52:25",
  AssignmentEnd: "2022-03-2500:31:00",
  StatusNumber: "AKNW",
  AssignedEngineer: "50374580",
  DispatchId: "modih",
  EngineerId: "modih",
  ItemTimeStamp: "2022-03-1112:39:00"
}

client = Stomp::Client.new config['login'], config['password'], config['host'], config['port'], true
client.publish topic, JSON.generate(hash), {:persistent => true}
