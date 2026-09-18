using System;
using System.Collections.Generic;
using System.Text;

namespace OperazioniParallele;
public static class DemoFiles
{
    public static async Task<(string Source, string Output)> CreateAsync()
    {
        string root = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
            "OfflineServiceDesk", "RepoTicket");
        Directory.CreateDirectory(root);

        string source = Path.Combine(root, "tickets.txt");
        string output = Path.Combine(root, "imported-tickets.json");

        string[] rows = Enumerable.Range(1, 120)
            .Select(n => $"Ticket dimostrativo {n: 000}").ToArray();

        await File.WriteAllLinesAsync(source, rows);
        return (source, output);

    }
}