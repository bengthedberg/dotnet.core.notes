# AWS .NET Core 

## References

* [AWS SDK for .NET Documentation](https://docs.aws.amazon.com/sdk-for-net/index.html)
* [.NET AWS Sample Code](https://github.com/awsdocs/aws-doc-sdk-examples/tree/master/dotnet/example_code)


## Local Development using Localstack

Localstack is a useful environment which allows for local development using the AWS cloud stack. In other words, it is a Mock AWS Stack with support for many of the infrastructure commonly coded against.

This setup is running on Docker, to simplify the environment even further.

* [LocalStack - A fully functional local AWS cloud stack](https://github.com/localstack/localstack)
* [AWS CLI in Docker](https://github.com/mesosphere/aws-cli)
* [Working with LocalStack](https://lobster1234.github.io/2017/04/05/working-with-localstack-command-line/)

**docker-compose.yml**

The following docker-compose example creates a local stack with dynamodb, kinesis and SQS in docker. It also creates any required resources; i.e. tables, streams, queues, etc. using the local aws-cli commands:

```yml
version: "3"

services:    

  localstack:
      image: localstack/localstack
      ports:
        - "4568-4576:4568-4576"
        - "${PORT_WEB_UI-8080}:${PORT_WEB_UI-8080}"
      environment:
        - DOCKER_HOST=unix:///var/run/docker.sock
        - SERVICES=dynamodb:4569,kinesis:4568,sqs:4576
        - DEFAULT_REGION=us-east-1
      volumes:
        - "/var/run/docker.sock:/var/run/docker.sock"
        - "/private${TMPDIR}:/tmp/localstack"
      networks:
        - my_local_network
      
  setup-resources:
      image: mesosphere/aws-cli
      volumes:
        - ./dev_env:/project/dev_env
      environment:
        - AWS_ACCESS_KEY_ID=dummyaccess
        - AWS_SECRET_ACCESS_KEY=dummysecret
        - AWS_DEFAULT_REGION=us-east-1
      entrypoint: /bin/sh -c
      command: >
        "
          # Needed so all localstack components will startup correctly (i'm sure there's a better way to do this)
          sleep 10;
  
          aws dynamodb create-table --endpoint-url=http://localstack:4569 --table-name my_table \
            --attribute-definitions AttributeName=key,AttributeType=S \
            --key-schema AttributeName=key,KeyType=HASH \
            --provisioned-throughput ReadCapacityUnits=5,WriteCapacityUnits=5;
  
          aws kinesis create-stream --endpoint-url=http://localstack:4568 --stream-name my_stream --shard-count 1;
  
          aws sqs create-queue --endpoint-url=http://localstack:4576 --queue-name inventory_request_12345;
          aws sqs create-queue --endpoint-url=http://localstack:4576 --queue-name inventory_response_12345;
  
          aws sqs list-queues --endpoint-url=http://localstack:4576
  
          # you can go on and put initial items in tables...
        "
      networks:
        - my_local_network
      depends_on:
        - localstack
```

Start the environment with ```docker-compose up``` and shut it down with ```docker-compose down```.

## SQS 
### What
Amazon **Simple Queue Service (SQS)** is a fully managed message queuing service that eliminates the complexity and overhead associated with managing and operating message-oriented middleware.

### Why
A message queue is a form of asynchronous service-to-service communication used in serverless and microservices architectures. 

Messages are stored on the queue until they are processed and deleted. 

Message queues can be used to decouple heavyweight processing, to buffer or batch work, and to smooth spiky workloads.

### Install
```
dotnet add package AWSSDK.SQS
dotnet add package AWSSDK.Extensions.NETCore.Setup  
```
### Configure

**Startup.cs**

```csharp
// This method gets called by the runtime. Use this method to add services to the container.
public void ConfigureServices(IServiceCollection services)
{
    // AWS Configuration
    services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
    services.AddAWSService<IAmazonSQS>();
}
```

**Appsettings.json**

```json
{
  "AWS": {
    "Profile": "default",
    "Region": "ap-southeast-2"
  }
}
```

See [JSON Configuration for AWS](https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/net-dg-config-netcore.html) for more information.

### Usage

#### Inject SQS Client

The following example shows a WebAPI using SQS:

```csharp
[ApiController]
public class HomeController : ControllerBase
{
    private readonly IAmazonSQS _sqsClient;

    public HomeController(IAmazonSQS sqsClient)
    {
         _sqsClient = sqsClient;
    }

}
```




