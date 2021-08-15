using System;
using System.Collections.Generic;
using System.Text;

namespace weatherApp.Shared.Exceptions
{
    /// <summary>
    /// Custom exception largely for handling HTTP 403 errors
    /// </summary>
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message) : base(message) { }
    }
}
