using System.Reflection;
using System.Text.Json;

// Assembly.GetEntryAssembly restituisce il programma avviato, non una libreria usata dal programma.
Assembly application = Assembly.GetEntryAssembly()
    ?? throw new InvalidOperationException("Assembly di avvio non disponibile.");

// AssemblyInformationalVersion e' adatta a mostrare anche un suffisso o l'hash del commit.
string version = application
    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
    .InformationalVersion ?? "versione sconosciuta";

Console.WriteLine($"Offline Service Desk - versione {version}");
Console.WriteLine($"Runtime: {Environment.Version}");
Console.WriteLine($"Sistema: {Environment.OSVersion}");

// I dati modificabili vanno fuori dalla cartella di installazione.
// In questo modo un aggiornamento dell'eseguibile non cancella le preferenze dell'utente.
string dataDirectory = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    "OfflineServiceDesk",
    "Modulo09");

Directory.CreateDirectory(dataDirectory);
string settingsPath = Path.Combine(dataDirectory, "user-settings.json");

if (!File.Exists(settingsPath))
{
    UserSettings defaults = new("Operatore aula", "it-IT", DateTimeOffset.UtcNow);

    // WriteIndented rende il file leggibile durante la dimostrazione in aula.
    string json = JsonSerializer.Serialize(defaults, new JsonSerializerOptions { WriteIndented = true });
    await File.WriteAllTextAsync(settingsPath, json);
}

string savedJson = await File.ReadAllTextAsync(settingsPath);
UserSettings settings = JsonSerializer.Deserialize<UserSettings>(savedJson)
    ?? throw new InvalidDataException("Il file delle impostazioni non e' valido.");

Console.WriteLine($"Profilo caricato: {settings.DisplayName} ({settings.Culture})");
Console.WriteLine($"Dati persistenti: {settingsPath}");
Console.WriteLine("Aggiornare il programma non modifica questo file.");

// Un record e' sufficiente per un piccolo oggetto di configurazione immutabile.
internal sealed record UserSettings(
    string DisplayName,
    string Culture,
    DateTimeOffset CreatedAtUtc);