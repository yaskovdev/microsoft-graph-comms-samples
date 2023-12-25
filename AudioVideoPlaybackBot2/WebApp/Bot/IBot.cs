namespace WebApp.Bot;

using System.Collections.Concurrent;
using Controllers;
using Microsoft.Graph;
using Microsoft.Graph.Communications.Calls;
using Microsoft.Graph.Communications.Client;
using Microsoft.Graph.Communications.Common.Telemetry;
using Sample.Common.Logging;

public interface IBot : IDisposable
{
    IGraphLogger Logger { get; }

    SampleObserver Observer { get; }

    internal ConcurrentDictionary<string, CallHandler> CallHandlers { get; }
    
    ICommunicationsClient Client { get; }

    Task<ICall> JoinCallAsync(JoinCallController.JoinCallBody joinCallBody);

    Task ChangeSharingRoleAsync(string callLegId, ScreenSharingRole role);

    Task<bool> EndCallByCallLegIdAsync(string callLegId);
}
