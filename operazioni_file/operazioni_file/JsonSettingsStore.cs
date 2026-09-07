using operazioni_file.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace operazioni_file;
internal class JsonSettingsStore
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
    };

    // path: app.json
    public async Task SaveAtomicallyAsync(
        string path, AppSettings settings, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path, nameof(path));
        ArgumentNullException.ThrowIfNull(settings, nameof(settings));

        string? directory = Path.GetFullPath(path);
        if (Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string temporaryPath = path + ".tmp";       // app.json.tmp

        try
        {
            await using (var stream = new FileStream(
                temporaryPath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 4096,
                useAsync: true))
            {
                await JsonSerializer.SerializeAsync(
                       stream, settings, JsonOptions, cancellationToken);

                await stream.FlushAsync(cancellationToken);
            }

            File.Move(temporaryPath, path, overwrite: true);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            if(File.Exists(temporaryPath))
                File.Delete(temporaryPath);
        }
    }

    public static async Task<AppSettings?> LoadAsync(
                                                string path,
                                                CancellationToken cancellationToken = default)
    {
        if (!File.Exists(path))
        {
            return null; // Primo avvio: assenza prevista, non eccezione.
        }

        await using FileStream stream = File.OpenRead(path);

        return await JsonSerializer.DeserializeAsync<AppSettings>(
            stream,
            cancellationToken: cancellationToken);
    }

}