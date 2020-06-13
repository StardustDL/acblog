if ($args.Count -gt 0) {
    switch ($args[0]) {
        "npmup?" {
            Set-Location src/AcBlog.Client.WebAssembly && ncu && Set-Location ../..
            if (!$?) {
                exit 1
            }
        }
        "npmup" {
            Set-Location src/AcBlog.Client.WebAssembly && ncu -u && npm install && Set-Location ../..
            if (!$?) {
                exit 1
            }
        }
        "restore" {
            Write-Output "Restore npm..."
            Set-Location src/AcBlog.Client.WebAssembly && npm ci && gulp && Set-Location ../..
            if (!$?) {
                exit 1
            }
            dotnet restore
            if (!$?) {
                exit 1
            }
        }
        "api" {
            Write-Output "Run API..."
            dotnet run -p ./src/AcBlog.Server.API
            if (!$?) {
                exit 1
            }
        }
        "wasm" {
            Write-Output "Run WebAssembly..."
            dotnet run -p ./src/AcBlog.Client.WebAssembly
            if (!$?) {
                exit 1
            }
        }
        "wasm-pub" {
            Write-Output "Publish WebAssembly..."
            Set-Location ./src/AcBlog.Client.WebAssembly
            Move-Item ./wwwroot/data ./data
            dotnet publish -c Release
            Move-Item ./data ./wwwroot/data
            Set-Location ../..
            if (!$?) {
                exit 1
            }
        }
        "pack" {
            mkdir packages
            dotnet pack -c Release /p:Version=${env:build_version} -o ./packages
            if ($?) { exit 0 }
            Write-Output "Retry packing..."
            dotnet pack -c Release /p:Version=${env:build_version} -o ./packages
            if ($?) { exit 0 }
            Write-Output "Retry packing..."
            dotnet pack -c Release /p:Version=${env:build_version} -o ./packages
            if (!$?) { exit 1 }
        }
        "format" {
            Write-Output "Format..."
            dotnet format
            if (!$?) { exit 1 }
        }
        "dev-deps" {
            Write-Output "Install dev deps..."
            dotnet nuget add source https://sparkshine.pkgs.visualstudio.com/StardustDL/_packaging/feed/nuget/v3/index.json -n aza -u sparkshine -p $env:NUGET_AUTH_TOKEN --store-password-in-clear-text
            if (!$?) { exit 1 }
            npm install --global gulp
            if (!$?) { exit 1 }
            dotnet tool install --global Microsoft.Web.LibraryManager.Cli
            if (!$?) { exit 1 }
            dotnet tool install dotnet-reportgenerator-globaltool --tool-path ./tools
            if (!$?) { exit 1 }
        }
        "report" {
            ./tools/reportgenerator -reports:./reports/test/coverage.xml -targetdir:./reports/test
            if (!$?) { exit 1 }
            if (-not (Test-Path -Path "reports/benchmark")) {
                New-Item -Path "reports/benchmark" -ItemType Directory
                if (!$?) { exit 1 }
            }
            if (!$?) { exit 1 }
            Copy-Item ./BenchmarkDotNet.Artifacts/* ./reports/benchmark -Recurse
            if (!$?) { exit 1 }
        }
        "test" {
            Write-Output "Test..."
            if (-not (Test-Path -Path "reports/test")) {
                New-Item -Path "reports/test" -ItemType Directory
                if (!$?) { exit 1 }
            }
            dotnet test -c Release /p:CollectCoverage=true /p:CoverletOutput=../../reports/test/coverage.json /p:MergeWith=../../reports/test/coverage.json /maxcpucount:1
            if (!$?) { exit 1 }
            dotnet test -c Release ./test/Test.Base /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=../../reports/test/coverage.xml /p:MergeWith=../../reports/test/coverage.json
            if (!$?) { exit 1 }
        }
        "benchmark" {
            Write-Output "Benchmark..."
            dotnet run -c Release --project ./test/Benchmark.Base
            if (!$?) { exit 1 }
        }
        Default {
            Write-Output "Unrecognized command"
            exit -1
        }
    }
}
else {
    Write-Output "Missing command"
    exit -1
}