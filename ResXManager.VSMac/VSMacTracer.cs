using ResXManager.Infrastructure;
using System;
using System.ComponentModel.Composition;

namespace ResXManager.VSMac
{
    [Export(typeof(ITracer))]
    public class VSMacTracer : ITracer
    {
        public void TraceError(string value)
        {
            WriteLine("Error: " + value);
        }

        public void TraceWarning(string value)
        {
            WriteLine("Warning: " + value);
        }

        public void WriteLine(string value)
        {
            Console.WriteLine(value);
        }
    }
}