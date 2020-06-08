FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

COPY ["TransactionStore.Data/TransactionStore.Data.csproj", "TransactionStore.Data/"]
COPY ["TransactionStore.Api/TransactionStore.Api.csproj", "TransactionStore.Api/"]

RUN dotnet restore "TransactionStore.Api/TransactionStore.Api.csproj"
COPY . .
WORKDIR "/src/TransactionStore.Api"
RUN dotnet build "TransactionStore.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TransactionStore.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TransactionStore.Api.dll"]