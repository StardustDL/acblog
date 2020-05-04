if ($args.Count -gt 0) {
    switch ($args[0]) {
        "restore" {
            Write-Output "Restore npm..."
            Set-Location src/AcBlog.Client.Components.Markdown ; npm ci ; gulp ; Set-Location ../..
            if (!$?) {
                exit 1
            }
            Set-Location src/AcBlog.Client.Components.CodeEditor ; npm ci ; gulp ; Set-Location ../..
            if (!$?) {
                exit 1
            }
            Set-Location src/AcBlog.Client.Components.Loader ; npm ci ; gulp ; Set-Location ../..
            if (!$?) {
                exit 1
            }
            Set-Location src/AcBlog.Client.WASM ; npm ci ; gulp ; Set-Location ../..
            if (!$?) {
                exit 1
            }
            dotnet restore -s https://api.nuget.org/v3/index.json
            if (!$?) {
                exit 1
            }
        }
        "format" {
            Write-Output "Format..."
            dotnet format
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
            Write-Output "Run WASM..."
            dotnet run -p ./src/AcBlog.Client.WASM
            if (!$?) {
                exit 1
            }
        }
        "wasm-pub" {
            Write-Output "Publish WASM..."
            Set-Location ./src/AcBlog.Client.WASM ; dotnet publish -c Release; Set-Location ../..
            if (!$?) {
                exit 1
            }
        }
        "test" {
            Write-Output "Test..."
            dotnet test /p:CollectCoverage=true
            if (!$?) {
                exit 1
            }
        }
        "benchmark" {
            Write-Output "Benchmark"
            dotnet run -c Release --project ./test/Benchmark.Base
            if (!$?) {
                exit 1
            }
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