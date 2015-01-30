using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebTests
{
    public static class Resources
    {
        private static readonly Assembly ExecutingAssembly = Assembly.GetExecutingAssembly();

        private static Stream GetResourceStream(string name)
        {
            const string basePath = "WebTests.Resources";
            string resourcePath = basePath + "." + name;
            return ExecutingAssembly.GetManifestResourceStream(resourcePath);
        }

        public static readonly Stream SamplePdfReport = GetResourceStream("Dist.pdf");
    }
}
