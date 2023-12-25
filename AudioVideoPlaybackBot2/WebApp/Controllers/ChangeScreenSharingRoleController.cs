// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChangeScreenSharingRoleController.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
// </copyright>
// <summary>
//   ChangeScreenSharingRoleController is a third-party controller (non-Bot Framework) that changes the bot's screen sharing role
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApp.Controllers
{
    using System.Net;
    using Bot;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Graph;

    /// <summary>
    /// ChangeScreenSharingRoleController is a third-party controller (non-Bot Framework) that changes the bot's screen sharing role.
    /// </summary>
    [ApiController]
    public class ChangeScreenSharingRoleController : ControllerBase
    {
        private readonly IBot _bot;

        public ChangeScreenSharingRoleController(IBot bot)
        {
            _bot = bot;
        }

        /// <summary>
        /// Changes screen sharing role.
        /// </summary>
        /// <param name="callLegId">
        /// The call leg identifier.
        /// </param>
        /// <param name="changeRoleBody">
        /// The role to change to.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpPost(HttpRouteConstants.CallRoute + "/" + HttpRouteConstants.OnChangeRoleRoute)]
        public async Task<IActionResult> ChangeScreenSharingRoleAsync(string callLegId, [FromBody] ChangeRoleBody changeRoleBody)
        {
            try
            {
                await _bot.ChangeSharingRoleAsync(callLegId, changeRoleBody.Role).ConfigureAwait(false);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode((int)e.InspectExceptionAndReturnResponse().StatusCode);
            }
        }

        /// <summary>
        /// Request body content to update screen sharing role.
        /// </summary>
        public class ChangeRoleBody
        {
            /// <summary>
            /// Gets or sets the role.
            /// </summary>
            /// <value>
            /// The role to change to.
            /// </value>
            public ScreenSharingRole Role { get; set; }
        }
    }
}
