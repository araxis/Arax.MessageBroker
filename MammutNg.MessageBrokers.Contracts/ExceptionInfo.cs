namespace MammutNg.MessageBrokers.Contracts
{
    public class ExceptionInfo
    {
       public string ExceptionType { get; set; }

        /// <summary>
        /// The inner exception if present (also converted to ExceptionInfo)
        /// </summary>
        public  ExceptionInfo InnerException { get; set;}

        /// <summary>
        /// The stack trace of the exception site
        /// </summary>
        public  string StackTrace { get; set;}

        /// <summary>
        /// The exception message
        /// </summary>
        public  string Message { get;set; }

        /// <summary>
        /// The exception source
        /// </summary>
        public string Source { get; set;}
    }
}