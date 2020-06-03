#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/AcBlog.Server.API/AcBlog.Server.API.csproj", "src/AcBlog.Server.API/"]
COPY ["src/AcBlog.Data/AcBlog.Data.csproj", "src/AcBlog.Data/"]
COPY ["src/AcBlog.Data.Repositories.SQLServer/AcBlog.Data.Repositories.SQLServer.csproj", "src/AcBlog.Data.Repositories.SQLServer/"]
RUN dotnet restore "src/AcBlog.Server.API/AcBlog.Server.API.csproj" -s https://www.myget.org/F/stardustdl/api/v3/index.json -s https://api.nuget.org/v3/index.json
COPY . .
WORKDIR "/src/src/AcBlog.Server.API"
RUN dotnet build "AcBlog.Server.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AcBlog.Server.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AcBlog.Server.API.dll"]