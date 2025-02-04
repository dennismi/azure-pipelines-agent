using Agent.Sdk;
using System;
using System.IO;

namespace Microsoft.VisualStudio.Services.Agent.Util
{
    public static class PathUtil
    {
        public static string PathVariable
        {
            get =>
                PlatformUtil.RunningOnOS == PlatformUtil.OS.Windows
                ? "Path"
                : "PATH";

        }

        public static string PrependPath(string path, string currentPath)
        {
            ArgUtil.NotNullOrEmpty(path, nameof(path));
            if (string.IsNullOrEmpty(currentPath))
            {
                // Careful not to add a trailing separator if the PATH is empty.
                // On OSX/Linux, a trailing separator indicates that "current directory"
                // is added to the PATH, which is considered a security risk.
                return path;
            }

            // Not prepend path if it is already the first path in %PATH%
            if (currentPath.StartsWith(path + Path.PathSeparator, IOUtil.FilePathStringComparison))
            {
                return currentPath;
            }
            else
            {
                return path + Path.PathSeparator + currentPath;
            }
        }

        public static void PrependPath(string directory)
        {
            ArgUtil.Directory(directory, nameof(directory));

            // Build the new value.
            string currentPath = Environment.GetEnvironmentVariable(PathVariable);
            string path = PrependPath(directory, currentPath);

            // Update the PATH environment variable.
            Environment.SetEnvironmentVariable(PathVariable, path);
        }
    }
}