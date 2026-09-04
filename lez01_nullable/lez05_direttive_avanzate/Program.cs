//#if JSON_EXPORT
//Console.WriteLine("Exporting in JSON format");
//#endif
//#if TEXT_EXPORT
//Console.WriteLine("Exporting in TEXT format");
//#endif
//#if LEGACY_EXPORT
//Console.WriteLine("Exporting in LEGACY format");
//#endif

//Console.WriteLine("Hello, World!");

//------------------------------

using System.Text.Json;

Report report = new("Vendite", [12, 18, 15]);

IReportExporter exporter;
#if JSON_EXPORT
    exporter = new JsonReportExporter();
#elif TEXT_EXPORT
    exporter = new TextReportExporter();
#else
    #pragma warning disable CS0618 // Type or member is obsolete
    exporter = new LegacyReportExporter();
    #pragma warning restore CS0618
#endif

Console.WriteLine(exporter.Export(report));


#region Definizioni di classi ed Exporter
public sealed record Report(string Title, int[] Rows);

public interface IReportExporter
{
    string Name { get; }
    string Export(Report report);
}

public sealed class TextReportExporter : IReportExporter
{
    public string Name => "Text";

    public string Export(Report report) =>
        $"{report.Title}: {string.Join(", ", report.Rows)}";
}

public sealed class JsonReportExporter : IReportExporter
{
    public string Name => "JSON";

    public string Export(Report report) => JsonSerializer.Serialize(report);
}

[Obsolete("Compatibilita temporanea: preferire Text o JSON.")]
public sealed class LegacyReportExporter : IReportExporter
{
    public string Name => "Legacy";

    public string Export(Report report) =>
        $"{report.Title}|{string.Join(";", report.Rows)}";
}
#endregion