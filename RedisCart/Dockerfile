FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RedisCart.csproj", "./"]
RUN dotnet restore "./RedisCart.csproj"
COPY . .
RUN dotnet build "RedisCart.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "RedisCart.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "RedisCart.dll"]