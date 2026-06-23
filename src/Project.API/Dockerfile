FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/Project.API/Project.API.csproj", "src/Project.API/"]
COPY ["src/Project.Application/Project.Application.csproj", "src/Project.Application/"]
COPY ["src/Project.Domain/Project.Domain.csproj", "src/Project.Domain/"]
COPY ["src/Project.Infrastructure/Project.Infrastructure.csproj", "src/Project.Infrastructure/"]

RUN dotnet restore "src/Project.API/Project.API.csproj"

COPY . .

RUN dotnet publish "src/Project.API/Project.API.csproj" \
    --configuration Release \
    --output /app/publish \
    /p:UseAppHost=false

FROM runtime AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Project.API.dll"]