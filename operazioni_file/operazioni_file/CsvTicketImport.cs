using Microsoft.VisualBasic.FileIO;
using operazioni_file.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace operazioni_file
{
    internal class CsvTicketImport
    {
        public ImportError Import(string path)
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
                    string[]? fields = parser.ReadFields();

                    if (fields is null)
                    {
                        errors.Add(new(lineNumber, "", "Riga non leggibile."));
                        continue;
                    }

                    if (lineNumber == 1)
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
                }
            }
        }
    }
}
