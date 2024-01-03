require_relative 'common'
require 'json'
require 'stomp'

SERVER = 'dev'

config = Config.new[SERVER]
topic = 'com.amwater.mtrops.bpem-create.topic'
hash = {
  CaseCategory: "ZC14",
  CasePriority: "4",
  AuthorizationGroup: "",
  ObjectType: "ISUPARTNER",
  ObjectKey: "1100441425",
  OriginalDateOfClarificationCase: "20180731",
  CreationTimeOfClarificationCase: "8:55:07",
  LogicalSystem: "",
  CompanyCode: "1026",
  BusinessPartnerNumber: "1100441425",
  ContractAccountNumber: "210002109199",
  Premise: "9090038914"
}

client = Stomp::Client.new config['login'], config['password'], config['host'], config['port'], true
client.publish topic, JSON.generate(hash), {:persistent => true}
