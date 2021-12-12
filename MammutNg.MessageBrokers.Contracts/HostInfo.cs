namespace MammutNg.MessageBrokers.Contracts
{
    public class HostInfo
    {
        /// <summary>
        /// The machine name (or role instance name) of the local machine
        /// </summary>
       public string MachineName { get; set; }

        /// <summary>
        /// The process name hosting the routing slip activity
        /// </summary>
        public string ProcessName { get;set; }

        /// <summary>
        /// The processId of the hosting process
        /// </summary>
        public int ProcessId { get;set; }

        /// <summary>
        /// The assembly where the exception occurred
        /// </summary>
        public string Assembly { get;set; }

        /// <summary>
        /// The assembly version
        /// </summary>
        public string AssemblyVersion { get; set;}

        /// <summary>
        /// The .NET framework version
        /// </summary>
        public string FrameworkVersion { get;set; }

        /// <summary>
        /// The version of MassTransit used by the process
        /// </summary>
        public string MassTransitVersion { get;set; }

        /// <summary>
        /// The operating system version hosting the application
        /// </summary>
        public string OperatingSystemVersion { get; set;}
    }
}