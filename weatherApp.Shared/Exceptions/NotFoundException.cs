using System;
using System.Collections.Generic;
using System.Text;

namespace weatherApp.Shared.Exceptions
{
    /// <summary>
    /// Custom exception largely for handling HTTP 404 errors
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
}
