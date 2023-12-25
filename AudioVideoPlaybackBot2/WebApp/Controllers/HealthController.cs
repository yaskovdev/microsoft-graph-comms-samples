// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HealthController.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
// </copyright>
// <summary>
//   HealthController retrieves the service health
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApp.Controllers
{
    using System.Diagnostics;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// HealthController serves health status for the service.
    /// </summary>
    [ApiController]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HealthController" /> class.
        /// </summary>
        public HealthController()
        {
        }

        /// <summary>
        /// Handle a callback for an incoming call.
        /// </summary>
        /// <returns>The <see cref="HttpResponseMessage" />.</returns>
        [HttpGet(HttpRouteConstants.HealthRoute)]
        public IActionResult Health()
        {
            EventLog.WriteEntry(SampleConstants.EventLogSource, $"Serving {HttpRouteConstants.HealthRoute}", EventLogEntryType.Information);
            return Ok();
        }
    }
}
