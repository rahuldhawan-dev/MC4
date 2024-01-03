require_relative 'common'
require 'stomp'

SERVER = 'dev'

config = Config.new[SERVER]
topic = 'com.amwater.work1v.so-completion-other.topic'
client_id = 'mc-' + topic.gsub('.', '-')

client = Stomp::Client.new(
  {
    hosts: [{login: config['login'], password: config['password'], host: config['host'], port: config['port']}],
    connect_headers: {'client-id' => client_id}
  })

client.subscribe topic, {'ack' => 'client', 'activemq.subscriptionName' => client_id} do |msg|
  puts "######HEADERS#######"
  puts msg.headers
  puts "########BODY########"
  puts msg.body
  client.acknowledge msg
end

client.join

puts 'Press RETURN to exit'
while !gets.chomp.empty?
end
