namespace WebApp
{
    using System.Reflection;

    public static class Util
    {
        public static string GetResourcePath(string name)
        {
            var location = Assembly.GetExecutingAssembly().Location;
            var uriBuilder = new UriBuilder(location);
            var path = Uri.UnescapeDataString(uriBuilder.Path);
            return Path.Combine(Path.GetDirectoryName(path) ?? "", name);
        }
    }
}
