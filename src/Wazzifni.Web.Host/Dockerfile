FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY ["src/Wazzifni.Web.Host/Wazzifni.Web.Host.csproj", "src/Wazzifni.Web.Host/"]
COPY ["src/Wazzifni.Web.Core/Wazzifni.Web.Core.csproj", "src/Wazzifni.Web.Core/"]
COPY ["src/Wazzifni.Application/Wazzifni.Application.csproj", "src/Wazzifni.Application/"]
COPY ["src/Wazzifni.Core/Wazzifni.Core.csproj", "src/Wazzifni.Core/"]
COPY ["src/Wazzifni.EntityFrameworkCore/Wazzifni.EntityFrameworkCore.csproj", "src/Wazzifni.EntityFrameworkCore/"]
WORKDIR "/src/src/Wazzifni.Web.Host"
RUN dotnet restore 

WORKDIR /src
COPY ["src/Wazzifni.Web.Host", "src/Wazzifni.Web.Host"]
COPY ["src/Wazzifni.Web.Core", "src/Wazzifni.Web.Core"]
COPY ["src/Wazzifni.Application", "src/Wazzifni.Application"]
COPY ["src/Wazzifni.Core", "src/Wazzifni.Core"]
COPY ["src/Wazzifni.EntityFrameworkCore", "src/Wazzifni.EntityFrameworkCore"]
WORKDIR "/src/src/Wazzifni.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "Wazzifni.Web.Host.dll"]
