# AcBlog

![CI](https://github.com/acblog/acblog/workflows/CI/badge.svg) ![License](https://img.shields.io/github/license/acblog/acblog.svg)

## Features

- Based on WebAssembly
- Single Page APP
  - Installable
  - Offline
- Frontend
  - Full static files
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
- Docker deployment
  - Client.WASM [![Docker](https://img.shields.io/docker/pulls/acblog/wasm.svg)](https://hub.docker.com/r/acblog/wasm)

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
