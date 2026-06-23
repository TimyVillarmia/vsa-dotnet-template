FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/VsaTemplate.API/VsaTemplate.API.csproj", "src/VsaTemplate.API/"]
COPY ["src/VsaTemplate.Application/VsaTemplate.Application.csproj", "src/VsaTemplate.Application/"]
COPY ["src/VsaTemplate.Domain/VsaTemplate.Domain.csproj", "src/VsaTemplate.Domain/"]
COPY ["src/VsaTemplate.Infrastructure/VsaTemplate.Infrastructure.csproj", "src/VsaTemplate.Infrastructure/"]

RUN dotnet restore "src/VsaTemplate.API/VsaTemplate.API.csproj"

COPY . .

RUN dotnet publish "src/VsaTemplate.API/VsaTemplate.API.csproj" \
    --configuration Release \
    --output /app/publish \
    /p:UseAppHost=false

FROM runtime AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "VsaTemplate.API.dll"]