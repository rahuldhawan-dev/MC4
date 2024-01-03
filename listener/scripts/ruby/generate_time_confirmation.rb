require_relative 'common'
require 'json'
require 'stomp'

SERVER = 'dev'

config = Config.new[SERVER]
topic = 'com.amwater.work1v.so-time-confirmation.topic'
hash = {
  WorkOrderNumber: "512933889",
  OperationId: "1",
  DateCompleted: "20180524",
  WorkCenter: "B",
  PersonnelNumber: "1230001",
  ActualWork: 12.5,
  UnitOfMeasure: "H",
  FinalConfirmation: "X",
  NoRemainingWork: "X",
  WorkStartDate: "20180524",
  WorkStartTime: "090000",
  WorkFinishDate: "20180524",
  WorkFinishTime: "094500",
  ConfirmationText: "Work has been completed"
}

client = Stomp::Client.new config['login'], config['password'], config['host'], config['port'], true
client.publish topic, JSON.generate(hash), {:persistent => true}
