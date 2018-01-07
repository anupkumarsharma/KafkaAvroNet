dotnet clean
dotnet build CI-Kafka-Net.sln -c "Release"
dotnet pack .\CI\CI.Kafka\KafkaAvroNet.Manager.csproj -c "Release" --no-build --output ..\..\nupkgs  
dotnet pack .\Core\KafkaAvroNet.Avro\KafkaAvroNet.Avro.csproj  -c "Release"--no-build --output ..\..\nupkgs 
dotnet pack .\Core\KafkaAvroNet.Kafka\KafkaAvroNet.Kafka.csproj  -c "Release" --no-build --output ..\..\nupkgs  
dotnet pack .\Core\KafkaAvroNet.SchemaRegistry\KafkaAvroNet.SchemaRegistry.csproj -c "Release" --no-build --output ..\..\nupkgs 
#dotnet pack .\CI\CI.Kafka\KafkaAvroNet.Manager.csproj --no-build --output ..\..\nupkgs  /p:PackageVersion=1.1.0
#dotnet pack .\Core\KafkaAvroNet.Avro\KafkaAvroNet.Avro.csproj --no-build --output ..\..\nupkgs  /p:PackageVersion=1.1.0
#dotnet pack .\Core\KafkaAvroNet.Kafka\KafkaAvroNet.Kafka.csproj --no-build --output ..\..\nupkgs  /p:PackageVersion=1.1.0
#dotnet pack .\Core\KafkaAvroNet.SchemaRegistry\KafkaAvroNet.SchemaRegistry.csproj --no-build --output ..\..\nupkgs  /p:PackageVersion=1.1.0


