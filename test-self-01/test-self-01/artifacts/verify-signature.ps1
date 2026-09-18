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

# Controllo preventivo dei file
if (-not (Test-Path $publicKeyPath)) {
    throw "public.key non trovato: $publicKeyPath"
}

if (-not (Test-Path $manifestPath)) {
    throw "manifest.json non trovato: $manifestPath"
}

if (-not (Test-Path $signaturePath)) {
    throw "manifest.sig non trovato: $signaturePath"
}

# Leggo la chiave pubblica
$publicKey = [System.IO.File]::ReadAllBytes($publicKeyPath)

# Creo RSA compatibile con Windows PowerShell 5.1
$rsa = New-Object System.Security.Cryptography.RSACryptoServiceProvider

try {

    # Importo il CSP Blob generato da generate-keys.ps1
    $rsa.ImportCspBlob($publicKey)

    # Leggo manifest e firma
    [byte[]]$manifestBytes = [System.IO.File]::ReadAllBytes($manifestPath)
    [byte[]]$signature = [System.IO.File]::ReadAllBytes($signaturePath)

    # Creo esplicitamente SHA256 per evitare overload ambigui
    $sha256 = New-Object System.Security.Cryptography.SHA256CryptoServiceProvider

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