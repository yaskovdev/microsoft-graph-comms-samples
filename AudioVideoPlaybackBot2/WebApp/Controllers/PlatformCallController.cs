// <copyright file="PlatformCallController.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
// </copyright>

namespace WebApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Graph.Communications.Client;
    using Bot;
    using Microsoft.AspNetCore.Mvc.WebApiCompatShim;
    using Microsoft.Graph.Communications.Common.Telemetry;

    /// <summary>
    /// Entry point for handling call-related web hook requests from Skype Platform.
    /// </summary>
    [ApiController]
    public class PlatformCallController : ControllerBase
    {
        private readonly IBot _bot;

        public PlatformCallController(IBot bot)
        {
            _bot = bot;
        }

        /// <summary>
        /// Gets the logger instance.
        /// </summary>
        private IGraphLogger Logger => _bot.Logger;

        /// <summary>
        /// Gets a reference to singleton sample bot/client instance.
        /// </summary>
        private ICommunicationsClient Client => _bot.Client;

        /// <summary>
        /// Handle a callback for an existing call.
        /// </summary>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpPost(HttpRouteConstants.CallSignalingRoutePrefix)]
        public async Task<HttpResponseMessage> OnIncomingRequestAsync()
        {
            var httpRequestMessageFeature = new HttpRequestMessageFeature(Request.HttpContext);
            var request = httpRequestMessageFeature.HttpRequestMessage;
            this.Logger.Info($"Received HTTP {this.Request.Method}, {request.RequestUri}");

            // Pass the incoming message to the sdk. The sdk takes care of what to do with it.
            var response = await this.Client.ProcessNotificationAsync(request).ConfigureAwait(false);

            // Enforce the connection close to ensure that requests are evenly load balanced so
            // calls do no stick to one instance of the worker role.
            response.Headers.ConnectionClose = true;
            return response;
        }
    }
}