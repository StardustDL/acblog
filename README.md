# AcBlog

![CI](https://github.com/acblog/acblog/workflows/CI/badge.svg) ![CD](https://github.com/acblog/acblog/workflows/CD/badge.svg) [![Homepage](https://img.shields.io/github/workflow/status/acblog/acblog.github.io/Deploy/src?label=homepage)](https://github.com/acblog/acblog.github.io) ![Mirrors](https://img.shields.io/github/workflow/status/acblog/mirrors/Mirror/master?label=mirrors) ![License](https://img.shields.io/github/license/acblog/acblog.svg)

An open source extensible static & dynamic blog system.

![](https://repository-images.githubusercontent.com/259549650/50d50d00-9073-11ea-8e72-0d3f1d3a7d8c)

The [homepage](https://acblog.github.io) is powered by AcBlog hosted on GitHub Pages.

## Features

- Based on WebAssembly
- Single Page APP
  - Installable
  - Offline
- Frontend
  - WebAssembly: full static files
  - SPA with server prerender
- Backend
  - Static-file backend with generator
  - Dynamic server backend
- Post
  - Category & Keywords
  - Markdown rendering
  - LaTeX math rendering
  - Diagram rendering
  - Password protection
  - Table of contents
- Slides
  - Preview & Full page
- Note
- Docker deployment
  - [![Docker](https://img.shields.io/docker/pulls/acblog/wasm.svg)](https://hub.docker.com/r/acblog/wasm) Client.WebAssembly
  - [![Docker](https://img.shields.io/docker/pulls/acblog/wasm-host.svg)](https://hub.docker.com/r/acblog/wasm-host) Client.WebAssembly.Host
  - [![Docker](https://img.shields.io/docker/pulls/acblog/api.svg)](https://hub.docker.com/r/acblog/api) Server.API

## Guide

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

### Backend

#### Static

Use AcBlog's static generator:

```sh
dotnet tool install -g AcBlog.Tools.StaticGenerator --version 0.0.1 \
  --add-source https://sparkshine.pkgs.visualstudio.com/StardustDL/_packaging/feed/nuget/v3/index.json

acblog-sgen -o ./dist
```

For GitHub Pages hosting, you can use [static-backend-generate-action](https://github.com/acblog/static-backend-generate-action).

#### Dynamic

Use AcBlog's Api server docker image:

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
dotnet tool install -g AcBlog.Tools.Sdk --version 0.0.1 \
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
