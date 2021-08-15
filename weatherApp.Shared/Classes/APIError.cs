using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace weatherApp.Shared.Classes
{
    public class APIError
    {
        public string? Message { get; set; }

        /// <summary>
        /// Extra info returned to user - normally nothing
        /// </summary>
        public object? ExtraInfo { get; set; }
        /// <summary>
        /// HTTP code
        /// </summary>
        public HttpStatusCode HTTPCode { get; set; }

        /// <summary>
        /// Name of exception thrown
        /// </summary>
        public string? ExceptionName { get; set; }

    }
}
