$ErrorActionPreference = "Stop"

Write-Host "Generazione coppia di chiavi RSA..."

$rsa = New-Object System.Security.Cryptography.RSACryptoServiceProvider(2048)

try {

    # true = include anche la chiave privata
    $privateKey = $rsa.ExportCspBlob($true)

    # false = esporta soltanto la chiave pubblica
    $publicKey = $rsa.ExportCspBlob($false)

    [System.IO.File]::WriteAllBytes(
        "$PSScriptRoot\private.key",
        $privateKey
    )

    [System.IO.File]::WriteAllBytes(
        "$PSScriptRoot\public.key",
        $publicKey
    )

    Write-Host "Chiavi generate correttamente."
    Write-Host "Private key: $PSScriptRoot\private.key"
    Write-Host "Public key : $PSScriptRoot\public.key"
}
finally {

    $rsa.Dispose()
}