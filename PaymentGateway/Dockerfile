FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY ./PaymentGateway/PaymentGateway.csproj ./PaymentGateway/PaymentGateway.csproj
COPY ./PaymentGateway.Data/PaymentGateway.Data.csproj ./PaymentGateway.Data/PaymentGateway.Data.csproj
COPY ./PaymentGateway.Domain/PaymentGateway.Domain.csproj ./PaymentGateway.Domain/PaymentGateway.Domain.csproj
RUN dotnet restore ./PaymentGateway/PaymentGateway.csproj

# copy and publish app and libraries
COPY . ./
RUN dotnet publish ./PaymentGateway/PaymentGateway.csproj -c Release -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app .
EXPOSE 5000
ENTRYPOINT ["dotnet", "PaymentGateway.dll"]