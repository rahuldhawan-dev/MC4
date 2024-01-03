require 'stomp'

host = 'hsynlamqs001.amwaternp.net'
topic = 'com.amwater.work1v.so-status-response.topic'
client = Stomp::Client.new '', '', host, 61616, true

client.subscribe topic do |msg|
  puts msg
end

puts 'Press RETURN to exit'
while !gets.chomp.empty?
end
