using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace IntegritaSicurezza;
internal class DatabaseService
{
    public async Task InitializeDatabaseAsync(string databasePath)
    {
        await using var connection = Open(databasePath);
        await connection.OpenAsync();

        await using SqliteCommand command = connection.CreateCommand();
        command.CommandText = """
            PRAGMA user_version = 1;
            CREATE TABLE IF NOT EXISTS tickets (
                id INTEGER PRIMARY KEY,
                title TEXT NOT NULL
            );
            INSERT OR IGNORE INTO tickets(id, title) VALUES
                (1, 'Stampante reception'),
                (2, 'VPN sede distaccata');
            """;
        await command.ExecuteNonQueryAsync();
    }

    public async Task<BackupManifest> CreateVerifiedBackupAsync(
        string databasePath, string backupPath, CancellationToken cancellationToken = default)
    {
        string pendingPath = backupPath + ".pending";
        string manifestPath = backupPath + ".manifest.json";

        if (File.Exists(pendingPath))
        {
            File.Delete(pendingPath);
        }

        await using (var source = Open(databasePath))
        await using (var destination = Open(pendingPath))
        {
            await source.OpenAsync(cancellationToken);
            await destination.OpenAsync(cancellationToken);

            //Backup DB
            source.BackupDatabase(destination);
        }

        VerificationResult integrity = await CheckIntegrityAsync(pendingPath, cancellationToken);
        if (!integrity.IsValid)
        {
            throw new InvalidDataException($"Backup non integro: {integrity.Message}");
        }

        var info = new FileInfo(pendingPath);
        var manifest = new BackupManifest(
            Path.GetFileName(backupPath),
            info.Length,
            await ComputeSha256Async(pendingPath, cancellationToken),
            DateTimeOffset.Now,
            await ReadSchemaVersionAsync(pendingPath, cancellationToken)
            );

        File.Move(pendingPath, backupPath, overwrite: true);

        await File.WriteAllTextAsync(
            manifestPath, JsonSerializer.Serialize(manifest), cancellationToken);

        return manifest;
    }

    public async Task<VerificationResult> VerifyBackupAsync(
        string backupPath, CancellationToken cancellationToken = default)
    {
        string manifestPath = backupPath + ".manifest.json";
        if(!File.Exists(backupPath) || !File.Exists(manifestPath))
        {
            return new(false, "Backup o manifest mancante!");
        }

        BackupManifest? manifest = JsonSerializer.Deserialize<BackupManifest>(
            await File.ReadAllTextAsync(manifestPath, cancellationToken));

        if(manifest is null)
        {
            return new(false, "Manifest non leggibile!");
        }

        string backupHash = await ComputeSha256Async(backupPath, cancellationToken);
        if(!CryptographicOperations.FixedTimeEquals(
            Convert.FromHexString(backupHash), Convert.FromHexString(manifest.Sha256))
            ){
            return new(false, "Checksum SHA-256 differente!");
        }

        return await CheckIntegrityAsync(backupPath, cancellationToken);
    }

    public async Task<VerificationResult> CheckIntegrityAsync(
        string databasePath,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var connection = Open(databasePath, readOnly: true);
            await connection.OpenAsync(cancellationToken);
            await using SqliteCommand command = connection.CreateCommand();
            command.CommandText = "PRAGMA integrity_check;";

            string? result = Convert.ToString(await command.ExecuteScalarAsync(cancellationToken));
            return string.Equals(result, "ok", StringComparison.OrdinalIgnoreCase)
                ? new(true, "integrity_check: ok")
                : new(false, result ?? "Nessun risultato.");
        }
        catch (SqliteException exception)
        {
            return new(false, $"SQLite: {exception.SqliteErrorCode} - {exception.Message}");
        }
    }

    private static async Task<int> ReadSchemaVersionAsync(string path, CancellationToken cancellationToken)
    {
        await using var connection = Open(path, readOnly: true);
        await connection.OpenAsync(cancellationToken);

        await using SqliteCommand command = connection.CreateCommand();
        command.CommandText = "PRAGMA user_version";
        return Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken));
    }

    private static async Task<string> ComputeSha256Async(
        string path,
        CancellationToken cancellationToken)
    {
        await using FileStream stream = File.OpenRead(path);
        byte[] hash = await SHA256.HashDataAsync(stream, cancellationToken);
        return Convert.ToHexString(hash);
    }

    public static SqliteConnection Open(string path, bool readOnly = false) =>
        new( new SqliteConnectionStringBuilder
        {
            DataSource = path,
            Mode = readOnly ? SqliteOpenMode.ReadOnly : SqliteOpenMode.ReadWriteCreate,
            Pooling = false
        }.ToString());
}
