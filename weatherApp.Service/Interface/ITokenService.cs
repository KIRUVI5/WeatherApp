using System;
using System.Collections.Generic;
using System.Text;
using weatherApp.Shared.Model;

namespace weatherApp.Service.Interface
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, UserDTO user);
        bool IsTokenValid(string key, string issuer, string token);
    }
}
