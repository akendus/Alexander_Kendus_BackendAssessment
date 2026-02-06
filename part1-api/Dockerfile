# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY src/Metrics/Metrics.csproj src/Metrics/
RUN dotnet restore src/Metrics/Metrics.csproj

# Copy the rest of the source code
COPY src/Metrics/ src/Metrics/

# Build the application
WORKDIR /src/src/Metrics
RUN dotnet build Metrics.csproj -c Release -o /app/build

# Publish the application
RUN dotnet publish Metrics.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/publish .

# Set configurable port (default: 8080)
ENV METRICS_PORT=8080

# Expose port (default 8080, configurable via METRICS_PORT)
EXPOSE ${METRICS_PORT}

# Set environment variables
ENV ASPNETCORE_URLS=http://+:${METRICS_PORT}
ENV ASPNETCORE_ENVIRONMENT=Production

# Run the application
ENTRYPOINT ["dotnet", "Metrics.dll"]
