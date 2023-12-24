namespace AVPWindowsService
{
    using System;
    using System.ServiceProcess;

    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                var environment = Util.GetResourcePath(".env");
                DotEnv.Load(environment);
                var service = new AudioVideoPlaybackService();
                service.InteractiveStartupAndStop(args);
            }
            else
            {
                var servicesToRun = new ServiceBase[]
                {
                    new AudioVideoPlaybackService()
                };
                ServiceBase.Run(servicesToRun);
            }
        }
    }
}
