# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["BookStore.csproj", "./"]
RUN dotnet restore "BookStore.csproj"

# Copy everything else and build
COPY . .
RUN dotnet build "BookStore.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "BookStore.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Create non-root user for security
RUN addgroup --system --gid 1001 bookstore && \
    adduser --system --uid 1001 --gid 1001 bookstore

# Copy published app
COPY --from=publish /app/publish .

# Change ownership to non-root user
RUN chown -R bookstore:bookstore /app
USER bookstore

# Expose port
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

# Entry point
ENTRYPOINT ["dotnet", "BookStore.dll"] 