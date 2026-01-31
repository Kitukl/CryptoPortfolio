FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CryptoAnalyzer.Portfolio.API/CryptoAnalyzer.Portfolio.API.csproj", "CryptoAnalyzer.Portfolio.API/"]
COPY ["CryptoAnalyzer.Portfolio.DAL/CryptoAnalyzer.Portfolio.DAL.csproj", "CryptoAnalyzer.Portfolio.DAL/"]
COPY ["CryptoAnalyzer.Portfolio.BLL/CryptoAnalyzer.Portfolio.BLL.csproj", "CryptoAnalyzer.Portfolio.BLL/"]
RUN dotnet restore "CryptoAnalyzer.Portfolio.API/CryptoAnalyzer.Portfolio.API.csproj"
COPY . .
WORKDIR "/src/CryptoAnalyzer.Portfolio.API"
RUN dotnet build "CryptoAnalyzer.Portfolio.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CryptoAnalyzer.Portfolio.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN dotnet tool install --global dotnet-ef --version 9.0.*
ENV PATH="${PATH}:/root/.dotnet/tools"

COPY --from=build /src/CryptoAnalyzer.Portfolio.API/ /app/CryptoAnalyzer.Portfolio.API/
COPY --from=build /src/CryptoAnalyzer.Portfolio.DAL/ /app/CryptoAnalyzer.Portfolio.DAL/
COPY --from=build /src/CryptoAnalyzer.Portfolio.BLL/ /app/CryptoAnalyzer.Portfolio.BLL/

ENTRYPOINT ["/bin/bash", "-c", "dotnet ef database update --project /app/CryptoAnalyzer.Portfolio.DAL/CryptoAuth.DAL.csproj --startup-project /app/CryptoAnalyzer.Portfolio.API/CryptoAnalyzer.Portfolio.API.csproj && dotnet CryptoAnalyzer.Portfolio.API.dll"]
