# Use the official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the solution file and project files first to cache the restore layer
COPY ["ProConnect_Backend.sln", "./"]
COPY ["ProConnect_Backend/ProConnect_Backend.csproj", "ProConnect_Backend/"]
COPY ["ProConnect_Backend.Application/ProConnect_Backend.Application.csproj", "ProConnect_Backend.Application/"]
COPY ["ProConnect_Backend.Domain/ProConnect_Backend.Domain.csproj", "ProConnect_Backend.Domain/"]
COPY ["ProConnect_Backend.Infrastructure/ProConnect_Backend.Infrastructure.csproj", "ProConnect_Backend.Infrastructure/"]

# Restore dependencies
RUN dotnet restore

# Copy the rest of the source code
COPY . .

# Build and publish the application
WORKDIR "/src/ProConnect_Backend"
RUN dotnet publish -c Release -o /app/publish

# Use the official ASP.NET Core runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProConnect_Backend.dll"]
