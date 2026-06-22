$ErrorActionPreference = "Stop"

& (Join-Path $PSScriptRoot "migrate-database.ps1")
if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}

& (Join-Path $PSScriptRoot "install-libs.ps1")
if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}

& (Join-Path $PSScriptRoot "generate-certificate.ps1")
if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}
