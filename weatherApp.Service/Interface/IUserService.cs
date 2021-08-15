using System;
using System.Collections.Generic;
using System.Text;
using weatherApp.Shared.Model;

namespace weatherApp.Service.Interface
{
    public interface IUserService
    {
        UserDTO GetUser(UserModel userModel);
    }
}
