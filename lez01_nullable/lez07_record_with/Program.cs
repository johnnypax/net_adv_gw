//Creare `Playlist(string Name, List<string> Songs)`, copiarla con `with`, modificare la lista della copia e osservare l'originale.
//Correggere il problema creando una nuova lista durante la copia.

Playlist originale = new ("Rock anni 80", ["Sweet Child O' Mine", "Livin' on a Prayer", "Eye of the Tiger"]);

Playlist espansione = originale with { Name = "Rock di Giovanni" };

espansione.Songs.Add("Test");

Playlist indipendente = originale with
{
    Name = "Rock indipendente",
    Songs = [.. originale.Songs]
};

espansione.Songs.Add("Prova");

Console.WriteLine("Fine");
public sealed record Playlist(string Name, List<string> Songs);