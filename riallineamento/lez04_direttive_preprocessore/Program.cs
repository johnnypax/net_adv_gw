//Direttive al di fuori del progetto, single file approach
//#:property LangVersion=14.0
//#:property TargetFrameworks=net10.0
//#:property Nullable=enable

//#define PROFILO_TELEMETRIA

//#if DEBUG
//Console.WriteLine("Sono in Debug");
//#else
//    Console.WriteLine("Sono in Release");
//#endif

//string buildMode =
//#if DEBUG
//    "Debug";
//#elif PROFILO_TELEMETRIA
//    "Profilo Telemetria";
//#else
//    "Release";
//#endif

//Console.WriteLine(buildMode);

//--------------------------------

//#if DEBUG
//    #define PROFILO_TELEMETRIA
//#else
//    #define PROFILO_LOGGING
//#endif

//string profiloAttivo =
//#if PROFILO_TELEMETRIA
//    "Profilo Telemetria";
//#elif PROFILO_LOGGING
//    "Profilo Logging";
//#endif

//Console.WriteLine(profiloAttivo);

//--------------------------------

var versione_ = 
#if NET10_0_OR_GREATER
"Sono in .NET 10.0 o superiore";
#elif NET8_0_OR_GREATER
"Sono in .NET 8.0 o superiore";
#else
"Sono in una versione di .NET inferiore a 8.0";
#endif

Console.WriteLine(versione_);