require_relative 'common'
require 'json'
require 'stomp'

SERVER = 'dev'

config = Config.new[SERVER]
topic = 'com.amwater.work1v.so-recmnd-status.topic'
hash = {
  WorkOrderNumber: "517670624",
  OperationNumber: "0010",
  AssignmentStart: "2019-03-1212:52:25",
  AssignmentEnd: "2019-03-1400:31:00",
  StatusNumber: "ENRT",
  AssignedEngineer: "50374580",
  DispatchId: "modih",
  EngineerId: "modih",
  ItemTimeStamp: "2019-03-1312:39:00"
}

client = Stomp::Client.new config['login'], config['password'], config['host'], config['port'], true
client.publish topic, JSON.generate(hash), {:persistent => true}
