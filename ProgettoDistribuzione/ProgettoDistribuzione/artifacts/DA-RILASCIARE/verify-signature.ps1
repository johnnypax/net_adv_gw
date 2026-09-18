$ErrorActionPreference = "Stop"

$publicKeyPath = Join-Path $PSScriptRoot "public.key"
$manifestPath = Join-Path $PSScriptRoot "manifest.json"
$signaturePath = Join-Path $PSScriptRoot "manifest.sig"

Write-Host "=== VERIFICA FIRMA ==="
Write-Host ""

Write-Host "Public key:"
Write-Host $publicKeyPath

Write-Host "Manifest:"
Write-Host $manifestPath

Write-Host "Firma:"
Write-Host $signaturePath

Write-Host ""

if (-not (Test-Path $publicKeyPath)) {
    throw "public.key non trovato: $publicKeyPath"
}

if (-not (Test-Path $manifestPath)) {
    throw "manifest.json non trovato: $manifestPath"
}

if (-not (Test-Path $signaturePath)) {
    throw "manifest.sig non trovato: $signaturePath"
}

$publicKey = [System.IO.File]::ReadAllBytes($publicKeyPath)

$rsa = New-Object System.Security.Cryptography.RSACryptoServiceProvider

try {

    $rsa.ImportCspBlob($publicKey)

    [byte[]]$manifestBytes =
        [System.IO.File]::ReadAllBytes($manifestPath)

    [byte[]]$signature =
        [System.IO.File]::ReadAllBytes($signaturePath)

    $sha256 =
        New-Object System.Security.Cryptography.SHA256CryptoServiceProvider

    try {

        $valid = $rsa.VerifyData(
            $manifestBytes,
            $sha256,
            $signature
        )

    }
    finally {

        $sha256.Dispose()
    }

    Write-Host ""

    if ($valid) {
        Write-Host "FIRMA VALIDA"
    }
    else {
        Write-Host "FIRMA NON VALIDA"
    }
}
finally {

    $rsa.Dispose()
}