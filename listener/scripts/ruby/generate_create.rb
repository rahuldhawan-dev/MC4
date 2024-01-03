require_relative 'common'
require 'json'
require 'stomp'

SERVER = 'qa'

config = Config.new[SERVER]
topic = 'com.amwater.work1v.so-dispatch.topic'
hash = {
  IsUpdate: "true",
  Priority: "3: Customer Appt.",
  Status: "Dispatched",
  WorkOrder: "514202261",
}

client = Stomp::Client.new config['login'], config['password'], config['host'], config['port'], true
client.publish topic, JSON.generate(hash), {:persistent => true}
