using MyExtensions;
using System.Data;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using static System.Net.Mime.MediaTypeNames;

namespace Project;

public class Program
{
    
    static void Main()
    {
        Engine engine = new Engine();
        engine.Run();
    }
}