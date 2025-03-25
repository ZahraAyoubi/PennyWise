# Use .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set working directory inside the container
WORKDIR /src

# Copy the .csproj file first (faster caching)
COPY ["UserService/UserService.csproj", "UserService/"]

# Restore dependencies
RUN dotnet restore "UserService/UserService.csproj"

# Copy the rest of the project files
COPY . .

# Set working directory to the project folder
WORKDIR "/src/UserService"

# Build the project
RUN dotnet build "UserService.csproj" -c Release -o /app/build

# Publish the application
RUN dotnet publish "UserService.csproj" -c Release -o /app/publish

# Use a smaller runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose the application port
EXPOSE 80

# Run the application
ENTRYPOINT ["dotnet", "UserService.dll"]
