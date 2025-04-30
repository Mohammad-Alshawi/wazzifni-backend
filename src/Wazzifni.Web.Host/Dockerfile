FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src

# Copy and restore all projects in one step
COPY src/*.sln ./
COPY src/**/*.csproj ./src/
RUN find ./src -name '*.csproj' -exec cp --parents {} . \;
RUN dotnet restore src/Wazzifni.Web.Host/Wazzifni.Web.Host.csproj

# Copy the rest of the source code
COPY src/ ./src/

WORKDIR /src/src/Wazzifni.Web.Host
RUN dotnet publish -c Release -o /app --no-restore

# Final image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app .

EXPOSE 80
ENTRYPOINT ["dotnet", "Wazzifni.Web.Host.dll"]
