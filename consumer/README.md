## Running 

This project can be run locally by switching to the `Debug` configuration and pressing play within Visual Studio. 

## Installing as a local service

1. Build the project in the desired configuration
2. Execute the following commands where your desired configuration replaces the token [TARGET_CONF]:

```powershell
sc.exe create MapCallKafkaConsumer[TARGET_CONF] binPath= C:\solutions\mapcall-monorepo\consumer\src\MapCallKafkaConsumer.Service\bin\x64\[TARGET_CONF]\MapCallKafkaConsumer.Service.exe DisplayName= MapCallKafkaConsumer[TARGET_CONF]
net start MapCallKafkaConsumer[TARGET_CONF]
```

Actual example for QA3:

```powershell
sc.exe create MapCallKafkaConsumerQA3 binPath= C:\solutions\mapcall-monorepo\consumer\src\MapCallKafkaConsumer.Service\bin\x64\QA3\MapCallKafkaConsumer.Service.exe DisplayName= MapCallKafkaConsumerQA3
net start MapCallKafkaConsumerQA3
```

At some point you may want to stop and delete the service, these commands will do that:

```powershell
net stop MapCallKafkaConsumer[TARGET_CONF]
sc.exe delete MapCallKafkaConsumer[TARGET_CONF]
```

Actual example for QA3:

```powershell
net stop MapCallKafkaConsumerQA3
sc.exe delete MapCallKafkaConsumerQA3
```

## Deploying to QA

Available configurations are: QA, QA2, QA3, Training and Release.

Actual example for QA3:

```powershell
.\build --target="Deploy" --configuration="QA3"
```

## Creating New Consumers

The service will scan the `MapCallKafkaConsumer.Core` project for any "Consumers" and instantiate them then start them.

All consumers live under the `MapCallKafkaConsumer.Core` project in the `.\Consumers` folder, and then are logically grouped. For example, any consumers subscribing from the SAP domain might be under a `Sap` folder, likewise for `Lims`.

Further, each consumer should then be grouped under the domain folder - for example, a consumer for Lims Sequence Numbers might live under a `.\Consumers\Lims\SequenceNumber` folder.

### Configuration

At the root of each 'domain folder' should live a configuration class which implements a new interface inheriting from the `IKafkaServiceConfiguration` interface. Most importantly, the `GroupName` property of the implementing class represents the name of the configuration group containing settings for this consumer.

For example, if your kafka configuration is defined in an element like this: 

```xml
<lims>
    <kafka bootstrapServers="localhost:9092" consumerGroupId="mapcall-lims-kafka-consumer" />
</lims>
```

Then you would implement the `IKafkaServiceConfiguration` with: 

```c#
public string GroupName => "lims";
```

### Implementation

See `.\Consumers\Lims\SequenceNumber\LimsSequenceNumberConsumer.cs` for an example of how to implement.
