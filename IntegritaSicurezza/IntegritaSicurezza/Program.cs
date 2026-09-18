using IntegritaSicurezza;

string root =
    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
    "OfficeServiceDesk", "IntegritaSicurezza");
Directory.CreateDirectory(root);

string database = Path.Combine(root, "service-desk.db");
string backup = Path.Combine(root, "service-desk.backup.db");

var service = new DatabaseService();
await service.InitializeDatabaseAsync(database);

BackupManifest manifest = await service.CreateVerifiedBackupAsync(database, backup);
Console.WriteLine($"Backup verificato con manifest: {manifest.FileName}");
Console.WriteLine($"SHA-256: {manifest.Sha256}");

VerificationResult verified = await service.VerifyBackupAsync(backup);
Console.WriteLine($"Verifica manifest: {verified.IsValid} - {verified.Message}");

