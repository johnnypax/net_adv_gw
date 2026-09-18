using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace OperazioniParallele;

public sealed record ImportProgress(int Completed, int Total, string CurrentItem)
{
    public int Percentage => Total == 0 ? 100 : Completed * 100 / Total;
}

public sealed record ImportResult(int ImportedCount, string OutputPath);
public sealed record ImportedTicket(int RowNumer, string Title, string Fingerprint);


public sealed class ImportService
{
    public async Task<ImportResult> ImportAsync(
        string sourcePath, 
        string outputPath, 
        IProgress<ImportProgress>? progress = null,
        CancellationToken cancellationToken = default)
    {
        string[] lines = await File.ReadAllLinesAsync(sourcePath, cancellationToken);
        var imported = new ConcurrentBag<ImportedTicket>();
        int completed = 0;

        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = 10,
            CancellationToken = cancellationToken
        };

        await Parallel.ForEachAsync(
            ReadCandidateAsync(lines, cancellationToken),
            options,
            async (candidate, token) =>
            {
                await Task.Delay(500, token);

                byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(candidate.Title));
                imported.Add(
                    new ImportedTicket(
                        candidate.RowNumber, 
                        candidate.Title, 
                        Convert.ToHexString(hash)));

                int current = Interlocked.Increment(ref completed);
                progress?.Report(new ImportProgress(completed, lines.Length, candidate.Title));

                //Console.WriteLine($"Sto importando: {candidate.Title}");
            });

        ImportedTicket[] orderedTickets = imported.OrderBy(t => t.RowNumer).ToArray();

        string temporaryPath = $"{outputPath}.{Guid.NewGuid()}.tmp";

        try
        {
            await File.WriteAllTextAsync(
                temporaryPath, JsonSerializer.Serialize(orderedTickets), cancellationToken);

            File.Move(temporaryPath, outputPath, overwrite: true);
        } finally
        {
            if (File.Exists(temporaryPath))
            {
                File.Delete(temporaryPath);
            }
        }

        return new ImportResult(orderedTickets.Length, outputPath);
    }

    private static async 
        IAsyncEnumerable<(int RowNumber, string Title)> ReadCandidateAsync(
            IEnumerable<string> lines, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        int row = 0;

        foreach(string line in lines)
        {
            cancellationToken.ThrowIfCancellationRequested();

            row++;

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            yield return (row, line.Trim());
            await Task.Yield();
        }
    }
}
