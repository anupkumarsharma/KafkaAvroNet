# KafKaAvroNet [![Build status](https://ci.appveyor.com/api/projects/status/wscwrnu0d0s09s9b?svg=true)](https://ci.appveyor.com/project/anupkumarsharma/kafkaavronet) [![CircleCI](https://circleci.com/gh/anupkumarsharma/KafkaAvroNet/tree/master.svg?style=svg)](https://circleci.com/gh/anupkumarsharma/KafkaAvroNet/tree/master) [![codecov](https://codecov.io/gh/anupkumarsharma/KafkaAvroNet/branch/master/graph/badge.svg)](https://codecov.io/gh/anupkumarsharma/KafkaAvroNet)

End to End solution for .Net using Schema registry, Avro Serde using reflection, Kafka.

## Features ##

1. Uses reflection to convert POCO classes into Avro format using Apache Avro
2. Uses Confluent Kafka to write and read from Kafka 
3. Enables end to end use case of defining the schema, writing to Kafka using POCO classes, reading from Kafka and deserializing into POCO classes

### Updates coming soon

- [x] Kafka Producer
- [x] Support for primitive and array type 
- [ ] Kafka Consumer
- [ ] Support for more logical types 
- [ ] Performance improvement 
- [ ] Docker support

Sample
----------------

Sample coming up soon. For now E2E test should give be a good reference.