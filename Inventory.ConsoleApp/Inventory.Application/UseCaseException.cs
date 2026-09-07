using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Application;
public sealed class UseCaseException(string message) : Exception(message);