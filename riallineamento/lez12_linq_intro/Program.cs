List<int> values = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

//IEnumerable<int> query = values.Where(value =>
//{
//    //Console.WriteLine($"Filtering value: {value}");
//    return value > 5;
//});

//IEnumerable<int> query = values.Where(value =>
//{
//    Console.WriteLine($"Filtering value > 5: {value}");
//    return value > 5;
//}).Where(value =>
//{
//    Console.WriteLine($"Filtering value < 9: {value}");
//    return value < 9;
//});

IEnumerable<int> query = values
    .Where(value => value > 5)
    //.Where(value => value < 9)
    .Select(value => value * 10);

values.Add(11);                     // Nessun effetto


Console.WriteLine(string.Join(", ", query));

// Snapshot

int[] snapshot = query.ToArray();  // Esegue la query e crea uno snapshot dei risultati

values.Add(12);

Console.WriteLine(string.Join(", ", snapshot));