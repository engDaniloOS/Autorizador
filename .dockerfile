# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./Authorizer/ ./Authorizer
COPY ./AuthorizerTest/ ./AuthorizerTest
RUN dotnet restore ./Authorizer

# Copy everything else and build

RUN dotnet publish ./Authorizer/ -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Authorizer.dll"]