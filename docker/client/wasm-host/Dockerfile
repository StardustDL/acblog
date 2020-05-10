FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/AcBlog.Client.WebAssembly.Host/AcBlog.Client.WebAssembly.Host.csproj", "src/AcBlog.Client.WebAssembly.Host/"]
COPY ["src/AcBlog.Client.WebAssembly/AcBlog.Client.WebAssembly.csproj", "src/AcBlog.Client.WebAssembly/"]
COPY ["src/AcBlog.Client.Components.Loading/AcBlog.Client.Components.Loading.csproj", "src/AcBlog.Client.Components.Loading/"]
COPY ["src/AcBlog.Data/AcBlog.Data.csproj", "src/AcBlog.Data/"]
COPY ["src/AcBlog.SDK/AcBlog.SDK.csproj", "src/AcBlog.SDK/"]
COPY ["src/AcBlog.Data.Repositories.FileSystem/AcBlog.Data.Repositories.FileSystem.csproj", "src/AcBlog.Data.Repositories.FileSystem/"]
COPY ["src/AcBlog.Client.WebAssembly.Shared/AcBlog.Client.WebAssembly.Shared.csproj", "src/AcBlog.Client.WebAssembly.Shared/"]
COPY ["src/AcBlog.Client.Components.Markdown/AcBlog.Client.Components.Markdown.csproj", "src/AcBlog.Client.Components.Markdown/"]
COPY ["src/AcBlog.Client.Components.CodeEditor/AcBlog.Client.Components.CodeEditor.csproj", "src/AcBlog.Client.Components.CodeEditor/"]
COPY ["src/AcBlog.Client.Components.Slides/AcBlog.Client.Components.Slides.csproj", "src/AcBlog.Client.Components.Slides/"]
RUN dotnet restore "src/AcBlog.Client.WebAssembly.Host/AcBlog.Client.WebAssembly.Host.csproj"
COPY . .
WORKDIR "/src/src/AcBlog.Client.WebAssembly.Host"
RUN dotnet build "AcBlog.Client.WebAssembly.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AcBlog.Client.WebAssembly.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AcBlog.Client.WebAssembly.Host.dll"]