FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY ["FinanceTrackerCM.API/FinanceTrackerCM.API.csproj", "FinanceTrackerCM.API/"]
COPY ["FinanceTrackerCM.Application/FinanceTrackerCM.Application.csproj", "FinanceTrackerCM.Application/"]
COPY ["FinanceTrackerCM.Infrastructure/FinanceTrackerCM.Infrastructure.csproj", "FinanceTrackerCM.Infrastructure/"]

RUN dotnet restore "FinanceTrackerCM.API/FinanceTrackerCM.API.csproj"

# copy everything else and build
COPY . .
WORKDIR /src/FinanceTrackerCM.API
RUN dotnet publish -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "FinanceTrackerCM.API.dll"]
