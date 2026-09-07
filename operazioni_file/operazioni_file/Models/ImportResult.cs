using Microsoft.VisualBasic.FileIO;
using operazioni_file.Models;
using System.Diagnostics.CodeAnalysis;

namespace operazioni_file.Models;

public sealed class CsvTicketImporter
{
    public ImportResult Import(string path)
    {
        var valid = new List<Ticket>();
        var errors = new List<ImportError>();

        using var parser = new TextFieldParser(path);
        parser.TextFieldType = FieldType.Delimited;
        parser.SetDelimiters(",");
        parser.HasFieldsEnclosedInQuotes = true;

        int lineNumber = 0;

        while (!parser.EndOfData)
        {
            lineNumber++;

            try
            {
                // ReadFields gestisce anche virgole racchiuse tra virgolette.
                string[]? fields = parser.ReadFields();

                if (fields is null)
                {
                    errors.Add(new(lineNumber, "", "Riga non leggibile."));
                    continue;
                }

                if (lineNumber == 1 && IsHeader(fields))
                {
                    continue; // L'intestazione descrive le colonne, non è un ticket.
                }

                if (!TryMap(fields, out Ticket? ticket, out string? error))
                {
                    errors.Add(new(
                        lineNumber,
                        string.Join(',', fields),
                        error ?? "Errore sconosciuto."));
                    continue;
                }

                // Il controllo precedente garantisce ticket non null.
                valid.Add(ticket);
            }
            catch (MalformedLineException exception)
            {
                // Un record guasto non interrompe l'intero batch.
                errors.Add(new(lineNumber, exception.Message, "CSV non valido."));
            }
        }

        return new ImportResult(valid, errors);
    }

    private static bool IsHeader(string[] fields) =>
        fields.Length >= 3 &&
        string.Equals(fields[0], "externalId", StringComparison.OrdinalIgnoreCase);

    private static bool TryMap(
        string[] fields,
        // NotNullWhen(true) collega il bool restituito al parametro out:
        // se il metodo restituisce true, il compilatore sa che ticket non è null.
        [NotNullWhen(true)] out ImportedTicket? ticket,
        out string? error)
    {
        ticket = null;
        error = null;

        if (fields.Length != 3)
        {
            error = $"Attese 3 colonne, trovate {fields.Length}.";
            return false;
        }

        string externalId = fields[0].Trim();
        string title = fields[1].Trim();
        string priority = fields[2].Trim().ToLowerInvariant();

        if (externalId.Length == 0 || title.Length == 0)
        {
            error = "ID esterno e titolo sono obbligatori.";
            return false;
        }

        if (priority is not ("low" or "normal" or "high" or "critical"))
        {
            error = $"Priorità non riconosciuta: {priority}.";
            return false;
        }

        ticket = new ImportedTicket(externalId, title, priority);
        return true;
    }
}
