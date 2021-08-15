using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using weatherApp.Service.Interface;
using weatherApp.Shared.Model;

namespace weatherApp.Service.Services
{
    public class UserService : IUserService
    {
        private readonly List<UserDTO> users = new List<UserDTO>();
        public UserService()
        {
            users.Add(new UserDTO { UserName = "joydipkanjilal", Password = "joydip123", Role = "manager" });
            users.Add(new UserDTO { UserName = "michaelsanders", Password = "michael321", Role = "developer" });
            users.Add(new UserDTO { UserName = "stephensmith", Password = "stephen123", Role = "tester" });
            users.Add(new UserDTO { UserName = "rodpaddock", Password = "rod123", Role = "admin" });
            users.Add(new UserDTO { UserName = "admin", Password = "admin", Role = "admin" });
        }
        public UserDTO GetUser(UserModel userModel)
        {
            return users.Where(x => x.UserName.ToLower() == userModel.UserName.ToLower()
                && x.Password == userModel.Password).FirstOrDefault();
        }
    }
}
