Customer[] customers = new Customer[]
{
    new Customer("C-1", "Alice"),
    new Customer("C-2", "Bob"),
    new Customer("C-3", "Charlie"),
    new Customer("C-4", "Leslie")
};

Order[] orders = new Order[]
{
    new Order("O-1", "C-1", 100.0m),    //G1
    new Order("O-2", "C-1", 150.0m),    //G1
    new Order("O-3", "C-2", 200.0m),    //G2
    new Order("O-4", "C-3", 50.0m),     //G3
    new Order("O-5", "C-3", 75.0m)      //G3
};

//var totals = orders
//    .GroupBy(order => order.CustomerId) //GX
//    .Select(group => new
//    {
//        CustomerId = group.Key,
//        TotalAmount = group.Sum(order => order.Total)
//    });

//var totals = orders
//    .GroupBy(order => order.CustomerId) //GX
//    .Select(group => new
//    {
//        CustomerId = group.Key,
//        TotalAmount = group.Sum(order => order.Total)
//    });

//foreach (var t in totals)
//{
//    Console.WriteLine(t);
//}

IEnumerable<CustomerTotal> totals = orders
    .GroupBy(order => order.CustomerId) //GX
    .Select(group => new CustomerTotal(group.Key, group.Sum(order => order.Total)));

//foreach (var t in totals)
//{
//    Console.WriteLine(t);
//}

var report = customers.LeftJoin(
    totals,
    customer => customer.Id,
    total => total.CustomerId,
    (customer, total) => new
    {
        CustomerId = customer.Id,
        CustomerName = customer.Name,
        TotalAmount = total?.TotalAmount ?? 0.0m
    });

foreach(var row in report)
{
    Console.WriteLine($"CustomerId: {row.CustomerId}, CustomerName: {row.CustomerName}, TotalAmount: {row.TotalAmount}");
}

public sealed record Customer(string Id, string Name);
public sealed record Order(string Id, string CustomerId, decimal Total);
public sealed record CustomerTotal(string CustomerId, decimal TotalAmount)
{
    public override string ToString() => $"CustomerId: {CustomerId}, TotalAmount: {TotalAmount}";
};
