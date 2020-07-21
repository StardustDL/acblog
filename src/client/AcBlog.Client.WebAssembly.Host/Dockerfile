#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/client/AcBlog.Client.WebAssembly.Host/AcBlog.Client.WebAssembly.Host.csproj", "src/client/AcBlog.Client.WebAssembly.Host/"]
COPY ["src/client/AcBlog.Client.WebAssembly/AcBlog.Client.WebAssembly.csproj", "src/client/AcBlog.Client.WebAssembly/"]
COPY ["src/AcBlog.Data/AcBlog.Data.csproj", "src/AcBlog.Data/"]
COPY ["src/AcBlog.Sdk/AcBlog.Sdk.csproj", "src/AcBlog.Sdk/"]
COPY ["src/AcBlog.Data.Repositories.FileSystem/AcBlog.Data.Repositories.FileSystem.csproj", "src/AcBlog.Data.Repositories.FileSystem/"]
COPY ["src/client/AcBlog.Client.WebAssembly.Shared/AcBlog.Client.WebAssembly.Shared.csproj", "src/client/AcBlog.Client.WebAssembly.Shared/"]
RUN dotnet restore "src/client/AcBlog.Client.WebAssembly.Host/AcBlog.Client.WebAssembly.Host.csproj" -s https://sparkshine.pkgs.visualstudio.com/StardustDL/_packaging/feed/nuget/v3/index.json -s https://api.nuget.org/v3/index.json
COPY . .
WORKDIR "/src/src/client/AcBlog.Client.WebAssembly.Host"
RUN dotnet build "AcBlog.Client.WebAssembly.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AcBlog.Client.WebAssembly.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AcBlog.Client.WebAssembly.Host.dll"]