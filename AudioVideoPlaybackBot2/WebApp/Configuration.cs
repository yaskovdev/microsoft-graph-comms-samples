namespace WebApp;

using System.Collections.Immutable;
using System.Globalization;
using System.Net;
using Controllers;
using Microsoft.Skype.Bots.Media;

public class Configuration : IConfiguration
{
    private readonly IImmutableDictionary<string, string> _config;

    public string ServiceDnsName => _config["ServiceDnsName"];

    public int BotInstanceExternalPort => int.Parse(_config["BotInstanceExternalPort"]);

    public Uri CallControlBaseUrl => new Uri($"https://{ServiceDnsName}:{BotInstanceExternalPort}/{HttpRouteConstants.CallSignalingRoutePrefix}");

    public Uri PlaceCallEndpointUrl => new Uri("https://graph.microsoft.com/v1.0"); // TODO: may this be a problem?

    public string AadAppId => _config["AadAppId"];

    public string AadAppSecret => _config["AadAppSecret"];

    public MediaPlatformSettings MediaPlatformSettings { get; }

    public Configuration()
    {
        var config = new Dictionary<string, string>();
        List<string[]> parameters = Load(Util.GetResourcePath(".env"));
        foreach (var parameter in parameters)
        {
            var key = parameter[0].Split("__")[1];
            config[key] = parameter[1];
        }
        _config = config.ToImmutableDictionary();
        MediaPlatformSettings = new MediaPlatformSettings
        {
            MediaPlatformInstanceSettings = new MediaPlatformInstanceSettings
            {
                CertificateThumbprint = config["CertificateThumbprint"],
                InstanceInternalPort = int.Parse(config["MediaInternalPort"], CultureInfo.InvariantCulture),
                InstancePublicIPAddress = IPAddress.Any,
                InstancePublicPort = int.Parse(config["MediaInstanceExternalPort"], CultureInfo.InvariantCulture),
                ServiceFqdn = config["MediaDnsName"]
            },
            ApplicationId = AadAppId,
        };
    }

    public void Dispose()
    {
    }

    private static List<string[]> Load(string filePath) =>
        File
            .ReadAllLines(filePath)
            .Select(line => line.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries))
            .Where(parts => parts.Length == 2)
            .ToList();
}
