# .NET Core 

* [AWS](./AWS/README.md)
* [Documentation](./Documentation/README.md)
* [Logging](./Logging/README.md)


## Useful Docker Images

### Sql Server

[SQL Server on Linux](https://docs.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver15&pivots=cs1-bash)

docker-compose.yml

```yml
version: '3'
services:

  sqlserver:
    image: microsoft/mssql-server-linux:latest
    container_name: sqlserver
    volumes:
      - sqlserverdata:/var/opt/mssql 
    ports:
      - "1434:1433"
    environment:
      - ACCEPT_EULA=Y 
      - MSSQL_PID=Developer
      - SA_PASSWORD=8jkGh47hnDw89Haq8LN2

volumes:
  sqlserverdata:
    external: true         
```

application.json configuration 

```json
{
  "ConnectionStrings": {
    "NotificationServiceCN": "server=localhost,1434;user id=sa;password=8jkGh47hnDw89Haq8LN2;database=MyDatabase;MultipleActiveResultSets=True;"
  }
}
```

### Mail Server

[MailDev](https://maildev.github.io/maildev/)

SMTP Server + Web Interface for viewing and testing emails during development.

The MailDev inbox: http://localhost:4000.

docker-compose.yml

```yml
version: '3'
services:

  mailserver:
    image: djfarrelly/maildev
    container_name: mailserver
    ports:
      - "25:25"
      - "4000:80"
```

application.json configuration 

```json
{
  "Email": {
    "Host": "localhost",
    "Port": 25,
    "User": "mailuser",
    "Pwd": "jeLkT5f2Lmzp7HaqH3c9"
  }
}
```

### Seq

[Seq with Docker](https://docs.datalust.co/docs/getting-started-with-docker)

The Seq logging console: http://localhost:5341.

docker-compose.yml

```yml
version: '3'
services:

  logserver:
    image: datalust/seq:latest
    container_name: logserver
    ports:
      - "5341:80"
    environment:
      - ACCEPT_EULA=Y 
```

application.json configuration 

```json
{
  "Serilog": {
    "Using": [ "Serilog.Sinks.Console" ],
    "MinimumLevel": "Information",
    "Properties": {
      "Application": "NotificationService"
    },
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "Seq",
        "Args": { "serverUrl": "http://localhost:5341" }
      }
    ]
  }
}
```

### RabbitMQ

[Get started with RabbitMQ and Docker](https://levelup.gitconnected.com/rabbitmq-with-docker-on-windows-in-30-minutes-172e88bb0808)

The RabbitMQ management dashboard: http://localhost:15672.
```
Username: rabbitmquser
Password: DEBmbwkSrzy9D1T9cJfa
```

docker-compose.yml

```yml
version: '3'
services:

  rabbitmq:
    image: rabbitmq:3-management
    container_name: rabbitmq
    volumes:
      - rabbitmqdata:/var/lib/rabbitmq
    ports:
      - "15672:15672"
      - "5672:5672"
    environment:
      - RABBITMQ_DEFAULT_USER=rabbitmquser
      - RABBITMQ_DEFAULT_PASS=DEBmbwkSrzy9D1T9cJfa

volumes:  
  rabbitmqdata:
    external: true      
```

application.json configuration 

```json
{
   "RabbitMQ": {
    "Host": "localhost",
    "Username": "rabbitmquser",
    "Password": "DEBmbwkSrzy9D1T9cJfa"
  }
}
```

### 