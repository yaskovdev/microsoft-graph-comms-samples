// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DemoController.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
// </copyright>
// <summary>
//   ScreenshotsController retrieves the screenshots stored by the bot
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApp.Controllers
{
    using System.Diagnostics;
    using System.Net;
    using Bot;
    using System.Text;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Graph.Communications.Common.Telemetry;
    using Microsoft.Graph.Communications.Core.Serialization;
    using Sample.Common.Logging;

    /// <summary>
    /// DemoController serves as the gateway to explore the bot.
    /// From here you can get a list of calls, and functions for each call.
    /// </summary>
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly IBot _bot;
        private readonly IConfiguration _configuration;

        public DemoController(IBot bot, IConfiguration configuration)
        {
            _bot = bot;
            _configuration = configuration;
        }

        /// <summary>
        /// Gets the logger instance.
        /// </summary>
        private IGraphLogger Logger => _bot.Logger;

        /// <summary>
        /// Gets the sample log observer.
        /// </summary>
        private SampleObserver Observer => _bot.Observer;

        /// <summary>
        /// The GET logs.
        /// </summary>
        /// <param name="skip">The skip.</param>
        /// <param name="take">The take.</param>
        /// <returns>
        /// The <see cref="Task" />.
        /// </returns>
        [HttpGet(HttpRouteConstants.Logs + "/")]
        public IActionResult OnGetLogs(
            int skip = 0,
            int take = 1000)
        {
            EventLog.WriteEntry(SampleConstants.EventLogSource, $"Serving {HttpRouteConstants.Logs}/", EventLogEntryType.Information);
            var logs = this.Observer.GetLogs(skip, take);

            return Ok(logs);
        }

        /// <summary>
        /// The GET logs.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="take">The take.</param>
        /// <returns>
        /// The <see cref="Task" />.
        /// </returns>
        [HttpGet(HttpRouteConstants.Logs + "/{filter}")]
        public IActionResult OnGetLogs(
            string filter,
            int skip = 0,
            int take = 1000)
        {
            EventLog.WriteEntry(SampleConstants.EventLogSource, $"Serving {HttpRouteConstants.Logs}/{filter}", EventLogEntryType.Information);
            var logs = this.Observer.GetLogs(filter, skip, take);

            return Ok(logs);
        }

        /// <summary>
        /// The GET calls.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet(HttpRouteConstants.Calls + "/")]
        public IActionResult OnGetCalls()
        {
            EventLog.WriteEntry(SampleConstants.EventLogSource, $"Serving {HttpRouteConstants.Calls}/", EventLogEntryType.Information);
            this.Logger.Info("Getting calls");

            if (_bot.CallHandlers.IsEmpty)
            {
                return StatusCode((int)HttpStatusCode.NoContent);
            }

            var calls = new List<Dictionary<string, string>>();
            foreach (var callHandler in _bot.CallHandlers.Values)
            {
                var call = callHandler.Call;
                var callPath = "/" + HttpRouteConstants.CallRoute.Replace("{callLegId}", call.Id);
                var callUri = new Uri(_configuration.CallControlBaseUrl, callPath).AbsoluteUri;
                var values = new Dictionary<string, string>
                {
                    { "legId", call.Id },
                    { "scenarioId", call.ScenarioId.ToString() },
                    { "call", callUri },
                    { "logs", callUri.Replace("/calls/", "/logs/") },
                    { "changeScreenSharingRole", callUri + "/" + HttpRouteConstants.OnChangeRoleRoute },
                };
                calls.Add(values);
            }

            var serializer = new CommsSerializer(pretty: true);
            var json = serializer.SerializeObject(calls);
            return Ok(json);
        }

        /// <summary>
        /// Change the hue of video for the call.
        /// </summary>
        /// <param name="callLegId">
        /// Id of the call to change hue.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpDelete(HttpRouteConstants.CallRoute)]
        public async Task<IActionResult> OnEndCallAsync(string callLegId)
        {
            EventLog.WriteEntry(SampleConstants.EventLogSource, $"Serving {HttpRouteConstants.CallRoute}", EventLogEntryType.Information);
            this.Logger.Info($"Ending call {callLegId}");

            try
            {
                var removed = await _bot.EndCallByCallLegIdAsync(callLegId).ConfigureAwait(false);
                return removed
                    ? Ok()
                    : NotFound();
            }
            catch (Exception e)
            {
                return Problem(e.ToString());
            }
        }
    }
}