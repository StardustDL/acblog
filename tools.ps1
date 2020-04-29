if ($args.Count -gt 0) {
    switch ($args[0]) {
        "dep" {
            Write-Output "Install dependencies..."
            if (!$?) {
                exit 1
            }
        }
        "dep-dev" {
            Write-Output "Install dependencies for development..."
            if (!$?) {
                exit 1
            }
        }
        "format" {
            Write-Output "Formatting..."
            dotnet format
            if (!$?) {
                exit 1
            }
        }
        "clean" {
            Write-Output "Clean generated files.."
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