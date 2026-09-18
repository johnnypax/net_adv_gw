using IntegritaSicurezza;

string root =
    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
    "OfficeServiceDesk", "IntegritaSicurezza");
Directory.CreateDirectory(root);

string database = Path.Combine(root, "service-desk.db");
string backup = Path.Combine(root, "service-desk.backup.db");
string corrupt = Path.Combine(root, "service-desk.corrupt-test.db");


#region Inizializzazione DB
var service = new DatabaseService();
//await service.InitializeDatabaseAsync(database);
#endregion

#region Creazione del Backup
//BackupManifest manifest = await service.CreateVerifiedBackupAsync(database, backup);
//Console.WriteLine($"Backup verificato con manifest: {manifest.FileName}");
//Console.WriteLine($"SHA-256: {manifest.Sha256}");
#endregion

#region Verifica di un backup esistente tramite Manifest
//VerificationResult verified = await service.VerifyBackupAsync(backup);
//Console.WriteLine($"Verifica manifest: {verified.IsValid} - {verified.Message}");
#endregion

#region Crea la copia corrotta del DB
//await service.CorruptCopyAsync(database, corrupt);
//VerificationResult corruptCheck = await service.CheckIntegrityAsync(corrupt);
//Console.WriteLine($"Verifica della corruzione: {corruptCheck.IsValid} - {corruptCheck.Message}");
#endregion

#region Restoring
VerificationResult corruptCheck = await service.RestoreVerifiedAsync(backup, database);
Console.WriteLine($"Verifica della corruzione: {corruptCheck.IsValid} - {corruptCheck.Message}");
#endregion






