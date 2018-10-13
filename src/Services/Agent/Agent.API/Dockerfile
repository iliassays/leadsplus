FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["src/Services/Agent/Agent.API/Agent.API.csproj", "src/Services/Agent/Agent.API/"]
COPY ["src/Services/Agent/Agent.Domain/Agent.Domain.csproj", "src/Services/Agent/Agent.Domain/"]
COPY ["src/BuildingBlocks/Core/LeadsPlus.Core/LeadsPlus.Core.csproj", "src/BuildingBlocks/Core/LeadsPlus.Core/"]
COPY ["src/BuildingBlocks/Core/LeadsPlus.GoogleApi/LeadsPlus.GoogleApis.csproj", "src/BuildingBlocks/Core/LeadsPlus.GoogleApi/"]
COPY ["src/BuildingBlocks/HealthChecks/src/Microsoft.AspNetCore.HealthChecks/Microsoft.AspNetCore.HealthChecks.csproj", "src/BuildingBlocks/HealthChecks/src/Microsoft.AspNetCore.HealthChecks/"]
COPY ["src/BuildingBlocks/HealthChecks/src/Microsoft.Extensions.HealthChecks/Microsoft.Extensions.HealthChecks.csproj", "src/BuildingBlocks/HealthChecks/src/Microsoft.Extensions.HealthChecks/"]
COPY ["src/BuildingBlocks/EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj", "src/BuildingBlocks/EventBus/EventBusRabbitMQ/"]
COPY ["src/BuildingBlocks/EventBus/EventBus/EventBus.csproj", "src/BuildingBlocks/EventBus/EventBus/"]
COPY ["src/BuildingBlocks/EventBus/EventBusServiceBus/EventBusServiceBus.csproj", "src/BuildingBlocks/EventBus/EventBusServiceBus/"]
RUN dotnet restore "src/Services/Agent/Agent.API/Agent.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Agent/Agent.API"
RUN dotnet build "Agent.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Agent.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Agent.API.dll"]
