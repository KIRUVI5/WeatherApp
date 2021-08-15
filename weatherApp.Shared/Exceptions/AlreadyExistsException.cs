using System;
using System.Collections.Generic;
using System.Text;

namespace weatherApp.Shared.Exceptions
{
    /// <summary>
    /// Custom exception largely for handling HTTP 409 errors
    /// </summary>
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException(string message) : base(message) { }
    }
}
