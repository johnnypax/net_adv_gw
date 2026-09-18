using System.Text.Json;
using System.Text.Json.Nodes;

static Dictionary<string, object> CreateOperation(
    DateTimeOffset timestamp, 
    string version,
    bool failure,
    int durationMs,
    Guid correlationId) => new()
    {
        ["timestamp"] = timestamp,
        ["level"] = failure ? "Error" : "Information",
        ["action"] = "ticket.save",
        ["outcome"] = failure ? "failure" : "success",
        ["applicationVersion"] = version,
        ["correlationId"] = correlationId,
        ["durationMs"] = durationMs
    };

static Guid DeterministicGuid(int value)
{
    Span<byte> bytes = stackalloc byte[16];
    BitConverter.TryWriteBytes(bytes, value);
    return new Guid(bytes);
}

string outputDir = args.ElementAtOrDefault(0) ?? "logs";
Directory.CreateDirectory(outputDir);
string outputPath = Path.Combine(outputDir, "release-incident.json");

var events = new List<Dictionary<string, object>>();
DateTimeOffset start = DateTimeOffset.UtcNow.AddMinutes(-130);
Guid deploymentId = Guid.NewGuid();


for (int i = 0; i<60; i++)
{
    bool failure = i % 20 == 0;
    events.Add(CreateOperation(
        start.AddMinutes(i),
        "1.0.0",
        failure,
        durationMs: 40 + i % 30,
        correlationId: DeterministicGuid(i)
        ));
}

events.Add(new Dictionary<string, object>
{
    ["timestamp"] = start.AddMinutes(60),
    ["level"] = "Information",
    ["action"] = "deployment.activated",
    ["outcome"] = "success",
    ["applicationVersion"] = "1.1.0",
    ["deploymentId"] = deploymentId,
    ["correlationId"] = deploymentId,
    ["durationMs"] = 0
});

for (int index = 0; index < 60; index++)
{
    bool failure = index % 3 == 0;
    events.Add(CreateOperation(
        start.AddMinutes(61 + index),
        "1.1.0",
        failure,
        durationMs: 150 + index % 80,
        correlationId: DeterministicGuid(1_000 + index)));
}

IEnumerable<string> lines = events.Select(itm => JsonSerializer.Serialize(itm));
await File.WriteAllLinesAsync(outputPath, lines);

Console.WriteLine($"Creati {events.Count} eventi: {Path.GetFullPath(outputPath)}");