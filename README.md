# AcBlog

![CI](https://github.com/acblog/acblog/workflows/CI/badge.svg) ![CD](https://github.com/acblog/acblog/workflows/CD/badge.svg) [![Homepage](https://img.shields.io/github/workflow/status/acblog/acblog.github.io/Deploy/src?label=homepage)](https://github.com/acblog/acblog.github.io) ![Mirrors](https://img.shields.io/github/workflow/status/acblog/mirrors/Mirror/master?label=mirrors) ![License](https://img.shields.io/github/license/acblog/acblog.svg) [![AcBlog.Tools.SDK](https://buildstats.info/nuget/AcBlog.Tools.SDK)](https://www.nuget.org/packages/AcBlog.Tools.SDK/)

An open source extensible static & dynamic blog system.

![](https://repository-images.githubusercontent.com/259549650/50d50d00-9073-11ea-8e72-0d3f1d3a7d8c)

The [homepage](https://acblog.github.io) is powered by AcBlog hosted on GitHub Pages.

- A mirror homepage on [Gitee](https://acblog.gitee.io).

![](https://github.com/StardustDL/own-staticfile-hosting/raw/master/acblog/images/preview.png)

## Features

- Based on WebAssembly & SignalR
- Single Page APP
  - Installable
  - Offline
- Frontend
  - WebAssembly: full static files
  - SPA by WebAssembly with server prerender
  - SPA without WebAssembly by communicating with server
- Backend
  - Static-file backend with generator
  - Dynamic server backend
- Post
  - Category & Keywords
  - Markdown rendering
  - LaTeX math rendering
  - Diagram rendering
  - Media links
  - Password protection
  - Table of contents
- Slides
  - Preview & Full page
- Note
- Custom pages
  - Custom layout
  - Full HTML
- Visitor statistic
- Comments
- Sitemap
- Feeds (Atom & RSS)
- Docker deployment
  - [![Docker](https://img.shields.io/docker/pulls/acblog/wasm.svg)](https://hub.docker.com/r/acblog/wasm) Client.WebAssembly
  - [![Docker](https://img.shields.io/docker/pulls/acblog/wasm-host.svg)](https://hub.docker.com/r/acblog/wasm-host) Client.WebAssembly.Host
  - [![Docker](https://img.shields.io/docker/pulls/acblog/client.svg)](https://hub.docker.com/r/acblog/client) Client.Server
  - [![Docker](https://img.shields.io/docker/pulls/acblog/api.svg)](https://hub.docker.com/r/acblog/api) Server.API

## Guide

### Full Static Hosting

GitHub Pages hosting, based on [wasm-ghpages-generate-action](https://github.com/acblog/wasm-ghpages-generate-action) and [static-backend-generate-action](https://github.com/acblog/static-backend-generate-action).

- [Demo Project](https://github.com/acblog/acblog.github.io)
- [中文说明](https://stardustdl.gitee.io/posts/Development%2FAcBlog-staticgen)

### Frontend

Use AcBlog's WebAssembly client docker image:

```sh
docker pull acblog/wasm:latest
docker run -d -p 8000:80 acblog/wasm:latest
```

You can use volumn to apply settings:

```sh
docker run -d \
  -v $PWD/appsettings.json:/app/appsettings.json \
  -v $PWD/manifest.json:/app/manifest.json \
  -p 8000:80 acblog/wasm:latest
```

For GitHub Pages hosting, you can use [wasm-ghpages-generate-action](https://github.com/acblog/wasm-ghpages-generate-action).

---

Use AcBlog's WebAssembly hosted client docker image:

```sh
docker pull acblog/wasm-host:latest
docker run -d -p 8000:80 acblog/wasm-host:latest
```

You can use volumn to apply settings:

```sh
docker run -d \
  -v $PWD/appsettings.json:/app/appsettings.json \
  -v $PWD/manifest.json:/app/wwwroot/manifest.json \
  -p 8000:80 acblog/wasm-host:latest
```

---

Use AcBlog's server client docker image (no WebAssembly):

```sh
docker pull acblog/client:latest
docker run -d -p 8000:80 acblog/client:latest
```

You can use volumn to apply settings:

```sh
docker run -d \
  -v $PWD/appsettings.json:/app/appsettings.json \
  -p 8000:80 acblog/client:latest
```

### Backend

#### Static

Use AcBlog's SDK:

```sh
dotnet tool install -g AcBlog.Tools.Sdk \
  --add-source https://sparkshine.pkgs.visualstudio.com/StardustDL/_packaging/feed/nuget/v3/index.json

acblog init
acblog remote add origin "./dist"
acblog push
```

For GitHub Pages hosting, you can use [static-backend-generate-action](https://github.com/acblog/static-backend-generate-action).

#### Dynamic

Use AcBlog's API server docker image:

```sh
docker pull acblog/api:latest
docker run -d -p 8000:80 acblog/api:latest
```

### Compose

Use docker-compose to deploy WebAssembly hosted client and Api server:

```sh
cd docker/deploy
docker-compose up
```

Maybe you need to restart api container after database initializing.

### SDK

Use AcBlog's command-line Sdk tool to communicate with AcBlog server.

```sh
dotnet tool install -g AcBlog.Tools.Sdk \
  --add-source https://sparkshine.pkgs.visualstudio.com/StardustDL/_packaging/feed/nuget/v3/index.json

acblog --help
```

## Build

1. Install .NET Core SDK 3.1, NodeJS 12.x and npm.
2. Install Gulp & Libman
3. Install psake

```ps1
npm install -g gulp
dotnet tool install --global Microsoft.Web.LibraryManager.Cli
Set-PSRepository -Name PSGallery -InstallationPolicy Trusted; Install-Module -Name psake
```

4. Restore dependencies

Add NuGet source: https://sparkshine.pkgs.visualstudio.com/StardustDL/_packaging/feed/nuget/v3/index.json.

```ps1
Invoke-psake Restore
```

1. Build project

```ps1
Invoke-psake Build
```

## Test & Benchmark

```sh
Invoke-psake CI
```

## Dependencies

- [.NET](https://github.com/dotnet/runtime) & [ASP.NET](https://github.com/dotnet/aspnetcore) For basic framework.
- [Entity Framework](https://github.com/dotnet/efcore) For database access.
- [ant-design-blazor](https://github.com/ant-design-blazor/ant-design-blazor) For UI designs.
- [scriban](https://github.com/lunet-io/scriban) For layouts.
- [RazorComponents.Markdown](https://github.com/StardustDL/RazorComponents.Markdown) For Markdown rendering.
- [loment](https://github.com/StardustDL/loment) For comment service.
- [listat](https://github.com/StardustDL/listat) For statistic service.

## Status

![](https://buildstats.info/github/chart/acblog/acblog?branch=master)