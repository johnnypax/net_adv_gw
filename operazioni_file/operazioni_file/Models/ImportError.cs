using System;
using System.Collections.Generic;
using System.Text;

namespace operazioni_file.Models
{
    internal sealed record ImportError(int LineNumber, string RawLine, string Message);
}
