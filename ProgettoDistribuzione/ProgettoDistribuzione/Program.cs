using System.Reflection;
using System.Text.Json;

Assembly application = Assembly.GetEntryAssembly()
    ?? throw new InvalidOperationException("Assembly di avvio non disponibile.");

string version = application
    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
    .InformationalVersion ?? "versione sconosciuta";

Console.WriteLine($"Offline Service Desk - versione {version}");
Console.WriteLine($"Runtime: {Environment.Version}");
Console.WriteLine($"Sistema: {Environment.OSVersion}");

string dataDirectory = 
    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
    "OfflineServiceDeskGW2", "ProgettoDistribuzione");
Directory.CreateDirectory(dataDirectory);

string settingsPath = Path.Combine(dataDirectory, "user-settings.json");

if (!File.Exists(settingsPath))
{
    UserSettings defaults = new("User", "IT", DateTimeOffset.Now);

    //string json = JsonSerializer.Serialize(defaults, new JsonSerializerOptions { WriteIndented = true });
    string json = JsonSerializer.Serialize(defaults);
    await File.WriteAllTextAsync(settingsPath, json);
}

string savedJson = await File.ReadAllTextAsync(settingsPath);

UserSettings settings = JsonSerializer.Deserialize<UserSettings>(savedJson)
    ?? throw new InvalidDataException("Il file delle impostazioni non e' valido.");

Console.WriteLine($"Profilo caricato: {settings.DisplayName} ({settings.Culture})");
Console.WriteLine($"Dati persistenti: {settingsPath}");

internal sealed record UserSettings(
    string DisplayName,
    string Culture,
    DateTimeOffset CreatedAtUtc);
