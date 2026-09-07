using operazioni_file;
using operazioni_file.Models;

JsonSettingsStore settingsStore = new JsonSettingsStore();

string pathSettings = Path.Combine(Path.GetTempPath(), "settings-gio.json");
Console.WriteLine(pathSettings);

await settingsStore.SaveAtomicallyAsync(pathSettings, new AppSettings("Giovanni Pace", 5));