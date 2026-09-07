using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Application;

public sealed record ProductDto
    (Guid Id, string Name, int Quantity, int MinimumStock, bool IsLowStock);