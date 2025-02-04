# Introduction

Local media samples give the developer direct access to the inbound and outbound media streams.  The 2 samples here are focused on different local media scenarios.

## AudioVideoPlaybackBot

The [AudioVideoPlaybackBot](AudioVideoPlaybackBot/README.md) demostrates several features of local media scenarios:
- Plays a movie in multiple resolutions as the main video output feed.
- Listens to dominant speaker events and subscribes to inbound video feeds of those participants.
- Allows switching between screen viewing sharer and viewer, and publishes video through the screen sharing socket.

## HueBot

The [HueBot](HueBot/README.md) demonstrates local media scenarios.
- Listens to dominant speaker events and changes the hue color of the dominant speaker video.

## PolicyRecordingBot
The [PolicyRecordingBot](PolicyRecordingBot/README.md) demostrates compliance recording policy based scenarios:
- Based on organizational compliance recording policy specified, this bot is pulled into a call when a user with compliance recording policy joins the call, thereby recording the call.

# Getting Started

1. [Register Calling Bot](https://microsoftgraph.github.io/microsoft-graph-comms-samples/docs/articles/calls/register-calling-bot.html).
1. Configure your permissions. Go to Settings page and click on the *Manage* link near *Microsoft App ID*. In the new page, click *Add* button after *Application Permissions* and select *Calls.AccessMedia.All*, and then save the change. If your bot also need to join meeting, select *Calls.JoinGroupCall.All* as well.
1. The permission needs to be consented by tenant admin. Go to "https://login.microsoftonline.com/common/adminconsent?client_id=<app_id>&state=<any_number>&redirect_uri=<callback_url>" using tenant admin to sign-in, then consent for the whole tenant.
1. Please read the [Concepts](https://docs.microsoft.com/en-us/microsoftteams/platform/concepts/calls-and-meetings/real-time-media-concepts) articles for more in depth knowledge on local media bots.
1. Follow instructions in the sample to publish your bot to the web.
1. Please read the [API Reference](https://microsoftgraph.github.io/microsoft-graph-comms-samples/docs/) for specific API information.
1. The samples use Communications client to communicate with Microsoft Graph which when building out a request would add a `User-Agent` header. This header is automatically built when initializing the client object and communicates the Graph Calling SDK version being used by the sample. 
    - The general format of `User-Agent` header is as follows ```{GraphCommunicationsClient}-{AppName}/{GraphCallingSDKVersion}```. Example header: User-Agent: GraphCommunicationsClient-Sample.AudioVideoPlaybackBot.FrontEnd/1.2.0.5304.</sup>

# Build and Test

Before we build, we will need to gather all the configurations from the **Getting Started** guide.  The pieces of information that are needed are:
- The bot name from step 1.
- The bot app id/secret from step 1.  Once the bot is created you can go to the *Settings* section and then *Manage App Id* to generate your secret.

We have created a convenience script that will take all the information above and populate the configuration files.  Please run configure_cloud.ps1 and enter the above information.

To build, simply open the .sln file in question and build all.  Testing the bot can be done in 1 of 2 ways:

## Deploy the bot to a cloud service

If you choose to go down this route, please take a look at the deployment steps in the **Getting Started** guide.  In each local media solution we have also provided you with sample [Postman](https://www.getpostman.com/) requests you can use to talk to your bot deployment.

## Run your bot locally

To run your bot locally you will need to configure a local proxy.  [ngrok](https://ngrok.com/) is a great service that allows local proxy of both http (signalling) and tcp (media) traffic.  For more information please reference our [Local Testing Guide](https://microsoftgraph.github.io/microsoft-graph-comms-samples/docs/articles/Testing.html)

Do not forget to update the values of `AzureSettings__ServiceDnsName` and `AzureSettings__MediaInstanceExternalPort`
after every Ngrok restart.

## Join a call

To join a call, use the next cURL command (replace the `tenantId` and `joinURL` with your own):

```powershell
curl -v -X POST -H 'Content-Type: application/json' `
    http://localhost:5228/joinCall `
    -d '{ "tenantId": "72f988bf-86f1-41af-91ab-2d7cd011db47", "joinURL": "https://teams.microsoft.com/l/meetup-join/19%3ameeting_MDA1NDJjZDgtNDRhYy00MGY4LWE2YzQtMjI1YzFlNTAzYzMw%40thread.v2/0?context=%7b%22Tid%22%3a%2272f988bf-86f1-41af-91ab-2d7cd011db47%22%2c%22Oid%22%3a%22b1b11b68-1839-4792-a462-1854254ddfe8%22%2c%22MessageId%22%3a%220%22%7d" }'
```
