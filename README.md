# LeadsPlus - Microservices Architecture and Docker Containers based Application powered by Microsoft .NET Core 2.1


### Architecture overview 
This application is cross-platform at the server and client side, thanks to .NET Core services capable of running on Linux or Windows containers depending on your Docker host and any browser for the client web apps.

The architecture proposes a microservice oriented architecture implementation with multiple autonomous microservices each one owning its own data/db.

It is implementing different approaches within each microservice like CRUD or DDD/CQRS patterns.

It is using Http as the communication protocol between the client apps and the microservices.

It supports asynchronous communication for data updates propagation across multiple services based on Integration Events and an Event Bus - a light message broker, to choose between RabbitMQ or Azure Service Bus.

## Technology Used
JavaScript (ES6, ES 5), Angular 6, Material Design, SCSS, SPA

.NetCore 2.1, RabbitMQ, Docker, Microservice, CQRS, Domain Driven Design

MongoDB, SQLSERVER


## Intigration Used
Typeform

Google spreadsheet

Zapier

Cloudmailin

Sendgrid

Zapier parser

Datastudio

