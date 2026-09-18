using System;
using System.Collections.Generic;
using System.Text;

namespace IntegritaSicurezza;

public sealed record BackupManifest(
    string FileName, long SizeBytes, string Sha256, DateTimeOffset CreatedAtUtc, int SchemaVersion);

public sealed record VerificationResult(bool IsValid, string Message);  //Equivalente dell'exception