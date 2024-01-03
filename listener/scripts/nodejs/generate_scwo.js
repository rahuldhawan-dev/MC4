var Stomp = require('stomp-client');
var client = new Stomp('hsynlamqs001.amwaternp.net', 61616, '', '');

client.connect(function(sessionId) {
    client.publish('com.amwater.work1v.so-status-update.topic', '{ "WorkOrderNumber": "512925257123", "OperationNumber": "1234", "AssignmentStart": "2018-03-06T15:39:00Z", "AssignmentEnd": "2018-03-06T16:12:00Z", "Status": "AKNW", "AssignedEngineer": "50193841", "DispatchId": "AWWNP\\AGRAWA", "EngineerId": "AGRAWAA", "ItemTimeStamp": "2018-03-06T16:12:00Z" }');
});
