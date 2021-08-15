using System;
using System.Collections.Generic;
using System.Text;

namespace weatherApp.Shared.Exceptions
{
    public class ExtendedArgumentException : Exception
    {
        public ExtendedArgumentException(string message, object extraInfo) : base(message)
        {
            ExtraInfo = extraInfo;
        }

        /// <summary>
        /// These details are used in the ExceptionMiddleware to fill in the ExtraInfo field of APIError
        /// </summary>
        public object ExtraInfo { get; set; }
    }
}
