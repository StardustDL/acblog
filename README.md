# AcBlog

![CI](https://github.com/acblog/acblog/workflows/CI/badge.svg) ![License](https://img.shields.io/github/license/acblog/acblog.svg)

An open source extensible static & dynamic blog system.

![](https://repository-images.githubusercontent.com/259549650/50d50d00-9073-11ea-8e72-0d3f1d3a7d8c)

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
  - Password protection
  - Table of contents
- Slides
  - Preview & Full page
- Note
- Docker deployment
  - Client.WebAssembly [![Docker](https://img.shields.io/docker/pulls/acblog/wasm.svg)](https://hub.docker.com/r/acblog/wasm)
  - Client.WebAssembly.Host [![Docker](https://img.shields.io/docker/pulls/acblog/wasm.svg)](https://hub.docker.com/r/acblog/wasm-host)

## Guide

### Frontend

Use AcBlog's WebAssembly client docker image:

```sh
docker pull acblog/wasm:latest
docker run -d -p 8000:80 acblog/wasm:latest
```

You can use volumn to apply settings:

```sh
docker run -d -v $PWD/appsettings.json:/app/appsettings.json -p 8000:80 acblog/wasm:latest
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
docker run -d -v $PWD/appsettings.json:/app/appsettings.json -p 8000:80 acblog/wasm-host:latest
```

### Backend

#### Static

Use AcBlog's static generator:

```sh
dotnet tool install -g AcBlog.Tools.StaticGenerator --version 0.0.1 --add-source https://www.myget.org/F/stardustdl/api/v3/index.json

acblog-sgen -o ./dist
```

For GitHub Pages hosting, you can use [static-backend-generate-action](https://github.com/acblog/static-backend-generate-action).

## Build

1. Install .NET Core SDK 3.1.201, NodeJS 12.x and npm.
2. Install Gulp & Libman

```sh
npm install -g gulp
dotnet tool install --global Microsoft.Web.LibraryManager.Cli
```

3. Restore dependencies

```sh
pwsh -c tools.ps1 restore
```

4. Build project

```sh
dotnet build
```

## Test & Benchmark

```sh
pwsh -c tools.ps1 test

pwsh -c tools.ps1 benchmark
```
