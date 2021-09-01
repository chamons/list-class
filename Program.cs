using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Linq;

namespace list_class
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 1) {
                Console.Error.WriteLine ("list_class.exe [assembly]");
                return -1;
            }
            try
            {
                string[] runtimeAssemblies = Directory.GetFiles("/Library/Frameworks/Mono.framework/Versions/Current/lib/mono/4.8-api/", "*.dll");
                var paths = new List<string>(runtimeAssemblies);
                paths.Add(args[0]);
                var resolver = new PathAssemblyResolver(paths);
                
                using (var mlc = new MetadataLoadContext(resolver))
                {
                    Assembly assembly = mlc.LoadFromAssemblyPath(args[0]);
                    foreach (var type in assembly.GetModules().SelectMany(m => m.GetTypes()))
                    {
                        Console.WriteLine($"Name: '{type.Name}'");
                        Console.WriteLine($"Namespace: '{type.Namespace}'");
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error: {e}");
                return -1;
            }
            return 0;
        }
    }
}
