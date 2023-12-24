namespace AVPWindowsService
{
    using System;
    using System.IO;
    using System.Linq;

    public static class DotEnv
    {
        public static void Load(string filePath) =>
            File
                .ReadAllLines(filePath)
                .Select(line => line.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries))
                .Where(parts => parts.Length == 2)
                .ToList()
                .ForEach(parts => Environment.SetEnvironmentVariable(parts[0], parts[1]));
    }
}
