using OperazioniParallele;

#region Versione senza progress
//(string source, string output) = await DemoFiles.CreateAsync();

//var service = new ImportService();

//ImportResult result = await service.ImportAsync(source, output, null);

//Console.WriteLine($"Import completato: {result.ImportedCount} righe.");
//Console.WriteLine($"Output: {result.OutputPath}");
#endregion

#region Versione CON progress
(string source, string output) = await DemoFiles.CreateAsync();

var service = new ImportService();

int lastPercentage = -1;
var progress = new Progress<ImportProgress>(value =>
{
    int band = value.Percentage / 10 * 10;

    if(band > lastPercentage)
    {
        lastPercentage = band;
        Console.WriteLine($"Avanzamento: {band}% - {value.CurrentItem}");
    }

});

ImportResult result = await service.ImportAsync(source, output, progress);

Console.WriteLine($"Import completato: {result.ImportedCount} righe.");
Console.WriteLine($"Output: {result.OutputPath}");
#endregion