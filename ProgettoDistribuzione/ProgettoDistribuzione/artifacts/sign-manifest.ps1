$ErrorActionPreference = "Stop"

$privateKeyPath = Join-Path $PSScriptRoot "private.key"
$manifestPath = Join-Path $PSScriptRoot "manifest.json"
$signaturePath = Join-Path $PSScriptRoot "manifest.sig"

Write-Host "=== FIRMA MANIFEST ==="
Write-Host ""

if (-not (Test-Path $privateKeyPath)) {
    throw "private.key non trovato: $privateKeyPath"
}

if (-not (Test-Path $manifestPath)) {
    throw "manifest.json non trovato: $manifestPath"
}

$privateKey = [System.IO.File]::ReadAllBytes($privateKeyPath)

$rsa = New-Object System.Security.Cryptography.RSACryptoServiceProvider

try {

    $rsa.ImportCspBlob($privateKey)

    [byte[]]$manifestBytes =
        [System.IO.File]::ReadAllBytes($manifestPath)

    $sha256 =
        New-Object System.Security.Cryptography.SHA256CryptoServiceProvider

    try {

        [byte[]]$signature = $rsa.SignData(
            $manifestBytes,
            $sha256
        )

    }
    finally {

        $sha256.Dispose()
    }

    [System.IO.File]::WriteAllBytes(
        $signaturePath,
        $signature
    )

    Write-Host "Manifest firmato correttamente."
    Write-Host "Firma creata:"
    Write-Host $signaturePath
}
finally {

    $rsa.Dispose()
}