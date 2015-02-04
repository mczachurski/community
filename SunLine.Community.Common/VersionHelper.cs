using System;
using System.IO;
using System.Reflection;

namespace SunLine.Community.Common
{
    public static class VersionHelper
    {
        private static string _version;
        private static string _compileDate;

        public static string GetApplicationVersion()
        {
            if (_version != null)
            {
                return _version;
            }

            Assembly asm = Assembly.GetExecutingAssembly();
            Version version = asm.GetName().Version;
            _version = string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);

            return _version;
        }

        public static string GetApplicationCompileDate()
        {
            if (_compileDate != null)
            {
                return _compileDate;
            }

            Assembly asm = Assembly.GetExecutingAssembly();

            DateTime lastWriteTime = DateTime.UtcNow;
            if (File.Exists(asm.Location))
            {
                FileInfo fi = new FileInfo(asm.Location);
                lastWriteTime = fi.LastWriteTime;
            }
				
            _compileDate = lastWriteTime.Ticks.ToString("X");

            return _compileDate;
        }
    }
}
