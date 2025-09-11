# Use the official .NET 9 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set the working directory
WORKDIR /src

# Copy solution file
COPY StudentManagementSystem.sln ./

# Copy project files
COPY StudentManagementSystem.API/StudentManagementSystem.API.csproj StudentManagementSystem.API/
COPY StudentManagementSystem.Application/StudentManagementSystem.Application.csproj StudentManagementSystem.Application/
COPY StudentManagementSystem.Domain/StudentManagementSystem.Domain.csproj StudentManagementSystem.Domain/
COPY StudentManagementSystem.Infrastructure/StudentManagementSystem.Infrastructure.csproj StudentManagementSystem.Infrastructure/

# Restore dependencies
RUN dotnet restore

# Copy the rest of the source code
COPY . .

# Build the application
RUN dotnet build -c Release --no-restore

# Publish the application
RUN dotnet publish StudentManagementSystem.API/StudentManagementSystem.API.csproj -c Release -o /app/publish --no-restore

# Use the official .NET 9 runtime image for running
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# Set the working directory
WORKDIR /app

# Copy the published application
COPY --from=build /app/publish .

# Expose the port
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Create a script to apply migrations and start the application
RUN echo '#!/bin/bash\n\
dotnet ef database update\n\
exec dotnet StudentManagementSystem.API.dll' > /app/start.sh && \
chmod +x /app/start.sh

# Start the application
ENTRYPOINT ["/app/start.sh"]