# Use the official .NET SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Set the working directory
WORKDIR /src

# Copy the .csproj files into the container
COPY ["src/Wazzifni.Web.Host/Wazzifni.Web.Host.csproj", "src/Wazzifni.Web.Host/"]
COPY ["src/Wazzifni.Web.Core/Wazzifni.Web.Core.csproj", "src/Wazzifni.Web.Core/"]
COPY ["src/Wazzifni.Application/Wazzifni.Application.csproj", "src/Wazzifni.Application/"]
COPY ["src/Wazzifni.Core/Wazzifni.Core.csproj", "src/Wazzifni.Core/"]
COPY ["src/Wazzifni.EntityFrameworkCore/Wazzifni.EntityFrameworkCore.csproj", "src/Wazzifni.EntityFrameworkCore/"]

# Restore the dependencies
WORKDIR "/src/src/Wazzifni.Web.Host"
RUN dotnet restore

# Copy all the application code to the container
WORKDIR /src
COPY ["src/Wazzifni.Web.Host", "src/Wazzifni.Web.Host"]
COPY ["src/Wazzifni.Web.Core", "src/Wazzifni.Web.Core"]
COPY ["src/Wazzifni.Application", "src/Wazzifni.Application"]
COPY ["src/Wazzifni.Core", "src/Wazzifni.Core"]
COPY ["src/Wazzifni.EntityFrameworkCore", "src/Wazzifni.EntityFrameworkCore"]

# Build and publish the application
WORKDIR "/src/src/Wazzifni.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

# Use the official ASP.NET runtime image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:7.0

# Expose port 80
EXPOSE 80

# Set the working directory
WORKDIR /app

# Copy the build output from the build stage
COPY --from=build /publish .

# Define the entry point for the application
ENTRYPOINT ["dotnet", "Wazzifni.Web.Host.dll"]