#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0.301 AS build
WORKDIR /src
COPY ["src/AcBlog.Server.Api/AcBlog.Server.Api.csproj", "src/AcBlog.Server.Api/"]
COPY ["src/AcBlog.Data/AcBlog.Data.csproj", "src/AcBlog.Data/"]
COPY ["src/AcBlog.Data.Repositories.SqlServer/AcBlog.Data.Repositories.SqlServer.csproj", "src/AcBlog.Data.Repositories.SqlServer/"]
RUN dotnet restore "src/AcBlog.Server.Api/AcBlog.Server.Api.csproj" -s https://sparkshine.pkgs.visualstudio.com/StardustDL/_packaging/feed/nuget/v3/index.json -s https://api.nuget.org/v3/index.json
COPY . .
WORKDIR "/src/src/AcBlog.Server.Api"
RUN dotnet build "AcBlog.Server.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AcBlog.Server.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AcBlog.Server.Api.dll"]
