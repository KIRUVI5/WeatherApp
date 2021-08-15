using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace weatherApp.Shared.Enum
{
    /// <summary>
    /// Enums for HttpVerb, it's used as a prameter in api calls
    /// </summary>
    public enum HttpVerb
    {
        GET = 1,
        POST = 2,
        PUT = 3,
        DELETE = 4
    }
}
