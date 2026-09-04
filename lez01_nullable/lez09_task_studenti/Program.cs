using System.Collections;

Catalog<Studente> catalog = new();

catalog.Add(new Studente("S001", "Mario Rossi", 2023));
catalog.Add(new Studente("S002", "Luigi Bianchi", 2023));
catalog.Add(new Studente("S002", "Luigi Bianchi", 2023));

Console.WriteLine(catalog.ToString());

public sealed record Studente(String Matricola, string Nome, int Anno) : IHasMatricolaAnno;

public interface IHasMatricolaAnno
{
    String Matricola { get; }
    int Anno { get; }
}

public sealed class Catalog<T> where T : IHasMatricolaAnno
{
    private readonly Queue<T> _items = new();
    private readonly HashSet<string> _already_inserted = new();

    public void Add(T item)
    {
        if (!_already_inserted.Add($"${item.Matricola}_{item.Anno}"))
        {
            throw new InvalidOperationException($"Item with Matricola {item.Matricola} and Anno {item.Anno} already exists.");
        }

        _items.Enqueue(item);
    }

}