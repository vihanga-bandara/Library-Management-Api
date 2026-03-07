# Script to run API, gRPC server, and tests

$solutionPath = "LibrarySystem.slnx"
$configuration = "Debug"
$grpcProcess = $null
$apiProcess = $null

Write-Host "Restoring solution..." -ForegroundColor Green
dotnet restore $solutionPath
if ($LASTEXITCODE -ne 0) {
	exit $LASTEXITCODE
}

Write-Host "Building solution..." -ForegroundColor Green
dotnet build $solutionPath --configuration $configuration --no-restore
if ($LASTEXITCODE -ne 0) {
	exit $LASTEXITCODE
}

try {
	Write-Host "Starting gRPC server..." -ForegroundColor Green
	$grpcProcess = Start-Process -FilePath "dotnet" -ArgumentList "run --project Library.Backend.Grpc --configuration $configuration --no-build --no-restore" -PassThru -NoNewWindow

	Write-Host "Starting API..." -ForegroundColor Green
	$apiProcess = Start-Process -FilePath "dotnet" -ArgumentList "run --project Library.Api --configuration $configuration --no-build --no-restore" -PassThru -NoNewWindow

	Write-Host "Waiting for servers to start (10 seconds)..." -ForegroundColor Yellow
	Start-Sleep -Seconds 10

	Write-Host "Running tests..." -ForegroundColor Green
	dotnet test $solutionPath --configuration $configuration --no-build --no-restore
	$testExitCode = $LASTEXITCODE
}
finally {
	Write-Host "`nStopping servers..." -ForegroundColor Yellow

	if ($grpcProcess) {
		Stop-Process -Id $grpcProcess.Id -Force -ErrorAction SilentlyContinue
	}

	if ($apiProcess) {
		Stop-Process -Id $apiProcess.Id -Force -ErrorAction SilentlyContinue
	}
}

Write-Host "Done!" -ForegroundColor Green
exit $testExitCode
