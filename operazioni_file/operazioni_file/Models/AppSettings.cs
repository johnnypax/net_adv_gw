using System;
using System.Collections.Generic;
using System.Text;

namespace operazioni_file.Models;

internal sealed record AppSettings(string OperatorName, int AutoBackupMinutes);
