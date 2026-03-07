# Script to run API, gRPC server, and tests

Write-Host "Starting gRPC server..." -ForegroundColor Green
$grpcProcess = Start-Process -FilePath "dotnet" -ArgumentList "run --project Library.Backend.Grpc" -PassThru -NoNewWindow

Write-Host "Starting API..." -ForegroundColor Green
$apiProcess = Start-Process -FilePath "dotnet" -ArgumentList "run --project Library.Api" -PassThru -NoNewWindow

Write-Host "Waiting for servers to start (10 seconds)..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

Write-Host "Running tests..." -ForegroundColor Green
dotnet test

Write-Host "`nStopping servers..." -ForegroundColor Yellow
Stop-Process -Id $grpcProcess.Id -Force -ErrorAction SilentlyContinue
Stop-Process -Id $apiProcess.Id -Force -ErrorAction SilentlyContinue

Write-Host "Done!" -ForegroundColor Green
