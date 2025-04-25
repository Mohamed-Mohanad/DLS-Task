using System.Reflection;

namespace DLS.Infrastructure;

public class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
